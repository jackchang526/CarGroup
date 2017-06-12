using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI.MobileControls.Adapters;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
    /// <summary>
    /// GetCarHtml 的摘要说明
    /// </summary>
    public class GetCarHtml : PageBase, IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        private string dept = "";
        int pageIndex = 1;
        int allSerialCount;
        private StringBuilder sb = new StringBuilder();
        protected string serialWhiteImageUrl = string.Empty;
        DataSet brandDs;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Charset = "utf-8";

            PageHelper.SetPageCache(60);
            response = context.Response;
            request = context.Request;
            if (!string.IsNullOrEmpty(request.QueryString["dept"]))
            { dept = request.QueryString["dept"].ToString().Trim().ToLower(); }

            switch (dept)
            {
                case "getcarlistforbaidu":
                    {
                        RenderCarListForBaidu();
                        break;
                    }
                case "getnewcarlistforbaidu":
                    {
                        RenderNewCarListForBaidu();
                        break;
                    }
                case "getseriallistforbaidu":
                    {
                        RenderSerialListForBaidu();
                        break;
                    }
                case "getmobileseriallistforbaidu":
                    {
                        RenderMobileSerialListForBaidu();
                        break;
                    }
                case "getmobilecarlistforbaidu":
                    {
                        RenderMobileCarListForBaidu();
                        break;
                    }
                default: break;
            }
            response.Write(sb.ToString());
        }

        private void RenderCarListForBaidu()
        {
            int csid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["csid"]))
            {
                if (int.TryParse(request.QueryString["csid"].ToString(), out csid))
                { }
            }
            if (csid > 0)
            {

                Car_SerialBll csBll = new Car_SerialBll();
                Car_SerialEntity csObj = csBll.Get_Car_SerialByCsID(csid);
                if (csObj == null)
                { return; }

                Car_BasicBll carBLL = new Car_BasicBll();
                List<CarInfoForSerialSummaryEntity> carinfoList = carBLL.GetCarInfoForSerialSummaryBySerialId(csid);

                Dictionary<int, string> dic724 = carBLL.GetCarParamExDic(724);

                List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
                .FindAll(p => p.SaleState == "在销");

                sb.AppendLine("<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<title>车款列表</title>");
                sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://image.bitautoimg.com/carchannel/Cooperation/css/baidutemp20140924.css\" media=\"all\" />");
                sb.AppendLine("</head><body>");
                sb.AppendLine("<div style=\"width:790px; clear:both;\">");
                sb.AppendLine(string.Format("<div class=\"head\"><h3><span>{0}在售车款</span> (共{1}款)</h3> <a target=\"_blank\" href=\"http://car.bitauto.com/{2}/?WT.mc_id=bdbkifr\">上易车查看更多车款信息&gt;</a></div>"
                    , csObj.Cs_ShowName
                    , carinfoSaleList.Count
                    , csObj.Cs_AllSpell));
                sb.AppendLine("<div class=\"c-list-2014\">");
                // StringBuilder sbTempCarList = new StringBuilder();
                if (carinfoSaleList.Count > 0)
                {
                    sb.AppendLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<th width=\"46%\" class=\"first-item\">车型名称</th>");
                    sb.AppendLine("<th width=\"7%\" class=\"pd-left-one\">排量</th>");
                    sb.AppendLine("<th width=\"15%\" class=\"pd-left-one\">变速箱</th>");
                    sb.AppendLine("<th width=\"12%\" class=\"pd-left-one\" style=\"text-align:right;\">厂商指导价</th>");
                    sb.AppendLine("<th width=\"20%\" class=\"pd-left-three\" style=\"text-align:right;\"><span class=\"pd-left-three\" style=\"text-align:right;\"><span class=\"pd-left-three\" style=\"text-align:center;\">参考最低价&nbsp;&nbsp;</span></span></th>");
                    sb.AppendLine("</tr>");

                    carinfoSaleList.Sort(NodeCompare.CompareCarByYear);
                    int loop = 1;
                    foreach (CarInfoForSerialSummaryEntity carinfo in carinfoSaleList)
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine(string.Format("<td class=\"first-item\"><a href=\"http://car.bitauto.com/{1}/m{2}/?WT.mc_id=bdbkifr\" target=\"_blank\">{0}</a></td>"
                            , (carinfo.CarYear != "" ? carinfo.CarYear + "款 " : "") + carinfo.CarName
                            , csObj.Cs_AllSpell
                            , carinfo.CarID));
                        sb.AppendLine(string.Format("<td style=\"text-align:left;\">{0}</td>", carinfo.Engine_Exhaust));
                        sb.AppendLine(string.Format("<td style=\"text-align:left;\">{0}{1}</td>"
                            , (dic724.ContainsKey(carinfo.CarID) && dic724[carinfo.CarID] != "无级" && dic724[carinfo.CarID] != "待查") ? dic724[carinfo.CarID] + "挡" : ""
                            , carinfo.TransmissionType));
                        sb.AppendLine(string.Format("<td style=\"text-align:right;\">{0}万</td>", carinfo.ReferPrice));
                        sb.AppendLine(string.Format("<td style=\"text-align:right;\"><div class=\"car-summary-btn-xunjia button_blue\"><a href=\"http://dealer.bitauto.com/zuidijia/nb{1}/nc{2}/?T=2&leads_source=20009&WT.mc_id=bdbkifr\" target=\"_blank\">询价</a></div><strong>{0}</strong></td>"
                            , ((carinfo.CarPriceRange.Length != 0 && carinfo.CarPriceRange.IndexOf("-") > 0) ? carinfo.CarPriceRange.Substring(0, carinfo.CarPriceRange.IndexOf('-')) : "暂无")
                            , csObj.Cs_Id
                            , carinfo.CarID));
                        sb.AppendLine("</tr>");
                        if (loop >= 8)
                        { break; }
                        loop++;
                    }

                    sb.AppendLine("</table>");
                    // int carCount = carinfoSaleList.Count;
                }
                sb.AppendLine("</div></div>");
                sb.AppendLine("</body></html>");
            }
        }
        /// <summary>
        /// 百度百科子品牌iframe
        /// </summary>
        private void RenderNewCarListForBaidu()
        {
            int csid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["csid"]))
            {
                if (int.TryParse(request.QueryString["csid"].ToString(), out csid))
                { }
            }
            if (csid > 0)
            {

                Car_SerialBll csBll = new Car_SerialBll();
                Car_SerialEntity csObj = csBll.Get_Car_SerialByCsID(csid);
                if (csObj == null)
                { return; }

                Car_BasicBll carBLL = new Car_BasicBll();
                List<CarInfoForSerialSummaryEntity> carinfoList = carBLL.GetCarInfoForSerialSummaryBySerialId(csid);

                Dictionary<int, string> dic724 = carBLL.GetCarParamExDic(724);
                serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(csid).Replace("_2.", "_3.");
                List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
                .FindAll(p => p.SaleState == "在销").OrderByDescending(p => p.CarPV).ToList();

                sb.AppendLine("<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<title>百度百科-词条页</title>");
                sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://image.bitautoimg.com/carchannel/Cooperation/css/citiao20141112.css\" media=\"all\" />");
                sb.AppendLine("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js\"></script>");
                sb.AppendLine("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/apigetcarhtml.js?v=20141111\"></script>");
                sb.AppendLine("</head><body>");
                sb.AppendLine("<div class=\"iframe_con\">");
                sb.AppendLine(string.Format("<div class=\"com_tit\">{0}在售车型<span class=\"gray\">(共{1}款)</span><div class=\"more\"><a href=\"http://car.bitauto.com/{2}/?WT.mc_id=bdbkifr\" target=\"_blank\">上易车查看更多车款信息&gt;</a></div></div>"
                    , csObj.Cs_ShowName
                    , carinfoSaleList.Count
                    , csObj.Cs_AllSpell));

                sb.AppendLine("<div class=\"cont_box\">");

                #region 焦点图 大图 颜色
                //子品牌焦点图
                List<SerialFocusImage> imgList = csBll.GetSerialFocusImageList(csid);
                //在售车款颜色图
                List<SerialColorEntity> serialColorList = csBll.GetProduceSerialColors(csid);
                List<SerialColorForSummaryEntity> colorList = csBll.GetSerialColorRGBByCsID(csid, 0, serialColorList);
                //排序 有图在前 无图在后 颜色 按色值大小从大到小排序
                colorList.Sort(NodeCompare.SerialColorCompare);

                List<string> smallImages = new List<string>();
                sb.AppendLine("<div class=\"focus_box\">");
                List<string> bigImage = new List<string>();
                //大图 默认第一张 焦点图第一张 没有焦点图 白底图
                if (imgList.Count > 0)
                {
                    SerialFocusImage csImg = imgList[0];
                    string firstFocusImage = csImg.ImageUrl;
                    if (csImg.ImageId > 0)
                    {
                        firstFocusImage = String.Format(firstFocusImage, 4);
                    }

                    bigImage.Insert(0, string.Format("<div class=\"img_box\" id=\"focus_image_first\"><a href=\"http://car.bitauto.com/{0}/?WT.mc_id=bdbkifr\" target=\"_blank\"><img src=\"{1}\"/></a></div>",
                        csObj.Cs_AllSpell,
                        firstFocusImage));
                }
                else
                {
                    bigImage.Insert(0, string.Format("<div class=\"img_box\" id=\"focus_image_first\"><a href=\"http://car.bitauto.com/{1}/?WT.mc_id=bdbkifr\" target=\"_blank\"><img src=\"{0}\"/></a></div>",
                        serialWhiteImageUrl.Replace("_3.", "_6."),
                        csObj.Cs_AllSpell));
                }
                if (colorList.Count > 0)
                {
                    var firstFocusImageId = 0;
                    if (imgList != null && imgList.Count > 0) firstFocusImageId = imgList[0].ImageId;
                    //颜色补充实拍图
                    string serialColorPath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialColorImagePath, csid));
                    XmlDocument xmlSerialColor = CommonFunction.ReadXmlFromFile(serialColorPath);
                    //IsGuanfangPic = colorList.Find(p => !string.IsNullOrEmpty(p.ImageUrl)) != null;
                    int loop = 0;
                    foreach (SerialColorForSummaryEntity color in colorList)
                    {
                        if (loop >= 8)
                        {
                            break;
                        }
                        string imageUrl = color.ImageUrl.Replace("_5.", "_4.");
                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            XmlNode colorNode = xmlSerialColor.SelectSingleNode("/CarImageList/CarImage[@ColorName='" + color.ColorName + "']");
                            if (colorNode != null)
                            {
                                imageUrl = string.Format(colorNode.Attributes["ImageUrl"].Value, 4);
                                color.Link = colorNode.Attributes["Link"].Value;
                            }
                        }
                        if (string.IsNullOrEmpty(imageUrl))
                            continue;
                        loop++;
                        bigImage.Add(string.Format("<div id=\"focuscolor_{3}\" style=\"display:{2}\" class=\"img_box\"><a href=\"http://car.bitauto.com/{0}/?WT.mc_id=bdbkifr\" target=\"_blank\"><img src=\"{1}\"/></a></div>",
                            csObj.Cs_AllSpell,
                            imageUrl,
                             "none", loop));
                        smallImages.Add(string.Format("<a href=\"http://car.bitauto.com/{0}/?WT.mc_id=bdbkifr\" title=\"{2}\" target=\"_blank\" style=\"background:{1}\"></a>",
                            csObj.Cs_AllSpell, color.ColorRGB, color.ColorName));
                    }
                }

                sb.Append(string.Concat(bigImage.ToArray()));
                sb.Append("<div class=\"focus_warp\" id=\"focus_color_box\">");
                sb.Append(string.Concat(smallImages.ToArray()));
                sb.Append("</div>");
                sb.Append("</div>");

                #endregion

                sb.AppendLine(" <div class=\"zaishou\">");
                if (carinfoSaleList.Count > 0)
                {
                    sb.AppendLine("<table border=\"0\">");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<th width=\"220\" scope=\"col\">车款名称</th>");
                    sb.AppendLine("<th width=\"38\" scope=\"col\">排量</th>");
                    sb.AppendLine("<th width=\"110\" scope=\"col\">变速箱</th>");
                    sb.AppendLine("<th width=\"112\" class=\"t_center\" scope=\"col\">参考最低价</th>");
                    sb.AppendLine("</tr>");

                    int loop = 1;
                    foreach (CarInfoForSerialSummaryEntity carinfo in carinfoSaleList)
                    {
                        string name = (carinfo.CarYear != "" ? carinfo.CarYear + "款 " : "") + carinfo.CarName;
                        if (name.Length > 24)
                        {
                            name = name.Substring(0, 24);
                        }
                        sb.AppendLine(string.Format("<tr class= carlist_{0} style=\"display:{1}\">", loop / 4 + (loop % 4 == 0 ? 0 : 1), "none"));
                        sb.AppendLine(string.Format("<td><a href=\"http://car.bitauto.com/{1}/m{2}/?WT.mc_id=bdbkifr\" target=\"_blank\">{0}</a></td>"
                            , name
                            , csObj.Cs_AllSpell
                            , carinfo.CarID));
                        sb.AppendLine(string.Format("<td>{0}</td>", carinfo.Engine_Exhaust));
                        sb.AppendLine(string.Format("<td>{0}{1}</td>"
                            , (dic724.ContainsKey(carinfo.CarID) && dic724[carinfo.CarID] != "无级" && dic724[carinfo.CarID] != "待查") ? dic724[carinfo.CarID] + "挡" : ""
                            , carinfo.TransmissionType));
                        sb.AppendLine(string.Format("<td class=\"t_right\">{0} <a href=\"http://dealer.bitauto.com/zuidijia/nb{1}/nc{2}/?T=2&leads_source=20009&WT.mc_id=bdbkifr\" target=\"_blank\" class=\"bg_btn\">询价</a></td>"
                            , ((carinfo.CarPriceRange.Length != 0 && carinfo.CarPriceRange.IndexOf("-") > 0) ? carinfo.CarPriceRange.Substring(0, carinfo.CarPriceRange.IndexOf('-')) : "暂无")
                            , csObj.Cs_Id
                            , carinfo.CarID));
                        sb.AppendLine("</tr>");
                        loop++;
                    }
                    sb.AppendLine("</table>");
                    int carCount = carinfoSaleList.Count;
                    int pageCount = carCount / 4 + (carCount % 4 == 0 ? 0 : 1);
                    sb.AppendLine(string.Format("<div class=\"page_box\"><a id= \"pageIndex\">{0}/</a><a>/</a><a id= \"pageCount\">{1}</a>", pageIndex, pageCount));
                    if (carinfoSaleList.Count > 4)
                    {
                        sb.AppendLine("<a class=\"left\" href=\"#\"></a><a class=\"right\" href=\"#\"></a>");
                    }
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div></div></div>");
                sb.AppendLine("</body></html>");
            }
        }

        /// <summary>
        /// 百度百科品牌iframe
        /// </summary>
        private void RenderSerialListForBaidu()
        {
            int masterid = 0;
            string _masterName = string.Empty;
            if (!string.IsNullOrEmpty(request.QueryString["bsid"]))
            {
                if (int.TryParse(request.QueryString["bsid"].ToString(), out masterid))
                { }
            }
            if (masterid > 0)
            {
                brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(masterid, false);
                sb.AppendLine("<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<title>百度百科-词条页</title>");
                sb.AppendLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://image.bitautoimg.com/carchannel/Cooperation/css/citiao20141112.css\" media=\"all\" />");
                sb.AppendLine("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js\"></script>");
                sb.AppendLine("<script type=\"text/javascript\" src=\"http://image.bitautoimg.com/carchannel/jsnew/apigetcarhtml.js?v=20141111\"></script>");
                sb.AppendLine("</head><body>");
                sb.AppendLine("<div class=\"iframe_con\">");
                DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(masterid);
                string _masterSpell = string.Empty;
                if (drInfo != null)
                {
                    _masterName = drInfo["bs_name"].ToString().Trim();
                    _masterSpell = drInfo["urlspell"].ToString().Trim();
                }
                sb.AppendLine(string.Format("<div class=\"com_tit\">{0}-车型</span><div class=\"more\"><a href=\"http://car.bitauto.com/{1}/?WT.mc_id=bdbkifr\" target=\"_blank\">上易车查看更多车型信息&gt;</a></div></div>", _masterName, _masterSpell));
                RenderBrandList();
                sb.AppendLine("<div class=\"cont_focus\">");
                sb.AppendLine("<div class=\"cont_img_box\">");
                sb.Append("<ul id = \"serialList_0\" class =\"serialList\">");
                allSerialCount = 0;
                if (brandDs != null && brandDs.Tables.Count > 0)
                {
                    foreach (DataTable brandTable in brandDs.Tables)
                    {
                        sb.Append(GetSerialHtml(brandTable, allSerialCount));
                    }
                }
                if (allSerialCount > 4)
                {
                    sb.Append("<a class=\"left_btn\" href=\"#\">上一张</a><a class=\"right_btn\" href=\"#\">下一张</a>");
                }
                int pageCount = allSerialCount / 4 + (allSerialCount % 4 == 0 ? 0 : 1);
                sb.Append(string.Format("<span class=\"span_r\" style=\"display:{2}\"><a id= \"pageIndex\">{0}</a><a>/</a><a id= \"pageCount\">{1}</a></span>", pageIndex, pageCount, "none"));
                sb.Append("</ul>");
                if (brandDs != null && brandDs.Tables.Count > 0)
                {
                    allSerialCount = 0;
                    int loopTemp = 1;
                    foreach (DataTable brandTable in brandDs.Tables)
                    {
                        sb.Append(string.Format("<ul id= serialList_{0} class =\"serialList\">", loopTemp));
                        sb.Append(GetSerialHtml(brandTable, 0));
                        if (allSerialCount > 4)
                        {
                            sb.Append("<a class=\"left_btn\" href=\"#\">上一张</a><a class=\"right_btn\" href=\"#\">下一张</a>");
                        }
                        sb.Append(string.Format("<span class=\"span_r\" style=\"display:{2}\"><a id= \"pageIndex\">{0}</a><a>/</a><a id= \"pageCount\">{1}</a></span>", pageIndex, allSerialCount / 4 + (allSerialCount % 4 == 0 ? 0 : 1), "none"));
                        sb.Append("</ul>");
                        loopTemp++;
                    }
                }
                sb.AppendLine("</div></div></div>");
                sb.AppendLine("</body></html>");
            }
        }

        /// <summary>
        /// 生成主品牌下各品牌的子品牌列表
        /// </summary>
        private void RenderBrandList()
        {
            if (brandDs != null && brandDs.Tables.Count > 0)
            {
                int brandCount = 0;
                sb.AppendLine("<div class=\"cont_box\">");
                sb.AppendLine("<div class=\"tabs_box\">");
                sb.AppendLine("<div class=\"tab\">");
                sb.AppendLine(string.Format("<a id=\"brand_{0}\" class=\"bg_btn hover\" href=\"#\">全部</a>", brandCount));
                foreach (DataTable brandTable in brandDs.Tables)
                {
                    if (brandTable.Rows.Count == 0)
                    {
                        continue;
                    }
                    brandCount++;
                    sb.AppendFormat("|<a id= brand_{1} href=\"#\">{0}</a>", brandTable.TableName, brandCount);
                }
                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
            }
        }

        private string GetSerialHtml(DataTable brandTable, int loop)
        {
            StringBuilder serialList = new StringBuilder();
            foreach (DataRow row in brandTable.Rows)
            {
                #region 不显示的子品牌
                string csLevel = ConvertHelper.GetString(row["cslevel"]);
                if (csLevel == "概念车")
                    continue;
                string csName = ConvertHelper.GetString(row["cs_name"]);
                if (csName.IndexOf("停用") >= 0)
                { continue; }
                string sellState = ConvertHelper.GetString(row["CsSaleState"]);
                string imgUrl = ConvertHelper.GetString(row["csImageUrl"]).ToLower();
                // 无图片的
                if (sellState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                { continue; }
                #endregion

                int serialId = ConvertHelper.GetInteger(row["cs_id"]);
                string csShowName = ConvertHelper.GetString(row["cs_ShowName"]);
                if (serialId == 1568)
                { csShowName = "索纳塔八"; }
                string csSpell = ConvertHelper.GetString(row["csspell"]).Trim().ToLower();
                string priceRange = sellState;
                string serialUrl = "/" + csSpell + "/";
                if (sellState == "在销")
                {
                    priceRange = base.GetSerialPriceRangeByID(serialId);
                    if (priceRange.Trim().Length == 0)
                    {
                        priceRange = "暂无报价";
                    }
                    loop++;
                    serialList.AppendFormat("<li class= seiallist_{5} style=\"display:{6}\"><a href=\"http://car.bitauto.com/{0}/?WT.mc_id=bdbkifr\" target=\"_blank\"><img src=\"{1}\"/></a><p class=\"car_name\"><a href=\"http://car.bitauto.com/{0}/?WT.mc_id=bdbkifr\" target=\"_blank\">{2}</a></p> <p class=\"car_jiage\"><span>{3}</span> <a href=\"http://dealer.bitauto.com/zuidijia/nb{4}/?WT.mc_id=bdbkifr\" class=\"bg_btn\" target=\"_blank\">询价</a></p></li>", csSpell, imgUrl, csShowName, priceRange, serialId, loop / 4 + (loop % 4 == 0 ? 0 : 1), "none");
                }

            }
            allSerialCount = loop;
            return serialList.ToString();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region  百度移动端html

        private string LoadCssStr()
        {
            const string cacheNameStr = "MobileForBaiduCss_Cache";
            var cssStr = CacheManager.GetCachedData(cacheNameStr);
            if (cssStr != null)
            {
                return cssStr.ToString();
            }
            var str = string.Empty;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\css\\GetCarhtml\\main.css");
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            var sr = new StreamReader(path);
            try
            {
                str = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                CommonFunction.WriteLog(e.ToString());
            }
            finally
            {
                sr.Close();
            }
            CacheManager.InsertCache(cacheNameStr, str, WebConfig.CachedDuration);
            return str;
        }


        private void RenderMobileSerialListForBaidu()
        {
            int masterid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["bsid"]))
            {
                if (int.TryParse(request.QueryString["bsid"], out masterid))
                { }
            }
            if (masterid > 0)
            {
                var masterRow = new Car_BrandBll().GetCarMasterBrandInfoByBSID(masterid);
                if (masterRow == null) return;
                var masterSpell = masterRow["urlspell"];
                brandDs = new Car_BrandBll().GetCarSerialPhotoListByBSID(masterid, false);
                sb.AppendLine("<style type=\"text/css\">");
                sb.AppendLine(LoadCssStr());
                sb.AppendLine("</style>");
                //sb.AppendLine("<script type=\"text/javascript\" src=\"script/jquery-1.10.1.min.js\"></script>");
                //sb.AppendLine("<script type=\"text/javascript\" src=\"script/GetCarhtml/renderMobileSerialListForBaidu.js\"></script>");
                sb.AppendLine("<div id=\"titWrap\" class=\"car-container\">");
                sb.AppendLine("<div class=\"tit-box clearfix\">");
                sb.AppendFormat("<h3>在售车型</h3><em id=\"em_serialcount\"></em>");
                if (brandDs.Tables.Count > 1)
                {
                    sb.AppendLine("<a href=\"javascript:\" class=\"car-dress\" id=\"btn_shaixuan\">筛选</a>");
                }
                sb.AppendLine("<div class=\"car-list-box\" id=\"div_brandList\" >");
                sb.AppendLine("<ul>");
                sb.AppendFormat("<li class=\"current\" brand=\"全部\" spell=\"{0}\">", masterSpell);
                sb.AppendLine("<span class=\"car-name\">全部</span>");
                sb.AppendLine("<i class=\"car-type\"></i>");
                sb.AppendLine("</li>");
                foreach (DataTable dt in brandDs.Tables)
                {
                    var brandName = dt.TableName;
                    var brandSpell = dt.Rows[0]["cbspell"];
                    sb.AppendFormat("<li brand=\"{0}\" spell=\"{1}\">", brandName, brandSpell);
                    sb.AppendFormat("<span class=\"car-name\" >{0}</span>", brandName);
                    sb.AppendLine("<i class=\"car-type\"></i>");
                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ul></div></div>");
                sb.AppendLine("<div id=\"div_seriallist\" class=\"car-list-box\">");
                sb.AppendLine("<ul>");
                var serialcount = 0;
                foreach (DataTable dt in brandDs.Tables)
                {
                    var brandName = dt.TableName;
                    foreach (DataRow dr in dt.Rows)
                    {
                        var csid = dr["cs_id"];
                        var csseoname = dr["cs_seoname"];
                        var csspell = dr["csspell"];
                        var csimg = Car_SerialBll.GetSerialImageUrl(Int32.Parse(csid.ToString()), "3");
                        if (csimg.Equals(WebConfig.DefaultCarPic))
                        {
                            continue;
                        }
                        var cspricerange = GetSerialPriceRangeByID(Int32.Parse(csid.ToString()));
                        if (string.IsNullOrEmpty(cspricerange))
                        {
                            cspricerange = "暂无报价";
                        }
                        serialcount++;
                        var isShow = serialcount > 4 ? "style=\"display:none\"" : "";//默认显示4条
                        sb.AppendFormat("<li brand=\"{0}\" {1}>", brandName, isShow);
                        sb.AppendFormat("<a href=\"{0}\" class=\"ck-link\">", string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk", csspell));
                        sb.AppendFormat("<div class=\"img-box\"><img src=\"{0}\" alt=\"\" ></div>", csimg);
                        sb.AppendLine("<div class=\"txt-box\">");
                        sb.AppendFormat("<span class=\"p-name\">{0}</span>", csseoname);
                        sb.AppendFormat("<span class=\"p-price\">{0}</span>", cspricerange);
                        sb.AppendLine("</div></a>");
                        sb.AppendFormat("<a href=\"http://price.m.yiche.com/zuidijia/nb{0}/?WT.mc_id=mbdbk\" class=\"xj-btn\">询价</a>", csid);
                        sb.AppendLine("</li>");
                    }
                }
                sb.AppendLine("</ul></div>");
                sb.AppendLine("</div>");
                if (serialcount > 4)
                {
                    sb.AppendLine("<a id=\"a_more4\" href=\"javascript:\" class=\"more-list\">点击加载更多车型<i class=\"more-message\"></i></a>");
                    sb.AppendFormat("<a id=\"a_moreserials\" style=\"display:none\" href=\"http://car.m.yiche.com/brandlist/{0}/?WT.mc_id=mbdbk\" class=\"more-list\">上易车网查看更多车型信息</a>"
                        , masterSpell);
                }
                else
                {
                    sb.AppendFormat("<a id=\"a_moreserials\" href=\"http://car.m.yiche.com/brandlist/{0}/?WT.mc_id=mbdbk\" class=\"more-list\">上易车网查看更多车型信息</a>"
                    , masterSpell);
                }
            }
        }
        private void RenderMobileCarListForBaidu()
        {
            var htmlStr = new StringBuilder();
            var cssStr = new StringBuilder();
            int csid = 0;
            if (!string.IsNullOrEmpty(request.QueryString["csid"]))
            {
                if (int.TryParse(request.QueryString["csid"], out csid))
                { }
            }
            if (csid > 0)
            {
                var csBll = new Car_SerialBll();
                Car_SerialEntity csObj = csBll.Get_Car_SerialByCsID(csid);
                if (csObj == null)
                { return; }
                
                var carBll = new Car_BasicBll();
                Dictionary<int, string> dic724 = carBll.GetCarParamExDic(724);
                List<CarInfoForSerialSummaryEntity> carinfoList = carBll.GetCarInfoForSerialSummaryBySerialId(csid);
                serialWhiteImageUrl = Car_SerialBll.GetSerialImageUrl(csid).Replace("_2.", "_3.");
                List<CarInfoForSerialSummaryEntity> carinfoSaleList = carinfoList
                .FindAll(p => p.SaleState == "在销").OrderByDescending(p => p.CarPV).ToList();

                cssStr.AppendLine("<style type=\"text/css\">");
                cssStr.AppendLine(LoadCssStr());
                htmlStr.AppendLine("<div class=\"car-container\">");
                htmlStr.AppendLine("<div class=\"tit-box clearfix\" id=\"titWrap\">");
                htmlStr.AppendFormat("<h3>{0}</h3><em>{1}款</em>", csObj.Cs_SeoName, carinfoSaleList.Count);
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("<!--焦点图 start-->");
                htmlStr.AppendLine("<div class=\"focus-img-box\">");
                htmlStr.AppendLine("<div class=\"img-box\">");
                htmlStr.AppendLine("<div class=\"m-focus\">");
                htmlStr.AppendLine("<div class=\"m-focus-box\">");
                htmlStr.AppendLine("<div class=\"m-focus-scroll-box swiper-container\" id=\"m-focus-box\">");
                htmlStr.AppendLine("<ul class=\"swiper-wrapper\">");
                #region 焦点图 大图 颜色
                //子品牌焦点图
                List<SerialFocusImage> imgList = csBll.GetSerialFocusImageList(csid);
                //在售车款颜色图
                List<SerialColorEntity> serialColorList = csBll.GetProduceSerialColors(csid);
                List<SerialColorForSummaryEntity> colorList = csBll.GetSerialColorRGBByCsID(csid, 0, serialColorList);
                //排序 有图在前 无图在后 颜色 按色值大小从大到小排序
                colorList.Sort(NodeCompare.SerialColorCompare);
                int loop = 0;
                if (colorList.Count > 0)
                {
                    string serialColorPath = Path.Combine(PhotoImageConfig.SavePath, string.Format(PhotoImageConfig.SerialColorImagePath, csid));
                    XmlDocument xmlSerialColor = CommonFunction.ReadXmlFromFile(serialColorPath);
                    
                    foreach (SerialColorForSummaryEntity color in colorList)
                    {
                        if (loop >= 6)
                        {
                            break;
                        }
                        string imageUrl = color.ImageUrl.Replace("_5.", "_4.");
                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            XmlNode colorNode = xmlSerialColor.SelectSingleNode("/CarImageList/CarImage[@ColorName='" + color.ColorName + "']");
                            if (colorNode != null)
                            {
                                imageUrl = string.Format(colorNode.Attributes["ImageUrl"].Value, 4);
                                color.Link = colorNode.Attributes["Link"].Value;
                            }
                        }
                        if (string.IsNullOrEmpty(imageUrl))
                            continue;
                        loop++;
                        htmlStr.AppendFormat("<li class=\"swiper-slide\"><a href=\"{0}\"><img src=\"{1}\" ></a></li>",
                           string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk", csObj.Cs_AllSpell),
                            imageUrl
                            );
                        cssStr.AppendFormat(".m-focus-box span:nth-child({0}){1}", loop, "{ background: " + color.ColorRGB + "}");
                    }
                }
                if (loop==0)
                {
                    //没有颜色图 用焦点图 白底图
                    if (imgList.Count > 0)
                    {
                        SerialFocusImage csImg = imgList[0];
                        string firstFocusImage = csImg.ImageUrl;
                        if (csImg.ImageId > 0)
                        {
                            firstFocusImage = String.Format(firstFocusImage, 4);
                        }
                        htmlStr.AppendFormat("<li class=\"swiper-slide\"><a href=\"{0}\"><img src=\"{1}\" ></a></li>",
                           string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk", csObj.Cs_AllSpell),
                            firstFocusImage
                            );
                    }
                    else if (!string.IsNullOrEmpty(serialWhiteImageUrl))
                    {
                        htmlStr.AppendFormat("<li class=\"swiper-slide\"><a href=\"{0}\"><img src=\"{1}\" ></a></li>",
                            string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk", csObj.Cs_AllSpell),
                            serialWhiteImageUrl.Replace("_3.", "_4.")
                            );
                    }
                    else
                    {
                        //暂无图片
                        htmlStr.AppendFormat("<li class=\"swiper-slide\"><a href=\"{0}\"><img src=\"{1}\" ></a></li>",
                               string.Format("http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk", csObj.Cs_AllSpell),
                                WebConfig.DefaultCarPic
                                );
                    }
                }
                #endregion

                htmlStr.AppendLine("</ul>");
                htmlStr.AppendLine("<div class=\"pagination-focus\"></div>");
                htmlStr.AppendLine("<div class=\"clear\"></div>");
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("</div>");
                htmlStr.AppendLine("<!--焦点图 end-->");
                cssStr.Append(@".m-focus-box span.swiper-active-switch{ height: 9px; top: 3px}");
                cssStr.AppendLine("</style>");
                htmlStr.AppendLine("<div id=\"div_carlist\" class=\"car-list-box car-zpp\">");
                htmlStr.AppendLine("<ul>");
                var carCount = 0;
                if (carinfoSaleList.Count > 0)
                {
                    foreach (var carinfo in carinfoSaleList)
                    {
                        string name = (carinfo.CarYear != "" ? carinfo.CarYear.Substring(2) + "款 " : "") + carinfo.CarName;
                        if (carCount >= 6)
                        {
                            htmlStr.AppendLine("<li style=\"display:none\">");
                        }
                        else
                        {
                            htmlStr.AppendLine("<li>");
                        }
                        htmlStr.AppendLine("<div class=\"txt-box z-pp\">");
                        htmlStr.AppendFormat("<a href=\"http://price.m.yiche.com/nc{0}/?WT.mc_id=mbdbk\" class=\"xd-link\">"
                            ,carinfo.CarID);
                        htmlStr.AppendFormat("<div class=\"p-name\">{0}</div>", name);
                        htmlStr.AppendLine("<div class=\"p-price\">");
                        
                        htmlStr.AppendLine(string.Format("<span class=\"s1\">{2} {0}{1}</span>"
                            , (dic724.ContainsKey(carinfo.CarID) && dic724[carinfo.CarID] != "无级" && dic724[carinfo.CarID] != "待查") ? dic724[carinfo.CarID] + "挡" : ""
                            , carinfo.TransmissionType
                            , carinfo.Engine_Exhaust));

                        var price =((carinfo.CarPriceRange.Length != 0 && carinfo.CarPriceRange.IndexOf("-") > 0)
                            ? carinfo.CarPriceRange.Substring(0, carinfo.CarPriceRange.IndexOf('-'))+"起"
                            : "暂无");
                        htmlStr.AppendFormat("<span class=\"s2\">{0}</span>", price);
                        htmlStr.AppendLine("</div>");
                        htmlStr.AppendLine("</a>");
                        htmlStr.AppendFormat("<a href=\"http://price.m.yiche.com/zuidijia/nc{0}/?WT.mc_id=mbdbk\" class=\"xj-btn\">询价</a>"
                            , carinfo.CarID);
                        htmlStr.AppendLine("</div></li>");
                        carCount++;
                        if (carCount >= 12)
                        {
                            break;
                        }
                    }
                }
                htmlStr.AppendLine("</ul>");
                htmlStr.AppendLine("</div></div>");
                if (carCount > 6)
                {
                    htmlStr.AppendLine("<a id=\"a_more6\" href=\"javascript:\" class=\"more-list\">点击加载更多车型<i class=\"more-message\"></i></a>");
                    htmlStr.AppendFormat("<a id=\"a_morecar\" href=\"http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk\" style=\"display:none\" class=\"more-list\">上易车查看更多车型信息</a>", csObj.Cs_AllSpell);
                }
                else
                {
                    htmlStr.AppendFormat("<a id=\"a_morecar\" href=\"http://car.m.yiche.com/{0}/?WT.mc_id=mbdbk\" class=\"more-list\">上易车查看更多车型信息</a>", csObj.Cs_AllSpell);
                }
                //htmlStr.AppendLine("<script type=\"text/javascript\" src=\"script/jquery-1.10.1.min.js\"></script>");
                //htmlStr.AppendLine("<script type=\"text/javascript\" src=\"script/GetCarhtml/swiper.js\"></script>");
                //htmlStr.AppendLine("<script type=\"text/javascript\" src=\"script/GetCarhtml/main.js\"></script>");
                sb.Append(cssStr);
                sb.Append(htmlStr);
            }
        }

        #endregion
    }
}