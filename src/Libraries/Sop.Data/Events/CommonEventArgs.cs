using System;

namespace Sop.Data.Events
{
    /// <summary>
    /// 通用事件参数
    /// </summary>
    public class CommonEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventOperationType">事件操作类型
        /// <remarks>
        /// 建议使用<see cref="Events.EventOperationType"/>协助输入，例如：<br/>
        /// EventOperationType.Instance().Create()
        /// </remarks>
        /// </param>
        public CommonEventArgs(string eventOperationType)
        {
            this.EventOperationType = eventOperationType;
        }

        /// <summary>
        /// 事件操作类型
        /// </summary>
        /// <remarks>
        /// 建议使用<see cref="Events.EventOperationType"/>协助输入，例如：<br/>
        /// EventOperationType.Instance().Create()
        /// </remarks>
        public string EventOperationType { get; }
    }
}