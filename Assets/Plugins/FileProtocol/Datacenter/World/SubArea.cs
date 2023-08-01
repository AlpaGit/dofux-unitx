using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.World
{
	public sealed class SubArea : IDofusObject
	{
		public static string Module => "SubAreas";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int AreaId { get; set; }

		public List<List<int>> Playlists { get; set; }

		public List<double> MapIds { get; set; }

		public List<int> Shape { get; set; }

		public int WorldmapId { get; set; }

		public List<uint> CustomWorldMap { get; set; }

		public uint PackId { get; set; }

		public uint Level { get; set; }

		public bool IsConquestVillage { get; set; }

		public bool BasicAccountAllowed { get; set; }

		public bool DisplayOnWorldMap { get; set; }

		public bool MountAutoTripAllowed { get; set; }

		public bool PsiAllowed { get; set; }

		public List<uint> Monsters { get; set; }

		public bool Capturable { get; set; }

		public List<List<double>> Quests { get; set; }

		public List<List<double>> Npcs { get; set; }

		public List<int> Harvestables { get; set; }

		public int AssociatedZaapMapId { get; set; }

		public List<int> Neighbors { get; set; }

	}
}
