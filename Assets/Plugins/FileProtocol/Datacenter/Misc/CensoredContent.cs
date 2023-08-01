
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class CensoredContent : IDofusObject
	{
		public static string Module => "CensoredContents";

		public string Lang { get; set; }

		public int Type { get; set; }

		public int OldValue { get; set; }

		public int NewValue { get; set; }

	}
}
