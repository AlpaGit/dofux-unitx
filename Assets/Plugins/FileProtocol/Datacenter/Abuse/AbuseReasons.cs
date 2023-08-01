
namespace DofusCoube.FileProtocol.Datacenter.Abuse
{
	public sealed class AbuseReasons : IDofusObject
	{
		public static string Module => "AbuseReasons";

		public int AbuseReasonId { get; set; }

		public int Mask { get; set; }

		[I18N]
		public string ReasonText { get; set; } = string.Empty;

		public int ReasonTextId { get; set; }

	}
}
