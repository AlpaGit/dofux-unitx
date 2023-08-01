
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class CreatureBoneOverride : IDofusObject
	{
		public static string Module => "CreatureBonesOverrides";

		public int BoneId { get; set; }

		public int CreatureBoneId { get; set; }

	}
}
