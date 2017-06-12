<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsRegionNews.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsRegionNews" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title><%=pageTitle%></title>
	<meta name="Keywords" content="<%=pageKeywords%>" /> 
	<meta name="Description" content="<%=pageDesc%>" /> 
	<!--#include file="~/ushtml/0000/car_common_v2-272.shtml"-->
	<link href="http://beijing.bitauto.com/car/citylogo.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <!--#include file="~/html/header2012.shtml"-->
        <!--ad start-->
<div class="bt_ad">
    <%=serialTopAdCode%>
</div>
<!--ad end-->
    <!--smenu start-->
	<div class="bt_page">
	<div class="bt_smenuNew">
		<div class="bt_navigatev1New">
		<div>
        <span>您当前的位置：</span>
        <a href="http://<%=citySpell %>.bitauto.com/" target="_blank"><%=cityName %>易车网</a>
         &gt; <a href="http://<%=citySpell %>.bitauto.com/car/" target="_blank"><%=cityName %>购车</a>
         &gt; <strong><%=serialShowName%></strong></div>
		</div>
		<div class="bt_searchNew">
		<!--#include file="~/html/bt_searchNew.shtml"-->
		</div>
	</div>
	</div>
	<!--smenu end-->
	<%= CsHead %>
    <div class="bt_page">
    <div class="col-con">
	<asp:Literal ID="lrContent" EnableViewState="false" runat="Server" />
	</div>
	<div class="col-side">
	<!--<script charset="gb2312" type="text/javascript" src="http://go.bitauto.com/handlers/ModelsBrowsedRecently.ashx"></script>-->
	<!--百度推广-->
    <div class="line_box baidupush">
   <script type="text/javascript">
   	/*bitauto 200*200，导入创建于2011-10-17*/
   	var cpro_id = 'u646188';
				</script>
				<script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
	</div>
	<!--百度推广-End -->
	</div>
	<!--footer start-->
<!--#include file="~/html/footer2012.shtml"-->
<!--footer end-->
 <script src="http://www.bitauto.com/themes/2009/js/headcommon.js" type="text/javascript"></script>
 <script type="text/javascript" src="http://www.bitauto.com/themes/2009/js/search.js"></script>
 
	</div>
    <script language="javascript" type="text/javascript">
    	// 浏览过的子品牌
    	// initPageForVisitSerial("<%= serialId %>");
</script>
	<!--WebTrends Analytics-->
	<!-- START OF SmartSource Data Collector TAG -->
	<SCRIPT SRC="http://css.bitauto.com/bt/webtrends/dcs_tag_city13.js" TYPE="text/javascript"></SCRIPT>
	<!-- END OF SmartSource Data Collector TAG -->
	<!--news stat-->
	<!-- bitauto stat begin -->
	<script type="text/javascript" src="http://log.bitauto.com/newsstat/stat.js"></script>
	<!-- bitauto stat end -->
	<!--AD stat-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
