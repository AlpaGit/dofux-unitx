
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class PresetIcon : IDofusObject
	{
		public static string Module => "PresetIcons";

		public int Id { get; set; }

		public int Order { get; set; }

	}
}
