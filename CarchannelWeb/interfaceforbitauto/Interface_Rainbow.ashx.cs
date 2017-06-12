using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using System.Collections.Generic;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// Interface_Rainbow 的摘要说明
	/// </summary>
	public class Interface_Rainbow : InterfacePageBase, IHttpHandler
	{
		private int csid = -1;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/XML";

			try
			{
				string strCsID = context.Request.QueryString["csid"];
				Int32.TryParse(strCsID, out csid);

				if (csid > 0)
				{
					context.Response.Write(new RainbowListBll().GetRainbowListXML_CSID(csid));
				}
				else
				{
					string cacheKey = "Interface_Rainbow_xml";
					string xmlStr = (string)CacheManager.GetCachedData(cacheKey);
					if (xmlStr == null)
					{
						xmlStr = "";
						string xmlFileName = Path.Combine(WebConfig.DataBlockPath, "Data\\Interface_Rainbow.xml");
						if (File.Exists(xmlFileName))
						{
							xmlStr = File.ReadAllText(xmlFileName, Encoding.UTF8);
						}
						CacheManager.InsertCache(cacheKey, xmlStr, 10);
					}
					context.Response.Write(xmlStr);
				}
			}
			catch { }
			context.Response.End();
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