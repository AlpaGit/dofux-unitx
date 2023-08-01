using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Monsters
{
	public sealed class Monster : IDofusObject
	{
		public static string Module => "Monsters";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Race { get; set; }

		public List<MonsterGrade> Grades { get; set; }

		public string Look { get; set; }

		public bool UseSummonSlot { get; set; }

		public bool UseBombSlot { get; set; }

		public List<AnimFunMonsterData> AnimFunList { get; set; }

		public bool IsBoss { get; set; }

		public List<MonsterDrop> Drops { get; set; }

		public List<MonsterDrop> TemporisDrops { get; set; }

		public List<uint> Subareas { get; set; }

		public List<uint> Spells { get; set; }

		public List<String> SpellGrades { get; set; }

		public int FavoriteSubareaId { get; set; }

		public bool IsMiniBoss { get; set; }

		public bool IsQuestMonster { get; set; }

		public int CorrespondingMiniBossId { get; set; }

		public int SpeedAdjust { get; set; }

		public int CreatureBoneId { get; set; }

		public bool FastAnimsFun { get; set; }

		public bool CanPlay { get; set; }

		public bool CanTackle { get; set; }

		public bool CanBePushed { get; set; }

		public bool CanSwitchPos { get; set; }

		public bool CanSwitchPosOnTarget { get; set; }

		public bool CanBeCarried { get; set; }

		public bool CanUsePortal { get; set; }

		public List<uint> IncompatibleChallenges { get; set; }

		public bool UseRaceValues { get; set; }

		public int AggressiveZoneSize { get; set; }

		public int AggressiveLevelDiff { get; set; }

		public string AggressiveImmunityCriterion { get; set; }

		public int AggressiveAttackDelay { get; set; }

		public int ScaleGradeRef { get; set; }

		public List<List<double>> CharacRatios { get; set; }

	}
}
