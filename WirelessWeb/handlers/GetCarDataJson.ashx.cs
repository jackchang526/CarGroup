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
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCarDataJson 的摘要说明
	/// </summary>
	public class GetCarDataJson : WirelessPageBase, IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		private string[] CharList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(30);
			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			string action = ConvertHelper.GetString(request.QueryString["action"]);
			switch (action)
			{
				case "master": RenderMasterBrand(); break;
				case "serial": RenderSerial(); break;
				case "car": RenderCar(); break;
			}
		}
		/// <summary>
		/// 输出车款
		/// </summary>
		private void RenderCar()
		{
			int serialId = ConvertHelper.GetInteger(request.QueryString["pid"]);

			string cacheKey = "Car_Wireless_CarDataJson_Car_" + serialId;
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				response.Write((string)obj);
				return;
			}

			var serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			var serialCarList = new List<EnumCollection.CarInfoForSerialSummary>();
			List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
			if (serialEntity.SaleState == "停销")
			{
				// 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
				serialCarList = GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
			}
			else
			{
				// 非停销子品牌取 子品牌的非停销所有年款车型
				serialCarList = base.GetAllCarInfoForSerialSummaryByCsID(serialId);
			}
			string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

			List<string> groupJsonList = new List<string>();
			serialCarList.Sort(NodeCompare.CompareCarByYear);
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
							carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-'));
						else
							carMinPrice = carPriceRange;
					}
					carJsonList.Add(string.Format("{{CarId:{0},CarName:\"{1}\",Price:\"{2}\"}}", entity.CarID, entity.CarName, carMinPrice));
				}
				groupJsonList.Add(string.Format("\"s{0}\":[{1}]", key.Year, string.Join(",", carJsonList.ToArray())));
			}
			var json = string.Format("{{ImageUrl:\"{1}\",CarList:{{{0}}}}}", string.Join(",", groupJsonList.ToArray()), imgUrl);
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			response.Write(json);
		}
		/// <summary>
		/// 输出品牌、子品牌
		/// </summary>
		private void RenderSerial()
		{
			int masterId = ConvertHelper.GetInteger(request.QueryString["pid"]);

			string cacheKey = "Car_Wireless_CarDataJson_Serial_" + masterId;
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				response.Write((string)obj);
				return;
			}
            string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\MasterToBrandToSerialAllSaleAndLevel.xml");
            XmlDocument xmlDoc = AutoStorageService.GetAllAutoAndLevelXml();
            XPathDocument pathdoc = new XPathDocument(xmlPath);
           	XmlNode masterNode = xmlDoc.SelectSingleNode("/Params/MasterBrand[@ID='" + masterId + "']");
			List<string> brandList = new List<string>();
            if (masterNode != null)
            {
                foreach (XmlNode node in masterNode.ChildNodes)
                {
                    if (node.ChildNodes.Count <= 0)
                    {
                        continue;
                    }
                    int brandId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
                    string brandName = node.Attributes["Name"].Value;
                    string brandAllspell = node.Attributes["AllSpell"].Value;
                    List<string> serailList = new List<string>();
                    XPathNavigator nav = pathdoc.CreateNavigator();
                    string xpath = String.Format("/Params/MasterBrand[@ID={0}]/Brand[@ID={1}]/Serial", masterId, node.Attributes["ID"].Value);
                    XPathExpression exp = nav.Compile(xpath);
                    exp.AddSort("@CsSaleState", XmlSortOrder.Ascending, XmlCaseOrder.UpperFirst, "", XmlDataType.Text);
                    exp.AddSort("@CsPV", XmlSortOrder.Descending, XmlCaseOrder.None, "", XmlDataType.Number);
                    
                    XPathNodeIterator nodeIter = nav.Select(exp);
                    node.RemoveAll();
                    while (nodeIter.MoveNext())
                    {
                        int serialId = ConvertHelper.GetInteger(nodeIter.Current.GetAttribute("ID",""));
                        string serialName = nodeIter.Current.GetAttribute("Name", "");
                        string serialAllspell = nodeIter.Current.GetAttribute("AllSpell", "");
                        string csSaleState = nodeIter.Current.GetAttribute("CsSaleState", "");
                        string csLevel = nodeIter.Current.GetAttribute("CsLevel", "");
                        if (csLevel == "概念车")
                        { continue; }
                        if (csSaleState == "停销")
                        { continue; }
                        string priceRange = base.GetSerialPriceRangeByID(serialId);
                        if (csSaleState == "待销")
                            priceRange = "未上市";
                        if (string.IsNullOrEmpty(priceRange))
                            priceRange = "暂无报价";

                        string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

                        serailList.Add(string.Format("{{SerialId:{0},SerialName:\"{1}\",Allspell:\"{2}\",Price:\"{3}\",ImageUrl:\"{4}\"}}", serialId, serialName, serialAllspell,
                            priceRange, imgUrl));
                    }
                    brandList.Add(string.Format("{{BrandId:{0},BrandName:\"{1}\",BrandAllspell:\"{2}\",Child:[{3}]}}", brandId, brandName, brandAllspell, string.Join(",", serailList.ToArray())));              
                }
            }
			var json = string.Format("[{0}]", string.Join(",", brandList.ToArray()));
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			response.Write(json);
		}
		/// <summary>
		/// 输出主品牌
		/// </summary>
		private void RenderMasterBrand()
		{
			string cacheKey = "Car_Wireless_CarDataJson_Master";
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				response.Write((string)obj);
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
					list.Add(string.Format("{{MasterId:{0},MasterName:\"{1}\",AllSpell:\"{2}\"}}", mbId, mbName, masterSpell));
					dict.Add(firstChar, list);
				}
				else
				{
					dict[firstChar].Add(string.Format("{{MasterId:{0},MasterName:\"{1}\",AllSpell:\"{2}\"}}", mbId, mbName, masterSpell));
				}
			}
			for (int i = 0; i < CharList.Length; i++)
			{
				var firstChar = CharList[i];
				charList.Add(string.Format("{0}:{1}", firstChar, dict.ContainsKey(firstChar) ? 1 : 0));
			}
			List<string> masterList = new List<string>();
			foreach (string key in dict.Keys)
			{
				masterList.Add(string.Format("{0}:[{1}]", key, string.Join(",", dict[key].ToArray())));
			}
			var json = string.Format("{{CharList:{{{0}}},MsterList:{{{1}}}}}",
			string.Join(",", charList.ToArray()), string.Join(",", masterList.ToArray()));
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			response.Write(json);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}