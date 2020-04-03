using System;

namespace ParkingLot.ParkingFeeCalculator
{
    public interface IParkingFeeCalculator
    {
        float CalcFee(DateTime parkingTime, DateTime pickUpTime);
    }

    public class LinearParkingFeeCalculator : IParkingFeeCalculator
    {
        private const float MoneyPerHour = 5;
        public float CalcFee(DateTime parkingTime, DateTime pickUpTime)
        {
            var timeSpan = pickUpTime.Subtract(parkingTime);
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            hours = minutes > 0 ? hours + 1 : hours;
            return hours * MoneyPerHour;
        }
    }
}