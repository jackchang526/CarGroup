<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialForSinglePriceList.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageList.SerialForSinglePriceList" %>
<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="300" VaryByParam="price" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=priceText %>车型_汽车车型大全】-易车网BitAuto.com</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta name="Keywords" content="<%=priceText %>汽车车型_按价格区间查找车型,车型大全, 车型数据库, 汽车品牌大全, 汽车最新报价" /> 
    <meta name="Description" content="易车网(BitAuto.com) 汽车车型为您提供各种汽车车型所有信息。包括汽车报价、最新报价、汽车图片、汽车参数、汽车配置、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />   
 	<!--#include file="~/ushtml/0000/car_common_v2-272.shtml"-->
 	<script type="text/javascript">
    <!--
    var tagIframe = null;
	var currentTagId = <%=priceTagId %>;		//当前页的标签ID
    -->
    </script>
</head>
<body>
<!--#include file="~/html/header2012.shtml"-->
	<div class="bt_page" id="pageTop">
    
    <!--#include file="~/html/searchBarAndGoCarType.shtml"-->
	<!--page start-->
		<div class="publicTab2 mb10">
		<%=navHtml %>
		</div>
	<!--热门车型-->
        <div class="col-con">
			<div class="line_box">
				<h3 class="car"><span><span class="caption">热门车型</span></span></h3>
				<div class="hotcx">
					<ul>
					<%=hotSerialHtml%>
					</ul>
				</div>
				<div class="clear"></div>
			</div>
		</div>
		
		<!--热门新车-->
				<div class="col-side">
			<div class="line_box">
				<h3><span>热门新车</span></h3>
				<div class="hotnewcar">
					<%=hotNewCarHtml %>					
				</div>
			</div>
		</div>
		 <!--  -->
		  <div class="clear"></div> 

	<asp:Literal ID="lrContent" EnableViewState="false" runat="Server" />
	<!-- 调用尾 -->
	<!--#include file="~/html/friendLink.shtml"-->
	<!--#include file="~/html/footer2012.shtml"-->
	</div>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>