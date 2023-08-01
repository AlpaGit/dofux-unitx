
namespace DofusCoube.FileProtocol.Datacenter.Quest.TreasureHunt
{
	public sealed class LegendaryTreasureHunt : IDofusObject
	{
		public static string Module => "LegendaryTreasureHunts";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Level { get; set; }

		public uint ChestId { get; set; }

		public uint MonsterId { get; set; }

		public uint MapItemId { get; set; }

		public double XpRatio { get; set; }

	}
}
