
namespace DofusCoube.FileProtocol.Datacenter.Temporis
{
	public sealed class AchievementProgress : IDofusObject
	{
		public static string Module => "AchievementProgress";

		public int Id { get; set; }

		public string Name { get; set; }

		public int SeasonId { get; set; }

	}
}
