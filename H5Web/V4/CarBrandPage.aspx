<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPage.aspx.cs" Inherits="H5Web.V4.CarBrandPage" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【汽车大全_热门车型大全】-易车</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <script type="text/javascript">
        var date = new Date();
        var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
        document.write(unescape('%3Cscript type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/detectcache.js?v=' + version + '"%3E%3C/script%3E'));
    </script>
    <script type="text/javascript">
        var summary = { serialId: 0 };
    </script>
    <!-- #include file="/ushtml/0000/4th_2015-2_brand_style-1009.shtml" -->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min.js"></script>
</head>
<body style="padding:0">
    <!--小头开始-->
    <div class="t-header">
        选车
        <a href="/" class="head_go"></a>
        <%--<div class="head_share"></div>--%>
    </div>
    <!--小头结束-->
    <script type="text/javascript">
        var WTmc_idForHide = "";
        function getQueryStringForHideHead(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return r[2]; return null;
        }
        var tempVarForHide = getQueryStringForHideHead("WT.mc_id");
        if (tempVarForHide != null) {
            WTmc_idForHide = tempVarForHide;
        }
        var hideWTArra = ["mjzgh5"];
        if (WTmc_idForHide && WTmc_idForHide != "") {
            for (var hideLoop = 0; hideLoop < hideWTArra.length; hideLoop++)
                if (hideWTArra[hideLoop] == WTmc_idForHide.toLowerCase())
                { $(".t-header").hide(); break; }
        }
    </script>

    <!-- 车款列表 start -->
    <div id="carlist">
        <%=HtmlBrandToSerial %>
    </div>
    <!-- 车款列表 end -->
    <script type="text/javascript">
        function getQueryStringByName(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return r[2];
            return null;
        }
        var par = "";
        var wtMcId = getQueryStringByName("WT.mc_id");

        if (wtMcId) {
            par += "&WT.mc_id=" + wtMcId;
        }
        var ad = getQueryStringByName("ad");
        if (ad) {
            par += "&ad=" + ad;
        }
        var order = getQueryStringByName("order");
        if (order) {
            par += "&order=" + order;
        }
        $(function () {
            if (par.indexOf("&") >= 0) {
                $("#carlist a").each(function (index, item) {
                    var href = $(item).attr("href");
                    $(item).attr("href", href + "?" + par);
                });
                $(".t-header a").each(function (index, item) {
                    var href = $(item).attr("href");
                    $(item).attr("href", href + "?" + par);
                });
            }
        });
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
    <script type="text/javascript">
        var XCWebLogCollector = { area: '201' };
        if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
            XCWebLogCollector.area = bit_locationInfo.cityId;
        }
        if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
            XCWebLogCollector.serial = summary.serialId;
        }
    </script>
    <script type="text/javascript">
        var __zp_tag_params = {
            modelId: summary.serialId,
            carId: 0
        };
    </script>
    <script type="text/javascript">
        var forweixinObj = {
            debug: false,
            appId: 'wx0c56521d4263f190',
            jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
        }
        var pageShareContent = {
            title: '【汽车大全_热门车型大全】-易车网',
            keywords: '掌上车型手册，360°解读你所关注的车型。',
            desc: '掌上车型手册，360°解读你所关注的车型。',
            link: 'http://car.h5.yiche.com/?',
            imgUrl: 'http://image.bitautoimg.com/carchannel/pic/yiche100.jpg'
        }
    </script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/H5Stat.js?20150716"></script>
    <!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
</body>
</html>

