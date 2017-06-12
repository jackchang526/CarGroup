using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class SelectListForCompare : PageBase
	{
		private int type = 1;    // 类型 (1：图片对比，2：评测对比)
		private int level = 1;    // 级别 (1：主品牌级别，2：子品牌级别，3：取子品牌的热门其他子品牌)
		private int masterID = 0;
		// add by chengl Jan.20.2012
		private int topHot = 6;
		private List<int> listCurrentCsID = new List<int>();
		private int adSerialId = 0;//广告子品牌Id

		private Hashtable htSerial = new Hashtable();
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			//if (!this.IsPostBack)
			//{
			//	this.GetPageParam();
			//	if (level >= 1 && level <= 2)
			//	{
			//		// 取图片对比、评测对比的下拉列表
			//		this.GetSerialHash();
			//	}
			//	else if (level == 3)
			//	{
			//		// 取某个子品牌的热门子品牌
			//		GetHotSerialByCsID();
			//	}
			//	else
			//	{ }
			//	Response.Write(sb.ToString());
			//}
		}

		//// 取参数
		//private void GetPageParam()
		//{
		//	// 图片对比或者评测对比
		//	if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
		//	{
		//		string strType = this.Request.QueryString["type"].ToString();
		//		if (int.TryParse(strType, out type))
		//		{
		//			if (type < 1 || type > 2)
		//			{
		//				type = 1;
		//			}
		//		}
		//	}
		//	// 级联级别
		//	if (this.Request.QueryString["level"] != null && this.Request.QueryString["level"].ToString() != "")
		//	{
		//		string strLevel = this.Request.QueryString["level"].ToString();
		//		if (int.TryParse(strLevel, out level))
		//		{
		//			if (level < 1 || level > 3)
		//			{
		//				level = 1;
		//			}
		//		}
		//	}
		//	// 主品牌ID
		//	if (this.Request.QueryString["masterID"] != null && this.Request.QueryString["masterID"].ToString() != "")
		//	{
		//		string strMasterID = this.Request.QueryString["masterID"].ToString();
		//		if (int.TryParse(strMasterID, out masterID))
		//		{
		//		}
		//	}

		//	// 子品牌ID
		//	if (this.Request.QueryString["serialIDs"] != null && this.Request.QueryString["serialIDs"].ToString() != "")
		//	{
		//		string strSerialID = this.Request.QueryString["serialIDs"].ToString();
		//		string[] arrayCsID = strSerialID.Split(',');
		//		if (arrayCsID.Length > 0)
		//		{
		//			foreach (string csidStr in arrayCsID)
		//			{
		//				int csid = 0;
		//				if (int.TryParse(csidStr.Trim(), out csid))
		//				{
		//					if (csid > 0 && !listCurrentCsID.Contains(csid))
		//					{ listCurrentCsID.Add(csid); }
		//				}
		//			}
		//		}
		//	}

		//	// 热门子品牌的条数
		//	if (this.Request.QueryString["topHot"] != null && this.Request.QueryString["topHot"].ToString() != "")
		//	{
		//		string strTopHot = this.Request.QueryString["topHot"].ToString();
		//		if (int.TryParse(strTopHot, out topHot))
		//		{
		//		}
		//	}
		//	if (topHot <= 0 || topHot > 10)
		//	{ topHot = 6; }

		//	//广告子品牌Id
		//	adSerialId = ConvertHelper.GetInteger(Request.QueryString["adSerialId"]);
		//}

		//// 取子品牌集合
		//private void GetSerialHash()
		//{
		//	DataSet ds = base.GetAllSerialbyMasterForCompareList();
		//	if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//	{
		//		CommonService cs = new CommonService();
		//		// 图片对比
		//		if (type == 1)
		//		{
		//			htSerial = cs.GetPhotoCompareSerialList();
		//			// 主品牌
		//			if (level == 1)
		//			{
		//				sb.Append("<option value=\"-1\">选择品牌</option>");
		//				int lastBsID = 0;
		//				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		//				{
		//					if (htSerial.ContainsKey(ds.Tables[0].Rows[i]["cs_id"].ToString()))
		//					{
		//						if (lastBsID != int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString()))
		//						{
		//							sb.Append("<option value=\"" + ds.Tables[0].Rows[i]["bs_id"].ToString() + "\">" + ds.Tables[0].Rows[i]["bsname"].ToString().Trim() + "</option>");
		//							lastBsID = int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString());
		//						}
		//					}
		//				}
		//			}
		//			// 子品牌
		//			if (level == 2)
		//			{
		//				sb.Append("<option value=\"-1\">选择车型</option>");
		//				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		//				{
		//					if (masterID == int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString()))
		//					{
		//						if (htSerial.ContainsKey(ds.Tables[0].Rows[i]["cs_id"].ToString()))
		//						{
		//							sb.Append("<option value=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\">" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "</option>");
		//						}
		//					}
		//				}
		//			}
		//		}
		//		// 评测对比
		//		if (type == 2)
		//		{
		//			htSerial = cs.GetPingCeCompareSerialList();
		//			// 主品牌
		//			if (level == 1)
		//			{
		//				sb.Append("<option value=\"-1\">选择品牌</option>");
		//				int lastBsID = 0;
		//				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		//				{
		//					if (htSerial.ContainsKey(ds.Tables[0].Rows[i]["cs_id"].ToString()))
		//					{
		//						if (lastBsID != int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString()))
		//						{
		//							sb.Append("<option value=\"" + ds.Tables[0].Rows[i]["bs_id"].ToString() + "\">" + ds.Tables[0].Rows[i]["bsname"].ToString().Trim() + "</option>");
		//							lastBsID = int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString());
		//						}
		//					}
		//				}
		//			}
		//			// 子品牌
		//			if (level == 2)
		//			{
		//				sb.Append("<option value=\"-1\">选择车型</option>");
		//				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		//				{
		//					if (masterID == int.Parse(ds.Tables[0].Rows[i]["bs_id"].ToString()))
		//					{
		//						if (htSerial.ContainsKey(ds.Tables[0].Rows[i]["cs_id"].ToString()))
		//						{
		//							sb.Append("<option value=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\">" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "</option>");
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

		///// <summary>
		///// 根据子品牌ID取子品牌热门还关注的其他子品牌 并且满足图片对比
		///// </summary>
		//private void GetHotSerialByCsID()
		//{
		//	if (listCurrentCsID.Count > 0 && listCurrentCsID[0] > 0)
		//	{
		//		List<string> listHotOther = new List<string>();
		//		CommonService cs = new CommonService();
		//		htSerial = cs.GetPhotoCompareSerialList();
		//		List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(listCurrentCsID[0], 10);
		//		//广告
		//		if (adSerialId > 0)
		//			GetAdSerial(listHotOther);
		//		int loop = 0;
		//		foreach (EnumCollection.SerialToSerial sts in lsts)
		//		{
		//			if (listCurrentCsID.Contains(sts.ToCsID) || sts.ToCsID == adSerialId)
		//			{ continue; }
		//			if (htSerial.ContainsKey(sts.ToCsID.ToString()))
		//			{
		//				// 此子品牌满足图片对比
		//				listHotOther.Add("<a class=\"hasCar\" target=\"_self\" href=\"javascript:selectThisSerial(" + sts.ToCsID.ToString() + ",'" + sts.ToCsShowName + "',-1);\">");
		//				listHotOther.Add("<img alt=\"\" src=\"" + sts.ToCsPic.ToString() + "\">");
		//				listHotOther.Add("<strong>" + sts.ToCsShowName + "</strong>");
		//				listHotOther.Add("<div style=\"display: none;\" class=\"ico_hover_add\"></div>");
		//				listHotOther.Add("</a>");
		//				loop++;
		//				if (loop >= topHot - (adSerialId > 0 ? 1 : 0))
		//				{ break; }
		//			}
		//		}
		//		sb.Append(String.Concat(listHotOther.ToArray()));
		//	}
		//}

		//private void GetAdSerial(List<string> listHotOther)
		//{
		//	SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, adSerialId);
		//	string serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(adSerialId).Replace("_2.", "_5.");
		//	// 此子品牌满足图片对比
		//	listHotOther.Add("<a class=\"hasCar\" target=\"_self\" href=\"javascript:selectThisSerial(" + adSerialId + ",'" + serialEntity.ShowName + "',-1);\">");
		//	listHotOther.Add("<img alt=\"\" src=\"" + serialWhiteImageUrl + "\">");
		//	listHotOther.Add("<strong>" + serialEntity.ShowName + "</strong>");
		//	listHotOther.Add("<div style=\"display: none;\" class=\"ico_hover_add\"></div>");
		//	listHotOther.Add("</a>");
		//}

	}
}