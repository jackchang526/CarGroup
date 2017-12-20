using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetPingCeList 的摘要说明
    /// </summary>
    public class GetPingCeList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);

            //context.Response.ContentType = "application/x-javascript";
            context.Response.ContentType = "text/plain";
            string callback = context.Request.QueryString["callback"];
            int index = ConvertHelper.GetInteger(context.Request.QueryString["index"]);
            int pageSize = ConvertHelper.GetInteger(context.Request.QueryString["pagesize"]);

            string result = "{}";
            string message = "成功";
            int status = 1;
            int total = 0;
            List<object> list = new List<object>();
            EvaluationBll evaluationBll = new EvaluationBll();
            IMongoQuery query = Query.EQ("Status", 1);
            List<string> paraList = new List<string>
            {
                "CreateDateTime",
                "CarId",
                "Status",
                "EvaluationId",
                "Score",
                "CommonInfoGroup.CommonInformationEntity.Title",
                "CommonInfoGroup.CommonInformationEntity.MainImageUrl",
                "BodyAndSpaceGroup.Score",
                "RidingComfortGroup.Score",
                "DynamicPerformanceGroup.Score",
                "JsBaseGroup.Score",
                "SafetyGroup.Score",
                "YhBaseGroup.Score",
                "YhBaseGroup.YaEntity",
                "CostBaseGroup.Score",
                "GeneralBaseGroupl.Score"
            };
            Dictionary<string, int> sortdic = new Dictionary<string, int>
            {
                { "CreateDateTime", 0 }
            };
            List<AssessmentEntity> pingCeAllList = evaluationBll.GetPingCeList<AssessmentEntity>(query, index, pageSize, out total, sortdic, paraList.ToArray());
            List<int> evaluationIdList = (from item in pingCeAllList select item.EvaluationId).ToList();
            Dictionary<int, PingCeEntity> dic = evaluationBll.GetEvaluationDate(evaluationIdList);
            List<GroupScore> groupScoreList = evaluationBll.ReadXmlScore();//评分标准

            foreach (AssessmentEntity item in pingCeAllList)
            {
                PingCeEntity entity = new PingCeEntity();
                entity.Score = item.Score;
                entity.EvaluationId = item.EvaluationId;
                entity.CreateDateTime = item.CreateDateTime;
                if (item.CommonInfoGroup != null && item.CommonInfoGroup.CommonInformationEntity != null)
                {
                    entity.ImageUrl = item.CommonInfoGroup.CommonInformationEntity.MainImageUrl;
                    entity.Title = item.CommonInfoGroup.CommonInformationEntity.Title;
                }
                if (dic.ContainsKey(item.EvaluationId))
                {
                    PingCeEntity temp = dic[item.EvaluationId];
                    entity.Acceleration = temp.Acceleration;
                    entity.BrakingDistance = temp.BrakingDistance;
                    entity.Fuel = temp.Fuel;
                    entity.Year = temp.Year;
                    entity.ModelDisplayName = temp.ModelDisplayName;
                    entity.StyleName = temp.StyleName;
                    entity.FuelType = temp.FuelType;
                }
                Dictionary<string, double> dicGroupScore = null;
                entity.Finished = evaluationBll.GetFinishedStatusAndGroupScore(item, out dicGroupScore);
                evaluationBll.SetGoodAndBad(dicGroupScore, entity, groupScoreList);
                evaluationBll.SetScoreDes(dicGroupScore, entity, groupScoreList);
                list.Add(
                    new
                    {
                        Score = entity.Score,
                        EvaluationId = entity.EvaluationId,
                        ImageUrl = entity.ImageUrl,
                        Title = entity.Title,
                        Acceleration = Math.Round(entity.Acceleration,1),
                        BrakingDistance = Math.Round(entity.BrakingDistance, 1),
                        Fuel = Math.Round(entity.Fuel, 1),
                        Year = entity.Year,
                        ModelDisplayName = entity.ModelDisplayName,
                        StyleName = entity.StyleName,
                        FuelType = entity.FuelType,
                        GoodDiscription = entity.GoodDiscription,
                        GoodGroup = entity.GoodGroup,
                        BadDiscription = entity.BadDiscription,
                        BadGroup = entity.BadGroup,
                        Finished = entity.Finished,
                        ScoreDic = entity.ScoreDic
                    }
                    );
            }

            var obj = new
            {
                status = status,
                message = message,
                data = new { List = list, Total = total }
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