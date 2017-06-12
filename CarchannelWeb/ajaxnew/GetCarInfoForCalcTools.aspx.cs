using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.Common;
using System.Text;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class GetCarInfoForCalcTools : PageBase
	{
		#region Param
		private int carID = 0;
		private string type = string.Empty;
		private StringBuilder sb = new StringBuilder();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
            this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				// 检查参数
				this.CheckParam();
				if (carID > 0)
				{
					this.GetCarCountryEngineAndSeatNumByCarID();
					Response.Write(sb.ToString());
				}
			}
		}

		private void CheckParam()
		{
			if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
			{
				string strCarID = this.Request.QueryString["carID"].ToString().Trim();
				if (int.TryParse(strCarID, out carID))
				{
				}
			}
			if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
			{
				type = this.Request.QueryString["type"].ToString().Trim().ToLower();
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

				base.GetCarCountryEngineAndSeatNumByCarID(carID, out isGuoChan, out engine, out seatNum, out referPrice);
				#endregion
				//根据车型取参数
				System.Collections.Generic.Dictionary<int, string> dict = new BitAuto.CarChannel.BLL.Car_BasicBll().GetCarAllParamByCarID(carID);
				//排量 L
				string exhaustforfloat = dict.ContainsKey(785) ? dict[785] : "";
				//车船税
				// string traveltax=dict.ContainsKey(895) ? HttpUtility.UrlEncodeUnicode(dict[895]) : "";
				// modified by chengl Mar.14.2012 
				string traveltax = dict.ContainsKey(895) ? dict[895] : "";
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
					sb.Append("\"seatNum\":\"" + seatNum + "\",");
					sb.Append("\"referPrice\":" + referPrice);
					sb.Append("};");
				}
			}
		}
	}
}