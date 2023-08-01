
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class IncarnationLevel : IDofusObject
	{
		public static string Module => "IncarnationLevels";

		public int Id { get; set; }

		public int IncarnationId { get; set; }

		public int Level { get; set; }

		public int RequiredXp { get; set; }

	}
}
