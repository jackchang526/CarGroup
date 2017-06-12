using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class SellOff : H5PageBase
    {
        protected int SerialId = 0;
        protected int CityId = 0;
        protected SerialEntity BaseSerialEntity;
        private Car_SerialBll _serialBll;
        private List<int> _baoXiao = null;
        private List<int> _import = null;
        protected bool IsBaoXiao = false;
        protected bool IsImport = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            if (Request.QueryString["cityid"] != null)
            {
                int.TryParse(Request.QueryString["cityid"], out CityId);
            }

            _serialBll=new Car_SerialBll();
            _serialBll.GetCarParallelAndSell(SerialId, CityId, out _import, out _baoXiao);
            IsBaoXiao = _baoXiao.Count > 0;
            IsImport = _import.Count > 0;
            
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
        }

        //判断是否是包销车
    }
}