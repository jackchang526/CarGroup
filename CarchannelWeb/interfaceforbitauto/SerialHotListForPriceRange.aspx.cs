using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 价格区间热门子品牌(杨立锋)
	/// </summary>
	public partial class SerialHotListForPriceRange : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<HotList>");
				GetAllSerialHotListByPriceRange();
				sb.Append("</HotList>");
				Response.Write(sb.ToString());
			}
		}

		private void GetAllSerialHotListByPriceRange()
		{
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			for (int i = 0; i <= 8; i++)
			{
				XmlNodeList serialNodeList;
				sb.Append("<PriceRange Range=\"" + i.ToString() + "\">");
				if (i == 0)
				{
					serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
				}
				else
				{
					//遍历所有子品牌节点
					serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[contains(@MultiPriceRange,\"," + i + ",\")]");
				}

				TopPVSerialSelector tpvSelector = new TopPVSerialSelector(10);
				tpvSelector.SelectNewCar = true;
				foreach (XmlElement serialNode in serialNodeList)
				{
					tpvSelector.AddSerial(serialNode);
				}
				List<XmlElement> listCsPriceRange = tpvSelector.GetTopSerialList();
				//图片Url
				Dictionary<int, XmlElement> urlDic = AutoStorageService.GetImageUrlDic();
				foreach (XmlElement serialNode in listCsPriceRange)
				{
					int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));

					string imgUrl = "";
					if (urlDic.ContainsKey(serialId))
					{
						int imgId = Convert.ToInt32(urlDic[serialId].GetAttribute("ImageId"));
						imgUrl = urlDic[serialId].GetAttribute("ImageUrl");
						if (imgId == 0 || imgUrl == "")
							imgUrl = WebConfig.DefaultCarPic;
						else
							imgUrl = new OldPageBase().GetPublishImage(2, imgUrl, imgId);
					}
					else
						imgUrl = WebConfig.DefaultCarPic;

					string serialName = serialNode.GetAttribute("Name");
					string serialShowName = serialNode.GetAttribute("ShowName");
					string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
					string serialLevel = serialNode.GetAttribute("CsLevel");
					string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));


					sb.Append("<Serial ID=\"" + serialId.ToString() + "\" ");
					sb.Append("Name=\"" + serialShowName.Trim() + "\" ");
					sb.Append("Img=\"" + imgUrl + "\" ");
					if (priceRange.Trim().Length == 0)
						sb.Append("PriceRange=\"暂无报价\" ");
					else
						sb.Append("PriceRange=\"" + priceRange + "\" ");
					sb.Append("Level=\"" + serialLevel + "\" />");
				}
				sb.Append("</PriceRange>");
			}
		}
	}
}