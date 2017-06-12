using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// GetCarInfo 的摘要说明
	/// </summary>
	public class GetCarInfo : InterfacePageBase, IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;

		public void ProcessRequest(HttpContext context)
		{
			request = context.Request;
			response = context.Response;
			string dept = request.QueryString["dept"];
			if (!string.IsNullOrEmpty(dept))
			{
				switch (dept.ToLower())
				{
					case "car": GetCarInfoById(); break;
				}
			}
			context.Response.End();
		}

		//根据车型ID获取车型信息
		private void GetCarInfoById()
		{
			int carId = ConvertHelper.GetInteger(request.QueryString["id"]);
			StringBuilder sb = new StringBuilder();
			if (carId > 0)
			{
				string sql = string.Format(@"select car.car_id,car.car_name,cs.cs_id, 
                    (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust 
                     ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 
                     when cei.UnderPan_TransmissionType like '%自动' then 2 
                     when cei.UnderPan_TransmissionType like '%手自一体' then 3  
                     else 4 end) as TransmissionType 
                     ,car.car_ReferPrice,car.Car_YearType as CarYearType
				  from car_basic car 
                     left join car_serial cs on car.cs_id=cs.cs_id 
                     left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id 
                     where car.isState=1 and cs.isState=1 and car.car_id={0}
                     order by cs_id,car.Car_YearType desc,TransmissionType,car.car_ReferPrice", carId);
				DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				if (ds.Tables[0].Rows.Count > 0)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
					sb.Append("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" ");
					sb.AppendFormat(" SerialId=\"{0}\"", dr["cs_id"]);
					sb.Append(" CarYearType=\"" + dr["CarYearType"].ToString().Trim() + "\" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
					//根据车型ID取参数
					Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carId);
					sb.AppendFormat(" EngineExhaust=\"{0}\" ", dict.ContainsKey(423) ? dict[423] : "");
					sb.AppendFormat(" EngineExhaustForFloat=\"{0}\" ", dict.ContainsKey(785) ? dict[785] : "");
					sb.AppendFormat(" UnderPanTransmissionType=\"{0}\" ", dict.ContainsKey(712) ? dict[712] : "");
					sb.AppendFormat(" EngineType=\"{0}\" ", dict.ContainsKey(436) ? System.Security.SecurityElement.Escape(dict[436]) : "");
					sb.AppendFormat(" OilFuelType=\"{0}\" ", dict.ContainsKey(578) ? dict[578] : "");
					sb.AppendFormat(" EngineMaxPower=\"{0}\" ", dict.ContainsKey(430) ? dict[430] : "");
					sb.AppendFormat(" PerfDriveType=\"{0}\" ", dict.ContainsKey(655) ? dict[655] : "");
					sb.AppendFormat(" PerfWeight=\"{0}\" ", dict.ContainsKey(669) ? dict[669] : "");
					sb.AppendFormat(" PerfTonnage=\"{0}\" ", dict.ContainsKey(667) ? dict[667] : "");
					string InhaleType = "";
					string Engine_InhaleType = dict.ContainsKey(425) ? dict[425] : "";
					if (Engine_InhaleType == "增压")
					{ InhaleType = "T"; }
					// 高总的逻辑 除了 T 其他都是L 
					// add by chengl Mar.5.2012
					//else if (Engine_InhaleType == "待查" || Engine_InhaleType == "")
					//{ InhaleType = ""; }
					else
					{ InhaleType = "L"; }
					sb.Append(" InhaleType=\"" + InhaleType + "\" ");
					// 车型报价
					sb.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
					sb.Append("/>");
				}
			}
			Echo(sb.ToString());
		}
		//统一输出XML
		private void Echo(string str)
		{
			HttpContext.Current.Response.ContentType = "text/xml";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			HttpContext.Current.Response.Clear();
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.Append("<root>");
			sb.Append(str);
			sb.Append("</root>");
			response.Write(sb.ToString());
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