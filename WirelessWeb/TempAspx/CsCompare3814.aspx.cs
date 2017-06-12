using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;

namespace WirelessWeb.TempAspx
{
    public partial class CsCompare3814 : PageBase
    {
        protected SerialEntity se = new SerialEntity();
        protected int csID = 3814;
        protected string CsHeadHTML = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            this.GetPageParam();
            CsHeadHTML = base.GetCommonNavigation("MCsCompare", csID);
        }
        private void GetPageParam()
        {
            //string csIDstr = this.Request.QueryString["csID"];
            //if (!string.IsNullOrEmpty(csIDstr)
            //    && int.TryParse(csIDstr, out csID))
            //{
            //}
            if (csID > 0)
            {
                se = (SerialEntity) DataManager.GetDataEntity(EntityType.Serial, csID);
            }
        }
    }
}