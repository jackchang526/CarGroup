using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// SavePageDestop 的摘要说明
	/// </summary>
	public class SavePageDestop : IHttpHandler
	{
		private int serialId = 0;

		public void ProcessRequest(HttpContext context)
		{
			GetParams(context);
			Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialId);
			if (cse == null || cse.Cs_Id < 1) return;
			//设置保存桌面链接
			SetSaveDestopUrl(cse.Cs_SeoName, cse.Cs_AllSpell.ToLower(), context);
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		/// <param name="context"></param>
		private void GetParams(HttpContext context)
		{
			serialId = string.IsNullOrEmpty(context.Request.QueryString["id"])
				? 0 : ConvertHelper.GetInteger(context.Request.QueryString["id"]);
		}
		/// <summary>
		/// 设置保存桌面链接
		/// </summary>
		private void SetSaveDestopUrl(string name, string spell, HttpContext context)
		{
			context.Response.ClearContent();

			if (HttpContext.Current.Request.Browser.Browser != "IE")
			{
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + name + ".url");
			}
			else
			{
				string sitename = HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(name + ".url"));
				context.Response.AddHeader("content-disposition", "attachment; filename=" + sitename);
			}
			context.Response.ContentType = "APPLICATION/OCTET-STREAM";

			StringBuilder content = new StringBuilder();
			content.Append("[InternetShortcut]");
			content.Append(System.Environment.NewLine);
			content.AppendFormat("URL=http://car.bitauto.com/{0}/", spell);
			content.Append(System.Environment.NewLine);
			content.AppendLine("IDList=");
			content.AppendLine("[{000214A0-0000-0000-C000-000000000046}]");
			content.AppendLine("Prop3=19,2");
			context.Response.Write(content.ToString());

			//context.Response.Write("[InternetShortcut]/r/n");
			//context.Response.Write(string.Format("URL=http://car.bitauto.com/{0}//r/n", spell));
			//context.Response.Write("IDList=/r/n");
			//context.Response.Write("[{000214A0-0000-0000-C000-000000000046}]/r/n");
			//context.Response.Write("Prop3=19,2/r/n");
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