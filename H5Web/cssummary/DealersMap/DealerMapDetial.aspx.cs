using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;

namespace H5Web.cssummary.DealersMap
{
    public partial class DealerMapDetial : H5PageBase
    {
        protected int SerialId = 0;
        protected int DealerId = 0;
        protected int CityId = 0;
        protected SerialEntity BaseSerialEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            GetParam();
            InitData();
        }

        private void GetParam()
        {
            if (!string.IsNullOrEmpty(Request["csID"]))
                SerialId = Convert.ToInt32(Request["csID"]);
            if (!string.IsNullOrEmpty(Request["dealerid"]))
                DealerId = Convert.ToInt32(Request["dealerid"]);
            if (!string.IsNullOrEmpty(Request["cityid"]))
                CityId = Convert.ToInt32(Request["cityid"]);
        }

        private void InitData()
        {
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }
    }
}