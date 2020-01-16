using System;
using System.Collections.Generic;
using System.Linq;
using ParkingLot.Exceptions;

namespace ParkingLot
{
    public interface IParkingSystem
    {
        List<IParkingLot> ParkingLots { get; set; }
        List<ParkingBoy> ParkingBoys { get; set; }
        ParkingSpace GetEmptySpace();
        ParkingTicket Parking(Car car, ParkingSpace parkingSpace);
        PickupResponse PickUp(ParkingTicket ticket);
    }

    public class ParkingSystem : IParkingSystem
    {
        public List<IParkingLot> ParkingLots { get; set; }
        public List<ParkingBoy> ParkingBoys { get; set; }
        public ParkingClock ParkingClock { get; set; }

        public ParkingSystem()
        {
            ParkingLots = new List<IParkingLot>();
            ParkingBoys = new List<ParkingBoy>();
            ParkingClock = new ParkingClock(DateTime.Now);
        }

        public ParkingSpace GetEmptySpace()
        {
            return ParkingLots.Any(lot => lot.HasEmptySpaces())
                ? ParkingLots.First(lot => lot.HasEmptySpaces()).GetEmptySpace()
                : throw new ParkingException("No empty parking space");
        }

        public ParkingTicket Parking(Car car, ParkingSpace parkingSpace)
        {
            if (ParkingBoys.Count <= 0) { throw new ParkingException("No parking boys"); }

            var parkingTicket = ParkingBoys.First().Parking(car, parkingSpace);
            parkingTicket.ParkingTime = ParkingClock.GetTime();
            return parkingTicket;
        }

        public PickupResponse PickUp(ParkingTicket ticket)
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