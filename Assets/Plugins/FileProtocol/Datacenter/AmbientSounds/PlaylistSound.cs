
namespace DofusCoube.FileProtocol.Datacenter.AmbientSounds
{
	public sealed class PlaylistSound : IDofusObject
	{
		public static string Module => "Playlists";

		public string Id { get; set; }

		public int Volume { get; set; }

		public int SoundOrder { get; set; }

	}
}
