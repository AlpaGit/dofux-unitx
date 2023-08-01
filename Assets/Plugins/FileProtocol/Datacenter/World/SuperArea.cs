
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class SuperArea : IDofusObject
	{
		public static string Module => "SuperAreas";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int WorldmapId { get; set; }

		public bool HasWorldMap { get; set; }

	}
}
