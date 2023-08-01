using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Arena
{
	public sealed class ArenaLeagueReward : IDofusObject
	{
		public static string Module => "ArenaLeagueRewards";

		public int Id { get; set; }

		public int SeasonId { get; set; }

		public int LeagueId { get; set; }

		public List<uint> TitlesRewards { get; set; }

		public bool EndSeasonRewards { get; set; }

	}
}
