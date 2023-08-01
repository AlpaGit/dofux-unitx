using System.Collections.Generic;

namespace Models.Maps
{
    public class ClientMovementPath
    {
        private const int MaxPathLength = 100;

        public MapPoint Start { get; set; } = MapPoint.GetPoint(0)!;
        public MapPoint End { get; set; } = MapPoint.GetPoint(0)!;
        public List<PathElement> Path { get; set; } = new();


        public void AddPoint(PathElement element)
        {
            Path.Add(element);
        }

        public void Compress()
        {
            if(Path.Count <= 1)
                return;

            var elem = Path.Count - 1;

            while (elem > 0)
            {
                if(Path[elem].Orientation == Path[elem - 1].Orientation)
                {
                    Path.RemoveAt(elem);
                }
            
                elem--;
            }
        }
        public IEnumerable<short> GetServerPath()
        {
            Compress();

            var movement        = new List<short>();
            var lastOrientation = Path[0].Orientation;
            foreach (var path in Path)
            {
                lastOrientation = path.Orientation;
                var value = ((int)lastOrientation & 0x07) << 12 | path.Step.CellId & 0xFFF;
                movement.Add((short)value);
            }

            var lastCell  = End.CellId;
            var lastValue = ((int)lastOrientation & 0x07) << 12 | lastCell & 0xFFF;

            movement.Add((short)lastValue);

            return movement;
        }
    }

    public class PathElement
    {
        public MapPoint Step { get; set; }
        public uint Orientation { get; set; }

        public PathElement(MapPoint step, uint orientation)
        {
            Step        = step;
            Orientation = orientation;
        }
    }
}