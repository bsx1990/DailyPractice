namespace ParkingLot
{
    public interface ICar
    {
        string LicensePlate { get; set; }
    }

    public class Car : ICar
    {
        public Car(string licensePlate)
        {
            LicensePlate = licensePlate;
        }

        public string LicensePlate { get; set; }
    }
}