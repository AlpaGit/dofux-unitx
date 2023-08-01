
namespace DofusCoube.FileProtocol.Datacenter.Breeds
{
	public sealed class Head : IDofusObject
	{
		public static string Module => "Heads";

		public int Id { get; set; }

		public string Skins { get; set; }

		public string AssetId { get; set; }

		public int Breed { get; set; }

		public int Gender { get; set; }

		public string Label { get; set; }

		public int Order { get; set; }

	}
}
