using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.LivingObjects
{
	public class SpeakingItemsTrigger : IDofusObject
	{
		public static string Module => "SpeakingItemsTriggers";

		public int TriggersId { get; set; }

		public List<int> TextIds { get; set; }

		public List<int> States { get; set; }

	}
}
