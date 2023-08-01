
namespace DofusCoube.FileProtocol.Datacenter.Arena
{
	public sealed class ArenaLeagueSeason : IDofusObject
	{
		public static string Module => "ArenaLeagueSeasons";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
