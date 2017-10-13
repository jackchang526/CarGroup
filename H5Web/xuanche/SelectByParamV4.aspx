<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectByParamV4.aspx.cs" Inherits="H5Web.xuanche.SelectByParam" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->

    <title>【配置选车|选车工具】-手机易车网</title>
    <meta name="keywords" content="发烧友选车,选车中心,选车工具,手机易车网"/>
    <meta name="description" content="多维度选车工具，选车中心,尽在手机易车网."/>
    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>
    <script type="text/javascript">
        var par = util.GetKeyValueString(["ad", "order", "lg", "ly","tele","WT.mc_id"]);//lg是否需要logo
    </script>
</head>
<body class="zhixuanche fashao-box">
<!-- 搜索 start -->
<!--#include file="/inc/search.v2.shtml"-->
<!-- 搜索 end -->
<header>
    <ul class="xuanche_class">
        <li>
            <a data-channelid="85.88.479" href="/chebiaodang/" onclick="MtaH5.clickStat('ac1')">品牌选车</a>
        </li>
        <li>
            <a data-channelid="85.88.480" href="/ganjuekong/" onclick="MtaH5.clickStat('ac2')">需求选车</a>
        </li>
        <li class="current">
            <a href="/fashaoyou/">配置选车</a>
        </li>
    </ul>
</header>
<div class="fashao">
<h2>
    <span>选择价格</span> 
</h2>
<!--条 开始-->
<div class="ruler">
    <ul class="line" data-width="1">
        <li></li>
        <li>
            <span>10</span></li>
        <li>
            <span>20</span></li>
        <li>
            <span>30</span></li>
        <li>
            <span>40</span></li>
        <li>
            <span>50</span></li>
        <li>
            <span>60</span></li>
        <li>
            <span>70</span></li>
        <li>
            <span>80</span></li>
        <li>
            <span>90</span></li>
    </ul>
    <div class="sliderbar" data-min="0" data-max="100">
        <div class="line-float max-line current" data-index="100" data-bar="2">
            <div id="max-dot" class="dot max-dot">
                <span>100+</span>
            </div>
        </div>
        <div class="line-float min-line current" data-index="0" data-bar="1">
            <div id="min-dot" class="dot min-dot">
                <span>0</span>
            </div>
        </div>
        <div class="dot touch-dot" style="display: none;">0</div>
    </div>
</div>
<!--条 结束-->
<h2>
    <span>级别</span>
</h2>
<ul class="txt_select car_select">
    <li id="l_1">
        <a data-channelid="85.88.481" href="javascript:void(0);" class="car1"><em></em>微型车</a>
    </li>
    <li id="l_2">
        <a data-channelid="85.88.482" href="javascript:void(0);" class="car2"><em></em>小型车</a>
    </li>
    <li id="l_3">
        <a data-channelid="85.88.483" href="javascript:void(0);" class="car3"><em></em>紧凑型</a>
    </li>
    <li id="l_5">
        <a data-channelid="85.88.484" href="javascript:void(0);" class="car4"><em></em>中型车</a>
    </li>
    <li id="l_4">
        <a data-channelid="85.88.485" href="javascript:void(0);" class="car5"><em></em>中大型车</a>
    </li>
    <li id="l_6">
        <a data-channelid="85.88.486" href="javascript:void(0);" class="car6"><em></em>豪华车</a>
    </li>
    <li id="l_7">
        <a data-channelid="85.88.487" href="javascript:void(0);" class="car7"><em></em>MPV</a>
    </li>
    <li id="l_8">
        <a data-channelid="85.88.488" href="javascript:void(0);" class="car8"><em></em>SUV</a>
    </li>
    <li id="l_9">
        <a data-channelid="85.88.489" href="javascript:void(0);" class="car9"><em></em>跑车</a>
    </li>
    <li id="l_11">
        <a data-channelid="85.88.490" href="javascript:void(0);" class="car10"><em></em>面包车</a>
    </li>
    <li id="l_12">
        <a data-channelid="85.88.491" href="javascript:void(0);" class="car11"><em></em>皮卡</a>
    </li>
</ul>
<h2>
    <span>车身</span>
</h2>
<ul class="txt_select">
    <li id="b_1">
        <a href="javascript:void(0);">两厢</a>
    </li>
    <li id="b_2">
        <a href="javascript:void(0);">三厢</a>
    </li>
    <li id="b_3">
        <a href="javascript:void(0);">旅行版</a>
    </li>
