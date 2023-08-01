using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Attributes
{
	// Token: 0x02000024 RID: 36
	public class GAFFuncAttribute : PropertyAttribute
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000054C7 File Offset: 0x000036C7
		public GAFFuncAttribute(Type _ReturnType, params Type[] _ArgumentTypes)
		{
			this.returnType = _ReturnType;
			this.argumentTypes = _ArgumentTypes.ToList<Type>();
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000054E2 File Offset: 0x000036E2
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x000054EA File Offset: 0x000036EA
		public Type returnType { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000054F3 File Offset: 0x000036F3
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000054FB File Offset: 0x000036FB
		public List<Type> argumentTypes { get; private set; }
	}
}
