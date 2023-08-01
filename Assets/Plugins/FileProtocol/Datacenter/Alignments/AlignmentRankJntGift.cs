using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Alignments
{
	public sealed class AlignmentRankJntGift : IDofusObject
	{
		public static string Module => "AlignmentRankJntGift";

		public int Id { get; set; }

		public List<int> Gifts { get; set; }

		public List<int> Levels { get; set; }

	}
}
