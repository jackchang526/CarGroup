using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 所有车型信息排量、年款、厂商指导价、变速器(熊玉辉)
	/// </summary>
	public partial class GetAllCarInfo : InterfacePageBase
	{
		private DataSet dsCar = new DataSet();
		private StringBuilder sb = new StringBuilder();
		private bool isAllSale = true;
		// 是否需要数据缓存 for 朱永旭 不要缓存数据
		private bool isNoCache = false;
		private int carID = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetAllCarInfoData();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				GenerateCarInfoXML();
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			string strIsAllSale = this.Request.QueryString["isAllSale"];
			if (!string.IsNullOrEmpty(strIsAllSale) && strIsAllSale == "0")
			{ isAllSale = false; }

			string strNoCache = this.Request.QueryString["isNoCache"];
			if (!string.IsNullOrEmpty(strNoCache) && strNoCache == "1")
			{ isNoCache = true; }

			string strCarid = this.Request.QueryString["carid"];
			if (!string.IsNullOrEmpty(strCarid))
			{
				if (int.TryParse(strCarid, out carID))
				{ }
			}
		}

		private void GetAllCarInfoData()
		{
			// 如果有车型ID 则不缓存数据
			string cacheKey = "interfaceforbitauto_GetAllCarInfo_" + isAllSale.ToString();
			if (CacheManager.IsCachedExist(cacheKey) && !isNoCache
				&& carID == 0)
			{
				// 当有缓存 并且 需要缓存
				dsCar = (DataSet)CacheManager.GetCachedData(cacheKey);
			}
			else
			{
				string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name,car.Car_YearType,
car.car_ReferPrice,car.car_SaleState,car.car_ProduceState,
cdb1.pvalue as Engine_Exhaust,cdb2.pvalue as UnderPan_TransmissionType
,cdb3.pvalue as Engine_ExhaustForFloat,cdb4.pvalue as Engine_InhaleType,c.classvalue as CarSaleState
from dbo.Car_relation car
left join dbo.Car_Serial cs on car.cs_id = cs.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carID and cdb1.paramid=423
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carID and cdb2.paramid=712
left join dbo.CarDataBase cdb3 on car.car_id=cdb3.carID and cdb3.paramid=785
left join dbo.CarDataBase cdb4 on car.car_id=cdb4.carID and cdb4.paramid=425
left join dbo.class c on  c.classid=car.car_SaleState
where car.isState=0 and cs.isState=0 {0} 
order by car.car_id";
				if (carID > 0)
				{
					// 如果有车型ID 则忽略销售状态
					sql = string.Format(sql, " and car.car_id=" + carID.ToString());
				}
				else if (isAllSale)
				{
					// 取全部销售状态
					sql = string.Format(sql, "");
				}
				else
				{
					// 取非停销车型
					sql = string.Format(sql, " and car.car_SaleState<>96 ");
				}
				dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
				if (carID == 0)
				{
					// 没有车型ID再缓存
					CacheManager.InsertCache(cacheKey, dsCar, WebConfig.CachedDuration);
				}
			}
		}

		private void GenerateCarInfoXML()
		{
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{
					if (carID > 0 && dsCar.Tables[0].Rows[i]["car_id"].ToString() != carID.ToString())
					{ continue; }
					sb.Append("<Item CarID=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCar.Tables[0].Rows[i]["car_name"].ToString()) + "\" ");
					sb.Append(" CsID=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" CarYearType=\"" + dsCar.Tables[0].Rows[i]["Car_YearType"].ToString() + "\" ");
					sb.Append(" CarSaleState=\"" + dsCar.Tables[0].Rows[i]["CarSaleState"].ToString() + "\" ");
					sb.Append(" ReferPrice=\"" + dsCar.Tables[0].Rows[i]["car_ReferPrice"].ToString() + "\" ");
					sb.Append(" EngineExhaust=\"" + dsCar.Tables[0].Rows[i]["Engine_Exhaust"].ToString() + "\" ");
					sb.Append(" EngineExhaustForFloat=\"" + dsCar.Tables[0].Rows[i]["Engine_ExhaustForFloat"].ToString() + "\" ");
					sb.Append(" Engine_InhaleType=\"" + dsCar.Tables[0].Rows[i]["Engine_InhaleType"].ToString() + "\" ");
					sb.Append(" UnderPanTransmissionType=\"" + dsCar.Tables[0].Rows[i]["UnderPan_TransmissionType"].ToString() + "\" />");
				}
			}
		}
	}
}