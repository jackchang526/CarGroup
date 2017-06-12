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
        protected string carIDAndName = string.Empty;
        
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
                    Response.Redirect("/error", true);
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
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i > maxCount - 1)
                        {
                            break;
                        }
                        if (carIDAndName != "" && carIDAndName.Length > 0)
                        {
                            carIDAndName += "|" + "id" + ds.Tables[0].Rows[i]["car_id"].ToString() + "," + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["TransmissionType"].ToString().Trim();
                            carIDs += "," + ds.Tables[0].Rows[i]["car_id"].ToString();
                        }
                        else
                        {
                            carIDAndName = "id" + ds.Tables[0].Rows[i]["car_id"].ToString() + "," + ds.Tables[0].Rows[i]["car_name"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["Engine_Exhaust"].ToString().Trim() + "," + ds.Tables[0].Rows[i]["TransmissionType"].ToString().Trim();
                            carIDs = ds.Tables[0].Rows[i]["car_id"].ToString();
                        }
                    }
                    ViewBag.CarIds = carIDs;
                }
            }
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
