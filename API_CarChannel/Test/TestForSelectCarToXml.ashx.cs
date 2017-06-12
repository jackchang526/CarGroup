using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using System.Xml;
using BitAuto.CarChannelAPI.Web.AppCode;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.Test
{
	/// <summary>
	/// TestForSelectCarToXml 的摘要说明
	/// </summary>
	public class TestForSelectCarToXml : PageBase, IHttpHandler
	{
		// 选车更多条件 <索引值,查询参数>
		Dictionary<int, IMongoQuery> dicMoreCondition = new Dictionary<int, IMongoQuery>();
		List<IMongoQuery> listq = new List<IMongoQuery>();
		private string mongoDBConnectionString = "mongodb://192.168.15.146/?safe=true";
		private string mongoDBDatabase = "carchannel";
		private string collectionName = "car.cardata";
		HttpResponse response;
		HttpRequest request;
		IMongoQuery query;
		// 最终结果的车款ID
		SortedSet<int> resultSS = new SortedSet<int>();

		SortedSet<int> resultMoreSS = null;

		Dictionary<int, SelectCarFromMongoDB> dicAllCarBaseInfo = new Dictionary<int, SelectCarFromMongoDB>();

		IList<string> keyForMemCache = new List<string>();
		Dictionary<string, List<string>> dicTempValue = new Dictionary<string, List<string>>();
		//Query.And(
		//// 子品牌ID
		//        Query.EQ("Cs_Id", serialId)
		//// 城市ID
		//        , Query.In("CityList", cityId)
		//// 上架时间小于等于当前时间，mongoDB内是UTC时间
		//        , Query.LTE("UpTime", DateTime.Now)
		//        , Query.GTE("DownTime", DateTime.Now)
		//        );
		// IMongoSortBy sortBy = new SortByDocument { { "BitautoPrice", 1 }, { "GoodsUrl", 1 } };

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

		// 需要查询的扩展参数 <paramid,<pvalue>>
		private Dictionary<int, List<string>> dicQuery = new Dictionary<int, List<string>>();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(15);
			response.Write("");
			response.End();

			//response = context.Response;
			//request = context.Request;
			//response.ContentType = "text/xml";
			//IntiAllCarBaseInfo();
			//// GetParameters();
			//GetParametersForMemCache();

			//var carXml = GetSelectCarXml();
			//if (carXml == "")
			//{
			//	carXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root></Root>";
			//}
			//response.Write(carXml);
			//response.End();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParameters()
		{
			IntiMoreCndition();

			listq.Add(Query.EQ("CarSaleState", "在销"));

			selectParas = new SelectCarParameters();
			#region 价格
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
			if (selectParas.MinPrice > 0)
			{
				listq.Add(Query.GTE("MaxPrice", selectParas.MinPrice));
			}

			if (selectParas.MinPrice == 0 && selectParas.MaxPrice != 0)
			{
				listq.Add(Query.GT("MinPrice", 0));
				listq.Add(Query.LTE("MinPrice", selectParas.MaxPrice));
			}
			else if (selectParas.MaxPrice != 0)
			{
				listq.Add(Query.LTE("MinPrice", selectParas.MaxPrice));
			}
			#endregion

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

			#region 级别
			//级别
			selectParas.Level = ConvertHelper.GetInteger(request.QueryString["l"]);
			if (selectParas.Level != 0)
			{
				List<string> listTempLevel = new List<string>();
				// List<IMongoQuery> listTempLevel = new List<IMongoQuery>();
				if ((selectParas.Level & 1) == 1)
				{
					listTempLevel.Add("微型车");
				}
				if ((selectParas.Level & 2) == 2)
				{
					listTempLevel.Add("小型车");
				}
				if ((selectParas.Level & 4) == 4)
				{
					listTempLevel.Add("紧凑型车");
				}
				if ((selectParas.Level & 8) == 8)
				{
					listTempLevel.Add("中大型车");
				}
				if ((selectParas.Level & 16) == 16)
				{
					listTempLevel.Add("中型车");
				}
				if ((selectParas.Level & 32) == 32)
				{
					listTempLevel.Add("豪华车");
				}
				if ((selectParas.Level & 64) == 64)
				{
					listTempLevel.Add("MPV");
				}
				if ((selectParas.Level & 128) == 128)
				{
					listTempLevel.Add("SUV");
				}
				if ((selectParas.Level & 256) == 256)
				{
					listTempLevel.Add("跑车");
				}
				if ((selectParas.Level & 512) == 512)
				{
					listTempLevel.Add("其它");
				}
				if ((selectParas.Level & 1024) == 1024)
				{
					listTempLevel.Add("面包车");
				}
				if ((selectParas.Level & 2048) == 2048)
				{
					listTempLevel.Add("皮卡");
				}
				if (listTempLevel.Count > 0)
				{
					listq.Add(Query.And(Query.In("CarLevel", new BsonArray(listTempLevel))));
				}
			}
			else
			{
				listq.Add(Query.NE("CarLevel", "概念车"));
			}


			//级别查询模式,lm=1 时按级别的多选方式查询
			int levelMode = ConvertHelper.GetInteger(request.QueryString["lm"]);
			//if (levelMode != 1)
			//{
			//    if (selectParas.Level > 0)
			//    {
			//        string levelName = String.Empty;
			//        if (selectParas.Level == 63)
			//            levelName = "轿车";
			//        else
			//        {
			//            EnumCollection.SerialLevelEnum level = (EnumCollection.SerialLevelEnum)selectParas.Level;
			//            if (Car_LevelBll.LevelNameDic.ContainsKey(level.ToString()))
			//            {
			//                levelName = Car_LevelBll.LevelNameDic[level.ToString()];
			//            }
			//            selectParas.Level = (int)Math.Pow(2, selectParas.Level - 1);
			//        }
			//    }
			//}
			#endregion

			#region 排量
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
			if (selectParas.MinDis > 0)
			{
				listq.Add(Query.GTE("Param.P_785", selectParas.MinDis));
			}
			if (selectParas.MaxDis > 0)
			{
				listq.Add(Query.LTE("Param.P_785", selectParas.MaxDis));
			}
			#endregion

			#region 变速箱
			//变速箱
			selectParas.TransmissionType = ConvertHelper.GetInteger(request.QueryString["t"]);
			if (selectParas.TransmissionType != 0)
			{
				List<string> listTempTT = new List<string>();
				if ((selectParas.TransmissionType & 1) == 1)
				{
					listTempTT.Add("手动");
				}
				if ((selectParas.TransmissionType & 2) == 2)
				{
					listTempTT.Add("自动");
				}
				if ((selectParas.TransmissionType & 4) == 4)
				{
					listTempTT.Add("手自一体");
				}
				if ((selectParas.TransmissionType & 8) == 8)
				{
					listTempTT.Add("CVT无级变速");
				}
				if ((selectParas.TransmissionType & 16) == 16)
				{
					listTempTT.Add("双离合");
				}
				if ((selectParas.TransmissionType & 32) == 32)
				{
					listTempTT.Add("半自动");
				}
				if (listTempTT.Count > 0)
				{
					listq.Add(Query.In("Param.P_712", new BsonArray(listTempTT)));
				}
			}
			#endregion

			//车型用途
			selectParas.Purpose = ConvertHelper.GetInteger(request.QueryString["pu"]);

			#region 国别
			//品牌国别
			selectParas.Country = ConvertHelper.GetInteger(request.QueryString["c"]);
			if (selectParas.Country > 0)
			{
				// 自主 合资 进口
				if (selectParas.Country < 8)
				{
					List<IMongoQuery> listTempC = new List<IMongoQuery>();
					if ((selectParas.Country & 1) == 1)
					{
						// 自主
						listTempC.Add(Query.And(Query.EQ("BsCountry", "中国"), Query.EQ("CpCountry", "中国")));
					}
					if ((selectParas.Country & 2) == 2)
					{
						// 合资
						listTempC.Add(Query.And(Query.NE("BsCountry", "中国"), Query.EQ("CpCountry", "中国")));
					}
					if ((selectParas.Country & 4) == 4)
					{
						// 进口
						listTempC.Add(Query.And(Query.NE("BsCountry", "中国"), Query.NE("CpCountry", "中国")));
					}
					listq.Add(Query.Or(listTempC.ToArray<IMongoQuery>()));
				}
				else
				{
					// 国家
					if (selectParas.Country == 8)
					{
						listq.Add(Query.EQ("BsCountry", "德国"));
					}
					else if (selectParas.Country == 9)
					{
						listq.Add(Query.In("BsCountry", "韩国", "日本"));
					}
					else if (selectParas.Country == 10)
					{
						listq.Add(Query.EQ("BsCountry", "美国"));
					}
					else if (selectParas.Country == 11)
					{
						listq.Add(Query.In("BsCountry", "德国", "法国", "英国", "意大利", "瑞典", "捷克", "荷兰", "西班牙"));
					}
					else if (selectParas.Country == 12)
					{
						listq.Add(Query.EQ("BsCountry", "日本"));
					}
					else if (selectParas.Country == 16)
					{
						listq.Add(Query.EQ("BsCountry", "韩国"));
					}
				}
			}
			#endregion

			#region 车身形式
			//车身形式
			selectParas.BodyForm = ConvertHelper.GetInteger(request.QueryString["b"]);
			if (selectParas.BodyForm > 0)
			{
				List<string> listTempBF = new List<string>();
				if ((selectParas.BodyForm & 1) == 1)
				{
					listTempBF.Add("两厢轿车");
				}
				if ((selectParas.BodyForm & 2) == 2)
				{
					listTempBF.Add("三厢轿车");
				}
				if ((selectParas.BodyForm & 4) == 4)
				{
					listTempBF.Add("Cross混型车");
				}
				if ((selectParas.BodyForm & 8) == 8)
				{
					listTempBF.Add("Wagon旅行车");
				}
				if ((selectParas.BodyForm & 16) == 16)
				{
					listTempBF.Add("Pick-up皮卡");
				}
				if ((selectParas.BodyForm & 32) == 32)
				{
					listTempBF.Add("Micro-Bus厢式车");
				}
				if (listTempBF.Count > 0)
				{
					listq.Add(Query.In("CsBodyForm", new BsonArray(listTempBF)));
				}
			}
			#endregion

			//品牌类型
			selectParas.BrandType = ConvertHelper.GetInteger(request.QueryString["g"]);
			//舒适性配置
			selectParas.ComfortableConfig = ConvertHelper.GetInteger(request.QueryString["comf"]);
			//安全性配置
			selectParas.SafetyConfig = ConvertHelper.GetInteger(request.QueryString["safe"]);

			#region 驱动
			//驱动
			selectParas.DriveType = ConvertHelper.GetInteger(request.QueryString["dt"]);
			if (selectParas.DriveType > 0)
			{
				List<string> listTempDT = new List<string>();
				if ((selectParas.DriveType & 1) == 1)
				{
					listTempDT.Add("前轮驱动");
					listTempDT.Add("前置前驱");
				}
				if ((selectParas.DriveType & 2) == 2)
				{
					listTempDT.Add("后轮驱动");
					listTempDT.Add("前置后驱");
				}
				if ((selectParas.DriveType & 4) == 4)
				{
					listTempDT.Add("全时四驱");
				}
				if ((selectParas.DriveType & 8) == 8)
				{
					listTempDT.Add("分时四驱");
				}
				if ((selectParas.DriveType & 16) == 16)
				{
					listTempDT.Add("适时四驱");
				}
				if (listTempDT.Count > 0)
				{
					listq.Add(Query.In("Param.P_655", new BsonArray(listTempDT)));
				}
			}
			#endregion

			#region 燃料
			//燃料
			selectParas.FuelType = ConvertHelper.GetInteger(request.QueryString["f"]);
			if (selectParas.FuelType > 0)
			{
				List<string> listTempFT = new List<string>();
				// List<IMongoQuery> listTempFT = new List<IMongoQuery>();
				if ((selectParas.FuelType & 1) == 1)
				{
					listTempFT.Add("汽油");
				}
				if ((selectParas.FuelType & 2) == 2)
				{
					listTempFT.Add("油电混合动力");
				}
				if ((selectParas.FuelType & 4) == 4)
				{
					listTempFT.Add("油气混合动力");
				}
				if ((selectParas.FuelType & 8) == 8)
				{
					listTempFT.Add("柴油");
				}
				if ((selectParas.FuelType & 16) == 16)
				{
					listTempFT.Add("电力");
				}
				if (listTempFT.Count > 0)
				{
					listq.Add(Query.In("Param.P_578", new BsonArray(listTempFT)));
				}
			}
			#endregion

			#region 车门数
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
			if (selectParas.MinBodyDoors > 0)
			{
				listq.Add(Query.GTE("Param.P_563", selectParas.MinBodyDoors));
			}
			if (selectParas.MaxBodyDoors > 0)
			{
				listq.Add(Query.LTE("Param.P_563", selectParas.MaxBodyDoors));
			}
			#endregion

			#region 座位数
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
			if (selectParas.MinPerfSeatNum > 0 && selectParas.MaxPerfSeatNum > 0)
			{
				listq.Add(Query.And(Query.GTE("Param.P_665", selectParas.MinPerfSeatNum)
					, Query.LTE("Param.P_665", selectParas.MaxPerfSeatNum)));
			}
			else
			{
				if (selectParas.MinPerfSeatNum > 0)
				{
					listq.Add(Query.EQ("Param.P_665", selectParas.MinPerfSeatNum));
				}
			}
			#endregion

			#region 是否旅行版
			//是否旅行版
			selectParas.IsWagon = ConvertHelper.GetInteger(request.QueryString["lv"]);
			if (selectParas.IsWagon > 0)
			{
				listq.Add(Query.EQ("IsWagon", true));
			}
			#endregion

			#region 更多条件
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
						if (dicMoreCondition.ContainsKey(i + 1))
						{
							listq.Add(dicMoreCondition[i + 1]);
						}
					}
				}
			}
			#endregion

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
		/// 获取参数
		/// </summary>
		private void GetParametersForMemCache()
		{
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
				// CommonFunction.WriteInvokeLog("memcache");
				for (int i = 0; i < mcLength; i++)
				{
					if (carConfig[i] == '1')
					{
						// selectParas.CarConfig += (int)Math.Pow(2, i);
						//if (resultMoreSS != null && resultMoreSS.Count == 0)
						//{ break; }
						int paramid = 0;
						List<string> listParam = null;
						GetParamIDByGroupIndex(i, out paramid, out listParam);
						// SortedSet<int> paramSet = GetCarIDSetByParamIDAndValue(paramid, listParam);
						if (paramid > 0 && listParam != null)
						{
							keyForMemCache.Add("P_" + paramid);
							dicTempValue.Add("P_" + paramid, listParam);
						}
						//if (resultMoreSS == null)
						//{ resultMoreSS = paramSet; }
						//else
						//{
						//	resultMoreSS.IntersectWith(paramSet);
						//}
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

		private void GetParamIDByGroupIndex(int index, out int paramid, out List<string> listPvale)
		{
			paramid = 0;
			listPvale = null;
			if (index >= 0)
			{
				switch (index)
				{
					case 0: paramid = 425; listPvale = new List<string>() { "涡轮增压", "增压", "机械增压" }; break;
					case 1: paramid = 655; listPvale = new List<string>() { "前置四驱", "分时四驱", "智能四驱", "全时四驱", "适时四驱" }; break;
					case 2: paramid = 726; listPvale = new List<string>() { "盘式", "实心盘", "通风盘" }; break;
					case 3: paramid = 567; listPvale = new List<string>() { "单天窗", "双天窗", "全景" }; break;
					case 4: paramid = 601; listPvale = new List<string>() { "前后电动窗" }; break;
					case 5: paramid = 544; listPvale = new List<string>() { "真皮" }; break;
					case 6: paramid = 508; listPvale = new List<string>() { "电动" }; break;
					case 7: paramid = 504; listPvale = new List<string>() { "驾驶位", "前排座椅", "后排座椅", "双排座椅" }; break;
					case 8: paramid = 471; listPvale = new List<string>() { "'自动", "半自动" }; break;
					case 9: paramid = 622; listPvale = new List<string>() { "有" }; break;
					case 10: paramid = 700; listPvale = new List<string>() { "有", "ESC", "VSC", "ESP", "DSC" }; break;
					case 11: paramid = 703; listPvale = new List<string>() { "有" }; break;
					case 12: paramid = 702; listPvale = new List<string>() { "有" }; break;
					case 13: paramid = 516; listPvale = new List<string>() { "有" }; break;
					case 14: paramid = 816; listPvale = new List<string>() { "有" }; break;
					case 15: paramid = 545; listPvale = new List<string>() { "有" }; break;
					case 16: paramid = 469; listPvale = new List<string>() { "有" }; break;
					case 17: paramid = 836; listPvale = new List<string>() { "有" }; break;
					case 18: paramid = 481; listPvale = new List<string>() { "有" }; break;
					case 19: paramid = 494; listPvale = new List<string>() { "有" }; break;
					case 20: paramid = 495; listPvale = new List<string>() { "有" }; break;
					case 21: paramid = 614; listPvale = new List<string>() { "氙气" }; break;
					// case 22: paramid = 665; listPvale = new List<string>() { "" }; break;
					case 23: paramid = 714; listPvale = new List<string>() { "有" }; break;
					case 24: paramid = 578; listPvale = new List<string>() { "油气混合动力", "油电混合动力", "电力", "LPG", "CNG" }; break;
					case 25: paramid = 905; listPvale = new List<string>() { "离子发生器", "车内空气质量控制系统", "光触媒空气清新器", "有" }; break;
					case 26: paramid = 594; listPvale = new List<string>() { "全车车窗", "前排车窗", "驾驶员车窗" }; break;
					case 27: paramid = 547; listPvale = new List<string>() { "有" }; break;
					default: break;
				}
			}
		}

		/// <summary>
		/// 获取 选车数据 xml
		/// </summary>
		/// <returns></returns>
		private string GetSelectCarXml()
		{
			// Dictionary<int, int> pvDic = Car_SerialBll.GetSerialPvDic();

			//var clauses = new[] { Query.Or(Query.EQ("Code", "abc"),Query.EQ("Code", "abc2"))
			//    , Query.Or(Query.EQ("Code6", "abc"),Query.EQ("Code6", "abc2"))
			//    , Query.EQ("Code1", "abc"), Query.EQ("Description2", "def") };
			//// var query = Query.And(Query.Or(clauses), Query.EQ("Flag", 1));
			//// var query = Query.And(clauses);
			//var query = Query.And(Query.Or(Query.EQ("Code", "abc"), Query.EQ("Code", "abc2"))
			//    , Query.Or(Query.EQ("Code6", "abc"), Query.EQ("Code6", "abc2"))
			//    , Query.EQ("Code1", "abc"), Query.EQ("Description2", "def"));

			// query = Query.EQ("CarSaleState", "在销");
			// Query.And;
			//foreach (IMongoQuery imq in listq)
			//{
			//	query = Query.And(imq);
			//}
			// var queryTemp;


			//var temp = new List<IMongoQuery>();
			//var finalQuery = Query.And(temp);
			//query = Query.And(temp);

			//		query = Query.And(listq.ToArray());

			//		List<SelectCarFromMongoDB> listTemp = CarUtils.MongoDBHelper.GetAll<SelectCarFromMongoDB>(
			//mongoDBConnectionString
			//, mongoDBDatabase, collectionName, query, null, null
			//, new string[] { "_id", "CarID", "CsID", "ReferPrice", "CsShowName", "CsAllSpell" });
			int stattime = System.Environment.TickCount;
			int endtime = System.Environment.TickCount;
			if (keyForMemCache.Count > 0)
			{

				IDictionary<string, object> dicMemCache = MemCache.GetMultipleMemCacheByKey(keyForMemCache);
				endtime = System.Environment.TickCount;
				// CommonFunction.WriteInvokeLog(string.Format("取memcache数据结束 {0}", Math.Round((decimal)(endtime - stattime) / 1000, 2)));
				if (dicMemCache != null && dicMemCache.Count > 0
					&& dicTempValue != null && dicTempValue.Count > 0)
				{
					foreach (KeyValuePair<string, List<string>> kvp in dicTempValue)
					{
						if (dicMemCache.ContainsKey(kvp.Key) && dicTempValue.ContainsKey(kvp.Key))
						{
							SortedSet<int> ss = new SortedSet<int>();
							Dictionary<string, SortedSet<int>> dicParam = (Dictionary<string, SortedSet<int>>)dicMemCache[kvp.Key];
							if (dicParam != null)
							{
								foreach (string pvalue in dicTempValue[kvp.Key])
								{
									if (dicParam.ContainsKey(pvalue))
									{
										ss.UnionWith(dicParam[pvalue]);
									}
								}
								if (resultMoreSS == null)
								{ resultMoreSS = ss; }
								else
								{
									resultMoreSS.IntersectWith(ss);
								}
							}
						}
					}
				}
				if (resultMoreSS == null)
				{ resultMoreSS = new SortedSet<int>(); }
			}
			int ed = System.Environment.TickCount;
			// CommonFunction.WriteInvokeLog(string.Format("memcache数据结束 {0}", Math.Round((decimal)(ed - endtime) / 1000, 2)));
			// 如果有更多条件，并且更多条件取出的车款数为0 说明没有符合的车款，不用再查数据库
			if (resultMoreSS != null && resultMoreSS.Count == 0)
			{ return ""; }



			// List<BitAuto.CarChannel.BLL.SerialInfo> tmpListOld = new SelectCarToolBll().SelectCarByParameters(selectParas);
			// CommonFunction.WriteInvokeLog("resultMoreSS count:" + resultMoreSS.Count.ToString());
			DataTable dt = null;
			if (resultMoreSS != null && resultMoreSS.Count > 0)
			{
				dt = new DataTable();
				dt.Columns.Add(new DataColumn("CarID", Type.GetType("System.Int32")));
				foreach (int id in resultMoreSS)
				{
					DataRow dr = dt.NewRow();
					dr["CarID"] = id;
					dt.Rows.Add(dr);
				}
			}

			resultSS = SelectCar(selectParas, dt);
			if (resultMoreSS != null && resultMoreSS.Count > 0)
			{
				// resultSS = resultMoreSS;
				resultSS.IntersectWith(resultMoreSS);
			}

			Dictionary<int, int> pvDic = Car_SerialBll.GetAllSerialUVDict();
			List<BitAuto.CarChannel.BLL.SerialInfo> tmpList = new List<SerialInfo>();
			if (resultSS.Count > 0)
			{
				int carNum = 0;
				int serialNum = 0;
				Dictionary<int, SerialInfo> serialDic = new Dictionary<int, SerialInfo>();
				foreach (int carid in resultSS)
				{
					if (dicAllCarBaseInfo.ContainsKey(carid))
					{

						SelectCarFromMongoDB sc = dicAllCarBaseInfo[carid];
						int serialId = sc.CsID;
						double referPrice = sc.ReferPrice;
						if (!serialDic.ContainsKey(serialId))
						{
							string showName = sc.CsShowName;
							string spell = sc.CsAllSpell;
							SerialInfo info = new SerialInfo(serialId, showName, spell);
							if (pvDic.ContainsKey(serialId))
								info.PVNum = pvDic[serialId];
							else
								info.PVNum = 0;
							info.CarNum = 0;
							info.MinPrice = 0.0;
							info.MinReferPrice = referPrice;
							info.ImageUrl = Car_SerialBll.GetSerialImageUrl(serialId);
							info.PriceRange = "0";// new PageBase().GetSerialPriceRangeByID(serialId).Trim();
							if (info.PriceRange.Length == 0)
								info.PriceRange = "暂无报价";
							else
							{
								//取一个最低报价
								string[] priceSeg = info.PriceRange.Replace("万", "").Split('-');
								if (priceSeg.Length > 0)
								{
									double minPrice = 0.0;
									bool isDouble = Double.TryParse(priceSeg[0], out minPrice);
									if (isDouble)
										info.MinPrice = minPrice;
									info.MaxPrice = (priceSeg.Length > 1) ? ConvertHelper.GetDouble(priceSeg[1]) : minPrice;
								}
							}
							serialDic[serialId] = info;
							tmpList.Add(serialDic[serialId]);
						}
						else
						{
							//更新最低指导价，排序用
							if (referPrice < serialDic[serialId].MinReferPrice)
								serialDic[serialId].MinReferPrice = referPrice;
						}
						serialDic[serialId].CarNum++;
						serialDic[serialId].CarIdList += carid + ",";
					}
				}
			}
			else
			{ return ""; }


			//排序
			switch (sortMode)
			{
				case 1:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerial);
					break;
				case 2:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPrice);
					break;
				case 3:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPriceDesc);
					break;
				case 5:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinReferPrice);
					break;
				case 6:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinReferPriceDesc);
					break;
				case 4:
				default:
					tmpList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByPVDesc);
					break;
			}

			SerialNum = tmpList.Count;
			CarNum = 0;

			int startIndex = (pageNum - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;
			int counter = 0;
			StringBuilder sbTemp = new StringBuilder();
			int CarCount = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in tmpList)
			{
				CarCount = CarCount + info.CarNum;
				counter++;
				if (counter < startIndex || counter > endIndex)
				{ continue; }

				sbTemp.AppendLine(string.Format("<Serial id=\"{0}\" showName=\"{1}\" image=\"{2}\" price=\"{3}\" allspell=\"{4}\"/>"
					, info.SerialId
					, System.Security.SecurityElement.Escape(info.ShowName)
					, info.ImageUrl
					, info.PriceRange
					, info.AllSpell));

			}

			return string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><SelectResult serialCount=\"{0}\" carCount=\"{1}\">{2}</SelectResult>"
				, SerialNum, CarCount, sbTemp.ToString());
		}

		private SortedSet<int> GetCarIDSetByParamIDAndValue(int paramid, List<string> pvalueList)
		{
			SortedSet<int> ss = new SortedSet<int>();
			if (paramid > 0 && pvalueList.Count > 0)
			{
				Dictionary<string, SortedSet<int>> dicParam = new Dictionary<string, SortedSet<int>>();
				if (WebConfig.IsUseMemcache)
				{
					// memcache
					object obj = MemCache.GetMemCacheByKey("P_" + paramid);
					if (obj != null)
					{
						dicParam = (Dictionary<string, SortedSet<int>>)obj;
					}
				}
				else
				{
					// cache
					object obj = CacheManager.GetCachedData("P_" + paramid);
					if (obj != null)
					{
						dicParam = (Dictionary<string, SortedSet<int>>)obj;
					}
					else
					{
						string sqlGetByPid = @"select cdb.CarId,cdb.Pvalue
										from Car_relation car
										left join dbo.CarDataBase cdb
										on car.Car_Id=cdb.CarId and cdb.ParamId={0}
										where car.IsState=0 and cdb.Pvalue<>''
										order by cdb.Pvalue,cdb.CarId";
						DataSet dsGetByPid = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
							, CommandType.Text, string.Format(sqlGetByPid, paramid));
						if (dsGetByPid != null && dsGetByPid.Tables.Count > 0 && dsGetByPid.Tables[0].Rows.Count > 0)
						{
							foreach (DataRow dr in dsGetByPid.Tables[0].Rows)
							{
								int carid = int.Parse(dr["CarId"].ToString());
								string pvalue = dr["Pvalue"].ToString().Trim();
								if (pvalue != "")
								{
									if (dicParam.ContainsKey(pvalue))
									{
										if (!dicParam[pvalue].Contains(carid))
										{ dicParam[pvalue].Add(carid); }
									}
									else
									{
										SortedSet<int> sst = new SortedSet<int>();
										sst.Add(carid);
										dicParam.Add(pvalue, sst);
									}
								}
							}
						}
						CacheManager.InsertCache("P_" + paramid, dicParam, 60 * 1);
					}
				}

				// 取所有符合value的并集
				if (dicParam != null && dicParam.Count > 0)
				{
					foreach (string pvalue in pvalueList)
					{
						if (dicParam.ContainsKey(pvalue) && dicParam[pvalue].Count > 0)
						{
							ss.UnionWith(dicParam[pvalue]);
						}
					}
				}
			}

			return ss;
		}

		/// <summary>
		/// 取2个集合的交集 返回第一个集合
		/// </summary>
		/// <param name="ss1"></param>
		/// <param name="ss2"></param>
		private bool SetSortedSetAndSortedSet(SortedSet<int> ss1, SortedSet<int> ss2)
		{
			// 交集后是否还有元素
			bool hasCar = false;
			if (ss1 != null && ss2 != null && ss2.Count > 0 && ss2.Count > 0)
			{
				ss1.IntersectWith(ss2);
				if (ss1.Count > 0)
				{ hasCar = true; }
			}
			else
			{
				ss1 = new SortedSet<int>();
			}
			return hasCar;
		}

		private void IntiMoreCndition()
		{
			string cacheKey = "TestForSelectCarToXml_MoreCnditionKey";
			dicMoreCondition = (Dictionary<int, IMongoQuery>)CacheManager.GetCachedData(cacheKey);
			if (dicMoreCondition == null)
			{
				dicMoreCondition = new Dictionary<int, IMongoQuery>();
				#region 更多条件索引1
				{
					dicMoreCondition.Add(1, Query.In("Param.P_425", "涡轮增压", "增压", "机械增压"));
				}
				#endregion

				#region 更多条件索引2
				{
					// dicMoreCondition.Add(2, Query.In("Param.P_655", "前置四驱", "分时四驱", "四轮驱动", "智能四驱", "全时四驱", "适时四驱"));
				}
				#endregion

				#region 更多条件索引3
				{
					dicMoreCondition.Add(3,
					Query.And(
						Query.In("Param.P_726", "盘式", "实心盘", "通风盘")
					  , Query.In("Param.P_718", "盘式", "实心盘", "通风盘")
					  )
					);
				}
				#endregion

				#region 更多条件索引4
				{
					dicMoreCondition.Add(4, Query.In("Param.P_567", "单天窗", "双天窗", "全景"));
				}
				#endregion

				#region 更多条件索引5
				{
					dicMoreCondition.Add(5, Query.EQ("Param.P_601", "前后电动窗"));
				}
				#endregion

				#region 更多条件索引6
				{
					dicMoreCondition.Add(6, Query.EQ("Param.P_544", "真皮"));
				}
				#endregion

				#region 更多条件索引7
				{
					dicMoreCondition.Add(7, Query.EQ("Param.P_508", "电动"));
				}
				#endregion

				#region 更多条件索引8
				{
					dicMoreCondition.Add(8, Query.In("Param.P_504", "驾驶位", "前排座椅", "后排座椅", "双排座椅"));
				}
				#endregion

				#region 更多条件索引9
				{
					dicMoreCondition.Add(9, Query.In("Param.P_471", "自动", "半自动"));
				}
				#endregion

				#region 更多条件索引10
				{
					dicMoreCondition.Add(10, Query.EQ("Param.P_622", "有"));
				}
				#endregion

				#region 更多条件索引11
				{
					dicMoreCondition.Add(11, Query.In("Param.P_700", "有", "ESC", "VSC", "ESP", "DSC"));
				}
				#endregion

				#region 更多条件索引12
				{
					dicMoreCondition.Add(12, Query.EQ("Param.P_703", "有"));
				}
				#endregion

				#region 更多条件索引13
				{
					dicMoreCondition.Add(13, Query.EQ("Param.P_702", "有"));
				}
				#endregion

				#region 更多条件索引14
				{
					dicMoreCondition.Add(14, Query.EQ("Param.P_516", "有"));
				}
				#endregion

				#region 更多条件索引15
				{
					dicMoreCondition.Add(15, Query.EQ("Param.P_816", "有"));
				}
				#endregion

				#region 更多条件索引16
				{
					dicMoreCondition.Add(16, Query.EQ("Param.P_545", "有"));
				}
				#endregion

				#region 更多条件索引17
				{
					dicMoreCondition.Add(17, Query.EQ("Param.P_469", "有"));
				}
				#endregion

				#region 更多条件索引18
				{
					dicMoreCondition.Add(18, Query.EQ("Param.P_836", "有"));
				}
				#endregion

				#region 更多条件索引19
				{
					dicMoreCondition.Add(19, Query.EQ("Param.P_481", "有"));
				}
				#endregion

				#region 更多条件索引20
				{
					dicMoreCondition.Add(20, Query.EQ("Param.P_494", "有"));
				}
				#endregion

				#region 更多条件索引21
				{
					dicMoreCondition.Add(21, Query.EQ("Param.P_495", "有"));
				}
				#endregion

				#region 更多条件索引22
				{
					dicMoreCondition.Add(22, Query.EQ("Param.P_614", "氙气"));
				}
				#endregion

				#region 更多条件索引23
				{
					// dicMoreCondition.Add(23, Query.GT("Param.P_665", 5));
				}
				#endregion

				#region 更多条件索引24
				{
					dicMoreCondition.Add(24, Query.EQ("Param.P_714", "有"));
				}
				#endregion

				#region 更多条件索引25
				{
					dicMoreCondition.Add(25, Query.In("Param.P_578", "油气混合动力", "油电混合动力", "电力", "LPG", "CNG"));
				}
				#endregion

				#region 更多条件索引26
				{
					dicMoreCondition.Add(26, Query.In("Param.P_905", "离子发生器", "车内空气质量控制系统", "光触媒空气清新器", "有"));
				}
				#endregion

				#region 更多条件索引27
				{
					dicMoreCondition.Add(27, Query.In("Param.P_594", "全车车窗", "前排车窗", "驾驶员车窗"));
				}
				#endregion

				#region 更多条件索引28
				{
					dicMoreCondition.Add(28, Query.EQ("Param.P_547", "有"));
				}
				#endregion

				CacheManager.InsertCache(cacheKey, dicMoreCondition, 60);
			}
		}

		/// <summary>
		/// 根据条件选车
		/// </summary>
		/// <param name="paras"></param>
		/// <returns></returns>
		public SortedSet<int> SelectCar(SelectCarParameters paras, DataTable dt)
		{
			string condition = "1=1";
			List<SqlParameter> sqlParas = new List<SqlParameter>();
			if (paras.MinPrice != 0)
			{
				condition += " AND a.MaxPrice >= @minPrice";
				sqlParas.Add(new SqlParameter("@minPrice", paras.MinPrice));
			}
			if (paras.MinPrice == 0 && paras.MaxPrice != 0)
			{
				condition += " AND a.MaxPrice > 0 AND a.MinPrice <= @maxPrice";
				sqlParas.Add(new SqlParameter("@maxPrice", paras.MaxPrice));
			}
			else if (paras.MaxPrice != 0)
			{
				condition += " AND a.MinPrice <= @maxPrice";
				sqlParas.Add(new SqlParameter("@maxPrice", paras.MaxPrice));
			}

			if (paras.MinReferPrice != 0)
			{
				condition += " AND a.CarReferPrice >= @minReferPrice";
				sqlParas.Add(new SqlParameter("@minReferPrice", paras.MinReferPrice));
			}
			if (paras.MaxReferPrice != 0)
			{
				condition += " AND a.CarReferPrice <= @maxReferPrice";
				sqlParas.Add(new SqlParameter("@maxReferPrice", paras.MaxReferPrice));
			}

			if (paras.MinDis != 0)
			{
				condition += " AND a.ExhaustL >= @minDis";
				sqlParas.Add(new SqlParameter("@minDis", paras.MinDis));
			}
			if (paras.MaxDis != 0)
			{
				condition += " AND a.ExhaustL <= @maxDis";
				sqlParas.Add(new SqlParameter("@maxDis", paras.MaxDis));
			}
			if (paras.TransmissionType != 0)
			{
				condition += " AND a.TransmissionType&@trans > 0";
				sqlParas.Add(new SqlParameter("@trans", paras.TransmissionType));
			}
			if (paras.BodyForm != 0)
			{
				condition += " AND a.BodyForm&@body > 0";
				sqlParas.Add(new SqlParameter("@body", paras.BodyForm));
			}
			if (paras.Level != 0)
			{
				condition += " AND a.CarLevel&@level > 0";
				sqlParas.Add(new SqlParameter("@level", paras.Level));
			}
			else
			{
				// add by chengl Mar.5.2014 排除概念车
				condition += " AND a.CarLevel > 0";
			}
			if (paras.Purpose != 0)
			{
				condition += " AND a.Purpose&@purpose > 0";
				sqlParas.Add(new SqlParameter("@purpose", paras.Purpose));
			}
			if (paras.Country != 0)
			{
				condition += " AND a.CarCountry&@country > 0";
				sqlParas.Add(new SqlParameter("@country", paras.Country));
			}
			if (paras.ComfortableConfig != 0)
			{
				condition += " AND a.Comfortable&@comf = @comf";
				sqlParas.Add(new SqlParameter("@comf", paras.ComfortableConfig));
			}
			if (paras.SafetyConfig != 0)
			{
				condition += " AND a.Safety&@safe = @safe";
				sqlParas.Add(new SqlParameter("@safe", paras.SafetyConfig));
			}
			if (paras.DriveType > 0)
			{
				condition += " AND a.DriveType&@DriveType > 0";
				sqlParas.Add(new SqlParameter("@DriveType", paras.DriveType));
			}
			if (paras.FuelType > 0)
			{
				condition += " AND a.FuelType&@FuelType > 0";
				sqlParas.Add(new SqlParameter("@FuelType", paras.FuelType));
			}
			if (paras.MinBodyDoors != 0)
			{
				condition += " AND a.Doors >= @MinBodyDoors";
				sqlParas.Add(new SqlParameter("@MinBodyDoors", paras.MinBodyDoors));
			}
			if (paras.MaxBodyDoors != 0)
			{
				condition += " AND a.Doors <= @MaxBodyDoors";
				sqlParas.Add(new SqlParameter("@MaxBodyDoors", paras.MaxBodyDoors));
			}

			if (paras.MinPerfSeatNum > 0 && paras.MaxPerfSeatNum > 0)
			{
				condition += " AND a.SeatNum >= @MinSeatNum AND a.SeatNum <= @MaxSeatNum";
				sqlParas.Add(new SqlParameter("@MinSeatNum", paras.MinPerfSeatNum));
				sqlParas.Add(new SqlParameter("@MaxSeatNum", paras.MaxPerfSeatNum));
			}
			else
			{
				if (paras.MinPerfSeatNum > 0)
				{
					condition += " AND a.SeatNum = @SeatNum";
					sqlParas.Add(new SqlParameter("@SeatNum", paras.MinPerfSeatNum));
				}
			}
			if (paras.IsWagon > 0)
			{
				condition += " AND a.IsWagon=@IsWagon";
				sqlParas.Add(new SqlParameter("@IsWagon", paras.IsWagon));
			}

			if (paras.BrandType != 0)
			{
				switch (paras.BrandType)
				{
					case 8:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 4));
						break;
					case 9:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 18));
						break;
					case 10:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 8));
						break;
					case 11:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 484));
						break;
					case 12:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 2));
						break;
					case 16:
						condition += " AND a.CarCountry&@country > 0";
						sqlParas.Add(new SqlParameter("@country", 16));
						break;
					default:
						condition += " AND a.BrandType&@brandType > 0";
						sqlParas.Add(new SqlParameter("@brandType", paras.BrandType));
						break;
				}
			}

			string sql2 = "";
			if (dt != null && dt.Rows.Count > 0)
			{
				sql2 = " inner join @carids t	on a.carid=t.carid";
				SqlParameter spType = new SqlParameter("@carids", SqlDbType.Structured);
				spType.TypeName = "Ty_CarIDList";
				spType.Value = dt;
				sqlParas.Add(spType);
			}

			string sqlStr = @"SELECT a.carId
											FROM CarInfoForSelecting a 
											 {1} {0}
											order by a.carId";
			if (condition.Length > 0)
			{
				sqlStr = string.Format(sqlStr
					, " WHERE " + condition
					, sql2);
			}
			else
			{
				sqlStr = string.Format(sqlStr, " ");
			}



			SortedSet<int> ss = new SortedSet<int>();
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, sqlParas.ToArray());
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					int carid = int.Parse(dr["carId"].ToString());
					if (!ss.Contains(carid))
					{ ss.Add(carid); }
				}
			}
			return ss;
		}

		private void IntiAllCarBaseInfo()
		{
			string keyAllCarBaseInfo = "TestForSelectCarToXml_AllCarBaseInfo";
			object obj = CacheManager.GetCachedData(keyAllCarBaseInfo);
			if (obj != null)
			{
				dicAllCarBaseInfo = (Dictionary<int, SelectCarFromMongoDB>)obj;
			}
			else
			{
				string sql = @"SELECT car.car_Id,car.Car_Name,car.cs_Id,car.Car_ReferPrice,cs.cs_ShowName,cs.allSpell 
									from Car_Basic car
									left join car_serial cs on car.Cs_Id=cs.cs_Id
									where car.IsState=1 and cs.IsState=1";
				DataSet dsAllCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
					, CommandType.Text, sql);
				if (dsAllCar != null && dsAllCar.Tables.Count > 0 && dsAllCar.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in dsAllCar.Tables[0].Rows)
					{
						int carid = int.Parse(dr["car_Id"].ToString());
						string carName = dr["Car_Name"].ToString().Trim();
						int csid = int.Parse(dr["cs_Id"].ToString());
						double rp = 0;
						if (double.TryParse(dr["Car_ReferPrice"].ToString(), out rp))
						{ }
						string csShowName = dr["cs_ShowName"].ToString().Trim();
						string csAllSpell = dr["allSpell"].ToString().Trim();
						SelectCarFromMongoDB sc = new SelectCarFromMongoDB();
						sc.CsID = csid;
						sc.ReferPrice = rp;
						sc.CsShowName = csShowName;
						sc.CsAllSpell = csAllSpell;
						if (!dicAllCarBaseInfo.ContainsKey(carid))
						{ dicAllCarBaseInfo.Add(carid, sc); }
					}
				}
				CacheManager.InsertCache(keyAllCarBaseInfo, dicAllCarBaseInfo, 60 * 24);
			}

		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}

	[Serializable]
	public class SelectCarFromMongoDB
	{
		// public object _id { get; set; }
		// public int CarID { get; set; }
		public int CsID { get; set; }
		public double ReferPrice { get; set; }
		public string CsShowName { get; set; }
		public string CsAllSpell { get; set; }
	}

}