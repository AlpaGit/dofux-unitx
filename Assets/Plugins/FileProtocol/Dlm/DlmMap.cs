using System;
using System.Drawing;
using System.Linq;
using System.Text;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public class DlmMap
    {
        public const uint Width = 14;
        public const uint Height = 20;
        public const int CellCount = 560;

        public byte Version { get; set; }

        public int Id { get; set; }

        public bool Encrypted { get; set; }

        public byte EncryptionVersion { get; set; }

        public uint RelativeId { get; set; }

        public byte MapType { get; set; }

        public int SubAreaId { get; set; }

        public int BottomNeighbourId { get; set; }

        public int LeftNeighbourId { get; set; }

        public int RightNeighbourId { get; set; }

        public int ShadowBonusOnEntities { get; set; }

        public Color BackgroundColor { get; set; }

        public Color GridColor { get; set; }

        public ushort ZoomScale { get; set; }

        public short ZoomOffsetX { get; set; }

        public short ZoomOffsetY { get; set; }

        public bool UseLowPassFilter { get; set; }

        public bool UseReverb { get; set; }

        public int PresetId { get; set; }

        public int TacticalMode { get; set; }

        public DlmFixture[] BackgroudFixtures { get; set; } = Array.Empty<DlmFixture>();

        public int TopNeighbourId { get; set; }

        public DlmFixture[] ForegroundFixtures { get; set; } = Array.Empty<DlmFixture>();

        public DlmCellData[] Cells { get; set; } = Array.Empty<DlmCellData>();

        public int Unknown { get; set; }

        public int GroundCrc { get; set; }

        public DlmLayer[] Layers { get; set; } = Array.Empty<DlmLayer>();

        public bool UsingNewMovementSystem { get; set; }

        public static DlmMap ReadFromStream(BigEndianReader givenReader, DlmReader dlmReader)
        {
            DlmMap? map = null;
            try
            {
                var reader = givenReader;
                map = new()
                {
                    Version = reader.ReadUInt8(),
                    Id      = (int)reader.ReadUInt32(),
                };

                switch (map.Version)
                {
                    case > 11:
                        throw new($"Reader outdated for this map (old version:{11} new version:{map.Version})");
                    case >= 7:
                    {
                        map.Encrypted         = reader.ReadBoolean();
                        map.EncryptionVersion = reader.ReadUInt8();
                        var n = reader.ReadInt32();
                        if (map.Encrypted)
                        {
                            var s = dlmReader.DecryptionKey;
                        
                            if (s == null)
                            {
                                throw new InvalidOperationException(
                                    $"Cannot decrypt the map {map.Id} without decryption key");
                            }

                            var buffer = reader.ReadSpan(n).ToArray();
                            var bytes  = Encoding.Default.GetBytes(s);
                            if (s.Length > 0)
                            {
                                for (var index = 0; index < buffer.Length; ++index)
                                {
                                    buffer[index] = (byte)(buffer[index] ^ (uint)bytes[index % s.Length]);
                                }

                                reader = new(buffer);
                            }
                        }

                        break;
                    }
                }

                map.RelativeId = reader.ReadUInt32();
                map.MapType    = reader.ReadUInt8();
                if (map.MapType is < 0 or > 1)
                {
                    throw new("Invalid decryption key");
                }

                map.SubAreaId             = reader.ReadInt32();
                map.TopNeighbourId        = reader.ReadInt32();
                map.BottomNeighbourId     = reader.ReadInt32();
                map.LeftNeighbourId       = reader.ReadInt32();
                map.RightNeighbourId      = reader.ReadInt32();
                map.ShadowBonusOnEntities = (int)reader.ReadUInt32();

                switch (map.Version)
                {
                    case >= 9:
                        var bgRed   = reader.ReadUInt8();
                        var bgBlue  = reader.ReadUInt8();
                        var bgGreen = reader.ReadUInt8();
                        var bgAlpha = reader.ReadUInt8();
                    
                        var gridRed   = reader.ReadUInt8();
                        var gridBlue  = reader.ReadUInt8();
                        var gridGreen = reader.ReadUInt8();
                        var gridAlpha = reader.ReadUInt8();
                    
                        map.BackgroundColor = Color.FromArgb(bgRed, bgBlue, bgGreen, bgAlpha);
                        map.GridColor       = Color.FromArgb(gridRed, gridBlue, gridGreen, gridAlpha);
                        break;
                    case >= 3:
                        var red   = reader.ReadUInt8();
                        var blue  = reader.ReadUInt8();
                        var green = reader.ReadUInt8();
                    
                        map.BackgroundColor = Color.FromArgb(red, blue, green);
                        break;
                }

                if (map.Version >= 4)
                {
                    map.ZoomScale   = reader.ReadUInt16();
                    map.ZoomOffsetX = reader.ReadInt16();
                    map.ZoomOffsetY = reader.ReadInt16();
                }

                if (map.Version > 10)
                {
                    map.TacticalMode = reader.ReadInt32();
                }

                // Deprecated without version
                //map.UseLowPassFilter = (int)reader.ReadByte() == 1;
                //map.UseReverb = (int)reader.ReadByte() == 1;
                //if (map.UseReverb)
                //    map.PresetId = reader.ReadInt();
                //map.PresetId = -1;

                map.BackgroudFixtures = new DlmFixture[reader.ReadUInt8()];
                for (var index = 0; index < map.BackgroudFixtures.Length; ++index)
                {
                    map.BackgroudFixtures[index] = DlmFixture.ReadFromStream(map, reader);
                }

                map.ForegroundFixtures = new DlmFixture[reader.ReadUInt8()];
                for (var index = 0; index < map.ForegroundFixtures.Length; ++index)
                {
                    map.ForegroundFixtures[index] = DlmFixture.ReadFromStream(map, reader);
                }

                map.Unknown   = reader.ReadInt32();
                map.GroundCrc = reader.ReadInt32();
                map.Layers    = new DlmLayer[reader.ReadUInt8()];
                for (var index = 0; index < map.Layers.Length; ++index)
                {
                    map.Layers[index] = DlmLayer.ReadFromStream(map, reader);
                }

                map.Cells = new DlmCellData[560];

                int? nullable1 = null;

                for (short id = 0; id < map.Cells.Length; ++id)
                {
                    map.Cells[id] = DlmCellData.ReadFromStream(id, map.Version, reader);

                    if (!nullable1.HasValue)
                    {
                        nullable1 = map.Cells[id].MoveZone;
                    }
                    else
                    {
                        int moveZone = map.Cells[id].MoveZone;
                        if (nullable1.GetValueOrDefault() != moveZone)
                        {
                            map.UsingNewMovementSystem = true;
                        }
                    }
                }

                return map;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DlmCellData.LastCell);
                Console.WriteLine(DlmCellData.LastCellData);
                throw new BadEncodedMapException(ex, map?.Id ?? -1);
            }
        }

        public static byte[] DecryptXor(byte[] inputBytes, byte[] decryptionKey)
        {
            if (inputBytes == null)
            {
                throw new ArgumentNullException(nameof(inputBytes));
            }

            if (decryptionKey == null)
            {
                throw new ArgumentNullException(nameof(decryptionKey));
            }

            var numArray = new byte[inputBytes.Length];
            inputBytes.CopyTo(numArray, 0);
            for (uint index = 0; index < numArray.Length; ++index)
            {
                numArray[(int)index] ^= decryptionKey[index % decryptionKey.Length];
            }

            return numArray;
        }

        public static byte[] EncryptXor(byte[] inputBytes, byte[] decryptionKey) =>
            DecryptXor(inputBytes, decryptionKey);
    }
}