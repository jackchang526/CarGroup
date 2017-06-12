using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 运营决策系统指标需求(bitauto:董博)
	/// seoforserial:SEO杨建姣
	/// </summary>
	public partial class StatisticInfo : InterfacePageBase
	{
		private string dept = "";
		private StringBuilder sb = new StringBuilder();
		private DateTime dt = DateTime.Now.AddDays(-1);

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(10);
				GetPageParam();
				if (dept == "bitauto")
				{
					// 取车型频道统计信息
					GetStatisticInfo();
				}
				else if (dept == "seoforserial")
				{
					// 取子品牌名、拼音、创建时间 for SEO
					GetSerialOrderByTime();
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

			if (this.Request.QueryString["reqdate"] != null && this.Request.QueryString["reqdate"].ToString() != "")
			{
				if (DateTime.TryParse(this.Request.QueryString["reqdate"].ToString(), out dt))
				{ }
			}
			//if (dt == null)
			//{
			//    dt = DateTime.Now.AddDays(-1);
			//}
		}

		/// <summary>
		/// 取车型频道统计信息
		/// </summary>
		private void GetStatisticInfo()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendLine("<COUNT>");
			sb.AppendLine("<product>车型产品研发部</product>");
			sb.AppendLine("<author>段德龙、程亮</author>");
			sb.AppendLine("<date>" + dt.ToShortDateString() + "</date>");
			sb.AppendLine("<items>");
			string sql = "";

			#region 厂商
			sql = "select count(*) as sumCount from dbo.Car_producer ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and isState=0";
			int CpSumCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CpSumCount\" value=\"" + CpSumCount.ToString() + "\" desc=\"厂商总数\"/>");

			sql = "select count(*) as sumCount from dbo.Car_producer ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and CreateTime>='" + dt.ToShortDateString() + "' ";
			int CpAddCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CpAddCount\" value=\"" + CpAddCount.ToString() + "\" desc=\"厂商新增数\"/>");
			#endregion

			#region 主品牌
			sql = " select count(*) from dbo.Car_MasterBrand ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and isState=0 ";
			int BsSumCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"BsSumCount\" value=\"" + BsSumCount.ToString() + "\" desc=\"主品牌总数\"/>");

			sql = "select count(*) from dbo.Car_MasterBrand ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and CreateTime>='" + dt.ToShortDateString() + "' ";
			int BsAddCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"BsAddCount\" value=\"" + BsAddCount.ToString() + "\" desc=\"主品牌新增数\"/>");
			#endregion

			#region 品牌
			sql = "select count(*) from dbo.Car_Brand ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and isState=0 ";
			int CbSumCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CbSumCount\" value=\"" + CbSumCount.ToString() + "\" desc=\"品牌总数\"/>");

			sql = "select count(*) from dbo.Car_Brand ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and CreateTime>='" + dt.ToShortDateString() + "' ";
			int CbAddCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CbAddCount\" value=\"" + CbAddCount.ToString() + "\" desc=\"品牌新增数\"/>");
			#endregion

			#region 子品牌
			sql = "select count(*) from dbo.Car_Serial ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and isState=0 ";
			int CsSumCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CsSumCount\" value=\"" + CsSumCount.ToString() + "\" desc=\"子品牌总数\"/>");

			sql = "select count(*) from dbo.Car_Serial ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and CreateTime>='" + dt.ToShortDateString() + "' ";
			int CsAddCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CsAddCount\" value=\"" + CsAddCount.ToString() + "\" desc=\"子品牌新增数\"/>");
			#endregion

			#region 车型
			sql = "select count(car.car_id) from dbo.Car_relation car ";
			sql += " left join car_serial cs on car.cs_id=cs.cs_id ";
			sql += " where car.CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and car.isState=0 and cs.isState=0 ";
			int CarSumCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CarSumCount\" value=\"" + CarSumCount.ToString() + "\" desc=\"车型总数\"/>");

			sql = "select count(*) from dbo.Car_relation ";
			sql += " where CreateTime<'" + dt.AddDays(1).ToShortDateString() + "' and CreateTime>='" + dt.ToShortDateString() + "' ";
			int CarAddCount = Convert.ToInt32(BitAuto.Utils.Data.SqlHelper.ExecuteScalar(WebConfig.AutoStorageConnectionString, CommandType.Text, sql));

			sb.AppendLine("<item name=\"CarAddCount\" value=\"" + CarAddCount.ToString() + "\" desc=\"车型新增数\"/>");
			#endregion

			if (dt >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
			{
				sb.AppendLine("<error>请传小于当前时间的时间点</error>");
			}
			else
			{
				sb.AppendLine("<error></error>");
			}
			sb.AppendLine("</items>");
			sb.AppendLine("</COUNT>");

		}

		/// <summary>
		/// 取子品牌名、拼音、创建时间 for SEO
		/// </summary>
		private void GetSerialOrderByTime()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendLine("<Root>");
			string sql = "select cs_id,csname,allSpell,createtime from car_serial where isState=0 order by createtime desc";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine("<Serial CsID=\"" + dr["cs_id"].ToString() + "\" CsName=\"" + dr["csname"].ToString().Replace("&", "&amp;") + "\" AllSpell=\"" + dr["allSpell"].ToString().ToLower() + "\" CreateTime=\"" + dr["createtime"].ToString() + "\" />");
				}
			}
			sb.AppendLine("</Root>");

		}
	}
}