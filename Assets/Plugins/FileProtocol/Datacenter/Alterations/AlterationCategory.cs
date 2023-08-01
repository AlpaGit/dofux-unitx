
namespace DofusCoube.FileProtocol.Datacenter.Alterations
{
	public sealed class AlterationCategory : IDofusObject
	{
		public static string Module => "AlterationCategories";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int ParentId { get; set; }

	}
}
