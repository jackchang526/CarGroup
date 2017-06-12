<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexV4.aspx.cs" Inherits="H5Web.xuanche.IndexV4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->

    <title>【选车工具|车型大全】-手机易车网</title>
    <meta name="Keywords" content="选车,选车工具,手机易车网"/>
    <meta name="Description" content="易车网车型频道为您提供按条件选车工具,包括按汽车价格、汽车级别、国产进口、变速方式等信息"/>
    <%--<script type="text/javascript">
        var date = new Date();
        var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
        document.write(unescape('%3Cscript type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/detectcache.js?v=' + version + '"%3E%3C/script%3E'));
    </script>--%>
    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
<div class="zhixuanche_index">
    <h1>你怎样选车？</h1>
    <h2>选车难于选媳妇儿？一分钟帮你找到真爱</h2>
    <div class="xuanche zhixuanche1">
        <a data-channelid="85.85.458" href="/chebiaodang/">
            <span>品牌选车</span></a>
    </div>
    <div class="xuanche zhixuanche2">
        <a data-channelid="85.85.459" href="/ganjuekong/">
            <span>需求选车</span></a>
    </div>
    <div class="xuanche zhixuanche3">
        <a data-channelid="85.85.460" href="/fashaoyou/">
            <span>配置选车</span></a>
    </div>
</div>
<script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
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
    var lg = util.GetQueryStringByName("lg");
    if (lg) {
        par += "&lg=" + lg;
    }
    $(function() {
        if (par.indexOf("&") >= 0) {
            $(".xuanche a").each(function(index, item) {
                var href = $(item).attr("href");
                $(item).attr("href", href + "?" + par);
            });
        }
    });
</script>
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
    };
    var pageShareContent = {
        title: '车型手册-选车难于选媳妇儿？一分钟帮你找到真爱。',
        keywords: '选车,选车工具,手机易车网',
        desc: '易车网车型频道为您提供按条件选车工具,包括按汽车价格、汽车级别、国产进口、变速方式等信息',
        link: 'http://car.h5.yiche.com/?',
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/laiyiche.png'
    };
</script>
<!--#include file="~/inc/The3rdStat.html"-->
<!--#include file="~/inc/WeiXinJs.shtml"-->
<!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
<%--<script type="text/javascript">
    (function(param) {
        var c = { query: [], args: param || {} };
        c.query.push(["_setAccount", "12"]);
        c.query.push(["_setSiteID", "1"]);
        (window.__zpSMConfig = window.__zpSMConfig || []).push(c);
        var zp = document.createElement("script");
        zp.type = "text/javascript";
        zp.async = true;
        zp.src = ("https:" == document.location.protocol ? "https:" : "http:") + "//cdn.zampda.net/s.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(zp, s);
    })(window.__zp_tag_params);
</script>--%>
<!--按钮统计-->
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
</body>
</html>