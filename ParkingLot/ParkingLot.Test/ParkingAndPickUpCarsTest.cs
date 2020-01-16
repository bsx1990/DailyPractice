using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ParkingLot.Test
{
    [TestFixture]
    public class ParkingAndPickUpCarsTest
    {
        [Test]
        public void ShoudNotGotException_WhenParkingAndPickUpACar()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
                                {
                                    ParkingLots = new List<IParkingLot> { parkingLot },
                                    ParkingBoys = new List<ParkingBoy> { parkingBoy },
                                    ParkingClock = clock
            };
            var parkingSpace = parkingSystem.GetEmptySpace();

            var parkingTicket = parkingSystem.Parking(car, parkingSpace);
            Assert.AreEqual(car.LicensePlate, parkingTicket.LicensePlate);
            Assert.AreEqual(parkingSpace.Id, parkingTicket.ParkingSpaceId);
            Assert.IsFalse(parkingSpace.IsEmpty);

            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 16, 0, 0));

            var actual = parkingSystem.PickUp(parkingTicket);

            Assert.AreEqual(car, actual.Car);
            Assert.AreEqual(10, actual.Fee);
            Assert.IsTrue(parkingSpace.IsEmpty);
        }
    }
}
