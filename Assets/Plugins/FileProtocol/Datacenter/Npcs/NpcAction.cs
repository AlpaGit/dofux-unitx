
namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class NpcAction : IDofusObject
	{
		public static string Module => "NpcActions";

		public int Id { get; set; }

		public int RealId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
