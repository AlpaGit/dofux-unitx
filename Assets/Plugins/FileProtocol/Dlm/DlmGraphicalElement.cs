using DofusCoube.FileProtocol.Ele;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public class DlmGraphicalElement : DlmBasicElement
    {
        public const float CellHalfWidth = 43f;
        public const float CellHalfHeight = 21.5f;
        private Point _offset;
        private Point _pixelOffset;

        public DlmGraphicalElement(DlmCell cell)
            : base(cell) =>
            Type = ElementTypesEnum.Graphical;

        public ElementTypesEnum ElementType => ElementTypesEnum.Graphical;

        public ColorMultiplicator ColorMultiplicator => FinalTeint;

        public int Altitude { get; set; }

        public uint ElementId { get; set; }

        public ColorMultiplicator FinalTeint { get; set; } = null!;

        public ColorMultiplicator Hue { get; set; } = null!;

        public uint Identifier { get; set; }

        public Point Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public Point PixelOffset
        {
            get => _pixelOffset;
            set => _pixelOffset = value;
        }

        public ColorMultiplicator Shadow { get; set; } = null!;

        public new static DlmGraphicalElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var graphicalElement = new DlmGraphicalElement(cell)
            {
                ElementId = reader.ReadUInt32(),
                Hue = new(
                    reader.ReadInt8(),
                    reader.ReadInt8(),
                    reader.ReadInt8()),
                Shadow = new(
                    reader.ReadInt8(),
                    reader.ReadInt8(),
                    reader.ReadInt8()),
            };
        
            if (cell.Layer.Map.Version <= 4)
            {
                graphicalElement._offset.X      = reader.ReadUInt8();
                graphicalElement._offset.Y      = reader.ReadUInt8();
                graphicalElement._pixelOffset.X = (int)(graphicalElement._offset.X * 43.0);
                graphicalElement._pixelOffset.Y = (int)(graphicalElement._offset.Y * 21.5);
            }
            else
            {
                graphicalElement._pixelOffset.X = reader.ReadInt16();
                graphicalElement._pixelOffset.Y = reader.ReadInt16();
                graphicalElement._offset.X      = (int)(graphicalElement._pixelOffset.X / 43.0);
                graphicalElement._offset.Y      = (int)(graphicalElement._pixelOffset.Y / 21.5);
            }

            graphicalElement.Altitude   = reader.ReadUInt8();
            graphicalElement.Identifier = reader.ReadUInt32();
            return graphicalElement;
        }


        public void CalculateFinalTeint()
        {
            var r = Hue.Red + Shadow.Red;
            var g = Hue.Green + Shadow.Green;
            var b = Hue.Blue + Shadow.Blue;

            r = ColorMultiplicator.Clamp((r + 128) * 2, 0, 512);
            g = ColorMultiplicator.Clamp((g + 128) * 2, 0, 512);
            b = ColorMultiplicator.Clamp((b + 128) * 2, 0, 512);

            FinalTeint = new ColorMultiplicator(r, g, b, true);
        }
    }
}