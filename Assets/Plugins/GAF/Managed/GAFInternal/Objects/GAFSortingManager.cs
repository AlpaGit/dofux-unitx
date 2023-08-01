using System;
using System.Collections.Generic;
using GAF.Managed.GAFInternal.Core;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200004A RID: 74
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("")]
	public class GAFSortingManager : GAFBehaviour
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000082E1 File Offset: 0x000064E1
		private List<GAFObjectImpl> sortedObjects
		{
			get
			{
				if (m_SortedObjects == null)
				{
					m_SortedObjects = new List<GAFObjectImpl>();
				}
				return m_SortedObjects;
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000082FC File Offset: 0x000064FC
		public void initialize()
		{
			m_Clip = GetComponent<GAFBaseClip>();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000830A File Offset: 0x0000650A
		public void reload()
		{
			if (m_Clip == null)
			{
				m_Clip = GetComponent<GAFBaseClip>();
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008328 File Offset: 0x00006528
		public void updateToState()
		{
			if (isStateSet(State.Sort))
			{
				sortedObjects.Clear();
				foreach (var keyValuePair in m_Objects)
				{
					if (keyValuePair.Value.currentState.alpha > 0f)
					{
						sortedObjects.Add(keyValuePair.Value);
					}
				}
				sortedObjects.Sort();
				var num = 0f;
				var num2 = 1f * m_Clip.settings.zLayerScale;
				foreach (var gafobjectImpl in sortedObjects)
				{
					gafobjectImpl.updateZPosition(num);
					num += num2 * (float)gafobjectImpl.gafTransform.depth;
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008434 File Offset: 0x00006634
		internal void registerSubObject(uint _ID, GAFObjectImpl _Object)
		{
			m_Objects.Add(_ID, _Object);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008443 File Offset: 0x00006643
		internal void unregisterSubObject(uint _ID)
		{
			m_Objects.Remove(_ID);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008452 File Offset: 0x00006652
		public void clear()
		{
			m_SortedObjects = null;
			m_Objects.Clear();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008466 File Offset: 0x00006666
		public void pushSortRequest()
		{
			m_State |= State.Sort;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008476 File Offset: 0x00006676
		private bool isStateSet(State _State)
		{
			return (m_State & _State) == _State;
		}

		// Token: 0x040000D4 RID: 212
		[SerializeField]
		[HideInInspector]
		private GAFBaseClip m_Clip;

		// Token: 0x040000D5 RID: 213
		private State m_State;

		// Token: 0x040000D6 RID: 214
		private Dictionary<uint, GAFObjectImpl> m_Objects = new Dictionary<uint, GAFObjectImpl>();

		// Token: 0x040000D7 RID: 215
		private List<GAFObjectImpl> m_SortedObjects;

		// Token: 0x0200004B RID: 75
		[Flags]
		private enum State
		{
			// Token: 0x040000D9 RID: 217
			Null = 0,
			// Token: 0x040000DA RID: 218
			Sort = 1
		}
	}
}
