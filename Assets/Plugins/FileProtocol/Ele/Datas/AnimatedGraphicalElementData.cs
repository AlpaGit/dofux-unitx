using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public sealed class AnimatedGraphicalElementData : NormalGraphicalElementData, INotifyPropertyChanged
    {
        public AnimatedGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.Animated;

        public uint MinDelay { get; set; }

        public uint MaxDelay { get; set; }

        public new event PropertyChangedEventHandler? PropertyChanged;

        public new static AnimatedGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new AnimatedGraphicalElementData(instance, id)
            {
                Gfx                = reader.ReadInt32(),
                Height             = reader.ReadUInt8(),
                HorizontalSymmetry = reader.ReadBoolean(),
                Origin             = new Point(reader.ReadInt16(), reader.ReadInt16()),
                Size               = new Point(reader.ReadInt16(), reader.ReadInt16()),
            };

            var flag = instance.Version == 4;
            if (!flag)
            {
                return data;
            }

            data.MinDelay = reader.ReadUInt32();
            data.MaxDelay = reader.ReadUInt32();
            return data;
        }
    }
}