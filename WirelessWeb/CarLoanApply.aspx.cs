using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;

namespace WirelessWeb
{
    public partial class CarLoanApply : WirelessPageBase
    {
        protected int MasterId { get; set; }

        protected int SerialId { get; set; }

        protected int CarId { get; set; }

        protected SerialEntity SerialInfo { get; set; }

        protected CarEntity CarInfo { get; set; }

        protected Car_BasicBll CarBll { get; set; }

        protected List<CarInfoForSerialSummaryEntity> CarList = new List<CarInfoForSerialSummaryEntity>();

        public HashSet<int> AdCsIds = AdConfig.AdCsIds;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageCache(30);
        }

        protected override void OnInit(EventArgs e)
        {
            int serialId;
            if (int.TryParse(Request.QueryString["csid"], out serialId))
            {
                this.SerialId = serialId;
            }

            CarBll = new Car_BasicBll();

            int carId;
            if (int.TryParse(Request.QueryString["carid"], out carId))
            {
                this.CarId = carId;
            }

            this.SerialInfo = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);

            if (this.SerialInfo == null)
            {
                this.Response.Redirect("/404error.aspx");
                return;
            }

            this.MasterId = this.SerialInfo.Brand.MasterBrand.Id;

            if (this.CarId > 0)
            {
                CarInfo = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarId);
            }

            CarList = CarBll.GetCarInfoForSerialSummaryBySerialId(this.SerialId).Where(x => x.SaleState == "在销" && !string.IsNullOrWhiteSpace(x.ReferPrice)).ToList();

            if (CarInfo == null && CarList.Count > 0)
            {
                var carInfo = CarList.OrderByDescending(x => x.CarPV).FirstOrDefault();

                CarInfo = new CarEntity() { Id = carInfo.CarID, Name = carInfo.CarName };
            }

            base.OnInit(e);
        }
    }
}