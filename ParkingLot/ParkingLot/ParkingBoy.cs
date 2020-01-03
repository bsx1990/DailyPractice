using System.Linq;
using ParkingLot.Exceptions;

namespace ParkingLot
{
    public class ParkingBoy
    {
        public ParkingTicket Parking(Car car, ParkingLot parkingLot)
        {
            if (!parkingLot.HasEmptyParkingSpace()) { throw new ParkingException("Parking Lot is full, could not parking"); }

            var parkingSpace = parkingLot.ParkingSpaces.First();
            parkingSpace.Car = car;
            return new ParkingTicket
                   {
                       LicensePlate = car.LicensePlate,
                       ParkingSpaceNumber = parkingSpace.Number
                   };
        }

        public Car PickUp(ParkingTicket ticket, ParkingLot parkingLot)
        {
            var ticketParkingSpaceNumber = ticket.ParkingSpaceNumber;
            var parkingSpace = parkingLot.ParkingSpaces.FirstOrDefault(space => space.Number == ticketParkingSpaceNumber);
            if (parkingSpace == null)
            {
                throw new PickUpException($"No Parking Space with number {ticketParkingSpaceNumber}");
            }

            if (parkingSpace.Car.LicensePlate != ticket.LicensePlate)
            {
                throw new PickUpException($"Ticket's car's license plate {ticket.LicensePlate} is invalid");
            }
            var result = parkingSpace.Car;
            parkingSpace.Car = null;
            return result;
        }
    }
}