
namespace DofusCoube.FileProtocol.Datacenter.Interactives
{
	public sealed class Interactive : IDofusObject
	{
		public static string Module => "Interactives";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int ActionId { get; set; }

		public bool DisplayTooltip { get; set; }

	}
}
