using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Sop.Core.Utilities
{
    /// <summary>
    /// 提供与Web请求时可使用的工具类，包括Url解析、Url/Html编码、获取IP地址、返回http状态码
    /// </summary>
    public static class WebUtility
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly string HtmlNewLine = "<br />";

        #region Url & Path

        /// <summary>
        /// 将URL转换为在请求客户端可用的 URL（转换 ~/ 为绝对路径）
        /// </summary>
        /// <param name="relativeUrl">相对url</param>
        /// <returns>返回绝对路径</returns>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            //TODO
            //if (relativeUrl.StartsWith("~/"))
            //{
            //    string[] urlParts = relativeUrl.Split('?');
            //    string resultUrl = VirtualPathUtility.ToAbsolute(urlParts[0]);
            //    if (urlParts.Length > 1)
            //        resultUrl += "?" + urlParts[1];

            //    return resultUrl;
            //}

            return relativeUrl;
        }

        /// <summary>
        /// 获取带传输协议的完整的主机地址
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns>
        /// <para>返回带传输协议的完整的主机地址</para>
        ///     <example>https://www.spacebuilder.cn:8080</example>
        /// </returns>
        public static string HostPath(Uri uri)
        {
            if (uri == null)
                return string.Empty;

            string port = uri.IsDefaultPort ? string.Empty : (":" + Convert.ToString(uri.Port, CultureInfo.InvariantCulture));
            return uri.Scheme + Uri.SchemeDelimiter + uri.Host + port;
        }

        /// <summary>
        /// 获取物理文件路径
        /// </summary>
        /// <param name="filePath">
        ///     <remarks>
        ///         <para>filePath支持以下格式：</para>
        ///         <list type="bullet">
        ///         <item>~/abc/</item>
        ///         <item>c:\abc\</item>
        ///         <item>\\192.168.0.1\abc\</item>
        ///         </list>
        ///     </remarks>
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetPhysicalFilePath(string filePath)
        {
            string calculatedFilePath = null;

            // Make sure it isn't a drive reference like "c:\blah" or a UNC name like "\\machine\share"
            if ((filePath.IndexOf(":\\", StringComparison.Ordinal) != -1) || (filePath.IndexOf("\\\\", StringComparison.Ordinal) != -1))
                calculatedFilePath = filePath;
            else
            {
                //if (HostingEnvironment.IsHosted)
                //{
                //    calculatedFilePath = HostingEnvironment.MapPath(filePath);
                //}
                //else
                //{
                //    filePath = filePath.Replace('/', Path.DirectorySeparatorChar).Replace("~", "");
                //    calculatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                //}
                filePath = filePath.Replace('/', Path.DirectorySeparatorChar).Replace("~", "");
                calculatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            }
            return calculatedFilePath;
        }

        /// <summary>
        /// 把content中的虚拟路径转化成完整的url
        /// </summary>
        /// <remarks>
        /// 例如： /abc/e.aspx 转化成 http://www.SOPCCE.cn/abc/e.aspx
        /// </remarks>
        /// <param name="content">content</param>
        public static string FormatCompleteUrl(string content)
        {
            //todo
            //string srcPatternFormat = "src=[\"']\\s*(/[^\"']*)\\s*[\"']";
            //string hrefPatternFormat = "href=[\"']\\s*(/[^\"']*)\\s*[\"']";

            //string hostPath = HostPath(HttpContext.Current.Request.Url);

            //content = Regex.Replace(content, srcPatternFormat, "src=\"" + hostPath + "$1\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //content = Regex.Replace(content, hrefPatternFormat, "href=\"" + hostPath + "$1\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return content;
        }

        /// <summary>
        /// 获取根域名
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="domainRules">域名规则</param>
        public static string GetServerDomain(Uri uri, string[] domainRules)
        {
            if (uri == null)
                return string.Empty;

            string strHostName = uri.Host.ToString().ToLower(); // 此处获取值转换为小写
            if (strHostName.IndexOf('.') > 0)
            {
                string[] strArr = strHostName.Split('.');
                string lastStr = strArr.GetValue(strArr.Length - 1).ToString();
                int number = -1;
                if (int.TryParse(lastStr, out number)) // 如果最后一位是数字，那么说明是IP地址
                {
                    return strHostName; // 替换.为纯数字输出
                }

                string findStr = string.Empty;
                string replaceStr = string.Empty;
                string returnStr = string.Empty;
                for (int i = 0; i < domainRules.Length; i++)
                {
                    if (strHostName.EndsWith(domainRules[i].ToLower())) // 如果最后有找到匹配项
                    {
                        findStr = domainRules[i].ToLower(); // www.spacebuilder.CN
                        replaceStr = strHostName.Replace(findStr, ""); // 将匹配项替换为空，便于再次判断
                        if (replaceStr.IndexOf('.') > 0) // 存在二级域名或者三级，比如：www.spacebuilder
                        {
                            string[] replaceArr = replaceStr.Split('.'); // www spacebuilder
                            returnStr = replaceArr.GetValue(replaceArr.Length - 1).ToString() + findStr;
                            return returnStr;
                        }

                        returnStr = replaceStr + findStr; // 连接起来输出为：spacebuilder.com
                        return returnStr;
                    }
                    else
                    {
                        returnStr = strHostName;
                    }
                }

                return returnStr;
            }

            return strHostName;
        }

        #endregion Url & Path

        #region Encode/Decode

        /// <summary>
        /// html编码
        /// </summary>
        /// <remarks>
        /// <para>调用HttpUtility.HtmlEncode()，当前已知仅作如下转换：</para>
        /// <list type="bullet">
        ///     <item>&lt; = &amp;lt;</item>
        ///     <item>&gt; = &amp;gt;</item>
        ///     <item>&amp; = &amp;amp;</item>
        ///     <item>&quot; = &amp;quot;</item>
        /// </list>
        /// </remarks>
        /// <param name="rawContent">待编码的字符串</param>
        public static string HtmlEncode(String rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
                return rawContent;

            return HttpUtility.HtmlEncode(rawContent);
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="rawContent">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlDecode(String rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
                return rawContent;

            return HttpUtility.HtmlDecode(rawContent);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="urlToEncode">待编码的url字符串</param>
        /// <returns>编码后的url字符串</returns>
        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
                return urlToEncode;

            return HttpUtility.UrlEncode(urlToEncode).Replace("'", "%27");
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="urlToDecode">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
                return urlToDecode;

            return HttpUtility.UrlDecode(urlToDecode);
        }

        #endregion Encode/Decode
 

 
 
    }
}