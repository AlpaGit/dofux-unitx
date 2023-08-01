
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class CreatureBoneType : IDofusObject
	{
		public static string Module => "CreatureBonesTypes";

		public int Id { get; set; }

		public int CreatureBoneId { get; set; }

	}
}
