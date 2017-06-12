﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="H5Web.V7.CsSummary" %>
<%@ Import Namespace="H5Web.V7" %>
<%@ Register Src="~/UserControl/CommonTmpForCustomization.ascx" TagPrefix="h5" TagName="CommonTmpForCustomization" %>
<%@ Register Src="~/UserControl/UserTmp.ascx" TagPrefix="h5" TagName="UserTmp" %>

<!DOCTYPE html>
<html>
<head>
    <title>【<%= BaseSerialEntity.SeoName %>】车型手册-报价、配置、图片应有尽有-易车网</title>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="猴赛雷！<%= BaseSerialEntity.SeoName %>-购车查报价、比配置、看图片、享优惠，在这里都能轻松搞定，只能帮你到这咯~"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/> 
    <meta content="telephone=no" name="format-detection"/>

    <!-- #include file="/ushtml/0000/4th_2015-2_yonghu_color_style-1116.shtml" -->

    <script type="text/javascript">
        var date = new Date();
        var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
        document.write(unescape('%3Cscript type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/detectcache.js?v=' + version + '"%3E%3C/script%3E'));
    </script> 

    <script type="text/javascript">
        var summary = { serialId: <%= SerialId %>, IsNeedShare: false, IsUserEdition: false };
        try {
            summary.IsNeedShare = <%= IsNeedShare.ToString().ToLower() %>;
            summary.IsUserEdition = <%= IsUserEdition.ToString().ToLower() %>;
        } catch (err) {
        }
    </script>

    <script type="text/javascript">
        var Config = {
            version: "20160412",
            auchors: [],
            serialId: <%= SerialId %>,
            dealerId: <%= Dealerid %>,
            brokerId: <%= Brokerid %>,
            agentid: <%= AgentId %>,
            masterBrandId: <%= BaseSerialEntity.Brand.MasterBrandId %>,
            carMinReferPrice: <%= CarMinReferPrice %>,
            zhezhukaMark: '<%= Chezhuka %>',
            dealerpersonid: '<%= DealerPersonId %>',
            DefaultCarPic: '<%= DefaultCarPic %>',
            isAd: -1, //ad=0 去掉广告
            customizationList: [<%= (int) CustomizationType.User %>], //定制列表，暂时支持用户版
            currentCustomizationType: '<%= CurrentCustomizationType %>',
            CustomizationTypeEnum: {
                '<%= CustomizationType.CheZhuKa %>': '<%= (int) CustomizationType.CheZhuKa %>',
                '<%= CustomizationType.Broker %>': '<%= (int) CustomizationType.Broker %>',
                '<%= CustomizationType.Dealer %>': '<%= (int) CustomizationType.Dealer %>',
                '<%= CustomizationType.DealerSale %>': '<%= (int) CustomizationType.DealerSale %>',
                '<%= CustomizationType.User %>': '<%= (int) CustomizationType.User %>'
            }
        };
    </script>

    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201604011615"></script>
    <script type="text/javascript">
        //页面顺序是否定制
        var isCustomizationPage = false;
        var orderStr = util.GetQueryStringByName("order");
        if (orderStr != null && typeof orderStr != "undefined" && Config.customizationList.indexOf(<%= (int) CurrentCustomizationType %>) >= 0) {
            isCustomizationPage = true;
            $(".indexmenu-all").hide();
        }
        //是否去广告
        var isad = util.GetQueryStringByName("ad");
        if (!isNaN(isad)) {
            Config.isAd = parseInt(isad);
        }
    </script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.fullpage2.6.5.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.tmpl.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
<div id="sharefloat" class="sharefloat"></div>
<div class="screen-landscape">竖屏浏览体验效果更佳</div>
<div class="screen-bg"></div>
<ul class="navigation" id="navigation">
    <li class="nav-change" id="nav-change">
        <span>换车</span></li>
    <li class="nav-price" id="nav-price">
        <span>询价</span></li>
    <li class="nav-main" id="nav-main">
        <span>导航</span></li>
</ul>

