using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace Sop.Core.Utilities
{
    /// <summary>
    /// 用于文本数据格式化
    /// </summary>
    public class TextFormatter
    {
        /// <summary>
        /// 多行纯文本型转化为可以在HTML中显示
        /// </summary>
        /// <remarks>
        /// 一般在存储到数据库之前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForStorage(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (keepWhiteSpace)
            {
                plainText = plainText.Replace(" ", "&nbsp;");
                plainText = plainText.Replace("\t", "&nbsp;&nbsp;");
            }
            plainText = plainText.Replace("\r\n", WebUtility.HtmlNewLine);
            plainText = plainText.Replace("\n", WebUtility.HtmlNewLine);

            return plainText;
        }

        /// <summary>
        /// 多行纯文本型转化为可以在TextArea中正常显示
        /// </summary>
        /// <remarks>
        /// 一般在进行编辑前进行转化
        /// </remarks>
        /// <param name="plainText">需要转化的纯文本</param>
        /// <param name="keepWhiteSpace">是否保留空格</param>
        public static string FormatMultiLinePlainTextForEdit(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            string result = plainText;
            result = result.Replace(WebUtility.HtmlNewLine, "\n");
            if (keepWhiteSpace)
                result = result.Replace("&nbsp;", " ");

            return result;
        }

        /// <summary>
        /// 格式化纯文本评论
        /// </summary>
        /// <remarks>
        /// 进行以下操作：
        /// 1、敏感词过滤
        /// 2、所有链接增加nofollow属性
        /// 3、保留换行及空格的格式
        /// </remarks>
        /// <param name="text">格式化的内容</param>
        /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
        /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
        public static string FormatPlainTextComment(string text)
        {
            return FormatPlainTextComment(text, true, true);
        }

        /// <summary>
        /// 清除标签名称中的非法字词
        /// </summary>
        public static string CleanTagName(string appKey)
        {
            //Remark:20090808_zhengw 删除Url中可编码的特殊字符：'#','&','=','/','%','?','+', '$',
            string[] parts = appKey.Split('!', '.', '@', '^', '*', '(', ')', '[', ']', '{', '}', '<', '>', ',', '\\', '\'', '~', '`', '|');
            appKey = string.Join("", parts);
            return appKey;
        }

        /// <summary>
        /// 友好的文件大小信息
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        public static string FormatFriendlyFileSize(double fileSize)
        {
            if (fileSize > 0)
            {
                if (fileSize > 1024 * 1024 * 1024)
                    return string.Format("{0:F2}G", (fileSize / (1024 * 1024 * 1024F)));
                else if (fileSize > 1024 * 1024)
                    return string.Format("{0:F2}M", (fileSize / (1024 * 1024F)));
                else if (fileSize > 1024)
                    return string.Format("{0:F2}K", (fileSize / (1024F)));
                else
                    return string.Format("{0:F2}Bytes", fileSize);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 格式化评论内容
        /// </summary>
        /// <param name="text">格式化的内容</param>
        /// <param name="enableNoFollow">Should we include the nofollow rel.</param>
        /// <param name="enableConversionToParagraphs">Should newlines be converted to P tags.</param>
        private static string FormatPlainTextComment(string text, bool enableNoFollow, bool enableConversionToParagraphs)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = WebUtility.HtmlEncode(text);

            if (enableNoFollow)
            {
                //Find any links
                StringCollection uniqueMatches = new StringCollection();

                string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
                MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                foreach (Match m in matches)
                {
                    if (!uniqueMatches.Contains(m.ToString()))
                    {
                        text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m + "\">" + m.ToString().Trim() + "</a>");
                        uniqueMatches.Add(m.ToString());
                    }
                }
            }

            // Replace Line breaks with <br> and every other concurrent space with &nbsp; (to allow line breaking)
            if (enableConversionToParagraphs)
                text = ConvertPlainTextToParagraph(text);// text.Replace("\n", "<br />");

            text = text.Replace("  ", " &nbsp;");

            return text;
        }

        /// <summary>
        /// 把纯文字格式化成html段落
        /// </summary>
        /// <remarks>
        /// 使文本在Html中保留换行的格式
        /// </remarks>
        private static string ConvertPlainTextToParagraph(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace("\r\n", "\n").Replace("\r", "\n");

            string[] lines = text.Split('\n');

            StringBuilder paragraphs = new StringBuilder();

            foreach (string line in lines)
            {
                if (line != null && line.Trim().Length > 0)
                    paragraphs.AppendFormat("{0}<br />\n", line);
            }
            return paragraphs.ToString().Remove(paragraphs.ToString().LastIndexOf("<br />"));
        }
    }
}