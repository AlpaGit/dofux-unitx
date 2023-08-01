
namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class KothRole : IDofusObject
	{
		public static string Module => "KothRoles";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public bool IsDefault { get; set; }

	}
}
