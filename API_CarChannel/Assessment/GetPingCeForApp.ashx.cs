using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetPingCeForApp 的摘要说明
    /// </summary>
    public class GetPingCeForApp : IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        public void ProcessRequest(HttpContext context)
        {
            //PageHelper.SetPageCache(60);

            response = context.Response;
            request = context.Request;
            response.ContentType = "application/x-javascript";
            
            int status = 0;
            string message = "success";
            var action = request.QueryString["action"];
            WriteResult<object> writeResult = new WriteResult<object>();

            switch (action)
            {
                case "intr":                    
                    break;                
                default:                    
                    break;
            }
            writeResult.Status = status;
            writeResult.Message = message;

            PingCeWrite(writeResult);
        }

        private void PingCeWrite<T>(WriteResult<T> result)
        {
            string callback = request.QueryString["callback"];
            var json = JsonConvert.SerializeObject(result);
            response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
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