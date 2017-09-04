using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.MongoDB;
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
    /// GetAssessmentInfo 的摘要说明
    /// </summary>
    public class GetAssessmentInfo : IHttpHandler
    {
        /// <summary>
        /// 评测信息接口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];

            int EvaluationId = 0;
            string evaluationId = context.Request.QueryString["evaluationId"];
            int.TryParse(evaluationId, out EvaluationId);

            string groupname = context.Request.QueryString["groupname"];
            List<string> paraList = new List<string>();
            paraList.Add("CreateDateTime");
            paraList.Add("SerialId");
            paraList.Add("CarId");
            paraList.Add("Status");
            paraList.Add("EvaluationId");
            paraList.Add("Score");
            paraList.Add("CommonInfoGroup");
            if (!string.IsNullOrEmpty(groupname))
            {
                paraList.Add(groupname);
            }
            //IMongoQuery query = Query.EQ("EvaluationId", EvaluationId);
            IMongoQuery query = Query.And(Query.EQ("EvaluationId", EvaluationId), Query.EQ("Status", 1));
            Dictionary<string, int> sortdic = new Dictionary<string, int>();
            sortdic.Add("CreateDateTime", 0);
            EvaluationBll evaluationBll = new EvaluationBll();
            AssessmentEntity assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
            if (assessmentEntity != null)
            {
                CarEntity cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, assessmentEntity.CarId);
                if (cbe != null)
                {
                    assessmentEntity.CarName = CommonFunction.GetUnicodeByString(cbe.Serial.ShowName + cbe.CarYear + "款" + cbe.Name);
                }
                else
                {
                    assessmentEntity.CarName = "";
                }
            }           
            var json = JsonConvert.SerializeObject(assessmentEntity);
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