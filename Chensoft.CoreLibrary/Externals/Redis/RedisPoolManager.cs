
using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Chensoft.Externals.Redis
{
	internal static class RedisPoolManager
	{
		private static ConcurrentDictionary<string, Chensoft.Collections.ObjectPool<ServiceStack.Redis.IRedisClient>> _cache;

		static RedisPoolManager()
		{
			_cache = new ConcurrentDictionary<string, Collections.ObjectPool<ServiceStack.Redis.IRedisClient>>();
		}

		public static Chensoft.Collections.ObjectPool<ServiceStack.Redis.IRedisClient> GetRedisPool(IPEndPoint address, int poolSize, Func<ServiceStack.Redis.IRedisClient> creator)
		{
			return _cache.GetOrAdd(address.ToString(), new Collections.ObjectPool<ServiceStack.Redis.IRedisClient>(creator, p => p.Dispose(), Math.Max(poolSize, 16)));
		}
	}
}
