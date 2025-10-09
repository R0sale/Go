using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Redis
{
    public static class DistributedCacheExtensions
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            AllowTrailingCommas = true
        };

        public static Task SetAsync<T>(IDistributedCache cache, string key, T value)
        {
            return SetAsync(cache, key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            }); 
        }

        public static Task SetAsync<T>(IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options = null)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
            return cache.SetAsync(key, bytes, options);
        }

        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            var bytes = cache.Get(key);
            if (bytes is null || bytes.Length == 0)
            {
                value = default;    
                return false;
            }
            var json = Encoding.UTF8.GetString(bytes);
            value = JsonSerializer.Deserialize<T>(json, serializerOptions);
            return true;
        }

        public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T?>> task, DistributedCacheEntryOptions options = null)
        {
            if (options is null)
            {
                options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };
            }

            if (cache.TryGetValue<T>(key, out T? value))
                return value;


            value = await task();

            if (value is not null)
            {
                await cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                return value;
            }
                

            return value;
        }
    }
}
