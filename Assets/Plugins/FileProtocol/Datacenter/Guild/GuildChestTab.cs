
namespace DofusCoube.FileProtocol.Datacenter.Guild
{
	public sealed class GuildChestTab : IDofusObject
	{
		public static string Module => "GuildChestTabs";

		public int TabId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Index { get; set; }

		public int GfxId { get; set; }

		public int ServerType { get; set; }

		public int Cost { get; set; }

		public int Seniority { get; set; }

		public int OpenRight { get; set; }

		public int DropRight { get; set; }

		public int TakeRight { get; set; }

	}
}
