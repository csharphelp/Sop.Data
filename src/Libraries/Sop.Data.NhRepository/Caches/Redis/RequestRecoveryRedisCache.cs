using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using NHibernate.Caches.Redis;
using StackExchange.Redis;

namespace Sop.Data.NhRepositories.Caches.Redis
{
    /// <summary>
    /// Allow NHibernate not to continue to timeout for every operation when Redis server is unavailable
    /// https://github.com/TheCloudlessSky/NHibernate.Caches.Redis
    /// redis 不可用是记录数据到内存中
    /// </summary>
    public class RequestRecoveryRedisCache : RedisCache
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SKIP_N_HIBERNATE_CACHE_KEY = "__SkipNHibernateCache__";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="properties"></param>
        /// <param name="connectionMultiplexer"></param>
        /// <param name="options"></param>
        public RequestRecoveryRedisCache(RedisCacheConfiguration configuration,
            IDictionary<string, string> properties,
            ConnectionMultiplexer connectionMultiplexer,
            RedisCacheProviderOptions options)
            : base(configuration, connectionMultiplexer, options)
        {
            //TODO 需要测试
            //var _httpContextCurrent = new AsyncLocal<HttpContext>();
            //_httpContextCurrent.Value.Items[RequestRecoveryRedisCache.SkipNHibernateCacheKey] = true;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[RequestRecoveryRedisCache.SKIP_N_HIBERNATE_CACHE_KEY] = true;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key)
        {
            if (HasFailedForThisHttpRequest())
                return null;
            return base.Get(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(object key, object value)
        {
            if (HasFailedForThisHttpRequest())
                return;
            base.Put(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            if (HasFailedForThisHttpRequest())
                return;
            base.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            if (HasFailedForThisHttpRequest())
                return;
            base.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            if (HasFailedForThisHttpRequest())
                return;
            base.Destroy();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Lock(object key)
        {
            base.Lock(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockValue"></param>
        public void Unlock(object key, object lockValue)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Unlock(object key)
        {
            base.Unlock(key);
        }

        /// <summary>
        /// 
        /// </summary>
        public string RegionName { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        private bool HasFailedForThisHttpRequest()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Items.TryGetValue(SKIP_N_HIBERNATE_CACHE_KEY, out object usok);
            }
            else
            {
                return CallContext<object>.GetData(SKIP_N_HIBERNATE_CACHE_KEY) != null;
            }
        }
    }


}