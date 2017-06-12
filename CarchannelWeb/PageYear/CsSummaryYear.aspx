<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummaryYear.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageYear.CsSummaryYear" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%= serialSeoName%>】<%= serialSeoName%>最新报价_<%= serialSeoName%>图片-易车网</title>
    <meta name="Keywords" content="<%= serialSeoName%>,<%= serialSeoName%>报价,<%= serialSeoName%>价格,<%= serialSeoName%>油耗,<%= serialSeoName%>图片,易车网,car.bitauto.com" />
    <meta name="Description" content="易车提供<%= serialSeoName%>最新报价,<%= serialSeoName%>图片, <%= serialSeoName%>油耗,查<%= serialSeoName%>最新价格,就上易车网" />
    <link rel="canonical" href="http://car.bitauto.com/<%=serialSpell %>/<%=carYear.ToString()%>/" />
    <!--<link rel="stylesheet" type="text/css" href="/car/css/carsummary2010_0401_3.css" />-->
    <!--#include file="~/ushtml/0000/yiche_2014_cube_carniankuan-725.shtml"-->
    <style type="text/css">
        .zs_koubei_card { display: none; }
    </style>
</head>
<body> 
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <!--smenu start-->
    <!--a_d start-->
    <div class="bt_ad">
        <%=serialTopAdCode%>
    </div>
    <!--a_d end-->
    <%= CsHead %>
    <!--page start-->
    <div class="bt_page">
        <!-- 概览 -->
        <%--<%=SerialInfoBarHtml%>--%>
        <script>
            function showCarList() { document.getElementById("this").style.display = "block"; document.getElementById("tit").style.color = "#000"; document.getElementById("tit").style.cursor = "default"; }
            function hideCarList() { document.getElementById("this").style.display = "none"; document.getElementById("tit").style.color = "#164A84"; document.getElementById("tit").style.cursor = "pointer"; }
        </script>
        <!-- 两 栏之图片 -->
        <div class="col-all">
            <div class="col-con">
                <div class="card-head-box clearfix line-box_t0">
                    <div class="img-box">
                        <%= CsPicJiaodian %>
                    </div>
                    <div class="txt-box zs-ys-card">
                        <p class="p-tit">
                            <%if (minPriceRange == "停售" || minPriceRange == "暂无报价")
                              {%>
                            <strong>
                                <%=minPriceRange %></strong>
                            <%}
                              else
                              {%>参考最低价：<strong><a href="/<%=serialSpell %>/baojia/" target="_blank"><%=minPriceRange%></a></strong><%} %>
                            <a id="btnDirectSell" style="display: none;" href="#" target="_blank" class="ico-shangchengtehui">直销</a>
                        </p>
                        <ul>
                            <li><i>厂商指导价：</i><span id="guidPrice"><%=referPrice%></span> </li>
                            <li class="current"><i class="i-w">二手车：</i><span><%if (string.IsNullOrEmpty(serialUCarPrice))
                                                                               { %>
								暂无报价
								<%}
                                                                               else
                                                                               { %>
                                <a href="http://www.taoche.com/buycar/serial/<%=serialSpell %>/" target="_blank">
                                    <%=serialUCarPrice%></a>
                                <%} %></span> </li>
                            <% if (isElectrombile)
                               {%>
                            <li><i>充电时间：</i><span><%=chargeTimeRange%></span> </li>
                            <li class="current"><i>快充时间：</i><span><%=fastChargeTimeRange%></span> </li>
                            <li><i>续航里程：</i><span><%=mileageRange%></span> </li>
                            <li class="current"><i class="i-w">保&nbsp;&nbsp;&nbsp;&nbsp;修：</i><span><%=sic.SerialRepairPolicy %></span>
                            </li>
                            <%}
                               else
                               {%>
                            <li><i>排量：</i><span title="<%=serialExhaustalt %>"><%=serialExhaust%></span> </li>
                            <li class="current"><i class="i-w">变速箱：</i><span><%=transmissionTypes%></span> </li>
                            <%} %>
                            <li class="color-box"><i>颜色：</i>
                                <div class="color-sty clearfix" id="color-sty">
                                    <%=serialColorHtml%>
                                </div>
                            </li>
                        </ul>
                        <div class="sc-btn-box">
                            <%if (minPriceRange == "停售")
                              { %>
                            <span class="button_orange btn-xj-w"><a href="http://www.taoche.com/buycar/serial/<%=serialSpell %>/"
                                target="_blank">买二手车</a> </span><span class="button_gray"><a href="http://www.taoche.com/pinggu/" class="" target="_blank">二手车估价</a> </span>
                            <%}
                              else
                              { %>
                            <span class="button_orange btn-xj-w"><a id="cardXunjia" href="http://dealer.bitauto.com/zuidijia/nb<%=serialId %>/?T=1&leads_source=20006"
                                target="_blank">询底价</a> </span><span id="divDemandCsBut" class="button_gray btn-qt-w"
                                    style="display: none;"><a class="" href="#">特卖</a> </span><%=shijiaOrHuimaiche %><span class="button_gray btn-qt-w">
                                        <a href="<%=chedaiADLink %>" class="dk-l" target="_blank">贷款</a> </span>
                            <span class="button_gray btn-qt-w" id="btnZhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?serial=<%=serialId %>&leads_source=20007">置换</a> </span>
                            <%} %>
                        </div>
                    </div>
                </div>
                <div class="summaryMiddleAD">
                    <ins id="middleADForCar" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="37940534-3acb-4358-8f99-ac9abc6624ca"></ins>
                </div>
                <!-- 在销车型 -->
                <%=carListTableHtml%>
                <!-- 某车图片 -->
                <div class="line-box">
                    <%=serialImageHtml%>
                </div>
                <!-- 视频
