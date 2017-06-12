using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace H5Web.V2
{
	public partial class Page1 : H5PageBase
	{
		#region params
		protected int SerialBrandId = 0;
		protected SerialEntity BaseSerialEntity = null;
		protected List<SerialColorForSummaryEntity> SerialColorList = new List<SerialColorForSummaryEntity>();
		#endregion

		#region Services

		private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
		#endregion
		protected void Page_Load(object sender, EventArgs e)
		{
			GetParams();
			InitData();
			InitColorList();
		}
		protected virtual void GetParams()
		{
			if (!string.IsNullOrEmpty(Request["serialbrandid"]))
			{
				SerialBrandId = Convert.ToInt32(Request["serialbrandid"]);
			}

		}

		private void InitData()
		{
			BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialBrandId);
		}


		private void InitColorList()
		{
			var colorList = serialFourthStageBll.GetSerialColorList(SerialBrandId);
			if (colorList != null && colorList.Count > 0)
			{
				if (colorList.Count > 12)
					SerialColorList = colorList.GetRange(0, 12);
				else
				{
					SerialColorList = colorList;
				}
			}
		}
	}
}