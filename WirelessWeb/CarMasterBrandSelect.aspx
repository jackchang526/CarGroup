<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterBrandSelect.aspx.cs"
	Inherits="WirelessWeb.CarMasterBrandSelect" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【汽车大全|汽车标志_汽车标志大全_热门车型大全】-手机易车网</title>
	<meta name="keywords" content="汽车大全,汽车标志,车型大全,汽车标志大全" />
	<meta name="description" content="汽车大全:手机易车网车型大全频道为您提供各种汽车品牌型号信息,包括汽车报价,汽车标志,汽车图片,汽车经销商,汽车油耗,汽车资讯,汽车点评,汽车问答,汽车论坛等等……" />
	<!--#include file="~/ushtml/0000/m_subpage-354.shtml"-->
</head>
<body>
	<div class="mbt-page">
		<!--#include file="~/html/header.shtml"-->
        <div class="b-return">
	    <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
	    <span>选车</span>
        </div>
		<div class="clear">
		</div>
		<!-- 标签切换 start -->
		<div class="m-tags">
			<ul>
				<li><a href="/"><span>快速选车</span></a></li>
				<li class="current"><a href="/brandlist.html"><span>按品牌</span></a></li>
				<li><a href="<%=this._SearchUrl %>"><span>条件自选</span></a></li>
			</ul>
		</div>
		<!-- 标签切换 end -->
		<!-- 按品牌选车 start -->
		<!--#include file="~/include/pd/2012/wap/00001/201209_wap_cxapp_gb2312_Manual.shtml" -->
		<!-- 按品牌选车 end -->
		<!-- bottom search start -->
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchor.js?v=201209"></script>
		<!-- bottom search end -->
	</div>
    <script type="text/javascript">
    var url = "http://m.yiche.com/";
    </script>
	<!--#include file="~/html/footer.shtml"-->
</body>
</html>
