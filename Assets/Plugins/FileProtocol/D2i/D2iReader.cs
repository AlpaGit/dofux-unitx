using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.D2i
{
    public sealed class D2IReader
    {
        private readonly BigEndianReader _reader;
        private const string UnknownTextId = "[UNKNOWN_TEXT_ID_{0}]";

        public D2IReader(string filePath)
        {
            FilePath      = filePath;
            IndexedTexts  = new Dictionary<int, D2IText<int>>();
            NamedTexts    = new Dictionary<string, D2IText<string>>();
            SortedIndexes = new Dictionary<int, int>();
        
            _reader = new BigEndianReader(File.ReadAllBytes(FilePath));
        }
    
        public D2IReader(byte[] data)
        {
            IndexedTexts  = new Dictionary<int, D2IText<int>>();
            NamedTexts    = new Dictionary<string, D2IText<string>>();
            SortedIndexes = new Dictionary<int, int>();
        
            _reader = new BigEndianReader(data);
        }
    
        public string FilePath { get; }
        public IDictionary<int, D2IText<int>> IndexedTexts { get; }

        public IDictionary<string, D2IText<string>> NamedTexts { get; }

        public IDictionary<int, int> SortedIndexes { get; }

        public void ReadAllTexts()
        {
            var indexPosition = _reader.ReadInt32();
            _reader.Seek(indexPosition);
            var indexLength = _reader.ReadInt32();

            for (var i = 0; i < indexLength; i += 9)
            {
                var key            = _reader.ReadInt32();
                var notDiacritical = _reader.ReadBoolean();
                var textPosition   = _reader.ReadInt32();
                var position       = _reader.Position;

                _reader.Seek(textPosition);
                var text = _reader.ReadUtf();

                _reader.Seek(position);

                if (notDiacritical)
                {
                    i += sizeof(int);

                    var criticalPosition = _reader.ReadInt32();
                    position = _reader.Position;
                    _reader.Seek(criticalPosition);


                    var notDiacriticalText = _reader.ReadUtf();

                    _reader.Seek(position);

                    IndexedTexts.Add(key, new(key, text, notDiacriticalText));
                }
                else
                {
                    IndexedTexts.Add(key, new(key, text));
                }
            }

            indexLength = _reader.ReadInt32();

            while (indexLength > 0)
            {
                var position = _reader.Position;
                var key      = _reader.ReadUtf();

                var textPosition = _reader.ReadInt32();

                indexLength -= _reader.Position - position;
                position    =  _reader.Position;

                _reader.Seek(textPosition);
                NamedTexts.Add(key, new(key, _reader.ReadUtf()));

                _reader.Seek(position);
            }

            indexLength = _reader.ReadInt32();

            var c = 0;
            while (indexLength > 0)
            {
                SortedIndexes.Add(_reader.ReadInt32(), c++);
                indexLength -= sizeof(int);
            }
        }

        public string GetText(int id) =>
            IndexedTexts.TryGetValue(id, out var entry)
                ? entry.Text
                : string.Format(UnknownTextId, id);

        public string GetText(string id) =>
            NamedTexts.TryGetValue(id, out var entry)
                ? entry.Text
                : string.Format(UnknownTextId, id);

        public void SetText(int id, string text)
        {
            if (!IndexedTexts.TryGetValue(id, out var entry))
            {
                IndexedTexts.Add(id, entry = new(id, text));
            }
            else
            {
                entry.Text = text;
            }

            entry.UseNotDiacriticalText = false;
        }

        public void SetText(string id, string text)
        {
            if (!NamedTexts.TryGetValue(id, out var entry))
            {
                NamedTexts.Add(id, new(id, text));
            }
            else
            {
                entry.Text = text;
            }
        }

        public bool RemoveText(int id) =>
            IndexedTexts.Remove(id);

        public bool RemoveText(string id) =>
            NamedTexts.Remove(id);

        public int GetOrAddText(string text)
        {
            var existingTest =
                IndexedTexts.Values.FirstOrDefault(x =>
                    string.Equals(x.Text, text, StringComparison.InvariantCulture));

            if (existingTest != null)
            {
                return existingTest.Id;
            }

            var lastId = IndexedTexts.Keys.Max() + 5000000;
            IndexedTexts[lastId] = new(lastId, text);
            return lastId;
        }

        public int GetOrderIndex(int key)
        {
            if (SortedIndexes.TryGetValue(key, out var value))
            {
                return value;
            }

            return -1;
        }
    }
}