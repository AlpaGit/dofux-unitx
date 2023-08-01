using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class CompanionCharacteristic : IDofusObject
	{
		public static string Module => "CompanionCharacteristics";

		public int Id { get; set; }

		public int CaracId { get; set; }

		public int CompanionId { get; set; }

		public int Order { get; set; }

		public List<List<double>> StatPerLevelRange { get; set; }

	}
}
