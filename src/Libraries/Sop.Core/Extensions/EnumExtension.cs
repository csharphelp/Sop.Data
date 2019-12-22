using System;
using System.Linq;

namespace Sop.Data.Extensions
{
    /// <summary>
    /// 枚举类扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举项上设置的显示文字
        /// </summary>
        /// <param name="value">被扩展对象</param>
        public static string EnumMetadataDisplay(this Enum value)
        {
            var attribute = value.GetType().GetField(Enum.GetName(value.GetType(), value)).GetCustomAttributes(
                 typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                 .Cast<System.ComponentModel.DataAnnotations.DisplayAttribute>()
                 .FirstOrDefault();
            if (attribute != null)
            {
                return attribute.Name;
            }

            return Enum.GetName(value.GetType(), value);
        }
    }
}