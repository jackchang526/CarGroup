<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarshowSerial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarshowSerial" %>

<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="300" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=serialBase.SerialShowName %>-广州车展|广州车展<%=serialBase.SerialShowName %>图片_2009广州车展】-
        易车网BitAuto.com</title>
    <meta name="keywords" content="<%=serialBase.SerialShowName %>车展,<%=serialBase.SerialShowName %>图片,<%=serialBase.SerialShowName %>美女车模" />
    <meta name="description" content="<%=serialBase.SerialShowName %>2009广州车展，易车网24小时为您提供全程视频图文直播<%=serialBase.SerialShowName %>车展实况,提供最新最快的<%=serialBase.SerialShowName %>车展信息" />
    <!--#include file="~/ushtml/car/bitauto_car_summary.shtml"-->
    <!--#include file="~/ushtml/autoshow_gz2009/autoshow_2009gz.shtml"-->
</head>
<body>
    <!-- 头 -->
    <!--#include file="~/html/exhibitionheader.shtml"-->
    <div class="bt_page">
        <div class="col-all">
            <div class="lh_er_pic lh_er_pic_huanan p_mt10">
                <h1>
                    2009广州国际车展</h1>
            </div>
            <div class="lh_position">
                <p>
                    <a href="http://www.bitauto.com">易车网</a> <a href="http://car.bitauto.com">车型</a><em>&gt;</em><a
                        href="http://chezhan.bitauto.com/guangzhou-chezhan/gd_2009/">2009广州车展</a><em>&gt;</em><span><a
                            href="http://Chezhan.bitauto.com/Guangzhou-chezhan/gd_2009/zhanguan/<%=pavilion.Replace("馆","") %>"><%=pavilion %></a></span><em>&gt;</em><span><a
                                href="http://chezhan.bitauto.com/guangzhou-chezhan/2009/<%=serialBase.MasterbrandSpell %>/"><%=serialBase.MasterBrandName%></a></span><em>&gt;</em><span><%=serialBase.SerialShowName %></span></p>
            </div>
        </div>
        <!-- -->
        <div class="col-all">
            <%= CsHead %>
        </div>
        <!-- -->
        <!-- 焦点图 -->
        <div class="col-sub">
            <%=focusImageHtml %>
        </div>
        <!-- 头条 -->
        <div class="col-main">
            <%=topNewsHtml %>
        </div>
        <!-- 热门车型排行榜 -->
        <div class="col-side">
            <%=hotCarHtml %>
        </div>
        <div class="clear">
        </div>
        <!-- 子品牌 车展图 车型图 -->
        <div class="col-all">
            <div class="line_box">
                <%=serialImagesHtml %>
            </div>
        </div>
        <!-- 车模 -->
        <div class="col-all">
            <div class="line_box p_mo1">
                <%=modelImagesHtml %>
            </div>
            <div class="clear"></div>
        </div>
        <!-- 其它车型 -->
        <div class="col-all">
            <div class="line_box">
                <%=otherSerialHtml%>
            </div>
        </div>
        <!-- 视频 -->
        <div class="col-all">
            <div class="line_box">
                <%=masterVideosHtml %>
            </div>
            <div class="clear"></div>
        </div>
        <!-- 自主车其它车型 -->
        <%=sameTypeSerialHtml%>
        <!--#include file="~/html/chezhanallbrand.shtml"-->

    </div>
    <!--#include file="~/html/exhibitionfooter.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>

