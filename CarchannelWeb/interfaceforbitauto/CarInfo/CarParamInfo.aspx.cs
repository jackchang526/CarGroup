using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	public partial class CarParamInfo : InterfacePageBase
	{
		private string dept = "";
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "carparaminfo")
				{
					GetCarParamInfo();
				}
				else
				{
					sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Root><!-- 无效参数 --></Root>");
				}
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
		}

		/// <summary>
		/// 取所有车型参数的信息
		/// </summary>
		private void GetCarParamInfo()
		{
			string sql = @"select ParamId,GradeNum,ParamName,AliasName,ModuleDec,IsParent
						from dbo.ParamList 
						where IsState=1
						order by GradeNum";
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<CarParanList>");
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString,
				CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				int loop = 0;
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int ParamId = int.Parse(dr["ParamId"].ToString());
					int IsParent = int.Parse(dr["IsParent"].ToString());
					string ParamName = System.Security.SecurityElement.Escape(dr["ParamName"].ToString().Trim());
					string AliasName = System.Security.SecurityElement.Escape(dr["AliasName"].ToString().Trim());
					string ModuleDec = System.Security.SecurityElement.Escape(dr["ModuleDec"].ToString().Trim());

					if (IsParent == 1)
					{
						// 是否是分类
						if (loop > 0)
						{
							sb.AppendLine("</Group>");
						}
						sb.AppendLine("<Group ID=\"" + ParamId + "\" Name=\"" + ParamName + "\" >");
					}
					else
					{
						// 参数
						sb.AppendLine("<Param ID=\"" + ParamId + "\" Name=\"" + ParamName + "\" EName=\"" + AliasName + "\" Unit=\"" + ModuleDec + "\" />");
					}
					loop++;
				}
				sb.AppendLine("</Group>");
			}
			sb.AppendLine("</CarParanList>");

		}
	}
}