
namespace DofusCoube.FileProtocol.Datacenter.Jobs
{
	public sealed class Job : IDofusObject
	{
		public static string Module => "Jobs";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int IconId { get; set; }

		public bool HasLegendaryCraft { get; set; }

	}
}
