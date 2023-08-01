using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Alignments
{
	public sealed class AlignmentRank : IDofusObject
	{
		public static string Module => "AlignmentRank";

		public int Id { get; set; }

		public int OrderId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int MinimumAlignment { get; set; }

		public List<int> Gifts { get; set; }

	}
}
