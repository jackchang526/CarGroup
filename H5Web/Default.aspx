<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="H5Web.Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
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
    <!--#include file="~/ushtml/0000/4th_2015_car_style-974.shtml"-->
    <script src="http://image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/shouye.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <div class="header">
        <a href="http://m.yiche.com/" class="logo">易车网</a>
        <div class="chunjie"></div>
    </div>
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
                { $(".header").hide(); break; }
        }
    </script>
    <div class="letter-list">
        <ul>
            <%= HtmlAllChar %>
        </ul>
    </div>
    <div class="brand_box">
        <%= HtmlAllSerial %>
    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
    <div class="footer">
        <div class="right-part"><a href="http://m.yiche.com/wap2.0/feedback/">提意见&gt;</a></div>
    </div>
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
