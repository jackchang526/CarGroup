using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetMasterInfo 的摘要说明
	/// add dept = getmasterinfo 取主品牌基本信息 for liurw
	/// add dept = getmasterinfo 取主品牌基本信息 for huzh 增加主品牌首字母
	/// </summary>
	public class GetMasterInfo : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		private string dept = "";
		private StringBuilder sb = new StringBuilder();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			response = context.Response;
			request = context.Request;
			if (!string.IsNullOrEmpty(request.QueryString["dept"]))
			{ dept = request.QueryString["dept"].ToString().Trim().ToLower(); }

			switch (dept)
			{
				case "getmasterinfo": RenderMasterInfo(); break;
				default: CommonFunction.EchoXml(response, "<!-- 缺少参数 -->", ""); ; break;
			}
		}

		/// <summary>
		/// 主品牌基本信息
		/// </summary>
		private void RenderMasterInfo()
		{
			string sql = @"select cmb.bs_Id,cmb.bs_Name,cmb.urlspell,left(cmb.spell,1)as spell,mb30.UVCount
				from Car_MasterBrand cmb
				left join Car_MasterBrand_30UV mb30
				on cmb.bs_id=mb30.bs_id
				where cmb.isState=1";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine("<Master ID=\"" + dr["bs_Id"].ToString().Trim() + "\" ");
					sb.Append(" Name=\"" + System.Security.SecurityElement.Escape(dr["bs_Name"].ToString()) + "\"");
					sb.Append(" Spell=\"" + System.Security.SecurityElement.Escape(dr["spell"].ToString()) + "\"");
					sb.Append(" UV=\"" + dr["UVCount"].ToString().Trim() + "\"");
					sb.Append(" AllSpell=\"" + System.Security.SecurityElement.Escape(dr["urlspell"].ToString()) + "\"/>");
				}
			}
			CommonFunction.EchoXml(response, sb.ToString(), "Root");
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