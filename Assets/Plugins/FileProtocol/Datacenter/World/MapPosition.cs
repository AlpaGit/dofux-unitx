using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class MapPosition : IDofusObject
	{
		public static string Module => "MapPositions";

		public double Id { get; set; }

		public int PosX { get; set; }

		public int PosY { get; set; }

		public bool Outdoor { get; set; }

		public int Capabilities { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public List<List<int>> Playlists { get; set; }

		public int SubAreaId { get; set; }

		public int WorldMap { get; set; }

		public bool HasPriorityOnWorldmap { get; set; }

		public bool AllowPrism { get; set; }

		public bool IsTransition { get; set; }

		public bool MapHasTemplate { get; set; }

		public int TacticalModeTemplateId { get; set; }

		public bool HasPublicPaddock { get; set; }

	}
}
