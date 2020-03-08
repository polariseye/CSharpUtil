

namespace Polaris.Utility
{
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 缓存操作工具类
    /// </summary>
    public class MemoryCacheUtil
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        public static MemoryCache CacheObj { get; private set; }

        static MemoryCacheUtil()
        {
            var opt = new MemoryCacheOptions();
            CacheObj = new MemoryCache(opt);
        }

        /// <summary>
        /// 取缓存项，如果不存在则返回空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetItem<T>(String key)
        {
            try
            {
                return CacheObj.Get<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 是否包含指定键的缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            Object val;
            return CacheObj.TryGetValue(key, out val);
        }

        /// <summary>
        /// 取缓存项,如果不存在则新增缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cachePopulate"></param>
        /// <returns></returns>
        public static T GetOrAddItem<T>(String key, Func<T> cachePopulate)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (cachePopulate == null) throw new ArgumentNullException("cachePopulate");

            return CacheObj.GetOrCreate(key, (ent) => { return cachePopulate(); });
        }

        /// <summary>
        /// 移除指定键的缓存项
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveItem(string key)
        {
            CacheObj.Remove(key);
        }
    }
}
