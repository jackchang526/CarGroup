using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;

namespace H5Web.V2
{
	public partial class CsSummary : H5PageBase
	{
		#region Params

		protected int SerialBrandId;
		protected int DealerId;//经销商编号
		protected int BrokerId;//经纪人编号

		protected SerialEntity BaseSerialEntity;
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			GetParams();
			InitData();
		}
		private void GetParams()
		{
			if (!string.IsNullOrEmpty(Request["csID"]))
				SerialBrandId = Convert.ToInt32(Request["csID"]);
			if (!string.IsNullOrEmpty(Request["dealerid"]))
				DealerId = Convert.ToInt32(Request["dealerid"]);
			if (!string.IsNullOrEmpty(Request["brokerid"]))
				BrokerId = Convert.ToInt32(Request["brokerid"]);
		}

		private void InitData()
		{
			BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialBrandId);
		}
	}
}