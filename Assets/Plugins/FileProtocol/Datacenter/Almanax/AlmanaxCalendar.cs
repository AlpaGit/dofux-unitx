using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Almanax
{
	public sealed class AlmanaxCalendar : IDofusObject
	{
		public static string Module => "AlmanaxCalendars";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		[I18N]
		public string Desc { get; set; } = string.Empty;

		public int DescId { get; set; }

		public int NpcId { get; set; }

		public List<int> BonusesIds { get; set; }

	}
}
