
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class Pack : IDofusObject
	{
		public static string Module => "Pack";

		public int Id { get; set; }

		public string Name { get; set; }

		public bool HasSubAreas { get; set; }

	}
}
