
namespace DofusCoube.FileProtocol.Datacenter.OptionalFeatures
{
	public sealed class ForgettableSpell : IDofusObject
	{
		public static string Module => "ForgettableSpells";

		public int Id { get; set; }

		public int PairId { get; set; }

		public int ItemId { get; set; }

	}
}
