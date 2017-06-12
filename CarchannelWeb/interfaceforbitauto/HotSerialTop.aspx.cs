using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 首页热门车型数据(互动产品 熊玉辉)
	/// </summary>
	public partial class HotSerialTop : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private string dept = "";
		private string TuJieXML = "http://imgsvr.bitauto.com/autochannel/SerialImageService.aspx?dataname=serialaccountbygroup&groupid=12";
		private XmlDocument xmlTuJie = new XmlDocument();
		private TreeData treeData = null;
		private Dictionary<int, XmlElement> urlDic = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();

				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				GetHotSerialTop();
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().Trim().ToLower();
			}
		}

		private void GetHotSerialTop()
		{
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();
			XmlNodeList serialList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
			List<XmlElement> serialNodeList = new List<XmlElement>();
			TopPVSerialSelector tpvSelector = new TopPVSerialSelector(100);
			foreach (XmlElement serialNode in serialList)
			{
				tpvSelector.AddSerial(serialNode);
			}

			urlDic = CarSerialImgUrlService.GetImageUrlDicNew();


			if (dept == "bitautodaogou")
			{
				// 导购
				treeData = new TreeFactory().GetTreeDataObject("daogou");
				//IsSpecialForContent(tpvSelector);
				//return;
			}
			else if (dept == "bitautopingce")
			{
				// 评测
				treeData = new TreeFactory().GetTreeDataObject("pingce");
			}
			else if (dept == "bitautohangqing")
			{
				// 行情
				treeData = new TreeFactory().GetTreeDataObject("hangqing");
			}
			else if (dept == "bitautoanquan")
			{
				// 安全
				treeData = new TreeFactory().GetTreeDataObject("anquan");
			}
			else if (dept == "bitautokeji")
			{
				// 科技
				treeData = new TreeFactory().GetTreeDataObject("keji");
			}
			else if (dept == "bitautobaoyang")
			{
				// 保养
				treeData = new TreeFactory().GetTreeDataObject("baoyang");
			}
			else if (dept == "bitautotujie")
			{
				// 图解
				treeData = new TreeFactory().GetTreeDataObject("tujie");
				try
				{
					xmlTuJie.Load(TuJieXML);
				}
				catch
				{ }
			}
			else
			{ }


			int loop = 1;
			foreach (XmlElement serialNode in tpvSelector.GetTopSerialList())
			{

				if (loop > 10)
				{ break; }
				int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
				if (dept == "bitautotujie")
				{
					XmlNodeList xnl = xmlTuJie.SelectNodes("/Root/Serial[@ID='" + serialId + "']");
					if (xnl != null && xnl.Count > 0)
					{ }
					else
					{ continue; }
				}
				string imgUrl = "";
				if (urlDic.ContainsKey(serialId))
				{
					// modified by chengl Jan.4.2010
					if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
					{
						// 有新封面
						imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), "2");
					}
					else
					{
						// 没有新封面
						if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
						{
							imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), "2");
						}
						else
						{
							imgUrl = WebConfig.DefaultCarPic;
						}
					}
				}
				else
					imgUrl = WebConfig.DefaultCarPic;

				string serialName = "";
				serialName = serialNode.GetAttribute("ShowName");

				string serialLevel = serialNode.GetAttribute("CsLevel");
				string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();

				if (dept == "bitautodaogou" || dept == "bitautopingce" || dept == "bitautohangqing" || dept == "bitautotujie" || dept == "bitautoanquan" || dept == "bitautokeji" || dept == "bitautobaoyang")
				{
					// 当新闻数量不够时跳过
					int carNewsNum = treeData.GetSerialId(serialId);
					if (carNewsNum < 1)
					{ continue; }
				}

				sb.AppendLine("<Serial ID=\"" + serialId.ToString() + "\" ");
				sb.AppendLine(" ShowName=\"" + serialName.ToString().Trim() + "\" ");
				sb.AppendLine(" CsLevel=\"" + serialLevel.ToString().Trim() + "\" ");
				sb.AppendLine(" AllSpell=\"" + serialSpell.ToString().Trim() + "\" ");
				// 导购、评测、行情、图解 安全 科技 用150图
				if (dept == "bitautodaogou" || dept == "bitautopingce" || dept == "bitautohangqing" || dept == "bitautotujie" || dept == "bitautoanquan" || dept == "bitautokeji" || dept == "bitautobaoyang")
				{
					sb.AppendLine(" Pic=\"" + imgUrl.ToString().Trim().Replace("_2.", "_1.") + "\" ");
				}
				else
				{
					sb.AppendLine(" Pic=\"" + imgUrl.ToString().Trim() + "\" ");
				}
				if (dept == "bitautodaogou")
				{
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" DaoGou=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautopingce")
				{
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" PingCe=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautohangqing")
				{
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" HangQing=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautoanquan")
				{
					// 安全
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" AnQuan=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautokeji")
				{
					// 科技
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" KeJi=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautobaoyang")
				{
					// 保养
					int carNewsNum = treeData.GetSerialId(serialId);
					sb.AppendLine(" BaoYang=\"" + carNewsNum.ToString().Trim() + "\" />");
				}
				else if (dept == "bitautotujie")
				{
					// int carNewsNum = treeData.GetSerialId(serialId);
					XmlNodeList xnl = xmlTuJie.SelectNodes("/Root/Serial[@ID='" + serialId + "']");
					if (xnl != null && xnl.Count > 0)
					{
						sb.AppendLine(" TuJie=\"" + xnl[0].Attributes["TuJie"].Value.ToString().Trim() + "\" />");
					}
					else
					{ continue; }
				}
				else
				{
					string priceRange = base.GetSerialPriceRangeByID(Convert.ToInt32(serialId));
					sb.AppendLine(" Price=\"" + priceRange.ToString().Trim() + "\" />");
				}
				loop++;
			}
		}

		/// <summary>
		/// 是否数据做特殊处理(导购接口)
		/// Serial ID="2415"
		/// ShowName="莲花L3"
		/// Pic="http://gimg.bitauto.com/ResourceFiles/0/0/33/20101027111656996.jpg"
		/// </summary>
		private void IsSpecialForContent(TopPVSerialSelector tpv)
		{
			sb.AppendLine("<Serial ID=\"2415\" ");
			sb.AppendLine(" ShowName=\"莲花L3\" ");
			sb.AppendLine(" CsLevel=\"紧凑型\" ");
			sb.AppendLine(" AllSpell=\"lianhual3\" ");
			sb.AppendLine(" Pic=\"http://gimg.bitauto.com/ResourceFiles/0/0/33/20101027111656996.jpg\" ");
			int carNewsNum = treeData.GetSerialId(2415);
			sb.AppendLine(" DaoGou=\"" + carNewsNum.ToString().Trim() + "\" />");

			int loop = 1;
			foreach (XmlElement serialNode in tpv.GetTopSerialList())
			{
				if (loop > 9)
				{ break; }
				int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
				if (serialId == 2415)
				{ continue; }
				string imgUrl = "";
				if (urlDic.ContainsKey(serialId))
				{
					// modified by chengl Jan.4.2010
					if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
					{
						// 有新封面
						imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), "2");
					}
					else
					{
						// 没有新封面
						if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
						{
							imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), "2");
						}
						else
						{
							imgUrl = WebConfig.DefaultCarPic;
						}
					}
				}
				else
					imgUrl = WebConfig.DefaultCarPic;

				string serialName = "";
				serialName = serialNode.GetAttribute("ShowName");

				string serialLevel = serialNode.GetAttribute("CsLevel");
				string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();

				sb.AppendLine("<Serial ID=\"" + serialId.ToString() + "\" ");
				sb.AppendLine(" ShowName=\"" + serialName.ToString().Trim() + "\" ");
				sb.AppendLine(" CsLevel=\"" + serialLevel.ToString().Trim() + "\" ");
				sb.AppendLine(" AllSpell=\"" + serialSpell.ToString().Trim() + "\" ");
				sb.AppendLine(" Pic=\"" + imgUrl.ToString().Trim().Replace("_2.", "_1.") + "\" ");
				sb.AppendLine(" DaoGou=\"" + treeData.GetSerialId(serialId) + "\" />");
				loop++;
			}
		}
	}
}