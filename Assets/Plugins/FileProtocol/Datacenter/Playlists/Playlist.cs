using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.AmbientSounds;

namespace DofusCoube.FileProtocol.Datacenter.Playlists
{
	public sealed class Playlist : IDofusObject
	{
		public static string Module => "Playlists";

		public int Id { get; set; }

		public int Type { get; set; }

		public List<PlaylistSound> Sounds { get; set; }

		public bool StartRandom { get; set; }

		public bool StartRandomOnce { get; set; }

		public int CrossfadeDuration { get; set; }

		public bool Random { get; set; }

	}
}
