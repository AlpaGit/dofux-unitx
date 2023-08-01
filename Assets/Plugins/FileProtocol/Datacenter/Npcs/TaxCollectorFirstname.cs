
namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class TaxCollectorFirstname : IDofusObject
	{
		public static string Module => "TaxCollectorFirstnames";

		public int Id { get; set; }

		[I18N]
		public string Firstname { get; set; } = string.Empty;

		public int FirstnameId { get; set; }

	}
}
