using System;
using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Tiphon.Types;

namespace DofusCoube.FileProtocol.Datacenter.Appearance
{
	public sealed class SkinPosition : IDofusObject
	{
		public static string Module => "SkinPositions";

		public int Id { get; set; }

		public List<TransformData> Transformation { get; set; }

		public List<String> Clip { get; set; }

		public List<uint> Skin { get; set; }

	}
}
