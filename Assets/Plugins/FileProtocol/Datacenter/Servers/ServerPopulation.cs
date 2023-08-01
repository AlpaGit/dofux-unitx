
namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class ServerPopulation : IDofusObject
	{
		public static string Module => "ServerPopulations";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Weight { get; set; }

	}
}
