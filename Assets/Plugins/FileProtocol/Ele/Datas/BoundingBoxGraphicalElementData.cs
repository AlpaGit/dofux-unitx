using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public sealed class BoundingBoxGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public BoundingBoxGraphicalElementData(EleInstance instance, int id)
            : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.BoundingBox;

        public new event PropertyChangedEventHandler? PropertyChanged;

        public new static BoundingBoxGraphicalElementData ReadFromStream(EleInstance instance, int id,
            BigEndianReader reader)
        {
            var graphicalElementData = new BoundingBoxGraphicalElementData(instance, id)
            {
                Gfx                = reader.ReadInt32(),
                Height             = reader.ReadUInt8(),
                HorizontalSymmetry = reader.ReadBoolean(),
                Origin             = new(reader.ReadInt16(), reader.ReadInt16()),
                Size               = new(reader.ReadInt16(), reader.ReadInt16()),
            };
            return graphicalElementData;
        }
    }
}