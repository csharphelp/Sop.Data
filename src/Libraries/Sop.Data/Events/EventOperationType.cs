namespace Sop.Data.Events
{
    /// <summary>
    /// 事件操作类型
    /// </summary>
    public class EventOperationType
    {
        #region Instance

        private static volatile EventOperationType _instance = null;
        private static readonly object LockObject = new object();

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>返回EventOperationType对象</returns>
        public static EventOperationType Instance()
        {
            if (_instance == null)
            {
                lock (LockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new EventOperationType();
                    }
                }
            }
            return _instance;
        }

  

        #endregion Instance

        /// <summary>
        /// 创建
        /// </summary>
        public string Create()
        {
            return "Create";
        }

        /// <summary>
        /// 更新
        /// </summary>
        public string Update()
        {
            return "Update";
        }

        /// <summary>
        /// 删除
        /// </summary>
        public string Delete()
        {
            return "Delete";
        }
        
    }
}