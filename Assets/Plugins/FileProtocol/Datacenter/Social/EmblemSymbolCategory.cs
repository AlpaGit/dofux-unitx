
namespace DofusCoube.FileProtocol.Datacenter.Social
{
	public sealed class EmblemSymbolCategory : IDofusObject
	{
		public static string Module => "EmblemSymbolCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
