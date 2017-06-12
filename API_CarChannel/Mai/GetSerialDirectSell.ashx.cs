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
	/// GetSerialDirectSell 的摘要说明 直销数据
	/// </summary>
	public class GetSerialDirectSell : IHttpHandler
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
			List<string> mResultList = new List<string>();
			DataSet dsDirectSell = GetCarDirectSellData(serialId, cityId);
			sb.Append("{");
			if (dsDirectSell != null && dsDirectSell.Tables.Count > 0 && dsDirectSell.Tables[0].Rows.Count > 0)
			{
				string firstCsUrl = "";
				string mFirstCsUrl = string.Empty;
				List<string> hasCarID = new List<string>();
				foreach (DataRow dr in dsDirectSell.Tables[0].Rows)
				{
					if (firstCsUrl == "")
					{ firstCsUrl = CommonFunction.GetUnicodeByString(dr["CsUrl"].ToString()); }
					if (string.IsNullOrEmpty(mFirstCsUrl))
					{ mFirstCsUrl = CommonFunction.GetUnicodeByString(dr["MCsUrl"].ToString()); }
					string caridStr = dr["CarId"].ToString();
					if (hasCarID.Contains(caridStr))
					{ continue; }
					else
					{ hasCarID.Add(caridStr); }
					resultList.Add(string.Format("{{CarId:\"{0}\",Price:\"{1}\",Url:\"{2}\"}}",
						caridStr,
						dr["Price"].ToString(),
						CommonFunction.GetUnicodeByString(dr["Url"].ToString())
						));
					mResultList.Add(string.Format("{{CarId:\"{0}\",Price:\"{1}\",Url:\"{2}\"}}",
						caridStr,
						dr["Price"].ToString(),
						CommonFunction.GetUnicodeByString(dr["MUrl"].ToString())
						));
				}
				sb.AppendFormat("CsUrl:\"{1}\",CarList:[{0}],MCsUrl:\"{2}\",MCarList:[{3}]",
					string.Join(",", resultList.ToArray()), firstCsUrl,
					mFirstCsUrl, string.Join(",", mResultList.ToArray()));
			}
			else
			{
				sb.Append("CsUrl:\"\",CarList:[]");
			}
			sb.Append("}");

			if (string.IsNullOrEmpty(callback))
				response.Write(string.Format("{0}", sb.ToString()));
			else
				response.Write(string.Format("{1}({0})", sb.ToString(), callback));
		}

		/// <summary>
		/// 根据子品牌ID、城市ID 取易集客数据
		/// </summary>
		/// <returns></returns>
		private DataSet GetCarDirectSellData(int csid, int cityid)
		{
			DataSet ds = new DataSet();
			SqlParameter[] param = new SqlParameter[]{
				new SqlParameter("@CsId", SqlDbType.Int) { Value = csid },
				new SqlParameter("@CityId", SqlDbType.Int) { Value = cityid }
			};
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString
				, CommandType.StoredProcedure, "SP_Car_DirectSell_Get", param);
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