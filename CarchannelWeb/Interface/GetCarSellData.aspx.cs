using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class GetCarSellData : InterfacePageBase
	{
		private DateTime dataDate;		//取数据的日期
		private string dataType;		//取数据类型，按级别分为：轿车(car)，suv,mpv,还有品牌树目录：brandtree，查询结果：query
		private int producerId;
		private int brandId;
		private int serialId;
		protected void Page_Load(object sender, EventArgs e)
		{
			string xmlRes = "";
			try
			{
				WriteIpHistory();
				GetParameter();
				ProduceAndSellDataBll psBll = new ProduceAndSellDataBll();

				if (dataType == "brandtree")
					xmlRes = psBll.GetBrandTree();
				else if (dataType == "query")
					xmlRes = psBll.GetQueryData(producerId, brandId, serialId);
				else if (dataType == "datamap")
					xmlRes = psBll.GetSellDataMap();
				else
					xmlRes = psBll.GetSellDataXml(dataDate, dataType);
				//Response.ContentType = "Text/XML";
				//Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				//Response.Write(xmlRes);
				//Response.End();
			}
			catch (Exception ex)
			{
				WriteOperateLog(ex.ToString());
			}
			Response.ContentType = "Text/XML";
			Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			Response.Write(xmlRes);
			Response.End();
		}

		private void GetParameter()
		{
			//日期
			bool isDate = DateTime.TryParse(Request.QueryString["date"], out dataDate);
			if (!isDate)
				dataDate = DateTime.MinValue; //new ProduceAndSellDataBll().GetLastMonths();//DateTime.Now.AddMonths(-1);


			//数据类型
			dataType = Request.QueryString["dataType"];
			if (String.IsNullOrEmpty(dataType))
				dataType = "car";
			dataType = dataType.ToLower();
			// 		if (dataType != "mpv" && dataType != "suv" && dataType != "brandtree" && dataType!="query" && dataType != "datamap")
			// 			dataType = "car";

			if (dataType == "query")
			{
				string pStrId = Convert.ToString(Request.QueryString["pId"]);
				if (String.IsNullOrEmpty(pStrId))
					pStrId = "0";
				producerId = Convert.ToInt32(pStrId);

				string bStrId = Convert.ToString(Request.QueryString["bId"]);
				if (String.IsNullOrEmpty(bStrId))
					bStrId = "0";
				brandId = Convert.ToInt32(bStrId);

				string sStrId = Convert.ToString(Request.QueryString["sId"]);
				if (String.IsNullOrEmpty(sStrId))
					sStrId = "0";
				serialId = Convert.ToInt32(sStrId);
			}
		}

		private void WriteOperateLog(string logContent)
		{
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "log\\error\\";
			// string sDir = "E:\\wwwroot\\AutoChannel\\log\\";
			try
			{
				if (!System.IO.Directory.Exists(sDir))
				{
					System.IO.Directory.CreateDirectory(sDir);
				}
				using (StreamWriter sw = new StreamWriter(sDir + DateTime.Now.ToShortDateString() + ".txt", true))
				{
					sw.Write(logContent);
					sw.Close();
				}
			}
			catch
			{ }
		}

		private void WriteIpHistory()
		{
			string ipStr = WebUtil.GetClientIP();
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "log\\";
			// string sDir = "E:\\wwwroot\\AutoChannel\\log\\";
			try
			{
				if (!System.IO.Directory.Exists(sDir))
				{
					System.IO.Directory.CreateDirectory(sDir);
				}
				using (StreamWriter sw = new StreamWriter(sDir + "selldataIp_reffer.txt", true))
				{
					sw.Write(ipStr + ":refer:" + Request.UrlReferrer + "\r\n");
					sw.Close();
				}
			}
			catch
			{ }
		}
	}
}