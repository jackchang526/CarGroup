using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 取所有车型的主品牌名、主品牌link、级别、界别link（付剑飞、燕庆）
	/// modified by chengl Aug.8.2011
	/// </summary>
	public partial class GetCarMasterAndLevel : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				base.SetPageCache(60);
				string xmlContent = string.Empty;
				//string catchkey = "interfaceforbitauto_GetCarMasterAndLevel";
				//object interfaceforbitauto_GetCarMasterAndLevel = null;
				//CacheManager.GetCachedData(catchkey, out interfaceforbitauto_GetCarMasterAndLevel);
				//if (interfaceforbitauto_GetCarMasterAndLevel == null)
				//{
				GetAllCarMasterAndLevel();
				xmlContent = sb.ToString();
				// CacheManager.InsertCache(catchkey, xmlContent, 60);
				//}
				//else
				//{
				//    xmlContent = Convert.ToString(interfaceforbitauto_GetCarMasterAndLevel);
				//}
				Response.Write(xmlContent);
			}
		}

		private void GetAllCarMasterAndLevel()
		{
			string sql = " select car.car_id,car.car_name,car.oldcar_id,cs.cs_id,cs.cs_CarLevel,cmb.bs_id,cmb.bs_name,cmb.urlSpell ";
			sql += " from dbo.Car_Basic car ";
			sql += " left join dbo.Car_Serial cs on car.cs_id = cs.cs_id ";
			sql += " left join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
			sql += " left join dbo.Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
			sql += " left join dbo.Car_MasterBrand cmb on cmbr.bs_id = cmb.bs_id ";
			sql += " where car.isState=1 and cs.isState=1 ";

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<CarMasterAndLevel>");
			DataSet dsCars = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (dsCars != null && dsCars.Tables.Count > 0 && dsCars.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCars.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Item CarID=\"" + dsCars.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCars.Tables[0].Rows[i]["car_name"].ToString()) + "\" ");
					sb.Append(" CarLevelName=\"" + dsCars.Tables[0].Rows[i]["cs_CarLevel"].ToString() + "\" ");
					sb.Append(" CarLevelURL=\"" + GetCarLevelByName(dsCars.Tables[0].Rows[i]["cs_CarLevel"].ToString()) + "\" ");
					sb.Append(" CarMasterName=\"" + System.Security.SecurityElement.Escape(dsCars.Tables[0].Rows[i]["bs_name"].ToString()) + "\" ");
					sb.Append(" CarMasterURL=\"http://car.bitauto.com/" + dsCars.Tables[0].Rows[i]["urlSpell"].ToString().ToLower() + "/\" />");
				}
			}
			sb.Append("</CarMasterAndLevel>");
		}

		private string GetCarLevelByName(string levelName)
		{
			string url = "http://car.bitauto.com/";
			switch (levelName)
			{
				case "微型车": url += "weixingche/"; break;
				case "豪华车": url += "haohuaxingche/"; break;
				case "中大型车": url += "zhongdaxingche/"; break;
				case "小型车": url += "xiaoxingche/"; break;
				case "MPV": url += "mpv/"; break;
				case "中型车": url += "zhongxingche/"; break;
				case "紧凑型车": url += "jincouxingche/"; break;
				case "SUV": url += "suv/"; break;
				case "跑车": url += "paoche/"; break;
				case "其它": url += "qita/"; break;
				default: ; break;
			}
			return url;
		}
	}
}