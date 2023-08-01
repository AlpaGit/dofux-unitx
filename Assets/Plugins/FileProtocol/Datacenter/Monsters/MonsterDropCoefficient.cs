
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterDropCoefficient : IDofusObject
	{
		public static string Module => "Monsters";

		public int MonsterId { get; set; }

		public int MonsterGrade { get; set; }

		public double DropCoefficient { get; set; }

		public string Criteria { get; set; }

	}
}
