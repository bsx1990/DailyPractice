using System.Collections.Generic;
using System.Linq;

namespace ParkingLot
{
    public interface IParkingLot
    {
        int TotalSpace { get; }
        ParkingSpace GetEmptySpace();
        ParkingSpace GetParkedSpace(string id);
        bool HasEmptySpaces();
    }

    public class ParkingLot : IParkingLot
    {
        private readonly IList<ParkingSpace> _parkedSpaces;
        private readonly IList<ParkingSpace> _emptyParkingSpaces;

        public int TotalSpace => _emptyParkingSpaces.Count + _parkedSpaces.Count;

        public ParkingLot(int totalSpace)
        {
            _emptyParkingSpaces = CreateSpaces(totalSpace);
            _parkedSpaces = new List<ParkingSpace>();
        }

        private static IList<ParkingSpace> CreateSpaces(int totalSpace)
        {
            var result = new List<ParkingSpace>();
            for (var i = 0; i < totalSpace; i++)
            {
                result.Add(new ParkingSpace($"P{i+1}"));
            }

            return result;
        }

        public ParkingSpace GetEmptySpace()
        {
            if (_emptyParkingSpaces.All(space=>!space.IsEmpty))
            {
                throw new ParkingSpaceException("No empty parking space");
            }

            var result = _emptyParkingSpaces.First(space => space.IsEmpty);
            _emptyParkingSpaces.Remove(result);
            _parkedSpaces.Add(result);
            return result;
        }

        public ParkingSpace GetParkedSpace(string id)
        {
            var parkedSpace = _parkedSpaces.FirstOrDefault(space => space.Id == id);
            return parkedSpace ?? throw new ParkingSpaceException($"Can not found parked parking space with id:{id}");
        }

        public bool HasEmptySpaces()
        {
            return _emptyParkingSpaces.Any(space => space.IsEmpty);
        }
    }
}