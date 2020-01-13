namespace ParkingLot
{
    public class ParkingSpace
    {
        private ParkingSpaceStatus _status;

        public string Id { get; }
        public Car Car { get; set; }
        public bool IsEmpty => _status == ParkingSpaceStatus.Empty;

        public ParkingSpace(string id)
        {
            Id = id;
            _status = ParkingSpaceStatus.Empty;
        }

        public void ParkedWithCar(Car car)
        {
            MarkAsParked();
            Car = car;
        }

        public Car PickUpCar()
        {
            MarkAsEmpty();
            var result = Car;
            Car = null;
            return result;
        }

        private void MarkAsParked()
        {
            _status = ParkingSpaceStatus.Parked;
        }

        private void MarkAsEmpty()
        {
            _status = ParkingSpaceStatus.Empty;
        }
    }

    internal enum ParkingSpaceStatus
    {
        Empty,
        Parked
    }
}