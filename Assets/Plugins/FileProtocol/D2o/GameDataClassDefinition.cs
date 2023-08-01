using System;
using System.Collections.Generic;
using System.Linq;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.D2o
{
    public sealed class GameDataClassDefinition
    {
        private readonly GameDataFileAccessor _fileAccessor;
        private readonly string _initialPackageName;
        private readonly string _namespaceClass;

        public GameDataClassDefinition(GameDataFileAccessor fileAccessor, string initialPackageName, string packageName,
            string className)
        {
            _fileAccessor       = fileAccessor;
            _initialPackageName = initialPackageName;
            _namespaceClass     = string.Concat(packageName, ".", className);

            Class     = _fileAccessor.AssemblyData.GetType(_namespaceClass);
            ClassName = className;
            Fields    = new List<GameDataField>();
        }

        public Type? Class { get; private set; }

        public string? FieldIndexName { get; private set; }

        public string ClassName { get; }

        public IList<GameDataField> Fields { get; }

        public object Read(string module, BigEndianReader stream)
        {
            var instance = Activator.CreateInstance(Class!);

            foreach (var field in Fields)
            {
                var value     = field.ReadData?.Invoke(module, stream, 0);
                var fieldName = field.Name; // Capitalize(field.Name);
            
                if(field.Name == "luaFormula")
                    fieldName = "LuaFormula_";
                
                var property = Class!.GetProperty(fieldName);

                if (property == null)
                {
                    fieldName = Capitalize(field.Name);
                    property  = Class!.GetProperty(fieldName);
                }

                try
                {
                    if (value is uint && property != null && property.PropertyType == typeof(int))
                        property?.SetValue(instance, Convert.ToInt32(value));
                    else
                        property?.SetValue(instance, value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (field.FieldType is not GameDataFieldTypes.I18N) continue;

                property = Class.GetProperty(fieldName.Replace("Id", string.Empty));

                if (string.Equals(fieldName, "TextKey", StringComparison.OrdinalIgnoreCase))
                    property = Class.GetProperty("TextKeyText");

                if (property is not null)
                    switch (value)
                    {
                        case int valueInt:
                            property.SetValue(instance, _fileAccessor.TextReader.GetText(valueInt));
                            break;
                        case uint valueUInt:
                            property.SetValue(instance, _fileAccessor.TextReader.GetText((int)valueUInt));
                            break;
                        case string valueStr:
                            property.SetValue(instance, _fileAccessor.TextReader.GetText(valueStr));
                            break;
                    }
            }


            if (Class!.GetInterface("IPostInit") is not null)
                Class.GetMethod("PostInit")?.Invoke(instance, Array.Empty<object>());

            return instance!;
        }

        public void AddField(string fieldName, BigEndianReader stream)
        {
            var field = new GameDataField(_fileAccessor, fieldName);
            field.ReadType(stream);

            Fields.Add(field);
        }

        public static string Capitalize(string s)
        {
            var ss = s.Substring(0, 1).ToUpper();
        
            return string.Concat(ss.ToUpper(), s.Substring(1, s.Length - 1));
        }

        public void CreateClass()
        {
            var fields = Fields.Select(field => new ClassField(field.Name, typeof(object), field.FieldType));

            Class ??= ClassBuilder.CompileResultType(_namespaceClass, fields);

            if (Class.Name is "InfoMessage")
                FieldIndexName = Class.GetProperty("messageId") is not null ? "messageId" : string.Empty;
            else
                FieldIndexName = Class.GetProperty("id") is not null ? "id" : string.Empty;
        }
    }
}