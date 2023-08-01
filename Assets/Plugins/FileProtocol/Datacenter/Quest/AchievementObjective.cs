
namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class AchievementObjective : IDofusObject
	{
		public static string Module => "AchievementObjectives";

		public int Id { get; set; }

		public int AchievementId { get; set; }

		public int Order { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string Criterion { get; set; }

	}
}
