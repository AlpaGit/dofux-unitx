
namespace DofusCoube.FileProtocol.Datacenter.Breeds
{
	public sealed class BreedRole : IDofusObject
	{
		public static string Module => "BreedRoles";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int AssetId { get; set; }

		public int Color { get; set; }

	}
}
