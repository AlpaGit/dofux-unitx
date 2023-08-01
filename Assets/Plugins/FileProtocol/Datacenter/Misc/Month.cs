
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class Month : IDofusObject
	{
		public static string Module => "Months";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
