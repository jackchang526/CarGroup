<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCarCompare.aspx.cs" Inherits="H5Web.carcompare.AddCarCompare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <title>对比选车</title>

    <!--#include file="~/ushtml/0000/4th_2015-2_chexinduibi_style-1120.shtml"-->

    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>
</head>
<body class="bg-gray">
<div id="container">
    <header>
        <h2>车款对比</h2>
        <a class="search-car" href="/">
            选车
        </a>
        <script type="text/javascript">
            var par = util.GetKeyValueString(["ad", "order", "lg", "ly", "tele", "WT.mc_id"]); //lg是否需要logo
            if (par.length > 0) {
                $(".search-car").attr("href", "/?" + par);
            }
        </script>
    </header>
    <div class="con_top_bg"></div>
    <div class="car-pk">
        <a href="javascript:;" data-action="changecar" data-index="1">添加车款</a>
        <i class="icon_pk"></i>
        <a href="javascript:;" data-action="changecar" data-index="2">添加车款</a>
    </div>
    <div class="wrap20 mt30 bt30">
        <a href="javascript:void(0);" id="buttoncompare" class="button gray" data-channelid="85.98.931">两车对决</a>
    </div>
    <div id="master_container" style="display: none; z-index: 888888;" class="brandlayer mthead">
        <!--#include file="~/inc/compareCarTemplate.html"-->
    </div>
</div>
<%--<script type="text/javascript" src="/Scripts/carcompare/iscroll.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/underscore.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/model.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/note.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/rightswipe.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/brand.v1.js"></script>
<script type="text/javascript" src="/Scripts/carcompare/changecar.v1.js"></script>--%>

<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/carcompare/iscroll.v1.js,carchannel/h5/js/carcompare/underscore.v1.js,carchannel/h5/js/carcompare/model.v1.js,carchannel/h5/js/carcompare/note.v1.js,carchannel/h5/js/carcompare/rightswipe.v1.js,carchannel/h5/js/carcompare/brand.v1.js,carchannel/h5/js/carcompare/changecar.v1.js?20160727"></script>
<script type="text/javascript">
    $(function() {
        changecar.init({ serialId: <%= SerialId %>, carId: <%= CarId %>, callback: changecar.SelectCarCallback });
    });
</script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
<script type="text/javascript">
    var summary = { serialId: 0, IsNeedShare: true, IsUserEdition: true };
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
        title: '"来一车"车型分析报告，摆脱选车困扰。',
        keywords: "",
        desc: "最全面的车型对比分析，小白分分钟变车神。",
        link: window.location.href.indexOf("?") >= 0 ? window.location.href : window.location.href + "?",
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/laiyiche.png'
    };
</script>
<!--#include file="~/inc/The3rdStat.html"-->
<!--#include file="~/inc/WeiXinJs.shtml"-->
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
</body>
</html>