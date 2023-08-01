using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class Server : IDofusObject
	{
		public static string Module => "Servers";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Comment { get; set; } = string.Empty;

		public int CommentId { get; set; }

		public double OpeningDate { get; set; }

		public string Language { get; set; }

		public int PopulationId { get; set; }

		public int GameTypeId { get; set; }

		public int CommunityId { get; set; }

		public List<String> RestrictedToLanguages { get; set; }

		public bool MonoAccount { get; set; }

	}
}
