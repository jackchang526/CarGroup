<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsPingCe.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerialV2.CsPingCe" %>

<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <title>【<%= cse.SeoName %>评测】<%= cse.Brand.MasterBrand.Name + cse.Name %>评测_最新<%= cse.SeoName %>车型详解-易车网</title>
    <meta name="keywords" content="<%= cse.SeoName %>评测,<%= cse.SeoName %>单车评测,<%= cse.Brand.MasterBrand.Name + cse.Name %>优点,<%= cse.SeoName %>缺点,<%= cse.SeoName %>车型详解,易车网,car.bitauto.com"/>
    <meta name="description" content="<%= cse.Brand.MasterBrand.Name + cse.Name %>评测，易车网提供<%= cse.SeoName %>深度评测，包含<%= cse.SeoName %>外观,内饰,空间,视野,灯光,动力,操控,舒适性,油耗,配置与安全性等<%= cse.SeoName %>评测内容。"/>
    <meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%= cse.AllSpell %>/pingce/"/>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <!--#include file="~/ushtml/0000/yiche_2016_cube_xwzw-1280.shtml"-->
</head>
<body class="body-sty favo">
<span id="yicheAnchor" name="yicheAnchor" style="display: block; font-size: 0; height: 0; line-height: 0; width: 0;"></span>
<!--#include file="~/htmlV2/header2016.shtml"-->
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
<script type="text/javascript">
    <%= JsForCommonHead %>
</script>

<!-- 内容 start -->
<div class="container container-w">

<!--content start-->

<div class="content-box clearfix">

