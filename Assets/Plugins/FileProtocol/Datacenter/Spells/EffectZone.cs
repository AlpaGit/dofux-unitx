
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class EffectZone : IDofusObject
	{
		public static string Module => "SpellLevels";

		public int Id { get; set; }

		public string RawDisplayZone { get; set; }

		public bool IsDefaultPreviewZoneHidden { get; set; }

		public string CasterMask { get; set; }

		public string RawActivationZone { get; set; }

		public string ActivationMask { get; set; }

	}
}
