using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterDrop : IDofusObject
	{
		public static string Module => "Monsters";

		public int DropId { get; set; }

		public int MonsterId { get; set; }

		public int ObjectId { get; set; }

		public double PercentDropForGrade1 { get; set; }

		public double PercentDropForGrade2 { get; set; }

		public double PercentDropForGrade3 { get; set; }

		public double PercentDropForGrade4 { get; set; }

		public double PercentDropForGrade5 { get; set; }

		public int Count { get; set; }

		public string Criteria { get; set; }

		public bool HasCriteria { get; set; }

		public bool HiddenIfInvalidCriteria { get; set; }

		public List<MonsterDropCoefficient> SpecificDropCoefficient { get; set; }

	}
}
