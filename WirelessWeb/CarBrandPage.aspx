﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPage.aspx.cs" Inherits="WirelessWeb.CarBrandPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【<%=_brandName %>】<%=_brandName %>汽车报价_图片_<%=DateTime.Now.Year+_brandName %>新款车型-手机易车网</title>
	<meta name="Keywords" content="<%=_brandName %>,<%=_brandName %>汽车,<%=_brandName %>汽车报价,<%=DateTime.Now.Year+_brandName %>新款车型-手机易车网,car.m.yiche.com" />
	<meta name="Description" content="<%=_brandName %>汽车:提供最新<%=_brandName %>汽车报价,<%=_brandName %>汽车图片,<%=_brandName %>汽车新闻,视频,口碑,问答,二手车等。<%=_brandName %>在线询价、低价买车尽在手机易车网。" />
	<!--#include file="~/ushtml/0000/myiche2015_cube_pinpai-1077.shtml"-->
</head>
<body>
	<div class="mbt-page">
		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%=_brandReplaceName %></h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
		<%//品牌列表 start %>
		<%=_brandListHtml %>
		<%//品牌列表 end %>
	</div>
	<script type="text/javascript">
		var url = "http://car.m.yiche.com/<%=_masterSpell%>/";
	</script>
	<%// --footer start--%>
	<div class="footer15">
		<!--搜索框-->
		<!--#include file="~/html/footersearch.shtml"-->
		<!--#include file="~/html/footerV3.shtml"-->
	</div>
	<div class="float-r-box">
		<!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
	</div>
	<%// --footer end--%>
</body>
</html>
