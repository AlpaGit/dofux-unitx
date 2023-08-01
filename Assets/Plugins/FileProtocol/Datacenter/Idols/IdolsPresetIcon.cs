
namespace DofusCoube.FileProtocol.Datacenter.Idols
{
	public sealed class IdolsPresetIcon : IDofusObject
	{
		public static string Module => "IdolsPresetIcons";

		public int Id { get; set; }

		public int Order { get; set; }

	}
}
