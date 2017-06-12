﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404error.aspx.cs" Inherits="WirelessWeb._404error" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>404</title>
    <!--#include file="~/ushtml/0000/m_subpage-354.shtml"-->
</head>
<body>
    <div class="mbt-page">
       <!-- header start -->
	<div class="op-nav">
		<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
		<div class="tt-name">
			<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>无效页面</h1>
		</div>
		<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
	</div>
	<div class="op-nav-mark" style="display: none;"></div>
	<!-- header end -->
        <div class="m-result-page-box m-result-page-fail m-result-page-multi-line">
            很抱歉，您访问的页面不存在。
        </div>
        <a class="m-btn-more-news m-btn-gray" href="/">返回首页</a>
    </div>
    <script type="text/javascript">
        var url = "http://m.yiche.com/";
    </script>
    <div class="footer15">
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
	    <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
</body>
</html>
