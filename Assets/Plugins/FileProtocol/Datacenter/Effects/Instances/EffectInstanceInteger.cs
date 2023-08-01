
namespace DofusCoube.FileProtocol.Datacenter.Effects.Instances
{
	public class EffectInstanceInteger : EffectInstance, IDofusObject
	{
		public new static string Module => "SpellLevels";

		public int Value { get; set; }

	}
}
