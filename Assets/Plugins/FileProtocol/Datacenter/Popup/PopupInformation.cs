using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Popup
{
	public sealed class PopupInformation : IDofusObject
	{
		public static string Module => "PopupInformations";

		public int Id { get; set; }

		public int ParentId { get; set; }

		[I18N]
		public string Title { get; set; } = string.Empty;

		public int TitleId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public string IlluName { get; set; }

		public List<PopupButton> Buttons { get; set; }

		public string Criterion { get; set; }

		public int CacheType { get; set; }

		public bool AutoTrigger { get; set; }

	}
}
