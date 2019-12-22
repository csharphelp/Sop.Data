using System.Reflection;
using Autofac;

namespace Sop.Data.Environment
{
    /// <summary>
    /// 应用程序启动器接口
    /// </summary>
    public interface IApplicationStarter
    {
        /// <summary>
        /// ff
        /// </summary>
        /// <param name="container"></param>
        /// <param name="assemblies"></param>
        void Start(IContainer container, Assembly[] assemblies);
    }
}
