using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class DlmSoundElement : DlmBasicElement
    {
        public DlmSoundElement(DlmCell cell)
            : base(cell)
        {
        }

        public int BaseVolume { get; set; }

        public int FullVolumedistance { get; set; }

        public int MaxDelayBetweenloops { get; set; }

        public int MinDelayBetweenloops { get; set; }

        public int NullVolumedistance { get; set; }

        public int SoundId { get; set; }

        public new static DlmSoundElement ReadFromStream(DlmCell cell, BigEndianReader reader) =>
            new(cell)
            {
                SoundId              = reader.ReadInt32(),
                BaseVolume           = reader.ReadInt16(),
                FullVolumedistance   = reader.ReadInt32(),
                NullVolumedistance   = reader.ReadInt32(),
                MinDelayBetweenloops = reader.ReadInt16(),
                MaxDelayBetweenloops = reader.ReadInt16(),
            };
    }
}