namespace ParkingLot
{
    public class ParkingBoy
    {
        public ParkingTicket Parking(Car car, ParkingSpace parkingSpace)
        {
            var result = new ParkingTicket {LicensePlate = car.LicensePlate, ParkingSpaceId = parkingSpace.Id};
            parkingSpace.ParkedWithCar(car);
            return result;
        }

        public Car PickUp(ParkingSpace parkingSpace)
        {
            return parkingSpace.PickUpCar();
        }
    }
}