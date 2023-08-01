
namespace DofusCoube.FileProtocol.Datacenter.Breach
{
	public sealed class BreachWorldMapSector : IDofusObject
	{
		public static string Module => "BreachWorldMapSectors";

		public int Id { get; set; }

		[I18N]
		public string SectorName { get; set; } = string.Empty;

		public int SectorNameId { get; set; }

		[I18N]
		public string Legend { get; set; } = string.Empty;

		public int LegendId { get; set; }

		public string SectorIcon { get; set; }

		public int MinStage { get; set; }

		public int MaxStage { get; set; }

	}
}
