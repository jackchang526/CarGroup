using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Cache;
using System;
using System.Linq.Expressions;

namespace WirelessWeb.handlers
{
	/// <summary>
	///     GetMoreCarList 的摘要说明
	/// </summary>
	public class GetMoreCarList : WirelessPageBase, IHttpHandler
	{
		private Car_BasicBll _carBLL;
		private string _carList;
		private List<CarInfoForSerialSummaryEntity> _serialCarList;
		private SerialEntity _serialEntity;
		private int _serialId;
		private string _serialShowName;
		private string _serialYear;
		private int pageNum;
		private int pageSize = 7;
		private HttpRequest request;
		private HttpResponse response;
		protected bool isElectrombile = false;
		private string _optionType = string.Empty;
		private string _subOptionVal = string.Empty;
		private string action = string.Empty;
		//筛选项变量
		private string _stopYearOption = string.Empty;
		private string _levelOption = string.Empty;
		private string _bodyFormOption = string.Empty;
        protected int maxPv = 0;

		/// <summary>
		/// 车系为电动车的续航里程区间
		/// </summary>
		protected string mileageRange = string.Empty;
		public void ProcessRequest(HttpContext context)
		{
			response = context.Response;
			request = context.Request;
			action = ConvertHelper.GetString(request.QueryString["action"]);
			switch (action)
			{
				case "getSubItem": getSubItemInit(context); break;
				case "getOptionDataList": getOptionDataListInit(context); break;
				default:
					//DefaultActionInit(context);
					break;
			}

		}
		private void DefaultActionInit(HttpContext context)
		{
			context.Response.ContentType = "text/html";
			GetParamter();
			InitSerialInfo();
			if (pageNum <= 1) return;
			string result = string.Empty;
			CarList(_serialYear, _stopYearOption, _levelOption, _bodyFormOption, out result);
		}
		private void getSubItemInit(HttpContext context)
		{
			base.SetPageCache(30);
			response.ContentType = "application/json";  //"application/json"; 
			GetParamter();  //获取公共参数
			InitSerialInfo();//初始化数据集
			_optionType = request.QueryString["option"];  //stopyear,level,bodyform
			RenderSubItemByOption(_serialYear, _optionType);
		}
		private void getOptionDataListInit(HttpContext context)
		{
			base.SetPageCache(30);
			response.ContentType = "text/html";  //"application/json"; 
			GetParamter();  //获取公共参数
			InitSerialInfo();//初始化数据集
			RenderDataListByOption(_serialYear, _stopYearOption, _levelOption, _bodyFormOption);
		}

		public bool IsReusable
		{
			get { return false; }
		}

		private void GetParamter()
		{
			_serialId = ConvertHelper.GetInteger(request.QueryString["ID"]);
			_serialYear = request.QueryString["year"];
			_stopYearOption = request.QueryString["stopYearOption"];
			_levelOption = request.QueryString["levelOption"];
			_bodyFormOption = request.QueryString["bodyFormOption"];
			pageNum = string.IsNullOrEmpty(request.QueryString["page"]) ? 1 : ConvertHelper.GetInteger(request.QueryString["page"]);
			_serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, _serialId);
			_serialShowName = _serialEntity.ShowName;
		}

		private void InitSerialInfo()
		{
			_carBLL = new Car_BasicBll();
			_serialCarList = _carBLL.GetCarInfoForSerialSummaryBySerialId(_serialId);
			//_serialCarList = new List<CarInfoForSerialSummaryEntity>();
			//if (_serialEntity.SaleState == "停销")
			//{
			//    // 停销子品牌取 停销子品牌最新年款的车型(高总逻辑 Oct.11.2011)
			//    _serialCarList = GetAllCarInfoForNoSaleSerialNewSummaryByCsID(_serialId);
			//}
			//else
			//{
			//    // 非停销子品牌取 子品牌的非停销所有年款车型
			//    _serialCarList = base.GetAllCarInfoForSerialNewSummaryByCsID(_serialId, false);
			//}
			_serialCarList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
		}
		/// <summary>
		///     车型列表
		/// </summary>
		private void CarList(string year, string _stopYearOption, string _levelOption, string _bodyFormOption, out string result)
		{
			var htmlCode = new StringBuilder();
			var stringBuilder = new StringBuilder();
			int minMileage = 0;
			int maxMileage = 0;
			int count = (pageNum - 1) * pageSize;
			List<CarInfoForSerialSummaryEntity> currentYearCarList;
			IEnumerable<IGrouping<object, CarInfoForSerialSummaryEntity>> querySale;

            maxPv = _serialCarList.Max(m => m.CarPV);
			#region 数据筛选
			if (string.IsNullOrEmpty(_stopYearOption) && string.IsNullOrEmpty(_levelOption) && string.IsNullOrEmpty(_bodyFormOption))
			{
				switch (year)
				{
					case "all":
						currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销");

						querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
							p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
							p => p);
						break;
					case "unlisted":
						currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销");

						querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
							p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
							p => p);
						break;
					case "nosalelist":
						//停售tab栏默认只显示当前所有停售车款里的最新年份
						var stopCarYears = new List<string>();
						stopCarYears = _serialCarList.Where(p => p.CarYear.Length > 0 && p.SaleState == "停销")
								.Select(p => p.CarYear)
								.Distinct()
								.ToList();
						string nearestYear = string.Empty;//所有停售车款里的最近年份
						if (stopCarYears.Count > 0)
						{
							stopCarYears.Sort(NodeCompare.CompareStringDesc);
							nearestYear = stopCarYears[0];
						}
						currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == nearestYear);

						querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
							p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
							p => p);
						break;
					default:
						if (_serialEntity.SaleState == "停销")
						{
							currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销" && p.CarYear == year);
							//取车系为停销状态的车款列表
						}
						else
						{
							currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销" && p.CarYear == year);
							//取车系为在销状态的车款列表
						}
						querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
							p =>
								new
								{
									p.Engine_Exhaust,
									p.Engine_InhaleType,
									p.Engine_AddPressType,
									p.Engine_MaxPower,
									p.Electric_Peakpower
								},
							p => p);
						break;
				}
			}
			else
			{
				switch (year)
				{
					case "all": currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "在销"); break;
					case "unlisted": currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "待销"); break;
					case "nosalelist": currentYearCarList = _serialCarList.FindAll(p => p.SaleState == "停销"); break;
					default: currentYearCarList = _serialCarList.FindAll(p => p.SaleState == _serialEntity.SaleState && p.CarYear == year); break;
				}


				if (!string.IsNullOrEmpty(_levelOption) && _levelOption != "不限")
				{
					currentYearCarList = currentYearCarList.FindAll(p => p.Engine_Exhaust == _levelOption);
				}
				if (!string.IsNullOrEmpty(_bodyFormOption) && _bodyFormOption != "不限")
				{
					currentYearCarList = currentYearCarList.FindAll(p => p.TransmissionType == _bodyFormOption);
				}
				if (!string.IsNullOrEmpty(_stopYearOption) && _stopYearOption != "不限")
				{
					currentYearCarList = currentYearCarList.FindAll(p => p.CarYear == _stopYearOption);
				}
				querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
							p =>
								new
								{
									p.Engine_Exhaust,
									p.Engine_InhaleType,
									p.Engine_AddPressType,
									p.Engine_MaxPower,
									p.Electric_Peakpower
								},
							p => p);  //取所有显示
			}
			#endregion

			int totalPageNum = currentYearCarList.Count / pageSize + (currentYearCarList.Count % pageSize == 0 ? 0 : 1);//总页数

			//start add by sk 2014-09-03 候姐 整组 停产 把整组移到最底 且保持前 排序规则
			var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
			var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
			foreach (var info in querySale)
			{
				CarInfoForSerialSummaryEntity isStopState = info.FirstOrDefault(p => p.ProduceState != "停产");
				if (isStopState != null)
					listGroupNew.Add(info);
				else
					listGroupOff.Add(info);
			}
			listGroupNew.AddRange(listGroupOff);


			if (listGroupNew.Count > 0)
			{
				foreach (var info in listGroupNew)
				{
					#region 基础信息准备

					var key = CommonFunction.Cast(info.Key,
						new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
					string strMaxPowerAndInhaleType = string.Empty;
					string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
					string inhaleType = key.Engine_InhaleType;
					if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
					{
						if (inhaleType == "增压")
						{
							inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType)
								? inhaleType
								: key.Engine_AddPressType;
						}
						if (key.Electric_Peakpower > 0)
						{
							maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
						}
						strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
					}

					#endregion
                
					stringBuilder.Append("<div class='tt-small'>");
					stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L", "升"),
						(string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
							? ""
							: "/", strMaxPowerAndInhaleType);
					stringBuilder.Append("</div>");

                    stringBuilder.Append("<div class='car-card b-shadow'>");
					stringBuilder.Append("<ul>");

					#region

					List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList(); //分组后的集合
                   

					foreach (CarInfoForSerialSummaryEntity carInfo in carGroupList)
					{
						//if (num > 9)
						//    break;

						string carFullName = "";
						carFullName = carInfo.CarName;
						if (carInfo.CarName.StartsWith(_serialShowName))
						{
							carFullName = carInfo.CarName.Substring(_serialShowName.Length);
						}
						if (year == "全部在售" || year == "未上市")
						{
							/////////////////////////////
						}
						carFullName = carInfo.CarYear + "款 " + carFullName;
						string stopPrd = "";
						if (carInfo.ProduceState == "停产")
							stopPrd = "<em class='tingchan'>停产" + (_serialEntity.SaleState == "停销" ? "停售" : "在售") + "</em>";//此为原版本：stopPrd = "<em class='tingchan'>停产在售</em>";
						if (carInfo.ProduceState == "停产" && carInfo.SaleState == "停销")
							stopPrd = "<em class='tingchan'>停售</em>";
						string carMinPrice;
						string carPriceRange = carInfo.CarPriceRange.Trim();
						if (carInfo.SaleState == "待销")//顾晓 确认的逻辑 （待销的车款没有价格，全部显示未上市） 2015-07-09
						{
							carMinPrice = "未上市";
						}
						else if (carInfo.CarPriceRange.Trim().Length == 0)
						{
							carMinPrice = "暂无报价";
						}
						else
						{
							if (carPriceRange.IndexOf('-') != -1)
								carMinPrice = carPriceRange.Substring(0, carPriceRange.IndexOf('-')); //+ "万"
							else
								carMinPrice = carPriceRange;
						}

						Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(carInfo.CarID);

						#region 纯电动车续航里程

						if (isElectrombile)
						{
							//纯电最高续航里程
							if (dictCarParams.ContainsKey(883))
							{
								var mileage = ConvertHelper.GetInteger(dictCarParams[883]);
								if (minMileage == 0 && mileage > 0)
									minMileage = mileage;
								if (mileage < minMileage)
									minMileage = mileage;
								if (mileage > maxMileage)
									maxMileage = mileage;
							}
							if (maxMileage > 0)
							{
								mileageRange = minMileage == maxMileage
									? string.Format("{0}公里", minMileage)
									: string.Format("{0}-{1}公里", minMileage, maxMileage);
							}
						}

						#endregion


						// 档位个数
						string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" &&
												 dictCarParams[724] != "待查")
							? dictCarParams[724] + "挡"
							: "";

						//平行进口车标签
						string parallelImport = "";
						if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
						{
							parallelImport = "<em>平行进口</em>";
						}

						stringBuilder.Append("<li>");

						stringBuilder.AppendFormat(
							"<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}' data-channelid=\"27.23.915\">",
							 "/" + _serialEntity.AllSpell + "/m" + carInfo.CarID + "/");

						stringBuilder.AppendFormat("<h2>{0}</h2>", carFullName);
						stringBuilder.AppendFormat("<span>{0}</span>{1}{2}", forwardGearNum + carInfo.TransmissionType,
							parallelImport, stopPrd);
						stringBuilder.AppendFormat("<dl><dt>{0}</dt><dd>指导价：{1}</dd></dl>", carMinPrice,
							carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
                        //add date :2016-2-3  添加热度
                        int percent = 0;
                        if (maxPv > 0)
                        { percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero); }
                        //add date :2016-2-25 减税 购置税
                        string strTravelTax = "<div class=\"tap-box\"></div>";
                        double dEx = 0.0;
                        Double.TryParse(carInfo.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
                        if (carInfo.SaleState == "在销")
                        {
                            if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
                            {
                                if (dictCarParams[986] == "免征")
                                {
                                    strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">免税</em></div>";
                                }
                                else
                                {
                                    strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">减税</em></div>";
                                } 
                            }
                            else if (dEx > 0 && dEx <= 1.6)
                            {
                                strTravelTax = " <div class=\"tap-box\"><em class=\"bt\">减税</em></div>";
                            }
                        }
                        stringBuilder.AppendFormat("<div class=\"gzd-box\" style=\"\"><div class=\"tit-box\">关注度</div><span class=\"gz-sty\"><i data-pv=\"{0}\" style=\"width:{0}%\"></i></span>{1}</div>", percent, strTravelTax);
                        
                        stringBuilder.Append("</a>");
                        bool maiBtnFlag=false;
                        if (year != "unlisted" && year != "nosalelist" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
                        {
                            maiBtnFlag = true;
                        }
                        string ulStyle = "car-btn";
                        if (!maiBtnFlag)
                        {
                            ulStyle = "car-btn car-btn-three";
                        }
                        stringBuilder.Append("<ul class='" + ulStyle + "'>");
						//stringBuilder.AppendFormat(
						//	"<li><a href=\"http://car.m.yiche.com/chexingduibi/?carIDs={0}\">加入对比</a></li>",
						//	carInfo.CarID);
						stringBuilder.AppendFormat(
							"<li><a id=\"car-compare-{0}\" href=\"javascript:;\" class=\"btnDuibi\" data-action=\"car\" data-id=\"{0}\" data-name=\"{1} {2}\" data-channelid=\"27.23.910\">加入对比</a></li>",
							carInfo.CarID, _serialShowName, carInfo.CarName);
						stringBuilder.AppendFormat(
							"<li><a id = \"car_filter_id_{0}_{1}\" href='/gouchejisuanqi/?carID={0}' data-channelid=\"27.23.911\">购车计算</a></li>",
							carInfo.CarID, "");
                        if (year != "unlisted" && year != "nosalelist" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
                        {
                            stringBuilder.AppendFormat(
                                   "<li><a data-car=\"{0}\" href='javascript:void(0)' class=\"btn-mmm\"  data-action=\"mmm\" data-channelid=\"27.23.1321\">买买买</a></li>",
                                   carInfo.CarID);
                        }
						if (carInfo.SaleState != "停销")
						{
							stringBuilder.AppendFormat(
							"<li class=\"btn-org\"><a id =\"car_filterzuidi_id_{0}_{1}\" href=\"http://price.m.yiche.com/zuidijia/nc{0}/?t=yichemobiletest2&leads_source=20018\" data-channelid=\"27.23.912\">询底价</a></li>",
							carInfo.CarID, "");
						}
						else
						{
							stringBuilder.AppendFormat("<li class='btn-org'><a href='http://m.taoche.com/all/?carid={0}&WT.mc_id=yichezswap' data-channelid=\"27.23.913\">买二手车</a></li>", carInfo.CarID);
						}
						stringBuilder.Append("</ul>");

						stringBuilder.Append("</li>");
						//num++;
					}

					#endregion
					stringBuilder.Append("</ul>");
				}
				if (action == "getOptionDataList" && totalPageNum > pageNum)
				{
					stringBuilder.AppendFormat(string.Format("<a id='btnLoadNext' page='{0}' totalpage='{1}' class='btn-more btn-add-more' href='javascript:void(0);'><i>加载更多</i></a>", pageNum + 1, totalPageNum));
				}
			}

			result = stringBuilder.ToString();
			response.Write(stringBuilder.ToString());
		}

		private void RenderSubItemByOption(string year, string _optionType)
		{
			string cacheKey = "Car_Wireless_GetMoreCarList_GetSubItem_" + _serialId + "_" + year + "_" + _optionType;
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				response.Write((string)obj);
				return;
			}

			List<string> curOptionTypes = new List<string>();
			string distinctField = string.Empty;
			switch (_optionType)
			{
				case "stopyear": distinctField = "CarYear"; break;
				case "level": distinctField = "Engine_Exhaust"; break;
				case "bodyform": distinctField = "TransmissionType"; break;
				default: break;
			}
			ParameterExpression parameter = Expression.Parameter(typeof(CarInfoForSerialSummaryEntity), "p");
			var bin = Expression.PropertyOrField(parameter, distinctField);
			var lambda = Expression.Lambda<Func<CarInfoForSerialSummaryEntity, string>>(bin, parameter);

			switch (year)
			{
				case "all": curOptionTypes = _serialCarList.Where(p => p.SaleState == "在销").Select(
				  lambda.Compile()
				).Distinct().ToList(); break;
				case "unlisted": curOptionTypes = _serialCarList.Where(p => p.SaleState == "待销").Select(
					lambda.Compile()
					).Distinct().ToList(); break;
				case "nosalelist": curOptionTypes = _serialCarList.Where(p => p.SaleState == "停销").Select(
					lambda.Compile()
					).Distinct().ToList(); break;
				default: curOptionTypes = _serialCarList.Where(p => p.CarYear.Length > 0 && p.CarYear == year && p.SaleState == _serialEntity.SaleState).Select(
					lambda.Compile()
					).Distinct().ToList(); break;
			}
			if (_optionType == "level")
			{ curOptionTypes.Sort(NodeCompare.ExhaustCompare); }
			else
			{ curOptionTypes.Sort(NodeCompare.CompareStringDesc); }

			curOptionTypes = curOptionTypes.Select(p =>
			{
				if (_optionType == "stopyear")
					return "\"" + p + "款\"";
				else
					return "\"" + p + "\"";
			}).ToList();
			string json = "{\"SubItem\":[" + string.Join(",", curOptionTypes.ToArray()) + "]}";
			CacheManager.InsertCache(cacheKey, json, WebConfig.CachedDuration);
			response.Write(json);
		}

		private void RenderDataListByOption(string year, string _stopYearOption, string _levelOption, string _bodyFormOption)
		{
			string cacheKey = "mCarList_" + _serialId + "_" + year + "_" + _stopYearOption + "_" + _levelOption + "_" + _bodyFormOption + "_p" + pageNum;
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				response.Write((string)obj);
				return;
			}
			string result = string.Empty;
			CarList(year, _stopYearOption, _levelOption, _bodyFormOption, out result);
			CacheManager.InsertCache(cacheKey, result, WebConfig.CachedDuration);
		}
	}
}