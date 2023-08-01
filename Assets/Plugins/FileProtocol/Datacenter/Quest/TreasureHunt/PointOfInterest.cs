
namespace DofusCoube.FileProtocol.Datacenter.Quest.TreasureHunt
{
	public sealed class PointOfInterest : IDofusObject
	{
		public static string Module => "PointOfInterest";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int CategoryId { get; set; }

	}
}
