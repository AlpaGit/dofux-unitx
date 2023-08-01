using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class AchievementCategory : IDofusObject
	{
		public static string Module => "AchievementCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int ParentId { get; set; }

		public string Icon { get; set; }

		public int Order { get; set; }

		public string Color { get; set; }

		public List<uint> AchievementIds { get; set; }

		public string VisibilityCriterion { get; set; }

	}
}
