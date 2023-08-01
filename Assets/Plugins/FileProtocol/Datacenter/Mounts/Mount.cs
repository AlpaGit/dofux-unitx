using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Effects;

namespace DofusCoube.FileProtocol.Datacenter.Mounts
{
	public sealed class Mount : IDofusObject
	{
		public static string Module => "Mounts";

		public int Id { get; set; }

		public int FamilyId { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public string Look { get; set; }

		public int CertificateId { get; set; }

		public List<EffectInstance> Effects { get; set; }

	}
}
