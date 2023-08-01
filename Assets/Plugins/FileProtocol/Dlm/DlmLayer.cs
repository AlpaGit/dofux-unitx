using System;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class DlmLayer
    {
        public DlmLayer(DlmMap map) =>
            Map = map;

        public DlmLayer(DlmMap map, int layerId)
        {
            Map     = map;
            LayerId = layerId;
        }

        public DlmCell[] Cells { get; set; } = Array.Empty<DlmCell>();

        public DlmMap Map { get; set; }

        public int LayerId { get; set; }

        public static DlmLayer ReadFromStream(DlmMap map, BigEndianReader reader)
        {
            var layer = new DlmLayer(map)
            {
                LayerId = map.Version >= 9 ? reader.ReadUInt8() : reader.ReadInt32(),
                Cells   = new DlmCell[reader.ReadInt16()],
            };

            for (var index = 0; index < layer.Cells.Length; ++index)
            {
                layer.Cells[index] = DlmCell.ReadFromStream(layer, reader);
            }

            return layer;
        }
    }
}