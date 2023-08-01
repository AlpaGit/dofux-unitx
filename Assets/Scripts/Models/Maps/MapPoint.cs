using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Models.Maps
{

    /// <summary>
    ///     Represents a point on a 2 dimensional plan from a map cell
    /// </summary>
    public sealed class MapPoint
    {
        public const int MinXCoord = 0;
        public const int MaxXCoord = 33;
        public const int MinYCoord = -19;
        public const int MaxYCoord = 13;

        public const uint MapSize = MapConstants.Width * MapConstants.Height * 2;

        private static readonly Point VectorRight = new(1, 1);
        private static readonly Point VectorDownRight = new(1, 0);
        private static readonly Point VectorDown = new(1, -1);
        private static readonly Point VectorDownLeft = new(0, -1);
        private static readonly Point VectorLeft = new(-1, -1);
        private static readonly Point VectorUpLeft = new(-1, 0);
        private static readonly Point VectorUp = new(-1, 1);
        private static readonly Point VectorUpRight = new(0, 1);

        private static bool _initialized;
        public static readonly MapPoint[] OrthogonalGridReference = new MapPoint[MapSize];

        public static MapPoint Middle => Points[315];

        public static MapPoint[] Points { get; }

        static MapPoint()
        {
            Points = new MapPoint[560];

            for (short i = 0; i < Points.Length; i++)
            {
                Points[i] = new MapPoint(i);
            }
        }

        public MapPoint(short cellId)
        {
            CellId = cellId;

            SetFromCellId();
        }

        public MapPoint(Cell cell)
        {
            CellId = cell.Id;

            SetFromCellId();
        }

        public MapPoint(int x, int y)
        {
            X = x;
            Y = y;

            SetFromCoords();
        }

        public MapPoint(Point point)
        {
            X = point.X;
            Y = point.Y;

            SetFromCoords();
        }

        public short CellId { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private void SetFromCoords()
        {
            if (!_initialized)
            {
                InitializeStaticGrid();
            }

            CellId = (short)((X - Y) * MapConstants.Width + Y + (X - Y) / 2);
        }

        private void SetFromCellId()
        {
            if (!_initialized)
            {
                InitializeStaticGrid();
            }

            if (CellId < 0 || CellId > MapSize)
            {
                throw new("Cell identifier out of bounds (" + CellId + ").");
            }

            var point = OrthogonalGridReference[CellId];
            X = point.X;
            Y = point.Y;
        }

        public uint EuclideanDistanceTo(MapPoint point) =>
            (uint)Math.Sqrt((point.X - X) * (point.X - X) + (point.Y - Y) * (point.Y - Y));

        public uint ManhattanDistanceTo(MapPoint point) => (uint)(Math.Abs(X - point.X) + Math.Abs(Y - point.Y));

        public uint ManhattanDistanceToX(MapPoint point) => (uint)Math.Abs(X - point.X);

        public uint ManhattanDistanceToY(MapPoint point) => (uint)Math.Abs(Y - point.Y);

        public uint SquareDistanceTo(MapPoint point) => (uint)Math.Max(Math.Abs(X - point.X), Math.Abs(Y - point.Y));

        public bool IsAdjacentTo(MapPoint point) => ManhattanDistanceTo(point) == 1;

        public bool IsAdjacentOrEqualTo(MapPoint point)
        {
            var distance = ManhattanDistanceTo(point);
            return distance is 1 or 0;
        }

        public Direction OrientationToAdjacent(MapPoint point)
        {
            var vector = new Point
            {
                X = point.X > X ? 1 : point.X < X ? -1 : 0,
                Y = point.Y > Y ? 1 : point.Y < Y ? -1 : 0,
            };

            if (vector == VectorRight)
            {
                return Direction.East;
            }

            if (vector == VectorDownRight)
            {
                return Direction.SouthEast;
            }

            if (vector == VectorDown)
            {
                return Direction.South;
            }

            if (vector == VectorDownLeft)
            {
                return Direction.SouthWest;
            }

            if (vector == VectorLeft)
            {
                return Direction.West;
            }

            if (vector == VectorUpLeft)
            {
                return Direction.NorthWest;
            }

            if (vector == VectorUp)
            {
                return Direction.North;
            }

            if (vector == VectorUpRight)
            {
                return Direction.NorthEast;
            }

            return Direction.East;
        }

        public Direction OrientationTo(MapPoint point, bool diagonal = true)
        {
            var dx = point.X - X;
            var dy = Y - point.Y;

            var distance       = Math.Sqrt(dx * dx + dy * dy);
            var angleInRadians = Math.Acos(dx / distance);

            var angleInDegrees   = angleInRadians * 180 / Math.PI;
            var transformedAngle = angleInDegrees * (point.Y > Y ? -1 : 1);

            var orientation = !diagonal
                ? MathHelper.Round(transformedAngle / 90) * 2 + 1
                : MathHelper.Round(transformedAngle / 45) + 1;

            if (orientation < 0)
            {
                orientation += 8;
            }

            return (Direction)(uint)orientation;
        }

        public Direction OrientationToX(MapPoint point, bool diagonal = false)
        {
            var dx = point.X - X;

            var distance       = Math.Sqrt(dx * dx);
            var angleInRadians = Math.Acos(dx / distance);

            var angleInDegrees = angleInRadians * 180 / Math.PI;

            var orientation = !diagonal
                ? MathHelper.Round(angleInDegrees / 90) * 2 + 1
                : MathHelper.Round(angleInDegrees / 45) + 1;

            if (orientation < 0)
            {
                orientation += 8;
            }

            return (Direction)(uint)orientation;
        }

        public Direction OrientationToY(MapPoint point, bool diagonal = false)
        {
            const int dx = 0;
            var       dy = Y - point.Y;

            var distance       = Math.Sqrt(dx * dx + dy * dy);
            var angleInRadians = Math.Acos(dx / distance);

            var angleInDegrees   = angleInRadians * 180 / Math.PI;
            var transformedAngle = angleInDegrees * (point.Y > Y ? -1 : 1);

            var orientation = !diagonal
                ? MathHelper.Round(transformedAngle / 90) * 2 + 1
                : MathHelper.Round(transformedAngle / 45) + 1;

            if (orientation < 0)
            {
                orientation += 8;
            }

            return (Direction)(uint)orientation;
        }


        public IEnumerable<MapPoint> GetAllCellsInRectangle(MapPoint oppositeCell, bool skipStartAndEndCells = true,
            Func<MapPoint, bool>? predicate = null)
        {
            int x1 = Math.Min(oppositeCell.X, X),
                y1 = Math.Min(oppositeCell.Y, Y),
                x2 = Math.Max(oppositeCell.X, X),
                y2 = Math.Max(oppositeCell.Y, Y);
            for (var x = x1; x <= x2; x++)
                for (var y = y1; y <= y2; y++)
                {
                    if (skipStartAndEndCells && (x == X && y == Y || x == oppositeCell.X && y == oppositeCell.Y))
                    {
                        continue;
                    }

                    var cell = GetPoint(x, y);
                    if (cell != null && (predicate == null || predicate(cell)))
                    {
                        yield return cell;
                    }
                }
        }

        public bool IsOnSameLine(MapPoint point) => point.X == X || point.Y == Y;

        public bool IsOnSameDiagonal(MapPoint point) =>
            point.X + point.Y == X + Y ||
            point.X - point.Y == X - Y;

        /// <summary>
        ///     Returns true whenever this point is between points A and B
        /// </summary>
        /// <returns></returns>
        public bool IsBetween(MapPoint a, MapPoint b)
        {
            // check colinearity
            if ((X - a.X) * (b.Y - Y) - (Y - a.Y) * (b.X - X) != 0)
            {
                return false;
            }

            var min = new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
            var max = new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
            // check position
            return X >= min.X && X <= max.X && Y >= min.Y && Y <= max.Y;
        }

        public MapPoint[] GetCellsOnLineBetween(MapPoint destination)
        {
            var result    = new List<MapPoint>();
            var direction = OrientationTo(destination);
            var current   = this;
            for (var i = 0; i < MapConstants.Height * MapConstants.Width / 2; i++)
            {
                current = current.GetCellInDirection(direction, 1);

                if (current == null)
                {
                    break;
                }

                if (current.CellId == destination.CellId)
                {
                    break;
                }

                result.Add(current);
            }

            return result.ToArray();
        }

        public IEnumerable<MapPoint> GetCellsInLine(MapPoint destination) => GetCellsInLine(this, destination);

        public static IEnumerable<MapPoint> GetCellsInLine(MapPoint source, MapPoint destination)
        {
            // http://playtechs.blogspot.fr/2007/03/raytracing-on-grid.html

            var dx      = Math.Abs(destination.X - source.X);
            var dy      = Math.Abs(destination.Y - source.Y);
            var x       = source.X;
            var y       = source.Y;
            var n       = 1 + dx + dy;
            var vectorX = destination.X > source.X ? 1 : -1;
            var vectorY = destination.Y > source.Y ? 1 : -1;
            var error   = dx - dy;
            dx *= 2;
            dy *= 2;

            for (; n > 0; --n)
            {
                var cell = GetPoint(x, y);
                if (cell != null)
                {
                    yield return cell;
                }

                switch (error)
                {
                    case > 0:
                        x     += vectorX;
                        error -= dy;
                        break;
                    case 0:
                        x += vectorX;
                        y += vectorY;
                        n--;
                        break;
                    default:
                        y     += vectorY;
                        error += dx;
                        break;
                }
            }
        }


        public static IEnumerable<MapPoint> GetBorderCells(MapNeighbour mapNeighbour)
        {
            return mapNeighbour switch
                   {
                       MapNeighbour.Top    => GetCellsInLine(new(546), new(559)),
                       MapNeighbour.Left   => GetCellsInLine(new(27), new(559)),
                       MapNeighbour.Bottom => GetCellsInLine(new(0), new(13)),
                       MapNeighbour.Right  => GetCellsInLine(new(0), new(532)),
                       _                   => Array.Empty<MapPoint>(),
                   };
        }

        public static IEnumerable<MapPoint> GetBorderCells(Direction mapNeighbour)
        {
            return mapNeighbour switch
                   {
                       Direction.North => GetCellsInLine(new(546), new(559)),
                       Direction.West  => GetCellsInLine(new(27), new(559)),
                       Direction.South => GetCellsInLine(new(0), new(13)),
                       Direction.East  => GetCellsInLine(new(0), new(532)),
                       _               => Array.Empty<MapPoint>(),
                   };
        }

        public MapPoint? GetCellInDirection(Direction direction, int step)
        {
            var mapPoint = direction switch
                           {
                               Direction.East      => GetPoint(X + step, Y + step),
                               Direction.SouthEast => GetPoint(X + step, Y),
                               Direction.South     => GetPoint(X + step, Y - step),
                               Direction.SouthWest => GetPoint(X, Y - step),
                               Direction.West      => GetPoint(X - step, Y - step),
                               Direction.NorthWest => GetPoint(X - step, Y),
                               Direction.North     => GetPoint(X - step, Y + step),
                               Direction.NorthEast => GetPoint(X, Y + step),
                               _                   => null,
                           };

            if (mapPoint != null)
            {
                return IsInMap(mapPoint.X, mapPoint.Y) ? mapPoint : null;
            }

            return null;
        }

        public MapPoint? GetNearestCellInDirection(Direction direction) => GetCellInDirection(direction, 1);

        public IEnumerable<MapPoint> GetAdjacentCells(bool diagonal = false)
        {
            return GetAdjacentCells(_ => true, diagonal);
        }

        public IEnumerable<MapPoint> GetAdjacentCells(byte range, bool diagonal = false)
        {
            return GetAdjacentCells(_ => true, diagonal, range);
        }

        public IEnumerable<MapPoint> GetAdjacentCells(Func<short, bool> predicate, bool diagonal = false,
            byte range = 1)
        {
            for (var i = 1; i <= range; i++)
            {
                var northEast = new MapPoint(X, Y + i);
                if (IsInMap(northEast.X, northEast.Y) && predicate(northEast.CellId))
                {
                    yield return northEast;
                }

                var southEast = new MapPoint(X + i, Y);
                if (IsInMap(southEast.X, southEast.Y) && predicate(southEast.CellId))
                {
                    yield return southEast;
                }

                var southWest = new MapPoint(X, Y - i);
                if (IsInMap(southWest.X, southWest.Y) && predicate(southWest.CellId))
                {
                    yield return southWest;
                }

                var northWest = new MapPoint(X - i, Y);
                if (IsInMap(northWest.X, northWest.Y) && predicate(northWest.CellId))
                {
                    yield return northWest;
                }

                if (!diagonal)
                {
                    continue;
                }

                var south = new MapPoint(X + i, Y - i);
                if (IsInMap(south.X, south.Y) && predicate(south.CellId))
                {
                    yield return south;
                }

                var west = new MapPoint(X - i, Y - i);
                if (IsInMap(west.X, west.Y) && predicate(west.CellId))
                {
                    yield return west;
                }

                var north = new MapPoint(X - i, Y + i);
                if (IsInMap(north.X, north.Y) && predicate(north.CellId))
                {
                    yield return north;
                }

                var east = new MapPoint(X + i, Y + i);
                if (IsInMap(east.X, east.Y) && predicate(east.CellId))
                {
                    yield return east;
                }
            }
        }

        public uint AdvancedOrientationTo(MapPoint? target, bool fourDir = true)
        {
            if (target == null)
            {
                return 0;
            }

            var xDifference = target.X - X;
            var yDifference = Y - target.Y;

            var angle =
                Math.Acos(xDifference / Math.Sqrt(Math.Pow(xDifference, 2) + Math.Pow(yDifference, 2))) * 180 /
                Math.PI *
                (target.Y > Y ? -1 : 1);

            if (fourDir)
            {
                angle = MathHelper.Round(angle / 90) * 2 + 1;
            }
            else
            {
                angle = MathHelper.Round(angle / 45) + 1;
            }

            if (angle < 0)
            {
                angle += 8;
            }

            return (uint)angle;
        }

        public bool IsInMap() => IsInMap(X, Y);

        public static bool IsInMap(int x, int y) =>
            x + y >= 0 && x - y >= 0 && x - y < MapConstants.Height * 2 && x + y < MapConstants.Width * 2;

        public static uint CoordToCellId(int x, int y)
        {
            if (!_initialized)
            {
                InitializeStaticGrid();
            }

            return (uint)((x - y) * MapConstants.Width + y + (x - y) / 2);
        }

        public static Point? CellIdToCoord(uint cellId)
        {
            if (!_initialized)
            {
                InitializeStaticGrid();
            }

            var point = GetPoint((short)cellId);

            if (point == null)
            {
                return null;
            }

            return new Point(point.X, point.Y);
        }

        /// <summary>
        ///     Initialize a static 2D plan that is used as reference to convert a cell to a (X,Y) point
        /// </summary>
        private static void InitializeStaticGrid()
        {
            _initialized = true;

            var posX      = 0;
            var posY      = 0;
            var cellCount = 0;

            for (var x = 0; x < MapConstants.Height; x++)
            {
                for (var y = 0; y < MapConstants.Width; y++)
                {
                    OrthogonalGridReference[cellCount++] = new(posX + y, posY + y);
                }

                posX++;

                for (var y = 0; y < MapConstants.Width; y++)
                {
                    OrthogonalGridReference[cellCount++] = new(posX + y, posY + y);
                }

                posY--;
            }
        }

        public static MapPoint? GetPoint(int cellId)
        {
            return GetPoint((short)cellId);
        }

        public static MapPoint? GetPoint(int x, int y)
        {
            return GetPoint(CoordToCellId(x, y));
        }

        public static MapPoint? GetPoint(uint cell)
        {
            return GetPoint((short)cell);
        }

        public static MapPoint? GetPoint(short cell)
        {
            if (cell < 0 || cell >= MapSize)
            {
                return null;
            }

            if (!OrthogonalGridReference.Any(x => x.CellId == cell))
            {
                return null;
            }

            return OrthogonalGridReference[cell];
        }

        public static MapPoint? GetPoint(Cell? cell) => cell == null ? null : GetPoint(cell.Id);

        public static MapPoint[] GetAllPoints()
        {
            if (!_initialized)
            {
                InitializeStaticGrid();
            }

            return OrthogonalGridReference;
        }

        public static implicit operator MapPoint(Cell cell) => new(cell);

        public override string ToString() => "[MapPoint(x:" + X + ", y:" + Y + ", id:" + CellId + ")]";

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is MapPoint point && Equals(point);
        }

        private bool Equals(MapPoint? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.CellId == CellId && other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = CellId;
                result = result * 397 ^ X;
                result = result * 397 ^ Y;
                return result;
            }
        }

        public int GetOrientationsDistance(int currentOrientation, int defaultOrientation)
        {
            return Math.Min(Math.Abs(defaultOrientation - currentOrientation),
                Math.Abs(8 - defaultOrientation + currentOrientation));
        }
    }
}