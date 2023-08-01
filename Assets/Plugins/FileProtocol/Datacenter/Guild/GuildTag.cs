
namespace DofusCoube.FileProtocol.Datacenter.Guild
{
	public sealed class GuildTag : IDofusObject
	{
		public static string Module => "GuildTags";

		public int Id { get; set; }

		public int TypeId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

	}
}
