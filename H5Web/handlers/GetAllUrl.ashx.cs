using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;

namespace H5Web.handlers
{
	/// <summary>
	/// GetAllUrl 的摘要说明
	/// </summary>
	public class GetAllUrl : H5PageBase, IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(60);

			context.Response.ContentType = "Text/XML";
			// context.Response.Write("Hello World");

			string wtStr = (string.IsNullOrEmpty(context.Request["WT.mc_id"]) ? "?WT.mc_id=tempdefault" : "?WT.mc_id=" + context.Request["WT.mc_id"].ToString().Trim());

			List<string> listTempStr = new List<string>();
			XmlDocument doc = AutoStorageService.GetAllAutoXml();
			List<int> listAllCsID = (new SerialFourthStageBll()).GetAllSerialInH5();
			if (listAllCsID.Count > 0 && doc != null && doc.HasChildNodes)
			{
				foreach (int csid in listAllCsID)
				{
					XmlNode xnCs = doc.SelectSingleNode("/Params/MasterBrand/Brand/Serial[@ID='" + csid + "']");
					if (xnCs != null)
					{
						string csName = System.Security.SecurityElement.Escape(xnCs.Attributes["Name"].Value.ToString());
						string csAllSpell = xnCs.Attributes["AllSpell"].Value.ToString();

						string cbName = System.Security.SecurityElement.Escape(xnCs.ParentNode.Attributes["Name"].Value.ToString());
						string bsName = System.Security.SecurityElement.Escape(xnCs.ParentNode.ParentNode.Attributes["Name"].Value.ToString());

						listTempStr.Add(string.Format("<Item BsName=\"{0}\" CbName=\"{1}\" ID=\"{2}\" Name=\"{3}\" Url=\"http://car.h5.yiche.com/{4}/{5}\" Img=\"http://image.bitautoimg.com/carchannel/pic/cspic/{2}.jpg\" />"
							, bsName, cbName, csid, csName, csAllSpell, wtStr));
					}
				}
			}
			if (listTempStr.Count > 0)
			{
				CommonFunction.EchoXml(context.Response, string.Join("", listTempStr.ToArray()), "Root");
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