using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Effects;

namespace DofusCoube.FileProtocol.Datacenter.Alterations
{
	public sealed class Alteration : IDofusObject
	{
		public static string Module => "Alterations";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int CategoryId { get; set; }

		public int IconId { get; set; }

		public bool IsVisible { get; set; }

		public string Criteria { get; set; }

		public bool IsWebDisplay { get; set; }

		public List<EffectInstance> PossibleEffects { get; set; }

	}
}
