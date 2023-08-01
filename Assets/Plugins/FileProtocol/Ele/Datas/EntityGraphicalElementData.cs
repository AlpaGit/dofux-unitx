using System.ComponentModel;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele.Datas
{
    public sealed class EntityGraphicalElementData : EleGraphicalData, INotifyPropertyChanged
    {
        public EntityGraphicalElementData(EleInstance instance, int id) : base(instance, id)
        {
        }

        public string EntityLook { get; set; } = string.Empty;

        public bool HorizontalSymmetry { get; set; }

        public bool PlayAnimation { get; set; }

        public bool PlayAnimStatic { get; set; }

        public uint MinDelay { get; set; }

        public uint MaxDelay { get; set; }

        public override EleGraphicalElementTypes Type => EleGraphicalElementTypes.Entity;

        public new event PropertyChangedEventHandler? PropertyChanged;

        public static EntityGraphicalElementData ReadFromStream(EleInstance instance, int id, BigEndianReader reader)
        {
            var data = new EntityGraphicalElementData(instance, id)
            {
                EntityLook         = reader.ReadBigString(),
                HorizontalSymmetry = reader.ReadBoolean(),
            };
            var flag = instance.Version >= 7;
            if (flag)
            {
                data.PlayAnimation = reader.ReadBoolean();
            }

            var flag2 = instance.Version >= 6;
            if (flag2)
            {
                data.PlayAnimStatic = reader.ReadBoolean();
            }

            var flag3 = instance.Version >= 5;
            if (!flag3)
            {
                return data;
            }

            data.MinDelay = reader.ReadUInt32();
            data.MaxDelay = reader.ReadUInt32();
            return data;
        }
    }
}