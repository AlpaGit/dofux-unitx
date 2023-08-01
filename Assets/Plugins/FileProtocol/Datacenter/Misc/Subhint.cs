
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class Subhint : IDofusObject
	{
		public static string Module => "Subhints";

		public int Hint_id { get; set; }

		public string Hint_parent_uid { get; set; }

		public string Hint_anchored_element { get; set; }

		public int Hint_anchor { get; set; }

		public int Hint_position_x { get; set; }

		public int Hint_position_y { get; set; }

		public int Hint_width { get; set; }

		public int Hint_height { get; set; }

		public string Hint_highlighted_element { get; set; }

		public int Hint_order { get; set; }

		[I18N]
		public string Hint_tooltip_textText { get; set; } = string.Empty;

		public int Hint_tooltip_text { get; set; }

		public int Hint_tooltip_position_enum { get; set; }

		public string Hint_tooltip_url { get; set; }

		public int Hint_tooltip_offset_x { get; set; }

		public int Hint_tooltip_offset_y { get; set; }

		public int Hint_tooltip_width { get; set; }

		public double Hint_creation_date { get; set; }

	}
}
