using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System.Data;
using System.Web.UI;
using BitAuto.CarChannel.BLL;
using System.Text;

namespace MWeb.Controllers
{
    public class CsCompareController : Controller
    {
        //
        // GET: /CsCompare/  
        private PageBase pageBase;
        private SerialEntity se;
        protected int csID = 0;
        private int maxCount = 40;
        protected string carIDs = string.Empty;
        //protected string carIDAndName = string.Empty;
        //protected string carBaseInfo = string.Empty;
        Dictionary<string, SortedDictionary<string, List<int>>> baseInfoDic = new Dictionary<string, SortedDictionary<string, List<int>>>();
        List<int> carIdList = new List<int>();

        private Car_SerialBll carSerialBll = new Car_SerialBll();
        private Car_BasicBll carBasicBll = new Car_BasicBll();

        public CsCompareController()
        {
            pageBase = new PageBase();
            se=new SerialEntity();
        }
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream,VaryByParam = "*")]
        public ActionResult Index()
        {
          
            GetPageParam(RouteData.Values);
            this.GetCarIDByCsID();
			if (se == null || se.Id <= 0)
			{
				Response.Redirect("/error", true);
				return new EmptyResult();
			}
            GetSerialOptionalPackageData();
            ViewBag.CsHeadHTML = pageBase.GetCommonNavigation("MCsCompare", csID);
            ViewBag.CsId = csID;

            return View(se);
        }
        private void GetPageParam(RouteValueDictionary values)
        {
            csID = ConvertHelper.GetInteger(values["id"]);
        }
        private void GetCarIDByCsID()
        {
            if (csID > 0)
            {
                se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
				if (se == null || se.Id <= 0)
				{
					//Response.Redirect("/error", true);
					return;
				}
                DataSet ds = new DataSet();
                if (se.SaleState == "停销")
                {
                    // 停销子品牌取最新年款的车型
                    ds = pageBase.GetCarIDAndNameForNoSaleCS(csID, WebConfig.CachedDuration);
                }
                else
                {
                    ds = pageBase.GetCarIDAndNameForCS(csID, WebConfig.CachedDuration);
                }
                
                baseInfoDic.Add("EngineExhaust",new SortedDictionary<string, List<int>>());//排量
                baseInfoDic.Add("YearType", new SortedDictionary<string, List<int>>());//年款
                baseInfoDic.Add("BodyType", new SortedDictionary<string, List<int>>());//车身形式
                baseInfoDic.Add("SeatNum", new SortedDictionary<string, List<int>>());//座位数
                baseInfoDic.Add("TransmissionType", new SortedDictionary<string, List<int>>());//变速箱类型
                baseInfoDic.Add("DriveType", new SortedDictionary<string, List<int>>());//驱动方式：前驱、后驱、四驱
                baseInfoDic.Add("FuelType", new SortedDictionary<string, List<int>>());//能源类型
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int carId = ConvertHelper.GetInteger(ds.Tables[0].Rows[i]["car_id"]);
                        carIdList.Add(carId);

                        //string engineExhaust = ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString();
                        string yearType = ds.Tables[0].Rows[i]["Car_YearType"].ToString();
                        
                        if (baseInfoDic["YearType"].ContainsKey(yearType))
                        {
                            baseInfoDic["YearType"][yearType].Add(carId);
                        }
                        else
                        {
                            baseInfoDic["YearType"].Add(yearType, new List<int>() { carId });
                        }
                    }
                    GetCarParam();
                    ViewBag.CarIds = string.Join(",",carIdList);
                }
            }
        }

        /// <summary>
        /// 获取车款基本信息
        /// </summary>
        private void GetCarParam()
        {
            Dictionary<int, Dictionary<string, string>> carParams = carBasicBll.GetCarCompareDataWithOptionalByCarIDs(carIdList);
            foreach (int carId in carIdList)
            {
                if (!carParams.ContainsKey(carId)) continue;
                string engine_ExhaustForFloat = string.Empty;//排量Engine_ExhaustForFloat
                string engine_InhaleType = string.Empty;//进气形式Engine_InhaleType
                string bodyType = string.Empty;//车身形式 Body_Type
                string fuelType = string.Empty;//燃料类型 Oil_FuelType
                string seatNum = string.Empty;//座位数 Perf_SeatNum
                string driveType = string.Empty;//驱动方式Perf_DriveType
                string transmissionType = string.Empty;//变速箱类型 UnderPan_TransmissionType


                if (carParams[carId].ContainsKey("Engine_InhaleType"))
                {
                    engine_InhaleType = carParams[carId]["Engine_InhaleType"];
                }
                if (carParams[carId].ContainsKey("Engine_ExhaustForFloat"))
                {
                    engine_ExhaustForFloat = carParams[carId]["Engine_ExhaustForFloat"];
                    if (!string.IsNullOrEmpty(engine_ExhaustForFloat))
                    {
                        if (engine_InhaleType.IndexOf("增压") > -1)
                        {
                            engine_ExhaustForFloat += "T";
                        }
                        else
                        {
                            engine_ExhaustForFloat += "L";
                        }
                    }
                }
                if (carParams[carId].ContainsKey("Body_Type"))
                {
                    bodyType = carParams[carId]["Body_Type"];
                }
                if (carParams[carId].ContainsKey("Oil_FuelType"))
                {
                    fuelType = carParams[carId]["Oil_FuelType"];
                }
                if(carParams[carId].ContainsKey("UnderPan_TransmissionType"))
                {
                    transmissionType = carParams[carId]["UnderPan_TransmissionType"];
                }
                if (carParams[carId].ContainsKey("Perf_SeatNum"))
                {
                    seatNum = carParams[carId]["Perf_SeatNum"];
                }
                if (carParams[carId].ContainsKey("Engine_InhaleType"))
                {
                    driveType = carParams[carId]["Perf_DriveType"];
                    if (driveType.IndexOf("前轮") > -1)
                    {
                        driveType = "前驱";
                    }
                    else if(driveType.IndexOf("后轮") > -1)
                    {
                        driveType = "后驱";
                    }
                    else if (driveType.IndexOf("四驱") > -1)
                    {
                        driveType = "四驱";
                    }
                }

                if (baseInfoDic["EngineExhaust"].ContainsKey(engine_ExhaustForFloat))
                {
                    baseInfoDic["EngineExhaust"][engine_ExhaustForFloat].Add(carId);
                }
                else
                {
                    baseInfoDic["EngineExhaust"].Add(engine_ExhaustForFloat, new List<int>() { carId });
                }

                if (baseInfoDic["BodyType"].ContainsKey(bodyType))
                {
                    baseInfoDic["BodyType"][bodyType].Add(carId);
                }
                else
                {
                    baseInfoDic["BodyType"].Add(bodyType, new List<int>() { carId });
                }
                if (baseInfoDic["SeatNum"].ContainsKey(seatNum))
                {
                    baseInfoDic["SeatNum"][seatNum].Add(carId);
                }
                else
                {
                    baseInfoDic["SeatNum"].Add(seatNum, new List<int>() { carId });
                }

                if (baseInfoDic["TransmissionType"].ContainsKey(transmissionType))
                {
                    baseInfoDic["TransmissionType"][transmissionType].Add(carId);
                }
                else
                {
                    baseInfoDic["TransmissionType"].Add(transmissionType, new List<int>() { carId });
                }

                if (baseInfoDic["DriveType"].ContainsKey(driveType))
                {
                    baseInfoDic["DriveType"][driveType].Add(carId);
                }
                else
                {
                    baseInfoDic["DriveType"].Add(driveType, new List<int>() { carId });
                }
                if (baseInfoDic["FuelType"].ContainsKey(fuelType))
                {
                    baseInfoDic["FuelType"][fuelType].Add(carId);
                }
                else
                {
                    baseInfoDic["FuelType"].Add(fuelType, new List<int>() { carId });
                }
                
            }
            //StringBuilder sb = new StringBuilder("var SelectJson = [");
            List<string> list = new List<string>();
            list.Add("var SelectJson = {");
            foreach (KeyValuePair<string, SortedDictionary<string, List<int>>> kv in baseInfoDic)
            {
                string key = kv.Key;
                list.Add("\"" + kv.Key + "\":{");
                foreach (KeyValuePair<string, List<int>> kv1 in kv.Value)
                {
                    if (string.IsNullOrEmpty(kv1.Key)) continue;
                    list.Add(string.Format("\"{0}\":[{1}],",kv1.Key,string.Join(",",kv1.Value)));
                }
                list[list.Count - 1] = list[list.Count - 1].Trim(',');
                list.Add("},");
            }
            list[list.Count - 1] = list[list.Count - 1].Trim(',');
            list.Add("};");
            //sb.Append("];");
            ViewBag.SelectJson = string.Join("", list);
        }

        /// <summary>
        /// 获取车系选配包json
        /// </summary>
        private void GetSerialOptionalPackageData()
        {
            ViewData["PackageJsContent"] = carSerialBll.GetSerialOptionalPackageJson(csID);
        }
        #region  应王淞要求，下线参配测试车系3524、3814
        //public ActionResult Index3524()
        //{
        //    csID = 3524;
        //    ViewBag.CsHeadHTML = pageBase.GetCommonNavigation("MCsCompare", csID);
        //    ViewBag.CsId = csID;
        //    se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
        //    if (se == null || se.Id <= 0)
        //        Response.Redirect("/error",true);
        //    return View(se);
        //}
        //public ActionResult Index3814()
        //{
        //    csID = 3814;
        //    ViewBag.CsHeadHTML = pageBase.GetCommonNavigation("MCsCompare", csID);
        //    ViewBag.CsId = csID;
        //    se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
        //    if (se == null || se.Id <= 0)
        //        Response.Redirect("/error", true);
        //    return View(se);
        //}
        #endregion
    }
}
