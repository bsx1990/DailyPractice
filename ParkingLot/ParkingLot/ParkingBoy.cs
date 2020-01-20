namespace ParkingLot
{
    public interface IParkingBoy
    {
        IParkingTicket Parking(ICar car, IParkingSpace parkingSpace);
        ICar PickUp(IParkingSpace parkingSpace);
    }

    public class ParkingBoy : IParkingBoy
    {
        public IParkingTicket Parking(ICar car, IParkingSpace parkingSpace)
        {
            var result = new ParkingTicket {LicensePlate = car.LicensePlate, ParkingSpaceId = parkingSpace.Id};
            parkingSpace.ParkedWithCar(car);
            return result;
        }

        public ICar PickUp(IParkingSpace parkingSpace)
        {
            return parkingSpace.PickUpCar();
        }
    }
}