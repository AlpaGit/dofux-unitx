
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class MapReference : IDofusObject
	{
		public static string Module => "MapReferences";

		public int Id { get; set; }

		public int MapId { get; set; }

		public int CellId { get; set; }

	}
}
