using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
    /// <summary>
    /// ListForComparev2._1 的摘要说明
    /// </summary>
    public class ListForComparev2 : IHttpHandler {

		private StringBuilder sb = new StringBuilder();
		private string pageXML = string.Empty;
		private int type = 1;

        public void ProcessRequest (HttpContext context) {
			context.Response.Cache.SetNoServerCaching();
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(60));
			GetPageParam(context);
			context.Response.ContentType = "text/xml";
			//string keyTemp = "ListForComparev2.1_ashx" + type.ToString();
			object listForCompare_ashx = null;
			//CacheManager.GetCachedData(keyTemp, out listForCompare_ashx);
			if (listForCompare_ashx == null)
			{
				CommonService cs = new CommonService();
				List<XmlElement> lxe = cs.GetAllSerialForCompareListForPrice(type);
				if (lxe != null && lxe.Count > 0)
				{
					sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
					sb.Append("<Root>");
					foreach (XmlElement xe in lxe)
					{
						// sb.Append(xe.InnerXml);
						sb.Append("<Item ID=\"" + xe.GetAttribute("ID").Trim() + "\" ");
						sb.Append(" Name=\"" + xe.GetAttribute("Name").Trim() + "\" ");
						sb.Append(" ShowName=\"" + xe.GetAttribute("ShowName").Trim() + "\" ");
						sb.Append(" Level=\"" + xe.GetAttribute("CsLevel").Trim() + "\" ");
						sb.Append(" BodyType=\"" + xe.GetAttribute("BodyType").Trim() + "\" ");
						sb.Append(" MultiPriceRange=\"" + xe.GetAttribute("MultiPriceRange").Trim() + "\" ");
						sb.Append(" Pv=\"" + xe.GetAttribute("CsPV").Trim() + "\" />");
						// xe.GetAttribute("CsLevel");
					}
					sb.Append("</Root>");
					pageXML = sb.ToString();
				}
				// pageXML = cs.GetAllSerialForCompareListForPrice();
				//CacheManager.InsertCache(keyTemp, pageXML, 10);
			}
			else
			{
				pageXML = Convert.ToString(listForCompare_ashx);
			}
			context.Response.Write(pageXML);
        }

		private void GetPageParam(HttpContext contex)
		{
			if (contex.Request.QueryString["type"] != null && contex.Request.QueryString["type"].ToString() != "")
			{
				string strType = contex.Request.QueryString["type"].ToString();
				if (int.TryParse(strType, out type))
				{
					if (type < 1 || type > 12)
					{ type = 1; }
				}
			}
		}
     
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}