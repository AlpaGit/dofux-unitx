using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class SmileyPack : IDofusObject
	{
		public static string Module => "SmileyPacks";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public List<uint> Smileys { get; set; }

	}
}
