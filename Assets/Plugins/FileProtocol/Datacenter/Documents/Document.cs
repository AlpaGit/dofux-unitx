
namespace DofusCoube.FileProtocol.Datacenter.Documents
{
	public sealed class Document : IDofusObject
	{
		public static string Module => "Documents";

		public int Id { get; set; }

		public int TypeId { get; set; }

		public bool ShowTitle { get; set; }

		public bool ShowBackgroundImage { get; set; }

		[I18N]
		public string Title { get; set; } = string.Empty;

		public int TitleId { get; set; }

		[I18N]
		public string Author { get; set; } = string.Empty;

		public int AuthorId { get; set; }

		[I18N]
		public string SubTitle { get; set; } = string.Empty;

		public int SubTitleId { get; set; }

		[I18N]
		public string Content { get; set; } = string.Empty;

		public int ContentId { get; set; }

		public string ContentCSS { get; set; }

		public string ClientProperties { get; set; }

	}
}
