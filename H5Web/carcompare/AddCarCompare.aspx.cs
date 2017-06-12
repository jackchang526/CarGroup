using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;

namespace H5Web.carcompare
{

    public partial class AddCarCompare : H5PageBase
    {
        protected int CarId = 0;
        protected int SerialId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(30);
            GetParmas();
        }

        private void GetParmas()
        {
            CarId = ConvertHelper.GetInteger(Request["carid"]);
            SerialId = ConvertHelper.GetInteger(Request["csid"]);
        }
    }
}