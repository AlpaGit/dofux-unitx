
namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class HintCategory : IDofusObject
	{
		public static string Module => "HintCategory";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
