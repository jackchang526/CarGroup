using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// 对比浮动框
	/// </summary>
	public partial class GetCarCsInfoForCompare : PageBase
	{
		private int carID = 0;
		private string resultString = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);

			GetParameter();
			RenderContent();
 		}

		private void GetParameter()
		{
			carID = ConvertHelper.GetInteger(Request.QueryString["carid"]);
		}

		private void RenderContent()
		{
			if (carID > 0)
			{
				CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
				if (  ce != null && ce.Id > 0)
				{
					resultString =string.Format("{0}^{1}",ce.Serial.Name,ce.Serial.AllSpell);
				}
 			}
			Response.Write(resultString);
		}
	}
}