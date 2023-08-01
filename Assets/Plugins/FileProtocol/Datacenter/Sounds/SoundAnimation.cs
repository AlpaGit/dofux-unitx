
namespace DofusCoube.FileProtocol.Datacenter.Sounds
{
	public sealed class SoundAnimation : IDofusObject
	{
		public static string Module => "SoundBones";

		public int Id { get; set; }

		public string Label { get; set; }

		public string Name { get; set; }

		public string Filename { get; set; }

		public int Volume { get; set; }

		public int Rolloff { get; set; }

		public int AutomationDuration { get; set; }

		public int AutomationVolume { get; set; }

		public int AutomationFadeIn { get; set; }

		public int AutomationFadeOut { get; set; }

		public bool NoCutSilence { get; set; }

		public int StartFrame { get; set; }

	}
}
