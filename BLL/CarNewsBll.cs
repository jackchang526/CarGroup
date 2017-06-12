using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using System.Xml;
using BitAuto.CarChannel.Common;
using System.Web;
using Newtonsoft.Json.Linq;

namespace BitAuto.CarChannel.BLL
{
	/// <summary>
	/// 车型新闻库
	/// </summary>
	public class CarNewsBll
	{
		private CarNewsDll newsDll = null;
		public CarNewsBll()
		{
			newsDll = new CarNewsDll();
		}
		/// <summary>
		/// 获取厂商新闻列表
		/// </summary>
		public DataSet GetProducerNews(int producerId, CarNewsType carNewsType, int pageSize, int pageIndex, out int rowCount)
		{
			if (producerId <= 0)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			return newsDll.GetProducerNews(producerId, (int)carNewsType, pageSize, pageIndex, out rowCount);
		}
		/// <summary>
		/// 获取厂商新闻列表
		/// </summary>
		public DataSet GetTopProducerNews(int producerId, CarNewsType carNewsType, int top)
		{
			if (producerId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopProducerNews(producerId, (int)carNewsType, top);
		}
		/// <summary>
		/// 获取主品牌新闻列表
		/// </summary>
		public DataSet GetMasterBrandNews(int masterBrandId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int recordCount)
		{
			if (masterBrandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategroyIds= newsDll.GetCarNewsType((int)carNewsType);
            if (arrCategroyIds.Length < 1)
            { return null; }

            string cacheKey = string.Format("Car_NewsBll_GetMbn_{0}_{1}_{2}_{3}", masterBrandId,(int)carNewsType, pageSize, pageIndex);
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetMbn_Count_{0}_{1}_{2}_{3}", masterBrandId, (int)carNewsType, pageSize, pageIndex);
            object objMasterBrandNews = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objMasterBrandNews != null)
            {
                if (objRecordCount != null)
                {
                    recordCount = (int)objRecordCount;
                }
                return (objMasterBrandNews as DataSet);
            }
            DataSet dsResult=newsDll.GetMasterBrandNews(masterBrandId, arrCategroyIds, pageSize, pageIndex, ref recordCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, recordCount, WebConfig.CachedDuration);
            return dsResult;
		}
		public DataSet GetMasterBrandNews(int masterBrandId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int recordCount)
		{
			if (masterBrandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategroyIds = newsDll.GetCarNewsType(carNewsTypeIdList);
            if (arrCategroyIds.Length < 1)
            { return null; }

            string cacheKey = string.Format("Car_NewsBll_GetMbn_{0}_{1}_{2}_{3}", masterBrandId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetMbn_Count_{0}_{1}_{2}_{3}", masterBrandId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));
            object objMasterBrandNews = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objMasterBrandNews != null)
            {
                if (objRecordCount != null)
                {
                    recordCount = (int)objRecordCount;
                }
                return (objMasterBrandNews as DataSet);
            }
            DataSet dsResult=newsDll.GetMasterBrandNews(masterBrandId, arrCategroyIds, pageSize, pageIndex, ref recordCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, recordCount, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取主品牌新闻列表
		/// </summary>
		public DataSet GetTopMasterBrandNews(int masterBrandId, CarNewsType carNewsType, int top)
		{
			if (masterBrandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopMasterBrandNews(masterBrandId, (int)carNewsType, top);
		}
		/// <summary>
		/// 获取主品牌焦点新闻及新闻列表
		/// </summary>
		public DataSet GetTopMasterBrandFocusNews(int masterBrandId, CarNewsType carNewsType, int top)
		{
			if (masterBrandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;
            string cacheKey = string.Format("Car_NewsBll_GetTMbfn_{0}_{1}_{2}", masterBrandId, (int)carNewsType, top);
            object objGetTMbfn = CacheManager.GetCachedData(cacheKey);
            if (objGetTMbfn != null)
                return (objGetTMbfn as DataSet);
            DataSet dsResult =newsDll.GetTopMasterBrandFocusNews(masterBrandId, (int)carNewsType, (int)NewsBlockOrderTypes.masterfocus, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取主品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetMasterBrandNewsAllData(int masterBrandId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int recordCount)
		{
			if (masterBrandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			return newsDll.GetMasterBrandNewsAllData(masterBrandId, (int)carNewsType, pageSize, pageIndex, ref recordCount);
		}
		/// <summary>
		/// 获取主品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetTopMasterBrandNewsAllData(int masterBrandId, CarNewsType carNewsType, int top)
		{
			if (masterBrandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopMasterBrandNewsAllData(masterBrandId, (int)carNewsType, top);
		}
		/// <summary>
		/// 获取品牌新闻列表
		/// </summary>
		public DataSet GetBrandNews(int brandId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int recordCount)
		{
			if (brandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategoryIds = newsDll.GetCarNewsType((int)carNewsType);

            string cacheKey = string.Format("Car_NewsBll_GetBn_{0}_{1}_{2}_{3}", brandId, (int)carNewsType, pageSize,pageIndex);
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetBn_Count_{0}_{1}_{2}_{3}", brandId, (int)carNewsType, pageSize, pageIndex);
            object objGetBn = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetBn != null)
            {
                if (objRecordCount != null)
                {
                    recordCount = (int)objRecordCount;
                }
                return (objGetBn as DataSet);
            }
            DataSet dsResult = newsDll.GetBrandNews(brandId, arrCategoryIds, pageSize, pageIndex, ref recordCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, recordCount, WebConfig.CachedDuration);
            return dsResult;
		}

		public DataSet GetBrandNews(int brandId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int recordCount)
		{
			if (brandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategoryIds = newsDll.GetCarNewsType(carNewsTypeIdList);

            string cacheKey = string.Format("Car_NewsBll_GetBn_{0}_{1}_{2}_{3}", brandId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetBn_Count_{0}_{1}_{2}_{3}", brandId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));

            object objGetBn = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetBn != null)
            {
                if (objRecordCount != null)
                {
                    recordCount = (int)objRecordCount;
                }
                return (objGetBn as DataSet);
            }
            DataSet dsResult = newsDll.GetBrandNews(brandId, arrCategoryIds, pageSize, pageIndex, ref recordCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, recordCount, WebConfig.CachedDuration);
            return dsResult;
		}

		/// <summary>
		/// 获取品牌新闻列表
		/// </summary>
		public DataSet GetTopBrandNews(int brandId, CarNewsType carNewsType, int top)
		{
			if (brandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopBrandNews(brandId, (int)carNewsType, top);
		}
		/// <summary>
		/// 获取品牌新闻排序列表
		/// </summary>
		public DataSet GetTopBrandFocusNews(int brandId, CarNewsType carNewsType, int top)
		{
			if (brandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

            string cacheKey = string.Format("Car_NewsBll_GetTbfn_{0}_{1}_{2}", brandId,(int)carNewsType,top);
            object objGetTbfn = CacheManager.GetCachedData(cacheKey);
            if (objGetTbfn != null)
                return (objGetTbfn as DataSet);
            DataSet dsResult = newsDll.GetTopBrandFocusNews(brandId, (int)carNewsType, (int)NewsBlockOrderTypes.brandfocus, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetBrandNewsAllData(int brandId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int recordCount)
		{
			if (brandId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			return newsDll.GetBrandNewsAllData(brandId, (int)carNewsType, pageSize, pageIndex, ref recordCount);
		}
		/// <summary>
		/// 获取品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetTopBrandNewsAllData(int brandId, CarNewsType carNewsType, int top)
		{
			if (brandId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopBrandNewsAllData(brandId, (int)carNewsType, top);
		}
		/// <summary>
		/// 获取级别新闻列表
		/// </summary>
		public DataSet GetLevelNews(int levelId, CarNewsType carNewsType, int pageSize, int pageIndex, out int recordCount)
		{
			if (levelId <= 0)
			{
				recordCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			return newsDll.GetLevelNews(levelId, (int)carNewsType, pageSize, pageIndex, out recordCount);
		}
		/// <summary>
		/// 获取级别新闻列表
		/// </summary>
		public DataSet GetTopLevelNews(int levelId, CarNewsType carNewsType, int top)
		{
			if (levelId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

			return newsDll.GetTopLevelNews(levelId, (int)carNewsType, top);
		}
        /// <summary>
        /// 获取级别新闻列表
        /// </summary>
        public DataSet GetLevelNewsWithComment(int levelId, CarNewsType carNewsType, int top)
        {
            if (levelId <= 0)
            {
                return null;
            }
            if (top < 0)
                top = 20;

            return newsDll.GetLevelNewsWithComment(levelId, (int)carNewsType, top);
        }
        /// <summary>
        /// 获取级别新闻列表
        /// </summary>
        public DataSet GetLevelNewsWithComment(int levelId, List<int> carNewsType, int top)
        {
            if (levelId <= 0)
            {
                return null;
            }
            if (top < 0)
                top = 20;

            return newsDll.GetLevelNewsWithComment(levelId, carNewsType, top);
        }
		/// <summary>
		/// 得到城市行情新闻
		/// 注：当cityid的新闻数量不满足top时，将根据parentCityId取数据，并舍弃cityid的数据
		/// </summary>
		public List<News> GetTopCityNews(int serialId, int cityId, int top)
		{
			return GetTopCityNews(serialId, cityId, -1, top);
		}
		/// <summary>
		/// 得到城市行情新闻。
		/// 注：当cityid的新闻数量不满足top时，将根据parentCityId取数据，并舍弃cityid的数据
		/// </summary>
		public List<News> GetTopCityNews(int serialId, int cityId, int parentCityId, int top)
		{
			if (serialId < 1 || cityId < 0 || top < 1)
			{
				return null;
			}
			DataSet ds = newsDll.GetTopCityNews(serialId, cityId, top);
			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < top)
			{
				if (parentCityId >= 0)
				{
					DataSet pds = newsDll.GetTopCityNews(serialId, parentCityId, top);
					if (pds != null && pds.Tables.Count > 0)
						ds = pds;
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(ds.Tables[0].Rows.Count);
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					result.Add(new News()
					{
						NewsId = ConvertHelper.GetInteger(row["cmsnewsid"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						FaceTitle = Common.CommonFunction.NewsTitleDecode(row["FaceTitle"].ToString()),
						PageUrl = row["FilePath"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						CategoryName = "hangqing"
					});
				}
			}
			return result;
		}
		/// <summary>
		/// 得到城市行情新闻
		/// 注：当cityid的新闻数量不满足top时，其余数据将有parentCityId补充
		/// </summary>
		public List<News> GetTopCityNews2(int serialId, int cityId, int top)
		{
			return GetTopCityNews2(serialId, cityId, -1, top);
		}
		/// <summary>
		/// 得到城市行情新闻。
		/// 注：当cityid的新闻数量不满足top时，其余数据将有parentCityId补充
		/// </summary>
		public List<News> GetTopCityNews2(int serialId, int cityId, int parentCityId, int top)
		{
			if (serialId < 1 || cityId < 0 || top < 1)
			{
				return null;
			}
			DataSet ds = newsDll.GetTopCityNews(serialId, cityId, top);
			if (ds == null || ds.Tables.Count < 1)
			{
				if (parentCityId >= 0)
				{
					DataSet pds = newsDll.GetTopCityNews(serialId, parentCityId, top);
					if (pds != null && pds.Tables.Count > 0)
						ds = pds;
				}
			}
			else if (ds.Tables[0].Rows.Count < top)
			{
				if (parentCityId >= 0)
				{
					// modified by chengl May.29.2012
					// DataSet pds = newsDll.GetTopCityNews(serialId, parentCityId, top);
					DataSet pds = newsDll.GetTopCityNews(serialId, parentCityId, top - ds.Tables[0].Rows.Count);
					if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
					{
						ds.Tables[0].Merge(pds.Tables[0]);
					}
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				List<int> newsIds = new List<int>(ds.Tables[0].Rows.Count);
				result = new List<News>(ds.Tables[0].Rows.Count);
				int newsId = 0;
				foreach (DataRow row in ds.Tables[0].Select(string.Empty, "PublishTime desc"))
				{
					newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
					if (newsIds.Contains(newsId)) continue;
					newsIds.Add(newsId);

					result.Add(new News()
					{
						NewsId = newsId,
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						FaceTitle = Common.CommonFunction.NewsTitleDecode(row["FaceTitle"].ToString()),
						PageUrl = row["FilePath"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						CategoryName = "hangqing"
					});

					if (newsIds.Count >= top)
						break;
				}
			}
			return result;
		}
		/// <summary>
		/// 是否有子品牌新闻
		/// </summary>
		public bool IsSerialNews(int serialId, int year, CarNewsType carNewsType)
		{
			return GetSerialNewsCount(serialId, year, carNewsType) > 0;
		}
		/// <summary>
		/// 获取子品牌新闻数量
		/// </summary>
		public int GetSerialNewsCount(int serialId, int year, CarNewsType carNewsType)
		{
			if (serialId > 0)
			{
				if (year < 1)
				{
					Dictionary<string, Dictionary<int, int>> daoGouNum = AutoStorageService.GetCacheTreeSerialNewsCount();
					if (daoGouNum != null && daoGouNum.Count > 0)
					{
						string type = carNewsType.ToString();
						if (daoGouNum.ContainsKey(type) && daoGouNum[type].ContainsKey(serialId))
						{
							return daoGouNum[type][serialId];
						}
					}
				}
				else
				{
					return newsDll.GetSerialNewsCount(serialId, year, carNewsType);
				}
			}
			return 0;
		}
		/// <summary>
		/// 获取子品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetSerialNewsAllData(int serialId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int rowCount)
		{
			if (serialId <= 0)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			//Dictionary<string, Dictionary<int, int>> daoGouNum = AutoStorageService.GetCacheTreeSerialNewsCount();
			//if (daoGouNum != null)
			//{
			//	string type = carNewsType.ToString();
			//	if (daoGouNum.ContainsKey(type) && daoGouNum[type].ContainsKey(serialId))
			//	{
			//		rowCount = daoGouNum[type][serialId];
			//	}
			//}

            string cacheKey = string.Format("Car_NewsBll_GetSnad_{0}_{1}_{2}_{3}", serialId, (int)carNewsType, pageSize,pageIndex);
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetSnad_Count_{0}_{1}_{2}_{3}", serialId, (int)carNewsType, pageSize, pageIndex);
            object objGetSnad = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetSnad != null)
            {
                if (objRecordCount != null)
                {
                    rowCount = (int)objRecordCount;
                }
                return (objGetSnad as DataSet);
            }
            DataSet dsResult = newsDll.GetSerialNewsAllData(serialId, (int)carNewsType, pageSize, pageIndex, ref rowCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, rowCount, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取多个新闻分类文章
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="carNewsTypeIdList">分类值List</param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="rowCount"></param>
		/// <returns></returns>
		public DataSet GetSerialNewsAllData(int serialId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int rowCount)
		{
			if (serialId <= 0)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategoryIds =newsDll.GetCarNewsType(carNewsTypeIdList);
            

            string cacheKey = string.Format("Car_NewsBll_GetSnad_{0}_{1}_{2}_{3}", serialId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetSnad_Count_{0}_{1}_{2}_{3}", serialId, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));

            object objGetSnad = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetSnad != null)
            {
                if(objRecordCount!=null)
                {
                    rowCount=(int)objRecordCount;
                }
                return (objGetSnad as DataSet);
            }
            DataSet dsResult = newsDll.GetSerialNewsAllData(serialId, arrCategoryIds, pageSize, pageIndex, ref rowCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
             CacheManager.InsertCache(cacheRecordCountKey, rowCount, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取子品牌新闻列表，关联新闻表
		/// </summary>
		public DataSet GetTopSerialNewsAllData(int serialId, CarNewsType carNewsType, int top)
		{
			if (serialId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;

            string cacheKey = string.Format("Car_NewsBll_GetTsnad_{0}_{1}_{2}", serialId, (int)carNewsType, top);
            object objGetTsnad = CacheManager.GetCachedData(cacheKey);
            if (objGetTsnad != null)
                return (objGetTsnad as DataSet);
            DataSet dsResult = newsDll.GetTopSerialNewsAllData(serialId, (int)carNewsType, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取子品牌新闻列表
		/// </summary>
		public DataSet GetSerialNews(int serialId, CarNewsType carNewsType, int pageSize, int pageIndex, ref int rowCount)
		{
			if (serialId <= 0)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
			Dictionary<string, Dictionary<int, int>> daoGouNum = AutoStorageService.GetCacheTreeSerialNewsCount();
			if (daoGouNum != null)
			{
				string type = carNewsType.ToString();
				if (daoGouNum.ContainsKey(type) && daoGouNum[type].ContainsKey(serialId))
				{
					rowCount = daoGouNum[type][serialId];
				}
			}

			return newsDll.GetSerialNews(serialId, (int)carNewsType, pageSize, pageIndex, ref rowCount);
		}
		/// <summary>
		/// 获取子品牌新闻列表
		/// </summary>
		public DataSet GetTopSerialNews(int serialId, CarNewsType carNewsType, int top)
		{
			if (serialId <= 0)
			{
				return null;
			}
			if (top < 0)
				top = 20;
            string cacheKey = string.Format("Car_NewsBll_GetTsn_{0}_{1}_{2}", serialId, (int)carNewsType, top);
            object objGetTsn = CacheManager.GetCachedData(cacheKey);
            if (objGetTsn != null)
                return (objGetTsn as DataSet);
            DataSet dsResult = newsDll.GetTopSerialNews(serialId, (int)carNewsType, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
		}

        /// <summary>
        /// 获取子品牌新闻列表
        /// </summary>
        public DataSet GetTopSerialNews(int serialId, int top)
        {
            if (serialId <= 0)
            {
                return null;
            }
            if (top < 0)
                top = 20;
            string cacheKey = string.Format("Car_NewsBll_GetTsn_NoNewsType_{0}_{1}", serialId, top);
            object objGetTsn = CacheManager.GetCachedData(cacheKey);
            if (objGetTsn != null)
                return (objGetTsn as DataSet);
            DataSet dsResult = newsDll.GetTopSerialNews(serialId, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
        }

		/// <summary>
		/// 获取子品牌年款新闻列表，关联新闻表
		/// </summary>
		public DataSet GetSerialYearNewsAllData(int serialId, int year, CarNewsType carNewsType, int pageSize, int pageIndex, out int rowCount)
		{
			if (serialId < 1 || year < 1)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategoryIds = newsDll.GetCarNewsType((int)carNewsType);

            string cacheKey = string.Format("Car_NewsBll_GetSynad_{0}_{1}_{2}_{3}_{4}", serialId,year, (int)carNewsType,pageSize,pageIndex);
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetSynad_Count_{0}_{1}_{2}_{3}_{4}", serialId, year, (int)carNewsType, pageSize, pageIndex);

            DataSet dsResult;
            object objGetSynad = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetSynad != null)
            {
                if (objRecordCount != null)
                {
                    rowCount = (int)objRecordCount;
                }
                else
                {
                    rowCount = 0;
                }
                dsResult = (objGetSynad as DataSet);
                return dsResult;
            }
            dsResult = newsDll.GetSerialYearNewsAllData(serialId, year, arrCategoryIds, pageSize, pageIndex, out rowCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, rowCount, WebConfig.CachedDuration);
            return dsResult;
		}

		public DataSet GetSerialYearNewsAllData(int serialId, int year, List<int> carNewsTypeIdList, int pageSize, int pageIndex, out int rowCount)
		{
			if (serialId < 1 || year < 1)
			{
				rowCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
            int[] arrCategoryIds = newsDll.GetCarNewsType(carNewsTypeIdList);

            string cacheKey = string.Format("Car_NewsBll_GetSynad_{0}_{1}_{2}_{3}_{4}", serialId, year, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));
            string cacheRecordCountKey = string.Format("Car_NewsBll_GetSynad_Count_{0}_{1}_{2}_{3}_{4}", serialId, year, pageSize, pageIndex, string.Join("_", carNewsTypeIdList.ToArray()));

            DataSet dsResult;
            object objGetSynad = CacheManager.GetCachedData(cacheKey);
            object objRecordCount = CacheManager.GetCachedData(cacheRecordCountKey);
            if (objGetSynad != null)
            {
                if (objRecordCount!=null)
                {
                    rowCount = (int)objRecordCount;
                }
                else
                {
                    rowCount = 0;
                }
                dsResult = (objGetSynad as DataSet);
                return dsResult;
            }
            dsResult = newsDll.GetSerialYearNewsAllData(serialId, year, arrCategoryIds, pageSize, pageIndex, out rowCount);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            CacheManager.InsertCache(cacheRecordCountKey, rowCount, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取子品牌年款新闻列表，关联新闻表
		/// </summary>
		public DataSet GetTopSerialYearNewsAllData(int serialId, int year, CarNewsType carNewsType, int top)
		{
			if (serialId < 1 || year < 1)
			{
				return null;
			}
			if (top < 0)
				top = 20;

            string cacheKey = string.Format("Car_NewsBll_GetTsnad_{0}_{1}_{2}_{3}", serialId,year, (int)carNewsType, top);
            object objGetTsnad = CacheManager.GetCachedData(cacheKey);
            if (objGetTsnad != null)
                return (objGetTsnad as DataSet);
            DataSet dsResult = newsDll.GetTopSerialYearNewsAllData(serialId, year, (int)carNewsType, top);
            CacheManager.InsertCache(cacheKey, dsResult, WebConfig.CachedDuration);
            return dsResult;
		}
		/// <summary>
		/// 获取子品牌城市或省行情
		/// </summary>
		public DataSet GetSerialCityOrProvinceNewsAllData(int serialId, bool isProvince, int objectId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (serialId <= 0 || objectId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
			if (isProvince)
				return newsDll.GetSerialProvinceNewsAllData(serialId, objectId, pageSize, pageIndex, ref newsCount);
			else
				return newsDll.GetSerialCityNewsAllData(serialId, objectId, pageSize, pageIndex, ref newsCount);
		}
		/// <summary>
		/// 获取子品牌省行情
		/// </summary>
		public DataSet GetSerialProvinceNewsAllData(int serialId, int provinceId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (serialId <= 0 || provinceId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
			newsCount = GetSerialCityNewsCount(serialId, provinceId);
			return newsDll.GetSerialProvinceNewsAllData(serialId, provinceId, pageSize, pageIndex, ref newsCount);
		}
		/// <summary>
		/// 获取子品牌城市行情--
		/// </summary>
		public DataSet GetSerialCityNewsAllData(int serialId, int cityId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (serialId <= 0 || cityId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;
			newsCount = GetSerialCityNewsCount(serialId, cityId);
			return newsDll.GetSerialCityNewsAllData(serialId, cityId, pageSize, pageIndex, ref newsCount);
		}

		/// <summary>
		/// 获取省行情，包含全国,全国id=0
		/// </summary>
		public DataSet GetProvinceNewsAllData(int provinceId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (provinceId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			Dictionary<int, int> countDic = GetProviceCityNewsCount();
			if (countDic != null && countDic.ContainsKey(provinceId))
				newsCount = countDic[provinceId];

			return newsDll.GetProvinceNewsAllData(provinceId, pageSize, pageIndex, ref newsCount);
		}
		/// <summary>
		/// 获取城市行情
		/// </summary>
		public DataSet GetCityNewsAllData(int cityId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (cityId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 20;
			if (pageIndex < 0)
				pageIndex = 0;

			Dictionary<int, int> countDic = GetProviceCityNewsCount();
			if (countDic != null && countDic.ContainsKey(cityId))
				newsCount = countDic[cityId];

			return newsDll.GetCityNewsAllData(cityId, pageSize, pageIndex, ref newsCount);
		}

		/// <summary>
		/// 获取子品牌城市新闻数
		/// </summary>
		public Dictionary<int, int> GetSerialCityNewsCount(int serialId)
		{
			if (serialId < 1) return null;

			string cacheKey = "GetSerialCityNewsCount_" + serialId.ToString();
			Dictionary<int, int> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, int>;
			if (dic == null)
			{
				DataSet ds = newsDll.GetSerialCityNewsCount(serialId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dic = new Dictionary<int, int>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						//CityId,Num
						dic.Add(Convert.ToInt32(row["CityId"]), Convert.ToInt32(row["Num"]));
					}
				}
				if (dic == null)
					dic = new Dictionary<int, int>();
				CacheManager.InsertCache(cacheKey, dic, 10);
			}
			return dic;
		}
		/// <summary>
		/// 获取子品牌城市新闻数
		/// </summary>
		public int GetSerialCityNewsCount(int serialId, int cityId)
		{
			Dictionary<int, int> dic = GetSerialCityNewsCount(serialId);
			if (dic == null || dic.Count < 1) return 0;
			return dic.ContainsKey(cityId) ? dic[cityId] : 0;
		}
		/// <summary>
		/// 获取省市行情新闻数
		/// </summary>
		public Dictionary<int, int> GetProviceCityNewsCount()
		{
			string cacheKey = "carnews_GetProviceCityNewsCount";
			Dictionary<int, int> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, int>;
			if (dic == null)
			{
				DataSet ds = newsDll.GetProviceCityNewsCount();
				if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
					dic = new Dictionary<int, int>();
				else
				{
					int count;
					dic = new Dictionary<int, int>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						count = Convert.ToInt32(row["num"]);
						if (count > 0)
							dic.Add(Convert.ToInt32(row["CityId"]), count);
					}
				}
				CacheManager.InsertCache(cacheKey, dic, 10);
			}
			return dic;
		}

		/// <summary>
		/// 合作站级别页首页新闻
		/// </summary>
		public DataSet GetCopperationLevelTopNews(int levelId)
		{
			if (levelId > 0)
			{
				return newsDll.GetCopperationLevelTopNews(levelId);
			}
			return null;
		}

		/// <summary>
		/// 获取编辑评测试驾子品牌对应车型
		/// </summary>
		public Dictionary<int, int> GetEditorCommentCarId()
		{
			string cache = "GetEditorCommentDic";
			Dictionary<int, int> result = CacheManager.GetCachedData(cache) as Dictionary<int, int>;
			if (result == null)
			{
				DataSet ds = newsDll.GetEditorCommentCarId();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					result = new Dictionary<int, int>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						result.Add(Convert.ToInt32(row["SerialId"]), Convert.ToInt32(row["CarId"]));
					}
				}
				if (result == null)
					result = new Dictionary<int, int>();
				CacheManager.InsertCache(cache, result, 15);
			}
			return result;
		}
		/// <summary>
		/// 获取编辑评测试驾子品牌对应车型
		/// </summary>
		public int GetEditorCommentCarId(int serialId)
		{
			if (serialId > 0)
			{
				Dictionary<int, int> dic = GetEditorCommentCarId();
				if (dic != null && dic.Count > 0 && dic.ContainsKey(serialId))
					return dic[serialId];
			}
			return -1;
		}

		/// <summary>
		/// 获取品牌置换信息
		/// </summary>
		public DataSet GetBrandZhiHuanNews(int brandId, int cityId, int top)
		{
			if (brandId < 1 || cityId < 1)
			{
				return null;
			}
			if (top < 0)
				top = 10;

			return newsDll.GetBrandZhiHuanNews(brandId, cityId, top);
		}

		/// <summary>
		/// 获取子品牌城市置换数
		/// </summary>
		public Dictionary<int, int> GetSerialCityZhiHuanCount(int serialId)
		{
			if (serialId < 1) return null;

			string cacheKey = "GetSerialCityZhiHuanCount" + serialId.ToString();
			Dictionary<int, int> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, int>;
			if (dic == null)
			{
				DataSet ds = newsDll.GetSerialCityZhiHuanCount(serialId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dic = new Dictionary<int, int>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						//CityId,Num
						dic.Add(Convert.ToInt32(row["CityId"]), Convert.ToInt32(row["Num"]));
					}
				}
				if (dic == null)
					dic = new Dictionary<int, int>();
				CacheManager.InsertCache(cacheKey, dic, 10);
			}
			return dic;
		}
		/// <summary>
		/// 获取子品牌城市置换数
		/// </summary>
		public int GetSerialCityZhiHuanCount(int serialId, int cityId)
		{
			Dictionary<int, int> dic = GetSerialCityZhiHuanCount(serialId);
			if (dic == null || dic.Count < 1) return 0;
			return dic.ContainsKey(cityId) ? dic[cityId] : 0;
		}
		/// <summary>
		/// 获取子品牌城市置换新闻
		/// </summary>
		public DataSet GetSerialCityZhiHuanNewsAllData(int serialId, int cityId, int pageSize, int pageIndex, ref int newsCount)
		{
			if (serialId <= 0 || cityId < 0)
			{
				newsCount = 0;
				return null;
			}
			if (pageSize < 0)
				pageSize = 10;
			if (pageIndex < 0)
				pageIndex = 0;
			newsCount = GetSerialCityZhiHuanCount(serialId, cityId);
			return newsDll.GetSerialCityZhiHuanNewsAllData(serialId, cityId, pageSize, pageIndex, ref newsCount);
		}
		/// <summary>
		/// 获取品牌城市经销商置换行情
		/// </summary>
		public DataSet GetBrandZhiHuanDealerNews(int brandId, int cityId, int top)
		{
			if (brandId < 1 || cityId < 1) return null;
			if (top < 1)
				top = 3;

			return newsDll.GetBrandZhiHuanDealerNews(brandId, cityId, top);
		}

		/// <summary>
		/// 得到城市置换新闻
		/// 注：当cityid的新闻数量不满足top时，其余数据将有parentCityId补充
		/// </summary>
		public List<News> GetTopCityZhiHuanNews2(int serialId, int cityId, int top)
		{
			return GetTopCityZhiHuanNews2(serialId, cityId, -1, top);
		}
		/// <summary>
		/// 得到城市置换新闻。
		/// 注：当cityid的新闻数量不满足top时，其余数据将有parentCityId补充
		/// </summary>
		public List<News> GetTopCityZhiHuanNews2(int serialId, int cityId, int parentCityId, int top)
		{
			if (serialId < 1 || cityId < 0 || top < 1)
			{
				return null;
			}
			DataSet ds = newsDll.GetTopCityZhiHuanNews(serialId, cityId, top);
			if (ds == null || ds.Tables.Count < 1)
			{
				if (parentCityId >= 0)
				{
					DataSet pds = newsDll.GetTopCityZhiHuanNews(serialId, parentCityId, top);
					if (pds != null && pds.Tables.Count > 0)
						ds = pds;
				}
			}
			else if (ds.Tables[0].Rows.Count < top)
			{
				if (parentCityId >= 0)
				{
					DataSet pds = newsDll.GetTopCityZhiHuanNews(serialId, parentCityId, top - ds.Tables[0].Rows.Count);
					if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
					{
						ds.Tables[0].Merge(pds.Tables[0]);
					}
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				List<int> newsIds = new List<int>(ds.Tables[0].Rows.Count);
				result = new List<News>(ds.Tables[0].Rows.Count);
				int newsId = 0;
				foreach (DataRow row in ds.Tables[0].Select(string.Empty, "PublishTime desc"))
				{
					newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
					if (newsIds.Contains(newsId)) continue;
					newsIds.Add(newsId);

					result.Add(new News()
					{
						NewsId = newsId,
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						FaceTitle = Common.CommonFunction.NewsTitleDecode(row["FaceTitle"].ToString()),
						PageUrl = row["FilePath"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						CategoryName = "zhihuan"
					});

					if (newsIds.Count >= top)
						break;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取有置换信息的省份和城市列表
		/// </summary>
		public List<int> GetZhiHuanCityIds()
		{
			string cacheKey = "CarNewsBll_GetZhiHuanCityIds";
			object result = CacheManager.GetCachedData(cacheKey);
			if (result == null)
			{
				DataSet ds = newsDll.GetZhiHuanCityIds();
				List<int> dataList = null;
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					Dictionary<int, CityExtend> cityList = AutoStorageService.Get350CityDicKeyCityId();
					dataList = new List<int>(cityList.Count);
					int cityId;
					foreach (DataRow cityRow in ds.Tables[0].Rows)
					{
						cityId = ConvertHelper.GetInteger(cityRow["cityid"].ToString());
						if (cityList.ContainsKey(cityId))
						{
							if (!dataList.Contains(cityId))
								dataList.Add(cityId);
						}
						else
						{
							foreach (CityExtend cityObj in cityList.Values)
							{
								if (cityObj.ParentId == cityId && !dataList.Contains(cityObj.CityId))
								{
									dataList.Add(cityObj.CityId);
								}
							}
						}
					}
				}
				if (dataList == null)
					result = new object();
				else
					result = dataList;

				CacheManager.InsertCache(cacheKey, result, 15);
			}
			return result as List<int>;
		}
		/// <summary>
		/// 获取置换品牌id列表
		/// </summary>
		public List<int> GetZhiHuanBrandIds(int cityId)
		{
			if (cityId < 1)
				return null;

			string cacheKey = "CarNewsBll_GetZhiHuanBrandIds_" + cityId.ToString();
			object result = CacheManager.GetCachedData(cacheKey);
			if (result == null)
			{
				List<int> dataList = null;
				Dictionary<int, CityExtend> cityList = AutoStorageService.Get350CityDicKeyCityId();
				if (cityList.ContainsKey(cityId))
				{
					DataSet ds = newsDll.GetZhiHuanBrandIds(cityId, cityList[cityId].ParentId);

					if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						XmlDocument autoData = AutoStorageService.GetAutoXml();
						if (autoData != null)
						{
							dataList = new List<int>();
							XmlNode brandNode;
							int brandId;
							foreach (DataRow serialRow in ds.Tables[0].Rows)
							{
								brandNode = autoData.SelectSingleNode(string.Format("Params/MasterBrand/Brand[Serial[@ID='{0}']]", serialRow["serialid"].ToString()));
								if (brandNode != null)
								{
									brandId = ConvertHelper.GetInteger(brandNode.Attributes["ID"].Value);
									if (!dataList.Contains(brandId))
										dataList.Add(brandId);
								}
							}
						}
					}
				}
				if (dataList == null)
					result = new object();
				else
					result = dataList;

				CacheManager.InsertCache(cacheKey, result, 15);
			}
			return result as List<int>;
		}

		/// <summary>
		/// 获取全部品牌下多少子品牌有置换数据
		/// key=品牌id,value=子品牌数
		/// </summary>
		public Dictionary<int, int> GetZhiHuanSerialCount()
		{
			object result;
			string cacheKey = "CarNewsBll_GetZhiHuanSerialCount";
			if (!CacheManager.GetCachedData(cacheKey, out result))
			{
				List<int> serials = GetZhiHuanSerials();
				if (serials != null && serials.Count > 0)
				{
					XmlDocument doc = AutoStorageService.GetAutoXml();
					XmlNodeList nodes = doc.SelectNodes("//Params/MasterBrand/Brand");
					if (nodes.Count > 0)
					{
						Dictionary<int, int> brands = new Dictionary<int, int>(nodes.Count);
						foreach (XmlNode brandNode in nodes)
						{
							int brandId = ConvertHelper.GetInteger(brandNode.Attributes["ID"].Value);
							int serialcount = 0;
							foreach (XmlNode serialNode in brandNode.ChildNodes)
							{
								int serialId = ConvertHelper.GetInteger(serialNode.Attributes["ID"].Value);
								if (serials.Contains(serialId))
									serialcount++;
							}
							if (serialcount > 0)
								brands.Add(brandId, serialcount);
						}
						result = brands;
					}
				}
				if (result == null)
					result = new object();

				CacheManager.InsertCache(cacheKey, result, 30);
			}
			return result as Dictionary<int, int>;
		}

		/// <summary>
		/// 获取全部有置换数据的子品牌
		/// </summary>
		public List<int> GetZhiHuanSerials()
		{
			object result;
			string cacheKey = "CarNewsBll_GetZhiHuanSerials";
			if (!CacheManager.GetCachedData(cacheKey, out result))
			{
				DataSet ds = newsDll.GetZhiHuanSerials();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					List<int> list = new List<int>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						list.Add(ConvertHelper.GetInteger(row["SerialId"]));
					}
					result = list;
				}
				if (result == null)
					result = new object();

				CacheManager.InsertCache(cacheKey, result, 30);
			}
			return result as List<int>;
		}

		#region 降价新闻

		/// <summary>
		/// 取降价新闻 包含经销商数据 add by chengl Aug.14.2013
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <param name="top"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<JiangJiaNews> GetSerialJiangJiaTopDealer(int serialId, int cityId, int top, Int16 type)
		{
			return GetSerialJiangJiaTopDealer(serialId, cityId, -1, top, type);
		}

		/// <summary>
		///  取降价新闻 包含经销商数据 add by chengl Aug.14.2013
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="cityId"></param>
		/// <param name="centerCityId"></param>
		/// <param name="top"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<JiangJiaNews> GetSerialJiangJiaTopDealer(int serialId, int cityId, int centerCityId, int top, Int16 type)
		{
			if (serialId < 1 || top < 1 || cityId < 0)
				return null;
			List<int> listVendorId = new List<int>();
			List<JiangJiaNews> result = new List<JiangJiaNews>();

			//030329 anh. 注：高总修改逻辑,对应城市无降价文章的提取上一级城市的降价文章
			DataSet ds = newsDll.GetSerialJiangJiaTopNews(serialId, cityId, top, type);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					result.Add(new JiangJiaNews()
					{
						NewsId = ConvertHelper.GetInteger(row["newsid"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						NewsUrl = row["NewsUrl"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						MaxFavorablePrice = Convert.ToDecimal(row["MaxFavorablePrice"]),
						VendorId = ConvertHelper.GetInteger(row["VendorId"]),
						VendorName = row["VendorName"].ToString().Trim()
					});
					listVendorId.Add(ConvertHelper.GetInteger(row["newsid"]));
				}
			}
			// 如果不够调试 并且还有中心城
			if (result.Count < top && centerCityId >= 0)
			{
				// 补中心城经销商
				ds = newsDll.GetSerialJiangJiaTopNews(serialId, centerCityId, top, type);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						// 如果已取过此经销商则跳过
						if (listVendorId.Contains(ConvertHelper.GetInteger(row["newsid"])))
						{ continue; }
						result.Add(new JiangJiaNews()
						{
							NewsId = ConvertHelper.GetInteger(row["newsid"]),
							Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
							NewsUrl = row["NewsUrl"].ToString(),
							PublishTime = Convert.ToDateTime(row["PublishTime"]),
							MaxFavorablePrice = Convert.ToDecimal(row["MaxFavorablePrice"]),
							VendorId = ConvertHelper.GetInteger(row["VendorId"]),
							VendorName = row["VendorName"].ToString().Trim()
						});
						if (result.Count >= top)
						{ break; }
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 获取子品牌降价新闻
		/// </summary>
		/// <param name="serialId">子品牌id</param>
		/// <param name="cityId">省id|市id|0，0为全国</param>
		/// <param name="type">1=全国，2=省，3=市</param>
		public List<News> GetSerialJiangJiaTopNews(int serialId, int cityId, int top, Int16 type)
		{
			return GetSerialJiangJiaTopNews(serialId, cityId, -1, top, type);
		}
		/// <summary>
		/// 获取子品牌降价新闻
		/// </summary>
		/// <param name="serialId">子品牌id</param>
		/// <param name="cityId">省id|市id|0，0为全国</param>
		/// <param name="type">1=全国，2=省，3=市</param>
		public List<News> GetSerialJiangJiaTopNews(int serialId, int cityId, int centerCityId, int top, Int16 type)
		{
			if (serialId < 1 || top < 1 || cityId < 0)
				return null;

			//030329 anh. 注：高总修改逻辑,对应城市无降价文章的提取上一级城市的降价文章
			DataSet ds = newsDll.GetSerialJiangJiaTopNews(serialId, cityId, top, type);
			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
			{
				if (centerCityId >= 0)
				{
					ds = newsDll.GetSerialJiangJiaTopNews(serialId, centerCityId, top, type);
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(ds.Tables[0].Rows.Count);
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					result.Add(new News()
					{
						NewsId = ConvertHelper.GetInteger(row["newsid"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						PageUrl = row["NewsUrl"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						VendorId = ConvertHelper.GetInteger(row["VendorId"]),
						CategoryName = "jiangjia",
						CarImage = row["CarImage"].ToString()
					});
				}
			}
			return result;
		}
		/// <summary>
		/// 获取品牌降价新闻
		/// </summary>
		/// <param name="brandId">品牌id</param>
		/// <param name="cityId">省id|市id|0，0为全国</param>
		/// <param name="type">1=全国，2=省，3=市</param>
		public List<News> GetBrandJiangJiaTopNews(int brandId, int cityId, int centerCityId, int top, Int16 type)
		{
			if (brandId < 1 || top < 1 || cityId < 0)
				return null;
			DataSet ds = newsDll.GetBrandJiangJiaTopNews(brandId, cityId, top, type);
			if (ds == null || ds.Tables.Count < 1)
			{
				if (centerCityId >= 0)
				{
					DataSet pds = newsDll.GetBrandJiangJiaTopNews(brandId, centerCityId, top, type);
					if (pds != null && pds.Tables.Count > 0)
						ds = pds;
				}
			}
			else if (ds.Tables[0].Rows.Count < top)
			{
				if (centerCityId >= 0)
				{
					DataSet pds = newsDll.GetBrandJiangJiaTopNews(brandId, centerCityId, top - ds.Tables[0].Rows.Count, type);
					if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
					{
						ds.Tables[0].Merge(pds.Tables[0]);
					}
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(ds.Tables[0].Rows.Count);
				foreach (DataRow row in ds.Tables[0].Select(string.Empty, "PublishTime desc"))
				{
					result.Add(new News()
					{
						NewsId = ConvertHelper.GetInteger(row["newsid"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						PageUrl = row["NewsUrl"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"])
					});

					if (result.Count >= top)
						break;
				}
			}
			return result;
		}
		/// <summary>
		/// 获取主品牌降价新闻
		/// </summary>
		/// <param name="masterId">主品牌id</param>
		/// <param name="cityId">省id|市id|0，0为全国</param>
		/// <param name="type">1=全国，2=省，3=市</param>
		public List<News> GetMasterBrandJiangJiaTopNews(int masterId, int cityId, int centerCityId, int top, Int16 type)
		{
			if (masterId < 1 || top < 1 || cityId < 0)
				return null;

			DataSet ds = newsDll.GetMasterBrandJiangJiaTopNews(masterId, cityId, top, type);
			if (ds == null || ds.Tables.Count < 1)
			{
				if (centerCityId >= 0)
				{
					DataSet pds = newsDll.GetMasterBrandJiangJiaTopNews(masterId, centerCityId, top, type);
					if (pds != null && pds.Tables.Count > 0)
						ds = pds;
				}
			}
			else if (ds.Tables[0].Rows.Count < top)
			{
				if (centerCityId >= 0)
				{
					DataSet pds = newsDll.GetMasterBrandJiangJiaTopNews(masterId, centerCityId, top - ds.Tables[0].Rows.Count, type);
					if (pds != null && pds.Tables.Count > 0 && pds.Tables[0].Rows.Count > 0)
					{
						ds.Tables[0].Merge(pds.Tables[0]);
					}
				}
			}

			List<News> result = null;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(ds.Tables[0].Rows.Count);
				foreach (DataRow row in ds.Tables[0].Select(string.Empty, "PublishTime desc"))
				{
					result.Add(new News()
					{
						NewsId = ConvertHelper.GetInteger(row["newsid"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						PageUrl = row["NewsUrl"].ToString(),
						PublishTime = Convert.ToDateTime(row["PublishTime"])
					});

					if (result.Count >= top)
						break;
				}
			}
			return result;
		}
		/// <summary>
		/// 子品牌城市降价新闻汇总数据
		/// </summary>
		public SerialJiangJiaNewsSummary GetSerialJiangJiaNewsSummary(int serialId, int cityId)
		{
			if (serialId < 1 || cityId < 0)
				return null;
			Dictionary<int, SerialJiangJiaNewsSummary> summary = GetSerialJiangJiaNewsSummary(serialId);
			if (summary != null && summary.ContainsKey(cityId))
				return summary[cityId];
			return null;
		}
		/// <summary>
		/// 子品牌降价新闻汇总数据
		/// </summary>
		public Dictionary<int, SerialJiangJiaNewsSummary> GetSerialJiangJiaNewsSummary(int serialId)
		{
			if (serialId < 1) return null;

			string cacheKey = "CarNewsBll_GetSerialJiangJiaNewsSummary_" + serialId.ToString();
			Dictionary<int, SerialJiangJiaNewsSummary> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, SerialJiangJiaNewsSummary>;
			if (dic == null)
			{
				DataSet ds = newsDll.GetSerialJiangJiaNewsSummary(serialId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dic = new Dictionary<int, SerialJiangJiaNewsSummary>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						//CityId,Num,MaxFavorablePrice
						dic.Add(ConvertHelper.GetInteger(row["CityId"]),
							new SerialJiangJiaNewsSummary()
							{
								NewsCount = ConvertHelper.GetInteger(row["Num"]),
								MaxFavorablePrice = ConvertHelper.GetDecimal(row["MaxFavorablePrice"]),
								VendorNum = ConvertHelper.GetInteger(row["VendorNum"]),
								CarNum = ConvertHelper.GetInteger(row["CarNum"]),
								MaxFavorableRate = ConvertHelper.GetDecimal(row["MaxFavorableRate"])
							});
					}
				}
				if (dic == null)
					dic = new Dictionary<int, SerialJiangJiaNewsSummary>();
				CacheManager.InsertCache(cacheKey, dic, 10);
			}
			return dic;
		}

		/// <summary>
		/// 车型降价新闻汇总数据
		/// key=子品牌id，value=车款列表
		/// </summary>
		public List<CarJiangJiaNewsSummary> GetCarJiangJiaNewsSummary(int serialId, int cityId)
		{
			if (serialId < 1 || cityId < 0) return null;

			string cacheKey = string.Format("GetCarJiangJiaNewsSummary_{0}_{1}", serialId.ToString(), cityId.ToString());
			List<CarJiangJiaNewsSummary> dic = CacheManager.GetCachedData(cacheKey) as List<CarJiangJiaNewsSummary>;
			if (dic == null)
			{
				DataSet ds = newsDll.GetCarJiangJiaNewsSummary(serialId, cityId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dic = new List<CarJiangJiaNewsSummary>(ds.Tables[0].Rows.Count);
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						//CarId, MaxFavorablePrice, MaxFavorableRate
						dic.Add(new CarJiangJiaNewsSummary()
						{
							CarId = Convert.ToInt32(row["CarId"]),
							MaxFavorablePrice = ConvertHelper.GetDecimal(row["MaxFavorablePrice"]),
							MaxFavorableRate = ConvertHelper.GetDecimal(row["MaxFavorableRate"])
						});
					}
				}
				if (dic == null)
					dic = new List<CarJiangJiaNewsSummary>();
				CacheManager.InsertCache(cacheKey, dic, 10);
			}
			return dic;
		}

		/// <summary>
		/// 取车型的全国降价
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllCarJiangJia()
		{
			string cacheKey = "CarNewsBll_GetAllCarJiangJia";
			Dictionary<int, string> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, string>;
			if (dic == null)
			{
				dic = new Dictionary<int, string>();
				DataSet ds = newsDll.GetAllCarJiangJia();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int carid = int.Parse(dr["CarId"].ToString());
						decimal maxP = decimal.Parse(dr["MaxFavorablePrice"].ToString());
						if (!dic.ContainsKey(carid))
						{
							dic.Add(carid, maxP.ToString("F2") + "万");
						}
					}
				}
				CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
			}
			return dic;
		}

		/// <summary>
		/// 取子品牌的全国降价
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllSerialJiangJia()
		{
			string cacheKey = "CarNewsBll_GetAllSerialJiangJia";
			Dictionary<int, string> dic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, string>;
			if (dic == null)
			{
				dic = new Dictionary<int, string>();
				DataSet ds = newsDll.GetAllSerialJiangJia();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int csid = int.Parse(dr["SerialId"].ToString());
						decimal maxP = decimal.Parse(dr["MaxFavorablePrice"].ToString());
						if (!dic.ContainsKey(csid))
						{
							dic.Add(csid, maxP.ToString("F2") + "万");
						}
					}
				}
				CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
			}
			return dic;
		}
		/// <summary>
		/// 获取降价新闻，每个城市取固定条目
		/// </summary>
		public List<News> GetJiangJiaNewsByEveryCityTop(string serialIds, int top)
		{
			if (string.IsNullOrEmpty(serialIds) || top < 1) return null;

			List<News> result = null;
			DataSet ds = newsDll.GetJiangJiaNewsByEveryCityTop(serialIds, top);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(ds.Tables[0].Rows.Count);
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					result.Add(new News()
					{
						CityId = ConvertHelper.GetInteger(row["cityid"].ToString()),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						PageUrl = row["NewsUrl"].ToString(),
						RelatedMainSerialID = ConvertHelper.GetInteger(row["serialid"].ToString())
					});
				}
			}
			return result;
		}

		/// <summary>
		/// 取子品牌某个城市的经销商报价 前几 按经销商权重排序 Table[0]:经销商数量 Table[1]:经销商报价
		/// </summary>
		/// <param name="top">前几位</param>
		/// <param name="csid">子品牌ID</param>
		/// <param name="cityid">城市ID</param>
		/// <returns></returns>
		public DataSet GetDealerPriceByCsIDAndCityID(int top, int csid, int cityid)
		{
			return newsDll.GetDealerPriceByCsIDAndCityID(top, csid, cityid);
		}

		#endregion

		/// <summary>
		/// 批量获取指定分类列表的子品牌新闻
		/// </summary>
		public Dictionary<CarNewsType, List<News>> GetTopSerialNewsByCarNewsTypes(int serialId, int top, List<CarNewsType> types)
		{
			if (serialId < 1 || top < 1 || types == null || types.Count < 1) return null;

			DataSet ds = newsDll.GetTopSerialNewsByCarNewsTypes(serialId, top, types);

			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
				return null;

			Dictionary<CarNewsType, List<News>> result = new Dictionary<CarNewsType, List<News>>(types.Count);

			DataTable newsTable = ds.Tables[0];

			foreach (CarNewsType type in types)
			{
				DataRow[] rows = newsTable.Select(string.Format("CarNewsTypeId=" + (int)type));
				if (rows.Length < 1)
					continue;

				List<News> news = new List<News>(rows.Length);
				foreach (DataRow row in rows)
				{
					news.Add(new News()
					{
						Title = row["title"].ToString(),
						FaceTitle = row["FaceTitle"].ToString(),
						PageUrl = row["FilePath"].ToString()
					});
				}
				result.Add(type, news);
			}

			return result;
		}
		/// <summary>
		/// 获取子品牌新闻 根据分类
		/// </summary>
		/// <param name="serialId">子品牌id</param>
		/// <param name="categoryId">新闻分类</param>
		/// <param name="top">前几条</param>
		/// <returns></returns>
		public List<NewsForSerialSummaryEntity> GetSerialNewsByCategoryId(int serialId, int categoryId, int top)
		{
			string cacheKey = string.Format("Car_NewsBll_SerialNewsByCategoryId_{0}_{1}_{2}", serialId, categoryId, top);
			object objUCarPrice = CacheManager.GetCachedData(cacheKey);
			if (objUCarPrice != null)
				return (List<NewsForSerialSummaryEntity>)objUCarPrice;

			List<NewsForSerialSummaryEntity> list = new List<NewsForSerialSummaryEntity>();
			DataSet ds = newsDll.GetSerialNewsByCategoryId(serialId, categoryId, top);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					list.Add(new NewsForSerialSummaryEntity()
					{
						CmsNewsId = ConvertHelper.GetInteger(dr["CmsNewsId"]),
						Title = ConvertHelper.GetString(dr["title"]),
						FaceTitle = ConvertHelper.GetString(dr["facetitle"]),
						Picture = ConvertHelper.GetString(dr["Picture"]),
						FilePath = ConvertHelper.GetString(dr["FilePath"]),
						PublishTime = ConvertHelper.GetDateTime(dr["PublishTime"])
					});
				}
				CacheManager.InsertCache(cacheKey, list, WebConfig.CachedDuration);
			}
			return list;
		}
		/// <summary>
		/// 获取同时关联2个子品牌的新闻
		/// </summary>
		/// <param name="serialIdArray">子品牌 ID </param>
		/// <param name="categoryId">新闻分类</param>
		/// <param name="top"></param>
		/// <returns></returns>
		public DataSet GetRelatedMoreSerialNewsData(int[] serialIdArray, int categoryId, int top)
		{
			return newsDll.GetRelatedMoreSerialNewsData(serialIdArray, categoryId, top);
		}

		/// <summary>
		/// 获取评测新闻信息
		/// </summary>
		/// <param name="newsId"></param>
		/// <returns></returns>
		public SerialPingCeNews GetSerialPingCeNews(int newsId)
		{
			SerialPingCeNews newsEntity = null;
			string cacheKey = "Car_CarNewsBLL_SerialPingCeNews_" + newsId.ToString();
			object pingCeNewByNewID = null;
			CacheManager.GetCachedData(cacheKey, out pingCeNewByNewID);
			if (pingCeNewByNewID == null)
			{
				try
				{
					string newsJson = CommonFunction.GetContentByUrl(string.Format(WebConfig.SerialPingCeDataNew, newsId));
					if (string.IsNullOrWhiteSpace(newsJson))
					{
						return newsEntity;
					}
					newsEntity = new SerialPingCeNews();
					JObject o = JObject.Parse(newsJson);
					newsEntity.NewsId = ConvertHelper.GetInteger(o["newsId"]);
					newsEntity.Title = ConvertHelper.GetString(o["title"]);
					newsEntity.ShortTitle = ConvertHelper.GetString(o["shortTitle"]);
					newsEntity.UniqueId = ConvertHelper.GetString(o["uniqueId"]);
					newsEntity.Url = ConvertHelper.GetString(o["url"]);
					newsEntity.Author = ConvertHelper.GetString(o["author"]);
					newsEntity.PublishTime = ConvertHelper.GetDateTime(o["publishTime"]);
					newsEntity.CategoryId = ConvertHelper.GetInteger(o["categoryId"]);
					newsEntity.CopyRight = ConvertHelper.GetInteger(o["copyright"]);
					newsEntity.Source = new SerialPingceNewsSource()
					{
						Id = ConvertHelper.GetInteger(o["source"]["id"]),
						Name = ConvertHelper.GetString(o["source"]["name"]),
						Url = ConvertHelper.GetString(o["source"]["url"])
					};
					newsEntity.Editor = new SerialPingceNewsEditor()
					{
						Id = ConvertHelper.GetInteger(o["editor"]["id"]),
						Name = ConvertHelper.GetString(o["editor"]["name"]),
						Url = ConvertHelper.GetString(o["editor"]["url"])
					};
					List<SerialPingceNewsPageContent> pageContentList = new List<SerialPingceNewsPageContent>();
					int page = 1;
					foreach (var item in o["pages"])
					{
						SerialPingceNewsPageContent pageContent = new SerialPingceNewsPageContent();
						//pageContent.CarId = ConvertHelper.GetInteger(item["carId"]);
						pageContent.PageIndex = page;
						pageContent.Title = ConvertHelper.GetString(item["title"]);
						pageContent.Content = ConvertHelper.GetString(item["content"]);
						pageContentList.Add(pageContent);
						page++;
					}
					newsEntity.PageContent = pageContentList;
					CacheManager.InsertCache(cacheKey, newsEntity, WebConfig.CachedDuration);
				}
				catch(Exception ex)
				{
					CommonFunction.WriteLog(ex.ToString());
				}
			}
			else
			{
				newsEntity = (SerialPingCeNews)pingCeNewByNewID;
			}
			return newsEntity;
		}
	}
}
