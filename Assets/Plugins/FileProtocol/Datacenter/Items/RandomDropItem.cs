
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class RandomDropItem : IDofusObject
	{
		public static string Module => "RandomDropGroups";

		public int Id { get; set; }

		public int ItemId { get; set; }

		public double Probability { get; set; }

		public int MinQuantity { get; set; }

		public int MaxQuantity { get; set; }

	}
}
