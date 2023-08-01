
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class Ornament : IDofusObject
	{
		public static string Module => "Ornaments";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public bool Visible { get; set; }

		public int AssetId { get; set; }

		public int IconId { get; set; }

		public int Order { get; set; }

	}
}
