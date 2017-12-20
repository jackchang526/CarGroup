using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using MongoDB.Driver.Builders;
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
    /// GetEvaluationSimpleRank 简略排行接口
    /// </summary>
    public class GetEvaluationSimpleRank : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string result = "{}";
            string callback = context.Request.QueryString["callback"];
            int carId = ConvertHelper.GetInteger(context.Request.QueryString["carId"]);
            int levelId = ConvertHelper.GetInteger(context.Request.QueryString["levelId"]);
            int propertyId = ConvertHelper.GetInteger(context.Request.QueryString["propertyId"]);
            List<EvaluationRank> list = new List<EvaluationRank>();
            int levelTotal = 0, levelNum = 0;
            string levelName = "",levelSpell="";
            double beat = 0;
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(@"Data\EvaluationRank\Rank_{0}\{1}.xml", propertyId, carId));
            try
            {
                if (File.Exists(xmlFile))
                {
                    XDocument doc = XDocument.Load(xmlFile);
                    var query = from p in doc.Element("Root").Element("EvaluationRankList").Elements("EvaluationRank") select p;
                    query.ToList().ForEach(item =>
                    {
                        EvaluationRank er = new EvaluationRank();
                        er.StyleId = ConvertHelper.GetInteger(item.Element("StyleId").Value);
                        er.PropertyValue = ConvertHelper.GetDouble(item.Element("PropertyValue").Value);
                        er.PropertyId = ConvertHelper.GetInteger(item.Element("PropertyId").Value);
                        er.EvaluationId = ConvertHelper.GetInteger(item.Element("EvaluationId").Value);
                        er.EditorsName = item.Element("EditorsName").Value;
                        er.Weather = item.Element("Weather").Value;
                        er.Wind = item.Element("Wind").Value;
                        er.Temperature = item.Element("Temperature").Value;
                        er.StyleName = item.Element("StyleName").Value;
                        er.MasterBrandName = item.Element("MasterBrandName").Value;
                        er.ModelName = item.Element("ModelName").Value;
                        er.Year = ConvertHelper.GetInteger(item.Element("Year").Value);
                        er.ModelDisplayName = item.Element("ModelDisplayName").Value;
                        er.ModelLevel = item.Element("ModelLevel").Value;
                        er.LevelNum = ConvertHelper.GetInteger(item.Element("LevelNum").Value);
                        er.RankNum = ConvertHelper.GetInteger(item.Element("RankNum").Value);
                        er.Unit = item.Element("Unit").Value;
                        er.FuelType = item.Element("FuelType").Value;
                        er.TabText = item.Element("TabText").Value;
                        er.CurrentFlag = ConvertHelper.GetBoolean(item.Element("CurrentFlag").Value);
                        er.IsExistReport = ConvertHelper.GetBoolean(item.Element("IsExistReport").Value);
                        er.ModelAllSpell = item.Element("ModelAllSpell").Value;
                        list.Add(er);
                    });
                    levelTotal = ConvertHelper.GetInteger(doc.Element("Root").Element("LevelTotal").Value);
                    levelNum = ConvertHelper.GetInteger(doc.Element("Root").Element("LevelNum").Value);
                    levelName = doc.Element("Root").Element("LevelName").Value;
                    beat = ConvertHelper.GetDouble((levelTotal - levelNum)*100 / levelTotal);
                }
                levelSpell = CarLevelDefine.GetLevelSpellById(levelId);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }

            var obj = new
            {
                list = list,
                LevelTotal = levelTotal,
                LevelNum = levelNum,
                LevelName = levelName,
                PropertyId = propertyId,
                LevelId = levelId,
                LevelSpell= levelSpell,
                Beat= beat,
            };
            result = JsonConvert.SerializeObject(obj);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
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