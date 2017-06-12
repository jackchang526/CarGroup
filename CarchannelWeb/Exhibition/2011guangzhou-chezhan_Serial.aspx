<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2011guangzhou-chezhan_Serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2011guangzhou_chezhan_Serial" %>

<%@ OutputCache Duration="300" VaryByParam="*" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
	<title>【图】广州车展<%=_SerialSeoName %>新车_2011广州<%=_SerialSeoName %>新车图片_报价_论坛-易车网</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="Keywords" content="广州车展<%=_SerialSeoName %>，2011广州车展<%=_SerialSeoName %>新车，<%=_SerialSeoName %>汽车报价，易车网" />
	<meta name="Description" content="广州车展<%=_SerialSeoName %>汽车：易车网(BitAuto.com)提供广州车展<%=_SerialSeoName %>新车报价、2011广州车展<%=_SerialSeoName %>图片、广州国际车展<%=_SerialSeoName %>油耗等新车信息。" />
	<!--CSS-->
	<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_css_Manual.shtml"-->
	<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_css_ssxc_Manual.shtml"-->
	<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_css_wszt_Manual.shtml"-->
	<!--Js-->
	<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_js_Manual.shtml"-->
</head>
<body class="subpage">
	<!--专题公共头-->
	<!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
	<!-- 首页头部开始-->
	<div class="zh_head_page">
		<div class="zh_zh_header"><h1>第9届中国(广州)国际汽车展览会</h1>
			<!--     <h2 class="mvcm">美女车模</h2>-->
			<h2 class="wszt">
				网上展厅</h2>
			<!--   <h2 class="mvsp">美女视频</h2>
        <h2 class="ssxc">上市新车</h2>
        <h2 class="djjx">独家解析</h2>
        <h2 class="gdft">高端访谈</h2>
        <h2 class="gdxw">滚动新闻</h2>
        <h2 class="xctp">新车图片</h2>
        <h2 class="czsp">车展视频</h2>
        <h2 class="xcxw">车展新闻</h2>-->
			<!--<h2 class="qxtg">抢先探馆</h2>-->
		</div>
		<!--公用导航_主导航-->
		<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_gydh_zdh_Manual.shtml"-->
		<!--公用导航_新车展厅导航 -->
		<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_gydh_xcztdh_Manual.shtml"-->
	</div>
	<!-- 首页头部结束-->
	<!--面包屑开始-->
	<div class="swd_breadcrumbs">
		<div class="swd_breadcrumbs_left">您现在是在：<a href="http://chezhan.bitauto.com/guangzhou-chezhan/">2011广州车展</a> &gt;
			<%= _GuilderString %></div>
		<div class="swd_breadcrumbs_right">
			<!--Search cys-->
			<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_gydh_rsss_Manual.shtml"-->
			<!--图片数量 count-->
			<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/2011gzcz_gydh_tkmns_Manual.shtml"-->
		</div>
	</div>
	<!--面包屑结束-->
	<!--二级页面 start-->
	<div class="bt_page bd_top_none">
		<div class="swd_sub_top2">
			<div class="mxl_page">
				<%= _OuterOtherGuilderString %>
				<div class="mxl_col_con">
					<%= _FocusContent %>
					<div class="col-con">
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<%= _CarImageList %>
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<%= _TuJieList %>
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<%= _CarModelList %>
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<!--2011上海车展_车型页_车型图片区_车型图片-->
						<%= _VideoList %>
						<!--2011上海车展_车型页_车型图片区_车型图片-->
					</div>
				</div>
				<div class="col-side">
					<!-- 车模 -->
					<%= _HotModelList %>
					<!-- 车模 -->
					<!-- 热门车展 -->
					<%= _HotCarTypeList %>
					<!-- 热门车展 -->
					<!-- 品牌新闻 -->
					<%= _BrandNewsList %>
					<!-- 品牌新闻 -->
				</div>
			</div>
			<!-- 网上展厅 -->
			<div class="clear"></div>
			<div class="line_box wd_linebox_4 wzh_linbox"><h3><span>网上展厅</span></h3>
				<!--#include file="~/include/z/gzcz2011/gzcz2011ztggk/00001/gzcz2011_ejlm_wszt_Manual.shtml"-->
			</div>
		</div>
	</div>
	<!--二级页面 end-->
	<!--专题公共底-->
	<!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_utf8_Manual.shtml"-->
	<!--监测代码-->
	<!--WebTrends Analytics-->
	<!-- START OF SmartSource Data Collector TAG -->
	<script src="http://css.bitauto.com/bt/webtrends/dcs_tag_chezhan16.js" type="text/javascript"></script>
	<!-- END OF SmartSource Data Collector TAG -->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
