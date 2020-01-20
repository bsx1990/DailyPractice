using System;
using System.Collections.Generic;
using System.Linq;
using ParkingLot.Exceptions;

namespace ParkingLot
{
    public interface IParkingSystem
    {
        List<IParkingLot> ParkingLots { get; set; }
        List<IParkingBoy> ParkingBoys { get; set; }
        IParkingFeeCalculator ParkingFeeCalculator { get; set; }
        IParkingSpace GetEmptySpace();
        IParkingTicket Parking(ICar car, IParkingSpace parkingSpace);
        IPickupResponse PickUp(IParkingTicket ticket);
    }

    public class ParkingSystem : IParkingSystem
    {
        public List<IParkingLot> ParkingLots { get; set; }
        public List<IParkingBoy> ParkingBoys { get; set; }
        public ParkingClock ParkingClock { get; set; }
        public IParkingFeeCalculator ParkingFeeCalculator { get; set; }

        public ParkingSystem()
        {
            ParkingLots = new List<IParkingLot>();
            ParkingBoys = new List<IParkingBoy>();
            ParkingClock = new ParkingClock(DateTime.Now);
            ParkingFeeCalculator = new ParkingFeeCalculator();
        }

        public IParkingSpace GetEmptySpace()
        {
            return ParkingLots.Any(lot => lot.HasEmptySpaces())
                ? ParkingLots.First(lot => lot.HasEmptySpaces()).GetEmptySpace()
                : throw new ParkingException("No empty parking space");
        }

        public IParkingTicket Parking(ICar car, IParkingSpace parkingSpace)
        {
            if (ParkingBoys.Count <= 0) { throw new ParkingException("No parking boys"); }

            var parkingTicket = ParkingBoys.First().Parking(car, parkingSpace);
            parkingTicket.ParkingTime = ParkingClock.GetTime();
            return parkingTicket;
        }

        public IPickupResponse PickUp(IParkingTicket ticket)
        {
            var space = ParkingLots.First().GetParkedSpace(ticket.ParkingSpaceId);
            return ParkingBoys.Count > 0
                ? new PickupResponse
                  {
                      Car = ParkingBoys.First().PickUp(space),
                      Fee = ParkingFeeCalculator.CalcFee(ticket.ParkingTime, ParkingClock.GetTime())
                  }
                : throw new PickUpException("No parking boys");
        }
    }
}