
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class CharacterXPMapping : IDofusObject
	{
		public static string Module => "CharacterXPMappings";

		public int Level { get; set; }

		public double ExperiencePoints { get; set; }

	}
}
