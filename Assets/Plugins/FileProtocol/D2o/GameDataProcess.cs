using System;
using System.Collections.Generic;
using DofusCoube.FileProtocol.D2i;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.D2o
{
    public sealed class GameDataProcess
    {
        private readonly IDictionary<string, IDictionary<int, object>> _sortIndex;

        private readonly BigEndianReader _stream;
        private readonly D2IReader _textReader;

        public GameDataProcess(BigEndianReader stream, D2IReader textReader)
        {
            _stream     = stream;
            _textReader = textReader;
            _sortIndex  = new Dictionary<string, IDictionary<int, object>>();

            ParseStream();
        }

        public IList<string>? QueryableField { get; set; }

        public IDictionary<string, long>? SearchFieldIndex { get; set; }

        public IDictionary<string, int>? SearchFieldType { get; set; }

        public IDictionary<string, int>? SearchFieldCount { get; set; }

        private void ParseStream()
        {
            QueryableField   = new List<string>();
            SearchFieldIndex = new Dictionary<string, long>();
            SearchFieldType  = new Dictionary<string, int>();
            SearchFieldCount = new Dictionary<string, int>();

            var fieldListSize     = _stream.ReadInt32();
            var indexSearchOffset = _stream.Position + fieldListSize + 4;

            while (fieldListSize > 0)
            {
                var size      = _stream.BytesAvailable;
                var fieldName = _stream.ReadUtf();
                var index     = _stream.ReadInt32();

                QueryableField.Add(fieldName);
                SearchFieldIndex[fieldName] = index + indexSearchOffset;
                SearchFieldType[fieldName]  = _stream.ReadInt32();
                SearchFieldCount[fieldName] = _stream.ReadInt32();

                fieldListSize -= size - _stream.BytesAvailable;
            }

            foreach (var field in QueryableField)
            {
                try
                {
                    if (SearchFieldType[field] == -5)
                    {
                        BuildI18nSortIndex(field);
                    }
                    else
                    {
                        BuildSortIndex(field);
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }

        public List<object> Query(string fieldName)
        {
            var result = new List<object>();

            if (!SearchFieldIndex!.ContainsKey(fieldName))
            {
                return result;
            }

            var type      = SearchFieldType![fieldName];
            var itemCount = SearchFieldCount![fieldName];

            var readFct = GetReadFunction(type);

            _stream.Seek((int)SearchFieldIndex[fieldName]);

            if (readFct is null)
            {
                return result;
            }

            for (var i = 0; i < itemCount; i++)
            {
                readFct();

                var idsCount = _stream.ReadInt32() * 0.25;

                for (var j = 0; j < idsCount; j++)
                {
                    result.Add(_stream.ReadInt32());
                }
            }

            return result;
        }

        private void BuildI18nSortIndex(string fieldName)
        {
            if (_sortIndex.ContainsKey(fieldName) || !SearchFieldIndex!.ContainsKey(fieldName))
            {
                return;
            }

            var itemCount = SearchFieldCount![fieldName];
            _stream.Seek((int)SearchFieldIndex[fieldName]);

            var rf = new Dictionary<int, object>();

            _sortIndex[fieldName] = rf;

            for (var i = 0; i < itemCount; i++)
            {
                var key       = _stream.ReadInt32();
                var idsCount  = _stream.ReadInt32() * 0.25;
                var i18NOrder = _textReader.GetOrderIndex(key);

                for (var j = 0; j < idsCount; j++)
                {
                    rf[_stream.ReadInt32()] = i18NOrder;
                }
            }
        }

        private void BuildSortIndex(string fieldName)
        {
            if (_sortIndex.ContainsKey(fieldName) || !SearchFieldIndex!.ContainsKey(fieldName))
            {
                return;
            }

            var itemCount = SearchFieldCount![fieldName];
            _stream.Seek((int)SearchFieldIndex[fieldName]);

            var rf = new Dictionary<int, object>();

            _sortIndex[fieldName] = rf;

            var type    = SearchFieldType![fieldName];
            var readFct = GetReadFunction(type);

            if (readFct is null)
            {
                return;
            }

            for (var i = 0; i < itemCount; i++)
            {
                var value    = readFct();
                var idsCount = _stream.ReadInt32() * 0.25;

                for (var j = 0; j < idsCount; j++)
                {
                    rf[_stream.ReadInt32()] = value;
                }
            }
        }

        private ReadFunction? GetReadFunction(int type) =>
            (GameDataFieldTypes)type switch
            {
                GameDataFieldTypes.Int     => ReadInt,
                GameDataFieldTypes.Number  => ReadNumber,
                GameDataFieldTypes.Boolean => ReadBoolean,
                GameDataFieldTypes.String  => ReadUtf,
                GameDataFieldTypes.UInt    => ReadUInt,
                GameDataFieldTypes.I18N    => ReadInt,
                _                          => null,
            };

        private object ReadNumber() =>
            _stream.ReadDouble();

        private object ReadInt() =>
            _stream.ReadInt32();

        private object ReadBoolean() =>
            _stream.ReadBoolean();

        private object ReadUtf() =>
            _stream.ReadUtf();

        private object ReadUInt() =>
            _stream.ReadUInt32();


        private delegate object ReadFunction();

        private delegate object WriteFunction();
    }
}