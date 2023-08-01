
namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class QuestObjectiveType : IDofusObject
	{
		public static string Module => "QuestObjectiveTypes";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

	}
}
