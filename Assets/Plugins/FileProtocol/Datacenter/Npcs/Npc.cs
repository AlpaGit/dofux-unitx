using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Npcs
{
	public sealed class Npc : IDofusObject
	{
		public static string Module => "Npcs";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public List<List<int>> DialogMessages { get; set; }

		public List<List<int>> DialogReplies { get; set; }

		public List<uint> Actions { get; set; }

		public int Gender { get; set; }

		public string Look { get; set; }

		public List<AnimFunNpcData> AnimFunList { get; set; }

		public bool FastAnimsFun { get; set; }

		public bool TooltipVisible { get; set; }

	}
}
