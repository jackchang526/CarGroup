using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using System.Xml;
using System.Text;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using System.Linq;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarUtils.Define;

namespace WirelessWeb
{
	public partial class _Default : WirelessPageBase
	{
		private Car_SerialBll serial;

		protected string HotCarHtml = string.Empty;
		protected string NewCarHtml = string.Empty;
        protected int _serialId; 

		public _Default()
		{
			serial = new Car_SerialBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
            _serialId = ConvertHelper.GetInteger(Request.QueryString["ID"]);
			MakeHotCarHtml();
			MakeNewCarHtml();
		}
		//上市新车
		private void MakeNewCarHtml()
		{
			StringBuilder sbCarList = new StringBuilder();
			Dictionary<int, string> dict = serial.GetAllSerialMarkDay();
			var newcarList = dict.ToList()
				.Where(p => CommonFunction.DateDiff("d", ConvertHelper.GetDateTime(p.Value), DateTime.Now) >= 0);
			sbCarList.Append("<ul>");
			int loop = 0;
			foreach (KeyValuePair<int, string> kv in newcarList)
			{
				int serialId = kv.Key;
				string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
				if (imgUrl == WebConfig.DefaultCarPic) continue;
				loop++;
				if (loop > 6) break;
				Car_SerialEntity cs = serial.Get_Car_SerialByCsID(serialId);
				string serialUrl = "/" + cs.Cs_AllSpell + "/";
				string priceRange = base.GetSerialPriceRangeByID(Convert.ToInt32(serialId));
				if (priceRange.Trim().Length == 0)
					priceRange = "暂无报价";
				sbCarList.Append("<li>");
				sbCarList.AppendFormat("<a href=\"{0}\"><img src=\"{1}\" /></a>", serialUrl, imgUrl);
				sbCarList.AppendFormat("<a href=\"{0}\" class=\"block\">{1}</a>", serialUrl, cs.Cs_ShowName);
				sbCarList.AppendFormat("<p>{0}</p>", priceRange);
				sbCarList.Append("</li>");
			}
			sbCarList.Append("</ul>");
			NewCarHtml = sbCarList.ToString();
		}
		//热门车型
		private void MakeHotCarHtml()
		{
			List<XmlElement> serialList = serial.GetHotSerial(6);
			StringBuilder sbCarList = new StringBuilder();
			sbCarList.Append("<ul>");
			foreach (XmlElement serialNode in serialList)
			{
				int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
				string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
				string serialName = "";
				serialName = serialNode.GetAttribute("ShowName");
				string serialLevel = serialNode.GetAttribute("CsLevel");
				string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
				//EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), serialLevel);
				string serialUrl = "/" + serialSpell + "/";
				//string levelUrl = "/" + ((EnumCollection.SerialLevelSpellEnum)levelEnum).ToString() + "/";
				string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(serialLevel));

				string priceRange = base.GetSerialPriceRangeByID(Convert.ToInt32(serialId));
				if (priceRange.Trim().Length == 0)
					priceRange = "暂无报价";
				sbCarList.Append("<li>");
				sbCarList.AppendFormat("<a href=\"{0}\"><img src=\"{1}\" /></a>", serialUrl, imgUrl);
				sbCarList.AppendFormat("<a href=\"{0}\" class=\"block\">{1}</a>", serialUrl, serialName);
				sbCarList.AppendFormat("<p>{0}</p>", priceRange);
				sbCarList.Append("</li>");
			}
			sbCarList.AppendLine("</ul>");
			HotCarHtml = sbCarList.ToString();
		}
	}
}