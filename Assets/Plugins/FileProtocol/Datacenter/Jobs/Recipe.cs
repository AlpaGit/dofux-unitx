using System.Collections.Generic;

namespace DofusCoube.FileProtocol.Datacenter.Jobs
{
	public sealed class Recipe : IDofusObject
	{
		public static string Module => "Recipes";

		public int ResultId { get; set; }

		[I18N]
		public string ResultName { get; set; } = string.Empty;

		public int ResultNameId { get; set; }

		public uint ResultTypeId { get; set; }

		public int ResultLevel { get; set; }

		public List<int> IngredientIds { get; set; }

		public List<uint> Quantities { get; set; }

		public int JobId { get; set; }

		public int SkillId { get; set; }

		public string ChangeVersion { get; set; }

		public double TooltipExpirationDate { get; set; }

	}
}
