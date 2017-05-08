

using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace Chensoft.Common
{
	public static class Convert
	{
		#region ����ת��
		public static T ConvertValue<T>(object value)
		{
			return ConvertValue<T>(value, default(T));
		}

		public static T ConvertValue<T>(object value, T defaultValue)
		{
			return (T)ConvertValue(value, typeof(T), () => defaultValue);
		}

		public static object ConvertValue(object value, Type conversionType)
		{
			return ConvertValue(value, conversionType, () => GetDefaultValue(conversionType));
		}

		public static object ConvertValue(object value, Type conversionType, object defaultValue)
		{
			return ConvertValue(value, conversionType, () => defaultValue);
		}

		public static object ConvertValue(object value, Type conversionType, Func<object> getDefaultValue)
		{
			if(conversionType == null)
				return value;

			if(value == null || System.Convert.IsDBNull(value))
			{
				if(conversionType == typeof(DBNull))
					return DBNull.Value;
				else
					return getDefaultValue();
			}

			Type type = conversionType;

			if(conversionType.IsGenericType && (!conversionType.IsGenericTypeDefinition) && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
				type = conversionType.GetGenericArguments()[0];

			if(type == value.GetType() || type.IsAssignableFrom(value.GetType()))
				return value;

			try
			{
				if(type == typeof(Encoding))
				{
					if(value == null)
						return getDefaultValue();

					if(value.GetType() == typeof(string))
					{
						switch(((string)value).ToLowerInvariant())
						{
							case "utf8":
							case "utf-8":
								return Encoding.UTF8;
							case "utf7":
							case "utf-7":
								return Encoding.UTF7;
							case "utf32":
								return Encoding.UTF32;
							case "unicode":
								return Encoding.Unicode;
							case "ascii":
								return Encoding.ASCII;
							case "bigend":
							case "bigendian":
								return Encoding.BigEndianUnicode;
							default:
								try
								{
									return Encoding.GetEncoding((string)value);
								}
								catch
								{
									return getDefaultValue();
								}
						}
					}
					else
					{
						switch(Type.GetTypeCode(value.GetType()))
						{
							case TypeCode.Byte:
							case TypeCode.Decimal:
							case TypeCode.Double:
							case TypeCode.Int16:
							case TypeCode.Int32:
							case TypeCode.Int64:
							case TypeCode.SByte:
							case TypeCode.Single:
							case TypeCode.UInt16:
							case TypeCode.UInt32:
							case TypeCode.UInt64:
								return Encoding.GetEncoding((int)System.Convert.ChangeType(value, typeof(int)));
						}
					}
				}

				//��ʼ���ض�����ת������ӳ��
				InitializeTypeConverters();

				TypeConverter converter = TypeDescriptor.GetConverter(type);

				if(converter != null && converter.CanConvertFrom(value.GetType()))
					return converter.ConvertFrom(value);

				return System.Convert.ChangeType(value, type);
			}
			catch
			{
				return getDefaultValue();
			}
		}

		public static bool TryConvertValue<T>(object value, out T result)
		{
			bool b = true;

			result = (T)ConvertValue(value, typeof(T), () =>
			{
				b = false;
				return default(T);
			});

			return b;
		}

		public static bool TryConvertValue(object value, Type conversionType, out object result)
		{
			result = ConvertValue(value, conversionType, () => typeof(Convert));

			if(result == typeof(Convert))
			{
				result = null;
				return false;
			}

			return true;
		}
		#endregion

		#region ת��ӳ��
		private static int _initialized;
		private static void InitializeTypeConverters()
		{
			var initialized = System.Threading.Interlocked.CompareExchange(ref _initialized, 1, 0);

			if(initialized == 0)
			{
				//TypeDescriptor.AddAttributes(typeof(System.Enum), new Attribute[] { new TypeConverterAttribute(typeof(Chensoft.ComponentModel.EnumConverter)) });
				TypeDescriptor.AddAttributes(typeof(System.Guid), new Attribute[] { new TypeConverterAttribute(typeof(Chensoft.ComponentModel.GuidConverter)) });
				TypeDescriptor.AddAttributes(typeof(System.Net.IPEndPoint), new Attribute[] { new TypeConverterAttribute(typeof(Chensoft.Communication.IPEndPointConverter)) });
			}
		}
		#endregion

		#region ȡĬ��ֵ
		public static object GetDefaultValue(Type type)
		{
			if(type == typeof(DBNull))
				return DBNull.Value;

			if(type == null || type.IsClass || type.IsInterface)
				return null;

			if(type.IsEnum)
			{
				var attributes = type.GetCustomAttributes(typeof(DefaultValueAttribute), true);

				if(attributes.Length > 0)
				{
					return ((DefaultValueAttribute)attributes[0]).Value;
				}
				else
				{
					Array values = Enum.GetValues(type);

					if(values.Length > 0)
						return values.GetValue(0);
				}
			}

			return Activator.CreateInstance(type);
		}
		#endregion

		#region ��ֵ�ж�
		/// <summary>
		/// �ж�ָ����ֵ�Ƿ�Ϊ�ջ���DBNull��������򷵻���(True)�����򷵻ؼ�(False)��
		/// </summary>
		/// <param name="value">Ҫ�жϵ�ֵ��</param>
		/// <returns>���صĽ����</returns>
		public static bool IsNullOrDBNull(object value)
		{
			return (value == null || System.Convert.IsDBNull(value));
		}

		/// <summary>
		/// �ж�ָ����ֵ�Ƿ�Ϊ�ջ���DBNull��������򷵻�ָ���������͵�Ĭ��ֵ�����򷵻ز�������
		/// </summary>
		/// <typeparam name="T">ָ���Ĳ����ķ��͡�</typeparam>
		/// <param name="value">ָ���Ĳ���ֵ��</param>
		/// <returns>���صĽ����</returns>
		public static T IsNullOrDBNull<T>(object value)
		{
			return IsNullOrDBNull<T>(value, default(T));
		}

		/// <summary>
		/// �ж�ָ����ֵ�Ƿ�Ϊ�ջ���DBNull��������򷵻�ָ����Ĭ��ֵ�����򷵻ز�������
		/// </summary>
		/// <typeparam name="T">ָ���Ĳ����ķ��͡�</typeparam>
		/// <param name="value">ָ���Ĳ���ֵ��</param>
		/// <param name="defaultValue">Ĭ��ֵ��</param>
		/// <returns>����ֵ��Ĭ��ֵ��</returns>
		public static T IsNullOrDBNull<T>(object value, T defaultValue)
		{
			if(value == null || System.Convert.IsDBNull(value))
				return defaultValue;

			try
			{
				return (T)value;
			}
			catch
			{
			}

			try
			{
				return (T)System.Convert.ChangeType(value, typeof(T));
			}
			catch
			{
			}

			return defaultValue;
		}
		#endregion

		#region �ֽ��ı�
		/// <summary>
		/// ��ָ�����ֽ�����ת��Ϊ����ʮ���������ֱ���ĵ�Ч�ַ�����ʾ��ʽ��
		/// </summary>
		/// <param name="buffer">һ�� 8 λ�޷����ֽ����顣</param>
		/// <returns>������Ԫ�ص��ַ�����ʾ��ʽ����ʮ�������ı���ʾ��</returns>
		public static string ToHexString(byte[] buffer)
		{
			return ToHexString(buffer, '\0');
		}

		/// <summary>
		/// ��ָ�����ֽ�����ת��Ϊ����ʮ���������ֱ���ĵ�Ч�ַ�����ʾ��ʽ������ָ���Ƿ��ڷ���ֵ�в���ָ�����
		/// </summary>
		/// <param name="buffer">һ�� 8 λ�޷����ֽ����顣</param>
		/// <param name="separator">ÿ�ֽڶ�Ӧ��ʮ�������ı��м�ķָ�����</param>
		/// <returns>������Ԫ�ص��ַ�����ʾ��ʽ����ʮ�������ı���ʾ��</returns>
		public static string ToHexString(byte[] buffer, char separator)
		{
			if(buffer == null || buffer.Length < 1)
				return string.Empty;

			StringBuilder builder = new StringBuilder(buffer.Length * 2);

			for(int i = 0; i < buffer.Length; i++)
			{
				builder.AppendFormat("{0:X2}", buffer[i]);

				if(separator != '\0' && i < buffer.Length - 1)
					builder.Append(separator);
			}

			return builder.ToString();
		}

		/// <summary>
		/// ��ָ����ʮ�����Ƹ�ʽ���ַ���ת��Ϊ��Ч���ֽ����顣
		/// </summary>
		/// <param name="text">Ҫת����ʮ�����Ƹ�ʽ���ַ�����</param>
		/// <returns>��<paramref name="text"/>��Ч���ֽ����顣</returns>
		/// <exception cref="System.FormatException"><paramref name="text"/>�����к��зǿհ��ַ���</exception>
		/// <remarks>�÷�����ʵ��ʼ�պ���<paramref name="text"/>�����еĿհ��ַ���</remarks>
		public static byte[] FromHexString(string text)
		{
			return FromHexString(text, '\0', true);
		}

		/// <summary>
		/// ��ָ����ʮ�����Ƹ�ʽ���ַ���ת��Ϊ��Ч���ֽ����顣
		/// </summary>
		/// <param name="text">Ҫת����ʮ�����Ƹ�ʽ���ַ�����</param>
		/// <param name="separator">Ҫ���˵��ķָ����ַ���</param>
		/// <returns>��<paramref name="text"/>��Ч���ֽ����顣</returns>
		/// <exception cref="System.FormatException"><paramref name="text"/>�����к��зǿհ��ַ����ָ���ķָ�����</exception>
		/// <remarks>�÷�����ʵ��ʼ�պ���<paramref name="text"/>�����еĿհ��ַ���</remarks>
		public static byte[] FromHexString(string text, char separator)
		{
			return FromHexString(text, separator, true);
		}

		/// <summary>
		/// ��ָ����ʮ�����Ƹ�ʽ���ַ���ת��Ϊ��Ч���ֽ����顣
		/// </summary>
		/// <param name="text">Ҫת����ʮ�����Ƹ�ʽ���ַ�����</param>
		/// <param name="separator">Ҫ���˵��ķָ����ַ���</param>
		/// <param name="throwExceptionOnFormat">ָ���������ı��к��зǷ��ַ�ʱ�Ƿ��׳�<seealso cref="System.FormatException"/>�쳣��</param>
		/// <returns>��<paramref name="text"/>��Ч���ֽ����顣</returns>
		/// <exception cref="System.FormatException">��<param name="throwExceptionOnFormat"����Ϊ�棬����<paramref name="text"/>�����к��зǿհ��ַ����ָ���ķָ�����</exception>
		/// <remarks>�÷�����ʵ��ʼ�պ���<paramref name="text"/>�����еĿհ��ַ���</remarks>
		public static byte[] FromHexString(string text, char separator, bool throwExceptionOnFormat)
		{
			if(string.IsNullOrEmpty(text))
				return new byte[0];

			int index = 0;
			char[] buffer = new char[2];
			List<byte> result = new List<byte>();

			foreach(char character in text)
			{
				if(char.IsWhiteSpace(character) || character == separator)
					continue;

				buffer[index++] = character;
				if(index == buffer.Length)
				{
					index = 0;
					byte value = 0;

					if(TryParseHex(buffer, out value))
						result.Add(value);
					else
					{
						if(throwExceptionOnFormat)
							throw new FormatException();
						else
							return new byte[0];
					}
				}
			}

			return result.ToArray();
		}

		public static bool TryParseHex(char[] characters, out byte value)
		{
			long number;

			if(TryParseHex(characters, out number))
			{
				if(number >= byte.MinValue && number <= byte.MaxValue)
				{
					value = (byte)number;
					return true;
				}
			}

			value = 0;
			return false;
		}

		public static bool TryParseHex(char[] characters, out short value)
		{
			long number;

			if(TryParseHex(characters, out number))
			{
				if(number >= short.MinValue && number <= short.MaxValue)
				{
					value = (short)number;
					return true;
				}
			}

			value = 0;
			return false;
		}

		public static bool TryParseHex(char[] characters, out int value)
		{
			long number;

			if(TryParseHex(characters, out number))
			{
				if(number >= int.MinValue && number <= int.MaxValue)
				{
					value = (int)number;
					return true;
				}
			}

			value = 0;
			return false;
		}

		public static bool TryParseHex(char[] characters, out long value)
		{
			value = 0;

			if(characters == null)
				return false;

			int count = 0;
			byte[] digits = new byte[characters.Length];

			foreach(char character in characters)
			{
				if(char.IsWhiteSpace(character))
					continue;

				if(character >= '0' && character <= '9')
					digits[count++] = (byte)(character - '0');
				else if(character >= 'A' && character <= 'F')
					digits[count++] = (byte)((character - 'A') + 10);
				else if(character >= 'a' && character <= 'f')
					digits[count++] = (byte)((character - 'a') + 10);
				else
					return false;
			}

			long number = 0;

			if(count > 0)
			{
				for(int i = 0; i < count; i++)
				{
					number += digits[i] * (long)Math.Pow(16, count - i - 1);
				}
			}

			value = number;
			return true;
		}
		#endregion

		#region �������

		#region Ĭ�Ͻ���
		private static readonly Action<ObjectResolvingContext> DefaultResolve = (ctx) =>
		{
			if(ctx.Container == null)
				return;

			if(ctx.Direction == ObjectResolvingDirection.Get)
			{
				var member = GetMember(ctx.Container.GetType(), ctx.Name, (BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty), true);

				if(member == null)
					throw new ArgumentException("Invalid path of target object.");

				switch(member.MemberType)
				{
					case MemberTypes.Field:
						ctx.Value = ((FieldInfo)member).GetValue(ctx.Container);
						break;
					case MemberTypes.Property:
						ctx.Value = ((PropertyInfo)member).GetValue(ctx.Container, null);
						break;
				}

				ctx.Handled = true;
			}
			else if(ctx.Direction == ObjectResolvingDirection.Set)
			{
				var value = ctx.Value;
				var member = GetMember(ctx.Container.GetType(), ctx.Name, (BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty), true);

				if(member == null)
					throw new ArgumentException("Invalid path of target object.");

				switch(member.MemberType)
				{
					case MemberTypes.Field:
						value = Chensoft.Common.Convert.ConvertValue(ctx.Value, ((FieldInfo)member).FieldType);
						((FieldInfo)member).SetValue(ctx.Container, value);
						break;
					case MemberTypes.Property:
						value = Chensoft.Common.Convert.ConvertValue(ctx.Value, ((PropertyInfo)member).PropertyType);
						((PropertyInfo)member).SetValue(ctx.Container, value, null);
						break;
				}

				ctx.Handled = true;
			}
		};
		#endregion

		#region ��ȡ����
		public static object GetValue(object target, string path)
		{
			if(target == null || path == null || path.Length < 1)
				return target;

			return GetValue(target, path.Split('.'), null);
		}

		public static object GetValue(object target, string path, Action<ObjectResolvingContext> resolve)
		{
			if(target == null || path == null || path.Length < 1)
				return target;

			return GetValue(target, path.Split('.'), resolve);
		}

		public static object GetValue(object target, string[] memberNames)
		{
			if(target == null || memberNames == null || memberNames.Length < 1)
				return target;

			return GetValue(target, memberNames, 0, memberNames.Length, null);
		}

		public static object GetValue(object target, string[] memberNames, Action<ObjectResolvingContext> resolve)
		{
			if(target == null || memberNames == null || memberNames.Length < 1)
				return target;

			return GetValue(target, memberNames, 0, memberNames.Length, resolve);
		}

		public static object GetValue(object target, string[] memberNames, int start, int length, Action<ObjectResolvingContext> resolve)
		{
			if(target == null || memberNames == null || memberNames.Length < 1)
				return target;

			if(start < 0 || start >= memberNames.Length)
				throw new ArgumentOutOfRangeException("start");

			//�������������Ķ���
			ObjectResolvingContext context = new ObjectResolvingContext(target, string.Join(".", memberNames));

			for(int i = 0; i < Math.Min(memberNames.Length - start, length); i++)
			{
				string memberName = memberNames[start + i];

				if(memberName == null || memberName.Trim().Length < 1)
					continue;

				if(context.Value == null)
					continue;

				context.Handled = false;
				context.Name = memberName;
				context.Container = context.Value;

				//���������Ա
				if(resolve == null)
					DefaultResolve(context);
				else
				{
					resolve(context);

					if(!context.Handled)
						DefaultResolve(context);
				}
			}

			return context.Value;
		}
		#endregion

		#region ���÷���
		public static void SetValue(object target, string path, object value)
		{
			SetValue(target, path, value, null);
		}

		public static void SetValue(object target, string path, object value, Action<ObjectResolvingContext> resolve)
		{
			if(target == null || path == null || path.Length < 1)
				return;

			SetValue(target, path.Split('.'), value, resolve);
		}

		public static void SetValue(object target, string[] memberNames, object value)
		{
			SetValue(target, memberNames, value, null);
		}

		public static void SetValue(object target, string[] memberNames, object value, Action<ObjectResolvingContext> resolve)
		{
			if(target == null || memberNames == null || memberNames.Length < 1)
				return;

			object container = target;

			if(memberNames.Length > 1)
			{
				container = GetValue(target, memberNames, 0, memberNames.Length - 1, resolve);

				if(container == null)
					throw new InvalidOperationException(string.Format("The '{0}' member is not exists in object of '{1}' type.", string.Join(".", memberNames), target.GetType().FullName));
			}

			//�����������������Ķ���
			var context = new ObjectResolvingContext(target, container, memberNames[memberNames.Length - 1], value, string.Join(".", memberNames));

			//���ý����ص�����
			if(resolve == null)
				DefaultResolve(context);
			else
			{
				resolve(context);

				if(!context.Handled)
					DefaultResolve(context);
			}
		}
		#endregion

		#region ˽�з���
		private static MemberInfo GetMember(Type type, string name, BindingFlags binding, bool ignoreCase)
		{
			if(type == null || string.IsNullOrWhiteSpace(name))
				return null;

			var members = type.FindMembers((MemberTypes.Field | MemberTypes.Property),
								(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty),
								(member, criteria) =>
								{
									return string.Equals((string)criteria, member.Name,
														 (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
								},
								name);

			if(members != null && members.Length > 0)
				return members[0];

			return null;
		}
		#endregion

		#region Ƕ������
		/// <summary>
		/// ��ʾ�����Ա�Ľ�������
		/// </summary>
		public enum ObjectResolvingDirection
		{
			/// <summary>��ȡ����ĳ�Աֵ��</summary>
			Get,
			/// <summary>���ö���ĳ�Աֵ��</summary>
			Set,
		}

		/// <summary>
		/// ��ʾ�ڶ����Ա���������еĲ��������ġ�
		/// </summary>
		public class ObjectResolvingContext : MarshalByRefObject
		{
			#region ��Ա�ֶ�
			private ObjectResolvingDirection _direction;
			private object _target;
			private object _container;
			private string _path;
			private object _value;
			private string _name;
			private bool _handled;
			#endregion

			#region ���캯��
			internal ObjectResolvingContext(object target, string path)
			{
				if(target == null)
					throw new ArgumentNullException("target");

				if(string.IsNullOrWhiteSpace(path))
					throw new ArgumentNullException("path");

				_direction = ObjectResolvingDirection.Get;
				_target = target;
				_container = target;
				_value = target;
				_path = path;
			}

			internal ObjectResolvingContext(object target, object container, string name, object value, string path)
			{
				if(target == null)
					throw new ArgumentNullException("target");

				if(string.IsNullOrWhiteSpace(path))
					throw new ArgumentNullException("path");

				_direction = ObjectResolvingDirection.Set;
				_target = target;
				_container = container;
				_name = name;
				_value = value;
				_path = path;
			}
			#endregion

			#region ��������
			/// <summary>
			/// ��ȡ���������е�ǰ����ķ���
			/// </summary>
			public ObjectResolvingDirection Direction
			{
				get
				{
					return _direction;
				}
			}

			/// <summary>
			/// ��ȡ���������Ŀ�������
			/// </summary>
			public object Target
			{
				get
				{
					return _target;
				}
			}

			/// <summary>
			/// ��ȡ���������е�ǰ��Ա����������
			/// </summary>
			public object Container
			{
				get
				{
					return _container;
				}
				internal set
				{
					_container = value;
				}
			}

			/// <summary>
			/// ��ȡ������������Ա·��������һ���ԡ�.���ָ����ַ�����
			/// </summary>
			public string Path
			{
				get
				{
					return _path;
				}
			}

			/// <summary>
			/// ��ȡ������һ��������ֵ���������ڲ�ͬ����������ʾ�ĺ���Ϳ������Ծ���ͬ��������ο���ע��
			/// </summary>
			/// <remarks>
			///		<para>��<see cref="Direction"/>����ֵ����<seealso cref="ObjectResolvingDirection.Get"/>ʱ�������Կ����ã���ʾ������������������ĳ�Աֵ��</para>
			///		<para>��<see cref="Direction"/>����ֵ����<seealso cref="ObjectResolvingDirection.Set"/>ʱ�������Բ������ã���ʾ�����û�ָ��Ҫ���õ�Ŀ��ֵ��</para>
			/// </remarks>
			/// <exception cref="System.InvalidOperationException">��<see cref="Direction"/>����ֵ������<seealso cref="ObjectResolvingDirection.Get"/>ʱ������</exception>
			public object Value
			{
				get
				{
					return _value;
				}
				set
				{
					if(_direction != ObjectResolvingDirection.Get)
						throw new InvalidOperationException();

					_value = value;
				}
			}

			/// <summary>
			/// ��ȡ��ǰ�����ĳ�Ա���ơ�
			/// </summary>
			public string Name
			{
				get
				{
					return _name;
				}
				internal set
				{
					_name = value;
				}
			}

			/// <summary>
			/// ��ȡ�����ô�����ɱ�ǡ�
			/// </summary>
			/// <remarks>
			///		<para>������ø�����Ϊ��(true)����ʾ�Զ�����������Ѿ���ɶԵ�ǰ��Ա�Ľ��������ʾ��֪ϵͳ��Ҫ�ٶԵ�ǰ��Ա�Ľ��н��������ˣ�</para>
			///		<para>������ø�����Ϊ��(false)����Ĭ��ֵ����ʾ�Զ����Զ����������δ�Ե�ǰ��Ա���н���������ζ����ϵͳ�Ե�ǰ��Ա���н�������</para>
			/// </remarks>
			public bool Handled
			{
				get
				{
					return _handled;
				}
				set
				{
					_handled = value;
				}
			}
			#endregion
		}
		#endregion

		#endregion
	}
}
