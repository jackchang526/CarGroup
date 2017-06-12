using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BitAuto.Services.Cache;

namespace BitAuto.CarChannel.Common.Cache
{
	public class MemCache
	{

		/// <summary>
		/// 根据key取缓存
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Object GetMemCacheByKey(string key)
		{
			Object o = null;
			if (WebConfig.IsUseMemcache)
			{
				o = DistCacheWrapper.Get(key);
			}
			else
			{
				o = CacheManager.GetCachedData(key);
			}
			return o;
		}

		/// <summary>
		/// 加入MemCache
		/// </summary>
		/// <param name="key"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static void SetMemCacheByKey(string key, Object o)
		{
			if (BitAuto.Utils.StringHelper.GetRealLength(key) <= 250)
			{
				if (WebConfig.IsUseMemcache)
				{
					DistCacheWrapper.Insert(key, o);
				}
				else
				{
					CacheManager.InsertCache(key, o, WebConfig.CachedDuration);
				}
			}
		}

		/// <summary>
		/// 加入MemCache 带过期时间的
		/// </summary>
		/// <param name="key"></param>
		/// <param name="o"></param>
		/// <param name="milliSecond"></param>
		public static void SetMemCacheByKey(string key, Object o, long milliSecond)
		{
			if (BitAuto.Utils.StringHelper.GetRealLength(key) <= 250)
			{
				if (WebConfig.IsUseMemcache)
				{
					DistCacheWrapper.Insert(key, o, milliSecond);
				}
				else
				{
					CacheManager.InsertCache(key, o, 60);
				}
			}
		}

		/// <summary>
		/// 根据key删除MemCache
		/// </summary>
		/// <param name="key"></param>
		public static void DelMemCacheByKey(string key)
		{
			if (WebConfig.IsUseMemcache)
			{
				DistCacheWrapper.Remove(key);
			}
			else
			{
				CacheManager.RemoveCachedData(key);
			}
		}

		/// <summary>
		/// 根据多个key取MemCache
		/// </summary>
		/// <param name="listKey">key 列表</param>
		/// <returns></returns>
		public static IDictionary<string, object> GetMultipleMemCacheByKey(IList<string> listKey)
		{
			IDictionary<string, object> dic = new Dictionary<string, object>();
			if (WebConfig.IsUseMemcache)
			{
				dic = DistCacheWrapper.GetMultiValue(listKey);
			}
			else
			{
				foreach (string key in listKey)
				{
					object o = CacheManager.GetCachedData(key);
					if (o != null && !dic.ContainsKey(key))
					{
						dic.Add(key, o);
					}
				}
			}
			return dic;
		}

	}
}
