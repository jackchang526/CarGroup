using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WirelessWeb
{
    public partial class CalcAutoLoan :WirelessPageBase
    {
        protected int carId = 0;
        protected double carReferPrice = 0;
        protected string title = string.Empty;
        //protected string serialName = string.Empty;
        CarEntity ce;
        protected int carSerialId = 0;
        protected int carBrandId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            base.SetPageCache(10);
            GetParamter();
            InitData();
        }
        private void GetParamter()
        {
            carId = ConvertHelper.GetInteger(Request.QueryString["carid"]);
        }

        private void InitData()
        {
            if (carId > 0)
            {
                ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (ce != null && ce.Id > 0)
                {
                    carReferPrice = Math.Round(ce.ReferPrice * 10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial.Brand.MasterBrandId;
                    //serialName = ce.Serial.ShowName;
                    title = string.Format("{0}款 {1} {2}", ce.CarYear, ce.Serial.ShowName, ce.Name);
                }
            }
        }
    }
}