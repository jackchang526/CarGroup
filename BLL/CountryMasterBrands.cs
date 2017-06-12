using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BitAuto.CarChannel.BLL
{
	/// <summary>
	/// 某国所有的主品牌
	/// </summary>
	public class CountryMasterBrands:List<XmlElement>
	{
		private string m_label;							//国家标签
		private string m_countryName;					//国家显示名称
		private static Dictionary<string, string> m_labelInfo;	//国家对应的名称

		/// <summary>
		/// 国家标签
		/// </summary>
		public string Label
		{
			get { return m_label; }
		}
		/// <summary>
		/// 国家显示名称
		/// </summary>
		public string CountryName
		{
			get { return m_countryName; }
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="countryName"></param>
		public CountryMasterBrands(string label)
		{
			m_label = label;
			m_countryName = m_labelInfo[label];
		}

		/// <summary>
		/// 静态构造
		/// </summary>
		static CountryMasterBrands()
		{
			m_labelInfo = new Dictionary<string, string>();
			m_labelInfo["zz"] = "自主";
			m_labelInfo["dx"] = "德国";
			m_labelInfo["rx"] = "日本";
			m_labelInfo["mx"] = "美国";
			m_labelInfo["hx"] = "韩国";
			m_labelInfo["fx"] = "法国";
			m_labelInfo["yx"] = "英国";
			m_labelInfo["yx2"] = "意大利";
			m_labelInfo["other"] = "其他";
		}

		/// <summary>
		/// 获取国家名称的标签
		/// </summary>
		/// <param name="countryName"></param>
		/// <returns></returns>
		public static string GetCountryLabel(string countryName)
		{
			string label = "other";
			switch(countryName)
			{
				case "中国":
					label = "zz";
					break;
				case "德国":
					label = "dx";
					break;
				case "日本":
					label = "rx";
					break;
				case "美国":
					label = "mx";
					break;
				case "韩国":
					label = "hx";
					break;
				case "法国":
					label = "fx";
					break;
				case "英国":
					label = "yx";
					break;
				case "意大利":
					label = "yx2";
					break;
				default:
					label = "other";
					break;
			}
			return label;
		}
	}
}
