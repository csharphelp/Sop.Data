using System;

namespace Sop.Data.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value" />
        /// <param name="cacheTime"></param>
        void Set(string key, object value, int cacheTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value" />
        /// <param name="timeSpan"></param>
        void Set(string key, object value, TimeSpan timeSpan);

        /// <summary />
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key" />
        void Remove(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern" />
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clear
        /// </summary>
        void Clear();
    }
}