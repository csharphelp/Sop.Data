using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Sop.Core.Utilities;

//[assembly: PreApplicationStartMethod(typeof(ApplicationManager), "Initialize")]

namespace Sop.Data.Environment
{
    /// <summary>
    /// 应用程序加载器
    /// </summary>
    public class ApplicationManager
    {
        //private static readonly ILog logger = LogManager.GetLogger<ApplicationManager>();

        private static readonly string ApplicationsFolder = WebUtility.GetPhysicalFilePath("~/Applications/");
        /// <summary>
        /// 
        /// </summary>
        private static readonly string DependenciesFolder = WebUtility.GetPhysicalFilePath("~/App_Data/Dependencies/");

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, Application> allApplications;

        /// <summary>
        /// 扫描应用程序目录，加载所有的应用程序
        /// </summary>
        public static void Initialize()
        {
            //DirectoryInfo dependenciesFolder = new DirectoryInfo(DependenciesFolder);

            ////标记是否要重启站点
            //bool restartAppDomain = false;

            ////创建Dependencies目录
            //if (!dependenciesFolder.Exists)
            //{
            //    dependenciesFolder.Create();
            //}

            ////删除Dependencies目录中已经标记为待删除的文件
            //var toDeleteDlls = dependenciesFolder.GetFiles("*.dll.delete");
            //foreach (var dll in toDeleteDlls)
            //{
            //    try
            //    {
            //        dll.Delete();
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}

            //加载所有的应用程序
            //allApplications = new Dictionary<string, Application>();
            //foreach (var application in AvailableApplications())
            //{
            //    //加载应用程序，只有有一个应用程序标记需要重启站点，则重启站点
            //    if (LoadApplication(application))
            //    {
            //        restartAppDomain = true;
            //    }

            //    allApplications.Add(application.Id, application);
            //}

            //var dependenciesDlls = dependenciesFolder.GetFiles("*.dll");

            ////删除Dependencies中有但是Applications中已经没有的程序集
            //foreach (var dll in dependenciesDlls)
            //{
            //    var dllName = dll.Name.Remove(dll.Name.LastIndexOf('.'));
            //    var application = GetApplication(dllName);
            //    if (application == null)
            //    {
            //        try
            //        {
            //            dll.Delete();
            //        }
            //        catch (Exception e)
            //        {
            //            // 如果Dependencies被占用，则覆盖时会抛异常，这时可以先重命名原文件，将其标记为待删除，再重启站点
            //            dll.MoveTo(dll.FullName + ".delete");
            //            restartAppDomain = true;
            //        }
            //    }
            //}

            //if (restartAppDomain)
            //{
            //    logger.Info("站点因加载应用程序的需要而准备重启...");
            //    RunningEnvironment.RestartAppDomain();
            //    return;
            //}

            ////加载Dependencies中的程序集
            //dependenciesDlls = dependenciesFolder.GetFiles("*.dll");
            //foreach (var dll in dependenciesDlls)
            //{
            //    var assemblyName = AssemblyName.GetAssemblyName(dll.FullName);
            //    var assembly = Assembly.Load(assemblyName);
            //    //BuildManager.AddReferencedAssembly(assembly);
            //}

        }

        /// <summary>
        /// 根据一个应用的Id获取应用对应的元数据
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static Application GetApplication(string applicationId)
        {
            Application application = null;
            allApplications.TryGetValue(applicationId, out application);

            return application;
        }

        /// <summary>
        /// 根据一个应用的程序集获取应用对应的元数据
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Application GetApplication(Assembly assembly)
        {
            var assemblyName = assembly.GetName();

            return GetApplication(assemblyName.Name);
        }

        /// <summary>
        /// 获取所有的应用程序
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Application> GetAllWidgets()
        {
            return allApplications.Values;
        }

        /// <summary>
        /// 判断一个应用是否已经安装
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static bool IsApplicationInstalled(string applicationId)
        {
            return GetApplication(applicationId) != null;
        }

        /// <summary>
        /// 所有可用的应用程序
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Application> AvailableApplications()
        {
            var applications = new List<Application>();

            if (!Directory.Exists(ApplicationsFolder))
            {
                return applications;
            }

            var applicationDirectories = Directory.GetDirectories(ApplicationsFolder);
            foreach (var applicationDirectory in applicationDirectories)
            {
                string applicationId = Path.GetFileName(applicationDirectory.TrimEnd('/', '\\'));
                var applicationConfigPath = Path.Combine(applicationDirectory, "Application.config");

                var application = FindApplication(applicationConfigPath);
                if (application == null || !application.Enabled || application.Id != applicationId)
                {
                    continue;
                }

                applications.Add(application);
            }

            return applications;
        }

        /// <summary>
        /// 根据一个应用的Id获取其程序集在Applications目录中的路径和名称
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        private static string GetAssemblyPath(string applicationId)
        {
            var assemblyPath = Path.Combine(ApplicationsFolder, applicationId, "bin", applicationId + ".dll");

            return assemblyPath;
        }

        /// <summary>
        /// 根据一个应用的Id获取其程序集在Dependencies目录中的路径和名称
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        private static string GetDependenciesAssemblyPath(string applicationId)
        {
            var assemblyPath = Path.Combine(DependenciesFolder, applicationId + ".dll");

            return assemblyPath;
        }

        /// <summary>
        /// 读取配置文件，加载一个应用程序
        /// </summary>
        /// <param name="applicationConfigPath"></param>
        /// <returns></returns>
        private static Application FindApplication(string applicationConfigPath)
        {
            if (!File.Exists(applicationConfigPath))
            {
                return null;
            }

            XElement applicationConfig = XElement.Load(applicationConfigPath);

            //读取各个application节点中的属性
            if (applicationConfig != null)
            {
                var application = new Application();
                application.Id = applicationConfig.Element("id").Value;
                application.Name = applicationConfig.Element("name").Value;
                application.Description = applicationConfig.Element("description").Value;
                if (applicationConfig.Element("enabled") != null)
                {
                    application.Enabled = bool.Parse(applicationConfig.Element("enabled").Value);
                }
                else
                {
                    application.Enabled = true;
                }
                return application;
            }

            return null;
        }

        /// <summary>
        /// 加载指定的应用程序
        /// </summary>
        /// <param name="application"></param>
        private static bool LoadApplication(Application application)
        {
            var assemblyPath = WebUtility.GetPhysicalFilePath(GetAssemblyPath(application.Id));
            if (!File.Exists(assemblyPath))
            {
                return false;
            }

            var dependenciesAssemblyPath = WebUtility.GetPhysicalFilePath(GetDependenciesAssemblyPath(application.Id));

            // 如果Dependencies目录不存在该程序集，或程序集比Applications目录里的比较旧，则需要复制程序集
            var copyAssembly = !File.Exists(dependenciesAssemblyPath) || File.GetLastWriteTimeUtc(assemblyPath) > File.GetLastWriteTimeUtc(dependenciesAssemblyPath);

            if (copyAssembly)
            {
                try
                {
                    File.Copy(assemblyPath, dependenciesAssemblyPath, true);
                }
                catch (Exception e)
                {
                    // 如果Dependencies被占用，则覆盖时会抛异常，这时可以先重命名原文件，将其标记为待删除，再重启站点
                    if (File.Exists(dependenciesAssemblyPath))
                    {
                        File.Move(dependenciesAssemblyPath, dependenciesAssemblyPath + ".delete");
                        File.Copy(assemblyPath, dependenciesAssemblyPath, true);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
