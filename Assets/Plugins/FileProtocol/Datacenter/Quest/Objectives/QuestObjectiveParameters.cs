
namespace DofusCoube.FileProtocol.Datacenter.Quest.Objectives
{
	public sealed class QuestObjectiveParameters : IDofusObject
	{
		public static string Module => "QuestObjectives";

		public uint NumParams { get; set; }

		public int Parameter0 { get; set; }

		public int Parameter1 { get; set; }

		public int Parameter2 { get; set; }

		public int Parameter3 { get; set; }

		public int Parameter4 { get; set; }

		public bool DungeonOnly { get; set; }

	}
}