<div class="navigation-box" id="navigation-box">
    <ul class="navigation-box-menu">
        <li class="nav-close" id="nav-close">
            <span>关闭</span>
        </li>
        <li class="nav-price" data-channelid="85.6.904">
            <span></span>
            <p>询底价</p>
        </li>
        <li class="nav-change" data-channelid="85.6.903">
            <span></span>
            <p>换车</p>
        </li>
        <%--<li class="nav-count ">
            <span></span>
            <p>购车计算器</p>
        </li>
        <li class="nav-compare">
            <span></span>
            <p>车型对比</p>
        </li>--%>
    </ul>
    <%
        switch (CurrentCustomizationType)
        {
            case CustomizationType.User:
    %>
        <ul class="navigation-box-button" id="navigation-box-button">
            <li>
                <a data-channelid="85.6.15" href="#page1">封面</a>
            </li>
            <li>
                <a data-channelid="85.6.20" href="#page7">报价</a>
            </li>
            <li>
                <a data-channelid="85.6.21" href="#page8">优惠</a>
            </li>
            <li>
                <a data-channelid="85.6.886" href="#page14">二手车</a>
            </li>
            <li>
                <a data-channelid="85.6.17" href="#page4">评测</a>
            </li>
            <li>
                <a data-channelid="85.6.18" href="#page5">配置</a>
            </li>
            <li>
                <a data-channelid="85.6.16" href="#page3">图片</a>
            </li>
            <li>
                <a data-channelid="85.6.19" href="#page6">点评</a>
            </li>
            <li>
                <a data-channelid="85.6.22" href="#page9">经销商</a>
            </li>
            <li>
                <a data-channelid="85.6.23" href="#page10">车险</a>
            </li>
            <li>
                <a data-channelid="85.6.885" href="#page11">养护</a>
            </li>
            <li>
                <a data-channelid="85.6.887" href="#page12">同级车</a>
            </li>
            <li>
                <a data-channelid="85.6.888" href="#page13">公众号</a>
            </li>
        </ul>
    <%
                break;
        }
    %>

</div>
<div id="navigation-bg"></div>

