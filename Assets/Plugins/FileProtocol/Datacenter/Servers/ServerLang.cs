
namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class ServerLang : IDofusObject
	{
		public static string Module => "ServerLangs";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string LangCode { get; set; }

	}
}
