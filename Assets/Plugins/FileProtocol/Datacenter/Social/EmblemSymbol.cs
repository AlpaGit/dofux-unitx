
namespace DofusCoube.FileProtocol.Datacenter.Social
{
	public sealed class EmblemSymbol : IDofusObject
	{
		public static string Module => "EmblemSymbols";

		public int Id { get; set; }

		public int SkinId { get; set; }

		public int IconId { get; set; }

		public int Order { get; set; }

		public int CategoryId { get; set; }

		public bool Colorizable { get; set; }

	}
}