<div id="fullpage">
<!--第一屏开始-->
<div class="section page1" data-anchor="page1">
    <div id="container" style="display: none">
        <%
            switch (CurrentCustomizationType)
            {
                case CustomizationType.User:
        %>

            <!--固定层开始-->
            <div class="fixed_box" id="logo">
                <!--换车-->
                <div class="changecar_f">
                    <a id="changecar_f" data-channelid="85.6.748" href="#">换车</a>
                </div>
                <script type="text/javascript">
                    /* star 换车按钮*/

                    var par = "", forshare = "";
                    var wtMcId = util.GetQueryStringByName("WT.mc_id");

                    if (wtMcId) {
                        par += "&WT.mc_id=" + wtMcId;
                        forshare = "WT.mc_id=" + wtMcId;
                    }
                    var ad = util.GetQueryStringByName("ad");
                    if (ad) {
                        par += "&ad=" + ad;
                    }
                    var order = util.GetQueryStringByName("order");
                    if (order) {
                        par += "&order=" + order;
                    }
                    var returnUrl = 'http://' + window.location.host;
                    var h5From = util.GetQueryStringByName("h5from");
                    switch (h5From) {
                    case "fashao":
                    case "feel":
                    case "search":
                        if (window.sessionStorage["listUrl"]) {
                            returnUrl = window.sessionStorage["listUrl"];
                        } else {
                            returnUrl += "?" + par;
                        }
                        break;
                    case "brand":
                        returnUrl = 'http://' + window.location.host + "/chebiaodang/?" + par;
                        break;
                    default:
                        returnUrl = 'http://' + window.location.host + "/chebiaodang/";
                    }
                    //换车按钮属性修改及事件绑定
                    $("#changecar_f").attr("href", returnUrl);
                    $(".nav-change").bind("click", function() {
                        window.location.href = returnUrl;
                    });
                    //询底价按钮事件
                    $(".nav-price").bind("click", function() {
                        window.location.href = "http://price.m.yiche.com/zuidijia/nb<%= SerialId %>/?leads_source=H001012";
                    });

                    /* end 换车按钮*/

                </script>
                <!--换车-->

                <div class="img">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
                </div>
                <h1><%= BaseSerialEntity.ShowName %></h1>
                <p>厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
                <%--<p class="txt"><a href="#">易车补贴：4S议价后立减6500元></a></p>--%>
                <ins id="div_12f616ba-172e-4419-9445-dc47b6d7a2d6" type="ad_play" adplay_IP="" adplay_AreaName="" adplay_CityName="" adplay_BrandID="<%= SerialId %>" adplay_BrandName="" adplay_BrandType="" adplay_BlockCode="12f616ba-172e-4419-9445-dc47b6d7a2d6" style="text-decoration: none"> </ins>
            </div>
            <!--固定层结束-->
            <% if (IsExistColor)
               { %>
                <!--换色车型图-->
                <div class="standard_car_pic" id="standard_car_pic_1">
                    <% for (var i = 0; i < SerialColorList.Count; i++)
                       {
                           var item = SerialColorList[i];
                           if (i == 0)
                           {
                    %>
                            <img src="<%= item.ImageUrl.Replace(".com", ".com/newsimg-600-w0-1-q80") %>"
                                 style="display: <%= i == 0 ? "display" : "none" %>;"/>
                        <% }
                           else
                           { %>
                            <img data-src="<%= item.ImageUrl.Replace(".com", ".com/newsimg-600-w0-1-q80") %>"
                                 style="display: <%= i == 0 ? "display" : "none" %>;"/>
                    <% }
                       } %>
                </div>

                <!--颜色名称-->
                <div class="car_color_text" id="car_color_text">
                    <% var itemName = SerialColorList[0]; %>
                    <span style="display: block;"><%= itemName.ColorName %></span>
                </div>

                <!--颜色切换-->
                <ul class="changecolor" id="changecolor" style="display: none">
                    <% for (var i = 0; i < SerialColorList.Count; i++)
                       {
                           var color = SerialColorList[i];
                    %>
                        <li <%= i == 0 ? "class='current'" : string.Empty %>>
                            <span style="background: <%= color.ColorRGB %>;" data-value="<%= color.ColorName %>"></span></li>
                    <% } %>
                </ul>
                <script type="text/javascript">
                    //圆点居中 
                    $('#changecolor').width($('#changecolor li').outerWidth(true) * $('#changecolor li').length).show(3);
                </script>
            <% }
               else
               { %>
                <div class="standard_car_pic">
                     <img src="<%= DefaultCarPic %>"/>
                    <%--<img src="http://img1.bitautoimg.com/uimg/4th/img2/404-600x400.jpg"/>--%>
                </div>
            <% } %>

            <!--新锚点-->
            <ul class="indexmenu-all">
                <li data-menuanchor="">
                    <a data-channelid="85.5.11" href="#page7">报价</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.12" href="#page8">优惠</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.884" href="#page14">二手车</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.8" href="#page4">评测</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.9" href="#page5">配置</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.7" href="#page3">图片</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.10" href="#page6">点评</a>
                </li>
                <li data-menuanchor="">
                    <a data-channelid="85.5.13" href="#page9">经销商</a>
                </li>
            </ul>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        <%
                    break;
            }
        %>
    </div>
