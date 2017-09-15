using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// NewCarList 的摘要说明 
    /// </summary>
    public class NewCarList :PageBase, IHttpHandler
    {
        HttpResponse response;
        private Car_SerialBll _serialBLL;
        private Car_BasicBll _carBLL;
    
        public void ProcessRequest(HttpContext context)
        { 
            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            _serialBLL = new Car_SerialBll();
            _carBLL = new Car_BasicBll();
            GetNewCarList();
        }
        /// <summary>
        /// 取上市新车逻辑:按照倒序时间，取30台车
        /// </summary>
        private void GetNewCarList()
        {
            try
            {
                // 数据缓存1小时
                string jsonResult = string.Empty;
                string cacheKey = "SerialBaseInfo_GetTop30NewCarList";
                object obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    jsonResult = obj.ToString();
                }
                else
                {
                    #region 查询数据
                    string sql = @"select top 30 * from (
                               select  TT.*, ROW_NUMBER() over(partition by cs_id order by convert(int,pyear) DESC,convert(int,pmonth) DESC,convert(int,pday) desc) as rownum from (
                                    select  cs.cs_id,cs.csname,cs.csshowname,cl.classvalue as carLevel,cs.allspell
							        ,car.car_id,car.car_name,car.car_ReferPrice
							        ,cdb1.pvalue as pyear,cdb2.pvalue as pmonth,cdb3.pvalue as pday
							        from car_relation car 
							        left join car_serial cs on car.cs_id=cs.cs_id
							        left join class cl on cs.carlevel=cl.classid
							        left join cardatabase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=385
							        left join cardatabase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=384
							        left join cardatabase cdb3 on car.car_id=cdb3.carid and cdb3.paramid=383
							        where car.isstate=0 and cs.isstate=0 and isnumeric(cdb2.pvalue)>=1 and isnumeric(cdb3.pvalue)>=1 
					            ) as TT 
					            ) as T where T.rownum=1 order by convert(int,T.pyear) desc,convert(int,T.pmonth) desc,convert(int,T.pday) desc";
                    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                        , CommandType.Text, sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        // 白底
                        Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                        List<string> jsonString = new List<string>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            int csid = int.Parse(dr["cs_id"].ToString());
                            string csName = dr["csname"].ToString().Trim();
                            string csShowName = dr["csshowname"].ToString().Trim();
                            string csLevel = dr["carLevel"].ToString().Trim();
                            string csAllspell = dr["allspell"].ToString().Trim();
                            int pyear = int.Parse(dr["pyear"].ToString());
                            int pmonth = int.Parse(dr["pmonth"].ToString());
                            int pday = int.Parse(dr["pday"].ToString());
                            int carid = int.Parse(dr["car_id"].ToString());
                            string carName = dr["car_name"].ToString().Trim();
                            EnumCollection.SerialInfoCard serialInfo = _serialBLL.GetSerialInfoCard(csid);
                            string baojiaRange = serialInfo.CsPriceRange; //报价区间
                            string enginExHaust = serialInfo.CsEngineExhaust; //排量
                            string transmissionType = serialInfo.CsTransmissionType; //变速箱类型
                            string fuelCost = serialInfo.CsSummaryFuelCost; //油耗
                            #region 处理 " 合资紧凑级 "
                            SerialEntity curSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);  //获取当前子品牌信息
                            string carSerialLevel = string.Empty; //级别
                            carSerialLevel = _serialBLL.GetSerialTotalPV(csid);
                            if (!string.IsNullOrEmpty(carSerialLevel))
                            {
                                carSerialLevel = carSerialLevel.Substring(0, carSerialLevel.IndexOf("第"));
                            }
                            string chanDi = string.Empty; //进口or合资
                            if (curSerialEntity != null)
                            {
                                string masterCountry = curSerialEntity.Brand.MasterBrand.Country; //主品牌国别
                                string producerCountry = curSerialEntity.Brand.Country; //厂商国别
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
                            List<CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(csid);
                            List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList.FindAll(p => p.SaleState == "在销");
                            var fuelTypeList = carinfoList.Where(p => p.Oil_FuelType != "")
                                .GroupBy(p => p.Oil_FuelType)
                                .Select(g => g.Key).ToList();
                            bool isElectrombile = fuelTypeList.Count == 1 && fuelTypeList[0] == "纯电" ? true : false;

                            string oneRowText = string.Empty;
                            if (isElectrombile)
                            {
                                oneRowText = GetElecticParamsText(carinfoList);
                            }
                            else
                            {
                                //取在销车型的排量、变速箱数据
                                List<string> lsEnginExhaust = new List<string>();
                                var groupEnginExhaust = carinfoSaleList.GroupBy(p => new { EnginExhaust = p.Engine_Exhaust }, p => p);
                                foreach (var g in groupEnginExhaust)
                                {
                                    //var key = CommonFunction.Cast(g.Key, new { Year = "" });
                                    lsEnginExhaust.Add(g.Key.EnginExhaust.ToString());
                                }
                                List<string> lsTransType = new List<string>();
                                var groupTransType = carinfoSaleList.GroupBy(p => new { TransType = p.TransmissionType }, p => p);
                                foreach (var g in groupTransType)
                                {
                                    lsTransType.Add(g.Key.TransType.ToString());
                                }
                                oneRowText = string.Format("\"isElectrombile\":\"false\",\"enginExHaust\":\"{0}\",\"transmissionType\":\"{1}\",\"fuelCost\":\"{2}\"",
                                    string.Join(" ", lsEnginExhaust), string.Join(" ", transmissionType), fuelCost);
                            }
                            string whitePic = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                            jsonString.Add(string.Format("{{\"carId\":{0},\"csId\":{1},\"csShowName\":\"{2}\",\"whitePic\":\"{3}\",\"baojiaRange\":\"{4}\",\"Year\":{5},\"Month\":{6},\"Day\":{7},{8},\"producerAndLevel\":\"{9}\"}}",
                                carid, csid, csShowName, whitePic.Replace("_2.jpg", "_6.jpg"), baojiaRange, pyear, pmonth, pday, oneRowText, chanDi + carSerialLevel));
                        }
                        jsonResult = string.Format("{{\"CarList\":[{0}]}}", string.Join(",", jsonString.ToArray()));
                        CacheManager.InsertCache(cacheKey, jsonResult, 60);
                    }
                    #endregion
                }
                response.Write(jsonResult);
            }
            catch(Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString()+";StackTrace:"+ex.StackTrace);
            }
        }
        private string GetElecticParamsText(List<CarInfoForSerialSummaryEntity> carinfoList)
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