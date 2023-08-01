
namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class ServerGameType : IDofusObject
	{
		public static string Module => "ServerGameTypes";

		public int Id { get; set; }

		public bool SelectableByPlayer { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Rules { get; set; } = string.Empty;

		public int RulesId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

	}
}
