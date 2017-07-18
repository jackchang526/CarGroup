using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.Data;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// datatype:
	/// 0:在销 待销
	/// 1:在销 待销 停销
	/// 2:在销
	/// </summary>
	public class GetCarDataJson : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		private string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

		private string callback = string.Empty;
		private int dataType;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 60,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			dataType = ConvertHelper.GetInteger(request.QueryString["datatype"]);
			callback = ConvertHelper.GetString(request.QueryString["callback"]);
			string action = ConvertHelper.GetString(request.QueryString["action"]);
			switch (action)
			{
				case "master": RenderMasterBrand(); break;
				case "serial": RenderSerialNew(); break;
				case "serialv2": RanderSerialV2(); break;
				case "car": RenderCar(); break;
			}
		}
		/// <summary>
		/// 输出车款
		/// </summary>
		private void RenderCar()
		{
			int serialId = ConvertHelper.GetInteger(request.QueryString["pid"]);

			string cacheKey = string.Format("Car_API_CarDataJson_Car_{0}_{1}", dataType, serialId);
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				RenderJsonP((string)obj);
				return;
			}

			var serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			if (serialEntity == null || serialEntity.Id <= 0)
			{
				RenderJsonP("{}");
				return;
			}
			var serialCarList = new List<EnumCollection.CarInfoForSerialSummary>();
			List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
			//if (serialEntity.SaleState == "停销")
			//{
			//	// 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
			//	serialCarList = new PageBase().GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
			//}
			//else
			//{
			//	// 非停销子品牌取 子品牌的非停销所有年款车型
			//	serialCarList = new PageBase().GetAllCarInfoForSerialSummaryByCsID(serialId);
			//}
			//dataType > 0 包含停销车款
			if (dataType > 0)
			{
				serialCarList = new PageBase().GetAllCarInfoForSerialSummaryByCsID(serialId, true);
			}
			else
			{
				if (serialEntity.SaleState == "停销")
				{
					// 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
					serialCarList = new PageBase().GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
				}
				else
				{
					serialCarList = new PageBase().GetAllCarInfoForSerialSummaryByCsID(serialId, false);
				}
			}
			string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

			List<string> groupJsonList = new List<string>();
			serialCarList.Sort(NodeCompare.CompareCarByYearAndSale);
			var group = serialCarList.GroupBy(p => new { Year = p.CarYear }, p => p);
			foreach (var g in group)
			{
				var key = CommonFunction.Cast(g.Key, new { Year = "" });
				List<string> carJsonList = new List<string>();
				foreach (var entity in g.ToList())
				{
					//modified by sk 2013.06.03 修改最低报价
					string carMinPrice = string.Empty;
					string carPriceRange = entity.CarPriceRange.Trim();
					if (entity.CarPriceRange.Trim().Length == 0)
						carMinPrice = "暂无报价";
					else
					{
						if (carPriceRange.IndexOf('-') != -1)
							carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')) + "万";
						else
							carMinPrice = carPriceRange;
					}
					carJsonList.Add(string.Format("{{\"CarId\":{0},\"CarName\":\"{1}\",\"Price\":\"{2}\",\"ReferPrice\":\"{3}\",\"SaleState\":\"{4}\",\"Year\":\"{5}\"}}"
						, entity.CarID
						, entity.CarName
						, carMinPrice
						, entity.ReferPrice
						, entity.SaleState
						, entity.CarYear));
				}
				groupJsonList.Add(string.Format("\"s{0}\":[{1}]", key.Year, string.Join(",", carJsonList.ToArray())));
			}
			var json = string.Format("{{\"SerialName\":\"{2}\",\"ImageUrl\":\"{1}\",\"CarList\":{{{0}}}}}", string.Join(",", groupJsonList.ToArray()), imgUrl, serialEntity.ShowName);
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			RenderJsonP(json);
		}

		/// <summary>
		/// 输出品牌车系
		/// 车系销售排序规则 在销有价格>在销没有价格>未上市>待查>停销
		/// </summary>
		private void RanderSerialV2()
		{
			int masterId = ConvertHelper.GetInteger(request.QueryString["pid"]);
			string cacheKey = string.Format("Car_API_CarDataJson_Serialv2_{0}_{1}", dataType, masterId);
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				RenderJsonP((string)obj);
				return;
			}
			DataSet ds = new Car_BrandBll().GetCarSerialSortListByBSID(masterId, dataType);
			Dictionary<int, string> dict = new Dictionary<int, string>();
			Dictionary<int, string> brandList = new Dictionary<int, string>();
			var result = new List<string>();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string brandFormat = "\"BrandId\":{0},\"BrandName\":\"{1}\",\"BrandAllspell\":\"{2}\"";
				string serialFormat = "{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"Price\":\"{3}\",\"ImageUrl\":\"{4}\",\"CsSaleState\":\"{5}\"}}";
				Dictionary<int, List<string>> serialList = new Dictionary<int, List<string>>();
				Dictionary<int, List<string>> allwaitSale = new Dictionary<int, List<string>>();
				Dictionary<int, List<string>> allnoPrice = new Dictionary<int, List<string>>();
				Dictionary<int, List<string>> allwaitCheck = new Dictionary<int, List<string>>();
				Dictionary<int, List<string>> allstopSale = new Dictionary<int, List<string>>();
				int brandId = 0;
				string brandName = string.Empty;
				string brandAllspell = string.Empty;
				string country = string.Empty;
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					brandId = ConvertHelper.GetInteger(dr["cb_id"].ToString());
					brandName = dr["cb_name"].ToString();
					brandAllspell = dr["cbspell"].ToString();
					country = dr["Cp_Country"].ToString().Trim();
					if (country != "中国")
					{
						brandName = "进口" + brandName;
					}
					if (!serialList.ContainsKey(brandId)) serialList.Add(brandId, new List<string>());
					if (!allwaitSale.ContainsKey(brandId)) allwaitSale.Add(brandId, new List<string>());
					if (!allnoPrice.ContainsKey(brandId)) allnoPrice.Add(brandId, new List<string>());
					if (!allwaitCheck.ContainsKey(brandId)) allwaitCheck.Add(brandId, new List<string>());
					if (!allstopSale.ContainsKey(brandId)) allstopSale.Add(brandId, new List<string>());

					int serialId = ConvertHelper.GetInteger(dr["cs_id"].ToString());
					string serialName = dr["cs_ShowName"].ToString().Trim();
					string serialAllspell = dr["csspell"].ToString();
					string csSaleState = dr["CsSaleState"].ToString();
					string csLevel = dr["cslevel"].ToString();
					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

					#region 不显示
					if (csLevel == "概念车")
					{ continue; }
					if (csSaleState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
					{
						continue;
					}
					string csName = ConvertHelper.GetString(dr["cs_name"]);
					if (csName.IndexOf("停用") >= 0)
					{ continue; }
					#endregion

					string priceRange = string.Empty;
					if (csSaleState == "在销")
					{
						priceRange = dr["ReferPriceRange"].ToString();
						if (priceRange.Trim().Length == 0)
						{
							priceRange = "暂无指导价";
							allnoPrice[brandId].Add(string.Format(serialFormat, serialId, serialName, serialAllspell, priceRange, imgUrl, csSaleState.Trim()));
						}
						else
						{
							priceRange = priceRange + "万";
							serialList[brandId].Add(string.Format(serialFormat, serialId, serialName, serialAllspell, priceRange, imgUrl, csSaleState.Trim()));
						}
					}
					else if (csSaleState == "待销")
					{
						priceRange = "未上市";
						allwaitSale[brandId].Add(string.Format(serialFormat, serialId, serialName, serialAllspell, priceRange, imgUrl, csSaleState.Trim()));
					}
					else if (csSaleState == "待查")
					{
						priceRange = "暂无指导价";
						allwaitCheck[brandId].Add(string.Format(serialFormat, serialId, serialName, serialAllspell, priceRange, imgUrl, csSaleState.Trim()));
					}
					else
					{
						// 停销
						priceRange = "停售";
						allstopSale[brandId].Add(string.Format(serialFormat, serialId, serialName, serialAllspell, priceRange, imgUrl, csSaleState.Trim()));
					}

					if (!brandList.ContainsKey(brandId))
					{
						brandList.Add(brandId, string.Format(brandFormat, brandId, brandName, brandAllspell));
					}
				}
				foreach (int bid in brandList.Keys)
				{
					if (allnoPrice[bid].Count > 0)
						serialList[bid].Add(string.Join(",", allnoPrice[bid].ToArray()));
					if (allwaitSale[bid].Count > 0)
						serialList[bid].Add(string.Join(",", allwaitSale[bid].ToArray()));
					if (allwaitCheck[bid].Count > 0)
						serialList[bid].Add(string.Join(",", allwaitCheck[bid].ToArray()));
					if (allstopSale[bid].Count > 0)
						serialList[bid].Add(string.Join(",", allstopSale[bid].ToArray()));
					dict.Add(bid, string.Join(",", serialList[bid].ToArray()));
				}
			}
			foreach (int key in dict.Keys)
			{
				result.Add(string.Format("{{{0},\"Child\":[{1}]}}", brandList[key], dict[key]));
			}

			var json = string.Format("[{0}]", string.Join(",", result.ToArray()));
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			RenderJsonP(json);
		}

		/// <summary>
		/// 输出品牌、子品牌
		/// </summary>
		private void RenderSerialNew()
		{
			int masterId = ConvertHelper.GetInteger(request.QueryString["pid"]);

			//string cacheKey = string.Format("Car_API_CarDataJson_Serial_{0}_{1}", dataType, masterId);
			//var obj = CacheManager.GetCachedData(cacheKey);
			//if (obj != null)
			//{
			//	RenderJsonP((string)obj);
			//	return;
			//}

			DataSet ds = new Car_BrandBll().GetCarSerialSortListByBSID(masterId, dataType);
			Dictionary<int, List<string>> dict = new Dictionary<int, List<string>>();
			Dictionary<int, string> brandList = new Dictionary<int, string>();
			var result = new List<string>();

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int brandId = ConvertHelper.GetInteger(dr["cb_id"].ToString());
					string brandName = dr["cb_name"].ToString();
					string brandAllspell = dr["cbspell"].ToString();
					string country = dr["Cp_Country"].ToString().Trim();
					if (country != "中国")
					{
						brandName = "进口" + brandName;
					}
					int serialId = ConvertHelper.GetInteger(dr["cs_id"].ToString());
					string serialName = dr["cs_ShowName"].ToString().Trim();
					string serialAllspell = dr["csspell"].ToString();
					string csSaleState = dr["CsSaleState"].ToString();
					string csLevel = dr["cslevel"].ToString();
					if (csLevel == "概念车")
					{ continue; }

					//string priceRange = new PageBase().GetSerialPriceRangeByID(serialId).Trim();
					//if (csSaleState.Trim() == "待销")
					//	priceRange = "未上市";
					//if (string.IsNullOrEmpty(priceRange))
					//	priceRange = "暂无报价";

					string referPrice = dr["ReferPriceRange"].ToString();
					if (csSaleState == "在销")
					{
						if (referPrice.Length == 0)
						{
							referPrice = "暂无指导价";
						}
						else
						{
							referPrice = referPrice + "万";
						}
					}
					else if (csSaleState == "待销")
					{
						referPrice = "未上市";
					}
					else if (csSaleState == "待查")
					{
						referPrice = "暂无指导价";
					}
					else
					{
						// 停销
						referPrice = "停售";
					}

					string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");
					if (csSaleState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
					{
						continue;
					}
					if (!dict.ContainsKey(brandId))
					{
						var serailList = new List<string>();
						serailList.Add(string.Format("{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"Price\":\"{3}\",\"ImageUrl\":\"{4}\",\"CsSaleState\":\"{5}\"}}"
							, serialId
							, serialName
							, serialAllspell
							, referPrice//serialInfo.CsSaleState == "停销" ? noSaleLastReferPrice : serialEntity.ReferPrice
							, imgUrl
							, csSaleState.Trim()));
						dict.Add(brandId, serailList);
						brandList.Add(brandId, string.Format("\"BrandId\":{0},\"BrandName\":\"{1}\",\"BrandAllspell\":\"{2}\"", brandId, brandName, brandAllspell));
					}
					else
					{
						dict[brandId].Add(string.Format("{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"Price\":\"{3}\",\"ImageUrl\":\"{4}\",\"CsSaleState\":\"{5}\"}}"
							, serialId
							, serialName
							, serialAllspell
							, referPrice
							, imgUrl
							, csSaleState.Trim()));
					}
				}
			}
			foreach (int key in dict.Keys)
			{
				result.Add(string.Format("{{{0},\"Child\":[{1}]}}", brandList[key], string.Join(",", dict[key].ToArray())));
			}

			var json = string.Format("[{0}]", string.Join(",", result.ToArray()));
			//CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			RenderJsonP(json);
		}
		/// <summary>
		/// 输出主品牌
		/// </summary>
		private void RenderMasterBrand()
		{
			string cacheKey = string.Format("Car_API_CarDataJson_Master_{0}", dataType);
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				RenderJsonP((string)obj);
				return;
			}
			List<string> charList = new List<string>();
			Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");

			for (int i = 0; i < mbNodeList.Count; i++)
			{
				XmlElement mbNode = (XmlElement)mbNodeList[i];
				string masterSpell = mbNode.GetAttribute("AllSpell").ToLower();
				//首字母
				string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();
				//生成主品牌图标
				int mbId = ConvertHelper.GetInteger(mbNode.GetAttribute("ID"));
				string mbName = mbNode.GetAttribute("Name");

				XmlNodeList serialNodeList = mbNode.SelectNodes("./Brand/Serial");
				if (serialNodeList.Count <= 0) continue;
				if (!dict.ContainsKey(firstChar))
				{
					var list = new List<string>();
					list.Add(string.Format("{{\"MasterId\":{0},\"MasterName\":\"{1}\",\"AllSpell\":\"{2}\"}}", mbId, mbName, masterSpell));
					dict.Add(firstChar, list);
				}
				else
				{
					dict[firstChar].Add(string.Format("{{\"MasterId\":{0},\"MasterName\":\"{1}\",\"AllSpell\":\"{2}\"}}", mbId, mbName, masterSpell));
				}
			}
			for (int i = 0; i < CharList.Length; i++)
			{
				var firstChar = CharList[i];
				charList.Add(string.Format("\"{0}\":{1}", firstChar, dict.ContainsKey(firstChar) ? 1 : 0));
			}
			List<string> masterList = new List<string>();
			foreach (string key in dict.Keys)
			{
				masterList.Add(string.Format("\"{0}\":[{1}]", key, string.Join(",", dict[key].ToArray())));
			}
			var json = string.Format("{{\"CharList\":{{{0}}},\"MsterList\":{{{1}}}}}",
			string.Join(",", charList.ToArray()), string.Join(",", masterList.ToArray()));
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);

			RenderJsonP(json);
		}

		private void RenderJsonP(string content)
		{
			if (!string.IsNullOrEmpty(callback))
				content = string.Format("{0}({1})", callback, content);
			response.Write(content);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		private sealed class OutputCachedPage : Page
		{
			private OutputCacheParameters _cacheSettings;

			public OutputCachedPage(OutputCacheParameters cacheSettings)
			{
				ID = Guid.NewGuid().ToString();
				_cacheSettings = cacheSettings;
			}

			protected override void FrameworkInitialize()
			{
				base.FrameworkInitialize();
				InitOutputCache(_cacheSettings);
			}
		}
	}
}