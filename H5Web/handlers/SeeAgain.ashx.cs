using System.Collections.Generic;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using Newtonsoft.Json;

namespace H5Web.handlers
{
    /// <summary>
    ///     SeeAgain 的摘要说明
    /// </summary>
    public class SeeAgain : H5PageBase, IHttpHandler
    {
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();
        protected List<EnumCollection.SerialToSerial> AttentionSerials;
        protected bool IsExistAttention;

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);

            //context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.ContentType = "text/plain";

            if (context.Request.QueryString["csid"] == null && string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                context.Response.Write("<div class='message-failure'><img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png' /><h2>很遗憾！</h2><p>数据抓紧完善中，敬请期待！</p></div><div class='arrow_down'></div>");
                context.Response.End();
                return;
            }

            var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

            var topCount = 4;

            if (context.Request.QueryString["top"] != null)
            {
                int.TryParse(context.Request.QueryString["top"], out topCount);
            }

            var keyList = new List<int> {serialId, topCount};

            var cacheKey = string.Format("H5V3_SeeAgain20160530_{0}", string.Join("_", keyList));

            var obj = CacheManager.GetCachedData(cacheKey);
            var sbBuilder = new StringBuilder();
            if (obj != null)
            {
                context.Response.Write(obj);
            }
            else
            {
                var serialAdList = GetSeeToSeeAd(serialId);
                var dicAllCsPic = GetAllSerialPicURL(true);
                AttentionSerials = _serialFourthStageBll.MakeSerialToSerialHtml(serialId);
                // 广告强制位置
                if (serialAdList != null && serialAdList.Count > 0)
                {
                    for (var i = AttentionSerials.Count - 1; i >= 0; i--)
                    {
                        var item = AttentionSerials[i];
                        foreach (var values in serialAdList.Values)
                        {
                            if (values.ContainsValue(item.ToCsID.ToString()))
                            {
                                AttentionSerials.Remove(item);
                                break;
                            }
                        }
                    }
                }
                if (AttentionSerials != null && AttentionSerials.Count > 0)
                {
                    IsExistAttention = true;
                    var loop = 1;
                    var listTemp = new List<string>();
                    var listToID = new List<int>();
                    
                    sbBuilder.AppendFormat("<header>");
                    sbBuilder.AppendFormat("	<h2>看了还看</h2>");
                    sbBuilder.AppendFormat("</header>");
                    sbBuilder.AppendFormat("<div class='con_top_bg'></div>");
                    sbBuilder.AppendFormat("    <div class='big_bg big_bg_car_list'>");
                    sbBuilder.AppendFormat("	    <ul class='car_list'>");


                    foreach (var sts in AttentionSerials)
                    {
                        if (loop > 4)
                        {
                            break;
                        }
                        if (serialAdList != null && serialAdList.Count > 0
                            && serialAdList.ContainsKey(loop) &&
                            !listToID.Contains(ConvertHelper.GetInteger(serialAdList[loop]["AD_SerialID"])))
                        {
                            var otherID = ConvertHelper.GetInteger(serialAdList[loop]["AD_SerialID"]);
                            listToID.Add(otherID);

                            sbBuilder.AppendFormat("			<li>");
                            sbBuilder.AppendFormat("				<a href='{0}'>", serialAdList[loop]["Url"]);
                            sbBuilder.AppendFormat("					<img src='{0}'>",
                                (dicAllCsPic.ContainsKey(otherID)
                                    ? dicAllCsPic[otherID].Replace("_2.", "_3.")
                                    : WebConfig.DefaultCarPic));
                            sbBuilder.AppendFormat("					<span>{0}</span>", serialAdList[loop]["Title"]);
                            if (!string.IsNullOrEmpty(serialAdList[loop]["PriceRange"]))
                            {
                                sbBuilder.AppendFormat("					<p>{0}</p>", serialAdList[loop]["PriceRange"]);
                            }
                            else
                            {
                                sbBuilder.AppendFormat("					<p>{0}</p>", GetSerialPriceRangeByID(otherID));
                            }
                            sbBuilder.AppendFormat("				</a>");
                            sbBuilder.AppendFormat("            </li>");
                        }
                        else
                        {
                            if (!listToID.Contains(sts.ToCsID))
                            {
                                sbBuilder.AppendFormat("			<li>");
                                sbBuilder.AppendFormat("				<a href='/{0}'>", sts.ToCsAllSpell);
                                //sbBuilder.AppendFormat("					<img src='{0}'>", sts.ToCsPic.Replace("_5", "_3"));
                                sbBuilder.AppendFormat("					<img src='{0}'>", sts.ToCsPic.Replace("_5", "_6"));
                                sbBuilder.AppendFormat("					<span>{0}</span>", sts.ToCsShowName);
                                sbBuilder.AppendFormat("					<p>{0}</p>", sts.ToCsPriceRange);
                                sbBuilder.AppendFormat("				</a>");
                                sbBuilder.AppendFormat("            </li>");
                            }
                        }
                        loop++;
                    }

                    sbBuilder.AppendFormat("	    </ul>");
                    //sbBuilder.AppendFormat(
                    //    "	    <button class='button_gray'><a href='http://car.h5.yiche.com'>查看全部车型</a></button>");
                    sbBuilder.AppendFormat("    </div>");
                    sbBuilder.AppendFormat("<!--下箭头 固定-->");
                    sbBuilder.AppendFormat("<div class='arrow_down'></div>");

                    CacheManager.InsertCache(cacheKey, sbBuilder.ToString(), WebConfig.CachedDuration);

                    context.Response.Write(sbBuilder.ToString());
                }
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}