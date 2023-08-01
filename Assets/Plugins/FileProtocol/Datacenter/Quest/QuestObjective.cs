using DofusCoube.FileProtocol.Datacenter.Geom;
using DofusCoube.FileProtocol.Datacenter.Quest.Objectives;

namespace DofusCoube.FileProtocol.Datacenter.Quest
{
	public sealed class QuestObjective : IDofusObject
	{
		public static string Module => "QuestObjectives";

		public int Id { get; set; }

		public int StepId { get; set; }

		public int TypeId { get; set; }

		public int DialogId { get; set; }

		public QuestObjectiveParameters Parameters { get; set; }

		public Point Coords { get; set; }

		public int MapId { get; set; }

	}
}
