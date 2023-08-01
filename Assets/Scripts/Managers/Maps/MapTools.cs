using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Components.Maps;
using Models.Maps;

namespace Managers.Maps
{
    public static class MapTools
    {
        private static readonly int MapGridWidth;
        private static readonly int MapGridHeight;

        private static readonly int MinXCoord;
        private static readonly int MaxXCoord;

        private static readonly int MinYCoord;
        private static readonly int MaxYCoord;

        public static readonly short[] EveryCellId;

        public static readonly int MapCountCell;
        public const int InvalidCellId = -1;
        private const int PseudoInfinite = 63;

        private static readonly Point[] CoordinatesDirection =
        {
            new(1, 1),
            new(1, 0),
            new(1, -1),
            new(0, -1),
            new(-1, -1),
            new(-1, 0),
            new(-1, 1),
            new(0, 1),
        };

        public static double CoeffForRebaseOnClosest8Direction = Math.Tan(Math.PI / 8);

        static MapTools()
        {
            MapGridWidth  = MapToolsConfig.Dofus2Config.MapGridWidth;
            MapGridHeight = MapToolsConfig.Dofus2Config.MapGridHeight;
            MinXCoord     = MapToolsConfig.Dofus2Config.MinXCoord;
            MaxXCoord     = MapToolsConfig.Dofus2Config.MaxXCoord;
            MinYCoord     = MapToolsConfig.Dofus2Config.MinYCoord;
            MaxYCoord     = MapToolsConfig.Dofus2Config.MaxYCoord;

            MapCountCell = MapGridWidth * MapGridHeight * 2;
            EveryCellId  = new short[MapCountCell];

            for (var i = 0; i < MapCountCell; i++)
            {
                EveryCellId[i] = (short)i;
            }
        }

        public static bool IsValidCellId(int cellId)
        {
            return cellId >= 0 && cellId < MapCountCell;
        }

        public static bool IsValidCoord(int xCoord, int yCoord)
        {
            if (yCoord >= -xCoord && yCoord <= xCoord && yCoord <= MapGridWidth + MaxYCoord - xCoord)
            {
                return yCoord >= xCoord - (MapGridHeight - MinYCoord);
            }

            return false;
        }

        /// <summary>
        /// Determines if two cells are in the same diagonal.
        /// </summary>
        /// <param name="cellId1">The ID of the first cell.</param>
        /// <param name="cellId2">The ID of the second cell.</param>
        /// <returns>True if the cells are in the same diagonal, otherwise False.</returns>
        public static bool IsInDiag(int cellId1, int cellId2)
        {
            // Get the X and Y coordinates of the two cells
            var cellCoord1 = GetCellCoordById(cellId1);
            var cellCoord2 = GetCellCoordById(cellId2);

            if (cellCoord1 == null || cellCoord2 == null)
            {
                return false;
            }

            // Check if the cells are in the same diagonal
            return IsInDiagByCoord(cellCoord1.Value.X, cellCoord1.Value.Y, cellCoord2.Value.X, cellCoord2.Value.Y);
        }

        /// <summary>
        /// Determines if two cells specified by their X and Y coordinates are in the same diagonal.
        /// </summary>
        /// <param name="xCoord1">The X coordinate of the first cell.</param>
        /// <param name="yCoord1">The Y coordinate of the first cell.</param>
        /// <param name="xCoord2">The X coordinate of the second cell.</param>
        /// <param name="yCoord2">The Y coordinate of the second cell.</param>
        /// <returns>True if the cells are in the same diagonal, otherwise False.</returns>
        public static bool IsInDiagByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            if (!IsValidCoord(xCoord1, yCoord1) || !IsValidCoord(xCoord2, yCoord2))
            {
                return false;
            }

            return (int)Math.Floor(Math.Abs((double)(xCoord1 - xCoord2))) ==
                   (int)Math.Floor(Math.Abs((double)(yCoord1 - yCoord2)));
        }

