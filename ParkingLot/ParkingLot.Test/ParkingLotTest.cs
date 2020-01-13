using NUnit.Framework;

namespace ParkingLot.Test
{
    [TestFixture]
    public class ParkingLotTest
    {
        [Test]
        public void ShoudGetAnEmptySpace_WhenHasValidSpaces()
        {
            var parkingLot = new ParkingLot(1);
            var parkingSpace = new ParkingSpace("P1");

            var actual = parkingLot.GetEmptySpace();
            Assert.AreEqual(parkingSpace.Id, actual.Id);
            Assert.IsTrue(actual.IsEmpty);
        }

        [Test]
        public void ShoudNotGetSameSpace_WhenGetMultipleSpaces()
        {
            var parkingLot = new ParkingLot(5);
            
            var parkingSpace1 = parkingLot.GetEmptySpace();
            var parkingSpace2 = parkingLot.GetEmptySpace();

            var condition = parkingSpace1.Id == parkingSpace2.Id;
            Assert.IsFalse(condition);
        }

        [Test]
        public void ShoudGetException_WhenNoValidSpaces()
        {
            var parkingLot = new ParkingLot(0);
            Assert.Throws<ParkingSpaceException>(()=> { parkingLot.GetEmptySpace(); });
        }
    }
}