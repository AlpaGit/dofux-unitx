using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public class GAFFuncData
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x000052B5 File Offset: 0x000034B5
		public void clear()
		{
			this.m_Target = null;
			this.m_TargetType = "None";
			this.m_Method = "None";
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000052D4 File Offset: 0x000034D4
		public static List<string> getCandidatesList(Type _TargetType, Type _ReturnType, List<Type> _ArgumentTypes)
		{
			var candidates = new List<string>();
			_TargetType.FindMembers(MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public, delegate(MemberInfo member, object criteria)
			{
				MethodInfo method;
				if ((method = _TargetType.GetMethod(member.Name, _ArgumentTypes.ToArray())) != null && method.ReturnType == _ReturnType)
				{
					candidates.Add(method.Name);
					return true;
				}
				return false;
			}, null);
			return candidates;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000532C File Offset: 0x0000352C
		public static List<string> getStaticCandidatesList(Type _TargetType, Type _ReturnType, List<Type> _ArgumentTypes)
		{
			var candidates = new List<string>();
			_TargetType.FindMembers(MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, delegate(MemberInfo member, object criteria)
			{
				MethodInfo method;
				if ((method = _TargetType.GetMethod(member.Name, _ArgumentTypes.ToArray())) != null && method.ReturnType == _ReturnType)
				{
					candidates.Add(method.Name);
					return true;
				}
				return false;
			}, null);
			return candidates;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00005381 File Offset: 0x00003581
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00005389 File Offset: 0x00003589
		public MonoBehaviour target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				this.m_Target = value;
				this.m_TargetType = string.Empty;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000539D File Offset: 0x0000359D
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000053A5 File Offset: 0x000035A5
		public string targetType
		{
			get
			{
				return this.m_TargetType;
			}
			set
			{
				this.m_TargetType = value;
				this.m_Target = null;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000053B5 File Offset: 0x000035B5
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000053BD File Offset: 0x000035BD
		public string method
		{
			get
			{
				return this.m_Method;
			}
			set
			{
				this.m_Method = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000053C6 File Offset: 0x000035C6
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000053CE File Offset: 0x000035CE
		public string returnType
		{
			get
			{
				return this.m_ReturnType;
			}
			set
			{
				this.m_ReturnType = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000053D7 File Offset: 0x000035D7
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000053DF File Offset: 0x000035DF
		public List<string> argumentTypes
		{
			get
			{
				return this.m_ArgumentTypes;
			}
			set
			{
				this.m_ArgumentTypes = value;
			}
		}

		// Token: 0x0400006B RID: 107
		[SerializeField]
		[HideInInspector]
		protected MonoBehaviour m_Target;

		// Token: 0x0400006C RID: 108
		[HideInInspector]
		[SerializeField]
		protected string m_TargetType = "None";

		// Token: 0x0400006D RID: 109
		[HideInInspector]
		[SerializeField]
		protected string m_Method = "None";

		// Token: 0x0400006E RID: 110
		[SerializeField]
		[HideInInspector]
		protected string m_ReturnType = "System.Void";

		// Token: 0x0400006F RID: 111
		[HideInInspector]
		[SerializeField]
		protected List<string> m_ArgumentTypes = new List<string>
		{
			"System.Void"
		};
	}
}
