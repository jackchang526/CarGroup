using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarUtils.Define;
namespace BitAuto.CarChannel.BLL.Data
{
	/// <summary>
	/// 子品牌数据
	/// </summary>
	public class SerialEntity : BaseEntity
	{
		protected int m_brandId;
		protected string m_showName;
        protected int m_pvNum;
        protected double m_minPrice;
		//protected BrandEntity m_brand;
		protected WeakReference m_brand;
		protected LevelEntity m_level;
		protected CarEntity[] m_carList;
		protected string m_virtues;
		protected string m_defect;
		protected string[] m_exhaust;
		protected string[] m_gearbox;
		protected string[] m_purpose;
		protected string[] m_colors;
		protected string m_referPrice;
		protected string m_price;
		protected string m_saleState;
		protected string m_officialSite;
		protected string m_repairPolicy;

		protected string m_maiCheCeShi;
		#region 需要后加载的一些信息
		//这些信息使用InitOtherData()方法加载
		protected bool m_otherDataLoaded;		//是否已经加载过
		protected string m_officialFuelCost;
		protected string m_summaryFuelCost;
		protected string m_guestFuelcost;
		protected int m_askCount;
		protected int m_dianpingCount;
		protected int m_picCount;
		protected int m_videoCount;
		protected string m_defaultPic;
		protected string m_baaUrl;

		//各类文章
		protected string m_gouCheShouCe;
		protected string m_keJi;
		protected string m_shangShi;
		protected string m_weiXiuBaoYang;
		protected string m_xiaoShouShuJu;
		protected string m_yiCheCeShi;
		protected string m_anQuan;
		protected string m_youHao;

		#endregion

		/// <summary>
		/// 构造函数
		/// </summary>
		public SerialEntity()
		{
			m_otherDataLoaded = false;
		}
		public SerialEntity(DataRow row)
			: this()
		{
			SetValueByDataRow(row);
		}
		/// <summary>
		/// 子品牌显示名
		/// </summary>
		public string ShowName
		{
			get { return m_showName; }
			set { m_showName = value; }
		}

        public int PvNum
        {
            get { return m_pvNum; }
            set { m_pvNum = value; }
        }

        public double MinPrice
        {
            get { return m_minPrice; }
            set { m_minPrice = value; }
        }

		/// <summary>
		/// 所属品牌
		/// </summary>
		public BrandEntity Brand
		{
			get
			{
				BrandEntity brand = null;
				if (m_brand != null)
					brand = m_brand.Target as BrandEntity;
				if (brand == null)
				{
					brand = (BrandEntity)DataManager.GetDataEntity(EntityType.Brand, m_brandId);
					if (m_brand != null)
						m_brand.Target = brand;
					else
						m_brand = new WeakReference(brand);
				}
				return brand;
			}
		}

		/// <summary>
		/// 所属品牌ID
		/// </summary>
		public int BrandId
		{
			get { return m_brandId; }
		}

		/// <summary>
		/// 子品牌级别
		/// </summary>
		public LevelEntity Level
		{
			get { return m_level; }
		}

		/// <summary>
		/// 子品牌优点，好评
		/// </summary>
		public string Virtues
		{
			get { return m_virtues; }
		}

		/// <summary>
		/// 子品牌缺点，差评
		/// </summary>
		public string Defect
		{
			get { return m_defect; }
		}

		/// <summary>
		/// 子品牌排量列表
		/// </summary>
		public string[] ExhaustList
		{
			get { return m_exhaust; }
		}

		/// <summary>
		/// 变速箱形式列表
		/// </summary>
		public string[] GearBoxList
		{
			get { return m_gearbox; }
		}

		/// <summary>
		/// 销售状态
		/// </summary>
		public string SaleState
		{
			get { return m_saleState; }
		}

		/// <summary>
		/// 车型列表
		/// </summary>
		public CarEntity[] CarList
		{
			get
			{
				if (m_carList == null)
				{
					//string muName = "mutex_carlist_" + this.Id;
					//Mutex m = new Mutex(false, muName);
					//m.WaitOne();
					if (m_carList == null)
						QuickInitCarList();
					//m.ReleaseMutex();
					//m.Close();
				}
				return m_carList;
			}
		}

		/// <summary>
		/// 子品牌指导价
		/// </summary>
		public string ReferPrice
		{
			get
			{
                //if (String.IsNullOrEmpty(m_referPrice))
                //    GetReferPrice();
				return m_referPrice;
			}
		}

