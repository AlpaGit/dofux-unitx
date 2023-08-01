
namespace DofusCoube.FileProtocol.Datacenter.Effects.Instances
{
	public class EffectInstanceDice : EffectInstanceInteger, IDofusObject
	{
		public new static string Module => "SpellLevels";

		public int DiceNum { get; set; }

		public int DiceSide { get; set; }

	}
}
