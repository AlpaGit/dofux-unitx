using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Effects
{
	public sealed class EvolutiveEffect : IDofusObject
	{
		public static string Module => "EvolutiveEffects";

		public int Id { get; set; }

		public int ActionId { get; set; }

		public int TargetId { get; set; }

		public List<List<double>> ProgressionPerLevelRange { get; set; }

	}
}
