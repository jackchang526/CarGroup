using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Enum;


using System.Linq;
using System.Web;


namespace BitAuto.CarChannel.DAL
{
    /// <summary>
    /// 车型新闻库访问层
    /// </summary>
    public class CarNewsDll
    {

        #region 增加新闻数量 add by sk 20117-07-05


        public int GetSerialNewsCount(int serialId, int[] CategoryIdList)
        {
            int result = 0;
            if (serialId <= 0 || !CategoryIdList.Any())
            {
                return result;
            }

            string sql = @"SELECT  COUNT(1)
                            FROM    dbo.Car_SerialNewsV2 sn WITH ( NOLOCK ) 
			                            INNER JOIN func_splitid_clustered(@CategoryId,',') c ON c.c1 =sn.CategoryId
                            WHERE   sn.SerialId = @SerialId
                                    AND sn.CopyRight = 0";

            SqlParameter[] param = {
                new SqlParameter("@SerialId",SqlDbType.Int),
                new SqlParameter("@CategoryId",SqlDbType.VarChar)
            };

            param[0].Value = serialId;
            param[1].Value = string.Join(",", CategoryIdList);
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, param));
        }



        public int GetNewsCount(int[] serialIdList, int[] CategoryIdList)
        {
            int result = 0;
            if (!serialIdList.Any() || !CategoryIdList.Any())
            {
                return result;
            }

            string sql = @"SELECT  COUNT(1)
                            FROM    dbo.Car_SerialNewsV2 sn
                                    INNER JOIN dbo.func_splitid_clustered(@SerialIds, ',') st ON sn.SerialId = st.c1
                                    INNER JOIN dbo.func_splitid_clustered(@CategoryIds, ',') ct ON sn.CategoryId = ct.c1
                            WHERE   sn.CopyRight = 0";

            SqlParameter[] param = {
                new SqlParameter("@SerialIds",SqlDbType.VarChar),
                new SqlParameter("@CategoryIds",SqlDbType.VarChar)
            };

            param[0].Value = string.Join(",", serialIdList);
            param[1].Value = string.Join(",", CategoryIdList);
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, param));
        }

        #endregion


        /// <summary>
        /// 获取厂商新闻列表
        /// </summary>
        public DataSet GetProducerNews(int producerId, int carNewsTypeId, int pageSize, int pageIndex, out int rowCount)
        {
            rowCount = 0;
            if (producerId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Direction = ParameterDirection.Output;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@ProducerId",producerId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetProducerNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取厂商新闻列表
        /// </summary>
        public DataSet GetTopProducerNews(int producerId, int carNewsTypeId, int top)
        {
            if (producerId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;


            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@ProducerId",producerId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"SELECT TOP (@Top) CmsNewsId, CategoryId, Title, FilePath, PublishTime, SerialId
FROM ProducerNews
WHERE ProducerId=@ProducerId AND CarNewsTypeId=@CarNewsTypeId ORDER BY PublishTime DESC", param);
            return ds;
        }
        /// <summary>
        /// 获取主品牌新闻列表
        /// </summary>
        public DataSet GetMasterBrandNews(int masterBrandId, int[] arrCategoryIds, int pageSize, int pageIndex, ref int recordCount)
        {
            SqlParameter[] param = {
                                       new SqlParameter("@PageSize",SqlDbType.Int),
                                   new SqlParameter("@PageIndex",SqlDbType.Int),
                                   new SqlParameter("@SerialIds",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar),
                                   new SqlParameter("@RecordCount",SqlDbType.Int)
                                   };
            int[] arrSerialIds = GetSerialIdByMaster(masterBrandId);
            param[0].Value = pageSize;
            param[1].Value = pageIndex;
            param[2].Value = arrSerialIds == null ? "" : string.Join(",", arrSerialIds);
            param[3].Value = string.Join(",", arrCategoryIds);
            param[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_CarNewsV2_GetNewsForPage]", param);
            recordCount = ConvertHelper.GetInteger(param[4].Value);
            return ds;
        }
        public int[] GetCarNewsType(int carNewsType)
        {
            string cacheKey = string.Format("CarNewsType_{0}", carNewsType);
            object cacheObj = HttpContext.Current.Cache.Get(cacheKey);
            if (cacheObj != null)
            {
                return cacheObj as int[];
            }
            string sql = @"SELECT CmsCategoryId
							FROM    [dbo].[CarNewsTypeDef]
							WHERE   CarNewsTypeId = @CarNewsTypeId";
            SqlParameter[] param = {
                                       new SqlParameter("@CarNewsTypeId",SqlDbType.Int)
                                   };

            param[0].Value = carNewsType;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int[] result = ds.Tables[0].AsEnumerable().Select(row => ConvertHelper.GetInteger(row["CmsCategoryId"])).ToArray();
                if (result != null && result.Length > 0)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                    return result;
                }
            }
            return null;
        }
        public int[] GetCarNewsType(List<int> carNewsTypeIdList)
        {
            string cacheKey = string.Format("CarNewsType_{0}", string.Join("_", carNewsTypeIdList));
            object cacheObj = HttpContext.Current.Cache.Get(cacheKey);
            if (cacheObj != null)
            {
                return cacheObj as int[];
            }
            SqlParameter[] param = {
                                      new SqlParameter("@CarNewsTypeIdList",SqlDbType.Structured)
                                   };
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
            carNewsTypeIdList.ForEach(id =>
            {
                DataRow dr = dt.NewRow();
                dr["id"] = id;
                dt.Rows.Add(dr);
            });
            param[0].Value = dt;

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_GetCategoryIdsByTypeIds]", param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int[] result = ds.Tables[0].AsEnumerable().Select(row => ConvertHelper.GetInteger(row["CmsCategoryId"])).ToArray();
                if (result != null && result.Length > 0)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                    return result;
                }
            }
            return null;
        }
        public int[] GetSerialIdByMaster(int masterId)
        {
            string cacheKey = string.Format("GetSerialIdByMaster_{0}", masterId);
            object cacheObj = HttpContext.Current.Cache.Get(cacheKey);
            if (cacheObj != null)
            {
                return cacheObj as int[];
            }

            string sql = @"SELECT  cs.cs_Id
							FROM    dbo.Car_Serial cs
									LEFT JOIN dbo.Car_Brand cb ON cs.cb_Id = cb.cb_Id
									LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_Id = cmbr.cb_Id
									LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_Id = cmb.bs_Id
							WHERE   cs.IsState = 1
									AND cmb.bs_Id = @MasterId";
            SqlParameter[] param = {
                                       new SqlParameter("@MasterId",SqlDbType.Int)
                                   };

            param[0].Value = masterId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int[] result = ds.Tables[0].AsEnumerable().Select(row => ConvertHelper.GetInteger(row["cs_Id"])).ToArray();
                if (result != null && result.Length > 0)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                    return result;
                }
            }
            return null;
        }
        public int[] GetSerialIdByBrand(int brandId)
        {
            string cacheKey = string.Format("GetSerialIdByBrand_{0}", brandId);
            object cacheObj = HttpContext.Current.Cache.Get(cacheKey);
            if (cacheObj != null)
            {
                return cacheObj as int[];
            }

            string sql = @"SELECT  cs.cs_Id FROM    dbo.Car_Serial cs	LEFT JOIN dbo.Car_Brand cb ON cs.cb_Id = cb.cb_Id
							WHERE   cs.IsState = 1 AND cb.cb_Id = @BrandId";
            SqlParameter[] param = {
                                       new SqlParameter("@BrandId",SqlDbType.Int)
                                   };

            param[0].Value = brandId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int[] result = ds.Tables[0].AsEnumerable().Select(row => ConvertHelper.GetInteger(row["cs_Id"])).ToArray();
                if (result != null && result.Length > 0)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                    return result;
                }
            }
            return null;
        }
        public int[] GetSerialIdByCarLevel(string levelName)
        {
            string cacheKey = string.Format("GetSerialIdByCarLevel_{0}", levelName);
            object cacheObj = HttpContext.Current.Cache.Get(cacheKey);
            if (cacheObj != null)
            {
                return cacheObj as int[];
            }

            string sql = @"SELECT  CS_ID  FROM CAR_SERIAL WHERE CS_CARLEVEL=@LevelName AND ISSTATE=1";
            SqlParameter[] param = {
                                       new SqlParameter("@LevelName",SqlDbType.NVarChar)
                                   };

            param[0].Value = levelName;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int[] result = ds.Tables[0].AsEnumerable().Select(row => ConvertHelper.GetInteger(row["CS_ID"])).ToArray();
                if (result != null && result.Length > 0)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, result, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
                    return result;
                }
            }
            return null;
        }

        //public DataSet GetMasterBrandNews(int masterBrandId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int rowCount)
        //{
        //    if (masterBrandId <= 0 || carNewsTypeIdList.Count <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

        //    SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
        //    outRowCount.Direction = ParameterDirection.InputOutput;

        //    SqlParameter[] param = new SqlParameter[]{
        //        new SqlParameter("@PageSize",pageSize),
        //        new SqlParameter("@PageIndex",pageIndex),
        //        new SqlParameter("@MasterBrandId",masterBrandId),
        //        new SqlParameter("@CarNewsTypeIdList",SqlDbType.Structured),
        //        outRowCount
        //    };

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
        //    carNewsTypeIdList.ForEach(id =>
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["id"] = id;
        //        dt.Rows.Add(dr);
        //    });

        //    param[3].Value = dt;

        //    DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_MasterNews_GetDataByTypeIdsForPage]", param);
        //    rowCount = ConvertHelper.GetInteger(outRowCount.Value);
        //    return ds;
        //}
        /// <summary>
        /// 获取主品牌新闻列表
        /// </summary>
        public DataSet GetTopMasterBrandNews(int masterBrandId, int carNewsTypeId, int top)
        {
            if (masterBrandId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@MasterBrandId",masterBrandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"SELECT TOP (@Top) CmsNewsId,CategoryId,Title,FilePath,PublishTime,SerialId 
FROM MasterBrandNews 
WHERE MasterBrandId=@MasterBrandId AND CarNewsTypeId=@CarNewsTypeId 
ORDER BY PublishTime DESC", param);
            return ds;
        }
        /// <summary>
        /// 获取主品牌焦点新闻及新闻列表
        /// </summary>
        //        public DataSet GetTopMasterBrandFocusNews(int masterBrandId, int carNewsTypeId, int newsBlockOrderTypeId, int top)
        //        {
        //            if (masterBrandId <= 0 || carNewsTypeId <= 0 || top <= 0 || newsBlockOrderTypeId <= 0) return null;

        //            SqlParameter[] param = new SqlParameter[]{
        //                new SqlParameter("@Top",top),
        //                new SqlParameter("@MasterBrandId",masterBrandId),
        //                new SqlParameter("@CarNewsTypeId",carNewsTypeId),

        //                new SqlParameter("@BlockType",newsBlockOrderTypeId),
        //                new SqlParameter("@NowTime",DateTime.Now.Date),
        //                new SqlParameter("@StartOrder",1),
        //                new SqlParameter("@EndOrder",6),
        //            };
        //            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
        //@"SELECT TOP (@Top) mbn.CmsNewsId,mbn.CategoryId,mbn.FilePath,mbn.PublishTime,mbn.SerialId,n.FaceTitle
        //FROM MasterBrandNews mbn LEFT JOIN dbo.News n ON mbn.CarNewsId=n.ID 
        //WHERE MasterBrandId=@MasterBrandId AND CarNewsTypeId=@CarNewsTypeId 
        //ORDER BY PublishTime DESC
        //
        //SELECT TOP (@EndOrder) CmsNewsId, Title, FaceTitle, FilePath, PublishTime, CategoryId, OrderNumber 
        //from NewsBlockOrder 
        //WHERE [ObjId]=@MasterBrandId AND BlockType=@BlockType AND StartTime<=@NowTime AND EndTime>=@NowTime AND OrderNumber BETWEEN @StartOrder AND @EndOrder
        //ORDER BY OrderNumber ASC
        //", param);
        //            return ds;
        //        }
        /// <summary>
        /// Date:2016-8-31
        /// Description:新闻数据改版
        /// By: zf
        /// </summary>
        /// <param name="masterBrandId"></param>
        /// <param name="carNewsTypeId"></param>
        /// <param name="newsBlockOrderTypeId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public DataSet GetTopMasterBrandFocusNews(int masterBrandId, int carNewsTypeId, int newsBlockOrderTypeId, int top)
        {
            if (masterBrandId <= 0 || carNewsTypeId <= 0 || top <= 0 || newsBlockOrderTypeId <= 0) return null;
            int[] arrSerialIds = GetSerialIdByMaster(masterBrandId);
            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@SerialIds",arrSerialIds == null ? "" : string.Join(",", arrSerialIds)),
                new SqlParameter("@CategoryIds",arrCategoryIds==null?"": string.Join(",", arrCategoryIds)),
                 new SqlParameter("@MasterBrandId",masterBrandId),
                new SqlParameter("@BlockType",newsBlockOrderTypeId),
                new SqlParameter("@NowTime",DateTime.Now.Date),
                new SqlParameter("@StartOrder",1),
                new SqlParameter("@EndOrder",6),
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"SELECT TOP ( @Top ) n.NewsId AS CmsNewsId,  n.Title, n.ShortTitle AS FaceTitle, n.Summary AS Content,  n.ImageConverUrl AS Picture,
                        n.Url AS FilePath, n.CategoryId, n.PublishTime,sn.SerialId,'' AS FirstPicUrl
                FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@SerialIds, ',') st ON sn.SerialId = st.c1
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds,
                                                              ',') ct ON sn.CategoryId = ct.c1
                where sn.CopyRight = 0
                ORDER BY sn.PublishTime DESC

