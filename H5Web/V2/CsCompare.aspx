<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompare.aspx.cs" Inherits="H5Web.V2.CsCompare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>【<%= BaseSerialEntity.SeoName %>】参数配置-易车</title>
	<meta charset="utf-8" />
	<meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑" />
	<meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/选车工具.css" />
	<link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/完善/css/style.css" />
</head>
<body>
	<div class="context_scroll" data-name="cscompare">
		<div class="context_scroll_box">
			<!--内容开始-->
			<table class="datatable" id="datatable">
			</table>

			<!-- black popup start -->
			<div class="leftmask leftmask1" style="display: none;"></div>
			<div class="leftPopup popup-car" data-back="leftmask1" style="display: none; z-index: 99999;">

				<div class="swipeLeft swipeLeft-sub">
					<div class="swipeLeft-loading" style="display: none;">
						<img src="http://image.bitautoimg.com/uimg/mbt2014/images/loading.gif" />
						<p>正在加载...</p>
					</div>
					<div class="y2015-car-02">
						<div class="slider-box">
							<dl class="tt-list">
							</dl>
						</div>
					</div>
				</div>
			</div>
			<%--</div>--%>
			<div class="h60"></div>
		</div>
	</div>
	<!-- black popup end -->
	<!--内容结束-->
	<!--#include file="~/inc/footer_script_common.shtml"-->
	<script type="text/javascript" src="/Scripts/V2/carcompare.js?v=2015082715"></script>
	<script type="text/javascript" src="/Scripts/V2/iscroll.js"></script>
	<script type="text/javascript">
        <%= sbForApi.ToString() %>;
        <%= sbForHotCompare.ToString() %>;
        $(function () {
            CarCompareObj.Init();
        });
	</script>

</body>
</html>
