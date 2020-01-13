using NUnit.Framework;

namespace ParkingLot.Test
{
    [TestFixture]
    public class ParkingBoyTest
    {
        [Test]
        public void ShouldGetATicket_WhenParkingSuccessed()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingSpace = parkingLot.GetEmptySpace();
            var parkingBoy = new ParkingBoy();

            var actual = parkingBoy.Parking(car, parkingSpace);
            Assert.AreEqual(car.LicensePlate,actual.LicensePlate);
            Assert.AreEqual(parkingSpace.Id,actual.ParkingSpaceId);
            Assert.IsFalse(parkingSpace.IsEmpty);
        }

        [Test]
        public void ShouldGetACar_WhenPickUpWithATicket()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(1);
            var parkingSpace = parkingLot.GetEmptySpace();
            parkingSpace.ParkedWithCar(car);
            var parkingBoy = new ParkingBoy();
            var ticket = new ParkingTicket
                         {
                             LicensePlate = car.LicensePlate,
                             ParkingSpaceId = parkingSpace.Id
                         };
            var space = parkingLot.GetParkedSpace(ticket.ParkingSpaceId);

            var actual = parkingBoy.PickUp(space);

            Assert.AreEqual(car,actual);
            Assert.IsTrue(space.IsEmpty);
        }
    }
}
