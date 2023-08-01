
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterGrade : IDofusObject
	{
		public static string Module => "Monsters";

		public int Grade { get; set; }

		public int MonsterId { get; set; }

		public int Level { get; set; }

		public int LifePoints { get; set; }

		public int ActionPoints { get; set; }

		public int MovementPoints { get; set; }

		public int Vitality { get; set; }

		public int PaDodge { get; set; }

		public int PmDodge { get; set; }

		public int Wisdom { get; set; }

		public int EarthResistance { get; set; }

		public int AirResistance { get; set; }

		public int FireResistance { get; set; }

		public int WaterResistance { get; set; }

		public int NeutralResistance { get; set; }

		public int GradeXp { get; set; }

		public int DamageReflect { get; set; }

		public uint HiddenLevel { get; set; }

		public int Strength { get; set; }

		public int Intelligence { get; set; }

		public int Chance { get; set; }

		public int Agility { get; set; }

		public int BonusRange { get; set; }

		public int StartingSpellId { get; set; }

		public MonsterBonusCharacteristics BonusCharacteristics { get; set; }

	}
}
