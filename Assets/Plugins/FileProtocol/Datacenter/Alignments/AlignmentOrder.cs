
namespace DofusCoube.FileProtocol.Datacenter.Alignments
{
	public sealed class AlignmentOrder : IDofusObject
	{
		public static string Module => "AlignmentOrder";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int SideId { get; set; }

	}
}
