using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace H5Web.carcompare
{
    public partial class CarCompare : H5PageBase
    {
        protected int CarId1 = 0;
        protected int CarId2 = 0;
        protected CarEntity carEntity1 = null;
        protected CarEntity carEntity2 = null;
        protected string TitleHtml = string.Empty;
        protected string BottomTitleHtml = string.Empty;
        protected string PriceHtml = string.Empty;
        //protected string PriceDetailHtml = string.Empty;
        protected string KoubeiHtml = string.Empty;
        protected string BaoZhiLvHtml = string.Empty;
        protected string PriceDetailJson = string.Empty;
        protected string AllCarJsArray = string.Empty;
        private double baoZhilv1 = 0;
        private double baoZhilv2 = 0;
        private double allPrice1 = 0;
        private double allPrice2 = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(30);
            GetParams();
            Dictionary<int, Dictionary<string, string>> dicParams = GetH5CarCompareParams(CarId1, CarId2);
            if (dicParams.Count < 2) Response.Redirect("/addchexingduibi/" + (dicParams.Count > 0 ? ("?carid=" + dicParams.First().Key) : ""));

            InitPriceHtml(dicParams);
            InitKoubeiHtml(dicParams);
            InitBaoZhiLvHtml(dicParams);
            InitTitleHtml(dicParams);

            GetValidCarJsObject();
        }

        /// <summary>
        /// 车款参数json
        /// </summary>
        /// <returns></returns>
        private void GetValidCarJsObject()
        {
            //List<string> sbForApi = new List<string>();
            StringBuilder sbForApi = new StringBuilder();
            Dictionary<int, Dictionary<string, string>> dicCarParam = (new Car_BasicBll()).GetCarCompareDataByCarIDs(new List<int> { CarId1, CarId2 });
            Dictionary<int, List<string>> dicTemp = base.GetCarParameterJsonConfigNew();
            if (dicTemp != null && dicTemp.Count > 0)
            {
                int loopCar = 0;
                foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
                {
                    if (loopCar > 0)
                    { sbForApi.Append(","); }

                    sbForApi.Append("[");
                    // 循环模板
                    foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
                    {
                        if (kvpTemp.Key == 0)
                        {
                            // 基本数据
                            sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
                            sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
                            foreach (string param in kvpTemp.Value)
                            {
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append(",\"\""); }
                            }
                            sbForApi.Append("]");
                        }
                        else
                        {
                            // 扩展数据
                            sbForApi.Append(",[");
                            int loop = 0;
                            foreach (string param in kvpTemp.Value)
                            {
                                if (loop > 0)
                                { sbForApi.Append(","); }
                                if (kvpCar.Value.ContainsKey(param))
                                { sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                else
                                { sbForApi.Append("\"\""); }
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
                sbForApi.Insert(0, "var carCompareJson = [");
                sbForApi.Append("];");
            }
            else
            {
                sbForApi.Append("var carCompareJson = null;");
            }
            AllCarJsArray = sbForApi.ToString();
        }

        private void InitPriceHtml(Dictionary<int, Dictionary<string, string>> dic)
        {
            int gouZhiShui1 = ConvertHelper.GetInteger(dic[carEntity1.Id]["GouZhiShui"]);
            int cheChuanShui1 = ConvertHelper.GetInteger(dic[carEntity1.Id]["CheChuanShui"]);
            int baoXian1 = ConvertHelper.GetInteger(dic[carEntity1.Id]["BaoXian"]);
            int chePai1 = ConvertHelper.GetInteger(dic[carEntity1.Id]["ChePai"]);
            int gouZhiShui2 = ConvertHelper.GetInteger(dic[carEntity2.Id]["GouZhiShui"]);
            int cheChuanShui2 = ConvertHelper.GetInteger(dic[carEntity2.Id]["CheChuanShui"]);
            int baoXian2 = ConvertHelper.GetInteger(dic[carEntity2.Id]["BaoXian"]);
            int chePai2 = ConvertHelper.GetInteger(dic[carEntity2.Id]["ChePai"]);
            allPrice1 = carEntity1.ReferPrice + (double)(gouZhiShui1 + cheChuanShui1 + baoXian1 + chePai1) / 10000;
            allPrice1 = allPrice1 > 100 ? Math.Round(allPrice1) : Math.Round(allPrice1, 2);
            allPrice2 = carEntity2.ReferPrice + (double)(gouZhiShui2 + cheChuanShui2 + baoXian2 + chePai2) / 10000;
            allPrice2 = allPrice2 > 100 ? Math.Round(allPrice2) : Math.Round(allPrice2, 2);
            double referPrice1 = carEntity1.ReferPrice > 100 ? Math.Round(carEntity1.ReferPrice) : carEntity1.ReferPrice;
            double referPrice2 = carEntity2.ReferPrice > 100 ? Math.Round(carEntity2.ReferPrice) : carEntity2.ReferPrice;

            StringBuilder sb = new StringBuilder();
            sb.Append("<h4>全款总价</h4>");
            sb.Append("<ul class=\"col-2\">");
            sb.Append("<li class=\"col\">");
            sb.Append("<div class=\"circle orange\">");
            sb.Append("<div class=\"content\">");
            sb.AppendFormat("<span>{0}</span>", carEntity1.ReferPrice == 0 ? "<em>暂无</em>" : "<em>" + allPrice1.ToString() + "</em>万元");
            sb.AppendFormat("<p>指导价:{0}</p>", carEntity1.ReferPrice == 0 ? "暂无" : (referPrice1.ToString() + "万元"));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<a href=\"###\" class=\"more\" data-index=\"0\" data-action=\"mark-details\">查看总价构成>></a>");
            sb.Append("</li>");
            sb.Append("<li class=\"col\">");
            sb.Append("<div class=\"circle\">");
            sb.Append("<div class=\"content\">");
            sb.AppendFormat("<span>{0}</span>", carEntity2.ReferPrice == 0 ? "<em>暂无</em>" : "<em>" + allPrice2.ToString() + "</em>万元");
            sb.AppendFormat("<p>指导价:{0}</p>", carEntity2.ReferPrice == 0 ? "暂无" : (referPrice2.ToString() + "万元"));
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<a href=\"###\" class=\"more\" data-index=\"1\" data-action=\"mark-details\">查看总价构成>></a>");
            sb.Append("</li>");
            sb.Append("</ul>");

            PriceHtml = sb.ToString();

            PriceDetailJson = string.Format("[{{\"CarId\":\"{10}\",\"CsId\":\"{12}\",\"ReferPrice\":\"{0}\",\"GouZhiShui\":\"{1}\",\"CheChuanShui\":\"{2}\",\"BaoXian\":\"{3}\",\"ChePai\":\"{4}\",\"CsAllSpell\":\"{14}\"}},{{\"CarId\":\"{11}\",\"CsId\":\"{13}\",\"ReferPrice\":\"{5}\",\"GouZhiShui\":\"{6}\",\"CheChuanShui\":\"{7}\",\"BaoXian\":\"{8}\",\"ChePai\":\"{9}\",\"CsAllSpell\":\"{15}\"}}]"
                , carEntity1.ReferPrice, gouZhiShui1, cheChuanShui1, baoXian1, chePai1, carEntity2.ReferPrice, gouZhiShui2, cheChuanShui2, baoXian2, chePai2, carEntity1.Id, carEntity2.Id, carEntity1.SerialId, carEntity2.SerialId, carEntity1.Serial.AllSpell, carEntity2.Serial.AllSpell);
        }

        private void InitKoubeiHtml(Dictionary<int, Dictionary<string, string>> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<h4>网友评分</h4>");
            sb.Append("<ul class=\"col-2\">");
            sb.Append("<li class=\"col\">");
            sb.AppendFormat("<div class=\"canvas-box\" data-barbgcolor=\"#fcedeb\" data-barcolor=\"#f19c93\" data-value=\"{0}\" data-maxvalue=\"5\" data-format=\"<em>{0}</em>分\">", dic[carEntity1.Id]["Koubei"]);
            sb.Append("<div class=\"canvas-title win\">");
            sb.Append("<span><em>0</em>分</span>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.AppendFormat("<a href=\"http://car.m.yiche.com/{0}/koubei/\" class=\"more\">{1}口碑报告>></a>", carEntity1.Serial.AllSpell, carEntity1.Serial.ShowName);
            sb.Append("</li>");
            sb.Append("<li class=\"col\">");
            sb.AppendFormat("<div class=\"canvas-box\" data-barbgcolor=\"#eaf0fb\" data-barcolor=\"#85a4e9\" data-value=\"{0}\" data-maxvalue=\"5\" data-format=\"<em>{0}</em>分\">", dic[carEntity2.Id]["Koubei"]);
            sb.Append("<div class=\"canvas-title\">");
            sb.Append("<span><em>0</em>分</span>");
            sb.Append("</div>");
            sb.Append("</div>");
            //sb.AppendFormat("<a href=\"http://car.m.yiche.com/{0}/koubei/\" class=\"more\">{1}口碑报告>></a>", carEntity2.Serial.AllSpell, carEntity2.Serial.ShowName);
            sb.Append("</li>");
            sb.Append("</ul>");

            KoubeiHtml = sb.ToString();
        }

        private void InitBaoZhiLvHtml(Dictionary<int, Dictionary<string, string>> dic)
        {
            double price1 = ConvertHelper.GetDouble(dic[carEntity1.Id]["Price3"]);
            double price2 = ConvertHelper.GetDouble(dic[carEntity2.Id]["Price3"]);

            if (carEntity1.ReferPrice > 0 && price1 > 0)
            {
                baoZhilv1 = Math.Round((price1 / carEntity1.ReferPrice) * 100);
            }
            if (carEntity2.ReferPrice > 0 && price2 > 0)
            {
                baoZhilv2 = Math.Round((price2 / carEntity2.ReferPrice) * 100);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<h4>3年旧车保值率</h4>");
            sb.Append("<ul class=\"col-2\">");
            sb.Append("<li class=\"col\">");
            if (carEntity1.ReferPrice == 0 || price1 == 0)
            {
                sb.Append("<div class=\"canvas-box\" data-barbgcolor=\"#fcedeb\" data-barcolor=\"#f19c93\"  data-value=\"0\" data-format=\"暂无\">");
                sb.Append("<div class=\"canvas-title win rows-2\">");
                sb.Append("<span>暂无</span>");
                //sb.AppendFormat("<p>{0}</p>", carEntity1.ReferPrice == 0 ? "暂无" : (carEntity1.ReferPrice + "万元"));
                sb.Append("</div>");
                sb.Append("</div>");
            }
            else
            {
                sb.AppendFormat("<div class=\"canvas-box\" data-barbgcolor=\"#fcedeb\" data-barcolor=\"#f19c93\" data-value=\"{0}\" data-format=\"{0}%\">", baoZhilv1);
                sb.Append("<div class=\"canvas-title win rows-2\">");
                sb.Append("<span>0%</span>");
                sb.AppendFormat("<p>{0}万元</p>", price1);
                sb.Append("</div>");
                sb.Append("</div>");
            }
            //sb.AppendFormat("<a href=\"http://m.taoche.com/{0}/\" class=\"more\">{1}二手车>></a>", carEntity1.Serial.AllSpell, carEntity1.Serial.ShowName);
            sb.Append("</li>");
            sb.Append("<li class=\"col\">");
            if (carEntity2.ReferPrice == 0 || price2 == 0)
            {
                sb.Append("<div class=\"canvas-box\" data-barbgcolor=\"#eaf0fb\" data-barcolor=\"#85a4e9\" data-value=\"0\" data-format=\"暂无\">");
                sb.Append("<div class=\"canvas-title rows-2\">");
                sb.Append("<span>暂无</span>");
                //sb.AppendFormat("<p>{0}</p>", carEntity2.ReferPrice == 0 ? "暂无" : (carEntity2.ReferPrice + "万元"));
                sb.Append("</div>");
                sb.Append("</div>");
            }
            else
            {
                sb.AppendFormat("<div class=\"canvas-box\" data-barbgcolor=\"#eaf0fb\" data-barcolor=\"#85a4e9\" data-value=\"{0}\" data-format=\"{0}%\">", baoZhilv2);
                sb.Append("<div class=\"canvas-title rows-2\">");
                sb.Append("<span>0%</span>");
                sb.AppendFormat("<p>{0}万元</p>", price2);
                sb.Append("</div>");
                sb.Append("</div>");
            }
            //sb.AppendFormat("<a href=\"http://m.taoche.com/{0}/\" class=\"more\">{1}二手车>></a>", carEntity2.Serial.AllSpell, carEntity2.Serial.ShowName);
            sb.Append("</li>");
            sb.Append("</ul>");

            BaoZhiLvHtml = sb.ToString();
        }

        private void InitTitleHtml(Dictionary<int, Dictionary<string, string>> dic)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbBottom = new StringBuilder();
            string csPic1 = string.Empty;
            string csPic2 = string.Empty;
            int count1 = 0;
            int count2 = 0;
            GetSerialPicAndCountByCsID(carEntity1.SerialId, out csPic1, out count1, true);
            GetSerialPicAndCountByCsID(carEntity2.SerialId, out csPic2, out count2, true);
            bool isTuijianFirst = GetTuiJianFirst(dic);
            sb.Append("<div class=\"flex-header-duibi flex\">");
            sbBottom.Append("<div class=\"block flex-header-duibi rz\">");
            sb.Append("<div class=\"vs-box\">");
            sbBottom.Append("<div class=\"vs-box\">");
            sb.Append("<div class=\"left\">");
            sbBottom.Append("<div class=\"left\">");
            //sb.Append("<a href=\"###\">");
            //sbBottom.Append("<a href=\"###\">");
            sb.AppendFormat("<a id='imgleft' data-channelid='85.99.1361' href=\"/{1}/\"><img src=\"{0}\" /></a>", csPic1.Replace("_2.", "_6."),carEntity1.Serial.AllSpell);
            sbBottom.AppendFormat("<img src=\"{0}\" />", csPic1.Replace("_2.", "_6."));
            sb.Append("<div class=\"content\">");
            sbBottom.Append("<div class=\"content\">");
            sb.AppendFormat("<h6>{0}</h6>", carEntity1.Serial.ShowName);
            sbBottom.AppendFormat("<h6>{0}</h6>", carEntity1.Serial.ShowName);
            sb.AppendFormat("<p>{0}</p>", (carEntity1.CarYear > 0 ? carEntity1.CarYear + "款 " : "") + carEntity1.Name);
            sbBottom.AppendFormat("<p>{0}</p>", (carEntity1.CarYear > 0 ? carEntity1.CarYear + "款 " : "") + carEntity1.Name);
            if (isTuijianFirst)
            {
                sb.Append("<i class=\"stamp red\"></i>");
                sbBottom.Append("<i class=\"stamp red\"></i>");
            }
            sbBottom.Append("</div>");
            sb.Append("</div>");
            // sbBottom.Append("</a>");
            // sb.Append("</a>");
            sb.AppendFormat("<a href=\"javaScript:;\" data-action=\"changecar\" data-index=\"1\" carid=\"{0}\" csid=\"{1}\" class=\"more\">其它车型>></a>", carEntity1.Id, carEntity1.SerialId);
            //sbBottom.AppendFormat("<a href=\"http://price.m.yiche.com/zuidijia/nc{0}/?leads_source=H001013\" class=\"btu\" data-action=\"popup-share\" data-channelid=\"85.99.932\">询价</a>", carEntity1.Id);
            sb.Append("</div>");
            sbBottom.Append("</div>");
            sb.Append("<div class=\"center\"><i></i></div>");
            sbBottom.Append("<div class=\"center\"><i></i></div>");
            sb.Append("<div class=\"right\">");
            sbBottom.Append("<div class=\"right\">");
            sb.Append("<a href=\"###\">");
            sbBottom.Append("<a href=\"###\">");
            sb.AppendFormat("<a id='imgright' data-channelid='85.99.1362' href=\"/{1}/\"><img src=\"{0}\" /></a>", csPic2.Replace("_2.", "_6."), carEntity2.Serial.AllSpell);
            sbBottom.AppendFormat("<img src=\"{0}\" />", csPic2.Replace("_2.", "_6."));
            sb.Append("<div class=\"content\">");
            sbBottom.Append("<div class=\"content\">");
            sb.AppendFormat("<h6>{0}</h6>", carEntity2.Serial.ShowName);
            sbBottom.AppendFormat("<h6>{0}</h6>", carEntity2.Serial.ShowName);
            sb.AppendFormat("<p>{0}</p>", (carEntity2.CarYear > 0 ? carEntity2.CarYear + "款 " : "") + carEntity2.Name);
            sbBottom.AppendFormat("<p>{0}</p>", (carEntity2.CarYear > 0 ? carEntity2.CarYear + "款 " : "") + carEntity2.Name);
            if (!isTuijianFirst)
            {
                sb.Append("<i class=\"stamp\"></i>");
                sbBottom.Append("<i class=\"stamp\"></i>");
            }
            sb.Append("</div>");
            sbBottom.Append("</div>");
            sb.Append("</a>");
            sbBottom.Append("</a>");
            sb.AppendFormat("<a href=\"javaScript:;\" class=\"more\"  data-action=\"changecar\" data-index=\"2\" carid=\"{0}\" csid=\"{1}\" class=\"more\">其它车型>></a>", carEntity2.Id, carEntity2.SerialId);
            //sbBottom.AppendFormat("<a href=\"http://price.m.yiche.com/zuidijia/nc{0}/?leads_source=H001013\" class=\"btu\" data-action=\"popup-share\" data-channelid=\"85.99.932\">询价</a>", carEntity2.Id);
            sb.Append("</div>");
            sbBottom.Append("</div>");
            sb.Append("</div>");
            sbBottom.Append("</div>");
            sb.Append("</div>");
            sbBottom.Append("</div>");
            TitleHtml = sb.ToString();
            //BottomTitleHtml = sbBottom.ToString();
        }

        private bool GetTuiJianFirst(Dictionary<int, Dictionary<string, string>> dic)
        {
            double tuiJianPoint1 = 0;
            double tuiJianPoint2 = 0;
            if (baoZhilv1 > 0 && baoZhilv2 > 0)
            {
                tuiJianPoint1 = baoZhilv1 / (baoZhilv1 + baoZhilv2) * 0.35;
                tuiJianPoint2 = baoZhilv2 / (baoZhilv1 + baoZhilv2) * 0.35;
            }
            if (carEntity1.CarPV > 0 && carEntity2.CarPV > 0)
            {
                tuiJianPoint1 += carEntity1.CarPV / (carEntity1.CarPV + carEntity2.CarPV) * 0.15;
                tuiJianPoint2 += carEntity2.CarPV / (carEntity1.CarPV + carEntity2.CarPV) * 0.15;
            }
            double koubei1 = ConvertHelper.GetDouble(dic[carEntity1.Id]["Koubei"]);
            double koubei2 = ConvertHelper.GetDouble(dic[carEntity2.Id]["Koubei"]);
            if (koubei1 > 0 && koubei2 > 0)
            {
                tuiJianPoint1 += koubei1 / (koubei1 + koubei2) * 0.1;
                tuiJianPoint2 += koubei2 / (koubei1 + koubei2) * 0.1;
            }
            if (allPrice1 > 0 && allPrice2 > 0)
            {
                tuiJianPoint1 += 1 - allPrice1 / (allPrice1 + allPrice2) * 0.15;
                tuiJianPoint2 += 1 - allPrice2 / (allPrice1 + allPrice2) * 0.15;
            }
            Dictionary<int, int> dicPv = Car_SerialBll.GetAllSerialUVDict();
            if (dicPv[carEntity1.Serial.Id] > 0 && dicPv[carEntity2.Serial.Id] > 0)
            {
                tuiJianPoint1 += dicPv[carEntity1.Serial.Id] / (dicPv[carEntity1.Serial.Id] + dicPv[carEntity2.Serial.Id]) * 0.25;
                tuiJianPoint2 += dicPv[carEntity2.Serial.Id] / (dicPv[carEntity1.Serial.Id] + dicPv[carEntity2.Serial.Id]) * 0.25;
            }
            if (tuiJianPoint1 > tuiJianPoint2 || (tuiJianPoint1 == tuiJianPoint2 && carEntity1.Id > carEntity2.Id))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParams()
        {
            string CarIds = Request["carids"];
            if (string.IsNullOrWhiteSpace(CarIds)) Response.Redirect("/addchexingduibi/");
            string[] CarIdArray = CarIds.Split(',');

            if (CarIdArray.Length < 2) Response.Redirect("/addchexingduibi/?carid=" + CarIds);
            CarId1 = ConvertHelper.GetInteger(CarIdArray[0]);
            CarId2 = ConvertHelper.GetInteger(CarIdArray[1]);

            carEntity1 = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarId1);
            if (carEntity1 == null)
            {
                Response.Redirect("/addchexingduibi/?carid=" + (carEntity2 == null ? string.Empty : carEntity2.Id.ToString()));
            }
            carEntity2 = (CarEntity)DataManager.GetDataEntity(EntityType.Car, CarId2);

            if (carEntity2 == null)
            {
                Response.Redirect("/addchexingduibi/?carid=" + carEntity1.Id);
            }
        }
    }
}