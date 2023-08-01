using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class LegendaryPowerCategory : IDofusObject
	{
		public static string Module => "LegendaryPowersCategories";

		public int Id { get; set; }

		public string CategoryName { get; set; }

		public bool CategoryOverridable { get; set; }

		public List<int> CategorySpells { get; set; }

	}
}
