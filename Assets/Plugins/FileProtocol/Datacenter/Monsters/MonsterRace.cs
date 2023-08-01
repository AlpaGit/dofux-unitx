using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterRace : IDofusObject
	{
		public static string Module => "MonsterRaces";

		public int Id { get; set; }

		public int SuperRaceId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int AggressiveZoneSize { get; set; }

		public int AggressiveLevelDiff { get; set; }

		public string AggressiveImmunityCriterion { get; set; }

		public int AggressiveAttackDelay { get; set; }

		public List<uint> Monsters { get; set; }

	}
}
