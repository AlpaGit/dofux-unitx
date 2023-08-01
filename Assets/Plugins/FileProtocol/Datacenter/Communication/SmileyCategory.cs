
namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class SmileyCategory : IDofusObject
	{
		public static string Module => "SmileyCategories";

		public int Id { get; set; }

		public int Order { get; set; }

		public string GfxId { get; set; }

		public bool IsFake { get; set; }

	}
}
