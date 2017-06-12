using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WirelessWeb.SUVChannel
{
    public partial class _default : WirelessPageBase
    {
        protected string ver = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
            GetParamter();
        }

        private void GetParamter()
        {
            ver = Request.QueryString["ver"];
        }
    }
}