using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class SerialInfoForNew : InterfacePageBase
	{
		#region parameter
		private int csID = 0;
		private int oldCsID = 0;
		private StringBuilder sb = new StringBuilder();
		// private BitAuto.BusiPlat.AutoStorage.Common.UI.WebPage wp;
		private bool isNeedComment = false;
		private bool isNeedAllSerial = false;
		private string waiGuan = "";
		private string neiShi = "";
		private int waiGuanCount = 0;
		private int neiShiCount = 0;
		private string defaultImg = "";
		private string waiGuanURL = "";
		private string neiShiURL = "";
		private ArrayList m_CarIDlist = new ArrayList();
		private ArrayList m_CompareParameter = new ArrayList();
		private Hashtable ht = new Hashtable();
		XmlDocument docCSSort = new XmlDocument();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckSerialID();
				this.GetAllSerialInfo();
			}
		}

		// 接收子品牌参数
		private void CheckSerialID()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["csID"].ToString();
				try
				{
					csID = int.Parse(tempCsID);
				}
				catch
				{ }
			}
			else if (this.Request.QueryString["oldCsID"] != null && this.Request.QueryString["oldCsID"].ToString() != "")
			{
				string tempOldCsID = this.Request.QueryString["oldCsID"].ToString();
				try
				{
					oldCsID = int.Parse(tempOldCsID);
				}
				catch
				{ }
			}
			// 是否显示注释
			if (this.Request.QueryString["isNeedComment"] != null && this.Request.QueryString["isNeedComment"].ToString() != "")
			{
				isNeedComment = true;
			}
			// 是否显示全部子品牌
			if (this.Request.QueryString["isNeedAll"] != null)
			{
				isNeedAllSerial = true;
				// 显示全部子品牌时不显示注释
				isNeedComment = false;
			}
		}

		// 根据ID取子品牌信息
		private void GetAllSerialInfo()
		{
			if (isNeedAllSerial)
			{
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial") != null)
				{
					Response.Write(HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial").ToString());
					return;
				}
			}
			else if (this.Context.Cache[Request.Path + oldCsID.ToString()] != null)
			{
				Response.Write(this.Context.Cache[Request.Path + oldCsID.ToString()].ToString());
				return;
			}

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfo>");
			if (isNeedComment)
			{
				sb.Append("<!--请求根节点开始-->");
			}
			if (isNeedAllSerial)
			{
				DataSet ds = base.GetAllCSForInterface();
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						this.GetDataForSerial(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 0, ref sb);
					}
				}
			}
			else
			{
				this.GetDataForSerial(csID, oldCsID, ref sb);
			}
			sb.Append("</SerialInfo>");
			if (isNeedAllSerial)
			{
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial") == null)
				{
					HttpContext.Current.Cache.Insert("InterfaceForBitautoAllSerial", sb.ToString(), null, DateTime.Now.AddHours(12), TimeSpan.Zero);
				}
			}
			else
			{
				if (oldCsID != 0)
				{
					HttpContext.Current.Cache.Insert(Request.Path + oldCsID.ToString(), sb.ToString(), null, DateTime.Now.AddHours(12), TimeSpan.Zero);
				}
			}
			Response.Write(sb.ToString());
		}


		private void GetDataForSerial(int _csID, int _oldCsID, ref StringBuilder _sb)
		{
			DataSet ds = null;
			if (_csID > 0)
			{
				ds = base.GetSerialInfoByIDForInterface(true, _csID);
			}
			else if (_oldCsID > 0)
			{
				ds = base.GetSerialInfoByIDForInterface(false, _oldCsID);
			}
			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				this.GetSerialImg(int.Parse(ds.Tables[0].Rows[0]["cs_id"].ToString()));

				_sb.Append("<CurrentSerialInfo CsID=\"" + ds.Tables[0].Rows[0]["cs_id"].ToString() + "\" ");
				_sb.Append(" CsName=\"" + ds.Tables[0].Rows[0]["cs_Name"].ToString() + "\" ");
				_sb.Append(" OldCBID=\"" + ds.Tables[0].Rows[0]["oldcb_id"].ToString() + "\" ");
				_sb.Append(" CommonClassId=\"\" ");
				_sb.Append(" Prices=\"" + base.GetSerialPriceRangeByID(int.Parse(ds.Tables[0].Rows[0]["cs_id"].ToString())).Replace("万", "") + "\" ");
				_sb.Append(" ReferPriceRange=\"" + ds.Tables[0].Rows[0]["ReferPriceRange"].ToString() + "\" ");
				_sb.Append(" EngineExhaust=\"" + ds.Tables[0].Rows[0]["Engine_Exhaust"].ToString() + "\" ");
				_sb.Append(" UnderPan_Num_Type=\"" + ds.Tables[0].Rows[0]["UnderPan_Num_Type"].ToString() + "\" ");
				_sb.Append(" Body_Doors=\"" + ds.Tables[0].Rows[0]["Body_Doors"].ToString() + "\" ");
				_sb.Append(" WaiGuanIMG=\"" + waiGuan + "\" ");
				_sb.Append(" WaiGuanCount=\"" + waiGuanCount + "\" ");
				_sb.Append(" WaiGuanURL=\"" + waiGuanURL + "\" ");
				_sb.Append(" NeiShiIMG=\"" + neiShi + "\" ");
				_sb.Append(" NeiShiCount=\"" + neiShiCount + "\" ");
				_sb.Append(" NeiShiURL=\"" + neiShiURL + "\" ");
				_sb.Append(" Virtue=\"" + ds.Tables[0].Rows[0]["cs_Virtues"].ToString().Replace("\"", "'").Replace("<", "《").Replace(">", "》") + "\" ");
				_sb.Append(" Defect=\"" + ds.Tables[0].Rows[0]["cs_Defect"].ToString().Replace("\"", "'").Replace("<", "《").Replace(">", "》") + "\" ");
				_sb.Append(" Level=\"" + ds.Tables[0].Rows[0]["cs_CarLevel"].ToString() + "\" ");

				string sortTemp = "0";
				if (ds.Tables[0].Rows[0]["cs_CarLevel"].ToString() != "")
				{
					DataSet currentSort = base.GetSerialSortListByTimeAndCarLevel(ds.Tables[0].Rows[0]["cs_CarLevel"].ToString(), DateTime.Today.AddDays(-1), DateTime.Today);
					if (currentSort != null && currentSort.Tables[0].Rows.Count > 0)
					{
						for (int i = 0; i < currentSort.Tables[0].Rows.Count; i++)
						{
							if (currentSort.Tables[0].Rows[i]["cs_id"].ToString() == ds.Tables[0].Rows[0]["cs_id"].ToString())
							{
								sortTemp = currentSort.Tables[0].Rows[i]["Rank"].ToString();
								break;
							}
						}
					}
				}
				_sb.Append(" Sort=\"" + sortTemp + "\" ");
				_sb.Append(" BitautoTestURL=\"" + ds.Tables[0].Rows[0]["bitautoTestURL"].ToString() + "\" ");
				_sb.Append(" AllSpell=\"" + ds.Tables[0].Rows[0]["CSAllSpell"].ToString() + "\" ");
				this.GetSerialYesterdayAndLastWeek(ds.Tables[0].Rows[0]["cs_id"].ToString(), ref _sb);

				#region 取最近车型 长 宽 高
				string sqlNewCar = " select top 1 cb.car_id from dbo.Car_Basic cb ";
				sqlNewCar += " left join dbo.Car_Extend_Item cei on cb.car_id = cei.car_id ";
				sqlNewCar += " where cb.cs_id = " + ds.Tables[0].Rows[0]["cs_id"].ToString() + " and  cei.Car_MarketDate = ";
				sqlNewCar += " (select Max(Car_MarketDate) from Car_Extend_Item ";
				sqlNewCar += " where Car_Id in (select car_id from dbo.Car_Basic where cs_id = " + ds.Tables[0].Rows[0]["cs_id"].ToString() + ")) ";
				sqlNewCar += " and cb.IsState=1 and cb.car_SaleState='在销' order by cei.car_id desc ";
				// string sqlNewCar = " select top 1 cb.car_id from dbo.Car_Basic cb where cb.cs_id = " + ds.Tables[0].Rows[0]["cs_id"].ToString() + " and cb.IsState=1 and cb.car_SaleState='在销' order by cb.car_id desc";
				DataSet dsNewCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlNewCar);
				if (dsNewCar != null && dsNewCar.Tables[0].Rows.Count > 0)
				{
					int carIDtemp = int.Parse(dsNewCar.Tables[0].Rows[0]["car_id"].ToString());
					string OutSet_Length = new Car_BasicBll().GetCarParamEx(carIDtemp, 588);
					string OutSet_Width = new Car_BasicBll().GetCarParamEx(carIDtemp, 593);
					string OutSet_Height = new Car_BasicBll().GetCarParamEx(carIDtemp, 586);

					_sb.Append(" OutSet_Length:\"" + OutSet_Length + "\" ");
					_sb.Append(" OutSet_Width:\"" + OutSet_Width + "\" ");
					_sb.Append(" OutSet_Height:\"" + OutSet_Height + "\" ");

					// CarStorageAI carStorage = new CarStorageAI();
					//m_CarIDlist.Add(carIDtemp);
					//m_CompareParameter.Add("OutSet_Length");
					//m_CompareParameter.Add("OutSet_Width");
					//m_CompareParameter.Add("OutSet_Height");
					//Hashtable hsCar = carStorage.GetCarDataHash(m_CarIDlist, m_CompareParameter);
					//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Length"))
					//{
					//    _sb.Append(" OutSet_Length=\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Length"].ToString() + "\" ");
					//}
					//else
					//{
					//    _sb.Append(" OutSet_Length=\"\" ");
					//}
					//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Width"))
					//{
					//    _sb.Append(" OutSet_Width=\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Width"].ToString() + "\" ");
					//}
					//else
					//{
					//    _sb.Append(" OutSet_Width=\"\" ");
					//}
					//if (hsCar.ContainsKey(carIDtemp) && ((Hashtable)hsCar[carIDtemp]).ContainsKey("OutSet_Height"))
					//{
					//    _sb.Append(" OutSet_Height=\"" + ((Hashtable)hsCar[carIDtemp])["OutSet_Height"].ToString() + "\" ");
					//}
					//else
					//{
					//    _sb.Append(" OutSet_Height=\"\" ");
					//}
				}
				else
				{
					_sb.Append(" OutSet_Length=\"\" OutSet_Width=\"\" OutSet_Height=\"\" ");
				}
				#endregion

				string catchKeyCard = "CsSummaryCsCard_CsID" + csID.ToString();
				object serialInfoCardByCsID = null;
				CacheManager.GetCachedData(catchKeyCard, out serialInfoCardByCsID);
				EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();
				if (serialInfoCardByCsID == null)
				{
					sic = base.GetSerialInfoCardByCsID(csID);
					CacheManager.InsertCache(catchKeyCard, sic, 60);
				}
				else
				{
					sic = (EnumCollection.SerialInfoCard)serialInfoCardByCsID;
				}

				// new 
				// _sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost + "\" ");
				if (sic.CsSummaryFuelCost.Length > 0)
					_sb.Append(" CsOfficialFuelCost=\"" + sic.CsSummaryFuelCost + "\" ");                    //综合工况油耗
				else
					_sb.Append(" CsOfficialFuelCost=\"" + sic.CsOfficialFuelCost + "\" ");              //官方油耗

				_sb.Append(" CsPicCount=\"" + sic.CsPicCount.ToString() + "\" ");
				_sb.Append(" CsDianPingCount=\"" + sic.CsDianPingCount.ToString() + "\" ");
				_sb.Append(" CsAskCount=\"" + sic.CsAskCount.ToString() + "\" ");
				_sb.Append(" CsShowName=\"" + sic.CsShowName + "\" ");
				// add for new 

				_sb.Append(" CsImage=\"" + sic.CsDefaultPic + "\"/>");
				// new 
				//string imgUrl = ds.Tables[0].Rows[0]["SiteImageUrl"].ToString();
				//if (imgUrl != "")
				//{
				//    int imgId = int.Parse(ds.Tables[0].Rows[0]["SiteImageId"].ToString());
				//    string img = base.GetPublishImage(1, imgUrl, imgId);
				//    _sb.Append(" CsImage=\"" + img + "\"/>");
				//}
				//else
				//{
				//    _sb.Append(" CsImage=\"\" />");
				//}
				if (isNeedComment)
				{
					_sb.Append("<!--子品牌信息开始(CsID:子品牌ID,CsName:子品牌名,OldCBID:对应老库品牌ID");
					_sb.Append(",CommonClassId:图片分类ID,Prices:报价范围,ReferPriceRange:厂商指导价范围");
					_sb.Append(",EngineExhaust:排量范围,WaiGuanIMG:外观图,WaiGuanCount:外观张数,WaiGuanURL:外观链接");
					_sb.Append(",NeiShiIMG:内饰图,NeiShiCount:内饰张数,NeiShiURL:内饰链接");
					_sb.Append(",Virtue:子品牌优点,Defect:子品牌缺点,Level:子品牌级别,Sort:子品牌排行,CsImage:子品牌图片)-->");
				}
				_sb.Append("<CurrentCB CbID=\"" + ds.Tables[0].Rows[0]["cb_id"].ToString() + "\" ");
				_sb.Append(" CbName=\"" + ds.Tables[0].Rows[0]["cb_name"].ToString() + "\" ");
				_sb.Append(" MasterLogo=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/30/m_" + ds.Tables[0].Rows[0]["bs_id"].ToString() + "_30.png\" ");
				_sb.Append(" AllSpell=\"" + ds.Tables[0].Rows[0]["CBAllSpell"].ToString() + "\" ");
				_sb.Append(" OldCsID=\"" + ds.Tables[0].Rows[0]["oldcs_id"].ToString() + "\"/>");
				if (isNeedComment)
				{
					_sb.Append("<!--品牌信息(CbID:品牌ID,CbName:品牌名,OldCsID:对应老库系列ID)-->");
				}
				_sb.Append("<CurrentCP CpID=\"" + ds.Tables[0].Rows[0]["cp_id"].ToString() + "\" ");
				_sb.Append(" CpName=\"" + ds.Tables[0].Rows[0]["cp_name"].ToString() + "\" ");
				_sb.Append(" CpLogo=\"" + base.GetLogImage(ds.Tables[0].Rows[0]["cp_id"].ToString(), "p", "b") + "\" ");
				_sb.Append(" OldCpID=\"" + ds.Tables[0].Rows[0]["oldcp_id"].ToString() + "\"/>");
				if (isNeedComment)
				{
					_sb.Append("<!--厂商信息(CpID:厂商ID,CpName:厂商名,CpLogo:厂商Logo,OldCpID:对应老库厂商ID)-->");
				}

				this.GetTheSerialToSerial(int.Parse(ds.Tables[0].Rows[0]["cs_id"].ToString()), ref _sb);
				this.GetSortList(ref _sb);

				// add for new 
				_sb.Append("<SerialNews>");
				_sb.Append("<SerialNewsItem Name=\"上市专题\" URL=\"" + sic.CsNewShangShi + "\" />");
				_sb.Append("<SerialNewsItem Name=\"购车手册\" URL=\"" + sic.CsNewGouCheShouChe + "\" />");
				_sb.Append("<SerialNewsItem Name=\"销售数据\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/xiaoliang/\" />");
				_sb.Append("<SerialNewsItem Name=\"维修保养\" URL=\"" + sic.CsNewWeiXiuBaoYang + "\" />");
				_sb.Append("<SerialNewsItem Name=\"科技\" URL=\"" + sic.CsNewKeJi + "\" />");
				_sb.Append("</SerialNews>");
				// add for new 
			}
		}

		// 取相同关注子品牌
		private void GetTheSerialToSerial(int csId, ref StringBuilder _sb)
		{
			_sb.Append("<OtherSerial>");
			DataSet otherSerial = new DataSet();
			otherSerial = base.GetSerialForSerialByID(csId);
			if (otherSerial != null && otherSerial.Tables[0].Rows.Count > 0)
			{
				int max = otherSerial.Tables[0].Rows.Count > 4 ? 4 : otherSerial.Tables[0].Rows.Count;
				for (int i = 0; i < max; i++)
				{
					_sb.Append("<Serial CsID=\"" + otherSerial.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" CsName=\"" + otherSerial.Tables[0].Rows[i]["cs_name"].ToString() + "\" ");
					_sb.Append(" OldCbID=\"" + otherSerial.Tables[0].Rows[i]["oldcb_id"].ToString() + "\" ");
					_sb.Append(" AllSpell=\"" + otherSerial.Tables[0].Rows[i]["allSpell"].ToString() + "\" ");
					_sb.Append(" Price=\"" + otherSerial.Tables[0].Rows[i]["prices"].ToString() + "\" ");

					string imgUrl = otherSerial.Tables[0].Rows[i]["SiteImageUrl"].ToString();
					if (imgUrl != "")
					{
						int imgId = int.Parse(otherSerial.Tables[0].Rows[i]["SiteImageId"].ToString());

						string img = base.GetPublishImage(1, imgUrl, imgId);

						_sb.Append(" SerialImage=\"" + img + "\" />");
					}
					else
					{
						_sb.Append(" SerialImage=\"\" />");
					}
					if (isNeedComment)
					{
						_sb.Append("<!--相同关注子品牌(CsID:子品牌ID,CsName:子品牌名,OldCbID:对应老库品牌ID,Price:价格范围,SerialImage:子品牌图片)-->");
					}
				}
			}
			_sb.Append("</OtherSerial>");
		}

		private void GetSerialImg(int csid)
		{
			DataSet ds = base.GetSerialImgByIDForInterface(csid);
			if (ds != null && ds.Tables.Count == 3)
			{
				if (ds.Tables[2].Rows.Count > 0)
				{
					defaultImg = base.GetPublishImage(1, ds.Tables[2].Rows[0]["SiteImageUrl"].ToString(), int.Parse(ds.Tables[2].Rows[0]["SiteImageId"].ToString())); ;
				}
			}
			#region 新规则的内饰外观
			// 新规则的内饰外观
			string url = string.Format(ConfigurationManager.AppSettings["PhotoService"], csid.ToString());
			try
			{
				string cacheKey = "InterfaceForBitautoCarPhoto-" + csid.ToString();
				DataSet dsImgInterface = new DataSet();
				if (Cache[cacheKey] == null)
				{
					dsImgInterface.ReadXml(url);
					Cache.Insert(cacheKey, dsImgInterface, null, DateTime.Now.AddHours(6), TimeSpan.Zero);
				}
				else
				{
					dsImgInterface = (DataSet)Cache[cacheKey];
				}

				if (dsImgInterface != null && dsImgInterface.Tables.Count > 0)
				{
					// 外观内饰总数
					if (dsImgInterface.Tables.Contains("A") && dsImgInterface.Tables["A"].Rows.Count > 0)
					{
						for (int i = 0; i < dsImgInterface.Tables["A"].Rows.Count; i++)
						{
							if (dsImgInterface.Tables["A"].Rows[i]["G"].ToString() == "7")
							{
								neiShiCount = int.Parse(dsImgInterface.Tables["A"].Rows[i]["N"].ToString());
							}
							if (dsImgInterface.Tables["A"].Rows[i]["G"].ToString() == "6")
							{
								waiGuanCount = int.Parse(dsImgInterface.Tables["A"].Rows[i]["N"].ToString());
							}
						}
					}
					// 外观内饰
					if (dsImgInterface.Tables.Contains("C") && dsImgInterface.Tables["C"].Rows.Count > 0)
					{
						for (int i = 0; i < dsImgInterface.Tables["C"].Rows.Count; i++)
						{
							if (waiGuan != "" && neiShi != "")
							{
								break;
							}
							if (neiShi == "" && dsImgInterface.Tables["C"].Rows[i]["P"].ToString() == "7")
							{
								neiShi = base.GetPublishImage(1, dsImgInterface.Tables["C"].Rows[i]["U"].ToString(), int.Parse(dsImgInterface.Tables["C"].Rows[i]["I"].ToString()));
								neiShiURL = "http://photo.bitauto.com/serialmore/" + csid.ToString() + "/7/1";
							}
							if (waiGuan == "" && dsImgInterface.Tables["C"].Rows[i]["P"].ToString() == "6")
							{
								waiGuan = base.GetPublishImage(1, dsImgInterface.Tables["C"].Rows[i]["U"].ToString(), int.Parse(dsImgInterface.Tables["C"].Rows[i]["I"].ToString()));
								waiGuanURL = "http://photo.bitauto.com/serialmore/" + csid.ToString() + "/6/1";
							}
						}
					}
				}
			}
			catch
			{
			}
			#endregion
		}

		// 子品牌排行
		private void GetSerialYesterdayAndLastWeek(string csID, ref StringBuilder sb)
		{
			CultureInfo myCI = new CultureInfo("zh-CN");
			System.Globalization.Calendar myCal = myCI.Calendar;
			CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
			DayOfWeek myFirstDOW = DayOfWeek.Monday;
			int thisWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);

			string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/xml/SerialSort-D" + DateTime.Now.ToShortDateString() + "-W" + Convert.ToString(thisWeek - 1) + ".xml";
			try
			{
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdayAndLastWeekSort" + DateTime.Now.ToShortDateString()) != null)
				{
					docCSSort = (XmlDocument)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdayAndLastWeekSort" + DateTime.Now.ToShortDateString());
				}
				else
				{
					docCSSort.Load(xmlPath);
					HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialYesterdayAndLastWeekSort" + DateTime.Now.ToShortDateString(), docCSSort, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
				}
				XmlNode xn = docCSSort.SelectSingleNode("/Params/Serial[@ID='" + csID + "']");
				string yes = xn.Attributes["YesterdaySort"].Value;
				string last = xn.Attributes["LastWeekSort"].Value;
				int lastInt = int.Parse(last) - int.Parse(yes);
				sb.Append(" YesterdaySort=\"" + yes + "\" ");
				sb.Append(" LastWeekSort=\"" + lastInt.ToString() + "\" ");
			}
			catch
			{
				sb.Append(" YesterdaySort=\"0\" ");
				sb.Append(" LastWeekSort=\"0\" ");
			}
		}

		// 子品牌排行
		private void GetSortList(ref StringBuilder _sb)
		{
			_sb.Append("<SortList>");

			// 修改 Jul.21.2009 时间范围
			DataSet dsYesterday1 = this.GetAllSerialYesterdaySort(10, "小型车");
			DataSet dsYesterday2 = this.GetAllSerialYesterdaySort(10, "紧凑型车");
			DataSet dsYesterday3 = this.GetAllSerialYesterdaySort(10, "中型车");

			this.ForeachTableForXML(dsYesterday1, ref _sb, "小型车");
			this.ForeachTableForXML(dsYesterday2, ref _sb, "紧凑型车");
			this.ForeachTableForXML(dsYesterday3, ref _sb, "中型车");

			if (isNeedComment)
			{
				_sb.Append("<!--级别排行(Sort:当前排位,CsID:对应子品牌ID,CsName:对应子品牌名,CurrentTotal:当前总数,Position:涨幅状况)-->");
			}
			_sb.Append("</SortList>");
		}

		private void ForeachTableForXML(DataSet dsYesterday, ref StringBuilder _sb, string level)
		{
			if (dsYesterday != null && dsYesterday.Tables[0].Rows.Count > 0)
			{
				_sb.Append("<Level Name=\"" + level + "\" >");
				for (int i = 0; i < dsYesterday.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item Sort=\"" + Convert.ToString(i + 1) + "\" ");
					_sb.Append(" CsID=\"" + dsYesterday.Tables[0].Rows[i]["cs_id"].ToString() + "\" CsName=\"" + dsYesterday.Tables[0].Rows[i]["cs_Name"].ToString() + "\" ");
					_sb.Append(" AllSpell=\"" + dsYesterday.Tables[0].Rows[i]["allSpell"].ToString() + "\" ");
					_sb.Append(" CurrentTotal=\"" + dsYesterday.Tables[0].Rows[i]["FeignedPV"].ToString() + "\" ");

					if (docCSSort != null && docCSSort.HasChildNodes)
					{
						XmlNode xn = docCSSort.SelectSingleNode("/Params/Serial[@ID='" + dsYesterday.Tables[0].Rows[i]["cs_id"].ToString() + "']");
						string yes = "0";
						string last = "0";
						if (xn != null)
						{
							yes = xn.Attributes["YesterdaySort"].Value;
							last = xn.Attributes["LastWeekSort"].Value;
						}
						int yesterday = int.Parse(yes);
						int lastWeek = int.Parse(last);
						if (lastWeek > yesterday)
						{
							_sb.Append(" Position=\"1\" ");
						}
						else if (yesterday == lastWeek)
						{
							_sb.Append(" Position=\"0\" ");
						}
						else
						{
							_sb.Append(" Position=\"-1\" ");
						}
					}
					_sb.Append(" Price=\"" + dsYesterday.Tables[0].Rows[i]["prices"].ToString() + "\" ");
					_sb.Append(" />");
				}
				_sb.Append("</Level>");
			}
		}

		/// <summary>
		/// 取所有子品牌昨天访问量
		/// </summary>
		/// <returns></returns>
		private DataSet GetAllSerialYesterdaySort(int top, string carLevel)
		{
			DataSet ds = new DataSet();
			if (HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySort_" + carLevel) != null)
			{
				ds = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoSerialYesterdaySort_" + carLevel);
			}
			else
			{
				string sql = " select top " + top.ToString() + " csp.cs_id,csp.pv_sumnum as FeignedPV,cs.oldcb_id,cs.cs_CarLevel,cs.allSpell,cs.cs_name,csi.Prices ";
				sql += " from dbo.Chart_Serial_Pv csp ";
				sql += " left join dbo.Car_Serial cs on csp.cs_id = cs.cs_id ";
				sql += " left join dbo.Car_Serial_Item csi on csp.cs_id = csi.cs_id ";
				sql += " where csp.createDateStr>=convert(varchar(10),getdate()-1,120) ";
				sql += " and csp.createDateStr<convert(varchar(10),getdate(),120)  ";
				sql += " and  cs.cs_CarLevel is not null and cs.cs_CarLevel='" + carLevel + "' ";
				sql += " order by  cs.cs_CarLevel,csp.pv_sumnum desc ";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("InterfaceForBitautoSerialYesterdaySort_" + carLevel, ds, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}
			return ds;
		}
	}
}