using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetCarList 的摘要说明
    /// </summary>
    public class GetCarList :PageBase, IHttpHandler
    {
        //访问格式：http://api.car.bitauto.com/WxApp/GetCarList.ashx?sid=1765

        private HttpRequest request;
        private HttpResponse response;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            request = context.Request;
            response = context.Response;
            RenderCar();
        }
        private void RenderCar()
        {
            try
            {
                int serialId = ConvertHelper.GetInteger(request.QueryString["sid"]);
                string cacheKey = "Car_WxApp_CarDataJson_Car_" + serialId;
                var obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    response.Write((string)obj);
                    return;
                }

                //var serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                var serialCarList = new List<CarInfoForSerialSummaryEntity>();
                //List<EnumCollection.CarInfoForSerialSummary> saleCarInfo = new List<EnumCollection.CarInfoForSerialSummary>();
                //if (serialEntity.SaleState == "停销")
                //{
                //    // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
                //    serialCarList = GetAllCarInfoForNoSaleSerialSummaryByCsID(serialId);
                //}
                //else
                //{
                // 非停销子品牌取 子品牌的非停销所有年款车型
                serialCarList = new Car_BasicBll().GetCarListForSerialSummaryBySerialId(serialId, false);
                //}
                //string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

                List<string> groupJsonList = new List<string>();
                var group = serialCarList.GroupBy(p => new { Year = p.CarYear }, p => p);
                //按年分组进行遍历
                foreach (var g in group.OrderByDescending(x => x.Key.Year))
                {
                    var key = CommonFunction.Cast(g.Key, new { Year = "" });
                    List<string> carYearJsonList = new List<string>();
                    //var engineExhaustGroup = g.ToList().GroupBy(p => new { Engin_Exhaust = p.Engine_Exhaust }, p => p)
                    var engineExhaustGroup = g.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p); //先按排量再按功率对车款进行分组:  2.0L/138kw , 2.0L/108kw
                    //按排量分组遍历
                    foreach (var engineExhaust in engineExhaustGroup)
                    {
                        Dictionary<string, List<string>> dictEngintPowerGroup = new Dictionary<string, List<string>>(); //1.5L/90kw carjson1; 1.5L/109kw carjson2;
                        //相同排量下，按功率大小分组遍历
                        foreach (var entity in engineExhaust.ToList())
                        {
                            string engine_Exhaust = entity.Engine_Exhaust;
                            string transmissionType = entity.TransmissionType; //变速箱类型
                            string carReferPrice = entity.ReferPrice; //指导价
                            string carMinPrice = string.Empty;
                            string carPriceRange = entity.CarPriceRange.Trim(); //售价区间
                            if (entity.CarPriceRange.Trim().Length == 0)
                                carMinPrice = "暂无报价";    
                            
                            else
                            {
                                if (carPriceRange.IndexOf('-') != -1)
                                    carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-'));
                                else
                                    carMinPrice = carPriceRange;
                            }

                            //组织json,按相同排量不同功率分组插入到字典中

                            string curEnginExhasutAndPower = string.Empty;
                            if (entity.Oil_FuelType == "纯电")
                            {
                                curEnginExhasutAndPower = "电动车/"+ entity.Electric_Peakpower + "kw";
                            }
                            else if (entity.Oil_FuelType == "油电混合" || entity.Oil_FuelType == "插电混合")
                            {
                                curEnginExhasutAndPower = engine_Exhaust + "/" + entity.Engine_MaxPower + "kw-" + entity.Electric_Peakpower + "kw";
                            }
                            else
                            {
                                curEnginExhasutAndPower = entity.Engine_MaxPower == 9999 ? engine_Exhaust : engine_Exhaust + "/" + entity.Engine_MaxPower + "kw";
                            }

                            string singleCarJson = string.Format(
                                "{{\"CarId\":{0},\"CarName\":\"{1}\",\"Price\":\"{2}\",\"ReferPrice\":\"{3}\",\"TransmissionType\":\"{4}\",\"Engine_Exhaust\":\"{5}\", \"Engine_MaxPower\":\"{6}\",\"Electric_Peakpower\":\"{7}\",\"GearNum\":\"{8}\"}}",
                                entity.CarID, entity.CarName, carMinPrice.Replace("万",""), carReferPrice, transmissionType,
                                engine_Exhaust, entity.Engine_MaxPower, entity.Electric_Peakpower, string.IsNullOrEmpty(entity.UnderPan_ForwardGearNum) ?"": entity.UnderPan_ForwardGearNum + "挡");
                            List<string> lsPowerCar = new List<string>();
                            lsPowerCar.Add(singleCarJson);
                            if (dictEngintPowerGroup.ContainsKey(curEnginExhasutAndPower))
                            {
                                dictEngintPowerGroup[curEnginExhasutAndPower].Add(singleCarJson);
                            }
                            else
                            {
                                dictEngintPowerGroup.Add(curEnginExhasutAndPower, lsPowerCar);
                            }
                            //lsPowerCar.Clear();
                        }
                        foreach (var powerKey in dictEngintPowerGroup.Keys)
                        {
                            carYearJsonList.Add(string.Format("{{\"{0}\":[{1}]}}", powerKey, string.Join(",", dictEngintPowerGroup[powerKey].ToArray())));
                        }
                    }
                    groupJsonList.Add(string.Format("{{\"s{0}\":[{1}]}}", key.Year, string.Join(",", carYearJsonList.ToArray())));
                }
                var json = string.Format("{{\"CarList\":[{0}]}}", string.Join(",", groupJsonList.ToArray()));
                CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
                response.Write(json);
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString() + ";StackTrace:" + ex.StackTrace);
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