        public static Point? GetCellCoordById(int cellId)
        {
            if (!IsValidCellId(cellId))
            {
                return null;
            }

            var floorDivision     = Math.Floor((double)cellId / MapGridWidth);
            var halfFloorDivision = Math.Floor((floorDivision + 1) / 2);
            var diff              = floorDivision - halfFloorDivision;
            var xCoord            = cellId - floorDivision * MapGridWidth;

            return new Point((int)(halfFloorDivision + xCoord), (int)(xCoord - diff));
        }

        public static int GetCellIdXCoord(int cellId)
        {
            var floorDivision     = (int)Math.Floor((double)cellId / MapGridWidth);
            var halfFloorDivision = (int)Math.Floor((double)(floorDivision + 1) / 2);
            var xCoord            = cellId - floorDivision * MapGridWidth;
            return halfFloorDivision + xCoord;
        }

        public static int GetCellIdYCoord(int cellId)
        {
            var floorDivision     = (int)Math.Floor((double)cellId / MapGridWidth);
            var halfFloorDivision = (int)Math.Floor((double)(floorDivision + 1) / 2);
            var diff              = floorDivision - halfFloorDivision;
            var yCoord            = cellId - floorDivision * MapGridWidth;
            return yCoord - diff;
        }

        public static int GetCellIdByCoord(int xCoord, int yCoord)
        {
            if (!IsValidCoord(xCoord, yCoord))
            {
                return InvalidCellId;
            }

            return (int)Math.Floor((xCoord - yCoord) * MapGridWidth + yCoord + (double)(xCoord - yCoord) / 2);
        }

        public static bool FloatAlmostEquals(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < 0.0001;
        }

        public static IEnumerable<int> GetCellsIdBetween(int cellId1, int cellId2)
        {
            if (cellId1 == cellId2)
            {
                yield break;
            }

            if (!IsValidCellId(cellId1) || !IsValidCellId(cellId2))
            {
                yield break;
            }

            var x1 = GetCellIdXCoord(cellId1);
            var y1 = GetCellIdYCoord(cellId1);
            var x2 = GetCellIdXCoord(cellId2);
            var y2 = GetCellIdYCoord(cellId2);

            var dx = x2 - x1;
            var dy = y2 - y1;

            var distance = Math.Sqrt(dx * dx + dy * dy);
            var stepX    = dx / distance;
            var stepY    = dy / distance;

            var absStepX = Math.Abs(1 / stepX);
            var absStepY = Math.Abs(1 / stepY);

            var signX = stepX < 0 ? -1 : 1;
            var signY = stepY < 0 ? -1 : 1;

            var progressX = 0.5 * absStepX;
            var progressY = 0.5 * absStepY;

            while (x1 != x2 || y1 != y2)
            {
                if (FloatAlmostEquals(progressX, progressY))
                {
                    progressX += absStepX;
                    progressY += absStepY;
                    x1        += signX;
                    y1        += signY;
                }
                else if (progressX < progressY)
                {
                    progressX += absStepX;
                    x1        += signX;
                }
                else
                {
                    progressY += absStepY;
                    y1        += signY;
                }

                yield return GetCellIdByCoord(x1, y1);
            }
        }

        /// <summary>
        /// Gets the look direction between two cells based on their IDs.
        /// </summary>
        /// <param name="cellId1">The ID of the first cell.</param>
        /// <param name="cellId2">The ID of the second cell.</param>
        /// <returns>The look direction between the two cells, or -1 if the coordinates are not valid.</returns>
        public static int GetLookDirection4(int cellId1, int cellId2)
        {
            var cellCoord1 = GetCellCoordById(cellId1);
            var cellCoord2 = GetCellCoordById(cellId2);

            if (cellCoord1 == null || cellCoord2 == null)
            {
                return InvalidCellId;
            }

            return GetLookDirection4ByCoord(cellCoord1.Value.X, cellCoord1.Value.Y, cellCoord2.Value.X,
                cellCoord2.Value.Y);
        }

