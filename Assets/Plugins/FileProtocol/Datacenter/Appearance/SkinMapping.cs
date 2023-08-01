
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class SkinMapping : IDofusObject
	{
		public static string Module => "SkinMappings";

		public int Id { get; set; }

		public int LowDefId { get; set; }

	}
}
