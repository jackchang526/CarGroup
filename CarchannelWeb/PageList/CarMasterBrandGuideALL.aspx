<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterBrandGuideALL.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageList.CarMasterBrandGuideALL" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <!--Author: liuhuan-->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>品牌大全</title>
    <meta name="Keywords" content="品牌大全,车型数据库, 汽车最新报价,车型导购,汽车评测,汽车新闻,汽车图片,汽车问答,汽车点评，汽车经销商，汽车论坛" /> 
    <meta name="Description" content="易车网(BitAuto.com) 汽车车型为您提供各种汽车车型所有信息。包括汽车报价、最新报价、汽车图片、汽车参数、汽车配置、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />     
    <!--#include file="~/ushtml/0000/car_common_v2_B-306.shtml"-->
</head>
<body>
   <!--#include file="~/html/header2012.shtml"-->
<!--smenu end-->
<div class="bt_page">
	<!--nav start-->
	<div class="publicTabNew">
		<ul class="tab">
			<li><a target="_blank" href="http://car.bitauto.com/">汽车大全</a></li>
			<li class="on"><a>品牌大全</a></li>
		</ul>
	</div>
	<!--nav end-->
	<div class="col-all spic">
       <div class="line_box pic_album bybrand_list">
			<h3><span>热门品牌</span></h3>
			<ul class="zimu">
			<%=MaoDianList %>
			</ul>
			<!--#include file="~/html/HotMasterBrandList.shtml"-->
		</div>
	</div>
	<%=MasterBrandLogoList%>
	

    <!--#include file="~/html/footer2012.shtml"-->

</div>
<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>