<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPingCeCompareList.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.CarPingCeCompareList" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>
		<%= titleForSEO%>-易车网</title>
	<meta name="keywords" content="汽车评测,评测对比,易车网" />
	<meta name="description" content="汽车评测对比:易车网车型频道为您提供最详细的汽车评测对比。包括汽车导语、外观、内饰、空间、视野、灯光、动力、操控、舒适性、油耗、配置等详细评测对比" />
	<!--#include file="~/ushtml/0000/car_pickbceping_comparetool_v2-251.shtml"-->
	<script type="text/javascript">
    <!--
		var tagIframe = null;
		var currentTagId = 25; 	//当前页的标签ID

		// 图片缩放
		function changeImageHeightWidth(imgObj) {
			var ThroldValue = 400;
			var imgWidth = parseInt(imgObj.offsetWidth);
			var imgHeight = parseInt(imgObj.offsetHeight);
			if (imgObj.offsetWidth <= ThroldValue) {
				return;
			}
			imgObj.style.width = ThroldValue + "px";
			imgObj.style.height = imgHeight * (ThroldValue / imgWidth) + "px";
		}
      -->
	</script>
	<style type="text/css">
		table.table_pics
		{
			width: 415px;
		}
		table.table_pics td
		{
			width: 200px;
		}
		table.table_pics td p
		{
			width: 200px !important;
		}
		table.table_pics td img
		{
			width: 200px !important;
			height: 133px !important;
		}
		table.table_pics td p img
		{
			width: 200px !important;
		}
	</style>
