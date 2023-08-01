
namespace DofusCoube.FileProtocol.Datacenter.Breeds
{
	public sealed class BreedRoleByBreed : IDofusObject
	{
		public static string Module => "Breeds";

		public int BreedId { get; set; }

		public int RoleId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int Value { get; set; }

		public int Order { get; set; }

	}
}
