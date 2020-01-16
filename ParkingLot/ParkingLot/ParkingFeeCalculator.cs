using System;

namespace ParkingLot
{
    public class ParkingFeeCalculator
    {
        private const float MoneyPerHour = 5;
        public static double CalcFee(DateTime parkingTime, DateTime pickUpTime)
        {
            var timeSpan = pickUpTime.Subtract(parkingTime);
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            hours = minutes > 0 ? hours + 1 : hours;
            return hours * MoneyPerHour;
        }
    }
}