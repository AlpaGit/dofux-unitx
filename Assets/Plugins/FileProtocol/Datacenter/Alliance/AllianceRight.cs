
namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class AllianceRight : IDofusObject
	{
		public static string Module => "AllianceRights";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public int GroupId { get; set; }

	}
}
