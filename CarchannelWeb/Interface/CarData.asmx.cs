using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.CarChannel.BLL;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	/// <summary>
	/// CarData 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
	// [System.Web.Script.Services.ScriptService]
	public class CarData : System.Web.Services.WebService
	{

		[WebMethod]
		public DataSet GetCarDataForGoogle()
		{
			DataSet ds = null;
			try
			{
				ds = new Car_BasicBll().GetCarDataForGoogle();
			}
			catch (System.Exception ex)
			{
				ds = new DataSet();
				DataTable dt = new DataTable("Exception");
				dt.Columns.Add("Message");
				DataRow row = dt.NewRow();
				row["Message"] = ex.ToString();
				dt.Rows.Add(row);
				ds.Tables.Add(dt);
			}
			return ds;
		}
	}
}
