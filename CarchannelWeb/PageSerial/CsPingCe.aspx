<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsPingCe.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsPingCe" %>

<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>【<%= cse.SeoName %>评测】<%= cse.Brand.MasterBrand.Name + cse.Name %>评测_最新<%= cse.SeoName %>车型详解-易车网</title>
    <meta name="keywords" content="<%= cse.SeoName %>评测,<%= cse.SeoName %>单车评测,<%= cse.Brand.MasterBrand.Name + cse.Name %>优点,<%= cse.SeoName %>缺点,<%= cse.SeoName %>车型详解,易车网,car.bitauto.com"/>
    <meta name="description" content="<%= cse.Brand.MasterBrand.Name + cse.Name %>评测，易车网提供<%= cse.SeoName %>深度评测，包含<%= cse.SeoName %>外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全性等<%= cse.SeoName %>评测内容。"/>
    <meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%= cse.AllSpell %>/pingce/"/>
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingxiangjie-745.shtml"-->
</head>
<body>
<span id="yicheAnchor" name="yicheAnchor" style="display: block; font-size: 0; height: 0; line-height: 0; width: 0;"></span>
<!--#include file="~/html/header2014.shtml"-->
<!--a_d start-->
<div class="bt_ad">
    <!-- AD New Dec.31.2011 -->
    <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
         adplay_cityname="" adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype=""
         adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9">
    </ins>
</div>
<!--a_d end-->
<!--smenu start-->
<%= CsHead %>
<script language="javascript">
    <%= JsForCommonHead %>
</script>
<!--page start-->
<div class="bt_page" id="bt_page">
<div class="col-all col-all_noborder col-all_car_detail">
<!-- 两栏左 -->
<div class="col-con">
    <% if (HasPingCeNew)
       { %>
        <div class="float_bar" id="float">
            <ul id="leftUl">
                <%= _LeftTagContent %>
                <%-- <% if(currentPageIndex == 100){ %>
                            <li class="current">优惠</li>
                        <%}else{ %>
                            <li><a href="<%= BaseUrl %>100/">优惠</a></li>
                        <% } %>--%>
            </ul>
        </div>
        <div class="news_con ">
            <div id="content_bit">
                <div class="con_main">
                    <!-- article start -->
                    <article>
                        <h1 class="con">
                            <span class="yuanchuang"><a href="http://news.bitauto.com<%= PingCeFilePath %>" target="_blank"><%= PingCeTitle %></a></span> </h1>
                        <div class="article_box">
                            <p class="article-information">
                                <span class="time" id="time"><%= PingCePublishTime %></span>
                                <span class="from">来源：易车</span>
                                <span class="author"><%= PingCeUserName %></span>
                                <%--<span class="comment" id="commentCont"><a href="#" target="_blank">评论</a></span>--%>
                            </p>
                            <div class="article_share">
                                <%--<a href="#" class="fenxiang">分享</a>
				<a href="#" class="shoucang">收藏</a>--%>
                                <a href="http://news.bitauto.com<%= PingCeFilePath %>#comment" target="_blank" class="pinglun">评论</a>
                            </div>
                        </div>
                        <div class="article-contents">
                            <%= PingCeContent %>
                            <%= PingceEditorComment %>
                        </div>
                    </article>
                    <!-- article end-->
                    <!--翻页 开始-->
                    <%= PingCePagination %>
                    <!--翻页 结束-->
                    <!--推荐口碑 开始-->
                    <%= TuijianKoubeiHtml %>
                    <!--推荐口碑 结束-->
                </div>
            </div>
        </div>
    <% }
       else
       { %>
        <div class="no-txt-box no-txt-m">
            <p class="tit">
                很抱歉，该车型暂无评测文章！
            </p>
            <p>
                我们正在努力更新，请查看其他...
            </p>
            <p>
                <a href="/<%= cse.AllSpell %>/" target="_blank">返回<%= strCs_ShowName %>频道&gt;&gt;</a>
            </p>
        </div>
    <% } %>
    <div class="clear">
    </div>
    <!--更多测评-->
    <%= _MorePingCeContent %>
    <script type="text/javascript">
        var carFloatBar, //导航条
            carFloatBarHeight, //导航条高度
            carContainer, //正文容器
            carContainerHeight, //正文高度
            topHeight, //正文距离顶部高度
            navHeigth = 47, //横向导航高度
            pageHeight = 63; //
        function carFloatBarScroll() {
            var top1 = topHeight - navHeigth; //410;
            var top2 = 0;
            var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
            top2 = scrollHeight - 10;
            var marginTop = carContainerHeight - carFloatBarHeight - pageHeight; //63 分页高度
            var top3 = top1 + carContainerHeight - carFloatBarHeight;

            var isSetting = false;
            if (navigator.userAgent.indexOf('MSIE 6.0') != -1) {
                if (top2 > top3 - 30 - pageHeight) {
                    carFloatBar.style.position = "absolute";
                    carFloatBar.style.top = "0px";
                    carFloatBar.style.marginTop = marginTop + "px";
                } else if (top2 > top1) {
                    var length = carFloatBarHeight / 2;
                    if (length < 35) length = 35;
                    var topValue = (top2 + 10 - length) + "px";
                    carFloatBar.style.position = "absolute";
                    carFloatBar.style.top = topValue;
                    carFloatBar.style.marginTop = "0px";
                } else {
                    carFloatBar.style.position = "absolute";
                    carFloatBar.style.top = "0px";
                    carFloatBar.style.marginTop = "50px";
                }
            } else {
                //console.log(top1 + "|" + top2);
                if (top2 > top1) {
                    carFloatBar.style.position = "fixed";
                    carFloatBar.style.top = navHeigth + "px";
                    if (top2 > (top3 - navHeigth - pageHeight)) {
                        carFloatBar.style.position = "absolute";
                        carFloatBar.style.top = "";
                        carFloatBar.style.marginTop = (marginTop - navHeigth) + "px";
                    } else {
                        carFloatBar.style.position = "fixed";
                        carFloatBar.style.top = navHeigth + "px";
                        carFloatBar.style.marginTop = "";
                    }
                } else {
                    carFloatBar.style.position = "absolute";
                    carFloatBar.style.top = "";
                }
            }
        }

        function addEvent(elm, evType, fn, useCapture) {
            if (elm.addEventListener) {
                elm.addEventListener(evType, fn, useCapture);
                return true;
            } else if (elm.attachEvent) {
                var r = elm.attachEvent('on' + evType, fn);
                return r;
            } else {
                elm['on' + evType] = fn;
            }
        }

        function getElementTop(element) {
            var actualTop = element.offsetTop;
            var current = element.offsetParent;
            while (current !== null) {
                actualTop += current.offsetTop;
                current = current.offsetParent;
            }
            return actualTop;
        }

        function ScrollInit() {
            carFloatBar = document.getElementById("float");
            if (!carFloatBar) return;
            carFloatBarHeight = carFloatBar.offsetHeight;
            carContainer = document.getElementById("content_bit");
            carContainerHeight = carContainer.clientHeight;
            topHeight = getElementTop(document.getElementById("content_bit"));
            //alert(topHeight);
            addEvent(window, "scroll", function() { carFloatBarScroll(); }, false);
        }

        ScrollInit();
        //addEvent(window, "load", function () { ScrollInit(); }, false);
    </script>
    <!-- SEO导航 -->
    <!-- SEO底部热门 -->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
    <!-- SEO底部热门 -->
