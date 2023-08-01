
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellConversion : IDofusObject
	{
		public static string Module => "SpellConversions";

		public int OldSpellId { get; set; }

		public int NewSpellId { get; set; }

	}
}
