using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class Emoticon : IDofusObject
	{
		public static string Module => "Emoticons";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Shortcut { get; set; } = string.Empty;

		public int ShortcutId { get; set; }

		public uint Order { get; set; }

		public string DefaultAnim { get; set; }

		public bool Persistancy { get; set; }

		public bool Eight_directions { get; set; }

		public bool Aura { get; set; }

		public List<String> Anims { get; set; }

		public uint Cooldown { get; set; }

		public uint Duration { get; set; }

		public uint Weight { get; set; }

		public uint SpellLevelId { get; set; }

	}
}
