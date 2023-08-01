
namespace DofusCoube.FileProtocol.Datacenter.Mounts
{
	public sealed class MountBehavior : IDofusObject
	{
		public static string Module => "MountBehaviors";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

	}
}
