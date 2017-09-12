using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// PC和移动站中进入评测页面的入口数据
    /// </summary>
    public class GetEntranceData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";

            string callback = context.Request.QueryString["callback"];
            int SerialId = 0;
            string csid = context.Request.QueryString["csid"];
            int.TryParse(csid, out SerialId);
            List<string> paraList = new List<string>();
            paraList.Add("CreateDateTime");
            paraList.Add("SerialId");
            paraList.Add("CarId");
            paraList.Add("Status");
            paraList.Add("EvaluationId");
            paraList.Add("Score");
            paraList.Add("CommonInfoGroup");
            paraList.Add("BodyAndSpaceGroup.Score");
            paraList.Add("SafetyGroup.Score");
            paraList.Add("RidingComfortGroup.Score");
            paraList.Add("DynamicPerformanceGroup.Score");
            paraList.Add("JsBaseGroup.Score");
            paraList.Add("YhBaseGroup.Score");
            paraList.Add("CostBaseGroup.Score");
            //IMongoQuery query = Query.EQ("SerialId", SerialId);
            IMongoQuery query = Query.And(Query.EQ("SerialId", SerialId), Query.EQ("Status", 1));
            Dictionary<string, int> sortdic = new Dictionary<string, int>();
            sortdic.Add("CreateDateTime", 0);
            EvaluationBll evaluationBll = new EvaluationBll();
            AssessmentEntity assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);

            string ImageUrl = "", CarName = "";
            int SId = 0, CarId = 0, EId = 0;
            double TotalScore = 0, CommonInfoGroupScore = 0, BodyAndSpaceGroupScore = 0, SafetyGroupScore = 0, RidingComfortGroupScore = 0, DynamicPerformanceGroupScore = 0, JsBaseGroupScore = 0, YhBaseGroupScore = 0, CostBaseGroupScore = 0;

            if (assessmentEntity != null)
            {
                SId = assessmentEntity.SerialId;
                CarId = assessmentEntity.CarId;
                EId = assessmentEntity.EvaluationId;
                TotalScore = assessmentEntity.Score;

                if (assessmentEntity.CommonInfoGroup != null)
                {
                    CommonInfoGroupScore = assessmentEntity.CommonInfoGroup.Score;
                    ImageUrl = assessmentEntity.CommonInfoGroup.CommonInformationEntity.MainImageUrl;
                }
                if (assessmentEntity.BodyAndSpaceGroup != null)
                {
                    BodyAndSpaceGroupScore = assessmentEntity.BodyAndSpaceGroup.Score;
                }
                if (assessmentEntity.SafetyGroup != null)
                {
                    SafetyGroupScore = assessmentEntity.SafetyGroup.Score;
                }
                if (assessmentEntity.RidingComfortGroup != null)
                {
                    RidingComfortGroupScore = assessmentEntity.RidingComfortGroup.Score;
                }
                if (assessmentEntity.DynamicPerformanceGroup != null)
                {
                    DynamicPerformanceGroupScore = assessmentEntity.DynamicPerformanceGroup.Score;
                }
                if (assessmentEntity.JsBaseGroup != null)
                {
                    JsBaseGroupScore = assessmentEntity.JsBaseGroup.Score;
                }
                if (assessmentEntity.YhBaseGroup != null)
                {
                    YhBaseGroupScore = assessmentEntity.YhBaseGroup.Score;
                }
                if (assessmentEntity.CostBaseGroup != null)
                {
                    CostBaseGroupScore = assessmentEntity.CostBaseGroup.Score;
                }
                CarEntity cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, assessmentEntity.CarId);
                if (cbe != null)
                {
                    CarName = CommonFunction.GetUnicodeByString(cbe.Serial.ShowName + " " + cbe.CarYear + "款" + cbe.Name);
                }
                else
                {
                    CarName = "";
                }
            }

            var data = new
            {
                ImageUrl = ImageUrl,
                SerialId = SId,
                CarId = CarId,
                CarName = CarName,
                EvaluationId = EId,
                TotalScore = TotalScore,
                CommonInfoGroupScore = CommonInfoGroupScore,
                BodyAndSpaceGroupScore = BodyAndSpaceGroupScore,
                SafetyGroupScore = SafetyGroupScore,
                RidingComfortGroupScore = RidingComfortGroupScore,
                DynamicPerformanceGroupScore = DynamicPerformanceGroupScore,
                JsBaseGroupScore = JsBaseGroupScore,
                YhBaseGroupScore = YhBaseGroupScore,
                CostBaseGroupScore = CostBaseGroupScore
            };
            var json = JsonConvert.SerializeObject(data);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
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