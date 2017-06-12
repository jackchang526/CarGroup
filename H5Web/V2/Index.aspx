<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="H5Web.V2.Page1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>【<%=BaseSerialEntity.SeoName %>】报价及图片_评测_口碑_配置-易车网</title>
	<meta charset="utf-8" />
	<meta name="Keywords" content="<%=BaseSerialEntity.SeoName %>,<%=BaseSerialEntity.SeoName %>报价,<%=BaseSerialEntity.SeoName %>图片,<%=BaseSerialEntity.SeoName %>口碑" />
	<meta name="Description" content="易车网提供<%=BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%=BaseSerialEntity.SeoName %>优惠行情、<%=BaseSerialEntity.SeoName %>导购信息，最新<%=BaseSerialEntity.SeoName %>降价促销活动尽在易车网。" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/style.css">
</head>
<body>
	<div class="context_scroll" data-name="index">
		<div class="context_scroll_box">
			<!--固定层开始-->
			<div class="fixed_box" id="logo">
				<div class="img">
					<img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%=BaseSerialEntity.Brand.MasterBrandId %>_100.png" />
				</div>
				<h1><%=BaseSerialEntity.ShowName %></h1>
				<p>厂商指导价：<%= BaseSerialEntity.SaleState == "停销"?"暂无":BaseSerialEntity.ReferPrice%></p>
			</div>
			<!--固定层结束-->

			<!--换色车型图-->
			<div class="standard_car_pic" id="standard_car_pic_1">
				<% for (var i = 0; i < SerialColorList.Count; i++)
	   {
		   var item = SerialColorList[i];
				%>
				<img src="http://image.bitautoimg.com/newsimg-600-w0-1-q80/<%=item.ImageUrl.Substring(27) %>" style="display: <%=i==0?"display":"none"%>;" />
				<% } %>
			</div>

			<!--颜色名称-->
			<div class="car_color_text" id="car_color_text">
				<%  if (SerialColorList.Count > 0)
		{
			var itemName = SerialColorList[0];
				%>
				<span><%=itemName.ColorName %></span>
				<%} %>
			</div>
			<!--颜色切换-->
			<ul class="changecolor" id="changecolor">
				<% for (int i = 0; i < SerialColorList.Count; i++)
	   {
		   var color = SerialColorList[i];
				%>
				<li <%=i==0?"class='current'":string.Empty %>><span style="background: <%=color.ColorRGB%>;" data-value="<%=color.ColorName %>"></span></li>
				<% } %>
			</ul>
		</div>
	</div>
	<!--#include file="~/inc/footer_script_common.shtml"-->
	<script type="text/javascript" src="/Scripts/V2/colorslide.js"></script>
</body>
</html>
