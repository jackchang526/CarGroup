using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 易车网子品牌排行(按价格，按级别) 朱永旭
	/// </summary>
	public partial class SerialSortByLevelOrPrice : InterfacePageBase
	{
		private int type = 1; // (1:按价格,2:按级别,3:全部子品牌前5,4:全部子品牌按级别(刘荣伟),5:全级别前10排行(李传松),6:按价格区间前10排行增加价格区间属性(李传松))
		protected string temp = string.Empty;
		private StringBuilder sb = new StringBuilder();
		private int cityID = 0;      // 0:全国
		private string levelDataTop10 = "SerialCityPVLevelTop10.xml";
		private string priceRangeDataTop10 = "SerialCityPVPriceRangeTop10.xml";

		protected void Page_Load(object sender, EventArgs e)
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Root>");
			if (!this.IsPostBack)
			{
				this.CheckParam();
				this.GetSerialSortDataByType();
			}
			sb.Append("</Root>");
			Response.Write(sb.ToString());
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
			{
				string typestr = this.Request.QueryString["type"].ToString();
				if (int.TryParse(typestr, out type))
				{
					if (type < 1 || type > 6)
					{
						type = 1;
					}
				}
			}

			if (this.Request.QueryString["cityID"] != null && this.Request.QueryString["cityID"].ToString() != "")
			{
				string cityIDstr = this.Request.QueryString["cityID"].ToString();
				if (int.TryParse(cityIDstr, out cityID))
				{
					if (cityID < 1)
					{
						cityID = 0;
					}
				}
			}
		}

		private void GetSerialSortDataByType()
		{
			if (type == 3)
			{
				// 取全部子品牌前5
				GenerateAllSerialTop5();
			}
			else if (type == 4)
			{
				// 取全部子品牌的各级别及部分级别排行
				GenerateAllSerialSortForLevelAndNoLevel();
			}
			else
			{
				if (cityID > 0)
				{
					// 取特定城市
					GetTop10ByCityID();
				}
				else
				{
					// 取全国
					GetAllCityFor();
				}
			}
		}

		/// <summary>
		/// 取全国排行
		/// </summary>
		private void GetAllCityFor()
		{
			/*
			 select t1.csID as cs_ID,t1.sumCount as Pv_SumNum ,cs.cs_CarLevel from ( 
	   select csID,sum(uvcount) as sumCount 
	   from dbo.StatisticSerialPVUVCity 
	   group by csid ) t1 
	   left join AutoCarChannel.dbo.car_serial cs on t1.csID=cs.cs_id 
	   order by t1.sumCount desc
			 */
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			#region 按价格 type == 1
			// 按价格
			if (type == 1)
			{
				List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList(); // base.GetAllSerialNewly7DayToList();
				List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
				for (int i = 0; i < 8; i++)
				{
					List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
					arrSSfi[i] = ll1;
				}
				foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
				{
					if (ssfi.CsPriceRange.IndexOf(",1,") >= 0 && arrSSfi[0].Count < 10)
					{ arrSSfi[0].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",2,") >= 0 && arrSSfi[1].Count < 10)
					{ arrSSfi[1].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",3,") >= 0 && arrSSfi[2].Count < 10)
					{ arrSSfi[2].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",4,") >= 0 && arrSSfi[3].Count < 10)
					{ arrSSfi[3].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",5,") >= 0 && arrSSfi[4].Count < 10)
					{ arrSSfi[4].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",6,") >= 0 && arrSSfi[5].Count < 10)
					{ arrSSfi[5].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",7,") >= 0 && arrSSfi[6].Count < 10)
					{ arrSSfi[6].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",8,") >= 0 && arrSSfi[7].Count < 10)
					{ arrSSfi[7].Add(ssfi); }
				}
				for (int i = 0; i < arrSSfi.Length; i++)
				{
					sb.Append("<Group Name=\"" + GetPriceRangeName(i) + "\" >");
					if (arrSSfi[i].Count > 0)
					{
						foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
						{
							sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
							sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
							sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
							sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
							sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
						}
					}
					sb.Append("</Group>");
				}
				// temp = CSBillboardListService.GetCSBillboardListHTML_Price(false);
			}
			#endregion

			#region 按级别 type == 2
			// 按级别
			if (type == 2)
			{
				List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();// GetAllSerialNewly7DayToList();
				List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[9];
				for (int i = 0; i < 9; i++)
				{
					List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
					arrSSfi[i] = ll1;
				}
				foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
				{
					if (ssfi.CsLevel == "微型车" && arrSSfi[0].Count < 10)
					{ arrSSfi[0].Add(ssfi); }
					if (ssfi.CsLevel == "小型车" && arrSSfi[1].Count < 10)
					{ arrSSfi[1].Add(ssfi); }
					if (ssfi.CsLevel == "紧凑型车" && arrSSfi[2].Count < 10)
					{ arrSSfi[2].Add(ssfi); }
					if (ssfi.CsLevel == "中型车" && arrSSfi[3].Count < 10)
					{ arrSSfi[3].Add(ssfi); }
					if (ssfi.CsLevel == "中大型车" && arrSSfi[4].Count < 10)
					{ arrSSfi[4].Add(ssfi); }
					if (ssfi.CsLevel == "豪华车" && arrSSfi[5].Count < 10)
					{ arrSSfi[5].Add(ssfi); }

					if (ssfi.CsLevel == "MPV" && arrSSfi[6].Count < 10)
					{ arrSSfi[6].Add(ssfi); }
					if (ssfi.CsLevel == "SUV" && arrSSfi[7].Count < 10)
					{ arrSSfi[7].Add(ssfi); }

					if (ssfi.CsLevel == "跑车" && arrSSfi[8].Count < 10)
					{ arrSSfi[8].Add(ssfi); }
				}
				for (int i = 0; i < arrSSfi.Length; i++)
				{
					sb.Append("<Group Name=\"" + GetLevelName(i) + "\" >");
					if (arrSSfi[i].Count > 0)
					{
						foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
						{
							sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
							sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
							sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
							sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
							sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
						}
					}
					sb.Append("</Group>");
				}
				// temp = CSBillboardListService.GetCSBillboardListHTML_Level(true);
			}
			#endregion

			#region 按全级别级别 type == 5
			// 按全级别级别
			if (type == 5)
			{
				List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();// GetAllSerialNewly7DayToList();
				List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[12];
				for (int i = 0; i < 12; i++)
				{
					List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
					arrSSfi[i] = ll1;
				}
				foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
				{
					if (ssfi.CsLevel == "微型车" && arrSSfi[0].Count < 10)
					{ arrSSfi[0].Add(ssfi); }
					if (ssfi.CsLevel == "小型车" && arrSSfi[1].Count < 10)
					{ arrSSfi[1].Add(ssfi); }
					if (ssfi.CsLevel == "紧凑型车" && arrSSfi[2].Count < 10)
					{ arrSSfi[2].Add(ssfi); }
					if (ssfi.CsLevel == "中型车" && arrSSfi[3].Count < 10)
					{ arrSSfi[3].Add(ssfi); }
					if (ssfi.CsLevel == "中大型车" && arrSSfi[4].Count < 10)
					{ arrSSfi[4].Add(ssfi); }
					if (ssfi.CsLevel == "豪华车" && arrSSfi[5].Count < 10)
					{ arrSSfi[5].Add(ssfi); }

					if (ssfi.CsLevel == "MPV" && arrSSfi[6].Count < 10)
					{ arrSSfi[6].Add(ssfi); }
					if (ssfi.CsLevel == "SUV" && arrSSfi[7].Count < 10)
					{ arrSSfi[7].Add(ssfi); }

					if (ssfi.CsLevel == "跑车" && arrSSfi[8].Count < 10)
					{ arrSSfi[8].Add(ssfi); }
					if (ssfi.CsLevel == "面包车" && arrSSfi[9].Count < 10)
					{ arrSSfi[9].Add(ssfi); }
					if (ssfi.CsLevel == "皮卡" && arrSSfi[10].Count < 10)
					{ arrSSfi[10].Add(ssfi); }
					if (ssfi.CsLevel == "其它" && arrSSfi[11].Count < 10)
					{ arrSSfi[11].Add(ssfi); }
				}
				for (int i = 0; i < arrSSfi.Length; i++)
				{
					sb.Append("<Group Name=\"" + GetLevelName(i) + "\" >");
					if (arrSSfi[i].Count > 0)
					{
						foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
						{
							sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
							sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
							sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
							sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
							string priceRange = base.GetSerialPriceRangeByID(ss.CsID);
							sb.Append(" CsPriceRange=\"" + priceRange + "\" ");
							sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
						}
					}
					sb.Append("</Group>");
				}
			}
			#endregion

			#region 按价格 type == 6 增加了报价
			if (type == 6)
			{
				List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList(); // base.GetAllSerialNewly7DayToList();
				List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[8];
				for (int i = 0; i < 8; i++)
				{
					List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
					arrSSfi[i] = ll1;
				}
				foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
				{
					if (ssfi.CsPriceRange.IndexOf(",1,") >= 0 && arrSSfi[0].Count < 10)
					{ arrSSfi[0].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",2,") >= 0 && arrSSfi[1].Count < 10)
					{ arrSSfi[1].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",3,") >= 0 && arrSSfi[2].Count < 10)
					{ arrSSfi[2].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",4,") >= 0 && arrSSfi[3].Count < 10)
					{ arrSSfi[3].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",5,") >= 0 && arrSSfi[4].Count < 10)
					{ arrSSfi[4].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",6,") >= 0 && arrSSfi[5].Count < 10)
					{ arrSSfi[5].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",7,") >= 0 && arrSSfi[6].Count < 10)
					{ arrSSfi[6].Add(ssfi); }
					if (ssfi.CsPriceRange.IndexOf(",8,") >= 0 && arrSSfi[7].Count < 10)
					{ arrSSfi[7].Add(ssfi); }
				}
				for (int i = 0; i < arrSSfi.Length; i++)
				{
					sb.Append("<Group Name=\"" + GetPriceRangeName(i) + "\" >");
					if (arrSSfi[i].Count > 0)
					{
						foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
						{
							sb.Append("<Serial ID=\"" + ss.CsID.ToString() + "\" ");
							sb.Append(" CsName=\"" + ss.CsName.ToString() + "\" ");
							sb.Append(" CsShowName=\"" + ss.CsShowName.ToString() + "\" ");
							sb.Append(" CsAllSpell=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsAll=\"" + ss.CsAllSpell.ToString() + "\" ");
							sb.Append(" CsLevel=\"" + ss.CsLevel.ToString() + "\" ");
							string priceRange = base.GetSerialPriceRangeByID(ss.CsID);
							sb.Append(" CsPriceRange=\"" + priceRange + "\" ");
							sb.Append(" CsPV=\"" + ss.CsPV.ToString() + "\" />");
						}
					}
					sb.Append("</Group>");
				}
			}
			#endregion

		}

		private void GetTop10ByCityID()
		{
			XmlDocument doc = new XmlDocument();
			// 按价格
			if (type == 1)
			{
				string cacheName = "interfaceforbitauto_SerialSortByLevelOrPrice_priceRangeDataTop10";
				string dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\" + priceRangeDataTop10);
				if (HttpContext.Current.Cache[cacheName] != null)
				{
					doc = (XmlDocument)HttpContext.Current.Cache[cacheName];
				}
				else
				{
					doc.Load(dataPath);
					System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(dataPath);
					Cache.Insert(cacheName, doc, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
				}
				GenerateCityPricerangeSort(doc);
			}
			// 按级别
			if (type == 2)
			{
				string cacheName = "interfaceforbitauto_SerialSortByLevelOrPrice_levelDataTop10";
				string dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\" + levelDataTop10);
				if (HttpContext.Current.Cache[cacheName] != null)
				{
					doc = (XmlDocument)HttpContext.Current.Cache[cacheName];
				}
				else
				{
					doc.Load(dataPath);
					System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(dataPath);
					Cache.Insert(cacheName, doc, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
				}
				GenerateCityLevelSort(doc);
			}

		}

		/// <summary>
		/// 生成特定城市级别排行
		/// </summary>
		/// <param name="doc"></param>
		private void GenerateCityLevelSort(XmlDocument doc)
		{
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/SerialSort/City[@ID='" + cityID.ToString() + "']");

				if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
				{
					GenerateLevelDataFormXmlNode(xnl, "微型车");
					GenerateLevelDataFormXmlNode(xnl, "小型车");
					GenerateLevelDataFormXmlNode(xnl, "紧凑型车");
					GenerateLevelDataFormXmlNode(xnl, "中型车");
					GenerateLevelDataFormXmlNode(xnl, "中大型车");
					GenerateLevelDataFormXmlNode(xnl, "豪华车");
					GenerateLevelDataFormXmlNode(xnl, "SUV");
					GenerateLevelDataFormXmlNode(xnl, "MPV");
					GenerateLevelDataFormXmlNode(xnl, "跑车");
				}
				else
				{
					// 如果此城市取不到数据 则去北京的数据
					xnl = doc.SelectNodes("/SerialSort/City[@ID='201']");
					if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
					{
						GenerateLevelDataFormXmlNode(xnl, "微型车");
						GenerateLevelDataFormXmlNode(xnl, "小型车");
						GenerateLevelDataFormXmlNode(xnl, "紧凑型车");
						GenerateLevelDataFormXmlNode(xnl, "中型车");
						GenerateLevelDataFormXmlNode(xnl, "中大型车");
						GenerateLevelDataFormXmlNode(xnl, "豪华车");
						GenerateLevelDataFormXmlNode(xnl, "SUV");
						GenerateLevelDataFormXmlNode(xnl, "MPV");
						GenerateLevelDataFormXmlNode(xnl, "跑车");
					}
				}
			}
		}

		/// <summary>
		/// 生成特定城市价格区间排行
		/// </summary>
		/// <param name="doc"></param>
		private void GenerateCityPricerangeSort(XmlDocument doc)
		{
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnl = doc.SelectNodes("/SerialSort/City[@ID='" + cityID.ToString() + "']");

				if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
				{
					GeneratePriceRangeDataFormXmlNode(xnl, "0-5", "5万以内");
					GeneratePriceRangeDataFormXmlNode(xnl, "5-8", "5-8万");
					GeneratePriceRangeDataFormXmlNode(xnl, "8-12", "8-12万");
					GeneratePriceRangeDataFormXmlNode(xnl, "12-18", "12-18万");
					GeneratePriceRangeDataFormXmlNode(xnl, "18-25", "18-25万");
					GeneratePriceRangeDataFormXmlNode(xnl, "25-40", "25-40万");
					GeneratePriceRangeDataFormXmlNode(xnl, "40-80", "40-80万");
					GeneratePriceRangeDataFormXmlNode(xnl, "80-", "80万以上");
				}
				else
				{
					// 如果此城市取不到数据 则去北京的数据
					xnl = doc.SelectNodes("/SerialSort/City[@ID='201']");
					if (xnl != null && xnl.Count > 0 && xnl[0].ChildNodes.Count > 0)
					{
						GeneratePriceRangeDataFormXmlNode(xnl, "0-5", "5万以内");
						GeneratePriceRangeDataFormXmlNode(xnl, "5-8", "5-8万");
						GeneratePriceRangeDataFormXmlNode(xnl, "8-12", "8-12万");
						GeneratePriceRangeDataFormXmlNode(xnl, "12-18", "12-18万");
						GeneratePriceRangeDataFormXmlNode(xnl, "18-25", "18-25万");
						GeneratePriceRangeDataFormXmlNode(xnl, "25-40", "25-40万");
						GeneratePriceRangeDataFormXmlNode(xnl, "40-80", "40-80万");
						GeneratePriceRangeDataFormXmlNode(xnl, "80-", "80万以上");
					}
				}
			}
		}

		private void GenerateLevelDataFormXmlNode(XmlNodeList xnl, string levelName)
		{
			sb.Append("<Group Name=\"" + levelName + "\">");
			XmlNodeList xnlLevel = xnl[0].SelectNodes("Level[@Name='" + levelName + "']");
			if (xnlLevel != null && xnlLevel.Count > 0 && xnlLevel[0].ChildNodes.Count > 0)
			{
				foreach (XmlNode xn in xnlLevel[0])
				{
					sb.Append("<Serial ID=\"" + xn.Attributes["ID"].Value.Trim() + "\" ");
					sb.Append(" CsName=\"" + xn.Attributes["CsName"].Value.Trim() + "\" ");
					sb.Append(" CsShowName=\"" + xn.Attributes["CsShowName"].Value.Trim() + "\" ");
					sb.Append(" CsAllSpell=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
					sb.Append(" CsAll=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
					sb.Append(" CsLevel=\"" + xn.Attributes["CsLevel"].Value.Trim() + "\" ");
					sb.Append(" CsPV=\"" + xn.Attributes["CsPV"].Value.Trim() + "\" />");
				}
			}
			sb.Append("</Group>");
		}

		private void GeneratePriceRangeDataFormXmlNode(XmlNodeList xnl, string priceName, string priceShowName)
		{
			sb.Append("<Group Name=\"" + priceShowName + "\">");
			XmlNodeList xnlPrice = xnl[0].SelectNodes("Seria_disPriceNew[@Name='" + priceName + "']");
			if (xnlPrice != null && xnlPrice.Count > 0 && xnlPrice[0].ChildNodes.Count > 0)
			{
				foreach (XmlNode xn in xnlPrice[0])
				{
					sb.Append("<Serial ID=\"" + xn.Attributes["ID"].Value.Trim() + "\" ");
					sb.Append(" CsName=\"" + xn.Attributes["CsName"].Value.Trim() + "\" ");
					sb.Append(" CsShowName=\"" + xn.Attributes["CsShowName"].Value.Trim() + "\" ");
					sb.Append(" CsAllSpell=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
					sb.Append(" CsAll=\"" + xn.Attributes["CsAllSpell"].Value.Trim().ToLower() + "\" ");
					sb.Append(" CsLevel=\"" + xn.Attributes["CsLevel"].Value.Trim() + "\" ");
					sb.Append(" CsPV=\"" + xn.Attributes["CsPV"].Value.Trim() + "\" />");
				}
			}
			sb.Append("</Group>");
		}

		private string GetPriceRangeName(int index)
		{
			string name = "";
			switch (index)
			{
				case 0: name = "5万以内"; break;
				case 1: name = "5-8万"; break;
				case 2: name = "8-12万"; break;
				case 3: name = "12-18万"; break;
				case 4: name = "18-25万"; break;
				case 5: name = "25-40万"; break;
				case 6: name = "40-80万"; break;
				case 7: name = "80万以上"; break;
				default: name = ""; break;
			}
			return name;
		}

		private string GetLevelName(int index)
		{
			string name = "";
			switch (index)
			{
				case 0: name = "微型车"; break;
				case 1: name = "小型车"; break;
				case 2: name = "紧凑型车"; break;
				case 3: name = "中型车"; break;
				case 4: name = "中大型车"; break;
				case 5: name = "豪华车"; break;
				case 6: name = "MPV"; break;
				case 7: name = "SUV"; break;
				case 8: name = "跑车"; break;
				case 9: name = "面包车"; break;
				case 10: name = "皮卡"; break;
				case 11: name = "其它"; break;
				default: name = ""; break;
			}
			return name;
		}

		private int GetLevelId(string levelName)
		{
			//if (levelName == "紧凑型车")
			//    levelName = "紧凑型";
			//else if (levelName == "中大型车")
			//    levelName = "中大型";

			//EnumCollection.SerialLevelEnum level = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), levelName);
			return CarLevelDefine.GetLevelIdByName(levelName);
		}

		private void GenerateAllSerialTop5()
		{
			DataSet ds = new DataSet();
			string sql = " select top 5 t1.*,cs.cs_name,cs.cs_showname,cs.allSpell ";
			sql += " from ( ";
			sql += " select csID,sum(uvcount)as uv from dbo.StatisticSerialPVUVCity ";
			sql += " group by csID )t1 ";
			sql += " left join dbo.car_serial cs on t1.csid=cs.cs_id ";
			sql += " order by uv desc ";
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["csID"].ToString().Trim() + "\" ");
					sb.Append(" CsName=\"" + ds.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "\" ");
					sb.Append(" ShowName=\"" + ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "\" ");
					sb.Append(" CsUV=\"" + ds.Tables[0].Rows[i]["uv"].ToString().Trim() + "\" ");
					sb.Append(" CsAllSpell=\"" + ds.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower() + "\" />");
				}
			}

		}

		private void GenerateAllSerialSortForLevelAndNoLevel()
		{
			StringBuilder sbAlllevel = new StringBuilder();
			sbAlllevel.Append("<AllLevel><PVRank SerialList=\"");
			// StringBuilder sbLevel = new StringBuilder();
			List<EnumCollection.SerialSortForInterface> lssfi = base.GetAllSerialNewly30DayToList();
			List<EnumCollection.SerialSortForInterface>[] arrSSfi = new List<EnumCollection.SerialSortForInterface>[10];
			for (int i = 0; i < 10; i++)
			{
				List<EnumCollection.SerialSortForInterface> ll1 = new List<EnumCollection.SerialSortForInterface>();
				arrSSfi[i] = ll1;
			}
			int loop = 0;
			foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
			{
				if (loop == 0)
				{
					sbAlllevel.Append(ssfi.CsID.ToString());
				}
				else
				{
					sbAlllevel.Append("," + ssfi.CsID.ToString());
				}
				loop++;

				if (ssfi.CsLevel == "微型车")
				{ arrSSfi[0].Add(ssfi); }
				if (ssfi.CsLevel == "小型车")
				{ arrSSfi[1].Add(ssfi); }
				if (ssfi.CsLevel == "紧凑型车")
				{ arrSSfi[2].Add(ssfi); }
				if (ssfi.CsLevel == "中型车")
				{ arrSSfi[3].Add(ssfi); }
				if (ssfi.CsLevel == "中大型车")
				{ arrSSfi[4].Add(ssfi); }
				if (ssfi.CsLevel == "豪华车")
				{ arrSSfi[5].Add(ssfi); }

				if (ssfi.CsLevel == "MPV")
				{ arrSSfi[6].Add(ssfi); }
				if (ssfi.CsLevel == "SUV")
				{ arrSSfi[7].Add(ssfi); }

				if (ssfi.CsLevel == "跑车")
				{ arrSSfi[8].Add(ssfi); }
				if (ssfi.CsLevel == "面包车")
				{ arrSSfi[9].Add(ssfi); }
			}
			sbAlllevel.Append("\" /></AllLevel>");

			for (int i = 0; i < arrSSfi.Length; i++)
			{
				string levelName = GetLevelName(i);
				int levelId = CarLevelDefine.GetLevelIdByName(levelName);
				sb.Append("<Level ID=\"" + levelId + "\" Name=\"" + levelName + "\">\r\n");
				if (arrSSfi[i].Count > 0)
				{
					sb.Append("<PVRank SerialList=\"");
					int loopLevel = 0;
					foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
					{
						if (loopLevel == 0)
						{ sb.Append(ss.CsID.ToString()); }
						else
						{ sb.Append("," + ss.CsID.ToString()); }
						loopLevel++;
					}
					sb.Append("\" />");
				}

				//新车
				List<EnumCollection.NewCarForLevel> lncfl = base.GetLevelNewCarsByLevelID(levelId);
				string newSerials = ",";
				sb.Append("<NewCarRank SerialList=\"");
				int serialNum = 0;
				foreach (EnumCollection.NewCarForLevel ncfl in lncfl)
				{
					if (newSerials.IndexOf("," + ncfl.CsID.ToString() + ",") >= 0)
					{
						continue;
					}
					newSerials += ncfl.CsID + ",";
					if (serialNum == 0)
						sb.Append(ncfl.CsID.ToString());
					else
						sb.Append("," + ncfl.CsID);
					serialNum++;
				}

				//不足时补充按PV排序的子品牌
				int needNewCarNum = 20;
				if (serialNum < needNewCarNum && arrSSfi[i].Count > 0)
				{
					foreach (EnumCollection.SerialSortForInterface ss in arrSSfi[i])
					{
						if (serialNum == 0)
						{ sb.Append(ss.CsID.ToString()); }
						else
						{ sb.Append("," + ss.CsID.ToString()); }
						serialNum++;
						if (serialNum >= needNewCarNum)
							break;
					}
				}
				sb.Append(" \" />");
				sb.Append("</Level>");
			}
			sb.Append(sbAlllevel.ToString());
		}
	}
}