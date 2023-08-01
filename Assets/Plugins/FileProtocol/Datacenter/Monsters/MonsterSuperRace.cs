
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterSuperRace : IDofusObject
	{
		public static string Module => "MonsterSuperRaces";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
