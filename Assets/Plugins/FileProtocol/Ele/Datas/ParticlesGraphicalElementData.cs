using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public sealed class ParticlesGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public ParticlesGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.Animated;

        public int ScriptId { get; set; }

        public new event PropertyChangedEventHandler? PropertyChanged;

        public static ParticlesGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader) =>
            new(instance, id)
            {
                ScriptId = reader.ReadInt16(),
            };

    }
}