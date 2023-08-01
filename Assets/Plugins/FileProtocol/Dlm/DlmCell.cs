using System;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class DlmCell
    {
        public DlmCell(DlmLayer layer) =>
            Layer = layer;

        public DlmLayer Layer { get; set; }

        public short Id { get; set; }

        public DlmBasicElement[] Elements { get; set; } = Array.Empty<DlmBasicElement>();

        public static DlmCell ReadFromStream(DlmLayer layer, BigEndianReader reader)
        {
            var cell = new DlmCell(layer)
            {
                Id       = reader.ReadInt16(),
                Elements = new DlmBasicElement[reader.ReadInt16()],
            };

            for (var index = 0; index < cell.Elements.Length; ++index)
            {
                var dlmBasicElement = DlmBasicElement.ReadFromStream(cell, reader);
                cell.Elements[index] = dlmBasicElement;
            }

            return cell;
        }
    }
}