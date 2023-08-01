using System;

namespace Models.Maps
{
    public class Cell
    {
        public Cell(short id, short data)
        {
            if (id is < 0 or > 559)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Cell id must be between 0 and 559");
            }

            Id   = id;
            Data = data;
        }

        public short Id { get; set; }
        public short Data { get; set; }
        public byte MapChangeData { get; set; }
        public byte LinkedZone { get; set; }
        public byte MoveZone { get; set; }

        public bool IsWalkable => (Data & 1) == 0;
        public bool IsNonWalkableDuringFight => (Data & 2) != 0;
        public bool IsNonWalkableDuringRp => (Data & 4) != 0;
        public bool IsLineOfSight => (Data & 8) == 0;
        public bool IsBlue => (Data & 16) != 0;
        public bool IsRed => (Data & 32) != 0;
        public bool IsVisible => (Data & 64) != 0;
        public bool IsFarmCell => (Data & 128) != 0;
        public bool IsHavenbagCell => (Data & 256) != 0;
        public bool IsAllWalkable => IsWalkable && !IsNonWalkableDuringFight;

        public int LinkedZoneRp => (LinkedZone & 0xF0) >> 4;
        public short Speed { get; set; }
        public float Floor { get; set; }

        /*public bool IsRightMapChange() => ((MapChangeData & 0x01) == 0 || (Id + 1) % (Map.Width * 2) == 0) &&
                                          ((MapChangeData & 0x02) == 0 || (Id + 1) % (Map.Width * 2) == 0 && (MapChangeData & 0x80) == 0);
        public bool IsLeftMapChange() => (MapChangeData & 0x08) == 0 &&
                                          (MapChangeData & 0x20) == 0;*/

        public static bool IsLeftCol(int cellId)
        {
            return cellId % 14 == 0;
        }

        public static bool IsRightCol(int cellId)
        {
            return IsLeftCol((cellId + 1));
        }

        public static bool IsTopRow(int cellId)
        {
            return cellId < 28;
        }

        public static bool IsBottomRow(int cellId)
        {
            return cellId > 531;
        }

        public static bool IsEvenRow(int cellId)
        {
            return ((Math.Floor((cellId / 14d)) % 2) == 0);
        }
    }
}