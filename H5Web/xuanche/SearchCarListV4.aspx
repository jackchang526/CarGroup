<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchCarListV4.aspx.cs" Inherits="H5Web.xuanche.SearchCarListV4" %>

<%@ Register Src="~/UserControl/CarListHtmlTmplUserControl.ascx" TagPrefix="h5" TagName="CarListHtmlTmplUserControl" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->

    <title>【汽车报价|汽车报价大全】-手机易车网</title>
    <meta name="keywords" content="汽车报价,汽车报价大全,选车工具,手机易车网"/>
    <meta name="description" content="易车网报价大全，提供最新的最全的汽车报价、车价查询、帮助您方便快捷的购车!"/>
    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body class="car_list_body">
<div id="CarListWraper">
</div>
<h5:CarListHtmlTmplUserControl ID="CarListHtmlTmplUserControl" runat="server"/>
<script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.tmpl.min.js"></script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>

<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/CarListModuleV4.js?v=201606220953"></script>
<%--<script src="/Scripts/xuanche/CarListModuleV4.js"></script>--%>

<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/carlist_setad.v1.js?v=20160118"></script>
<%--<script src="/Scripts/xuanche/carlist_setad.v1.js"></script>--%>

<script type="text/javascript">
    SearchCarList.apiUrl = "http://select.car.yiche.com/selectcartoolv2/searchresult";
    SearchCarList.Params = <%= ParamJson %>;
    SearchCarList.init();
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
        title: '越野党？上班族？家人多？个性选车，就用来一车！',
        keywords: '汽车报价,汽车报价大全,选车工具,手机易车网',
        desc: '选车、买车、用车，易车-来一车，你喜欢的车子我都有。',
        link: 'http://car.h5.yiche.com/?',
        //imgUrl: 'http://image.bitautoimg.com/carchannel/pic/yiche100.jpg'
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/laiyiche.png'
    };
    try {
        pageShareContent.link = window.location.href;
    } catch (err) {
    }
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

</body>
</html>