<!-- col-left start -->
<div class="col-lef  pull-left">

    <% if (HasPingCeNew)
       { %>
        <!--侧导航 start -->
        <div class="side-nav-box" id="float">
            <ul class="c-list" id="leftUl">
                <%= _LeftTagContent %>
            </ul>
        </div>
        <!--侧导航 end-->

        <!-- 文章内容 start -->
        <div class="col-main-box col-sty">
            <div class="cont-box" id="content_bit">

                <!-- article start -->
                <article>
                    <h1 class="tit-h1">
                        <a href="<%= PingCeFilePath %>" target="_blank">
                            <%= PingCeTitle %><em class="bg-ico-sty"></em>
                        </a>
                    </h1>
                    <div class="article-information">
                         <em id="time"><%= PingCePublishTime %></em><span>来源：<a href="http://www.bitauto.com" target="_blank">易车</a></span><span class='author'><%= PingCeUserName %></span>
                    </div>
                    <!-- 新闻内容 start -->
                    <div class="article-content" id="article-content">
                        <!-- 文章正文 start -->
                        <%= PingCeContent %>
                        <!-- 文章正文 end -->

                        <!-- 编辑有话说 start -->
                        <%= PingceEditorComment %>
                        <!-- 编辑有话说 end -->
                    </div>
                    <!-- 新闻内容 end -->

                </article>
                <!-- article end -->
                <div id="favo"></div>
                <!-- 分页 start -->
                <div id="pagePagination" class="pagination">
                    <%= PingCePagination %>
                </div>
                <!-- 分页 end -->

                <!-- 官方口碑 start -->
                <%= TuijianKoubeiHtml %>
                <!-- 官方口碑 end -->
            </div>
        </div>
        <!-- 文章内容 end -->
    <% }
       else
       { %>
        
    <% } %>
    <!-- 相关文章 start -->
    <%= _MorePingCeContent %>
    <!-- 相关文章 end -->

    <script type="text/javascript">
        var carFloatBar, //导航条
            carFloatBarHeight, //导航条高度
            carContainer, //正文容器
            carContainerHeight, //正文高度
            topHeight, //正文距离顶部高度
            navHeigth = $("#car_tag").height(), //横向导航高度
            pageHeight = 63; //
        function carFloatBarScroll() {
            var top1 = topHeight - navHeigth; //410;
            var top2 = 0;
            var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
            top2 = scrollHeight - 10;
            var marginTop = carContainerHeight - carFloatBarHeight- pageHeight; //63 分页高度
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

        setTimeout(ScrollInit, 1000);// ScrollInit();
        //addEvent(window, "load", function () { ScrollInit(); }, false);
    </script>

    <!-- SEO底部热门推荐 -->
    
    <!-- SEO底部热门推荐 -->

</div>
<!-- col-left end -->

<!-- col-right start -->
<div class="col-rig  pull-right">
    <!-- 广告 start -->
    <!-- 广告 end -->

    <!-- 看了还看 start -->
    <div class="col-sty col-side-sty" id="serialtosee_box">
        <h2>看了还看</h2>
        <!-- 图片列表 start -->
        <div class="look-car-list clearfix" id="serialtosee_content">
        </div>
        <!-- 图片列表 end -->
    </div>
    <!-- 看了还看 end -->

    <div class="col-sty col-side-sty" id="car-videobox">
        <h2><%= strCs_ShowName %> 相关视频</h2>
        <!-- 视频列表 start -->
        <div class="car-video-list">
            <%= _SerialShiPinHtml %>
        </div>
        <!-- 视频列表 end -->
    </div>

    <!-- 网友印象 start -->
    <% if (!(string.IsNullOrEmpty(KoubeiRatingHtml) && string.IsNullOrEmpty(CompetitiveKoubeiHtml)))
       { %>
        <div class="col-sty col-side-sty effect-box">
            <h2>网友对此车的印象</h2>
            <div class="eff-cont">

                <!--网友对此车的印象-->
                <%= KoubeiRatingHtml %>
                <!--网友对此车的印象-->

                <!-- 口碑排行 start -->
                <%= CompetitiveKoubeiHtml %>
                <!-- 口碑排行 end -->

            </div>
        </div>
    <% } %>
    <!-- 网友印象 end -->

    <!--大家拿他和谁比-->
    <%= carCompareHtml %>
    <!--大家拿他和谁比-->

    <!-- 我看过的品牌 -->
    <%= GetBrandOtherSerial() %>
    <!-- 我看过的品牌 -->

    <!-- 相关二手车 start -->
    <div class="col-sty col-side-sty" id="line_boxforucar_box" style="display: none;">
        <h2>相关二手车</h2>
        <div class="used-car-box" id="ucar_serialcity">
        </div>
    </div>
    <!-- 相关二手车 end -->

</div>
<!-- col-right end -->

</div>

<!--content end-->

</div>
<!-- 内容 end -->

<script type="text/javascript">
    var CarCommonBSID = "<%= cse.Brand == null ? 0 : cse.Brand.MasterBrandId %>"; //大数据组统计用
    var CarCommonCBID = "<%= cse.Brand == null ? 0 : cse.Brand.Id %>";
    var CarCommonCSID = '<%= csID.ToString() %>';
</script>
<script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
<script type="text/javascript">
    var serialId = "<%= csID %>";
    <%= serialToSeeJson %>;
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/serialadposition.js?_=<%= DateTime.Now.ToString("yyyyMMddHHmm").Substring(0, 11) + "0" %>"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/serialtoseead.min.js?v=20161228"></script>
<%--<script src="/jsnewV2/serialtoseead.js?v=20151211" type="text/javascript" ></script>--%>
<script src="http://image.bitautoimg.com/carchannel/jsnewv2/ucarserialcity.min.js?v=20161228" type="text/javascript"></script>
<%--<script src="http://image.bitautoimg.com/carchannel/jsnewv2/pingceyouhui.min.js?v=2016122917" type="text/javascript"></script>--%>
<%--<script src="/jsnewV2/pingceyouhui.js?v=20151211" type="text/javascript" ></script>--%>
     
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
                strHtml.push("<div class=\"img-info-layout img-info-layout-14093\">");
                strHtml.push("    <div class=\"img\">");
                strHtml.push("        <a href=\"" + n.CarlistUrl + "\" target=\"_blank\" title=\"" + n.BrandName + "\">");
                strHtml.push("            <img src=\"" + n.PictureUrl + "\"");
                strHtml.push("        </a>");
                strHtml.push("    </div>");
                strHtml.push("    <ul class=\"p-list\">");
                strHtml.push("        <li class=\"name\">");
                strHtml.push("            <a href=\"" + n.CarlistUrl + "\" target=\"_blank\" title=\"" + n.BrandName + "\">" + n.BrandName + "</a>");
                strHtml.push("        </li>");
                strHtml.push("        <li class=\"info\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</li>");
                strHtml.push("        <li class=\"price\">");
                strHtml.push("            <a href=\"" + n.CarlistUrl + "\">" + n.DisplayPrice + "</a>");
                strHtml.push("        </li>");
                strHtml.push("    </ul>");
                strHtml.push("</div>");

            });
            $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
        } catch (e) {
        }
    }

    //if("<%= currentPageIndex %>" == "100"){
    //$(function(){ //优惠
    //PingceYouhui.Init(CarCommonCSID, cityId, <%= currentPageIndex %>, "<%= BaseUrl %>");
    //});
    //}

    
    //视频播放次数和回复数
    function GetVedioNum() {
        var videoObj = $("#car-videobox .img-info-layout");
        if ($(videoObj).length == 0) return;
        var vidArr = [],
            vfidArr = [];
        for (var i = 0; i < $(videoObj).length; i++){
            if ($(videoObj[i]).attr("data-type") == "vf") {
                vfidArr.push($(videoObj[i]).attr("data-id"));
            }
            else {
                vidArr.push($(videoObj[i]).attr("data-id"));
            }
        }
        $.ajax({
            url: "http://api.admin.bitauto.com/videoforum/Promotion/GetVideoByVideoIds?vIds=" + vidArr.join(",") + "&vfids=" + vfidArr.join(","), dataType: "jsonp", cache: true, jsonpCallback: "getvedionumcallback", success: function (data) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        $("#car-videobox div[data-id='" + data[i].VideoId + "'] .play").html(data[i].FormatPlayCount);
                        $("#car-videobox div[data-id='" + data[i].VideoId + "'] .comment").html(data[i].ReplyCount);
                    }
                }
            }
        });
    }
    $(function() {
        GetVedioNum();
    });    
</script>
<script type="text/javascript">
    var adplay_CityName = ''; //城市
    var adplay_AreaName = ''; //区域
    var adplay_BrandID = '<%= csID %>'; //品牌id 
    var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
    var adplay_BrandName = ''; //品牌
    var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>
<% if (csID == 2370 || csID == 2608 || csID == 3398 || csID == 3023 || csID == 2388)
   { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
<% } %>
<!-- 调用尾 -->
<!--#include file="~/htmlv2/rightbar.shtml"-->
<!--#include file="~/htmlV2/footer2016.shtml"-->
<!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
<!-- 晶赞 -->
<script type="text/javascript">
    var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
        modelStr = '<%= csID %>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
    var zamplus_tag_params = {
        modelId: modelStr,
        carId: 0
    };
</script>
<script type="text/javascript">
    var _zampq = [];
    _zampq.push(["_setAccount", "12"]);
    (function() {
        var zp = document.createElement("script");
        zp.type = "text/javascript";
        zp.async = true;
        zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(zp, s);
    })();
</script>
<!-- 晶赞 -->
</body>
</html>
<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript" language="javascript">
    OldPVStatistic.ID1 = "<%= csID.ToString() %>";
    OldPVStatistic.ID2 = "0";
    OldPVStatistic.Type = 0;
    mainOldPVStatisticFunction();
</script>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->