
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class Appearance : IDofusObject
	{
		public static string Module => "Appearances";

		public int Id { get; set; }

		public int Type { get; set; }

		public string Data { get; set; }

	}
}
