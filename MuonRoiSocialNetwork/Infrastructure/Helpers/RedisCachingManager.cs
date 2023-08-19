using BaseConfig.MethodResult;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace MuonRoiSocialNetwork.Infrastructure.Helpers
{
    /// <summary>
    /// Manager Redis
    /// </summary>
    public static class RedisCachingManager
    {
        private static readonly string _redisConfigName = "127.0.0.1:6379";
        /// <summary>
        /// Set cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
                                                   string recordId,
                                                   T data,
                                                   TimeSpan? absoluteExpireTime = null,
                                                   TimeSpan? slidingExpireTime = null)
        {
            DistributedCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = slidingExpireTime
            };

            string jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }
        /// <summary>
        /// Get cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache,
                                                       string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
        /// <summary>
        /// Clear all cache in server
        /// </summary>
        /// <returns></returns>
        public static async Task<MethodResult<bool>> ClearApplicationCache()
        {
            MethodResult<bool> methodResult = new();
            ConnectionMultiplexer redis = await GetConnectionMultiplexer();
            if (redis == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.Result = false;
                return methodResult;
            }
            IDatabase cache = redis.GetDatabase();
            if (cache == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.Result = false;
                return methodResult;
            }
            IServer server = redis.GetServer(_redisConfigName);
            if (server == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.Result = false;
                return methodResult;
            }
            IEnumerable<RedisKey> keys = server.Keys();
            if (keys == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.Result = false;
                return methodResult;
            }
            await cache.KeyDeleteAsync(keys.Select(k => k).ToArray());
            redis.Close();
            redis.Dispose();
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = true;
            return methodResult;
        }
        /// <summary>
        /// Get all cache in server
        /// </summary>
        /// <returns></returns>
        public static async Task<MethodResult<List<string>>> GetApplicationCache()
        {
            ConnectionMultiplexer redis = await GetConnectionMultiplexer();
            MethodResult<List<string>> methodResult = new();
            List<RedisKey> keysExist = new();
            if (redis == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            IDatabase cache = redis.GetDatabase();
            if (cache == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            IServer server = redis.GetServer(_redisConfigName);
            if (server == null || !server.TryWait(server.PingAsync()))
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            IEnumerable<RedisKey> keys = server.Keys();
            if (keys == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            foreach (var item in keys)
            {
                keysExist.Add(item);
                methodResult.StatusCode = StatusCodes.Status200OK;
            }
            redis.Close();
            redis.Dispose();
            methodResult.Result = keysExist.Select(x => x.ToString()).ToList();
            return methodResult;
        }
        private async static Task<ConnectionMultiplexer> GetConnectionMultiplexer()
        {
            var options = ConfigurationOptions.Parse(_redisConfigName);
            options.ConnectRetry = 5;
            options.AllowAdmin = true;
            return await ConnectionMultiplexer.ConnectAsync(options);
        }
    }
}
