using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Breeds
{
	public sealed class Breed : IDofusObject
	{
		public static string Module => "Breeds";

		public int Id { get; set; }

		[I18N]
		public string ShortName { get; set; } = string.Empty;

		public int ShortNameId { get; set; }

		[I18N]
		public string LongName { get; set; } = string.Empty;

		public int LongNameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		[I18N]
		public string GameplayDescription { get; set; } = string.Empty;

		public int GameplayDescriptionId { get; set; }

		[I18N]
		public string GameplayClassDescription { get; set; } = string.Empty;

		public int GameplayClassDescriptionId { get; set; }

		public string MaleLook { get; set; }

		public string FemaleLook { get; set; }

		public int CreatureBonesId { get; set; }

		public int MaleArtwork { get; set; }

		public int FemaleArtwork { get; set; }

		public List<List<uint>> StatsPointsForStrength { get; set; }

		public List<List<uint>> StatsPointsForIntelligence { get; set; }

		public List<List<uint>> StatsPointsForChance { get; set; }

		public List<List<uint>> StatsPointsForAgility { get; set; }

		public List<List<uint>> StatsPointsForVitality { get; set; }

		public List<List<uint>> StatsPointsForWisdom { get; set; }

		public List<uint> BreedSpellsId { get; set; }

		public List<BreedRoleByBreed> BreedRoles { get; set; }

		public List<uint> MaleColors { get; set; }

		public List<uint> FemaleColors { get; set; }

		public int SpawnMap { get; set; }

		public int Complexity { get; set; }

		public int SortIndex { get; set; }

	}
}
