using System;
using System.Collections.Generic;
using System.Text;
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
                                    ParkingBoys = new List<IParkingBoy> { parkingBoy }
                                };

            var actual = parkingSystem.Parking(car);
            Assert.AreEqual(car.LicensePlate, actual.LicensePlate);
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
                                    ParkingBoys = new List<IParkingBoy>()
                                };

            Assert.Throws<ParkingException>(() => { parkingSystem.Parking(car); });
        }

        [Test]
        public void ShouldGetAResponseWithFee_WhenPickUpACarWithTimeSharingCalculator()
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
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock
            };
            var ticket = parkingSystem.Parking(car);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 16, 0, 0));
            var actual = parkingSystem.PickUp(ticket);
            Assert.AreEqual(car, actual.Car);
            Assert.AreEqual(12, actual.Fee);
        }

        [Test]
        public void ShouldGetAResponseWithFee_WhenPickUpACarWithMonthlyCalculator()
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
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock,
                MonthlyCars = new List<string> { "A1" }
            };
            var ticket = parkingSystem.Parking(car);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 16, 0, 0));
            var actual = parkingSystem.PickUp(ticket);
            Assert.AreEqual(car, actual.Car);
            Assert.AreEqual(0, actual.Fee);
        }

        [Test]
        public void ShouldGetExpectedProfit_WhenParking1MonthlyCar()
        {
            var car = new Car("A1");
            var parkingLot = new ParkingLot(10);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
            {
                ParkingLots = new List<IParkingLot> { parkingLot },
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock,
                MonthlyCars = new List<string> { "A1" }
            };
            parkingSystem.Parking(car);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 2, 0, 0));
            var expectedProfit = new StringBuilder();
            expectedProfit.Append("Total monthly cars count: 1, monthly fee total: 400");
            expectedProfit.Append($"ParkingLot Id: {parkingLot.Id}, current parked monthly cars count: 1, current timesharing Fee total: 0");
            var actual = parkingSystem.GetProfitDescription();
            Assert.AreEqual(expectedProfit.ToString(), actual);
        }

        [Test]
        public void ShouldGetExpectedProfit_WhenParkingFullMonthlyCars()
        {
            var car1 = new Car("A1");
            var car2 = new Car("A2");
            var car3 = new Car("A3");
            var parkingLot = new ParkingLot(10);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
            {
                ParkingLots = new List<IParkingLot> { parkingLot },
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock,
                MonthlyCars = new List<string> { "A1", "A2", "A3" }
            };
            parkingSystem.Parking(car1);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 1, 0, 0));
            parkingSystem.Parking(car2);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 3, 0, 0));
            parkingSystem.Parking(car3);

            var expectedProfit = new StringBuilder();
            expectedProfit.Append("Total monthly cars count: 3, monthly fee total: 1200");
            expectedProfit.Append($"ParkingLot Id: {parkingLot.Id}, current parked monthly cars count: 3, current timesharing Fee total: 0");
            var actual = parkingSystem.GetProfitDescription();
            Assert.AreEqual(expectedProfit.ToString(), actual);
        }

        [Test]
        public void ShouldGetExpectedProfit_WhenParkingMonthlyCars()
        {
            var car1 = new Car("A1");
            var car2 = new Car("A2");
            var car3 = new Car("A3");
            var parkingLot = new ParkingLot(10);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
            {
                ParkingLots = new List<IParkingLot> { parkingLot },
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock,
                MonthlyCars = new List<string> { "A1", "A2", "A3" }
            };
            parkingSystem.Parking(car1);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 1, 0, 0));
            parkingSystem.Parking(car2);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 2, 0, 0));

            var expectedProfit = new StringBuilder();
            expectedProfit.Append("Total monthly cars count: 3, monthly fee total: 1200");
            expectedProfit.Append($"ParkingLot Id: {parkingLot.Id}, current parked monthly cars count: 2, current timesharing Fee total: 0");
            var actual = parkingSystem.GetProfitDescription();
            Assert.AreEqual(expectedProfit.ToString(), actual);
        }

        [Test]
        public void ShouldGetAResponseWithFee_WhenParking1MonthlyCarAnd1TimeSharingCar()
        {
            var car1 = new Car("A1");
            var car2 = new Car("A2");
            var parkingLot = new ParkingLot(10);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
            {
                ParkingLots = new List<IParkingLot> { parkingLot },
                ParkingBoys = new List<IParkingBoy> { parkingBoy },
                ParkingClock = clock,
                MonthlyCars = new List<string> { "A1" }
            };
            parkingSystem.Parking(car1);
            parkingSystem.Parking(car2);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 2, 0, 0));
            var expectedProfit = new StringBuilder();
            expectedProfit.Append("Total monthly cars count: 1, monthly fee total: 400");
            expectedProfit.Append($"ParkingLot Id: {parkingLot.Id}, current parked monthly cars count: 1, current timesharing Fee total: 12");
            var actual = parkingSystem.GetProfitDescription();
            Assert.AreEqual(expectedProfit.ToString(), actual);
        }

        [Test]
        public void ShouldGetExpectedProfit_WhenParkingMonthlyCarsAndTimeSharingCars()
        {
            var car1 = new Car("A1");
            var car2 = new Car("A2");
            var car3 = new Car("A3");
            var parkingLot = new ParkingLot(10);
            var parkingBoy = new ParkingBoy();
            var now = DateTime.Now;
            var parkingTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var clock = new ParkingClock(parkingTime);
            var parkingSystem = new ParkingSystem
                                {
                                    ParkingLots = new List<IParkingLot> { parkingLot },
                                    ParkingBoys = new List<IParkingBoy> { parkingBoy },
                                    ParkingClock = clock,
                                    MonthlyCars = new List<string> { "A1", "A2" }
                                };
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 1, 0, 0));
            parkingSystem.Parking(car2);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 2, 0, 0));
            parkingSystem.Parking(car3);
            clock.SetTime(new DateTime(now.Year, now.Month, now.Day, 5, 0, 0));

            var expectedProfit = new StringBuilder();
            expectedProfit.Append("Total monthly cars count: 2, monthly fee total: 800");
            expectedProfit.Append($"ParkingLot Id: {parkingLot.Id}, current parked monthly cars count: 1, current timesharing Fee total: 14");
            var actual = parkingSystem.GetProfitDescription();
            Assert.AreEqual(expectedProfit.ToString(), actual);
        }
    }
}
