using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace BitAuto.CarChannelAPI.Web.Mai
{
    /// <summary>
    /// GetSerialYichehui 的摘要说明
    /// </summary>
    public class GetSerialYichehui : IHttpHandler
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
                Duration = 60 * 60,
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

        private void GetParameter()
        {
            serialId = ConvertHelper.GetInteger(request.QueryString["serialId"]);
            cityId = ConvertHelper.GetInteger(request.QueryString["cityid"]);

            callback = request.QueryString["callback"];
        }
        private void RenderContent()
        {
            string descYichehui = "易车惠";
            StringBuilder sb = new StringBuilder();
            List<string> resultList = new List<string>();
            DataSet ds = GetCarInfo_YiCheHui(serialId, cityId);
            sb.Append("{");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                List<string> hasCarID = new List<string>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carId = ConvertHelper.GetInteger(dr["CarId"]);
                     string Price = dr["Price"].ToString();
                    resultList.Add(string.Format("{{CarId:\"{0}\",Price:\"{1}\",Url:\"{2}\",MUrl:\"{3}\"}}",
                        carId,
                        Price,
                        dr["Url"],
                        dr["MUrl"]
                    ));
                }
                sb.AppendFormat("Desc:\"{0}\",CsId:\"{1}\",CityId:\"{2}\",CarList:[{3}]",
                    CommonFunction.GetUnicodeByString(descYichehui),
                    serialId,
                    cityId,
                    string.Join(",", resultList.ToArray())
                    );
            }
            else
            {
                sb.AppendFormat("Desc:\"{0}\",CsId:\"{1}\",CityId:\"{2}\",CarList:[]",
                   CommonFunction.GetUnicodeByString(descYichehui),
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
        /// 根据子品牌ID、城市ID 取易车惠车款款式信息
        /// </summary>
        /// <returns></returns>
        private DataSet GetCarInfo_YiCheHui(int csid, int cityid)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[]{
				new SqlParameter("@CsId", SqlDbType.Int) { Value = csid },
				new SqlParameter("@CityId", SqlDbType.Int) { Value = cityid }
			};
            string sqltext = @"SELECT  a.CsId, a.CarId, a.Price, Url, MUrl
                                FROM    Car_YiCheHui a
                                WHERE   a.CsId = @CsId
                                        AND a.CityId = @CityId
                                ORDER BY a.Price ASC";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqltext, param);
            return ds;
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