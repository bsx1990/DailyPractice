using System;
using NUnit.Framework;
using ParkingLot.ParkingFeeCalculator;

namespace ParkingLot.Test.ParkingFeeCalculatorTest
{
    [TestFixture]
    internal class MonthlyParkingFeeCalculatorTest
    {
        [Test]
        public void ShouldBe0_WhenParkingFrom1400To1500()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void ShouldBe0_WhenParkingFrom1400To1600()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 16, 0, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void ShouldBe0_WhenParkingFrom1400To1550()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 15, 50, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void ShouldBe0_WhenParkingFrom1400To1610()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 16, 10, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void ShouldBe0_WhenParkingFrom0000To1100()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void ShouldBe400_WhenParkingFrom0000To1110()
        {
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var pickUpTime = new DateTime(now.Year, now.Month, now.Day, 11, 10, 0);
            var calculator = new MonthlyParkingFeeCalculator();

            var actual = calculator.CalcFee(parkingTime, pickUpTime);

            Assert.AreEqual(0, actual);
        }
    }
}
