using DofusCoube.FileProtocol.Ele;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class DlmFixture
    {
        public DlmFixture(DlmMap map) =>
            Map = map;

        public DlmMap Map { get; set; }

        public int FixtureId { get; set; }

        public Point Offset { get; set; }

        public short Rotation { get; set; }

        public short ScaleX { get; set; }

        public short ScaleY { get; set; }

        public int Hue { get; set; }

        public sbyte RedMultiplier { get; set; }

        public sbyte GreenMultiplier { get; set; }

        public sbyte BlueMultiplier { get; set; }

        public byte Alpha { get; set; }

        public static DlmFixture ReadFromStream(DlmMap map, BigEndianReader reader)
        {
            var dlmFixture = new DlmFixture(map)
            {
                FixtureId       = reader.ReadInt32(),
                Offset          = new(reader.ReadInt16(), reader.ReadInt16()),
                Rotation        = reader.ReadInt16(),
                ScaleX          = reader.ReadInt16(),
                ScaleY          = reader.ReadInt16(),
                RedMultiplier   = reader.ReadInt8(),
                GreenMultiplier = reader.ReadInt8(),
                BlueMultiplier  = reader.ReadInt8(),
            };
            dlmFixture.Hue   = (dlmFixture.RedMultiplier | dlmFixture.GreenMultiplier) | dlmFixture.BlueMultiplier;
            dlmFixture.Alpha = reader.ReadUInt8();
            return dlmFixture;
        }
    }
}