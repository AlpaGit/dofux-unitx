
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class AnimFunMonsterData : IDofusObject
	{
		public static string Module => "Monsters";

		public int AnimId { get; set; }

		public int EntityId { get; set; }

		public string AnimName { get; set; }

		public int AnimWeight { get; set; }

	}
}
