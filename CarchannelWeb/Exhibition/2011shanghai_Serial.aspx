<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2011shanghai_Serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2011shanghai_Serial" %>
<%@ OutputCache Duration="60" VaryByParam="*" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【图】上海车展<%=_SerialSeoName %>新车_2011上海<%=_SerialSeoName %>新车图片_报价_论坛-易车网</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="Keywords" content="上海车展<%=_SerialSeoName %>，2011上海车展<%=_SerialSeoName %>新车，<%=_SerialSeoName %>汽车报价，易车网" />
    <meta name="Description" content="上海车展<%=_SerialSeoName %>汽车：易车网(BitAuto.com)提供上海车展<%=_SerialSeoName %>新车报价、2011上海车展<%=_SerialSeoName %>图片、上海国际车展<%=_SerialSeoName %>油耗等新车信息。" />
    <!--CSS-->
    <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_css_Manual.shtml"-->
</head>
<body class="sec_page">
    <!--专题头-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
    <!--车型头开始-->
    <div class="header_wzh">
      <div class="htou chexing">
        <h1>2011上海车展首页--看资讯</h1>
      </div>
      </div>
    <!--车型头结束-->
    <!--导航开始-->
    <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_gydh_zdh_Manual.shtml"-->
    <!--导航结束-->
    <!--小导航开始-->
    <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_gydh_xcdh_Manual.shtml"-->
    <!--小导航结束-->
    <!--车型开始 -->
    <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_gydh_cxdh_Manual.shtml"-->
    <!--车型结束 -->
    <!--面包屑开始-->
    <div class="topnav_wzh">
        <div class="topnav_bg">
            <dl>
                <dt></dt>
                <%=_GuilderString %>
            </dl>
            <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_gydh_xcmnzs_Manual.shtml"-->
        </div>
    </div>
    <!--面包屑结束-->
    <div class="bt_con_wx">
        <div class="bt_page">
            <%=_OuterOtherGuilderString %>
            <div class="col-con">
                <%=_FocusContent %> 
                <!--2011上海车展_车型页_车型图片区_车型图片-->              
                <%=_CarImageList %>
                <!--2011上海车展_车型页_车型图片区_车型图片-->
                <!--2011上海车展_车型页_模特区_图解专访-->
                <%=_TuJieList %>                
                <!--2011上海车展_车型页_模特区_图解专访-->
                <!--2011上海车展_车型页_模特区_模特专访-->
                <%=_CarModelList %>
                <!--2011上海车展_车型页_模特区_模特专访-->                
                <!--2011上海车展_车型页_视频区_视频列表-->
                <%=_VideoList%>
                <!--2011上海车展_车型页_视频区_视频列表-->
            </div>
            <div class="col-side">
                <%=_HotModelList %>
                <%=_HotCarTypeList %>
                <%=_BrandNewsList%>
            </div>
            <!-- 网上展厅 -->
            <!--#include file="~/Interface/Exhibition/shanghai2011_ExhibitionInterface_Footer.aspx"-->
        </div>
    </div>
    <script type="text/javascript">
    	document.getElementById("sug_encoding").value = "utf-8";
    </script>
    <!--专题底-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_Content_Manual.shtml"-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_Jiaoben_Manual.shtml"-->
    <!--车展_监测开始-->
    <!--#include file="~/include/special/00001/bitauto_analytics_CheZhan_Manual.shtml"-->
    <!--车展_监测结束-->
    <!--JS-->
    <!--#include file="~/include/z/shanghaichezhan2011/2011shczzhanzhongban/2011shczgonggong/00001/2011shcz_js_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
