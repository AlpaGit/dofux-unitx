
namespace DofusCoube.FileProtocol.Datacenter.Social
{
	public class SocialRight : IDofusObject
	{
		public static string Module => "GuildRightGroups";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public int GroupId { get; set; }

	}
}
