<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2012guangzhou-chezhan_Serial.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.Exhibition._2012guangzhou_chezhan_Serial" %>

<%@ OutputCache Duration="300" VaryByParam="*" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
	<title>【图】广州车展<%=_SerialSeoName %>新车_2012广州<%=_SerialSeoName %>新车图片_报价_论坛-易车网</title>
	<!--#include file="~/ushtml/0000/2012gz_mxl2-407.shtml"-->
	<meta name="Keywords" content="广州车展<%=_SerialSeoName %>，2012广州车展<%=_SerialSeoName %>新车，<%=_SerialSeoName %>汽车报价，易车网" />
	<meta name="Description" content="广州车展<%=_SerialSeoName %>汽车：易车网(BitAuto.com)提供广州车展<%=_SerialSeoName %>新车报价、2012广州车展<%=_SerialSeoName %>图片、广州国际车展<%=_SerialSeoName %>油耗等新车信息。" />
</head>
<body class="body_sub">
	<!--头 start-->
	<!--#include file="~/include/special/yc/00001/2010TopicCommonHead_Manual.shtml"-->
	<!--头 end-->
	<!--头图 start-->
	<div class="bt_sub_head">
		<div class="sub_header_swd">
			<h1>
				2012广州车展</h1>
			<h2>
				网<span>上</span><br />
				<b>展</b><strong>厅</strong></h2>
		</div>
	</div>
	<!--头图 end-->
	<!--导航 start-->
	<!--#include file="~/include/z/gzcz/2012/shouye/00001/201211_gzcz_sy_dh_gb2312_Manual.shtml"-->
	<!--导航 end-->
	<div class="bt_con_swd sub_bt_con_swd">
		<!--面包屑 start-->
		<div class="breadcrumbs">
			<strong>当前位置：</strong><a href="http://chezhan.bitauto.com/guangzhou-chezhan/" target="_blank">2012广州车展</a>
			<b>&gt;</b>
			<%= _GuilderString %>
		</div>
		<!--面包屑 end-->
		<!--搜索 start-->
		<!--#include file="~/include/z/gzcz/2012/shouye/00001/201211_gzcz_ssk_gb2312_Manual.shtml"-->
		<!--搜索 end-->
		<script type="text/javascript">
			setMenuItemSelect("m3");
		</script>
		<div class="clear">
		</div>
	</div>
	<div class="bt_con_swd">
		<div class="swd_sub_top2">
			<div class="mxl_page">
				<%=_OuterOtherGuilderString%>
				<div class="mxl_col_con">
					<%=_FocusContent %>
					<div class="col-con">
						<%=_CarImageList%>
						<%=_TuJieList %>
						<%=_CarModelList%>
						<%=_VideoList%>
					</div>
				</div>
				<div class="col-side">
					<div class="line_box mgb0">
						<h3>
							<span>热门车模</span></h3>
						<div class="more">
							<a target="_blank" href="http://photo.bitauto.com/exhibit/class/60158/1.html">更多&gt;&gt;</a></div>
						<!--#include file="~/include/z/gzcz/2012/wszt/00001/gz_2012_ejy_cmph_Manual.shtml"-->
					</div>
					<!--#include file="~/include/z/gzcz/2012/wszt/00001/gz2012_ejy_cxph_Manual.shtml"-->
					<%=_BrandNewsList%>
				</div>
			</div>
			<!-- 网上展厅 -->
			<div class="clear">
			</div>
			<div class="line_box subtt">
				<h3>
					<span><a href="http://chezhan.bitauto.com/guangzhou-chezhan/n2012/zhanguan/1/" target="_blank">
						网上展厅</a></span><b></b></h3>
				<div class="more">
					<a href="http://chezhan.bitauto.com/guangzhou-chezhan/n2012/zhanguan/1/" target="_blank">
						更多&gt;&gt;</a></div>
			</div>
			<div class="line_box mgb20">
				<!--#include file="~/include/z/gzcz/2012/wszt/00001/gzcz2012_ejy_wszt_ggnr_gb2312_Manual.shtml"-->
			</div>
		</div>
	</div>
	<div class="h10">
	</div>
	<!--js start-->
	<!--#include file="~/include/z/gzcz/2012/shouye/00001/201211_gzcz_sy_Js_Manual.shtml"-->
	<!--百度分享 start-->
	<!--#include file="~/include/z/gzcz/2012/shouye/00001/201211_gzcz_bdfx_gb2312_Manual.shtml"-->
	<!--百度分享 end-->
	<!--footer start-->
	<!--#include file="~/include/special/yc/00001/2010TopicCommonFoot_Manual.shtml"-->
	<!--footer end-->
	<!--监测代码-->
	<!--#include file="~/include/special/00001/bitauto_analytics_chezhan_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>

