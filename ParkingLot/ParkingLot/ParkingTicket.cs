using System;

namespace ParkingLot
{
    public interface IParkingTicket
    {
        string LicensePlate { get; set; }
        string ParkingSpaceId { get; set; }
        DateTime ParkingTime { get; set; }
    }

    public class ParkingTicket : IParkingTicket
    {
        public string LicensePlate { get; set; }
        public string ParkingSpaceId { get; set; }
        public DateTime ParkingTime { get; set; }
    }
}