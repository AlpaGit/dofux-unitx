
namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class MonsterBonusCharacteristics : IDofusObject
	{
		public static string Module => "Monsters";

		public int LifePoints { get; set; }

		public int Strength { get; set; }

		public int Wisdom { get; set; }

		public int Chance { get; set; }

		public int Agility { get; set; }

		public int Intelligence { get; set; }

		public int EarthResistance { get; set; }

		public int FireResistance { get; set; }

		public int WaterResistance { get; set; }

		public int AirResistance { get; set; }

		public int NeutralResistance { get; set; }

		public int TackleEvade { get; set; }

		public int TackleBlock { get; set; }

		public int BonusEarthDamage { get; set; }

		public int BonusFireDamage { get; set; }

		public int BonusWaterDamage { get; set; }

		public int BonusAirDamage { get; set; }

		public int APRemoval { get; set; }

	}
}
