using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannelAPI.Web.cn.ucar.api;
using BitAuto.Utils;
using System.Xml;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.Ucar
{
	/// <summary>
	/// 子品牌综述页及其他子品牌使用
	/// 通过Ucar webservice 接口取子品牌城市数据
	/// http://api.ucar.cn/CarSourceInterface/CarSourceForBitAuto.asmx
	/// 方法：GetCarSourceListByByMoreRegulations
	/// 综述页右侧二手车源数据（车系ID,车型ID,城市ID，获取的条数,排序字段标志，1:价格，2:发布日期，3:上牌年份 4:行驶里程,排序顺序标志1:升序，2降序） 
	/// 目前使用 GetCarSourceListByByMoreRegulations(csid, cityID, count, 1, 2);
	/// 使用memcache缓存
	/// 接收参数：子品牌ID，城市ID
	/// 返回json 供不同页面使用
	/// add by chengl Apr.5.2012
	/// </summary>
	public partial class SerialCityUCarJson : PageBase
	{
		string tmpMemcacheKey = "UCar_List_GetSerialCityJson_{0}_{1}";
		protected void Page_Load(object sender, EventArgs e)
		{
			//GetSerialCityUCar();
		}

		private void GetSerialCityUCar()
		{
			int csId = ConvertHelper.GetInteger(Request.QueryString["csid"]);
			int cityId = ConvertHelper.GetInteger(Request.QueryString["cityid"]);
			string callback = Request.QueryString["callback"];
			string memcacheKey = string.Format(tmpMemcacheKey, csId, cityId);
			//MemCache.DelMemCacheByKey(memcacheKey);
			string outstr = (string)MemCache.GetMemCacheByKey(memcacheKey);
			if (string.IsNullOrEmpty(outstr))
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[");
				if (csId > 0 && cityId > 0)
				{
					BitAuto.CarChannelAPI.Web.cn.ucar.api.CarSourceForBitAuto wsCar = new CarSourceForBitAuto();
					XmlNode node = wsCar.GetCarSourceListByByMoreRegulations(csId, cityId, 10, 1, 2);
					if (node != null)
					{
						if (node.HasChildNodes)
						{
							XmlNodeList nodeList = node.SelectNodes("//item");
							foreach (XmlNode nodeItem in nodeList)
							{
								sb.Append("{");
								XmlNode nodeBuyCarDate = nodeItem.SelectSingleNode("./BuyCarDate");
								if (nodeBuyCarDate != null)
								{
									sb.AppendFormat("\"buycardate\":\"{0}\",", nodeBuyCarDate.InnerText);
								}
								XmlNode nodeBrandName = nodeItem.SelectSingleNode("./BrandName");
								if (nodeBrandName != null)
								{
									sb.AppendFormat("\"brandname\":\"{0}\",", nodeBrandName.InnerText);
								}
								XmlNode nodeCityName = nodeItem.SelectSingleNode("./CityName");
								if (nodeCityName != null)
								{
									sb.AppendFormat("\"cityname\":\"{0}\",", nodeCityName.InnerText);
								}
								XmlNode nodeDisplayPrice = nodeItem.SelectSingleNode("./DisplayPrice");
								if (nodeDisplayPrice != null)
								{
									sb.AppendFormat("\"displayprice\":\"{0}\",", nodeDisplayPrice.InnerText);
								}
								XmlNode nodeCarlistUrl = nodeItem.SelectSingleNode("./CarlistUrl");
								if (nodeCarlistUrl != null)
								{
									sb.AppendFormat("\"carlisturl\":\"{0}\",", nodeCarlistUrl.InnerText);
								}
								XmlNode nodeCityUrL = nodeItem.SelectSingleNode("./CityUrL");
								if (nodeCityUrL != null)
								{
									sb.AppendFormat("\"cityurl\":\"{0}\",", nodeCityUrL.InnerText);
								}
								sb.Remove(sb.Length - 1, 1);
								sb.Append("},");
							}
							sb.Remove(sb.Length - 1, 1);
						}
					}
				}
				sb.Append("]");
				MemCache.SetMemCacheByKey(memcacheKey, sb.ToString(), 60 * 60 * 1000);
				string result = string.IsNullOrEmpty(callback) ? sb.ToString() : callback + "(" + sb.ToString() + ")";
				Response.Write(result);
			}
			else
			{
				string result = string.IsNullOrEmpty(callback) ? outstr : callback + "(" + outstr + ")";
				Response.Write(result);
			}
		}
	}
}