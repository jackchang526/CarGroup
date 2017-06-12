<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsChannel.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.NewsChannel" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>
		<%=pageTitle%></title>
	<meta name="Keywords" content="<%=metaKeywords%>" />
	<meta name="Description" content="<%=metaDescription%>" />
	<!--#include file="~/ushtml/0000/car_common_v2_B-306.shtml"-->
</head>
<body>
	<!--#include file="~/html/header2012.shtml"-->
	<div class="bt_page">
		<div class="bt_smenuNew">
			<div class="bt_navigatev1New">
				<div><span>您当前的位置：</span><a href="http://www.bitauto.com" target="_blank">易车网</a> &gt;
					<%=navString %></div>
			</div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchNew.shtml"-->
			</div>
		</div>
	</div>
	<div class="bt_page">
		<div class="col-all">
			<div class="line_box mainlist_box all_newslist"><h3><span>
				<%=bodyTitle %></span></h3>
				<%=newsContent%>
				<BitAutoControl:Pager ID="AspNetPager1" runat="server" HrefPattern="String" Visible="false"/>
			</div>
		</div>
	</div>
	<!--page end-->
	<!-- 调用尾 -->
	<!--#include file="~/html/footer2012.shtml"-->
	<!--新闻监测代码开始-->
	<!--#include  file="~/html/bitauto_analytics_News_Manual.shtml"-->
	<!--新闻监测代码结束-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
