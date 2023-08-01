using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class QuestStep : IDofusObject
	{
		public static string Module => "QuestSteps";

		public int Id { get; set; }

		public int QuestId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int DialogId { get; set; }

		public int OptimalLevel { get; set; }

		public double Duration { get; set; }

		public List<uint> ObjectiveIds { get; set; }

		public List<uint> RewardsIds { get; set; }

	}
}