SELECT TOP (@EndOrder) CmsNewsId, Title, FaceTitle, FilePath, PublishTime, CategoryId, OrderNumber 
from NewsBlockOrder 
WHERE [ObjId]=@MasterBrandId AND BlockType=@BlockType AND StartTime<=@NowTime AND EndTime>=@NowTime AND OrderNumber BETWEEN @StartOrder AND @EndOrder
ORDER BY OrderNumber ASC
", param);
            return ds;
        }
        /// <summary>
        /// 获取主品牌新闻列表，关联新闻表
        /// </summary>
        public DataSet GetMasterBrandNewsAllData(int masterBrandId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        {
            if (masterBrandId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = rowCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@MasterBrandId",masterBrandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetMasterBrandNewsAllDataForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取主品牌新闻列表，关联新闻表
        /// </summary>
        public DataSet GetTopMasterBrandNewsAllData(int masterBrandId, int carNewsTypeId, int top)
        {
            if (masterBrandId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@MasterBrandId",masterBrandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@" SELECT TOP (@Top) mbn.CategoryId,mbn.SerialId, mbn.PublishTime,mbn.CmsNewsId
 ,n.Title, n.FilePath, n.CreativeType, n.Picture, n.FaceTitle, n.Duration ,'' AS FirstPicUrl
 FROM MasterBrandNews AS mbn  
 INNER JOIN News n ON n.ID=mbn.CarNewsId   
 WHERE mbn.MasterBrandId=@MasterBrandId AND mbn.CarNewsTypeId=@CarNewsTypeId   
 ORDER BY mbn.PublishTime DESC  ", param);
            return ds;
        }
        /// <summary>
        /// 获取品牌新闻列表
        /// </summary>
        public DataSet GetBrandNews(int brandId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        {
            if (brandId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = rowCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetBrandNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        public DataSet GetBrandNews(int brandId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int rowCount)
        {
            if (brandId <= 0 || carNewsTypeIdList.Count <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = rowCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@CarNewsTypeIdList",SqlDbType.Structured),
                outRowCount
            };

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
            carNewsTypeIdList.ForEach(id =>
            {
                DataRow dr = dt.NewRow();
                dr["id"] = id;
                dt.Rows.Add(dr);
            });

            param[3].Value = dt;

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_BrandNews_GetDataByTypeIdsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// Description:新闻改版 
        /// date:2016-9-1
        /// by :zf
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="arrCategoryIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetBrandNews(int brandId, int[] arrCategoryIds, int pageSize, int pageIndex, ref int rowCount)
        {
            if (brandId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter[] param = {
                                       new SqlParameter("@PageSize",SqlDbType.Int),
                                   new SqlParameter("@PageIndex",SqlDbType.Int),
                                   new SqlParameter("@SerialIds",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar),
                                   new SqlParameter("@RecordCount",SqlDbType.Int)
                                   };
            int[] arrSerialIds = GetSerialIdByBrand(brandId);
            param[0].Value = pageSize;
            param[1].Value = pageIndex;
            param[2].Value = arrSerialIds == null ? "" : string.Join(",", arrSerialIds);
            param[3].Value = arrCategoryIds == null ? "" : string.Join(",", arrCategoryIds);
            param[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_CarNewsV2_GetNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(param[4].Value);
            return ds;
        }
        /// <summary>
        /// 获取品牌新闻列表
        /// </summary>
        public DataSet GetTopBrandNews(int brandId, int carNewsTypeId, int top)
        {
            if (brandId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @" SELECT TOP (@Top) CmsNewsId,CategoryId,Title,FilePath,PublishTime,SerialId 
 FROM BrandNews  
 WHERE BrandId=@BrandId AND CarNewsTypeId=@CarNewsTypeId 
 ORDER BY PublishTime DESC", param);
            return ds;
        }
        /// <summary>
        /// 获取品牌新闻排序列表及新闻列表
        /// </summary>
        //        public DataSet GetTopBrandFocusNews(int brandId, int carNewsTypeId, int newsBlockOrderTypeId, int top)
        //        {
        //            if (brandId <= 0 || carNewsTypeId <= 0 || top <= 0 || newsBlockOrderTypeId <= 0) return null;

        //            SqlParameter[] param = new SqlParameter[]{
        //                new SqlParameter("@Top",top),
        //                new SqlParameter("@BrandId",brandId),
        //                new SqlParameter("@CarNewsTypeId",carNewsTypeId),

        //                new SqlParameter("@BlockType",newsBlockOrderTypeId),
        //                new SqlParameter("@NowTime",DateTime.Now.Date),
        //                new SqlParameter("@StartOrder",1),
        //                new SqlParameter("@EndOrder",6),
        //            };
        //            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,  //,bn.Title
        //                @"SELECT TOP (@Top) bn.CmsNewsId,bn.CategoryId,n.FaceTitle,bn.FilePath,bn.PublishTime,bn.SerialId 
        // FROM BrandNews bn   LEFT JOIN dbo.News n ON bn.CarNewsId=n.ID 
        // WHERE BrandId=@BrandId AND CarNewsTypeId=@CarNewsTypeId 
        // ORDER BY PublishTime DESC
        //
        //SELECT TOP (@EndOrder) CmsNewsId, Title, FaceTitle, FilePath, PublishTime, CategoryId, OrderNumber 
        //from NewsBlockOrder 
        //WHERE [ObjId]=@BrandId AND BlockType=@BlockType AND StartTime<=@NowTime AND EndTime>=@NowTime AND OrderNumber BETWEEN @StartOrder AND @EndOrder
        //ORDER BY OrderNumber ASC"
        //                , param);
        //            return ds;
        //        }
        /// <summary>
        /// Date:2016-8-31
        /// Description:新闻改版 
        /// By: zf
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="carNewsTypeId"></param>
        /// <param name="newsBlockOrderTypeId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public DataSet GetTopBrandFocusNews(int brandId, int carNewsTypeId, int newsBlockOrderTypeId, int top)
        {
            if (brandId <= 0 || carNewsTypeId <= 0 || top <= 0 || newsBlockOrderTypeId <= 0) return null;
            int[] arrSerialIds = GetSerialIdByBrand(brandId);
            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@SerialIds",arrSerialIds == null ? "" : string.Join(",", arrSerialIds)),
                new SqlParameter("@CategoryIds",arrCategoryIds == null ? "" :string.Join(",", arrCategoryIds)),

                new SqlParameter("@BlockType",newsBlockOrderTypeId),
                new SqlParameter("@NowTime",DateTime.Now.Date),
                new SqlParameter("@StartOrder",1),
                new SqlParameter("@EndOrder",6),
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,  //,bn.Title
                @"SELECT TOP ( @Top ) n.NewsId AS CmsNewsId,  n.Title, n.ShortTitle AS FaceTitle, n.Summary AS Content,  n.ImageConverUrl AS Picture,
                        n.Url AS FilePath, n.CategoryId, n.PublishTime,sn.SerialId
                FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@SerialIds, ',') st ON sn.SerialId = st.c1
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds,
                                                              ',') ct ON sn.CategoryId = ct.c1
                where sn.CopyRight = 0
                ORDER BY sn.PublishTime DESC

SELECT TOP (@EndOrder) CmsNewsId, Title, FaceTitle, FilePath, PublishTime, CategoryId, OrderNumber 
from NewsBlockOrder 
WHERE [ObjId]=@BrandId AND BlockType=@BlockType AND StartTime<=@NowTime AND EndTime>=@NowTime AND OrderNumber BETWEEN @StartOrder AND @EndOrder
ORDER BY OrderNumber ASC"
                , param);
            return ds;
        }
        /// <summary>
        /// 获取品牌新闻列表，关联新闻表
        /// </summary>
        public DataSet GetBrandNewsAllData(int brandId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        {
            if (brandId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = rowCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetBrandNewsAllDataForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取品牌新闻列表，关联新闻表
        /// </summary>
        public DataSet GetTopBrandNewsAllData(int brandId, int carNewsTypeId, int top)
        {
            if (brandId <= 0 || carNewsTypeId <= 0 || top <= 0) { return null; }

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@BrandId",brandId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@" SELECT TOP (@Top) mbn.CategoryId,mbn.SerialId, mbn.PublishTime,mbn.CmsNewsId
  ,n.Title, n.FilePath, n.CreativeType, n.Picture, n.FaceTitle, n.Duration
 FROM BrandNews AS mbn  
 INNER JOIN News n ON n.ID=mbn.CarNewsId   
 WHERE mbn.BrandId=@BrandId AND mbn.CarNewsTypeId=@CarNewsTypeId   
 ORDER BY mbn.PublishTime DESC  ", param);
            return ds;
        }
        /// <summary>
        /// 获取级别新闻列表
        /// </summary>
        public DataSet GetLevelNews(int levelId, int carNewsTypeId, int pageSize, int pageIndex, out int rowCount)
        {
            rowCount = 0;
            if (levelId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) return null;

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Direction = ParameterDirection.Output;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@LevelId",levelId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetLevelNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取级别新闻列表
        /// </summary>
        public DataSet GetTopLevelNews(int levelId, int carNewsTypeId, int top)
        {
            if (levelId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@LevelId",levelId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @" SELECT TOP (@Top) CmsNewsId,CategoryId,Title,FilePath,PublishTime,SerialId 
 FROM LevelNews 
 WHERE LevelId=@LevelId AND CarNewsTypeId=@CarNewsTypeId 
 ORDER BY PublishTime DESC", param);
            return ds;
        }
        /// <summary>
        /// 获取级别新闻列表old
        /// </summary>
        //        public DataSet GetLevelNewsWithComment(int levelId, int carNewsTypeId, int top)
        //        {
        //            if (levelId <= 0 || carNewsTypeId <= 0 || top <= 0) return null; 

        //            SqlParameter[] param = new SqlParameter[]{
        //                new SqlParameter("@Top",top),
        //                new SqlParameter("@LevelId",levelId),
        //                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
        //            };
        //            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
        //                @" SELECT TOP (@Top) ln.CmsNewsId,ln.CategoryId,ln.Title,ln.FilePath,ln.PublishTime,ln.SerialId,n.Picture,n.FaceTitle,n.Duration,n.Author,n.CommentNum,n.SourceName,n.[Content],n.EditorName,n.EditorId,n.SourceUrl,n.FirstPicUrl 
        //                     FROM LevelNews as ln
        //                     INNER JOIN News n ON n.ID = ln.CarNewsId
        //                     WHERE LevelId=@LevelId AND CarNewsTypeId=@CarNewsTypeId AND n.CreativeType =0
        //                     ORDER BY PublishTime DESC", param);
        //            return ds;
        //        }
        /// <summary>
        /// 获取级别新闻列表new 
        /// </summary>
        public DataSet GetLevelNewsWithComment(int levelId, int carNewsTypeId, int top)
        {
            if (levelId <= 0 || carNewsTypeId <= 0 || top <= 0) return null;

            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);
            return GetLevelNewsData(top, arrCategoryIds);
        }
        /// <summary>
        /// 获取级别评论与导购类新闻列表 old
        /// </summary>
        //        public DataSet GetLevelNewsWithComment(int levelId, List<int> carNewsTypeId, int top)
        //        {
        //            if (levelId <= 0 || carNewsTypeId.Count <= 0 || top <= 0) return null;

        //            List<SqlParameter> sqlParas = new List<SqlParameter>();
        //            sqlParas.Add(new SqlParameter("@Top", top));
        //            sqlParas.Add(new SqlParameter("@LevelId", levelId));

        //            DataTable dt = new DataTable();
        //            dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
        //            carNewsTypeId.ForEach(id =>
        //            {
        //                DataRow dr = dt.NewRow();
        //                dr["id"] = id;
        //                dt.Rows.Add(dr);
        //            });
        //            SqlParameter spType = new SqlParameter("@CarNewsTypeId", SqlDbType.Structured);
        //            spType.TypeName = "TY_TB_IdList";
        //            spType.Value = dt;
        //            sqlParas.Add(spType);

        //            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
        //                @" SELECT TOP (@Top) ln.CmsNewsId,ln.CategoryId,ln.Title,ln.FilePath,ln.PublishTime,ln.SerialId,n.Picture,n.FaceTitle,n.Duration,n.Author,n.CommentNum,n.SourceName,n.[Content],n.EditorName,n.EditorId,n.SourceUrl,n.FirstPicUrl 
        //                     FROM LevelNews as ln
        //                     INNER JOIN News n ON n.ID = ln.CarNewsId
        //                     WHERE LevelId=@LevelId AND ln.PublishTime>=dateadd(year,-1,getdate()) AND CarNewsTypeId IN (SELECT  Id
        //                                              FROM  @CarNewsTypeId)
        //                     ORDER BY n.CommentNum DESC", sqlParas.ToArray());
        //            return ds;
        //        }
        /// <summary>
        /// 获取级别评论与导购类新闻列表 new
        /// </summary>
        public DataSet GetLevelNewsWithComment(int levelId, List<int> carNewsTypeId, int top)
        {
            if (levelId <= 0 || carNewsTypeId.Count <= 0 || top <= 0) return null;

            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);
            return GetLevelNewsData(top, arrCategoryIds);
        }
        public DataSet GetLevelNewsData(int top, int[] arrCategoryIds)
        {
            string sql = @"SELECT TOP ( @Top ) n.NewsId AS CmsNewsId,n.Title, n.ShortTitle AS FaceTitle, n.Summary AS Content,  n.ImageConverUrl AS Picture,
                        n.Url AS FilePath, n.CategoryId, n.PublishTime,sn.SerialId,n.Author,n.SourceName,n.EditorName,n.EditorId,n.SourceUrl,n.CommentCount AS CommentNum,'' AS FirstPicUrl
                FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@SerialIds, ',') st ON sn.SerialId = st.c1
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds, ',') ct ON sn.CategoryId = ct.c1
                where sn.CopyRight = 0
                ORDER BY sn.PublishTime DESC";

            SqlParameter[] param = {
                                       new SqlParameter("@Top",SqlDbType.Int),
                                   new SqlParameter("@SerialIds",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar)
                                   };
            int[] arrSerialIds = GetSerialIdByCarLevel("SUV");
            param[0].Value = top;
            param[1].Value = arrSerialIds == null ? "" : string.Join(",", arrSerialIds);
            param[2].Value = arrCategoryIds == null ? "" : string.Join(",", arrCategoryIds);

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sql, param);
            return ds;
        }
        /// <summary>
        /// 得到城市行情新闻
        /// </summary>
        public DataSet GetTopCityNews(int serialId, int cityId, int top)
        {
            if (serialId < 1 || cityId < 0 || top < 1) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@SerialId", serialId),
                new SqlParameter("@CityId", cityId), 
				// new SqlParameter("@CarNewsTypeId", (int)CarNewsType.hangqing),
				new SqlParameter("@Top", top)
            };

            //            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
            //@"SELECT TOP(@Top) n.CmsNewsId,n.Title, n.FaceTitle,n.FilePath,n.PublishTime
            //  FROM CityNews cn 
            //  INNER JOIN News n ON n.CmsNewsId=cn.CmsNewsId 
            //  INNER JOIN SerialNews sn ON n.ID=sn.CarNewsId
            //WHERE cn.RelateCityId=@CityId AND sn.SerialId=@SerialId AND sn.CarNewsTypeId=@CarNewsTypeId AND cn.IsHQ=1
            //ORDER BY sn.PublishTime DESC", param);

            // modified by chengl Jul.19.2012

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"select top (@Top) * from dbo.VCityHangqingNews with (noexpand)
where RelateCityId=@CityId and SerialId=@SerialId
order by publishtime desc", param);
        }
        /// <summary>
        /// 获取子品牌新闻数量
        /// </summary>
        public int GetSerialNewsCount(int serialId, int year, CarNewsType carNewsType)
        {
            if (serialId > 0)
            {
                string selectSql = string.Empty;
                int[] arrCategoryIds = GetCarNewsType((int)carNewsType);

                SqlParameter[] param = null;
                if (year > 0)
                {
                    selectSql = "SELECT COUNT(1) FROM Car_SerialNewsV2 sn INNER JOIN dbo.func_splitid_clustered(@CategoryIds,',') ct ON sn.CategoryId = ct.c1 WHERE sn.SerialId = @SerialId  and sn.CopyRight = 0 AND YearType=@Year";
                    param = new SqlParameter[] {
                        new SqlParameter("@CategoryIds",arrCategoryIds==null?"":string.Join(",", arrCategoryIds)),
                        new SqlParameter("@SerialId", serialId), new SqlParameter("@Year", year) };
                }
                else
                {
                    selectSql = "SELECT COUNT(1) FROM Car_SerialNewsV2 sn INNER JOIN dbo.func_splitid_clustered(@CategoryIds,',') ct ON sn.CategoryId = ct.c1 WHERE sn.SerialId = @SerialId  and sn.CopyRight = 0";
                    param = new SqlParameter[] {
                        new SqlParameter("@CategoryIds",arrCategoryIds==null?"": string.Join(",", arrCategoryIds)),
                        new SqlParameter("@SerialId", serialId) };
                }
                return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(WebConfig.CarDataUpdateConnectionString, CommandType.Text, selectSql, param));
            }
            return 0;
        }
        /// <summary>
        /// 获取子品牌新闻列表
        /// </summary>
        public DataSet GetSerialNews(int serialId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        {
            if (serialId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = rowCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取子品牌新闻列表
        /// </summary>
        public DataSet GetTopSerialNews(int serialId, int carNewsTypeId, int top)
        {
            if (serialId < 1 || carNewsTypeId < 1 || top < 1) { return null; }

            int[] arrCategoryIds = GetCarNewsType((int)carNewsTypeId);
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CategoryIds",arrCategoryIds==null?"": string.Join(",",arrCategoryIds))
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @" SELECT TOP ( @Top )  n.NewsId AS CmsNewsId,n.CategoryId,sn.CarId, n.Title, n.Url AS FilePath,n.PublishTime,n.ImageConverUrl AS Picture,
                    n.ShortTitle AS FaceTitle,n.Author, n.CommentCount AS CommentNum,n.Content,n.SourceName,n.SourceUrl,n.EditorId,n.EditorName,n.EditorUrl,'' AS firstPicUrl,sn.YearType
                        FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds,',') ct ON sn.CategoryId = ct.c1
                WHERE   sn.SerialId = @SerialId and  sn.CopyRight = 0 
                ORDER BY sn.PublishTime DESC", param);
            return ds;
        }

        /// <summary>
        /// 获取子品牌新闻列表 不分类
        /// </summary>
        public DataSet GetTopSerialNews(int serialId, int top)
        {
            if (serialId < 1 || top < 1) { return null; }

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@SerialId",serialId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @" SELECT TOP ( @Top )  n.NewsId AS CmsNewsId,n.Title, n.Url AS FilePath,n.ShortTitle AS FaceTitle
                        FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                   WHERE   sn.SerialId = @SerialId and  sn.CopyRight = 0 
                   ORDER BY sn.PublishTime DESC", param);
            return ds;
        }

        ///// <summary>
        ///// 获取子品牌新闻列表，关联新闻表
        ///// </summary>
        //public DataSet GetSerialNewsAllData(int serialId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        //{
        //    if (serialId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

        //    SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
        //    outRowCount.Value = rowCount;
        //    outRowCount.Direction = ParameterDirection.InputOutput;

        //    SqlParameter[] param = new SqlParameter[]{
        //        new SqlParameter("@PageSize",pageSize),
        //        new SqlParameter("@PageIndex",pageIndex),
        //        new SqlParameter("@SerialId",serialId),
        //        new SqlParameter("@CarNewsTypeId",carNewsTypeId),
        //        outRowCount
        //    };
        //    DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialNewsAllDataForPage]", param);
        //    rowCount = ConvertHelper.GetInteger(outRowCount.Value);
        //    return ds;
        //}
        ///// <summary>
        ///// 获取多个分类新闻文章
        ///// </summary>
        ///// <param name="serialId"></param>
        ///// <param name="carNewsTypeIdList">分类值List</param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="rowCount"></param>
        ///// <returns></returns>
        //public DataSet GetSerialNewsAllData(int serialId, List<int> carNewsTypeIdList, int pageSize, int pageIndex, ref int rowCount)
        //{
        //    if (serialId <= 0 || carNewsTypeIdList.Count <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

        //    SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
        //    //outRowCount.Value = rowCount;
        //    outRowCount.Direction = ParameterDirection.InputOutput;

        //    SqlParameter[] param = new SqlParameter[]{
        //        new SqlParameter("@PageSize",pageSize),
        //        new SqlParameter("@PageIndex",pageIndex),
        //        new SqlParameter("@SerialId",serialId),
        //        new SqlParameter("@CarNewsTypeIdList",SqlDbType.Structured),
        //        outRowCount
        //    };

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
        //    carNewsTypeIdList.ForEach(id =>
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["id"] = id;
        //        dt.Rows.Add(dr);
        //    });

        //    param[3].Value = dt;
        //    DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_SerialNews_GetDataByTypeIdsForPage]", param);
        //    rowCount = ConvertHelper.GetInteger(outRowCount.Value);
        //    return ds;
        //}
        public DataSet GetSerialNewsAllData(int serialId, int carNewsTypeId, int pageSize, int pageIndex, ref int rowCount)
        {
            if (serialId <= 0 || carNewsTypeId <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter[] param = {
                                       new SqlParameter("@PageSize",SqlDbType.Int),
                                   new SqlParameter("@PageIndex",SqlDbType.Int),
                                   new SqlParameter("@SerialId",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar),
                                   new SqlParameter("@RecordCount",SqlDbType.Int)
                                   };
            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);
            param[0].Value = pageSize;
            param[1].Value = pageIndex;
            param[2].Value = serialId;
            param[3].Value = arrCategoryIds == null ? "" : string.Join(",", arrCategoryIds);
            param[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_CarNewsV2_GetSerialNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(param[4].Value);
            return ds;
        }
        /// <summary>
        /// 获取多个分类新闻文章
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="carNewsTypeIdList">分类值List</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetSerialNewsAllData(int serialId, int[] arrCategoryIds, int pageSize, int pageIndex, ref int rowCount)
        {
            if (serialId <= 0 || arrCategoryIds.Length <= 0 || pageSize <= 0 || pageIndex <= 0) { rowCount = 0; return null; }

            SqlParameter[] param = {
                                       new SqlParameter("@PageSize",SqlDbType.Int),
                                   new SqlParameter("@PageIndex",SqlDbType.Int),
                                   new SqlParameter("@SerialId",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar),
                                   new SqlParameter("@RecordCount",SqlDbType.Int)
                                   };
            param[0].Value = pageSize;
            param[1].Value = pageIndex;
            param[2].Value = serialId;
            param[3].Value = arrCategoryIds == null ? "" : string.Join(",", arrCategoryIds);
            param[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_CarNewsV2_GetSerialNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(param[4].Value);
            return ds;
        }
        /// <summary>
        /// 获取子品牌新闻列表，关联新闻表
        /// </summary>
        //        public DataSet GetTopSerialNewsAllData(int serialId, int carNewsTypeId, int top)
        //        {
        //            if (serialId <= 0 || carNewsTypeId <= 0 || top <= 0) { return null; }

        //            SqlParameter[] param = new SqlParameter[]{
        //                new SqlParameter("@Top",top),
        //                new SqlParameter("@SerialId",serialId),
        //                new SqlParameter("@CarNewsTypeId",carNewsTypeId)
        //            };
        //            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
        //@" SELECT TOP (@Top) sn.CmsNewsId,sn.CategoryId,sn.CarId,n.Title,n.FilePath,n.PublishTime  
        //  , n.Picture, n.FaceTitle, n.Duration, n.Author, n.CommentNum,n.SourceName,n.[Content],n.EditorName,n.EditorId,n.SourceUrl,n.FirstPicUrl  
        //   FROM SerialNews AS sn  
        // INNER JOIN News n ON n.ID=sn.CarNewsId   
        // WHERE sn.SerialId=@SerialId AND sn.CarNewsTypeId=@CarNewsTypeId   
        // ORDER BY sn.PublishTime DESC  ", param);
        //            return ds;
        //        }
        /// <summary>
        /// Date:2016-9-1
        /// Description:新闻改版 
        /// By:zf
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="carNewsTypeId"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public DataSet GetTopSerialNewsAllData(int serialId, int carNewsTypeId, int top)
        {
            if (serialId <= 0 || carNewsTypeId <= 0 || top <= 0) { return null; }

            int[] arrCategoryIds = GetCarNewsType(carNewsTypeId);
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CategoryIds",arrCategoryIds==null?"": string.Join(",", arrCategoryIds))
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@" SELECT TOP (@Top) n.NewsId AS CmsNewsId,sn.CategoryId,sn.CarId,  n.Title, n.ShortTitle AS FaceTitle, n.Summary AS Content,  n.ImageConverUrl AS Picture,
                        n.Url AS FilePath, n.PublishTime,n.CommentCount AS CommentNum,n.SourceName,n.SourceUrl,n.EditorId,n.EditorName,'' AS FirstPicUrl
                FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds,',') ct ON sn.CategoryId = ct.c1
                where  sn.SerialId = @SerialId and sn.CopyRight = 0
                ORDER BY sn.PublishTime DESC  ", param);
            return ds;
        }
        /// <summary>
        /// 获取子品牌年款新闻列表，关联新闻表
        /// </summary>
        public DataSet GetSerialYearNewsAllData(int serialId, int year, int carNewsTypeId, int pageSize, int pageIndex, out int rowCount)
        {
            rowCount = 0;
            if (serialId < 1 || year < 1 || carNewsTypeId < 1 || pageSize < 1 || pageIndex < 1) return null;

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Direction = ParameterDirection.Output;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CarYear",year),
                new SqlParameter("@CarNewsTypeId",carNewsTypeId),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialYearNewsAllDataForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取多个子品牌年款新闻
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="year"></param>
        /// <param name="carNewsTypeIdList">新闻类型列表</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetSerialYearNewsAllData(int serialId, int year, List<int> carNewsTypeIdList, int pageSize, int pageIndex, out int rowCount)
        {
            rowCount = 0;
            if (serialId < 1 || year < 1 || carNewsTypeIdList.Count < 1 || pageSize < 1 || pageIndex < 1) return null;

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Direction = ParameterDirection.Output;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CarYear",year),
                new SqlParameter("@CarNewsTypeIdList",SqlDbType.Structured),
                outRowCount
            };
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", Type.GetType("System.Int32")));
            carNewsTypeIdList.ForEach(id =>
            {
                DataRow dr = dt.NewRow();
                dr["id"] = id;
                dt.Rows.Add(dr);
            });

            param[4].Value = dt;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_SerialNews_GetYearDataByTypeIdsForPage]", param);
            rowCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// Date:2016-9-1 
        /// Desctipion:新闻改版           
        /// By:zf
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="year"></param>
        /// <param name="arrCategoryIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public DataSet GetSerialYearNewsAllData(int serialId, int year, int[] arrCategoryIds, int pageSize, int pageIndex, out int rowCount)
        {
            rowCount = 0;
            if (serialId < 1 || year < 1 || pageSize < 1 || pageIndex < 1) return null;

            SqlParameter[] param = {
                                       new SqlParameter("@PageSize",SqlDbType.Int),
                                   new SqlParameter("@PageIndex",SqlDbType.Int),
                                   new SqlParameter("@SerialId",SqlDbType.VarChar),
                                   new SqlParameter("@CategoryIds",SqlDbType.VarChar),
                                    new SqlParameter("@YearType",SqlDbType.Int),
                                   new SqlParameter("@RecordCount",SqlDbType.Int)
                                   };

            param[0].Value = pageSize;
            param[1].Value = pageIndex;
            param[2].Value = serialId;
            param[3].Value = arrCategoryIds == null ? "" : string.Join(",", arrCategoryIds);
            param[4].Value = year;
            param[5].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[SP_CarNewsV2_GetSerialYearNewsForPage]", param);
            rowCount = ConvertHelper.GetInteger(param[5].Value);
            return ds;
        }
        /// <summary>
        /// 获取子品牌年款新闻列表，关联新闻表
        /// </summary>
        public DataSet GetTopSerialYearNewsAllData(int serialId, int year, int carNewsTypeId, int top)
        {
            if (serialId < 1 || year < 1 || carNewsTypeId < 1 || top < 1) return null;

            int[] arrCategoryIds = GetCarNewsType((int)carNewsTypeId);
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@Top",top),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@CarYear",year),
                new SqlParameter("@CategoryIds",arrCategoryIds==null?"": string.Join(",",arrCategoryIds))
            };

            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @" SELECT TOP ( @Top )  n.NewsId AS CmsNewsId,n.CategoryId,sn.CarId, n.Title, n.Url AS FilePath,n.PublishTime,n.ImageConverUrl AS Picture,
                n.ShortTitle AS FaceTitle,n.Author, n.CommentCount AS CommentNum, n.Summary AS Content,n.SourceName,n.SourceUrl,n.EditorId,n.EditorName,n.EditorUrl,'' AS firstPicUrl,sn.YearType
                        FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@CategoryIds,',') ct ON sn.CategoryId = ct.c1
                WHERE   sn.SerialId = @SerialId and  sn.CopyRight = 0 AND sn.YearType=@CarYear 
                ORDER BY sn.PublishTime DESC", param);
            return ds;
        }
        /// <summary>
        /// 获取子品牌城市行情
        /// </summary>
        public DataSet GetSerialCityNewsAllData(int serialId, int cityId, int pageSize, int pageIndex, ref int newsCount)
        {
            if (serialId <= 0 || cityId <= 0 || pageSize <= 0 || pageIndex <= 0) { newsCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = newsCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@RelateCityId",cityId),
                new SqlParameter("@CarNewsTypeId",(int)CarNewsType.hangqing),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialCityNewsAllDataForPage]", param);
            newsCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取子品牌省行情
        /// </summary>
        public DataSet GetSerialProvinceNewsAllData(int serialId, int provinceId, int pageSize, int pageIndex, ref int newsCount)
        {
            if (serialId <= 0 || provinceId <= 0 || pageSize <= 0 || pageIndex <= 0) { newsCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = newsCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@RelateProvinceId",provinceId),
                new SqlParameter("@CarNewsTypeId",(int)CarNewsType.hangqing),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialProvinceNewsAllDataForPage]", param);
            newsCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取子品牌省市新闻数
        /// </summary>
        public DataSet GetSerialCityNewsCount(int serialId)
        {
            if (serialId < 1) return null;
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @"SELECT CityId,Num from SerialCityNewsNumber WHERE SerialId=@SerialId ORDER BY Num DESC",
                new SqlParameter("@SerialId", serialId));
        }
        /// <summary>
        /// 获取省行情新闻数
        /// </summary>
        public DataSet GetProviceCityNewsCount()
        {
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, @"SELECT CityId, Num FROM ProvinceCityNewsNumber");
        }

        /// <summary>
        /// 获取省行情
        /// </summary>
        public DataSet GetProvinceNewsAllData(int provinceId, int pageSize, int pageIndex, ref int newsCount)
        {
            if (provinceId < 0 || pageSize <= 0 || pageIndex <= 0) { newsCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = newsCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@RelateProvinceId",provinceId),
                new SqlParameter("@CarNewsTypeId",(int)CarNewsType.hangqing),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetProvinceNewsAllDataForPage]", param);
            newsCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 获取城市行情
        /// </summary>
        public DataSet GetCityNewsAllData(int cityId, int pageSize, int pageIndex, ref int newsCount)
        {
            if (cityId <= 0 || pageSize <= 0 || pageIndex <= 0) { newsCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = newsCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@RelateCityId",cityId),
                new SqlParameter("@CarNewsTypeId",(int)CarNewsType.hangqing),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetCityNewsAllDataForPage]", param);
            newsCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }
        /// <summary>
        /// 合作站级别页首页新闻
        /// </summary>
        public DataSet GetCopperationLevelTopNews(int levelId)
        {
            if (levelId < 1) return null;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure,
                "[dbo].[GetCopperationLevelTopNewsForPage]", new SqlParameter("@LevelId", levelId));
        }

        /// <summary>
        /// 获取编辑评测试驾子品牌对应车型
        /// </summary>
        public DataSet GetEditorCommentCarId()
        {
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, "SELECT SerialId, CarId FROM EditorComment");
        }
        /// <summary>
        /// 获取品牌置换信息
        /// </summary>
        public DataSet GetBrandZhiHuanNews(int brandId, int cityId, int top)
        {
            SqlParameter[] param = new SqlParameter[4]{
                new SqlParameter("@top",top),
                new SqlParameter("@brandId", brandId),
                new SqlParameter("@cityId", cityId),
                new SqlParameter("@carNewsType", ((int)CarNewsType.zhihuan))
            };

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, @"GetBrandZhiHuanTopNews", param);
        }
        /// <summary>
        /// 获取子品牌城市置换数
        /// </summary>
        public DataSet GetSerialCityZhiHuanCount(int serialId)
        {
            if (serialId < 1) return null;
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
                @"SELECT CityId,Num from SerialZhiHuanNewsNumber WHERE SerialId=@SerialId",
                new SqlParameter("@SerialId", serialId));
        }
        /// <summary>
        /// 获取子品牌城市置换新闻
        /// </summary>
        public DataSet GetSerialCityZhiHuanNewsAllData(int serialId, int cityId, int pageSize, int pageIndex, ref int newsCount)
        {
            if (serialId <= 0 || cityId <= 0 || pageSize <= 0 || pageIndex <= 0) { newsCount = 0; return null; }

            SqlParameter outRowCount = new SqlParameter("@RecordCount", SqlDbType.Int, 4);
            outRowCount.Value = newsCount;
            outRowCount.Direction = ParameterDirection.InputOutput;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@SerialId",serialId),
                new SqlParameter("@RelateCityId",cityId),
                new SqlParameter("@CarNewsTypeId",(int)CarNewsType.zhihuan),
                outRowCount
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[dbo].[GetSerialCityZhiHuanNewsAllDataForPage]", param);
            newsCount = ConvertHelper.GetInteger(outRowCount.Value);
            return ds;
        }

        public DataSet GetTopCityZhiHuanNews(int serialId, int cityId, int top)
        {
            if (serialId < 1 || cityId < 0 || top < 1) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@SerialId", serialId),
                new SqlParameter("@CityId", cityId),
                new SqlParameter("@CarNewsTypeId", (int)CarNewsType.zhihuan),
                new SqlParameter("@Top", top)
            };

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"SELECT TOP(@Top) n.CmsNewsId,n.Title, n.FaceTitle,n.FilePath,n.PublishTime
  FROM CityNews cn 
  INNER JOIN News n ON n.CmsNewsId=cn.CmsNewsId 
  INNER JOIN SerialNews sn ON n.ID=sn.CarNewsId
WHERE cn.RelateCityId=@CityId AND sn.SerialId=@SerialId AND sn.CarNewsTypeId=@CarNewsTypeId AND cn.IsZH=1
ORDER BY sn.PublishTime DESC", param);
        }
        /// <summary>
        /// 获取品牌城市经销商置换行情
        /// </summary>
        public DataSet GetBrandZhiHuanDealerNews(int brandId, int cityId, int top)
        {
            if (brandId < 1 || cityId < 0 || top < 1) return null;

            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@BrandId", brandId),
                new SqlParameter("@CityId", cityId),
                new SqlParameter("@Top", top)
            };

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text,
@"SELECT TOP(@Top) NewsId,NewsTitle,NewsUrl,PublishTime FROM [DealerReplaceNews] WHERE BrandId=@BrandId AND CityId=@CityId ORDER BY PublishTime DESC", param);

        }

        /// <summary>
        /// 获取有置换信息的省份和城市列表
        /// </summary>
        public DataSet GetZhiHuanCityIds()
        {
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, "SELECT DISTINCT cityid FROM CarReplacement");
        }
        /// <summary>
        /// 获取置换品牌id列表
        /// </summary>
        public DataSet GetZhiHuanBrandIds(int cityId, int cityParentId)
        {
            string where = string.Empty;
            if (cityId > 0)
                where = " cityId=" + cityId.ToString();
            if (cityParentId > 0)
            {
                if (string.IsNullOrEmpty(where))
                    where = " cityId=" + cityParentId.ToString();
                else
                    where += " or cityId=" + cityParentId.ToString();
            }
            if (string.IsNullOrEmpty(where))
                return null;
            string select = "SELECT distinct serialid FROM CarReplacement WHERE" + where;
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, select);
        }

        /// <summary>
        /// 全部有置换数据的子品牌
        /// </summary>
        public DataSet GetZhiHuanSerials()
        {
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text
                , "SELECT DISTINCT SerialId FROM CarReplacementInfo");
        }

        #region 降价新闻
        /// <summary>
        /// 获取子品牌降价新闻
        /// </summary>
        /// <param name="serialId">子品牌id</param>
        /// <param name="cityId">省id|市id|0，0为全国</param>
        /// <param name="type">1=全国，2=省，3=市</param>
        public DataSet GetSerialJiangJiaTopNews(int serialId, int cityId, int top, Int16 type)
        {
            string spName = "GetSerialJiangJiaTopNews";
            SqlParameter[] sqlParams = SqlHelperParameterCache.GetCachedParameterSet(WebConfig.CarDataUpdateConnectionString, spName);
            if (sqlParams == null)
            {
                sqlParams = new SqlParameter[]{
                    new SqlParameter("@SerialId", SqlDbType.Int,4),
                    new SqlParameter("@CityId", SqlDbType.Int,4),
                    new SqlParameter("@Top", SqlDbType.Int,4),
                    new SqlParameter("@Type", SqlDbType.SmallInt, 2)
                };
                SqlHelperParameterCache.CacheParameterSet(WebConfig.CarDataUpdateConnectionString, spName, sqlParams);
            }
            sqlParams[0].Value = serialId;
            sqlParams[1].Value = cityId;
            sqlParams[2].Value = top;
            sqlParams[3].Value = type;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, spName, sqlParams);
        }
        /// <summary>
        /// 获取品牌降价新闻
        /// </summary>
        /// <param name="brandId">品牌id</param>
        /// <param name="cityId">省id|市id|0，0为全国</param>
        /// <param name="type">1=全国，2=省，3=市</param>
        public DataSet GetBrandJiangJiaTopNews(int brandId, int cityId, int top, Int16 type)
        {
            string spName = "GetBrandJiangJiaTopNews";
            SqlParameter[] sqlParams = SqlHelperParameterCache.GetCachedParameterSet(WebConfig.CarDataUpdateConnectionString, spName);
            if (sqlParams == null)
            {
                sqlParams = new SqlParameter[]{
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@CityId", SqlDbType.Int,4),
                    new SqlParameter("@Top", SqlDbType.Int,4),
                    new SqlParameter("@Type", SqlDbType.SmallInt, 2)
                };
                SqlHelperParameterCache.CacheParameterSet(WebConfig.CarDataUpdateConnectionString, spName, sqlParams);
            }
            sqlParams[0].Value = brandId;
            sqlParams[1].Value = cityId;
            sqlParams[2].Value = top;
            sqlParams[3].Value = type;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, spName, sqlParams);
        }
        /// <summary>
        /// 获取主品牌降价新闻
        /// </summary>
        /// <param name="masterId">主品牌id</param>
        /// <param name="cityId">省id|市id|0，0为全国</param>
        /// <param name="type">1=全国，2=省，3=市</param>
        public DataSet GetMasterBrandJiangJiaTopNews(int masterId, int cityId, int top, Int16 type)
        {
            string spName = "GetMasterBrandJiangJiaTopNews";
            SqlParameter[] sqlParams = SqlHelperParameterCache.GetCachedParameterSet(WebConfig.CarDataUpdateConnectionString, spName);
            if (sqlParams == null)
            {
                sqlParams = new SqlParameter[]{
                    new SqlParameter("@MasterId", SqlDbType.Int,4),
                    new SqlParameter("@CityId", SqlDbType.Int,4),
                    new SqlParameter("@Top", SqlDbType.Int,4),
                    new SqlParameter("@Type", SqlDbType.SmallInt, 2)
                };
                SqlHelperParameterCache.CacheParameterSet(WebConfig.CarDataUpdateConnectionString, spName, sqlParams);
            }
            sqlParams[0].Value = masterId;
            sqlParams[1].Value = cityId;
            sqlParams[2].Value = top;
            sqlParams[3].Value = type;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, spName, sqlParams);
        }
        /// <summary>
        /// 子品牌降价新闻汇总数据
        /// </summary>
        public DataSet GetSerialJiangJiaNewsSummary(int serialId)
        {
            string sqlTxt = "SELECT CityId, Num, MaxFavorablePrice, VendorNum, CarNum, MaxFavorableRate FROM JiangJiaNewsSummary WHERE SerialId=@SerialId";
            SqlParameter[] serialParam = SqlHelperParameterCache.GetCachedParameterSet(WebConfig.CarDataUpdateConnectionString, sqlTxt);
            if (serialParam == null)
            {
                serialParam = new SqlParameter[] { new SqlParameter("@SerialId", SqlDbType.Int, 4) };
                SqlHelperParameterCache.CacheParameterSet(WebConfig.CarDataUpdateConnectionString, sqlTxt, serialParam);
            }
            serialParam[0].Value = serialId;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqlTxt, serialParam);
        }

        /// <summary>
        /// 根据子品牌id、城市id，获取相关车款最高降幅与降额
        /// </summary>
        public DataSet GetCarJiangJiaNewsSummary(int serialId, int cityId)
        {
            string sqlTxt = "SELECT CarId, MaxFavorablePrice, MaxFavorableRate FROM JiangJiaNewsCarSummary WHERE SerialId=@SerialId AND CityId=@CityId";

            SqlParameter[] serialParams = SqlHelperParameterCache.GetCachedParameterSet(WebConfig.CarDataUpdateConnectionString, sqlTxt);
            if (serialParams == null)
            {
                serialParams = new SqlParameter[] {
                    new SqlParameter("@SerialId", SqlDbType.Int, 4),
                    new SqlParameter("@CityId", SqlDbType.Int, 4)
                };

                SqlHelperParameterCache.CacheParameterSet(WebConfig.CarDataUpdateConnectionString, sqlTxt, serialParams);
            }
            serialParams[0].Value = serialId;
            serialParams[1].Value = cityId;

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqlTxt, serialParams);
        }

        /// <summary>
        /// 取车型的全国降价 参数配置页使用
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarJiangJia()
        {
            string sql = @"SELECT CarId, MaxFavorablePrice 
								FROM  dbo.JiangJiaNewsCarSummary
								WHERE CityId=0 and MaxFavorablePrice>0";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.CarDataUpdateConnectionString
                , CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取子品牌的全国降价
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialJiangJia()
        {
            string sql = @"SELECT SerialId, Num, MaxFavorablePrice FROM JiangJiaNewsSummary 
									WHERE CityId=0 and MaxFavorablePrice>0";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.CarDataUpdateConnectionString
                , CommandType.Text, sql);
            return ds;
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
            string spName = "VendorPrice_GetByCsIDAndCityID";
            SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@Top", SqlDbType.Int, 4),
                    new SqlParameter("@CsID", SqlDbType.Int, 4),
                    new SqlParameter("@CityID", SqlDbType.Int, 4)
                };
            param[0].Value = top;
            param[1].Value = csid;
            param[2].Value = cityid;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                           WebConfig.CarDataUpdateConnectionString
                           , CommandType.StoredProcedure, spName, param);
            return ds;
        }

        #endregion
        /// <summary>
        /// 批量获取指定分类列表的子品牌新闻
        /// </summary>
        //public DataSet GetTopSerialNewsByCarNewsTypes(int serialId, int top, List<CarNewsType> types)
        //{
        //    if (serialId < 1 || top < 1 || types == null || types.Count < 1) return null;

        //    SqlParameter[] parameters = new SqlParameter[types.Count + 2];

        //    parameters[0] = new SqlParameter("@CsId", SqlDbType.Int) { Value = serialId };
        //    parameters[1] = new SqlParameter("@Top", SqlDbType.Int) { Value = top };

        //    string[] sqls = new string[types.Count];
        //    for (int i = 2, j = 0; i < parameters.Length; i++, j++)
        //    {
        //        CarNewsType type = types[j];
        //        parameters[i] = new SqlParameter("@" + type.ToString(), SqlDbType.Int) { Value = (int)type };
        //        sqls[j] = string.Format("select a.CarNewsTypeId, a.Title, a.FilePath, n.FaceTitle from (select top(@Top) CarNewsTypeId, Title,FilePath,CarNewsId from SerialNews where SerialId=@CsId and CarNewsTypeId=@{0} ORDER BY PublishTime DESC) as a join News n on n.ID=a.CarNewsId {1}"
        //            , type.ToString(), (j < types.Count - 1 ? " union all " : string.Empty));
        //    }

        //    return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, string.Join(string.Empty, sqls), parameters);
        //}
        public DataSet GetTopSerialNewsByCarNewsTypes(int serialId, int top, List<CarNewsType> types)
        {
            if (serialId < 1 || top < 1 || types == null || types.Count < 1) return null;

            SqlParameter[] parameters = new SqlParameter[types.Count + 2];

            parameters[0] = new SqlParameter("@SerialId", SqlDbType.Int) { Value = serialId };
            parameters[1] = new SqlParameter("@Top", SqlDbType.Int) { Value = top };

            string[] sqls = new string[types.Count];
            for (int i = 2, j = 0; i < parameters.Length; i++, j++)
            {
                CarNewsType type = types[j];
                int[] curArrCategoryIds = GetCarNewsType((int)type);
                parameters[i] = new SqlParameter("@" + type.ToString(), SqlDbType.VarChar) { Value = (curArrCategoryIds == null ? "" : string.Join(",", curArrCategoryIds)) };
                sqls[j] = string.Format(@" SELECT top(@Top) {0} as CarNewsTypeId, n.Title,n.ShortTitle AS FaceTitle,n.Url AS FilePath,sn.CarNewsId,sn.PublishTime
                        FROM    dbo.Car_SerialNewsV2 sn
                        INNER JOIN dbo.Car_NewsV2 n ON sn.CarNewsId = n.Id
                        INNER JOIN dbo.func_splitid_clustered(@{2},',') ct ON sn.CategoryId = ct.c1
                WHERE   sn.SerialId = @SerialId and  sn.CopyRight = 0
                 {1}"
                    , (int)type, (j < types.Count - 1 ? " union all " : string.Empty), type.ToString());
            }
            string orderBy = " ORDER BY sn.PublishTime DESC";
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, string.Join(string.Empty, sqls) + orderBy, parameters);
        }
        /// <summary>
        /// 获取降价新闻，每个城市取固定条目
        /// </summary>
        public DataSet GetJiangJiaNewsByEveryCityTop(string serialIds, int top)
        {
            if (string.IsNullOrEmpty(serialIds) || top < 1) return null;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("serialIds", SqlDbType.VarChar, -1){Value=serialIds}
                ,new SqlParameter("Top", SqlDbType.Int){Value=top}
            };

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "GetJiangJiaNewsByEveryCityTop", parms);
        }
        /// <summary>
        /// 获取子品牌新闻 根据分类
        /// </summary>
        /// <param name="serialId">子品牌id</param>
        /// <param name="categoryId">分类id</param>
        /// <param name="top">前几条</param>
        /// <returns></returns>
        public DataSet GetSerialNewsByCategoryId(int serialId, int categoryId, int top)
        {
            if (serialId <= 0 || top < 1) return null;

            int[] arrCmsCategoryIds = GetCarNewsType(categoryId);
            SqlParameter[] _params = new SqlParameter[]{
                new SqlParameter("@SerialId", SqlDbType.Int)  ,
                new SqlParameter("@CategoryIds", SqlDbType.VarChar) ,
                 new SqlParameter("@TopN", SqlDbType.Int)
            };
            _params[0].Value = serialId;
            _params[1].Value = string.Join(",", arrCmsCategoryIds);
            _params[2].Value = top;
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[SP_CarNewsV2_GetSerialNewsByCategoryId]", _params);
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
            if (serialIdArray.Length <= 0 || top < 1) return null;
            int[] arrCmsCategoryIds = GetCarNewsType(categoryId);
            SqlParameter[] _params = new SqlParameter[]{
                new SqlParameter("@SerialIds", SqlDbType.VarChar)  ,
                new SqlParameter("@CategoryIds", SqlDbType.VarChar) ,
                 new SqlParameter("@TopN", SqlDbType.Int)
            };

            _params[0].Value = string.Join(",", serialIdArray);
            _params[1].Value = string.Join(",", arrCmsCategoryIds);
            _params[2].Value = top;
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure, "[SP_CarNewsV2_GetRelatedMoreSerialNews]", _params);
        }
    }
}
