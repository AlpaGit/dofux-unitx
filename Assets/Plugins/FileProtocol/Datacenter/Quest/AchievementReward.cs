using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class AchievementReward : IDofusObject
	{
		public static string Module => "AchievementRewards";

		public int Id { get; set; }

		public int AchievementId { get; set; }

		public string Criteria { get; set; }

		public double KamasRatio { get; set; }

		public double ExperienceRatio { get; set; }

		public bool KamasScaleWithPlayerLevel { get; set; }

		public List<uint> ItemsReward { get; set; }

		public List<uint> ItemsQuantityReward { get; set; }

		public List<uint> EmotesReward { get; set; }

		public List<uint> SpellsReward { get; set; }

		public List<uint> TitlesReward { get; set; }

		public List<uint> OrnamentsReward { get; set; }

	}
}
