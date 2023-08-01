using System;
using System.Collections.Generic;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x02000045 RID: 69
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	[Serializable]
	public abstract class GAFBaseObjectsManager : GAFBehaviour
	{
		/// <summary>
		/// Reference to a clip
		/// </summary>
		/// <value>The clip.</value>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000170 RID: 368
		public abstract GAFBaseClip clip { get; }

		/// <summary>
		/// All objects list. Serialized data
		/// </summary>
		/// <value>The objects.</value>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000171 RID: 369
		public abstract List<IGAFObject> objects { get; }

		/// <summary>
		/// Objects list organized as dictionary. Non serialized data.
		/// </summary>
		/// <value>The objects dictionary.</value>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000172 RID: 370
		public abstract Dictionary<uint, IGAFObject> objectsDict { get; }

		/// <summary>
		/// Initialize object manager parameters. 
		/// <para />Animation subobjects are created here.
		/// </summary>
		// Token: 0x06000173 RID: 371
		public abstract void initialize();

		/// <summary>
		/// Reload object manager.
		/// <para />Non serialized subojects data reloads here.
		/// </summary>
		// Token: 0x06000174 RID: 372
		public abstract void reload();

		/// <summary>
		/// Reset clip view.
		/// <para />Clean up non serialized objects data.
		/// </summary>
		// Token: 0x06000175 RID: 373
		public abstract void cleanView();

		/// <summary>
		/// Clear serialized and non serialized information about objects. 
		/// <para />Don't destroy children.
		/// </summary>
		// Token: 0x06000176 RID: 374
		public abstract void clear();

		/// <summary>
		/// Clear serialized and non serialized information about objects.
		/// <para />Destroy children.
		/// </summary>
		// Token: 0x06000177 RID: 375
		public abstract void deepClear();

		/// <summary>
		/// Updates all objects presented in states list.
		/// </summary>
		/// <param name="_States">The _ states.</param>
		/// <param name="_Refresh">if set to <c>true</c> [_ refresh].</param>
		// Token: 0x06000178 RID: 376
		public abstract void updateToFrame(Dictionary<uint, GAFObjectStateData> _States, bool _Refresh);

		// Token: 0x06000179 RID: 377
		public abstract void updateToKeyFrame(List<GAFObjectStateData> _States);

		// Token: 0x0600017A RID: 378
		protected abstract void createObjects();

		// Token: 0x0600017B RID: 379 RVA: 0x000078D8 File Offset: 0x00005AD8
		protected string getObjectName(GAFInternal.Data.GAFObjectData _Object)
		{
			var namedParts = clip.asset.getNamedParts(clip.timelineID);
			
			var gafnamedPartData = namedParts.Find(partData => partData.objectID == _Object.id);
			
			var result = string.Empty;
			if (gafnamedPartData == null)
			{
				if (_Object.type == GAFObjectType.Timeline)
				{
					result = clip.asset.getTimeline((int)_Object.atlasElementID).linkageName;
				}
				else
				{
					result = _Object.atlasElementID + "_" + _Object.id;
				}
			}
			else
			{
				result = gafnamedPartData.name;
			}
			return result;
		}
	}
}
