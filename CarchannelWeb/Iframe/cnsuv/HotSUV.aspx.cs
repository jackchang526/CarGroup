using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Iframe.cnsuv
{
	public partial class HotSUV : PageBase
	{
		int i = 0;
		protected string showSerialSUV = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			//页面缓存
			base.SetPageCache(30);
			InitPage();
		}
		private void InitPage()
		{
			StringBuilder sb = new StringBuilder();
			try
			{
				XmlDocument mbDoc = AutoStorageService.GetAutoXml();
				XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[@CsLevel=\"SUV\"]");
				List<XmlElement> serialList = new List<XmlElement>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					serialList.Add(serialNode);
				}
				serialList.Sort(NodeCompare.CompareSerialByPvDesc);
				foreach (XmlElement serialNode in serialList)
				{
					i++;
					if (i > 10) break;
					int sId = Convert.ToInt32(serialNode.GetAttribute("ID"));
					string sName = serialNode.GetAttribute("ShowName");
					string allSpell = serialNode.GetAttribute("AllSpell");
					string realUrl = Car_SerialBll.GetSerialCoverHashImgUrl(sId);
					string serialUrl = "http://car.bitauto.com/" + allSpell + "/";
					string serialPhotoUrl = "http://car.bitauto.com/" + allSpell + "/tupian/";
					string serialBaoJiaUrl = "http://car.bitauto.com/" + allSpell + "/baojia/";
					string serialTaocheUrl = "http://yiche.taoche.com/similarcar/serial/" + allSpell + "/paesf201bxc/?from=bitauto";
					sb.Append("<li>");
					sb.AppendFormat("    <a href=\"{2}\" target=\"_blank\"><img src=\"{0}\" alt=\"{1}\" title=\"{1}\"/></a>",
						realUrl,
						sName,
						serialUrl
						);
					sb.AppendFormat("    <p class=\"name\"><a href=\"{0}\" class=\"name\" target=\"_blank\">{1}</a></p>",
						serialUrl,
						sName
						);
					sb.AppendFormat("    <p class=\"list_mxl\"><a href=\"{0}\" target=\"_blank\">图片</a>|<a href=\"{1}\" target=\"_blank\">报价</a>|<a href=\"{2}\" target=\"_blank\">二手车</a></p>",
						serialPhotoUrl,
						serialBaoJiaUrl,
						serialTaocheUrl
						);
					sb.Append("</li>");
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.Message + ex.StackTrace);
			}
			showSerialSUV = sb.ToString();
		}
	}
}