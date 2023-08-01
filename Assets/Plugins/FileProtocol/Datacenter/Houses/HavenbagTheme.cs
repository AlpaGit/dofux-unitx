
namespace DofusCoube.FileProtocol.Datacenter.Houses
{
	public sealed class HavenbagTheme : IDofusObject
	{
		public static string Module => "HavenbagThemes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int MapId { get; set; }

	}
}
