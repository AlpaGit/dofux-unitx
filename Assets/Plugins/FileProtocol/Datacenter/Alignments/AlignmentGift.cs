
namespace DofusCoube.FileProtocol.Datacenter.Alignments
{
	public sealed class AlignmentGift : IDofusObject
	{
		public static string Module => "AlignmentGift";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
