using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    /// SeeAgainAdLevel 的摘要说明
    /// </summary>
    public class SeeAgainAdLevel : H5PageBase, IHttpHandler
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

            string level = string.Empty;
            if (context.Request.QueryString["level"] != null)
            {
                level = context.Request.QueryString["level"];
            }

            var keyList = new List<string> { serialId.ToString(), topCount.ToString() , level };

            var cacheKey = "H5V3_SeeAgainAdLevel_"+string.Join("_", keyList);

            var obj = CacheManager.GetCachedData(cacheKey);

            var sbBuilder = new StringBuilder();

            if (obj != null)
            {
                context.Response.Write(obj);
            }
            else
            {
                var dicAllCsPic = GetAllSerialPicURL(true);
                AttentionSerials = _serialFourthStageBll.MakeSerialToSerialHtml(serialId);

                Dictionary<int, Dictionary<string, string>> targetAdDic=new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, Dictionary<int, Dictionary<string, string>>> serialAdDictionary = GetSeeAgainByLevel(); 
                
                if (serialAdDictionary != null && serialAdDictionary.Count > 0)
                {
                    foreach (var key in serialAdDictionary.Keys)
                    {
                        if (!key.Contains(level)) continue;
                        foreach (var dicItem in serialAdDictionary[key])
                        {
                            targetAdDic.Add(dicItem.Key, dicItem.Value);
                        }
                    }
                }

                // 广告强制位置
                if (targetAdDic.Count > 0)
                {
                    for (var i = AttentionSerials.Count - 1; i >= 0; i--)
                    {
                        var item = AttentionSerials[i];
                        foreach (var dicItem in targetAdDic.Values)
                        {
                            if (dicItem["SerialID"] != item.ToCsID.ToString()) continue;
                            AttentionSerials.Remove(item);
                            break;
                        }
                    }
                }

                if (AttentionSerials != null && AttentionSerials.Count > 0)
                {
                    IsExistAttention = true;
                    var position = 1;
                    var listToId = new List<int>();

                    sbBuilder.AppendFormat("<header>");
                    sbBuilder.AppendFormat("	<h2>看了还看</h2>");
                    sbBuilder.AppendFormat("</header>");
                    sbBuilder.AppendFormat("<div class='con_top_bg'></div>");
                    sbBuilder.AppendFormat("    <div class='big_bg big_bg_car_list'>");
                    sbBuilder.AppendFormat("	    <ul class='car_list'>");


                    foreach (var sts in AttentionSerials)
                    {
                        if (position > 4)
                        {
                            break;
                        }
                        if ( targetAdDic.Count > 0
                            && targetAdDic.ContainsKey(position) &&
                            !listToId.Contains(ConvertHelper.GetInteger(targetAdDic[position]["SerialID"])))
                        {
                            var otherId = ConvertHelper.GetInteger(targetAdDic[position]["SerialID"]);
                            listToId.Add(otherId);

                            sbBuilder.AppendFormat("			<li>");
                            sbBuilder.AppendFormat("				<a href='{0}'>", targetAdDic[position]["Url"]);
                            sbBuilder.AppendFormat("					<img src='{0}'>",
                                (dicAllCsPic.ContainsKey(otherId)
                                    ? dicAllCsPic[otherId].Replace("_2.", "_3.")
                                    : WebConfig.DefaultCarPic));
                            sbBuilder.AppendFormat("					<span>{0}</span>", targetAdDic[position]["Title"]);
                            //改为指导价
                            sbBuilder.AppendFormat("					<p>{0}</p>",GetSerialReferPriceByID(otherId));
                            sbBuilder.AppendFormat("				</a>");
                            sbBuilder.AppendFormat("            </li>");
                        }
                        else
                        {
                            if (!listToId.Contains(sts.ToCsID))
                            {
                                sbBuilder.AppendFormat("			<li>");
                                sbBuilder.AppendFormat("				<a href='/{0}'>", sts.ToCsAllSpell);
                                sbBuilder.AppendFormat("					<img src='{0}'>", sts.ToCsPic.Replace("_5", "_6"));
                                sbBuilder.AppendFormat("					<span>{0}</span>", sts.ToCsShowName);
                                sbBuilder.AppendFormat("					<p>{0}</p>", sts.ToCsPriceRange);
                                sbBuilder.AppendFormat("				</a>");
                                sbBuilder.AppendFormat("            </li>");
                            }
                        }
                        position++;
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