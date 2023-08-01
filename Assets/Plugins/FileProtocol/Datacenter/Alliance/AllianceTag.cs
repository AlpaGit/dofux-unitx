
namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class AllianceTag : IDofusObject
	{
		public static string Module => "AllianceTags";

		public int Id { get; set; }

		public int TypeId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

	}
}
