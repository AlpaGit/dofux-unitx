
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class Hint : IDofusObject
	{
		public static string Module => "Hints";

		public int Id { get; set; }

		public int CategoryId { get; set; }

		public int Gfx { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int MapId { get; set; }

		public int RealMapId { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public bool Outdoor { get; set; }

		public int SubareaId { get; set; }

		public int WorldMapId { get; set; }

		public uint Level { get; set; }

	}
}
