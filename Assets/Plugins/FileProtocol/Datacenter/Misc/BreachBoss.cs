using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class BreachBoss : IDofusObject
	{
		public static string Module => "BreachBosses";

		public int Id { get; set; }

		public int MonsterId { get; set; }

		public int Category { get; set; }

		public string ApparitionCriterion { get; set; }

		public string AccessCriterion { get; set; }

		public List<int> IncompatibleBosses { get; set; }

		public uint RewardId { get; set; }

	}
}
