using System;
using System.Collections.Generic;
using System.Linq;
using GAF.Managed.GAFInternal.Assets;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x02000086 RID: 134
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class GAFTransform : GAFBehaviour
	{
		public List<GAFTransform> Childs => m_Childs;
		
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000411 RID: 1041 RVA: 0x000122D0 File Offset: 0x000104D0
		// (remove) Token: 0x06000412 RID: 1042 RVA: 0x00012308 File Offset: 0x00010508
		public event Action<GAFTransform.TransformType> onTransformChanged;

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00012340 File Offset: 0x00010540
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0001238F File Offset: 0x0001058F
		public Dictionary<string, Material> baseMaterials
		{
			get
			{
				if (this.m_BaseMaterials == null)
				{
					var component = this.gafChilds[0].GetComponent<GAFBaseClip>();
					this.m_BaseMaterials = new Dictionary<string, Material>();
					this.setupMaterials(component.resource, component.settings.stencilValue);
				}
				return this.m_BaseMaterials;
			}
			set
			{
				this.m_BaseMaterials = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00012398 File Offset: 0x00010598
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x000123B3 File Offset: 0x000105B3
		public List<Material> registeredMaterials
		{
			get
			{
				if (this.m_RegisteredMaterials == null)
				{
					this.m_RegisteredMaterials = new List<Material>();
				}
				return this.m_RegisteredMaterials;
			}
			set
			{
				this.m_RegisteredMaterials = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000123BC File Offset: 0x000105BC
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000123C4 File Offset: 0x000105C4
		public bool realVisibility
		{
			get
			{
				return this.m_RealVisibility;
			}
			set
			{
				this.m_RealVisibility = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000123CD File Offset: 0x000105CD
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000123D5 File Offset: 0x000105D5
		public bool hasIndividualMaterial
		{
			get
			{
				return this.m_HasIndividualMaterial;
			}
			set
			{
				this.m_HasIndividualMaterial = value;
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000123E0 File Offset: 0x000105E0
		public void setupMaterials(GAFTexturesResourceInternal _Resource, int _StencilValue)
		{
			if (this.baseMaterials.Count == 0)
			{
				foreach (var gafbaseClip in base.GetComponentsInParent<GAFBaseClip>())
				{
					if (gafbaseClip.parent != null && gafbaseClip.parent.hasIndividualMaterial)
					{
						this.baseMaterials = gafbaseClip.parent.baseMaterials;
						this.registeredMaterials = gafbaseClip.parent.registeredMaterials;
						return;
					}
				}
				foreach (var gafresourceData in _Resource.data)
				{
					if (this.hasIndividualMaterial)
					{
						var material = new Material(_Resource.getSharedMaterial(gafresourceData.name));
						material.renderQueue = 3000;
						material.SetInt("_StencilID", GAFTransform.combineStencil(this.stencilValue, _StencilValue));
						this.baseMaterials.Add(gafresourceData.name, material);
					}
					else
					{
						this.baseMaterials.Add(gafresourceData.name, _Resource.getSharedMaterial(gafresourceData.name));
					}
				}
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001250C File Offset: 0x0001070C
		public Material getMaterial(string _Name)
		{
			return this.baseMaterials[_Name];
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001251A File Offset: 0x0001071A
		public void registerExternalMaterial(Material _Material)
		{
			if (!this.registeredMaterials.Contains(_Material))
			{
				this.registeredMaterials.Add(_Material);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00012536 File Offset: 0x00010736
		public static Matrix4x4 combineGeometry(Matrix4x4 _Matrix1, Matrix4x4 _Matrix2)
		{
			return _Matrix1 * _Matrix2;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001253F File Offset: 0x0001073F
		public static bool combineVisibility(bool _Visible1, bool _Visible2)
		{
			return _Visible1 && _Visible2;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00012544 File Offset: 0x00010744
		public static bool combineHasIndiviudalMaterial(bool _HasIndividualMaterial1, bool _HasIndividualMaterial2)
		{
			return _HasIndividualMaterial1 || _HasIndividualMaterial2;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00012549 File Offset: 0x00010749
		public static Color combineColor(Color _Color1, Color _Color2)
		{
			return _Color1 * _Color2;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00012554 File Offset: 0x00010754
		public static Vector4 combineColorOffset(Vector4 _Offset1, Vector4 _Offset2, Color _Multiplier)
		{
			var vector = _Offset1;
			vector.x *= _Multiplier.r;
			vector.y *= _Multiplier.g;
			vector.z *= _Multiplier.b;
			vector.w *= _Multiplier.a;
			return vector + _Offset2;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000125AE File Offset: 0x000107AE
		public static int combineStencil(int _Stencil1, int _Stencil2)
		{
			return Mathf.Clamp(_Stencil1 + _Stencil2, 0, 255);
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000125BE File Offset: 0x000107BE
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x000125C8 File Offset: 0x000107C8
		public GAFTransform gafParent
		{
			get
			{
				return this.m_Parent;
			}
			set
			{
				if (this.m_Parent != value)
				{
					if (this.m_Parent != null)
					{
						this.m_Parent.m_Childs.Remove(this);
					}
					if (value != null)
					{
						value.m_Childs.Add(this);
					}
					this.m_Parent = value;
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00012620 File Offset: 0x00010820
		public Dictionary<int, GAFTransform> gafChilds
		{
			get
			{
				if (this.m_ChildsDict == null)
				{
					this.m_ChildsDict = new Dictionary<int, GAFTransform>();
					foreach (var gaftransform in this.m_Childs)
					{
						this.m_ChildsDict.Add(gaftransform.GetInstanceID(), gaftransform);
					}
				}
				return this.m_ChildsDict;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00012698 File Offset: 0x00010898
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x000126A0 File Offset: 0x000108A0
		public Matrix4x4 localMatrix
		{
			get
			{
				return this.m_LocalMatrix;
			}
			set
			{
				if (this.m_LocalMatrix != value)
				{
					this.m_LocalMatrix = value;
					foreach (var gaftransform in this.m_Childs)
					{
						gaftransform.invokeTransformChanged(GAFTransform.TransformType.Geometry);
					}
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00012708 File Offset: 0x00010908
		public Matrix4x4 matrix
		{
			get
			{
				if (!(this.gafParent == null))
				{
					return GAFTransform.combineGeometry(this.gafParent.matrix, this.localMatrix);
				}
				return this.localMatrix;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00012735 File Offset: 0x00010935
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x00012740 File Offset: 0x00010940
		public bool localVisible
		{
			get
			{
				return this.m_IsLocalVisible;
			}
			set
			{
				if (this.gafParent != null)
				{
					this.m_IsLocalVisible = (value && this.gafParent.visible);
				}
				else
				{
					this.m_IsLocalVisible = value;
				}
				base.cachedRenderer.enabled = this.m_IsLocalVisible;
				foreach (var gaftransform in this.m_Childs)
				{
					gaftransform.invokeTransformChanged(GAFTransform.TransformType.Visibility);
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000127D0 File Offset: 0x000109D0
		public bool visible
		{
			get
			{
				if (!(this.gafParent == null))
				{
					return GAFTransform.combineVisibility(this.gafParent.visible, this.localVisible);
				}
				return this.localVisible;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x000127FD File Offset: 0x000109FD
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00012808 File Offset: 0x00010A08
		public GAFPair<Color, Vector4> localColorData
		{
			get
			{
				return this.m_LocalColorData;
			}
			set
			{
				if (this.m_LocalColorData.Key != value.Key || this.m_LocalColorData.Value != value.Value)
				{
					this.m_LocalColorData.Key = value.Key;
					this.m_LocalColorData.Value = value.Value;
					foreach (var gaftransform in this.m_Childs)
					{
						gaftransform.invokeTransformChanged(GAFTransform.TransformType.Color);
					}
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000128AC File Offset: 0x00010AAC
		public Color colorMultiplier
		{
			get
			{
				if (!(this.gafParent == null))
				{
					return GAFTransform.combineColor(this.gafParent.colorMultiplier, this.m_LocalColorData.Key);
				}
				return this.m_LocalColorData.Key;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x000128E4 File Offset: 0x00010AE4
		public Vector4 colorOffset
		{
			get
			{
				if (!(this.gafParent == null))
				{
					return GAFTransform.combineColorOffset(this.gafParent.colorOffset, this.m_LocalColorData.Value, this.m_LocalColorData.Key);
				}
				return this.m_LocalColorData.Value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00012934 File Offset: 0x00010B34
		public int depth
		{
			get
			{
				if (this.m_Depth == -1)
				{
					if (this.m_Childs.Count > 0)
					{
						this.m_Depth = this.m_Childs.Sum((GAFTransform _transform) => _transform.depth);
					}
					else
					{
						this.m_Depth = 1;
					}
				}
				return this.m_Depth;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00012997 File Offset: 0x00010B97
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x000129A0 File Offset: 0x00010BA0
		public int localStencilValue
		{
			get
			{
				return this.m_StencilValue;
			}
			set
			{
				if (this.m_StencilValue != value)
				{
					this.m_StencilValue = value;
					foreach (var gaftransform in this.m_Childs)
					{
						gaftransform.invokeTransformChanged(GAFTransform.TransformType.Masking);
					}
				}
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00012A04 File Offset: 0x00010C04
		public int stencilValue
		{
			get
			{
				if (!(this.gafParent == null))
				{
					return GAFTransform.combineStencil(this.gafParent.stencilValue, this.localStencilValue);
				}
				return this.localStencilValue;
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00012A31 File Offset: 0x00010C31
		public void invokeTransformChanged(GAFTransform.TransformType _Type)
		{
			if (this.onTransformChanged != null)
			{
				this.onTransformChanged(_Type);
			}
		}

		// Token: 0x04000217 RID: 535
		[SerializeField]
		private GAFTransform m_Parent;

		// Token: 0x04000218 RID: 536
		[SerializeField]
		private List<GAFTransform> m_Childs = new List<GAFTransform>();

		// Token: 0x04000219 RID: 537
		[SerializeField]
		[HideInInspector]
		private GAFPair<Color, Vector4> m_LocalColorData = new GAFPair<Color, Vector4>(Color.white, Vector4.zero);

		// Token: 0x0400021A RID: 538
		[SerializeField]
		[HideInInspector]
		private Matrix4x4 m_LocalMatrix = Matrix4x4.identity;

		// Token: 0x0400021B RID: 539
		[SerializeField]
		[HideInInspector]
		private bool m_IsLocalVisible = true;

		// Token: 0x0400021C RID: 540
		[HideInInspector]
		[SerializeField]
		private bool m_RealVisibility;

		// Token: 0x0400021D RID: 541
		[SerializeField]
		[HideInInspector]
		private int m_StencilValue;

		// Token: 0x0400021E RID: 542
		[HideInInspector]
		[SerializeField]
		private bool m_HasIndividualMaterial;

		// Token: 0x0400021F RID: 543
		[HideInInspector]
		[NonSerialized]
		private int m_Depth = -1;

		// Token: 0x04000220 RID: 544
		[HideInInspector]
		[NonSerialized]
		private Dictionary<int, GAFTransform> m_ChildsDict;

		// Token: 0x04000221 RID: 545
		[HideInInspector]
		[NonSerialized]
		protected Dictionary<string, Material> m_BaseMaterials = new Dictionary<string, Material>();

		// Token: 0x04000222 RID: 546
		[HideInInspector]
		[NonSerialized]
		protected List<Material> m_RegisteredMaterials = new List<Material>();

		// Token: 0x02000087 RID: 135
		[Flags]
		public enum TransformType
		{
			// Token: 0x04000224 RID: 548
			None = 0,
			// Token: 0x04000225 RID: 549
			Geometry = 1,
			// Token: 0x04000226 RID: 550
			Visibility = 2,
			// Token: 0x04000227 RID: 551
			Color = 4,
			// Token: 0x04000228 RID: 552
			Masking = 8
		}
	}
}
