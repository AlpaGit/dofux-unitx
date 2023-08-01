using DofusCoube.FileProtocol.Datacenter.Geom;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class Area : IDofusObject
	{
		public static string Module => "Areas";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int SuperAreaId { get; set; }

		public bool ContainHouses { get; set; }

		public bool ContainPaddocks { get; set; }

		public Rectangle Bounds { get; set; }

		public int WorldmapId { get; set; }

		public bool HasWorldMap { get; set; }

		public bool HasSuggestion { get; set; }

	}
}
