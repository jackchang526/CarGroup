using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannelAPI.Web.Mai
{
	/// <summary>
	/// 易车商城 包销 平行进口 接口
	/// </summary>
	public class GetSerialParallelAndSell : IHttpHandler
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
			DataSet ds = GetCarParallelAndSell(serialId, cityId);
			sb.Append("{");
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				List<string> hasCarID = new List<string>();
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carId = ConvertHelper.GetInteger(dr["CarId"]);
					int carType = Convert.ToInt32(dr["CarType"].ToString());
					CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
					int year = carEntity.CarYear;
					string carName = carEntity != null ? carEntity.Name : string.Empty;
					carName = year > 0 ? string.Format("{0}款 {1}", year, carName) : carName;
					resultList.Add(string.Format("{{CarId:\"{0}\",CarName:\"{3}\",Price:\"{1}\",CarType:\"{2}\"}}",
						carId,
						dr["Price"].ToString(),
						dr["CarType"].ToString(),
						CommonFunction.GetUnicodeByString(carName)));
				}
				sb.AppendFormat("CsId:\"{0}\",CityId:\"{1}\",CarList:[{2}]",
					serialId,
					cityId,
					string.Join(",", resultList.ToArray())
					);
			}
			else
			{
				sb.AppendFormat("CsId:\"{0}\",CityId:\"{1}\",CarList:[]",
				   serialId,
				   cityId);
			}
			sb.Append("}");

			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("{0}", sb.ToString()));
			else
				response.Write(string.Format("{1}({0})", sb.ToString(), callback));
		}

		/// <summary>
		/// 根据子品牌ID、城市ID 取易车商城 包销 平行进口
		/// </summary>
		/// <returns></returns>
		private DataSet GetCarParallelAndSell(int csid, int cityid)
		{
			DataSet ds = new DataSet();
			SqlParameter[] param = new SqlParameter[]{
				new SqlParameter("@CsId", SqlDbType.Int) { Value = csid },
				new SqlParameter("@CityId", SqlDbType.Int) { Value = cityid }
			};
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString
				, CommandType.StoredProcedure, "SP_ParallelAndSell_Get", param);
			return ds;
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

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}