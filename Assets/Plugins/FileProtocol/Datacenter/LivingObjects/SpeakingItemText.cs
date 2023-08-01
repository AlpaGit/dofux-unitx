
namespace DofusCoube.FileProtocol.Datacenter.LivingObjects
{
	public class SpeakingItemText : IDofusObject
	{
		public static string Module => "SpeakingItemsText";

		public int TextId { get; set; }

		public double TextProba { get; set; }

		[I18N]
		public string TextString { get; set; } = string.Empty;

		public int TextStringId { get; set; }

		public int TextLevel { get; set; }

		public int TextSound { get; set; }

		public string TextRestriction { get; set; }

	}
}
