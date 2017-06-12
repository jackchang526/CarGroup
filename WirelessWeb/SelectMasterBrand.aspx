﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMasterBrand.aspx.cs" Inherits="WirelessWeb.SelectMasterBrand" %>

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
	<!--#include file="~/ushtml/0000/myiche2015_cube_brand_style-996.shtml"-->
</head>
<body>
	<div class="mbt-page">
		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>选车</h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
		<div class="brandfilter">
			<!-- 标签切换 start -->
			<div class="first-tags">
				<ul>
					<li class="current"><a href="/brandlist.html"  data-channelid="27.150.1436"><span>按品牌</span></a></li>
					<li id="xc_condition"><a href="/" data-channelid="27.150.1435"><span>按条件</span></a></li>
                    <li id="xc_scene"  class="tags-new"><a href="/scenelist.html" data-channelid="27.150.1416"><span>按情景<b><i>NEW<i></i></i></b></span></a></li>
				</ul>
			</div>
			<!-- 标签切换 end -->

			<!-- 按品牌选车 start -->
			<div class="brand-list bybrand_list">
				<div class="tt-small phone-title" data-key="#">
					<span>推荐</span>
				</div>
				<div class="brand-hot">
					<%if (re == "true")
	   { %>
					<!--#include file="~/html/HomepageHotBrandSEO.shtml"-->
					<%}
	   else
	   { %>
					<!--#include file="~/html/HomepageHotBrand.shtml"-->
					<%} %>
				</div>
				<%=brandListHtml%>
			</div>
			<!-- 按品牌选车 end -->

			<!-- 字母导航 start -->
			<div class="fixed-nav" style="opacity: 1; display: block;">
				<ul class="rows-box">
					<li><a href="#">#</a></li>
					<%=letterListHtml %>
				</ul>
			</div>
			<div class="alert" style="display: none;"><span>Y</span></div>
			<!-- 字母导航 end -->
		</div>

		<%-- <ins id="div_2a05eecc-6416-4c0b-ab7e-018e54388d3a" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="2a05eecc-6416-4c0b-ab7e-018e54388d3a">  
           </ins>  
		--%>
		<!-- bottom search start -->
		<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=201209"></script>--%>
		<!-- bottom search end -->
	</div>

	<!--车型层 start-->
	<div class="leftmask mark leftmask3" style="display: none;"></div>
	<div class="leftPopup car-model car" data-back="leftmask3" style="display: none;" data-key="car">
		<div class="swipeLeft swipeLeft-sub">
			<div class="loading">
				<img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="" />
				<p>正在加载...</p>
			</div>
		</div>
	</div>
	<!--车型层 end-->
	<!--loading模板 start -->
	<div class="template-loading" style="display: none;">
		<div class="loading">
			<img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" />
			<p>正在加载...</p>
		</div>
	</div>
	<!--loading模板 end -->
	<!--车型模板 start-->
	<script type="text/template" id="carTemplate">
		<div class="choose-car-name-close bybrand_list">
			<div class="brand-logo-none-border m_9_b"></div>
			<span class="brand-name"></span>
			<!-- <a href="#" class="choose-car-btn-close">关闭</a> -->
		</div>
		<div class="clear"></div>
		{ for(var i = 0 ; i < list.length ; i++){ }
						<!-- 车款列表 start -->
		{if(list[i].Child.length > 0){ }
		<div class="tt-small">
			<span>{= list[i].BrandName }</span>
		</div>
		{}}
		<!-- 图文混排横向 start -->
		<div class="pic-txt-h pic-txt-9060">
			<ul>
				{ for(var j = 0 ; j < list[i].Child.length ; j++){ }
							<li>
								<a class="imgbox-2" href="/{=list[i].Child[j].Allspell}/" data-id="{= list[i].Child[j].SerialId}">
									<div class="img-box">
										<img src="{= list[i].Child[j].ImageUrl}" alt="" />
									</div>
									<div class="c-box">
										<h4>{= list[i].Child[j].SerialName }</h4>
										<p><strong>{=list[i].Child[j].Price}</strong></p>
									</div>
								</a>
							</li>
				{}}
			</ul>
		</div>
		<!-- 图文混排横向 end -->
		{}}
	</script>
	<!--车型模板 end-->

	<!--baidu ad star-->
	<%--<script type="text/javascript">
        /*yiche wap news*/
        var cpro_id = "u1409352";
    </script>
    <script src="http://cpro.baidustatic.com/cpro/ui/cm.js" type="text/javascript"></script>--%>
	<!--baidu ad end-->
	<script type="text/javascript">
		var url = "http://m.yiche.com/";
	</script>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll.js?v=20150828"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/model.js?v=20151012"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/uimg/mbt2015/js/note.js?v=20151012"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js,carchannel/wirelessjs/v2/note.js?v=20170105"></script>
	<script type="text/javascript">
		$(function () {
			if (typeof (bitLoadScript) == "undefined")
				bitLoadScript = function (url, callback, charset) {
					var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
					if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
					else { s.onload = function () { if (callback) callback(); }; }
					s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
				};

			bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
				PageAreaStatistics.init("281");
			}, 'utf-8');
			var $body = $('body');
			//笔记本滑动效果
			$body.trigger('note');
			<%if (re != "ture")
	 {%>
			//右侧子品牌效果
			$body.trigger('rightswipe3', { model_hide: true });			<%}%>
		});
	</script>
	<div class="footer15">
		<!--搜索框-->
		<!--#include file="~/html/footersearch.shtml"-->
		<div class="breadcrumb">
			<div class="breadcrumb-box">
				<a href="http://m.yiche.com/">首页</a> &gt; <span>选车</span>
			</div>
		</div>
		<!--#include file="~/html/footerV3.shtml"-->
	</div>
	<div class="float-r-box">
		<!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
	</div>
</body>
</html>

