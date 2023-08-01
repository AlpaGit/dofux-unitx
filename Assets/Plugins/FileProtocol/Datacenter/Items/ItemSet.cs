using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Effects;

namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class ItemSet : IDofusObject
	{
		public static string Module => "ItemSets";

		public int Id { get; set; }

		public List<uint> Items { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public bool BonusIsSecret { get; set; }

		public List<List<EffectInstance>> Effects { get; set; }

	}
}
