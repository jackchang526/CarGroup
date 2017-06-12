<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="H5Web.cssummary.v3.CsSummary" %>
<%@ Import Namespace="H5Web.cssummary.v3" %>
<%@ Register Src="~/UserControl/CommonTmpForCustomization.ascx" TagPrefix="h5" TagName="CommonTmpForCustomization" %>
<%@ Register Src="~/UserControl/BrokerTmp.ascx" TagPrefix="h5" TagName="BrokerTmp" %>
<%@ Register Src="~/UserControl/DealerTmp.ascx" TagPrefix="h5" TagName="DealerTmp" %>
<!DOCTYPE html>
<html>
<head>
    <title>
        <%
            switch (CurrentCustomizationType)
            {
                case CustomizationType.CheZhuKa:
                case CustomizationType.Broker:
                case CustomizationType.HuiMaiCheGuWen:
        %>
            【<%= BaseSerialEntity.SeoName %>】车型手册-易车
            <%
                    break;
                case CustomizationType.Dealer:
                case CustomizationType.DealerSale:
            %>
            【<%= BaseSerialEntity.SeoName %>】
        <%
                    break;
            }
        %>
    </title>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <%
        switch (CurrentCustomizationType)
        {
            case CustomizationType.CheZhuKa:
    %>
        <!-- #include file="/ushtml/0000/4th_2015-2_chezhuka_style-1044.shtml" -->
        <%
                break;
            case CustomizationType.Broker:
        %>
        <!-- #include file="/ushtml/0000/4th_2015-2_cheguwen_style-1012.shtml" -->
        <%
                break;
            case CustomizationType.Dealer:
        %>
        <!-- #include file="/ushtml/0000/4th_2015-2_jingxiaoshang_style-1011.shtml" -->
        <%
                break;
            case CustomizationType.DealerSale:
        %>
        <!-- #include file="/ushtml/0000/4th_2015-2_rendianche_style-1054.shtml" -->
        <%
                break;
            case CustomizationType.HuiMaiCheGuWen:
        %>
        <!-- #include file="/ushtml/0000/4th_2015-2_huimaiche_style-1094.shtml" -->

    <%
                break;
        }
    %>

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
        function checkData(data) {
            return (data.indexOf("/head>") < 0) && (data.lastIndexOf("/body>") < 0);
        }

        var isMicroMessager = function() {
            var ua = window.navigator.userAgent.toLowerCase();
            if (ua.match(/MicroMessenger/i) === "micromessenger") {
                return true;
            } else {
                return false;
            }
        }();
        var Config = {
            timeout: 3000,
            version: "20160415",
            allSpell: '<%= BaseSerialEntity.AllSpell %>',
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
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.fullpage2.6.5.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.tmpl.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <%
        switch (CurrentCustomizationType)
        {
            case CustomizationType.Broker:
    %>
        <script type="text/javascript" src="/handlers/GetDataAsynV3.ashx?service=agent&method=share&csid=<%= SerialId %>&brokerid=<%= Brokerid %>&type=4"></script>
        <%
                break;
            case CustomizationType.Dealer:
        %>
        <script type="text/javascript">
            document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=dealer&method=dealershare&dealerid=<%= Dealerid %>&csid=<%= SerialId %>&' type='text/javascript'%3E%3C/script%3E"));
        </script>
    <%
                break;
        }
    %>

</head>
<body>
<%--<div id="sharefloat" class="sharefloat"></div>--%>
<%
    switch (CurrentCustomizationType)
    {
        case CustomizationType.HuiMaiCheGuWen:
%>
    <div class="screen-landscape">竖屏浏览体验效果更佳</div>
    <div class="screen-bg"></div>
<%
            break;
    }
%>
<!--固定层开始-->
<%
    switch (CurrentCustomizationType)
    {
        case CustomizationType.CheZhuKa:
%>
    <div class="fixed_box" id="logo" style="display: none">
        <div class="img">
            <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
        </div>
        <h1><%= BaseSerialEntity.ShowName %></h1>
        <p>厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
    </div>
<%
            break;
    }
%>
<!--固定层结束-->

<!--菜单开始-->
<%
    switch (CurrentCustomizationType)
    {
        case CustomizationType.Broker:
        case CustomizationType.Dealer:
        case CustomizationType.HuiMaiCheGuWen:
%>
    <div class="menu">
        <div class="button" id="menubutton" style="display: none"></div>
    </div>
    <%
            break;
        case CustomizationType.DealerSale:
    %>
    <div class="menu speak">
        <a href="http://dealer.h5.yiche.com/<%= DealerPersonId %>/enquiryCsOrder/<%= SerialId %>?from=pc">
            <div class="button" id="speakbutton">
            </div>
        </a>
    </div>
    <script type="text/javascript">
        document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleshare&dealerpersonid=<%= DealerPersonId %>&csid=<%= SerialId %>&' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <div class="menu">
        <div class="button" id="menubutton" style="display: block"></div>
    </div>
<%
            break;
    }
%>

<div class="menu_box_bg" id="menu_box_bg"></div>
<div class="menu_box" id="menu_box">
    <%
        switch (CurrentCustomizationType)
        {
            case CustomizationType.CheZhuKa:
    %>
        <div class="menu house">
            <a href="<%= ChezhukaReturnUrl %>">
                <div class="button" id="housebutton"></div>
            </a>
        </div>
        <%
                break;
            case CustomizationType.Broker:
        %>
        <ul class="menu_box_ul2 menu_box_ul2top">
            <li>
                <a data-channelid="85.62.327" href="#page1">封面</a>
            </li>
            <li>
                <a data-channelid="85.62.328" href="#page3">图片</a>
            </li>
            <li>
                <a data-channelid="85.62.329" href="#page4">评测</a>
            </li>
            <li>
                <a data-channelid="85.62.330" href="#page5">配置</a>
            </li>
            <li>
                <a data-channelid="85.62.331" href="#page6">点评</a>
            </li>
            <li>
                <a data-channelid="85.62.332" href="#page7">报价</a>
            </li>
            <li>
                <a data-channelid="85.62.333" href="#page8">车顾问</a>
            </li>
        </ul>
        <%
                break;
            case CustomizationType.Dealer:
        %>
        <ul class="menu_box_ul2 menu_box_ul2top">
            <li>
                <a data-channelid="85.63.334" href="#page1">封面</a>
            </li>
            <li>
                <a data-channelid="85.63.335" href="#page3">图片</a>
            </li>
            <li>
                <a data-channelid="85.63.336" href="#page4">评测</a>
            </li>
            <li>
                <a data-channelid="85.63.337" href="#page5">配置</a>
            </li>
            <li>
                <a data-channelid="85.63.338" href="#page6">报价</a>
            </li>
            <li>
                <a data-channelid="85.63.339" href="#page7">活动</a>
            </li>
            <li>
                <a data-channelid="85.63.340" href="#page9">店铺</a>
            </li>
        </ul>
        <%
                break;
            case CustomizationType.DealerSale:
        %>
        <ul class="menu_box_ul2 menu_box_ul2top">
            <li>
                <a data-channelid="85.64.341" href="#page1">封面</a>
            </li>
            <li>
                <a data-channelid="85.64.342" href="#page3">图片</a>
            </li>
            <li>
                <a data-channelid="85.64.343" href="#page4">评测</a>
            </li>
            <li>
                <a data-channelid="85.64.344" href="#page5">配置</a>
            </li>
            <li>
                <a data-channelid="85.64.345" href="#page6">报价</a>
            </li>
            <li>
                <a data-channelid="85.64.346" href="#page7">促销</a>
            </li>
            <li>
                <a data-channelid="85.64.347" href="#page9">店铺</a>
            </li>
        </ul>
        <%
                break;
            case CustomizationType.HuiMaiCheGuWen:
        %>
        <ul class="menu_box_ul2 menu_box_ul2top">
            <li>
                <a data-channelid="85.66.352" href="#page1">封面</a>
            </li>
            <li>
                <a data-channelid="85.66.353" href="#page3">图片</a>
            </li>
            <li>
                <a data-channelid="85.66.354" href="#page4">评测</a>
            </li>
            <li>
                <a data-channelid="85.66.355" href="#page5">配置</a>
            </li>
            <li>
                <a data-channelid="85.66.356" href="#page6">点评</a>
            </li>
            <li>
                <a data-channelid="85.66.357" href="#page7">报价</a>
            </li>
            <li>
                <a data-channelid="85.66.358" href="#page8">优惠</a>
            </li>
            <li>
                <a data-channelid="85.66.359" href="#page9">买车</a>
            </li>
        </ul>
    <%
                break;
        }
    %>

</div>
<script type="text/javascript">
    $("#menu_box li").hide();
</script>
<!--菜单结束-->
<div id="fullpage">
<!--第一屏开始-->
<div class="section page1" data-anchor="page1">

    <%
        switch (CurrentCustomizationType)
        {
            case CustomizationType.CheZhuKa:
    %>
        <!--车主卡 车型反白图-->
        <div class="standard_car_pic">
            <img src="<%= DefaultCarPic %>"/>
        </div>
        <!--锚点-->
        <%--<ul class="indexmenu-all">--%>
        <ul class="indexmenu" style="display: none">
            <li data-menuanchor="">
                <a data-channelid="85.65.348" href="#page3">图片</a>
            </li>
            <li data-menuanchor="">
                <a data-channelid="85.65.349" href="#page4">评测</a>
            </li>
            <li data-menuanchor="">
                <a data-channelid="85.65.350" href="#page5">配置</a>
            </li>
            <li data-menuanchor="">
                <a data-channelid="85.65.351" href="#page6">点评</a>
            </li>
        </ul>
        <!--下箭头 固定-->
        <div class="arrow_down"></div>
        <%
                break;
            case CustomizationType.Broker:
        %>
        <div class="section-box">
            <script type="text/javascript">
                document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=agent&method=header&csid=<%= SerialId %>&brokerid=<%= Brokerid %>&type=1' type='text/javascript'%3E%3C/script%3E"));
            </script>
            <div class="img">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
            </div>
            <div class="context">
                <h1><%= BaseSerialEntity.ShowName %></h1>
                <p class="cs-price">厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
                <div class="standard_car_pic">
                    <%--<img src="http://image.bitautoimg.com/carchannel/pic/cspic/<%= BaseSerialEntity.Id %>.jpg"/>--%>
                    <img src="<%= DefaultCarPic %>"/>
                </div>
            </div>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <%
                break;
            case CustomizationType.Dealer:
        %>
        <div class="section-box">
        </div>
        <%
                break;
            case CustomizationType.DealerSale:
        %>
        <div class="section-box">
            <script type="text/javascript">
                document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=dealersale&method=header&dealerpersonid=<%= DealerPersonId %>&' type='text/javascript'%3E%3C/script%3E"));
            </script>
            <div class="img">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
            </div>
            <div class="context">
                <h1><%= BaseSerialEntity.ShowName %></h1>
                <p class="cs-price">厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
                <div class="standard_car_pic">
                    <img src="<%= DefaultCarPic %>"/>
                </div>
            </div>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <%
                break;
            case CustomizationType.HuiMaiCheGuWen:
        %>
        <div class="section-box">
            <script type="text/javascript">
                document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=huimaiche&method=header&csid=<%= SerialId %>&agentid=<%= AgentId %>&' type='text/javascript'%3E%3C/script%3E"));
            </script>
            <div class="img">
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
            </div>
            <div class="context">
                <h1><%= BaseSerialEntity.ShowName %></h1>
                <p class="cs-price">厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
                <div class="standard_car_pic">
                    <%--<img src="http://image.bitautoimg.com/carchannel/pic/cspic/<%= BaseSerialEntity.Id %>.jpg"/>--%>
                    <img src="<%= DefaultCarPic %>"/>
                </div>
            </div>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
    <%
                break;
        }
    %>
</div>
<!--第一屏结束-->

<% if (IsExistColor)
   { %>
    <!--第二屏 颜色列表 开始-->
    <div class="section page2" data-anchor="page2">
        <% if (Brokerid > 0 || Dealerid > 0 || DealerPersonId > 0 || AgentId > 0)
           { %>
            <div class="section-box">
                <div class="img">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png"/>
                </div>
                <div class="context">
                    <h1><%= BaseSerialEntity.ShowName %></h1>
                    <p class="cs-price">厂商指导价：<%= BaseSerialEntity.SaleState == "停销" ? "暂无" : BaseSerialEntity.ReferPrice %></p>
                </div>
            </div>
        <% } %>
        <!--换色车型图-->
        <div class="standard_car_pic standard_car_pic_1" id="standard_car_pic">
            <% for (var i = 0; i < SerialColorList.Count; i++)
               {
                   var item = SerialColorList[i];
            %>
                <img data-src="http://image.bitautoimg.com/newsimg-600-w0-1-q80/<%= item.ImageUrl.Substring(27) %>?20151214" style="display: <%= i == 0 ? "display" : "none" %>;"/>
            <% } %>
        </div>
        <!--颜色名称-->
        <div class="car_color_text" id="car_color_text">
            <% var itemName = SerialColorList[0]; %>
            <span><%= itemName.ColorName %></span>
        </div>
        <!--颜色切换-->
        <ul class="changecolor" id="changecolor">
            <% for (var i = 0; i < SerialColorList.Count; i++)
               {
                   var color = SerialColorList[i];
            %>
                <li <%= i == 0 ? "class='current'" : string.Empty %>>
                    <span style="background: <%= color.ColorRGB %>;" data-value="<%= color.ColorName %>"></span></li>
            <% } %>
        </ul>
        <!--下箭头 固定-->
        <div class="arrow_down"></div>
    </div>
    <!--第二屏结束-->
<% } %>


<!--第三屏 外观设计 开始-->
<div class="section page3" data-anchor="page3">
</div>
<!--第三屏结束-->
<div class="section page4" data-anchor="page4">
</div>
<div class="section page5" data-anchor="page5">
</div>
<div class="section page6" data-anchor="page6">
</div>
<%
   switch (CurrentCustomizationType)
   {
       case CustomizationType.Broker:
%>
    <div class="section page7" data-anchor="page7">
    </div>
    <div class="section page8" data-anchor="page8">
    </div>
    <div class="section page9" data-anchor="page9">
        <div class="info_logo">
            <img src="http://img1.bitauto.com/uimg/4th/img2/logo_yiche01.png"/>
        </div>
        <div class="info_t">
            <h2>车型手册</h2>
            <h3>汽车信息的权威字典</h3>
        </div>
        <div class="info">
            <h5>专业免费车顾问，等你来约</h5>
            <img src="http://img1.bitauto.com/uimg/4th/img2/cheguwen-DR.png"/>
        </div>
    </div>
    <%
           break;
       case CustomizationType.Dealer:
    %>
    <div class="section page7" data-anchor="page7">
    </div>
    <div class="section page8" data-anchor="page8">
    </div>
    <div class="section page10 yanghu" data-anchor="page10">
    </div>
    <div class="section page9" data-anchor="page9">
    </div>
    <div class="section page11 erweima" data-anchor="page11">
    </div>
    <%
           break;
       case CustomizationType.DealerSale:
    %>
    <div class="section page7" data-anchor="page7">
    </div>
    <div class="section page8" data-anchor="page8">
    </div>
    <div class="section page9" data-anchor="page9">
    </div>
    <%
           break;
       case CustomizationType.HuiMaiCheGuWen:
    %>
    <div class="section page7" data-anchor="page7">
    </div>
    <div class="section page8" data-anchor="page8">
    </div>
    <div class="section page9" data-anchor="page9">
    </div>
    <div class="section page10" data-anchor="page10">
    </div>
    <div class="section page11" data-anchor="page11">
        <div class="info_logo2bg">
            <div class="info_logo info_logo2">
                <img src="http://img1.bitautoimg.com/uimg/4th/img2/logo_mcgw.png"/>
            </div>
            <div class="con_top_bg"></div>
        </div>
        <div class="info_logo2_txt">
            <h3>还在为询价奔波？还在位车价不透明而头疼？</h3>
            <h2>全城底价任你选</h2>
            <a href="http://m.huimaiche.com/issue/gwapp/">下载APP</a>
        </div>
    </div>
<%
           break;
   }
%>
</div>
<script type="text/javascript">
    var defaultAuchors = [];
    var pageList = $(".section");
    $.each(pageList, function(index, item) {
        var pagename = $(item).attr("data-anchor");
        defaultAuchors.push(pagename);
    });
    Config.auchors = defaultAuchors;

</script>
<!--template-->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/base.js"></script>
<script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
<%
    switch (CurrentCustomizationType)
    {
        case CustomizationType.CheZhuKa:
%>
    <!--车主卡模板放置位置-->
    <%
            break;
        case CustomizationType.Broker:
    %>
    <h5:BrokerTmp runat="server" id="BrokerTmp"/>
    <%
            break;
        case CustomizationType.Dealer:
        case CustomizationType.DealerSale:
    %>
    <h5:DealerTmp runat="server" id="DealerTmp"/>
<%
            break;
    }
%>
<h5:CommonTmpForCustomization runat="server" id="CommonTmpForCustomization"/>
<!--/template-->

<%
    switch (CurrentCustomizationType)
    {
        case CustomizationType.CheZhuKa:
%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/ColorModule.v1.js,carchannel/h5/js/cssummary/CheZhuKa.v1.js?v=201608111115"></script>
    <%--<script src="/Scripts/cssummary/ColorModule.v1.js"></script>
    <script src="/Scripts/cssummary/CheZhuKa.v1.js"></script>--%>
    <%
            break;
        case CustomizationType.Broker:
    %>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/ColorModule.v1.js,carchannel/h5/js/cssummary/BrokerDataModule.v1.js?v=201608111115"></script>
    <%--<script src="/Scripts/cssummary/ColorModule.v1.js"></script>
    <script src="/Scripts/cssummary/BrokerDataModule.v1.js"></script>--%>
    <%
            break;
        case CustomizationType.Dealer:
    %>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/ColorModule.v1.js,carchannel/h5/js/cssummary/DealerDataModule.v1.js?v=201605241622"></script>
    <%--<script src="/Scripts/cssummary/ColorModule.v1.js"></script>
    <script src="/Scripts/cssummary/DealerDataModule.v1.js"></script>--%>
    <%
            break;
        case CustomizationType.DealerSale:
    %>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/ColorModule.v1.js,carchannel/h5/js/cssummary/DealerSaleDataModule.v1.js?v=201605241622"></script>
    <%--<script src="/Scripts/cssummary/ColorModule.v1.js"></script>
    <script src="/Scripts/cssummary/DealerSaleDataModule.v1.js"></script>--%>
    <%
            break;
        case CustomizationType.HuiMaiCheGuWen:
    %>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/cssummary/ColorModule.v1.js,carchannel/h5/js/cssummary/huimaicheguwen.v1.js?v=201608111115"></script>
    <%--<script src="/Scripts/cssummary/ColorModule.v1.js"></script>
    <script src="/Scripts/cssummary/huimaicheguwen.v1.js"></script>--%>
<%
            break;
    }
%>

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
        title: '【<%= BaseSerialEntity.SeoName %>】车型手册-易车网',
        keywords: '<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑',
        desc: '易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。',
        link: 'http://car.h5.yiche.com/<%= BaseSerialEntity.AllSpell %>/?',
        imgUrl: '<%= ShareImgUrl %>'
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
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
</body>
</html>