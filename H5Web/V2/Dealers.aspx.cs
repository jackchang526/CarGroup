using System;
using BitAuto.CarChannel.BLL.Data;

namespace H5Web.V2
{
    public partial class Dealers : H5PageBase
    {
        protected SerialEntity BaseSerialEntity;
        //城市
        protected int CityId = 201;
        //车系ID
        protected int SerialId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            if (Request.QueryString["cityid"] != null)
            {
                int.TryParse(Request.QueryString["cityid"], out CityId);
            }
            BaseSerialEntity = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }
    }
}