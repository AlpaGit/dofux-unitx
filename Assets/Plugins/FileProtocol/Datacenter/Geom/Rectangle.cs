
namespace DofusCoube.FileProtocol.Datacenter.Geom
{
	public sealed class Rectangle : IDofusObject
	{
		public static string Module => "SubAreas";

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

	}
}
