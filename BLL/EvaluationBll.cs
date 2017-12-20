using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BitAuto.CarChannel.BLL
{
    public class EvaluationBll
    {
        /// <summary>
        /// 根据车系ID获取车款的最新评测数据
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <returns></returns>
        public List<EvaluationEntity> GetByStyleEvaluation(int evaluationId)
        {
            List<EvaluationEntity> list = new List<EvaluationEntity>();
            EvaluationDal evaluationDal = new EvaluationDal();
            DataSet ds = evaluationDal.GetByStyleEvaluation(evaluationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EvaluationEntity item = new EvaluationEntity();
                    item.CarId = Convert.ToInt32(dr["StyleId"].ToString());
                    item.SerialId = Convert.ToInt32(dr["ModelId"].ToString());
                    item.EvaluationId = Convert.ToInt32(dr["EvaluationId"].ToString());
                    item.GroupId = Convert.ToInt32(dr["GroupId"].ToString());
                    item.GroupName = Convert.ToString(dr["GroupNmae"]).Trim();
                    item.Name = Convert.ToString(dr["Name"]).Trim();
                    item.PropertyId = Convert.ToInt32(dr["PropertyId"].ToString());
                    item.PropertyValue = Convert.ToString(dr["PropertyValue"]).Trim();
                    item.Unit = Convert.ToString(dr["Unit"]).Trim();
                    list.Add(item);
                }
            }
            return list;
        }
        public TestBaseEntity GetTestBaseEntityById(int evaluationId)
        {
            TestBaseEntity item = new TestBaseEntity();
            EvaluationDal evaluationDal = new EvaluationDal();
            DataSet ds = evaluationDal.GetTestBaseEntityById(evaluationId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<string> testerList = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    item.ModelName = Convert.ToString(dr["ModelName"].ToString());
                    item.Year = Convert.ToString(dr["Year"].ToString());
                    item.StyleName = Convert.ToString(dr["StyleName"].ToString());
                    item.TestTime = Convert.ToDateTime(dr["EvaluatingTime"].ToString());
                    item.Site = Convert.ToString(dr["Site"]).Trim();
                    //item.Tester = Convert.ToString(dr["EditorsName"]).Trim();

                    //20171106 去掉括号中的部门（马保青）
                    string[] testArr = Convert.ToString(dr["EditorsName"]).Trim().Split(',', '、', '，');
                    foreach (string tester in testArr)
                    {
                        int index = tester.IndexOf('(');
                        if (index >= 0)
                        {
                            testerList.Add(tester.Substring(0, index));
                        }
                        else
                        {
                            testerList.Add(tester);
                        }

                    }
                    item.Tester = string.Join(",", testerList.ToArray());

                    item.EquipmentOperator = Convert.ToString(dr["EquipmentOperator"].ToString());
                    item.Kilometers = Convert.ToInt32(dr["Kilometers"]);
                    item.WeatherDesc = Convert.ToString(dr["WeatherDesc"].ToString());
                    item.Weather = Convert.ToString(dr["Weather"].ToString());
                    item.StyleId = Convert.ToInt32(dr["StyleId"]);
                    item.Wind = Convert.ToString(dr["Wind"].ToString());
                    item.Temperature = Convert.ToString(dr["Temperature"].ToString());
                    break;//只获取一条数据
                }
            }
            return item;
        }
        public T GetOne<T>(IMongoQuery query)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetOne<T>(query);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public T GetOne<T>(IMongoQuery query, string[] fields, Dictionary<string, int> sortdic = null)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetOne<T>(query, fields, sortdic);
        }

        public List<int> GetExistReportEvaluationId()
        {
            List<int> list = new List<int>();
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\ExistReportEvaluationIdList\EvaluationIdList.xml");
            try
            {
                if (File.Exists(xmlFile))
                {
                    XDocument doc = XDocument.Load(xmlFile);
                    var query = from p in doc.Element("Root").Element("EvaluationIdList").Elements("Item") select p;
                    query.ToList().ForEach(item =>
                    {
                        int evaluationId = ConvertHelper.GetInteger(item.Attribute("EvaluationId").Value);
                        list.Add(evaluationId);
                    });
                }
            }
            catch (Exception e)
            {
                CommonFunction.WriteLog(e.ToString());
            }
            return list;
        }

        public List<T> GetPingCeAllList<T>(IMongoQuery query, int top, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetPingCeAllList<T>(query, top,sortdic, fields);
        }

        public List<T> GetPingCeList<T>(IMongoQuery query, int index, int pageCount, out int total, Dictionary<string, int> sortdic = null, params string[] fields)
        {
            EvaluationDal evaluationDal = new EvaluationDal();
            return evaluationDal.GetPingCeList<T>(query, index, pageCount,out total, sortdic, fields);
        }

        /// <summary>
        /// 根据MogonDB中的评测ID列表获取评测数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<int, PingCeEntity> GetEvaluationDate(List<int> list)
        {
            EvaluationDal evaluationDal = new EvaluationDal();            
            return evaluationDal.GetEvaluationDate(list);
        }

        /// <summary>
        /// 获取评测是否完成的状态及每组的评分
        /// </summary>
        /// <param name="assessmentEntity"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool GetFinishedStatusAndGroupScore(AssessmentEntity assessmentEntity, out Dictionary<string, double> dic)
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
                        //if (assessmentEntity.CommonInfoGroup.CommonInformationEntity != null)
                        //{

                        //}
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
        public void SetGoodAndBad(Dictionary<string, double> dic, PingCeEntity pingCeTopPcEntity, List<GroupScore> list)
        {
            try
            {
                //参与最好最坏标签的组
                List<string> target = new List<string>
                {
                    "BodyAndSpaceGroup",
                    "RidingComfortGroup",
                    "DynamicPerformanceGroup",
                    "JsBaseGroup",
                    "SafetyGroup"
                };
                //如果空气质量评测完成，油耗参与最好最坏标签显示
                if (pingCeTopPcEntity.Finished)
                {
                    target.Add("YhBaseGroup");
                }

                Dictionary<string, double> percentDic = new Dictionary<string, double>();
                foreach (GroupScore item in list)
                {
                    if (dic.Keys.Contains(item.GroupName)&&target.Contains(item.GroupName))
                    {
                        percentDic.Add(item.GroupName, dic[item.GroupName] / item.Score);
                    }
                }
                var orderDic = percentDic.OrderByDescending(i => i.Value);

                if (orderDic.Count() > 0)
                {
                    string maxKey = orderDic.First().Key;
                    string minKey = orderDic.Last().Key;
                    double max = percentDic[maxKey];
                    double min = percentDic[minKey];

                    var maxArr = percentDic.Where(i => i.Value == max);
                    var minArr = percentDic.Where(i => i.Value == min);

                    int maxCount = maxArr.Count();
                    if (maxCount > 1)//是否有并列第一
                    {
                        foreach (var item in target)
                        {
                            var aa = maxArr.Where(i => i.Key == item);
                            if (aa.Count() > 0)
                            {
                                maxKey = item;
                                break;
                            }
                        }
                    }
                    int minCount = minArr.Count();
                    if (minCount > 1)//是否有并列倒是第一
                    {
                        foreach (var item in target)
                        {
                            var aa = minArr.Where(i => i.Key == item);
                            if (aa.Count() > 0)
                            {
                                minKey = item;
                                break;
                            }
                        }
                    }

                    if (list != null)
                    {
                        GroupScore max_GroupScore = list.First(i => i.GroupName == maxKey);                       
                        foreach (var item in max_GroupScore.ScoreDesc)
                        {
                            string[] arr = item.Key.Split('-').ToArray();
                            pingCeTopPcEntity.GoodGroup = GetGroupIdByName(maxKey);
                            if (max >= ConvertHelper.GetDouble(arr[0]) && max <= ConvertHelper.GetDouble(arr[1]))
                            {
                                pingCeTopPcEntity.GoodDiscription = max_GroupScore.CommonDesc[item.Value];
                            }                            
                        }

                        GroupScore min_GroupScore = list.First(i => i.GroupName == minKey);
                        foreach (var item in min_GroupScore.ScoreDesc)
                        {
                            string[] arr = item.Key.Split('-').ToArray();
                            pingCeTopPcEntity.BadGroup = GetGroupIdByName(minKey);
                            if (min >= ConvertHelper.GetDouble(arr[0]) && min <= ConvertHelper.GetDouble(arr[1]))
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

        public void SetScoreDes(Dictionary<string, double> dic, PingCeEntity pingCeTopPcEntity, List<GroupScore> list)
        {
            Dictionary<string, object> tempdic = new Dictionary<string, object>();
            foreach (GroupScore item in list)
            {
                if (dic.Keys.Contains(item.GroupName))
                {
                    double currentScore = dic[item.GroupName];
                    foreach (var k in item.ScoreDesc.Keys)
                    {
                        string[] arr = k.Split('-').ToArray();
                        double percent = currentScore / item.Score;
                        if (percent >= ConvertHelper.GetDouble(arr[0]) && percent <= ConvertHelper.GetDouble(arr[1]))
                        {
                            tempdic.Add(item.GroupName, new { Score = item.Score, ScoreDesc = item.ScoreDesc[k], CurrentScore = currentScore, Percent = percent });
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
        public List<GroupScore> ReadXmlScore()
        {
            string xmlFile = System.Web.HttpContext.Current.Server.MapPath(@"~/config/EvaluationScore.xml");

            string key = "EvaluationScore_Xml";

            List<GroupScore> list = new List<GroupScore>();

            var obj = CacheManager.GetCachedData(key);

            try
            {
                if (obj != null)
                {
                    list = (List<GroupScore>)obj;
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

        private int GetGroupIdByName(string groupName)
        {
            //该字典根据评测后台项目中定义的枚举整理
            Dictionary<string, int> dic = new Dictionary<string, int>
            {
                { "CommonInfoGroup", 1 },
                { "BodyAndSpaceGroup", 2 },
                { "SafetyGroup", 3 },
                { "RidingComfortGroup", 4 },
                { "DynamicPerformanceGroup", 5 },
                { "JsBaseGroup", 6 },
                { "YhBaseGroup", 7 },
                { "CostBaseGroup", 1 },
                { "GeneralBaseGroup", 1 }
            };
            return dic[groupName];
        }
    }
}
