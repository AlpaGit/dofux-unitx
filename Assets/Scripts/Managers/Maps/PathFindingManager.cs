using System;
using System.Collections.Generic;
using Components.Maps;
using Models.Maps;

namespace Managers.Maps
{
    public static class PathFindingManager
    {
        public static ClientMovementPath FindClientPath(Map map, short startCellId, short endCellId, bool allowDiag = true,
            bool allowThroughEntity = true)
        {
            if (startCellId == endCellId)
                return new ClientMovementPath();

            var distanceToEnd = MapTools.GetDistance(startCellId, endCellId);
            var start         = MapTools.GetCellCoordById(startCellId)!;
            var endX          = MapTools.GetCellIdXCoord(endCellId);
            var endY          = MapTools.GetCellIdYCoord(endCellId);

            short endCellAuxId = -1;

            var costOfCell      = new double[MapTools.MapCountCell];
            var openListWeights = new double[MapTools.MapCountCell];
            var parentOfCell    = new int[MapTools.MapCountCell];
            var isCellClosed    = new bool[MapTools.MapCountCell];
            var isEntityOnCell  = new bool[MapTools.MapCountCell];
            var openList        = new List<int>(40);

            for (var cellId = 0; cellId < MapTools.MapCountCell; cellId++)
            {
                parentOfCell[cellId]   = -1;
                isCellClosed[cellId]   = false;
                isEntityOnCell[cellId] = false;
            }

            openList.Clear();
            costOfCell[startCellId] = 0;
            openList.Add(startCellId);

            while (openList.Count > 0 && isCellClosed[endCellId] == false)
            {
                var minimum           = 99999999d;
                var smallestCostIndex = 0;

                for (var i = 0; i < openList.Count; i++)
                {
                    var cost = openListWeights[openList[i]];
                    if (cost <= minimum)
                    {
                        minimum           = cost;
                        smallestCostIndex = i;
                    }
                }

                var parentId = openList[smallestCostIndex];
                var parentX  = MapTools.GetCellIdXCoord(parentId);
                var parentY  = MapTools.GetCellIdYCoord(parentId);

                openList.RemoveAt(smallestCostIndex);
                isCellClosed[parentId] = true;
                var y = parentY - 1;

                while (y <= parentY + 1)
                {
                    var x = parentX - 1;
                    while (x <= parentX + 1)
                    {
                        var cellId = MapTools.GetCellIdByCoord(x, y);
                        if (cellId != MapTools.InvalidCellId && isCellClosed[cellId] == false &&
                            cellId != parentId &&
                            map.PointMov(x, y, parentId) &&
                            (y == parentY || x == parentX || allowDiag &&
                                (map.PointMov(parentX, y, parentId) || map.PointMov(x,
                                    parentY, parentId))))
                        {
                            var pointWeight = 0d;

                            if (cellId == endCellId)
                            {
                                pointWeight = 1;
                            }
                            else
                            {
                                var speed = map.Cells[(short)cellId].Speed;

                                if (allowThroughEntity)
                                {
                                    if (isEntityOnCell[cellId])
                                    {
                                        pointWeight = 20;
                                    }
                                    else
                                    {
                                        if (speed >= 0)
                                        {
                                            pointWeight = 6 - speed;
                                        }
                                        else
                                        {
                                            pointWeight = 12 + Math.Abs(speed);
                                        }
                                    }
                                }
                                else
                                {
                                    pointWeight = 1;

                                    if (isEntityOnCell[cellId])
                                    {
                                        pointWeight += 0.3;
                                    }

                                    if (MapTools.IsValidCoord(x + 1, y) &&
                                        isEntityOnCell[MapTools.GetCellIdByCoord(x + 1, y)])
                                    {
                                        pointWeight += 0.3;
                                    }

                                    if (MapTools.IsValidCoord(x, y + 1) &&
                                        isEntityOnCell[MapTools.GetCellIdByCoord(x, y + 1)])
                                    {
                                        pointWeight += 0.3;
                                    }

                                    if (MapTools.IsValidCoord(x - 1, y) &&
                                        isEntityOnCell[MapTools.GetCellIdByCoord(x - 1, y)])
                                    {
                                        pointWeight += 0.3;
                                    }

                                    if (MapTools.IsValidCoord(x, y - 1) &&
                                        isEntityOnCell[MapTools.GetCellIdByCoord(x, y - 1)])
                                    {
                                        pointWeight += 0.3;
                                    }
                                }
                            }

                            var movementCost = costOfCell[parentId] +
                                               (y == parentY || x == parentX ? 10 : 15) * pointWeight;

                            if (allowThroughEntity)
                            {
                                var cellOnEndColumn   = x + y == endX + endY;
                                var cellOnStartColumn = x + y == start.Value.X + start.Value.Y;
                                var cellOnEndLine     = x - y == endX - endY;
                                var cellOnStartLine   = x - y == start.Value.X - start.Value.Y;

                                if (!cellOnEndColumn && !cellOnEndLine || !cellOnStartColumn && !cellOnStartLine)
                                {
                                    movementCost += MapTools.GetDistance(cellId, endCellId);
                                    movementCost += MapTools.GetDistance(cellId, startCellId);
                                }

                                if (x == endX || y == endY)
                                {
                                    movementCost -= 3;
                                }

                                if (cellOnEndColumn || cellOnEndLine || x + y == parentX + parentY ||
                                    x - y == parentX - parentY)
                                {
                                    movementCost -= 2;
                                }

                                if (x == start.Value.X || y == start.Value.Y)
                                {
                                    movementCost -= 3;
                                }

                                if (cellOnStartColumn || cellOnStartLine)
                                {
                                    movementCost -= 2;
                                }

                                var distanceTmpToEnd = MapTools.GetDistance(cellId, endCellId);
                                if (distanceTmpToEnd < distanceToEnd)
                                {
                                    endCellAuxId  = (short)cellId;
                                    distanceToEnd = distanceTmpToEnd;
                                }
                            }

                            if (parentOfCell[cellId] == MapTools.InvalidCellId || movementCost < costOfCell[cellId])
                            {
                                parentOfCell[cellId] = parentId;
                                costOfCell[cellId]   = movementCost;
                                var heuristic = 10 * Math.Sqrt((endY - y) * (endY - y) + (endX - x) * (endX - x));
                                openListWeights[cellId] = heuristic + movementCost;

                                if (!openList.Contains(cellId))
                                {
                                    openList.Add(cellId);
                                }
                            }
                        }

                        x++;
                    }

                    y++;
                }
            }

            var movPath = new ClientMovementPath
            {
                Start = map.GetCell(startCellId)!,
            };

            if (parentOfCell[endCellId] == MapTools.InvalidCellId)
            {
                endCellId = endCellAuxId;
            }

            movPath.End = map.GetCell(endCellId)!;

            var cursor = endCellId;

            while (cursor != startCellId)
            {
                if (allowDiag)
                {
                    var parent      = parentOfCell[cursor];
                    var grandParent = parent == MapTools.InvalidCellId ? MapTools.InvalidCellId : parentOfCell[parent];
                    var grandGrandParent = grandParent == MapTools.InvalidCellId
                        ? MapTools.InvalidCellId
                        : parentOfCell[grandParent];

                    var kX = MapPoint.GetPoint(cursor)!.X;
                    var kY = MapPoint.GetPoint(cursor)!.Y;

                    if (grandParent != MapTools.InvalidCellId && MapTools.GetDistance(cursor, grandParent) == 1)
                    {
                        if (map.PointMov(kX, kY, grandParent))
                        {
                            parentOfCell[cursor] = grandParent;
                        }
                    }
                    else
                    {
                        if (grandGrandParent != MapTools.InvalidCellId &&
                            MapTools.GetDistance(cursor, grandGrandParent) == 2)
                        {
                            var nextX  = MapPoint.GetPoint(grandGrandParent)!.X;
                            var nextY  = MapPoint.GetPoint(grandGrandParent)!.Y;
                            var interX = kX + MathHelper.Round((nextX - kX) / 2d);
                            var interY = kY + MathHelper.Round((nextY - kY) / 2d);

                            if (map.PointMov(interX, interY, cursor) &&
                                map.PointWeight(interX, interY) < 2)
                            {
                                parentOfCell[cursor] = MapTools.GetCellIdByCoord(interX, interY);
                            }
                        }
                        else
                        {
                            if (grandParent != MapTools.InvalidCellId && MapTools.GetDistance(cursor, grandParent) == 2)
                            {
                                var nextX  = MapTools.GetCellIdXCoord(grandParent);
                                var nextY  = MapTools.GetCellIdYCoord(grandParent);
                                var interX = MapTools.GetCellIdXCoord(parent);
                                var interY = MapTools.GetCellIdYCoord(parent);

                                if (kX + kY == nextX + nextY && kX - kY != interX - interY &&
                                    !map.IsChangeZone(MapTools.GetCellIdByCoord(kX, kY),
                                        MapTools.GetCellIdByCoord(interX, interY)) &&
                                    !map.IsChangeZone(MapTools.GetCellIdByCoord(interX, interY),
                                        MapTools.GetCellIdByCoord(nextX, nextY)))
                                {
                                    parentOfCell[cursor] = grandParent;
                                }
                                else if (kX - kY == nextX - nextY && kX - kY != interX - interY &&
                                         !map.IsChangeZone(MapTools.GetCellIdByCoord(kX, kY),
                                             MapTools.GetCellIdByCoord(interX, interY)) &&
                                         !map.IsChangeZone(MapTools.GetCellIdByCoord(interX, interY),
                                             MapTools.GetCellIdByCoord(nextX, nextY)))
                                {
                                    parentOfCell[cursor] = grandParent;
                                }
                                else if (kX == nextX && kX != interX && map.PointWeight(kX, interY) < 2 &&
                                         map.PointMov(kX, interY, cursor))
                                {
                                    parentOfCell[cursor] = MapTools.GetCellIdByCoord(kX, interY);
                                }
                                else if (kY == nextY && kY != interY && map.PointWeight(interX, kY) < 2 &&
                                         map.PointMov(interX, kY, cursor))

                                {
                                    parentOfCell[cursor] = MapTools.GetCellIdByCoord(interX, kY);
                                }
                            }
                        }
                    }
                }

                movPath.AddPoint(new PathElement(MapPoint.GetPoint(parentOfCell[cursor])!, (uint)MapTools.GetLookDirection8Exact(parentOfCell[cursor], cursor)));
                cursor = (short)parentOfCell[(int)cursor];
            }

            movPath.Path.Reverse();
            return movPath;
        }
    }
}