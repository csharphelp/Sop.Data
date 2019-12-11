using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Sop.Core.Utilities;

namespace Sop.Core.Environment
{
    /// <summary>
    /// 默认运行环境实现
    /// </summary>
    public class RunningEnvironment
    {
        private const string WebConfigPath = "~/web.config";
        private const string BinPath = "~/bin";
        private const string RefreshHtmlPath = "~/refresh.html";

        /// <summary>
        /// 是否完全信任运行环境
        /// </summary>
        public static bool IsFullTrust()
        {
            return AppDomain.CurrentDomain.IsHomogenous && AppDomain.CurrentDomain.IsFullyTrusted;
        }

      

        /// <summary>
        /// 尝试修改web.config最后更新时间
        /// </summary>
        /// <remarks>目的是使应用程序自动重新加载</remarks>
        /// <returns>修改成功返回true，否则返回false</returns>
        private static bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(WebUtility.GetPhysicalFilePath(WebConfigPath), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试引起bin文件夹的改动
        /// </summary>
        /// <remarks>目的是使应用程序自动重新加载</remarks>
        /// <returns>成功写入返回true，否则返回false</returns>
        private static bool TryWriteBinFolder()
        {
            try
            {
                var binMarker = WebUtility.GetPhysicalFilePath(BinPath + "HostRestart");
                Directory.CreateDirectory(binMarker);
                using (var stream = File.CreateText(Path.Combine(binMarker, "log.txt")))
                {
                    stream.WriteLine("Restart on '{0}'", DateTime.UtcNow);
                    stream.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 判断一个程序集是否已加载
        /// </summary>
        /// <param name="name">程序集名称前缀</param>
        /// <returns></returns>
        public static bool IsAssemblyLoaded(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Any(assembly => new AssemblyName(assembly.FullName).Name == name);
        }

    }
}