</div>
<!-- 两栏右 -->
<div class="col-side">
    <div class="col-side_ad" style="overflow: hidden; width: 220px;">
        <ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853">
        </ins>
    </div>
    <%--<div class="col-side_ad" style="width: 220px; overflow: hidden">
                    <ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
                        adplay_cityname="" adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype=""
                        adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
                </div>--%>
    <!-- ad -->
    <div class="col-side_ad" style="overflow: hidden; width: 220px;">
        <ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
             adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
        </ins>
    </div>
    <div class="col-side_ad" style="overflow: hidden; width: 220px;">
        <ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
             adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
        </ins>
    </div>
    <div class="col-side_ad" style="overflow: hidden; width: 220px;">
        <ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
             adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
        </ins>
    </div>
    <div class="col-side_ad" style="overflow: hidden; width: 220px;">
        <ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
             adplay_cityname="" adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype=""
             adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db">
        </ins>
    </div>
    <%--<!--车型图片对比-->
				<%= serialPhotoHotCompareHtml%>--%>
    <!-- 看过某车的还看过 -->
    <div class="line-box" id="serialtosee_box">
        <div class="side_title">
            <h4>
                看过此车的人还看
            </h4>
        </div>
        <ul class="pic_list">
           <%-- <%= serialToSeeHtml %>--%>
        </ul>
    </div>
    <%--<Car:SerialToSee ID="ucSerialToSee" runat="server"></Car:SerialToSee>--%>
    <!-- AD -->
    <!-- add by chengl Sep.13.2012 -->
    <%--<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
         adplay_cityname="" adplay_brandid="<%= csID %>" adplay_brandname="" adplay_brandtype=""
         adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c">
    </ins>--%>
    <!--视频块-->
    <%= _SerialShiPinHtml %>
    <!--口碑评分块-->
    <%= KoubeiRatingHtml %>
    <!--竞品口碑-->
    <%= CompetitiveKoubeiHtml %>
    <!--大家拿他和谁比-->
    <%= carCompareHtml %>
    <!-- 我看过的品牌 -->
    <!--<script charset="gb2312" type="text/javascript" src="http://go.bitauto.com/handlers/ModelsBrowsedRecently.ashx"></script>-->
    <!--此品牌下其别子品牌-->
    <div class="line-box">
        <%= GetBrandOtherSerial() %>
        <div class="clear">
        </div>
    </div>
    <!--二手车-->
    <%--<%=UCarHtml %>--%>
    <div class="line-box display_n" id="line_boxforucar_box" style="display: none;">
        <div class="side_title">
            <h4>
                <a target="_blank" href=" http://www.taoche.com/<%= cse.AllSpell %>/"><%= cse.SeoName %>二手车</a>
            </h4>
        </div>
        <div class="theme_list play_ol">
            <ul class="secondary_list" id="ucar_serialcity">
            </ul>
        </div>
    </div>
    <script type="text/javascript">
        var serialId = "<%= csID %>";
        <%= serialToSeeJson %>
        //var SerialAdpositionContentInfo = {
        //    "2370": [
        //        {
        //            "SerialID": "2371",
        //            "Text": "速腾",
        //            "Link": "http://car.bitauto.com/suteng/",
        //            "Image": "http://img3.bitautoimg.com/autoalbum/files/20120816/666/0407166663_5.jpg"
        //        }, null]
        //};
    </script>
    <script type="text/javascript" src="http://gimg.bitauto.com/resourcefiles/chexing/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0,11) + "0" %>"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/serialtoseead.js"></script>
    <!--百度推广-->
    <div class="line_box baidupush line_box_top_b">
        <script type="text/javascript">
            /*bitauto 200*200，导入创建于2011-10-17*/
            var cpro_id = 'u646188';
        </script>
        <script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
    </div>
    <!--百度推广-End -->
