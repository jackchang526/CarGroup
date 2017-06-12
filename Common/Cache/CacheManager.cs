using System;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;

namespace BitAuto.CarChannel.Common.Cache
{
	public class CacheManager
	{
		public static Dictionary<string, CacheStatisticsObj> CacheStat = new Dictionary<string, CacheStatisticsObj>();

		/// <summary>
		/// 判断缓存中只否存在指定键值的缓存
		/// </summary>
		/// <param name="cacheKey">缓存键值</param>
		/// <returns>一个值，指示缓存是否存在</returns>
		public static bool IsCachedExist(string cacheKey)
		{
			return (HttpContext.Current.Cache.Get(cacheKey) == null ? false : true);
		}

		/// <summary>
		/// 添加缓存
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="cachedData"></param>
		/// <returns></returns>
		public static bool InsertCache(string cacheKey, object cachedData, int cacheInterval)
		{
			if (cacheKey != null && cacheKey.Length != 0 && HttpContext.Current != null)
			{
				//建立回调委托的一个实例
				CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

				HttpContext.Current.Cache.Insert(cacheKey, cachedData, null, DateTime.Now.AddMinutes(cacheInterval), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, callBack);
				//InsertToCache(cacheKey);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 使用缓存依赖插入一个缓存项
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="cacheData"></param>
		/// <param name="cacheDependency"></param>
		/// <returns></returns>
		public static bool InsertCache(string cacheKey, object cacheData, CacheDependency cacheDependency, DateTime absoluteExpiration)
		{
			if (cacheKey != null && cacheKey.Length != 0)
			{
				HttpContext.Current.Cache.Insert(cacheKey, cacheData, cacheDependency, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
				//InsertToCache(cacheKey);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 获取缓存数据
		/// </summary>
		/// <param name="cacheKey">缓存键值</param>
		/// <param name="cachedData">缓存数据</param>
		/// <returns>是否获取成功？</returns>
		public static bool GetCachedData(string cacheKey, out object cachedData)
		{
			cachedData = HttpContext.Current.Cache.Get(cacheKey);

			if (cachedData == null)
			{
				return false;
			}
			//HitCache(cacheKey);
			return true;
		}

		/// <summary>
		/// 获取缓存数据
		/// </summary>
		/// <param name="cacheKey">缓存键值</param>
		/// <returns></returns>
		public static object GetCachedData(string cacheKey)
		{
			object obj = HttpContext.Current.Cache.Get(cacheKey);
			//if(obj != null)
			//    HitCache(cacheKey);
			return obj;

		}

		/// <summary>
		/// 手动移除“一键一值”对应的值
		/// </summary>
		/// <param name="strIdentify"></param>
		/// <returns></returns>
		public static bool RemoveCachedData(string cacheKey)
		{
			//取出值
			if (HttpContext.Current.Cache.Get(cacheKey) != null)
			{
				HttpContext.Current.Cache.Remove(cacheKey);
			}

			return true;
		}

		//此方法在值失效之前调用，可以用于在失效之前更新数据库，或从数据库重新获取数据
		private static void onRemove(string strIdentify, object userInfo, CacheItemRemovedReason reason)
		{
		}

		/// <summary>
		/// 有一个缓存项插入到缓存中
		/// </summary>
		/// <param name="cacheKey"></param>
		private static void InsertToCache(string cacheKey)
		{
			//CacheStatisticsObj cacheObj = null;
			//if (!CacheStat.ContainsKey(cacheKey))
			//{
			//    cacheObj = new CacheStatisticsObj(cacheKey);
			//    CacheStat[cacheKey] = cacheObj;
			//}
			//else
			//    cacheObj = CacheStat[cacheKey];
			//cacheObj.InsertOnce();
		}

		/// <summary>
		/// 缓存项被命中一次
		/// </summary>
		/// <param name="cacheKey"></param>
		private static void HitCache(string cacheKey)
		{
			//if (!CacheStat.ContainsKey(cacheKey))
			//    InsertToCache(cacheKey);
			//CacheStat[cacheKey].Hit();
		}
		/// <summary>
		/// 设置页面缓存
		/// </summary>
		public static void SetPageCache(int interval)
		{
			if (HttpContext.Current != null && HttpContext.Current.Response.Cache != null)
			{
				HttpContext.Current.Response.Cache.SetNoServerCaching();
				HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
				HttpContext.Current.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(interval));
			}
		}
	}
}