</head>
<body>
	<!--#include file="~/html/header2012.shtml"-->
	<!--smenu start-->
	<div class="bt_page">
		<div class="bt_smenuNew">
			<div class="bt_navigateNew"><a class="bt_logo bt_logo_channel" target="_blank" href="http://www.bitauto.com/">易车</a> <a class="bt_channel" href="http://car.bitauto.com/chexingduibi/">对比工具</a> </div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchNew.shtml"-->
			</div>
			<div id="divSerialSummaryMianBaoAD" class="top_ad02"><ins id="div_ba10f730-0c13-4dcf-aa81-8b5ccafc9e21" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="ba10f730-0c13-4dcf-aa81-8b5ccafc9e21"> </ins></div>
		</div>
	</div>
	<!--page start-->
	<div class="bt_page">
		<div class="allTab">
			<div class="tabrightlink"><a href="/qichebaoxianjisuan/" target="_blank">保险计算</a></div>
			<div class="tabrightlink"><a href="/qichedaikuanjisuanqi/" target="_blank">贷款购车</a></div>
			<div class="tabrightlink"><a href="/gouchejisuanqi/" target="_blank">全款购车</a></div>
			<!-- <div class="tabrightlink"><a href="http://car.bitauto.com/xuanchegongju/" target="_blank">选车工具</a></div>-->
			<ul class="tab">
				<li><a href="/chexingduibi/<%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">车型对比</a></li>
				<li><a href="/tupianduibi/<%= csIDs==""?"":"?csids="+csIDs %><%= carIDs==""?"":(csIDs==""?"?carIDs="+carIDs:"&carIDs="+carIDs) %>">图片对比</a></li>
				<li><a href="http://koubei.bitauto.com/duibi/<%= csIDs==""?"":csIDs+".html" %>">口碑对比</a></li>
				<li class="on"><a>评测对比</a></li>
			</ul>
		</div>
		<!--添加车型 start-->
		<div class="add_compare">
			<div class="line_box">
				<ul class="tab">
					<li class="t" onclick="showOrHideElementById();"><b>+</b>快速添加车型</li>
					<li id="top_1" class="current"><a href="javascript:ajaxForTabListSelectSerial(1);">5万以内</a></li>
					<li id="top_2"><a href="javascript:ajaxForTabListSelectSerial(2);">5-8万</a></li>
					<li id="top_3"><a href="javascript:ajaxForTabListSelectSerial(3);">8-12万</a></li>
					<li id="top_4"><a href="javascript:ajaxForTabListSelectSerial(4);">12-18万</a></li>
					<li id="top_5"><a href="javascript:ajaxForTabListSelectSerial(5);">18-25万</a></li>
					<li id="top_6"><a href="javascript:ajaxForTabListSelectSerial(6);">25-40万</a></li>
					<li id="top_7"><a href="javascript:ajaxForTabListSelectSerial(7);">40-80万</a></li>
					<li id="top_8"><a href="javascript:ajaxForTabListSelectSerial(8);">80万以上</a></li>
					<li id="top_9"><a href="javascript:ajaxForTabListSelectSerial(9);">SUV</a></li>
					<li id="top_10"><a href="javascript:ajaxForTabListSelectSerial(10);">MPV</a></li>
					<li id="top_11"><a href="javascript:ajaxForTabListSelectSerial(11);">跑车</a></li>
					<li id="top_12"><a href="javascript:ajaxForTabListSelectSerial(12);">其他</a></li>
					<li class="last" id="last"><a id="TabListHideLink" href="javascript:showOrHideElementById();" class="show">收起</a></li>
				</ul>
				<div style="" id="ListForCompare_divContent" class="l"></div>
			</div>
			<div class="clear"></div>
		</div>
		<!--添加车型 end-->
		<!-- new start-->
		<!-- 标签 -->
		<ul class="com_pingce_nav com_pingce_nav_top">
			<li <%= GetTagClassByID(12) %>><a <%= HasThisTag(11) %>>总结</a></li>
			<li <%= GetTagClassByID(11) %>><a <%= HasThisTag(10) %>>配置</a></li>
			<li <%= GetTagClassByID(10) %>><a <%= HasThisTag(9) %>>油耗</a></li>
			<li <%= GetTagClassByID(9) %>><a <%= HasThisTag(8) %>>舒适性</a></li>
			<li <%= GetTagClassByID(8) %>><a <%= HasThisTag(7) %>>操控</a></li>
			<li <%= GetTagClassByID(7) %>><a <%= HasThisTag(6) %>>动力</a></li>
			<li <%= GetTagClassByID(6) %>><a <%= HasThisTag(5) %>>灯光</a></li>
			<li <%= GetTagClassByID(5) %>><a <%= HasThisTag(4) %>>视野</a></li>
			<li <%= GetTagClassByID(4) %>><a <%= HasThisTag(3) %>>空间</a></li>
			<li <%= GetTagClassByID(3) %>><a <%= HasThisTag(2) %>>内饰</a></li>
			<li <%= GetTagClassByID(2) %>><a <%= HasThisTag(1) %>>外观</a></li>
			<li <%= GetTagClassByID(1) %> id="Li2"><a <%= HasThisTag(0) %>>导语</a></li>
		</ul>
		<!-- 标签结束 -->
		<div class="line_box bdc_blue mgb0">
			<div class="com_select_car" id="com_select_car">
				<div id="divSelect_0" class="com_select_car_item com_select_car_item_first w477">
					<%= htmlLeftTitle%>
				</div>
				<div id="divSelect_1" class="com_select_car_item bdr_none w477">
					<%= htmlRightTitle%>
				</div>
			</div>
			<div class="com_pingce_con">
				<%= htmlLeft %>
				<%= htmlRight %>
			</div>
			<div class="com_pingce_page">
				<div class="the_pages">
					<div>
						<%= pagePagination %></div>
				</div>
			</div>
		</div>
		<!-- 标签 -->
		<ul class="com_pingce_nav com_pingce_nav_bottom">
			<li <%= GetTagClassByID(12) %>><a <%= HasThisTag(11) %>>总结</a></li>
			<li <%= GetTagClassByID(11) %>><a <%= HasThisTag(10) %>>配置</a></li>
			<li <%= GetTagClassByID(10) %>><a <%= HasThisTag(9) %>>油耗</a></li>
			<li <%= GetTagClassByID(9) %>><a <%= HasThisTag(8) %>>舒适性</a></li>
			<li <%= GetTagClassByID(8) %>><a <%= HasThisTag(7) %>>操控</a></li>
			<li <%= GetTagClassByID(7) %>><a <%= HasThisTag(6) %>>动力</a></li>
			<li <%= GetTagClassByID(6) %>><a <%= HasThisTag(5) %>>灯光</a></li>
			<li <%= GetTagClassByID(5) %>><a <%= HasThisTag(4) %>>视野</a></li>
			<li <%= GetTagClassByID(4) %>><a <%= HasThisTag(3) %>>空间</a></li>
			<li <%= GetTagClassByID(3) %>><a <%= HasThisTag(2) %>>内饰</a></li>
			<li <%= GetTagClassByID(2) %>><a <%= HasThisTag(1) %>>外观</a></li>
			<li <%= GetTagClassByID(1) %> id="Li3"><a <%= HasThisTag(0) %>>导语</a></li>
		</ul>
		<!-- 标签结束 -->
		<!-- new end-->
		<div class="clear"></div>
	</div>
	<!--#include file="~/html/footer2012.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
<script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
<script language="javascript" type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/JsForPingCeComparev1.js?Aug152012"></script>
<script language="javascript" type="text/javascript">
	PingCeComparePageObject.TagID = "<%= tagID %>";
	PingCeComparePageObject.CurrentCsIDs = "<%= csIDsHtml %>";
	PingCeComparePageObject.ValidCount = "<%= validCount.ToString() %>";
	intiPageSelectList();
</script>
