using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace H5Web.V2.module.dealer
{
    public partial class SerialMap : H5PageBase
    {
        //城市
        protected int CityId = 0;
        //车系ID
        protected int SerialId = 0;

        protected SerialEntity BaseSerialEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["cityid"] != null)
            {
                int.TryParse(Request.QueryString["cityid"], out CityId);
            }

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }
    }
}