using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Effects.Instances;

namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellLevel : IDofusObject
	{
		public static string Module => "SpellLevels";

		public int Id { get; set; }

		public int SpellId { get; set; }

		public int Grade { get; set; }

		public int SpellBreed { get; set; }

		public int ApCost { get; set; }

		public int MinRange { get; set; }

		public int Range { get; set; }

		public bool CastInLine { get; set; }

		public bool CastInDiagonal { get; set; }

		public bool CastTestLos { get; set; }

		public int CriticalHitProbability { get; set; }

		public bool NeedFreeCell { get; set; }

		public bool NeedTakenCell { get; set; }

		public bool NeedVisibleEntity { get; set; }

		public bool NeedCellWithoutPortal { get; set; }

		public bool PortalProjectionForbidden { get; set; }

		public bool NeedFreeTrapCell { get; set; }

		public bool RangeCanBeBoosted { get; set; }

		public int MaxStack { get; set; }

		public int MaxCastPerTurn { get; set; }

		public int MaxCastPerTarget { get; set; }

		public int MinCastInterval { get; set; }

		public int InitialCooldown { get; set; }

		public int GlobalCooldown { get; set; }

		public int MinPlayerLevel { get; set; }

		public bool HideEffects { get; set; }

		public bool Hidden { get; set; }

		public bool PlayAnimation { get; set; }

		public string StatesCriterion { get; set; }

		public List<EffectInstanceDice> Effects { get; set; }

		public List<EffectInstanceDice> CriticalEffect { get; set; }

		public List<EffectZone> PreviewZones { get; set; }

	}
}
