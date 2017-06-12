using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace WirelessWeb
{
	public partial class CalcInsuranceTool : WirelessPageBase
	{
        //protected int _carPrice = 0;
        //protected int _compulsoryIdx = 0;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    base.SetPageCache(60);
        //    GetParams();
        //}

        //private void GetParams()
        //{
        //    _carPrice = ConvertHelper.GetInteger(Request.QueryString["CarPrice"]);
        //    _compulsoryIdx = ConvertHelper.GetInteger(Request.QueryString["CompulsoryIdx"]);
        //    _compulsoryIdx = _compulsoryIdx > 1 || _compulsoryIdx < 0 ? 0 : _compulsoryIdx;
        //}


        protected int carId = 0;
        protected string carName = string.Empty;
        protected string serialName = string.Empty;
        protected double carReferPrice = 0;
        protected string yearType = string.Empty;
        protected string title = string.Empty;
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
                    carReferPrice = Math.Round(ce.ReferPrice*10000);
                    carSerialId = ce.SerialId;
                    carBrandId = ce.Serial.Brand.MasterBrandId;
                    serialName = ce.Serial.ShowName;
                    title = string.Format("{0}款 {1} {2}", ce.CarYear, ce.Serial.ShowName, ce.Name);
                }
            }
        }
	}
}