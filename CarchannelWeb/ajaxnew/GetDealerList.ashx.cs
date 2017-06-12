using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetDealerList 的摘要说明
	/// 废弃页面 modified by chengl Apr.17.2014
	/// </summary>
	public class GetDealerList : IHttpHandler
	{
		HttpRequest request;
		HttpResponse response;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			request = context.Request;
			response = context.Response;

			response.Write("{}");

			//int cityId = ConvertHelper.GetInteger(request["cityid"]);
			//int carId = ConvertHelper.GetInteger(request["carid"]);

			//BitAuto.CarChannel.BLL.com.bitauto.partner.Service s = new BitAuto.CarChannel.BLL.com.bitauto.partner.Service();
			//string info = s.GetDealerInfoByCarIdAndCityId("4FA9B62E-0159-462F-A5C2-4BF7DF02B3F0", carId, cityId);
			//DataSet ds = ConvertXMLToDataSet(info);
			//StringBuilder sb = new StringBuilder();
			//sb.Append("{\"dealerInfos\":[");
			//foreach (DataRow r in ds.Tables[0].Rows)
			//{
			//    sb.Append("{" + string.Format("\"carid\":{0},\"dealershortName\":\"{1}\",\"dealerId\":{2},\"dealerContactAddress\":\"{3}\",\"businessModel\":\"{4}\",\"dealerSalesPhones\":\"{5}\",\"locationName\":\"{6}\",\"salePrice\":{7}",
			//              r["carid"].ToString(), r["dealershortName"].ToString(), r["dealerId"].ToString(), r["dealerContactAddress"].ToString(),
			//              r["businessModel"].ToString(), r["dealerSalesPhones"].ToString(), r["locationName"].ToString(), r["salePrice"].ToString()) + "},");
			//}
			//if (ds.Tables[0].Rows.Count > 0) sb.Remove(sb.Length - 1, 1);
			//sb.Append("]}");
			//response.Write(sb.ToString());
		}

		private DataSet ConvertXMLToDataSet(string xmlData)
		{
			try
			{
				DataSet ds = new DataSet();
				using (StringReader sr = new StringReader(xmlData))
				using (XmlTextReader xtr = new XmlTextReader(sr))
				{
					ds.ReadXml(xtr);
				}

				return ds;
			}
			catch (System.Exception ex)
			{
				throw new Exception(ex.Message + ex.StackTrace);
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