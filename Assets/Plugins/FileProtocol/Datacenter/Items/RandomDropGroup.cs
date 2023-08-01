using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class RandomDropGroup : IDofusObject
	{
		public static string Module => "RandomDropGroups";

		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public List<RandomDropItem> RandomDropItems { get; set; }

		public bool DisplayContent { get; set; }

		public bool DisplayChances { get; set; }

	}
}
