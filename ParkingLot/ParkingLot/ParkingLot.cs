using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLot
{
    public interface IParkingLot
    {
        string Id { get; }
        int TotalSpace { get; }
        ParkingSpace GetEmptySpace();
        ParkingSpace GetParkedSpace(string id);
        bool HasEmptySpaces();
        IList<ParkingSpace> GetParkedSpaces();
    }

    public class ParkingLot : IParkingLot
    {
        private readonly IList<ParkingSpace> _parkedSpaces;
        private readonly IList<ParkingSpace> _emptyParkingSpaces;

        public string Id { get; }
        public int TotalSpace => _emptyParkingSpaces.Count + _parkedSpaces.Count;

        public ParkingLot(int totalSpace)
        {
            Id = new Guid().ToString();
            _emptyParkingSpaces = CreateSpaces(totalSpace);
            _parkedSpaces = new List<ParkingSpace>();
        }

        private IList<ParkingSpace> CreateSpaces(int totalSpace)
        {
            var result = new List<ParkingSpace>();
            for (var i = 0; i < totalSpace; i++)
            {
                result.Add(new ParkingSpace($"P{i + 1}") {ParkingLotId = Id});
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

        public IList<ParkingSpace> GetParkedSpaces()
        {
            return _parkedSpaces;
        }
    }
}