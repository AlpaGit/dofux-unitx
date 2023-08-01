using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public class NormalGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public NormalGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public int Gfx { get; set; }

        public uint Height { get; set; }

        public bool HorizontalSymmetry { get; set; }

        public Point Origin { get; set; }

        public Point Size { get; set; }

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.Normal;

        public new event PropertyChangedEventHandler? PropertyChanged;

        public static NormalGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader) =>
            new(instance, id)
            {
                Gfx                = reader.ReadInt32(),
                Height             = reader.ReadUInt8(),
                HorizontalSymmetry = reader.ReadBoolean(),
                Origin             = new(reader.ReadInt16(), reader.ReadInt16()),
                Size               = new(reader.ReadInt16(), reader.ReadInt16()),
            };
    
    }
}