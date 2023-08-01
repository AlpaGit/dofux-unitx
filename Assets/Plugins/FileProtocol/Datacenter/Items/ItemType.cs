
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class ItemType : IDofusObject
	{
		public static string Module => "ItemTypes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int SuperTypeId { get; set; }

		public int CategoryId { get; set; }

		public bool IsInEncyclopedia { get; set; }

		public bool Plural { get; set; }

		public int Gender { get; set; }

		public string RawZone { get; set; }

		public bool Mimickable { get; set; }

		public int CraftXpRatio { get; set; }

		public int EvolutiveTypeId { get; set; }

	}
}
