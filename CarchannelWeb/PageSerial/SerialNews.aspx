<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialNews.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.SerialNews" %>

<%@ Register TagPrefix="car" TagName="SerialToSee" Src="~/UserControls/SerialToSee.ascx" %>
<% Response.ContentType = "text/html"; %>
<%--<%@ OutputCache Duration="600" VaryByParam="*" %>--%>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=pageTitle%>
    </title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/CommonFunction.js"></script>
    <!--#include file="~/ushtml/0000/yiche_2014_cube_car_wenzhang-746.shtml"-->
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <!--a_d start-->
    <div class="bt_ad">
        <!-- AD New Dec.31.2011 -->
        <% if (newsType == "hangqing")
           { %>
        <ins id="div_d8a72a70-98ff-4cec-a4f7-c2bb07d10829" type="ad_play" adplay_ip="" adplay_areaname="<%= cityIDForAD %>" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d8a72a70-98ff-4cec-a4f7-c2bb07d10829"></ins>
        <% }
           else
           { %>
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
        <% } %>
    </div>
    <!--a_d end-->
    <!--smenu start-->
    <%= SerialNewHead %>
    <!--page start-->
    <div class="bt_page">
        <div class="col-all">
            <div class="col-con">
                <!-- 行情城市开始 -->
                <% if (isHangQingPage)
                   { %>
                <div class="line_box c0622_01" id="tempNoborder">
                    <h3></h3>
                    <div class="h3_tab2" id="sub_index">
                        <ul>
                            <li class="current" style="cursor: inherit;">按城市看行情</li>
                        </ul>
                    </div>
                    <div class="c0621_01" id="showhideCon">
                        <div id="sub_index_con_0" class="c0705_01">
                            <div class="leftCon">地域：</div>
                            <div class="rightCon">
                                <ul id="all_provice" class="pdnone">
                                    <%=_ProvinceList%>
                                </ul>
                                <div class="dashed" style="display: <%=_IsShowCityAndLine%>"></div>
                                <ul class="pdnone" style="display: <%=_IsShowCityAndLine%>">
                                    <%=_CityList%>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <!--#include file="~/html/list_hangqing.shtml"-->
                <% } %>
                <!-- 行情城市结束 -->
                <%= HangQingDealerHTML%>
                <div class="line-box line-box_t0">
                    <%=newsNavHtml %>
                    <%=newsHtml %>
                    <BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false"
                        NoLinkClassName="preview_off" />
                </div>
				<!--评论数JS-->
				<!--#include file="/include/pd/2014/common/00001/201506_typls_js_Manual.shtml"-->
				<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newsViewCount.min.js"></script>
                <!-- SEO导航 -->
                <div class="line-box">
                    <div class="title-box title-box2">
                        <h4>
                            <a href="javascript:;">接下来要看</a></h4>
                    </div>
                    <div class="text-list-box-b">
                        <div class="text-list-box">
                            <ul class="text-list text-list-float text-list-float3">
                                <li><a href="/<%= cse.Cs_AllSpell %>/peizhi/">
                                    <%= serialShowName%>参数配置</a></li>
                                <li><a href="/<%= cse.Cs_AllSpell %>/tupian/">
                                    <%= serialShowName%>图片</a></li>
                                <%=nextSeePingceHtml %>
                                <li><a href="/<%= cse.Cs_AllSpell %>/baojia/">
                                    <%= serialShowName%>报价</a></li>
                                <li><a href="http://www.taoche.com/<%= cse.Cs_AllSpell %>/" target="_blank">
                                    <%= serialShowName%>二手车</a></li>
                                <li><a href="/<%= cse.Cs_AllSpell %>/koubei/">
                                    <%= serialShowName%>怎么样</a></li>
                                <li><a href="/<%= cse.Cs_AllSpell %>/youhao/">
                                    <%= serialShowName%>油耗</a></li>
                                <li><a href="<%= baaUrl %>" target="_blank">
                                    <%= serialShowName%>论坛</a></li>
                                <%=nextSeeDaogouHtml %>
                            </ul>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <!-- SEO底部热门 -->
                <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
                <!-- SEO底部热门 -->
            </div>
            <div class="col-side">
                <!-- ad -->
                <div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853"></ins>
                </div>
                <%--<div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
                </div>--%>
                <div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e"></ins>
                </div>
                <div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891"></ins>
                </div>
                <div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                        adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26"></ins>
                </div>
                <div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db"></ins>
                </div>
                <!--子品牌相关活动-->
                <!---<script type="text/javascript" src="http://life.bitauto.com/huodong/recommend/forum_recommend.aspx?serial=<%= serialId %>&count=2&source=youce"></script>-->

                <!-- 看过某车的还看过 -->
                <%--<div class="line-box">
				<div class="side_title">
					<h4>
						看了<%=serialShowName.Replace("(进口)", "").Replace("（进口）", "") %>的还看</h4>
				</div>
				<ul class="pic_list">
					<%=serialToSeeHtml%>
				</ul>
			</div>--%>
                <Car:SerialToSee ID="ucSerialToSee" runat="server"></Car:SerialToSee>
                <%=carCompareHtml%>
                <!-- AD -->
                <!-- add by chengl Sep.13.2012 -->
                <%--<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
                    adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
                    adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c"></ins>--%>
                <!--浏览过的品牌-->
                <!--<script charset="gb2312" type="text/javascript" src="http://go.bitauto.com/handlers/ModelsBrowsedRecently.ashx"></script>-->
                <!--此品牌下其别子品牌-->
                <div class="line-box">
                    <%=GetBrandOtherSerial() %>
                    <div class="clear">
                    </div>
                </div>
                <!--二手车-->
                <%--<%=UCarHtml %>--%>
                <div class="line-box display_n" id="line_boxforucar_box" style="display: none;">
                    <div class="side_title">
                        <h4><a target="_blank" href=" http://www.taoche.com/<%=cse.Cs_AllSpell %>/"><%=serialShowName %>二手车</a></h4>
                    </div>
                    <div class="theme_list play_ol">
                        <ul class="secondary_list" id="ucar_serialcity">
                        </ul>
                    </div>
                </div>
                <%--<!--百度推广-->
                <div class="line_box baidupush line_box_top_b">
                    <script type="text/javascript">
                        /*bitauto 200*200，导入创建于2011-10-17*/
                        var cpro_id = 'u646188';
                    </script>
                    <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
                </div>
                <!--百度推广-End -->--%>
            </div>
            <div class="clear">
            </div>
        </div>
        <script type="text/javascript" language="javascript">
            var CarCommonCSID = '<%= serialId.ToString() %>';
        </script>
        <script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
        <script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115" type="text/javascript"></script>
        <script type="text/javascript">
            var cityId = 201;
            if(typeof (bit_locationInfo) != 'undefined'){
                cityId = bit_locationInfo.cityId;
            }
            if(typeof(showUCar)!="undefined"){ 
                showUCar(<%=serialId %>, cityId,'<%=cse.Cs_AllSpell %>','<%=serialShowName%>',getUCarForSider,undefined,undefined,undefined,'zsy_wz');
            }
            //二手车数据
            function getUCarForSider(data, csId, csSpell, csShowName) {
                try {
                    data = data.CarListInfo;
                    if (data.length <= 0) return;
                    var strHtml = [];
                    $.each(data, function (i, n) {
                        if (i > 6) return;
                        strHtml.push("<li><a class=\"img\" title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">");
                        strHtml.push("<img src=\"" + n.PictureUrl + "\"></a><p class=\"tit\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.BrandName + "</a></p>");
                        strHtml.push("<p class=\"hui\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</p>");
                        strHtml.push("<p class=\"red\">" + n.DisplayPrice + "</p>");
                        strHtml.push("</li>");
                    });
                    $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
                } catch (e) { }
            }
        </script>
        <script type="text/javascript">
            var adplay_CityName = ''; //城市
            var adplay_AreaName = ''; //区域
            var adplay_BrandID = '<%= serialId %>'; //品牌id 
            var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
            var adplay_BrandName = ''; //品牌
            var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
        </script>
        <script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>

    </div>
    <!-- 调用尾 -->
    <!--#include file="~/html/footer2014.shtml"-->
    <%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
      { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <%} %>
    <!--提意见浮层-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=serialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
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
</body>
</html>
<script type="text/javascript">
    $(".the_pages.pages_top").removeClass("pages_top");

    if (document.getElementById("car_tab_ul3") && document.getElementById("data_table3_0")) {
        addLoadEvent(function () { tabs("car_tab_ul3", "data_table3", null, true) });
    }
</script>
<!--本站统计代码-->
<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript" language="javascript">
    OldPVStatistic.ID1 = "<%=serialId.ToString() %>";
    OldPVStatistic.ID2 = "0";
    OldPVStatistic.Type = 0;
    mainOldPVStatisticFunction();
</script>
<!--本站统计结束-->
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>

