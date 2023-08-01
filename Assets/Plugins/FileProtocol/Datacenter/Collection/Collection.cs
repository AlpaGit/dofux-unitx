using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Collection
{
	public sealed class Collection : IDofusObject
	{
		public static string Module => "Collections";

		public int TypeId { get; set; }

		public string Name { get; set; }

		public string Criterion { get; set; }

		public List<Collectable> Collectables { get; set; }

	}
}
