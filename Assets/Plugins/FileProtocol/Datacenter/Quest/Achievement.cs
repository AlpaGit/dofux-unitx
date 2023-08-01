using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class Achievement : IDofusObject
	{
		public static string Module => "Achievements";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int CategoryId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int IconId { get; set; }

		public int Points { get; set; }

		public int Level { get; set; }

		public int Order { get; set; }

		public bool AccountLinked { get; set; }

		public List<int> ObjectiveIds { get; set; }

		public List<int> RewardIds { get; set; }

	}
}
