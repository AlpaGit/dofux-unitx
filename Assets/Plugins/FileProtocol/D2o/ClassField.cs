using System;

namespace DofusCoube.FileProtocol.D2o
{
    public sealed class ClassField
    {
        public ClassField(string fieldName, Type type, GameDataFieldTypes fieldFieldType)
        {
            Name     = fieldName;
            Type     = type;
            DataType = fieldFieldType;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public GameDataFieldTypes DataType { get; set; }
    }
}
