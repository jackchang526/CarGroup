using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WirelessWeb
{
    public partial class YaGaoSelectCar : WirelessPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
        }
    }
}