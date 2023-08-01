using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public struct DlmCellData
    {
        public static string LastCellData = "";
        public static DlmCellData LastCell;
        private short? _floor;
        private readonly sbyte _rawFloor;

        public int Version { get; set; }

        public short Floor => _floor ?? (_floor = (short)(_rawFloor * 10)).Value;

        public bool Mov { get; set; }

        public bool NonWalkableDuringFight { get; set; }

        public bool NonWalkableDuringRp { get; set; }

        public bool Los { get; set; }

        public bool Blue { get; set; }

        public bool Red { get; set; }

        public bool FarmCell { get; set; }

        public bool Visible { get; set; }

        public bool HavenbagCell { get; set; }

        public short Id { get; set; }

        public byte MapChangeData { get; set; }

        public byte MoveZone { get; set; }

        public byte Speed { get; set; }

        public short Data { get; set; }

        public bool UseTopArrow { get; set; }

        public bool UseBottomArrow { get; set; }

        public bool UseRightArrow { get; set; }

        public bool UseLeftArrow { get; set; }

        public byte LinkedZone { get; set; }

        public bool HasLinkedZoneRp => Mov && !FarmCell;

        public bool HasLinkedZoneFight => Mov && !NonWalkableDuringFight && !FarmCell && !HavenbagCell;

        public DlmCellData(short id)
        {
            Version                = 11;
            Id                     = id;
            Data                   = 3;
            _rawFloor              = 0;
            _floor                 = 0;
            Speed                  = 0;
            MapChangeData          = 0;
            MoveZone               = 0;
            LinkedZone             = 0;
            Mov                    = false;
            Los                    = false;
            Blue                   = false;
            Red                    = false;
            NonWalkableDuringFight = false;
            NonWalkableDuringRp    = false;
            Visible                = false;
            FarmCell               = false;
            HavenbagCell           = false;
            UseTopArrow            = false;
            UseBottomArrow         = false;
            UseLeftArrow           = false;
            UseRightArrow          = false;
        }

        public static DlmCellData ReadFromStream(short id, byte version, BigEndianReader reader)
        {
            var dlmCellDataNew = new DlmCellData(id)
            {
                Version = version,
                _floor  = reader.ReadInt8(),
            };

            if (dlmCellDataNew.Floor == -1280)
            {
                return dlmCellDataNew;
            }

            if (version >= 9)
            {
                var num = reader.ReadInt16();
                dlmCellDataNew.Data                   = num;
                dlmCellDataNew.Mov                    = (num & 1) == 0;
                dlmCellDataNew.NonWalkableDuringFight = ((uint)num & 2U) != 0U;
                dlmCellDataNew.NonWalkableDuringRp    = ((uint)num & 4U) != 0U;
                dlmCellDataNew.Los                    = (num & 8) == 0;
                dlmCellDataNew.Blue                   = ((uint)num & 16U) != 0U;
                dlmCellDataNew.Red                    = ((uint)num & 32U) != 0U;
                dlmCellDataNew.Visible                = ((uint)num & 64U) != 0U;
                dlmCellDataNew.FarmCell               = ((uint)num & 128U) != 0U;
                if (version >= 10)
                {
                    dlmCellDataNew.HavenbagCell   = ((uint)num & 256U) != 0U;
                    dlmCellDataNew.UseTopArrow    = ((uint)num & 512U) != 0U;
                    dlmCellDataNew.UseBottomArrow = ((uint)num & 1024U) != 0U;
                    dlmCellDataNew.UseRightArrow  = ((uint)num & 2048U) != 0U;
                    dlmCellDataNew.UseLeftArrow   = ((uint)num & 4096U) != 0U;
                }
            }

            dlmCellDataNew.Speed         = reader.ReadUInt8();
            dlmCellDataNew.MapChangeData = reader.ReadUInt8();
            dlmCellDataNew.MoveZone      = reader.ReadUInt8();
            if (version >= 10 && (dlmCellDataNew.HasLinkedZoneFight || dlmCellDataNew.HasLinkedZoneRp))
            {
                dlmCellDataNew.LinkedZone = reader.ReadUInt8();
            }

            LastCell     = dlmCellDataNew;
            LastCellData = $"Id: {dlmCellDataNew.Id} / Data {dlmCellDataNew.Data}";
            return dlmCellDataNew;
        }
    }
}