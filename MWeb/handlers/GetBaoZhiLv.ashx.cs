using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace MWeb.handlers
{
    /// <summary>
    /// GetBaoZhiLv 的摘要说明
    /// </summary>
    public class GetBaoZhiLv : IHttpHandler
    {
        private int pageSize = 0;
        private int pageIndex;
        private string level;
        private Car_SerialBll serialBll = new Car_SerialBll();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            GetParmas(context);
            if (string.IsNullOrEmpty(level))
            {
                context.Response.Write("");
                return;
            }
            RenderHtml(context);
        }

        /// <summary>
        /// 生成html代码
        /// </summary>
        private void RenderHtml(HttpContext context)
        {
            int LevelId = (int)(SerialLevelSpellEnum)Enum.Parse(typeof(SerialLevelSpellEnum), level);
            string LevelName = CarLevelDefine.GetLevelNameById(LevelId);
            List<XmlElement> list = serialBll.GetSerialBaoZhiLv(LevelName);
            if (list != null && list.Count > 0)
            {
                int count = list.Count;
                int start = (pageIndex - 1) * pageSize;
                int end = start + pageSize - 1;

                if (start > count)
                {
                    context.Response.Write("");
                    return;
                }
                if (count < end)
                {
                    end = count;
                }

                StringBuilder sb = new StringBuilder();
                foreach (XmlElement ele in list)
                {
                    int index = list.IndexOf(ele);
                    if (index < start)
                        continue;
                    if (index > end)
                        break;

                    sb.AppendFormat("<li><i{3}>{0}</i><a href=\"/{4}/\">{1}</a><strong>{2}%</strong></li>",
                        (index + 1) < 10 ? (index + 1).ToString().PadLeft(2, '0') : (index + 1).ToString(),
                        ele.Attributes["ShowName"].InnerText,
                        Math.Round(ConvertHelper.GetDouble(ele.Attributes["ResidualRatio5"].InnerText) * 100.0, 1),
                       (index < 3) ? " class=\"n" + (index + 1) + "\"" : "",
                       ele.Attributes["AllSpell"].InnerText
                        );
                }
                context.Response.Write(sb.ToString());
                return;
            }
            context.Response.Write("");
            return;
        }

        private void GetParmas(HttpContext context)
        {
            pageIndex = ConvertHelper.GetInteger(context.Request["pageIndex"]);
            pageSize = ConvertHelper.GetInteger(context.Request["pageSize"]);
            level = context.Request["level"];
            if (pageIndex <= 1)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
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