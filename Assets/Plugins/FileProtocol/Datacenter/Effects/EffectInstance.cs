
namespace DofusCoube.FileProtocol.Datacenter.Effects
{
	public class EffectInstance : IDofusObject
	{
		public static string Module => "SpellLevels";

		public int EffectUid { get; set; }

		public int BaseEffectId { get; set; }

		public int EffectId { get; set; }

		public int Order { get; set; }

		public int TargetId { get; set; }

		public string TargetMask { get; set; }

		public int Duration { get; set; }

		public double Random { get; set; }

		public int Group { get; set; }

		public bool VisibleInTooltip { get; set; }

		public bool VisibleInBuffUi { get; set; }

		public bool VisibleInFightLog { get; set; }

		public bool VisibleOnTerrain { get; set; }

		public bool ForClientOnly { get; set; }

		public int Dispellable { get; set; }

		public string RawZone { get; set; }

		public int Delay { get; set; }

		public string Triggers { get; set; }

		public int EffectElement { get; set; }

		public int SpellId { get; set; }

	}
}
