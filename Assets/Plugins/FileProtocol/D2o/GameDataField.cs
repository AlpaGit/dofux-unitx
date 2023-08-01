using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.D2o
{
    public delegate object ReadTypeData(string moduleName, BigEndianReader stream, uint innerIndex);

    public sealed class GameDataField
    {
        private const int NullIdentifier = -1431655766;

        private readonly GameDataFileAccessor _fileAccessor;

        public GameDataField(GameDataFileAccessor fileAccessor, string fieldName)
        {
            _fileAccessor = fileAccessor;
            Name          = fieldName;
        }

        public string Name { get; }

        public ReadTypeData? ReadData { get; private set; }

        public GameDataFieldTypes FieldType { get; private set; }


        private IList<ReadTypeData>? InnerReadMethods { get; set; }

        public IList<string>? InnerTypeNames { get; private set; }

        public IList<string>? InitialInnerTypeNames { get; private set; }

        public IList<int>? InnerTypeIds { get; private set; }

    
        public string GetTypeAs =>
            InnerTypeNames!.FirstOrDefault()!;

        public bool IsQueryable() =>
            (FieldType == GameDataFieldTypes.Vector && InnerTypeIds!.Count > 0 && InnerTypeIds[0] > 0) || FieldType > 0;

        public void ReadType(BigEndianReader stream)
        {
            var type = stream.ReadInt32();

            ReadData  = GetReadMethod(type, stream);
            FieldType = (GameDataFieldTypes)type;
        }

        private ReadTypeData GetReadMethod(int type, BigEndianReader stream)
        {
            switch ((GameDataFieldTypes)type)
            {
                case GameDataFieldTypes.Int:
                    return ReadInteger;
                case GameDataFieldTypes.Boolean:
                    return ReadBoolean;
                case GameDataFieldTypes.String:
                    return ReadString;
                case GameDataFieldTypes.Number:
                    return ReadNumber;
                case GameDataFieldTypes.I18N:
                    return ReadI18N;
                case GameDataFieldTypes.UInt:
                    return ReadUnsignedInteger;
                case GameDataFieldTypes.Vector:
                    if (InnerReadMethods is null)
                    {
                        InnerReadMethods      = new List<ReadTypeData>();
                        InnerTypeNames        = new List<string>();
                        InnerTypeIds          = new List<int>();
                        InitialInnerTypeNames = new List<string>();
                    }

                    var nsClass = stream.ReadUtf();
                    InitialInnerTypeNames!.Add(nsClass);
                    var packageName = _fileAccessor.FixPackageName(nsClass);
                    InnerTypeNames!.Add(packageName);

                    var t = stream.ReadInt32();

                    InnerTypeIds!.Add(t);
                    InnerReadMethods.Insert(0, GetReadMethod(t, stream));

                    return ReadVector;
                default:
                    if (type > 0)
                    {
                        return ReadObject!;
                    }

                    throw new($"Unknown type {type}.");
            }
        }

        private object ReadVector(string moduleName, BigEndianReader stream, uint innerIndex = 0)
        {
            var len            = stream.ReadInt32();
            var vectorTypeName = InnerTypeNames![(int)innerIndex];

            var type = _fileAccessor.FindNetType(
                vectorTypeName.Remove(vectorTypeName.Length - 1, 1)
                              .Remove(0, "Vector.<".Length));

            var listType            = typeof(List<>);
            var constructedListType = listType.MakeGenericType(type);

            var content = Activator.CreateInstance(constructedListType) as IList;

            for (var i = 0; i < len; i++)
            {
                var readT = InnerReadMethods?[(int)innerIndex];

                content?.Add(readT?.Invoke(moduleName, stream, innerIndex + 1));
            }

            return content!;
        }

        private object? ReadObject(string moduleName, BigEndianReader stream, uint innerIndex = 0)
        {
            var classIdentifier = stream.ReadInt32();

            return classIdentifier is NullIdentifier
                ? null
                : _fileAccessor.GetClassDefinition(moduleName, classIdentifier).Read(moduleName, stream);
        }

        private static object ReadInteger(string moduleName, BigEndianReader stream, uint innerIndex = 0) =>
            stream.ReadInt32();

        private static object ReadBoolean(string moduleName, BigEndianReader stream, uint innerIndex = 0) =>
            stream.ReadBoolean();

        private static object ReadString(string moduleName, BigEndianReader stream, uint innerIndex = 0)
        {
            var result = stream.ReadUtf();

            if (result is "null")
            {
                result = "null";
            }

            return result;
        }

        private static object ReadNumber(string moduleName, BigEndianReader stream, uint innerIndex = 0) =>
            stream.ReadDouble();

        private static object ReadI18N(string moduleName, BigEndianReader stream, uint innerIndex = 0) =>
            stream.ReadInt32();

        private static object ReadUnsignedInteger(string moduleName, BigEndianReader stream, uint innerIndex = 0) =>
            stream.ReadUInt32();
    }
}