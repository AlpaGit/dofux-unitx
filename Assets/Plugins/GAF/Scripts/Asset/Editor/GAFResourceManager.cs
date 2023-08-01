
// File:			GAFResourceManager.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using GAF.Managed.Editor.Assets;
using GAF.Managed.GAFInternal.Assets;
using GAF.Managed.GAFInternal.Data;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GAF.Scripts.Asset.Editor
{
	public class GAFResourceManager : GAFResourceManagerInternal
	{
		public override void createMecanimResources(GAFAnimationAssetInternal _Asset)
		{
			var serializedAsset = new SerializedObject(_Asset);
			var mecanimFolder = serializedAsset.FindProperty("m_MecanimResourcesDirectory");

			var mecanimFolderPath = string.Empty;
			if (!string.IsNullOrEmpty(mecanimFolder.stringValue) &&
				System.IO.Directory.Exists(Application.dataPath + "/" + mecanimFolder.stringValue))
			{
				mecanimFolderPath = "Assets/" + mecanimFolder.stringValue;
			}
			else
			{
				var assetDirectoryPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(_Asset));
				mecanimFolderPath = assetDirectoryPath + "/" + _Asset.name + "_animator/";
				mecanimFolder.stringValue = mecanimFolderPath.Substring("Assets/".Length, mecanimFolderPath.Length - "Assets/".Length);
				serializedAsset.ApplyModifiedProperties();

				if (!System.IO.Directory.Exists(Application.dataPath + "/" + mecanimFolderPath.Substring("Assets/".Length, mecanimFolderPath.Length - "Assets/".Length)))
					AssetDatabase.CreateFolder(assetDirectoryPath, _Asset.name + "_animator");
			}

			List<RuntimeAnimatorController> controllersList = new List<RuntimeAnimatorController>();
			foreach (var timeline in _Asset.getTimelines())
			{
				var controllerPath = mecanimFolderPath + "[" + _Asset.name + "]_Timeline_" + timeline.linkageName + ".controller";
				var animatorController = AssetDatabase.LoadAssetAtPath(controllerPath, typeof(AnimatorController)) as AnimatorController;
				if (animatorController == null)
					animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

				createAnimations(timeline, animatorController, mecanimFolderPath);

				controllersList.Add(animatorController);
			}

			var controllers = serializedAsset.FindProperty("m_AnimatorControllers");
			controllers.ClearArray();
			for (int i = 0; i < controllersList.Count; ++i)
			{
				controllers.InsertArrayElementAtIndex(0);
				var property = controllers.GetArrayElementAtIndex(0);
				property.objectReferenceValue = controllersList[i];
			}

			serializedAsset.ApplyModifiedProperties();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		protected override void createAnimations(GAFTimelineData _Timeline, AnimatorController _AnimatorController, string _AnimationsPath)
		{
			var layer				= _AnimatorController.layers[0];
			var stateMachine		= layer.stateMachine;
			var isNewStateMachine 	= stateMachine.states.Length == 0;

            var i = 0;

			foreach (var sequence in _Timeline.sequences)
			{
				int stateFoundIndex = -1;

				for (int stateIndex = 0; stateIndex < stateMachine.states.Length; ++stateIndex)
				{
					if (stateMachine.states[stateIndex].state.name == sequence.name)
					{
						stateFoundIndex = stateIndex;
						break;
					}
				}

				var clipName	= _AnimatorController.name + "_" + sequence.name + ".anim";
				var clip		= AssetDatabase.LoadAssetAtPath(_AnimationsPath + clipName, typeof(AnimationClip)) as AnimationClip;
				if (clip == null)
					clip = createAnimationClip(_Timeline, sequence, _AnimationsPath + clipName);

				AnimatorState state = null;
				if (stateFoundIndex >= 0)
				{
					state = stateMachine.states[stateFoundIndex].state;
				}
				else
				{
					state = stateMachine.AddState(sequence.name, new Vector3(0, 50f, 0f) * i);
				}

				state.motion = clip;

				if (isNewStateMachine)
				{
					if (sequence.name.ToLower() == "default")
					{
						stateMachine.defaultState = state;
					}
				}
				++i;
			}
		}

		protected override AnimationClip createAnimationClip(GAFTimelineData _Timeline, GAFSequenceData _Sequence, string _Path)
		{
			var animationClip = new AnimationClip()
			{
				frameRate = 30
				,
				name = _Sequence.name
			};

			var framesCount = _Sequence.endFrame - _Sequence.startFrame + 1;
			var timeInterval = framesCount / animationClip.frameRate / framesCount;

			var time = 0f;
			var events = new AnimationEvent[framesCount];
			for (uint i = _Sequence.startFrame, j = 0; i <= _Sequence.endFrame; i++, j++)
			{
				var animationEvent = new AnimationEvent();
				animationEvent.functionName = i != _Sequence.startFrame ? "updateToFrameAnimator" : "updateToFrameAnimatorWithRefresh";
				animationEvent.intParameter = (int)i;
				animationEvent.time = time;

				events[j] = animationEvent;

				time += timeInterval;
			}

			var serializedObj = new SerializedObject(animationClip);
			var settings = serializedObj.FindProperty("m_AnimationClipSettings");
			var loopTime = settings.FindPropertyRelative("m_LoopTime");

			loopTime.boolValue = true;
			serializedObj.ApplyModifiedProperties();

			AnimationUtility.SetAnimationEvents(animationClip, events);

			AssetDatabase.CreateAsset(animationClip, _Path);

			return animationClip;
		}
	}
}