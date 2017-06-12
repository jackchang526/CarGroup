<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2012beijing_Serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2012beijing_Serial" %>

<%@ OutputCache Duration="300" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>【图】北京车展<%=_SerialSeoName %>新车_2012北京<%=_SerialSeoName %>新车图片_报价_论坛-易车网</title>
    <!--CSS-->
    <!--#include file="~/ushtml/0000/wzh-231.shtml"-->
    <meta name="Keywords" content="北京车展<%=_SerialSeoName %>，2012北京车展<%=_SerialSeoName %>新车，<%=_SerialSeoName %>汽车报价，易车网" />
    <meta name="Description" content="北京车展<%=_SerialSeoName %>汽车：易车网(BitAuto.com)提供北京车展<%=_SerialSeoName %>新车报价、2012北京车展<%=_SerialSeoName %>图片、北京国际车展<%=_SerialSeoName %>油耗等新车信息。" />
</head>
<body>
    <!--头 start-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
    <!--头 end-->
    <!--头图 start-->
    <div class="bt_sub_head">
        <div class="sub_header_swd">
            <h1>
                2012北京车展</h1>
            <h2>
                网上展厅</h2>
        </div>
    </div>
    <!--头图 end-->
    <!--导航 start-->
    <!--#include file="~/include/z/bjcz/2012/shouye/00001/201204_bjczsy_dh_gb2312_Manual.shtml"-->
    <!--导航 end-->
    <!--车切换 start-->
    <div class="bt_con_swd carbox_swd_index">
        <div class="nav_con_swd h0">
        </div>
        <!--面包屑 start-->
        <div class="breadcrumbs">
            您现在是在：<a href="http://chezhan.bitauto.com/beijing/" target="_blank">2012北京车展</a>
            <b>&gt;</b>
            <%= _GuilderString %>
        </div>
        <!--面包屑 end-->
        <!--搜索 start-->
        <!--#include file="~/include/z/bjcz/2012/shouye/00001/201204_bjczsy_ss_gb2312_Manual.shtml"-->
        <!--搜索 end-->
        <div class="clear">
        </div>
    </div>
    <!--车切换 end-->
    <div class="bt_con_wx">
        <div class="bt_page">
            <%=_OuterOtherGuilderString%>
            <div class="col-con">
                <%=_FocusContent %>
                <!--2011上海车展_车型页_车型图片区_车型图片-->
                <%=_CarImageList %>
                <!--2011上海车展_车型页_车型图片区_车型图片-->
                <!--2011上海车展_车型页_模特区_图解专访-->
                <%=_TuJieList %>
                <!--2011上海车展_车型页_模特区_图解专访-->
                <!--2011上海车展_车型页_模特区_模特专访-->
                <%=_CarModelList%>
                <!--2011上海车展_车型页_模特区_模特专访-->
                <!--2011上海车展_车型页_视频区_视频列表-->
                <%=_VideoList%>
                <!--2011上海车展_车型页_视频区_视频列表-->
            </div>
            <div class="col-side">
                <%--<%=_HotModelList %>--%>
                <div class="line_box">
                    <h3>
                        <span>热门车模</span></h3>
                    <div class="hotrank_wzh hotrank2_wzh rank_meinv">
                        <!--#include file="~/include/z/bjcz/2012/shouye/00001/201204_bjczsy_jdq_mnphb_gb2312_Manual.shtml"-->
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <%=_HotCarTypeList %>
            </div>
            <!-- 网上展厅 -->
            <!--#include file="~/include/z/bjcz/2012/common/00001/201204_bjcz_wszt_gb2312_Manual.shtml"-->
        </div>
    </div>
    <!--footer start-->
    <!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_Manual.shtml"-->
    <!--footer end-->
    <!--IE6 png 透明-->
    <!--[if lt IE 7]>
<script type="text/javascript" src="js/unitpngfix.js"></script>
<![endif]-->
    <!--监测代码-->
    <!--#include file="~/include/special/00001/bitauto_analytics_chezhan_Manual.shtml"-->
    <!--#include file="~/include/z/bjcz/2012/shouye/00001/201204_bjczsy_js_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
