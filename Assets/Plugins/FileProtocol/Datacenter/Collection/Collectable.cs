
namespace DofusCoube.FileProtocol.Datacenter.Collection
{
	public sealed class Collectable : IDofusObject
	{
		public static string Module => "Collections";

		public int EntityId { get; set; }

		public string Name { get; set; }

		public int TypeId { get; set; }

		public int GfxId { get; set; }

		public int Order { get; set; }

		public int Rarity { get; set; }

	}
}
