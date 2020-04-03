using System;

namespace ParkingLot.ParkingFeeCalculator
{
    public class MonthlyParkingFeeCalculator : IParkingFeeCalculator
    {
        private const float FreeParkingForMonthly = 0;
        public float CalcFee(DateTime parkingTime, DateTime pickUpTime)
        {
            return FreeParkingForMonthly;
        }
    }
}