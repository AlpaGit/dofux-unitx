
namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class InfoMessage : IDofusObject
	{
		public static string Module => "InfoMessages";

		public int TypeId { get; set; }

		public int MessageId { get; set; }

		[I18N]
		public string Text { get; set; } = string.Empty;

		public int TextId { get; set; }

	}
}
