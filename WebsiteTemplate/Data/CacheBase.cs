using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebsiteTemplate.Data
{
    /// <summary>
    /// Base class for caching items
    /// </summary>
    public abstract class CacheBase<TItem, TKey> : IDisposable
    {
        #region Fields

        readonly MemoryCache itemCache;
        readonly TimeSpan cacheTime;

        #endregion

        public CacheBase(TimeSpan cacheTime)
        {
            this.cacheTime = cacheTime;

            MemoryCacheOptions userCacheOptions = new MemoryCacheOptions()
            {

            };

            itemCache = new MemoryCache(userCacheOptions);
        }

        public TItem GetItem(TKey key)
        {
            TItem item;

            lock (itemCache)
            {
                // see if item is cached
                if (itemCache.TryGetValue(key, out item))
                    return item;

                item = CreateNewItem(key);

                AddItem(item, key);
            }

            ItemAdded(item);

            return item;
        }

        protected abstract TItem CreateNewItem(TKey key);

        protected void AddItem(TItem item, TKey key)
        {
            MemoryCacheEntryOptions entryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = cacheTime,
            };

            itemCache.Set(key, item, entryOptions);
        }

        protected virtual void ItemAdded(TItem item)
        {

        }

        public void RemoveItem(TKey key)
        {
            lock (itemCache)
            {
                itemCache.Remove(key);
            }
        }

        public void Dispose()
        {
            itemCache.Dispose();
        }
    }
}
