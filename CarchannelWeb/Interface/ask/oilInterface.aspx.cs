using System;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.ask
{
	public partial class oilInterface : InterfacePageBase
	{
		//得到油耗的XML对象
		// protected string oilXmlObj = string.Empty;
		private StringBuilder sb = new StringBuilder();
		Car_BasicBll carBll = new Car_BasicBll();

		protected void Page_Load(object sender, EventArgs e)
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><root>");
			GetAllSerialAndCar();
			sb.AppendLine("</root>");
			Response.Write(sb.ToString());

			////创建XML分类
			//XmlDocument xmlDoc = new XmlDocument();
			//XmlNode rootNode = xmlDoc.CreateElement("root");
			//xmlDoc.AppendChild(rootNode);
			//XmlDeclaration xmlDeclar = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
			//xmlDoc.InsertBefore(xmlDeclar, rootNode);

			//DataSet ds = new Car_BasicBll().GetOilMessageByAskInterface();
			//if (ds == null)
			//{
			//    oilXmlObj = xmlDoc.InnerXml;
			//    return;
			//}
			////循环创建品牌
			//foreach (DataRow carDr in ds.Tables[0].Rows)
			//{
			//    //车型ID
			//    int carId = ConvertHelper.GetInteger(carDr["CarId"]);
			//    //易车测试油耗
			//    string Perf_MeasuredFuel = carDr.IsNull("Perf_MeasuredFuel")
			//                                ? "0" : carDr["Perf_MeasuredFuel"].ToString();
			//    //CNCAP市郊工况油耗
			//    string Perf_CNCAPSuburbsfuelconsumption = carDr.IsNull("Perf_CNCAPSuburbsfuelconsumption")
			//                                                ? "0" : carDr["Perf_CNCAPSuburbsfuelconsumption"].ToString();
			//    //CNCAP市区工况油耗
			//    string Perf_CNCAPfuelconsumption = carDr.IsNull("Perf_CNCAPfuelconsumption")
			//                                            ? "0" : carDr["Perf_CNCAPfuelconsumption"].ToString();
			//    //市区工况油耗
			//    string Perf_ShiQuYouHao = carDr.IsNull("Perf_ShiQuYouHao")
			//                                            ? "0" : carDr["Perf_ShiQuYouHao"].ToString();
			//    //市郊工况油耗
			//    string Perf_ShiJiaoYouHao = carDr.IsNull("Perf_ShiJiaoYouHao")
			//                                            ? "0" : carDr["Perf_ShiJiaoYouHao"].ToString();
			//    //三部委检测油耗
			//    string Perf_Prototypetestingfuelconsumption = carDr.IsNull("Perf_Prototypetestingfuelconsumption")
			//                                            ? "0" : carDr["Perf_Prototypetestingfuelconsumption"].ToString();

			//    CarEntity carObj = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
			//    //变速箱
			//    string TransmissonType = carObj[712];
			//    //排量
			//    string ExhaustForFloat = carObj[785];
			//    //车型名称
			//    string carname = carObj.Name;
			//    if (carObj.IsState != 1) continue;
			//    SerialEntity serialObj = carObj.Serial;
			//    if (serialObj.IsState != 1) continue;
			//    //子品牌ID
			//    int serialId = serialObj.Id;
			//    //子品牌名称     
			//    string serialName = serialObj.Name;

			//    XmlNode serialNode = xmlDoc.SelectSingleNode(string.Format("root/Serial[@ID={0}]", serialId));
			//    if (serialNode == null)
			//    {
			//        serialNode = xmlDoc.CreateElement("Serial");
			//        ((XmlElement)serialNode).SetAttribute("ID", serialId.ToString());
			//        ((XmlElement)serialNode).SetAttribute("Name", serialName.ToString());
			//        rootNode.AppendChild(serialNode);
			//    }
			//    XmlElement carNode = xmlDoc.CreateElement("Car");
			//    carNode.SetAttribute("ID", carId.ToString());
			//    carNode.SetAttribute("Name", carname.ToString());
			//    carNode.SetAttribute("TransmissonType", TransmissonType);
			//    carNode.SetAttribute("ExhaustForFloat", ExhaustForFloat);
			//    carNode.SetAttribute("Perf_CNCAPfuelconsumption", Perf_CNCAPfuelconsumption);
			//    carNode.SetAttribute("Perf_CNCAPSuburbsfuelconsumption", Perf_CNCAPSuburbsfuelconsumption);
			//    carNode.SetAttribute("Perf_MeasuredFuel", Perf_MeasuredFuel);
			//    carNode.SetAttribute("Perf_ShiQuYouHao", Perf_ShiQuYouHao);
			//    carNode.SetAttribute("Perf_ShiJiaoYouHao", Perf_ShiJiaoYouHao);
			//    carNode.SetAttribute("Perf_Prototypetestingfuelconsumption", Perf_Prototypetestingfuelconsumption);
			//    serialNode.AppendChild(carNode);
			//}

			//oilXmlObj = xmlDoc.InnerXml;
		}


		public void GetAllSerialAndCar()
		{
			string sql = @"select cs.cs_id,cs.csname,car.car_id,car.car_name 
						from car_relation car 
						left join car_serial cs on car.cs_id=cs.cs_id 
						where car.isState=0 and cs.isState=0 
						order by cs.cs_id,car.car_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				// 油耗
				Dictionary<int, string> dic788 = carBll.GetCarParamExDic(788);
				// CNCAP市郊工况油耗
				Dictionary<int, string> dic855 = carBll.GetCarParamExDic(855);
				// CNCAP市区工况油耗
				Dictionary<int, string> dic854 = carBll.GetCarParamExDic(854);
				// 市区工况油耗
				Dictionary<int, string> dic783 = carBll.GetCarParamExDic(783);
				// 市郊工况油耗
				Dictionary<int, string> dic784 = carBll.GetCarParamExDic(784);
				// 三部委检测油耗
				Dictionary<int, string> dic862 = carBll.GetCarParamExDic(862);
				// 综合工况油耗
				Dictionary<int, string> dic782 = carBll.GetCarParamExDic(782);
				// 变速箱类型
				Dictionary<int, string> dic712 = carBll.GetCarParamExDic(712);
				// 排量（升）
				Dictionary<int, string> dic785 = carBll.GetCarParamExDic(785);

				int lastCsID = 0;
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carid = int.Parse(dr["car_id"].ToString());
					string carName = dr["car_name"].ToString().Trim();
					int csid = int.Parse(dr["cs_id"].ToString());
					string csName = dr["csname"].ToString().Trim();
					if (csid != lastCsID)
					{
						// 新子品牌节点
						if (lastCsID > 0)
						{ sb.AppendLine("</Serial>"); }
						sb.AppendLine("<Serial ID=\"" + csid.ToString() + "\" Name=\"" + System.Security.SecurityElement.Escape(csName) + "\">");
						lastCsID = csid;
					}
					sb.AppendLine("<Car ID=\"" + carid + "\" Name=\"" + System.Security.SecurityElement.Escape(carName) + "\" ");
					sb.AppendLine("TransmissonType=\"" + (dic712.ContainsKey(carid) ? dic712[carid] : "")
						+ "\" ExhaustForFloat=\"" + (dic785.ContainsKey(carid) ? dic785[carid] : "") + "\" ");
					sb.AppendLine("Perf_CNCAPfuelconsumption=\"" + (dic854.ContainsKey(carid) ? dic854[carid] : "0")
						+ "\" Perf_CNCAPSuburbsfuelconsumption=\"" + (dic855.ContainsKey(carid) ? dic855[carid] : "0") + "\" ");
					sb.AppendLine("Perf_MeasuredFuel=\"" + (dic788.ContainsKey(carid) ? dic788[carid] : "0")
						+ "\" Perf_ShiQuYouHao=\"" + (dic783.ContainsKey(carid) ? dic783[carid] : "0")
						+ "\" Perf_ShiJiaoYouHao=\"" + (dic784.ContainsKey(carid) ? dic784[carid] : "0") + "\" ");
					sb.AppendLine("Perf_Prototypetestingfuelconsumption=\"" + (dic862.ContainsKey(carid) ? dic862[carid] : "0")
						+ "\" Perf_ZongHeYouHao=\"" + (dic782.ContainsKey(carid) ? dic782[carid] : "0") + "\" />");
				}
				sb.AppendLine("</Serial>");
			}

		}

	}
}