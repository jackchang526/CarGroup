using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
    /// <summary>
    /// 停售车型列表（按年款）
    /// </summary>
    public class GetNoSaleSerailListByYear : PageBase
    {
        #region Param
        private int csID = 0;
        private string type = string.Empty;
        private int yearPara = 0;
        private int maxPv = 0;
        protected string serialSpell = string.Empty;
        protected string serialNoSaleDisplacement = string.Empty;//停销车款 排量
        protected string serialNoSaleDisplacementalt = string.Empty;//停销车款 排量
        private Dictionary<int, string> dicUcarPrice;	// 二手车报价区间
        private StringBuilder sb = new StringBuilder();
        private string callback = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(60);
            if (!this.IsPostBack)
            {
                // 检查参数
                this.CheckParam();
                if (csID > 0 && csID < 10000)
                {
                    var serialInfo = new Car_SerialBll().GetSerialInfoCard(csID);
                    var serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
                    serialSpell = serialEntity.AllSpell;
                    List<CarInfoForSerialSummaryEntity> resultTemp = GetCarByCsID();
                    MakeCarListJson(resultTemp);
                    if (string.IsNullOrEmpty(callback))
                    {
                        Response.Write(string.Format("{0}", sb.ToString()));
                    }
                    else
                    {
                        Response.Write(string.Format("{1}({0})", sb.ToString(), callback));
                    }
                }
            }
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        private void CheckParam()
        {
            if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
            {
                string strCsID = this.Request.QueryString["csID"].ToString().Trim();
                if (int.TryParse(strCsID, out csID))
                {}
            }
            if (this.Request.QueryString["year"] != null && this.Request.QueryString["year"].ToString() != "")
            {
                string strYear = this.Request.QueryString["year"].ToString().Trim();
                if (int.TryParse(strYear, out yearPara))
                {}
            }
            callback = this.Request.QueryString["callback"];
        }

        /// <summary>
        /// 取车型列表
        /// </summary>
        /// <returns></returns>
        private List<CarInfoForSerialSummaryEntity> GetCarByCsID()
        {
            List<CarInfoForSerialSummaryEntity> result = new List<CarInfoForSerialSummaryEntity>();
            Car_BasicBll _carBLL = new Car_BasicBll();
            List<BitAuto.CarChannel.Model.CarInfoForSerialSummaryEntity> carinfoList = _carBLL.GetCarInfoForSerialSummaryBySerialId(csID);
            
            List<string> saleYearList = new List<string>();
            List<string> noSaleYearList = new List<string>();
            foreach (CarInfoForSerialSummaryEntity carInfo in carinfoList)
            {
                if (carInfo.CarPV > maxPv)
                    maxPv = carInfo.CarPV;
                if (carInfo.CarYear.Length > 0)
                {
                    string yearType = carInfo.CarYear + "款";

                    if (carInfo.SaleState == "停销")
                    {
                        if (!noSaleYearList.Contains(yearType))
                            noSaleYearList.Add(yearType);
                    }
                    else
                    {
                        if (!saleYearList.Contains(yearType))
                            saleYearList.Add(yearType);
                    }
                }
            }
            //排除包含在售年款
			//foreach (string year in saleYearList)
			//{
			//	if (noSaleYearList.Contains(year))
			//	{
			//		noSaleYearList.Remove(year);
			//	}
			//}
            List<CarInfoForSerialSummaryEntity> carinfoNoSaleList = carinfoList
                .FindAll(p => p.SaleState == "停销");

            #region add by songcl 2014-11-21 停销车款 排量输出
            var maxYear = carinfoNoSaleList.Max(s => s.CarYear);
            var tempList = carinfoNoSaleList.Where(s => s.CarYear == maxYear).ToList();

            List<string> noSaleExhaustList = tempList.Where(p => p.Engine_Exhaust.EndsWith("L"))
                                                              .Select(
                                                                  p =>
																  p.Engine_InhaleType.IndexOf("增压") >= 0
                                                                      ? p.Engine_Exhaust.Replace("L", "T")
                                                                      : p.Engine_Exhaust)
                                                              .GroupBy(p => p)
                                                              .Select(group => group.Key).ToList();

            List<string> fuelTypeListForNoSeal = tempList.Where(p => p.Oil_FuelType != "")
                                                                  .GroupBy(p => p.Oil_FuelType)
                                                                  .Select(g => g.Key).ToList();

            if (noSaleExhaustList.Count > 0)
            {
                noSaleExhaustList.Sort(NodeCompare.ExhaustCompareNew);
                if (noSaleExhaustList.Count > 3)
                {
                    serialNoSaleDisplacement = string.Concat(noSaleExhaustList[0], " ", noSaleExhaustList[1]
                                                             , "..."
                                                             , noSaleExhaustList[noSaleExhaustList.Count - 1],
                                                             fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
                }
                else
                    serialNoSaleDisplacement = string.Join(" ", noSaleExhaustList.ToArray()) +
                                               (fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
                serialNoSaleDisplacementalt = string.Join(" ", noSaleExhaustList.ToArray()) +
                                              (fuelTypeListForNoSeal.Contains("纯电") ? " 电动" : "");
            }

            #endregion
            carinfoNoSaleList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
            noSaleYearList.Sort(NodeCompare.CompareStringDesc);
            if (noSaleYearList.Count > 0 && noSaleYearList.Contains(yearPara + "款")) 
            {
                result = carinfoNoSaleList.FindAll(s => s.CarYear == yearPara.ToString());
            }
            return result;
        }

        /// <summary>
        /// 生成JSON
        /// </summary>
        private void MakeCarListJson(List<CarInfoForSerialSummaryEntity> carList)
        {
            if (carList.Count > 0)
            {
                Car_BasicBll _carBLL = new Car_BasicBll();
                // 如果是停销年款 取二手车报价
                dicUcarPrice = new Car_BasicBll().GetAllUcarPrice();
                var listGroupNew = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                var listGroupOff = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();
                var listGroupImport = new List<IGrouping<object, CarInfoForSerialSummaryEntity>>();

                var importGroup = carList.GroupBy(p => new { p.IsImport }, p => p);
                foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in importGroup)
                {
                    var key = CommonFunction.Cast(info.Key, new { IsImport = 0 });
                    if (key.IsImport == 1)
                    {
                        listGroupImport.Add(info);

                    }
                    else
                    {
						var querySale = info.ToList().GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower, p.Electric_Peakpower }, p => p);
                        foreach (IGrouping<object, CarInfoForSerialSummaryEntity> subInfo in querySale)
                        {
                            var isStopState = subInfo.FirstOrDefault(p => p.ProduceState != "停产");
                            if (isStopState != null)
                                listGroupNew.Add(subInfo);
                            else
                                listGroupOff.Add(subInfo);
                        }
                    }
                }
                listGroupNew.AddRange(listGroupOff);
                listGroupNew.AddRange(listGroupImport);

                int groupIndex = 0;
                //sb.Append("{");
                sb.Append("[");
                foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in listGroupNew)
                {
                    string strMaxPowerAndInhaleType = string.Empty;
                    string maxPower = string.Empty;
                    string inhaleType = string.Empty;
                    string exhaust = string.Empty;

                    if (groupIndex == listGroupNew.Count - 1 && listGroupImport.Any())
                    {
                        exhaust = "平行进口车";
                    }
                    else
                    {
						var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0, Electric_Peakpower = 0 });

                        maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
                        inhaleType = key.Engine_InhaleType;
                        exhaust = key.Engine_Exhaust;
                        if (inhaleType == "增压")
                        {
                            inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType) ? inhaleType : key.Engine_AddPressType;
                        }
						if (key.Electric_Peakpower > 0)
						{
							maxPower = string.Format("发动机：{0}，发电机：{1}", maxPower, key.Electric_Peakpower + "kW");
						}
                        strMaxPowerAndInhaleType = string.Format("{0}{1}", maxPower, " " + inhaleType);
                    }
                    if (groupIndex > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append("{");
                    sb.AppendFormat("\"Engine_Exhaust\":\"{0}\",", exhaust);
                    sb.AppendFormat("\"MaxPower\":\"{0}\",", maxPower);
                    sb.AppendFormat("\"InhaleType\":\"{0}\",", inhaleType);
                    sb.Append("\"carList\":[");
                    groupIndex++;
                    List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();//分组后的集合
                    int itemIndex = 0;
                    foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
                    {
                        #region 获取数据
                        string yearType = entity.CarYear.Trim();
                        if (yearType.Length > 0)
                            yearType += "款";
                        else
                            yearType = "未知年款";
                        string stopPrd = "";
                        if (entity.ProduceState == "停产")
                            stopPrd = "停产";
                        Dictionary<int, string> dictCarParams = _carBLL.GetCarAllParamByCarID(entity.CarID);

                        // 节能补贴 Sep.2.2010
                        string strEnergySubsidy = "";
                        bool hasEnergySubsidy = false;
                        //补贴功能临时去掉 modified by chengl Oct.24.2013
                        //modified by sk 2015-01-30 只有第七 、 八批次 补贴
                        if ((dictCarParams.ContainsKey(963) && (dictCarParams[963] == "第10批")) && dictCarParams.ContainsKey(853) && dictCarParams[853] == "3000元")
                        {
                            hasEnergySubsidy = true;
                            strEnergySubsidy = "可获得3000元节能补贴";
                        }
                        //============2012-04-09 减税============================
                        string strTravelTax = "";
                        bool isTravelTax = false;
                        //if (dictCarParams.ContainsKey(895))
                        //{
                            
                        //    if (dictCarParams[895] == "减半")
                        //    {
                        //        strTravelTax = "减征50%车船使用税";
                        //        isTravelTax = true;
                        //    }
                        //    else if (dictCarParams[895] == "免征")
                        //    {
                        //        strTravelTax = "免征车船使用税";
                        //        isTravelTax = true;
                        //    }
                        //    else
                        //    {
                        //        strTravelTax = "";
                        //    }
                        //}

                        //平行进口车标签
                        string parallelImport = "";
                        if (dictCarParams.ContainsKey(382) && dictCarParams[382] == "平行进口")
                        {
                            parallelImport = "平行进口";
                        }
                        //计算百分比
                        int percent = (int)Math.Round((double)entity.CarPV / maxPv * 100.0, MidpointRounding.AwayFromZero);
                        // 档位个数
                        string forwardGearNum = (dictCarParams.ContainsKey(724) && dictCarParams[724] != "无级" && dictCarParams[724] != "待查") ? dictCarParams[724] + "挡" : "";
                        // 停销年款用二手车报价
                        string uCarPrice = string.Empty;
                        if (dicUcarPrice != null && dicUcarPrice.Count > 0 && dicUcarPrice.ContainsKey(entity.CarID))
                        {
                            uCarPrice = dicUcarPrice[entity.CarID];
                        }
                        #endregion
                        if (itemIndex > 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append("{");
                        sb.AppendFormat("\"CarID\":\"{0}\",", entity.CarID);
                        sb.AppendFormat("\"Spell\":\"{0}\",", serialSpell);
                        sb.AppendFormat("\"YearType\":\"{0}\",", yearType);
                        sb.AppendFormat("\"Name\":\"{0}\",", entity.CarName);
                        sb.AppendFormat("\"isTravelTax\":\"{0}\",", isTravelTax);
                        sb.AppendFormat("\"strTravelTax\":\"{0}\",", strTravelTax);
                        sb.AppendFormat("\"hasEnergySubsidy\":\"{0}\",", hasEnergySubsidy);
                        sb.AppendFormat("\"strEnergySubsidy\":\"{0}\",", strEnergySubsidy);
                        sb.AppendFormat("\"ParallelImport\":\"{0}\",", parallelImport);
                        sb.AppendFormat("\"StopPrd\":\"{0}\",", stopPrd);
                        sb.AppendFormat("\"Percent\":\"{0}\",", percent);
                        sb.AppendFormat("\"ForwardGearNum\":\"{0}\",", forwardGearNum);
                        sb.AppendFormat("\"TransmissionType\":\"{0}\",", entity.TransmissionType);
                        sb.AppendFormat("\"ReferPrice\":\"{0}\",", string.IsNullOrEmpty(entity.ReferPrice) ? "暂无" : entity.ReferPrice + "万");
                        sb.AppendFormat("\"UCarPrice\":\"{0}\",", uCarPrice);
                        sb.AppendFormat("\"Oil_FuelType\":\"{0}\"", entity.Oil_FuelType);
                        sb.Append("}");
                        itemIndex++;
                    }
                    sb.Append("]");
                    sb.Append("}");
                }
                sb.Append("]");
              //sb.Append("}");
            }
        }
    }
}