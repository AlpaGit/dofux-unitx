
namespace DofusCoube.FileProtocol.Datacenter.Progression
{
	public sealed class ActivitySuggestionsCategory : IDofusObject
	{
		public static string Module => "ActivitySuggestionsCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int ParentId { get; set; }

	}
}
