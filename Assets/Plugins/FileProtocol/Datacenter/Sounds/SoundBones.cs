using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Sounds
{
	public sealed class SoundBones : IDofusObject
	{
		public static string Module => "SoundBones";

		public int Id { get; set; }

		public List<String> Keys { get; set; }

		public List<List<SoundAnimation>> Values { get; set; }

	}
}
