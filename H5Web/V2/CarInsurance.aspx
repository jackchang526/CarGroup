<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarInsurance.aspx.cs" Inherits="H5Web.V2.CarInsurance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>【<%= BaseSerialEntity.SeoName %>】保险-易车</title>
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
	<div class="context_scroll" data-name="carinsurance">
		<div class="context_scroll_box">
			<!--固定层开始-->
			<div class="fixed_box" id="logo">
				<div class="img">
					<img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%= BaseSerialEntity.Brand.MasterBrandId %>_100.png" />
				</div>
				<h1><%= BaseSerialEntity.ShowName %></h1>
				<p>厂商指导价：<%= BaseSerialEntity.ReferPrice %></p>
			</div>
			<!--固定层结束-->
			<!--保险开始-->
			<div class="youhui_box baoxian_price">
				<ul class="two">
					<li>
						<span>交强险指导价</span>
						<em id="lblCompulsory">0</em>元起
					</li>
					<li>
						<span>商业险指导价</span>
						<em id="lblCommonTotal">0</em>元起
					</li>
				</ul>
			</div>
			<div class="box baoxian_logo">
				<h2>商业合作</h2>
				<ul>
					<li>
						<a href="http://mchanxian.sinosig.com/activity/yiche/yiche.html" target="_parent">
							<img src="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/img/baoxian_yg.png" />阳光车险</a>
					</li>
					<li>
						<a href="http://www.epicc.com.cn/wap/cooperE/?cityCode=110100&comName=BDyichewang0100&carrier=002&channel=WAP01" target="_parent">
							<img src="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/img/baoxian_rb.png" />人保车险</a>
					</li>
					<li>
						<a href="http://chexian.axatp.com/m/m-auto?isAgent=68&ms=yiche?carid=0&tagname=#acWord" target="_parent">
							<img src="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/img/baoxian_as.png" />安盛天平</a>
					</li>
				</ul>
			</div>
			<!--保险结束-->
			<div class="h60"></div>
		</div>
	</div>
	<script src="/Scripts/V2/CarCalculator.js"></script>
	<script type="text/javascript">
        var carMinReferPrice = "<%= carMinReferPrice %>";
        document.body.onload = CarCalculator.Init();
	</script>
	<!--#include file="~/inc/footer_script_common.shtml"-->
</body>
</html>
