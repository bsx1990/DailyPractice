using System;

namespace ParkingLot.ParkingFeeCalculator
{
    public class TimeSharingParkingFeeCalculator : IParkingFeeCalculator
    {
        private const float MoneyPerHour = 2;
        private const float StartingPrice = 10;
        private const int MaxParkingFeeHours = 11;
        private const float MaxParkingFee = 30;
        public float CalcFee(DateTime parkingTime, DateTime pickUpTime)
        {
            var timeSpan = pickUpTime.Subtract(parkingTime);
            if (parkingTime.Day == pickUpTime.Day)
            {
                return IntraDayFeeCalculate(timeSpan);
            }

            var parkingDay2400 = new DateTime(parkingTime.Year,parkingTime.Month,parkingTime.Day,23,59,59);
            var parkingDayTimeSpan = parkingDay2400.Subtract(parkingTime);
            var parkingDayFee = IntraDayFeeCalculate(parkingDayTimeSpan);

            var pickUpDay0000 = new DateTime(pickUpTime.Year, pickUpTime.Month, pickUpTime.Day, 0, 0, 0);
            var pickUpDayTimeSpan = pickUpTime.Subtract(pickUpDay0000);
            var pickUpDayFee = IntraDayFeeCalculate(pickUpDayTimeSpan);

            return parkingDayFee + (pickUpTime.Day-parkingTime.Day-1) * MaxParkingFee + pickUpDayFee;
        }

        private static float IntraDayFeeCalculate(TimeSpan timeSpan)
        {
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            hours = minutes > 0 ? hours + 1 : hours;
            return hours >= MaxParkingFeeHours 
                ? MaxParkingFee 
                : StartingPrice + (hours - 1) * MoneyPerHour;
        }
    }
}