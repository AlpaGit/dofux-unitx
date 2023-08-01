using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Characteristics
{
	public sealed class CharacteristicCategory : IDofusObject
	{
		public static string Module => "CharacteristicCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public List<uint> CharacteristicIds { get; set; }

	}
}
