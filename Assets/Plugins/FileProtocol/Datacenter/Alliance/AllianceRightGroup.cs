using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Social;

namespace DofusCoube.FileProtocol.Datacenter.Alliance
{
	public sealed class AllianceRightGroup : IDofusObject
	{
		public static string Module => "AllianceRightGroups";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int Order { get; set; }

		public List<SocialRight> Rights { get; set; }

	}
}
