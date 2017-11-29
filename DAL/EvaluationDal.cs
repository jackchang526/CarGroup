using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.MongoDB;
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
            //IMongoSortBy sort = null;
            SortByBuilder sort = null;
            if (sortdic!=null&&sortdic.Count > 0)
            {
                List<string> asclist = new List<string>();
                List<string> desclist = new List<string>();
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
   
    }
}