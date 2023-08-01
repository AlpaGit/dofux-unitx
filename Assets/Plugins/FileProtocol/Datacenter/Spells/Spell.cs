using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Spells
{
	public sealed class Spell : IDofusObject
	{
		public static string Module => "Spells";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int TypeId { get; set; }

		public int Order { get; set; }

		public string ScriptParams { get; set; }

		public string ScriptParamsCritical { get; set; }

		public int ScriptId { get; set; }

		public int ScriptIdCritical { get; set; }

		public int IconId { get; set; }

		public List<uint> SpellLevels { get; set; }

		public bool VerboseCast { get; set; }

		public string DefaultPreviewZone { get; set; }

		public bool BypassSummoningLimit { get; set; }

		public bool CanAlwaysTriggerSpells { get; set; }

		public string AdminName { get; set; }

	}
}
