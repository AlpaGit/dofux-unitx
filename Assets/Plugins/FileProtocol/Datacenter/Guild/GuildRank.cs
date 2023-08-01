
namespace DofusCoube.FileProtocol.Datacenter.Guild
{
	public sealed class GuildRank : IDofusObject
	{
		public static string Module => "GuildRanks";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public bool IsModifiable { get; set; }

		public int GfxId { get; set; }

	}
}
