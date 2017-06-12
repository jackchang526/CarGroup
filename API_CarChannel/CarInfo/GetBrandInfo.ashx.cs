using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetBrandInfo 的摘要说明
	/// add op = getcb for zhouxf Jun.27.2013 迁移car域名接口： http://car.bitauto.com/interfaceforbitauto/alldbforcar.aspx
	/// </summary>
	public class GetBrandInfo : PageBase,IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		private StringBuilder sb = new StringBuilder();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			response = context.Response;
			request = context.Request;
			string op = request.QueryString["op"];
			if (!string.IsNullOrEmpty(op))
			{ op = op.Trim().ToLower(); }
			switch (op)
			{
				case "getcb": RenderCBData(); break;
			}
		}

		private void RenderCBData()
		{
			DataSet dsCB = base.GetAllCBForInterface();
			if (dsCB != null && dsCB.Tables[0].Rows.Count > 0)
			{
				sb.AppendLine("<Brand>");
				for (int i = 0; i < dsCB.Tables[0].Rows.Count; i++)
				{
					sb.AppendLine("<Item Cb_Id=\"" + dsCB.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					sb.AppendLine(" Cp_Id=\"" + dsCB.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					sb.AppendLine(" Spell=\"" + System.Security.SecurityElement.Escape(dsCB.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					sb.AppendLine(" Cb_Name=\"" + System.Security.SecurityElement.Escape(dsCB.Tables[0].Rows[i]["cb_name"].ToString()) + "\" ");
					sb.AppendLine(" OldCs_Id=\"" + dsCB.Tables[0].Rows[i]["OldCs_Id"].ToString() + "\" />");
				}
				sb.AppendLine("</Brand>");
			}
			CommonFunction.EchoXml(response, sb.ToString(), "AutoCarChannel");
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