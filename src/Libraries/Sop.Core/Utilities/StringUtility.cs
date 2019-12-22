using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Sop.Core.Utilities
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class StringUtility
    {
        #region String length formatter

        /// <summary>
        /// 对字符串进行截字，截去字的部分用"..."替代
        /// </summary>
        /// <remarks>
        /// 一个字符指双字节字符，单字节字符仅算半个字符
        /// </remarks>
        /// <param name="rawString">待截字的字符串</param>
        /// <param name="charLimit">截字的长度，按双字节计数</param>
        /// <returns>截字后的字符串</returns>
        public static string Trim(string rawString, int charLimit)
        {
            return Trim(rawString, charLimit, "...");
        }

        /// <summary>
        /// 对字符串进行截字(区分单字节及双字节字符)
        /// </summary>
        /// <remarks>
        /// 一个字符指双字节字符，单字节字符仅算半个字符
        /// </remarks>
        /// <param name="rawString">待截字的字符串</param>
        /// <param name="charLimit">截字的长度，按双字节计数</param>
        /// <param name="appendString">截去字的部分用替代字符串</param>
        /// <returns>截字后的字符串</returns>
        public static string Trim(string rawString, int charLimit, string appendString)
        {
            //change by wangzb
            //截字在临界值时会有问题
            //输入：伙伴™V2.0时间节点确认.docx
            //输出：伙伴™V2.0时间节点确认.docx...
            //问题原因：UTF8编码中的汉字是3字节，而之前的处理似乎当作2字节处理
            //解决：不再提前通过UTF8编码的方式判断是否应该进行字符串截取方式
            //由于没有找到一种合适的编码方式，所以在字符串截取之后进行进一步的判断

            //判断字符数
            if (string.IsNullOrEmpty(rawString) || rawString.Length <= charLimit)
            {
                return rawString;
            }

            int appendedLenth = 0;
            StringBuilder checkedStringBuilder = new StringBuilder();
            charLimit = charLimit * 2 - Encoding.UTF8.GetBytes(appendString).Length;
            for (int i = 0; i < rawString.Length; i++)
            {
                char _char = rawString[i];
                checkedStringBuilder.Append(_char);

                appendedLenth += _char > 0x80 ? 2 : 1;

                if (appendedLenth >= charLimit)
                    break;
            }

            //处理后的字符数如果没有超出限制，则返回原字符串
            if (appendedLenth < charLimit)
            {
                return rawString;
            }

            return checkedStringBuilder.Append(appendString).ToString();
        }

        #endregion String length formatter

        #region Encode & Decode

        /// <summary>
        /// Unicode转义序列
        /// </summary>
        /// <param name="rawString">待编码的字符串</param>
        public static string UnicodeEncode(string rawString)
        {
            if (rawString == null || rawString == string.Empty)
                return rawString;
            StringBuilder text = new StringBuilder();
            foreach (int c in rawString)
            {
                string t = "";
                if (c > 126)
                {
                    text.Append("\\u");
                    t = c.ToString("x");
                    for (int x = 0; x < 4 - t.Length; x++)
                    {
                        text.Append("0");
                    }
                }
                else
                {
                    t = ((char)c).ToString();
                }
                text.Append(t);
            }

            return text.ToString();
        }

        #endregion Encode & Decode

        #region Xml clean

        /// <summary>
        /// 清除xml中的不合法字符
        /// </summary>
        /// <remarks>
        /// <para>无效字符：</para>
        /// <list type="number">
        /// <item>0x00 - 0x08</item>
        /// <item>0x0b - 0x0c</item>
        /// <item>0x0e - 0x1f</item>
        /// </list>
        /// </remarks>
        /// <param name="rawXml">待清理的xml字符串</param>
        public static string CleanInvalidCharsForXML(string rawXml)
        {
            if (string.IsNullOrEmpty(rawXml))
                return rawXml;

            StringBuilder checkedStringBuilder = new StringBuilder();
            Char[] chars = rawXml.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                int charValue = Convert.ToInt32(chars[i]);

                if ((charValue >= 0x00 && charValue <= 0x08)
                    || (charValue >= 0x0b && charValue <= 0x0c)
                    || (charValue >= 0x0e && charValue <= 0x1f))
                    continue;

                checkedStringBuilder.Append(chars[i]);
            }

            return checkedStringBuilder.ToString();
        }

        #endregion Xml clean

        #region SQLInjection 预防

        /// <summary>
        /// 清理Sql注入特殊字符
        /// </summary>
        /// <remarks>
        /// 需清理字符：'、--、exec 、' or
        /// </remarks>
        /// <param name="sql">待处理的sql字符串</param>
        /// <returns>清理后的sql字符串</returns>
        public static string StripSQLInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                //防止执行 ' or
                string pattern1 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";

                //过滤 ' --
                string pattern2 = @"(\%27)|(\')|(\-\-)";

                //防止执行sql server 内部存储过程或扩展存储过程
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";

                sql = Regex.Replace(sql, pattern1, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);

                sql = sql.Replace("%", "[%]");
            }
            return sql;
        }

        #endregion SQLInjection 预防
    }
}