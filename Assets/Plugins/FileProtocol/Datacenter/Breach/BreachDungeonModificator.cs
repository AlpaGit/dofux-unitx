
namespace DofusCoube.FileProtocol.Datacenter.Breach
{
	public sealed class BreachDungeonModificator : IDofusObject
	{
		public static string Module => "BreachDungeonModificators";

		public int Id { get; set; }

		public int ModificatorId { get; set; }

		public string Criterion { get; set; }

		public double AdditionalRewardPercent { get; set; }

		public double Score { get; set; }

		public bool IsPositiveForPlayers { get; set; }

		public string TooltipBaseline { get; set; }

	}
}
