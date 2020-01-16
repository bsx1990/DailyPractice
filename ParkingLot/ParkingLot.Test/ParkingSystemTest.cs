using System;
using System.Collections.Generic;
using NUnit.Framework;
using ParkingLot.Exceptions;

namespace ParkingLot.Test
{
    [TestFixture]
    public class ParkingSystemTest
    {
        [Test]
        public void ShouldGetATicket_WhenParkingSuccessed()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var parkingSystem = new ParkingSystem
                                {
                                    ParkingLots = new List<IParkingLot> { parkingLot },
                                    ParkingBoys = new List<ParkingBoy> { parkingBoy }
                                };
            var parkingSpace = parkingSystem.GetEmptySpace();

            var actual = parkingSystem.Parking(car, parkingSpace);
            Assert.AreEqual(car.LicensePlate, actual.LicensePlate);
            Assert.AreEqual(parkingSpace.Id, actual.ParkingSpaceId);
            Assert.IsFalse(parkingSpace.IsEmpty);
        }

        //todo: 如何验证exception的信息
        [Test]
        public void ShouldGetException_WhenParkingWithNoParkingBoys()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingSystem = new ParkingSystem
                                {
                                    ParkingLots = new List<IParkingLot> { parkingLot },
                                    ParkingBoys = new List<ParkingBoy>()
                                };
            var parkingSpace = parkingSystem.GetEmptySpace();

            Assert.Throws<ParkingException>(() => { parkingSystem.Parking(car, parkingSpace); });
        }

        [Test]
        public void ShouldGetAResponse_WhenPickUpWithATicket()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var parkingSystem = new ParkingSystem
                                {
                                    ParkingLots = new List<IParkingLot> { parkingLot },
                                    ParkingBoys = new List<ParkingBoy> { parkingBoy }
                                };
            var parkingSpace = parkingSystem.GetEmptySpace();
            parkingSpace.ParkedWithCar(car);
            var ticket = new ParkingTicket
                         {
                             LicensePlate = car.LicensePlate,
                             ParkingSpaceId = parkingSpace.Id
                         };

            var actual = parkingSystem.PickUp(ticket);
            Assert.AreEqual(car, actual.Car);
        }

        [Test]
        public void ShouldGetAResponseWithFee_WhenPickUpACar()
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
            var ticket = parkingSystem.Parking(car, parkingSpace);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 16, 0, 0));
            var actual = parkingSystem.PickUp(ticket);
            Assert.AreEqual(car, actual.Car);
            Assert.AreEqual(10,actual.Fee);
        }
    }
}