<div class="line_box vlist">
=videosHtml
</div> -->
                <%--<!-- 口碑 -->
				<%= dianpingHtml %>--%>
                <!-- 经销商 -->
                <div class="line-box" id="vendorInfo">
                    <div class="title-con">
                        <div class="title-box title-box2">
                            <h4>
                                <a target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">
                                    <%=serialSeoName %>-经销商推荐</a></h4>
                            <%--<ul class="title-tab">
							<li><a href="#" class="pop">北京<strong></strong></a></li>
						</ul>--%>
                            <div class="more">
                                <a target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">更多&gt;&gt;</a>
                            </div>
                        </div>
                    </div>
                    <%--<ins id="ep_union_4" partner="1" version="" isupdate="1" type="1" city_type="-1"
                        city_id="0" city_name="0" car_type="2" brandid="0" serialid=""
                        carid="0"></ins>--%>
					<script type="text/javascript">
						document.writeln("<ins Id=\"ep_union_128\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\""+bit_locationInfo.cityId+"\" city_name=\"0\" car_type=\"2\" brandId=\"0\" serialId=\"<%= serialId %>\" carId=\"0\"></ins>");
					</script>
                    <div class="clear">
                    </div>
                </div>
                <!--二手车-->
                <div class="line-box" id="ucarlist">
                </div>
                <!-- SEO导航 -->
                <div class="line-box">
                    <div class="title-con">
                        <div class="title-box title-box2">
                            <h4>接下来要看</h4>
                        </div>
                    </div>
                    <div class="text-list-box-b">
                        <div class="text-list-box">
                            <ul class="text-list text-list-float text-list-float3">
                                <li><a href="/<%= serialSpell %>/peizhi/">
                                    <%= serialShowName%>参数配置</a></li>
                                <li><a href="/<%= serialSpell %>/tupian/">
                                    <%= serialShowName%>图片</a></li>
                                <%=nextSeePingceHtml %>
                                <li><a href="/<%= serialSpell %>/baojia/">
                                    <%= serialShowName%>报价</a></li>
                                <li><a href="http://www.taoche.com/brand.aspx?spell=<%= serialSpell %>">
                                    <%= serialShowName%>二手车</a></li>
                                <li><a href="/<%= serialSpell %>/koubei/">
                                    <%= serialShowName%>怎么样</a></li>
                                <li><a href="/<%= serialSpell %>/youhao/">
                                    <%= serialShowName%>油耗</a></li>
                                <li><a href="<%= baaUrl %>" target="_blank">
                                    <%= serialShowName%>论坛</a></li>
                                <%=nextSeeDaogouHtml %>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- SEO底部热门 -->
                <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
                <!-- SEO底部热门 -->
            </div>
            <div class="col-side">
                <!-- ad -->
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e"></ins>
                </div>
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891"></ins>
                </div>
                <div class="col-side_ad">
                    <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
                </div>
                <!-- 热门文章 -->
                <!--hotNewsHtml -->
                <%=koubeiImpressionHtml%>
                <!-- 看过某车的还看过 -->
                <div class="line-box">
                    <div class="side_title">
                        <h4>看过此车的人还看</h4>
                    </div>
                    <ul class="pic_list">
                        <%=serialToSeeHtml%>
                    </ul>
                </div>
                <!-- 网友都用某车和谁比 -->
                <div class="line-box">
                    <div class="side_title">
                        <h4>网友都用它和谁比</h4>
                    </div>
                    <%=hotSerialCompareHtml %>
                    <div class="clear">
                    </div>
                </div>
                <%--<!-- 答疑 -->
			<%= serialAskHtml %>--%>
                <!--此品牌下其别子品牌-->
                <div class="line-box">
                    <%=GetBrandOtherSerial() %>
                    <div class="clear">
                    </div>
                </div>
                <!--二手车-->
                <%--<%=UCarHtml %>--%>
                <%--<div class="line_box ucar_box">
			</div>--%>
                <!--推广-->
                <div class="line_box baidupush">
                    <script type="text/javascript">
                        /*bitauto 200*200，导入创建于2011-10-17*/
                        var cpro_id = 'u646188';
                    </script>
                    <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
                </div>
                <!--百度推广-End -->
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <!-- 对比浮动框 -->
  <%--  <div id="divWaitCompareLayer" class="comparebar comparebar-index" style="display: none;">
    </div>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/city348IDMapName.js?v=20140701"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialexternalcommon.min.js?v=20160929"></script>
    <script language="javascript" type="text/javascript" defer="defer">
    	getDirectSell(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId,'yc-cxzs-year-1');
        getDemandAndJiangJia(<%=serialId %>,'<%=serialSpell %>',bit_locationInfo.cityId);
    	<%= JsTagForYear %>
    	var CarCommonBSID = <%=serialBaseInfo.MasterbrandId%>;
    	var CarCommonCBID = <%=cse.Cb_Id%>;
    	var CarCommonCSID = <%=serialId%>;
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115"></script>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/carcompareformini.js?v=20141009"></script>--%>

    <!-- 对比浮动框 -->
	<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none;" animateright="-533" animatebottom="180" data-page="compare">
		<div class="tt" id="bar_minicompare" style="cursor: move;">
			<h6>车型对比</h6>
			<a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
		</div>
		<div class="content">
			<ul id="idListULForWaitCompare" class="fixed-list"></ul>
			<div class="fixed-box">
				<div class="fixed-input" id="CarSelectSimpleContainerParent">
					<input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly" />
					<%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
					<div class="right" userful="showcartypesim">
						<div class="star">
							<i class="star-i"></i>
						</div>
					</div> 
					<div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
				</div>
				<div class="clear"></div>
				<div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
			</div>
			<div class="wamp">
				<em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
				<div class="clear"></div>
			</div>
			<div class="alert-center" id="AlertCenterDiv" style="display: none;">
				<p>最多对比10个车款</p>
			</div>
		</div>
	</div>
	<!--漂浮层模板start-->
	<div class="effect" style="display: none;">
		<div class="car-summary-btn-duibi button_gray"><a href="###" target="_self"><span>对比</span></a></div>
	</div>
    <%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150733"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160715"></script>
    <script type="text/javascript">
        $(function(){
            WaitCompareObj.Init();
        });
    </script>

    <script type="text/javascript">
        $(function(){
            $("#car_list tr[id^='car_filter_id_']").hover(
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
            $("#color-sty em").hover(function(){$(this).addClass("current").find(".tc.tc-color-box").show();},function(){$(this).removeClass("current").find(".tc.tc-color-box").hide();});
            $("#more-color").toggle(function () { $(this).addClass("more-current"); $("#more-color-sty").show(); }, function () { $(this).removeClass("more-current"); $("#more-color-sty").hide(); });
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
                strHtml.push("<div class=\"title-con\">");
                strHtml.push("<div class=\"title-box title-box2\">");
                strHtml.push("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/\">相关二手车推荐</a></h4>");
                strHtml.push("<div class=\"more\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/\">");
                if (dataRelateCityList.length > 0) {
                    $.each(dataRelateCityList, function(index, res) {
                        strHtml.push("<a href=\"" + res.CityUrl + "\" target=\"_blank\">" + res.CityName + "</a> | ");
                    });
                }
                strHtml.push("<a target=\"_blank\" href=\"http://www.taoche.com/" + csSpell + "/\">更多&gt;&gt;</a></div>");
                strHtml.push("</div>");
                strHtml.push("</div>");

                strHtml.push("<div class=\"cxdb-box taoche-box taoche-box-mline\">");
                strHtml.push("<ul>");
                $.each(dataCarListInfo, function(i, n) {
                    if (i > 7) return;
                    var className = "";
                    if (i == 0 || i == 4) className = "first";
                    if (i == 3 || i == 7) className = "last";
                    strHtml.push("<li class=\"" + className + "\"><div class=\"img-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\" class=\"pic\"><img width=\"150\" height=\"100\" src=\"" + n.PictureUrl + "\"></a></div>");
                    strHtml.push("<div class=\"txt-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\">" + n.BrandName + "</a></div>");
                    strHtml.push("<strong>" + n.DisplayPrice + "</strong>");
                    strHtml.push("<span>" + n.BuyCarDate.substring(2) + "上牌 " + n.DrivingMileage + "公里</span></li>");
                });
                strHtml.push("</ul>");
                strHtml.push("</div>");
                strHtml.push("<div class=\"clear\"></div>");

                $("#ucarlist").html(strHtml.join(''));
            } catch(e) {
            }
        }
    </script>
    <!--footer start-->
    <!-- 尾 -->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--footer end-->
    <!--提意见浮层-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
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
        //$("#compare div[class^=car-summary-btn-xunjia]").bind('click', function() {
        //    var objid = 10003;
        //    var _sentImg = new Image(1, 1);
        //    _sentImg.src ="http://carstat.bitauto.com/weblogger/img/c.gif?logtype=temptypestring&objid=" + objid + "&str1=" + encodeURIComponent("") + "&str2=" + encodeURIComponent("") + "&" + Math.random();
        //} );
    </script>
	<!--二手车模块统计代码结束-->

</body>
</html>
<!-- 经销商块改INS -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
