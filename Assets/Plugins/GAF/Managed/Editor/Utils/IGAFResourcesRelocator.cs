using UnityEditor;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x0200000E RID: 14
	internal interface IGAFResourcesRelocator
	{
		// Token: 0x06000031 RID: 49
		void relocate(SerializedObject _ResourcesContainer, string _Location);
	}
}
