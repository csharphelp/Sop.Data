using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sop.Data.NhRepositories.Caches.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate bool ShouldRetryAcquireLock(ShouldRetryAcquireLockArgs args);
}
