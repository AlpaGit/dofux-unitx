
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class VeteranReward : IDofusObject
	{
		public static string Module => "VeteranRewards";

		public int Id { get; set; }

		public uint RequiredSubDays { get; set; }

		public uint ItemGID { get; set; }

		public uint ItemQuantity { get; set; }

	}
}
