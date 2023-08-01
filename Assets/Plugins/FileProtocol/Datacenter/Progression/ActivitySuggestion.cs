
namespace DofusCoube.FileProtocol.Datacenter.Progression
{
	public sealed class ActivitySuggestion : IDofusObject
	{
		public static string Module => "ActivitySuggestions";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int CategoryId { get; set; }

		public uint Level { get; set; }

		public int MapId { get; set; }

		public bool IsLarge { get; set; }

		public double StartDate { get; set; }

		public double EndDate { get; set; }

		public string Icon { get; set; }

	}
}
