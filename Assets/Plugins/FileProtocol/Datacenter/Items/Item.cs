using System.Collections.Generic;
using DofusCoube.FileProtocol.Datacenter.Effects;

namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public class Item : IDofusObject
	{
		public static string Module => "Items";

		public int Id { get; set; }

		[I18N]
		public string Name { get; set; } = string.Empty;

		public int NameId { get; set; }

		public int TypeId { get; set; }

		[I18N]
		public string Description { get; set; } = string.Empty;

		public int DescriptionId { get; set; }

		public int IconId { get; set; }

		public int Level { get; set; }

		public int RealWeight { get; set; }

		public bool Cursed { get; set; }

		public int UseAnimationId { get; set; }

		public bool Usable { get; set; }

		public bool Targetable { get; set; }

		public bool Exchangeable { get; set; }

		public double Price { get; set; }

		public bool TwoHanded { get; set; }

		public bool Etheral { get; set; }

		public int ItemSetId { get; set; }

		public string Criteria { get; set; }

		public string CriteriaTarget { get; set; }

		public bool HideEffects { get; set; }

		public bool Enhanceable { get; set; }

		public bool NonUsableOnAnother { get; set; }

		public int AppearanceId { get; set; }

		public bool SecretRecipe { get; set; }

		public int RecipeSlots { get; set; }

		public List<uint> RecipeIds { get; set; }

		public List<uint> DropMonsterIds { get; set; }

		public List<uint> DropTemporisMonsterIds { get; set; }

		public bool ObjectIsDisplayOnWeb { get; set; }

		public bool BonusIsSecret { get; set; }

		public List<EffectInstance> PossibleEffects { get; set; }

		public List<uint> EvolutiveEffectIds { get; set; }

		public List<uint> FavoriteSubAreas { get; set; }

		public int FavoriteSubAreasBonus { get; set; }

		public int CraftXpRatio { get; set; }

		public bool NeedUseConfirm { get; set; }

		public bool IsDestructible { get; set; }

		public bool IsSaleable { get; set; }

		public bool IsColorable { get; set; }

		public bool IsLegendary { get; set; }

		public string CraftVisible { get; set; }

		public string CraftConditional { get; set; }

		public string CraftFeasible { get; set; }

		public string Visibility { get; set; }

		public double RecyclingNuggets { get; set; }

		public List<uint> FavoriteRecyclingSubareas { get; set; }

		public List<uint> ContainerIds { get; set; }

		public List<List<int>> ResourcesBySubarea { get; set; }

		[I18N]
		public string ImportantNotice { get; set; } = string.Empty;

		public int ImportantNoticeId { get; set; }

		public string ChangeVersion { get; set; }

		public double TooltipExpirationDate { get; set; }

	}
}
