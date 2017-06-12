using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Tree
{
	public partial class SelectCarToXml : InterfacePageBase
	{
		private SelectCarParameters selectParas;
		private int SerialNum;
		private int CarNum;
		private int pageNum;
		private int pageSize;
		private int sortMode;
		/*
		 s:排序模式
		1，关注度，由低到高
		2，报价，由低到高
		3，报价，由高到低
		4，关注度，由高到低
		5，指导价，由低到高
		6，指导价，由高到低

		 */

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(15);
			GetParameters();
			Response.ContentType = "Text/XML";
			Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			string carXml = GetSelectCarXml();
			Response.Write(carXml);
			Response.End();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParameters()
		{
			string conditionStr = "";
			selectParas = new SelectCarParameters();
			//价格
			string tmpStr = Request.QueryString["p"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				string[] pc = tmpStr.Split('-');
				if (pc.Length == 2)
				{
					selectParas.MinPrice = ConvertHelper.GetInteger(pc[0]);
					selectParas.MaxPrice = ConvertHelper.GetInteger(pc[1]);
					if (selectParas.MaxPrice == 9999)
						selectParas.MaxPrice = 0;
				}
			}
			tmpStr = Request.QueryString["rp"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				string[] pc = tmpStr.Split('-');
				if (pc.Length == 2)
				{
					selectParas.MinReferPrice = ConvertHelper.GetInteger(pc[0]);
					selectParas.MaxReferPrice = ConvertHelper.GetInteger(pc[1]);
					if (selectParas.MaxReferPrice == 9999)
						selectParas.MaxReferPrice = 0;
				}
			}
			//级别
			selectParas.Level = ConvertHelper.GetInteger(Request.QueryString["l"]);

			//级别查询模式,lm=1 时按级别的多选方式查询
			int levelMode = ConvertHelper.GetInteger(Request.QueryString["lm"]);
			if (levelMode != 1)
			{
				if (selectParas.Level > 0)
				{
					string levelName = String.Empty;
					if (selectParas.Level == 63)
						levelName = "轿车";
					else
					{
						//EnumCollection.SelectCarLevelEnum level = (EnumCollection.SelectCarLevelEnum)selectParas.Level;
						//if (Car_LevelBll.LevelNameDic.ContainsKey(level.ToString()))
						//{
						//    levelName = Car_LevelBll.LevelNameDic[level.ToString()];
						//}

						levelName = CarUtils.Define.CarLevelDefine.GetSelectCarLevelNameById(selectParas.Level);
						levelName = CarUtils.Define.CarLevelDefine.GetLevelDiscName(levelName);
						
						if (conditionStr.Length > 0)
							conditionStr += "_";
						conditionStr += levelName;
						selectParas.Level = (int)Math.Pow(2, selectParas.Level - 1);
					}
				}
			}

			//排量
			tmpStr = Request.QueryString["d"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += tmpStr + "L";

				string[] dc = tmpStr.Split('-');
				if (dc.Length == 2)
				{
					selectParas.MinDis = ConvertHelper.GetDouble(dc[0]);
					selectParas.MaxDis = ConvertHelper.GetDouble(dc[1]);
					if (selectParas.MaxDis == 9.0)
						selectParas.MaxDis = 0.0;
				}
			}

			//变速箱
			selectParas.TransmissionType = ConvertHelper.GetInteger(Request.QueryString["t"]);
			//变速箱查询模式，tm=1 时按详细的变速箱类别查询
			int transMode = ConvertHelper.GetInteger(Request.QueryString["tm"]);
			string transType = "";
			if (transMode != 1)
			{
				if (selectParas.TransmissionType >= 2)
				{
					transType = "自动(AT)";
					//selectParas.TransmissionType = 2 + 4 + 8 + 16;		//合并了自动，手自一体，CVT及双离合
				}
				else if (selectParas.TransmissionType == 1)
					transType = "手动";
			}

			if (selectParas.TransmissionType != 0)
			{
				if (conditionStr.Length > 0)
					conditionStr += "_";
				conditionStr += transType;
			}

			//车型用途
			selectParas.Purpose = ConvertHelper.GetInteger(Request.QueryString["pu"]);
			//品牌国别
			selectParas.Country = ConvertHelper.GetInteger(Request.QueryString["c"]);
			//车身形式
			selectParas.BodyForm = ConvertHelper.GetInteger(Request.QueryString["b"]);

			//品牌类型
			selectParas.BrandType = ConvertHelper.GetInteger(Request.QueryString["g"]);


			//舒适性配置
			selectParas.ComfortableConfig = ConvertHelper.GetInteger(Request.QueryString["comf"]);
			//安全性配置
			selectParas.SafetyConfig = ConvertHelper.GetInteger(Request.QueryString["safe"]);


			//驱动
			selectParas.DriveType = ConvertHelper.GetInteger(Request.QueryString["dt"]);
			//燃料
			selectParas.FuelType = ConvertHelper.GetInteger(Request.QueryString["f"]);
			//车门数
			var bodyDoors = Request.QueryString["bd"];
			if (!string.IsNullOrEmpty(bodyDoors))
			{
				string[] doors = bodyDoors.Split('-');
				if (doors.Length == 2)
				{
					selectParas.MinBodyDoors = ConvertHelper.GetInteger(doors[0]);
					selectParas.MaxBodyDoors = ConvertHelper.GetInteger(doors[1]);
				}
			}
			//座位数
			var perfSeatNum = Request.QueryString["sn"];
			if (!string.IsNullOrEmpty(perfSeatNum))
			{
				string[] seatArr = perfSeatNum.Split('-');
				if (seatArr.Length == 2)
				{
					selectParas.MinPerfSeatNum = ConvertHelper.GetInteger(seatArr[0]);
					selectParas.MaxPerfSeatNum = ConvertHelper.GetInteger(seatArr[1]);
				}
				else
				{
					selectParas.MinPerfSeatNum = ConvertHelper.GetInteger(seatArr[0]);
				}
			}
			//是否旅行版
			selectParas.IsWagon = ConvertHelper.GetInteger(Request.QueryString["lv"]);

			//更多条件
			tmpStr = Request.QueryString["m"];
			if (!String.IsNullOrEmpty(tmpStr))
			{
				int mcLength = tmpStr.Length;
				if (mcLength > 30)
					mcLength = 30;
				for (int i = 0; i < mcLength; i++)
				{
					if (tmpStr[i] == '1')
					{
						selectParas.CarConfig += (int)Math.Pow(2, i);
					}
				}
			}

			//页面尺寸
			tmpStr = Request.QueryString["pagesize"];
			bool isps = Int32.TryParse(tmpStr, out pageSize);
			if (!isps)
				pageSize = 20;

			//页号
			tmpStr = Request.QueryString["page"];
			bool isPage = Int32.TryParse(tmpStr, out pageNum);
			if (!isPage)
				pageNum = 1;

			//排序模式
			sortMode = ConvertHelper.GetInteger(Request.QueryString["s"]);
		}

		private string GetSelectCarXml()
		{
			List<BitAuto.CarChannel.BLL.SerialInfo> tmpList = new SelectCarToolBll().SelectCarByParameters(selectParas);
			//Copy一个复本，以免排序时影响原其他线程
			List<BitAuto.CarChannel.BLL.SerialInfo> serialList = tmpList.GetRange(0, tmpList.Count);

			//排序
			switch (sortMode)
			{
				case 1:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerial);
					break;
				case 2:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPrice);
					break;
				case 3:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPriceDesc);
					break;
				case 5:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinReferPrice);
					break;
				case 6:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinReferPriceDesc);
					break;
				case 4:
				default:
					serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByPVDesc);
					break;
			}

			SerialNum = serialList.Count;
			CarNum = 0;

			int startIndex = (pageNum - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;
			XmlDocument dataDoc = new XmlDocument();
			XmlElement root = dataDoc.CreateElement("SelectResult");
			root.SetAttribute("serialCount", SerialNum.ToString());
			dataDoc.AppendChild(root);

			int counter = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in serialList)
			{
				counter++;
				CarNum += info.CarNum;
				if (counter < startIndex || counter > endIndex)
					continue;

				XmlElement serialNode = dataDoc.CreateElement("Serial");
				serialNode.SetAttribute("id", info.SerialId.ToString());
				if (info.SerialId == 1568)
					serialNode.SetAttribute("showName", "索纳塔八");
				else
					serialNode.SetAttribute("showName", info.ShowName);
				serialNode.SetAttribute("image", info.ImageUrl);
				serialNode.SetAttribute("price", info.PriceRange);
				root.AppendChild(serialNode);
				//string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
				//string shortName = info.ShowName.Replace("(进口)", "");
				//htmlCode.Append("<img width=\"120\" height=\"80\" alt=\"" + info.ShowName + "\" src=\"" + info.ImageUrl + "\"></a>");
				//htmlCode.AppendLine("<br/><span>" + info.PriceRange + "</span>");
			}

			root.SetAttribute("carCount", CarNum.ToString());
			return root.OuterXml;
		}
	}
}