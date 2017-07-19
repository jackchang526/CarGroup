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
using System.Web.Mvc;
using System.IO;
using System.Xml.Serialization;

namespace WirelessWeb.handlers
{
	/// <summary>
	///     GetMoreCarList 的摘要说明
	/// </summary>
	public class GetMoreCarList : IHttpHandler
	{
		private Car_BasicBll _carBLL;
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
			BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
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

		private void getSubItemInit(HttpContext context)
		{
			response.ContentType = "application/json";  //"application/json"; 
			GetParamter();  //获取公共参数
			if (_serialId < 1 || _serialEntity == null)
			{
				response.Write("{\"SubItem\":[]}");
			}
			else
			{
				InitSerialInfo();//初始化数据集
				List<CarInfoForSerialSummaryEntity> serialCarList = new List<CarInfoForSerialSummaryEntity>();// _serialCarList.Select(p => p).ToList();
				serialCarList = this.Clone<List<CarInfoForSerialSummaryEntity>>(_serialCarList);
				serialCarList.ForEach(p =>
				{
					if (p.Engine_AddPressType == "涡轮增压")
					{
						p.Engine_Exhaust = p.Engine_Exhaust.Replace("L", "T");
					} 
				});
				_optionType = request.QueryString["option"];  //stopyear,level,bodyform
				RenderSubItemByOption(_serialYear, _optionType, serialCarList);
			}
		}
		private void getOptionDataListInit(HttpContext context)
		{
			response.ContentType = "text/plain";// "application/json"; 
			GetParamter();  //获取公共参数
			if (_serialId < 1 || _serialEntity == null)
			{
				response.Write("");
			}
			else
			{
				InitSerialInfo();//初始化数据集
				RenderDataListByOption(_serialYear, _stopYearOption, _levelOption, _bodyFormOption);
			}
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
			if (_serialEntity != null)
			{
				_serialShowName = _serialEntity.ShowName;
			}
		}

		private void InitSerialInfo()
		{
			_carBLL = new Car_BasicBll();
			_serialCarList = _carBLL.GetCarInfoForSerialSummaryBySerialId(_serialId);
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

						querySale = currentYearCarList.GroupBy(
							p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
							p => p);
						//querySale = currentYearCarList.Skip(count).Take(pageSize).GroupBy(
						//	p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower },
						//	p => p);
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
					if (_levelOption.IndexOf("T") > -1)
					{
						currentYearCarList = currentYearCarList.FindAll(p => p.Engine_Exhaust == _levelOption.Replace("T", "L") && p.Engine_AddPressType == "涡轮增压");
					}
					else
					{
						currentYearCarList = currentYearCarList.FindAll(p => p.Engine_Exhaust == _levelOption && p.Engine_AddPressType != "涡轮增压");
					}
				}
				if (!string.IsNullOrEmpty(_bodyFormOption) && _bodyFormOption != "不限")
				{
					currentYearCarList = currentYearCarList.FindAll(p => p.TransmissionType == _bodyFormOption);
				}
				if (!string.IsNullOrEmpty(_stopYearOption) && _stopYearOption != "不限")
				{
					currentYearCarList = currentYearCarList.FindAll(p => p.CarYear == _stopYearOption);
				}
				querySale = currentYearCarList.GroupBy(
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
			var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();

			var importGroup = currentYearCarList.GroupBy(p => new { p.IsImport }, p => p); //取第一页
			foreach (var info in importGroup)
			{
				var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
				if (key.IsImport == 1)
				{
					listGroupImport.Add(info);
				}
				else
				{
					var qs = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
					foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in qs)
					{
						CarInfoForSerialSummaryEntity isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "停产");
						if (isStopState != null)
							listGroupNew.Add(subInfo);
						else
							listGroupOff.Add(subInfo);
					}
				}
			}
			listGroupNew.AddRange(listGroupOff);
			listGroupNew.AddRange(listGroupImport);

