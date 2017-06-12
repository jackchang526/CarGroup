<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummaryYear.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageYearV2.CsSummaryYear" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%= serialSeoName%>】<%= serialSeoName%>最新报价_<%= serialSeoName%>图片-易车网</title>
      <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->     <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />    <meta name="Keywords" content="<%= serialSeoName%>,<%= serialSeoName%>报价,<%= serialSeoName%>价格,<%= serialSeoName%>油耗,<%= serialSeoName%>图片,易车网,car.bitauto.com" />
    <meta name="Description" content="易车提供<%= serialSeoName%>最新报价,<%= serialSeoName%>图片, <%= serialSeoName%>油耗,查<%= serialSeoName%>最新价格,就上易车网" />
    <link rel="canonical" href="http://car.bitauto.com/<%=serialSpell %>/<%=carYear.ToString()%>/" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_chexingzongshu_style-1264.shtml"-->
    <style type="text/css">
        .zs_koubei_card { display: none; }
    </style>
</head>
<body> 
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <!--smenu start-->
    <!--a_d start-->
    <div style="width:1200px;margin:0 auto;">
       <%-- <%=serialTopAdCode%>--%>
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" data-type="ad_play" data-adplay_IP="" data-adplay_AreaName="" data-adplay_CityName="" data-adplay_BrandID="" data-adplay_BrandName="" data-adplay_BrandType="" data-adplay_BlockCode="7e48ab6a-f563-413a-8427-5578aa3416f9"> </ins>
    </div>
    <!--a_d end-->
    <%= CsHead %>
    <div class="container caryear-section summary">
        <script>
            function showCarList() { document.getElementById("this").style.display = "block"; document.getElementById("tit").style.color = "#000"; document.getElementById("tit").style.cursor = "default"; }
            function hideCarList() { document.getElementById("this").style.display = "none"; document.getElementById("tit").style.color = "#164A84"; document.getElementById("tit").style.cursor = "pointer"; }
            var tongJiEndUrlParam='&ref=car3&rfpa_tracker=1_8';
        </script>
        <!-- 两 栏之图片 -->
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="card-section">
                        <div class="card-layout special-card1">
                            <div class="content clearfix">
                                <div class="img">
                                    <!--默认实拍图-->
                                    <%= CsPicJiaodian.Replace("###",focusPhotoCountUrl) %>
                                    <!--颜色切换图-->
                                    <div class="color-img-list" style="display:none">
                                         <%=serialColorDisplayHtml%>
                                    </div>
                                    <a href="<%=focusPhotoCountUrl %>" class="img-link" target="_blank"><%=focusPhotoCount %>张实拍图&gt;&gt;</a>
                                    <div class="img-cor-box" data-channelid="2.21.798" id="focus_color_box">
                                        <div class="focus-color-box">
                                            <div class="focus-color-warp">
                                                <ul id="color-listbox" style="position: absolute; top: 0; left: 0px;">
                                                     <%=serialColorHtml%>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="desc">
                                    <div class="top">
                                        <%if (!isNoSaleYear)
                                          { %>
                                        <h5>参考成交价：<a href="/<%= serialSpell %>/baojia/" target="_blank"><em><%=minPriceRange %></em></a>
                                             <% if (taxContent != "" && cse.Cs_SaleState == "在销")
                                                { %>
                                                <a title="<%=taxContent %>" href="http://news.bitauto.com/sum/20170105/1406774416.html" target="_blank"  class="ico-shangchengtehui"><%= taxContent %></a> </h5>
                                                <% } %>
                                            <%}
                                          else
                                          { %>
                                            <h2 class="not-onsale">停售</h2>
                                                <%} %>
                                    </div>
                                    <div class="mid row">
                                        <div class="col-xs-4">
                                            <em>厂商指导价</em>
                                            <h5><%=referPrice%></h5>
                                        </div>
                                        <div class="col-xs-4">
                                            <em>本地最高降幅</em>
                                            <a href="/<%=serialSpell %>/jiangjia/" target="_blank"><h5 class="price-reduction-lg"></h5></a>
                                        </div>
                                        <div class="col-xs-4">
                                            <em>二手车</em>
                                            <h5><%if (string.IsNullOrEmpty(serialUCarPrice))
                                                  { %>
								                        暂无报价
								                    <%}
                                                  else
                                                  { %>
                                                        <a href="http://www.taoche.com/buycar/serial/<%=serialSpell %>/?ref=pc_yc_ckzs_gs_escjg" class="em" target="_blank">
                                                        <%=serialUCarPrice%></a>
                                                <%} %>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="foot">
                                        <div class="btn-group">
                                            <%if (minPriceRange == "停售")
                                              { %>
                                               <a class="btn" href="http://www.taoche.com/buycar/serial/<%=serialSpell %>/?ref=pc_yc_ckzs_gs_escbuycar" target="_blank">买二手车</a>
                                               <a class="btn" href="http://www.taoche.com/pinggu/?ref=pc_yc_ckzs_gs_gujia" target="_blank">二手车估价</a> 
                                            <%}
                                              else
                                              { %>
                                                <a id="cardXunjia" class="btn" href="http://dealer.bitauto.com/zuidijia/nb<%=serialId %>/?T=1&leads_source=20006" target="_blank">询底价</a>
                                                <a id="divDemandCsBut" class="btn" href="javascript:void(0);" style="display: none;">特卖</a>
                                                <%--1200改版 要求去掉 shijiaOrHuimaiche--%>
                                                <a href="<%=chedaiADLink %>" class="btn" target="_blank">贷款</a> 
                                                <a class="btn" id="btnZhihuan" target="_blank" href="http://maiche.taoche.com/zhihuan/?serial=<%=serialId %>&leads_source=20007&ref=pc_yc_ckzs_gs_zhihuan">置换</a>
                                            <%} %>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- 在销车型 -->
                    <div class="layout-1 caryear-inner-section">
                        <%=carListTableHtml%>
                    </div>
                    <!-- 经销商 -->
                    <script type="text/javascript">
                        document.writeln("<ins Id=\"ep_union_139\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\""+bit_locationInfo.cityId+"\" city_name=\"0\" car_type=\"2\" brandId=\"0\" serialId=\"<%= serialId %>\" carId=\"0\"></ins>");
                    </script>
                    <!-- 某车图片 -->
                    <div class="layout-2 yearpic-section">
                        <%=serialImageHtml%>
                    </div>
                    <!--二手车-->
                    <div class="layout-2 oldcar-section" id="ucarlist"></div>
                    <!-- SEO导航 -->
                    <div class="layout-1 aftersee-section">
                        <div class="section-header header2">
                            <div class="box">
                                <h2>接下来要看</h2>
                            </div>
                        </div>
                        <div class="row">
                            <div class="list-txt list-txt-m list-txt-default list-txt-style6">
                                <ul>
                                    <li>
                                        <div class="txt">
                                            <a href="/<%= serialSpell %>/peizhi/">
                                                <%= serialShowName%>参数配置</a>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="txt">
                                            <a href="/<%= serialSpell %>/tupian/">
                                                <%= serialShowName%>图片</a>
                                        </div>
                                    </li>
                                    <%=nextSeePingceHtml %>
                                    <li>
                                        <div class="txt">
                                            <a href="/<%= serialSpell %>/baojia/">
                                                <%= serialShowName%>报价</a>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="txt">
                                            <a href="http://www.taoche.com/brand.aspx?spell=<%= serialSpell %>">
                                                <%= serialShowName%>二手车</a>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="txt">
                                            <a href="/<%= serialSpell %>/koubei/">
                                                <%= serialShowName%>怎么样</a>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="txt">
                                            <a href="/<%= serialSpell %>/youhao/">
                                                <%= serialShowName%>油耗</a>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="txt">
                                            <a href="<%= baaUrl %>" target="_blank">
                                                <%= serialShowName%>论坛</a>
                                        </div>
                                    </li>
                                    <%=nextSeeDaogouHtml %>
                                </ul>
                            </div>
                        </div>
                    </div>
                     <!-- SEO底部热门 -->
                        <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                        <!-- SEO底部热门 -->
                </div>
            </div>
            <div class="col-xs-3">
                <div class="section-right">
                    <!-- ad -->
                   <%-- <div class="layout-1 adv-sidebar">
                        <ins id="div_20b7e84d-7a5d-461b-b484-b3d0fec5a81e" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%=serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="20b7e84d-7a5d-461b-b484-b3d0fec5a81e"></ins>
                    </div>
                    <div class="layout-1 adv-sidebar">
                        <ins id="div_9e066309-54d0-4928-a251-b2f3c86ddc4d" data-type="ad_play" data-adplay_ip="" data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="<%=serialId %>" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="9e066309-54d0-4928-a251-b2f3c86ddc4d"></ins>
                    </div>--%>
                    <!-- 看过某车的还看过 -->
                    <%if (!string.IsNullOrEmpty(serialToSeeHtml))
                      { %>
                            <div class="layout-1 looking-sidebar">
                                <h3 class="top-title">看了还看</h3>
                                <div class="col2-140-box clearfix">
                                    <%=serialToSeeHtml%>
                                </div>
                            </div>
                    <%} %>
                    <!-- 网友都用某车和谁比 -->
                    <%if (!string.IsNullOrEmpty(hotSerialCompareHtml))
                      { %>
                        <div class="compare-sidebar">
                            <h3 class="top-title">综合对比</h3>
                            <%=hotSerialCompareHtml %>
                        </div>
                    <%} %>
                    <!--此品牌下其别子品牌-->
                    <div class="layout-1 samebrandcar-sidebar">
                        <%=GetBrandOtherSerial() %>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/serialexternalcommon.min.js?v=2016122815"></script>
    <script language="javascript" type="text/javascript" defer="defer">
    	getDirectSell(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId,'yc-cxzs-year-1');
        getDemandAndJiangJia(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId);
    	<%= JsTagForYear %>
    	var CarCommonBSID = <%=serialBaseInfo.MasterbrandId%>;
    	var CarCommonCBID = <%=cse.Cb_Id%>;
    	var CarCommonCSID = <%=serialId%>;
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/ucarserialcity.min.js?v=2016122815"></script>
    <script type="text/javascript">
        $(function(){
            $(".list-table tr[id^='car_filter_id_']").hover(
                    function () {
                        $(this).addClass('hover-bg-color').find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
                        if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
                            $(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
                    },
                    function () {
                        $(this).removeClass('hover-bg-color').find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
                        if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
                            $(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
                    });
            var timer=null;
            $("#color-listbox li").hover(function(){
                clearTimeout(timer);
                $(".color-img-list").show();
                $(this).addClass("current").siblings().removeClass("current");
                var index=$(this).index();
                $(".img .color-img-list img:eq("+index+")").show().siblings().hide();
            },function(){
                var $ele=$(this);
                timer=setTimeout(function(){
                    $(".color-img-list").hide();
                    $ele.removeClass("current");
                    var index=$ele.index();
                    $(".img .color-img-list img:eq("+index+")").hide();
                },100)
            });
            //$("#more-color").toggle(function () { $(this).addClass("more-current"); $("#more-color-sty").show(); }, function () { $(this).removeClass("more-current"); $("#more-color-sty").hide(); });
            //获取最高降价幅度
            $.ajax({
                url: "http://cdn.partner.bitauto.com/CarSerialPriceInfo/Handler/GetCsPriceCommon.ashx?action=GetMaxFavorAndDealerCount&brandId="+<%= serialId %>+"&cityId="+cityId,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "getdealercount",
                success: function (data) {
                    if(data&&data.length>0)
                    {
                        var downPrice=data[0].MaxFavorablePrice;
                        if(downPrice==null)
                        {
                            $(".price-reduction-lg").text("暂无降价");
                        }
                        else{
                            $(".price-reduction-lg").text(downPrice+"万");
                        }
                    }
                }
            });
            //补贴
            $.ajax({
                url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + <%= serialId %> + "&cityid=" + cityId,
                dataType: "jsonp",
                jsonpCallback: "getSubsidyCallback",
                cache: true,
                success: function (data) {
                    if (!(data && data.length > 0)) return;
                    $.each(data, function (i, n) {
                        if (!(n.StateSubsidies > 0 && n.LocalSubsidy > 0)) return;
                        var carLine = $("#carlist_" + n.CarId);
                        var sub = [];
                        if (n.StateSubsidies > 0)
                            sub.push("国家补贴" + n.StateSubsidies + "万元");
                        if (n.LocalSubsidy > 0)
                            sub.push("地方补贴" + n.LocalSubsidy + "万元");
                        var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"color-block2\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
                        if (carLine.find("span.color-block2").length > 0) {
                            carLine.find("span.color-block2").after(b);
                        } else {
                            carLine.find("a:first").after(b);
                        }
                        //}
                    });
                }
            });
        });
	
        var cityId = 201;
        if(typeof (bit_locationInfo) != 'undefined'){
            cityId = bit_locationInfo.cityId;
        }
        if (typeof(showUCar) != "undefined") {
            var guidPrice = $("#guidPrice").text();
            if (guidPrice != "暂无") {
                var lp;
                var hp;
                var guidPriceArray = guidPrice.split("-");
                if (guidPriceArray.length == 1) {
                    lp = guidPrice.substring(0, guidPrice.length - 1);
                    hp = lp;
                } else {
                    lp = guidPriceArray[0];
                    hp = guidPriceArray[1].substring(0, guidPriceArray[1].length - 1);
                }
                showUCar(<%=serialId %>, cityId, '<%=serialSpell %>', '<%=serialShowName%>', getUCarForBottom, 8, lp, hp);
            } else {
                showUCar(<%=serialId %>, cityId, '<%=serialSpell %>', '<%=serialShowName%>', getUCarForBottom, 8);
            }
        }
        // 对比浮动框
        //insertWaitCompareDiv();
        //二手车数据

        function getUCarForBottom(data, csId, csSpell, csShowName) {
            try {
                var dataCarListInfo = data.CarListInfo;
                var dataRelateCityList = data.RelateCityList;
                if (dataCarListInfo.length <= 0) return;
                var strHtml = [];

                strHtml.push("<div class=\"section-header header2 mbl\">");
                strHtml.push("<div class=\"box\">");
                strHtml.push("<h2><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/?ref=pc_yc_ckzs_tj_esc\">二手车推荐</a></h2></div>");
                strHtml.push("<div class=\"more\">");
                if (dataRelateCityList.length > 0) {
                    $.each(dataRelateCityList, function(index, res) {
                        strHtml.push("<a href=\"" + res.CityUrl + "?ref=pc_yc_ckzs_tj_dq\" target=\"_blank\">" + res.CityName + "</a>");
                    });
                }
                strHtml.push("<a target=\"_blank\" href=\"http://www.taoche.com/" + csSpell + "/?ref=pc_yc_ckzs_tj_gd\">更多&gt;&gt;</a></div>");
                strHtml.push("</div>");

                strHtml.push("<div class=\"row col4-180-box\">");
                $.each(dataCarListInfo, function(i, n) {
                    if (i > 7) return;
                    var className = "";
                    if (i == 0 || i == 4) className = "first";
                    if (i == 3 || i == 7) className = "last";

                    strHtml.push("<div class=\"img-info-layout-vertical img-info-layout-vertical-180120-2\">");
                    strHtml.push("<div class=\"img\"><a href=\""+n.CarlistUrl+"\" target=\"_blank\"><img src=\""+n.PictureUrl+"\" alt=\""+ n.BrandName+"\"></a></div>");
                    strHtml.push("<ul class=\"p-list\">");
                    strHtml.push("<li class=\"name\"><a href=\""+n.CarlistUrl+"\">"+n.BrandName+"</a></li>");
                    strHtml.push("<li class=\"price\">￥"+ n.DisplayPrice+"</li>");
                    strHtml.push("<li class=\"info no-wrap\">"+n.BuyCarDate +"上牌 "+n.DrivingMileage+"公里</li>");
                    strHtml.push("</ul>");
                    strHtml.push("</div>");
                });
                strHtml.push("</div>");
                strHtml.push("<div class=\"btn-box1\"><a class=\"btn btn-default\" target=\"_blank\" href=\"http://www.taoche.com/"+csSpell+"/?ref=pc_yc_ckzs_tj_db\"><span class=\"more\">更多二手车</span></a></div>");
                $("#ucarlist").html(strHtml.join(''));
            } catch(e) {
            }
        }
    </script>
    <!--#include file="~/htmlV2/rightbar.shtml"-->
    <!--footer start-->
    <!--#include file="~/htmlV2/footer2016.shtml"-->
    <!--footer end-->
    <!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->

	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr='<%=serialId%>-'+(zamCityId.length>=4?zamCityId.substring(0,2):zamCityId.substring(0,1))+'-'+zamCityId+'';
 		var zamplus_tag_params = {
			modelId:modelStr,
			carId:0
		};
    </script>
    <script type="text/javascript">
    	var _zampq = [];
    	_zampq.push(["_setAccount", "12"]);
    	(function () {
    		var zp = document.createElement("script"); zp.type = "text/javascript"; zp.async = true;
    		zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
    		var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(zp, s);
    	})();
    </script>
    <!--二手车模块统计代码-->
    <script type="text/javascript" src="http://image.bitautoimg.com/stat/PageAreaStatistics.js"> </script>
    <script type="text/javascript">
        PageAreaStatistics.init("362");
    </script>
	<!--二手车模块统计代码结束-->

</body>
</html>
<!-- 经销商块改INS -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
