using Models.Models.enums;
using System;
using System.Runtime.Caching;

namespace Models.Cash
{
    public interface ICaheDao
    {
        /// <summary>
        /// 檢查狀態
        /// </summary>
        /// <param name="name"></param>
        /// <returns>存在拾回傳true</returns>
        bool isSet(string name);
        /// <summary>
        /// 取出
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Load(string name);
        /// <summary>
        /// 刪除快取
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// 存入
        /// </summary>
        /// <param name="data"></param>
        void SaveOrUpadte(string name, object data, ExpirationEnums Expiration, int cacheTime);
    }

    public class CaheDao : ICaheDao
    {
        private ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        /// <summary>
        /// 開頭名稱，以免被其他程式取用
        /// </summary>
        private string IdNameStart = "Chicken_";

        /// <summary>
        /// 檢查狀態
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool isSet(string name)
        {
            return Cache[IdNameStart + name] != null;
        }
        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Load(string name)
        {
            return Cache[IdNameStart + name];
        }
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            Cache.Remove(IdNameStart + name);
        }
        /// <summary>
        /// 設定快取
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="Expiration"></param>
        /// <param name="cacheTime"></param>
        public void SaveOrUpadte(string name, object data, ExpirationEnums Expiration, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (Expiration == ExpirationEnums.Absolute)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            }
            else if (Expiration == ExpirationEnums.Sliding)
            {
                policy.SlidingExpiration = TimeSpan.FromMinutes(cacheTime);
            }
            Cache.Add(new CacheItem(IdNameStart + name, data), policy);
        }
    }
}