        /// <summary>
        /// Gets the look direction between two cells specified by their X and Y coordinates.
        /// </summary>
        /// <param name="xCoord1">The X coordinate of the first cell.</param>
        /// <param name="yCoord1">The Y coordinate of the first cell.</param>
        /// <param name="xCoord2">The X coordinate of the second cell.</param>
        /// <param name="yCoord2">The Y coordinate of the second cell.</param>
        /// <returns>The look direction between the two cells, or -1 if the coordinates are not valid.</returns>
        public static int GetLookDirection4ByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            if (!IsValidCoord(xCoord1, yCoord1) || !IsValidCoord(xCoord2, yCoord2))
            {
                return InvalidCellId;
            }

            var deltaX = xCoord1 - xCoord2;
            var deltaY = yCoord1 - yCoord2;

            if ((int)Math.Floor(Math.Abs((double)deltaX)) > (int)Math.Floor(Math.Abs((double)deltaY)))
            {
                return deltaX < 0 ? 1 : 5;
            }

            return deltaY < 0 ? 7 : 3;
        }

        /// <summary>
        /// Returns the 4-directional look direction between two cells.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>An integer representing the 4-directional look direction between the two given cells.</returns>
        public static int GetLookDirection4Exact(int cellId1, int cellId2)
        {
            var xCoord1 = GetCellIdXCoord(cellId1);
            var yCoord1 = GetCellIdYCoord(cellId1);

            var xCoord2 = GetCellIdXCoord(cellId2);
            var yCoord2 = GetCellIdYCoord(cellId2);

            return GetLookDirection4ExactByCoord(xCoord1, yCoord1, xCoord2, yCoord2);
        }

        /// <summary>
        /// Returns the 4-directional look direction between two coordinates.
        /// </summary>
        /// <param name="xCoord1">The first X coordinate.</param>
        /// <param name="yCoord1">The first Y coordinate.</param>
        /// <param name="xCoord2">The second X coordinate.</param>
        /// <param name="yCoord2">The second Y coordinate.</param>
        /// <returns>An integer representing the 4-directional look direction between the two given coordinates.</returns>
        public static int GetLookDirection4ExactByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            if (!IsValidCoord(xCoord1, yCoord1) || !IsValidCoord(xCoord2, yCoord2))
            {
                return InvalidCellId;
            }

            var deltaX = xCoord2 - xCoord1;
            var deltaY = yCoord2 - yCoord1;

            if (deltaY == 0)
            {
                return deltaX < 0 ? 5 : 1;
            }

            if (deltaX == 0)
            {
                return deltaY < 0 ? 3 : 7;
            }

            return InvalidCellId;
        }

        /// <summary>
        /// Returns the 4-directional diagonal look direction between two cells.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>An integer representing the 4-directional diagonal look direction between the two given cells.</returns>
        public static int GetLookDirection4Diag(int cellId1, int cellId2)
        {
            var xCoord1 = GetCellIdXCoord(cellId1);
            var yCoord1 = GetCellIdYCoord(cellId1);

            var xCoord2 = GetCellIdXCoord(cellId2);
            var yCoord2 = GetCellIdYCoord(cellId2);

            return GetLookDirection4DiagByCoord(xCoord1, yCoord1, xCoord2, yCoord2);
        }

        /// <summary>
        /// Returns the 4-directional diagonal look direction between two coordinates.
        /// </summary>
        /// <param name="xCoord1">The first X coordinate.</param>
        /// <param name="yCoord1">The first Y coordinate.</param>
        /// <param name="xCoord2">The second X coordinate.</param>
        /// <param name="yCoord2">The second Y coordinate.</param>
        /// <returns>An integer representing the 4-directional diagonal look direction between the two given coordinates.</returns>
        public static int GetLookDirection4DiagByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            if (!IsValidCoord(xCoord1, yCoord1) || !IsValidCoord(xCoord2, yCoord2))
            {
                return InvalidCellId;
            }

            var deltaX = xCoord2 - xCoord1;
            var deltaY = yCoord2 - yCoord1;

            if (deltaX >= 0 && deltaY <= 0 || deltaX <= 0 && deltaY >= 0)
            {
                return deltaX < 0 ? 6 : 2;
            }

            return deltaX < 0 ? 4 : 0;
        }

        /// <summary>
        /// Returns a value representing the diagonal direction between two cell IDs.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>A value representing the diagonal direction, or -1 if the cells are not in a diagonal.</returns>
        public static int GetLookDirection4DiagExact(int cellId1, int cellId2)
        {
            var cellCoord1 = GetCellCoordById(cellId1);
            var cellCoord2 = GetCellCoordById(cellId2);

            if (!cellCoord1.HasValue || !cellCoord2.HasValue)
            {
                return InvalidCellId;
            }

            return GetLookDirection4DiagExactByCoord(cellCoord1.Value.X, cellCoord1.Value.Y, cellCoord2.Value.X,
                cellCoord2.Value.Y);
        }

        /// <summary>
        /// Returns a value representing the diagonal direction between two coordinates in a 2D isometric game.
        /// </summary>
        /// <param name="xCoord1">The X coordinate of the first cell.</param>
        /// <param name="yCoord1">The Y coordinate of the first cell.</param>
        /// <param name="xCoord2">The X coordinate of the second cell.</param>
        /// <param name="yCoord2">The Y coordinate of the second cell.</param>
        /// <returns>A value representing the diagonal direction, or -1 if the cells are not in a diagonal.</returns>
        public static int GetLookDirection4DiagExactByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            if (!IsValidCoord(xCoord1, yCoord1) || !IsValidCoord(xCoord2, yCoord2))
            {
                return InvalidCellId;
            }

            var deltaX = xCoord2 - xCoord1;
            var deltaY = yCoord2 - yCoord1;

            if (deltaX == -deltaY)
            {
                return deltaX < 0 ? 6 : 2;
            }

            if (deltaX == deltaY)
            {
                return deltaX < 0 ? 4 : 0;
            }

            return InvalidCellId;
        }

        /// <summary>
        /// Determines the look direction in an 8-direction system for two given cell IDs.
        /// </summary>
        /// <param name="cellId1">The cell ID of the first cell.</param>
        /// <param name="cellId2">The cell ID of the second cell.</param>
        /// <returns>The look direction between the two cells. Returns -1 if the coordinates are invalid.</returns>
        public static int GetLookDirection8(int cellId1, int cellId2)
        {
            var xCoord1 = GetCellIdXCoord(cellId1);
            var yCoord1 = GetCellIdYCoord(cellId1);

            var xCoord2 = GetCellIdXCoord(cellId2);
            var yCoord2 = GetCellIdYCoord(cellId2);

            return GetLookDirection8ByCoord(xCoord1, yCoord1, xCoord2, yCoord2);
        }

        /// <summary>
        /// Returns a value representing the 8-direction between two coordinates in a 2D isometric game.
        /// </summary>
        /// <param name="point1">The coordinate of the first cell.</param>
        /// <param name="point2">The coordinate of the second cell.</param>
        /// <returns>A value representing the 8-direction, or -1 if the cells are not in a valid direction.</returns>
        public static int GetLookDirection8ByCoord(Point point1, Point point2)
        {
            return GetLookDirection8ByCoord(point1.X, point1.Y, point2.X, point2.Y);
        }

        /// <summary>
        /// Returns a value representing the 8-direction between two coordinates in a 2D isometric game.
        /// </summary>
        /// <param name="xCoord1">The X coordinate of the first cell.</param>
        /// <param name="yCoord1">The Y coordinate of the first cell.</param>
        /// <param name="xCoord2">The X coordinate of the second cell.</param>
        /// <param name="yCoord2">The Y coordinate of the second cell.</param>
        /// <returns>A value representing the 8-direction, or -1 if the cells are not in a valid direction.</returns>
        public static int GetLookDirection8ByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            var direction = GetLookDirection8ExactByCoord(xCoord1, yCoord1, xCoord2, yCoord2);

            if (MapDirection.IsValidDirection(direction))
            {
                return direction;
            }

            var deltaX    = xCoord2 - xCoord1;
            var deltaY    = yCoord2 - yCoord1;
            var absDeltaX = Math.Abs(deltaX);
            var absDeltaY = Math.Abs(deltaY);

            if (absDeltaX < absDeltaY)
            {
                if (deltaY > 0)
                {
                    direction = deltaX < 0 ? 6 : 7;
                }
                else
                {
                    direction = deltaX < 0 ? 3 : 2;
                }
            }
            else if (deltaX > 0)
            {
                direction = deltaY > 0 ? 0 : 1;
            }
            else
            {
                direction = deltaY < 0 ? 4 : 5;
            }

            return direction;
        }

        /// <summary>
        /// Returns a value representing the 8-direction between two cell IDs in a 2D isometric game.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>A value representing the 8-direction, or -1 if the cells are not in a valid direction.</returns>
        public static int GetLookDirection8Exact(int cellId1, int cellId2)
        {
            var cellCoord1 = GetCellCoordById(cellId1);
            var cellCoord2 = GetCellCoordById(cellId2);

            if (!cellCoord1.HasValue || !cellCoord2.HasValue)
            {
                return InvalidCellId;
            }

            return GetLookDirection8ExactByCoord(cellCoord1.Value.X, cellCoord1.Value.Y, cellCoord2.Value.X,
                cellCoord2.Value.Y);
        }

        public static int GetLookDirection8ExactByCoord(Point coord1, Point coord2)
        {
            return GetLookDirection8ExactByCoord(coord1.X, coord1.Y, coord2.X, coord2.Y);
        }

        /// <summary>
        /// Returns a value representing the 8-direction between two coordinates in a 2D isometric game, considering both 4-direction and diagonal.
        /// </summary>
        /// <param name="xCoord1">The X coordinate of the first cell.</param>
        /// <param name="yCoord1">The Y coordinate of the first cell.</param>
        /// <param name="xCoord2">The X coordinate of the second cell.</param>
        /// <param name="yCoord2">The Y coordinate of the second cell.</param>
        /// <returns>A value representing the 8-direction, or -1 if the cells are not in a valid direction.</returns>
        public static int GetLookDirection8ExactByCoord(int xCoord1, int yCoord1, int xCoord2, int yCoord2)
        {
            var direction = GetLookDirection4ExactByCoord(xCoord1, yCoord1, xCoord2, yCoord2);
            if (!MapDirection.IsValidDirection(direction))
            {
                direction = GetLookDirection4DiagExactByCoord(xCoord1, yCoord1, xCoord2, yCoord2);
            }

            return direction;
        }

        /// <summary>
        /// Returns the cell ID of the next cell in the given direction.
        /// </summary>
        /// <param name="cellId">The initial cell ID.</param>
        /// <param name="direction">The direction in which to look for the next cell.</param>
        /// <returns>The cell ID of the next cell in the given direction.</returns>
        public static int GetNextCellByDirection(int cellId, int direction)
        {
            var xCoord = GetCellIdXCoord(cellId);
            var yCoord = GetCellIdYCoord(cellId);

            return GetNextCellByDirectionAndCoord(xCoord, yCoord, direction);
        }

        /// <summary>
        /// Gets the next cell ID in the specified direction from the given coordinates.
        /// </summary>
        /// <param name="xCoord">The X coordinate of the current cell.</param>
        /// <param name="yCoord">The Y coordinate of the current cell.</param>
        /// <param name="direction">The direction to move towards (0-7).</param>
        /// <returns>The ID of the next cell in the specified direction, or -1 if the coordinates or direction are invalid.</returns>
        public static int GetNextCellByDirectionAndCoord(int xCoord, int yCoord, int direction)
        {
            if (!IsValidCoord(xCoord, yCoord) || !MapDirection.IsValidDirection(direction))
            {
                return InvalidCellId;
            }

            return GetCellIdByCoord(xCoord + CoordinatesDirection[direction].X,
                yCoord + CoordinatesDirection[direction].Y);
        }

        /// <summary>
        /// Returns the distance between two cells.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>The distance between the two cells or -1 if one of the cell IDs is invalid.</returns>
        public static int GetDistance(int cellId1, int cellId2)
        {
            if (!IsValidCellId(cellId1) || !IsValidCellId(cellId2))
            {
                return InvalidCellId;
            }

            var xCoord1 = GetCellIdXCoord(cellId1);
            var yCoord1 = GetCellIdYCoord(cellId1);

            var xCoord2 = GetCellIdXCoord(cellId2);
            var yCoord2 = GetCellIdYCoord(cellId2);

            return (int)Math.Floor((double)Math.Abs(xCoord2 - xCoord1) + Math.Abs(yCoord2 - yCoord1));
        }

        /// <summary>
        /// Determines if two cells are adjacent.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>True if the cells are adjacent, false otherwise.</returns>
        public static bool AreCellsAdjacent(int cellId1, int cellId2)
        {
            var distance = GetDistance(cellId1, cellId2);

            return distance is >= 0 and <= 1;
        }

        /// <summary>
        /// Returns the coordinates of the cells between two cells.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>A list of Point objects representing the coordinates of the cells between the two given cells.</returns>
        public static IList<Point> GetCellsCoordBetween(int cellId1, int cellId2)
        {
            var cellIdsBetween = GetCellsIdBetween(cellId1, cellId2);

            return cellIdsBetween.Select(GetCellCoordById)
                                 .Where(cellCoord => cellCoord.HasValue)
                                 .Select(cellCoord => cellCoord!.Value).ToList();
        }

        /// <summary>
        /// Returns the cell IDs on the path between two cells, taking into account large ways.
        /// </summary>
        /// <param name="cellId1">The first cell ID.</param>
        /// <param name="cellId2">The second cell ID.</param>
        /// <returns>A list of integers representing the cell IDs on the path between the two given cells.</returns>
        public static IList<int> GetCellsIdOnLargeWay(int cellId1, int cellId2)
        {
            var xCoord1 = GetCellIdXCoord(cellId1);
            var yCoord1 = GetCellIdYCoord(cellId1);

            var xCoord2 = GetCellIdXCoord(cellId2);
            var yCoord2 = GetCellIdYCoord(cellId2);

            var direction = GetLookDirection8ExactByCoord(xCoord1, yCoord1, xCoord2, yCoord2);

            if (!MapDirection.IsValidDirection(direction))
            {
                return Array.Empty<int>();
            }

            var cellIdsOnLargeWay = new List<int> { cellId1, };
            var currentCellId     = cellId1;

            while (currentCellId != cellId2)
            {
                if (MapDirection.IsCardinal(direction))
                {
                    var nextCellId1 = GetNextCellByDirection(currentCellId, (direction + 1) % 8);
                    if (IsValidCellId(nextCellId1))
                    {
                        cellIdsOnLargeWay.Add(nextCellId1);
                    }

                    var nextCellId2 = GetNextCellByDirection(currentCellId, (direction + 8 - 1) % 8);
                    if (IsValidCellId(nextCellId2))
                    {
                        cellIdsOnLargeWay.Add(nextCellId2);
                    }
                }

                currentCellId = GetNextCellByDirection(currentCellId, direction);
                cellIdsOnLargeWay.Add(currentCellId);
            }

            return cellIdsOnLargeWay;
        }
    }
}