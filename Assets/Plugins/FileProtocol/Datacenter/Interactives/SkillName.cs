
namespace DofusCoube.FileProtocol.Datacenter.Interactives
{
	public sealed class SkillName : IDofusObject
	{
		public static string Module => "SkillNames";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
