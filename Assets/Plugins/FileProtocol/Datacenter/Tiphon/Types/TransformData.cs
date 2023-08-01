
namespace DofusCoube.FileProtocol.Datacenter.Tiphon.Types
{
	public sealed class TransformData : IDofusObject
	{
		public static string Module => "SkinPositions";

		public double X { get; set; }

		public double Y { get; set; }

		public double ScaleX { get; set; }

		public double ScaleY { get; set; }

		public int Rotation { get; set; }

		public string OriginalClip { get; set; }

		public string OverrideClip { get; set; }

	}
}
