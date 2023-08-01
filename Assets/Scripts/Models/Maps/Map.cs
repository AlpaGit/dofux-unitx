#nullable enable
using System;
using System.Collections.Generic;
using Components.Maps;
using DofusCoube.FileProtocol.Datacenter.World;
using Managers.Maps;
using Models.Actors;

namespace Models.Maps
{
    public class Map 
    {
        public MapBasicInformation BasicInformation { get; }
        private readonly MapPosition _mapPosition;

        public readonly List<Cell> Cells;
        public readonly List<ActorSprite> Actors = new();
        
        public long Id => BasicInformation.id;

        public Map(MapBasicInformation basicInformation, MapPosition mapPosition)
        {
            BasicInformation = basicInformation;
            _mapPosition     = mapPosition;

            Cells = new List<Cell>();

            foreach (var (cellId, cellData) in basicInformation.cells.dictionary)
            {
                Cells.Add(new Cell((short)cellId, (short)cellData));
            }
        }

        public bool IsValidCellId(int cellId)
        {
            return cellId is >= 0 and < 561;
        }

        public Cell? GetCell(short cellId)
        {
            return !IsValidCellId(cellId) ? null : Cells[cellId];
        }

        public bool PointMov(int x, int y, int previousCell = -1, bool isInFight = false)
        {
            if (!MapPoint.IsInMap(x, y))
            {
                return false;
            }

            var cell = GetCell(MapPoint.GetPoint(x, y)!.CellId);

            var mov = cell!.IsWalkable;

            if (isInFight && !cell.IsNonWalkableDuringFight)
            {
                mov = false;
            }

            if (mov && previousCell != -1 && previousCell != cell.Id)
            {
                var previousCellData = Cells[(short)previousCell];
                var dif              = Math.Abs(Math.Abs(cell.Floor) - Math.Abs(previousCellData.Floor));

                if (previousCellData.MoveZone != cell.MoveZone && dif > 0 ||
                    previousCellData.MoveZone == cell.MoveZone && cell.MoveZone == 0 && dif > 11)
                {
                    mov = false;
                }
            }

            return mov;
        }

        public double PointWeight(int x, int y, bool allowTroughEntity = true)
        {
            var cellId = MapTools.GetCellIdByCoord(x, y);

            if (!IsValidCellId(cellId))
            {
                return 0d;
            }

            var speed  = Cells[(short)cellId].Speed;
            var weight = 0d;

            if (allowTroughEntity)
            {
                if (speed >= 0)
                {
                    weight += 5 - speed;
                }
                else
                {
                    weight += 11 + Math.Abs(speed);
                }

                /*var entityOnCell = GetActorOnCell(cellId);

                if (entityOnCell != null)
                {
                    weight = 20;
                }*/
            }

            else
            {
                if (GetActorOnCell(cellId) != null)
                {
                    weight += 0.3;
                }

                if (GetActorOnCell(MapTools.GetCellIdByCoord(x + 1, y)) != null)
                {
                    weight += 0.3;
                }

                if (GetActorOnCell(MapTools.GetCellIdByCoord(x, y + 1)) != null)
                {
                    weight += 0.3;
                }

                if (GetActorOnCell(MapTools.GetCellIdByCoord(x - 1, y)) != null)
                {
                    weight += 0.3;
                }

                if (GetActorOnCell(MapTools.GetCellIdByCoord(x, y - 1)) != null)
                {
                    weight += 0.3;
                }
            }

            return weight;
        }

        private ActorSprite? GetActorOnCell(int cellId)
        {
            return Actors.Find(actor => actor.CellId == cellId);
        }

        public bool IsChangeZone(int cellId1, int cellId2)
        {
            var cellData1 = Cells[(short)cellId1];
            var cellData2 = Cells[(short)cellId2];

            var dif = Math.Abs(Math.Abs(cellData1.Floor) - Math.Abs(cellData2.Floor));
            return (cellData1.MoveZone != cellData2.MoveZone && dif == 0);
        }
    }
}