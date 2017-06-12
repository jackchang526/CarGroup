using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace H5Web.handlers
{
	/// <summary>
	///     GetDateAsync 的摘要说明
	/// </summary>
	public class GetDateAsync : H5PageBase, IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(60);

		    // CommonFunction.WriteInvokeLog("start");
			context.Response.ContentType = "application/x-javascript";
			var h5ServiceDic = GetH5ServiceDicV2();
			var list = new List<string>();

			#region param

			var serviceName = context.Request["service"];
			list.Add(serviceName);

			var methodName = context.Request["method"];
			list.Add(methodName);

			var csid = context.Request["csid"];
			if (!string.IsNullOrEmpty(csid))
			{
				int cs_id = BitAuto.Utils.ConvertHelper.GetInteger(csid);
				if (cs_id > 0)
				{
					list.Add(cs_id.ToString());
				}
			}

			var cityId = context.Request["cityId"];
			if (!string.IsNullOrEmpty(cityId))
			{
				int city_id = BitAuto.Utils.ConvertHelper.GetInteger(cityId);
				if (city_id > 0)
				{
					list.Add(city_id.ToString());
				}
			}

			var dealerid = context.Request["dealerid"];
			if (!string.IsNullOrEmpty(dealerid))
			{
				int dealer_id = BitAuto.Utils.ConvertHelper.GetInteger(dealerid);
				if (dealer_id > 0)
				{
					list.Add(dealer_id.ToString());
				}
			}

			var type = context.Request["type"];
			if (!string.IsNullOrEmpty(type))
			{
				list.Add(type);
			}

			var brokerid = context.Request["brokerid"];
			if (!string.IsNullOrEmpty(brokerid))
			{
				int broker_id = BitAuto.Utils.ConvertHelper.GetInteger(brokerid);
				if (broker_id > 0)
				{
					list.Add(broker_id.ToString());
				}
			}

			#endregion

			var cacheKey = string.Format("H5V2_{0}", string.Join("_", list));

		    object obj = null; //CacheManager.GetCachedData(cacheKey);

			if (obj != null)
			{
				var res = (string)obj;
				if (methodName.ToLower() == "share")
				{
					context.Response.ContentType = "application/json; charset=utf-8";
					context.Response.Write(res);
				}
				else
				{
					context.Response.Write("document.write('" + CommonFunction.GetUnicodeByString(res) + "');");
				}
                context.Response.End();
			}
			else
			{
				Uri url = null;
				if (serviceName.ToLower() == "huimaiche")
				{
					url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
				}
				if (serviceName.ToLower() == "yichemall")
				{
					url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
				}
				if (serviceName.ToLower() == "dealer")
				{
                    if (methodName.ToLower() == "dealerimgmap" || methodName.ToLower() == "serialmap")
				    {
                        url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], cityId, csid));
				    }
				    else
				    {
                        url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], dealerid, csid));
				    }
				}
				if (serviceName.ToLower() == "daikuan")
				{
					url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
				}
				if (serviceName.ToLower() == "agent")
				{
					url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], brokerid, csid, type));
				}
				if (serviceName.ToLower() == "market")
				{
					url = new Uri(string.Format(h5ServiceDic[serviceName][methodName], csid, cityId));
				}
                if (serviceName.ToLower() == "ershouche")
                {
                    url = new Uri(string.Format(h5ServiceDic[serviceName][methodName],cityId, csid));
                }
				var webClient = new WebClient { Encoding = Encoding.UTF8 };

                //string tempStr = webClient.DownloadString(url);
                //CommonFunction.WriteInvokeLog("down end");
                //tempStr = CommonFunction.GetUnicodeByString(tempStr);
                //CommonFunction.WriteInvokeLog("code end");
                //context.Response.Write(tempStr);
                //CommonFunction.WriteInvokeLog("down end");

                webClient.DownloadStringAsync(url);

                webClient.DownloadStringCompleted += (s, evt) =>
                {
                    // CommonFunction.WriteInvokeLog("down end");
                    if (evt.Error != null)
                    {
                        // 记录异常
                        CommonFunction.WriteInvokeLog(string.Format("\r\n接口异常:{0}\r\nIP:{1}\r\nUrl:{2}\r\n{3}\r\n"
                            , url, BitAuto.Utils.WebUtil.GetClientIP(), context.Request.Url, evt.Error));
                        CacheManager.InsertCache(cacheKey, " ", WebConfig.CachedDuration);
                        context.Response.Write(" ");

                    }
                    else
                    {
                        CacheManager.InsertCache(cacheKey, evt.Result, WebConfig.CachedDuration);
                        if (methodName.ToLower() == "share")
                        {
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.Write(evt.Result);
                        }
                        else
                        {
                            context.Response.Write("document.write('" + CommonFunction.GetUnicodeByString(evt.Result) + "');");
                        }
                    }
                    context.Response.End();
                };
			}
            // CommonFunction.WriteInvokeLog("ashx end");
        }

		public bool IsReusable
		{
			get { return false; }
		}
	}
}