using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public sealed class BlendedGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public BlendedGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public string BlendMode { get; set; } = string.Empty;

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.Blended;

        public new event PropertyChangedEventHandler? PropertyChanged;

        public new static BlendedGraphicalElementData
            ReadFromStream(EleInstance instance, int id, BigEndianReader reader) =>
            new(instance, id)
            {
                Gfx                = reader.ReadInt32(),
                Height             = reader.ReadUInt8(),
                HorizontalSymmetry = reader.ReadBoolean(),
                Origin             = new(reader.ReadInt16(), reader.ReadInt16()),
                Size               = new(reader.ReadInt16(), reader.ReadInt16()),
                BlendMode          = reader.ReadBigString(),
            };

    }
}