using System.Threading;
using UnityEngine;

namespace GAF.Managed.Utils
{
	// Token: 0x02000002 RID: 2
	public class GAFTextureScale
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Point(Texture2D tex, int newWidth, int newHeight)
		{
			GAFTextureScale.ThreadedScale(tex, newWidth, newHeight, false);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205B File Offset: 0x0000025B
		public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
		{
			GAFTextureScale.ThreadedScale(tex, newWidth, newHeight, true);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
		private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
		{
			GAFTextureScale.texColors = tex.GetPixels();
			GAFTextureScale.newColors = new Color[newWidth * newHeight];
			if (useBilinear)
			{
				GAFTextureScale.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
				GAFTextureScale.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
			}
			else
			{
				GAFTextureScale.ratioX = (float)tex.width / (float)newWidth;
				GAFTextureScale.ratioY = (float)tex.height / (float)newHeight;
			}
			GAFTextureScale.w = tex.width;
			GAFTextureScale.w2 = newWidth;
			var num = Mathf.Min(SystemInfo.processorCount, newHeight);
			var num2 = newHeight / num;
			GAFTextureScale.finishCount = 0;
			if (GAFTextureScale.mutex == null)
			{
				GAFTextureScale.mutex = new Mutex(false);
			}
			if (num > 1)
			{
				int i;
				GAFTextureScale.ThreadData threadData;
				for (i = 0; i < num - 1; i++)
				{
					threadData = new GAFTextureScale.ThreadData(num2 * i, num2 * (i + 1));
					new Thread(useBilinear ? new ParameterizedThreadStart(GAFTextureScale.BilinearScale) : new ParameterizedThreadStart(GAFTextureScale.PointScale)).Start(threadData);
				}
				threadData = new GAFTextureScale.ThreadData(num2 * i, newHeight);
				if (useBilinear)
				{
					GAFTextureScale.BilinearScale(threadData);
				}
				else
				{
					GAFTextureScale.PointScale(threadData);
				}
				while (GAFTextureScale.finishCount < num)
				{
					Thread.Sleep(1);
				}
			}
			else
			{
				var obj = new GAFTextureScale.ThreadData(0, newHeight);
				if (useBilinear)
				{
					GAFTextureScale.BilinearScale(obj);
				}
				else
				{
					GAFTextureScale.PointScale(obj);
				}
			}
			tex.Reinitialize(newWidth, newHeight);
			tex.SetPixels(GAFTextureScale.newColors);
			tex.Apply();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021C4 File Offset: 0x000003C4
		public static void BilinearScale(object obj)
		{
			var threadData = (GAFTextureScale.ThreadData)obj;
			for (var i = threadData.start; i < threadData.end; i++)
			{
				var num = (int)Mathf.Floor((float)i * GAFTextureScale.ratioY);
				var num2 = num * GAFTextureScale.w;
				var num3 = (num + 1) * GAFTextureScale.w;
				var num4 = i * GAFTextureScale.w2;
				for (var j = 0; j < GAFTextureScale.w2; j++)
				{
					var num5 = (int)Mathf.Floor((float)j * GAFTextureScale.ratioX);
					var value = (float)j * GAFTextureScale.ratioX - (float)num5;
					GAFTextureScale.newColors[num4 + j] = GAFTextureScale.ColorLerpUnclamped(GAFTextureScale.ColorLerpUnclamped(GAFTextureScale.texColors[num2 + num5], GAFTextureScale.texColors[num2 + num5 + 1], value), GAFTextureScale.ColorLerpUnclamped(GAFTextureScale.texColors[num3 + num5], GAFTextureScale.texColors[num3 + num5 + 1], value), (float)i * GAFTextureScale.ratioY - (float)num);
				}
			}
			GAFTextureScale.mutex.WaitOne();
			GAFTextureScale.finishCount++;
			GAFTextureScale.mutex.ReleaseMutex();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022E8 File Offset: 0x000004E8
		public static void PointScale(object obj)
		{
			var threadData = (GAFTextureScale.ThreadData)obj;
			for (var i = threadData.start; i < threadData.end; i++)
			{
				var num = (int)(GAFTextureScale.ratioY * (float)i) * GAFTextureScale.w;
				var num2 = i * GAFTextureScale.w2;
				for (var j = 0; j < GAFTextureScale.w2; j++)
				{
					GAFTextureScale.newColors[num2 + j] = GAFTextureScale.texColors[(int)((float)num + GAFTextureScale.ratioX * (float)j)];
				}
			}
			GAFTextureScale.mutex.WaitOne();
			GAFTextureScale.finishCount++;
			GAFTextureScale.mutex.ReleaseMutex();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002384 File Offset: 0x00000584
		private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
		{
			return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
		}

		// Token: 0x04000001 RID: 1
		private static Color[] texColors;

		// Token: 0x04000002 RID: 2
		private static Color[] newColors;

		// Token: 0x04000003 RID: 3
		private static int w;

		// Token: 0x04000004 RID: 4
		private static float ratioX;

		// Token: 0x04000005 RID: 5
		private static float ratioY;

		// Token: 0x04000006 RID: 6
		private static int w2;

		// Token: 0x04000007 RID: 7
		private static int finishCount;

		// Token: 0x04000008 RID: 8
		private static Mutex mutex;

		// Token: 0x02000003 RID: 3
		public class ThreadData
		{
			// Token: 0x06000008 RID: 8 RVA: 0x000023F6 File Offset: 0x000005F6
			public ThreadData(int s, int e)
			{
				this.start = s;
				this.end = e;
			}

			// Token: 0x04000009 RID: 9
			public int start;

			// Token: 0x0400000A RID: 10
			public int end;
		}
	}
}