		/// <summary>
		/// 子品牌报价
		/// </summary>
		public string Price
		{
			get
			{
				if (String.IsNullOrEmpty(m_price))
					GetPrice();
				return m_price;
			}
		}

		/// <summary>
		/// 子品牌的排量
		/// </summary>
		public string Exhaust
		{
			get
			{
				return GetExhaust();
			}
		}

		/// <summary>
		/// 子品牌的变速箱列表
		/// </summary>
		public string Transmission
		{
			get
			{
				return GetTransmission();
			}
		}

		/// <summary>
		/// 子品牌的官方网站
		/// </summary>
		public string OfficialSite
		{
			get { return m_officialSite; }
		}

		/// <summary>
		/// 保修政策
		/// </summary>
		public string RepairPolicy
		{
			get { return m_repairPolicy; }
		}

		/// <summary>
		/// 子品牌用途
		/// </summary>
		public string[] PurposeList
		{
			get { return m_purpose; }
		}


		/// <summary>
		/// 颜色列表
		/// </summary>
		public string[] Colors
		{
			get
			{
				if (m_colors == null)
				{
					if (m_saleState == "停销")
						// 停销子品牌显示全部颜色
						m_colors = new Car_SerialBll().GetSerialColors(m_id, 0, true).ToArray();
					else
						// 非停销子品牌显示非停销车型颜色
						m_colors = new Car_SerialBll().GetSerialColors(m_id, 0, false).ToArray();
				}
				return m_colors;
			}
		}

		/// <summary>
		/// 买车测试文章
		/// </summary>
		public string MaiCheCeShi
		{
			get { return m_maiCheCeShi; }
		}


		/// <summary>
		/// 子品牌论坛Url
		/// </summary>
		public string BaaUrl
		{
			get
			{
				CheckOtherData();
				return m_baaUrl;
			}
		}

		/// <summary>
		/// 官方油耗
		/// </summary>
		public string OfficialFuelCost
		{
			get
			{
				CheckOtherData();
				return m_officialFuelCost;
			}
		}

		/// <summary>
		/// 综合油耗
		/// </summary>
		public string SummaryFuelCost
		{
			get
			{
				CheckOtherData();
				return m_summaryFuelCost;
			}
		}

		/// <summary>
		/// 网友提交的油耗
		/// </summary>
		public string GuestFuelCost
		{
			get
			{
				CheckOtherData();
				return m_guestFuelcost;
			}
		}

		/// <summary>
		/// 答疑数量
		/// </summary>
		public int AskCount
		{
			get
			{
				CheckOtherData();
				return m_askCount;
			}
		}

		/// <summary>
		/// 文章点评数量
		/// </summary>
		public int DianPingCount
		{
			get
			{
				CheckOtherData();
				return m_dianpingCount;
			}
		}

		/// <summary>
		/// 子品牌图片数量
		/// </summary>
		public int PicCount
		{
			get
			{
				CheckOtherData();
				return m_picCount;
			}
		}

		/// <summary>
		/// 子品牌默认图片
		/// </summary>
		public string DefaultPic
		{
			get
			{
				CheckOtherData();
				return m_defaultPic;
			}
		}

		/// <summary>
		/// 购车手册文章
		/// </summary>
		public string GouCheShouCe
		{
			get
			{
				CheckOtherData();
				return m_gouCheShouCe;
			}
		}

		/// <summary>
		/// 科技文章
		/// </summary>
		public string KeJi
		{
			get
			{
				CheckOtherData();
				return m_keJi;
			}
		}

		/// <summary>
		/// 上市文章
		/// </summary>
		public string ShangShi
		{
			get
			{
				CheckOtherData();
				return m_shangShi;
			}
		}

		/// <summary>
		/// 维修保养
		/// </summary>
		public string WeiXiuBaoYang
		{
			get
			{
				CheckOtherData();
				return m_weiXiuBaoYang;
			}
		}

		/// <summary>
		/// 销售数量链接
		/// </summary>
		public string XiaoShouShuJu
		{
			get
			{
				CheckOtherData();
				return m_xiaoShouShuJu;
			}
		}

		/// <summary>
		/// 易车测试
		/// </summary>
		public string YiCheCeShi
		{
			get
			{
				CheckOtherData();
				return m_yiCheCeShi;
			}
		}


		/// <summary>
		/// 安全文章
		/// </summary>
		public string AnQuan
		{
			get
			{
				CheckOtherData();
				return m_anQuan;
			}
		}

		/// <summary>
		/// 油耗文章
		/// </summary>
		public string Youhao
		{
			get
			{
				CheckOtherData();
				return m_youHao;
			}
		}

