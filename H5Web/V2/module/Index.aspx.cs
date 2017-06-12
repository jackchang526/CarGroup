using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace H5Web.V2.module
{
	public partial class Index : Page1
	{

		protected int DealerId;
		protected int BrokerId;
		protected override void GetParams()
		{
			if (!string.IsNullOrEmpty(Request["serialBrandId"]))
				SerialBrandId = Convert.ToInt32(Request["SerialBrandId"]);
			if (!string.IsNullOrEmpty(Request["brokerId"]))
				BrokerId = Convert.ToInt32(Request["brokerId"]);
			if (!string.IsNullOrEmpty(Request["dealerId"]))
				DealerId = Convert.ToInt32(Request["dealerid"]);
		}
	}
}