
namespace DofusCoube.FileProtocol.Datacenter.Externalnotifications
{
	public sealed class ExternalNotification : IDofusObject
	{
		public static string Module => "ExternalNotifications";

		public int Id { get; set; }

		public int CategoryId { get; set; }

		public int IconId { get; set; }

		public int ColorId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public bool DefaultEnable { get; set; }

		public bool DefaultSound { get; set; }

		public bool DefaultMultiAccount { get; set; }

		public bool DefaultNotify { get; set; }

		public string Name { get; set; }

		[I18N]
		public string Message { get; set; } = string.Empty;

		public int MessageId { get; set; }

	}
}
