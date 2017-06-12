using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageToolV2
{
    public partial class CalcAutoCashTool : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(60);
            var carPrice = Request.QueryString["CarPrice"];
            if (carPrice != null)
            {
                int result = 0;
                if (Int32.TryParse(carPrice, out result))
                {
                    hidCarPrice.Value = result.ToString();
                }
            }

            int carId = ConvertHelper.GetInteger(Request.QueryString["carID"]);
            int serialId = ConvertHelper.GetInteger(Request.QueryString["serialId"]);
            int masterId = 0;
            if (carId > 0)
            {
                base.GetIDsByCarID(carId, out masterId, out serialId);
                CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                hidCarPrice.Value = carEntity.ReferPrice.ToString();
            }
            else if (serialId > 0)
            {
                SerialEntity se = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                masterId = se.Brand.MasterBrandId;
            }
            if (carId == 0)
                carId = -1;
            if (serialId == 0)
                serialId = -1;
            if (masterId == 0)
                masterId = -1;
            hidCarID.Value = carId.ToString();
            hidCsID.Value = serialId.ToString();
            hidBsID.Value = masterId.ToString();

            // 传经销商报价
            if (!String.IsNullOrEmpty(this.Request.QueryString["carPrice"]))
            {
                float tempCarPriceFloat = 0;
                string tempCarPrice = this.Request.QueryString["carPrice"].ToString().Trim();
                if (float.TryParse(tempCarPrice, out tempCarPriceFloat))
                {
                    carPrice = Convert.ToString(Math.Round(tempCarPriceFloat / 10000, 2));
                    if (carPrice.Length > 12)
                    { carPrice = ""; }
                }
            }
            

        }
    }
}