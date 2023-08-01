
namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class AllianceTagsType : IDofusObject
	{
		public static string Module => "AllianceTagsTypes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
