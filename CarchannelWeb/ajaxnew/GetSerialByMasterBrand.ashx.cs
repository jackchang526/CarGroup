using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetSerialByMasterBrand 的摘要说明
	/// </summary>
	public class GetSerialByMasterBrand : IHttpHandler
	{
		#region Param
		private int bsid = 0;
		private string type = string.Empty;
		private System.Text.StringBuilder sb = new System.Text.StringBuilder();
		#endregion

		public void ProcessRequest(HttpContext context)
		{
			// 检查参数
			this.CheckParam(context);
			if (bsid > 0 && bsid < 10000)
			{
				GetDtSerialByMasterBrand();
				context.Response.Write(sb.ToString());
			}
		}

		private void CheckParam(HttpContext context)
		{
			if (null != context.Request.QueryString["bsid"] && context.Request.QueryString["bsid"].ToString().Length != 0)
			{
				string strBsID = context.Request.QueryString["bsid"].ToString().Trim();
				if (int.TryParse(strBsID, out bsid))
				{ }
			}
			if (null != context.Request.QueryString["type"] && context.Request.QueryString["type"].ToString().Length != 0)
			{
				type = context.Request.QueryString["type"].ToString().Trim();
			}
		}

		private void GetDtSerialByMasterBrand()
		{
			DataTable dtSerials = new DataTable();
			if (null != HttpContext.Current.Cache.Get("ajaxnew_GetSerialByMasterBrand_" + bsid.ToString()))
			{
				dtSerials = (System.Data.DataTable)HttpContext.Current.Cache.Get("ajaxnew_GetSerialByMasterBrand_" + bsid.ToString());
			}
			else
			{
				dtSerials = CarMasterToSerialService.GetDTSerialByMasterBrandID(bsid);
				HttpContext.Current.Cache.Insert("ajaxnew_GetSerialByMasterBrand_" + bsid.ToString(), dtSerials, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}

			if (null != dtSerials && dtSerials.Rows.Count > 0)
			{
				if (type.ToLower() == "json")
				{
					sb.Append("[");
					for (int i = 0; i < dtSerials.Rows.Count; i++)
					{
						if (i != 0)
						{
							sb.Append(",");
						}
						sb.Append("{");
						sb.Append("\"ID\":" + dtSerials.Rows[i]["ID"].ToString().Trim() + ",");
						sb.Append("\"Name\":\"" + dtSerials.Rows[i]["Name"].ToString().Trim() + "\"");
						sb.Append("}");
					}
					sb.Append("]");
				}
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