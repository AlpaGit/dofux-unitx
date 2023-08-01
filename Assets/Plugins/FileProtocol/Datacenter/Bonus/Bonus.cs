using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Bonus
{
	public sealed class Bonus : IDofusObject
	{
		public static string Module => "Bonuses";

		public int Id { get; set; }

		public uint Type { get; set; }

		public int Amount { get; set; }

		public List<int> CriterionsIds { get; set; }

	}
}
