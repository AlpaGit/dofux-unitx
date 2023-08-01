using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class Dungeon : IDofusObject
	{
		public static string Module => "Dungeons";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int OptimalPlayerLevel { get; set; }

		public List<double> MapIds { get; set; }

		public double EntranceMapId { get; set; }

		public double ExitMapId { get; set; }

	}
}
