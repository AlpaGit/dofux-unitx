using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellVariant : IDofusObject
	{
		public static string Module => "SpellVariants";

		public int Id { get; set; }

		public int BreedId { get; set; }

		public List<uint> SpellIds { get; set; }

	}
}
