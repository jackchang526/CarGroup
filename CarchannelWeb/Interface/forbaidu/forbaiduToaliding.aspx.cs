using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.forbaidu
{
	public partial class forbaiduToaliding : InterfacePageBase
	{
		//内容字符串
		protected string _ContentString = string.Empty;
		//配置文件地址
		private string _ConfigFileAddress = "~/Interface/forbaidu/forbaiduToaliding.xml";
		private Dictionary<string, int> _SerialIdList = new Dictionary<string, int>();
		private EnumCollection.SerialInfoCard sic;	//子品牌名片
		private Car_SerialEntity cse;				//子品牌信息
		private Car_SerialBll csb = new Car_SerialBll();//子品牌业务类 
		private CommonFunction cf = new CommonFunction();

		protected void Page_Load(object sender, EventArgs e)
		{
			//得到显示要用的子品牌列表
			GetIsShowSerialList();
			//打印XML信息
			PrintXmlContent();
		}
		/// <summary>
		/// 得到显示的子品牌列表
		/// </summary>
		private void GetIsShowSerialList()
		{
			//配置文件地址
			string filePath = Server.MapPath(_ConfigFileAddress);

			if (!File.Exists(filePath)) return;

			XmlDocument xmlDoc = new XmlDocument();

			try
			{
				xmlDoc.Load(filePath);

				if (xmlDoc == null) return;

				XmlNodeList xNodeList = xmlDoc.SelectNodes("root/add");

				if (xNodeList == null || xNodeList.Count < 1) return;

				foreach (XmlElement entity in xNodeList)
				{
					string name = entity.GetAttribute("name");
					int id = ConvertHelper.GetInteger(entity.GetAttribute("id"));
					if (_SerialIdList.ContainsKey(name)) continue;

					_SerialIdList.Add(name, id);
				}

			}
			catch
			{

			}
		}
		/// <summary>
		/// 打印XML内容
		/// </summary>
		private void PrintXmlContent()
		{
			if (_SerialIdList == null || _SerialIdList.Count < 1) return;

			StringBuilder _xmlContent = new StringBuilder();
			_xmlContent.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			_xmlContent.Append("<urlset>");
			//循环赋值
			int index = 1;
			foreach (KeyValuePair<string, int> entity in _SerialIdList)
			{
				int id = entity.Value;
				//子品牌信息
				sic = csb.GetSerialInfoCard(id);
				if (sic.CsID < 1) continue;
				cse = csb.GetSerialInfoEntity(id);
				if (cse == null || cse.Cs_Id < 1) continue;
				string serialPrice = sic.CsPriceRange;
				if (serialPrice.Length == 0)
					serialPrice = "暂无报价";
				string imgUrl = "";
				if (index < 10)
				{
					imgUrl = string.Format("http://image.bitautoimg.com/autoalbum/common/images/carimg/car_{0}.jpg", "0" + index);
				}
				else
				{
					imgUrl = string.Format("http://image.bitautoimg.com/autoalbum/common/images/carimg/car_{0}.jpg", index);
				}
				index++;
				_xmlContent.Append("<url>");
				_xmlContent.AppendFormat("<loc>http://car.bitauto.com/{0}/?from=ald&amp;WT.srch=1</loc>", cse.Cs_AllSpell.ToLower());
				_xmlContent.AppendFormat("<lastmod>{0}</lastmod>", DateTime.Now.ToString("yyyy-MM-dd"));
				_xmlContent.Append("<changefreq>always</changefreq>");
				_xmlContent.Append("<priority>1.0</priority> ");
				_xmlContent.Append("<data>");
				_xmlContent.Append("<display>");
				_xmlContent.AppendFormat("<name>{0}</name>", cse.MasterName);
				_xmlContent.AppendFormat("<alias>{0}</alias>", System.Security.SecurityElement.Escape(cse.Cp_ShortName));
				_xmlContent.AppendFormat("<type>{0}</type>", entity.Key);
				_xmlContent.AppendFormat("<price>{0}</price>", serialPrice);
				_xmlContent.AppendFormat("<sprice>{0}</sprice>", GetPriceValue(id));
				_xmlContent.AppendFormat("<advantages>{0}</advantages>", cf.ToXml(cse.Cs_Virtues));
				_xmlContent.AppendFormat("<disadvantages>{0}</disadvantages>", cf.ToXml(cse.Cs_Defect));
				_xmlContent.AppendFormat("<image>{0}</image>", imgUrl);
				_xmlContent.Append("<source>易车网</source> ");
				_xmlContent.Append("</display>");
				_xmlContent.Append("</data>");
				_xmlContent.Append("</url>");
			}

			_xmlContent.Append("</urlset>");

			_ContentString = _xmlContent.ToString();
		}
		/// <summary>
		/// 得到报价值
		/// </summary>
		private string GetPriceValue(int id)
		{
			List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(id);
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
			{
				double referPrice = 0.0;
				bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
				if (isDouble)
				{
					if (referPrice > maxPrice)
						maxPrice = referPrice;
					if (referPrice < minPrice)
						minPrice = referPrice;
				}
			}


			if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
				return "暂无";
			else
			{
				return minPrice + "万-" + maxPrice + "万";
			}
		}
	}
}