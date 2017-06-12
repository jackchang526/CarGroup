<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photo.aspx.cs" Inherits="H5Web.V2.Page2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>图片</title>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/style.css">
</head>
<body>
	<div class="context_scroll" data-name="photo">
		<div class="context_scroll_box">
			<!--内容开始-->
			<div class="box box-top">
				<div class="picbox">
					<% var firstImage = SerialImageList.Count > 0 ? SerialImageList[0] : null;
		var secondImage = SerialImageList.Count > 1 ? SerialImageList[1] : null;
		var thirdImage = SerialImageList.Count > 2 ? SerialImageList[2] : null;
					%>
					<% if (firstImage != null)
		{%>
					<div>
						<em><%=getTag(firstImage.PositionId) %></em><a href="<%=string.Format("http://photo.m.yiche.com/picture/{0}/{1}/",SerialBrandId,firstImage.ImageId) %>" target="_parent">
							<img src="<%=string.Format(firstImage.ImageUrl,3) %>" /></a>
					</div>
					<%} %>
					<% if (secondImage != null)
		{%>
					<div class="picleft">
						<em><%=getTag(secondImage.PositionId) %></em>
						<a href="<%=string.Format("http://photo.m.yiche.com/picture/{0}/{1}/",SerialBrandId,secondImage.ImageId) %>" target="_parent">
							<img src="<%=string.Format(secondImage.ImageUrl,4) %>" /></a>
					</div>
					<%} %>
					<% if (thirdImage != null)
		{%>
					<div class="picright">
						<em><%=getTag(thirdImage.PositionId) %></em>
						<a href="<%=string.Format("http://photo.m.yiche.com/picture/{0}/{1}/",SerialBrandId,thirdImage.ImageId) %>" target="_parent">
							<img src="<%=string.Format(thirdImage.ImageUrl,4) %>" /></a>
					</div>
					<%} %>

					<!--图解图片-->
					<%
						if (TujieImageEntity != null)
						{
					%>
					<div>
						<em>图解</em>
						<a href="<%=string.Format("http://photo.m.yiche.com/picture/{0}/{1}/",SerialBrandId,TujieImageEntity.ImageId) %>" target="_parent">
							<img src="<%=string.Format(TujieImageEntity.ImageUrl,3) %>" /></a>
					</div>
					<%} %>
				</div>
			</div>


			<!--内容结束-->
			<div class="h60"></div>
		</div>
	</div>
	<!--#include file="~/inc/footer_script_common.shtml"-->
</body>
</html>