</ul>
<h2>
    <span>厂商</span>
</h2>
<ul class="txt_select">
    <li id="g_1">
        <a href="javascript:void(0);">自主</a>
    </li>
    <li id="g_2">
        <a href="javascript:void(0);">合资</a>
    </li>
    <li id="g_4">
        <a href="javascript:void(0);">进口</a>
    </li>
</ul>
<h2>
    <span>能源</span>
</h2>
<ul class="txt_select">
    <li id="f_7">
        <a href="javascript:void(0);">汽油</a>
    </li>
    <li id="f_8">
        <a href="javascript:void(0);">柴油</a>
    </li>
    <li id="f_16">
        <a href="javascript:void(0);">纯电动</a>
    </li>
    <li id="f_2">
        <a href="javascript:void(0);">油电混合</a>
    </li>
    <li id="f_4">
        <a href="javascript:void(0);">插电混合</a>
    </li>
    <%-- <li id="f_32">
        <a href="javascript:void(0);">天然气</a>
    </li>--%>
</ul>
<h2>
    <span>国别</span>
</h2>
<ul class="txt_select">
    <li id="c_4">
        <a href="javascript:void(0);">德系</a>
    </li>
    <li id="c_2">
        <a href="javascript:void(0);">日系</a>
    </li>
    <li id="c_16">
        <a href="javascript:void(0);">韩系</a>
    </li>
    <li id="c_8">
        <a href="javascript:void(0);">美系</a>
    </li>
    <li id="c_484">
        <a href="javascript:void(0);">欧系</a>
    </li>
    <li id="c_509">
        <a href="javascript:void(0);">非日系</a>
    </li>
</ul>
<h2>
    <span>变速箱</span>
</h2>
<ul class="txt_select">
    <li id="t_1">
        <a href="javascript:void(0);">手动</a>
    </li>
    <li id="t_126">
        <a href="javascript:void(0);">自动</a>
    </li>
</ul>
<h2>
    <span>驱动</span>
</h2>
<ul class="txt_select">
    <li id="dt_1">
        <a href="javascript:void(0);">前驱</a>
    </li>
    <li id="dt_252">
        <a href="javascript:void(0);">四驱</a>
    </li>
    <li id="dt_2">
        <a href="javascript:void(0);">后驱</a>
    </li>
</ul>
<h2>
    <span>排量</span>
</h2>
<ul class="txt_select">
    <li id="d_1">
        <a href="javascript:void(0);">1.3L以下</a>
    </li>
    <li id="d_2">
        <a href="javascript:void(0);">1.3-1.6L</a>
    </li>
    <li id="d_3">
        <a href="javascript:void(0);">1.7-2.0L</a>
    </li>
    <li id="d_4">
        <a href="javascript:void(0);">2.1-3.0L</a>
    </li>
    <li id="d_5">
        <a href="javascript:void(0);">3.1-5.0L</a>
    </li>
    <li id="d_6">
        <a href="javascript:void(0);">5.0L以上</a>
    </li>
</ul>
<h2>
    <span>油耗</span>
</h2>
<ul class="txt_select">
    <li id="fc_0-6">
        <a href="javascript:void(0);">6L以下</a>
    </li>
    <li id="fc_6-8">
        <a href="javascript:void(0);">6-8L</a>
    </li>
    <li id="fc_8-10">
        <a href="javascript:void(0);">8-10L</a>
    </li>
    <li id="fc_10-12">
        <a href="javascript:void(0);">10-12L</a>
    </li>
    <li id="fc_12-15">
        <a href="javascript:void(0);">12-15L</a>
    </li>
    <li id="fc_15-9999">
        <a href="javascript:void(0);">15L以上</a>
    </li>
</ul>
<h2>
    <span>车门数</span></h2>

<%--<ul class="txt_select">
    <li id="more_268">
        <a href="javascript:void(0);">2-3门</a>
    </li>
    <li id="more_270">
        <a href="javascript:void(0);">4-6门</a>
    </li>
</ul>--%>

<h2>
    <span>座位数</span></h2>

<ul class="txt_select">
    <li id="more_279">
        <a href="javascript:void(0);">2座</a>
    </li>
    <li id="more_280">
        <a href="javascript:void(0);">4座</a>
    </li>
    <li id="more_281">
        <a href="javascript:void(0);">5座</a>
    </li>
    <li id="more_282">
        <a href="javascript:void(0);">6座</a>
    </li>
    <li id="more_283">
        <a href="javascript:void(0);">7座</a>
    </li>
    <li id="more_284">
        <a href="javascript:void(0);">7座以上</a>
    </li>
