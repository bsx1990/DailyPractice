using System;

namespace ParkingLot.Exceptions
{
    public class ParkingException : Exception
    {
        public ParkingException(string message):base(message) { }
    }
}