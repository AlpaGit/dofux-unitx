using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class QuestStepRewards : IDofusObject
	{
		public static string Module => "QuestStepRewards";

		public int Id { get; set; }

		public int StepId { get; set; }

		public int LevelMin { get; set; }

		public int LevelMax { get; set; }

		public double KamasRatio { get; set; }

		public double ExperienceRatio { get; set; }

		public bool KamasScaleWithPlayerLevel { get; set; }

		public List<List<uint>> ItemsReward { get; set; }

		public List<uint> EmotesReward { get; set; }

		public List<uint> JobsReward { get; set; }

		public List<uint> SpellsReward { get; set; }

		public List<uint> TitlesReward { get; set; }

	}
}
