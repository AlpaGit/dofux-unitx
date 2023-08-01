using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.OptionalFeatures
{
	public sealed class Modster : IDofusObject
	{
		public static string Module => "Modsters";

		public int Id { get; set; }

		public int ItemId { get; set; }

		public int ModsterId { get; set; }

		public List<int> ParentsModsterId { get; set; }

		public List<int> ModsterActiveSpells { get; set; }

		public List<int> ModsterPassiveSpells { get; set; }

		public List<int> ModsterHiddenAchievements { get; set; }

		public List<int> ModsterAchievements { get; set; }

		public int Order { get; set; }

	}
}
