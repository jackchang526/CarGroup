<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectBrand.aspx.cs" Inherits="WirelessWeb.SelectBrand" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【<%=_masterName %>汽车】<%=_masterName %>汽车报价_图片_经销商_论坛-手机易车网</title>
	<meta name="keywords" content="<%=_masterName %>,<%=_masterName %>汽车,<%=_masterName %>汽车报价,易车网" />
	<meta name="description" content="<%=_masterName %>汽车频道为您提供最新<%=_masterName %>汽车报价,<%=_masterName %>汽车图片,<%=_masterName %>汽车新闻,<%=_masterName %>视频,<%=_masterName %>口碑,<%=_masterName %>问答,<%=_masterName %>二手车等,查<%=_masterName %>汽车最新报价,就上易车网" />
	<!--#include file="~/ushtml/0000/myiche2014_cube_xuanchepage-759.shtml"-->
</head>
<body>
	<div class="mbt-page">
		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>品牌选车</h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
		<!-- 主品牌 start -->
		<div class="choose-car-name-close bybrand_list">
			<a href="/<%=_masterSpell %>/">
				<div class="brand-logo-none-border m_<%=_masterId %>_b"></div>
				<span class="brand-name"><%=_masterName %></span>
			</a>
		</div>
		<!-- 主品牌 end -->
		<%//品牌列表 start %>
		<%=_brandListHtml %>
		<%//品牌列表 end %>
	</div>
	<script type="text/javascript">
		var url = "http://m.yiche.com/";
	</script>
	<div class="footer15">
		<!--搜索框-->
		<!--#include file="~/html/footersearch.shtml"-->
		<!--#include file="~/html/footerV3.shtml"-->
	</div>
	<div class="float-r-box">
		<!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
	</div>
</body>
</html>
