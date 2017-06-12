using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL
{
	public class SelectCarToolBll
	{
		/// <summary>
		/// 获取热点子品牌Html
		/// </summary>
		/// <returns></returns>
		public string GetHotSerialHtml()
		{
			string cacheKey = "SelectCarTool_hot";
			string code = (string)CacheManager.GetCachedData(cacheKey);
			if (code == null)
			{
				DataSet hotSet = new SelectCarToolDal().GetTopPVSerials(12);
				StringBuilder htmlCode = new StringBuilder();

				//图片Url
				Dictionary<int, XmlElement> urlDic = AutoStorageService.GetImageUrlDic();

				if (hotSet != null && hotSet.Tables.Count > 0)
				{
					foreach (DataRow row in hotSet.Tables[0].Rows)
					{
						int serialId = Convert.ToInt32(row["CS_Id"]);
						string showName = Convert.ToString(row["cs_ShowName"]);
						string spell = Convert.ToString(row["allSpell"]).Trim().ToLower();
						string serialUrl = "http://car.bitauto.com/" + spell + "/";
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

						htmlCode.AppendLine("<div class=\"list\">");
						htmlCode.AppendLine("<a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" width=\"120px\" height=\"80px\" /></a>");
						htmlCode.AppendLine("<a href=\"" + serialUrl + "\" target=\"_blank\">" + showName + "</a>");
						htmlCode.AppendLine("</div>");
					}
				}
				code = htmlCode.ToString();
				CacheManager.InsertCache(cacheKey, code, WebConfig.CachedDuration);
			}
			return code;
		}

		public List<SerialInfo> SelectCarByParameters(SelectCarParameters paras)
		{
			string cacheKey = "tree" + GenCacheKey(paras);

			//if (paras.MinPrice == 0)
			//    paras.MinPrice = 1;
			if (paras.MinDis == 0.0)
				paras.MinDis = 0;
			//if (paras.MinReferPrice == 0)
			//    paras.MinReferPrice = 1;

			List<SerialInfo> serialList = (List<SerialInfo>)CacheManager.GetCachedData(cacheKey);
			if (serialList == null)
			{
				Dictionary<int, int> pvDic = Car_SerialBll.GetAllSerialUVDict();
				DataSet csDs = new SelectCarToolDal().SelectCar(paras);
				serialList = new List<SerialInfo>();
				Dictionary<int, SerialInfo> serialDic = new Dictionary<int, SerialInfo>();
				int carNum = 0;
				int serialNum = 0;
				if (csDs != null && csDs.Tables.Count > 0)
				{

					foreach (DataRow row in csDs.Tables[0].Rows)
					{
						int serialId = Convert.ToInt32(row["csId"]);
						double referPrice = ConvertHelper.GetDouble(row["CarReferPrice"]);
						if (!serialDic.ContainsKey(serialId))
						{
							string showName = Convert.ToString(row["cs_ShowName"]).Trim();
							string spell = Convert.ToString(row["allSpell"]).Trim().ToLower();
							if (showName.Length == 0 || spell.Length == 0)
								continue;
							//int pv = ConvertHelper.GetInteger(row["Pv_SumNum"]);
							SerialInfo info = new SerialInfo(serialId, showName, spell);
							if (pvDic.ContainsKey(serialId))
								info.PVNum = pvDic[serialId];
							else
								info.PVNum = 0;
							info.CarNum = 0;
							info.MinPrice = 0.0;
							info.MinReferPrice = referPrice;
							info.ImageUrl = Car_SerialBll.GetSerialImageUrl(serialId);
							info.PriceRange = new PageBase().GetSerialPriceRangeByID(serialId).Trim();
							if (info.PriceRange.Length == 0)
								info.PriceRange = "暂无报价";
							else
							{
								//取一个最低报价
								string[] priceSeg = info.PriceRange.Replace("万", "").Split('-');
								if (priceSeg.Length > 0)
								{
									double minPrice = 0.0;
									bool isDouble = Double.TryParse(priceSeg[0], out minPrice);
									if (isDouble)
										info.MinPrice = minPrice;
									info.MaxPrice = (priceSeg.Length > 1) ? ConvertHelper.GetDouble(priceSeg[1]) : minPrice;
								}
							}
							serialDic[serialId] = info;
							serialList.Add(serialDic[serialId]);
						}
						else
						{
							//更新最低指导价，排序用
							if (referPrice < serialDic[serialId].MinReferPrice)
								serialDic[serialId].MinReferPrice = referPrice;
						}
						serialDic[serialId].CarNum++;
						int carId = Convert.ToInt32(row["carId"]);
						serialDic[serialId].CarIdList += carId + ",";
					}

					carNum = csDs.Tables[0].Rows.Count;
					serialNum = serialList.Count;
				}

				//排序
				serialList.Sort(SerialInfo.CompareSerialByPVDesc);
				CacheManager.InsertCache(cacheKey, serialList, WebConfig.CachedDuration);
			}

			return serialList;
		}

		/// <summary>
		/// 获取选车结果Html
		/// </summary>
		/// <returns></returns>
		public string GetSelectCarHtml(SelectCarParameters paras)
		{
			string cacheKey = GenCacheKey(paras);

			XmlDocument xmlDoc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
			if (xmlDoc == null)
			{
				DataSet csDs = new SelectCarToolDal().SelectCar(paras);
				List<SerialInfo> serialList = SelectCarByParameters(paras);
				xmlDoc = GenerateDoc(serialList);
				//xmlDoc.DocumentElement.SetAttribute("serila", serialNum.ToString());
				//xmlDoc.DocumentElement.SetAttribute("car", carNum.ToString());

				CacheManager.InsertCache(cacheKey, xmlDoc, WebConfig.CachedDuration);
			}

			XmlDocument pageDoc = GetPageXmlDocument(xmlDoc, paras.PageIndex);
			return pageDoc.OuterXml;
		}

		/// <summary>
		/// 获取车型信息
		/// </summary>
		/// <param name="carIdList"></param>
		/// <returns></returns>
		public string GetSelectCarInfoHtml(string carIdList)
		{
			string cacheKey = "allCarInformationFroSelectCar";
			DataSet carDs = (DataSet)CacheManager.GetCachedData(cacheKey);
			if (carDs == null)
			{
				SelectCarToolDal scDal = new SelectCarToolDal();
				carDs = scDal.SelectCarInfo();
				DataSet oilDs = scDal.SelectCarOil();
				if (carDs.Tables.Count > 0)
				{
					//合并处理数据
					carDs.Tables[0].Columns.Add("carOil");
					DataTable oilTable = oilDs.Tables[0];
					foreach (DataRow row in carDs.Tables[0].Rows)
					{
						int carId = Convert.ToInt32(row["carId"]);
						DataRow[] rows = oilTable.Select("Carid=" + carId);
						if (rows.Length > 0)
						{
							if (rows[0]["Pvalue"] == DBNull.Value)
								rows[0]["Pvalue"] = 0;
							row["carOil"] = rows[0]["Pvalue"];
						}
						else
							row["carOil"] = 0;

						if (row["car_ReferPrice"] == DBNull.Value)
							row["car_ReferPrice"] = 0;
						if (row["UnderPan_TransmissionType"] == DBNull.Value)
							row["UnderPan_TransmissionType"] = "";
						if (row["Engine_Exhaust"] == DBNull.Value)
							row["Engine_Exhaust"] = "";
						string trans = Convert.ToString(row["UnderPan_TransmissionType"]);
						int pos = trans.IndexOf("挡");
						if (pos > -1)
						{
							trans = trans.Substring(pos + 1);
							row["UnderPan_TransmissionType"] = trans;
						}
					}
				}
				carDs.AcceptChanges();
				CacheManager.InsertCache(cacheKey, carDs, WebConfig.CachedDuration);
			}

			return GetSelectCarInfoFromDs(carDs, carIdList);
		}


		/// <summary>
		/// 获取车型信息
		/// </summary>
		/// <param name="carDs"></param>
		/// <param name="carIdList"></param>
		/// <returns></returns>
		private string GetSelectCarInfoFromDs(DataSet carDs, string carIdList)
		{
			carIdList = carIdList.Trim(',');
			if (carIdList.Length == 0)
				return "";

			StringBuilder htmlCode = new StringBuilder();
			DataRow[] rows = carDs.Tables[0].Select("carId in (" + carIdList + ")", "Engine_Exhaust,car_ReferPrice");
			//计算最大PV
			int maxPv = 0;
			foreach (DataRow row in rows)
			{
				int pvNum = row["pvNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["pvNum"]);
				if (pvNum > maxPv)
					maxPv = pvNum;
			}

			htmlCode.AppendLine("<div class=\"view_terms\">");
			htmlCode.AppendLine("<table>");
			htmlCode.AppendLine("<col class=\"col1\" />");
			htmlCode.AppendLine("<col class=\"col2\" />");
			htmlCode.AppendLine("<col class=\"col3\" />");
			htmlCode.AppendLine("<col class=\"col4\" />");
			htmlCode.AppendLine("<col class=\"col5\" />");
			htmlCode.AppendLine("<col class=\"col6\" />");
			htmlCode.AppendLine("<col class=\"col7\" />");
			htmlCode.AppendLine("<thead>");
			htmlCode.AppendLine("<tr>");
			htmlCode.AppendLine("<th>符合条件的车款</th>");
			htmlCode.AppendLine("<th>热度</th>");
			htmlCode.AppendLine("<th>排量</th>");
			htmlCode.AppendLine("<th>变速箱</th>");
			htmlCode.AppendLine("<th>油耗</th>");
			htmlCode.AppendLine("<th>厂家指导价</th>");
			htmlCode.AppendLine("<th>参考成交价</th>");
			htmlCode.AppendLine("</tr>");
			htmlCode.AppendLine("</thead>");
			htmlCode.AppendLine("<tbody>");

			foreach (DataRow row in rows)
			{
				string serialShowName = Convert.ToString(row["cs_ShowName"]).Trim();
				string serialAllSpell = Convert.ToString(row["allspell"]).Trim().ToLower();
				string carName = Convert.ToString(row["Car_Name"]).Trim();
				string exhaust = Convert.ToString(row["Engine_Exhaust"]).Trim();
				string trans = Convert.ToString(row["UnderPan_TransmissionType"]).Trim().Replace("变速器", "");
				double referPrice = Convert.ToDouble(row["car_ReferPrice"]);
				double minPrice = Convert.ToDouble(row["MinPrice"]);
				double maxPrice = Convert.ToDouble(row["MaxPrice"]);
				double oil = Convert.ToDouble(row["carOil"]);
				string yearType = ConvertHelper.GetString(row["Car_YearType"]);
				if (yearType.Length == 2)
					carName += "(" + yearType + "款)";
				else if (yearType.Length == 4)
					carName += "(" + yearType.Substring(2) + "款)";
				int carId = Convert.ToInt32(row["carId"]);
				int pvNum = row["pvNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["pvNum"]);
				htmlCode.AppendLine("<tr>");
				htmlCode.AppendLine("<td class=\"name\"><a href=\"http://car.bitauto.com/" + serialAllSpell + "/m" + carId + "/\" target=\"_blank\">" + serialShowName + " " + carName + "</a></td>");
				double percent = pvNum / (double)maxPv * 100;
				int pvHot = (int)Math.Round(percent);
				htmlCode.AppendLine("<td><div class=\"hot\" style=\"width:" + pvHot + "px\"></div></td>");
				htmlCode.AppendLine("<td>" + exhaust + "</td>");
				htmlCode.AppendLine("<td>" + trans + "</td>");
				if (oil > 0)
					htmlCode.AppendLine("<td>" + oil + "L</td>");
				else
					htmlCode.AppendLine("<td>无</td>");
				if (referPrice > 0)
					htmlCode.AppendLine("<td>" + referPrice + "万</td>");
				else
					htmlCode.AppendLine("<td>暂无</td>");
				if (minPrice == 0 && maxPrice == 0)
					htmlCode.AppendLine("<td class=\"mart\">暂无报价</td>");
				else
					htmlCode.AppendLine("<td class=\"mart\"><a href=\"http://car.bitauto.com/" + serialAllSpell + "/m" + carId + "/baojia/\" target=\"_blank\">" + minPrice + "万-" + maxPrice + "万</a></td>");
				htmlCode.AppendLine("</tr>");
			}

			htmlCode.AppendLine("</tbody>");
			htmlCode.AppendLine("</table>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("</div>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 根据查询条件生成缓存键值
		/// </summary>
		/// <param name="paras"></param>
		/// <returns></returns>
		private string GenCacheKey(SelectCarParameters paras)
		{
			string cacheKey = "SelectCar";
			if (paras.MinPrice != 0)
			{
				cacheKey += "_lp" + paras.MinPrice;
			}
			if (paras.MaxPrice != 0)
			{
				cacheKey += "_rp" + paras.MaxPrice;
			}
			if (paras.MinReferPrice != 0)
				cacheKey += "_lrp" + paras.MinReferPrice;
			if (paras.MaxReferPrice != 0)
				cacheKey += "_rrp" + paras.MaxReferPrice;

			if (paras.MinDis != 0)
			{
				cacheKey += "_ld" + paras.MinDis;
			}
			if (paras.MaxDis != 0)
			{
				cacheKey += "_rd" + paras.MaxDis;
			}
			if (paras.TransmissionType != 0)
			{
				cacheKey += "_tr" + paras.TransmissionType;
			}
			if (paras.BodyForm != 0)
			{
				cacheKey += "_bf" + paras.BodyForm;
			}
			if (paras.Level != 0)
			{
				cacheKey += "_le" + paras.Level;
			}
			if (paras.Purpose != 0)
			{
				cacheKey += "_pu" + paras.Purpose;
			}
			if (paras.Country != 0)
			{
				cacheKey += "_co" + paras.Country;
			}
			if (paras.ComfortableConfig != 0)
			{
				cacheKey += "_com" + paras.ComfortableConfig;
			}
			if (paras.SafetyConfig != 0)
			{
				cacheKey += "_sa" + paras.SafetyConfig;
			}
			if (paras.CarConfig != 0)
				cacheKey += "_cfg" + paras.CarConfig;
			if (paras.BrandType != 0)
				cacheKey += "_bt" + paras.BrandType;
			if (paras.DriveType != 0)
				cacheKey += "_dt" + paras.DriveType;
			if (paras.FuelType != 0)
				cacheKey += "_f" + paras.FuelType;
			if (paras.MinBodyDoors != 0)
				cacheKey += "_lbd" + paras.MinBodyDoors;
			if (paras.MaxBodyDoors != 0)
				cacheKey += "_rbd" + paras.MaxBodyDoors;
			if (paras.MinPerfSeatNum != 0)
				cacheKey += "_lsn" + paras.MinPerfSeatNum;
			if (paras.MaxPerfSeatNum != 0)
				cacheKey += "_rsn" + paras.MaxPerfSeatNum;
			if (paras.IsWagon == 1)
				cacheKey += "_lv" + paras.IsWagon;
			return cacheKey;
		}

		/// <summary>
		/// 取单页的内容
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <param name="pageNum"></param>
		/// <returns></returns>
		private XmlDocument GetPageXmlDocument(XmlDocument xmlDoc, int pageNum)
		{
			XmlDocument pageDoc = new XmlDocument();
			pageDoc.AppendChild(pageDoc.ImportNode(xmlDoc.DocumentElement, false));
			XmlDeclaration xmlDeclar = pageDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			pageDoc.InsertBefore(xmlDeclar, pageDoc.DocumentElement);

			int pageCount = Convert.ToInt32(pageDoc.DocumentElement.GetAttribute("page"));
			if (pageCount < pageNum)
				pageNum = pageCount;

			XmlNode pageNode = xmlDoc.SelectSingleNode("/root/Page[@pageNum=\"" + pageNum + "\"]");
			if (pageNode != null)
				pageDoc.DocumentElement.AppendChild(pageDoc.ImportNode(pageNode, true));

			return pageDoc;
		}

		/// <summary>
		/// 生成Xml格式的数据
		/// </summary>
		/// <param name="serialList"></param>
		/// <returns></returns>
		private XmlDocument GenerateDoc(List<SerialInfo> serialList)
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement root = xmlDoc.CreateElement("root");
			xmlDoc.AppendChild(root);
			XmlDeclaration xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			xmlDoc.InsertBefore(xmlDeclar, root);

			//图片Url
			Dictionary<int, XmlElement> urlDic = AutoStorageService.GetImageUrlDic();

			StringBuilder htmlCode = new StringBuilder();
			int counter = 0;
			int layerNum = 1;
			int pageNum = 1;
			int carNum = 0;
			List<XmlElement> tempList = new List<XmlElement>();		//存储每页的子品牌
			foreach (SerialInfo info in serialList)
			{
				carNum += info.CarNum;
				counter++;

				//加入子品牌中的车型数据
				XmlElement serialEle = xmlDoc.CreateElement("Serial");
				serialEle.SetAttribute("id", info.SerialId.ToString());
				serialEle.SetAttribute("cars", info.CarIdList);
				tempList.Add(serialEle);

				//生成代码,按页
				string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
				string imgUrl = "";
				if (urlDic.ContainsKey(info.SerialId))
				{
					int imgId = Convert.ToInt32(urlDic[info.SerialId].GetAttribute("ImageId"));
					imgUrl = urlDic[info.SerialId].GetAttribute("ImageUrl");
					if (imgId == 0 || imgUrl == "")
						imgUrl = WebConfig.DefaultCarPic;
					else
						imgUrl = new OldPageBase().GetPublishImage(2, imgUrl, imgId);
				}
				else
					imgUrl = WebConfig.DefaultCarPic;

				htmlCode.AppendLine("<div class=\"list\">");
				htmlCode.AppendLine("<a href=\"" + serialUrl + "\" target=\"_blank\"><img src=\"" + imgUrl + "\" width=\"120px\" height=\"80px\" /></a>");
				//string shortName = StringHelper.SubString(info.ShowName,16,false);
				// 				if(shortName != info.ShowName)
				// 					htmlCode.AppendLine("<a href=\"" + serialUrl + "\" title=\"" + info.ShowName + "\" target=\"_blank\">" + shortName + "</a>");
				// 				else
				htmlCode.AppendLine("<a href=\"" + serialUrl + "\" target=\"_blank\">" + info.ShowName + "</a>");
				htmlCode.AppendLine("<div class=\"view_model\"><a href=\"javascript:void(0);\" onclick=\"showCarInfo(this," + info.SerialId + "," + layerNum + ");\">查看车型</a></div>");
				htmlCode.AppendLine("</div>");

				//生成显示车型信息的代码
				if (counter % 6 == 0 || counter == serialList.Count)
				{
					GetSerialLayer(htmlCode, layerNum);
					layerNum++;
				}

				//生成每页的Html
				if (counter % 24 == 0 || counter == serialList.Count)
				{
					XmlElement pageEle = xmlDoc.CreateElement("Page");
					root.AppendChild(pageEle);
					pageEle.SetAttribute("pageNum", pageNum.ToString());
					XmlElement htmlEle = xmlDoc.CreateElement("HtmlCode");
					pageEle.AppendChild(htmlEle);
					htmlEle.InnerText = htmlCode.ToString();
					pageNum++;
					//清除数据
					htmlCode = new StringBuilder();
					layerNum = 1;

					foreach (XmlElement serialNode in tempList)
					{
						pageEle.AppendChild(serialNode);
					}
					tempList.Clear();
				}
			}
			pageNum -= 1;
			root.SetAttribute("page", pageNum.ToString());
			root.SetAttribute("serila", serialList.Count.ToString());
			root.SetAttribute("car", carNum.ToString());
			return xmlDoc;
		}
		/// <summary>
		/// 获取显示车型信息的代码
		/// </summary>
		/// <param name="htmlCode"></param>
		/// <param name="num"></param>
		private void GetSerialLayer(StringBuilder htmlCode, int num)
		{
			htmlCode.AppendLine("<div id=\"carInfoLayer" + num + "\" class=\"model_layer\">");
			htmlCode.AppendLine("<div class=\"view_terms\">");
			// 			htmlCode.AppendLine("<table>");
			// 			htmlCode.AppendLine("<col class=\"col1\" />");
			// 			htmlCode.AppendLine("<col class=\"col2\" />");
			// 			htmlCode.AppendLine("<col class=\"col3\" />");
			// 			htmlCode.AppendLine("<col class=\"col4\" />");
			// 			htmlCode.AppendLine("<col class=\"col5\" />");
			// 			htmlCode.AppendLine("<col class=\"col6\" />");
			// 			htmlCode.AppendLine("<col class=\"col7\" />");
			// 			htmlCode.AppendLine("<thead>");
			// 			htmlCode.AppendLine("<tr>");
			// 			htmlCode.AppendLine("<th>符合条件的车款</th>");
			// 			htmlCode.AppendLine("<th>热度</th>");
			// 			htmlCode.AppendLine("<th>排量</th>");
			// 			htmlCode.AppendLine("<th>变速箱</th>");
			// 			htmlCode.AppendLine("<th>油耗</th>");
			// 			htmlCode.AppendLine("<th>指导价</th>");
			// 			htmlCode.AppendLine("<th>售价</th>");
			// 			htmlCode.AppendLine("</tr>");
			// 			htmlCode.AppendLine("</thead>");
			// 			htmlCode.AppendLine("<tbody id=\"tbody" + num + "\">");
			// 			htmlCode.AppendLine("</tbody>");
			// 			htmlCode.AppendLine("</table>");
			htmlCode.AppendLine("</div>");
			htmlCode.AppendLine("</div>");
		}
	}

	public class SerialInfo
	{
		private int m_id;
		private string m_showName;
		private string m_allSpell;
		private string m_carIdList;
		private string m_serialImageUrl;
		private string m_priceRange;
		private double m_minPrice;
		private double m_minReferPrice;
		private int m_carNum;
		private int m_pvNum;

		public SerialInfo(int id, string showName, string spell)
		{
			m_id = id;
			m_showName = showName;
			m_allSpell = spell;
			m_carIdList = ",";
		}
		/// <summary>
		/// 子品牌广告链接地址
		/// </summary>
		public string ADSerialUrl { get; set; }

		public bool IsAdvertise { get; set; }

		public int SerialId
		{
			get { return m_id; }
			set { m_id = value; }
		}
		public string ShowName
		{
			get { return m_showName; }
			set { m_showName = value; }
		}
		public string AllSpell
		{
			get { return m_allSpell; }
			set { m_allSpell = value; }
		}
		public string CarIdList
		{
			get { return m_carIdList; }
			set { m_carIdList = value; }
		}

		public List<CarInfoForSerialSummaryEntity> CarList { get; set; }
		/// <summary>
		/// 符合条件的车的数量
		/// </summary>
		public int CarNum
		{
			get { return m_carNum; }
			set { m_carNum = value; }
		}
		public int PVNum
		{
			get { return m_pvNum; }
			set { m_pvNum = value; }
		}
		public string ImageUrl
		{
			get { return m_serialImageUrl; }
			set { m_serialImageUrl = value; }
		}
		public string PriceRange
		{
			get { return m_priceRange; }
			set { m_priceRange = value; }
		}

		/// <summary>
		/// 最低报价
		/// </summary>
		public double MinPrice
		{
			get { return m_minPrice; }
			set { m_minPrice = value; }
		}
		/// <summary>
		/// 最大报价
		/// </summary>
		public double MaxPrice { get; set; }

		/// <summary>
		/// 最低指导价（在选出来的车中的最低指导价）
		/// </summary>
		public double MinReferPrice
		{
			get { return m_minReferPrice; }
			set { m_minReferPrice = value; }
		}

		public static int CompareSerial(SerialInfo s1, SerialInfo s2)
		{
			if (s1.PVNum == s2.PVNum)
				return 0;
			else if (s1.PVNum > s2.m_pvNum)
				return 1;
			else
				return -1;
		}
		/// <summary>
		/// 按关注度从高到低排序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByPVDesc(SerialInfo s1, SerialInfo s2)
		{
			if (s1.PVNum == s2.PVNum)
				return 0;
			else if (s1.PVNum > s2.m_pvNum)
				return -1;
			else
				return 1;
		}

		/// <summary>
		/// 按最低报价从低到高排序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByMinPrice(SerialInfo s1, SerialInfo s2)
		{
			//暂无报价 永远排后
			if (s1.PriceRange == "暂无报价" || s2.PriceRange == "暂无报价")
			{
				if (s1.PriceRange == s2.PriceRange || s1.PriceRange == "暂无报价")
					return 1;
				else if (s2.PriceRange == "暂无报价")
					return -1;
			}
			if (s1.MinPrice == s2.MinPrice)
				return 0;
			else if (s1.MinPrice > s2.MinPrice)
				return 1;
			else
				return -1;
		}

		/// <summary>
		/// 按最低报价从高到低排序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByMinPriceDesc(SerialInfo s1, SerialInfo s2)
		{
			if (s1.MinPrice == s2.MinPrice)
				return 0;
			else if (s1.MinPrice > s2.MinPrice)
				return -1;
			else
				return 1;
		}
		/// <summary>
		/// 报价区间 按最大值 降序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByMaxPriceDesc(SerialInfo s1, SerialInfo s2)
		{
			if (s1.MaxPrice == s2.MaxPrice)
				return 0;
			else if (s1.MaxPrice > s2.MaxPrice)
				return -1;
			else
				return 1;
		}

		/// <summary>
		/// 按最低指导价从低到高排序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByMinReferPrice(SerialInfo s1, SerialInfo s2)
		{
			if (s1.MinReferPrice == s2.MinReferPrice)
				return 0;
			else if (s1.MinReferPrice > s2.MinReferPrice)
				return 1;
			else
				return -1;
		}

		/// <summary>
		/// 按最低指导价从高到低排序
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <returns></returns>
		public static int CompareSerialByMinReferPriceDesc(SerialInfo s1, SerialInfo s2)
		{
			if (s1.MinReferPrice == s2.MinReferPrice)
				return 0;
			else if (s1.MinReferPrice > s2.MinReferPrice)
				return -1;
			else
				return 1;
		}
	}


}
