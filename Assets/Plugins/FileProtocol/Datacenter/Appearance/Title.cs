
namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class Title : IDofusObject
	{
		public static string Module => "Titles";

		public int Id { get; set; }

		[I18N]
		public string NameMale { get; set; } = string.Empty;

		public int NameMaleId { get; set; }

		[I18N]
		public string NameFemale { get; set; } = string.Empty;

		public int NameFemaleId { get; set; }

		public bool Visible { get; set; }

		public int CategoryId { get; set; }

	}
}
