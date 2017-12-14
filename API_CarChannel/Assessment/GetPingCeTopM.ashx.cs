using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
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
    /// GetPingCeTopM 的摘要说明
    /// </summary>
    public class GetPingCeTopM : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);

            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];
            int top = 6;//ConvertHelper.GetInteger(context.Request.QueryString["top"]);

            string result = "{}";
            string message = "成功";
            int status = 1;
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
                "CommonInfoGroup.CommonInformationEntity.ImageUrl",
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
            Dictionary<string, int> sortdic = new Dictionary<string, int>();
            sortdic.Add("CreateDateTime", 0);
            List<AssessmentEntity> pingCeAllList = evaluationBll.GetPingCeAllList<AssessmentEntity>(query, top, sortdic, paraList.ToArray());
            List<int> evaluationIdList = (from item in pingCeAllList select item.EvaluationId).ToList();
            Dictionary<int, PingCeEntity> dic = evaluationBll.GetEvaluationDate(evaluationIdList);
            foreach (AssessmentEntity item in pingCeAllList)
            {
                string ImageUrl = "", Title = "", FuelType = "";
                bool Finished = true;
                if (item.CommonInfoGroup != null && item.CommonInfoGroup.CommonInformationEntity != null)
                {
                    ImageUrl = item.CommonInfoGroup.CommonInformationEntity.ImageUrl;
                    Title = item.CommonInfoGroup.CommonInformationEntity.Title;
                }
                if (dic.ContainsKey(item.EvaluationId))
                {
                    PingCeEntity temp = dic[item.EvaluationId];
                    FuelType = temp.FuelType;
                }
                Dictionary<string, double> dicGroupScore = null;
                Finished = GetFinishedStatusAndGroupScore(item, out dicGroupScore);
                list.Add(
                    new
                    {
                        Score = item.Score,
                        EvaluationId = item.EvaluationId,
                        ImageUrl = ImageUrl,
                        Title = Title,
                        FuelType = FuelType,
                        Finished = Finished
                    }
                    );
            }

            var obj = new
            {
                status = status,
                message = message,
                data = list
            };
            result = JsonConvert.SerializeObject(obj);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
        }

        /// <summary>
        /// 根据MogonDB中的评测ID列表获取评测数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //private Dictionary<int, PingCeEntity> GetEvaluationDate(List<int> list)
        //{
        //    Dictionary<int, PingCeEntity> dic = new Dictionary<int, PingCeEntity>();
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("EvaluationId", typeof(int));
        //        foreach (int item in list)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["EvaluationId"] = item;
        //            dt.Rows.Add(dr);
        //        }
        //        SqlParameter[] param = {
        //                        new SqlParameter("@evaluationIdList",SqlDbType.Structured)
        //                           };
        //        param[0].Value = dt;
        //        DataSet ds = SqlHelper.ExecuteDataset(WebConfig.CarsEvaluationDataConnectionString, CommandType.StoredProcedure, "[dbo].[proc_SE_SPV_Select_M]", param);
        //        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                PingCeEntity item = new PingCeEntity();
        //                item.EvaluationId = ConvertHelper.GetInteger(dr["Id"]);
        //                item.FuelType = dr["FuelType"].ToString();
        //                dic.Add(item.EvaluationId, item);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = ex.Message;
        //        CommonFunction.WriteLog(ex.ToString());
        //    }
        //    return dic;
        //}

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}