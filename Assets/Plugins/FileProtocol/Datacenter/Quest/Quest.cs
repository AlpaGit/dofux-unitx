using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class Quest : IDofusObject
	{
		public static string Module => "Quests";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int CategoryId { get; set; }

		public int RepeatType { get; set; }

		public int RepeatLimit { get; set; }

		public bool IsDungeonQuest { get; set; }

		public int LevelMin { get; set; }

		public int LevelMax { get; set; }

		public List<uint> StepIds { get; set; }

		public bool IsPartyQuest { get; set; }

		public string StartCriterion { get; set; }

		public bool Followable { get; set; }

	}
}
