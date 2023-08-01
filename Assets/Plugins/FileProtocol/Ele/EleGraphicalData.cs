using System.ComponentModel;
using DofusCoube.FileProtocol.Ele.Datas;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele
{
    public abstract class EleGraphicalData : INotifyPropertyChanged
    {
        public EleGraphicalData(EleInstance instance, int id)
        {
            Instance = instance;
            Id       = id;
        }

        public EleInstance Instance { get; set; }

        public int Id { get; set; }

        public abstract EleGraphicalElementTypes Type { get; }

        public event PropertyChangedEventHandler? PropertyChanged;


        public static EleGraphicalData ReadFromStream(int id, EleInstance instance, BigEndianReader reader)
        {
            var              type = (EleGraphicalElementTypes)reader.ReadUInt8();
            EleGraphicalData result;
            switch (type)
            {
                case EleGraphicalElementTypes.Normal:
                    result = NormalGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                case EleGraphicalElementTypes.BoundingBox:
                    result = BoundingBoxGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                case EleGraphicalElementTypes.Animated:
                    result = AnimatedGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                case EleGraphicalElementTypes.Entity:
                    result = EntityGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                case EleGraphicalElementTypes.Particles:
                    result = ParticlesGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                case EleGraphicalElementTypes.Blended:
                    result = BlendedGraphicalElementData.ReadFromStream(instance, id, reader);
                    break;
                default:
                    throw new("Unknown graphical data of type " + type);
            }

            return result;
        }

    
        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new(propertyName));
        }
    }
}