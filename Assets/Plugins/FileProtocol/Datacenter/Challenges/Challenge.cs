using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Challenges
{
	public sealed class Challenge : IDofusObject
	{
		public static string Module => "Challenges";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public List<uint> IncompatibleChallenges { get; set; }

		public int CategoryId { get; set; }

		public uint IconId { get; set; }

		public string CompletionCriterion { get; set; }

		public string ActivationCriterion { get; set; }

		public int TargetMonsterId { get; set; }

	}
}