			if (listGroupNew.Count > 0)
			{
				int tempCount = 0;
				int groupIndex = 0;
				//int tempGroupCount = 0;
				foreach (var info in listGroupNew)
				{

					#region 基础信息准备

					

					#endregion
					//tempGroupCount += info.Count();
					//if ((tempGroupCount > count && tempGroupCount <= count + pageSize) || (info.Count() > count && info.Count() <= count + pageSize))
					//{
					//	stringBuilder.Append("<div class='tt-small'>");
					//	stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L", "升"),
					//		(string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
					//			? ""
					//			: "/", strMaxPowerAndInhaleType);
					//	stringBuilder.Append("</div>");

					//	stringBuilder.Append("<div class='car-card b-shadow'>");
					//	stringBuilder.Append("<ul>");
					//}
					//else
					//{
					//	tempCount = tempGroupCount;
					//	continue;
					//}
					#region

					List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList(); //分组后的集合

					bool isAdded = false;
					foreach (CarInfoForSerialSummaryEntity carInfo in carGroupList)
					{
						//if (num > 9)
						//    break;
						tempCount++;
						if (tempCount <= count || tempCount > count + pageSize) continue;
						if (!isAdded)
						{
							isAdded = true;
							var key = new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 } ;
							string strMaxPowerAndInhaleType = string.Empty;
							if (groupIndex == listGroupNew.Count - 1 && listGroupImport.Any())
							{
								strMaxPowerAndInhaleType = "平行进口车";
							}
							else
							{
								key = CommonFunction.Cast(info.Key,
								new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });
							
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
							}

							stringBuilder.Append("<div class='tt-small'>");
							stringBuilder.AppendFormat("<span>{0}{1}{2}</span>", key.Engine_Exhaust.Replace("L", "升").Replace("T","升"),
								(string.IsNullOrEmpty(key.Engine_Exhaust) || string.IsNullOrEmpty(strMaxPowerAndInhaleType))
									? ""
									: "/", strMaxPowerAndInhaleType);
							stringBuilder.Append("</div>");

							stringBuilder.Append("<div class='car-card b-shadow'>");
							stringBuilder.Append("<ul>");
						}
						string carFullName = "";
						carFullName = carInfo.CarName;
						if (carInfo.CarName.StartsWith(_serialShowName))
						{
							carFullName = carInfo.CarName.Substring(_serialShowName.Length);
						}
						//if (year == "全部在售" || year == "未上市")
						//{
						//	/////////////////////////////
						//}
						carFullName = carInfo.CarYear + "款 " + carFullName;
						string stopPrd = "";
						if (carInfo.ProduceState == "停产")
							stopPrd = "<em>停产" + (_serialEntity.SaleState == "停销" ? "停售" : "在售") + "</em>";//此为原版本：stopPrd = "<em class='tingchan'>停产在售</em>";
						if (carInfo.ProduceState == "停产" && carInfo.SaleState == "停销")
							stopPrd = "<em>停售</em>";
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
						//string parallelImport = "";
						//if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
						//{
						//	parallelImport = "<em>平行进口</em>";
						//}

						stringBuilder.Append("<li>");

						stringBuilder.AppendFormat(
							"<a  id='carlist_" + carInfo.CarID + "' class='car-info' href='{0}' data-channelid=\"27.23.915\">",
							 "/" + _serialEntity.AllSpell + "/m" + carInfo.CarID + "/");

						stringBuilder.AppendFormat("<h2>{0}</h2>", carFullName);
						
						stringBuilder.AppendFormat("<dl><dt>{0}</dt></dl>", carMinPrice);
						stringBuilder.Append("<div class=\"car-info-bottom\">");//第二行开始
						stringBuilder.AppendFormat("<span>{0}</span>", forwardGearNum + carInfo.TransmissionType);
						//add date :2016-2-3  添加热度
						int percent = 0;
						if (maxPv > 0)
						{ percent = (int)Math.Round((double)carInfo.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero); }
						//add date :2016-2-25 减税 购置税
						string strTravelTax = string.Empty;
						double dEx = 0.0;
						Double.TryParse(carInfo.Engine_Exhaust.ToUpper().Replace("L", ""), out dEx);
						if (carInfo.SaleState == "在销")
						{
							if (dictCarParams.ContainsKey(987) && (dictCarParams[987] == "第1批" || dictCarParams[987] == "第2批" || dictCarParams[987] == "第3批" || dictCarParams[987] == "第4批" || dictCarParams[987] == "第5批" || dictCarParams[987] == "第6批") && dictCarParams.ContainsKey(986))
							{
								if (dictCarParams[986] == "免征")
								{
									strTravelTax = "<em>免税</em>";
								}
								else
								{
									strTravelTax = "<em>减税</em>";
								}
							}
							else if (dEx > 0 && dEx <= 1.6)
							{
								strTravelTax = "<em>减税</em>";
							}
						}
						stringBuilder.AppendFormat("<div class=\"gzd-box\"><div class=\"tit-box\">热度</div><span class=\"gz-sty\"><i data-pv=\"{0}\" style=\"width:{0}%\"></i></span></div>", percent);
						stringBuilder.AppendFormat("{0}{1}", strTravelTax, stopPrd);
						stringBuilder.AppendFormat("<b>指导价:{0}</b>", carInfo.ReferPrice.Trim().Length == 0 ? "暂无" : carInfo.ReferPrice.Trim() + "万");
						stringBuilder.Append("</div>");//第二行结束

						stringBuilder.Append("</a>");
						bool maiBtnFlag = false;
						//if (year != "unlisted" && year != "nosalelist" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
						//{
						//	maiBtnFlag = true;
						//}
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
							"<li><a id=\"car-compare-{0}\" href=\"javascript:;\" class=\"btnDuibi\" data-action=\"car\" data-id=\"{0}\" data-name=\"{1} {2}\" data-channelid=\"27.23.910\">加对比</a></li>",
							carInfo.CarID, _serialShowName, carInfo.CarName);
						stringBuilder.AppendFormat(
							"<li><a id = \"car_filter_id_{0}_{1}\" href='/gouchejisuanqi/?carID={0}' data-channelid=\"27.23.911\">计算器</a></li>",
							carInfo.CarID, "");
						if (year != "unlisted" && year != "nosalelist" && carInfo.SaleState != "待销" && carInfo.SaleState != "停销")
						{
							//stringBuilder.AppendFormat(
							//	   "<li><a data-car=\"{0}\" href='javascript:void(0)' class=\"btn-mmm\"  data-action=\"mmm\" data-channelid=\"27.23.1321\">买买买</a></li>",
							//	   carInfo.CarID);
						}
						if (carInfo.SaleState != "停销")
						{
							//add by gux 20170425
							string wtQuery = new int[] { 4123, 4881, 2608, 1574, 2573, 3987, 2032, 1905, 4847, 1798 }.Contains(_serialId) ? "&WT.mc_id=nbclx" : string.Empty;
							stringBuilder.AppendFormat(
							"<li class=\"btn-org\"><a id =\"car_filterzuidi_id_{0}_{1}\" href=\"http://price.m.yiche.com/zuidijia/nc{0}/?t=yichemobiletest2&leads_source=20018" + wtQuery + "\" data-channelid=\"27.23.912\">询底价</a></li>",
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
					if (isAdded)
					{
						stringBuilder.Append("</ul></div>");
					}
					groupIndex++;
				}
				if (action == "getOptionDataList" && totalPageNum > pageNum)
				{
					stringBuilder.AppendFormat(string.Format("<a id='btnLoadNext' page='{0}' totalpage='{1}' class='btn-more btn-add-more' href='javascript:void(0);'><i>加载更多</i></a>", pageNum + 1, totalPageNum));
				}
			}

			result = stringBuilder.ToString();
			response.Write(stringBuilder.ToString());
		}
		
		private void RenderSubItemByOption(string year, string _optionType, List<CarInfoForSerialSummaryEntity> _serialCarList)
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
			{ curOptionTypes.Sort(NodeCompare.ExhaustCompareNew); }
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

		/// <summary>
		/// 深度克隆
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="RealObject"></param>
		/// <returns></returns>
		public T Clone<T>(T RealObject)
		{
			using (Stream stream = new MemoryStream())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, RealObject);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)serializer.Deserialize(stream);
			}
		}
	}
}