
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellBomb : IDofusObject
	{
		public static string Module => "SpellBombs";

		public int Id { get; set; }

		public int ChainReactionSpellId { get; set; }

		public int ExplodSpellId { get; set; }

		public int WallId { get; set; }

		public int InstantSpellId { get; set; }

		public int ComboCoeff { get; set; }

	}
}
