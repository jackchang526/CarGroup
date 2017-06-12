using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageToolV2
{
    public partial class KouBeiCompare : PageBase
    {
        private const string DefaultSerialPicUrl =
            "http://img4.baa.bitautotech.com/usergroup/editor_pic/2016/12/20/d3df1bb1268549958601c05828922cd9.png";

        private double _allPriceL;
        private double _allPriceR;
        private double _baoZhilvL;
        private double _baoZhilvR;
        protected string AllCarJsArray = string.Empty;
        protected decimal CaoKongL;
        protected decimal CaoKongR;
        protected CarEntity CarEntityL;
        protected CarEntity CarEntityR;
        protected int CarIdL = -1;
        protected int CarIdR = -1;

        protected decimal DongLiL;

        protected decimal DongLiR;

        protected string KoubeiReport = string.Empty;

        //protected string KoubeiReportL = string.Empty;
        //protected string KoubeiReportR = string.Empty;
        protected List<NewsForSerialSummaryEntity> KoubeiReportListL=new List<NewsForSerialSummaryEntity>();
        protected List<NewsForSerialSummaryEntity> KoubeiReportListR = new List<NewsForSerialSummaryEntity>();
        protected int MasterIdL = -1;
        protected int MasterIdR = -1;
        protected decimal NeiShiL;
        protected decimal NeiShiR;
        protected decimal PeiZhiL;
        protected decimal PeiZhiR;
        protected int SerialIdL = -1;
        protected int SerialIdR = -1;

        protected string SerialNameL = string.Empty;
        protected string SerialNameR = string.Empty;
        protected Dictionary<int, string> SerialPicDictionary = null;
        protected string SerialPicUrlL = DefaultSerialPicUrl;
        protected string SerialPicUrlR = DefaultSerialPicUrl;
        protected Dictionary<int, string> SerialPicWhiteDictionary = new Dictionary<int, string>();

        protected string ShowNameL = string.Empty;
        protected string ShowNameR = string.Empty;
        protected decimal WaiGuanL;
        protected decimal WaiGuanR;

        protected string WangYouDianPingL = string.Empty;
        protected string WangYouDianPingR = string.Empty;

        protected string WhoIsValueable = string.Empty;
        protected decimal YouHaoL;
        protected decimal YouHaoR;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(10);
            SerialPicDictionary = GetAllSerialPicURL(false);
            SerialPicWhiteDictionary = GetAllSerialPicURL(true);
            
            CarIdL = ConvertHelper.GetInteger(Request.QueryString["car_id_l"]);
            CarIdR = ConvertHelper.GetInteger(Request.QueryString["car_id_r"]);

            if (CarIdL == 0 && CarIdR == 0)
            {
                var csids = Request.QueryString["csids"];
                if (csids != null)
                {
                    GetDefaultCarIds(csids);
                }
            }
            
            if (CarIdL > 0)
            {
                GetIDsByCarID(CarIdL, out MasterIdL, out SerialIdL);
            }

            if (CarIdR > 0)
            {
                GetIDsByCarID(CarIdR, out MasterIdR, out SerialIdR);
            }

            if (SerialIdL > 0 && SerialPicDictionary.Keys.Contains(SerialIdL))
            {
                SerialPicUrlL = SerialPicDictionary[SerialIdL].Replace("_2.", "_8.");
            }
            if (SerialIdR > 0 && SerialPicDictionary.Keys.Contains(SerialIdR))
            {
                SerialPicUrlR = SerialPicDictionary[SerialIdR].Replace("_2.", "_8.");
            }

            if (CarIdL > 0 && CarIdR > 0 & SerialIdL > 0 && SerialIdR > 0)
            {
                CarEntityL = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarIdL);
                CarEntityR = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarIdR);

                if (CarEntityL != null && CarEntityR != null)
                {
                    GetValidCarJsObject();

                    ShowNameL = (CarEntityL.CarYear > 0 ? CarEntityL.CarYear + "款 " : "") + CarEntityL.Name;
                    ShowNameR = (CarEntityR.CarYear > 0 ? CarEntityR.CarYear + "款 " : "") + CarEntityR.Name;

                    SerialNameL = CarEntityL.Serial.ShowName;
                    SerialNameR = CarEntityR.Serial.ShowName;

                    var dicParams = GetKouBeiCompareParams(CarIdL, CarIdR);

                    #region 谁更值

                    var gouZhiShuiL = 0;
                    var cheChuanShuiL = 0;
                    var baoXianL = 0;
                    var chePaiL = 0;
                    if (dicParams.ContainsKey(CarIdL))
                    {
                        gouZhiShuiL = ConvertHelper.GetInteger(dicParams[CarIdL]["GouZhiShui"]);
                        cheChuanShuiL = ConvertHelper.GetInteger(dicParams[CarIdL]["CheChuanShui"]);
                        baoXianL = ConvertHelper.GetInteger(dicParams[CarIdL]["BaoXian"]);
                        chePaiL = ConvertHelper.GetInteger(dicParams[CarIdL]["ChePai"]);
                    }

                    var gouZhiShuiR = 0;
                    var cheChuanShuiR = 0;
                    var baoXianR = 0;
                    var chePaiR = 0;
                    if (dicParams.ContainsKey(CarIdR))
                    {
                        gouZhiShuiR = ConvertHelper.GetInteger(dicParams[CarIdR]["GouZhiShui"]);
                        cheChuanShuiR = ConvertHelper.GetInteger(dicParams[CarIdR]["CheChuanShui"]);
                        baoXianR = ConvertHelper.GetInteger(dicParams[CarIdR]["BaoXian"]);
                        chePaiR = ConvertHelper.GetInteger(dicParams[CarIdR]["ChePai"]);
                    }

                    _allPriceL = CarEntityL.ReferPrice + (double)(gouZhiShuiL + cheChuanShuiL + baoXianL + chePaiL) / 10000;
                    _allPriceL = _allPriceL > 100 ? Math.Round(_allPriceL) : Math.Round(_allPriceL, 2);
                    _allPriceR = CarEntityR.ReferPrice + (double)(gouZhiShuiR + cheChuanShuiR + baoXianR + chePaiR) / 10000;
                    _allPriceR = _allPriceR > 100 ? Math.Round(_allPriceR) : Math.Round(_allPriceR, 2);
                    var referPriceL = Math.Round(CarEntityL.ReferPrice, 2);
                    var referPriceR = Math.Round(CarEntityR.ReferPrice, 2);

                    var newsBll = new CarNewsBll();
                    KoubeiReportListL = newsBll.GetSerialNewsByCategoryId(SerialIdL, 17, 1);
                    KoubeiReportListR = newsBll.GetSerialNewsByCategoryId(SerialIdR, 17, 1);


                    WhoIsValueable += GetWangYouPingFen(dicParams);

                    WhoIsValueable += GetQuanKuanZongJia(referPriceL, gouZhiShuiL, cheChuanShuiL, baoXianL, chePaiL,
                        referPriceR, gouZhiShuiR, cheChuanShuiR, baoXianR, chePaiR);

                    WhoIsValueable += GetBaoZhiHtml(dicParams);

                    #endregion

                    #region 网友点评

                    var commonhtmlBll = new CommonHtmlBll();
                    WangYouDianPingL = commonhtmlBll.GetCommonHtmlByBlockId(SerialIdL, CommonHtmlEnum.TypeEnum.Serial,
                        CommonHtmlEnum.TagIdEnum.KouBeiDuiBi,
                        CommonHtmlEnum.BlockIdEnum.WangYouDianPing);
                    WangYouDianPingR = commonhtmlBll.GetCommonHtmlByBlockId(SerialIdR, CommonHtmlEnum.TypeEnum.Serial,
                        CommonHtmlEnum.TagIdEnum.KouBeiDuiBi,
                        CommonHtmlEnum.BlockIdEnum.WangYouDianPing);


                    //KoubeiReportL = commonhtmlBll.GetCommonHtmlByBlockId(SerialIdL, CommonHtmlEnum.TypeEnum.Serial,
                    //    CommonHtmlEnum.TagIdEnum.KouBeiDuiBi,
                    //    CommonHtmlEnum.BlockIdEnum.KouBeiDuiBi);
                    //KoubeiReportR = commonhtmlBll.GetCommonHtmlByBlockId(SerialIdR, CommonHtmlEnum.TypeEnum.Serial,
                    //    CommonHtmlEnum.TagIdEnum.KouBeiDuiBi,
                    //    CommonHtmlEnum.BlockIdEnum.KouBeiDuiBi);

                    #endregion

                    #region 评分

                    var csb = new Car_SerialBll();
                    var dic = csb.GetAllCsKoubeiBaseInfo();
                    var csKoubeiBaseInfoL = dic[SerialIdL];
                    var csKoubeiBaseInfoR = dic[SerialIdR];

                    CaoKongL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["CaoKong"], 2, MidpointRounding.AwayFromZero);
                    PeiZhiL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["PeiZhi"], 2, MidpointRounding.AwayFromZero);
                    NeiShiL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["NeiShi"], 2, MidpointRounding.AwayFromZero);
                    DongLiL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["DongLi"], 2, MidpointRounding.AwayFromZero);
                    WaiGuanL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["WaiGuan"], 2, MidpointRounding.AwayFromZero);
                    YouHaoL = Math.Round(csKoubeiBaseInfoL.DicSubKoubei["YouHao"], 2, MidpointRounding.AwayFromZero);

                    CaoKongR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["CaoKong"], 2, MidpointRounding.AwayFromZero);
                    PeiZhiR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["PeiZhi"], 2, MidpointRounding.AwayFromZero);
                    NeiShiR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["NeiShi"], 2, MidpointRounding.AwayFromZero);
                    DongLiR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["DongLi"], 2, MidpointRounding.AwayFromZero);
                    WaiGuanR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["WaiGuan"], 2, MidpointRounding.AwayFromZero);
                    YouHaoR = Math.Round(csKoubeiBaseInfoR.DicSubKoubei["YouHao"], 2, MidpointRounding.AwayFromZero);

                    #endregion

                    GetHotCar();
                }
                
            }
        }

        private void GetDefaultCarIds(string csIDs)
        {
            if (string.IsNullOrEmpty(csIDs)) return;

            Car_BasicBll carBasicBll = new Car_BasicBll();
            var serialIdArray = csIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct().Select(serialId => ConvertHelper.GetInteger(serialId));
            var idArray = serialIdArray as int[] ?? serialIdArray.ToArray();
            Dictionary<int, int> dict = carBasicBll.GetHotCarForPhotoCompareBySerialId(idArray);

            if (idArray.Count()>= 2)
            {
                SerialIdL = idArray[0];
                SerialIdR = idArray[1];
            }
            if (dict.Count >= 2)
            {
                if (dict.ContainsKey(SerialIdL))
                {
                    CarIdL = dict[SerialIdL];
                }
                if (dict.ContainsKey(SerialIdR))
                {
                    CarIdR = dict[SerialIdR];
                }
            }
        }

        private string GetWangYouPingFen(Dictionary<int, Dictionary<string, string>> dicParams)
        {
            var sb = new StringBuilder();
            sb.Append("<h5><em></em>网友评分</h5>");
            sb.Append("<div class=\"pinfen-box\">	");
            sb.Append("	<div class=\"pf-cont fl\">");
            sb.Append("		<div class=\"circular\">");
            decimal koubeiL = 0;

            if (dicParams.ContainsKey(CarIdL))
            {
                decimal.TryParse(dicParams[CarIdL]["Koubei"], out koubeiL);
            }
            if (dicParams.ContainsKey(CarIdL)&&dicParams[CarIdL] != null && dicParams[CarIdL]["Koubei"] != ""&& koubeiL > 0)
            {
                sb.AppendFormat(
                    "			<input id=\"knob_l\" class=\"knob\" data-angleoffset=\"0\" readonly=\"readonly\" data-linecap=\"round\" data-value=\"{0}\" data-displayinput=\"false\" />",
                    koubeiL);


                sb.AppendFormat("			<div class=\"fen-box\">{0}<span class=\"units\">分</span></div>",
                    Math.Round(koubeiL, 2));

                if (KoubeiReportListL.Count > 0)
                {
                    sb.AppendFormat("			<p class=\"more\"><a target=\"_blank\" href=\"{0}\">查看口碑报告</a></p>",
                        KoubeiReportListL[0].FilePath);
                }
            }
            else
            {
                sb.Append(
                    "			<input id=\"knob_l\" class=\"knob\" data-angleoffset=\"0\" readonly=\"readonly\" data-linecap=\"round\" data-value=\"0\" data-displayinput=\"false\" />");
                sb.Append("			<p class=\"gray\">暂无评分</p>");
            }
            sb.Append("		</div>");

            #region 评价标签

            var impressionL = GetKouBeiImpresssion(SerialIdL);

            if (impressionL.Count > 0)
            {
                sb.Append("		<div class=\"impression\">");
                sb.Append("			<div class=\"tit\">评价标签</div>");
                sb.Append("			<div class=\"blue-box\">");
                if (impressionL.ContainsKey("good"))
                {
                    foreach (var good in impressionL["good"])
                    {
                        sb.AppendFormat("				<span class=\"tag\">{0}</span>", good);
                    }
                }
                else
                {
                    sb.Append("			暂无数据");
                }
                sb.Append("			</div>");
                sb.Append("			<div class=\"gray-box\">");

                if (impressionL.ContainsKey("bad"))
                {
                    foreach (var bad in impressionL["bad"])
                    {
                        sb.AppendFormat("				<span class=\"tag\">{0}</span>", bad);
                    }
                }
                else
                {
                    sb.Append("			暂无数据");
                }
                sb.Append("			</div>");
                sb.Append("		</div>");
            }
            else
            {
                sb.Append("		<div class=\"impression\">");
                sb.Append("			<span class=\"gray\">暂无印象</span>");
                sb.Append("		</div>");
            }

            #endregion

            sb.Append("	</div>");

            sb.Append("	<div class=\"pf-cont fr\">");
            sb.Append("		<div class=\"circular\">");
            decimal koubeiR = 0;
            if (dicParams.ContainsKey(CarIdR))
            {
                decimal.TryParse(dicParams[CarIdR]["Koubei"], out koubeiR);
            }
            if (dicParams.ContainsKey(CarIdR)&&dicParams[CarIdR] != null && dicParams[CarIdR]["Koubei"] != ""&& koubeiR > 0)
            {
                sb.AppendFormat(
                    "			<input id=\"knob_r\" class=\"knob\" data-angleoffset=\"0\" readonly=\"readonly\" data-linecap=\"round\" data-value=\"{0}\" data-displayinput=\"false\" />",
                    koubeiR);
                
                sb.AppendFormat("			<div class=\"fen-box\">{0}<span class=\"units\">分</span></div>",
                    Math.Round(koubeiR,2));

                if (KoubeiReportListR.Count > 0)
                {
                    sb.AppendFormat("			<p class=\"more\"><a target=\"_blank\" href=\"{0}\">查看口碑报告</a></p>",
                        KoubeiReportListR[0].FilePath);
                }
            }
            else
            {
                sb.Append(
                    "			<input id=\"knob_r\" class=\"knob\" data-angleoffset=\"0\" readonly=\"readonly\" data-linecap=\"round\" data-value=\"0\" data-displayinput=\"false\" />");
                sb.Append("			<p class=\"gray\">暂无评分</p>");
            }
            sb.Append("		</div>");

            #region 评价标签

            var impressionR = GetKouBeiImpresssion(SerialIdR);

            if (impressionR.Count > 0)
            {
                sb.Append("		<div class=\"impression\">"); ////////
                sb.Append("			<div class=\"tit\">评价标签</div>");
                sb.Append("			<div class=\"blue-box\">");
                if (impressionR.ContainsKey("good"))
                {
                    foreach (var good in impressionR["good"])
                    {
                        sb.AppendFormat("				<span class=\"tag\">{0}</span>", good);
                    }
                }
                else
                {
                    sb.Append("			暂无数据");
                }
                sb.Append("			</div>");

                sb.Append("			<div class=\"gray-box\">");
                if (impressionR.ContainsKey("bad"))
                {
                    foreach (var bad in impressionR["bad"])
                    {
                        sb.AppendFormat("				<span class=\"tag\">{0}</span>", bad);
                    }
                }
                sb.Append("			</div>");
                sb.Append("		</div>");
            }
            else
            {
                sb.Append("		<div class=\"impression\">");
                sb.Append("			<span class=\"gray\">暂无印象</span>");
                sb.Append("		</div>");
            }

            #endregion

            sb.Append("	</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        private string GetQuanKuanZongJia(double referPriceL, int gouZhiShuiL, int cheChuanShuiL, int baoXianL,
            int chePaiL, double referPriceR, int gouZhiShuiR, int cheChuanShuiR, int baoXianR, int chePaiR)
        {
            var sb = new StringBuilder();
            sb.Append("<h5><em></em>全款总价</h5>");
            sb.Append("<div class=\"zj-box\">");

            if (_allPriceL > 0)
            {
                sb.AppendFormat("<div class=\"zj-cont-{0} fl\">", _allPriceL < _allPriceR ? "win" : "fail");
                sb.AppendFormat("		<div class=\"jg\">{0}<em>万元</em>", _allPriceL);
                sb.Append("                     <a href=\"#\" class=\"zj-btn\">");
                sb.Append(
                    "                          <div class=\"zj-layer\" style=\"display:none\"><span class=\"jt\"></span>");
                sb.Append("			                      <div class=\"head\">购车费用</div>");
                sb.Append("			                  	  <ul>");
                sb.AppendFormat("			                  <li>厂商指导价：{0}万元</li>", referPriceL);
                sb.AppendFormat("			                  <li>购置税：{0}元</li>", gouZhiShuiL);
                sb.AppendFormat("			                  <li>车船税：{0}元</li>", cheChuanShuiL);
                sb.AppendFormat("			                  <li>保险：{0}元</li>", baoXianL);
                sb.AppendFormat("			                  <li>上牌费（平均）：{0}元</li>", chePaiL);
                sb.Append("			                  	  </ul>");
                sb.Append("			                  </div>");
                sb.Append("                     </a>");
                sb.Append("             </div>");
                sb.AppendFormat("		<p class=\"zdj\">指导价：{0}万元</p>", referPriceL);
                sb.Append("	</div>");
            }
            else
            {
                sb.Append("<div class=\"zj-cont-fail fl\">");
                sb.Append("     <div class=\"zj-gray\">暂无数据</div>");
                sb.Append("</div>");
            }

            if (_allPriceR > 0)
            {
                sb.AppendFormat("<div class=\"zj-cont-{0} fr\">", _allPriceR < _allPriceL ? "win" : "fail");
                sb.AppendFormat("		<div class=\"jg\">{0}<em>万元</em>", _allPriceR);
                sb.Append("                     <a href=\"#\" class=\"zj-btn\">");
                sb.Append(
                    "			                  <div class=\"zj-layer\" style=\"display:none\"><span class=\"jt\"></span>");
                sb.Append("			                      <div class=\"head\">购车费用</div>");
                sb.Append("			                  	  <ul>");
                sb.AppendFormat("			                  <li>厂商指导价：{0}万元</li>", referPriceR);
                sb.AppendFormat("			                  <li>购置税：{0}元</li>", gouZhiShuiR);
                sb.AppendFormat("			                  <li>车船税：{0}元</li>", cheChuanShuiR);
                sb.AppendFormat("			                  <li>保险：{0}元</li>", baoXianR);
                sb.AppendFormat("			                  <li>上牌费（平均）：{0}元</li>", chePaiR);
                sb.Append("			                  	  </ul>");
                sb.Append("			                  </div>");
                sb.Append("                     </a>");
                sb.Append("             </div>");
                sb.AppendFormat("		<p class=\"zdj\">指导价：{0}万元</p>", referPriceR);
                sb.Append("	</div>");
            }
            else
            {
                sb.Append("<div class=\"zj-cont-fail fr\">");
                sb.Append("     <div class=\"zj-gray\">暂无数据</div>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        private string GetBaoZhiHtml(Dictionary<int, Dictionary<string, string>> dicParams)
        {
            var sb = new StringBuilder();
            double priceL = 0;
            if (dicParams.ContainsKey(CarIdL))
            {
                priceL=Math.Round(ConvertHelper.GetDouble(dicParams[CarIdL]["Price3"]), 2);
            }
            double priceR = 0;
            if (dicParams.ContainsKey(CarIdR))
            {
                priceR=Math.Round(ConvertHelper.GetDouble(dicParams[CarIdR]["Price3"]), 2);
            }
            if (CarEntityL.ReferPrice > 0 && priceL > 0)
            {
                _baoZhilvL = Math.Round(priceL/CarEntityL.ReferPrice*100, MidpointRounding.AwayFromZero);
            }
            if (CarEntityR.ReferPrice > 0 && priceR > 0)
            {
                _baoZhilvR = Math.Round(priceR/CarEntityR.ReferPrice*100, MidpointRounding.AwayFromZero);
            }
            sb.Append("<h5><em></em>三年旧车保值率</h5>");
            sb.Append("<div class=\"bzl-box\">");
            if (_baoZhilvL > 0)
            {
                sb.AppendFormat("	<div class=\"{0} fl\">", _baoZhilvL >= _baoZhilvR ? "bzl-cont-blue" : "bzl-cont-gray");
                sb.AppendFormat("		<div class=\"bzl\">{0}<em>%</em><p>三年后估价约{1}万元</p></div>", _baoZhilvL, priceL);
            }
            else
            {
                sb.Append("	        <div class=\"bzl-cont-gray fl\">");
                sb.Append("	             <div class=\"bzl-gray\">暂无数据</div>");
            }
            sb.Append("	        </div>");

            
            if (_baoZhilvR > 0)
            {
                sb.AppendFormat("	<div class=\"{0} fr\">", _baoZhilvL < _baoZhilvR ? "bzl-cont-blue" : "bzl-cont-gray");
                sb.AppendFormat("		<div class=\"bzl\">{0}<em>%</em><p>三年后估价约{1}万元</p></div>", _baoZhilvR, priceR);
            }
            else
            {
                sb.Append("	        <div class=\"bzl-cont-gray fr\">");
                sb.Append("	            <div class=\"bzl-gray\">暂无数据</div>");
            }
            sb.Append("	        </div>");

            sb.Append("</div>");

            return sb.ToString();
        }

        /// <summary>
        ///     车款参数json
        /// </summary>
        /// <returns></returns>
        private void GetValidCarJsObject()
        {
            //List<string> sbForApi = new List<string>();
            var sbForApi = new StringBuilder();
            var dicCarParam = new Car_BasicBll().GetCarCompareDataByCarIDs(new List<int> {CarIdL, CarIdR});
            var dicTemp = GetCarParameterJsonConfigNew();
            if (dicTemp != null && dicTemp.Count > 0)
            {
                var loopCar = 0;
                foreach (var kvpCar in dicCarParam)
                {
                    if (loopCar > 0)
                    {
                        sbForApi.Append(",");
                    }

                    sbForApi.Append("[");
                    // 循环模板
                    foreach (var kvpTemp in dicTemp)
                    {
                        if (kvpTemp.Key == 0)
                        {
                            // 基本数据
                            sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
                            sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
                            foreach (var param in kvpTemp.Value)
                            {
                                if (kvpCar.Value.ContainsKey(param))
                                {
                                    sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\"");
                                }
                                else
                                {
                                    sbForApi.Append(",\"\"");
                                }
                            }
                            sbForApi.Append("]");
                        }
                        else
                        {
                            // 扩展数据
                            sbForApi.Append(",[");
                            var loop = 0;
                            foreach (var param in kvpTemp.Value)
                            {
                                if (loop > 0)
                                {
                                    sbForApi.Append(",");
                                }
                                if (kvpCar.Value.ContainsKey(param))
                                {
                                    sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\"");
                                }
                                else
                                {
                                    sbForApi.Append("\"\"");
                                }
                                loop++;
                            }
                            sbForApi.Append("]");
                        }
                    }
                    sbForApi.Append("]");

                    loopCar++;
                }
            }
            if (sbForApi.Length > 0)
            {
                sbForApi.Insert(0, "var koubeiCompareJson = [");
                sbForApi.Append("];");
            }
            else
            {
                sbForApi.Append("var koubeiCompareJson = null;");
            }
            AllCarJsArray = sbForApi.ToString();
        }

        private Dictionary<string, List<string>> GetKouBeiImpresssion(int csId)
        {
            var dictionary = new Dictionary<string, List<string>>();
            var xmlPath = Path.Combine(WebConfig.DataBlockPath, @"Data\Koubei\SerialKouBei\{0}.xml");
            var fileName = string.Format(xmlPath, csId);
            if (!File.Exists(fileName)) return dictionary;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            var goodNodeList = xmlDoc.SelectNodes("root/KoubeiImpression/Good/Item");
            var badNodeList = xmlDoc.SelectNodes("root/KoubeiImpression/Bad/Item");

            if (goodNodeList != null && goodNodeList.Count > 0)
            {
                var goodList = new List<string>();
                foreach (XmlNode node in goodNodeList)
                {
                    if (goodList.Count == 5)
                    {
                        break;
                    }
                    var wordNode = node.SelectSingleNode("Keyword");
                    if (wordNode == null)
                    {
                        continue;
                    }
                    goodList.Add(wordNode.InnerText);
                }
                dictionary.Add("good", goodList);
            }
            if (badNodeList != null && badNodeList.Count > 0)
            {
                var badList = new List<string>();
                foreach (XmlNode node in badNodeList)
                {
                    if (badList.Count == 5)
                    {
                        break;
                    }
                    var wordNode = node.SelectSingleNode("Keyword");
                    if (wordNode == null)
                    {
                        continue;
                    }
                    badList.Add(wordNode.InnerText);
                }
                dictionary.Add("bad", badList);
            }
            return dictionary;
        }

        protected string GetHotCar()
        {
            StringBuilder sb=new StringBuilder();
            Car_SerialBll carSerialBll=new Car_SerialBll();
            DataSet ds=carSerialBll.GetHotCarTop10();
            
            if (ds != null)
            {
                sb.Append("<h3>");
                sb.Append("     热门车型");
                sb.Append("</h3>");
                sb.Append("<div class=\"kbdb-bot\">");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var csId = ConvertHelper.GetInteger(row["cs_id"]);
                    string imgUrl = Car_SerialBll.GetSerialImageUrl(csId).Replace("_2.", "_3.");
                    string allSpell = row["allSpell"].ToString();
                    string showName = row["cs_ShowName"].ToString();
                    string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(csId));
                    sb.Append("    <div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                    sb.Append("        <div class=\"img\">");
                    sb.AppendFormat("      <a target=\"_blank\" href=\"/{0}/\">", allSpell);
                    sb.AppendFormat("                <img src=\"{0}\">", imgUrl);
                    sb.Append("            </a>");
                    sb.Append("        </div>");
                    sb.Append("        <ul class=\"p-list\">");
                    sb.Append("            <li class=\"name\">");
                    sb.AppendFormat("                <a target=\"_blank\" href=\"/{0}/\">{1}</a>", allSpell, showName);
                    sb.Append("            </li>");
                    sb.Append("            <li class=\"price\">");
                    sb.AppendFormat("                <a target=\"_blank\" href=\"/{0}/\">{1}</a>", allSpell, priceRange);
                    sb.Append("            </li>");
                    sb.Append("        </ul>");
                    sb.Append("    </div>");
                }
                sb.Append("</div>");
            }
            return sb.ToString();
        }
    }
}