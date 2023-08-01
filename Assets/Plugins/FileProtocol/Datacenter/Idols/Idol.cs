using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Idols
{
	public sealed class Idol : IDofusObject
	{
		public static string Module => "Idols";

		public int Id { get; set; }

		public string Description { get; set; }

		public int CategoryId { get; set; }

		public int ItemId { get; set; }

		public bool GroupOnly { get; set; }

		public int Score { get; set; }

		public int ExperienceBonus { get; set; }

		public int DropBonus { get; set; }

		public int SpellPairId { get; set; }

		public List<int> SynergyIdolsIds { get; set; }

		public List<double> SynergyIdolsCoeff { get; set; }

		public List<int> IncompatibleMonsters { get; set; }

	}
}
