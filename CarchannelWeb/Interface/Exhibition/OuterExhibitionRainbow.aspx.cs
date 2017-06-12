using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BCM = BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class OuterExhibitionRainbow : System.Web.UI.Page
	{
		private BitAuto.CarChannel.BLL.Exhibition _ExhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private XmlDocument _ConfigXmlDocument = new XmlDocument();
		private int _ExhibitionId = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			if (!ValidatorParam())
			{
				return;
			}
			PrintfElement();
		}
		/// <summary>
		/// 得到请求参数
		/// </summary>
		private void GetParam()
		{
			_ExhibitionId = string.IsNullOrEmpty(Request.QueryString["ID"])
						? 0 : ConvertHelper.GetInteger(Request.QueryString["ID"].ToString());
		}
		/// <summary>
		/// 验证参数是否有效
		/// </summary>
		private bool ValidatorParam()
		{
			if (_ExhibitionId < 1)
			{
				PrinfErrorResult();
				return false;
			}

			_ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionId);

			if (_ExhibitionXmlDoc == null)
			{
				PrinfErrorResult();
				return false;
			}
			_ConfigXmlDocument.Load(Path.Combine(WebConfig.DataBlockPath, @"Data\Exhibition\Rainbow\outer\RainbowConfig.xml"));

			if (_ConfigXmlDocument == null)
			{
				PrinfErrorResult();
				return false;
			}
			return true;
		}
		/// <summary>
		/// 打印错误结果
		/// </summary>
		private void PrinfErrorResult()
		{
			Response.Write("");
			Response.End();
		}
		/// <summary>
		/// 打印元素
		/// </summary>
		private void PrintfElement()
		{
			XmlNodeList xNodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand/Serial[@IsRB='1']");

			if (xNodeList == null || xNodeList.Count < 1)
			{
				PrinfErrorResult();
				return;
			}

			List<int> serialIdList = new List<int>();
			List<XmlElement> xElemList = new List<XmlElement>();

			foreach (XmlElement xElem in xNodeList)
			{
				serialIdList.Add(ConvertHelper.GetInteger(xElem.GetAttribute("ID")));
				xElemList.Add(xElem);
			}
			//子品牌排序
			xElemList.Sort(BitAuto.CarChannel.BLL.Exhibition.OuterOrderXMLElement);
			//得到彩虹条项
			Dictionary<int, Dictionary<int, BCM.ExhibitionRainbow>> rainbowList = _ExhibitionBLL.GetExhibitionRainbowBySerialIDList(serialIdList, _ExhibitionId);

			StringBuilder xmlContent = new StringBuilder("");
			xmlContent.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
			xmlContent.AppendFormat("<RainbowRoot Time=\"{0}\">", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
			xmlContent.AppendFormat("<Serials Type=\"{0}\"/>", "国产车");
			xmlContent.AppendFormat("<Serials Type=\"{0}\">", "进口车");

			foreach (XmlElement xElem in xElemList)
			{
				BCM.Car_SerialEntity csbEntity = new Car_SerialBll().GetSerialInfoEntity(ConvertHelper.GetInteger(xElem.GetAttribute("ID")));
				if (csbEntity.Cs_Id < 1)
				{
					continue;
				}
				xmlContent.AppendFormat("<Serial ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" AllSpell=\"{3}\">"
										, csbEntity.Cs_Id.ToString()
										, csbEntity.Cs_Name.ToString()
										, csbEntity.Cs_ShowName
										, csbEntity.Cs_AllSpell);
				xmlContent.Append(PrintRainbowElement(rainbowList, csbEntity.Cs_Id));
				xmlContent.Append("</Serial>");
			}

			xmlContent.Append("</Serials>");
			xmlContent.Append("</RainbowRoot>");

			Response.Write(xmlContent.ToString());
			Response.End();
		}
		/// <summary>
		/// 打印彩虹条元素
		/// </summary>
		/// <param name="rainbowList"></param>
		/// <returns></returns>
		private string PrintRainbowElement(Dictionary<int, Dictionary<int, BCM.ExhibitionRainbow>> rainbowList, int serailId)
		{

			XmlNodeList xNodeList = _ConfigXmlDocument.SelectNodes("root/Exhibition[@Id='" + _ExhibitionId.ToString() + "']/Item");
			if (xNodeList == null || xNodeList.Count < 1)
			{
				xNodeList = _ConfigXmlDocument.SelectNodes("root/Exhibition[@Id='0']/Item");
			}

			StringBuilder rainbowContent = new StringBuilder();
			foreach (XmlElement xElem in xNodeList)
			{
				string id = xElem.GetAttribute("ShowId");
				string name = xElem.GetAttribute("Name");
				BCM.ExhibitionRainbow rainbow = new BCM.ExhibitionRainbow();
				if (rainbowList != null
					&& rainbowList.ContainsKey(serailId)
					&& rainbowList[serailId].ContainsKey(ConvertHelper.GetInteger(xElem.GetAttribute("Id"))))
				{
					rainbow = rainbowList[serailId][ConvertHelper.GetInteger(xElem.GetAttribute("Id"))];
				}

				string url = rainbow == null ? "" : rainbow.Url;
				string happenTime = (rainbow == null || string.IsNullOrEmpty(rainbow.Url)) ? "" : rainbow.HappenTime.ToString("yyyy-MM-dd hh:mm:ss");

				rainbowContent.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\" URL=\"{2}\" Time=\"{3}\" />"
											, id, name, url, happenTime);
			}

			return rainbowContent.ToString();
		}
	}
}