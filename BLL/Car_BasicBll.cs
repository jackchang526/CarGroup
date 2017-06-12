using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;
using System.Linq;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace BitAuto.CarChannel.BLL
{
	public class Car_BasicBll
	{
		private static readonly Car_BasicDal cbd = new Car_BasicDal();
		private static readonly CarInfoDal cid = new CarInfoDal();

		public Car_BasicBll()
		{ }

		/// <summary>
		/// 取所有有效车型
		/// </summary>
		/// <returns></returns>
		public IList<Car_BasicEntity> Get_Car_BasicAll()
		{
			return cbd.Get_Car_BasicAll();
		}

		/// <summary>
		/// 取所有车型 包括无效的
		/// add by chengl Apr.10.2013
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllCarContainNoValid()
		{
			return cbd.GetAllCarContainNoValid();
		}

		/// <summary>
		/// 根据车型ID取车型基本表信息
		/// </summary>
		/// <param name="carid">车型ID</param>
		/// <returns></returns>
		public DataSet GetCarDetailById(int carid)
		{
			return cbd.GetCarDetailById(carid);
		}

		/// <summary>
		/// 取所有有效车型 根据子品牌ID、车型ID排序
		/// </summary>
		/// <returns></returns>
        public DataSet GetAllCarOrderbyCs(int serialId)
        {
            return cbd.GetAllCarOrderbyCs(serialId);
        }

		/// <summary>
		/// 根据车型ID取车型
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		public Car_BasicEntity Get_Car_BasicByCarID(int carID)
		{
			return cbd.Get_Car_BasicByCarID(carID);
		}

		/// <summary>
		/// 取子品牌下所有车型
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public IList<Car_BasicEntity> Get_Car_BasicByCsID(int csID)
		{
			return cbd.Get_Car_BasicByCsID(csID);
		}

		/// <summary>
		/// 获取所有在销与待销的车型数据
		/// </summary>
		/// <returns></returns>
		public DataSet GetCarDataForGoogle()
		{
			string cacheKey = "CarData_For_Google";
			DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
			if (ds == null)
			{
				ds = GetCarDataForGoogleFromDb();
				if (ds != null)
					CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
			}

			return ds;
		}

		/// <summary>
		/// 获取网友提交的油耗
		/// </summary>
		/// <param name="carId"></param>
		/// <returns></returns>
		public string GetCarNetfriendsFuel(int carId)
		{
			string fuelStr = "无";
			string cacheKey = "all_car_netfriend_fuel";
			Dictionary<int, string> fuelDic = (Dictionary<int, string>)CacheManager.GetCachedData(cacheKey);
			if (fuelDic == null)
			{
				string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data/Koubei/AllCarFuelV2.xml");
				try
				{
					fuelDic = new Dictionary<int, string>();
					XmlDocument fuelDoc = CommonFunction.ReadXmlFromFile(xmlFile);
					if (fuelDoc == null) return fuelStr;
					//XmlDocument fuelDoc = new XmlDocument();
					//fuelDoc.Load(xmlFile);
					XmlNodeList fuelList = fuelDoc.SelectNodes("//Trim");
					foreach (XmlElement fuelNode in fuelList)
					{
						int fCarId = Convert.ToInt32(fuelNode.GetAttribute("Id"));
						// 						double minFuel = Convert.ToDouble(fuelNode.GetAttribute("MinFuel"));
						// 						double maxFuel = Convert.ToDouble(fuelNode.GetAttribute("MaxFuel"));
						// 						if (minFuel == 0 && maxFuel == 0)
						// 							fuelDic[fCarId] = "无";
						// 						else
						// 							fuelDic[fCarId] = minFuel.ToString() + "L-" + maxFuel.ToString() + "L";
						double averageFuel = ConvertHelper.GetDouble(fuelNode.GetAttribute("UserAvgTrimFuel"));
						if (averageFuel == 0)
							fuelDic[fCarId] = "无";
						else
							fuelDic[fCarId] = averageFuel.ToString();
					}

					CacheManager.InsertCache(cacheKey, fuelDic, 60 * 24 * 7);
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog(ex.ToString());
				}
			}

			if (fuelDic.ContainsKey(carId))
				fuelStr = fuelDic[carId];

			return fuelStr;
		}

		private DataSet GetCarDataForGoogleFromDb()
		{
			DataSet baseDs = new Car_BasicDal().GetCarData();
			DataSet fuelDs = new PageBase().GetAllCarPerfFuelCostPer100();
			Dictionary<int, XmlElement> imgUrlDic = AutoStorageService.GetImageUrlDic();
			Dictionary<int, string> gotUrlDic = new Dictionary<int, string>();

			DataTable carDataTab = baseDs.Tables[0];
			carDataTab.Columns.Add("PriceUrl");
			carDataTab.Columns.Add("ImageUrl");
			carDataTab.Columns.Add("CarTitle");
			carDataTab.Columns.Add("CarInfo");
			DataTable fuelTable = fuelDs.Tables[0];
			foreach (DataRow row in baseDs.Tables[0].Rows)
			{
				int carId = Convert.ToInt32(row["CarId"]);
				int serialId = ConvertHelper.GetInteger(row["SerialId"]);
				string carName = ConvertHelper.GetString(row["CarName"]).Trim();
				string serialName = ConvertHelper.GetString(row["SerialShowName"]).Trim();
				string producerName = ConvertHelper.GetString(row["ProducerName"]).Trim();
				string carYear = ConvertHelper.GetString(row["CarYear"]).Trim();

				//生成车型名称与标题
				if (carName.StartsWith(serialName))
					carName = carName.Substring(serialName.Length);
				carName = serialName + " " + carName;
				if (carYear.Length > 0)
					carName += " " + carYear + "款";
				row["CarName"] = carName;
				row["CarTitle"] = producerName + " " + carName;

				//生成报价地址
				string serialSpell = ConvertHelper.GetString(row["SeialAllSpell"]);
				row["PriceUrl"] = "http://car.bitauto.com/" + serialSpell + "/m" + carId + "/baojia/";

				//生成图片地址
				if (gotUrlDic.ContainsKey(serialId))
					row["ImageUrl"] = gotUrlDic[serialId];
				else
				{
					string realUrl = WebConfig.DefaultCarPic;
					if (imgUrlDic.ContainsKey(serialId))
					{
						int imgId = Convert.ToInt32(imgUrlDic[serialId].GetAttribute("ImageId"));
						string imgUrl = imgUrlDic[serialId].GetAttribute("ImageUrl");
						if (imgId > 0 && imgUrl.Length > 0)
							realUrl = new OldPageBase().GetPublishImage(4, imgUrl, imgId);
					}
					gotUrlDic[serialId] = realUrl;			//存入临时字典
					row["ImageUrl"] = realUrl;
				}

				//生成信息说明
				//获取油耗
				string perFuel = "";
				DataRow[] rows = fuelTable.Select("carid=" + carId);
				if (rows.Length > 0)
				{
					DataRow tempRow = rows[0];
					perFuel = tempRow["pvalue"].ToString().Trim();
					if (perFuel.Length != 0)
						perFuel += "L";
				}

				//品牌名称
				string brandName = ConvertHelper.GetString(row["BrandName"]).Trim();
				string country = ConvertHelper.GetString(row["Cp_Country"]).Trim();
				if (country != "中国")
					row["BrandName"] = "进口" + brandName;

				string referPrice = ConvertHelper.GetString(row["ReferPrice"]).Trim();
				//string saleState = ConvertHelper.GetString(row["SaleState"]).Trim();
				string exhaust = ConvertHelper.GetString(row["Exhaust"]).Trim();
				string transmissionType = ConvertHelper.GetString(row["TransmissionType"]).Trim();
				string carLevel = ConvertHelper.GetString(row["Carlevel"]).Trim();
				string bodyType = ConvertHelper.GetString(row["BodyType"]).Trim();
				string repairPolicy = ConvertHelper.GetString(row["RepairPolicy"]).Trim();

				string carInfo = "生产厂家：" + producerName + "；";
				if (referPrice.Length > 0)
					carInfo += "厂家指导价：" + referPrice + "万；";
				if (exhaust.Length > 0)
					carInfo += "排量：" + exhaust + "；";
				if (carLevel.Length > 0)
					carInfo += "级别：" + carLevel + "；";
				if (transmissionType.Length > 0)
					carInfo += "变速器：" + transmissionType + "；";
				if (bodyType.Length > 0)
					carInfo += "厢式：" + bodyType + "；";
				if (repairPolicy.Length > 0)
					carInfo += "质保：" + repairPolicy + "；";
				if (perFuel.Length > 0)
					carInfo += "油耗：" + perFuel;
				else
					carInfo = carInfo.TrimEnd('；');

				row["CarInfo"] = carInfo;

				//厂商指导价单位改为元
				double refPrice = ConvertHelper.GetDouble(row["ReferPrice"]);
				row["ReferPrice"] = refPrice * 10000;
			}

			//删除无用列
			carDataTab.Columns.Remove("SaleState");
			carDataTab.Columns.Remove("SerialId");
			carDataTab.Columns.Remove("SerialName");
			carDataTab.Columns.Remove("SeialAllSpell");
			carDataTab.Columns.Remove("SerialShowName");
			carDataTab.Columns.Remove("ProducerName");
			carDataTab.Columns.Remove("CarYear");
			carDataTab.Columns.Remove("Cp_Country");
			//			carDataTab.Columns.Remove("ReferPrice");
			// 			carDataTab.Columns.Remove("Exhaust");
			// 			carDataTab.Columns.Remove("TransmissionType");
			// 			carDataTab.Columns.Remove("Carlevel");
			// 			carDataTab.Columns.Remove("BodyType");
			carDataTab.Columns.Remove("RepairPolicy");
			baseDs.AcceptChanges();

			return baseDs;
		}

		/// <summary>
		/// 取车型12张标准图 文件不更新
		/// </summary>
		/// <param name="csid"></param>
		/// <param name="carid"></param>
		/// <returns></returns>
		public XmlDocument GetCar12Photo(int csid, int carid)
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				//图库接口本地化更改 by sk 2012.12.21
				string xmlUrl = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarStandardImagePath);
				doc.Load(string.Format(xmlUrl, csid, carid));
				//doc.Load(string.Format(WebConfig.CarPhoto12ImageInterface, csid.ToString(), carid.ToString()));
			}
			catch (Exception ex)
			{ }
			return doc;
		}

		/// <summary>
		/// 取车型每个分类4张图片
		/// </summary>
		/// <param name="csid"></param>
		/// <param name="carid"></param>
		/// <returns></returns>
		public XmlDocument GetCarSummaryPhoto(int csid, int carid, int subfix)
		{
			XmlDocument doc = new XmlDocument();
			//图库接口本地化更改 by sk 2012.12.21
			string xmlPicPath = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialDefaultCarImagePath);
			if (File.Exists(string.Format(xmlPicPath, csid, carid)))
				doc.Load(string.Format(xmlPicPath, csid, carid));
			//doc.Load(WebConfig.PhotoCarInterface + "?dataname=caraccountbygroup&serialid=" + csid + "&carid=" + carid + "&showfullurl=true&subfix=" + subfix.ToString());
			return doc;
		}

		/// <summary>
		/// 取车型封面字典类型 加数据缓存
		/// </summary>
		/// <param name="subfix">图片规格</param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarDefaultPhotoDictionary(int subfix)
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();

			string cacheKey = "GetCarDefaultPhotoDictionary_" + subfix.ToString();
			object getCarDefaultPhotoDictionary = CacheManager.GetCachedData(cacheKey);
			if (getCarDefaultPhotoDictionary != null)
			{
				dic = (Dictionary<int, string>)getCarDefaultPhotoDictionary;
			}
			else
			{
				XmlDocument doc = GetCarDefaultPhoto();
				if (doc != null && doc.HasChildNodes)
				{
					XmlNodeList xnl = doc.SelectNodes("/ImageData/ImageList/ImageInfo");
					if (xnl != null && xnl.Count > 0)
					{
						foreach (XmlNode xn in xnl)
						{
							int carid = 0;
							if (int.TryParse(xn.Attributes["CarId"].Value.ToString(), out carid))
							{
								if (carid > 0)
								{
									if (!dic.ContainsKey(carid))
									{
										string imgUrl = xn.Attributes["ImageUrl"].Value;
										if (subfix != 2)
											imgUrl = imgUrl.Replace("_2.", string.Format("_{0}.", subfix));
										dic.Add(carid, imgUrl);
									}
								}
							}
						}
					}
				}
				CacheManager.InsertCache(cacheKey, dic, 60);
			}

			return dic;
		}

		/// <summary>
		/// 取车型的封面
		/// </summary>
		/// <param name="subfix">图片规格</param>
		/// <returns></returns>
		public XmlDocument GetCarDefaultPhoto()
		{
			XmlDocument doc = new XmlDocument();
			//图库接口本地化更改 by sk 2012.12.21
			string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarCoverImagePath);
			if (File.Exists(xmlFile))
				doc.Load(xmlFile);
			//doc.Load(WebConfig.PhotoSerialInterface + "?dataname=carcoverimage&showall=false&showfullurl=true&subfix=" + subfix.ToString());
			return doc;
		}

		/// <summary>
		/// 取车型3张标准图 modified by chengl Jun.13.2012 增加年款
		/// </summary>
		/// <param name="csID">子品牌ID</param>
		/// <param name="carID">车型ID</param>
		/// <returns></returns>
		public XmlDocument GetCarDefaultPhoto(int csID, int carID, int year)
		{
			XmlDocument doc = new XmlDocument();
			string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarFocusImagePath);
			xmlFile = string.Format(xmlFile, carID);
			if (File.Exists(xmlFile))
				doc.Load(xmlFile);
			return doc;
		}

		public Dictionary<int, string> GetCarPhotoCount()
		{
			var dic = new Dictionary<int, string>();
			const string cacheKey = "GetCarPhotoCountDictionary";
			object getCarDefaultPhotoDictionary = CacheManager.GetCachedData(cacheKey);
			if (getCarDefaultPhotoDictionary != null)
			{
				dic = (Dictionary<int, string>)getCarDefaultPhotoDictionary;
			}
			else
			{
				var doc = new XmlDocument();
				string xmlFile = Path.Combine(PhotoImageConfig.SavePath, "CarImageCount.xml");
				if (File.Exists(xmlFile))
				{
					doc.Load(xmlFile);
				}
				if (doc.HasChildNodes)
				{
					XmlNodeList xnl = doc.SelectNodes("/CarModels/Car");
					if (xnl != null && xnl.Count > 0)
					{
						foreach (XmlNode xn in xnl)
						{
							int carid = 0;
							if (int.TryParse(xn.Attributes["CarModelId"].Value, out carid))
							{
								if (carid > 0)
								{
									if (!dic.ContainsKey(carid))
									{
										dic.Add(carid, xn.Attributes["ImageCount"].Value.ToString());
									}
								}
							}
						}
					}
				}
				CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
			}
			return dic;
		}

		/// <summary>
		/// 获取车款口碑数据
		/// </summary>
		public XmlDocument GetCarKouBei(int carId)
		{
			//XmlDocument doc = null;
			//string cacheKey = "Car_BasicBll_GetCarKouBei_" + carId.ToString();
			//doc = (XmlDocument)CacheManager.GetCachedData(cacheKey);
			//if (doc == null)
			//{
				XmlDocument doc = new XmlDocument();
		        string xmlFile = Path.Combine(WebConfig.DataBlockPath,
		            string.Format(@"Data\Koubei\CarTopicList\{0}.xml", carId));
		        xmlFile = string.Format(xmlFile, carId);
		        if (File.Exists(xmlFile))
		            doc.Load(xmlFile);
              //  CacheManager.InsertCache(cacheKey, doc, WebConfig.CachedDuration);
		        //doc.Load(WebConfig.PhotoSerialInterface + "?dataname=serialfocusimage&serialid=" + csID + "&cId=" + carID + "&year=" + year + "&showall=false&showfullurl=true");
		    //}
		    return doc;
		}

		/// <summary>
		/// 取车型的对比列表
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		public DataSet GetCarCompareListByCarID(int carID)
		{
			string cacheKey = "Car_BasicBll_GetCarCompareListByCarID_" + carID.ToString();
			DataSet ds = (DataSet)CacheManager.GetCachedData(cacheKey);
			if (ds == null)
			{
				ds = cbd.GetCarCompareListByCarID(carID);
				CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
			}
			return ds;
		}

		/// <summary>
		/// 指定车型是否包含此扩展参数
		/// </summary>
		/// <param name="carid">车型ID</param>
		/// <param name="paramid">参数ID</param>
		/// <returns></returns>
		public bool CarHasParamEx(int carid, int paramid)
		{
			bool isHas = false;
			DataSet ds = new DataSet();
			string cacheKey = "GetCarParamEx_" + paramid.ToString();
			object carHasParamEx = CacheManager.GetCachedData(cacheKey);
			if (carHasParamEx != null)
			{
				ds = (DataSet)carHasParamEx;
			}
			else
			{
				ds = GetCarParamEx(paramid);
				CacheManager.InsertCache(cacheKey, ds, 60);
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select("car_id='" + carid.ToString() + "'");
				if (drs != null && drs.Length > 0)
				{
					if (drs[0]["pvalue"].ToString() == "3000元")
					{
						isHas = true;
					}
				}
			}
			return isHas;
		}

		/// <summary>
		/// 取某个车型的某个扩展参数
		/// </summary>
		/// <param name="carid"></param>
		/// <param name="paramid"></param>
		/// <returns></returns>
		public string GetCarParamEx(int carid, int paramid)
		{
			string carParamEx = "";
			DataSet ds = new DataSet();
			string cacheKey = "GetCarParamEx_" + paramid.ToString();
			object carHasParamEx = CacheManager.GetCachedData(cacheKey);
			if (carHasParamEx != null)
			{
				ds = (DataSet)carHasParamEx;
			}
			else
			{
				ds = GetCarParamEx(paramid);
				CacheManager.InsertCache(cacheKey, ds, 60);
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select("car_id='" + carid.ToString() + "'");
				if (drs != null && drs.Length > 0)
				{
					carParamEx = drs[0]["pvalue"].ToString().Trim();
				}
			}
			return carParamEx;
		}

		/// <summary>
		/// 取某个子品牌的某个扩展参数
		/// </summary>
		/// <param name="csid"></param>
		/// <param name="paramid"></param>
		/// <param name="isAllSale"></param>
		/// <param name="orderStr"></param>
		/// <returns></returns>
		public DataRow[] GetCarParamEx(int csid, int paramid, bool isAllSale, string orderStr)
		{
			DataRow[] drs = null;
			DataSet ds = new DataSet();
			string cacheKey = "GetCarParamEx_" + paramid.ToString();
			object carHasParamEx = CacheManager.GetCachedData(cacheKey);
			if (carHasParamEx != null)
			{
				ds = (DataSet)carHasParamEx;
			}
			else
			{
				ds = GetCarParamEx(paramid);
				CacheManager.InsertCache(cacheKey, ds, 60);
			}

			string saleStr = isAllSale ? "" : " and car_SaleState<>'停销' ";

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				if (orderStr != "")
				{ drs = ds.Tables[0].Select("cs_id='" + csid.ToString() + "'" + saleStr, orderStr); }
				else
				{ drs = ds.Tables[0].Select("cs_id='" + csid.ToString() + "'" + saleStr); }
			}
			return drs;
		}

		/// <summary>
		/// 根据参数ID取车型扩展参数
		/// </summary>
		/// <param name="paramid">参数ID</param>
		/// <returns></returns>
		public DataSet GetCarParamEx(int paramid)
		{
			return cbd.GetCarParamEx(paramid);
		}

		/// <summary>
		/// 根据参数ID取车型扩展参数 返回车型ID key 的字典
		/// </summary>
		/// <param name="paramid">参数ID</param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarParamExDic(int paramid)
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
			string cacheKey = "Car_BasicBll_GetCarParamExDic_" + paramid.ToString();
			object carHasParamEx = CacheManager.GetCachedData(cacheKey);
			if (carHasParamEx != null)
			{
				dic = (Dictionary<int, string>)carHasParamEx;
			}
			else
			{
				DataSet ds = cbd.GetCarParamEx(paramid);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int carid = int.Parse(dr["car_id"].ToString());
						string pvalue = dr["pvalue"].ToString().Trim();
						if (pvalue != "" && !dic.ContainsKey(carid))
						{ dic.Add(carid, pvalue); }
					}
				}
				CacheManager.InsertCache(cacheKey, dic, 60);
			}
			return dic;
		}

		/// <summary>
		///  根据参数ID取车型扩展参数 返回车型ID key 的字典 设定缓存时间
		/// </summary>
		/// <param name="paramid">参数ID</param>
		/// <param name="cacheInterval">缓存时间 分钟</param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarParamExDicByCacheInterval(int paramid, int cacheInterval)
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
			string cacheKey = "Car_BasicBll_GetCarParamExDicByCacheInterval_" + paramid.ToString();
			object carHasParamEx = CacheManager.GetCachedData(cacheKey);
			if (carHasParamEx != null)
			{
				dic = (Dictionary<int, string>)carHasParamEx;
			}
			else
			{
				DataSet ds = cbd.GetCarParamEx(paramid);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int carid = int.Parse(dr["car_id"].ToString());
						string pvalue = dr["pvalue"].ToString().Trim();
						if (pvalue != "" && !dic.ContainsKey(carid))
						{ dic.Add(carid, pvalue); }
					}
				}
				CacheManager.InsertCache(cacheKey, dic, cacheInterval);
			}
			return dic;
		}

		/// <summary>
		/// 得到CNCAP市区工况油耗； CNCAP市郊工况油耗；易车测试油耗的测试接口,对答疑
		/// </summary>
		/// <returns></returns>
		public DataSet GetOilMessageByAskInterface()
		{
			string cacheKey = "Car_BasicBll_GetOilMessageByAskInfo";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (DataSet)obj;

			DataSet ds = cbd.GetOilMessageByAskInterface();
			if (ds == null) return null;

			CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
			return ds;
		}

		/// <summary>
		/// 取车型全部参数项
		/// </summary>
		/// <param name="carID">车型ID</param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarAllParamByCarID(int carID)
		{
			return new Car_BasicDal().GetCarAllParamByCarID(carID);
		}
		/// <summary>
		/// 获取车款参数值
		/// </summary>
		/// <param name="carId"></param>
		/// <param name="paramId"></param>
		/// <returns></returns>
		public string GetCarParamValue(int carId, int paramId)
		{
			string result = string.Empty;
			if (carId <= 0 || paramId <= 0) return result;
			try
			{
				result = cbd.GetCarParamValue(carId, paramId);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return result;
		}
		/// <summary>
		/// 根据多个车款 参数值
		/// </summary>
		/// <param name="arrCarId">车款id数组</param>
		/// <param name="paramId"></param>
		/// <returns></returns>
		public Dictionary<int, string> GetCarParamValueByCarIds(int[] arrCarId, int paramId)
		{
			Dictionary<int, string> dictResult = new Dictionary<int, string>();
			if (arrCarId.Length <= 0 || paramId <= 0) return dictResult;
			try
			{
				dictResult = cbd.GetCarParamValueByCarIds(arrCarId, paramId);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return dictResult;

		}
		/// <summary>
		/// 取子品牌车型
		/// </summary>
		/// <param name="csid">子品牌ID 大于0为取特定子品牌 等于0为取全部子品牌</param>
		/// <returns></returns>
		public DataSet GetCarListGroupbyYear(int csid, bool isOnlySale)
		{
			return new Car_BasicDal().GetCarListGroupbyYear(csid, isOnlySale);
		}

		/// <summary>
		/// 取车型的全部城市经销商
		/// </summary>
		/// <param name="carID"></param>
		/// <returns></returns>
		public DataSet GetCarAllCityPriceDealerByCarID(int carID)
		{
			DataSet ds = new DataSet();
			string cacheName = "GetCarAllCityPriceDealer_" + carID.ToString();
			object carAllCityPriceDealer = null;
			CacheManager.GetCachedData(cacheName, out carAllCityPriceDealer);
			if (carAllCityPriceDealer != null)
			{
				ds = (DataSet)carAllCityPriceDealer;
			}
			else
			{
				com.bitauto.price.DealerPrice dp = new com.bitauto.price.DealerPrice();
				ds = dp.GetDealerPriceList(carID);
				CacheManager.InsertCache(cacheName, ds, 60);
			}
			return ds;
		}

		public List<DealerInfo> GetCarAllCityDealserFromMongoDB(int carId)
		{
			/*
			List<DealerInfo> dealerList = new List<DealerInfo>();
			 */
			string connectionString = WebConfig.MongoDBConnString;
			//string dbName = "DealerPrice";
			MongoServer server = MongoServer.Create(connectionString);
			//MongoCredentials credentials = new MongoCredentials("dealer", "dealer");
			//MongoDatabase database = server.GetDatabase(dbName, credentials);
			MongoDatabase database = server.GetDatabase("DealerPrice");
			/* 废弃 "dealerlist-" + carId 集合
			var dealers = database.GetCollection("dealerlist-" + carId);
			var items = dealers.FindAll().ToArray();
			foreach (BsonDocument d in items)
			{
				DealerInfo dealer = BsonSerializer.Deserialize(d, typeof(DealerInfo)) as DealerInfo;
				dealerList.Add(dealer);
			}
			*/

			var dealers = database.GetCollection("cardealerlist");
			if (dealers != null)
			{
				CarDealerList carDealerList = dealers.FindOneAs(typeof(CarDealerList), MongoDB.Driver.Builders.Query.EQ("CarId", carId)) as CarDealerList;
				if (carDealerList != null)
				{
					return carDealerList.Dealers;
				}
			}

			return new List<DealerInfo>();

		}

		#region 车型综述页 车型参数

		/// <summary>
		/// 取车型综述页参数配置
		/// </summary>
		/// <param name="carID">车型</param>
		/// <param name="name">显示的名字</param>
		/// <param name="allSpell">地址使用</param>
		/// <returns></returns>
		public string GetCarConfigurationForCarSummaey(int carID, string name, string allSpell)
		{
			string result = "";
			StringBuilder sbParameter = new StringBuilder(5000);
			StringBuilder sbConfiguration = new StringBuilder(5000);
			StringBuilder sbTemp = new StringBuilder(500);
			// StringBuilder sbForPage = new StringBuilder();
			// 车型XML
			// XmlDocument docCar = new XmlDocument();
			List<int> listValidCarID = new List<int>();
			listValidCarID.Add(carID);
			Dictionary<int, Dictionary<string, string>> dic = GetCarCompareDataByCarIDs(listValidCarID);
			if (!dic.ContainsKey(carID) || dic[carID].Count == 0)
			{ return ""; }
			else
			{
				XmlDocument docPC = new XmlDocument();
				string cache = "CarSummary_ParameterConfiguration";
				object parameterConfiguration = null;
				CacheManager.GetCachedData(cache, out parameterConfiguration);
				if (parameterConfiguration != null)
				{
					docPC = (XmlDocument)parameterConfiguration;
				}
				else
				{
					if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config"))
					{
						docPC.Load(System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfiguration.config");
						CacheManager.InsertCache(cache, docPC, 60);
					}
				}

				// 参数配置
				if (docPC != null && docPC.HasChildNodes)
				{
					XmlNode rootPC = docPC.DocumentElement;
					// 显示 参数
					if (docPC.ChildNodes.Count > 1)
					{
						sbParameter.AppendLine("<div id=\"DicCarParameter\" class=\"line_box car_config\">");
						sbParameter.AppendLine("<h3><span>" + name + " 参数</span><!--<i>注：●标配 ○选配 -无</i>--></h3>");
						sbParameter.AppendLine("<table cellspacing=\"0\" cellpadding=\"0\">");
						XmlNode parameter = rootPC.ChildNodes[0];
						foreach (XmlNode parameterList in parameter)
						{
							if (parameterList.NodeType == XmlNodeType.Element)
							{
								if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
								{
									sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
									bool isHasChild = false;
									int loopCount = 0;
									XmlNodeList xmlNode = parameterList.ChildNodes;
									foreach (XmlNode item in xmlNode)
									{
										if (item.NodeType != XmlNodeType.Element)
										{ continue; }
										if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
											&& dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")
										{
											isHasChild = true || isHasChild;
											if (loopCount % 2 == 0)
											{
												if (loopCount != 0)
												{
													sbTemp.AppendLine("</tr>");
												}
												sbTemp.AppendLine("<tr>");
											}
											string pvalue = dic[carID][item.Attributes.GetNamedItem("Value").Value] + item.Attributes.GetNamedItem("Unit").Value;
											// 牛B的逻辑不硬编码都不行
											// 燃料类型 汽油的话同时显示 燃油标号
											string pvalueOther;
											if (item.Attributes.GetNamedItem("ParamID").Value == "578"
												&& pvalue == "汽油")
											{
												if (dic[carID].ContainsKey("CarParams/Oil_FuelTab")
											&& dic[carID]["CarParams/Oil_FuelTab"] != "待查")
												{
													pvalueOther = dic[carID]["CarParams/Oil_FuelTab"];
													switch (pvalueOther)
													{
														case "90号": pvalueOther = pvalueOther + "(北京89号)"; break;
														case "93号": pvalueOther = pvalueOther + "(北京92号)"; break;
														case "97号": pvalueOther = pvalueOther + "(北京95号)"; break;
														default: break;
													}
													pvalue = pvalue + " " + pvalueOther;
												}
											}
											// 进气型式 如果自然吸气直接显示，如果是增压则显示增压方式
											if (item.Attributes.GetNamedItem("ParamID").Value == "425"
												&& pvalue == "增压")
											{
												if (dic[carID].ContainsKey("CarParams/Engine_AddPressType")
											&& dic[carID]["CarParams/Engine_AddPressType"] != "待查")
												{
													pvalueOther = dic[carID]["CarParams/Engine_AddPressType"];
													pvalue = pvalue + " " + pvalueOther;
												}
											}

											if (pvalue.IndexOf("有") == 0)
											{ pvalue = "●"; }
											if (pvalue.IndexOf("选配") == 0)
											{ pvalue = "○"; }
											if (pvalue.IndexOf("无") == 0)
											{ pvalue = "-"; }
											sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
											sbTemp.AppendLine("<td>" + pvalue + "</td>");
											loopCount++;
										}
									}
									if (loopCount % 2 == 1)
									{
										sbTemp.AppendLine("<th></th>");
										sbTemp.AppendLine("<td></td>");
									}
									// 如果有子项
									if (isHasChild)
									{
										sbParameter.AppendLine(sbTemp.ToString() + "</tr></tbody>");
									}
									if (sbTemp.Length > 0)
									{ sbTemp.Remove(0, sbTemp.Length); }
								}
							}
						}
						sbParameter.AppendLine("</table>");
						sbParameter.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">对比查看&gt;&gt;</a></div>");
						sbParameter.AppendLine("</div>");
					}

					// 显示配置
					if (docPC.ChildNodes.Count > 1)
					{
						sbConfiguration.Append("<div class=\"line_box car_config\">");
						sbConfiguration.Append("<h3><span>" + name + " 配置</span><i>注：●标配 ○选配 -无</i></h3>");
						sbConfiguration.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
						XmlNode parameter = rootPC.ChildNodes[1];
						foreach (XmlNode parameterList in parameter)
						{
							if (parameterList.NodeType == XmlNodeType.Element)
							{
								// string block = "";
								if (parameterList.HasChildNodes && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
								{
									sbTemp.AppendLine("<thead><tr><th colspan=\"4\">" + parameterList.Attributes.GetNamedItem("Name").Value + "</th></tr></thead><tbody>");
									bool isHasChild = false;
									int loopCount = 0;
									XmlNodeList xmlNode = parameterList.ChildNodes;
									foreach (XmlNode item in xmlNode)
									{
										if (item.NodeType != XmlNodeType.Element)
										{ continue; }
										if (dic[carID].ContainsKey(item.Attributes.GetNamedItem("Value").Value)
											&& dic[carID][item.Attributes.GetNamedItem("Value").Value] != "待查")
										{
											isHasChild = true || isHasChild;
											if (loopCount % 2 == 0)
											{
												if (loopCount != 0)
												{
													sbTemp.AppendLine("</tr>");
												}
												sbTemp.AppendLine("<tr>");
											}

											string pvalue = dic[carID][item.Attributes.GetNamedItem("Value").Value] + item.Attributes.GetNamedItem("Unit").Value;
											if (pvalue.IndexOf("有") == 0)
											{ pvalue = "●"; }
											if (pvalue.IndexOf("选配") == 0)
											{ pvalue = "○"; }
											if (pvalue.IndexOf("无") == 0)
											{ pvalue = "-"; }

											sbTemp.AppendLine("<th>" + item.Attributes.GetNamedItem("Name").Value + "</th>");
											// 车身颜色呈现特殊化
											if (item.Attributes.GetNamedItem("Name").Value == "车身颜色")
											{
												sbTemp.AppendLine("<td colspan=\"3\"><span class=\"c w530\"><!--车身颜色--></span></td>");
												loopCount++;
											}
											else
											{
												sbTemp.AppendLine("<td>" + pvalue + "</td>");
											}
											loopCount++;
										}
									}
									if (loopCount % 2 == 1)
									{
										sbTemp.AppendLine("<th></th>");
										sbTemp.AppendLine("<td></td>");
									}
									// 如果有子项
									if (isHasChild)
									{
										sbConfiguration.AppendLine(sbTemp.ToString() + "</tr></tbody>");
									}
									if (sbTemp.Length > 0)
									{ sbTemp.Remove(0, sbTemp.Length); }
								}
							}
						}
						sbConfiguration.AppendLine("</table>");
						sbConfiguration.AppendLine("<div class=\"more\"><a href=\"/" + allSpell + "/m" + carID.ToString() + "/peizhi/\" target=\"_blank\">对比查看&gt;&gt;</a></div>");
						sbConfiguration.AppendLine("</div>");
					}

					result = sbParameter.ToString() + sbConfiguration.ToString();
				}
			}
			return result;
		}

		#endregion

		#region 车型对比

		/// <summary>
		/// 取车型参数
		/// </summary>
		/// <param name="carIDs"></param>
		/// <returns></returns>
		public DataSet GetCarParamForCompare(string carIDs)
		{
			return new Car_BasicDal().GetCarParamForCompare(carIDs);
		}

		/// <summary>
		/// 取所有参数ID与英文名对于表
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllParamAliasName()
		{
			return new Car_BasicDal().GetAllParamAliasName();
		}

		/// <summary>
		/// 取所有参数ID与英文名对于表
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllParamAliasNameDictionary()
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
			DataSet ds = new DataSet();
			string cacheName = "GetAllParamAliasNameDictionary";
			object getAllParamAliasNameDictionary = null;
			CacheManager.GetCachedData(cacheName, out getAllParamAliasNameDictionary);
			if (getAllParamAliasNameDictionary != null)
			{
				dic = (Dictionary<int, string>)getAllParamAliasNameDictionary;
			}
			else
			{
				ds = GetAllParamAliasName();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int pid = int.Parse(dr["ParamId"].ToString());
						string aliasName = dr["AliasName"].ToString().Trim();
						if (!dic.ContainsKey(pid))
						{ dic.Add(pid, aliasName); }
					}
				}
				CacheManager.InsertCache(cacheName, dic, WebConfig.CachedDuration);
			}
			return dic;
		}

		/// <summary>
		/// 取车型基本信息
		/// </summary>
		/// <param name="carids"></param>
		/// <returns></returns>
		public DataSet GetCarBaseInfoForCompare(string carIDs)
		{
			return new Car_BasicDal().GetCarBaseInfoForCompare(carIDs);
		}

		/// <summary>
		/// 根据子品牌ID 取旗下车型及PV
		/// </summary>
		/// <param name="csIDs"></param>
		/// <returns></returns>
		public DataSet GetCarBaseInfoForCompareByCsIDs(string csIDs)
		{
			return new Car_BasicDal().GetCarBaseInfoForCompareByCsIDs(csIDs);
		}

		/// <summary>
		/// 根据车型ID列表取车型对比数据
		/// </summary>
		/// <param name="listCarID">车型ID列表</param>
		/// <returns></returns>
		public Dictionary<int, Dictionary<string, string>> GetCarCompareDataByCarIDs(List<int> listCarID)
		{
			Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
			if (listCarID.Count > 0)
			{
				string keyTemp = "Car_Dictionary_CarCompareData_{0}";
				IList<string> keyForMemCache = new List<string>();
				foreach (int carid in listCarID)
				{
					if (!keyForMemCache.Contains(string.Format(keyTemp, carid)))
					{ keyForMemCache.Add(string.Format(keyTemp, carid)); }
				}

				IDictionary<string, object> dicMemCache = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
				// Hashtable ht = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
				// 补齐没有memcache缓存的车型
				foreach (int carid in listCarID)
				{
					if (dicMemCache.Count > 0
						&& dicMemCache.ContainsKey(string.Format(keyTemp, carid))
						&& dicMemCache[string.Format(keyTemp, carid)] != null
						)
					{
						// 有memcache
						Dictionary<string, string> dicCar = dicMemCache[string.Format(keyTemp, carid)] as Dictionary<string, string>;
						if (dicCar != null && !dic.ContainsKey(carid))
						{ dic.Add(carid, dicCar); }
					}
					else
					{
						// modified Jan.13.2012 by chengl 当没有memcache时取数据重建memcache 缓存时间1天
						Dictionary<string, string> dicCar = new Dictionary<string, string>();
						GetCarInfoAndParamToDictionary(carid, ref dicCar);
						if (dicCar != null && dicCar.Count > 0)
						{
							//modified by sk mem 2小时
							MemCache.SetMemCacheByKey(string.Format(keyTemp, carid), dicCar, 1000 * 60 * 60 * 2);
						}
						if (!dic.ContainsKey(carid) && dicCar.Count > 0)
						{ dic.Add(carid, dicCar); }
					}
				}
			}
			return dic;
		}

        /// <summary>
        /// 车款参数json
        /// </summary>
        /// <returns></returns>
        public string GetValidCarJsObject(List<int> carIdList)
        {
            if (carIdList == null || carIdList.Count == 0) return string.Empty;
				
            StringBuilder sbForApi = new StringBuilder();
            Dictionary<int, Dictionary<string, string>> dicCarParam = GetCarCompareDataByCarIDs(carIdList);
            Dictionary<int, List<string>> dicTemp = new Common.PageBase().GetCarParameterJsonConfigNew();
            if (dicTemp != null && dicTemp.Count > 0)
            {
                int loopCar = 0;
                foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
                {
                    if (loopCar > 0)
                    { sbForApi.Append(","); }

                    sbForApi.Append("[");
                    // 循环模板
                    foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
                    {
                        if (kvpTemp.Key == 0)
                        {
                            // 基本数据
                            sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
                            sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
                            foreach (string param in kvpTemp.Value)
                            {
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append(",\"\""); }
                            }
                            sbForApi.Append("]");
                        }
                        else
                        {
                            // 扩展数据
                            sbForApi.Append(",[");
                            int loop = 0;
                            foreach (string param in kvpTemp.Value)
                            {
                                if (loop > 0)
                                { sbForApi.Append(","); }
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append("\"\""); }
                                loop++;
                            }
                            sbForApi.Append("]");
                        }
                    }
                    sbForApi.Append("]");

                    loopCar++;
                }
            }
            if (sbForApi.Length > 0)
            {
                sbForApi.Insert(0, "[");
                sbForApi.Append("];");
            }
            return sbForApi.ToString();
        }

		/// <summary>
		/// 取车型对比数据 字典
		/// </summary>
		/// <param name="carID"></param>
		/// <param name="dic"></param>
		private void GetCarInfoAndParamToDictionary(int carID, ref Dictionary<string, string> dic)
		{
			Dictionary<int, string> dicCarPhoto = GetCarDefaultPhotoDictionary(2);
			PageBase page = new PageBase();
			Dictionary<int, string> dicCsPhoto = page.GetAllSerialPicURL(false);
			Dictionary<int, string> dicCarPrice = page.GetAllCarPriceRange();
			// 车型行情价 add by chengl Aug.27.2012
			Dictionary<int, string> dicCarHangQingPrice = new HangQingTree().GetAllCarHangQingPrice();
			// 子品牌车身颜色RGB
			Dictionary<int, Dictionary<string, string>> dicSerialColor = new Car_SerialBll().GetAllSerialColorNameRGB();
			// 车型降价
			Dictionary<int, string> dicJiangJia = new CarNewsBll().GetAllCarJiangJia();

			#region 车型基本参数
			CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
			if (ce == null || ce.Id <= 0)
			{ return; }

			string carReferPrice = ce.ReferPrice <= 0 ? "无" : (decimal.Parse(ce.ReferPrice.ToString())).ToString("F2") + "万";
			string carYearType = ce.CarYear <= 0 ? "" : ce.CarYear.ToString();
			string bbsURL = new Car_SerialBll().GetForumUrlBySerialId(ce.SerialId);
			// 车型网友油耗
			string userFuel = new Car_BasicBll().GetCarNetfriendsFuel(carID);
			userFuel = (userFuel == "无" ? "" : userFuel);
			// 车型报价区间
			string carPriceRange = dicCarPrice.ContainsKey(carID) ? dicCarPrice[carID] : "无";
			// 车型图片 先检查车型是否有封面，再检查子品牌封面
			string carPic = WebConfig.DefaultCarPic;
			if (dicCarPhoto.ContainsKey(carID))
			{ carPic = dicCarPhoto[carID]; }
			else if (dicCsPhoto.ContainsKey(ce.SerialId))
			{ carPic = dicCsPhoto[ce.SerialId]; }
			else
			{ carPic = WebConfig.DefaultCarPic; }
			// 车型行情价
			string carHangQingPrice = "";
			if (dicCarHangQingPrice.ContainsKey(carID))
			{ carHangQingPrice = dicCarHangQingPrice[carID]; }
			// add by chengl Mar.25.2013
			string carJiangJiaPrice = "";
			if (dicJiangJia.ContainsKey(carID))
			{ carJiangJiaPrice = dicJiangJia[carID]; }

			dic.Add("Car_ID", carID.ToString());
			dic.Add("Car_Name", ce.Name);
			dic.Add("CarImg", carPic);
			dic.Add("Cs_ID", ce.SerialId.ToString());
			dic.Add("Cs_Name", ce.Serial == null ? "" : ce.Serial.Name);
			dic.Add("Cs_ShowName", ce.Serial == null ? "" : ce.Serial.ShowName);
			dic.Add("Cs_AllSpell", ce.Serial == null ? "" : ce.Serial.AllSpell);
			dic.Add("Car_YearType", ce.CarYear.ToString());
			dic.Add("Car_ProduceState", ce.ProduceState);
			dic.Add("Car_SaleState", ce.SaleState);
			dic.Add("CarReferPrice", carReferPrice);
			dic.Add("AveragePrice", carPriceRange);
			dic.Add("Car_UserFuel", userFuel);
			dic.Add("Cs_BBSUrl", bbsURL);
			dic.Add("Cs_CarLevel", (ce.Serial == null || ce.Serial.Level == null) ? "" : ce.Serial.Level.Name);
			// 车型行情价
			dic.Add("Car_HangQingPrice", carHangQingPrice);
			dic.Add("Car_JiangJiaPrice", carJiangJiaPrice);
			#endregion

			// 车型车身颜色中文名
			string bodyColor = string.Empty;

			#region 车型扩展参数
			// 参数ID 对于 名
			Dictionary<int, string> dicParamIDToName = GetAllParamAliasNameDictionary();

			// 车型扩展参数
			DataSet dsParam = new Car_BasicBll().GetCarParamForCompare(carID.ToString());
			if (dsParam != null && dsParam.Tables.Count > 0 && dsParam.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsParam.Tables[0].Rows)
				{
					int carid = Convert.ToInt32(dr["CarId"]);
					int pid = Convert.ToInt32(dr["Paramid"]);
					string aliasName = string.Empty;
					if (dicParamIDToName.ContainsKey(pid))
					{ aliasName = dicParamIDToName[pid]; }
					else { continue; }
					string pvalue = dr["Pvalue"].ToString().Trim();

					if (pvalue == "")
					{ continue; }

					if (!dic.ContainsKey(aliasName))
					{
						dic.Add(aliasName, pvalue);
					}
					// 如果是车身颜色
					if (aliasName == "OutStat_BodyColor")
					{ bodyColor = pvalue; }
				}
			}
			#endregion

			#region 车型车身颜色RGB值

			List<string> listBodyColorRGB = new List<string>();
			if (!string.IsNullOrEmpty(bodyColor))
			{
				if (dicSerialColor.ContainsKey(ce.SerialId))
				{
					// 临时车型参数颜色名
					List<string> listTemp = new List<string>();
					string[] colorNameArray = bodyColor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					if (colorNameArray.Length > 0)
					{
						foreach (string name in colorNameArray)
						{
							string colorName = name.Trim();
							if (colorName != "" && !listTemp.Contains(colorName))
							{
								listTemp.Add(colorName);
							}
						}
					}
					if (listTemp.Count > 0)
					{
						foreach (KeyValuePair<string, string> kvp in dicSerialColor[ce.SerialId])
						{
							if (listTemp.Contains(kvp.Key))
							{
								if (listBodyColorRGB.Count > 0)
								{ listBodyColorRGB.Add("|"); }
								listBodyColorRGB.Add(kvp.Key + "," + kvp.Value);
							}
						}
					}
				}
			}

			dic.Add("Car_OutStat_BodyColorRGB", string.Concat(listBodyColorRGB.ToArray()));

			#endregion

		}

		#endregion

		#region 二手车

		/// <summary>
		/// 取所有车型的二手车报价
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, string> GetAllUcarPrice()
		{
			Dictionary<int, string> dic = new Dictionary<int, string>();
			string cacheName = "Car_BasicBll_GetAllUcarPrice";
			object getAllUcarPrice = null;
			CacheManager.GetCachedData(cacheName, out getAllUcarPrice);
			if (getAllUcarPrice != null)
			{
				dic = (Dictionary<int, string>)getAllUcarPrice;
			}
			else
			{
				string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\UsedCarInfo\\AllUCarPrice.Xml");
				if (File.Exists(xmlFile))
				{
					using (XmlReader xmlReader = XmlReader.Create(xmlFile))
					{
						// modified by chengl Mar.26.2013 fix bug when file empty
						try
						{
							while (xmlReader.ReadToFollowing("ds"))
							{
								XmlReader inner = xmlReader.ReadSubtree();
								int carid = 0;
								decimal minP = 0;
								decimal maxP = 0;
								while (!inner.EOF)
								{
									if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "MinPrice")
									{
										decimal.TryParse(inner.ReadString(), out minP);
										if (minP > 0)
										{ minP = Math.Round(minP, 2); }
										if (minP > 100)
										{
											minP = Math.Round(minP, 0);
										}
									}
									if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "MaxPrice")
									{
										decimal.TryParse(inner.ReadString(), out maxP);
										if (maxP > 0)
										{ maxP = Math.Round(maxP, 2); }
										if (maxP > 100)
										{
											maxP = Math.Round(maxP, 0);
										}
									}
									if (inner.NodeType == XmlNodeType.Element && inner.LocalName == "CarId")
									{
										int.TryParse(inner.ReadString(), out carid);
									}
									inner.Read();
								}
								if (!dic.ContainsKey(carid) && carid > 0 && (minP > 0 || maxP > 0))
								{
									string ucarPrice = "";
									if (minP > 0 && maxP > 0)
									{
										ucarPrice = minP.ToString() + "-" + maxP.ToString() + "万";
									}
									else
									{
										ucarPrice = maxP > minP ? maxP.ToString() + "万" : minP + "万";
									}
									dic.Add(carid, ucarPrice);
								}
							}
						}
						catch
						{ }
					}
				}
				CacheManager.InsertCache(cacheName, dic, WebConfig.CachedDuration);
			}
			return dic;
		}

		#endregion

		/// <summary>
		/// 获取车款列表 根据子品牌Id
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<CarInfoForSerialSummaryEntity> GetCarInfoForSerialSummaryBySerialId(int serialId)
		{
			string cacheKey = string.Format("Car_CarInfoForSerialSummary_{0}", serialId);
			object allCarInfoForSerialSummary = CacheManager.GetCachedData(cacheKey);
			if (allCarInfoForSerialSummary != null)
				return (List<CarInfoForSerialSummaryEntity>)allCarInfoForSerialSummary;
			List<CarInfoForSerialSummaryEntity> carInfoList = new List<CarInfoForSerialSummaryEntity>();
			DataSet ds = cbd.GetAllCarInfoForSerialSummary(serialId);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				Dictionary<int, string> dictCarPriceRange = new PageBase().GetAllCarPriceRange();
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(dr["car_id"]);
					Dictionary<int, string> dictParams = GetCarAllParamByCarID(carId);
					string saleState = dr["Car_SaleState"].ToString().Trim();
					string carPriceRange = string.Empty;
					if (saleState == "停销")
						carPriceRange = "停售";
					else
						carPriceRange = dictCarPriceRange.ContainsKey(carId) ? dictCarPriceRange[carId] : "";
					//modified by sk 2013.08.07 进气形式为null/待查/自然吸气的，视为一种分类
					string inhaleType = string.Empty;
					if (dictParams.ContainsKey(425))
					{
						if (dictParams[425] == "" || dictParams[425] == "待查" || dictParams[425] == "自然吸气") { }
						else
							inhaleType = dictParams[425];
					}
					//add by sk 2014.3.31 增压方式
					string addPressType = string.Empty;
					if (dictParams.ContainsKey(408))
					{
						if (dictParams[408] == "" || dictParams[408] == "待查" || dictParams[408] == "无") { }
						else
							addPressType = dictParams[408];
					}
					////马力优先 马力参数 如果没有值 利用千万时计算马力 再没有排最后
					//int maxPower = 0;
					//if (dictParams.ContainsKey(791))
					//{
					//    maxPower = ConvertHelper.GetInteger(dictParams[791]);
					//}
					//if (maxPower == 0)
					//{
					//    if (dictParams.ContainsKey(430))
					//        maxPower = (int)(Convert.ToDouble(dictParams[430]) * 1.36);
					//}
					//maxPower = maxPower == 0 ? 9999 : maxPower;//最大马力 为0 排序用到 在同组排量后面
					var fuelType = dictParams.ContainsKey(578) ? dictParams[578] : string.Empty;
					int kw = 0;
					int electrickW = 0;
					if (fuelType == "电力")
					{
						kw = dictParams.ContainsKey(870) ? ConvertHelper.GetInteger(dictParams[870]) : 0;
					}
					else if (fuelType == "油电混合动力")
					{
						double tempDiankW;
						if (dictParams.ContainsKey(870) && double.TryParse(dictParams[870], out tempDiankW))
						{
							electrickW = Convert.ToInt32(tempDiankW);
						}

						double tempYoukW;
						if (dictParams.ContainsKey(430) && double.TryParse(dictParams[430], out tempYoukW))
						{
							kw = ConvertHelper.GetInteger(tempYoukW);
						}

						//double tempYoukW;
						//double.TryParse(dictParams[430], out tempYoukW);

						//int diankW = dictParams.ContainsKey(870) ? ConvertHelper.GetInteger(tempDiankW) : 0;
						//int youkW = dictParams.ContainsKey(430) ? ConvertHelper.GetInteger(tempYoukW) : 0;
						//kw = youkW;
						//electrickW = diankW;
					}
					else
					{
						if (dictParams.ContainsKey(430))
						{
							double tempkW;
							double.TryParse(dictParams[430], out tempkW);
							kw = (int)Math.Round(tempkW);
						}
					}
					kw = kw == 0 ? 9999 : kw;
					string exhaust = dr["Engine_Exhaust"].ToString();
					if (string.IsNullOrEmpty(exhaust) || ConvertHelper.GetDouble(exhaust.Replace("L", "")) <= 0)
					{
						exhaust = "其他";
						if (fuelType == "电力")
							exhaust = "电动车";
					}
					//是否是平行进口
					int isImport = (dictParams.ContainsKey(382) && dictParams[382] == "平行进口") ? 1 : 0;

					carInfoList.Add(new CarInfoForSerialSummaryEntity()
					{
						CarID = carId,
						CarName = dr["car_name"].ToString(),
						SaleState = saleState,
						CarPriceRange = carPriceRange,
						CarPV = dr["Pv_SumNum"].ToString() == "" ? 0 : int.Parse(dr["Pv_SumNum"].ToString()),
						ReferPrice = dr["car_ReferPrice"].ToString(),
						TransmissionType = dr["UnderPan_TransmissionType"].ToString(),//变速箱
						Engine_Exhaust = exhaust,//排量
						CarYear = dr["Car_YearType"] == DBNull.Value ? "" : dr["Car_YearType"].ToString(),
						ProduceState = dr["Car_ProduceState"].ToString(),
						UnderPan_ForwardGearNum = dictParams.ContainsKey(724) ? dictParams[724] : "",//档位个数
						Engine_MaxPower = kw,//dictParams.ContainsKey(430) ? (int)(Convert.ToDouble(dictParams[430]) * 1.36) : 0,//最大马力
						Electric_Peakpower = electrickW,
						Engine_InhaleType = inhaleType,//进气型式
						Engine_AddPressType = addPressType,//增压方式
						Oil_FuelType = dictParams.ContainsKey(578) ? dictParams[578] : "",//燃料类型
						IsImport = isImport
					});
				}
			}
			CacheManager.InsertCache(cacheKey, carInfoList, 30);
			return carInfoList;
		}

		/// <summary>
		/// 获取同级别热门车款 （不包含某子品牌）
		/// </summary>
		/// <param name="carLevel"></param>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public DataSet GetHotCarForCompare(string carLevel, int serialId)
		{
			return cbd.GetHotCarForCompare(carLevel, serialId);
		}

		/// <summary>
		/// 获取子品牌 下 热门车型 最新年款 最热门车
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public Dictionary<int, int> GetHotCarForPhotoCompareBySerialId(IEnumerable<int> serialIdArray)
		{
			var dict = new CommonService().GetPhotoCompareSerialAndCarList();
			Dictionary<int, int> dictSerialCarId = new Dictionary<int, int>();
			int carId = 0;
			foreach (var serialId in serialIdArray)
			{
				if (dict.ContainsKey(serialId))
				{
					List<int> carList = dict[serialId];
					DataTable dt = cbd.GetHotCarBySerialId(serialId).Tables[0];
					foreach (DataRow dr in dt.Rows)
					{
						carId = ConvertHelper.GetInteger(dr["car_id"]);
						if (carList.Contains(carId))
						{
							dictSerialCarId.Add(serialId, carId);
							break;
						}
					}
				}
			}
			return dictSerialCarId;
		}

		/// <summary>
		/// 获取子品牌 下 热门车型 最新年款 最热门车
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public int GetHotCarForPhotoCompareBySerialId(int serialId)
		{
			int carId = 0;
			var dict = new CommonService().GetPhotoCompareSerialAndCarList();
			if (!dict.ContainsKey(serialId)) return carId;
			List<int> carList = dict[serialId];
			DataTable dt = cbd.GetHotCarBySerialId(serialId).Tables[0];
			foreach (DataRow dr in dt.Rows)
			{
				var tempCarId = ConvertHelper.GetInteger(dr["car_id"]);
				if (carList.Contains(tempCarId))
				{
					carId = tempCarId;
					break;
				}
			}
			return carId;
		}

		public DataSet GetCarBaseDataBySerialId(int serialId, bool isAll = false)
		{
			return cbd.GetCarBaseDataBySerialId(serialId, isAll);
		}

		/// <summary>
		/// 获取 子品牌 车款 List 已排序
		/// 排序：年款 排量 变速箱 指导价
		/// 在售车系：提取全部未上市+在售车款。
		/// 停售车系：提取全部车款。
		/// 未上市车系：提取全部车款。
		/// 销售状态待查车系：提取全部车款
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="isZaiShou"></param>
		public List<CarInfoForSerialSummaryEntity> GetCarBaseListBySerialId(int serialId, bool isAll = false)
		{
			string cacheKey = "Car_BasicBll_GetCarBaseDataBySerialId_" + serialId + "_" + (isAll ? 1 : 0);
			var carList = CacheManager.GetCachedData(cacheKey);
			if (carList != null)
				return (List<CarInfoForSerialSummaryEntity>)carList;

			DataSet ds = cbd.GetCarBaseDataBySerialId(serialId, isAll);
			var carListTemp = new List<CarInfoForSerialSummaryEntity>();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(dr["car_id"]);
					carListTemp.Add(new CarInfoForSerialSummaryEntity()
					{
						CarID = carId,
						CarName = ConvertHelper.GetString(dr["Car_Name"]),
						ReferPrice = ConvertHelper.GetString(dr["car_ReferPrice"]),
						CarYear = ConvertHelper.GetString(dr["Car_YearType"]),
						Engine_Exhaust = ConvertHelper.GetString(dr["Engine_Exhaust"]),
						TransmissionType = ConvertHelper.GetString(dr["UnderPan_TransmissionType"])
					});
				}
				carListTemp.Sort(NodeCompare.CompareCarByYear);
				CacheManager.InsertCache(cacheKey, carListTemp, WebConfig.CachedDuration);
			}
			return carListTemp;
		}

		public DataSet GetAllCarInfoForSerialSummary(int serialId)
		{
			string cacheKey = "Car_BasicBll_GetAllCarInfoForSerialSummary_" + serialId;
			var carDataSet = CacheManager.GetCachedData(cacheKey);
			if (carDataSet != null)
			{
				return (DataSet)carDataSet;
			}
			DataSet ds = cbd.GetAllCarInfoForSerialSummary(serialId);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				CacheManager.InsertCache(cacheKey, ds, 30);
			}
			return ds;
		}
		#region CarInfoDal

		/// <summary>
		/// 取有效车型
		/// </summary>
		/// <returns></returns>
		public DataSet GetCarInfoByParams()
		{
			return cid.GetCarInfoByParams();
		}

		public DataSet GetCarPVData(int serialId)
		{
			try
			{
				if (serialId > 0)
					return cid.GetCarPVDataBySerialId(serialId);
				else
					return cid.GetCarPVData();
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
				return null;
			}
		}
		#endregion
	}
}
