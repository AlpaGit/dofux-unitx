
namespace DofusCoube.FileProtocol.Datacenter.Breach
{
	public sealed class BreachInfinityLevel : IDofusObject
	{
		public static string Module => "BreachInfinityLevels";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Level { get; set; }

	}
}
