using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.Data
{
    /// <summary>
    /// Overrides the Database class to cache certain items
    /// </summary>
    public sealed class CachedDatabase : Database
    {
        readonly TimeSpan cacheTime = TimeSpan.FromMinutes(10);
        readonly MemoryCache userCache;

        public CachedDatabase(string connectionString)
            : base(connectionString)
        {
            MemoryCacheOptions userCacheOptions = new MemoryCacheOptions()
            {

            };

            userCache = new MemoryCache(userCacheOptions);
        }

        public async override Task AddUserAsync(User user, CancellationToken token)
        {
            await base.AddUserAsync(user, token);
            userCache.Remove(user.Id);
        }

        public async override Task UpdateUserAsync(User user, CancellationToken token)
        {
            await base.UpdateUserAsync(user, token);
            userCache.Remove(user.Id);
        }

        public async override Task UpdateUserConfigAsync(User user, CancellationToken token)
        {
            await base.UpdateUserConfigAsync(user, token);
            userCache.Remove(user.Id);
        }

        public async override Task<User> GetUserAsync(int userId, CancellationToken token)
        {
            if (userCache.TryGetValue(userId, out User user))
                return user;

            user = await base.GetUserAsync(userId, token);

            if (user != null)
                userCache.Set(userId, user, cacheTime);

            return user;
        }

        public async override Task DeleteUserAsync(int userId, CancellationToken token)
        {
            await base.DeleteUserAsync(userId, token);
            userCache.Remove(userId);
        }
    }
}
