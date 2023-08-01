
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class CompanionSpell : IDofusObject
	{
		public static string Module => "CompanionSpells";

		public int Id { get; set; }

		public int SpellId { get; set; }

		public int CompanionId { get; set; }

		public string GradeByLevel { get; set; }

	}
}
