
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class BreachPrize : IDofusObject
	{
		public static string Module => "BreachPrizes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int CategoryId { get; set; }

		public int Currency { get; set; }

		public string TooltipKey { get; set; }

		[I18N]
		public string DescriptionKeyText { get; set; } = string.Empty;

		public int DescriptionKey { get; set; }

		public int ItemId { get; set; }

	}
}
