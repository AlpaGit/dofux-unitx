using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Progression
{
	public sealed class FeatureDescription : IDofusObject
	{
		public static string Module => "FeatureDescriptions";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int Priority { get; set; }

		public int ParentId { get; set; }

		public List<int> Children { get; set; }

		public string Criterion { get; set; }

	}
}
