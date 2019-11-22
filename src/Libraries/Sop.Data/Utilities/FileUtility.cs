namespace Sop.Core.Utilities
{
    public class FileUtility
    {
        public static string GetFileExtension(string fileName)
        {
            string fileExtension = fileName.Substring(fileName.LastIndexOf(".") + 1);
            return fileExtension.ToLowerInvariant();
        }

        public static string GetFileNameWithoutExtension(string fileName)
        {
            string fileNameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf("."));
            return fileNameWithoutExtension;
        }

        /// <summary>
        /// 友好的文件大小信息
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        public static string FormatSize(double fileSize)
        {
            if (fileSize > 0)
            {
                if (fileSize > 1024 * 1024 * 1024)
                    return string.Format("{0:F2}GB", (fileSize / (1024 * 1024 * 1024F)));
                else if (fileSize > 1024 * 1024)
                    return string.Format("{0:F2}MB", (fileSize / (1024 * 1024F)));
                else if (fileSize > 1024)
                    return string.Format("{0:F2}KB", (fileSize / (1024F)));
                else
                    return string.Format("{0:F2}B", fileSize);
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 根据格式化的文件大小（KB、MB、GB）获取文件的字节数
        /// </summary>
        /// <param name="formattedFileSize">式化的文件大小（KB、MB、GB）</param>
        /// <returns>文件的字节数</returns>
        public static long GetFileBytes(string formattedFileSize)
        {
            long fileBytes = 0;
            string _formattedFileSize = formattedFileSize.ToUpper();

            if (_formattedFileSize.EndsWith("KB"))
            {
                fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("KB"))) * 1024;
            }
            else if (_formattedFileSize.EndsWith("MB"))
            {
                fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("MB"))) * 1024 * 1024;
            }
            else if (_formattedFileSize.EndsWith("GB"))
            {
                fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("GB"))) * 1024 * 1024 * 1024;
            }
            else if (_formattedFileSize.EndsWith("TB"))
            {
                fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("TB"))) * 1024 * 1024 * 1024 * 1024;
            }
            else if (_formattedFileSize.EndsWith("B"))
            {
                fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("B")));
            }

            return fileBytes;
        }
    }
}