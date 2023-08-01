using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DofusCoube.FileProtocol.D2o
{
    public static class ClassBuilder
    {
        public static Type CompileResultType(string name, IEnumerable<ClassField> fields)
        {
            var typeBuilder = GetTypeBuilder(name);

            typeBuilder.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);

            foreach (var field in fields)
            {
                CreateProperty(typeBuilder, field.Name, field.Type);

                if (field.DataType == GameDataFieldTypes.I18N)
                {
                    CreateProperty(typeBuilder, field.Name.Replace("Id", string.Empty), typeof(string));
                }
            }

            return typeBuilder.CreateType();
        }

        private static TypeBuilder GetTypeBuilder(string name)
        {
            var assemblyName = new AssemblyName(name);

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder   = assemblyBuilder.DefineDynamicModule("MainModule");

            return moduleBuilder.DefineType(
                name,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                null);
        }

        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            var fieldBuilder =
                typeBuilder.DefineField(string.Concat('_', propertyName), propertyType, FieldAttributes.Private);

            var propertyBuilder =
                typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var gerPropertyMethodBuilder = typeBuilder.DefineMethod(
                string.Concat("get_", propertyName),
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);

            var getIl = gerPropertyMethodBuilder.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setPropertyMethodBuilder =
                typeBuilder.DefineMethod(
                    string.Concat("set_", propertyName),
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    null,
                    new[] { propertyType, });

            var setIl          = setPropertyMethodBuilder.GetILGenerator();
            var modifyProperty = setIl.DefineLabel();
            var exitSet        = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(gerPropertyMethodBuilder);
            propertyBuilder.SetSetMethod(setPropertyMethodBuilder);
        }
    }
}