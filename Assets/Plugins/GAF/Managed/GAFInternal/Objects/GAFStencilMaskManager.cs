using System.Collections.Generic;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200004C RID: 76
	public static class GAFStencilMaskManager
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00008496 File Offset: 0x00006696
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000849D File Offset: 0x0000669D
		private static Dictionary<int, Dictionary<uint, IGAFMaskInternal>> stencilMasks { get; set; } = new Dictionary<int, Dictionary<uint, IGAFMaskInternal>>();

		// Token: 0x060001BB RID: 443 RVA: 0x000084B4 File Offset: 0x000066B4
		internal static void registerMask(int _ClipInstanceID, uint _ObjectID, IGAFMaskInternal _Mask)
		{
			if (stencilMasks.ContainsKey(_ClipInstanceID))
			{
				stencilMasks[_ClipInstanceID][_ObjectID] = _Mask;
				return;
			}
			stencilMasks.Add(_ClipInstanceID, new Dictionary<uint, IGAFMaskInternal>());
			stencilMasks[_ClipInstanceID][_ObjectID] = _Mask;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008504 File Offset: 0x00006704
		internal static void unregisterMask(int _ClipInstanceID, uint _ObjectID)
		{
			if (stencilMasks.ContainsKey(_ClipInstanceID) && stencilMasks[_ClipInstanceID].ContainsKey(_ObjectID))
			{
				stencilMasks[_ClipInstanceID].Remove(_ObjectID);
				if (stencilMasks[_ClipInstanceID].Count == 0)
				{
					stencilMasks.Remove(_ClipInstanceID);
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008564 File Offset: 0x00006764
		internal static IGAFMaskInternal getMask(int _ClipID, uint _MaskID)
		{
			IGAFMaskInternal result = null;
			if (stencilMasks.ContainsKey(_ClipID) && stencilMasks[_ClipID].ContainsKey(_MaskID))
			{
				result = stencilMasks[_ClipID][_MaskID];
			}
			return result;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000085A6 File Offset: 0x000067A6
		internal static void clear()
		{
			stencilMasks.Clear();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000085B2 File Offset: 0x000067B2
		private static int defaultDelegate()
		{
			return 0;
		}
	}
}
