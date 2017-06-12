using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetSerialBaseInfo 的摘要说明
    /// </summary>
    public class GetSerialBaseInfo :PageBase, IHttpHandler
    {
        //访问格式：http://api.car.bitauto.com/WxApp/GetSerialBaseInfo.ashx?sid=1765
        
        private int serialId = 0;
        HttpResponse response;

        private Car_SerialBll _serialBLL;
        private Car_BasicBll _carBLL;
        private bool isElectrombile;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            _serialBLL=new Car_SerialBll();
            _carBLL=new Car_BasicBll();
            GetPageParam(context);
            GetCsJsonData();
        }

        private void GetPageParam(HttpContext context)
        {
            string strCsID = context.Request.QueryString["sid"];
            if (!string.IsNullOrEmpty(strCsID) && int.TryParse(strCsID, out serialId))
            { }
        }

     
        private void GetCsJsonData()
        {
            try
            {
                if (serialId > 0)
                {
                    string cacheKey = "Car_WxApp_GetSerialBaseInfo_" + serialId;
                    var obj = CacheManager.GetCachedData(cacheKey);
                    if (obj != null)
                    {
                        response.Write((string)obj);
                        return;
                    }

                    EnumCollection.SerialInfoCard serialInfo = _serialBLL.GetSerialInfoCard(serialId);
                    string csShowName = serialInfo.CsShowName;
                    string baojiaRange = serialInfo.CsPriceRange; //报价区间
                    string enginExHaust = serialInfo.CsEngineExhaust; //排量
                    string transmissionType = serialInfo.CsTransmissionType; //变速箱类型
                    string fuelCost = serialInfo.CsSummaryFuelCost; //油耗

                    List<CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(serialId);
                    //List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList.FindAll(p => p.SaleState == "在销");
                    var fuelTypeList = carinfoList.Where(p => p.Oil_FuelType != "")
                        .GroupBy(p => p.Oil_FuelType)
                        .Select(g => g.Key).ToList();
                    isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "电力" ? true : false;

                    string oneRowText = string.Empty;
                    if (isElectrombile)
                    {
                        oneRowText = GetElectricOneRowText(carinfoList);
                    }
                    else
                    {
                        oneRowText = string.Format("\"isElectrombile\":\"false\",\"enginExHaust\":\"{0}\",\"transmissionType\":\"{1}\",\"fuelCost\":\"{2}\"", enginExHaust, transmissionType, fuelCost);
                    }

                    //取当前子品牌关联的主品牌id(用于获取品牌logo)
                    SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                    int masterId = serialEntity.Brand.MasterBrandId;
                    int brandId = serialEntity.BrandId;

                    #region 处理 " 合资紧凑级 "
                    string carSerialLevel = string.Empty; //级别
                    carSerialLevel = _serialBLL.GetSerialTotalPV(serialId);
                    if (!string.IsNullOrEmpty(carSerialLevel))
                    {
                        carSerialLevel = carSerialLevel.Substring(0, carSerialLevel.IndexOf("第"));
                    }
                    string chanDi = string.Empty; //进口 or 合资
                    if (serialEntity != null)
                    {
                        string masterCountry = serialEntity.Brand.MasterBrand.Country; //主品牌国别
                        string producerCountry = serialEntity.Brand.Country; //厂商国别
                        if (masterCountry == producerCountry)
                        {
                            chanDi = producerCountry;
                            if (masterCountry.Contains("中国"))
                            {
                                chanDi = "自主";
                            }
                            else
                            {
                                chanDi = "进口";
                            }
                        }
                        else
                        {
                            chanDi = "合资";
                        }
                    }
                    #endregion
                    //取子品牌白底图
                    Dictionary<int, string> dicPicWhite = GetAllSerialPicURLWhiteBackground();
                    string whitePic = dicPicWhite.ContainsKey(serialId) ? dicPicWhite[serialId] : WebConfig.DefaultCarPic;
                    string jsonResult = string.Empty;
                    jsonResult = "{";
                    jsonResult += string.Format("\"masterId\":{0},\"brandId\":{1},\"csShowName\":\"{2}\",\"whitePic\":\"{3}\",\"baojiaRange\":\"{4}\",\"producerAndLevel\":\"{5}\",{6}", masterId, brandId, csShowName, whitePic.Replace("_2.jpg", "_6.jpg"), baojiaRange, chanDi + carSerialLevel, oneRowText);
                    jsonResult += "}";
                    CacheManager.InsertCache(cacheKey, jsonResult, WebConfig.CachedDuration);
                    response.Write(jsonResult);
                }
            }
            catch(Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString() + ";StackTrace:" + ex.StackTrace);
            }
        }

        private string GetElectricOneRowText(List<CarInfoForSerialSummaryEntity> carinfoList)
        { 
            //以下2个参数为电动车数据，用于组织json返回
            string chargeTimeRange = string.Empty; //续航里程
            string mileageRange = string.Empty; //充电时间

            int minChargeTime = 0;
            int maxChargeTime = 0;
            int minMileage = 0;
            int maxMileage = 0;
            foreach (CarInfoForSerialSummaryEntity entity in carinfoList)
            {
                Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);
                //add by 2014.05.04 获取电动车参数
                //普通充电时间
                if (dictCarParams.ContainsKey(879))
                {
                    var chargeTime = ConvertHelper.GetInteger(dictCarParams[879]);
                    if (minChargeTime == 0 && chargeTime > 0)
                        minChargeTime = chargeTime;
                    if (chargeTime < minChargeTime)
                        minChargeTime = chargeTime;
                    if (chargeTime > maxChargeTime)
                        maxChargeTime = chargeTime;
                }
                //纯电最高续航里程
                if (dictCarParams.ContainsKey(883))
                {
                    var mileage = ConvertHelper.GetInteger(dictCarParams[883]);
                    if (minMileage == 0 && mileage > 0)
                        minMileage = mileage;
                    if (mileage < minMileage)
                        minMileage = mileage;
                    if (mileage > maxMileage)
                        maxMileage = mileage;
                }
            }
            if (maxChargeTime > 0)
            {
                chargeTimeRange = minChargeTime == maxChargeTime
                    ? string.Format("{0}分钟", minChargeTime)
                    : string.Format("{0}-{1}分钟", minChargeTime, maxChargeTime);
            }
            if (maxMileage > 0)
            {
                mileageRange = minMileage == maxMileage
                    ? string.Format("{0}公里", minMileage)
                    : string.Format("{0}-{1}公里", minMileage, maxMileage);
            }
            string oneRowText = string.Format("\"isElectrombile\":\"true\",\"enginExHaust\":\"{0}\",\"transmissionType\":\"{1}\",\"fuelCost\":\"免购置税\"", chargeTimeRange, mileageRange);  //保持接口输出参数名一致

            return oneRowText;
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