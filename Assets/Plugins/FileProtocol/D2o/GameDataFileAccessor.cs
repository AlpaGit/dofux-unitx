using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DofusCoube.FileProtocol.D2i;
using DofusCoube.FileProtocol.Datacenter.Breeds;
using DofusCoube.FileProtocol.Datacenter.Monsters;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.D2o
{
    public sealed class GameDataFileAccessor
    {
        private readonly IDictionary<string, IDictionary<int, GameDataClassDefinition?>> _classes;
        private readonly IDictionary<int, GameDataClassDefinition> _classesDefinitions;
        private readonly IDictionary<string, int> _counters;
        private readonly IDictionary<string, GameDataProcess> _gameDataProcesses;
        private readonly IDictionary<string, IDictionary<int, int>> _indexes;
        private readonly string _newNamespace;

        private readonly Dictionary<string, Dictionary<object, IList<int>>> _searchTable = new();

        private readonly IDictionary<string, BigEndianReader> _streams;

        private readonly IDictionary<string, int> _streamStartIndex;

        public GameDataFileAccessor(D2IReader textReader)
        {
            AssemblyData = typeof(Monster).Assembly;
            TextReader   = textReader;

            _streams            = new Dictionary<string, BigEndianReader>();
            _streamStartIndex   = new Dictionary<string, int>();
            _indexes            = new Dictionary<string, IDictionary<int, int>>();
            _classes            = new Dictionary<string, IDictionary<int, GameDataClassDefinition?>>();
            _classesDefinitions = new Dictionary<int, GameDataClassDefinition?>();
            _counters           = new Dictionary<string, int>();
            _gameDataProcesses  = new Dictionary<string, GameDataProcess>();
            _newNamespace       = typeof(Breed).FullName.Replace(".Breeds.Breed", "");
        }

        public D2IReader TextReader { get; }

        public Assembly AssemblyData { get; }

        public void Init(string path)
        {
            var moduleName = Path.GetFileNameWithoutExtension(path);

            if (!_streams.TryGetValue(moduleName, out var stream))
            {
                stream                        = new(File.ReadAllBytes(path));
                _streams[moduleName]          = stream;
                _streamStartIndex[moduleName] = 7;
            }
            else
            {
                stream.Seek(0);
            }


            InitFromIDataInput(stream, moduleName);
        }
    
        public void Init(string module, byte[] fileData)
        {
            var moduleName = module;

            if (!_streams.TryGetValue(moduleName, out var stream))
            {
                stream                        = new(fileData);
                _streams[moduleName]          = stream;
                _streamStartIndex[moduleName] = 7;
            }
            else
            {
                stream.Seek(0);
            }


            InitFromIDataInput(stream, moduleName);
        }

        private void InitFromIDataInput(BigEndianReader stream, string moduleName)
        {
            _streams[moduleName] = stream;

            if (!_streamStartIndex.ContainsKey(moduleName))
            {
                _streamStartIndex[moduleName] = 7;
            }

            var indexes = new Dictionary<int, int>();

            _indexes[moduleName] = indexes;

            var header        = Encoding.ASCII.GetString(stream.ReadSpan(3));
            var contentOffset = 0;

            if (header is not "D2O")
            {
                stream.Seek(0);

                try
                {
                    header = stream.ReadUtf();
                }
                catch
                {
                    //ignore
                }

                if (header is not "AKSF")
                {
                    throw new("Corrupted game data file.");
                }

                var _   = stream.ReadInt16();
                var len = stream.ReadInt32();

                stream.Seek(len, SeekOrigin.Current);

                contentOffset                 = stream.Position;
                _streamStartIndex[moduleName] = contentOffset + 7;

                header = Encoding.ASCII.GetString(stream.ReadSpan(3));

                if (header is not "D2O")
                {
                    throw new("Corrupted game data file.");
                }
            }

            var indexesPointer = stream.ReadInt32();
            stream.Seek(contentOffset + indexesPointer);
            var indexesLength = stream.ReadInt32();

            var count = 0;

            for (var i = 0; i < indexesLength; i += 8)
            {
                var key     = stream.ReadInt32();
                var pointer = stream.ReadInt32();

                indexes[key] = contentOffset + pointer;
                count++;
            }

            _counters[moduleName] = count;

            var classes = new Dictionary<int, GameDataClassDefinition>();

            _classes[moduleName] = classes!;

            var classesCount = stream.ReadInt32();

            for (var i = 0; i < classesCount; i++)
            {
                var classIdentifier = stream.ReadInt32();
                var classDef        = ReadClassDefinition(stream);

                _classes[moduleName][classIdentifier] = classDef;
                _classesDefinitions[classIdentifier]  = classDef;
            }

            if (stream.BytesAvailable > 0)
            {
                _gameDataProcesses[moduleName] = new(stream, TextReader);
            }
        }

        public string FixPackageName(string packageName)
        {
            packageName = packageName.Replace("com.ankamagames.dofus.datacenter", _newNamespace);
            packageName = packageName.Replace("com.ankamagames.tiphon", _newNamespace + "." + "Tiphon");
            packageName = packageName.Replace("flash.geom", _newNamespace + "." + "Geom");

            var parts  = packageName.Split('.');
            var result = string.Empty;
            foreach (var part in parts)
            {
                result = string.Concat(result, GameDataClassDefinition.Capitalize(part), '.');
            }
            return result.Substring(0, result.Length - 1);
        }

        private GameDataClassDefinition ReadClassDefinition(BigEndianReader stream)
        {
            var className          = stream.ReadUtf();
            var packageName        = stream.ReadUtf();
            var initialPackageName = packageName;

            packageName = FixPackageName(packageName);

            var classDef    = new GameDataClassDefinition(this, initialPackageName, packageName, className);
            var fieldsCount = stream.ReadInt32();

            for (var i = 0; i < fieldsCount; i++)
            {
                classDef.AddField(stream.ReadUtf(), stream);
            }

            classDef.CreateClass();

            return classDef;
        }


        public GameDataClassDefinition? GetMainClassByName(string moduleName)
        {
            var classes = _classes[moduleName];

            string className = moduleName.Substring(0, moduleName.Length - 1);

            var mainClass = classes.FirstOrDefault(x => x.Value?.ClassName == className);

            if (mainClass.Value is null)
            {
                mainClass = classes.FirstOrDefault();
            }

            return mainClass.Value;
        }

        public GameDataClassDefinition GetClassDefinition(string moduleName, int classId) =>
            _classes[moduleName][classId]!;

        public IDictionary<int, GameDataClassDefinition> GetClassesByName(string moduleName) =>
            _classes[moduleName]!;

        public IDictionary<int, int> GetIndexes(string moduleName) =>
            _indexes[moduleName];

        public BigEndianReader GetStream(string moduleName) =>
            _streams[moduleName];

        public int GetCounter(string moduleName) =>
            _counters[moduleName];

        public IEnumerable<T> GetObjects<T>(string moduleName, bool clearStream = false) where T : class
        {
            if (!_counters.ContainsKey(moduleName))
            {
                return null;
            }

            var len     = _counters[moduleName];
            var stream  = _streams[moduleName];
            var classes = _classes[moduleName];

            stream.Seek(_streamStartIndex[moduleName]);

            var objs = new object[len];

            for (var i = 0; i < len; i++)
            {
                objs[i] = classes[stream.ReadInt32()]!.Read(moduleName, stream);
            }

            if (clearStream)
            {
                ResetStream(moduleName);
            }

            return objs.Cast<T>();
        }

        public void ResetStream(string moduleName)
        {
            _classes.Remove(moduleName);
            _counters.Remove(moduleName);
            _streams.Remove(moduleName);
        }

        private void AddToSearchTable(object value, string field, int currentIndex)
        {
            if (!_searchTable.ContainsKey(field))
            {
                _searchTable[field] = new();
            }

            if (!_searchTable[field].ContainsKey(value))
            {
                _searchTable[field][value] = new List<int>();
            }

            _searchTable[field][value].Add(currentIndex);
        }

        private static string GetFullFieldName(string parentField, string fieldName) =>
            string.IsNullOrEmpty(parentField) ? fieldName : $"{parentField}.{fieldName}";

        public static int GetClassObjectKey<T>(string moduleName, Type objType, T obj)
        {
            var i = 0;
            switch (moduleName)
            {
                case "InfoMessages":
                {
                    var messageId = (int)objType.GetProperty("messageId")!.GetValue(obj)!;
                    var typeId    = (int)objType.GetProperty("typeId")!.GetValue(obj)!;

                    i = typeId > 0 ? messageId + typeId * 10000 : messageId;
                    break;
                }
                case "MapScrollActions":
                case "MapPositions":
                {
                    var id = objType.GetProperty("Id")!.GetValue(obj);

                    var indexId = (double)id!;
                    i = (int)indexId;
                    break;
                }
                default:
                {
                    var id = objType.GetProperty("Id")!.GetValue(obj);

                    var indexId = (int)id!;

                    i = indexId;
                    break;
                }
            }

            return i;
        }


        public Type FindNetType(string typeName)
        {
            switch (typeName)
            {
                case "int":
                    return typeof(int);
                case "uint":
                    return typeof(uint);
                case "Number":
                    return typeof(double);
                case "String":
                    return typeof(string);
                default:
                    if (typeName.StartsWith("Vector.<"))
                    {
                        return typeof(List<>).MakeGenericType(
                            FindNetType(
                                typeName.Remove(typeName.Length - 1, 1)
                                        .Remove(0, "Vector.<".Length)));
                    }

                    if (typeName.Contains("::"))
                    {
                        typeName = typeName.Split(new string[] { "::" }, StringSplitOptions.None)[1];
                    }

                    var @class = _classesDefinitions.Values.FirstOrDefault(x => x?.ClassName == typeName);

                    if (@class is null)
                    {
                        throw new($"Cannot found .NET type associated to {typeName}.");
                    }

                    return @class.Class!;
            }
        }
    }
}