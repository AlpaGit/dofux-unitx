using System;
using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class WorldMap : IDofusObject
	{
		public static string Module => "WorldMaps";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int OrigineX { get; set; }

		public int OrigineY { get; set; }

		public double MapWidth { get; set; }

		public double MapHeight { get; set; }

		public bool ViewableEverywhere { get; set; }

		public double MinScale { get; set; }

		public double MaxScale { get; set; }

		public double StartScale { get; set; }

		public int TotalWidth { get; set; }

		public int TotalHeight { get; set; }

		public List<String> Zoom { get; set; }

		public bool VisibleOnMap { get; set; }

	}
}
