using System;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace H5Web.V2.module.broker
{
    public partial class Agent : H5PageBase
    {
        //子品牌ID
        protected int SerialId = 0;
        // 经纪人ID
        protected int Brokerid = 0;

        protected SerialEntity BaseSerialEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["brokerid"] != null)
            {
                int.TryParse(Request.QueryString["brokerid"], out Brokerid);
            }

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }
    }
}