		/// <summary>
		/// 子品牌的视频数量
		/// </summary>
		public int VideosCount
		{
			get
			{
				CheckOtherData();
				return m_videoCount;
			}
		}


		/// <summary>
		/// 初始化子品牌数据
		/// </summary>
		/// <param name="id"></param>
		public override void InitData(int id)
		{
			DataSet ds = new TSerialDAL().GetSerialDataById(id);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
				SetValueByDataRow(row);
			}
			else
				m_id = 0;
		}
		private void SetValueByDataRow(DataRow row)
		{
			if (row == null)
				return;
			m_id = ConvertHelper.GetInteger(row["cs_Id"]);
			m_name = row["cs_Name"].ToString().Trim();
			m_showName = row["cs_ShowName"].ToString().Trim();
			m_spell = row["spell"].ToString().Trim();
			m_allSpell = row["allSpell"].ToString().Trim().ToLower();
			m_virtues = row["cs_Virtues"].ToString().Trim();
			m_defect = row["cs_Defect"].ToString().Trim();
			m_seoName = row["cs_seoname"].ToString().Trim();
			m_officialSite = row["cs_Url"].ToString().Trim();
			m_repairPolicy = row["CsRepairPolicy"].ToString().Trim();
			m_maiCheCeShi = row["bitautoTestURL"].ToString().Trim();

			m_exhaust = row["Engine_Exhaust"].ToString().Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
			Array.Sort(m_exhaust);

			m_gearbox = row["UnderPan_Num_Type"].ToString().Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
			m_brandId = ConvertHelper.GetInteger(row["cb_Id"]);
			m_isState = ConvertHelper.GetInteger(row["IsState"]);
			m_saleState = row["CsSaleState"].ToString().Trim();
            m_referPrice = string.IsNullOrEmpty(row["ReferPriceRange"].ToString().Trim()) ? "暂无" : row["ReferPriceRange"].ToString().Trim() + "万";
			string levelName = row["cs_CarLevel"].ToString().Trim();
			//if (levelName == "紧凑型车")
			//    levelName = "紧凑型";
			//else if (levelName == "豪华型车")
			//    levelName = "豪华车";
			//else if (levelName == "中大型车")
			//    levelName = "中大型";
			try
			{
				// modified by chengl Nov.13.2012 当概念车是 级别为null
				// EnumCollection.SerialLevelEnum level = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), levelName);

				//EnumCollection.SerialAllLevelEnum level = (EnumCollection.SerialAllLevelEnum)Enum.Parse(typeof(EnumCollection.SerialAllLevelEnum), levelName);
				var levelId = CarLevelDefine.GetLevelIdByName(levelName);
				m_level = (LevelEntity)DataManager.GetDataEntity(EntityType.Level, levelId);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString() + "\r\nserialId=" + m_id);
			}

