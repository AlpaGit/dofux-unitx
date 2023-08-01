
namespace DofusCoube.FileProtocol.Datacenter.Breach
{
	public sealed class BreachWorldMapCoordinate : IDofusObject
	{
		public static string Module => "BreachWorldMapCoordinates";

		public int Id { get; set; }

		public int MapStage { get; set; }

		public int MapCoordinateX { get; set; }

		public int MapCoordinateY { get; set; }

		public int UnexploredMapIcon { get; set; }

		public int ExploredMapIcon { get; set; }

	}
}
