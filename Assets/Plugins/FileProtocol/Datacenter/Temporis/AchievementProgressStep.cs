
namespace DofusCoube.FileProtocol.Datacenter.Temporis
{
	public sealed class AchievementProgressStep : IDofusObject
	{
		public static string Module => "AchievementProgressSteps";

		public int Id { get; set; }

		public int ProgressId { get; set; }

		public int Score { get; set; }

		public bool IsCosmetic { get; set; }

		public int AchievementId { get; set; }

		public bool IsBuyable { get; set; }

	}
}
