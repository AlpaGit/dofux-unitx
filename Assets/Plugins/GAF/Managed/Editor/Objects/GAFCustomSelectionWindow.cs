using System;
using UnityEditor;
using UnityEngine;

namespace GAF.Managed.Editor.Objects
{
	// Token: 0x0200001C RID: 28
	public class GAFCustomSelectionWindow : EditorWindow
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000060 RID: 96 RVA: 0x00004748 File Offset: 0x00002948
		// (remove) Token: 0x06000061 RID: 97 RVA: 0x00004780 File Offset: 0x00002980
		public event Action<Rect> onRectChange;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000047B8 File Offset: 0x000029B8
		private Rect selectionInfoRect
		{
			get
			{
				return new Rect(base.position.width - 280f, base.position.height - 130f, 280f, 130f);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000063 RID: 99 RVA: 0x000022FC File Offset: 0x000004FC
		private bool verticalResize
		{
			get
			{
				return this.m_CapturedSide == GAFCustomSelectionWindow.CapturedSide.Bottom || this.m_CapturedSide == GAFCustomSelectionWindow.CapturedSide.Top;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002312 File Offset: 0x00000512
		private bool horizontalResize
		{
			get
			{
				return this.m_CapturedSide == GAFCustomSelectionWindow.CapturedSide.Left || this.m_CapturedSide == GAFCustomSelectionWindow.CapturedSide.Right;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002328 File Offset: 0x00000528
		public static GAFCustomSelectionWindow GetWindow(Texture2D _InspectedTexture)
		{
			GAFCustomSelectionWindow.m_Atlas = _InspectedTexture;
			return EditorWindow.GetWindow<GAFCustomSelectionWindow>(false, "", true);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000047FC File Offset: 0x000029FC
		private void OnEnable()
		{
			base.minSize = new Vector2(400f, 300f);
			float num = (float)Screen.currentResolution.width / 2f - base.minSize.x / 2f;
			float num2 = (float)Screen.currentResolution.height / 2f - base.minSize.y / 2f;
			base.position = new Rect(num, num2, base.minSize.x, base.minSize.y);
			if (GAFCustomSelectionWindow.m_Atlas != null)
			{
				this.m_SelectionRectStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("selectionRect");
				this.m_WindowSize         = new Vector2(base.position.width, base.position.height);
				this.m_Background         = new Texture2D((int)this.m_WindowSize.x, (int)this.m_WindowSize.y, TextureFormat.ARGB32, false);
				for (int i = 0; i < this.m_Background.width; i++)
				{
					for (int j = 0; j < this.m_Background.height; j++)
					{
						this.m_Background.SetPixel(i, j, default(Color));
					}
				}
				this.m_Background.Apply();
				this.m_BorderStyle = new GUIStyle(GUI.skin.box);
				this.m_BorderStyle.normal.background = EditorGUIUtility.whiteTexture;
				this.m_BorderStyle.border = new RectOffset(2, 2, 2, 2);
				this.m_TextureRect = new Rect(this.m_TexturePositionX, this.m_TexturePositionY, (float)GAFCustomSelectionWindow.m_Atlas.width, (float)GAFCustomSelectionWindow.m_Atlas.height);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000049C4 File Offset: 0x00002BC4
		private void OnGUI()
		{
			if (this.m_Background != null)
			{
				EditorGUI.DrawTextureTransparent(new Rect(0f, 0f, base.position.width, base.position.height), this.m_Background);
				this.m_ScrollPosition = EditorGUILayout.BeginScrollView(this.m_ScrollPosition, Array.Empty<GUILayoutOption>());
				GUI.DrawTexture(this.m_TextureRect, GAFCustomSelectionWindow.m_Atlas, ScaleMode.ScaleToFit, true);
				EditorGUILayout.EndScrollView();
				if (!this.m_RectDrawn)
				{
					this.drawSelectionRect(this.m_TextureRect);
				}
				else
				{
					this.drawSelectedRect(this.m_TextureRect);
				}
				if (Event.current.type == EventType.Repaint)
				{
					this.m_SelectionRectStyle.Draw(this.m_SelectionRect, "", false, false, false, false);
					this.setResizeRects();
				}
				if (Event.current.keyCode == KeyCode.Escape)
				{
					this.reset(true);
				}
				Color color = Handles.color;
				Handles.color = this.m_BorderColor;
				Handles.DrawLine(Vector3.one, new Vector3((float)GAFCustomSelectionWindow.m_Atlas.width, 0f));
				Handles.DrawLine(new Vector3((float)GAFCustomSelectionWindow.m_Atlas.width, 0f), new Vector3((float)GAFCustomSelectionWindow.m_Atlas.width, (float)GAFCustomSelectionWindow.m_Atlas.height));
				Handles.DrawLine(new Vector3((float)GAFCustomSelectionWindow.m_Atlas.width, (float)GAFCustomSelectionWindow.m_Atlas.height), new Vector3(0f, (float)GAFCustomSelectionWindow.m_Atlas.height));
				Handles.DrawLine(new Vector3(0f, (float)GAFCustomSelectionWindow.m_Atlas.height), Vector3.one);
				Handles.color = color;
				base.Repaint();
				this.drawInfoPanel();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004B74 File Offset: 0x00002D74
		private void drawInfoPanel()
		{
			GUILayout.BeginArea(this.selectionInfoRect);
			GUILayout.BeginVertical(new GUIContent("Settings"), GUI.skin.window, Array.Empty<GUILayoutOption>());
			GUI.enabled               = this.m_RectDrawn;
			this.m_SelectionRect      = EditorGUILayout.RectField("Selected region: ", this.m_SelectionRect, Array.Empty<GUILayoutOption>());
			EditorGUIUtility.wideMode = true;
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Commit", new GUILayoutOption[]
			{
				GUILayout.Width(100f)
			}))
			{
				if (this.onRectChange != null)
				{
					this.onRectChange(this.m_SelectionRect);
				}
				this.reset(true);
				GC.Collect();
				base.Close();
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Cancel", new GUILayoutOption[]
			{
				GUILayout.Width(100f)
			}))
			{
				this.reset(true);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUI.enabled = true;
			GUILayout.Space(6f);
			this.m_BorderColor = EditorGUILayout.ColorField("Border color: ", this.m_BorderColor, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004CAC File Offset: 0x00002EAC
		private void drawSelectionRect(Rect _TextureRect)
		{
			Event current = Event.current;
			this.m_OutOfTextureBounds = !_TextureRect.Contains(current.mousePosition);
			switch (current.type)
			{
			case 0:
				if (current.button == 0 && !this.m_IsDragging)
				{
					this.m_IsDragging = true;
					this.m_Left = 0;
					this.m_Top = 0;
					this.m_Right = 0;
					this.m_Bottom = 0;
					this.m_StartMousePosition = current.mousePosition;
					this.m_CurrentMousePosition = current.mousePosition;
					this.m_SelectionRect = default(Rect);
					return;
				}
				break;
			case EventType.MouseUp:
				if (this.m_IsDragging)
				{
					if (this.m_SelectionRect.width == 0f || this.m_SelectionRect.height == 0f)
					{
						this.reset(true);
						return;
					}
					this.m_RectDrawn = true;
				}
				this.reset(false);
				break;
			case EventType.MouseMove:
				break;
			case EventType.MouseDrag:
				this.drag(current);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004D9C File Offset: 0x00002F9C
		private void drawSelectedRect(Rect _TextureRect)
		{
			Event current = Event.current;
			switch (current.type)
			{
			case 0:
				if (current.button == 0 && !this.m_IsDragging)
				{
					if (!this.checkRect(current.mousePosition))
					{
						return;
					}
					this.m_IsDragging = true;
					return;
				}
				break;
			case EventType.MouseUp:
				if (this.m_IsDragging)
				{
					this.reset(false);
				}
				break;
			case EventType.MouseMove:
				break;
			case EventType.MouseDrag:
				this.resizeDrawnRect(current);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004E0C File Offset: 0x0000300C
		private void reset(bool _IsHard)
		{
			this.m_IsDragging = false;
			this.m_OutOfTextureBounds = false;
			this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.None;
			if (this.m_CurrentMousePosition.y < this.m_StartMousePosition.y)
			{
				Vector2 startMousePosition = this.m_StartMousePosition;
				this.m_StartMousePosition = new Vector2(this.m_StartMousePosition.x, this.m_CurrentMousePosition.y);
				this.m_CurrentMousePosition = new Vector2(this.m_CurrentMousePosition.x, startMousePosition.y);
			}
			if (this.m_CurrentMousePosition.x < this.m_StartMousePosition.x)
			{
				Vector2 startMousePosition2 = this.m_StartMousePosition;
				this.m_StartMousePosition = new Vector2(this.m_CurrentMousePosition.x, this.m_StartMousePosition.y);
				this.m_CurrentMousePosition = startMousePosition2;
			}
			if (_IsHard)
			{
				this.m_RectDrawn = false;
				this.m_SelectionRect = default(Rect);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004EE8 File Offset: 0x000030E8
		private void drag(Event _CurrentEvent)
		{
			if (this.m_IsDragging && !this.m_OutOfTextureBounds)
			{
				this.m_CurrentMousePosition = _CurrentEvent.mousePosition;
				if (this.m_StartMousePosition.x > this.m_CurrentMousePosition.x)
				{
					this.m_Left = (int)this.m_CurrentMousePosition.x;
					this.m_Right = (int)this.m_StartMousePosition.x;
				}
				else
				{
					this.m_Left = (int)this.m_StartMousePosition.x;
					this.m_Right = (int)this.m_CurrentMousePosition.x;
				}
				if (this.m_StartMousePosition.y > this.m_CurrentMousePosition.y)
				{
					this.m_Top = (int)this.m_CurrentMousePosition.y;
					this.m_Bottom = (int)this.m_StartMousePosition.y;
				}
				else
				{
					this.m_Top = (int)this.m_StartMousePosition.y;
					this.m_Bottom = (int)this.m_CurrentMousePosition.y;
				}
				this.m_SelectionRect = new Rect((float)this.m_Left, (float)this.m_Top, (float)(this.m_Right - this.m_Left), (float)(this.m_Bottom - this.m_Top));
				if (_CurrentEvent.mousePosition.x < 0f || _CurrentEvent.mousePosition.y < 0f || _CurrentEvent.mousePosition.x > base.position.width || _CurrentEvent.mousePosition.y > base.position.height)
				{
					this.reset(false);
					this.m_RectDrawn = true;
				}
				base.Repaint();
				_CurrentEvent.Use();
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005084 File Offset: 0x00003284
		private void resizeDrawnRect(Event _CurrentEvent)
		{
			if (this.m_IsDragging)
			{
				Vector2 startMousePosition = this.m_StartMousePosition;
				switch (this.m_CapturedSide)
				{
				case GAFCustomSelectionWindow.CapturedSide.TopLeft:
					this.m_StartMousePosition = _CurrentEvent.mousePosition;
					break;
				case GAFCustomSelectionWindow.CapturedSide.Top:
					this.m_StartMousePosition = new Vector2(startMousePosition.x, _CurrentEvent.mousePosition.y);
					break;
				case GAFCustomSelectionWindow.CapturedSide.TopRight:
					this.m_StartMousePosition = new Vector2(startMousePosition.x, _CurrentEvent.mousePosition.y);
					this.m_CurrentMousePosition = new Vector2(_CurrentEvent.mousePosition.x, this.m_CurrentMousePosition.y);
					break;
				case GAFCustomSelectionWindow.CapturedSide.Right:
					this.m_CurrentMousePosition = new Vector2(_CurrentEvent.mousePosition.x, this.m_CurrentMousePosition.y);
					break;
				case GAFCustomSelectionWindow.CapturedSide.BottomRight:
					this.m_CurrentMousePosition = _CurrentEvent.mousePosition;
					break;
				case GAFCustomSelectionWindow.CapturedSide.Bottom:
					this.m_CurrentMousePosition = new Vector2(this.m_CurrentMousePosition.x, _CurrentEvent.mousePosition.y);
					break;
				case GAFCustomSelectionWindow.CapturedSide.BottomLeft:
					this.m_StartMousePosition = new Vector2(_CurrentEvent.mousePosition.x, startMousePosition.y);
					this.m_CurrentMousePosition = new Vector2(this.m_CurrentMousePosition.x, _CurrentEvent.mousePosition.y);
					break;
				case GAFCustomSelectionWindow.CapturedSide.Left:
					this.m_StartMousePosition = new Vector2(_CurrentEvent.mousePosition.x, startMousePosition.y);
					break;
				}
				this.checkRestrictions();
				if (!this.verticalResize)
				{
					if (this.m_StartMousePosition.x > this.m_CurrentMousePosition.x)
					{
						this.m_Left = (int)this.m_CurrentMousePosition.x;
						this.m_Right = (int)this.m_StartMousePosition.x;
					}
					else
					{
						this.m_Left = (int)this.m_StartMousePosition.x;
						this.m_Right = (int)this.m_CurrentMousePosition.x;
					}
				}
				if (!this.horizontalResize)
				{
					if (this.m_StartMousePosition.y > this.m_CurrentMousePosition.y)
					{
						this.m_Top = (int)this.m_CurrentMousePosition.y;
						this.m_Bottom = (int)this.m_StartMousePosition.y;
					}
					else
					{
						this.m_Top = (int)this.m_StartMousePosition.y;
						this.m_Bottom = (int)this.m_CurrentMousePosition.y;
					}
				}
				this.m_SelectionRect = new Rect((float)this.m_Left, (float)this.m_Top, (float)(this.m_Right - this.m_Left), (float)(this.m_Bottom - this.m_Top));
				if (_CurrentEvent.mousePosition.x < 0f || _CurrentEvent.mousePosition.y < 0f || _CurrentEvent.mousePosition.x > base.position.width || _CurrentEvent.mousePosition.y > base.position.height)
				{
					this.reset(false);
				}
				base.Repaint();
				_CurrentEvent.Use();
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000537C File Offset: 0x0000357C
		private void setResizeRects()
		{
			if (this.m_SelectionRect != default(Rect))
			{
				this.m_TopLeftRect = new Rect(this.m_SelectionRect.x - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_TopRect = new Rect(this.m_SelectionRect.x + this.m_SelectionRect.width / 2f - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_TopRightRect = new Rect(this.m_SelectionRect.x + this.m_SelectionRect.width - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_RightRect = new Rect(this.m_SelectionRect.x + this.m_SelectionRect.width - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y + this.m_SelectionRect.height / 2f - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_BottomRightRect = new Rect(this.m_SelectionRect.x + this.m_SelectionRect.width - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y + this.m_SelectionRect.height - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_BottomRect = new Rect(this.m_SelectionRect.x + this.m_SelectionRect.width / 2f - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y + this.m_SelectionRect.height - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_BottomLeftRect = new Rect(this.m_SelectionRect.x - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y + this.m_SelectionRect.height - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				this.m_LeftRect = new Rect(this.m_SelectionRect.x - this.m_ResizeRectSize / 2f, this.m_SelectionRect.y + this.m_SelectionRect.height / 2f - this.m_ResizeRectSize / 2f, this.m_ResizeRectSize, this.m_ResizeRectSize);
				EditorGUI.DrawRect(this.m_TopLeftRect, Color.white);
				EditorGUI.DrawRect(this.m_TopRect, Color.white);
				EditorGUI.DrawRect(this.m_TopRightRect, Color.white);
				EditorGUI.DrawRect(this.m_RightRect, Color.white);
				EditorGUI.DrawRect(this.m_BottomRightRect, Color.white);
				EditorGUI.DrawRect(this.m_BottomRect, Color.white);
				EditorGUI.DrawRect(this.m_BottomLeftRect, Color.white);
				EditorGUI.DrawRect(this.m_LeftRect, Color.white);
				EditorGUIUtility.AddCursorRect(this.m_TopLeftRect, MouseCursor.ResizeUpLeft);
				EditorGUIUtility.AddCursorRect(this.m_TopRect, MouseCursor.ResizeVertical);
				EditorGUIUtility.AddCursorRect(this.m_TopRightRect, MouseCursor.ResizeUpRight);
				EditorGUIUtility.AddCursorRect(this.m_RightRect, MouseCursor.ResizeHorizontal);
				EditorGUIUtility.AddCursorRect(this.m_BottomRightRect, MouseCursor.ResizeUpLeft);
				EditorGUIUtility.AddCursorRect(this.m_BottomRect, MouseCursor.ResizeVertical);
				EditorGUIUtility.AddCursorRect(this.m_BottomLeftRect, MouseCursor.ResizeUpRight);
				EditorGUIUtility.AddCursorRect(this.m_LeftRect, MouseCursor.ResizeHorizontal);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000574C File Offset: 0x0000394C
		private bool checkRect(Vector2 _CurrentMousePosition)
		{
			this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.None;
			if (this.m_TopLeftRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.TopLeft;
			}
			if (this.m_TopRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.Top;
			}
			if (this.m_TopRightRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.TopRight;
			}
			if (this.m_RightRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.Right;
			}
			if (this.m_BottomRightRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.BottomRight;
			}
			if (this.m_BottomRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.Bottom;
			}
			if (this.m_BottomLeftRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.BottomLeft;
			}
			if (this.m_LeftRect.Contains(_CurrentMousePosition))
			{
				this.m_CapturedSide = GAFCustomSelectionWindow.CapturedSide.Left;
			}
			return this.m_TopLeftRect.Contains(_CurrentMousePosition) || this.m_TopRightRect.Contains(_CurrentMousePosition) || this.m_BottomRightRect.Contains(_CurrentMousePosition) || this.m_BottomLeftRect.Contains(_CurrentMousePosition) || this.horizontalResize || this.verticalResize;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005850 File Offset: 0x00003A50
		private void checkRestrictions()
		{
			if (this.m_StartMousePosition.x < this.m_TextureRect.x)
			{
				this.m_StartMousePosition = new Vector2(this.m_TextureRect.x, this.m_StartMousePosition.y);
			}
			if (this.m_StartMousePosition.y < this.m_TextureRect.y)
			{
				this.m_StartMousePosition = new Vector2(this.m_StartMousePosition.x, this.m_TextureRect.y);
			}
			if (this.m_CurrentMousePosition.x > this.m_TextureRect.x + this.m_TextureRect.width)
			{
				this.m_CurrentMousePosition = new Vector2(this.m_TextureRect.x + this.m_TextureRect.width, this.m_CurrentMousePosition.y);
			}
			if (this.m_CurrentMousePosition.y > this.m_TextureRect.y + this.m_TextureRect.height)
			{
				this.m_CurrentMousePosition = new Vector2(this.m_CurrentMousePosition.x, this.m_TextureRect.y + this.m_TextureRect.height);
			}
		}

		// Token: 0x0400003F RID: 63
		private static Texture2D m_Atlas;

		// Token: 0x04000040 RID: 64
		private Vector2 m_ScrollPosition;

		// Token: 0x04000041 RID: 65
		private Vector2 m_WindowSize;

		// Token: 0x04000042 RID: 66
		private Texture2D m_Background;

		// Token: 0x04000043 RID: 67
		private GUIStyle m_SelectionRectStyle;

		// Token: 0x04000044 RID: 68
		private GUIStyle m_BorderStyle;

		// Token: 0x04000045 RID: 69
		private float m_TexturePositionX;

		// Token: 0x04000046 RID: 70
		private float m_TexturePositionY;

		// Token: 0x04000047 RID: 71
		private Vector2 m_StartMousePosition;

		// Token: 0x04000048 RID: 72
		private Vector2 m_CurrentMousePosition;

		// Token: 0x04000049 RID: 73
		private Rect m_SelectionRect;

		// Token: 0x0400004A RID: 74
		private bool m_RectDrawn;

		// Token: 0x0400004B RID: 75
		private GAFCustomSelectionWindow.CapturedSide m_CapturedSide;

		// Token: 0x0400004C RID: 76
		private Rect m_TextureRect;

		// Token: 0x0400004D RID: 77
		private Rect m_TopLeftRect;

		// Token: 0x0400004E RID: 78
		private Rect m_TopRect;

		// Token: 0x0400004F RID: 79
		private Rect m_TopRightRect;

		// Token: 0x04000050 RID: 80
		private Rect m_RightRect;

		// Token: 0x04000051 RID: 81
		private Rect m_BottomRightRect;

		// Token: 0x04000052 RID: 82
		private Rect m_BottomRect;

		// Token: 0x04000053 RID: 83
		private Rect m_BottomLeftRect;

		// Token: 0x04000054 RID: 84
		private Rect m_LeftRect;

		// Token: 0x04000055 RID: 85
		private int m_Left;

		// Token: 0x04000056 RID: 86
		private int m_Top;

		// Token: 0x04000057 RID: 87
		private int m_Right;

		// Token: 0x04000058 RID: 88
		private int m_Bottom;

		// Token: 0x04000059 RID: 89
		private bool m_IsDragging;

		// Token: 0x0400005A RID: 90
		private bool m_OutOfTextureBounds;

		// Token: 0x0400005B RID: 91
		private float m_ResizeRectSize = 6f;

		// Token: 0x0400005C RID: 92
		private Color m_BorderColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x0200001D RID: 29
		private enum CapturedSide
		{
			// Token: 0x0400005E RID: 94
			None,
			// Token: 0x0400005F RID: 95
			TopLeft,
			// Token: 0x04000060 RID: 96
			Top,
			// Token: 0x04000061 RID: 97
			TopRight,
			// Token: 0x04000062 RID: 98
			Right,
			// Token: 0x04000063 RID: 99
			BottomRight,
			// Token: 0x04000064 RID: 100
			Bottom,
			// Token: 0x04000065 RID: 101
			BottomLeft,
			// Token: 0x04000066 RID: 102
			Left
		}
	}
}
