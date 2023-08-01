
namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class CensoredWord : IDofusObject
	{
		public static string Module => "CensoredWords";

		public int Id { get; set; }

		public int ListId { get; set; }

		public string Language { get; set; }

		public string Word { get; set; }

		public bool DeepLooking { get; set; }

	}
}
