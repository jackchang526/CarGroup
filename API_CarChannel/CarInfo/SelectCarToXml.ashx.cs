using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using System.Xml;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// SelectCarToXml 的摘要说明
	/// modified by chengl May.6.2014 增加参数 isGetAllCsID 是否只输出全部符合条件的子品牌ID
	/// </summary>
	public class SelectCarToXml : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

		private SelectCarParameters selectParas;//参数实体
		private int SerialNum;//子品牌数量
		private int CarNum;//车型数量
		private int pageNum;//分页
		private int pageSize;//每页数量
		private int sortMode;//排序模式
		/*
		 s:排序模式
		1，关注度，由低到高
		2，报价，由低到高
		3，报价，由高到低
		4，关注度，由高到低
		5，指导价，由低到高
		6，指导价，由高到低
 		 */

		/// <summary>
		/// add by chengl May.6.2014 默认false 如果为true则只输出符合条件的子品牌ID
		/// </summary>
		private bool isGetAllCsID = false;

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(15);

			response = context.Response;
			request = context.Request;
			response.ContentType = "text/xml";

			GetParameters();

			var carXml = GetSelectCarXml();
			response.Write(carXml);
			response.End();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParameters()
		{
			// add by chengl May.6.2014
			// 是否输出全部子品牌ID 业务可以拿全部子品牌ID做查询条件
			if (!string.IsNullOrEmpty(request.QueryString["isGetAllCsID"])
				&& request.QueryString["isGetAllCsID"] == "1")
			{
				isGetAllCsID = true;
			}

			selectParas = new SelectCarParameters();
			//价格
			var price = request.QueryString["p"];
			if (!String.IsNullOrEmpty(price))
			{
				string[] pc = price.Split('-');
				if (pc.Length == 2)
				{
					selectParas.MinPrice = ConvertHelper.GetInteger(pc[0]);
					selectParas.MaxPrice = ConvertHelper.GetInteger(pc[1]);
					if (selectParas.MaxPrice == 9999)
						selectParas.MaxPrice = 0;
				}
			}
			var referPrice = request.QueryString["rp"];
			if (!String.IsNullOrEmpty(referPrice))
			{
				string[] pc = referPrice.Split('-');
				if (pc.Length == 2)
				{
					selectParas.MinReferPrice = ConvertHelper.GetInteger(pc[0]);
					selectParas.MaxReferPrice = ConvertHelper.GetInteger(pc[1]);
					if (selectParas.MaxReferPrice == 9999)
						selectParas.MaxReferPrice = 0;
				}
			}
			//级别
			selectParas.Level = ConvertHelper.GetInteger(request.QueryString["l"]);

			//级别查询模式,lm=1 时按级别的多选方式查询
			int levelMode = ConvertHelper.GetInteger(request.QueryString["lm"]);
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
						selectParas.Level = (int)Math.Pow(2, selectParas.Level - 1);
					}
				}
			}

			//排量
			var dis = request.QueryString["d"];
			if (!String.IsNullOrEmpty(dis))
			{
				string[] dc = dis.Split('-');
				if (dc.Length == 2)
				{
					selectParas.MinDis = ConvertHelper.GetDouble(dc[0]);
					selectParas.MaxDis = ConvertHelper.GetDouble(dc[1]);
					if (selectParas.MaxDis == 9.0)
						selectParas.MaxDis = 0.0;
				}
			}

			//变速箱
			selectParas.TransmissionType = ConvertHelper.GetInteger(request.QueryString["t"]);
			//车型用途
			selectParas.Purpose = ConvertHelper.GetInteger(request.QueryString["pu"]);
			//品牌国别
			selectParas.Country = ConvertHelper.GetInteger(request.QueryString["c"]);
			//车身形式
			selectParas.BodyForm = ConvertHelper.GetInteger(request.QueryString["b"]);
			//品牌类型
			selectParas.BrandType = ConvertHelper.GetInteger(request.QueryString["g"]);
			//舒适性配置
			selectParas.ComfortableConfig = ConvertHelper.GetInteger(request.QueryString["comf"]);
			//安全性配置
			selectParas.SafetyConfig = ConvertHelper.GetInteger(request.QueryString["safe"]);
			//驱动
			selectParas.DriveType = ConvertHelper.GetInteger(request.QueryString["dt"]);
			//燃料
			selectParas.FuelType = ConvertHelper.GetInteger(request.QueryString["f"]);
			//车门数
			var bodyDoors = request.QueryString["bd"];
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
			var perfSeatNum = request.QueryString["sn"];
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
			selectParas.IsWagon = ConvertHelper.GetInteger(request.QueryString["lv"]);

			//更多条件
			var carConfig = request.QueryString["m"];
			if (!String.IsNullOrEmpty(carConfig))
			{
				int mcLength = carConfig.Length;
				if (mcLength > 30)
					mcLength = 30;
				for (int i = 0; i < mcLength; i++)
				{
					if (carConfig[i] == '1')
					{
						selectParas.CarConfig += (int)Math.Pow(2, i);
					}
				}
			}

			//页面尺寸
			var pSize = request.QueryString["pagesize"];
			bool isps = Int32.TryParse(pSize, out pageSize);
			if (!isps)
				pageSize = 20;

			//页号
			var page = request.QueryString["page"];
			bool isPage = Int32.TryParse(page, out pageNum);
			if (!isPage)
				pageNum = 1;

			//排序模式
			sortMode = ConvertHelper.GetInteger(request.QueryString["s"]);
		}

		/// <summary>
		/// 获取 选车数据 xml
		/// </summary>
		/// <returns></returns>
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
			var decl = dataDoc.CreateXmlDeclaration("1.0", "utf-8", null);
			dataDoc.InsertBefore(decl, dataDoc.DocumentElement);

			XmlElement root = dataDoc.CreateElement("SelectResult");
			root.SetAttribute("serialCount", SerialNum.ToString());
			dataDoc.AppendChild(root);

			int counter = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in serialList)
			{
				counter++;
				CarNum += info.CarNum;
				if (isGetAllCsID)
				{ }
				else
				{
					if (counter < startIndex || counter > endIndex)
					{ continue; }
				}

				XmlElement serialNode = dataDoc.CreateElement("Serial");
				serialNode.SetAttribute("id", info.SerialId.ToString());

				if (!isGetAllCsID)
				{

					if (info.SerialId == 1568)
						serialNode.SetAttribute("showName", "索纳塔八");
					else
						serialNode.SetAttribute("showName", info.ShowName);
					serialNode.SetAttribute("image", info.ImageUrl);
					serialNode.SetAttribute("price", info.PriceRange);
					serialNode.SetAttribute("allspell", info.AllSpell);
				}
				root.AppendChild(serialNode);
				//string serialUrl = "http://car.bitauto.com/" + info.AllSpell + "/";
				//string shortName = info.ShowName.Replace("(进口)", "");
				//htmlCode.Append("<img width=\"120\" height=\"80\" alt=\"" + info.ShowName + "\" src=\"" + info.ImageUrl + "\"></a>");
				//htmlCode.AppendLine("<br/><span>" + info.PriceRange + "</span>");
			}
			root.SetAttribute("carCount", CarNum.ToString());
			return dataDoc.OuterXml;
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}