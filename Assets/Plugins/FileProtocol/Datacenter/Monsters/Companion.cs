using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class Companion : IDofusObject
	{
		public static string Module => "Companions";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string Look { get; set; }

		public bool WebDisplay { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int StartingSpellLevelId { get; set; }

		public int AssetId { get; set; }

		public List<uint> Characteristics { get; set; }

		public List<uint> Spells { get; set; }

		public int CreatureBoneId { get; set; }

		public string Visibility { get; set; }

	}
}
