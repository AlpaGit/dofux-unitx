
namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class TaxCollectorName : IDofusObject
	{
		public static string Module => "TaxCollectorNames";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
