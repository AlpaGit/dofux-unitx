
namespace DofusCoube.FileProtocol.Datacenter.Sounds
{
	public sealed class SoundUiElement : IDofusObject
	{
		public static string Module => "SoundUi";

		public int Id { get; set; }

		public string Name { get; set; }

		public int HookId { get; set; }

		public string File { get; set; }

	}
}
