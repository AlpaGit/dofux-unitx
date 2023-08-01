using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class MapCoordinates : IDofusObject
	{
		public static string Module => "MapCoordinates";

		public int CompressedCoords { get; set; }

		public List<double> MapIds { get; set; }

	}
}
