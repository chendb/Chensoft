
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chensoft.Extensions
{
    public static class PublicExtensions
    {
  
        public static IList<TEnum> GetEnums<TEnum>()
        {
            var rs = new List<TEnum>();

            var enumType = typeof(TEnum);
            if (enumType.IsNullableType())
            {
                enumType = enumType.GetTypeOfNullable();
                rs.Add(default(TEnum));
            }

            var enums = Enum.GetValues(enumType).Cast<TEnum>();
            rs.AddRange(enums);
            return rs;
        }

        /// <summary>
        /// 判断位域是否为指定的值
        /// </summary>
        public static bool HasFlag(this Enum self, ulong value)
        {
            return (Convert.ToUInt64(self) & value) == value;
        }

        /// <summary>
        /// 判断位域是否为指定的值
        /// </summary>
        public static bool HasFlagX(this Enum self, Enum value)
        {
            return self.HasFlag(Convert.ToUInt64(value));
        }

        /// <summary>
        /// 返回枚举包含的位域项
        /// </summary>
        /// <param name="value">要拆分的枚举。</param>
        /// <param name="distinct">是否去除重复的值，当枚举有包含其它枚举的时候。</param>
        public static IEnumerable<TEnum> GetEnumFlags<TEnum>(this Enum value, bool distinct = false)
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                throw new ArgumentOutOfRangeException(string.Format("类型：{0} 不是枚举类型。", enumType.FullName));

            var values = enumType.GetEnumValues();
            var result = values.Cast<Enum>().Where(value.HasFlagX);

            if (!distinct)
                return result.Cast<TEnum>();

            var orderbyList = result.OrderByDescending(p => p).ToList();
            for (var i = orderbyList.Count - 1; i > 0; i--)
            {
                var item = orderbyList[i];
                if (orderbyList.Any(p => !p.Equals(item) && p.HasFlagX(item)))
                    orderbyList.Remove(item);
            }

            orderbyList.Reverse();
            return orderbyList.Cast<TEnum>();
        }

        /// <summary>
        /// 获取聚合后的枚举值
        /// </summary>
        public static TEnum GetAggregateEnumValue<TEnum>(this IEnumerable<TEnum> source)
            where TEnum : struct
        {
            var value = source.Select(p => Convert.ToInt64(p)).Aggregate(0L, (p, t) => p | t);

            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        /// <summary>
        /// 返回枚举定义的说明 没有则返回 null
        /// </summary>
        public static string Description(this Enum val)
        {
            if (val == null) return null;
            var enumType = val.GetType();
            var text = val.ToString();
            var field = enumType.GetField(text);
            
            if (field == null)
                return val.ToString();

            var desc = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return desc != null ? desc.Description : text;
        }
    }
}