</div>
<!--第一屏结束-->
<div class="section page7" data-anchor="page7">
</div>
<div class="section page8" data-anchor="page8">
</div>
<div class="section page11" data-anchor="page14">
</div>
<div class="section page4" data-anchor="page4">
</div>
<div class="section page5" data-anchor="page5">
</div>
<div class="section page3" data-anchor="page3">
</div>
<div class="section page6" data-anchor="page6">
</div>
<div class="section page9" data-anchor="page9">
</div>
<div class="section page10" data-anchor="page10">
</div>
<div class="section page11 yanghu" data-anchor="page11">
</div>
<div class="section page12" data-anchor="page12">
</div>
<!--ending-->
<div class="section page13" data-anchor="page13">
    <div class="slide" id="lastpageForAPP">
        <!--#include file="~/include/pd/2014/disiji/00001/201506_gg_gzgzh_gb2312_Manual.shtml"-->
    </div>
    <script type="text/javascript">
        (function() {
            var refParam = util.GetQueryStringByName('WT.mc_id');
            if (refParam && (refParam.toLowerCase() == 'nbycapp' || refParam.toLowerCase() == 'nbbjapp')) {
                var appLinkForH5 = {
                    IsAPP: false,
                    IsBaoJia: false,
                    APPForIos: 'http://itunes.apple.com/cn/app/qi-che-tong/id384399758?mt=8',
                    APPForAndroid: 'http://app.yiche.com/16/c18',
                    APPForWeiXin: 'http://a.app.qq.com/o/simple.jsp?pkgname=com.yiche.autoeasy',
                    BaoJiaForIos: 'http://itunes.apple.com/cn/app/qi-che-bao-jia-da-quan/id419261235?mt=8',
                    BaoJiaForAndroid: 'http://app.yiche.com/17/c45',
                    BaoJiaForWP: 'http://www.windowsphone.com/s?appid=f985f988-8cf8-4e74-b599-69c770798c71',
                    BaoJiaForWeiXin: 'http://a.app.qq.com/o/simple.jsp?pkgname=com.yiche.price'
                };
                if (refParam.toLowerCase() == 'nbbjapp') {
                    appLinkForH5.IsBaoJia = true;
                } else if (refParam.toLowerCase() == 'nbycapp') {
                    appLinkForH5.IsAPP = true;
                } else {
                    return;
                }
                var isAndroidForH5 = navigator.userAgent.match(/android/ig);
                var isIosForH5 = navigator.userAgent.match(/iphone|ipod|ipad/ig);
                var isInWeixinForH5 = navigator.userAgent.match(/MicroMessenger/ig);
               
                var lastPage = new Array();
                lastPage.push('<div class="info_logo2bg"><div class="info_logo info_logo2">');
                lastPage.push('<img src="' + (appLinkForH5.IsAPP ? 'http://img1.bitautoimg.com/uimg/4th/img/logo_yicheapp.png' : (appLinkForH5.IsBaoJia ? 'http://img1.bitautoimg.com/uimg/4th/img/logo_baojia.png' : '')) + '" />');
                lastPage.push('</div><div class="con_top_bg"></div></div>');
                lastPage.push('<div class="info_logo2_txt"><h3>厌倦了厂商指导价？试试这个吧！</h3><h2>体验秒回底价的感觉</h2>');
                lastPage.push('<a href="' + (appLinkForH5.IsAPP ? (isInWeixinForH5 ? appLinkForH5.APPForWeiXin : (isIosForH5 ? appLinkForH5.APPForIos : appLinkForH5.APPForAndroid)) : (appLinkForH5.IsBaoJia ? (isInWeixinForH5 ? appLinkForH5.BaoJiaForWeiXin : (isIosForH5 ? appLinkForH5.BaoJiaForIos : appLinkForH5.BaoJiaForAndroid)) : ''))
                    + '">下载APP</a></div>');
                $("#lastpageForAPP").html(lastPage.join(""));
            }
        }());
    </script>
</div>
<!--/ending-->
</div>

