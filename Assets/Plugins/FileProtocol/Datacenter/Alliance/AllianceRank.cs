
namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class AllianceRank : IDofusObject
	{
		public static string Module => "AllianceRanks";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public bool IsModifiable { get; set; }

		public int GfxId { get; set; }

	}
}
