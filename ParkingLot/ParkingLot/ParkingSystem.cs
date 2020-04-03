using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParkingLot.Exceptions;
using ParkingLot.ParkingFeeCalculator;

namespace ParkingLot
{
    public interface IParkingSystem
    {
        List<IParkingLot> ParkingLots { get; set; }
        List<IParkingBoy> ParkingBoys { get; set; }
        IParkingFeeCalculator ParkingFeeCalculator { get; set; }
        IParkingTicket Parking(ICar car);
        IPickupResponse PickUp(IParkingTicket ticket);
    }

    public class ParkingSystem : IParkingSystem
    {
        public List<IParkingLot> ParkingLots { get; set; }
        public List<IParkingBoy> ParkingBoys { get; set; }
        public ParkingClock ParkingClock { get; set; }
        public IParkingFeeCalculator ParkingFeeCalculator { get; set; }
        public List<string> MonthlyCars { get; set; }
        public float MonthlyFee { get; set; }

        private readonly IParkingFeeCalculator _monthlyParkingFeeCalculator;
        private readonly IParkingFeeCalculator _timeSharingParkingFeeCalculator;
        private List<ParkingRecord> _parkingRecords = new List<ParkingRecord>();
        private const float DefaultMonthlyFee = 400;

        public ParkingSystem()
        {
            ParkingLots = new List<IParkingLot>();
            ParkingBoys = new List<IParkingBoy>();
            ParkingClock = new ParkingClock(DateTime.Now);
            MonthlyCars = new List<string>();
            MonthlyFee = DefaultMonthlyFee;
            _monthlyParkingFeeCalculator = new MonthlyParkingFeeCalculator();
            _timeSharingParkingFeeCalculator = new TimeSharingParkingFeeCalculator();
        }

        public IParkingTicket Parking(ICar car)
        {
            return Parking(car, GetEmptySpace());
        }

        private IParkingSpace GetEmptySpace()
        {
            return ParkingLots.Any(lot => lot.HasEmptySpaces())
                ? ParkingLots.First(lot => lot.HasEmptySpaces()).GetEmptySpace()
                : throw new ParkingException("No empty parking space");
        }

        private IParkingTicket Parking(ICar car, IParkingSpace parkingSpace)
        {
            if (ParkingBoys.Count <= 0) { throw new ParkingException("No parking boys"); }

            var parkingTicket = ParkingBoys.First().Parking(car, parkingSpace);
            var parkingTime = ParkingClock.GetTime();
            parkingTicket.ParkingTime = parkingTime;
            _parkingRecords.Add(new ParkingRecord
                                {
                                    LicensePlate = car.LicensePlate,
                                    ParkingTime = parkingTime,
                                    ParkingLotId = parkingSpace.ParkingLotId,
                                    ParkingSpaceId = parkingSpace.Id,
                                    IsMonthlyCar = MonthlyCars.Contains(car.LicensePlate)
                                });
            return parkingTicket;
        }

        public IPickupResponse PickUp(IParkingTicket ticket)
        {
            ParkingFeeCalculator = GetCalculatorByLicensePlate(ticket.LicensePlate);
            var space = ParkingLots.First().GetParkedSpace(ticket.ParkingSpaceId);
            var parkingRecord = _parkingRecords.FirstOrDefault(record => record.LicensePlate == ticket.LicensePlate 
                                                                         && record.ParkingTime == ticket.ParkingTime);
            _parkingRecords.Remove(parkingRecord);
            return ParkingBoys.Count > 0
                ? new PickupResponse
                  {
                      Car = ParkingBoys.First().PickUp(space),
                      Fee = ParkingFeeCalculator.CalcFee(ticket.ParkingTime, ParkingClock.GetTime())
                  }
                : throw new PickUpException("No parking boys");
        }

        private IParkingFeeCalculator GetCalculatorByLicensePlate(string licensePlate)
        {
            return MonthlyCars.Contains(licensePlate)
                ? _monthlyParkingFeeCalculator
                : _timeSharingParkingFeeCalculator;
        }

        public string GetProfitDescription()
        {
            var profits = GetProfits();

            var profitResult = new StringBuilder();
            profitResult.Append($"Total monthly cars count: {MonthlyCars.Count}, monthly fee total: {MonthlyCars.Count * MonthlyFee}");
            profitResult.Append(string.Join(System.Environment.NewLine, profits));
            return profitResult.ToString();
        }

        private IList<ParkingLotProfit> GetProfits()
        {
            var currentTime = ParkingClock.GetTime();
            var profits = _parkingRecords.GroupBy(record => new { record.ParkingLotId })
                                         .Select(records => new
                                                            ParkingLotProfit
                                                            {
                                                                ParkingLotId = records.Key.ParkingLotId,
                                                                MonthlyCarsCount = records.Count(r => r.IsMonthlyCar),
                                                                TimeSharingFeeTotal = records.Where(r => !r.IsMonthlyCar)
                                                                                             .Sum(r =>_timeSharingParkingFeeCalculator.CalcFee(r.ParkingTime,currentTime))
                                                            })
                                         .ToList();
            return profits;
        }
    }

    public class ParkingLotProfit
    {
        public string ParkingLotId { get; set; }
        public int MonthlyCarsCount { get; set; }
        public float TimeSharingFeeTotal { get; set; }
        public override string ToString()
        {
            return $"ParkingLot Id: {ParkingLotId}, current parked monthly cars count: {MonthlyCarsCount}, current timesharing Fee total: {TimeSharingFeeTotal}";
        }
    }

    public class ParkingRecord
    {
        public string LicensePlate { get; set; }
        public string ParkingLotId { get; set; }
        public string ParkingSpaceId { get; set; }
        public DateTime ParkingTime { get; set; }
        public bool IsMonthlyCar { get; set; }
    }
}