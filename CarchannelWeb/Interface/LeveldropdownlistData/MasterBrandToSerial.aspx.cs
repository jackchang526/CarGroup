using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.LeveldropdownlistData
{
	/*
	* 0:包含在销，待销，停销，待查；旗下是否有车型不限
	* 1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
	* 2:包含在销，待销；非概念车；旗下是否有车型不限；
	* 3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
	* 4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；
	*/
	public partial class MasterBrandToSerial : PageBase
	{
		private int conditions = 4;
		protected void Page_Load(object sender, EventArgs e)
		{
			// add by cheng Jul.23.2012
			base.SetPageCache(60);
			GetParams(HttpContext.Current);
			string cacheKey = "JsonXml_GetIsConditionsDataSet_" + conditions;
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				Response.Write((string)obj);
				return;
			}
			string xmlString = GetJsonByDataSet();
			CacheManager.InsertCache(cacheKey, xmlString, 60);
			Response.Write(xmlString);
		}
		/// <summary>
		/// 得到当前内容的上下文
		/// </summary>
		/// <param name="context"></param>
		private void GetParams(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.QueryString["type"]))
			{
				conditions = Convert.ToInt32(context.Request.QueryString["type"]);
			}
		}

		/// <summary>
		/// 得到Json数据通过数据集
		/// </summary>
		/// <returns></returns>
		public string GetJsonByDataSet()
		{
			DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(conditions);
			if (ds == null) return "";
			DataTable masterdt = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
			DataView allDv = ds.Tables[0].DefaultView;
			StringBuilder jsonString = new StringBuilder();
			StringBuilder masterString = new StringBuilder();
			StringBuilder brandString = new StringBuilder();
			StringBuilder serialString = new StringBuilder();
			foreach (DataRow masterdr in masterdt.Rows)
			{
				masterString.Append(",\"m" + masterdr["bs_id"] + "\":{");
				StringBuilder brandlist = new StringBuilder();
				allDv.RowFilter = "bs_id=" + masterdr["bs_id"];
				DataTable brandTable = allDv.ToTable(true, "cb_id", "cb_name", "cballspell");
				allDv.RowFilter = "";
				foreach (DataRow brandDr in brandTable.Rows)
				{

					brandString.Append(",\"b" + brandDr["cb_id"] + "\":{");
					brandlist.AppendFormat(",{0}", brandDr["cb_id"]);
					StringBuilder seriallist = new StringBuilder();
					DataRow[] serialDrList = ds.Tables[0].Select("cb_id=" + brandDr["cb_id"]);
					foreach (DataRow serialDr in serialDrList)
					{
						seriallist.AppendFormat(",{0}", serialDr["cs_id"]);
						serialString.Append(",\"s" + serialDr["cs_id"] + "\":{");
						serialString.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{4}\",\"urlSpell\":\"{2}\",\"showName\":\"{3}\",\"csSale\":\"{5}\""
									  , serialDr["cs_id"]
									  , serialDr["cs_name"].ToString().Trim()
									  , serialDr["csallspell"].ToString().ToLower()
									  , serialDr["cs_ShowName"].ToString().Trim()
									  , brandDr["cb_id"]
									  , GetSerialSaleState(serialDr["csSaleState"].ToString()));
						serialString.Append("}");
					}
					seriallist = seriallist.Remove(0, 1);
					brandString.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{4}\",\"urlSpell\":\"{2}\",\"nlist\":[{3}]"
									   , brandDr["cb_id"]
									   , brandDr["cb_Name"].ToString().Trim()
									   , brandDr["cballspell"].ToString().ToLower()
									   , seriallist.ToString()
									   , masterdr["bs_id"]);
					brandString.Append("}");
				}
				brandlist = brandlist.Remove(0, 1);
				masterString.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"0\",\"urlSpell\":\"{2}\",\"tSpell\":\"{3}\",\"nlist\":[{4}]"
										, masterdr["bs_id"]
										, masterdr["bs_Name"].ToString().Trim()
										, masterdr["bsallspell"].ToString().ToLower()
										, masterdr["bsspell"]
										, brandlist.ToString());
				masterString.Append("}");
			}
			masterString = masterString.Remove(0, 1);
			brandString = brandString.Remove(0, 1);
			serialString = serialString.Remove(0, 1);
			jsonString.Append("var carData={");
			jsonString.Append("\"master\":{" + masterString.ToString() + "},");
			jsonString.Append("\"brand\":{" + brandString.ToString() + "},");
			jsonString.Append("\"serial\":{" + serialString.ToString() + "}");
			jsonString.Append("};");

			return jsonString.ToString();
		}
		/// <summary>
		/// 得到子品牌销售状态
		/// </summary>
		/// <param name="state"></param>
		private string GetSerialSaleState(string state)
		{
			switch (state)
			{
				case "在销":
					return "0";
				case "待销":
					return "1";
				case "停销":
					return "2";
				default: return "3";
			}
		}
	}
}