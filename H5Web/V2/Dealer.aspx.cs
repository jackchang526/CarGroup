using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class Dealer : H5PageBase
    {
        //子品牌ID
        protected int SerialId = 0;
        // 经纪人ID
        protected int Brokerid = 0;

        protected SerialEntity BaseSerialEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            Brokerid = ConvertHelper.GetInteger(Request.QueryString["brokerid"]);
            SerialId = ConvertHelper.GetInteger(Request.QueryString["csid"]);
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }
    }
}