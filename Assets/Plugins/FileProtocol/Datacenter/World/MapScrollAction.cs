
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class MapScrollAction : IDofusObject
	{
		public static string Module => "MapScrollActions";

		public double Id { get; set; }

		public bool RightExists { get; set; }

		public bool BottomExists { get; set; }

		public bool LeftExists { get; set; }

		public bool TopExists { get; set; }

		public double RightMapId { get; set; }

		public double BottomMapId { get; set; }

		public double LeftMapId { get; set; }

		public double TopMapId { get; set; }

	}
}
