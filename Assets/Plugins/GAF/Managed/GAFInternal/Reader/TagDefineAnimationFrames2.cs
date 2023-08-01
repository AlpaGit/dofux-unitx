using System;
using System.Collections.Generic;
using System.IO;
using GAF.Managed.GAF.Utils.LinearMath;
using GAF.Managed.GAFInternal.Core;
using GAF.Managed.GAFInternal.Data;
using GAF.Managed.GAFInternal.Utils;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000030 RID: 48
	public class TagDefineAnimationFrames2 : TagBase
	{
		// Token: 0x06000102 RID: 258 RVA: 0x00006354 File Offset: 0x00004554
		public override void Read(TagRecord _Tag, BinaryReader _GAFFileReader, ref GAFAnimationData _SharedData, ref GAFTimelineData _CurrentTimeline)
		{
			var num = _GAFFileReader.ReadUInt32();
			for (var num2 = 0U; num2 < num; num2 += 1U)
			{
				var num3 = _GAFFileReader.ReadUInt32();
				var gafframeData = new GAFFrameData(num3);
				_CurrentTimeline.states[num3] = new List<GAFObjectStateData>();
				var flag = _GAFFileReader.ReadBoolean();
				var flag2 = _GAFFileReader.ReadBoolean();
				if (flag)
				{
					var num4 = _GAFFileReader.ReadUInt32();
					for (var num5 = 0U; num5 < num4; num5 += 1U)
					{
						var item = this.ExctractState(_GAFFileReader, _CurrentTimeline);
						_CurrentTimeline.states[num3].Add(item);
					}
				}
				if (flag2)
				{
					var num6 = _GAFFileReader.ReadUInt32();
					for (var num7 = 0U; num7 < num6; num7 += 1U)
					{
						GAFBaseEvent item2 = null;
						var num8 = _GAFFileReader.ReadUInt32();
						if (num8 >= 0U)
						{
							var actionType = (GAFBaseEvent.ActionType)num8;
							var target = GAFReader.ReadString(_GAFFileReader);
							var num9 = _GAFFileReader.ReadUInt32();
							switch (actionType)
							{
							case GAFBaseEvent.ActionType.Stop:
								item2 = new GAFPlaybackEvent(actionType, target);
								break;
							case GAFBaseEvent.ActionType.Play:
								item2 = new GAFPlaybackEvent(actionType, target);
								break;
							case GAFBaseEvent.ActionType.GotoAndStop:
							case GAFBaseEvent.ActionType.GotoAndPlay:
							{
								var parameter = GAFReader.ReadString(_GAFFileReader);
								this.setGoToEvent(ref item2, parameter, actionType, target);
								break;
							}
							case GAFBaseEvent.ActionType.DispatchEvent:
							{
								var position = _GAFFileReader.BaseStream.Position;
								var list = new List<string>();
								while (_GAFFileReader.BaseStream.Position < position + (long)((ulong)num9))
								{
									list.Add(GAFReader.ReadString(_GAFFileReader));
								}
								if (list[0] == "gafPlaySound")
								{
									var dictionary = Json.Deserialize(list[3]) as Dictionary<string, object>;
									var id = Convert.ToInt32(dictionary["id"]);
									var action = (GAFSoundEvent.SoundAction)Convert.ToInt32(dictionary["action"]);
									var repeat = 0;
									var linkage = string.Empty;
									if (dictionary.ContainsKey("linkage"))
									{
										linkage = dictionary["linkage"].ToString();
									}
									if (dictionary.ContainsKey("repeat"))
									{
										repeat = Convert.ToInt32(dictionary["repeat"]);
									}
									item2 = new GAFSoundEvent(id, action, repeat, linkage, GAFBaseEvent.ActionType.SoundEvent, target);
								}
								else
								{
									item2 = new GAFDispatchEvent(list, actionType, target);
								}
								break;
							}
							}
						}
						gafframeData.events.Add(item2);
					}
				}
				_CurrentTimeline.frames.Add(gafframeData.frameNumber, gafframeData);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000065A0 File Offset: 0x000047A0
		private void setGoToEvent(ref GAFBaseEvent _Action, string _Parameter, GAFBaseEvent.ActionType _ActionType, string _Target)
		{
			uint frameNumber;
			if (!uint.TryParse(_Parameter, out frameNumber))
			{
				_Action = new GAFSetSequenceEvent(_Parameter, _ActionType == GAFBaseEvent.ActionType.GotoAndPlay, _ActionType, _Target);
				return;
			}
			_Action = new GAFGoToEvent(frameNumber, _ActionType, _Target);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000065D4 File Offset: 0x000047D4
		private GAFObjectStateData ExctractState(BinaryReader _Reader, GAFTimelineData _Timeline)
		{
			var flag = Convert.ToBoolean(_Reader.ReadByte());
			var flag2 = Convert.ToBoolean(_Reader.ReadByte());
			var flag3 = Convert.ToBoolean(_Reader.ReadByte());
			var gafobjectStateData = new GAFObjectStateData(_Reader.ReadUInt32());
			var localPosition = gafobjectStateData.localPosition;
			gafobjectStateData.useColorTransform = flag;
			gafobjectStateData.useFilterData = flag3;
			gafobjectStateData.zOrder = -_Reader.ReadInt32() + _Timeline.objects.Count;
			gafobjectStateData.alpha = _Reader.ReadSingle();
			var num = _Reader.ReadSingle();
			var num2 = _Reader.ReadSingle();
			var num3 = _Reader.ReadSingle();
			var num4 = _Reader.ReadSingle();
			localPosition.x = _Reader.ReadSingle();
			localPosition.y = _Reader.ReadSingle();
			gafobjectStateData.localPosition = localPosition;
			var vector  = new Vector2(num, num2);
			var vector2 = new Vector2(num3, num4);
			var   num5    = Mathf.Round(Vector2.Angle(vector, vector2));
			gafobjectStateData.useForceGeometry = !Mathf.Approximately(90f, num5);
			gafobjectStateData.a = num;
			gafobjectStateData.b = -num2;
			gafobjectStateData.c = -num3;
			gafobjectStateData.d = num4;
			var array = new Complex[2, 2];
			array[0, 0] = (double)gafobjectStateData.a;
			array[0, 1] = (double)gafobjectStateData.c;
			array[1, 0] = (double)gafobjectStateData.b;
			array[1, 1] = (double)gafobjectStateData.d;
			Complex[] tau = null;
			Complex[,] array2 = null;
			Complex[,] array3 = null;
			GAFLinearMath.matrixQR(ref array, out tau);
			GAFLinearMath.matrixQRUnpackQ(array, tau, out array2);
			GAFLinearMath.matrixQRUnpackR(array, out array3);
			var identity = Matrix4x4.identity;
			identity[0, 0] = (float)array2[0, 0].x;
			identity[0, 1] = (float)array2[0, 1].x;
			identity[1, 0] = (float)array2[1, 0].x;
			identity[1, 1] = (float)array2[1, 1].x;
			gafobjectStateData.rotation = Quaternion.LookRotation(identity.GetColumn(2), identity.GetColumn(1));
			var num6 = (float)Math.Round((double)(Mathf.Sign(gafobjectStateData.a) * Mathf.Abs((float)array3[0, 0].x)), 4);
			var num7 = (float)Math.Round((double)(Mathf.Sign(gafobjectStateData.d) * Mathf.Abs((float)array3[1, 1].x)), 4);
			gafobjectStateData.scale = new Vector3(num6, num7, 1f);
			if (flag)
			{
				var array4 = new float[4];
				var array5 = new float[4];
				array4[3] = gafobjectStateData.alpha;
				array5[3] = _Reader.ReadSingle();
				array4[0] = _Reader.ReadSingle();
				array5[0] = _Reader.ReadSingle();
				array4[1] = _Reader.ReadSingle();
				array5[1] = _Reader.ReadSingle();
				array4[2] = _Reader.ReadSingle();
				array5[2] = _Reader.ReadSingle();
				var color = new Color(array4[0], array4[1], array4[2], array4[3]);
				var color2 = new Color(array5[0], array5[1], array5[2], array5[3]);
				gafobjectStateData.colorTransformData = new GAFColorTransformData(color, color2);
			}
			if (flag3)
			{
				var b = _Reader.ReadByte();
				for (byte b2 = 0; b2 < b; b2 += 1)
				{
					switch (_Reader.ReadUInt32())
					{
					case 0U:
						gafobjectStateData.filterData = this.readDropShadowData(_Reader);
						break;
					case 1U:
						gafobjectStateData.filterData = this.readBlurData(_Reader);
						break;
					case 2U:
						gafobjectStateData.filterData = this.readGlowData(_Reader);
						break;
					case 6U:
						gafobjectStateData.filterData = this.readColorMatrixData(_Reader);
						break;
					}
				}
			}
			if (flag2)
			{
				gafobjectStateData.maskID = (int)_Reader.ReadUInt32();
			}
			return gafobjectStateData;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000069A4 File Offset: 0x00004BA4
		private GAFFilterData readBlurData(BinaryReader _Reader)
		{
			var blurX = _Reader.ReadSingle();
			var blurY = _Reader.ReadSingle();
			return GAFFilterData.createBlurData(blurX, blurY);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000069C4 File Offset: 0x00004BC4
		private GAFFilterData readGlowData(BinaryReader _Reader)
		{
			var color = _Reader.ReadUInt32();
			var blurX = _Reader.ReadSingle();
			var blurY = _Reader.ReadSingle();
			var strength = _Reader.ReadSingle();
			var inner = _Reader.ReadBoolean();
			var knockout = _Reader.ReadBoolean();
			return GAFFilterData.createGlowData(color, blurX, blurY, strength, inner, knockout);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006A08 File Offset: 0x00004C08
		private GAFFilterData readDropShadowData(BinaryReader _Reader)
		{
			var color = _Reader.ReadUInt32();
			var blurX = _Reader.ReadSingle();
			var blurY = _Reader.ReadSingle();
			var angle = _Reader.ReadSingle();
			var distance = _Reader.ReadSingle();
			var strength = _Reader.ReadSingle();
			var inner = _Reader.ReadBoolean();
			var knockout = _Reader.ReadBoolean();
			return GAFFilterData.createDropShadowData(color, blurX, blurY, angle, distance, strength, inner, knockout);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00006A60 File Offset: 0x00004C60
		private GAFFilterData readColorMatrixData(BinaryReader _Reader)
		{
			var array = new float[20];
			for (var i = 0; i < array.Length; i++)
			{
				array[i] = _Reader.ReadSingle();
			}
			return GAFFilterData.createColorMtxData(array);
		}
	}
}
