using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.Common.Cache;
using System.Collections.Specialized;
using System.Xml;
using System.IO;

namespace BitAuto.CarChannel.Common.Config
{
	/// <summary>
	/// Memcache key值对应文件数据配置文件
	/// </summary>
	public class FileToMemcacheConfig
	{
		public FileToMemcacheConfig() { }
		/// <summary>
		/// 获取 文件对应的memcache key值
		/// </summary>
		/// <param name="filePath">文件路径</param>
		/// <returns></returns>
		public static string GetValue(string filePath)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(filePath))
				return result;
			Dictionary<string, string> dict = GetConfigData();
			filePath = Path.GetFullPath(filePath);
			if (dict != null && dict.ContainsValue(filePath))
			{
				string key = dict.FirstOrDefault(pire => pire.Value == filePath).Key;
				if (!string.IsNullOrEmpty(key))
					return key;
			}
			return result;
		}
		/// <summary>
		/// 获取配置文件数据
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> GetConfigData()
		{
			string filePath = HttpContext.Current.Server.MapPath(@"~/config/FileToMemcache.config");
			string cacheKey = "Car_Config_FileToMemcache_Dict";
			Dictionary<string, string> dictFileToKey = (Dictionary<string, string>)CacheManager.GetCachedData(cacheKey);
			try
			{
				if (dictFileToKey != null)
					return dictFileToKey;
				else
				{
					dictFileToKey = new Dictionary<string, string>();
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(filePath);
					XmlNodeList nodeList = xmlDoc.SelectNodes("/FileToMemcache/add");
					foreach (XmlNode node in nodeList)
					{
						string fullName = node.Attributes["value"].Value;
						if (string.IsNullOrEmpty(fullName))
							continue;
						fullName = Path.GetFullPath(fullName);
						dictFileToKey.Add(node.Attributes["key"].Value, fullName);
					}
					CacheManager.InsertCache(cacheKey, dictFileToKey, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddDays(1));
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return dictFileToKey;
		}
	}
}
