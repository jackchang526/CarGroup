<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2010beijing_MasterBrand.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2010beijing_MasterBrand" %>

<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【北京车展<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>】-易车网BitAuto.com
    </title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="Keywords" content="北京车展<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>" />
    <meta name="Description" content="<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>:易车网(BitAuto.com)车型频道为您提供全国最新<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>报价,海量<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>图片,热门<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>论坛,权威<%=MasterBrandXmlNode.GetAttribute("MasterSEOName") %>参数配置、安全评测、油耗、口碑、答疑、视频、经销商等,是全国数千万购车意向客户首选汽车导购网站。" />
    <!--2010北京车展_公用导航_样式脚本开始-->
    <!--#include file="~/include/z/bj2010/ing/nav/00001/10bj_gydh_ysjb_Manual.shtml"-->
    <!--2010北京车展_公用导航_样式脚本结束-->
</head>
<body>
    <!--2010北京车展_公用导航_页头 开始-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
    <!--2010北京车展_公用导航_页头 结束-->
    <div class="zh_head_page zh_cx">
        <h1>
            2010北京车展</h1>
        <!--2010北京车展_公用导航_主导航 开始-->
        <!--#include file="~/include/z/bj2010/ing/nav/00001/10bj_gydh_zdh_Manual.shtml"-->
        <!--2010北京车展_公用导航_主导航 结束-->
    </div>
    <!--2010北京车展_公用导航_车型导航 开始-->
    <!--#include file="~/include/z/bj2010/ing/nav/00001/10bj_gydh_cxdh_Manual.shtml"-->
    <!--2010北京车展_公用导航_车型导航 结束-->
    <div class="zh_nav_box">
        <div class="zh_navigate">
            <strong>您现在是在：</strong><span><a href="http://chezhan.bitauto.com/">2010北京车展</a></span><%=BuilderPageGuilder()%></div>
        <!--#include file="~/include/z/bj2010/ing/nav/00001/10bj_gydh_rsss_Manual.shtml"-->
    </div>
    <div class="clear">
    </div>
    <!--页面logo头-->
    <%=PageTitle()%>
    <div class="bt_page">
        <!--焦点图-->
        <%=PageFocusChart() %>
        <!--新闻块-->
        <%=PageNewSpan()%>
        <!--热门车型排行-->
        <%=HotCarTypeOrder()%>
        <!--主品牌下面的子品牌-->
        <%=ContainsSerialList() %>
        <!--主品牌下面车模列表-->
        <%=CarTypeModule() %>
        <!--视频列表-->
        <%=VideoList() %>
        <!-- 展馆其他厂商 -->
        <%=OtherMasterBrand() %>
        <!-- 展馆列表 -->
        <!--#include file="~/Interface/Exhibition/beijing2010_ExhibitionInterface_Footer.aspx"-->
    </div>

    <script language="javascript" type="text/javascript">
    	function vote(ukey, itemName, mc) {
    		var script = document.createElement('script');
    		script.type = 'text/javascript';
    		script.src = 'http://www.bitauto.com/vote/vote.aspx?code=' + mc + '&akey=BitAuto.Chezhan.2010&ukey=' + ukey +
'&errorcallback=VoteResult&itemName=' + encodeURIComponent(itemName);
    		document.body.appendChild(script);
    	};
    	function VoteResult(content) {
    		alert('感谢您的参与，投票成功！');
    	};
    </script>

    <!--2010北京车展_页尾开始-->
    <!--#include file="~/include/z/bj2010/ing/nav/00001/10bj_gydh_cxyw_Manual.shtml"-->
    <!--2010北京车展_页尾结束-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_Jiaoben_Manual.shtml"-->
    <!--2010北京车展_监测开始-->
    <!--#include file="~/include/special/00001/bitauto_analytics_CheZhan_Manual.shtml"-->
    <!--2010北京车展_监测结束-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>