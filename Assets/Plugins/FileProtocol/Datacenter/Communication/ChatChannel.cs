
namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class ChatChannel : IDofusObject
	{
		public static string Module => "ChatChannels";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public string Shortcut { get; set; }

		public string ShortcutKey { get; set; }

		public bool IsPrivate { get; set; }

		public bool AllowObjects { get; set; }

	}
}
