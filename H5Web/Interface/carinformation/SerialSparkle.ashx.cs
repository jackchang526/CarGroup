using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.Interface.carinformation
{
    /// <summary>
    ///     亮点配置
    /// </summary>
    public class SerialSparkle : H5PageBase, IHttpHandler
    {
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            context.Response.ContentType = "text/plain";

            try
            {
                if (context.Request.QueryString["csid"] == null &&
                    string.IsNullOrEmpty(context.Request.QueryString["csid"]))
                {
                    return;
                }

                var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

                var topCount = 20;

                if (context.Request.QueryString["top"] != null)
                {
                    int.TryParse(context.Request.QueryString["top"], out topCount);
                }

                var keyList = new List<int> {serialId, topCount};

                var cacheKey = "Interface_SerialSparkle_" + string.Join("_", keyList);

                var obj = CacheManager.GetCachedData(cacheKey);

                if (obj != null)
                {
                    context.Response.Write(obj);
                }
                else
                {
                    var baseSerialEntity = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, serialId);
                    var serialSparkleList = _serialFourthStageBll.GetSerialSparkle(serialId);

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("<header>");
                    stringBuilder.Append("    <h2>亮点配置</h2>");
                    stringBuilder.Append("</header>");
                    stringBuilder.Append("<div class='con_top_bg'></div>");

                    if (serialSparkleList.Count > 0)
                    {
                        #region 有数据

                        stringBuilder.Append("<div class='contain contain_config'>");
                        stringBuilder.Append("    <ul class='highlight'>");
                        for (var i = 0; i < serialSparkleList.Take(topCount).Count(); i++)
                        {
                            if ((i + 1)%4 == 1)
                            {
                                stringBuilder.Append("        <li>");
                            }
                            stringBuilder.AppendFormat(
                                "            <span><img src='http://image.bitautoimg.com/carchannel/pic/sparkle/{0}.png' />{1}</span>",
                                serialSparkleList[i].H5SId, serialSparkleList[i].Name);

                            if ((i + 1)%4 == 0)
                            {
                                stringBuilder.Append("        </li>");
                            }
                        }
                        stringBuilder.Append("        <span>");
                        stringBuilder.AppendFormat(
                            "					<a href='http://car.m.yiche.com/{0}/peizhi/'>", baseSerialEntity.AllSpell);
                        stringBuilder.Append(
                            "						<img src='http://image.bitautoimg.com/carchannel/pic/sparkle/0.png'>全部配置");
                        stringBuilder.Append("					</a>");
                        stringBuilder.Append("				</span>					");
                        if (serialSparkleList.Count%4 > 0)
                        {
                            stringBuilder.Append("        </li>");
                        }
                        stringBuilder.Append("    </ul>");
                        stringBuilder.Append("</div>");

                        #endregion
                    }
                    else
                    {
                        #region 无数据

                        stringBuilder.Append("<div class='message-failure'>");
                        stringBuilder.Append("    <img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png'/>");
                        stringBuilder.Append("    <h2>很遗憾！</h2>");
                        stringBuilder.Append("    <p>数据抓紧完善中，敬请期待！</p>");
                        stringBuilder.Append("</div>");

                        #endregion
                    }
                    stringBuilder.Append("<div class='arrow_down'></div>");

                    var res = stringBuilder.ToString();

                    //CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);

                    context.Response.Write(res);
                }
            }
            catch (Exception)
            {
                context.Response.Write("");
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}