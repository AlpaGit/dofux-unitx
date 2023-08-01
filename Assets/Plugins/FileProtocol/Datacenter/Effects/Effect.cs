
namespace DofusCoube.FileProtocol.Datacenter.Effects
{
	public sealed class Effect : IDofusObject
	{
		public static string Module => "Effects";

		public int Id { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int IconId { get; set; }

		public int Characteristic { get; set; }

		public int Category { get; set; }

		public string Operator { get; set; }

		public bool ShowInTooltip { get; set; }

		public bool UseDice { get; set; }

		public bool ForceMinMax { get; set; }

		public bool Boost { get; set; }

		public bool Active { get; set; }

		public int OppositeId { get; set; }

		[I18N]
		public string TheoreticalDescription { get; set; } = string.Empty;

		public int TheoreticalDescriptionId { get; set; }

		public int TheoreticalPattern { get; set; }

		public bool ShowInSet { get; set; }

		public int BonusType { get; set; }

		public bool UseInFight { get; set; }

		public int EffectPriority { get; set; }

		public double EffectPowerRate { get; set; }

		public int ElementId { get; set; }

	}
}
