using System.Collections.Generic;
using System.Linq;

namespace ParkingLot
{
    public class ParkingLot
    {
        public ParkingLot(int totalParkingSpace)
        {
            TotalParkingSpaceCount = totalParkingSpace;
            InitParkingSpaces();
        }

        private void InitParkingSpaces()
        {
            ParkingSpaces = new List<ParkingSpace>(TotalParkingSpaceCount);
            for (var index = 0; index < TotalParkingSpaceCount; index++)
            {
                ParkingSpaces.Add(new ParkingSpace(index.ToString()));
            }
        }

        public int TotalParkingSpaceCount { get; set; }
        public IList<ParkingSpace> ParkingSpaces { get; set; }
        public int EmptyParkingSpaceCount => ParkingSpaces.Count(space => space.IsEmpty());

        public bool HasEmptyParkingSpace()
        {
            return EmptyParkingSpaceCount > 0;
        }
    }
}