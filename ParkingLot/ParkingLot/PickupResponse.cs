namespace ParkingLot
{
    public interface IPickupResponse
    {
        ICar Car { get; set; }
        double Fee { get; set; }
    }

    public class PickupResponse : IPickupResponse
    {
        public ICar Car { get; set; }
        public double Fee { get; set; }
    }
}