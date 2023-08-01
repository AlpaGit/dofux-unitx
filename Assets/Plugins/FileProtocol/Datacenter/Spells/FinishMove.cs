
namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class FinishMove : IDofusObject
	{
		public static string Module => "FinishMoves";

		public int Id { get; set; }

		public int Duration { get; set; }

		public bool Free { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Category { get; set; }

		public int SpellLevel { get; set; }

	}
}
