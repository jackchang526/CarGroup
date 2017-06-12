using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using PieceDataProviderLib;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	/// <summary>
	/// GetCityHotNewCar 的摘要说明
	/// </summary>
	public class GetCityHotNewCar : InterfacePageBase, IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			PieceClient pc = new PieceClient(context, "123456");
			pc.ReadConfig += new ReadConfigHandle(ReadConfig);
			pc.GetDataSet += new GetDataSetHandle(GetDataSet);

			pc.UpdateTime = System.DateTime.Now;
			pc.RunCommand();
		}

		/// <summary>
		/// 返回用户需要的配置文件
		/// </summary>
		public string ReadConfig()
		{
			string cfgFile = Path.Combine(WebConfig.WebRootPath, "Interface\\GetCityHotNewCar.xml");
			string config = File.ReadAllText(cfgFile, Encoding.UTF8);
			return config;
		}

		/// <summary>
		/// 返回用户需要的DATASET的XML文件
		/// </summary>
		/// <param name="sc">搜索条件对象</param>
		/// <returns>DATASET的XML结果</returns>
		public System.Data.DataSet GetDataSet(SearchCondition sc)
		{
			int cityId = 0;
			int priceRange = 0;
			string byNewsType = String.Empty;
			int level = 0;
			bool onlyMantance = false;
			foreach (MultCondition mc in sc.ConditionFields)
			{
				if (mc is Condition)
				{
					Condition c = (Condition)mc;
					switch (c.FieldName)
					{
						case "OnlayMaintance":
							onlyMantance = ConvertHelper.GetBoolean(c.DefaultValue.ToString());
							break;
						case "ByNewsType":		//maintance,anquan,keji
							byNewsType = c.DefaultValue.ToString();
							break;
						case "PriceRange":
							priceRange = ConvertHelper.GetInteger(c.DefaultValue);
							break;
						case "PublishCity":
							cityId = ConvertHelper.GetInteger(c.DefaultValue);
							break;
						case "Level": level = ConvertHelper.GetInteger(c.DefaultValue);
							break;
					}
				}
			}

			if (onlyMantance)
				byNewsType = "maintance";

			DataSet ds = null;
			Car_SerialBll csBll = new Car_SerialBll();
			if (priceRange == 0)
			{
				if (level > 0)
				{
					// 级别
					ds = csBll.GetHotNewCarByPriceRange(level, 0, cityId, byNewsType);
				}
				else
				{
					ds = csBll.GetHotNewCar(cityId, byNewsType);
				}
			}
			else
			{
				//按价格区间取子品牌
				ds = csBll.GetHotNewCarByPriceRange(0, priceRange, cityId, byNewsType);
			}

			return ds;
		}

		public bool IsReusable
		{
			get { throw new Exception("The method or operation is not implemented."); }
			//get
			//{
			//    return false;
			//}
		}
	}
}