</div>
<div class="clear">
</div>
</div>
</div>
<script type="text/javascript" language="javascript">
    var CarCommonCSID = '<%= csID.ToString() %>';
</script>
<script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
<script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115" type="text/javascript"></script>
<script src="http://image.bitautoimg.com/carchannel/jsnew/pingceyouhui.js?v=20161115" type="text/javascript"></script>
<%--<script src="/jsnew/pingceyouhui.js?v=20151211" type="text/javascript" ></script>--%>
<script type="text/javascript">
    var cityId = 201;
    if (typeof (bit_locationInfo) != 'undefined') {
        cityId = bit_locationInfo.cityId;
    }
    if (typeof(showUCar) != "undefined") {
        showUCar(<%= csID %>, cityId, '<%= cse.AllSpell %>', '<%= strCs_ShowName %>', getUCarForSider, undefined, undefined, undefined, 'zsy_cxjx');
    }

    //二手车数据
    function getUCarForSider(data, csId, csSpell, csShowName) {
        try {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var strHtml = [];
            $.each(data, function(i, n) {
                if (i > 6) return;
                strHtml.push("<li><a class=\"img\" title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">");
                strHtml.push("<img src=\"" + n.PictureUrl + "\"></a><p class=\"tit\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.BrandName + "</a></p>");
                strHtml.push("<p class=\"hui\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</p>");
                strHtml.push("<p class=\"red\">" + n.DisplayPrice + "</p>");
                strHtml.push("</li>");
            });
            $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
        } catch (e) {
        }
    }

    //if("<%= currentPageIndex %>" == "100"){
    //$(function(){ //优惠
    PingceYouhui.Init(CarCommonCSID, cityId, <%= currentPageIndex %>, "<%= BaseUrl %>");
    //});
    //}
</script>
<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = '<%= csID %>'; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<% if (csID == 2370 || csID == 2608 || csID == 3398 || csID == 3023 || csID == 2388)
   { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
<% } %>
<!-- 调用尾 -->
<!--#include file="~/html/footer2014.shtml"-->
<!--提意见浮层-->
<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript" language="javascript">
    OldPVStatistic.ID1 = "<%= csID.ToString() %>";
    OldPVStatistic.ID2 = "0";
    OldPVStatistic.Type = 0;
    mainOldPVStatisticFunction();
</script>
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>