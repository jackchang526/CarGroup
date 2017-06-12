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
		/// �жϻ�����ֻ�����ָ����ֵ�Ļ���
		/// </summary>
		/// <param name="cacheKey">�����ֵ</param>
		/// <returns>һ��ֵ��ָʾ�����Ƿ����</returns>
		public static bool IsCachedExist(string cacheKey)
		{
			return (HttpContext.Current.Cache.Get(cacheKey) == null ? false : true);
		}

		/// <summary>
		/// ��ӻ���
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="cachedData"></param>
		/// <returns></returns>
		public static bool InsertCache(string cacheKey, object cachedData, int cacheInterval)
		{
			if (cacheKey != null && cacheKey.Length != 0 && HttpContext.Current != null)
			{
				//�����ص�ί�е�һ��ʵ��
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
		/// ʹ�û�����������һ��������
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
		/// ��ȡ��������
		/// </summary>
		/// <param name="cacheKey">�����ֵ</param>
		/// <param name="cachedData">��������</param>
		/// <returns>�Ƿ��ȡ�ɹ���</returns>
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
		/// ��ȡ��������
		/// </summary>
		/// <param name="cacheKey">�����ֵ</param>
		/// <returns></returns>
		public static object GetCachedData(string cacheKey)
		{
			object obj = HttpContext.Current.Cache.Get(cacheKey);
			//if(obj != null)
			//    HitCache(cacheKey);
			return obj;

		}

		/// <summary>
		/// �ֶ��Ƴ���һ��һֵ����Ӧ��ֵ
		/// </summary>
		/// <param name="strIdentify"></param>
		/// <returns></returns>
		public static bool RemoveCachedData(string cacheKey)
		{
			//ȡ��ֵ
			if (HttpContext.Current.Cache.Get(cacheKey) != null)
			{
				HttpContext.Current.Cache.Remove(cacheKey);
			}

			return true;
		}

		//�˷�����ֵʧЧ֮ǰ���ã�����������ʧЧ֮ǰ�������ݿ⣬������ݿ����»�ȡ����
		private static void onRemove(string strIdentify, object userInfo, CacheItemRemovedReason reason)
		{
		}

		/// <summary>
		/// ��һ����������뵽������
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
		/// ���������һ��
		/// </summary>
		/// <param name="cacheKey"></param>
		private static void HitCache(string cacheKey)
		{
			//if (!CacheStat.ContainsKey(cacheKey))
			//    InsertToCache(cacheKey);
			//CacheStat[cacheKey].Hit();
		}
		/// <summary>
		/// ����ҳ�滺��
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
