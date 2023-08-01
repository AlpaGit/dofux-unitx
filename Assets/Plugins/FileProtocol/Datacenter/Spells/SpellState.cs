using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class SpellState : IDofusObject
	{
		public static string Module => "SpellStates";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public bool PreventsSpellCast { get; set; }

		public bool PreventsFight { get; set; }

		public bool IsSilent { get; set; }

		public bool CantBeMoved { get; set; }

		public bool CantBePushed { get; set; }

		public bool CantDealDamage { get; set; }

		public bool Invulnerable { get; set; }

		public bool CantSwitchPosition { get; set; }

		public bool Incurable { get; set; }

		public List<int> EffectsIds { get; set; }

		public string Icon { get; set; }

		public int IconVisibilityMask { get; set; }

		public bool InvulnerableMelee { get; set; }

		public bool InvulnerableRange { get; set; }

		public bool CantTackle { get; set; }

		public bool CantBeTackled { get; set; }

		public bool DisplayTurnRemaining { get; set; }

	}
}
