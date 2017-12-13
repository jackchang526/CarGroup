using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.MongoDB;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitAuto.CarChannel.DAL
{
    public class EvaluationDal
    {
        /// <summary>
        /// 根据车系ID获取车款的最新评测数据
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <returns></returns>
        public DataSet GetByStyleEvaluation(int evaluationId)
        {
            string sql = @"WITH  result
                                 AS ( SELECT TOP 1
                                                sjb.ModelId, se.StyleId,se.Id,sjb.StyleName,sjb.ModelName,sjb.[Year]
                                       FROM     dbo.StyleEvaluation se
                                                LEFT JOIN dbo.StyleJoinBrand sjb ON sjb.StyleId = se.StyleId
                                       WHERE    se.Id = @evaluationId
                                                AND se.IsRemoved = 0
                                       ORDER BY se.CreateTime DESC
                                     )
                                 SELECT  sp.GroupId,spg.Name as GroupNmae, sp.Name,r.ModelId,r.StyleId,r.StyleName,r.ModelName,r.[Year],
                                        spv.EvaluationId, spv.PropertyId, spv.PropertyValue,sp.Unit
                                 FROM    [dbo].[StylePropertyValue] spv
                                        JOIN result r ON r.Id = spv.EvaluationId
                                        LEFT JOIN [dbo].[StyleProperty] sp
                                        ON sp.Id = spv.PropertyId 
                                        LEFT JOIN StylePropertyGroup spg ON sp.GroupId=spg.Id
                                 ORDER BY spg.OrderId,sp.GroupId,sp.OrderId";
            SqlParameter[] parameters = {
                                            new SqlParameter("@evaluationId",SqlDbType.Int)
                                        };
            parameters[0].Value = evaluationId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.Text, sql, parameters);
            return ds;
        }

        public DataSet GetTestBaseEntityById(int evaluationId)
        {
            const string sql = @"SELECT sjb.*,se.[Id],se.[UserName] ,se.[IsRefitted] ,se.[Site] ,se.[Weather] ,se.[WeatherDesc] ,se.[Wind] ,se.[Kilometers] ,se.[EditorsName],se.[EvaluatingTime] ,
                                               se.[Temperature] ,se.[EquipmentOperator] ,se.[Type] ,se.[IsStandard],se.[Status],se.[Remarks]
							FROM    [dbo].[StyleEvaluation] as se
                            LEFT JOIN [dbo].[StyleJoinBrand] AS sjb  ON se.StyleId = sjb.StyleId
							WHERE   Id = @Id";

            SqlParameter[] parameters = {
                                            new SqlParameter("@Id",SqlDbType.Int)
                                        };
            parameters[0].Value = evaluationId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.Text, sql, parameters);
            return ds;
        }

        public T GetOne<T>(IMongoQuery query)
        {
            return MongoDBHelper.GetOne<T>(query);
        }

        public T GetOne<T>(IMongoQuery query, string[] fields, Dictionary<string, int> sortdic = null)
        {
            SortByBuilder sort = null;
            if (sortdic != null && sortdic.Count > 0)
            {
                foreach (string key in sortdic.Keys)
                {
                    if (sortdic[key] > 0)
                    {
                        sort = sort == null ? SortBy.Ascending(key) : sort.Ascending(key);
                    }
                    else
                    {
                        sort = sort == null ? SortBy.Descending(key) : sort.Descending(key);
                    }
                }
            }
            return MongoDBHelper.GetOne<T>(query, fields, sort);
        }        

        public List<T> GetPingCeAllList<T>(IMongoQuery query, int top, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            SortByBuilder sort = null;
            if (sortdic != null && sortdic.Count > 0)
            {
                foreach (string key in sortdic.Keys)
                {
                    if (sortdic[key] > 0)
                    {
                        sort = sort == null ? SortBy.Ascending(key) : sort.Ascending(key);
                    }
                    else
                    {
                        sort = sort == null ? SortBy.Descending(key) : sort.Descending(key);
                    }
                }
            }
            return MongoDBHelper.GetAll<T>(query, top, sort, fields);
        }

        public List<T> GetPingCeList<T>(IMongoQuery query, int index, int pageSize, out int total, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            string obj_key = "list_" + index + "_" + pageSize;
            string total_key = "total_list_" + index + "_" + pageSize;
            var obj = CacheManager.GetCachedData(obj_key);
            if (obj != null)
            {
                total= (int)CacheManager.GetCachedData(total_key);
                return (List<T>)obj;
            }
            else
            {
                SortByBuilder sort = null;
                if (sortdic != null && sortdic.Count > 0)
                {
                    foreach (string key in sortdic.Keys)
                    {
                        if (sortdic[key] > 0)
                        {
                            sort = sort == null ? SortBy.Ascending(key) : sort.Ascending(key);
                        }
                        else
                        {
                            sort = sort == null ? SortBy.Descending(key) : sort.Descending(key);
                        }
                    }
                }
                List<T> list= MongoDBHelper.GetAll<T>(query, index, pageSize, out total, sort, fields);
                CacheManager.InsertCache(obj_key, list, WebConfig.CachedDuration);
                CacheManager.InsertCache(total_key, total, WebConfig.CachedDuration);
                return null;
            }            
        }

        /// <summary>
        /// 根据MogonDB中的评测ID列表获取评测数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<int, PingCeEntity> GetEvaluationDate(List<int> list)
        {
            Dictionary<int, PingCeEntity> dic = new Dictionary<int, PingCeEntity>();
            try
            {
                foreach (int id in list)
                {
                    string key = "eid_" + id;
                    var obj = CacheManager.GetCachedData(key);
                    if (obj != null)
                    {
                        dic.Add(id, (PingCeEntity)obj);
                    }
                    else
                    {
                        string sql = @"SELECT se.[Id]
                                      ,se.[StyleId]
		                              ,sv0.PropertyValue AS Fuel
		                              ,sv1.PropertyValue AS BrakingDistance
		                              ,sv2.PropertyValue AS Acceleration	
		                              ,sjb.Year
		                              ,sjb.ModelDisplayName
		                              ,sjb.StyleName,sjb.FuelType        
                            FROM[dbo].[StyleEvaluation] se 
                            LEFT JOIN[CarsEvaluationData].[dbo].[StylePropertyValue] AS sv0 ON sv0.EvaluationId=se.Id AND sv0.PropertyId= 80 
                            LEFT JOIN [CarsEvaluationData].[dbo].[StylePropertyValue] AS sv1 ON sv1.EvaluationId= se.Id AND sv1.PropertyId= 11 
                            LEFT JOIN [CarsEvaluationData].[dbo].[StylePropertyValue] AS sv2 ON sv2.EvaluationId= se.Id AND sv2.PropertyId= 132 
                            LEFT JOIN [dbo].[StyleJoinBrand] AS sjb ON sjb.StyleId= se.StyleId WHERE se.Id= @evaluationId";
                        SqlParameter[] param = {
                                new SqlParameter("@evaluationId",SqlDbType.Int)
                                   };
                        param[0].Value = id;
                        DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.Text, sql, param);
                        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                PingCeEntity item = new PingCeEntity();
                                item.Acceleration = dr["Acceleration"] != null ? ConvertHelper.GetDouble(dr["Acceleration"]) : 0;
                                item.Fuel = dr["Fuel"] != null ? ConvertHelper.GetDouble(dr["Fuel"]) : 0;
                                item.BrakingDistance = dr["BrakingDistance"] != null ? ConvertHelper.GetDouble(dr["BrakingDistance"]) : 0;
                                item.EvaluationId = ConvertHelper.GetInteger(dr["Id"]);
                                item.Year = ConvertHelper.GetInteger(dr["Year"]);
                                item.ModelDisplayName = dr["ModelDisplayName"].ToString();
                                item.StyleName = dr["StyleName"].ToString();
                                item.FuelType = dr["FuelType"].ToString();
                                dic.Add(item.EvaluationId, item);
                                CacheManager.InsertCache(key, item, WebConfig.CachedDuration);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                CommonFunction.WriteLog(ex.ToString());
            }
            return dic;
        }

        /// <summary>
        /// 根据MogonDB中的评测ID列表获取评测数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private Dictionary<int, PingCeEntity> GetEvaluationDate_Test(List<int> list)
        {
            Dictionary<int, PingCeEntity> dic = new Dictionary<int, PingCeEntity>();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("EvaluationId", typeof(int));
                foreach (int item in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["EvaluationId"] = item;
                    dt.Rows.Add(dr);
                }
                SqlParameter[] param = {
                                new SqlParameter("@evaluationIdList",SqlDbType.Structured)
                                   };
                param[0].Value = dt;
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.StoredProcedure, "[dbo].[proc_SE_SPV_Select]", param);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PingCeEntity item = new PingCeEntity();
                        item.Acceleration = dr["Acceleration"] != null ? ConvertHelper.GetDouble(dr["Acceleration"]) : 0;
                        item.Fuel = dr["Fuel"] != null ? ConvertHelper.GetDouble(dr["Fuel"]) : 0;
                        item.BrakingDistance = dr["BrakingDistance"] != null ? ConvertHelper.GetDouble(dr["BrakingDistance"]) : 0;
                        item.EvaluationId = ConvertHelper.GetInteger(dr["Id"]);
                        item.Year = ConvertHelper.GetInteger(dr["Year"]);
                        item.ModelDisplayName = dr["ModelDisplayName"].ToString();
                        item.StyleName = dr["StyleName"].ToString();
                        item.FuelType = dr["FuelType"].ToString();
                        dic.Add(item.EvaluationId, item);
                    }
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                CommonFunction.WriteLog(ex.ToString());
            }
            return dic;
        }
    }
}