
namespace DofusCoube.FileProtocol.Datacenter.Bonus.Criterion
{
	public class BonusEquippedItemCriterion : IDofusObject
	{
		public static string Module => "BonusesCriterions";

		public int Id { get; set; }

		public uint Type { get; set; }

		public int Value { get; set; }

	}
}