<script type="text/javascript">
    /* star 页面顺序定制逻辑  加载即执行*/
    var defaultAuchors = [];
    var targetAuchors = [];
    var pageList = $(".section");
    $.each(pageList, function(index, item) {
        var pagename = $(item).attr("data-anchor");
        defaultAuchors.push(pagename);
    });
    //定制版
    if (isCustomizationPage) {
        var orderList = orderStr.split(',');
        //通过参数初始化 auchors 数组
        for (var i = 0; i < orderList.length; i++) {
            //保证存在于标准列表中且auchors中排重
            if (defaultAuchors.indexOf(orderList[i].toLowerCase()) >= 0 && targetAuchors.indexOf(orderList[i].toLowerCase()) < 0) {
                targetAuchors.push(orderList[i]);
            }
        }
        if (targetAuchors.length > 0) {
            //容错处理逻辑:page1,page13不参与排序,
            var
                page1Index = targetAuchors.indexOf("page1"),
                pageEndIndex = targetAuchors.indexOf("page13");

            if (page1Index < 0) {
                //初始化时 logo 隐藏 ,解决页面LOGO闪现问题
                $(".fixed_box").hide();
            }

            if ((page1Index === 0 || page1Index < 0) && (pageEndIndex === (targetAuchors.length - 1)) || pageEndIndex < 0) {
                Config.auchors = targetAuchors; //给定参数有效
                if (page1Index === 0) {
                    $("#container").show();
                }

                //移除没有被定制的页面容器 v7增
                $.each(defaultAuchors, function(index, name) {
                    if (targetAuchors.indexOf(name) < 0) {
                        $("div[data-anchor='" + name + "']").remove();
                    }
                });

            } else {
                Config.auchors = defaultAuchors; //参数无效则使用默认值
                $("#container").show();
            }
        } else {
            Config.auchors = defaultAuchors; //参数无效则使用默认值
            $("#container").show();
        }

        //最后一页去掉向下箭头（针对非动态加载页面）
        $("div[data-anchor='" + Config.auchors[Config.auchors.length - 1] + "']").find(".arrow_down").hide();

        //star 首页菜单定制 
        var indexCustomiztionMenu = [];
        var indexMenuList = $(".indexmenu-all li");
        indexMenuList.each(function(index, item) {
            var currentIndex = Config.auchors.indexOf($($(item).children()[0]).attr("href").replace("#", ""));
            if (currentIndex >= 0) {
                indexCustomiztionMenu[currentIndex] = item.outerHTML; //indexCustomiztionMenu元素可能存在undefined类型
            }
        });
        $(".indexmenu-all").html(indexCustomiztionMenu.join("").replace(",", ""));
        $(".indexmenu-all").show(500); //用户定制版设置显示
        //end 首页菜单定制 

        //star 浮动菜单顺序定制
        var customizationMenu = [];
        var menuList = $("#navigation-box-button li");
        menuList.each(function(index, item) {
            var currentIndex = Config.auchors.indexOf($($(item).children()[0]).attr("href").replace("#", ""));
            if (currentIndex >= 0) {
                customizationMenu[currentIndex] = item.outerHTML; //customizationMenu元素可能存在undefined类型
            }
        });
        if (customizationMenu.length > 0) { //定制页面存在于导航中则将结果显示
            $("#navigation-box-button").html(customizationMenu.join("").replace(",", ""));
        }
        //end 浮动菜单顺序定制

        //删除页面中未被定制的page容器（确保 auchors 和页面容器一致）
        for (var j = 0; j < defaultAuchors.length; j++) {
            if (Config.auchors.indexOf(defaultAuchors[j]) < 0) {
                $("div[data-anchor='" + defaultAuchors[j] + "']").remove();
            }
        }

    } else { //非定制版
        //非定制版显示
        $(".indexmenu-all").show();
        //为定制页面默认值
        Config.auchors = defaultAuchors;
        //非定制版显示第一页内容
        $("#container").show();
    }
    /* end 页面顺序定制逻辑*/
</script>

<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/base.js"></script>
<script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>

<!--template-->
<h5:UserTmp runat="server" id="UserTmp"/>
<h5:CommonTmpForCustomization runat="server" id="CommonTmpForCustomization"/>
<!--/template-->

<script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/underscore/underscore.js"></script>
<script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/backbone/backbone.js"></script>

<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/userdatamodule.v7.js,carchannel/h5/js/cssummary/ColorModule.v6.js,carchannel/h5/js/cssummary/fullpagemodule.v7.js,carchannel/h5/js/cssummary/userroutersetting.v7.js?201604131532"></script>

