using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class EvolutiveItemType : IDofusObject
	{
		public static string Module => "EvolutiveItemTypes";

		public int Id { get; set; }

		public int MaxLevel { get; set; }

		public double ExperienceBoost { get; set; }

		public List<int> ExperienceByLevel { get; set; }

	}
}
