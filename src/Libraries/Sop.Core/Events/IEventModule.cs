namespace Sop.Data.Events
{
    /// <summary>
    /// 事件处理程序模块接口
    /// </summary>
    public interface IEventModule
    {
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        void RegisterEventHandler();
    }
}