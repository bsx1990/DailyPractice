namespace ParkingLot
{
    public class ParkingSpace
    {
        public ParkingSpace(string number)
        {
            Number = number;
        }

        public string Number { get; set; }
        public Car Car { get; set; }

        public bool IsEmpty()
        {
            return Car == null;
        }
    }
}