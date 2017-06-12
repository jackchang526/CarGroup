<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompareTool.aspx.cs"
	Inherits="WirelessWeb.CarCompareTool" %>

<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>车型对比工具</title>
	<!--#include file="~/ushtml/0000/myiche2015_cube_duibi_style-news-1003.shtml"-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js?v=20130606"></script>
</head>
<body>
 	<div id="container">
		<!--新加弹出层 start-->
		<div id="master_container" style="z-index: 888888; display: none" class="brandlayer mthead">
			<!--#include file="~/html/compareCarTemplate.html"-->
		</div>

		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
 			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>对比工具</h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<div class="op-nav-mark" style="display: none;"></div>
		<!-- header end -->
		<div class="m-tool-compare y2015">
			<div id="CarCompareContent">
			</div>
			<div class="m-statement">以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a href="http://m.yiche.com/wap2.0/feedback/">“欢迎您及时指正！”</a></div>
		</div>
		<!--分类 start-->
		<%--<div id="popup-menulist" class="more-peizhi-list" style="display: none;">
			<div class="more-peizhi-list-box">
				<ul>
				</ul>
				<div class="clear"></div>
				<i class="ico-peizhi"></i>
			</div>
		</div>
		<a id="popup-menu" href="javascript:;" class="m-top category" data-action="cact">
			<div class="ico-top-arrow"></div>
			<p>分类</p>
		</a>--%>
		<div class="float-catalog-mask" id="popup-menumask" style="display: none"></div>
		<div id="popup-menu" class="float-catalog">
			目录
			<div id="popup-menulist" class="catalog-list" style="display: none;">
				<ul>
				</ul>
				<div class="ico-catalog-arrow"></div>
			</div>
		</div>
		<!--分类 end-->

		<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchorv2.js?v=201209"></script>--%>
		<script type="text/javascript">
			var url = "http://car.m.yiche.com/";
		</script>
		<div class="footer15">
         <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
        </div>
        <div class="float-r-box">
	        <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
        </div>
	</div>
	<%--	<div id="sel-container" style="display: none;">
		<div id="sel-cur-serial"></div>
		<div id="sel-master"></div>
		<div id="sel-serial"></div>
		<div id="sel-car"></div>
		<div id="sel-history"></div>
	</div>--%>
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>

	<%--	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll.js?v=20150828"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/model.js?v=2015090915"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/addcompare.js?v=201510271146"></script>--%>
	<script type="text/javascript">
		var apiBrandId = 0;
		var apiSerialId = 0;
		var apiCarId = 0;
	</script>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/iscroll.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/underscore.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/rightswipe.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/note.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/brand.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/addcompareV2.js?v=20160118"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js,carchannel/wirelessjs/v2/note.js,carchannel/wirelessjs/v2/brand.js,carchannel/wirelessjs/addcompareV2.js?v=20170105"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/carcomparetoolV2.min.js?v=20160713"></script>
    <%--<script type="text/javascript" src="/Js/carcomparetoolV2.js?v=20160225"></script>--%>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/carselect.js?v=20141229"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/CarCompareStatForWireless.js?201412"></script>
	<script type="text/javascript">
		function initCarInfo(carId) {
			loadJS.push("http://api.car.bitauto.com/CarInfo/GetCarParameter.ashx?isParamPage=1&carids=" + carId, "utf-8", function () {
				initPageForCompare();
			});
		}
		initCarInfo('<%=Request.QueryString["carIDs"] %>');
	</script>
</body>
</html>
