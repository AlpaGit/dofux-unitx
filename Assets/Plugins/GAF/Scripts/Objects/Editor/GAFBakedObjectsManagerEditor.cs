
// File:			GAFBakedObjectsManagerEditor.cs
// Version:			5.2
// Last changed:	2017/3/28 12:42
// Author:			Nikitin Nikolay, Nikitin Alexey
// Copyright:		© 2017 GAFMedia
// Project:			GAF Unity plugin


using System.Collections.Generic;
using System.Linq;
using GAF.Managed.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace GAF.Scripts.Objects.Editor
{
	[CustomEditor(typeof(GAFBakedObjectsManager))]
	public class GAFBakedObjectsManagerEditor : UnityEditor.Editor 
	{
		private bool 					m_ShowObjects 		= false;
		private List<GAFBakedObject> 	m_WithoutController = null;
		private List<GAFBakedObject> 	m_WithContoller 	= null;
		private Vector2 				m_ScrollPosition 	= new Vector2();
		
		#region Interface
		
		new public GAFBakedObjectsManager target
		{
			get 
			{
				return base.target as GAFBakedObjectsManager; 
			}
		}
		
		#endregion // Interface.

		public void OnEnable()
		{
			refillControllersLists();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			GUILayout.Space(2f);
			
			var enabled = target.objects != null && target.objects.Count() > 0 && target.objects.All(obj => !System.Object.Equals(obj, null));
			
			if (!enabled)
			{
				EditorGUILayout.LabelField("It's no baked objects in movie clip", EditorStyles.boldLabel);
			}
			else
			{
				GAFGuiUtil.drawLine(new Color(125f / 255f, 125f / 255f, 125f / 255f), 1f);
				GUILayout.Space(4f);

				m_ShowObjects = EditorGUILayout.Foldout(m_ShowObjects, "Objects: ");
				if (m_ShowObjects)
				{
					EditorGUILayout.BeginVertical();
					{
						var horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
						var verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
						var area = new GUIStyle(GUI.skin.textArea);
						
						m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, false, false, horizontalScrollbar, verticalScrollbar, area, GUILayout.Height(200f));
						{
							var selectedAllValue = target.objectsDict.Count == m_WithoutController.Count;
							var actualState = selectedAllValue;
							
							actualState = EditorGUILayout.ToggleLeft("\tAll", selectedAllValue, EditorStyles.boldLabel);
							
							GUILayout.Space(5f);
							
							if (selectedAllValue != actualState)
							{
								if (actualState)
								{
									m_WithoutController = m_WithoutController.Union(m_WithContoller).ToList();
									m_WithContoller.Clear();
								}
								else if (selectedAllValue)
								{
									m_WithContoller = m_WithContoller.Union(m_WithoutController).ToList();
									m_WithoutController.Clear();
								}
							}

							foreach (var obj in target.bakedObjects)
							{
								EditorGUILayout.BeginHorizontal();
								{
									var currentEnabled = m_WithoutController.Contains(obj);
									var nextEnabled = EditorGUILayout.ToggleLeft("\t" + obj.serializedProperties.name, currentEnabled, GUILayout.MaxWidth(150f));
									EditorGUILayout.LabelField("Type: " + obj.serializedProperties.type.ToString(), GUILayout.Width(90f));
									
									if (nextEnabled != currentEnabled)
									{
										if (nextEnabled)
										{
											m_WithoutController.Add(obj);
											m_WithContoller.Remove(obj);
										}
										else
										{
											m_WithContoller.Add(obj);
											m_WithoutController.Remove(obj);
										}
									}
								}
								EditorGUILayout.EndHorizontal();
								GUILayout.Space(1f);
							}
						}
						EditorGUILayout.EndScrollView();
						
						GUILayout.Space(5f);
						
						var actualWithout = getObjectsWithoutController();
						var actualWith = getObjectsWithController();
						
						GUI.enabled = 	actualWithout.Select(_obj => (int)(_obj.serializedProperties.objectID)).Sum() !=
										m_WithoutController.Select(_obj => (int)(_obj.serializedProperties.objectID)).Sum();
		
						EditorGUILayout.BeginHorizontal();
						{
							if (GUILayout.Button("Commit"))
							{
								var toRemove 	= m_WithoutController.Except(actualWithout).ToList();
								var toAdd 		= m_WithContoller.Except(actualWith).ToList();
								
								for (int i = 0; i < toAdd.Count; i++)
								{
									(toAdd[i] as GAFBakedObject).addController();
								}

								for (int i = 0; i < toRemove.Count; i++)
								{
									(toRemove[i] as GAFBakedObject).removeController();
								}

								target.clip.reload();
								
								refillControllersLists();
							}
							
							if (GUILayout.Button("Cancel"))
							{
								refillControllersLists();
							}
						}
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();
				}
				
				GUILayout.Space(5f);
			}
		}
		
		private void refillControllersLists()
		{
			m_WithoutController = getObjectsWithoutController();
			m_WithContoller 	= getObjectsWithController();
		}
		
		private List<GAFBakedObject> getObjectsWithoutController()
		{
			return target.bakedObjects.Where(obj => !obj.hasController()).ToList();
		}
		
		private List<GAFBakedObject> getObjectsWithController()
		{
			return target.bakedObjects.Where(obj => obj.hasController()).ToList();
		}
	}
}
