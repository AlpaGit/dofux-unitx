
namespace DofusCoube.FileProtocol.Datacenter.OptionalFeatures
{
	public sealed class CustomModeBreedSpell : IDofusObject
	{
		public static string Module => "CustomModeBreedSpells";

		public int Id { get; set; }

		public int PairId { get; set; }

		public int BreedId { get; set; }

		public bool IsInitialSpell { get; set; }

		public bool IsHidden { get; set; }

	}
}
