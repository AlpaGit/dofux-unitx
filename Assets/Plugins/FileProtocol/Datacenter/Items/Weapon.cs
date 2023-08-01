namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class Weapon : Item, IDofusObject
	{
		public new static string Module => "Items";

		public int CriticalFailureProbability { get; set; }

		public int CriticalHitBonus { get; set; }

		public int MinRange { get; set; }

		public int CriticalHitProbability { get; set; }

		public int Range { get; set; }

		public bool CastInLine { get; set; }

		public int ApCost { get; set; }

		public bool CastInDiagonal { get; set; }

		public bool CastTestLos { get; set; }

		public int MaxCastPerTurn { get; set; }

	}
}
