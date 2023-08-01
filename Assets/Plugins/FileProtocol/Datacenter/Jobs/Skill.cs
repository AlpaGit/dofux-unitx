using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Jobs
{
	public sealed class Skill : IDofusObject
	{
		public static string Module => "Skills";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int ParentJobId { get; set; }

		public bool IsForgemagus { get; set; }

		public List<int> ModifiableItemTypeIds { get; set; }

		public int GatheredRessourceItem { get; set; }

		public List<int> CraftableItemIds { get; set; }

		public int InteractiveId { get; set; }

		public int Range { get; set; }

		public bool UseRangeInClient { get; set; }

		public string UseAnimation { get; set; }

		public int Cursor { get; set; }

		public int ElementActionId { get; set; }

		public bool AvailableInHouse { get; set; }

		public bool ClientDisplay { get; set; }

		public int LevelMin { get; set; }

	}
}
