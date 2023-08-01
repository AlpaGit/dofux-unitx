using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.LivingObjects
{
	public sealed class LivingObjectSkinJntMood : IDofusObject
	{
		public static string Module => "LivingObjectSkinJntMood";

		public int SkinId { get; set; }

		public List<List<int>> Moods { get; set; }

	}
}
