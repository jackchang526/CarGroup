using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.Interface.carinformation
{
    /// <summary>
    ///     车系颜色和图片
    /// </summary>
    public class Color : H5PageBase, IHttpHandler
    {
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            context.Response.ContentType = "text/plain";

            try
            {
                var serialId = 0;
                if (!string.IsNullOrEmpty(context.Request.QueryString["csid"]))
                {
                    serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);
                }
                var colorList = _serialFourthStageBll.GetSerialColorList(serialId);
                if (colorList == null || colorList.Count <= 0) return;

                var keyList = new List<int> {serialId};
                var cacheKey = "Interface_Color_" + string.Join("_", keyList);

                var obj = CacheManager.GetCachedData(cacheKey);

                if (obj != null)
                {
                    context.Response.Write(obj);
                }
                else
                {
                    #region

                    var serialColorList = colorList.Count > 12 ? colorList.GetRange(0, 12) : colorList;
                    var baseSerialEntity = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, serialId);

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("            <div class='section-box'>");
                    stringBuilder.Append("                <div class='img'>");
                    stringBuilder.AppendFormat(
                        "<img src='http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{0}_100.png'/>",
                        baseSerialEntity.Brand.MasterBrand.Id);
                    stringBuilder.Append("                </div>");
                    stringBuilder.Append("                <div class='context'>");
                    stringBuilder.AppendFormat("                    <h1>{0}</h1>", baseSerialEntity.ShowName);
                    stringBuilder.AppendFormat("                    <p class='cs-price'>厂商指导价：{0}</p>",
                        baseSerialEntity.SaleState == "停销" ? "暂无" : baseSerialEntity.ReferPrice);
                    stringBuilder.Append("                </div>");
                    stringBuilder.Append("            </div>");
                    stringBuilder.Append(" <div class='standard_car_pic standard_car_pic_1' id='standard_car_pic'>");


                    for (var i = 0; i < serialColorList.Count; i++)
                    {
                        var item = serialColorList[i];
                        stringBuilder.AppendFormat(
                            "<img data-src='http://image.bitautoimg.com/newsimg-600-w0-1-q80/{0}' style='display: {1}'/>",
                            item.ImageUrl.Substring(27), i == 0 ? "display" : "none");
                    }
                    stringBuilder.Append("        </div>");

                    stringBuilder.Append("        <div class='car_color_text' id='car_color_text'>");
                    stringBuilder.AppendFormat("            <span>{0}</span>", serialColorList[0].ColorName);
                    stringBuilder.Append("        </div>");

                    stringBuilder.Append("<ul class='changecolor' id='changecolor'>");
                    for (var i = 0; i < serialColorList.Count; i++)
                    {
                        var color = serialColorList[i];
                        stringBuilder.AppendFormat("<li {0}>", i == 0 ? "class='current'" : string.Empty);
                        stringBuilder.AppendFormat("<span style='background: {0};' data-value='{1}'></span></li>",
                            color.ColorRGB, color.ColorName);
                    }
                    stringBuilder.Append("        </ul>");
                    stringBuilder.Append("        <div class='arrow_down'></div>");
                    var res = stringBuilder.ToString();

                    //CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);

                    context.Response.Write(res);

                    #endregion
                }
            }
            catch (Exception)
            {
                context.Response.Write("");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}