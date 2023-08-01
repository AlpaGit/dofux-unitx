
namespace DofusCoube.FileProtocol.Datacenter.Characteristics
{
	public sealed class Characteristic : IDofusObject
	{
		public static string Module => "Characteristics";

		public int Id { get; set; }

		public string Keyword { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string Asset { get; set; }

		public int CategoryId { get; set; }

		public bool Visible { get; set; }

		public int Order { get; set; }

		public int ScaleFormulaId { get; set; }

		public bool Upgradable { get; set; }

	}
}
