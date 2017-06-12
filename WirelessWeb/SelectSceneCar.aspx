﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSceneCar.aspx.cs" Inherits="WirelessWeb.SelectSceneCar" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>【汽车标志_汽车品牌大全_<%=DateTime.Now.ToString("yyyy") %>年热门车型推荐】-手机易车网</title>
	<meta name="keywords" content="汽车大全,汽车标志,车型大全,<%=DateTime.Now.ToString("yyyy") %>年热门车型，易车网" />
	<meta name="description" content="手机易车网选车频道，为您提供各种汽车品牌型号及报价信息，主要涵盖：“汽车品牌、汽车标志、汽车报价、<%=DateTime.Now.ToString("yyyy") %>年热门车型推荐等,是您选车购车的第一网络平台." />
	<!--#include file="~/ushtml/0000/myiche2016_cube_qingjingxuanche-1184.shtml"-->
    <script type="text/javascript">
        var _SearchUrl = '<%=_SearchUrl %>';
    </script>
</head>
<body>
    <!-- header start -->
	<div class="op-nav">
		<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
		<div class="tt-name">
			<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>条件选车</h1>
		</div>
		<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
	</div>
    <!-- header end -->
    <!-- 标签切换 start -->
    <div class="first-tags">
        <ul>
            <li id="xc_brand"><a href="/brandlist.html" data-channelid="27.150.1436"><span>按品牌</span></a></li>
            <li id="xc_condition"><a href="/" data-channelid="27.150.1435"><span>按条件</span></a></li>
            <li class="current tags-new""><a href="/scenelist.html"  data-channelid="27.150.1416"><span>按情景<b><i>NEW<i></i></i></b></span></a></li>
        </ul>
    </div>
    <div class="qingj-filter b-shadow">
        <div class="qingj-filter-info">
            <div class="qingj-filter-left">
                选择“<%=_wordName %>”，<br>
                需要如下配置：
            </div>
            <div class="qingj-filter-right" data-channelid="27.150.1417">
                为什么选<br>
                这些配置？
            </div>
        </div>
        <div class="qingj-filter-list">
            <%=allCheckBoxHtml %>
        </div>
    </div>
    <div class="pd15"><div class="car-list-tt">共<span></span>个车型符合已选配置</div></div>
    <div class="second-tags-scroll-box car-second-tags-scroll-box">
    <div class="pd15">
        <div class="second-tags-scroll price-range">
            <ul>
                <li class="current" id="price0" data-param=""><a href="javascript:;"><span>全部</span></a></li>
                <li id="price1" data-param="p=0-18"><a href="javascript:;"><span>18万以下</span></a></li>
                <li id="price2" data-param="p=18-25"><a href="javascript:;"><span>18-25万</span></a></li>
                <li id="price3" data-param="p=25-40"><a href="javascript:;"><span>25-40万</span></a></li>
                <li id="price4" data-param="p=40-9999"><a href="javascript:;"><span>40万以上</span></a></li>
            </ul>
        </div>
    </div>
    <div class="right-mask"></div>
</div>
    <!-- 车型图片 start -->
    <div class="car-list2"></div>  
    <!-- 车型图片 end --> 
    <div class="clear"></div>
    <!-- 分页start -->
    <div class="m-pages b-shadow" style="display:none">
        <a href="javascript:void(0)" class="m-pages-pre m-pages-none">上一页</a>
        <div class="m-pages-num">
            <div class="m-pages-num-con"><span id="curPageIndex">1</span>/<span id="totalPage">1</span></div>
            <div class="m-pages-num-arrow"></div>
        </div>
        <select class="m-pages-select">
        </select>
        <a href="javascript:void(0)" class="m-pages-next">下一页</a>
    </div>

    <!-- 弹出层 start -->
    <div class="qingj-mask" style="display: none;"></div>
    <div class="qingj-popup-box qingj-popup-box-hide">
        <div class="qingj-popup">
            <div class="qingj-popup-tt">推荐配置的理由</div>
            <p><%=_wordContent %></p>
        </div>
        <div class="qingj-popup-close"></div>
        <div class="qingj-anti">
            <div class="qingj-book1"></div>
            <div class="qingj-book2"></div>
            <div class="qingj-book3"></div>
            <div class="qingj-cross"></div>
            <div class="qingj-cross qingj-cross2"></div>
            <div class="qingj-oval"></div>
            <div class="qingj-oval qingj-oval2"></div>
        </div>
    </div>
    <!-- 弹出层 end -->    <div class="jump-pop" style="display:none">
        <span>请至少选择一个条件噢！</span>
    </div>
    <script type="text/javascript">
        $(function () {
            $('.qingj-filter-right').click(function () {
                $('.qingj-mask').show();
                $('.qingj-popup-box').removeClass('qingj-popup-box-hide');
                $('.qingj-anti').addClass('qingj-anti-move');
            });
            $('.qingj-popup-close').click(function () {
                $('.qingj-mask').hide();
                $('.qingj-popup-box').addClass('qingj-popup-box-hide');
                $('.qingj-anti').removeClass('qingj-anti-move');
            });
        })
    </script>
    <script src="http://image.bitautoimg.com/carchannel/WirelessJs/selectscenecar.min.js?v=2016092315" type="text/javascript"></script>
    <script type="text/javascript">
        <%=GenerateSearchInitScript() %>
        SelectSceneCarTool.initPageCondition();
    </script>
    <div class="footer15">
         <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <div class="breadcrumb">
            <div class="breadcrumb-box">
		    <a href="http://m.yiche.com/">首页</a> &gt; <a href="/scenelist.html">情景选车</a> 
            </div>
	    </div>
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    
</body>
</html>