<%--<script src="/Scripts/cssummary/userdatamodule.v7.js"></script>
<script src="/Scripts/cssummary/ColorModule.v6.js"></script>
<script src="/Scripts/cssummary/fullpagemodule.v7.js"></script>
<script src="/Scripts/cssummary/userroutersetting.v7.js"></script>--%>

<script type="text/javascript">
    $(function() {
        /* star page13页的数据是写在页面里的，这里需要特殊处理*/
        var endindex = Config.auchors.indexOf("page13");
        if (endindex < 0) {
            $("div[data-anchor='page13']").remove();
        }
        /* end page13页的数据是写在页面里的，这里需要特殊处理*/
        define = undefined;
    });
</script>

<script type="text/javascript">
    var XCWebLogCollector = { area: '201' };
    if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
        XCWebLogCollector.area = bit_locationInfo.cityId;
    }
    if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
        XCWebLogCollector.serial = Config.serialId;
    }
</script>
<script type="text/javascript">
    var pid_cid = "";
    if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
        pid_cid = "-" + (bit_locationInfo.cityId.length >= 4 ? bit_locationInfo.cityId.substring(0, 2) : bit_locationInfo.cityId.substring(0, 1)) + "-" + bit_locationInfo.cityId;
    }
    var __zp_tag_params = {
        modelId: summary.serialId + pid_cid,
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
        title: '【<%= BaseSerialEntity.SeoName %>】车型手册-报价、配置、图片应有尽有-易车网',
        keywords: '<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑',
        desc: '猴赛雷！<%= BaseSerialEntity.SeoName %>-购车查报价、比配置、看图片、享优惠，在这里都能轻松搞定，只能帮你到这咯~',
        link: 'http://car.h5.yiche.com/<%= BaseSerialEntity.AllSpell %>/?' + forshare,
        imgUrl: '<%= DefaultCarPic %>'
    };
    var dealerid = <%= Dealerid %>;
    var brokerid = <%= Brokerid %>;
    if (typeof (share_dealerInfo) != "undefined") {
        if (typeof (share_dealerInfo.shareTitle) != "undefined" && share_dealerInfo.shareTitle && share_dealerInfo.shareTitle != "") {
            pageShareContent.title = share_dealerInfo.shareTitle;
        }
        if (typeof (share_dealerInfo.shareDesc) != "undefined" && share_dealerInfo.shareDesc && share_dealerInfo.shareDesc != "") {
            pageShareContent.desc = share_dealerInfo.shareDesc;
        }
        pageShareContent.link = pageShareContent.link + "&dealerid=" + dealerid;
    }
    if (typeof (share_brokerInfo) != "undefined") {
        if (typeof (share_brokerInfo.shareTitle) != "undefined" && share_brokerInfo.shareTitle && share_brokerInfo.shareTitle != "") {
            pageShareContent.title = share_brokerInfo.shareTitle;
        }
        if (typeof (share_brokerInfo.shareDesc) != "undefined" && share_brokerInfo.shareDesc && share_brokerInfo.shareDesc != "") {
            pageShareContent.desc = share_brokerInfo.shareDesc;
        }
        pageShareContent.link = pageShareContent.link + "&brokerid=" + brokerid;
    }
</script>
<!--第一条广告-->
<ins id="div_29e5455f-c705-4489-bdd8-f24f9607267a" style="display: none; text-decoration: none" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= BaseSerialEntity.Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="29e5455f-c705-4489-bdd8-f24f9607267a"></ins>
<!--第二条广告-->
<ins id="div_4d63ae1f-37bc-49e8-81c3-dd2fd040389a" style="display: none" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= BaseSerialEntity.Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="4d63ae1f-37bc-49e8-81c3-dd2fd040389a"></ins>
<!--优惠广告-->
<ins id="div_655a7bd1-d2bb-48ad-bb54-7a0ab4e99a77" style="display: none;" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="<%= BaseSerialEntity.Id %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="655a7bd1-d2bb-48ad-bb54-7a0ab4e99a77"></ins>
<!--#include file="~/inc/WeiXinJs.shtml"-->
<!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
<script type="text/javascript">
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
</script>
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>
</body>
</html>