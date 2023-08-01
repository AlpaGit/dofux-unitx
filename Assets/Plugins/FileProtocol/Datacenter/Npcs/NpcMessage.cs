using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class NpcMessage : IDofusObject
	{
		public static string Module => "NpcMessages";

		public int Id { get; set; }

		[I18N]
		public string Message { get; set; } = string.Empty;

		public int MessageId { get; set; }

		public List<String> MessageParams { get; set; }

	}
}
