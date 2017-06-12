using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Linq;

using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace BitAuto.CarChannel.Common
{
	public class NodeCompare
	{
		#region 新版 综述页 车型列表 排序 add by sk 2013.08.05
		/// <summary>
		/// 按照排量有小到大排序，排量相同的按自然吸气的在前，涡轮增压在后；进气方式相同的按最大功率由小到大排序
		/// 先按年款由新到旧排序，同年款的按变速器排序（手动，半自动，自动，手自一体，无级变速，双离合），同变速箱的按指导价由低到高显示
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		public static int CompareCarByExhaustAndPowerAndInhaleType(CarInfoForSerialSummaryEntity car1, CarInfoForSerialSummaryEntity car2)
		{
			double exhaust1 = 0;
			if (!double.TryParse(car1.Engine_Exhaust.TrimEnd('L'), out exhaust1))
			{
				exhaust1 = 9999;
			}

			double exhaust2 = 0;
			if (!double.TryParse(car2.Engine_Exhaust.TrimEnd('L'), out exhaust2))
			{
				exhaust2 = 9999;
			}

			//int result = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
			int result = 0;
			if (exhaust1 > exhaust2)
				result = 1;
			else if (exhaust1 < exhaust2)
				result = -1;
			if (result == 0)
			{
				result = CompareInhaleType(car1.Engine_InhaleType, car2.Engine_InhaleType);
				if (result == 0)
				{
					result = CompareMaxPower(car1.Engine_MaxPower, car2.Engine_MaxPower);
					if (result == 0)
					{
						result = CompareCarByYear(car1, car2);
					}
				}
			}
			if (result == 0)
			{
				car2.CarID.CompareTo(car1.CarID);
			}
			return result;
		}
		/// <summary>
		/// 进气方式 排序
		/// </summary>
		/// <param name="inhaleType1"></param>
		/// <param name="inhaleType2"></param>
		/// <returns></returns>
		public static int CompareInhaleType(string inhaleType1, string inhaleType2)
		{
			if (inhaleType1.IndexOf("自然吸气") > -1)
				inhaleType1 = "a";
			else
				inhaleType1 = "b";

			if (inhaleType2.IndexOf("自然吸气") > -1)
				inhaleType2 = "a";
			else
				inhaleType2 = "b";

			return String.Compare(inhaleType1, inhaleType2);
		}
		/// <summary>
		/// 马力排序
		/// </summary>
		/// <param name="inhaleType1"></param>
		/// <param name="inhaleType2"></param>
		/// <returns></returns>
		public static int CompareMaxPower(int maxPower1, int maxPower2)
		{
			int reuslt = 0;
			if (maxPower1 > maxPower2)
				reuslt = 1;
			else if (maxPower2 > maxPower1)
				reuslt = -1;
			return reuslt;
		}
		/// <summary>
		/// 排序顺序 年款\排量\变速器\指导价
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		public static int CompareCarByYear(CarInfoForSerialSummaryEntity car1, CarInfoForSerialSummaryEntity car2)
		{
			int ret = 0;
			double year1 = ConvertHelper.GetDouble(car1.CarYear);
			double year2 = ConvertHelper.GetDouble(car2.CarYear);
			if (year1 > year2)
				ret = -1;
			else if (year1 < year2)
				ret = 1;
			else
			{
				ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
				if (ret == 0)
				{
					ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
					if (ret == 0)
					{
						double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
						double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
						if (price1 > price2)
							ret = 1;
						else if (price2 > price1)
							ret = -1;
					}
				}
			}

			return ret;
		}
		#endregion
		/// <summary>
		/// 实现品牌先按进口与国产排序，后按拼音排序
		/// </summary>
		/// <param name="ele1">第一个节点</param>
		/// <param name="ele2">第二个节点</param>
		/// <returns>比较结果</returns>
		public static int CompareBrandNode(XmlElement ele1, XmlElement ele2)
		{
			string country1 = ele1.GetAttribute("Country");
			string country2 = ele2.GetAttribute("Country");
			int ret = String.Compare(country1, country2);
			ret *= -1; //逆序
			if (ret == 0)
			{
				string spell1 = ele1.GetAttribute("Spell");
				string spell2 = ele2.GetAttribute("Spell");
				ret = String.Compare(spell1, spell2);
			}
			return ret;
		}

		/// <summary>
		/// 实现品牌先国产后进口与排序，再按拼音排序
		/// </summary>
		/// <param name="ele1">第一个节点</param>
		/// <param name="ele2">第二个节点</param>
		/// <returns>比较结果</returns>
		public static int CompareBrandNodeSelfFirst(XmlElement ele1, XmlElement ele2)
		{
			string country1 = ele1.GetAttribute("Country");
			string country2 = ele2.GetAttribute("Country");
			int ret = String.Compare(country1, country2);
			if (ret == 0)
			{
				string spell1 = ele1.GetAttribute("Spell");
				string spell2 = ele2.GetAttribute("Spell");
				ret = String.Compare(spell1, spell2);
			}
			return ret;
		}

		/// <summary>
		/// 对子品牌按spell排序
		/// </summary>
		/// <param name="ele1"></param>
		/// <param name="ele2"></param>
		/// <returns></returns>
		public static int CompareSerialBySpellAsc(XmlElement ele1, XmlElement ele2)
		{
			string spell1 = ele1.GetAttribute("Spell");
			string spell2 = ele2.GetAttribute("Spell");
			return String.Compare(spell1, spell2);
		}

		/// <summary>
		/// 对子品牌按关注度进行排序
		/// </summary>
		/// <param name="ele1"></param>
		/// <param name="ele2"></param>
		/// <returns></returns>
		public static int CompareSerialByPvDesc(XmlElement ele1, XmlElement ele2)
		{
			int ret = 0;
			int pv1 = Convert.ToInt32(ele1.GetAttribute("CsPV"));
			int pv2 = Convert.ToInt32(ele2.GetAttribute("CsPV"));
			if (pv1 > pv2)
				ret = -1;
			else if (pv1 < pv2)
				ret = 1;

			return ret;
		}

		/// <summary>
		/// 按车型排量排序
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		[Obsolete("Do not call this method.")]
		public static int CompareCarByExhaust(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
		{
			int ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
			if (ret == 0)
			{
				double year1 = ConvertHelper.GetDouble(car1.CarYear);
				double year2 = ConvertHelper.GetDouble(car2.CarYear);
				if (year1 > year2)
					ret = -1;
				else if (year1 < year2)
					ret = 1;
				else
				{
					ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
					if (ret == 0)
					{
						double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
						double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
						if (price1 > price2)
							ret = 1;
						else if (price2 > price1)
							ret = -1;
					}

				}
			}
			return ret;
		}
		/// <summary>
		/// 排序顺序 年款\销售状态\排量\变速器\指导价
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		public static int CompareCarByYearAndSale(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
		{
			int result = 0;
			int sale1 = 0;
			int sale2 = 0;
			if (car1.SaleState == "停销")
				sale1 = 1;

			if (car2.SaleState == "停销")
			{
				sale2 = 1;
			}

			double year1 = ConvertHelper.GetDouble(car1.CarYear);
			double year2 = ConvertHelper.GetDouble(car2.CarYear);
			if (year1 > year2)
				result = -1;
			else if (year1 < year2)
				result = 1;
			else
			{
				result = sale1 - sale2;
				if (result == 0)
				{
					result = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
					if (result == 0)
					{
						result = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
						if (result == 0)
						{
							double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
							double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
							if (price1 > price2)
								result = 1;
							else if (price2 > price1)
								result = -1;
						}
					}
				}
			}
 			return result;
		}
		/// <summary>
		/// 排序顺序 年款\排量\变速器\指导价
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		public static int CompareCarByYear(EnumCollection.CarInfoForSerialSummary car1, EnumCollection.CarInfoForSerialSummary car2)
		{
			int ret = 0;
			double year1 = ConvertHelper.GetDouble(car1.CarYear);
			double year2 = ConvertHelper.GetDouble(car2.CarYear);
			if (year1 > year2)
				ret = -1;
			else if (year1 < year2)
				ret = 1;
			else
			{
				ret = String.Compare(car1.Engine_Exhaust, car2.Engine_Exhaust);
				if (ret == 0)
				{
					ret = CompareTransmissionType(car1.TransmissionType, car2.TransmissionType);
					if (ret == 0)
					{
						double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
						double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
						if (price1 > price2)
							ret = 1;
						else if (price2 > price1)
							ret = -1;
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// 区域车型页报价列表排序
		/// </summary>
		/// <param name="row1"></param>
		/// <param name="row2"></param>
		/// <returns></returns>
		public static int CompareRegionPrice(DataRow row1, DataRow row2)
		{
			int ret = 0;
			int year1 = ConvertHelper.GetInteger(row1["Car_YearType"]);
			int year2 = ConvertHelper.GetInteger(row2["Car_YearType"]);
			if (year1 > year2)
				ret = -1;
			else if (year1 < year2)
				ret = 1;
			else
			{
				double exhaust1 = 0.0;
				double exhaust2 = 0.0;
				Double.TryParse(row1["Engine_Exhaust"].ToString().Trim().Replace("L", ""), out exhaust1);
				Double.TryParse(row2["Engine_Exhaust"].ToString().Trim().Replace("L", ""), out exhaust2);
				if (exhaust1 > exhaust2)
					ret = 1;
				else if (exhaust1 < exhaust2)
					ret = -1;
				else
				{
					string trans1 = row1["UnderPan_TransmissionType"].ToString();
					string trans2 = row2["UnderPan_TransmissionType"].ToString();
					ret = CompareTransmissionType(trans1, trans2);
					if (ret == 0)
					{
						double price1 = ConvertHelper.GetDouble(row1["car_ReferPrice"]);
						double price2 = ConvertHelper.GetDouble(row2["car_ReferPrice"]);
						if (price1 > price2)
							ret = 1;
						else if (price2 > price1)
							ret = -1;
					}
				}
			}
			return ret;
		}


		/// <summary>
		/// 比较变速器类型
		/// </summary>
		/// <param name="trans1"></param>
		/// <param name="trans2"></param>
		/// <returns></returns>
		public static int CompareTransmissionType(string trans1, string trans2)
		{
			if (trans1.IndexOf("手动") > -1)
				trans1 = "a";
			else if (trans1.IndexOf("半自动") > -1)
				trans1 = "b";
			else if (trans1.IndexOf("自动") > -1)
				trans1 = "c";
			else if (trans1.IndexOf("手自一体") > -1)
				trans1 = "d";
			else if (trans1.IndexOf("CVT") > -1)
				trans1 = "e";
			else trans1 = "f";



			if (trans2.IndexOf("手动") > -1)
				trans2 = "a";
			else if (trans2.IndexOf("半自动") > -1)
				trans2 = "b";
			else if (trans2.IndexOf("自动") > -1)
				trans2 = "c";
			else if (trans2.IndexOf("手自一体") > -1)
				trans2 = "d";
			else if (trans2.IndexOf("CVT") > -1)
				trans2 = "e";
			else
				trans2 = "f";

			return String.Compare(trans1, trans2);
		}

		/// <summary>
		/// 倒序排字符串
		/// </summary>
		/// <param name="str1"></param>
		/// <param name="str2"></param>
		/// <returns></returns>
		public static int CompareStringDesc(string str1, string str2)
		{
			return String.Compare(str1, str2) * -1;
		}
		/// <summary>
		/// 树形品牌排序规则
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int TreeBrandCompare(XmlElement xelem1, XmlElement xelem2)
		{
			if (xelem1.GetAttribute("Country") != xelem2.GetAttribute("Country")
				&& xelem1.GetAttribute("") == "国产")
			{
				return -1;
			}
			else if (xelem1.GetAttribute("Country") != xelem2.GetAttribute("Country")
				&& xelem2.GetAttribute("") == "国产")
			{
				return 1;
			}
			else if (xelem1.GetAttribute("Country").CompareTo(xelem2.GetAttribute("Country")) < 0)
			{
				return -1;
			}
			return 1;
		}
		/// <summary>
		/// 树形子品牌排序规则
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int TreeSerialCompare(XmlElement xelem1, XmlElement xelem2)
		{
			string xelem1Sale = xelem1.GetAttribute("CsSaleState");
			string xelem2Sale = xelem2.GetAttribute("CsSaleState");
			if (xelem1Sale == xelem2Sale && xelem1.GetAttribute("Spell").CompareTo(xelem2.GetAttribute("Spell")) < 0)
			{
				return -1;
			}
			else if (xelem1Sale == xelem2Sale)
			{
				return 1;
			}
			else if (xelem1Sale == "在销")
			{
				return -1;
			}
			else if (xelem1Sale == "待销" && xelem2Sale == "停销")
			{
				return -1;
			}
			else if (xelem1Sale == "待销" && xelem2Sale == "在销")
			{
				return 1;
			}
			else if (xelem1Sale == "停销")
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 树形子品牌排序
		/// </summary>
		/// <param name="cspe1"></param>
		/// <param name="cspe2"></param>
		/// <returns></returns>
		public static int TreeSerialCompare(CarSerialPhotoEntity cspe1, CarSerialPhotoEntity cspe2)
		{
			string xelem1Sale = cspe1.SaleState;
			string xelem2Sale = cspe2.SaleState;
			if (xelem1Sale == xelem2Sale && cspe1.Cs_spell.CompareTo(cspe2.Cs_spell) < 0)
			{
				return -1;
			}
			else if (xelem1Sale == xelem2Sale)
			{
				return 1;
			}
			else if (xelem1Sale == "在销")
			{
				return -1;
			}
			else if (xelem1Sale == "待销" && xelem2Sale == "停销")
			{
				return -1;
			}
			else if (xelem1Sale == "待销" && xelem2Sale == "在销")
			{
				return 1;
			}
			else if (xelem1Sale == "停销")
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 子品牌答疑发布时间排序
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int SerialAskPublishTimeCompare(XmlElement xelem1, XmlElement xelem2)
		{
			string firstPublishTime = xelem1.GetAttribute("lasttime");
			string secondPublisTime = xelem2.GetAttribute("lasttime");

			if (string.IsNullOrEmpty(firstPublishTime)) return 1;
			if (string.IsNullOrEmpty(secondPublisTime)) return 1;

			DateTime firstDt = ConvertHelper.GetDateTime(firstPublishTime);
			DateTime secondDt = ConvertHelper.GetDateTime(secondPublisTime);

			int result = firstDt.CompareTo(secondDt);
			if (result > 0)
			{
				return -1;
			}
			else if (result == 0)
			{
				return 0;
			}
			else
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 子品牌答疑数量排序
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int SerialAskNumberCompare(XmlElement xelem1, XmlElement xelem2)
		{
			int firstNumber = ConvertHelper.GetInteger(xelem1.GetAttribute("countnum"));
			int secondeNumber = ConvertHelper.GetInteger(xelem2.GetAttribute("countnum"));
			if (firstNumber > secondeNumber)
			{
				return -1;
			}
			else if (firstNumber == secondeNumber)
			{
				return 0;
			}
			else
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 子品牌图片更新时间排序
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int SerialImagePublishTimeCompare(XmlElement xelem1, XmlElement xelem2)
		{
			string firstPublishTime = xelem1.GetAttribute("T");
			string secondPublisTime = xelem2.GetAttribute("T");

			if (string.IsNullOrEmpty(firstPublishTime)) return 1;
			if (string.IsNullOrEmpty(secondPublisTime)) return 1;

			DateTime firstDt = ConvertHelper.GetDateTime(firstPublishTime);
			DateTime secondDt = ConvertHelper.GetDateTime(secondPublisTime);

			int result = firstDt.CompareTo(secondDt);
			if (result > 0)
			{
				return -1;
			}
			else if (result == 0)
			{
				return 0;
			}
			else
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 子品牌图片PV排序
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int SerialImagePvCompare(XmlElement xelem1, XmlElement xelem2)
		{
			int firstNumber = ConvertHelper.GetInteger(xelem1.GetAttribute("CsPV"));
			int secondeNumber = ConvertHelper.GetInteger(xelem2.GetAttribute("CsPV"));
			if (firstNumber > secondeNumber)
			{
				return -1;
			}
			else if (firstNumber == secondeNumber)
			{
				return 0;
			}
			else
			{
				return 1;
			}
			return 1;
		}
		/// <summary>
		/// 比对省与省之间的顺序
		/// </summary>
		/// <param name="xelem1"></param>
		/// <param name="xelem2"></param>
		/// <returns></returns>
		public static int CompareProvinceOrder(XmlElement xelem1, XmlElement xelem2)
		{
			int newsNumber1 = ConvertHelper.GetInteger(xelem1.GetAttribute("hangqing"));
			int newsNumber2 = ConvertHelper.GetInteger(xelem2.GetAttribute("hangqing"));
			if (newsNumber1 > newsNumber2)
			{
				return -1;
			}
			else if (newsNumber1 == newsNumber2)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}
		/// <summary>
		/// 子品牌颜色 排序
		/// 有图在前 无图在后 年款 从大到小  色值从大到小
		/// </summary>
		/// <param name="color1"></param>
		/// <param name="color2"></param>
		/// <returns></returns>
		public static int SerialColorCompare(SerialColorForSummaryEntity color1, SerialColorForSummaryEntity color2)
		{
			int result = EmptyCompare(color1.ImageUrl, color2.ImageUrl);
			if (result == 0)
			{
				result = color2.YearType - color1.YearType;
				if (result == 0)
				{
					result = string.Compare(color1.ColorRGB, color2.ColorRGB);
				}
			}
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int EmptyCompare(string s1, string s2)
		{
			int result = 0;
			if ((string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
						|| (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2)))
			{
				result = 0;
			}
			if (string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2))
				result = 1;
			if (!string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
				result = -1;
			return result;
		}
		/// <summary>
		/// 排量排序
		/// </summary>
		/// <param name="exhaust1"></param>
		/// <param name="exhaust2"></param>
		/// <returns></returns>
		public static int ExhaustCompare(string s1, string s2)
		{
			double exhaust1 = 0;
			double exhaust2 = 0;
			double.TryParse(s1.TrimEnd('L'), out exhaust1);
			double.TryParse(s2.TrimEnd('L'), out exhaust2);
			int result = 0;
			if (exhaust1 > exhaust2)
				result = 1;
			else if (exhaust1 < exhaust2)
				result = -1;
			return result;
		}
		/// <summary>
		/// 排量排序 包括 带T 
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int ExhaustCompareNew(string s1, string s2)
		{
			double exhaust1 = 0;
			double exhaust2 = 0;
			double.TryParse(s1.TrimEnd('L', 'T'), out exhaust1);
			double.TryParse(s2.TrimEnd('L', 'T'), out exhaust2);
			int result = 0;
			if (exhaust1 > exhaust2)
				result = 1;
			else if (exhaust1 < exhaust2)
				result = -1;
			else
			{
				result = string.Compare(s1, s2);
			}
			return result;
		}
	}
}
