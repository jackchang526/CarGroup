<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2010guangzhou-chezhan_Serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2010guangzhou_chezhan_Serial" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【图】广州车展<%=_SerialSeoName %>_2010广州车展<%=_BrandName %><%=_SerialName %>报价_图片_油耗-易车网</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="Keywords" content="广州车展<%=_SerialSeoName %>，2010广州车展<%=_BrandName %><%=_SerialName %>，广州车展<%=_SerialSeoName %>报价，广州车展<%=_SerialSeoName %>图片" />
    <meta name="Description" content="广州车展<%=_SerialSeoName %>(<%=_BrandName %><%=_SerialName %>)：易车网(BitAuto.com)提供广州车展<%=_BrandName %><%=_SerialName %>新车报价、2010广州车展<%=_SerialSeoName %>图片、广州国际车展<%=_SerialSeoName %>油耗等新车信息。" />
    <!--#include file="~/ushtml/car/bitauto_car_detailed.shtml"-->
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
            <strong>您现在是在：</strong><a href="http://chezhan.bitauto.com/guangzhou-chezhan/"><span>2010年广州车展</span></a>
            <%=PageGuilder()%>  
        </div>
        <!--2010广州车展_公用导航_热搜搜索开始-->
        <!--#include file="~/include/z/gzcz2010/gzcz2010zzb/gzcz2010gonggong/00001/2010gzcz_gydh_rsss_Manual.shtml"-->
        <!--2010广州车展_公用导航_热搜搜索结束-->
         <script type="text/javascript">
         	document.getElementById("sug_encoding").value = "utf-8";
         </script>
    </div>

    <div class="bt_page">
        <div class="bt_page_con">
        <!--集点图-->
        <%=PageFocusChart()%>
        <!--新闻列表-->
        <%=PageNewSpan()%>
        <!--热门排行-->
        <%=HotCarTypeOrder()%>
        <!-- 车型图片 -->
        <%=CarTypeImage() %>
        <!--本主品牌其他车型  -->
        <%=OtherCarType() %>        
        <!-- 主品牌车模 -->
        <%=CarTypeModule() %>
        <!-- 其他同级别车 -->
        <%=SameCsLevelCarType() %>
        <!-- 主品牌视频 -->
        <%=VideoList() %>
        <!-- 网上展厅 -->
        <!--#include file="~/Interface/Exhibition/guangzhou2010_ExhibitionInterface_Footer.aspx"-->
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