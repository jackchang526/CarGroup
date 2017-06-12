using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WirelessWeb.SUVChannel
{
    public partial class SerialList : WirelessPageBase
    {
        protected int channelId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            channelId = ConvertHelper.GetInteger(Request["id"]);
        }
    }
}