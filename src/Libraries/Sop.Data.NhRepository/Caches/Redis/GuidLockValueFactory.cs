using System;
using NHibernate.Caches.Redis;

namespace Sop.Data.NhRepositories.Caches.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class GuidLockValueFactory : ILockValueFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLockValue()
        {
            return "lock-" + Guid.NewGuid();            
        }
    }
}
