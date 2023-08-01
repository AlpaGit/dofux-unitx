
// File:			GAFCustomDelegateCreator.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Core;

namespace GAF.Scripts.Core
{
	public static class GAFCustomDelegateCreator
	{
		public static void Init()
		{
			GAFCustomResourceDelegate.DelegateCreator			= Creator;
			GAFCustomResourceDelegate.StaticDelegateCreator	= StaticCreator;
		}

		private static System.Func<string, GAFTexturesResourceInternal> Creator(object _Target, string _Method)
		{
			System.Func<string, GAFTexturesResourceInternal> result = null;

#if UNITY_WINRT && !UNITY_EDITOR

#if NETFX_CORE
			result = System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>.CreateDelegate(
				  typeof(System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>)
				, _Target
				, _Target.GetTypeInfo().GetDeclaredMethod(_Method)) as System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>;
#else
			result = System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>.CreateDelegate(
				  typeof(System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>)
				, _Target
				, _Target.GetType().GetMethod(_Method)) as System.Func<string, GAFInternal.Assets.GAFTexturesResourceInternal>;
#endif // NETFX_CORE

#else
			result = System.Delegate.CreateDelegate(
				  typeof(System.Func<string, GAFTexturesResourceInternal>)
				, _Target
				, _Target.GetType().GetMethod(_Method)) as System.Func<string, GAFTexturesResourceInternal>;
#endif // UNITY_WINRT && !UNITY_EDITOR

			return result;
		}

		private static System.Func<string, GAFTexturesResourceInternal> StaticCreator(string _TargetTypeString, string _Method)
		{
			var targetType = System.Type.GetType(_TargetTypeString);
			return System.Delegate.CreateDelegate(
				  typeof(System.Func<string, GAFTexturesResourceInternal>)
				, null
				, targetType.GetMethod(_Method)) as System.Func<string, GAFTexturesResourceInternal>;
		}
	}
}
