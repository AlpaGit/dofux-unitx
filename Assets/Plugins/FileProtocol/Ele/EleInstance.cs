using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Ele
{
    public sealed class EleInstance : INotifyPropertyChanged
    {
        private readonly Dictionary<int, int> _indexes = new();

        private readonly bool _lazyLoad;

        public EleInstance()
        {
            _lazyLoad      = false;
            GraphicalDatas = new();
            GfxJpgMap      = new();
        }

        public byte Version { get; set; }

        public List<EleGraphicalData> GraphicalDatas { get; set; }

        public List<int> GfxJpgMap { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static EleInstance ReadFromStream(BigEndianReader reader)
        {
            var instance = new EleInstance();
            var skypLen  = 0;
            instance.Version = reader.ReadUInt8();
            var count = reader.ReadUInt32();
            var i     = 0;
            while (i < count)
            {
                var flag = instance.Version >= 9;
                if (flag)
                {
                    skypLen = reader.ReadUInt16();
                }

                var id    = reader.ReadInt32();
                var flag2 = instance.Version <= 8 || !instance._lazyLoad;
                if (flag2)
                {
                    instance._indexes.Add(id, reader.Position);
                    var elem = EleGraphicalData.ReadFromStream(id, instance, reader);
                    instance.GraphicalDatas.Add(elem);
                }
                else
                {
                    instance._indexes.Add(id, reader.Position);
                    reader.ReadSpan(skypLen - 4);
                }

                i++;
            }

            var flag3 = instance.Version >= 8;
            if (!flag3)
            {
                return instance;
            }

            var gfxCount = reader.ReadInt32();
            for (var j = 0; j < gfxCount; j++)
            {
                instance.GfxJpgMap.Add(reader.ReadInt32());
            }

            return instance;
        }

        public EleGraphicalData? GetGraphicalData(int elementId)
        {
            return GraphicalDatas.FirstOrDefault(x => x.Id == elementId);
        }

        public void SetGraphicalData(EleGraphicalData data)
        {
            var old = GraphicalDatas.FirstOrDefault(x => x.Id == data.Id);
            if (old != null)
            {
                GraphicalDatas.Remove(old);
            }

            GraphicalDatas.Add(data);
        }

        public void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new(propertyName));
        }
    }
}