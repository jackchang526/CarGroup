using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using MongoDB.Bson;
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
    /// GetData 的摘要说明
    /// </summary>
    public class GetData : IHttpHandler
    {
        private EvaluationBll evaluationBll;
        public GetData()
        {
            evaluationBll = new EvaluationBll();
        }
        private HttpRequest request;
        private HttpResponse response;
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);

            response = context.Response;
            request = context.Request;
            string op = request.QueryString["op"];
            string action = request.QueryString["act"];

            if (op == "platform")
            {
                switch (action)
                {
                    case "getevaldatabydate": RanderDataByUpdateDateTime(); break;
                }
            }
        }

        private void RanderDataByUpdateDateTime()
        {
            int status = 0;
            string message = "no";
            DateTime date1 = ConvertHelper.GetDateTime(request.QueryString["date1"]);
            string date2 = request.QueryString["date2"];
            IMongoQuery query;
            if (CommonFunction.DateDiff("d", date1, DateTime.Now) <= 0)
            {
                response.Write(JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = "日期需小于当前日期！",
                    data = new { }
                }));
                return;
            }
            query = Query.And(
                Query.EQ("Status", 1),
                Query.GTE("UpdateDateTime", date1),
                Query.LT("UpdateDateTime", date1.AddDays(1)));
            if (!string.IsNullOrEmpty(date2))
            {
                DateTime d2 = ConvertHelper.GetDateTime(date2);
                if (CommonFunction.DateDiff("d", date1, DateTime.Now) <= 0 || CommonFunction.DateDiff("d", date1, d2) <= 0)
                {
                    response.Write(JsonConvert.SerializeObject(new
                    {
                        status = 0,
                        message = "日期输入错误",
                        data = new { }
                    }));
                    return;
                }
                query = Query.And(
                    Query.EQ("Status", 1),
                Query.GTE("UpdateDateTime", date1),
                Query.LT("UpdateDateTime", d2)
                );
            }

            List<AssessmentEntity> pingCeAllList = evaluationBll.GetPingCeAllList<AssessmentEntity>(query, 100, null, null);
            if (pingCeAllList.Any())
            {
                status = 1;
                message = "ok";
            }
            response.Write(JsonConvert.SerializeObject(new
            {
                status = status,
                message = message,
                data = new { list = pingCeAllList }
            }
            ));
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