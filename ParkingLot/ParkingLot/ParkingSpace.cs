namespace ParkingLot
{
    public interface IParkingSpace
    {
        string Id { get; }
        bool IsEmpty { get; }
        void ParkedWithCar(ICar car);
        ICar PickUpCar();
        ICar GetParkedCar();
        string ParkingLotId { get; set; }
    }

    public class ParkingSpace : IParkingSpace
    {
        private ParkingSpaceStatus _status;

        public string Id { get; }
        private ICar Car { get; set; }
        public bool IsEmpty => _status == ParkingSpaceStatus.Empty;
        public string ParkingLotId { get; set; }

        public ParkingSpace(string id)
        {
            Id = id;
            _status = ParkingSpaceStatus.Empty;
        }

        public void ParkedWithCar(ICar car)
        {
            MarkAsParked();
            Car = car;
        }

        public ICar PickUpCar()
        {
            MarkAsEmpty();
            var result = Car;
            Car = null;
            return result;
        }

        public ICar GetParkedCar()
        {
            return Car;
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