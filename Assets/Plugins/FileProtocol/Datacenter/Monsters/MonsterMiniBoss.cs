
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterMiniBoss : IDofusObject
	{
		public static string Module => "MonsterMiniBoss";

		public int Id { get; set; }

		public int MonsterReplacingId { get; set; }

	}
}
