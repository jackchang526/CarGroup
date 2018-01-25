using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using BitAuto.CarChannel.Common;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// GetCarInfoForCalcTools 的摘要说明
	/// </summary>
	public class GetCarInfoForCalcTools : IHttpHandler
	{

		int carID = 0;
		string type = string.Empty;
		StringBuilder sb = new StringBuilder();

		HttpResponse response;
		HttpRequest request;
		public void ProcessRequest(HttpContext context)
		{
            BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			// 检查参数
			this.CheckParam();
			if (carID > 0)
			{
				this.GetCarCountryEngineAndSeatNumByCarID();
				response.Write(sb.ToString());
			}
		}
		private void CheckParam()
		{
			if (this.request.QueryString["carID"] != null && this.request.QueryString["carID"].ToString() != "")
			{
				string strCarID = this.request.QueryString["carID"].ToString().Trim();
				if (int.TryParse(strCarID, out carID))
				{
				}
			}
			if (this.request.QueryString["type"] != null && this.request.QueryString["type"].ToString() != "")
			{
				type = this.request.QueryString["type"].ToString().Trim().ToLower();
			}
		}

		private void GetCarCountryEngineAndSeatNumByCarID()
		{
			if (carID > 0)
			{
				#region 取车型国别、排量、乘员人数

				int engine = 0;
				int seatNum = 0;
				double referPrice = 0.0;
				bool isGuoChan = true;
                PageBase pageBase = new PageBase();
                pageBase.GetCarCountryEngineAndSeatNumByCarID(carID, out isGuoChan, out engine, out seatNum, out referPrice);
				#endregion
				//根据车型取参数
				System.Collections.Generic.Dictionary<int, string> dict = new BitAuto.CarChannel.BLL.Car_BasicBll().GetCarAllParamByCarID(carID);
				//排量 L
				string exhaustforfloat = dict.ContainsKey(785) ? dict[785] : "";
				//车船税
				// string traveltax=dict.ContainsKey(895) ? HttpUtility.UrlEncodeUnicode(dict[895]) : "";
				// modified by chengl Mar.14.2012 
				string traveltax = dict.ContainsKey(895) ? dict[895] : "";
                //add by 2018-01-23
                string fuelType = dict.ContainsKey(578) ? dict[578] : "";
                //自动匹配排量 如果库中的数据为空则使用默认设置。
                //座位数量，玻璃单独破碎险的进口或国产。
                //购置税中排量对应车型库中的排量
                //车船使用税和交强险中使用的座位数对应基本性能中的成员人数（含司机）
                if (type == "json")
				{
					sb.Append("[");
					sb.Append("{");
					sb.Append("\"carID\":" + carID + ",");
					sb.Append("\"isGuoChan\":\"" + isGuoChan + "\",");
					sb.Append("\"engine\":" + engine + ",");
					sb.AppendFormat("\"exhaustforfloat\":\"{0}\",", exhaustforfloat);
					sb.AppendFormat("\"traveltax\":\"{0}\",", traveltax);
                    sb.AppendFormat("\"fuelType\":\"{0}\",", fuelType);
                    sb.Append("\"seatNum\":\"" + seatNum + "\"");
					sb.Append("}");
					sb.Append("]");
				}
				else if (type == "jsonwithname")
				{
					sb.Append("tmpCarInfo={");
					sb.Append("\"carID\":" + carID + ",");
					sb.Append("\"isGuoChan\":\"" + isGuoChan + "\",");
					sb.Append("\"engine\":" + engine + ",");
					sb.AppendFormat("\"exhaustforfloat\":\"{0}\",", exhaustforfloat);
					sb.AppendFormat("\"traveltax\":\"{0}\",", traveltax);
                    sb.AppendFormat("\"fuelType\":\"{0}\",", fuelType);
                    sb.Append("\"seatNum\":\"" + seatNum + "\",");
					sb.Append("\"referPrice\":" + referPrice);
					sb.Append("};");
				}
			}
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