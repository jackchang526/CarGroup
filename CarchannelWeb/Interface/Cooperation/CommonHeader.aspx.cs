using System;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Cooperation
{
	public partial class CommonHeader : InterfacePageBase
	{
		protected string m_RequestType = string.Empty;
		protected string m_PvSerailList = string.Empty;
		protected string m_newSerialList = string.Empty;
		private Car_SerialBll csb = new Car_SerialBll();

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			InitData();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			m_RequestType = string.IsNullOrEmpty(Request.QueryString["type"]) ? "s" : Request.QueryString["type"];
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			InitPVSerialData();
			InitNewsSerialData();
		}
		/// <summary>
		/// 初始化最近车型
		/// </summary>
		private void InitNewsSerialData()
		{
			DataSet ds = csb.GetNewsSerialList();
			if (ds == null) return;
			m_newSerialList = BuildSpan(ds, "csshowname", "allspell");
		}
		/// <summary>
		/// 初始化新门车型
		/// </summary>
		private void InitPVSerialData()
		{
			DataSet ds = new PageBase().GetAllSerialNewly30Day();
			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return;
			m_PvSerailList = BuildSpan(ds, "cs_showname", "allspell");
		}
		/// <summary>
		/// 生成块
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="CsName"></param>
		/// <param name="CsSpell"></param>
		/// <returns></returns>
		private string BuildSpan(DataSet ds, string CsName, string CsSpell)
		{
			if (ds == null) return "";
			int step = m_RequestType == "s" ? 4 : 3;
			StringBuilder newsSerail = new StringBuilder();
			for (int i = 0; i < step; i++)
			{
				newsSerail.AppendFormat("<dl class=\"{0}\">", BuildStyle(i + 1));
				for (int j = i * 3; j < (i * 3 + 3); j++)
				{
					if (ds.Tables[0].Rows.Count < j) break;
					newsSerail.AppendLine("<dd>");
					DataRow dr = ds.Tables[0].Rows[j];
					newsSerail.AppendFormat("<a href=\"http://car.bitauto.com/{0}/\" target=\"_blank\">{1}</a>"
						, dr[CsSpell].ToString().ToLower()
						, dr[CsName].ToString());
					newsSerail.AppendLine("</dd>");
				}
				newsSerail.AppendLine("</dl>");
			}
			return newsSerail.ToString();
		}
		/// <summary>
		/// 生成样式名
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private string BuildStyle(int i)
		{
			switch (i)
			{
				case 1:
					return "none";
				case 2:
					return "fist";
				case 3:
					return "";
				case 4:
					return "fist";
				default:
					return "";
			}
		}
	}
}