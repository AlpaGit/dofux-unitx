using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class Smiley : IDofusObject
	{
		public static string Module => "Smileys";

		public int Id { get; set; }

		public int Order { get; set; }

		public string GfxId { get; set; }

		public bool ForPlayers { get; set; }

		public List<String> Triggers { get; set; }

		public int ReferenceId { get; set; }

		public int CategoryId { get; set; }

	}
}
