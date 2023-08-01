
namespace DofusCoube.FileProtocol.Datacenter.Communication
{
	public sealed class NamingRule : IDofusObject
	{
		public static string Module => "NamingRules";

		public int Id { get; set; }

		public int MinLength { get; set; }

		public int MaxLength { get; set; }

		public string Regexp { get; set; }

	}
}
