using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public abstract class DlmBasicElement
    {
        public enum ElementTypesEnum
        {
            Graphical = 2,
            Sound = 33,
        }

        protected DlmBasicElement(DlmCell cell) =>
            Cell = cell;

        public DlmCell Cell { get; }

        public ElementTypesEnum Type { get; set; }

        public static DlmBasicElement ReadFromStream(DlmCell cell, BigEndianReader reader)
        {
            var num = reader.ReadUInt8();
            return (ElementTypesEnum)num switch
                   {
                       ElementTypesEnum.Graphical => DlmGraphicalElement.ReadFromStream(cell, reader),
                       ElementTypesEnum.Sound     => DlmSoundElement.ReadFromStream(cell, reader),
                       _                          => throw new("Unknown element ID : " + num + " CellID : " + cell.Id),
                   };
        }

    }
}