
namespace DofusCoube.FileProtocol.Datacenter.Mounts
{
	public sealed class RideFood : IDofusObject
	{
		public static string Module => "RideFood";

		public int Gid { get; set; }

		public int TypeId { get; set; }

		public int FamilyId { get; set; }

	}
}
