using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class AnimFunNpcData : IDofusObject
	{
		public static string Module => "Npcs";

		public int AnimId { get; set; }

		public int EntityId { get; set; }

		public string AnimName { get; set; }

		public int AnimWeight { get; set; }

		public List<AnimFunNpcData> SubAnimFunData { get; set; }

	}
}
