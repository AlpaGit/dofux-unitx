
namespace DofusCoube.FileProtocol.Datacenter.Mounts
{
	public sealed class MountFamily : IDofusObject
	{
		public static string Module => "MountFamily";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string HeadUri { get; set; }

	}
}
