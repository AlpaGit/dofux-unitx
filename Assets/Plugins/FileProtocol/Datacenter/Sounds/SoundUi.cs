using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Sounds
{
	public sealed class SoundUi : IDofusObject
	{
		public static string Module => "SoundUi";

		public int Id { get; set; }

		public string UiName { get; set; }

		public string OpenFile { get; set; }

		public string CloseFile { get; set; }

		public List<List<SoundUi>> SubElements { get; set; }

	}
}
