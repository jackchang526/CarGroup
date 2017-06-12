using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	public partial class SerialBaseInfoXML : PageBase
	{
		#region member

		private StringBuilder sb = new StringBuilder();
		private string op = string.Empty;
		private string dept = string.Empty;
		private string templateStr = "<Cs ID=\"{0}\" Name=\"{1}\" RP=\"{2}\" P=\"{3}\" JJ=\"{4}\"/>";

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(60);
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><Root>");
				GetPageParam();
				if (dept == "photo")
				{
					if (op == "getserialbaseinfo")
					{
						GetCsBaseInfoData();
					}
				}
				else
				{ sb.Append("<!-- 无效参数 -->"); }
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		#region private Method

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{

			if (!string.IsNullOrEmpty(this.Request.QueryString["op"]))
			{ op = this.Request.QueryString["op"].ToString().ToLower(); }

			if (!string.IsNullOrEmpty(this.Request.QueryString["dept"]))
			{ dept = this.Request.QueryString["dept"].ToString().ToLower(); }
		}

		/// <summary>
		/// 生成数据
		/// </summary>
		private void GetCsBaseInfoData()
		{
			sb.AppendLine("<!-- Cs:子品牌节点 ID:子品牌ID Name:子品牌名 RP:指导价区间(非停销车型) P:报价区间 JJ:子品牌最大降幅 -->");
			// 子品牌非停销指导价区间
			Dictionary<int, string> dicCsOfficePrice = new Car_SerialBll().GetAllSerialOfficePriceBySaleState(false);
			// 子品牌报价区间
			Dictionary<int, string> dicCsPrice = base.GetAllCsPriceRange();
			// 子品牌全国降价
			Dictionary<int, string> dicJiangJia = new CarNewsBll().GetAllSerialJiangJia();

			DataSet dsCs = new Car_SerialBll().GetAllValidSerial();
			if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCs.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					string csName = System.Security.SecurityElement.Escape(dr["cs_name"].ToString().Trim());
					string rp = dicCsOfficePrice.ContainsKey(csid) ? dicCsOfficePrice[csid] : "";
					string p = dicCsPrice.ContainsKey(csid) ? dicCsPrice[csid] : "";
					string jj = dicJiangJia.ContainsKey(csid) ? dicJiangJia[csid] : "";
					sb.AppendLine(string.Format(templateStr, csid, csName, rp, p, jj));
				}

			}
		}

		#endregion

	}
}