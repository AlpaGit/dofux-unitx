
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellPair : IDofusObject
	{
		public static string Module => "SpellPairs";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int IconId { get; set; }

	}
}
