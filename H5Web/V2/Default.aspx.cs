using System;
using BitAuto.CarChannel.Common;

namespace H5Web.V2
{
    public partial class Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);
        }
    }
}