using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace H5Web.xuanche
{
    public partial class SelectByBrandV4 : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30); 
        }
    }
}