</ul>

<h2>
    <span>配置</span></h2>

<ul class="txt_select">
    <li id="more_296">
        <a href="javascript:void(0);">天窗</a>
    </li>
     <li id="more_197">
        <a href="javascript:void(0);">倒车影像</a>
    </li>
    <li id="more_101">
        <a href="javascript:void(0);">涡轮增压</a>
    </li>
     <li id="more_226">
        <a href="javascript:void(0);">无钥匙启动</a>
    </li>
    <li id="more_192">
        <a href="javascript:void(0);">自动驻车</a>
    </li>
    <li id="more_244">
        <a href="javascript:void(0);">自动空调</a>
    </li>
    <li id="more_170">
        <a href="javascript:void(0);">ESP</a>
    </li>
    <li id="more_163">
        <a href="javascript:void(0);">换挡拨片</a>
    </li>
    <li id="more_189">
        <a href="javascript:void(0);">自动泊车</a>
    </li>
    <li id="more_196">
        <a href="javascript:void(0);">GPS导航</a>
    </li>
     <li id="more_182">
        <a href="javascript:void(0);">定速巡航</a>
    </li>
    <li id="more_178">
        <a href="javascript:void(0);">胎压监测</a>
    </li>
    <li id="more_184">
        <a href="javascript:void(0);">主动刹车</a>
    </li>
    <li id="more_194">
        <a href="javascript:void(0);">上坡辅助</a>
    </li>
    <li id="more_201">
        <a href="javascript:void(0);">LED日间行车灯</a>
    </li>
    <li id="more_246">
        <a href="javascript:void(0);">后排空调</a>
    </li>
    <li id="more_297">
        <a href="javascript:void(0);">发动机启停</a>
    </li>
    <li id="more_169">
        <a href="javascript:void(0);">牵引力制动</a>
    </li>
</ul>
</div>
<div class="button_fixed">
    <div class="button_del_box">
        <div class="button_w">
            <button class="button_del button_del_none" id="btDelAll" disabled="disabled">删除</button>
        </div>
        <div class="button_f">
            <button class="button_del_other" id="searchResult">有&nbsp;&nbsp;&nbsp;&nbsp;款车型符合要求</button>
        </div>
    </div>
</div>

<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/scrollbarX.js?v=201512141030"></script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/selectbyparam.v1.js?v=20160802" type="text/javascript"></script>
<script src="http://image.bitautoimg.com/uimg/mbt2015/js/model20160224.js" type="text/javascript"></script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/xuanche/selectCar_ParamPrice.v1.js?v=2015122117" type="text/javascript"></script>
<script type="text/javascript">
    var par = util.GetKeyValueString(["ad", "order", "lg", "ly","tele","WT.mc_id"]);//lg是否需要logo
    var forshare = util.GetKeyValueString(["WT.mc_id"]);     
    $(function() {
        $(".xuanche_class a").each(function (index, item) {
            var href = $(item).attr("href");
            var hrefLength = href.length;
            if (href.indexOf("?") >= 0) {
                if (href.indexOf("?") === (hrefLength - 1)) {
                    $(item).attr("href", href +  par);
                } else {
                    $(item).attr("href", href + "&" + par);
                }
            } else {
                if (par != null && par.length > 0) {
                    $(item).attr("href", href + "?" + par);
                }
            }
        });
    });
</script>
<script type="text/javascript">
    //$("#btDelAll").attr("disabled", true);
    SelectByParam.paramObj = <%= ParamJson %>,
        SelectByParam.apiUrl = "http://select.car.yiche.com/selectcartoolv2/searchresult",
        SelectByParam.init(),
        $("#searchResult").click(function() {
            var apiUrl = SelectByParam.GetQueryString(false);
            if (apiUrl === "") {
                apiUrl = "/carlist/" + "?h5from=fashao&" + par;
            } else {
                apiUrl = "/carlist/?" + apiUrl + "&h5from=fashao&" + par;
            }
            window.location.href = apiUrl;
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
        title: '想要全景天窗的SUV？还是最省油的小两厢？用来一车定制你的专属爱车！',
        keywords: '发烧友选车,选车中心,选车工具,手机易车网',
        desc: '买车无忧、用车不愁，易车-来一车，买车无难事。',
        link: 'http://car.h5.yiche.com/fashaoyou/?' + forshare,
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