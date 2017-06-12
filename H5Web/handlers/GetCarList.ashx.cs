using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using Newtonsoft.Json;
using System;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetCarList 的摘要说明
    /// </summary>
    public class GetCarList : H5PageBase, IHttpHandler
    {
        private readonly Car_BasicBll _carBll = new Car_BasicBll();
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();
        protected Dictionary<int, string> CarParamDictionary;

        public void ProcessRequest(HttpContext context)
        {
            base.SetPageCache(60);

            context.Response.ContentType = "application/json; charset=utf-8";

            if (context.Request.QueryString["csid"] == null && string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                return;
            }

            var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

            var topCount = 10;

            if (context.Request.QueryString["top"] != null)
            {
                int.TryParse(context.Request.QueryString["top"], out topCount);
            }

            var keyList = new List<int> { serialId, topCount };

            var cacheKey = string.Format("H5V3_GetCarList_{0}", string.Join("_", keyList));

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                context.Response.Write(obj);
            }
            else
            {
                var dictCarTaxTag = new Dictionary<int, string>();
                var carModelList = _serialFourthStageBll.MakeCarList(serialId);

                var targetList = carModelList.Take(topCount);

                var carInfoForSerialSummaryEntities = targetList as CarInfoForSerialSummaryEntity[] ??
                                                      targetList.ToArray();

                var carids = carInfoForSerialSummaryEntities.Select(p => p.CarID);

                CarParamDictionary = _carBll.GetCarParamValueByCarIds(carids.ToArray(), 724);
                //购置税减免批次
                var dictPurchaseTaxParamN = _carBll.GetCarParamValueByCarIds(carids.ToArray(), 987);
                //购置税减免
                var dictPurchaseTaxParam = _carBll.GetCarParamValueByCarIds(carids.ToArray(), 986);

                foreach (var item in carInfoForSerialSummaryEntities)
                {
                    if (CarParamDictionary != null && CarParamDictionary.ContainsKey(item.CarID))
                    {
                        var paramValue = CarParamDictionary[item.CarID];
                        item.UnderPan_ForwardGearNum = (!string.IsNullOrEmpty(paramValue) && paramValue != "无级" &&
                                                        paramValue != "待查")
                            ? paramValue
                            : "";
                    }
                    else
                    {
                        item.UnderPan_ForwardGearNum = (!string.IsNullOrEmpty(item.UnderPan_ForwardGearNum) &&
                                                        item.UnderPan_ForwardGearNum != "无级" &&
                                                        item.UnderPan_ForwardGearNum != "待查")
                            ? item.UnderPan_ForwardGearNum
                            : "";
                    }
                    //减税 免征
                    double dEx = 0.0;
                    Double.TryParse(item.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                    if (dictPurchaseTaxParamN.ContainsKey(item.CarID) && (dictPurchaseTaxParamN[item.CarID] == "第1批" || dictPurchaseTaxParamN[item.CarID] == "第2批" || dictPurchaseTaxParamN[item.CarID] == "第3批" || dictPurchaseTaxParamN[item.CarID] == "第4批" || dictPurchaseTaxParamN[item.CarID] == "第5批" || dictPurchaseTaxParamN[item.CarID] == "第6批") && dictPurchaseTaxParam.ContainsKey(item.CarID))
                    {
                        if (dictPurchaseTaxParam[item.CarID] == "减半")
                        {
                            if (!dictCarTaxTag.ContainsKey(item.CarID))
                            {
                                dictCarTaxTag.Add(item.CarID, "减税");
                            }
                        }
                        else if (dictPurchaseTaxParam[item.CarID] == "免征")
                        {
                            if (!dictCarTaxTag.ContainsKey(item.CarID))
                            {
                                dictCarTaxTag.Add(item.CarID, "免税");
                            }
                        }
                    }
                    else if (dEx > 0 && dEx <= 1.6)
                    {
                        if (!dictCarTaxTag.ContainsKey(item.CarID))
                        {
                            dictCarTaxTag.Add(item.CarID, "减税");
                        }
                    }
                }

                var serializeObject = JsonConvert.SerializeObject(new
                {
                    count = carModelList.Count,
                    carlist = carInfoForSerialSummaryEntities,
                    taxtag = dictCarTaxTag
                });

                CacheManager.InsertCache(cacheKey, serializeObject, WebConfig.CachedDuration);

                context.Response.Write(serializeObject);
            }

            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}