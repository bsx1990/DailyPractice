using System;

namespace ParkingLot
{
    public class ParkingSpaceException : Exception
    {
        public ParkingSpaceException(string message):base(message) { }
    }
}