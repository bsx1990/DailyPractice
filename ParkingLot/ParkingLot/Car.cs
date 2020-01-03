namespace ParkingLot
{
    public class Car
    {
        public Car(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public string LicensePlate { get; set; }
    }
}