using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using MongoDB.Driver;
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
    /// GetPingCeList 的摘要说明
    /// </summary>
    public class GetPingCeList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //PageHelper.SetPageCache(60);

            context.Response.ContentType = "application/x-javascript";
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
            List<GroupScore> groupScoreList = ReadXmlScore();//评分标准

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
                entity.Finished = GetFinishedStatusAndGroupScore(item, out dicGroupScore);
                SetGoodAndBad(dicGroupScore, entity, groupScoreList);
                SetScoreDes(dicGroupScore, entity, groupScoreList);
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
                        BadDiscription = entity.BadDiscription,
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

        /// <summary>
        /// 获取评测是否完成的状态及每组的评分
        /// </summary>
        /// <param name="assessmentEntity"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private bool GetFinishedStatusAndGroupScore(AssessmentEntity assessmentEntity, out Dictionary<string, double> dic)
        {
            bool finished = true;
            Dictionary<string, double> tempDic = new Dictionary<string, double>();
            if (assessmentEntity != null)
            {
                if (assessmentEntity.CommonInfoGroup != null)
                {
                    if (assessmentEntity.CommonInfoGroup.Score > 0)
                    {
                        tempDic.Add("CommonInfoGroup", assessmentEntity.CommonInfoGroup.Score);
                        if (assessmentEntity.CommonInfoGroup.CommonInformationEntity != null)
                        {
                            //处理逻辑
                        }
                    }
                }

                if (assessmentEntity.BodyAndSpaceGroup != null)
                {
                    if (assessmentEntity.BodyAndSpaceGroup.Score > 0)
                    {
                        tempDic.Add("BodyAndSpaceGroup", assessmentEntity.BodyAndSpaceGroup.Score);
                        //if (assessmentEntity.BodyAndSpaceGroup.SpaceEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.BodyAndSpaceGroup.TrunkEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.BodyAndSpaceGroup.StoragespaceEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.BodyAndSpaceGroup.ConvenienceEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.BodyAndSpaceGroup.QualityEntity != null)
                        //{

                        //}
                    }
                }

                if (assessmentEntity.RidingComfortGroup != null)
                {
                    if (assessmentEntity.RidingComfortGroup.Score > 0)
                    {
                        tempDic.Add("RidingComfortGroup", assessmentEntity.RidingComfortGroup.Score);
                        //if (assessmentEntity.RidingComfortGroup.ChairEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.RidingComfortGroup.AirConditionerEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.RidingComfortGroup.HangComfortEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.RidingComfortGroup.NoiseEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.RidingComfortGroup.NoiseFeelEntity != null)
                        //{

                        //}

                    }
                }

                if (assessmentEntity.DynamicPerformanceGroup != null)
                {
                    if (assessmentEntity.DynamicPerformanceGroup.Score > 0)
                    {
                        tempDic.Add("DynamicPerformanceGroup", assessmentEntity.DynamicPerformanceGroup.Score);
                        //if (assessmentEntity.DynamicPerformanceGroup.AccelerateEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.DynamicPerformanceGroup.EngineEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.DynamicPerformanceGroup.MotilityEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.DynamicPerformanceGroup.EngineSmoothnessEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.DynamicPerformanceGroup.GearboxEntity != null)
                        //{

                        //}
                    }
                }

                if (assessmentEntity.JsBaseGroup != null)
                {
                    if (assessmentEntity.JsBaseGroup.Score > 0)
                    {
                        tempDic.Add("JsBaseGroup", assessmentEntity.JsBaseGroup.Score);
                        //if (assessmentEntity.JsBaseGroup.JdaEntity != null)
                        //{

                        //}

                        //if (assessmentEntity.JsBaseGroup.JtsEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.JsBaseGroup.JdfEntity != null)
                        //{

                        //}
                    }
                }

                if (assessmentEntity.SafetyGroup != null)
                {
                    if (assessmentEntity.SafetyGroup.Score > 0)
                    {
                        tempDic.Add("SafetyGroup", assessmentEntity.SafetyGroup.Score);
                        //if (assessmentEntity.SafetyGroup.BrakeEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.SafetyGroup.ActiveSafetyEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.SafetyGroup.VisualFieldEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.SafetyGroup.LightEntity != null)
                        //{

                        //}

                    }
                }

                if (assessmentEntity.YhBaseGroup != null)
                {
                    if (assessmentEntity.YhBaseGroup.Score > 0)
                    {
                        tempDic.Add("YhBaseGroup", assessmentEntity.YhBaseGroup.Score);
                        //if (assessmentEntity.YhBaseGroup.YgEntity != null)
                        //{

                        //}

                        if (assessmentEntity.YhBaseGroup.YaEntity != null)
                        {

                            if (assessmentEntity.YhBaseGroup.YaEntity.AirQualityScore == 0 && string.IsNullOrWhiteSpace(assessmentEntity.YhBaseGroup.YaEntity.AirQualityGeneralCmt))
                            {
                                finished = false;//"测试中"
                            }
                        }
                    }
                }

                if (assessmentEntity.CostBaseGroup != null)
                {
                    if (assessmentEntity.CostBaseGroup.Score > 0)
                    {
                        tempDic.Add("CostBaseGroup", assessmentEntity.CostBaseGroup.Score);
                        //if (assessmentEntity.CostBaseGroup.CpEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.CostBaseGroup.CsyEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.CostBaseGroup.CgpEntity != null)
                        //{

                        //}
                        //if (assessmentEntity.CostBaseGroup.CgqEntity != null)
                        //{

                        //}
                    }
                }

                if (assessmentEntity.GeneralBaseGroup != null)
                {
                    if (assessmentEntity.GeneralBaseGroup.Score > 0)
                    {
                        tempDic.Add("GeneralBaseGroup", assessmentEntity.GeneralBaseGroup.Score);
                    }
                }

            }
            dic = tempDic;
            return finished;
        }

        /// <summary>
        /// 查找最高 和 最低分数并设置文案描述
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="pingCeTopPcEntity"></param>
        /// <param name="list"></param>
        private void SetGoodAndBad(Dictionary<string, double> dic, PingCeEntity pingCeTopPcEntity, List<GroupScore> list)
        {
            try
            {
                Dictionary<string, double> percentDic = new Dictionary<string, double>();
                foreach (GroupScore item in list)
                {
                    if (dic.Keys.Contains(item.GroupName))
                    {
                        percentDic.Add(item.GroupName, dic[item.GroupName] / item.Score);
                    }
                }
                var orderDic = percentDic.OrderByDescending(i => i.Value);

                if (orderDic.Count() > 0)
                {
                    string maxKey = orderDic.First().Key;
                    string minKey = orderDic.Last().Key;
                    double max = dic[maxKey];
                    double min = dic[minKey];

                    if (list != null)
                    {
                        GroupScore max_GroupScore = list.First(i => i.GroupName == maxKey);
                        GroupScore min_GroupScore = list.First(i => i.GroupName == minKey);

                        foreach (var item in max_GroupScore.ScoreDesc)
                        {
                            string[] arr = item.Key.Split('-').ToArray();

                            if (max >= ConvertHelper.GetDouble(arr[0]) && max < ConvertHelper.GetDouble(arr[1]))
                            {
                                pingCeTopPcEntity.GoodDiscription = max_GroupScore.CommonDesc[item.Value];
                            }
                        }

                        foreach (var item in min_GroupScore.ScoreDesc)
                        {
                            string[] arr = item.Key.Split('-').ToArray();

                            if (min >= ConvertHelper.GetDouble(arr[0]) && min < ConvertHelper.GetDouble(arr[1]))
                            {
                                pingCeTopPcEntity.BadDiscription = min_GroupScore.CommonDesc[item.Value];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }


        }

        private void SetScoreDes(Dictionary<string, double> dic, PingCeEntity pingCeTopPcEntity, List<GroupScore> list)
        {
            Dictionary<string, object> tempdic = new Dictionary<string, object>();
            foreach (var item in dic.Keys)
            {
                double currentScore = dic[item];
                GroupScore groupScore = list.First(i => i.GroupName == item);
                if (groupScore != null)
                {
                    foreach (var k in groupScore.ScoreDesc.Keys)
                    {
                        string[] arr = k.Split('-').ToArray();
                        double percent = currentScore / groupScore.Score;
                        if (percent >= ConvertHelper.GetDouble(arr[0]) && percent < ConvertHelper.GetDouble(arr[1]))
                        {
                            tempdic.Add(item, new { Score= groupScore.Score, ScoreDesc=groupScore.ScoreDesc[k], CurrentScore= currentScore,Percent= percent });
                            break;
                        }
                    }
                }
            }
            pingCeTopPcEntity.ScoreDic = tempdic;
        }

        /// <summary>
        /// 从配置文件中读取评测标准及文案描述信息
        /// </summary>
        /// <returns></returns>
        private List<GroupScore> ReadXmlScore()
        {
            string xmlFile = System.Web.HttpContext.Current.Server.MapPath(@"~/config/EvaluationScore.xml");

            string key = "EvaluationScore_Xml";

            List<GroupScore> list = new List<GroupScore>();

            var obj = CacheManager.GetCachedData(key);

            try
            {
                if (obj != null)
                {
                    list=(List<GroupScore>)obj;
                }
                else
                {
                    if (File.Exists(xmlFile))
                    {
                        XDocument doc = XDocument.Load(xmlFile);
                        var query = from p in doc.Element("Root").Element("Group").Elements("Item") select p;
                        query.ToList().ForEach(item =>
                        {
                            GroupScore gS = new GroupScore();
                            gS.GroupName = item.Element("GroupName").Value;
                            gS.Score = ConvertHelper.GetDouble(item.Element("Score").Value);
                            var query_ScoreDesc = from score in item.Element("ScoreDesc").Elements("Item") select score;
                            Dictionary<string, string> scoreDesc = new Dictionary<string, string>();
                            query_ScoreDesc.ToList().ForEach(qs =>
                            {
                                scoreDesc.Add(qs.Attribute("Range").Value, qs.Value);
                            });
                            gS.ScoreDesc = scoreDesc;


                            var query_commonDesc = from score in item.Element("CommonDesc").Elements("Item") select score;
                            Dictionary<string, string> commonDesc = new Dictionary<string, string>();
                            query_commonDesc.ToList().ForEach(qs =>
                            {
                                commonDesc.Add(qs.Attribute("Range").Value, qs.Value);
                            });
                            gS.CommonDesc = commonDesc;
                            list.Add(gS);
                        });
                        CacheManager.InsertCache(key, list, WebConfig.CachedDuration);
                    }
                }                
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return list;
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