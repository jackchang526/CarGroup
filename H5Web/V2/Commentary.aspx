﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Commentary.aspx.cs" Inherits="H5Web.V2.Commentary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>【<%= BaseSerialEntity.SeoName %>】评论-易车</title>
	<meta charset="utf-8" />
	<meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑" />
	<meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/style.css" />
</head>
<body>

	<div class="context_scroll" data-name="commentary">
		<div class="context_scroll_box">
			<!--<div id="sharefloat" class="sharefloat"></div>-->
			<!--内容开始-->
			<div class="box">

				<!--编辑点评开始-->
				<%= EditorComment %>
				<!--编辑点评结束-->

				<div class="dianping_t">
					<h2>网友点评</h2>
				</div>

				<%= Koubei %>
			</div>
			<!--内容结束-->
			<div class="h60"></div>
			<!--#include file="~/inc/footer_script_common.shtml"-->
		</div>
	</div>
</body>
</html>
<script type="text/javascript">
	var guestFuelCost = '<%= GuestFuelCost %>';
	$(function () {
		$("#guestFuelCost").html(guestFuelCost);
	});
</script>
