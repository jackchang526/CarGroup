using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 口碑数据接口(熊玉辉)
	/// </summary>
	public partial class AllSerialLinkInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private XmlDocument docCount = new XmlDocument();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.GetSerialNewsCount();
				this.GetAllSerialLinkInfo();
			}
		}

		private void GetAllSerialLinkInfo()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialLinkInfo>");
			DataSet ds = base.GetAllSErialInfo();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Serial CsID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CsName=\"" + ds.Tables[0].Rows[i]["cs_name"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" CsShowName=\"" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" CsAllSpell=\"" + ds.Tables[0].Rows[i]["allSpell"].ToString().ToLower() + "\" ");
					sb.Append(" CsVirtues=\"" + ds.Tables[0].Rows[i]["cs_Virtues"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" CsDefect=\"" + ds.Tables[0].Rows[i]["cs_Defect"].ToString().Replace("&", "&amp;") + "\" ");
					string csPic = "";
					int csCount = 0;
					base.GetSerialPicAndCountByCsID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), out csPic, out csCount, false);
					sb.Append(" CsPicCount=\"" + csCount + "\" >");
					// 文章 易车评测、买车测试、科技、安全
					sb.Append("<SerialLink>");
					DataSet csLink = base.GetAllSerialRainbowAndURLInfo();
					sb.Append("<Link Name=\"易车评测\" URL=\"" + GetSerialLinkByCsIDAndRainbowItemID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 43, csLink) + "\" />");
					sb.Append("<Link Name=\"买车测试\" URL=\"" + GetSerialLinkByCsIDAndRainbowItemID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 39, csLink) + "\" />");
					sb.Append("<Link Name=\"科技\" URL=\"" + GetSerialLinkByCsIDAndRainbowItemID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 41, csLink) + "\" />");
					sb.Append("<Link Name=\"安全\" URL=\"" + GetSerialLinkByCsIDAndRainbowItemID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 44, csLink) + "\" />");
					this.GetSerialNewLinkByID(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), ds.Tables[0].Rows[i]["allSpell"].ToString().ToLower());
					sb.Append("</SerialLink>");
					sb.Append("</Serial>");
				}
			}
			sb.Append("</SerialLinkInfo>");
			Response.Write(sb.ToString());
		}

		private string GetSerialLinkByCsIDAndRainbowItemID(int csID, int rainbowItemID, DataSet dsLink)
		{
			string link = "";
			if (dsLink != null && dsLink.Tables.Count > 0 && dsLink.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsLink.Tables[0].Select(" csid =" + csID.ToString() + " and rainbowItemID= " + rainbowItemID + " ");
				if (drs != null && drs.Length > 0)
				{
					link = drs[0]["URL"].ToString();
				}
			}
			return link;
		}

		private void GetSerialNewsCount()
		{
			string filePath = WebConfig.DataBlockPath + "Data\\SerialNews\\newsNum.xml";
			if (File.Exists(filePath))
			{
				try
				{
 					//modified by sk 2013.04.28 统一读取文件方法
					docCount = CommonFunction.ReadXmlFromFile(filePath);
				}
				catch
				{ }
			}
		}

		private void GetSerialNewLinkByID(int csID, string allSpell)
		{
			string linkShiJia = "";
			string linkYongChe = "";
			string linkXinwen = "";
			string linkHangQing = "";
			string linkDaoGou = "";
			if (docCount != null && docCount.HasChildNodes)
			{
				XmlNodeList xnl = docCount.SelectNodes("/SerilaList/Serial[@id='" + csID.ToString() + "']");
				if (xnl != null && xnl.Count > 0)
				{
					try
					{
						if (xnl[0].Attributes["xinwen"].Value.ToString() != "0")
						{ linkXinwen = "http://car.bitauto.com/" + allSpell.Trim().ToLower() + "/xinwen/"; }
						if (xnl[0].Attributes["daogou"].Value.ToString() != "0")
						{ linkDaoGou = "http://car.bitauto.com/" + allSpell.Trim().ToLower() + "/daogou/"; }
						if (xnl[0].Attributes["shijia"].Value.ToString() != "0")
						{ linkShiJia = "http://car.bitauto.com/" + allSpell.Trim().ToLower() + "/shijia/"; }
						if (xnl[0].Attributes["yongche"].Value.ToString() != "0")
						{ linkYongChe = "http://car.bitauto.com/" + allSpell.Trim().ToLower() + "/yongche/"; }
						if (xnl[0].Attributes["hangqing"].Value.ToString() != "0")
						{ linkHangQing = "http://car.bitauto.com/" + allSpell.Trim().ToLower() + "/hangqing/"; }
					}
					catch (Exception ex)
					{ }
				}
			}
			sb.Append("<Link Name=\"体验试驾\" URL=\"" + linkShiJia + "\" />");
			sb.Append("<Link Name=\"用车\" URL=\"" + linkYongChe + "\" />");
			sb.Append("<Link Name=\"新闻\" URL=\"" + linkXinwen + "\" />");
			sb.Append("<Link Name=\"行情\" URL=\"" + linkHangQing + "\" />");
			sb.Append("<Link Name=\"导购\" URL=\"" + linkDaoGou + "\" />");
		}
	}
}