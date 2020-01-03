using System;

namespace ParkingLot.Exceptions
{
    public class PickUpException : Exception
    {
        public PickUpException(string message):base(message) { }
    }
}