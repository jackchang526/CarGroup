using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL
{
	/// <summary>
	/// 第四级
	/// </summary>
	public class SerialFourthStageBll
	{
		private Car_SerialBll _serialBLL;
		protected bool isExistCarList = true;//是否存在车型
		protected bool isElectrombile = false;//是否是全系电动车
		private Dictionary<int, string> dictSerialBlockHtml;//静态块内容

		public SerialFourthStageBll()
		{
			_serialBLL = new Car_SerialBll();
		}

		/// <summary>
		/// 取所以有H5页面的子品牌ID集合
		/// add by chengl 2015-6-26
		/// </summary>
		/// <returns></returns>
		public List<int> GetAllSerialInH5()
		{
			List<int> listCsID = new List<int>();
			string cacheKey = "SerialFourthStageBll_GetAllSerialInH5";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{ listCsID = (List<int>)obj; }
			else
			{
				try
				{
					string filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\H5SelectSerialList.xml";
					if (File.Exists(filePath))
					{
						XmlDocument xmlDoc = new XmlDocument();
						xmlDoc.Load(filePath);
						XmlNodeList entries = xmlDoc.SelectNodes("/Group/Item");
						if (entries != null && entries.Count > 0)
						{
							foreach (XmlNode xn in entries)
							{
								int csid = 0;
								if (xn.Attributes["CsId"] != null
									&& xn.Attributes["CsId"].Value != ""
									&& int.TryParse(xn.Attributes["CsId"].Value, out csid))
								{ }
								if (csid > 0 && !listCsID.Contains(csid))
								{
									listCsID.Add(csid);
								}
							}
						}
						CacheManager.InsertCache(cacheKey, listCsID, WebConfig.CachedDuration);
					}
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog(ex.ToString());
				}
			}
			return listCsID;
		}

		/// <summary>
		/// 根据销售状态和级别判断是否有第四极地址
		/// 目前在销、代销、非概念车有第四极地址
		/// </summary>
		/// <param name="level"></param>
		/// <param name="sale"></param>
		/// <returns></returns>
		public bool HasH5ByCsInfo(string level, string sale)
		{
			bool hasH5 = false;
			if (!string.IsNullOrEmpty(level) && level!="概念车")
			{
				if (!string.IsNullOrEmpty(sale) && (sale == "在销" || sale == "待销"))
				{
					hasH5 = true;
				}
			}
			return hasH5;
		}

		/// <summary>
		/// 获取官方颜色图
		/// </summary>
		public List<SerialColorForSummaryEntity> GetSerialColorList(int serialId)
		{
			var result = new List<SerialColorForSummaryEntity>();
			EnumCollection.SerialInfoCard serialInfo = _serialBLL.GetSerialInfoCard(serialId);
			//modified by sk 停销 未上市 取最新年款下的 子品牌颜色图
			List<SerialColorEntity> serialColorList;
			if (serialInfo.CsSaleState == "停销")
			{
				serialColorList = _serialBLL.GetNoSaleSerialColors(serialId, serialInfo.ColorList);
			}
			else
			{
				serialColorList = _serialBLL.GetProduceSerialColors(serialId);
			}
			List<SerialColorForSummaryEntity> colorList = _serialBLL.GetSerialColorRGBByCsID(serialId, 0, serialColorList);
			//排序 有图在前 无图在后 颜色 按色值大小从大到小排序
			colorList.Sort(NodeCompare.SerialColorCompare);
			int count = 0;
			foreach (SerialColorForSummaryEntity color in colorList)
			{
				if (count > 18)
				{
					break;
				}
				string imageUrl = color.ImageUrl.Replace("_5.", "_8.");
				color.ImageUrl = imageUrl;
				if (!string.IsNullOrEmpty(imageUrl))
				{
					result.Add(color);
					count++;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取外观，内饰设计
		/// </summary>
		/// <param name="typeId"></param>
		/// <returns></returns>
		public List<SerialFourthStage> GetExteriorInteriorList(int serialId, int typeId)
		{
			var result = new List<SerialFourthStage>();
			var ds = new SerialFourthStageDal().GetSerialExteriorInteriorType(serialId, typeId);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					SerialFourthStage Obj = new SerialFourthStage();
					Obj.SerialId = ((row["SerialId"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["SerialId"]);
					Obj.Title = row["Title"].ToString().Trim();
					Obj.Description = row["Discription"].ToString().Trim();
					Obj.ImageUrl = row["ImageUrl"].ToString().Trim();
					Obj.OrderId = ((row["OrderWeight"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["OrderWeight"]);
					Obj.TypeId = ((row["Type"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["Type"]);
					result.Add(Obj);
				}
			}
			return result;
		}

		/// <summary>
		/// 获取图集列表页
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="typeId">6 外观 7 内饰</param>
		/// <returns></returns>
		public List<SerialFocusImage> GetPhtotoAtlas(int serialId, int typeId)
		{
			var result = new List<SerialFocusImage>();
			XmlDocument docPC = new XmlDocument();
			string cache = "SerialPhtotoAtlas_all_" + serialId;
			object serialPhoto = null;
			CacheManager.GetCachedData(cache, out serialPhoto);
			if (serialPhoto != null)
			{
				docPC = (XmlDocument)serialPhoto;
			}
			else
			{
				var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\PhotoImage\\SerialOfficalImage\\{0}.xml", serialId.ToString()));
				if (File.Exists(filePath))
				{
					docPC.Load(filePath);
					CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
				}
			}
			if (docPC != null && docPC.HasChildNodes)
			{
				XmlNode rootPC = docPC.DocumentElement;
				if (rootPC != null && rootPC.ChildNodes.Count > 0)
				{
					foreach (XmlNode xng in rootPC.ChildNodes)
					{
						if (xng.NodeType != XmlNodeType.Element)
						{ continue; }
						// 大类外观 内饰 
						if (xng != null && xng.ChildNodes.Count > 0 && Convert.ToInt32(xng.Attributes["ID"].Value) == typeId)
						{
							foreach (XmlNode xn in xng.ChildNodes)
							{
								if (xn.NodeType != XmlNodeType.Element)
								{ continue; }

								int imgId = 0;
								if (xn.Attributes["ImageID"] != null
									&& xn.Attributes["ImageID"].Value != ""
									&& int.TryParse(xn.Attributes["ImageID"].Value, out imgId))
								{ }
								if (imgId > 0 && !string.IsNullOrEmpty(xn.Attributes["ImageUrl"].Value))
								{
									var img = new SerialFocusImage();
									img.ImageId = imgId;
									img.ImageName = xn.Attributes["ImageName"].Value;
									img.ImageUrl = xn.Attributes["ImageUrl"].Value;
									result.Add(img);
								}
							}
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 评测、导购文章，要求易车原创
		/// </summary>
		/// <param name="serialId"></param>
		/// <param name="top"></param>
		/// <returns></returns>
		public List<News> GetSerialNewsWithCreative(int serialId, int top)
		{
			List<News> result = null;
			if (serialId <= 0)
			{
				return null;
			}
			if (top < 0)
			{
				top = 4;
			}

			List<int> carTypeIdList = new List<int>() 
			{
			    (int)CarNewsType.daogou,
                (int)CarNewsType.treepingce
			};
			DataSet newsDs = new SerialFourthStageDal().GetSerialNewsWithCreative(serialId, carTypeIdList, top);
			if (newsDs != null && newsDs.Tables.Count > 0 && newsDs.Tables[0].Rows.Count > 0)
			{
				result = new List<News>(newsDs.Tables[0].Rows.Count);
				foreach (DataRow row in newsDs.Tables[0].Rows)
				{
					string picUrl = ConvertHelper.GetString(row["Picture"]);
					string firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
					string pic = string.Empty;
					if (!string.IsNullOrEmpty(picUrl) && !picUrl.Contains("/not"))
					{
						pic = picUrl;
					}
					else if (!string.IsNullOrEmpty(firstPicUrl))
					{
						pic = firstPicUrl.Replace("/bitauto/", "/newsimg_300_w0_1/bitauto/")
							.Replace("/autoalbum/", "/newsimg_300_w0_1/autoalbum/");
					}
					else
					{
						pic = WebConfig.DefaultCarPic;
					}
					string newsUrl = Convert.ToString(row["filepath"]).Replace("news.bitauto.com", "news.h5.yiche.com");
					result.Add(new News()
					{
						NewsId = ConvertHelper.GetInteger(row["CmsNewsId"]),
						Title = Common.CommonFunction.NewsTitleDecode(row["title"].ToString()),
						PageUrl = newsUrl,
						PublishTime = Convert.ToDateTime(row["PublishTime"]),
						Author = Convert.ToString(row["author"]),
						CarImage = pic
					});
				}
			}
			return result;
		}

		/// <summary>
		/// 精选编辑口碑
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public string MakeEditorCommentHtml(int serialId)
		{
			var result = string.Empty;
			result = new CommonHtmlBll().GetCommonHtmlByBlockId(serialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.H5SerialSummary, CommonHtmlEnum.BlockIdEnum.EditorComment);
			return result;
		}

		/// <summary>
		/// 网友口碑点评精选
		/// </summary>
		/// <param name="serialId"></param>
		public string MakeKoubeiDianpingHtml(int serialId)
		{
			var result = string.Empty;
			//静态块内容
			dictSerialBlockHtml = new CommonHtmlBll().GetCommonHtml(serialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.H5SerialSummary);
			int koubei = (int)CommonHtmlEnum.BlockIdEnum.KoubeiReportNew;
			if (dictSerialBlockHtml.ContainsKey(koubei))
			{
				result = dictSerialBlockHtml[koubei];
			}
			return result;
		}

		/// <summary>
		/// 车款列表报价
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<CarInfoForSerialSummaryEntity> MakeCarList(int serialId)
		{
			var result = new List<CarInfoForSerialSummaryEntity>();
			List<CarInfoForSerialSummaryEntity> carinfoList = new Car_BasicBll().GetCarInfoForSerialSummaryBySerialId(serialId);

			List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
				.FindAll(p => p.SaleState == "在销");
			List<CarInfoForSerialSummaryEntity> carinfoWaitSaleList = carinfoList
				.FindAll(p => p.SaleState == "待销");
			List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = carinfoList
				.FindAll(p => p.SaleState == "停销");

			if (carinfoSaleList.Count <= 0 && carinfoWaitSaleList.Count <= 0)
			{
				isExistCarList = false;
			}
			//add by 2014.05.04 在销车款 电动车
			var fuelTypeList = carinfoSaleList.Where(p => p.Oil_FuelType != "")
											  .GroupBy(p => p.Oil_FuelType)
											  .Select(g => g.Key).ToList();
			isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力" ? true : false;
			//add by 2014.03.18 在销车款 排量输出
			var exhaustList = carinfoSaleList.Where(p => p.Engine_Exhaust.EndsWith("L"))
				.Select(p => p.Engine_InhaleType == "增压" ? p.Engine_Exhaust.Replace("L", "T") : p.Engine_Exhaust)
											.GroupBy(p => p)
											.Select(group => group.Key).ToList();
			//add by 2014.05.20 车型筛选所用
			var newExhaustList = exhaustList.GetRange(0, exhaustList.Count);
			if (fuelTypeList.Contains("电力"))
				newExhaustList.Add("电动");

			carinfoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

			#region 去掉待销车型
			//carinfoWaitSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);

			//bool isWaitSale = false;
			//if (carinfoWaitSaleList.Count > 0)
			//{
			//    isWaitSale = true;
			//}

			//if (isWaitSale)
			//{
			//    foreach (var carInfo in carinfoWaitSaleList)
			//    {
			//        result.Add(carInfo);

			//    }
			//}
			#endregion

			var listGroupNew = new List<CarInfoForSerialSummaryEntity>();
			var listGroupOff = new List<CarInfoForSerialSummaryEntity>();

			if (carinfoSaleList.Count > 0)
			{
				foreach (var carInfo in carinfoSaleList)
				{
					if (carInfo.ProduceState != "停产")
						listGroupNew.Add(carInfo);
					else
						listGroupOff.Add(carInfo);
				}
				listGroupNew.AddRange(listGroupOff);
			}
			result.AddRange(listGroupNew);
			return result;
		}

		/// <summary>
		/// 子品牌还关注
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<EnumCollection.SerialToSerial> MakeSerialToSerialHtml(int serialId)
		{
			List<EnumCollection.SerialToSerial> lsts = new PageBase().GetSerialToSerialByCsID(serialId, 6);
			return lsts;
		}

		/// <summary>
		/// 获取商配文章
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<News> GetCommerceNews(int serialId)
		{
			var result = new List<News>();
			XmlDocument docPC = new XmlDocument();
			string cache = "SerialCommerceNews_all_" + serialId;
			object serialCommerceNews = null;
			CacheManager.GetCachedData(cache, out serialCommerceNews);
			if (serialCommerceNews != null)
			{
				docPC = (XmlDocument)serialCommerceNews;
			}
			else
			{
				var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialNews\\CommerceNews\\Xml\\Serial_CommerceNews_{0}.xml", serialId.ToString()));
				if (File.Exists(filePath))
				{
					docPC.Load(filePath);
					CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
				}
			}
			XmlNodeList nodeList = docPC.SelectNodes("/NewDataSet/listNews");
			if (nodeList.Count > 0)
			{
				foreach (XmlNode entity in nodeList)
				{
					if (entity == null || entity.ChildNodes == null || entity.ChildNodes.Count < 1)
					{
						continue;
					}

					var newsId = (entity.SelectNodes("newsid") != null && entity.SelectNodes("newsid").Count > 0) ? entity.SelectNodes("newsid")[0].InnerText.ToString().Trim() : "";
					var pubTime = (entity.SelectNodes("publishtime") != null && entity.SelectNodes("publishtime").Count > 0) ? entity.SelectNodes("publishtime")[0].InnerText.ToString().Trim().Split('T')[0] : "";
					var news = new News();
					news.NewsId = ConvertHelper.GetInteger(newsId);
					news.Author = (entity.SelectNodes("author") != null && entity.SelectNodes("author").Count > 0) ? entity.SelectNodes("author")[0].InnerText.ToString().Trim() : "";
					news.FaceTitle = (entity.SelectNodes("title") != null && entity.SelectNodes("title").Count > 0) ? entity.SelectNodes("title")[0].InnerText.ToString().Trim() : "";
					news.PublishTime = ConvertHelper.GetDateTime(pubTime);
					news.PageUrl = (entity.SelectNodes("filepath") != null && entity.SelectNodes("filepath").Count > 0) ? entity.SelectNodes("filepath")[0].InnerText.ToString().Trim() : "";
					news.CarImage = (entity.SelectNodes("firstPicUrl") != null && entity.SelectNodes("firstPicUrl").Count > 0) ? entity.SelectNodes("firstPicUrl")[0].InnerText.ToString().Trim() : "";

					result.Add(news);
				}
			}
			//如没有商配文章，补后补商配文章
			if (result.Count < 2)
			{
				var backCount = 2 - result.Count;
				var backResult = GetBackupCommerceNews(serialId, backCount);
				foreach (var bk in backResult)
				{
					result.Add(bk);
				}
			}
			return result;
		}
		/// <summary>
		/// 补商配文章 按品牌提取总部编辑原创其他新闻(接口)
		/// </summary>
		/// <returns></returns>
		public List<News> GetBackupCommerceNews(int serialId, int num)
		{
			var result = new List<News>();
			XmlDocument docPC = new XmlDocument();
			string cache = "SerialCommerceNews_all_backup" + serialId;
			object commerceNews = null;
			CacheManager.GetCachedData(cache, out commerceNews);
			if (commerceNews != null)
			{
				docPC = (XmlDocument)commerceNews;
			}
			else
			{
				var filePath = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\SerialNews\\CommerceNewsBackup\\Xml\\Serial_CommerceNews_Backup_{0}.xml", serialId.ToString()));
				if (File.Exists(filePath))
				{
					docPC.Load(filePath);
					CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
				}
			}
			XmlNodeList nodeList = docPC.SelectNodes("/NewDataSet/listNews");
			int count = 0;
			if (nodeList.Count > 0)
			{
				foreach (XmlNode entity in nodeList)
				{
					if (entity == null || entity.ChildNodes == null || entity.ChildNodes.Count < 1)
					{
						continue;
					}
					if (count >= num)
					{
						break;
					}
					var newsId = (entity.SelectNodes("newsid") != null && entity.SelectNodes("newsid").Count > 0) ? entity.SelectNodes("newsid")[0].InnerText.ToString().Trim() : "";
					var pubTime = (entity.SelectNodes("publishtime") != null && entity.SelectNodes("publishtime").Count > 0) ? entity.SelectNodes("publishtime")[0].InnerText.ToString().Trim().Split('T')[0] : "";
					var news = new News();
					news.NewsId = ConvertHelper.GetInteger(newsId);
					news.Author = (entity.SelectNodes("author") != null && entity.SelectNodes("author").Count > 0) ? entity.SelectNodes("author")[0].InnerText.ToString().Trim() : "";
					news.FaceTitle = (entity.SelectNodes("title") != null && entity.SelectNodes("title").Count > 0) ? entity.SelectNodes("title")[0].InnerText.ToString().Trim() : "";
					news.PublishTime = ConvertHelper.GetDateTime(pubTime);
					news.PageUrl = (entity.SelectNodes("filepath") != null && entity.SelectNodes("filepath").Count > 0) ? entity.SelectNodes("filepath")[0].InnerText.ToString().Trim() : "";
					news.CarImage = (entity.SelectNodes("firstPicUrl") != null && entity.SelectNodes("firstPicUrl").Count > 0) ? entity.SelectNodes("firstPicUrl")[0].InnerText.ToString().Trim() : "";

					result.Add(news);
					count++;
				}
			}
			return result;
		}

		/// <summary>
		/// 获取子品牌亮点配置
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public List<SerialSparkle> GetSerialSparkle(int serialId)
		{
			var result = new List<SerialSparkle>();
			var ds = new SerialFourthStageDal().GetSerialSparkle(serialId);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					SerialSparkle Obj = new SerialSparkle();
					Obj.SerialId = serialId;// ((row["cs_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cs_Id"]);
					Obj.Name = row["Name"].ToString().Trim();
					Obj.OrderId = ((row["OrderId"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["OrderId"]);
					Obj.H5SId = ((row["H5SId"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["H5SId"]);
					Obj.ImageUrl = string.Format("http://image.bitautoimg.com/carchannel/pic/sparkle/{0}.png", Obj.H5SId);
					result.Add(Obj);
				}
			}
			return result;
		}
	}
}
