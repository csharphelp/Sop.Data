using System;
using System.Linq;
using System.Runtime.Caching;

namespace Sop.Data.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// 
        /// </summary>
        private ObjectCache Cache =>
            MemoryCache.Default;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public virtual void Set(string key,
                                object data,
                                int cacheTime)
        {
            if (data == null)
                return;
            var policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key,
                                    data),
                      policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        public virtual void Set(string key,
                                object value,
                                TimeSpan timeSpan)
        {
            if (value == null)
                return;
            var policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now + timeSpan
            };
            Cache.Add(new CacheItem(key,
                                    value),
                      policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern,
                                 Cache.Select(p => p.Key));
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}