
namespace DofusCoube.FileProtocol.Datacenter.Houses
{
	public sealed class House : IDofusObject
	{
		public static string Module => "Houses";

		public int TypeId { get; set; }

		public int DefaultPrice { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int GfxId { get; set; }

	}
}
