using System;
using System.Text;
using UnityEngine;

namespace GAF.Managed.GAFInternal.Utils
{
	// Token: 0x02000020 RID: 32
	public static class GAFUtils
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000051C7 File Offset: 0x000033C7
		public static void Assert(bool _statemant, string _Message)
		{
			if (!_statemant)
			{
				Debug.LogError(_Message);
				Debug.Break();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000051D7 File Offset: 0x000033D7
		public static void Error(string _Message)
		{
			Debug.LogError(_Message);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000051DF File Offset: 0x000033DF
		public static void Warning(string _Message)
		{
			Debug.LogWarning(_Message);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000051E7 File Offset: 0x000033E7
		public static void Log(string _Message, string _AdditionalInfo)
		{
			if (!string.IsNullOrEmpty(_AdditionalInfo))
			{
				_AdditionalInfo = " [" + _AdditionalInfo + "] ";
			}
			Debug.Log(string.Format("GAF{0}- {1}", _AdditionalInfo, _Message));
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005214 File Offset: 0x00003414
		public static string PropertyDump(object variable)
		{
			var properties = variable.GetType().GetProperties();
			var stringBuilder = new StringBuilder();
			foreach (var propertyInfo in properties)
			{
				if (propertyInfo.PropertyType == typeof(string) && propertyInfo.GetGetMethod() != null)
				{
					var name = propertyInfo.Name;
					var arg = propertyInfo.GetGetMethod().Invoke(variable, null);
					var value = string.Format("Name: {0} Value: {1}{2}", name, arg, Environment.NewLine);
					stringBuilder.Append(value);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400006A RID: 106
		public static readonly Vector3 InvalidVector = new Vector3(-2.1474836E+09f, -2.1474836E+09f, -2.1474836E+09f);
	}
}
