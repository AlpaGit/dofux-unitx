
namespace DofusCoube.FileProtocol.Datacenter.Quest.TreasureHunt
{
	public sealed class PointOfInterestCategory : IDofusObject
	{
		public static string Module => "PointOfInterestCategory";

		public int Id { get; set; }

		[I18N]
		public string ActionLabel { get; set; } = string.Empty;

		public int ActionLabelId { get; set; }

	}
}
