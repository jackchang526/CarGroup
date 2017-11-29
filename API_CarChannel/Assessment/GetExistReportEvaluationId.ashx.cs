using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetExistReportEvaluationId 的摘要说明
    /// 说明：输出有评测报告的评测ID列表
    /// </summary>
    public class GetExistReportEvaluationId : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string result = "{}";
            string callback = context.Request.QueryString["callback"];
            List<int> res = new List<int>();
            try
            {
                EvaluationBll evaluationBll = new EvaluationBll();               
                res = evaluationBll.GetExistReportEvaluationId();
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            result = JsonConvert.SerializeObject(res);
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