
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellType : IDofusObject
	{
		public static string Module => "SpellTypes";

		public int Id { get; set; }

		[I18N]
		public string LongName { get; set; } = string.Empty;

		public int LongNameId { get; set; }

		[I18N]
		public string ShortName { get; set; } = string.Empty;

		public int ShortNameId { get; set; }

	}
}
