
namespace DofusCoube.FileProtocol.Datacenter.Feature
{
	public sealed class OptionalFeature : IDofusObject
	{
		public static string Module => "OptionalFeatures";

		public int Id { get; set; }

		public string Keyword { get; set; }

		public bool IsClient { get; set; }

		public bool IsServer { get; set; }

		public bool IsActivationOnLaunch { get; set; }

		public bool IsActivationOnServerConnection { get; set; }

		public string ActivationCriterions { get; set; }

	}
}
