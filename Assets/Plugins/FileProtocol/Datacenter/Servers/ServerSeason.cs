
namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class ServerSeason : IDofusObject
	{
		public static string Module => "ServerSeasons";

		public int Uid { get; set; }

		public int SeasonNumber { get; set; }

		public string Information { get; set; }

		public double Beginning { get; set; }

		public double Closure { get; set; }

		public double ResetDate { get; set; }

		public int FlagObjectId { get; set; }

	}
}
