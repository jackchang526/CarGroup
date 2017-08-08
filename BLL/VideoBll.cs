using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using System.Data;
using BitAuto.Utils;
using System.Xml;
using System.IO;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL
{
	public class VideoBll
	{
        /*
		/// <summary>
		/// 获取子品牌视频数据量
		/// </summary>
		/// <param name="serialId">子品牌ID</param>
		/// <returns></returns>
		public static int GetVideoCountBySerialId(int serialId)
		{
			int count = 0;
			try
			{
				count = VideoDal.GetVideoCountBySerialId(serialId);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return count;
		}

		public static List<VideoEntity> GetVideoBySerialIdAndCategoryId(int serialId, int categoryType, int top)
		{
			List<VideoEntity> list = new List<VideoEntity>();
			try
			{
				var ds = VideoDal.GetVideoBySerialIdAndCategoryId(serialId, categoryType, top);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					list.Add(new VideoEntity()
					{
						VideoId = ConvertHelper.GetInteger(dr["VideoId"]),
						ShortTitle = dr["ShortTitle"].ToString(),
						ImageLink = dr["ImageLink"].ToString(),
						ShowPlayUrl = dr["ShowPlayUrl"].ToString()
					});
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return list;
		}
        */
		/// <summary>
		/// 获取多个分类视频
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="categoryIdList">分类ID List</param>
		/// <param name="top"></param>
		/// <returns></returns>
		public static List<VideoEntity> GetVideoBySerialIdAndCategoryId(int serialId, List<int> categoryIdList, int top)
		{
			List<VideoEntity> list = new List<VideoEntity>();
			try
			{
				if (categoryIdList.Count <= 0) return list;
				var ds = VideoDal.GetVideoBySerialIdAndCategoryId(serialId, categoryIdList, top);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					list.Add(new VideoEntity()
					{
						VideoId = ConvertHelper.GetInteger(dr["VideoId"]),
						ShortTitle = dr["ShortTitle"].ToString(),
						ImageLink = dr["ImageLink"].ToString(),
						ShowPlayUrl = dr["ShowPlayUrl"].ToString()
					});
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return list;
		}
		/// <summary>
		/// 获取子品牌关联视频
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public static List<VideoEntity> GetVideoBySerialId(int serialId, int top = 5)
		{
			List<VideoEntity> list = new List<VideoEntity>();
			try
			{
				if (top <= 0) return list;
				var ds = VideoDal.GetVideoBySerialId(serialId, top);
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					list.Add(new VideoEntity()
					{
						VideoId = ConvertHelper.GetInteger(dr["VideoId"]),
						ShortTitle = dr["ShortTitle"].ToString(),
						ImageLink = dr["ImageLink"].ToString(),
						ShowPlayUrl = dr["ShowPlayUrl"].ToString(),
						Duration = ConvertHelper.GetInteger(dr["Duration"])
					});
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return list;
		}
		/// <summary>
		/// 获取视频包含热门视频 M站 2个最新 2个最热视频
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public static List<VideoEntity> GetNewAndHotVideoBySerialIdForWireless(int serialId)
		{
			string cacheKey = "Car_M_GetNewAndHotVideoBySerialIdForWireless_" + serialId;
			var cacheData = CacheManager.GetCachedData(cacheKey);
			if (cacheData != null)
				return (List<VideoEntity>)cacheData;

			var resultList = new List<VideoEntity>();
			try
			{
				const int count = 4;
				var newList = GetVideoBySerialId(serialId, count);
				var hotList = GetHotVideoBySerialId(serialId, count);
			    if (hotList.Any())
				{
					resultList.AddRange(newList.Take(2));
					foreach (var entity in hotList)
					{
						if (resultList.Find(p => p.VideoId == entity.VideoId) != null) continue;
						resultList.Add(entity);
						if (resultList.Count >= count) break;
					}
					if (resultList.Count < count && newList.Count >= count) resultList.Add(newList[2]);
				}
				else
				{ resultList.AddRange(newList); }
				CacheManager.InsertCache(cacheKey, resultList, WebConfig.CachedDuration);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return resultList;
		}

		public static List<VideoEntity> GetHotVideoBySerialId(int serialId, int top = 2)
		{
			var list = new List<VideoEntity>();
			try
			{
				var cacheKey = string.Format("GetHotVideoBySerialId_{0}_{1}", serialId, top);
				var cacheData = CacheManager.GetCachedData(cacheKey);
				if (cacheData != null)
					return (List<VideoEntity>)cacheData;
				var serialHotVideoPath = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialNews\SerialNews\hotvideonew_90.xml");
			    if (!File.Exists(serialHotVideoPath)) return list;
				XmlDocument xmlHotSerialVideo = new XmlDocument();
				xmlHotSerialVideo.Load(serialHotVideoPath);
				if (xmlHotSerialVideo != null)
				{
					XmlNode serialNode = xmlHotSerialVideo.SelectSingleNode("/root/Serial[@id=" + serialId + "]");
					if (serialNode != null)
					{
						int loop = 0;
						foreach (XmlNode node in serialNode.ChildNodes)
						{
							int videoId = ConvertHelper.GetInteger(node.InnerText);
							//if (videoList.Find(p => p.VideoId == videoId) != null) continue;
							var dsVideo = VideoDal.GetVideoById(videoId);
							if (dsVideo != null && dsVideo.Tables.Count > 0 && dsVideo.Tables[0].Rows.Count > 0)
							{
								var dr = dsVideo.Tables[0].Rows[0];
								VideoEntity entity = new VideoEntity();
								entity.VideoId = ConvertHelper.GetInteger(dr["VideoId"]);
								entity.ShortTitle = dr["ShortTitle"].ToString();
								entity.ImageLink = dr["ImageLink"].ToString();
								entity.ShowPlayUrl = dr["ShowPlayUrl"].ToString();
								entity.Duration = ConvertHelper.GetInteger(dr["Duration"]);
								loop++;
								if (loop > top) break;
								list.Add(entity);
							}
						}
						CacheManager.InsertCache(cacheKey, list, 60 * 24);//1天
					}
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return list;
		}
	}
}
