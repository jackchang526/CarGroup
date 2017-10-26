using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetHotMaster 的摘要说明
    /// </summary>
    public class GetHotMaster : PageBase,IHttpHandler
    {

        private int num = 0;
        private StringBuilder sbHtml = new StringBuilder();

        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParam(context);
            GetHotMasterJson(num);
            context.Response.Write(sbHtml.ToString());
        }


        private void GetHotMasterJson(int num)
        {
            DataSet ds = new Car_MasterBrandBll().GetHotMasterBrand(num);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                sbHtml.Append("{\"master\":[");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        count++;
                        sbHtml.AppendFormat("{{\"Id\":\"{0}\",\"Name\":\"{1}\",\"Spell\":\"{2}\"}}{3}"
                            , dr["bs_Id"]
                            , dr["bs_Name"]
                            , dr["urlspell"]
                            , count == ds.Tables[0].Rows.Count ? "":",");
                    }
                }
                sbHtml.Append("]}");
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParam(HttpContext context)
        {
            num = ConvertHelper.GetInteger(context.Request["num"]);
            if (num <= 0)
            {
                num = 5;
            }
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