
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class Tips : IDofusObject
	{
		public static string Module => "Tips";

		public int Id { get; set; }

		[I18N]
		public string Desc { get; set; } = string.Empty;

		public int DescId { get; set; }

	}
}
