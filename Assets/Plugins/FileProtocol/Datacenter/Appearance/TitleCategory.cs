
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class TitleCategory : IDofusObject
	{
		public static string Module => "TitleCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
