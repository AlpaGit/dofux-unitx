
namespace DofusCoube.FileProtocol.Datacenter.Popup
{
	public sealed class PopupButton : IDofusObject
	{
		public static string Module => "PopupInformations";

		public int Id { get; set; }

		public int PopupId { get; set; }

		public int Type { get; set; }

		[I18N]
		public string Text { get; set; } = string.Empty;

		public int TextId { get; set; }

		public int ActionType { get; set; }

		public string ActionValue { get; set; }

	}
}
