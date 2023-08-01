
// File:			GAFObjectEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.Editor.Objects;
using UnityEditor;

namespace GAF.Scripts.Objects.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(GAFObject))]
	public class GAFObjectEditor : GAFObjectInternalEditor
	{
	}
}