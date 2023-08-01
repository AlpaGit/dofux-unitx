using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class QuestCategory : IDofusObject
	{
		public static string Module => "QuestCategory";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public List<uint> QuestIds { get; set; }

	}
}
