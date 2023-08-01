
namespace DofusCoube.FileProtocol.Datacenter.Notifications
{
	public sealed class Notification : IDofusObject
	{
		public static string Module => "Notifications";

		public int Id { get; set; }

		[I18N]
		public string Title { get; set; } = string.Empty;

		public int TitleId { get; set; }

		[I18N]
		public string Message { get; set; } = string.Empty;

		public int MessageId { get; set; }

		public int IconId { get; set; }

		public int TypeId { get; set; }

		public string Trigger { get; set; }

	}
}
