using DofusCoube.FileProtocol.Datacenter.Geom;

namespace DofusCoube.FileProtocol.Datacenter.Quest.Objectives
{
	public sealed class QuestObjectiveFreeForm : IDofusObject
	{
		public static string Module => "QuestObjectives";

		public int StepId { get; set; }

		public int TypeId { get; set; }

		public int MapId { get; set; }

		public int Id { get; set; }

		public int DialogId { get; set; }

		public QuestObjectiveParameters Parameters { get; set; }

		public Point Coords { get; set; }

	}
}
