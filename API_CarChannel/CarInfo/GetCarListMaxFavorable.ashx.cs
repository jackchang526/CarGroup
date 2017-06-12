using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// 根据子品牌id、城市id，获取相关车款最高降幅与降额
    /// var carListMaxFavorable = {
    ///     c123456:{	-- c = 关键词，123456=车款id
    ///	        MaxFavPrice:1.50 --最高降额
    ///	        MaxFavRate:0.34	--最高降幅
    ///     },
    ///     c456789:{	-- c = 关键词，456789=车款id
    ///	        MaxFavPrice:1.50 --最高降额
    ///	        MaxFavRate:0.34	--最高降幅
    ///     },
    ///     ...
    ///};
    /// </summary>
    public class GetCarListMaxFavorable : IHttpHandler
    {
        private int _SerialId = 0;
        private int _CityId = 0;

        private HttpRequest _request;
        private HttpResponse _response;

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(30);
            context.Response.ContentType = "application/x-javascript";

            _response = context.Response;
            _request = context.Request;

            GetParams();
            GetContent();
        }

        /// <summary>
        /// 得到页面参数
        /// </summary>
        private void GetParams()
        {
            _SerialId = ConvertHelper.GetInteger(_request.QueryString["csid"]);
            _CityId = ConvertHelper.GetInteger(_request.QueryString["cityid"]);
        }
        /// <summary>
        /// 得到内容
        /// </summary>
        private void GetContent()
        {
            StringBuilder result = new StringBuilder();

            if (_SerialId > 0 && _CityId >= 0)
            {
                try
                {
                    CarNewsBll newsBll = new CarNewsBll();

                    List<CarJiangJiaNewsSummary> summary = newsBll.GetCarJiangJiaNewsSummary(_SerialId, _CityId);

                    foreach (CarJiangJiaNewsSummary carSummary in summary)
                    {
                        result.AppendFormat("\"c{0}\"", carSummary.CarId.ToString()).Append(":{");

                        result.AppendFormat("\"MaxFavPrice\":\"{0}\",\"MaxFavRate\":\"{1}\"", carSummary.MaxFavorablePrice.ToString("0.##"), carSummary.MaxFavorableRate.ToString("0.##"));

                        result.Append("},");
                    }
                }
                catch { }
            }

            if (result.Length > 0)
                result.Remove(result.Length - 1, 1);

            _response.Write("var carListMaxFavorable = {");
            _response.Write(result.ToString());
            _response.Write("};");
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