using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.BLL.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using Newtonsoft.Json;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetCarInfo 的摘要说明
    /// add getallcarinfo for ask liurw Jun.9.2013 增加全部车型对应子品牌ID输出
    /// add getallcaroil for ask wangzt	Jun.17.2013  增加取车型油耗数据 迁移car域名接口 http://car.bitauto.com/Interface/ask/oilInterface.aspx
    /// add getlistbyyear for wangzt Jun.18.2013 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/CarInfo/CarListGroupByYear.aspx?dept=bitautocheguanjia
    /// add getcarinfobyid for wangzt Jun.18.2013 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/CarInfo/GetCarInfo.ashx?dept=car&id={0}
    /// add carcolor for wangzt Jun.25.2013 迁移car域名接口 http://car.bitauto.com/car/interfaceforbitauto/CarInfo/CarInfoList.aspx?dept=carcolor&carid=16356&colorType=0
    /// add carcolorbyyear for liangguanzhi Dec.26.2014  根据年款获取车型颜色
    /// add askrobot getpingcecarparam for liurw 取评测车型参数
    /// add getcarparam for 杨立锋 Aug.19.2013 根据车型ID取车型全部参数 迁移 http://carser.bitauto.com/xmldata/carinfo/c_103541.xml 
    /// add getcarforyichehuiv3 for 易车惠V3
    /// add getcarpv for wangzt Apr.24.2014
    /// add getcarparamtopgroupbylevel for CMS 根据参数ID 按级别取top车款，支持top、排序参数 isAllSale:是否取全部车款包括停销(默认只取在销) isOnePerCS:是否每个子品牌只取1个车款(默认取多个车款)  May.13.2014
    /// add getcarlistbycsid for 车贷迁移子品牌的车款列表
    /// add getelectricbycarids for zhuyx 按车款ID列表取电动车参数
    /// add getcarbaseparam for 李瑞文(汽车金融事业部) 取车型基本数据
    /// add getgroupcarlistbyserialid for app 5.0 取车款列表 逻辑与综述页一致分组 by sk 2014.09.16
    /// add getcarcomparelist for jingcg 迁移老接口 http://car.bitauto.com/interfaceforbitauto/ForCarCompareList.aspx?carID=11991&top=1
    /// add getcarcalcinfo for 易车公司-汽车金融事业部-产品设计部 李瑞文 取车款的排量、指导价、座位数 用于计算工具
    /// </summary>
    public class GetCarInfo : PageBase, IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        //BitAuto.CarChannel.BLL.Car_BasicBll basicBll = new BitAuto.CarChannel.BLL.Car_BasicBll();
        private string paramsIds = string.Empty;
        private bool nocache = false;
        private Car_BasicBll carBll = new Car_BasicBll();
        private StringBuilder sb = new StringBuilder();

        public void ProcessRequest(HttpContext context)
        {
            response = context.Response;
            request = context.Request;
            string op = request.QueryString["op"];
            if (!string.IsNullOrEmpty(op))
            { op = op.Trim().ToLower(); }
            string type = request.QueryString["type"];
            if (!string.IsNullOrEmpty(type))
            { type = type.Trim().ToLower(); }
            if (op == "koubei")
            {
                paramsIds = GetXmlData(@"//Channel[@name='koubei']/Car/CarParamList");
            }
            // 答疑机器人
            if (op == "askrobot")
            {
                paramsIds = GetXmlData(@"//Channel[@name='askrobot']/Car/CarParamList");
            }
            // 杨立锋 根据车型ID取车型全部参数
            if (op == "mobile")
            {
                // 取全部参数
            }
            //nocache = ConvertHelper.GetBoolean(request["nocache"]);
            bool.TryParse(request["nocache"], out nocache);
            switch (type)
            {
                case "master": RenderMasterData(); break;
                case "brand": RenderBrandData(); break;
                case "serial": RenderSerialData(); break;
                case "car": RenderCarData(); break;
                case "getallcar": RenderAllCarData(); break;
                case "getallcarinfobycsid": RenderCarInfoByCsID(); break;
                case "getallcaroil": PageHelper.SetPageCache(60); RenderAllCarOil(); break;
                case "getcarlistbyyear": PageHelper.SetPageCache(60); RenderAllCarByYear(); break;
                case "getcarinfobyid": PageHelper.SetPageCache(60); RenderCarInfoByID(); break;
                case "carcolor": RenderCarColor(); break;
                case "carcolorjson": RenderCarColorJson(); break;
                case "carcolorbyyear": RenderCarColorByYear(); break;
                case "getpingcecarparam": PageHelper.SetPageCache(60); RenderPingCeCarParam(); break;
                case "getcarparam": RenderCarParamByID(true); break;
                case "getcarbaseparam": PageHelper.SetPageCache(60); RenderCarParamByID(false); break;
                case "getcarforyichehuiv3": PageHelper.SetPageCache(60); RenderCarForYiCheHui(); break;
                case "getcarpv": PageHelper.SetPageCache(60); RenderCarPV(); break;
                case "getcarparamtopgroupbylevel": PageHelper.SetPageCache(60); RenderCarParamByParamidGroupbyLevel(); break;
                case "getcarlistbycsid": PageHelper.SetPageCache(60); RenderCarListByCsID(); break;
                case "getelectricbycarids": PageHelper.SetPageCache(60); RenderElectricByCarids(); break;
                case "getgroupcarlistbyserialid": if (!nocache) { PageHelper.SetPageCache(30); } RenderGroupCarListBySerialId(); break;
                case "getcarcomparelist": PageHelper.SetPageCache(60); RenderGetCarCompareList(); break;
                case "getcarcalcinfo": PageHelper.SetPageCache(60); RenderGetCarCalcInfo(); break;
                case "getcartaxinfo": PageHelper.SetPageCache(60); RenderGetCarParamInfo(); break;
                case "getcarparamforpc": PageHelper.SetPageCache(60); RenderGetCarParamInfoForEValuation(); break;
                default: CommonFunction.EchoXml(response, "<!-- 缺少参数 -->", ""); break;
            }
        }

        private void RenderGetCarParamInfoForEValuation()
        {
            response.ContentType = "application/x-javascript";
            string result = "{}";
            string callback = request.QueryString["callback"];
            int carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
            if (carId <= 0)
            {
                response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
                return;
            }
            string sql = @"WITH    result
                              AS(SELECT   CarId, [835], [845], [680], [691], [714], [715], [833],
                                            [898], [818], [811], [812], [813], [702], [800], [703],
                                            [624], [518], [576], [577], [578], [579], [782], [788],
                                             [398],[665],[470],[471],[663],[495]
                                   FROM(SELECT    CarId, ParamId, Pvalue
                                              FROM      dbo.CarDataBase
                                              WHERE     CarId = @CarId
                                            ) AS T1 PIVOT(MAX(Pvalue) FOR ParamId IN( [835],
                                                                                  [845], [680],
                                                                                  [691], [714],
                                                                                  [715], [833],
                                                                                  [898], [818],
                                                                                  [811], [812],
                                                                                  [813], [702],
                                                                                  [800], [703],
                                                                                  [624], [518],
                                                                                  [576], [577],
                                                                                  [578], [579],
                                                                                  [782], [788],
                                                                                   [398],[665],[470],[471],[663],[495])) AS T2
                                 )
                        SELECT car.car_ReferPrice,Car_Name,result.*
                       FROM    dbo.Car_relation car
                                LEFT JOIN result ON result.CarId = car.Car_Id
                        WHERE car.IsState = 0 AND car.Car_Id = @CarId";
            SqlParameter[] _params = {
                                         new SqlParameter("@CarId",SqlDbType.Int),
                                     };
            _params[0].Value = carId;
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _params);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                var obj = new
                {
                    CarId = dr["CarId"],
                    CarName = CommonFunction.GetUnicodeByString(ConvertHelper.GetString(dr["Car_Name"])),
                    ReferPrice = dr["car_ReferPrice"],
                    Safe_KneeGasBag = dr["835"],
                    InStat_BeltBag = dr["845"],
                    Safe_BsadGasbag = dr["680"],
                    Safe_FsadGasbag = dr["691"],
                    UnderPan_TyrePressureWatcher = dr["714"],
                    UnderPan_ZeroPressureDrive = dr["715"],
                    InStat_BSeatTuneType = dr["833"],
                    InStat_blindspotdetection = dr["898"],
                    InStat_ActiveSafetySystems = dr["818"],
                    InStat_AutoPark = dr["811"],
                    InStat_UphillAuxiliary = dr["812"],
                    InStat_HDC = dr["813"],
                    UnderPan_RRadarAfter = dr["702"],
                    UnderPan_PRadarBefore = dr["800"],
                    UnderPan_RImage = dr["703"],
                    OutStat_ReMirrorHot = dr["624"],
                    InStat_Hud = dr["518"],
                    Oil_FuelCapacity = dr["576"],
                    Oil_FuelTab = dr["577"],
                    Oil_FuelType = dr["578"],
                    Oil_SboxSpace = dr["579"],
                    Perf_ZongHeYouHao = dr["782"],
                    Perf_MeasuredFuel = dr["788"],
                    Car_RepairPolicy = dr["398"],
                    Perf_SeatNum = dr["665"],
                    InStat_AirCSystem = dr["470"],
                    InStat_AirCType = dr["471"],
                    Perf_MaxSpeed = dr["663"],
                    InStat_ChildSeatFix = dr["495"]
                };

                result = JsonConvert.SerializeObject(obj);
            }
            response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
        }

        /// <summary>
        /// 车款计算工具的数据 指导价，所属厂商是否国产，排量，座位数
        /// </summary>
        private void RenderGetCarCalcInfo()
        {
            #region 取参数
            int carid = 0;
            if (request.QueryString["carID"] != null && request.QueryString["carID"].ToString() != "")
            {
                string carIDStr = request.QueryString["carID"].ToString();
                if (int.TryParse(carIDStr, out carid))
                { }
            }
            #endregion
            string sql = @"SELECT  car.Car_Id, car.Car_Name, car.car_ReferPrice, cs.cs_id,
                                    cb.cb_Country AS Cp_Country
                            FROM    dbo.Car_Basic car
                                    LEFT JOIN car_serial cs ON car.Cs_Id = cs.cs_Id
                                    LEFT JOIN Car_Brand cb ON cs.cb_Id = cb.cb_Id
                            WHERE   car.IsState = 1
                                    AND cs.IsState = 1 {0} 
                            ORDER BY car.Car_Id";
            DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, string.Format(sql, (carid > 0 ? " and car.Car_Id=" + carid : " ")));
            if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
            {
                // 排量 升
                Dictionary<int, string> dic785 = carBll.GetCarParamExDic(785);
                // 乘员人数（含司机）
                Dictionary<int, string> dic665 = carBll.GetCarParamExDic(665);
                sb.AppendLine("<!-- 节点说明:Car车款节点 ID:车款ID RP:车款指导价 Country:国产|进口 EF:排量(升) SN:乘员人数(含司机)  -->");
                foreach (DataRow dr in dsCar.Tables[0].Rows)
                {
                    sb.AppendLine(string.Format("<Car ID=\"{0}\" RP=\"{1}\" Country=\"{2}\" EF=\"{3}\" SN=\"{4}\" />"
                        , dr["Car_Id"].ToString()
                        , dr["car_ReferPrice"].ToString()
                        , (dr["Cp_Country"].ToString() == "中国" ? "国产" : "进口")
                        , (dic785.ContainsKey(int.Parse(dr["Car_Id"].ToString())) ? dic785[int.Parse(dr["Car_Id"].ToString())] : "")
                        , (dic665.ContainsKey(int.Parse(dr["Car_Id"].ToString())) ? dic665[int.Parse(dr["Car_Id"].ToString())] : "")
                        ));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        private void RenderGetCarCompareList()
        {
            #region 取参数
            int carid = 0;
            if (request.QueryString["carID"] != null && request.QueryString["carID"].ToString() != "")
            {
                string carIDStr = request.QueryString["carID"].ToString();
                if (int.TryParse(carIDStr, out carid))
                { }
            }
            int topCount = 5;
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string top = request.QueryString["top"].ToString();
                if (int.TryParse(top, out topCount))
                {
                    if (topCount < 0 || topCount > 1000)
                    {
                        topCount = 5;
                    }
                }
            }
            //bool isContainSameSerial = false;
            //if (request.QueryString["isContainSameSerial"] != null && request.QueryString["isContainSameSerial"].ToString() != "")
            //{
            //	if (request.QueryString["isContainSameSerial"].ToString().Trim() == "1")
            //	{
            //		isContainSameSerial = true;
            //	}
            //}
            #endregion

            if (carid > 0 && topCount > 0)
            {
                CarEntity carE = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carid);
                DataSet dsCompare = new Car_BasicBll().GetCarCompareListByCarID(carid);
                if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
                {
                    int loop = 0;
                    for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
                    {
                        if (loop >= topCount)
                        { break; }
                        if (carE.SerialId.ToString() == dsCompare.Tables[0].Rows[i]["cs_id"].ToString())
                        {
                            continue;
                        }
                        sb.Append("<Item CarID=\"" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString() + "\" ");
                        sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(dsCompare.Tables[0].Rows[i]["Car_Name"].ToString()) + "\" ");
                        sb.Append(" CsID=\"" + dsCompare.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
                        sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(dsCompare.Tables[0].Rows[i]["cs_showname"].ToString()) + "\" />");
                        loop++;
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 获取车款列表 逻辑 综述页一致 
        /// </summary>
        private void RenderGroupCarListBySerialId()
        {
            int serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
            string saleState = request.QueryString["salestate"];
            if (serialId <= 0)
            {
                CommonFunction.EchoXml(response, "<!-- 缺少参数 -->", "CarList");
                return;
            }
            StringBuilder sb = new StringBuilder();
            try
            {
                //add by sk 增加 销售状态 筛选 2016.02.24
                Predicate<CarInfoForSerialSummaryEntity> predicate = (p) => (p.SaleState != "待查");
                switch (saleState)
                {
                    case "onsale": predicate = (p) => (p.SaleState == "在销" || p.SaleState == "待销"); break;
                    default: break;
                }
                var carList = carBll.GetCarInfoForSerialSummaryBySerialId(serialId, nocache).FindAll(predicate);
                carList.Sort(NodeCompare.CompareCarByExhaustAndPowerAndInhaleType);
                var querySale = carList.GroupBy(p => new { p.Engine_Exhaust, p.Engine_InhaleType, p.Engine_AddPressType, p.Engine_MaxPower }, p => p);
                foreach (IGrouping<object, CarInfoForSerialSummaryEntity> info in querySale)
                {
                    var key = CommonFunction.Cast(info.Key, new { Engine_Exhaust = "", Engine_InhaleType = "", Engine_AddPressType = "", Engine_MaxPower = 0 });
                    string strMaxPowerAndInhaleType = string.Empty;
                    string maxPower = key.Engine_MaxPower == 9999 ? "" : key.Engine_MaxPower + "kW";
                    string inhaleType = key.Engine_InhaleType;
                    if (!string.IsNullOrEmpty(maxPower) || !string.IsNullOrEmpty(inhaleType))
                    {
                        if (inhaleType == "增压")
                        {
                            inhaleType = string.IsNullOrEmpty(key.Engine_AddPressType) ? inhaleType : key.Engine_AddPressType;
                        }
                        strMaxPowerAndInhaleType = string.Format("/{0}{1}", maxPower, " " + inhaleType);
                    }

                    sb.AppendFormat("<CarGroup Name=\"{0}{1}\">", key.Engine_Exhaust, strMaxPowerAndInhaleType);
                    //sb.AppendFormat("<CarGroup Name=\"{0}{1}{2}\">", key.Engine_Exhaust, string.IsNullOrEmpty(inhaleType) ? "" : (" " + inhaleType), string.IsNullOrEmpty(maxPower) ? "" : ("/" + maxPower));
                    List<CarInfoForSerialSummaryEntity> carGroupList = info.ToList<CarInfoForSerialSummaryEntity>();
                    foreach (CarInfoForSerialSummaryEntity entity in carGroupList)
                    {
                        string forwardGearNum = (!string.IsNullOrEmpty(entity.UnderPan_ForwardGearNum) && entity.UnderPan_ForwardGearNum != "无级" && entity.UnderPan_ForwardGearNum != "待查") ? entity.UnderPan_ForwardGearNum + "挡" : "";
                        string minPrice = entity.CarPriceRange;
                        if (entity.CarPriceRange.IndexOf("-") != -1)
                            minPrice = entity.CarPriceRange.Substring(0, entity.CarPriceRange.IndexOf('-'));

                        Dictionary<int, string> dictCarParams = carBll.GetCarAllParamByCarID(entity.CarID);

                        sb.AppendFormat("<Car Id=\"{0}\" Name=\"{1}\" Year=\"{2}\" MinPrice=\"{3}\" Trans=\"{4}\" ReferPrice=\"{5}\" ImportType=\"{7}\" SaleState=\"{6}\" ProduceState=\"{9}\" CarPV=\"{8}\" />", entity.CarID,
                            System.Security.SecurityElement.Escape(entity.CarName),
                            entity.CarYear,
                            System.Security.SecurityElement.Escape(minPrice),
                            forwardGearNum + System.Security.SecurityElement.Escape(entity.TransmissionType)
                            , entity.ReferPrice
                            , entity.SaleState,
                            (dictCarParams.ContainsKey(382)) ? dictCarParams[382] : "",
                            entity.CarPV,
                            entity.ProduceState);
                    }
                    sb.Append("</CarGroup>");
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            CommonFunction.EchoXml(response, sb.ToString(), "CarList");
        }

        /// <summary>
        /// 取车型ID的电动车
        /// </summary>
        private void RenderElectricByCarids()
        {
            // 883 	纯电最高续航里程
            // 876	电池容量
            // 879	普通充电时间
            // 878  快速充电时间
            // 954	充电方式
            List<int> listCarIDs = new List<int>();
            #region 请求的车款ID
            if (request.QueryString["carids"] != null && request.QueryString["carids"].ToString() != "")
            {
                string strCarIds = request.QueryString["carids"].ToString();
                string[] arrCarIDs = strCarIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrCarIDs.Length > 0)
                {
                    foreach (string strid in arrCarIDs)
                    {
                        int carid = 0;
                        if (int.TryParse(strid, out carid))
                        {
                            if (carid > 0 && !listCarIDs.Contains(carid) && listCarIDs.Count < 20)
                            {
                                listCarIDs.Add(carid);
                            }
                        }
                    }
                }
            }
            #endregion
            if (listCarIDs.Count > 0)
            {
                string sqlGetCarInfo = @"select car.car_id,car.car_name,car.Car_YearType
								,cs.cs_id,cs.csname,cs.csshowname,cs.allspell
								,cdb1.pvalue as EM,cdb2.pvalue as EB,cdb3.pvalue as EN
								,cdb4.pvalue as EC,cdb5.pvalue as EF
								from car_relation car
								left join car_serial cs 
								on car.cs_id=cs.cs_id
								left join cardatabase cdb1 on cdb1.carid=car.car_id and cdb1.paramid=883
								left join cardatabase cdb2 on cdb2.carid=car.car_id and cdb2.paramid=876
								left join cardatabase cdb3 on cdb3.carid=car.car_id and cdb3.paramid=879
								left join cardatabase cdb4 on cdb4.carid=car.car_id and cdb4.paramid=954
								left join cardatabase cdb5 on cdb5.carid=car.car_id and cdb5.paramid=878
								where car.car_id in ({0})
								and car.isstate=0";
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                    , CommandType.Text, string.Format(sqlGetCarInfo, string.Join(",", listCarIDs.ToArray())));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // 白底
                    Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
                    sb.AppendLine("<!-- CarID:车款ID CarName:车款名 Year:年款 CsID:子品牌ID CsName:子品牌名 CsShowName:子品牌显示名 AllSpell:子品牌全拼地址使用 Price:报价区间 Img:白底图 EM:续航里程 EB:电池容量 EN:充电时间(普通) EF:充电时间(快速) EC:充电方式 -->");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int carid = int.Parse(dr["car_id"].ToString());
                        string carName = dr["car_name"].ToString().Trim();
                        string year = dr["Car_YearType"].ToString().Trim();
                        int csid = int.Parse(dr["cs_id"].ToString());
                        string csName = dr["csname"].ToString().Trim();
                        string csShowName = dr["csshowname"].ToString().Trim();
                        string allSpell = dr["allspell"].ToString().Trim();
                        string EM = dr["EM"].ToString().Trim();
                        string EB = dr["EB"].ToString().Trim();
                        string EN = dr["EN"].ToString().Trim();
                        string EF = dr["EF"].ToString().Trim();
                        string EC = dr["EC"].ToString().Trim();
                        string pr = base.GetCarPriceRangeByID(carid);
                        string img = dicPicWhite.ContainsKey(csid) ? dicPicWhite[csid] : WebConfig.DefaultCarPic;
                        sb.AppendLine(string.Format("<Item CarID=\"{0}\" CarName=\"{1}\" Year=\"{2}\" CsID=\"{3}\" CsName=\"{4}\" AllSpell=\"{5}\" Price=\"{6}\" Img=\"{7}\" EM=\"{8}\" EB=\"{9}\" EN=\"{10}\" EC=\"{11}\" EF=\"{12}\" CsShowName=\"{13}\" />"
                            , carid
                            , System.Security.SecurityElement.Escape(carName)
                            , year
                            , csid
                            , System.Security.SecurityElement.Escape(csName)
                            , System.Security.SecurityElement.Escape(allSpell)
                            , System.Security.SecurityElement.Escape(pr)
                            , System.Security.SecurityElement.Escape(img)
                            , System.Security.SecurityElement.Escape(EM)
                            , System.Security.SecurityElement.Escape(EB)
                            , System.Security.SecurityElement.Escape(EN)
                            , System.Security.SecurityElement.Escape(EC)
                            , System.Security.SecurityElement.Escape(EF)
                            , System.Security.SecurityElement.Escape(csShowName)));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 根据子品牌ID取其下车款(在销、车款PV)
        /// </summary>
        private void RenderCarListByCsID()
        {
            sb.AppendLine("<!-- ID:车款ID Name:车款名 Year:年款 ReferPrice:指导价 EE:排量 TT:变速器 PV:热度 -->");
            // 是否取全部车款(包括停销) 默认不包括 只取在销的
            bool isAllSale = false;
            if (request.QueryString["isAllSale"] != null && request.QueryString["isAllSale"].ToString() == "1")
            {
                isAllSale = true;
            }
            int csid = 0;
            if (request.QueryString["csid"] != null && request.QueryString["csid"].ToString() != "")
            {
                string csidStr = request.QueryString["csid"].ToString().Trim();
                if (int.TryParse(csidStr, out csid))
                { }
            }
            if (csid > 0)
            {
                string sqlGet = string.Format(@"SELECT  car.car_id, car.car_name, car.car_ReferPrice, car.Car_YearType,
														car.Car_ProduceState, car.Car_SaleState, cs.cs_id, cei.Engine_Exhaust,
														cei.UnderPan_TransmissionType, cs.cs_carLevel, ccp.PVSum AS Pv_SumNum
												FROM    dbo.Car_Basic car WITH ( NOLOCK )
														LEFT JOIN dbo.Car_Extend_Item cei WITH ( NOLOCK ) ON car.car_id = cei.car_id
														LEFT JOIN Car_serial cs WITH ( NOLOCK ) ON car.cs_id = cs.cs_id
														LEFT JOIN Car_Basic_PV ccp WITH ( NOLOCK ) ON car.Car_Id = ccp.CarId
												WHERE   car.isState = 1
														AND cs.isState = 1
														AND car.cs_id = @SerialId {0} 
												ORDER BY car.Car_YearType DESC, cei.Engine_Exhaust", (isAllSale ? "" : " AND car.Car_SaleState='在销' "));

                SqlParameter[] _params = {
                                         new SqlParameter("@SerialId",SqlDbType.Int),
                                     };
                _params[0].Value = csid;
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlGet, _params);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.AppendLine(string.Format("<Car ID=\"{0}\" Name=\"{1}\" Year=\"{2}\" ReferPrice=\"{3}\" EE=\"{4}\" TT=\"{5}\" PV=\"{6}\"/>"
                            , dr["car_id"].ToString()
                            , System.Security.SecurityElement.Escape(dr["car_name"].ToString().Trim())
                            , dr["Car_YearType"].ToString()
                            , dr["car_ReferPrice"].ToString()
                            , System.Security.SecurityElement.Escape(dr["Engine_Exhaust"].ToString())
                            , System.Security.SecurityElement.Escape(dr["UnderPan_TransmissionType"].ToString())
                            , dr["Pv_SumNum"].ToString()
                            ));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 根据参数ID、排序方式 按级别取在销车款的top车型信息
        /// </summary>
        private void RenderCarParamByParamidGroupbyLevel()
        {
            #region 取参数
            int paramid = 0;
            if (request.QueryString["paramid"] != null && request.QueryString["paramid"].ToString() != "")
            {
                string paramidStr = request.QueryString["paramid"].ToString();
                if (int.TryParse(paramidStr, out paramid))
                { }
            }
            int top = 10;// 每个级别取前几个车款，默认取10个
            if (request.QueryString["top"] != null && request.QueryString["top"].ToString() != "")
            {
                string topStr = request.QueryString["top"].ToString();
                if (int.TryParse(topStr, out top))
                { }
            }
            if (top < 0 || top > 10000)
            { top = 10; }

            bool order = false;// 默认升序
            if (request.QueryString["order"] != null && request.QueryString["order"].ToString() == "1")
            {
                order = true;
            }
            string unit = "";// 单位 目前只配置3个实测的单位
            switch (paramid)
            {
                case 786: unit = "秒"; break;
                case 787: unit = "米"; break;
                case 788: unit = "升"; break;
                default: break;
            }
            // 是否取全部车款(包括停销) 默认不包括 只取在销的
            bool isAllSale = false;
            if (request.QueryString["isAllSale"] != null && request.QueryString["isAllSale"].ToString() == "1")
            {
                isAllSale = true;
            }
            // 是否每个子品牌只取1个车款 默认每个子品牌可以取多个
            bool isOnePerCS = false;
            if (request.QueryString["isOnePerCS"] != null && request.QueryString["isOnePerCS"].ToString() == "1")
            {
                isOnePerCS = true;
            }
            #endregion

            if (paramid > 0)
            {
                string sqlGetData = @"select cs.cs_id,cs.csname,cs.csshowname,cs.allspell,cl.classvalue as csLevel
					,car.car_id,car.car_name,car.Car_YearType,t1.pvalue
					,row_number() over(partition by cl.classvalue 
					order by t1.pvalue {1})as levelRank
					from (
					select 
					carid
					,convert(float ,(case when pvalue='' then null else pvalue end))
					as pvalue
					from cardatabase
					where paramid={0}
					and pvalue<>'' and ISNUMERIC(pvalue)=1
					) t1 
					left join car_relation car 
					on t1.carid=car.car_id
					left join car_serial cs
					on car.cs_id=cs.cs_id
					left join class cl on cs.carlevel=cl.classid
					where car.isState=0 and cs.isstate=0 {2}";
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                    WebConfig.AutoStorageConnectionString, CommandType.Text
                    , string.Format(sqlGetData
                    , paramid
                    , order ? "desc" : ""
                    , isAllSale ? "" : "and car.car_SaleState=95"));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // 每个级别的子品牌ID集合，用于排重 限制数量
                    Dictionary<string, List<int>> dicLevelCount = new Dictionary<string, List<int>>();
                    sb.AppendLine("<!-- 根据参数ID取各级别top车款 /Root/Level:级别节点 Name:级别名 /Root/Level/Item:车款节点 CsID:子品牌ID、CsName:子品牌名、AllSpell:子品牌全拼地址使用、CarID:车款ID、CarName:车款名、CarYear:车款年款(没填为空)、Value:参数值 -->");
                    string lastLevelName = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int csid = int.Parse(dr["cs_id"].ToString());
                        string csName = dr["csname"].ToString().Trim();
                        string csShowName = dr["csshowname"].ToString().Trim();
                        string allspell = dr["allspell"].ToString().Trim();
                        string csLevel = dr["csLevel"].ToString().Trim();
                        int carID = int.Parse(dr["car_id"].ToString());
                        string carName = dr["car_name"].ToString().Trim();
                        string carYear = dr["Car_YearType"].ToString().Trim();
                        int levelRank = int.Parse(dr["levelRank"].ToString());
                        string value = dr["pvalue"].ToString().Trim();
                        if (lastLevelName != csLevel)
                        {
                            if (lastLevelName != "")
                            { sb.AppendLine("</Level>"); }
                            sb.AppendLine(string.Format("<Level Name=\"{0}\">", csLevel));
                            lastLevelName = csLevel;
                        }

                        if (isOnePerCS)
                        {
                            // 如果每个子品牌取1个车款  如果已有此子品牌 或 此级别子品牌数量到达上限 则continue
                            if (dicLevelCount != null && dicLevelCount.ContainsKey(csLevel)
                            && (dicLevelCount[csLevel].Contains(csid) || dicLevelCount[csLevel].Count >= top))
                            { continue; }
                            if (dicLevelCount.ContainsKey(csLevel))
                            {
                                dicLevelCount[csLevel].Add(csid);
                            }
                            else
                            {
                                List<int> list = new List<int>();
                                list.Add(csid);
                                dicLevelCount.Add(csLevel, list);
                            }
                        }
                        else
                        {
                            // 每个子品牌可以取多个
                            if (levelRank > top)
                            { continue; }
                        }
                        sb.AppendLine(string.Format("<Item CsID=\"{0}\" CsName=\"{1}\" AllSpell=\"{2}\" CarID=\"{3}\" CarName=\"{4}\" CarYear=\"{5}\" Value=\"{6}\" CsShowName=\"{7}\" />"
                            , csid, System.Security.SecurityElement.Escape(csName),
                            System.Security.SecurityElement.Escape(allspell), carID,
                            System.Security.SecurityElement.Escape(carName), carYear,
                            System.Security.SecurityElement.Escape(value) + unit
                            , System.Security.SecurityElement.Escape(csShowName)));
                    }
                    sb.AppendLine("</Level>");
                }
                CommonFunction.EchoXml(response, sb.ToString(), "Root");
            }
            else
            {
                CommonFunction.EchoXml(response, "<!-- 缺少参数ID -->", "");
            }
        }

        /// <summary>
        /// 车型前天PV
        /// </summary>
        private void RenderCarPV()
        {
            int serialId = ConvertHelper.GetInteger(request.QueryString["csid"]);
            DataSet dsCarPV = carBll.GetCarPVData(serialId);
            if (dsCarPV != null && dsCarPV.Tables.Count > 0 && dsCarPV.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:车型ID、PV:型前天PV数(数据为PV大于0数据) -->");
                foreach (DataRow dr in dsCarPV.Tables[0].Rows)
                {
                    int carid = int.Parse(dr["car_id"].ToString());
                    int pv = 0;
                    if (int.TryParse(dr["Pv_SumNum"].ToString(), out pv))
                    { }
                    if (pv > 0)
                    {
                        sb.AppendLine(string.Format("<Car ID=\"{0}\" PV=\"{1}\" />", carid, pv));
                    }
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 易车惠3期数据接口
        /// </summary>
        private void RenderCarForYiCheHui()
        {
            string sqlGetCarInfo = @"select car.car_id,car.car_name,car.car_ReferPrice
					,car.Car_YearType,car.cs_id
					from car_relation car
					left join car_serial cs on car.cs_id=cs.cs_id
					where car.isState=0 and cs.isState=0
					and car.car_SaleState=95
					order by car.cs_id,car.car_id";
            DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
            WebConfig.AutoStorageConnectionString, CommandType.Text, sqlGetCarInfo);
            if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine("<!-- ID:车型ID、Name:车型名、CsID:子品牌ID、Year:年款(没有为空值)、Price:指导价(没有为空值 单位万元) -->");
                foreach (DataRow dr in dsCar.Tables[0].Rows)
                {
                    sb.AppendLine(string.Format("<Car ID=\"{0}\" Name=\"{1}\" CsID=\"{2}\" Year=\"{3}\" Price=\"{4}\" />"
                        , dr["car_id"].ToString()
                        , System.Security.SecurityElement.Escape(dr["car_name"].ToString().Trim())
                        , dr["cs_id"].ToString(), dr["Car_YearType"].ToString()
                        , dr["car_ReferPrice"].ToString()));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 杨立锋 根据车型ID取车型全部参数 迁移 http://carser.bitauto.com/xmldata/carinfo/c_103541.xml
        /// </summary>
        private void RenderCarParamByID(bool isNeedParam)
        {
            int carid = 0;
            if (request.QueryString["carid"] != null && request.QueryString["carid"].ToString() != "")
            {
                string caridStr = request.QueryString["carid"].ToString();
                if (int.TryParse(caridStr, out carid))
                { }
            }

            CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carid);

            if (carid > 0 && ce != null && ce.Id > 0)
            {
                #region 车型基本信息
                DataSet dsCarBasic = carBll.GetCarDetailById(carid);
                if (dsCarBasic != null && dsCarBasic.Tables.Count > 0 && dsCarBasic.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsCarBasic.Tables[0].Rows[0];
                    //降价优惠 add by sk 2014.10.21
                    Dictionary<int, string> dicJiangJia = new CarNewsBll().GetAllCarJiangJia();
                    string carJiangJiaPrice = dicJiangJia.ContainsKey(carid) ? dicJiangJia[carid] : "";

                    // 车型基本信息
                    sb.AppendLine("<CarBasic>");
                    sb.AppendLine(string.Format("<Car_Id>{0}</Car_Id>", dr["Car_Id"].ToString()));
                    sb.AppendLine(string.Format("<Car_ProduceState>{0}</Car_ProduceState>", dr["Car_ProduceState"].ToString()));
                    sb.AppendLine(string.Format("<Car_SaleState>{0}</Car_SaleState>", dr["Car_SaleState"].ToString()));
                    sb.AppendLine(string.Format("<Cs_Id>{0}</Cs_Id>", dr["Cs_Id"].ToString()));
                    sb.AppendLine(string.Format("<Car_Name>{0}</Car_Name>", System.Security.SecurityElement.Escape(dr["Car_Name"].ToString())));
                    sb.AppendLine(string.Format("<SpellFirst>{0}</SpellFirst>", dr["SpellFirst"].ToString()));
                    sb.AppendLine(string.Format("<car_ReferPrice>{0}</car_ReferPrice>", dr["car_ReferPrice"].ToString()));
                    sb.AppendLine(string.Format("<Car_YearType>{0}</Car_YearType>", dr["Car_YearType"].ToString()));
                    sb.AppendLine(string.Format("<IsState>{0}</IsState>", dr["IsState"].ToString()));
                    sb.AppendLine(string.Format("<IsLock>{0}</IsLock>", dr["IsLock"].ToString()));
                    sb.AppendLine(string.Format("<OLdCar_Id>{0}</OLdCar_Id>", dr["OLdCar_Id"].ToString()));
                    sb.AppendLine(string.Format("<CreateTime>{0}</CreateTime>", dr["CreateTime"].ToString()));
                    sb.AppendLine(string.Format("<UpdateTime>{0}</UpdateTime>", dr["UpdateTime"].ToString()));
                    // 补充字段
                    // 报价区间
                    sb.AppendLine(string.Format("<AveragePrice>{0}</AveragePrice>", GetCarPriceRangeByID(carid)));
                    sb.AppendLine(string.Format("<Cs_Name>{0}</Cs_Name>", System.Security.SecurityElement.Escape(dr["CsName"].ToString())));
                    sb.AppendLine(string.Format("<Cs_ShowName>{0}</Cs_ShowName>", System.Security.SecurityElement.Escape(dr["CsShowName"].ToString())));
                    sb.AppendLine(string.Format("<Cs_AllSpell>{0}</Cs_AllSpell>", System.Security.SecurityElement.Escape(dr["AllSpell"].ToString())));
                    // CarImg
                    Dictionary<int, string> dicCarPhoto = carBll.GetCarDefaultPhotoDictionary(2);
                    PageBase page = new PageBase();
                    Dictionary<int, string> dicCsPhoto = page.GetAllSerialPicURL(true);
                    string carPic = "";
                    if (dicCarPhoto.ContainsKey(carid))
                    { carPic = dicCarPhoto[carid]; }
                    else if (dicCsPhoto.ContainsKey(ce.SerialId))
                    { carPic = dicCsPhoto[ce.SerialId]; }
                    else { }
                    sb.AppendLine(string.Format("<CarImg>{0}</CarImg>", carPic));
                    sb.AppendLine(string.Format("<Car_JiangJiaPrice>{0}</Car_JiangJiaPrice>", carJiangJiaPrice));
                    sb.AppendLine("</CarBasic>");
                }
                #endregion

                if (isNeedParam)
                {

                    #region 扩展参数

                    Dictionary<int, string> dicP = carBll.GetAllParamAliasNameDictionary();
                    Dictionary<int, string> dicCarP = carBll.GetCarAllParamByCarID(carid);
                    if (dicP != null && dicP.Count > 0 && dicCarP != null && dicCarP.Count > 0)
                    {
                        sb.AppendLine("<CarDetail>");
                        foreach (KeyValuePair<int, string> carP in dicCarP)
                        {
                            if (dicP.ContainsKey(carP.Key))
                            {
                                sb.AppendLine(string.Format("<{0}>{1}</{0}>"
                                    , System.Security.SecurityElement.Escape(dicP[carP.Key])
                                    , System.Security.SecurityElement.Escape(carP.Value)));
                            }
                        }
                        sb.AppendLine("</CarDetail>");
                    }

                    #endregion

                }

                CommonFunction.EchoXml(response, sb.ToString(), "BitAuto");
            }
            else
            {
                CommonFunction.EchoXml(response, "<!-- 缺少车型ID -->", "BitAuto");
            }
        }

        /// <summary>
        /// 答疑机器人 取评测车型参数
        /// </summary>
        private void RenderPingCeCarParam()
        {
            string[] arr = paramsIds.Split(',');
            string sqlGetPingCeCar = "SELECT [SerialId],[CarId] FROM [EditorComment]";
            DataSet dsPingCeCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.CarDataUpdateConnectionString, CommandType.Text, sqlGetPingCeCar);
            if (dsPingCeCar != null && dsPingCeCar.Tables.Count > 0 && dsPingCeCar.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsPingCeCar.Tables[0].Rows)
                {
                    sb.AppendLine("<Car>");
                    sb.AppendLine(string.Format("<CarID>{0}</CarID>", dr["CarId"]));
                    sb.AppendLine(string.Format("<CsID>{0}</CsID>", dr["SerialId"]));
                    Dictionary<int, string> dicCar = new Car_BasicBll().GetCarAllParamByCarID(int.Parse(dr["CarId"].ToString()));
                    sb.AppendLine("<Params>");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i]))
                        { continue; }
                        sb.AppendLine(string.Format("<Param ID=\"{0}\" Value=\"{1}\" />"
                            , arr[i]
                            , dicCar.ContainsKey(int.Parse(arr[i])) ? System.Security.SecurityElement.Escape(dicCar[int.Parse(arr[i])]) : ""));
                    }
                    sb.AppendLine("</Params>");
                    sb.AppendLine("</Car>");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 取所有车型颜色
        /// modified by chengl Dec.15.2011
        /// 增加内饰图数据输出
        /// </summary>
        private void RenderCarColor()
        {
            int carIDInt = 0;
            // 颜色类型0:车身颜色 1:内饰颜色
            int colorType = 0;
            if (request.QueryString["carid"] != null && request.QueryString["carid"].ToString() != "")
            {
                string caridStr = request.QueryString["carid"].ToString();
                if (int.TryParse(caridStr, out carIDInt))
                { }
            }
            if (request.QueryString["colorType"] != null && request.QueryString["colorType"].ToString() != "")
            {
                int tempColorType = 0;
                if (int.TryParse(request.QueryString["colorType"].ToString(), out tempColorType))
                {
                    // 默认为车身颜色，当制定颜色类型时取特定类型颜色(0:车身颜色 1:内饰颜色)
                    if (tempColorType > 0)
                    { colorType = tempColorType; }
                }
            }
            Dictionary<int, Dictionary<string, SerialColorStruct>> dicCsColor = new Dictionary<int, Dictionary<string, SerialColorStruct>>();
            // modified by chengl Dec.15.2011
            // string sqlCsRGB = "select autoID,cs_id,colorName,colorRGB from Car_SerialColor where [type] = 0 or [type] = 2";
            string sqlCsRGB = "select autoID,cs_id,colorName,colorRGB from Car_SerialColor where {0} ";
            // colorType (0:车身颜色 1:内饰颜色)
            if (colorType == 0)
            {
                // 车身颜色
                sqlCsRGB = string.Format(sqlCsRGB, " [type] = 0 or [type] = 2 ");
            }
            else if (colorType == 1)
            {
                // 内饰颜色
                sqlCsRGB = string.Format(sqlCsRGB, " [type] = 1 ");
            }
            else
            {
                // 不满足条件 返回
                return;
            }

            DataSet dsCsRGB = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCsRGB);
            if (dsCsRGB != null && dsCsRGB.Tables.Count > 0 && dsCsRGB.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCsRGB.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string colorName = dr["colorName"].ToString().Trim();
                    SerialColorStruct scs = new SerialColorStruct();
                    scs.AutoID = int.Parse(dr["autoID"].ToString());
                    scs.CsID = csid;
                    scs.ColorName = colorName;
                    scs.ColorRGB = dr["colorRGB"].ToString().Trim();
                    if (dicCsColor.ContainsKey(csid))
                    {
                        // 包含子品牌
                        if (!dicCsColor[csid].ContainsKey(colorName))
                        {
                            // 不包含颜色名
                            dicCsColor[csid].Add(colorName, scs);
                        }
                    }
                    else
                    {
                        Dictionary<string, SerialColorStruct> dic = new Dictionary<string, SerialColorStruct>();
                        dic.Add(colorName, scs);
                        dicCsColor.Add(csid, dic);
                    }
                }
            }

            string sqlAllCarColor = @"select car.cs_id,car.car_id,cdb.pvalue
						from car_relation car
						left join car_serial cs on car.cs_id=cs.cs_id
						left join carDataBase cdb on car.car_id=cdb.carid and cdb.paramid={0}
						where car.isState=0 and cs.isState=0 and cdb.pvalue <>''
						order by car.car_id";
            if (colorType == 0)
            {
                // 车身颜色
                sqlAllCarColor = string.Format(sqlAllCarColor, 598);
            }
            else if (colorType == 1)
            {
                // 内饰颜色
                sqlAllCarColor = string.Format(sqlAllCarColor, 801);
            }
            else
            {
                // 不满足条件 返回
                return;
            }
            DataSet dsAllCarColor = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlAllCarColor);
            if (dsAllCarColor != null && dsAllCarColor.Tables.Count > 0 && dsAllCarColor.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsAllCarColor.Tables[0].Rows)
                {
                    int carid = int.Parse(dr["car_id"].ToString());
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string colors = dr["pvalue"].ToString().Trim().Replace("，", ",");

                    if (carIDInt > 0 && carIDInt != carid)
                    { continue; }

                    sb.AppendLine("<Car ID=\"" + carid.ToString() + "\" CsID=\"" + csid.ToString() + "\">");
                    string[] arrColor = colors.Split(',');
                    if (arrColor.Length > 0)
                    {
                        foreach (string color in arrColor)
                        {
                            if (color.Trim() != "")
                            {
                                sb.Append("<Color Name=\"" + color.Trim() + "\" ");
                                if (dicCsColor.ContainsKey(csid) && dicCsColor[csid].ContainsKey(color.Trim()))
                                { sb.Append("RGB=\"" + dicCsColor[csid][color.Trim()].ColorRGB + "\" ID=\"" + dicCsColor[csid][color.Trim()].AutoID + "\"/>"); }
                                else
                                { sb.AppendLine("RGB=\"\" ID=\"\"/>"); }
                            }
                        }
                    }
                    sb.AppendLine("</Car>");
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "CarColor");
        }

        private void RenderCarColorJson()
        {
            int carIDInt = 0;
            // 颜色类型0:车身颜色 1:内饰颜色
            int colorType = 0;
            if (request.QueryString["carid"] != null && request.QueryString["carid"].ToString() != "")
            {
                string caridStr = request.QueryString["carid"].ToString();
                if (int.TryParse(caridStr, out carIDInt))
                { }
            }
            if (request.QueryString["colorType"] != null && request.QueryString["colorType"].ToString() != "")
            {
                int tempColorType = 0;
                if (int.TryParse(request.QueryString["colorType"].ToString(), out tempColorType))
                {
                    // 默认为车身颜色，当制定颜色类型时取特定类型颜色(0:车身颜色 1:内饰颜色)
                    if (tempColorType > 0)
                    { colorType = tempColorType; }
                }
            }
            string callback = "";

            if (request.QueryString["callback"] != null && request.QueryString["callback"].ToString() != "")
            {
                callback = request.QueryString["callback"].ToString();
            }
            string resultData = "";
            string carColors = carBll.GetCarParamValue(carIDInt, colorType == 0 ? 598 : 801);
            if (!string.IsNullOrEmpty(carColors))
            {
                CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carIDInt);
                int csId = ce != null ? ce.SerialId : 0;
                var serialColors = new Car_SerialBll().GetAllSerialColorRGB(csId, colorType == 0 ? 3 : 1);
                StringBuilder sb = new StringBuilder();
                string[] arrColor = carColors.Split(',');
                if (arrColor.Length > 0)
                {
                    for (int i = 0; i < arrColor.Length; i++)
                    {
                        if (i != 0)
                        {
                            sb.Append(",");
                        }
                        string color = arrColor[i].Trim();
                        if (color != "")
                        {
                            if (serialColors.ContainsKey(color))
                            {
                                sb.AppendFormat("{{\"name\":\"{0}\",\"rgb\":\"{1}\",\"id\":{2}}}", color, serialColors[color].ColorRGB, serialColors[color].AutoID);
                            }
                            else
                            {
                                sb.AppendFormat("{{\"name\":\"{0}\",\"rgb\":\"{1}\",\"id\":{2}}}", color, "", 0);
                            }
                        }
                    }
                    resultData = string.Format("{{\"carid\":{1},\"csid\":{2},\"color\":[{0}]}}", sb.ToString(), carIDInt, csId);
                }
                resultData = ResultUtil.SuccessResult(resultData);
            }
            else
            {
                resultData = ResultUtil.ErrorResult(0, "没有数据", "");
            }
            if (callback != "")
            {
                response.Write(ResultUtil.CallBackResult(callback, resultData));
            }
            else
            {
                response.Write(resultData);
            }
        }

        /// <summary>
        /// 根据年款获取车型颜色
        /// modified by wangzheng Dec.26.2014
        /// </summary>
        private void RenderCarColorByYear()
        {
            int csIDInt = 0;
            int yearInt = 0;
            // 颜色类型0:车身颜色 1:内饰颜色
            int colorType = 0;
            # region 获取参数
            if (!string.IsNullOrEmpty(request.QueryString["csid"]))
            {
                string csidStr = request.QueryString["csid"];
                if (int.TryParse(csidStr, out csIDInt))
                { }
            }
            if (!string.IsNullOrEmpty(request.QueryString["year"]))
            {
                string yearStr = request.QueryString["year"];
                if (int.TryParse(yearStr, out yearInt))
                { }
            }
            if (request.QueryString["colorType"] != null && request.QueryString["colorType"].ToString() != "")
            {
                int tempColorType = 0;
                if (int.TryParse(request.QueryString["colorType"].ToString(), out tempColorType))
                {
                    // 默认为车身颜色，当制定颜色类型时取特定类型颜色(0:车身颜色 1:内饰颜色)
                    if (tempColorType > 0)
                    { colorType = tempColorType; }
                }
            }

            if (csIDInt == 0 || yearInt == 0)
            {
                return;
            }

            # endregion

            var dicCsColor = new Dictionary<int, Dictionary<string, SerialColorStruct>>();
            #region 车系颜色
            string sqlCsRGB = "select autoID,cs_id,colorName,colorRGB from Car_SerialColor where {0} ";
            // colorType (0:车身颜色 1:内饰颜色)
            if (colorType == 0)
            {
                // 车身颜色
                sqlCsRGB = string.Format(sqlCsRGB, " [type] = 0 or [type] = 2 ");
            }
            else if (colorType == 1)
            {
                // 内饰颜色
                sqlCsRGB = string.Format(sqlCsRGB, " [type] = 1 ");
            }
            else
            {
                // 不满足条件 返回
                return;
            }

            DataSet dsCsRGB = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCsRGB);
            if (dsCsRGB != null && dsCsRGB.Tables.Count > 0 && dsCsRGB.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsCsRGB.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string colorName = dr["colorName"].ToString().Trim();
                    SerialColorStruct scs = new SerialColorStruct();
                    scs.AutoID = int.Parse(dr["autoID"].ToString());
                    scs.CsID = csid;
                    scs.ColorName = colorName;
                    scs.ColorRGB = dr["colorRGB"].ToString().Trim();
                    if (dicCsColor.ContainsKey(csid))
                    {
                        // 包含子品牌
                        if (!dicCsColor[csid].ContainsKey(colorName))
                        {
                            // 不包含颜色名
                            dicCsColor[csid].Add(colorName, scs);
                        }
                    }
                    else
                    {
                        Dictionary<string, SerialColorStruct> dic = new Dictionary<string, SerialColorStruct>();
                        dic.Add(colorName, scs);
                        dicCsColor.Add(csid, dic);
                    }
                }
            }
            # endregion


            string sqlAllCarColor = @"select car.cs_id,car.car_id,car.Car_YearType,cdb.pvalue
						from car_relation car
						left join car_serial cs on car.cs_id=cs.cs_id
						left join carDataBase cdb on car.car_id=cdb.carid and cdb.paramid={0}
						where car.isState=0 and car.cs_Id={2} and car.Car_YearType={1} and cs.isState=0 and cdb.pvalue <>''
						order by car.car_id";
            if (colorType == 0)
            {
                // 车身颜色
                sqlAllCarColor = string.Format(sqlAllCarColor, 598, yearInt, csIDInt);
            }
            else if (colorType == 1)
            {
                // 内饰颜色
                sqlAllCarColor = string.Format(sqlAllCarColor, 801, yearInt, csIDInt);
            }
            else
            {
                // 不满足条件 返回
                return;
            }
            DataSet dsCarColor = Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlAllCarColor);
            if (dsCarColor != null && dsCarColor.Tables.Count > 0 && dsCarColor.Tables[0].Rows.Count > 0)
            {
                var yearColor = new List<string>();
                sb.AppendLine("<Year CsID=\"" + csIDInt + "\" Year=\"" + yearInt + "\" >");
                foreach (DataRow dr in dsCarColor.Tables[0].Rows)
                {
                    int carid = int.Parse(dr["car_id"].ToString());
                    int csid = int.Parse(dr["cs_id"].ToString());
                    int year = int.Parse(dr["Car_YearType"].ToString());
                    string colors = dr["pvalue"].ToString().Trim().Replace("，", ",");

                    string[] arrColor = colors.Split(',');
                    if (arrColor.Length > 0)
                    {
                        foreach (string color in arrColor)
                        {
                            if (color.Trim() != "" && !yearColor.Contains(color.Trim()))
                            {
                                yearColor.Add(color.Trim());
                                sb.Append("<Color Name=\"" + color.Trim() + "\" ");
                                if (dicCsColor.ContainsKey(csid) && dicCsColor[csid].ContainsKey(color.Trim()))
                                { sb.Append("RGB=\"" + dicCsColor[csid][color.Trim()].ColorRGB + "\" ID=\"" + dicCsColor[csid][color.Trim()].AutoID + "\"/>"); }
                                else
                                { sb.AppendLine("RGB=\"\" ID=\"\"/>"); }
                            }
                        }
                    }
                }
                sb.AppendLine("</Year>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "YearColor");

        }

        /// <summary>
        /// 根据车型ID取车型基本信息
        /// </summary>
        private void RenderCarInfoByID()
        {
            int carid = ConvertHelper.GetInteger(request.QueryString["id"]);
            DataTable dt = TCarDAL.GetPartCarInfoById(carid).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sb.AppendFormat("<Car CarID=\"{0}\" CarName=\"{1}\" SerialId=\"{2}\" CarYearType=\"{3}\" ReferPrice=\"{4}\" "
                    , dr["car_id"], System.Security.SecurityElement.Escape(dr["car_name"].ToString())
                    , dr["cs_id"], dr["car_yeartype"], dr["car_referprice"]);
                Dictionary<int, string> dict = carBll.GetCarAllParamByCarID(carid);
                sb.AppendFormat(" EngineExhaust=\"{0}\" ", dict.ContainsKey(423) ? dict[423] : "");
                sb.AppendFormat(" EngineExhaustForFloat=\"{0}\" ", dict.ContainsKey(785) ? dict[785] : "");
                sb.AppendFormat(" UnderPanTransmissionType=\"{0}\" ", dict.ContainsKey(712) ? dict[712] : "");
                sb.AppendFormat(" EngineType=\"{0}\" ", dict.ContainsKey(436) ? System.Security.SecurityElement.Escape(dict[436]) : "");
                sb.AppendFormat(" OilFuelType=\"{0}\" ", dict.ContainsKey(578) ? dict[578] : "");
                sb.AppendFormat(" EngineMaxPower=\"{0}\" ", dict.ContainsKey(430) ? dict[430] : "");
                sb.AppendFormat(" PerfDriveType=\"{0}\" ", dict.ContainsKey(655) ? dict[655] : "");
                sb.AppendFormat(" PerfWeight=\"{0}\" ", dict.ContainsKey(669) ? dict[669] : "");
                sb.AppendFormat(" PerfTonnage=\"{0}\" ", dict.ContainsKey(667) ? dict[667] : "");
                string Engine_InhaleType = dict.ContainsKey(425) ? dict[425] : "";
                sb.Append(" InhaleType=\"" + CommonFunction.GetInhaleType(Engine_InhaleType, "") + "\" ");
                // 车型报价
                sb.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
                sb.Append("/>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "root");
        }

        /// <summary>
        /// 所有车型列表 按年款分组 可以按子品牌取
        /// </summary>
        private void RenderAllCarByYear()
        {
            bool onlySale = false;
            string isOnlySale = request.QueryString["isOnlySale"];
            if (!string.IsNullOrEmpty(isOnlySale) && isOnlySale == "1")
            { onlySale = true; }
            int csid = ConvertHelper.GetInteger(request.QueryString["csid"]);
            DataSet ds = carBll.GetCarListGroupbyYear(csid, onlySale);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string currentCS = "";
                string currentYear = "";
                // modified by chengl Apr.11.2012
                Dictionary<int, string> dic423 = carBll.GetCarParamExDic(423);
                Dictionary<int, string> dic785 = carBll.GetCarParamExDic(785);
                Dictionary<int, string> dic712 = carBll.GetCarParamExDic(712);
                Dictionary<int, string> dic436 = carBll.GetCarParamExDic(436);
                Dictionary<int, string> dic578 = carBll.GetCarParamExDic(578);
                Dictionary<int, string> dic430 = carBll.GetCarParamExDic(430);
                Dictionary<int, string> dic655 = carBll.GetCarParamExDic(655);
                Dictionary<int, string> dic669 = carBll.GetCarParamExDic(669);
                Dictionary<int, string> dic667 = carBll.GetCarParamExDic(667);
                Dictionary<int, string> dic425 = carBll.GetCarParamExDic(425);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (currentCS == "")
                    {
                        // 第1行
                        currentCS = dr["cs_id"].ToString().Trim();
                        currentYear = dr["Car_YearType"].ToString().Trim();
                        sb.AppendLine("<Serial CsID=\"" + currentCS + "\" >");
                        sb.AppendLine("<CarYear Year=\"" + currentYear + "\" >");
                    }
                    else
                    {
                        if (currentCS == dr["cs_id"].ToString().Trim())
                        {
                            // 相同子品牌
                            if (currentYear == dr["Car_YearType"].ToString().Trim())
                            {
                                // 相同年款
                            }
                            else
                            {
                                // 不同年款
                                sb.AppendLine("</CarYear>");
                                currentYear = dr["Car_YearType"].ToString().Trim();
                                sb.AppendLine("<CarYear Year=\"" + currentYear + "\" >");
                            }
                        }
                        else
                        {
                            // 不同子品牌
                            sb.AppendLine("</CarYear>");
                            sb.AppendLine("</Serial>");
                            currentCS = dr["cs_id"].ToString().Trim();
                            currentYear = dr["Car_YearType"].ToString().Trim();
                            sb.AppendLine("<Serial CsID=\"" + currentCS + "\" >");
                            sb.AppendLine("<CarYear Year=\"" + currentYear + "\" >");
                        }
                    }
                    // modified by chengl Dec.26.2011
                    // sb.Append("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&","＆").Replace("\"","'") + "\" />");
                    sb.AppendLine("<Car CarID=\"" + dr["car_id"].ToString().Trim() + "\" CarName=\"" + dr["car_name"].ToString().Trim().Replace("&", "＆").Replace("\"", "'") + "\" ");
                    sb.Append(" CarYearType=\"" + dr["CarYearType"].ToString().Trim() + "\" ReferPrice=\"" + dr["car_ReferPrice"].ToString() + "\" ");
                    sb.Append(string.Format(" Sale=\"{0}\"", System.Security.SecurityElement.Escape(dr["Car_SaleState"].ToString().Trim())));
                    //根据车型ID取参数
                    int carId = ConvertHelper.GetInteger(dr["car_id"]);
                    // modified by chengl Apr.11.2012
                    sb.Append(string.Format(" EngineExhaust=\"{0}\" ", dic423.ContainsKey(carId) ? dic423[carId] : ""));
                    sb.Append(string.Format(" EngineExhaustForFloat=\"{0}\" ", dic785.ContainsKey(carId) ? dic785[carId] : ""));
                    sb.Append(string.Format(" UnderPanTransmissionType=\"{0}\" ", dic712.ContainsKey(carId) ? dic712[carId] : ""));
                    sb.Append(string.Format(" EngineType=\"{0}\" ", dic436.ContainsKey(carId) ? System.Security.SecurityElement.Escape(dic436[carId]) : ""));
                    sb.Append(string.Format(" OilFuelType=\"{0}\" ", dic578.ContainsKey(carId) ? dic578[carId] : ""));
                    sb.Append(string.Format(" EngineMaxPower=\"{0}\" ", dic430.ContainsKey(carId) ? dic430[carId] : ""));
                    sb.Append(string.Format(" PerfDriveType=\"{0}\" ", dic655.ContainsKey(carId) ? dic655[carId] : ""));
                    sb.Append(string.Format(" PerfWeight=\"{0}\" ", dic669.ContainsKey(carId) ? dic669[carId] : ""));
                    sb.Append(string.Format(" PerfTonnage=\"{0}\" ", dic667.ContainsKey(carId) ? dic667[carId] : ""));
                    // modified by chengl Aug.16.2013
                    // 增压方式有包含增压字样的都算增压
                    string Engine_InhaleType = dic425.ContainsKey(carId) ? dic425[carId] : "";
                    sb.Append(" InhaleType=\"" + CommonFunction.GetInhaleType(Engine_InhaleType, "") + "\" ");
                    // 车型报价
                    sb.Append(" CarPriceRange=\"" + GetCarPriceRangeByID(int.Parse(dr["car_id"].ToString())) + "\" ");
                    sb.Append("/>");
                }
                sb.AppendLine("</CarYear>");
                sb.AppendLine("</Serial>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }

        /// <summary>
        /// 所有车型油耗
        /// </summary>
        private void RenderAllCarOil()
        {
            int serialId = ConvertHelper.GetInteger(request.QueryString["serialid"]);
            DataSet ds = carBll.GetAllCarOrderbyCs(serialId);
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
                // 进气方式
                Dictionary<int, string> dic425 = carBll.GetCarParamExDic(425);
                // 燃油类型
                Dictionary<int, string> dic578 = carBll.GetCarParamExDic(578);
                // 加速时间
                Dictionary<int, string> dic786 = carBll.GetCarParamExDic(786);
                // 制动距离
                Dictionary<int, string> dic787 = carBll.GetCarParamExDic(787);

                int lastCsID = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int carid = int.Parse(dr["car_id"].ToString());
                    string carName = dr["car_name"].ToString().Trim();
                    int csid = int.Parse(dr["cs_id"].ToString());
                    string csName = dr["cs_name"].ToString().Trim();
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
                        + "\" Perf_ZongHeYouHao=\"" + (dic782.ContainsKey(carid) ? dic782[carid] : "0") + "\" ");
                    // modified by chengl Aug.16.2013
                    // 增压方式有包含增压字样的都算增压
                    sb.AppendLine(" InhaleType=\"" + (dic425.ContainsKey(carid) ? CommonFunction.GetInhaleType(dic425[carid], "") : "L") + "\""
                        + " FuelType=\"" + (dic578.ContainsKey(carid) ? dic578[carid] : "") + "\" ");
                    //add 2016-10-13 加速时间 制动距离
                    sb.AppendLine(" Perf_MeasuredAcceleration=\"" + (dic786.ContainsKey(carid) ? dic786[carid] : "") + "\""
                       + " Perf_BrakingDistance=\"" + (dic787.ContainsKey(carid) ? dic787[carid] : "") + "\" />");
                }
                sb.AppendLine("</Serial>");
            }
            CommonFunction.EchoXml(response, sb.ToString(), "root");
        }

        /// <summary>
        /// add for liurw Ask
        /// 取所有车型ID和子品牌ID
        /// </summary>
        private void RenderCarInfoByCsID()
        {
            int csid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["csid"])
                && int.TryParse(request.QueryString["csid"].ToString(), out csid))
            { }
            if (csid <= 0)
            { Echo("<!--缺少子品牌ID-->"); }
            IList<Car_BasicEntity> listCat = new Car_BasicBll().Get_Car_BasicByCsID(csid);
            if (listCat != null && listCat.Count > 0)
            {
                foreach (Car_BasicEntity carBE in listCat)
                {
                    sb.AppendFormat("<Car ID=\"{0}\" Name=\"{1}\" Year=\"{2}\" Price=\"{3}\" />"
                        , carBE.Car_Id
                        , System.Security.SecurityElement.Escape(carBE.Car_Name)
                        , carBE.Car_YearType
                        , base.GetCarPriceRangeByID(carBE.Car_Id));
                }
            }
            this.Echo(sb.ToString());
        }

        /// <summary>
        /// 取所有车型包括无效
        /// </summary>
        private void RenderAllCarData()
        {
            DataSet ds = new Car_BasicBll().GetAllCarContainNoValid();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.AppendFormat("<Car><ID>{0}</ID>", dr["car_id"]);
                    // modified by chengl Nov.4.2013
                    sb.AppendFormat("<IsState>{0}</IsState></Car>", (
                        (dr["isState"].ToString() == "1" && dr["csisState"].ToString() == "1")
                        ? "有效" : "无效"));
                }
            }
            this.Echo(sb.ToString());
        }

        /// <summary>
        /// 生成主品牌数据
        /// </summary>
        private void RenderMasterData()
        {
            int masterId = ConvertHelper.GetInteger(request.QueryString["id"]);
            DataTable dt = TMasterBrandDAL.GetPartMasterInfoById(masterId).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sb.AppendFormat("<BsId>{0}</BsId>", dr["bs_id"]);
                sb.AppendFormat("<BsName>{0}</BsName>", dr["bs_name"]);
                sb.AppendFormat("<BsSpell>{0}</BsSpell>", dr["spell"]);
                sb.AppendFormat("<BsAllSpell>{0}</BsAllSpell>", dr["urlspell"]);
                sb.AppendFormat("<BsIsState>{0}</BsIsState>", ConvertHelper.GetInteger(dr["isstate"]) == 0 ? "有效" : "无效");
                sb.AppendFormat("<BsSeoName>{0}</BsSeoName>", dr["bs_seoname"]);
                sb.AppendFormat("<BsUpdateTime>{0}</BsUpdateTime>", dr["updatetime"]);
            }
            this.Echo(sb.ToString());
        }
        /// <summary>
        /// 生成品牌数据
        /// </summary>
        private void RenderBrandData()
        {
            int brandId = ConvertHelper.GetInteger(request.QueryString["id"]);
            DataTable dt = TBrandDAL.GetPartBrandDataById(brandId).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sb.AppendFormat("<CbId>{0}</CbId>", dr["cb_id"]);
                sb.AppendFormat("<CbName>{0}</CbName>", dr["cb_name"]);
                sb.AppendFormat("<CbSpell>{0}</CbSpell>", dr["spell"]);
                sb.AppendFormat("<CbAllSpell>{0}</CbAllSpell>", dr["allSpell"]);
                sb.AppendFormat("<CbSeoName>{0}</CbSeoName>", dr["cb_seoname"]);
                sb.AppendFormat("<CbIsState>{0}</CbIsState>", ConvertHelper.GetInteger(dr["isstate"]) == 0 ? "有效" : "无效");
                sb.AppendFormat("<CbCountry>{0}</CbCountry>", dr["CpCountryName"]);
                sb.AppendFormat("<CbCountryInt>{0}</CbCountryInt>", dr["CpCountry"]);
                sb.AppendFormat("<CbUpdateTime>{0}</CbUpdateTime>", dr["updatetime"]);
                sb.AppendFormat("<Producer>{0}</Producer>", dr["cp_name"]);
                sb.AppendFormat("<BsId>{0}</BsId>", dr["bs_id"]);
            }
            this.Echo(sb.ToString());
        }
        /// <summary>
        /// 生成子品牌数据
        /// </summary>
        private void RenderSerialData()
        {
            int serialId = ConvertHelper.GetInteger(request.QueryString["id"]);
            DataTable dt = TSerialDAL.GetPartSerialDataById(serialId).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sb.AppendFormat("<CsId>{0}</CsId>", dr["cs_id"]);
                sb.AppendFormat("<CsName>{0}</CsName>", dr["csname"]);
                sb.AppendFormat("<CsSpell>{0}</CsSpell>", dr["spell"]);
                sb.AppendFormat("<CsAllSpell>{0}</CsAllSpell>", dr["allSpell"]);
                sb.AppendFormat("<CsSeoName>{0}</CsSeoName>", dr["cs_seoname"]);
                sb.AppendFormat("<CsShowName>{0}</CsShowName>", dr["csshowname"]);
                sb.AppendFormat("<CsLevel>{0}</CsLevel>", dr["cscarlevel"]);
                sb.AppendFormat("<CsIsState>{0}</CsIsState>", ConvertHelper.GetInteger(dr["isstate"]) == 0 ? "有效" : "无效");
                sb.AppendFormat("<CsUpdateTime>{0}</CsUpdateTime>", dr["updatetime"]);
                sb.AppendFormat("<CsSaleState>{0}</CsSaleState>", dr["csSaleState"]);
                sb.AppendFormat("<CbId>{0}</CbId>", dr["cb_id"]);
            }
            this.Echo(sb.ToString());

        }
        /// <summary>
        /// 输出车型数据
        /// </summary>
        private void RenderCarData()
        {
            int carId = ConvertHelper.GetInteger(request.QueryString["id"]);

            DataTable dt = TCarDAL.GetPartCarInfoById(carId).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ////根据车型ID取参数
                //Dictionary<int, string> dict = basicBll.GetCarAllParamByCarID(carId);
                //string Engine_InhaleType = dict.ContainsKey(425) ? dict[425] : "";
                string[] arr = paramsIds.Split(',');
                sb.AppendFormat("<CarID>{0}</CarID>", dr["car_id"]);
                sb.AppendFormat("<CsID>{0}</CsID>", dr["cs_id"]);
                sb.AppendFormat("<CarLevel>{0}</CarLevel>", dr["cs_CarLevel"].ToString());
                sb.AppendFormat("<CpName><![CDATA[{0}]]></CpName>", dr["cp_name"].ToString());
                sb.AppendFormat("<IsState>{0}</IsState>", (dr["isState"].ToString() == "1" ? "有效" : "无效"));
                sb.AppendFormat("<CarName><![CDATA[{0}]]></CarName>", dr["car_name"].ToString());
                sb.AppendFormat("<CarYearType>{0}</CarYearType>", dr["car_yeartype"]);
                sb.AppendFormat("<ReferPrice>{0}</ReferPrice>", dr["car_referprice"]);
                sb.AppendFormat("<CarPriceRange>{0}</CarPriceRange>", base.GetCarPriceRangeByID(carId));
                sb.AppendFormat("<CarSaleState>{0}</CarSaleState>", dr["Car_SaleState"]);
                DataTable dtCar = TCarDAL.GetCarParamsListByCarId(carId).Tables[0];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (string.IsNullOrEmpty(arr[i]))
                        continue;
                    DataRow[] arrDrParam = dtCar.Select("ParamId=" + arr[i]);
                    foreach (DataRow drParam in arrDrParam)
                    {
                        if (arr[i] == "425")
                        {
                            string InhaleType = "L";
                            if (drParam["Pvalue"] != null && drParam["Pvalue"].ToString().IndexOf("增压") >= 0)
                                InhaleType = "T";
                            sb.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", drParam["AliasName"], InhaleType);
                        }
                        else
                        {
                            sb.AppendFormat("<{0}><![CDATA[{1}]]></{0}>", drParam["AliasName"], drParam["Pvalue"]);
                        }
                    }
                }
                //sb.AppendFormat("<EngineExhaust>{0}</EngineExhaust>", dict.ContainsKey(423) ? dict[423] : "");
                //sb.AppendFormat("<EngineExhaustForFloat>{0}</EngineExhaustForFloat>", dict.ContainsKey(785) ? dict[785] : "");
                //sb.AppendFormat("<EngineMaxPower>{0}</EngineMaxPower>", dict.ContainsKey(430) ? dict[430] : "");
                //sb.AppendFormat("<PerfDriveType>{0}</PerfDriveType>", dict.ContainsKey(655) ? dict[655] : "");
                //sb.AppendFormat("<PerfWeigth>{0}</PerfWeigth>", dict.ContainsKey(669) ? dict[669] : "");
                //sb.AppendFormat("<PerfTonnage>{0}</PerfTonnage>", dict.ContainsKey(667) ? dict[667] : "");
                //sb.AppendFormat("<InhaleType>{0}</InhaleType>", InhaleType);
                //sb.AppendFormat("<UnderPanTransmissionType>{0}</UnderPanTransmissionType>", dict.ContainsKey(712) ? dict[712] : "");
                //sb.AppendFormat("<EngineType>{0}</EngineType>", dict.ContainsKey(436) ? System.Security.SecurityElement.Escape(dict[436]) : "");
                //sb.AppendFormat("<OilFuelType>{0}</OilFuelType>", dict.ContainsKey(578) ? dict[578] : "");
            }
            this.Echo(sb.ToString());
        }
        //统一输出XML
        private void Echo(string str)
        {
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Clear();
            StringBuilder _sb = new StringBuilder();
            _sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            _sb.Append("<Root>");
            _sb.Append(str);
            _sb.Append("</Root>");
            response.Write(_sb.ToString());
        }
        //加载XML
        private XmlDocument LoadXml()
        {
            string physicsPath = System.Web.HttpContext.Current.Server.MapPath(@"~/config/CarInfo.config");
            XmlDocument xmlDoc = null;
            string cacheName = "BitAuto_CarInfo_Api";
            Cache cache = System.Web.HttpContext.Current.Cache;
            try
            {
                if (cache[cacheName] != null)
                {
                    xmlDoc = (XmlDocument)cache[cacheName];
                }
                else
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load(physicsPath);
                    XmlElement root = xmlDoc.DocumentElement;
                    cache.Insert(cacheName, xmlDoc, new CacheDependency(physicsPath));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return xmlDoc;
        }
        //获取XML数据
        private string GetXmlData(string xpath)
        {
            string result = "";
            try
            {
                XmlDocument xmlDoc = LoadXml();
                XmlElement root = xmlDoc.DocumentElement;
                XmlNode node = root.SelectSingleNode(xpath);
                if (node != null)
                    result = node.InnerText;
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            return result;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 车船税购置税减免数据
        /// </summary>
        private void RenderGetCarParamInfo()
        {
            int csId = ConvertHelper.GetInteger(request.QueryString["csid"]);
            string sql = string.Format(@"SELECT  car.Car_Id, car.cs_id, cdb.Pvalue AS Engine_Exhaust,
									cdb2.Pvalue AS Purchase_Taxrelief,
									cdb3.Pvalue AS Purchase_Taxrelief_number
							FROM    dbo.Car_relation car
									LEFT JOIN dbo.CarDataBase cdb ON cdb.CarId = car.Car_Id
																	 AND cdb.ParamId = 785
									LEFT JOIN dbo.CarDataBase cdb2 ON cdb2.CarId = car.Car_Id
																	  AND cdb2.ParamId = 986
									LEFT JOIN dbo.CarDataBase cdb3 ON cdb3.CarId = car.Car_Id
																	  AND cdb3.ParamId = 987
							WHERE   car.IsState = 0 {0}", (csId > 0 ? " and car.cs_id=@SerialId" : ""));
            SqlParameter[] _params = { new SqlParameter("@SerialId", csId) };
            DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
                , CommandType.Text, sql, _params);
            if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
            {
                //// 购置税减免
                //Dictionary<int, string> dic986 = carBll.GetCarParamExDic(986);
                //// 购置税减免批次
                //Dictionary<int, string> dic987 = carBll.GetCarParamExDic(987);
                foreach (DataRow dr in dsCar.Tables[0].Rows)
                {
                    double dEx = ConvertHelper.GetDouble(dr["Engine_Exhaust"]);
                    string Purchase_Taxrelief = ConvertHelper.GetString(dr["Purchase_Taxrelief"]);
                    string Purchase_Taxrelief_number = ConvertHelper.GetString(dr["Purchase_Taxrelief_number"]);
                    string TaxContent = string.Empty;
                    int carId = ConvertHelper.GetInteger(dr["Car_Id"]);
                    if (!string.IsNullOrEmpty(Purchase_Taxrelief_number) && (Purchase_Taxrelief_number == "第1批" || Purchase_Taxrelief_number == "第2批" || Purchase_Taxrelief_number == "第3批" || Purchase_Taxrelief_number == "第4批" || Purchase_Taxrelief_number == "第5批" || Purchase_Taxrelief_number == "第6批") && !string.IsNullOrEmpty(Purchase_Taxrelief))
                    {
                        if (Purchase_Taxrelief == "减半")
                        {
                            TaxContent = "购置税75折";
                        }
                        else if (Purchase_Taxrelief == "免征")
                        {
                            TaxContent = "免征购置税";
                        }
                    }
                    //else if (dEx > 0 && dEx <= 1.6)
                    //{
                    //    TaxContent = "购置税75折";
                    //}
                    if (string.IsNullOrEmpty(TaxContent))
                    {
                        continue;
                    }
                    sb.AppendLine(string.Format("<Item CarId=\"{0}\" CsId=\"{1}\" TaxRelief=\"{2}\"/>"
                           , dr["Car_Id"].ToString()
                           , dr["cs_id"].ToString()
                           , TaxContent
                           ));
                }
            }
            CommonFunction.EchoXml(response, sb.ToString(), "Root");
        }
    }

}