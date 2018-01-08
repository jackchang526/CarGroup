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
    /// GetEvalInfoForAppNews 的摘要说明
    /// </summary>
    public class GetEvalInfoForAppNews : IHttpHandler
    {

        /// <summary>
        /// 为易车app新闻要闻提供的评测库（包含评测报告mongo）数据接口,参数evaluationId ,status
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string message = "ok";
            int status = 1;
            string title = string.Empty;
            string coverImg = string.Empty;
            string relationMId = string.Empty;
            string relationSId = string.Empty;
            string keyword = string.Empty;
            string mediaName = string.Empty;
            DateTime passTime = new DateTime();
            bool resultFlag = true;
            //int mediaId = 0;
            //int isDeleted = 0;
            //int commentsCnt = 0;
            //int visitCnt = 0;
            //int visitUserCnt = 0;
            //int shareCnt = 0;
            //int displayOrder = 0;
            //bool isSoft = false;
            //List<string> newsTags = null;
            string guid = string.Empty;

            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];
            int EvaluationId = 0;
            string evaluationId = context.Request.QueryString["evaluationId"];
            int.TryParse(evaluationId, out EvaluationId);
            string strStatu =context.Request.QueryString["status"];
            int.TryParse(strStatu, out status);

            List<string> paraList = new List<string>();
            paraList.Add("CreateDateTime");
            paraList.Add("SerialId");
            paraList.Add("CarId");
            paraList.Add("Status");
            paraList.Add("EvaluationId");
            paraList.Add("Score");
            paraList.Add("CommonInfoGroup");
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
            List<string> dataResult = new List<string>();
            dataResult.Add(string.Format("\"Id\":{0}", evaluationId));
            dataResult.Add(string.Format("\"Title\":\"{0}\"", title));
            dataResult.Add(string.Format("\"CoverImg\":\"{0}\"", coverImg));
            dataResult.Add(string.Format("\"RelationMasterId\":\"{0}\"", relationMId));
            dataResult.Add(string.Format("\"RelationModelId\":\"{0}\"", relationSId));

            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, "{" + string.Join(",", dataResult.ToArray()) + "}") : "{" + string.Join(",", dataResult.ToArray()) + "}");
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