
namespace DofusCoube.FileProtocol.Datacenter.Arena
{
	public sealed class ArenaLeague : IDofusObject
	{
		public static string Module => "ArenaLeagues";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int OrnamentId { get; set; }

		public string Icon { get; set; }

		public string Illus { get; set; }

		public bool IsLastLeague { get; set; }

	}
}
