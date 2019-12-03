//using System;
//using System.Linq;
//using Microsoft.Extensions.Caching.Memory;
//
//namespace Sop.Data.Caching
//{
//    /// <summary>
//    /// Represents a manager for caching between HTTP requests (long term caching)
//https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/memory?view=aspnetcore-3.0
//    /// </summary>
//    public partial class MemoryCacheManager : ICacheManager
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        private IMemoryCache _cache;
//
//        public MemoryCacheManager(IMemoryCache memoryCache)
//        {
//            _cache = memoryCache;
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        public virtual T Get<T>(string key)
//        {
//            _cache.GetOrCreate(key,)
////            _cache.Get(key);
//            return (T)_cache[key];
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="data"></param>
//        /// <param name="cacheTime"></param>
//        public virtual void Set(string key,
//                                object data,
//                                int cacheTime)
//        {
//            if (data == null)
//                return;
//            var policy = new CacheItemPolicy()
//            {
//                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
//            };
//            _cache.Add(new CacheItem(key,
//                                    data),
//                      policy);
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="value"></param>
//        /// <param name="timeSpan"></param>
//        public virtual void Set(string key,
//                                object value,
//                                TimeSpan timeSpan)
//        {
//            if (value == null)
//                return;
//            var policy = new CacheItemPolicy()
//            {
//                AbsoluteExpiration = DateTime.Now + timeSpan
//            };
//            _cache.Add(new CacheItem(key,
//                                    value),
//                      policy);
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public virtual bool IsSet(string key)
//        {
//            return (_cache.Contains(key));
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        public virtual void Remove(string key)
//        {
//            _cache.Remove(key);
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="pattern"></param>
//        public virtual void RemoveByPattern(string pattern)
//        {
//            this.RemoveByPattern(pattern,
//                                 _cache.Select(p => p.Key));
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void Clear()
//        {
//            foreach (var item in _cache)
//                Remove(item.Key);
//        }
//
//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void Dispose()
//        {
//        }
//    }
//}