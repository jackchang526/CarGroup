using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using System.IO;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    public partial class GetCarSellData : PageBase
    {
        private DateTime dataDate;		//取数据的日期
        private string dataType;		//取数据类型，按级别分为：轿车(car)，suv,mpv,还有品牌树目录：brandtree，查询结果：query
        private int producerId;
        private int brandId;
        private int serialId;
        private string returnType;
		private string encoding;
        protected void Page_Load(object sender, EventArgs e)
        {
            string xmlRes = "";
            try
            {
                base.SetPageCache(15);

                GetParameter();

                ProduceAndSellDataBll psBll = new ProduceAndSellDataBll();

                if (returnType == "json")
                {
                    if (dataType == "brandtree")
                        xmlRes = string.Concat("var brandtreeJson = ", psBll.GetBrandTreeToJson(), ";");
                    else if (dataType == "query")
                        xmlRes = string.Concat("var queryJson = ", psBll.GetQueryDataToJson(producerId, brandId, serialId), ";");
                    else if (dataType == "datamap")
                        xmlRes = string.Concat("var datamapJson = ", psBll.GetSellDataMapToJson(), ";");
                    else
                        xmlRes = string.Concat("var sellDataJson = ", psBll.GetSellDataJson(dataDate, dataType), ";");
                }
                else
                {
                    if (dataType == "brandtree")
                        xmlRes = psBll.GetBrandTree();
                    else if (dataType == "query")
                        xmlRes = psBll.GetQueryData(producerId, brandId, serialId);
                    else if (dataType == "datamap")
                        xmlRes = psBll.GetSellDataMap();
                    else
                        xmlRes = psBll.GetSellDataXml(dataDate, dataType);
                }
            }
            catch (Exception ex)
            {
                WriteOperateLog(ex.ToString());
            }
            if (returnType == "json")
            {
				Response.ContentType = "application/x-javascript";
				if (!String.IsNullOrEmpty(encoding))
					Response.ContentEncoding = Encoding.GetEncoding(encoding);
				//Response.ContentEncoding = Encoding.UTF8;
                Response.Write(xmlRes);
            }
            else
            {
                Response.ContentType = "Text/XML";
                Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                Response.Write(xmlRes);
            }
        }

        private void GetParameter()
        {
            //返回格式 xml json
            returnType = Request.QueryString["returnType"] == "json" ? "json" : "xml";

			//请求编码
			encoding = String.Empty;
			encoding = Request.QueryString["encoding"];
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
    }
}