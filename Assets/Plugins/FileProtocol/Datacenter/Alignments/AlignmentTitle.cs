using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Alignments
{
	public sealed class AlignmentTitle : IDofusObject
	{
		public static string Module => "AlignmentTitles";

		public int SideId { get; set; }

		public List<int> NamesId { get; set; }

		public List<int> ShortsId { get; set; }

	}
}
