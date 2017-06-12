<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsRegionSEOSiteMap.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CsRegionSEOSiteMap" %>

<%@ OutputCache Duration="600" VaryByParam="city" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%= cityName %>品牌列表】-易车网</title>
    <!--#include file="~/ushtml/car/bitauto_car.shtml"-->
    <link href="http://image.bitauto.com/dealer/membersite/skins/default/style/jingxiaoshangSEO.css"
        rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            width: 950px;
            margin: auto;
            position: relative;
        }
    </style>
</head>
<body>
    <!--#include file="~/html/header.shtml"-->
    <div class="bt_smenu">
        <div class="bt_navigate">
            <a target="_blank" href="http://www.bitauto.com" class="bt_logo">易车网</a><div>
                <a href="http://www.bitauto.com/">首页</a> &gt;
                <%= cityName %>
            </div>
        </div>
        <!--#include file="~/html/bt_search.shtml"-->
    </div>
    <div class="bt_page">
        <div class="col-con">
            <h1 class="news">
                <span><span class="caption"><%= cityName %></span></span></h1>
            <div class="s0610_01" style="position: static;">
                <ul>
                    <%= SerialList %>
                </ul>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="col-side">
            <div class="line_box">
                <h3>
                    <span><%= cityName %>新闻</span></h3>
                <ul class="list">
                    <%= CityNews %>
                </ul>
                <div class="more">
                </div>
            </div>
            <!--#   include file="~/include/special/seo/00001/bitauto_city_sitemap_Manual.shtml"-->
            <!--内容如下-->
            <div class="line_box">
                <h3>
                    <span>汽车热点城市地图</span></h3>
                <ul class="list">
                    <li><a target="_blank" href="http://beijing.bitauto.com/sitemap.html">北京</a> <a target="_blank" href="http://changchun.bitauto.com/sitemap.html">长春</a> <a target="_blank" href="http://changsha.bitauto.com/sitemap.html">长沙</a> </li>
                    <li><a target="_blank" href="http://chengdu.bitauto.com/sitemap.html">成都</a> <a target="_blank" href="http://fuzhou.bitauto.com/sitemap.html">福州</a> <a target="_blank" href="http://guangzhou.bitauto.com/sitemap.html">广州</a> </li>
                    <li><a target="_blank" href="http://haerbin.bitauto.com/sitemap.html">哈尔滨</a> <a target="_blank" href="http://hangzhou.bitauto.com/sitemap.html">杭州</a> <a target="_blank" href="http://jinan.bitauto.com/sitemap.html">济南</a> </li>
                    <li><a target="_blank" href="http://nanjing.bitauto.com/sitemap.html">南京</a> <a target="_blank" href="http://qingdao.bitauto.com/sitemap.html">青岛</a> <a target="_blank" href="http://shanghai.bitauto.com/sitemap.html">上海</a> </li>
                    <li><a target="_blank" href="http://shenzhen.bitauto.com/sitemap.html">深圳</a> <a target="_blank" href="http://shenyang.bitauto.com/sitemap.html">沈阳</a> <a target="_blank" href="http://shijiazhuang.bitauto.com/sitemap.html">石家庄</a> </li>
                    <li><a target="_blank" href="http://taiyuan.bitauto.com/sitemap.html">太原</a> <a target="_blank" href="http://tianjin.bitauto.com/sitemap.html">天津</a> <a target="_blank" href="http://wuhan.bitauto.com/sitemap.html">武汉</a> </li>
                    <li><a target="_blank" href="http://xian.bitauto.com/sitemap.html">西安</a> <a target="_blank" href="http://zhengzhou.bitauto.com/sitemap.html">郑州</a> <a target="_blank" href="http://chongqing.bitauto.com/sitemap.html">重庆</a> </li>
                    <li><a target="_blank" href="http://baoding.bitauto.com/sitemap.html">保定</a> <a target="_blank" href="http://dalian.bitauto.com/sitemap.html">大连</a> <a target="_blank" href="http://daqing.bitauto.com/sitemap.html">大庆</a> </li>
                    <li><a target="_blank" href="http://dongguan.bitauto.com/sitemap.html">东莞</a> <a target="_blank" href="http://foshan.bitauto.com/sitemap.html">佛山</a> <a target="_blank" href="http://guiyang.bitauto.com/sitemap.html">贵阳</a> </li>
                    <li><a target="_blank" href="http://haikou.bitauto.com/sitemap.html">海口</a> <a target="_blank" href="http://hefei.bitauto.com/sitemap.html">合肥</a> <a target="_blank" href="http://huhehaote.bitauto.com/sitemap.html">呼和浩特</a> </li>
                    <li><a target="_blank" href="http://jinhua.bitauto.com/sitemap.html">金华</a> <a target="_blank" href="http://kunming.bitauto.com/sitemap.html">昆明</a> <a target="_blank" href="http://lanzhou.bitauto.com/sitemap.html">兰州</a> </li>
                    <li><a target="_blank" href="http://luoyang.bitauto.com/sitemap.html">洛阳</a> <a target="_blank" href="http://nanchang.bitauto.com/sitemap.html">南昌</a> <a target="_blank" href="http://nanning.bitauto.com/sitemap.html">南宁</a> </li>
                    <li><a target="_blank" href="http://ningbo.bitauto.com/sitemap.html">宁波</a> <a target="_blank" href="http://quanzhou.bitauto.com/sitemap.html">泉州</a> <a target="_blank" href="http://suzhou.bitauto.com/sitemap.html">苏州</a> </li>
                    <li><a target="_blank" href="http://tangshan.bitauto.com/sitemap.html">唐山</a> <a target="_blank" href="http://wenzhou.bitauto.com/sitemap.html">温州</a> <a target="_blank" href="http://wulumuqi.bitauto.com/sitemap.html">乌鲁木齐</a> </li>
                    <li><a target="_blank" href="http://wuxi.bitauto.com/sitemap.html">无锡</a> <a target="_blank" href="http://xiamen.bitauto.com/sitemap.html">厦门</a> <a target="_blank" href="http://xuzhou.bitauto.com/sitemap.html">徐州</a> </li>
                    <li><a target="_blank" href="http://yantai.bitauto.com/sitemap.html">烟台</a> <a target="_blank" href="http://yichang.bitauto.com/sitemap.html">宜昌</a> <a target="_blank" href="http://yinchuan.bitauto.com/sitemap.html">银川</a> </li>
                    <li><a target="_blank" href="http://zibo.bitauto.com/sitemap.html">淄博</a> </li>
                </ul>
                <div class="more">
                </div>
            </div>
        </div>
    </div>
    <!--  -->
    <div class="footer0904">
        <div class="footer_c">
            免长途费购车咨询热线：4008-898-198 传真：(010)68492726 <a href="http://huodong.bitauto.com/onlinefeed/default.aspx"
                target="_blank">[易车在线意见反馈]</a>
        </div>
        <div class="footer_about">
            <ul class="about">
                <li class="first"><a href="http://corp.bitauto.com/page/about/ycjs.shtml" target="_blank">关于易车</a></li>
                <li><a href="http://corp.bitauto.com/page/business/hlwmt.shtml" target="_blank">旗下业务</a></li>
                <li><a href="http://corp.bitauto.com/page/news/list_1.shtml" target="_blank">新闻中心</a></li>
                <li><a href="http://corp.bitauto.com/page/human_resource/list_1.shtml" target="_blank">招贤纳士</a></li>
                <li><a href="http://corp.bitauto.com/page/connection/lswm.shtml" target="_blank">联系我们</a></li>
                <li><a href="http://corp.bitauto.com/page/law_statement/flsm.shtml" target="_blank">服务声明</a></li>
                <li><a href="http://www.bitauto.com/map/map.shtml" target="_blank">网站地图</a></li>
                <li><a href="http://easy.bitauto.com/" target="_blank">车易通</a></li>
                <li class="last"><a href="http://baa.bitauto.com/" target="_blank">互动社区</a></li>
            </ul>
        </div>
        <div class="footer_c">
            CopyRight 2010 BitAuto,All Rights Reserved 版权所有 北京易车互联信息技术有限公司
        </div>

        <script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>

        <script src="http://js.inc.baa.bitautotech.com/bitauto/bitauto.login.js" type="text/javascript"></script>

        <script src="http://www.bitauto.com/themes/2009/js/headcommon.js" type="text/javascript"></script>

        <script type="text/javascript" src="http://www.bitauto.com/themes/2009/js/search.js"></script>

    </div>
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
