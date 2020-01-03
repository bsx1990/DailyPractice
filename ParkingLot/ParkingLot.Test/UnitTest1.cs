using System.Linq;
using NUnit.Framework;
using ParkingLot.Exceptions;

namespace ParkingLot.Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void WillGetATicketAfterParking()
        {
            var car = new Car("A000000");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var ticket = parkingBoy.Parking(car, parkingLot);
            Assert.NotNull(ticket);
        }

        [Test]
        public void WillGetExceptionWhenParkingToAFullParkingLot()
        {
            Assert.Throws<ParkingException>(() =>
                                     {
                                         var car = new Car("A000000");
                                         var parkingLot = new ParkingLot(1);
                                         parkingLot.ParkingSpaces.First().Car = new Car("T000000");
                                         var parkingBoy = new ParkingBoy();
                                         parkingBoy.Parking(car, parkingLot);
                                     });
        }

        [Test]
        public void ParkingTicketShouldRecordCarAndParkingSpace()
        {
            var car = new Car("A000000");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var ticket = parkingBoy.Parking(car, parkingLot);
            Assert.AreEqual("A000000", ticket.LicensePlate);
            Assert.NotNull(ticket.ParkingSpaceNumber);
        }

        [Test]
        public void EmptyParkingSpaceShouldReduceAfterParking()
        {
            var car = new Car("A000000");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            parkingBoy.Parking(car, parkingLot);
            Assert.AreEqual(0,parkingLot.EmptyParkingSpaceCount);
            Assert.AreEqual(false,parkingLot.HasEmptyParkingSpace());
            Assert.AreEqual(1,parkingLot.TotalParkingSpaceCount);
        }

        [Test]
        public void PickUpACarWithAParingTicket()
        {
            var car = new Car("A000000");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            parkingBoy.Parking(car, parkingLot);
            var ticket = new ParkingTicket { LicensePlate = "A000000", ParkingSpaceNumber = "0" };
            var pickedCar = parkingBoy.PickUp(ticket, parkingLot);
            Assert.AreEqual(car, pickedCar);
        }

        [Test]
        public void ShouldGetExceptionWhenPickingUpACarWithInvalidParingTicket()
        {
            Assert.Throws<PickUpException>(() =>
                                     {
                                         var car = new Car("A000000");
                                         var parkingLot = new ParkingLot(1);
                                         var parkingBoy = new ParkingBoy();
                                         parkingBoy.Parking(car, parkingLot);
                                         var ticket = new ParkingTicket { LicensePlate = "A000001", ParkingSpaceNumber = "0" };
                                         parkingBoy.PickUp(ticket, parkingLot);
                                     });
            Assert.Throws<PickUpException>(() =>
                                     {
                                         var car = new Car("A000000");
                                         var parkingLot = new ParkingLot(1);
                                         var parkingBoy = new ParkingBoy();
                                         parkingBoy.Parking(car, parkingLot);
                                         var ticket = new ParkingTicket { LicensePlate = "A000000", ParkingSpaceNumber = "999" };
                                         parkingBoy.PickUp(ticket, parkingLot);
                                     });
        }

        [Test]
        public void EmptyParkingSpaceShouldIncreaseAfterPickingUpACar()
        {
            var car = new Car("A000000");
            var parkingLot = new ParkingLot(1);
            var parkingBoy = new ParkingBoy();
            var ticket = parkingBoy.Parking(car, parkingLot);
            Assert.AreEqual(0, parkingLot.EmptyParkingSpaceCount);
            Assert.AreEqual(false, parkingLot.HasEmptyParkingSpace());
            Assert.AreEqual(1, parkingLot.TotalParkingSpaceCount);
            parkingBoy.PickUp(ticket, parkingLot);
            Assert.AreEqual(1, parkingLot.EmptyParkingSpaceCount);
            Assert.AreEqual(true, parkingLot.HasEmptyParkingSpace());
            Assert.AreEqual(1, parkingLot.TotalParkingSpaceCount);
        }
    }
}
