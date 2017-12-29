using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
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
    /// GetEvalInfoForBg 的摘要说明
    /// </summary>
    public class GetEvalInfoForBg : IHttpHandler
    {
        /// <summary>
        /// 为大数据 提供的评测库（包含评测报告mongo）数据接口,参数evaluationId ,status
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            bool resultFlag = true;
            string message = "ok";
            int status = 1;
            string title = string.Empty;
            string coverImg = string.Empty;
            string relationMId = string.Empty;
            string relationSId = string.Empty;
            string keyword = string.Empty;
            int mediaId = 0;
            string mediaName = string.Empty;
            DateTime passTime = new DateTime();
            int isDeleted = 0;
            int commentsCnt = 0;
            int visitCnt = 0;
            int visitUserCnt = 0;
            int shareCnt = 0;
            int displayOrder = 0;
            bool isSoft = false;
            List<string> newsTags = null;
            string guid = string.Empty;

            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];
            int EvaluationId = 0;
            string evaluationId = context.Request.QueryString["evaluationId"];
            int.TryParse(evaluationId, out EvaluationId);
            string strStatu = context.Request.QueryString["status"];
            int.TryParse(strStatu, out status);

            List<string> paraList = new List<string>();
            paraList.Add("CreateDateTime");
            paraList.Add("SerialId");
            paraList.Add("CarId");
            paraList.Add("Status");
            paraList.Add("EvaluationId");
            paraList.Add("Score");
            paraList.Add("CommonInfoGroup");
            paraList.Add("BodyAndSpaceGroup");
            paraList.Add("SafetyGroup");
            paraList.Add("RidingComfortGroup");
            paraList.Add("DynamicPerformanceGroup");
            paraList.Add("JsBaseGroup");
            paraList.Add("YhBaseGroup");
            paraList.Add("CostBaseGroup");
            paraList.Add("GeneralBaseGroup");
            IMongoQuery query = Query.And(Query.EQ("EvaluationId", EvaluationId), Query.EQ("Status", status));
            Dictionary<string, int> sortdic = new Dictionary<string, int>();
            sortdic.Add("CreateDateTime", 0);
            EvaluationBll evaluationBll = new EvaluationBll();
            AssessmentEntity assessmentEntity = null;
            try
            {
                assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
            }
            catch (Exception e)
            {
                message = e.Message;
                resultFlag = false;
            }
            if (assessmentEntity != null)
            {
                relationSId = assessmentEntity.SerialId.ToString();
                passTime = assessmentEntity.CreateDateTime;
                CarEntity cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, assessmentEntity.CarId);
                if (cbe != null)
                {
                    assessmentEntity.CarName = CommonFunction.GetUnicodeByString(cbe.Serial.ShowName + " " + cbe.CarYear + "款" + cbe.Name);
                    title = assessmentEntity.CarName;
                }
                else
                {
                    assessmentEntity.CarName = "";
                }
                if (assessmentEntity.CommonInfoGroup != null && assessmentEntity.CommonInfoGroup.CommonInformationEntity != null)
                {
                    coverImg = assessmentEntity.CommonInfoGroup.CommonInformationEntity.ImageUrl;
                }
            }
            else
            {
                resultFlag = false;
            }
            var jsonAsses = JsonConvert.SerializeObject(assessmentEntity);
            List<string> dataResult = new List<string>();
            dataResult.Add(string.Format("\"Id\":{0}", evaluationId));
            dataResult.Add(string.Format("\"Title\":\"{0}\"", title));
            dataResult.Add(string.Format("\"Content\":{0}", jsonAsses));
            dataResult.Add(string.Format("\"CoverImg\":\"{0}\"", coverImg));
            dataResult.Add(string.Format("\"RelationMasterId\":\"{0}\"", relationMId));
            dataResult.Add(string.Format("\"RelationModelId\":\"{0}\"", relationSId));
            dataResult.Add(string.Format("\"Keyword\":\"{0}\"", keyword));
            dataResult.Add(string.Format("\"Status\":{0}", status));
            dataResult.Add(string.Format("\"MediaId\":{0}", mediaId));
            dataResult.Add(string.Format("\"MediaName\":\"{0}\"", mediaName));
            dataResult.Add(string.Format("\"PassTime\":\"{0}\"", passTime.ToString()));
            dataResult.Add(string.Format("\"IsDeleted\":{0}", isDeleted));
            dataResult.Add(string.Format("\"CommentsCount\":{0}", commentsCnt));
            dataResult.Add(string.Format("\"VisitCount\":{0}", visitCnt));
            dataResult.Add(string.Format("\"VisitUsersCount\":{0}", visitUserCnt));
            dataResult.Add(string.Format("\"ShareCount\":{0}", shareCnt));
            dataResult.Add(string.Format("\"DisplayOrder\":{0}", displayOrder));
            dataResult.Add(string.Format("\"Guid\":\"{0}\"", guid));
            dataResult.Add(string.Format("\"IsSoft\":{0}", isSoft == true ? "true" : "false"));
            dataResult.Add(string.Format("\"NewsTags\":null"));
            List<string> listResult = new List<string>();
            listResult.Add(string.Format("\"success\":{0}", resultFlag == true ? "true" : "false"));
            listResult.Add(string.Format("\"status\":{0}", status));
            listResult.Add(string.Format("\"message\":\"{0}\"", message));
            listResult.Add(string.Format("\"data\":{0}", "{" + string.Join(",", dataResult.ToArray()) + "}"));

            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, "{" + string.Join(",", listResult.ToArray()) + "}") : "{" + string.Join(",", listResult.ToArray()) + "}");
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