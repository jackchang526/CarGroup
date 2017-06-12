using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using System.Web.UI;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.Mai
{
	/// <summary>
	/// GetSerialDemand 的摘要说明
	/// </summary>
	public class GetSerialDemand : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		private int serialId = 0;
		private int cityId = 0;
		private string callback = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				// Duration = 60 * 10,
				Duration = 60 * 60 * 4,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;
			//获取参数
			GetParameter();
			RenderContent();
		}

		private void GetParameter()
		{
			serialId = ConvertHelper.GetInteger(request.QueryString["serialId"]);
			cityId = ConvertHelper.GetInteger(request.QueryString["cityid"]);

			callback = request.QueryString["callback"];
		}

		private void RenderContent()
		{
			StringBuilder sb = new StringBuilder();
			List<string> resultList = new List<string>();

			sb.Append("{");
			DataSet ds = GetCarDemandData(serialId, cityId);
			int sumDealerCount = 0;
			// 经销商数量
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				sumDealerCount = BitAuto.Utils.ConvertHelper.GetInteger(ds.Tables[0].Rows[0]["dealerCount"].ToString());
			}
			sb.Append(string.Format("DealerCount:{0},", sumDealerCount));
			// 车款的优惠数据
			List<string> listTemp = new List<string>();
			if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
			{
				Dictionary<int, DataRow> dicCarName = GetCarIDByCsID(serialId);
				List<int> hasCarIDList = new List<int>();
				foreach (DataRow dr in ds.Tables[1].Rows)
				{
					int carid = BitAuto.Utils.ConvertHelper.GetInteger(dr["CarID"].ToString());
					if (hasCarIDList.Contains(carid))
					{ continue; }
					hasCarIDList.Add(carid);
					decimal rp = BitAuto.Utils.ConvertHelper.GetDecimal(dr["ReducedPrice"].ToString());
					if (dicCarName.ContainsKey(carid))
					{
						listTemp.Add(string.Format("{{CarID:\"{0}\",Name:\"{1}\",MaxRP:\"{2}\",RP:\"{3}\"}}"
							, carid
							, CommonFunction.GetUnicodeByString(dicCarName[carid]["car_name"].ToString().Trim())
							, rp
							, dicCarName[carid]["car_ReferPrice"].ToString().Trim()));
					}
				}
			}
			sb.Append(string.Format("CarList:[{0}]", listTemp.Count > 0 ? string.Join(",", listTemp.ToArray()) : ""));
			sb.Append("}");

			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("{0}", sb.ToString()));
			else
				response.Write(string.Format("{1}({0})", sb.ToString(), callback));
		}

		/// <summary>
		/// 根据子品牌ID取旗下车款数据
		/// </summary>
		/// <param name="csid"></param>
		/// <returns></returns>
		private Dictionary<int, DataRow> GetCarIDByCsID(int csid)
		{
			Dictionary<int, DataRow> dic = new Dictionary<int, DataRow>();
			string sql = @"select car.car_id,car.car_name,car.car_ReferPrice
						from dbo.Car_Basic car
						left join car_serial cs on car.cs_id=cs.cs_id
						where car.isstate=1 and car.cs_id={0}";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
				, CommandType.Text, string.Format(sql, csid));
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carid = int.Parse(dr["car_id"].ToString());
					// string carName = dr["car_name"].ToString().Trim();
					// string rp = dr["car_ReferPrice"].ToString().Trim();
					if (!dic.ContainsKey(carid))
					{ dic.Add(carid, dr); }
				}
			}
			return dic;
		}

		/// <summary>
		/// 根据子品牌ID、城市ID 取易集客数据
		/// </summary>
		/// <returns></returns>
		private DataSet GetCarDemandData(int csid, int cityid)
		{
			DataSet ds = new DataSet();
			SqlParameter[] param = new SqlParameter[]{
				new SqlParameter("@SerialId", SqlDbType.Int) { Value = csid },
				new SqlParameter("@CityId", SqlDbType.Int) { Value = cityid }
			};
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString
				, CommandType.StoredProcedure, "SP_Demand_Get", param);
			return ds;
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