			//用途
			string purposeStr = row["CsPurpose"].ToString().Trim();
			string[] purposes = purposeStr.Split(',');
			List<string> purposeList = new List<string>();
			foreach (string purIdStr in purposes)
			{
				int purId = ConvertHelper.GetInteger(purIdStr);
				if (purId > 0)
				{
					string purName = CommonFunction.GetPurposeById(purId);
					if (purName.Length > 0)
						purposeList.Add(purName);
				}
			}
			m_purpose = purposeList.ToArray();
		}
		/// <summary>
		/// 是否包含停销车型
		/// </summary>
		/// <returns></returns>
		public bool ContainsStopSaleCar()
		{
			bool isContains = false;
			foreach (CarEntity car in this.CarList)
			{
				if (car.SaleState == "停销")
				{
					isContains = true;
					break;
				}
			}

			return isContains;
		}


		/// <summary>
		/// 检查其他信息是否已经加载
		/// </summary>
		private void CheckOtherData()
		{
			if (!m_otherDataLoaded)
			{
				InitOtherData();
				m_otherDataLoaded = true;
			}
		}



		/// <summary>
		/// 初始化其他的一些信息
		/// </summary>
		private void InitOtherData()
		{
			// 子品油耗
			PageBase pageBase = new PageBase();
			m_officialFuelCost = pageBase.GetSerialPerfFuelCostPer100(m_id, 0);
			m_summaryFuelCost = pageBase.GetSerialSummaryFuel(m_id, 0);
			m_guestFuelcost = pageBase.GetSerialDianPingYouHaoByCsID(m_id);
			// 子品牌业务数量统计
			m_askCount = pageBase.GetSerialAskCountByCsID(m_id);
			m_dianpingCount = pageBase.GetSerialDianPingCountByCsID(m_id);
			pageBase.GetSerialPicAndCountByCsID(m_id, out m_defaultPic, out m_picCount, true);
			// 子品牌文章link
			m_gouCheShouCe = pageBase.GetCsRainbowAndURLInfo(m_id, 42);
			m_keJi = pageBase.GetCsRainbowAndURLInfo(m_id, 41);
			m_shangShi = pageBase.GetCsRainbowAndURLInfo(m_id, 37);
			m_weiXiuBaoYang = pageBase.GetCsRainbowAndURLInfo(m_id, 40);
			m_xiaoShouShuJu = "http://car.bitauto.com/" + m_allSpell + "/xiaoliang/";
			m_yiCheCeShi = pageBase.GetCsRainbowAndURLInfo(m_id, 43);
			m_anQuan = pageBase.GetCsRainbowAndURLInfo(m_id, 44);
			m_youHao = "";
			Car_SerialBll serialBll = new Car_SerialBll();
			m_baaUrl = serialBll.GetForumUrlBySerialId(m_id);
			m_videoCount = serialBll.GetSerialVideoCount(m_id);
		}

		///// <summary>
		///// 初始化车型列表
		///// </summary>
		//private void InitCarList()
		//{
		//    List<CarEntity> carList = new List<CarEntity>();
		//    DataSet ds = new TSerialDAL().GetCarIdListBySerialId(m_id);
		//    if(ds != null && ds.Tables.Count > 0)
		//    {
		//        foreach(DataRow row in ds.Tables[0].Rows)
		//        {
		//            int carId = ConvertHelper.GetInteger(row["Car_Id"]);
		//            if(carId > 0)
		//                carList.Add((CarEntity)DataManager.GetDataEntity(EntityType.Car, carId));
		//        }

		//        //默认排序
		//        carList.Sort(SerialEntity.CompareCarListByDefault);
		//        m_carList = carList.ToArray();
		//    }
		//}

		/// <summary>
		/// 一次访问数据库初始化车型列表
		/// </summary>
		private void QuickInitCarList()
		{
			List<CarEntity> carList = new List<CarEntity>();
			DataSet ds = new TCarDAL().GetCarsDataBySerialId(m_id);
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					CarEntity ce = new CarEntity(row);
					carList.Add(ce);
					DataManager.AddDataEntity(EntityType.Car, ce);
				}

				//默认排序
				carList.Sort(SerialEntity.CompareCarListByDefault);
				m_carList = carList.ToArray();
			}
		}

		/// <summary>
		/// 获取子品牌的指导价
		/// </summary>
		private void GetReferPrice()
		{
			double maxPrice = Double.MinValue;
			double minPrice = Double.MaxValue;
			foreach (CarEntity ce in CarList)
			{
				if (ce.SaleState == "停销")
					continue;
				// add by chengl Feb.3.2012
				if (ce.ReferPrice < 0.1)
				{ continue; }
				if (ce.ReferPrice > maxPrice)
					maxPrice = ce.ReferPrice;
				if (ce.ReferPrice < minPrice)
					minPrice = ce.ReferPrice;
			}

			if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
				m_referPrice = "暂无";
			else
			{
				// old
				// m_referPrice = minPrice + "万-" + maxPrice + "万";
				string min = minPrice.ToString();
				string max = maxPrice.ToString();
				if (minPrice >= 100)
				{ min = Convert.ToString((int)minPrice); }
				if (maxPrice >= 100)
				{ max = Convert.ToString((int)maxPrice); }

				if (min == max)
				{
					m_referPrice = min + "万";
				}
				else
				{
					m_referPrice = string.Format("{0}-{1}万", min, max);
				}
				//m_referPrice = min + "-" + max + "万";
			}
		}

		/// <summary>
		/// 获取子品牌报价
		/// </summary>
		private void GetPrice()
		{
			if (m_saleState == "停销")
				m_price = "停售";
			else if (m_saleState == "待销")
				m_price = "未上市";
			else
				m_price = new PageBase().GetSerialPriceRangeByID(m_id);
		}

		/// <summary>
		/// 获取子品牌的排量列表
		/// </summary>
		/// <returns></returns>
		private string GetExhaust()
		{
			string exhaust = "";
			string allExhaust = "";
			List<double> exhaustList = new List<double>();
			foreach (string tmpExhaust in m_exhaust)
			{
				double dEx = 0.0;
				Double.TryParse(tmpExhaust.ToUpper().Replace("L", ""), out dEx);
				if (!exhaustList.Contains(dEx) && dEx != 0.0)
					exhaustList.Add(dEx);
			}
			exhaustList.Sort();

			for (int i = 0; i < exhaustList.Count; i++)
			{
				string tmpExhaust = Math.Round(exhaustList[i], 1).ToString("F1");
				if (allExhaust.Length > 0)
					allExhaust += "　";
				allExhaust += tmpExhaust + "L";
			}
			if (exhaustList.Count <= 5)
				exhaust = allExhaust;
			else
				exhaust = exhaustList[0] + "L　" + exhaustList[1] + "L　" + exhaustList[2] + "L　…　" + exhaustList[exhaustList.Count - 1] + "L";
			return exhaust;
		}

		/// <summary>
		/// 获取子品牌的变速箱列表
		/// </summary>
		/// <returns></returns>
		private string GetTransmission()
		{
			string trans = "";
			List<string> tranList = new List<string>();
			foreach (string tmpTran in m_gearbox)
			{
				string newTran = tmpTran;
				int pos = newTran.IndexOf("挡");
				if (pos >= 0)
					newTran = newTran.Substring(pos + 1).Trim();
				if (newTran.ToUpper().IndexOf("双离合") >= 0)
					newTran = "双离合";
				if (newTran.Length > 0 && !tranList.Contains(newTran))
					tranList.Add(newTran);
			}
			tranList.Sort(NodeCompare.CompareTransmissionType);
			foreach (string tmpTran in tranList)
			{
				if (trans.Length > 0)
					trans += "　";
				trans += tmpTran;
			}
			if (tranList.Count > 3)
				trans = tranList[0] + "　" + tranList[1] + "　…　" + tranList[tranList.Count - 1];

			return trans;
		}

		/// <summary>
		/// 比较两个车型信息，用于默认排序
		/// </summary>
		/// <param name="car1"></param>
		/// <param name="car2"></param>
		/// <returns></returns>
		public static int CompareCarListByDefault(CarEntity car1, CarEntity car2)
		{
			int ret = String.Compare(car1["Engine_ExhaustForFloat"], car2["Engine_ExhaustForFloat"]);
			if (ret == 0)
			{
				double year1 = ConvertHelper.GetDouble(car1.CarYear);
				double year2 = ConvertHelper.GetDouble(car2.CarYear);
				if (year1 > year2)
					ret = -1;
				else if (year1 < year2)
					ret = 1;
				else
				{
					int mt1 = car1["UnderPan_TransmissionType"].IndexOf("手动");
					int mt2 = car2["UnderPan_TransmissionType"].IndexOf("手动");
					if (mt1 > -1 && mt2 == -1)
						ret = -1;
					else if (mt2 > -1 && mt1 == -1)
						ret = 1;
					else
						ret = String.Compare(car1["UnderPan_TransmissionType"], car2["UnderPan_TransmissionType"]);

					if (ret == 0)
					{
						double price1 = ConvertHelper.GetDouble(car1.ReferPrice);
						double price2 = ConvertHelper.GetDouble(car2.ReferPrice);
						if (price1 > price2)
							ret = 1;
						else if (price2 > price1)
							ret = -1;
					}
				}
			}
			return ret;
		}


		/// <summary>
		/// 根据获取数据字段名称取数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public override string GetDataValue(EntityType eType, string dataName)
		{
			string dataValue = String.Empty;
			if (eType != EntityType.Serial)
				dataValue = this.Brand.GetDataValue(eType, dataName);
			else
			{
				dataValue = base.GetDataValue(dataName);
				if (dataValue.Length == 0)
				{
					switch (dataName)
					{
						case "ShowName":
							dataValue = m_showName;
							break;
						case "Virtues":
							dataValue = m_virtues;
							break;
						case "Defect":
							dataValue = m_defect;
							break;
						case "SaleState":
							dataValue = m_saleState;
							break;
						case "RepairPolicy":
							dataValue = m_repairPolicy;
							break;
						case "ReferPrice":
							dataValue = m_referPrice;
							break;
						case "Price":
							dataValue = m_price;
							break;
						case "Exhaust":
							dataValue = this.Exhaust;
							break;
						case "Transmission":
							dataValue = this.Transmission;
							break;
						case "BaaUrl":
							dataValue = this.BaaUrl;
							break;
						case "PicCount":
							dataValue = this.PicCount.ToString();
							break;
						case "AskCount":
							dataValue = this.AskCount.ToString();
							break;
					}
				}
			}
			return dataValue;
		}
	}
}
