using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Servers
{
	public sealed class ServerCommunity : IDofusObject
	{
		public static string Module => "ServerCommunities";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public List<String> DefaultCountries { get; set; }

		public string ShortId { get; set; }

		public List<int> SupportedLangIds { get; set; }

		public int NamingRulePlayerNameId { get; set; }

		public int NamingRuleGuildNameId { get; set; }

		public int NamingRuleAllianceNameId { get; set; }

		public int NamingRuleAllianceTagId { get; set; }

		public int NamingRulePartyNameId { get; set; }

		public int NamingRuleMountNameId { get; set; }

		public int NamingRuleNameGeneratorId { get; set; }

		public int NamingRuleAdminId { get; set; }

		public int NamingRuleModoId { get; set; }

		public int NamingRulePresetNameId { get; set; }

	}
}
