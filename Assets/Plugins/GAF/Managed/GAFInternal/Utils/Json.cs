using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GAF.Managed.GAFInternal.Utils
{
	// Token: 0x0200001C RID: 28
	public static class Json
	{
		/// <summary>
		/// Parses the string json into a value
		/// </summary>
		/// <param name="json">A JSON string.</param>
		/// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
		// Token: 0x06000097 RID: 151 RVA: 0x0000489C File Offset: 0x00002A9C
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		/// <summary>
		/// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
		/// </summary>
		/// <param name="obj">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
		/// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
		// Token: 0x06000098 RID: 152 RVA: 0x000048A9 File Offset: 0x00002AA9
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x0200001D RID: 29
		private sealed class Parser : IDisposable
		{
			// Token: 0x06000099 RID: 153 RVA: 0x000048B1 File Offset: 0x00002AB1
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x0600009A RID: 154 RVA: 0x000048CE File Offset: 0x00002ACE
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x0600009B RID: 155 RVA: 0x000048E4 File Offset: 0x00002AE4
			public static object Parse(string jsonString)
			{
				object result;
				using (var parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x0600009C RID: 156 RVA: 0x0000491C File Offset: 0x00002B1C
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x0600009D RID: 157 RVA: 0x00004930 File Offset: 0x00002B30
			private Dictionary<string, object> ParseObject()
			{
				var dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					var nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						break;
					}
					if (nextToken == Json.Parser.TOKEN.CURLY_CLOSE)
					{
						return dictionary;
					}
					if (nextToken != Json.Parser.TOKEN.COMMA)
					{
						var text = this.ParseString();
						if (text == null)
						{
							goto Block_4;
						}
						if (this.NextToken != Json.Parser.TOKEN.COLON)
						{
							goto Block_5;
						}
						this.json.Read();
						dictionary[text] = this.ParseValue();
					}
				}
				return null;
				Block_4:
				return null;
				Block_5:
				return null;
			}

			// Token: 0x0600009E RID: 158 RVA: 0x00004998 File Offset: 0x00002B98
			private List<object> ParseArray()
			{
				var list = new List<object>();
				this.json.Read();
				var flag = true;
				while (flag)
				{
					var nextToken = this.NextToken;
					if (nextToken == Json.Parser.TOKEN.NONE)
					{
						return null;
					}
					if (nextToken != Json.Parser.TOKEN.SQUARED_CLOSE)
					{
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							var item = this.ParseByToken(nextToken);
							list.Add(item);
						}
					}
					else
					{
						flag = false;
					}
				}
				return list;
			}

			// Token: 0x0600009F RID: 159 RVA: 0x000049E8 File Offset: 0x00002BE8
			private object ParseValue()
			{
				var nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060000A0 RID: 160 RVA: 0x00004A04 File Offset: 0x00002C04
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x060000A1 RID: 161 RVA: 0x00004A74 File Offset: 0x00002C74
			private string ParseString()
			{
				var stringBuilder = new StringBuilder();
				this.json.Read();
				var flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					var nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							if (nextChar <= '\\')
							{
								if (nextChar == '"' || nextChar == '/' || nextChar == '\\')
								{
									stringBuilder.Append(nextChar);
								}
							}
							else if (nextChar <= 'f')
							{
								if (nextChar != 'b')
								{
									if (nextChar == 'f')
									{
										stringBuilder.Append('\f');
									}
								}
								else
								{
									stringBuilder.Append('\b');
								}
							}
							else if (nextChar != 'n')
							{
								switch (nextChar)
								{
								case 'r':
									stringBuilder.Append('\r');
									break;
								case 't':
									stringBuilder.Append('\t');
									break;
								case 'u':
								{
									var array = new char[4];
									for (var i = 0; i < 4; i++)
									{
										array[i] = this.NextChar;
									}
									stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
									break;
								}
								}
							}
							else
							{
								stringBuilder.Append('\n');
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060000A2 RID: 162 RVA: 0x00004BC4 File Offset: 0x00002DC4
			private object ParseNumber()
			{
				var nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x060000A3 RID: 163 RVA: 0x00004C02 File Offset: 0x00002E02
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004C2D File Offset: 0x00002E2D
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000A5 RID: 165 RVA: 0x00004C3F File Offset: 0x00002E3F
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004C54 File Offset: 0x00002E54
			private string NextWord
			{
				get
				{
					var stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000A7 RID: 167 RVA: 0x00004C98 File Offset: 0x00002E98
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					var peekChar = this.PeekChar;
					if (peekChar <= '[')
					{
						switch (peekChar)
						{
						case '"':
							return Json.Parser.TOKEN.STRING;
						case '#':
						case '$':
						case '%':
						case '&':
						case '\'':
						case '(':
						case ')':
						case '*':
						case '+':
						case '.':
						case '/':
							break;
						case ',':
							this.json.Read();
							return Json.Parser.TOKEN.COMMA;
						case '-':
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
							return Json.Parser.TOKEN.NUMBER;
						case ':':
							return Json.Parser.TOKEN.COLON;
						default:
							if (peekChar == '[')
							{
								return Json.Parser.TOKEN.SQUARED_OPEN;
							}
							break;
						}
					}
					else
					{
						if (peekChar == ']')
						{
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						if (peekChar == '{')
						{
							return Json.Parser.TOKEN.CURLY_OPEN;
						}
						if (peekChar == '}')
						{
							this.json.Read();
							return Json.Parser.TOKEN.CURLY_CLOSE;
						}
					}
					var nextWord = this.NextWord;
					if (nextWord == "false")
					{
						return Json.Parser.TOKEN.FALSE;
					}
					if (nextWord == "true")
					{
						return Json.Parser.TOKEN.TRUE;
					}
					if (!(nextWord == "null"))
					{
						return Json.Parser.TOKEN.NONE;
					}
					return Json.Parser.TOKEN.NULL;
				}
			}

			// Token: 0x0400005A RID: 90
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x0400005B RID: 91
			private StringReader json;

			// Token: 0x0200001E RID: 30
			private enum TOKEN
			{
				// Token: 0x0400005D RID: 93
				NONE,
				// Token: 0x0400005E RID: 94
				CURLY_OPEN,
				// Token: 0x0400005F RID: 95
				CURLY_CLOSE,
				// Token: 0x04000060 RID: 96
				SQUARED_OPEN,
				// Token: 0x04000061 RID: 97
				SQUARED_CLOSE,
				// Token: 0x04000062 RID: 98
				COLON,
				// Token: 0x04000063 RID: 99
				COMMA,
				// Token: 0x04000064 RID: 100
				STRING,
				// Token: 0x04000065 RID: 101
				NUMBER,
				// Token: 0x04000066 RID: 102
				TRUE,
				// Token: 0x04000067 RID: 103
				FALSE,
				// Token: 0x04000068 RID: 104
				NULL
			}
		}

		// Token: 0x0200001F RID: 31
		private sealed class Serializer
		{
			// Token: 0x060000A8 RID: 168 RVA: 0x00004DBA File Offset: 0x00002FBA
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00004DCD File Offset: 0x00002FCD
			public static string Serialize(object obj)
			{
				var serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x060000AA RID: 170 RVA: 0x00004DE8 File Offset: 0x00002FE8
			private void SerializeValue(object value)
			{
				if (value == null)
				{
					this.builder.Append("null");
					return;
				}
				string str;
				if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
					return;
				}
				if (value is bool)
				{
					this.builder.Append(((bool)value) ? "true" : "false");
					return;
				}
				IList anArray;
				if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
					return;
				}
				IDictionary obj;
				if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
					return;
				}
				if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
					return;
				}
				this.SerializeOther(value);
			}

			// Token: 0x060000AB RID: 171 RVA: 0x00004E8C File Offset: 0x0000308C
			private void SerializeObject(IDictionary obj)
			{
				var flag = true;
				this.builder.Append('{');
				foreach (var obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x060000AC RID: 172 RVA: 0x00004F34 File Offset: 0x00003134
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				var flag = true;
				foreach (var value in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00004FB4 File Offset: 0x000031B4
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				var array = str.ToCharArray();
				var i = 0;
				while (i < array.Length)
				{
					var c = array[i];
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					case '\v':
						goto IL_E0;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_E0;
							}
							this.builder.Append("\\\\");
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					}
					IL_129:
					i++;
					continue;
					IL_E0:
					var num = Convert.ToInt32(c);
					if (num >= 32 && num <= 126)
					{
						this.builder.Append(c);
						goto IL_129;
					}
					this.builder.Append("\\u");
					this.builder.Append(num.ToString("x4"));
					goto IL_129;
				}
				this.builder.Append('"');
			}

			// Token: 0x060000AE RID: 174 RVA: 0x00005108 File Offset: 0x00003308
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
					return;
				}
				if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
					return;
				}
				if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
					return;
				}
				this.SerializeString(value.ToString());
			}

			// Token: 0x04000069 RID: 105
			private StringBuilder builder;
		}
	}
}
