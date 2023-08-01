using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Bonus
{
	public sealed class QuestKamasBonus : IDofusObject
	{
		public static string Module => "Bonuses";

		public int Amount { get; set; }

		public int Id { get; set; }

		public List<int> CriterionsIds { get; set; }

		public uint Type { get; set; }

	}
}
