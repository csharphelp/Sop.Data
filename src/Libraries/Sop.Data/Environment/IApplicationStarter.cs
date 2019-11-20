using System.Reflection;
using Autofac;

namespace Sop.Data.Environment
{
    /// <summary>
    /// 应用程序启动器接口
    /// </summary>
    public interface IApplicationStarter
    {
        void Start(IContainer container, Assembly[] assemblies);
    }
}
