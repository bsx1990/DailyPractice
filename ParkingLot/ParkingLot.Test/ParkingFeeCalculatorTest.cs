using System;
using NUnit.Framework;

namespace ParkingLot.Test
{
    [TestFixture]
    public class ParkingFeeCalculatorTest
    {
        [Test]
        public void ShouldBe10_WhenParkingFrom1400To1600()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
            var calculator = new ParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(10,actual);
        }

        [Test]
        public void ShouldBe10_WhenParkingFrom1410To1600()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 10, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
            var calculator = new ParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(10,actual);
        }

        [Test]
        public void ShouldBe15_WhenParkingFrom1400To1610()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 16, 10, 0);
            var calculator = new ParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(15,actual);
        }
    }
}
