using System;
using System.IO;

namespace GAF.Managed.GAFInternal.Reader
{
	// Token: 0x02000029 RID: 41
	public class GAFHeader
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00005617 File Offset: 0x00003817
		public void Read(BinaryReader _Reader)
		{
			this.m_Compression = (GAFHeader.CompressionType)_Reader.ReadInt32();
			if (this.isValid)
			{
				this.m_MajorVersion = _Reader.ReadByte();
				this.m_MinorVersion = _Reader.ReadByte();
				this.m_FileLength = (int)_Reader.ReadUInt32();
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005651 File Offset: 0x00003851
		public bool isValid
		{
			get
			{
				return GAFHeader.isCorrectHeader(this.m_Compression);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000565E File Offset: 0x0000385E
		public GAFHeader.CompressionType compression
		{
			get
			{
				return this.m_Compression;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005666 File Offset: 0x00003866
		public ushort majorVersion
		{
			get
			{
				return Convert.ToUInt16(this.m_MajorVersion);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005673 File Offset: 0x00003873
		public ushort minorVersion
		{
			get
			{
				return Convert.ToUInt16(this.m_MinorVersion);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005680 File Offset: 0x00003880
		public uint fileLength
		{
			get
			{
				return (uint)this.m_FileLength;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005688 File Offset: 0x00003888
		public static int headerDataOffset
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000568C File Offset: 0x0000388C
		public static bool isCorrectHeader(GAFHeader.CompressionType _Compression)
		{
			return Enum.IsDefined(typeof(GAFHeader.CompressionType), _Compression);
		}

		// Token: 0x04000084 RID: 132
		private GAFHeader.CompressionType m_Compression;

		// Token: 0x04000085 RID: 133
		private int m_FileLength;

		// Token: 0x04000086 RID: 134
		private byte m_MajorVersion;

		// Token: 0x04000087 RID: 135
		private byte m_MinorVersion;

		// Token: 0x0200002A RID: 42
		public enum CompressionType
		{
			// Token: 0x04000089 RID: 137
			CompressedNone = 4669766,
			// Token: 0x0400008A RID: 138
			CompressedZip = 4669763
		}
	}
}
