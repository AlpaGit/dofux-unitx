
// File:			GAFBakedAnimatorEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Scripts.Asset;
using GAF.Scripts.Core.Editor.Interfaces;
using GAF.Scripts.Objects;
using UnityEditor;

namespace GAF.Scripts.Core.Editor
{
	[CustomEditor(typeof(GAFBakedAnimator))]
	[CanEditMultipleObjects]
	public class GAFBakedAnimatorEditor : GAFBakedAnimatorInternalEditor<GAFBakedObjectsManager, GAFTexturesResource>
	{
	}
}
