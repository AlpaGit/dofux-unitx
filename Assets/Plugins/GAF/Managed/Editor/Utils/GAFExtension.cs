using UnityEngine;

namespace GAF.Managed.Editor.Utils
{
	// Token: 0x02000004 RID: 4
	public static class GAFExtension
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002050 File Offset: 0x00000250
		public static bool KeyPressed<T>(this T _Value, string _ControlName, KeyCode _Key, out T _FieldValue)
		{
			_FieldValue = _Value;
			return GUI.GetNameOfFocusedControl() == _ControlName && (Event.current.type == EventType.KeyUp && Event.current.keyCode == _Key);
		}
	}
}
