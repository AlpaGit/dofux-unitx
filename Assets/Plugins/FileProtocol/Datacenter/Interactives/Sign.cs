
namespace DofusCoube.FileProtocol.Datacenter.Interactives
{
	public sealed class Sign : IDofusObject
	{
		public static string Module => "Signs";

		public int Id { get; set; }

		public string Params { get; set; }

		public int SkillId { get; set; }

		[I18N]
		public string TextKeyText { get; set; } = string.Empty;

		public int TextKey { get; set; }

	}
}
