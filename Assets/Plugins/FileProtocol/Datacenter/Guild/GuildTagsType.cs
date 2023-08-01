
namespace DofusCoube.FileProtocol.Datacenter.Guild
{
	public sealed class GuildTagsType : IDofusObject
	{
		public static string Module => "GuildTagsTypes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
