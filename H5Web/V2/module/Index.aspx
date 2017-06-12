<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="H5Web.V2.module.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>首页</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/style.css" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/cheguwen.css" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/jingxiaoshang-style.css" />
</head>
<body>
	<!--#include file="~/inc/footer_script_common.shtml"-->
	<div class="context_scroll" data-name="index">
		<div class="context_scroll_box">
			<!--固定层开始-->

			<% if (BrokerId > 0)
	  { %>
			<div class="dianping_editor">
				<script type="text/javascript">
					//经纪人接口：获取经纪人信息
					document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=agent&method=brokerinfo&csid=<%=SerialBrandId %>&type=1&brokerid=<%=BrokerId%>' type='text/javascript'%3E%3C/script%3E"));
				</script>

			</div>

			<!--换色车型图-->
			<div class="standard_car_pic" id="standard_car_pic_1">
				<% for (var i = 0; i < SerialColorList.Count; i++)
	   {
		   var item = SerialColorList[i];
				%>
				<img src="http://image.bitautoimg.com/newsimg-600-w0-1-q80/<%=item.ImageUrl.Substring(27) %>" style="display: <%=i==0?"display":"none"%>;" />
				<% } %>
			</div>
			<!--固定层开始-->
			<div class="fixed_box" id="logo">
				<h1><%=BaseSerialEntity.ShowName %></h1>
				<p class="del">厂商指导价：<em><%= BaseSerialEntity.SaleState == "停销"?"暂无":BaseSerialEntity.ReferPrice%></em></p>
			</div>
			<!--固定层结束-->


			<!--颜色切换-->
			<ul class="changecolor" id="changecolor">
				<% for (int i = 0; i < SerialColorList.Count; i++)
	   {
		   var color = SerialColorList[i];
				%>
				<li <%=i==0?"class='current'":string.Empty %>><span style="background: <%=color.ColorRGB%>;" data-value="<%=color.ColorName %>"></span></li>
				<% } %>
			</ul>

			<script type="text/javascript" src="/Scripts/V2/colorslide.js"></script>
			<% }
	  else if (DealerId > 0)
	  {%>
			<script type="text/javascript">
				//经销商接口：经销商首页（图片页面）
				document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=dealer&method=dealerpic&csid=<%=SerialBrandId %>&dealerid=<%=DealerId%>&122' type='text/javascript'%3E%3C/script%3E"));
			</script>
			<%} %>
			<!--占位元素-->
			<div class="h60"></div>
			<!--固定层结束-->
		</div>
	</div>
	<!--内容 end-->
</body>
</html>
