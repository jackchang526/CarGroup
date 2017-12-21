using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetEvaluationTopRank 的摘要说明
    /// </summary>
    public class GetEvaluationTopRank : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];
            int propertyId = ConvertHelper.GetInteger(context.Request.QueryString["propertyId"]);
            string order = "ASC";
            string result = "{}";
            string message = "成功";
            int status = 1;
            List<EvaluationTopRank> list = new List<EvaluationTopRank>();
            Dictionary<string, List<EvaluationTopRank>> dic = new Dictionary<string, List<EvaluationTopRank>>();
            try
            {
                Dictionary<int, string> dicPara = new Dictionary<int, string>
                {
                    { 132, "ASC" },
                    { 11, "ASC" },
                    { 27, "DESC" },
                    { 22, "DESC" }
                };
                if (dicPara.Keys.Contains(propertyId))
                {
                    EvaluationBll evaluationBll = new EvaluationBll();
                    List<int> reportEvaluationIdList = evaluationBll.GetExistReportEvaluationId();
                    order = dicPara[propertyId];
                    string sql = GetSql(propertyId, order);
                    SqlParameter[] _params = {
                                         new SqlParameter("@propertyId",SqlDbType.Int)
                                     };
                    _params[0].Value = propertyId;
                    DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.Text, sql, _params);

                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EvaluationTopRank er = new EvaluationTopRank();
                            er.StyleId = ConvertHelper.GetInteger(dr["StyleId"]);
                            er.PropertyValue = ConvertHelper.GetDouble(dr["PropertyValue"]);
                            er.EvaluationId = ConvertHelper.GetInteger(dr["EvaluationId"]);
                            //er.PropertyId = ConvertHelper.GetInteger(dr["PropertyId"]);
                            er.StyleName = dr["StyleName"].ToString();
                            er.Year = ConvertHelper.GetInteger(dr["Year"]);
                            er.ModelDisplayName = dr["ModelDisplayName"].ToString();
                            er.ModelLevel = dr["ModelLevel"].ToString();
                            er.Unit = dr["Unit"].ToString();
                            int levelId = CarLevelDefine.GetLevelIdByName(er.ModelLevel);
                            if (reportEvaluationIdList != null && reportEvaluationIdList.Contains(er.EvaluationId))
                            {
                                er.IsExistReport = true;
                            }
                            else
                            {
                                er.IsExistReport = false;
                            }
                            er.ModelAllSpell = dr["ModelAllSpell"].ToString();
                            er.LevelSpell = CarLevelDefine.GetLevelSpellById(levelId);
                            list.Add(er);
                        }
                    }
                }
                else
                {
                    status = 2;
                    message = "参数错误";
                }
            }
            catch (Exception ex)
            {
                message = "接口错误";
                status = 0;
                CommonFunction.WriteLog(ex.ToString());
            }
            var obj = new
            {
                status= status,
                message= message,
                data= list                
            };
        result = JsonConvert.SerializeObject(obj);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
        }
        private static string GetSql()
        {
            return @"WITH result1 AS 
                                (
                                    SELECT  
                                    se.StyleId
                                    ,sv.PropertyValue
				                    ,sv.PropertyId
                                    ,ROW_NUMBER() OVER( PARTITION BY se.StyleId ORDER BY CAST(sv.PropertyValue AS FLOAT) ASC) Num
                                    ,se.Id AS EvaluationId										
                                    FROM [dbo].[StylePropertyValue] AS sv  
				                    LEFT JOIN [dbo].[StyleEvaluation] AS se ON sv.EvaluationId = se.Id and sv.PropertyId = 132 			   
                                    WHERE se.[Status]=2
                                )
	                    ,result2 AS 
		                    (
			                    SELECT  
			                    se.StyleId
			                    ,sv.PropertyValue
			                    ,sv.PropertyId
			                    ,ROW_NUMBER() OVER( PARTITION BY se.StyleId ORDER BY CAST(sv.PropertyValue AS FLOAT) ASC) Num
			                    ,se.Id AS EvaluationId      		
			                    FROM [dbo].[StylePropertyValue] AS sv  
			                    LEFT JOIN [dbo].[StyleEvaluation] AS se ON sv.EvaluationId = se.Id and sv.PropertyId = 11   
			                    WHERE se.[Status]=2
		                    )
	                    ,result3 AS 
                            (
                                SELECT 
                                se.StyleId
                                ,sv.PropertyValue
			                    ,sv.PropertyId
                                ,ROW_NUMBER() OVER( PARTITION BY se.StyleId ORDER BY CAST(sv.PropertyValue AS FLOAT) DESC) Num
                                ,se.Id AS EvaluationId    
                                FROM [dbo].[StylePropertyValue] AS sv  
			                    LEFT JOIN [dbo].[StyleEvaluation] AS se ON sv.EvaluationId = se.Id and sv.PropertyId = 27     
                                WHERE se.[Status]=2
                            )
	                    ,result4 AS 
		                    (
			                    SELECT  
			                    se.StyleId
			                    ,sv.PropertyValue
			                    ,sv.PropertyId
			                    ,ROW_NUMBER() OVER( PARTITION BY se.StyleId ORDER BY CAST(sv.PropertyValue AS FLOAT) DESC) Num
			                    ,se.Id AS EvaluationId				
			                    FROM [dbo].[StylePropertyValue] AS sv  
			                    LEFT JOIN [dbo].[StyleEvaluation] AS se ON sv.EvaluationId = se.Id and sv.PropertyId = 22 				  
			                    WHERE se.[Status]=2
		                    )
	                    ,resall as 
		                    (						
			                    SELECT TOP 5 
				                    res.StyleId 
				                    ,PropertyValue
				                    ,PropertyId
				                    ,EvaluationId					
				                    ,ROW_NUMBER() OVER(ORDER BY CAST(PropertyValue AS FLOAT) ASC ) RankNum  
			                    FROM result1 res WHERE Num=1
			                    UNION ALL
			                    SELECT TOP 5 
				                    res.StyleId 
				                    ,PropertyValue
				                    ,PropertyId
				                    ,EvaluationId					
				                    ,ROW_NUMBER() OVER(ORDER BY CAST(PropertyValue AS FLOAT) ASC ) RankNum  
			                    FROM result2 res WHERE Num=1
			                    UNION ALL
			                    SELECT TOP 5 
				                    res.StyleId 
				                    ,PropertyValue
				                    ,PropertyId
				                    ,EvaluationId					
				                    ,ROW_NUMBER() OVER(ORDER BY CAST(PropertyValue AS FLOAT) DESC ) RankNum  
			                    FROM result3 res WHERE Num=1
			                    UNION ALL
			                    SELECT TOP 5 
				                    res.StyleId 
				                    ,PropertyValue
				                    ,PropertyId
				                    ,EvaluationId					
				                    ,ROW_NUMBER() OVER(ORDER BY CAST(PropertyValue AS FLOAT) DESC ) RankNum  
			                    FROM result4 res WHERE Num=1
	                    )
	                    SELECT 
	                    rall.StyleId
	                    ,PropertyValue
	                    ,PropertyId
	                    ,EvaluationId 
	                    ,sjb.StyleName			                    
	                    ,sjb.ModelDisplayName
	                    ,sjb.Year
	                    ,sjb.ModelAllSpell
                        ,sjb.ModelLevel
	                    ,sp.Unit
	                    FROM resall rall 
	                    LEFT JOIN [dbo].[StyleJoinBrand] AS sjb ON sjb.StyleId=rall.StyleId
	                    LEFT JOIN [dbo].[StyleProperty] AS sp ON sp.Id=rall.PropertyId   ";
        }
        private static string GetSql(int propertyId, string order)
        {
            string tempSql = @"WITH result1 AS 
                            (
                                SELECT  
                                se.StyleId
                                ,sv.PropertyValue
                       ,sv.PropertyId
                                ,ROW_NUMBER() OVER( PARTITION BY se.StyleId ORDER BY CAST(sv.PropertyValue AS FLOAT) {0}) Num
                                ,se.Id AS EvaluationId										
                                FROM [dbo].[StylePropertyValue] AS sv  
                       INNER JOIN [dbo].[StyleEvaluation] AS se ON sv.EvaluationId = se.Id and sv.PropertyId = @propertyId			   
                                WHERE se.[Status]=2
                            )	
                         ,resall AS 
                      (						
                        SELECT
                         res.StyleId 
                         ,PropertyValue
                         ,PropertyId
                         ,EvaluationId				
                         ,Num
                        FROM result1 res WHERE Num=1			
                      )
                      SELECT TOP 5 
                      rall.StyleId
                      ,PropertyValue
                      ,PropertyId
                      ,EvaluationId 
                      ,sjb.StyleName			                    
                      ,sjb.ModelDisplayName
                      ,sjb.Year
                      ,sjb.ModelAllSpell
                      ,sjb.ModelLevel
                      ,sp.Unit
                      FROM resall rall 
                      LEFT JOIN [dbo].[StyleJoinBrand] AS sjb ON sjb.StyleId=rall.StyleId
                      LEFT JOIN [dbo].[StyleProperty] AS sp ON sp.Id=rall.PropertyId
                      ORDER BY  CAST(PropertyValue AS FLOAT) {0}";


            //string tempSql = @"WITH result AS 
            //( 
            //	SELECT   
            //	MAX(EvaluationId) EvaluationId, se.StyleId,
            //	MAX(CAST(spv.PropertyValue AS FLOAT)) PropertyValue
            //	FROM     dbo.StyleEvaluation se
            //	LEFT JOIN [dbo].[StylePropertyValue] spv ON se.Id = spv.EvaluationId AND spv.PropertyId = @propertyId
            //	WHERE    se.[Status] = 2
            //	GROUP BY StyleId
            //)
            //SELECT TOP 5
            //         result.StyleId
            //        ,ModelAllSpell
            //        ,PropertyValue
            //        ,EvaluationId
            //        ,sjb.StyleName
            //        ,sjb.ModelDisplayName
            //        ,sjb.Year
            //        ,sjb.ModelAllSpell
            //        ,sjb.ModelLevel
            //        ,sp.Unit
            //FROM    result
            //        LEFT JOIN dbo.StyleJoinBrand sjb ON sjb.StyleId = result.StyleId 
            //        LEFT JOIN [dbo].[StyleProperty] AS sp ON sp.Id=@propertyId
            //ORDER BY PropertyValue {0}";
            string sql = string.Format(tempSql, order);
            return sql;
        }
        private string GetTabText(int propertyId, double propertyValue, int level, string fuelType)
        {
            var tab = "";
            switch (propertyId)
            {
                case 11://冷刹车
                    if (propertyValue > 0 && propertyValue <= 36)
                    {
                        tab = "秒刹";
                    }
                    else if (propertyValue > 36 && propertyValue <= 40)
                    {
                        tab = "刹车灵敏";
                    }
                    else if (propertyValue > 40 && propertyValue <= 45)
                    {
                        tab = "一般般";
                    }
                    else if (propertyValue > 45 && propertyValue <= 47)
                    {
                        tab = "刹车略肉";
                    }
                    else
                    {
                        tab = "刹车太肉";
                    }
                    break;
                case 22://18米绕桩
                    if (level == 9)//level=9是跑车
                    {
                        if (propertyValue > 0 && propertyValue <= 60.9)
                        {
                            tab = "代步级";
                        }
                        else if (propertyValue >= 61 && propertyValue <= 62.9)
                        {
                            tab = "家用级";
                        }
                        else if (propertyValue >= 63 && propertyValue <= 64.9)
                        {
                            tab = "运动级";
                        }
                        else if (propertyValue >= 65 && propertyValue <= 66.9)
                        {
                            tab = "性能级";
                        }
                        else if (propertyValue >= 67)//&& propertyValue <= 71
                        {
                            tab = "赛道级";
                        }
                    }
                    else
                    {
                        if (propertyValue > 0 && propertyValue <= 57.9)
                        {
                            tab = "代步级";
                        }
                        else if (propertyValue >= 58 && propertyValue <= 59.9)
                        {
                            tab = "家用级";
                        }
                        else if (propertyValue >= 60 && propertyValue <= 61.9)
                        {
                            tab = "运动级";
                        }
                        else if (propertyValue >= 62 && propertyValue <= 63.9)
                        {
                            tab = "性能级";
                        }
                        else if (propertyValue >= 64)//&& propertyValue <= 68
                        {
                            tab = "赛道级";
                        }
                    }
                    break;
                case 27://110米变线
                    if (level == 9)
                    {
                        if (propertyValue > 0 && propertyValue <= 122.9)
                        {
                            tab = "寸步难行";
                        }
                        else if (propertyValue > 123 && propertyValue <= 129.9)
                        {
                            tab = "步履蹒跚";
                        }
                        else if (propertyValue > 130 && propertyValue <= 134.9)
                        {
                            tab = "平稳通过";
                        }
                        else if (propertyValue > 135 && propertyValue <= 136.9)
                        {
                            tab = "超级稳定";
                        }
                        else if (propertyValue > 137)//&& propertyValue <= 141
                        {
                            tab = "行云流水";
                        }
                    }
                    else
                    {
                        if (propertyValue > 0 && propertyValue <= 114.9)
                        {
                            tab = "寸步难行";
                        }
                        else if (propertyValue >= 115 && propertyValue <= 118.9)
                        {
                            tab = "步履蹒跚";
                        }
                        else if (propertyValue >= 119 && propertyValue <= 121.9)
                        {
                            tab = "平稳通过";
                        }
                        else if (propertyValue >= 122 && propertyValue <= 124.9)
                        {
                            tab = "超级稳定";
                        }
                        else if (propertyValue >= 125)//&& propertyValue <= 129
                        {
                            tab = "行云流水";
                        }
                    }
                    break;
                case 132://加速
                    if (fuelType == "纯电" || fuelType == "插电混合")
                    {
                        if (propertyValue > 0 && propertyValue <= 4)
                        {
                            tab = "diao爆了";
                        }
                        else if (propertyValue >= 4.1 && propertyValue <= 5)
                        {
                            tab = "炫酷超车";
                        }
                        else if (propertyValue >= 5.1 && propertyValue <= 7.1)
                        {
                            tab = "一般般";
                        }
                        else if (propertyValue >= 7.2 && propertyValue <= 9)
                        {
                            tab = "弱弱哒";
                        }
                        else if (propertyValue >= 9.1)//&& propertyValue <= 11
                        {
                            tab = "弱爆了";
                        }
                    }
                    else
                    {
                        if (level == 9)//跑车
                        {
                            if (propertyValue > 0 && propertyValue <= 3.2)
                            {
                                tab = "diao爆了";
                            }
                            else if (propertyValue >= 3.3 && propertyValue <= 4.5)
                            {
                                tab = "炫酷超车";
                            }
                            else if (propertyValue >= 4.6 && propertyValue <= 5.5)
                            {
                                tab = "一般般";
                            }
                            else if (propertyValue >= 5.6 && propertyValue <= 7)
                            {
                                tab = "弱弱哒";
                            }
                            else if (propertyValue >= 7.1)//&& propertyValue <= 10
                            {
                                tab = "弱爆了";
                            }
                        }
                        else
                        {
                            if (propertyValue > 0 && propertyValue <= 6)
                            {
                                tab = "diao爆了";
                            }
                            else if (propertyValue >= 6.1 && propertyValue <= 9.1)
                            {
                                tab = "炫酷超车";
                            }
                            else if (propertyValue >= 9.2 && propertyValue <= 11.1)
                            {
                                tab = "一般般";
                            }
                            else if (propertyValue >= 11.2 && propertyValue <= 13.9)
                            {
                                tab = "弱弱哒";
                            }
                            else if (propertyValue >= 14)//&& propertyValue <= 14.9
                            {
                                tab = "弱爆了";
                            }
                        }
                    }
                    break;
            }
            return tab;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}