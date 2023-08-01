
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class Waypoint : IDofusObject
	{
		public static string Module => "Waypoints";

		public int Id { get; set; }

		public int MapId { get; set; }

		public int SubAreaId { get; set; }

		public bool Activated { get; set; }

	}
}
