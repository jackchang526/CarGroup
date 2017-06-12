<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2010guangzhou-chezhan_MasterBrand.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2010guangzhou_chezhan_MasterBrand" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【图】广州车展<%=_MasterBrandName%>新车_2010广州<%=_MasterBrandName%>新车图片_报价_论坛-易车网</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="Keywords" content="广州车展<%=_MasterBrandName %>，2010广州车展<%=_MasterBrandName %>新车，<%=_MasterBrandName %>汽车报价，易车网" />
    <meta name="Description" content="广州车展<%=_MasterBrandName %>汽车：易车网(BitAuto.com)提供广州车展<%=_MasterBrandName %>新车报价、2010广州车展<%=_MasterBrandName %>新车图片、广州国际车展<%=_MasterBrandName %>油耗等新车信息。" />
    <!--2010广州车展_公用导航_样式脚本开始-->
    <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_css_Manual.shtml"-->
    <!--2010广州车展_公用导航_样式脚本结束-->
</head>
<body class="subpage">
    <!--页头 开始-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
    <!--页头 结束-->
    <!-- 首页头部开始-->
    <div class="zh_head_page">
        <div class="zh_zh_header">
            <h1>
                第8届中国(广州)国际汽车展览会</h1>
            <h2 class="ssxc">
                上市新车</h2>
        </div>
        <!--主导航-->
        <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_gydh_zdh_Manual.shtml"-->
        <!--车型导航-->
        <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_gydh_cxdh_Manual.shtml"-->

        <script language="javascript">        	setMenuItemSelect('menu4');</script>

    </div>
    <!-- 首页头部结束-->
    <div class="zh_nav_box">
        <div class="zh_navigate">
            <strong>您现在是在：</strong> <a href="http://chezhan.bitauto.com/guangzhou-chezhan/"><span>
                2010广州车展</span></a><%=BuilderPageGuilder()%>
        </div>
        <!--2010广州车展_公用导航_热搜搜索开始-->
        <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_gydh_rsss_Manual.shtml"-->
        <!--2010广州车展_公用导航_热搜搜索结束-->
        <script type="text/javascript">
        	document.getElementById("sug_encoding").value = "utf-8";
        </script>
    </div>    
    <%=PageTitle() %>
    <div class="bt_page">
        <div class="bt_page_con">
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
            <!--#include file="~/Interface/Exhibition/guangzhou2010_ExhibitionInterface_Footer.aspx"-->
        </div>
    </div>
    </div>

    <script language="javascript" type="text/javascript">
    	function vote(ukey, itemName, mc) {
    		var script = document.createElement('script');
    		script.type = 'text/javascript';
    		script.src = 'http://www.bitauto.com/vote/vote.aspx?code=' + mc + '&akey=<%=_VoteFormat[_ExhibitionID] %>&ukey=' + ukey +
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
    <!--js-->
    <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_js_Manual.shtml"-->
    <!--2010广州车展_监测开始-->
    <!--#include file="~/include/special/00001/bitauto_analytics_CheZhan_Manual.shtml"-->
    <!--2010广州车展_监测结束-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
