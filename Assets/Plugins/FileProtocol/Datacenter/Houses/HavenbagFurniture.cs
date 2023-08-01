
namespace DofusCoube.FileProtocol.Datacenter.Houses
{
	public sealed class HavenbagFurniture : IDofusObject
	{
		public static string Module => "HavenbagFurnitures";

		public int TypeId { get; set; }

		public int ThemeId { get; set; }

		public int ElementId { get; set; }

		public int Color { get; set; }

		public int LayerId { get; set; }

		public bool BlocksMovement { get; set; }

		public bool IsStackable { get; set; }

		public uint CellsWidth { get; set; }

		public uint CellsHeight { get; set; }

	}
}
