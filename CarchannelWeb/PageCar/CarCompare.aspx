<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompare.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageCar.CarCompare" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
	<title>【<%= cbe.Serial.SeoName%>】<%=cbe.Serial.Brand.MasterBrand.Name+cbe.Serial.Name %>参数配置表_<%=cbe.Serial.SeoName %>发动机配置-易车网</title>
	<meta name="keywords" content="<%=cbe.Serial.SeoName%>参数,<%=cbe.Serial.SeoName%>配置,<%=cbe.Serial.Brand.MasterBrand.Name+cbe.Serial.Name%>,<%=cbe.Serial.SeoName %>参数配置表,<%=cbe.Serial.SeoName %>发动机配置,易车网,car.bitauto.com" />
	<meta name="description" content="<%=cbe.Serial.Brand.MasterBrand.Name+cbe.Serial.Name %>配置,易车网提供<%=cbe.Serial.Brand.MasterBrand.Name+cbe.Serial.Name %>配置参数表,包括,<%=cbe.Serial.SeoName %>发动机,<%=cbe.Serial.SeoName %>变速箱,<%=cbe.Serial.SeoName %>车轮,<%=cbe.Serial.SeoName %>灯光等配置等参数。" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_canshupeizhi-743.shtml"-->
	<script type="text/javascript">
		if (typeof (bitLoadScript) == "undefined")
			bitLoadScript = function (url, callback, charset) {
				var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
				if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
				else { s.onload = function () { if (callback) callback(); }; }
				s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
			};
	</script>
</head>
<body data-offset="-130" data-target=".left-nav" data-spy="scroll">
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<!--a_d start-->
	<div class="bt_ad">
		<!-- AD New Dec.31.2011 -->
		<ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%=cbe.Serial.Id %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
	</div>
	<!--a_d end-->
	<!--smenu start-->
	<%= CarHeadHTML %>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
	<!--page start-->
	<div class="bt_page" id="box">
		<style>
			.bt_page { overflow: visible; }
		</style>
		<!--[if IE 6]> <style>.bt_page{ overflow:hidden !important;}</style> <![endif]-->
		<div class="line-box mgt20" id="divFilter">
			<div class="title-box">
				<h3>参数配置</h3>
				<%--<span class="peizhi" id="spanFilterForYear"></span>
                <span class="peizhi" id="spanFilterForTT"></span>
				<span class="peizhi" id="spanFilterForEE"></span>--%>
			</div>
		</div>
		<!--左上小浮动层开始-->
		<div id="smallfixed" class="floatLayer floatLayer_peizhi w170" style="display: none;">
			<table cellspacing="0" cellpadding="0">
				<tbody>
					<tr>
						<th class="pd0">
							<div class="tableHead_left">
								<div class="check-box-item">
									<input type="checkbox" name="left_chkAdvantage" onclick="advantageForCompare();">
									<label for="left_chkAdvantage">
										标识优势项 <em></em>
									</label>
								</div>
								<div class="check-box-item">
									<input type="checkbox" name="checkboxForDiff" onclick="showDiffForCompare();" id="left_checkboxForDiff">
									<label for="left_checkboxForDiff">
										高亮不同项</label>
								</div>
								<div class="check-box-item">
									<input type="checkbox" name="checkboxForDelTheSame" onclick="delTheSameForCompare();"
										id="left_checkDiffer">
									<label for="left_checkDiffer">
										隐藏相同项</label>
								</div>
								<div class="dashline">
								</div>
								<p>
									●标配 ○选配&nbsp;&nbsp;- 无
								</p>
							</div>
						</th>
					</tr>
				</tbody>
			</table>
		</div>
		<!--左上小浮动层结束-->
		<!-- 浮动Top -->
		<div class="floatLayer floatLayer_peizhi" id="topfixed" style="display: none;">
		</div>
		<div class="tc-popup-box" id="popup-box" style="display: none;">
			<div class="tt">
				<h6>参数纠错</h6>
				<a href="javascript:;" class="btn-close">关闭</a>
			</div>
			<div class="tc-popup-con tc-popup-error-correction">
				<textarea id="correctError"></textarea>
				<div class="alert">
					<span></span>
				</div>
				<div class="button_orange button_99_35">
					<a href="javascript:;" name="btnCorrectError">提交</a>
				</div>
				<div class="button_gray button_99_35">
					<a href="javascript:;" id="btnErrorCancel">取消</a>
				</div>
			</div>
		</div>
		<div class="tc-popup-box" id="popup-box-success" style="display: none;">
			<div class="tt">
				<h6>参数纠错</h6>
				<a href="javascript:;" class="btn-close">关闭</a>
			</div>
			<div class="tc-popup-con">
				<div class="no-txt-box have-txt-box">
					<p class="tit">
						提交成功
					</p>
					<p>
						您提交的纠错信息我们已经收集到，感谢您的纠错。
					</p>
					<div class="button_gray button_94_35">
						<a href="javascript:;" id="btn-success-close">关闭</a>
					</div>
				</div>
				<div class="clear">
				</div>
			</div>
		</div>

		<div class="peizhi-filter">
			<span class="peizhi" id="spanFilterForYear"></span>
			<span class="peizhi" id="spanFilterForTT"></span>
			<span class="peizhi" id="spanFilterForEE"></span>
		</div>

		<!-- 车型对比 -->
		<div id="main_box" class="line_box_compare line_box_compare_peizhi y2015">
			<!-- 浮动Left -->
			<div id="leftfixed" style="display: none;">
			</div>
			<div id="CarCompareContent">
			</div>
			<%if (!string.IsNullOrEmpty(carIDAndName))
	 { %>
			<em class="btn-show-left-nav" style="display: none;" id="show-left-nav"></em>
			<!-- 左侧浮动层 start -->
			<div class="left-nav" id="left-nav" style="display: none;">
				<ul>
				</ul>
				<a href="javascript:;" class="close-left-nav" id="close-left-nav" style="display: none;">关闭浮层</a>
				<%--<em class="btn-hide-left-nav" style="display: none;"></em><a href="javascript:;"
					class="duibi-return-top" title="返回顶部">返回顶部</a>--%>
			</div>
			<!-- 左侧浮动层 end -->
			<%} %>
			<div class="td-tips">
				<div class="ts-box">
					以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a target="_blank" href="http://www.bitauto.com/feedback/">欢迎您及时指正！</a>
				</div>
			</div>
		</div>
		<% if (carIDAndName != "")
	 { %>
		<script type="text/javascript">
			<%= jsContent %>
		</script>
		<% } %>
		<script type="text/javascript" language="javascript">
			var CarCommonCSID = '<%=cbe.Serial.Id %>';
			var CarCommonCarID = '<%= carID.ToString() %>';
		</script>
		<!-- 对比浮动框 -->
		<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none;" data-page="compare" animateright="-533" animatebottom="130">
			<div class="tt" id="bar_minicompare" style="cursor: move;">
				<h6>车型对比</h6>
				<a class="b-close" id="b-close" href="javascript:void(0);">隐藏<i></i></a>
			</div>
			<div class="content">
				<ul id="idListULForWaitCompare" class="fixed-list"></ul>
				<div class="fixed-box">
					<div class="fixed-input" id="CarSelectSimpleContainerParent">
						<input type="text" userful="showcartypesim" value="请选择车款" readonly="readonly" />
						<%--<a class="right" href="javascript:void(0);" onclick="javascript:WaitCompareObj.GetYiCheSug();"><div class="star"><i></i></div></a>--%>
						<div class="right" userful="showcartypesim">
							<div class="star">
								<i class="star-i"></i>
							</div>
						</div>
						<div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
					</div>
					<div class="clear"></div>
					<div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
				</div>
				<div class="wamp">
					<em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
					<div class="clear"></div>
				</div>
				<div class="alert-center" id="AlertCenterDiv" style="display: none;">
					<p>最多对比10个车款</p>
				</div>
			</div>
		</div>
		<%--<div id="AlertCenterDiv" style="display: none;" class="alert-center"><p>最多对比10个车款</p></div>--%>
		<div class="loading" style="left: 50%; top: 400px; display: none; position: fixed" id="selectCarLoadingDiv">
			<i></i>
			<p>正在加载中...</p>
		</div>
		<!--漂浮层模板start-->
		<div class="effect" style="display: none;">
			<div class="car-summary-btn-duibi button_gray"><a href="javascript:;" target="_self"><span>对比</span></a></div>
		</div>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/draggable.js"></script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/CarChannelBaikeJson.js?v=20150831"></script>

		<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160120"></script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/JsForParameterv3.0.min.js?v=20160922"></script>
		<%--<script type="text/javascript" src="/jsnew/JsForParameterv3.0.js"></script>--%>
		<%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
        <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150733"></script>
        <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>--%>

		<script type="text/javascript" language="javascript">
			ComparePageObject.CarIDAndNames = "<%= carIDAndName %>";
			ComparePageObject.IsNeedFirstColor = true;
			ComparePageObject.CurrentCarID = '<%= carID.ToString() %>';
			initPageForCompare();
 			<%--bitLoadScript("http://image.bitautoimg.com/carchannel/jsnew/JsForParameterv3.0.js?v=20150807", function () {
			try {
				ComparePageObject.CarIDAndNames = "<%= carIDAndName %>";
				ComparePageObject.IsNeedFirstColor = true;
				ComparePageObject.CurrentCarID = '<%= carID.ToString() %>';
				initPageForCompare();
			}
			catch (err) { }
			}, "utf-8");--%>
			(function () {
				if (window.navigator.userAgent.indexOf("Chrome") !== -1) {
					var count = 0;
					var timer = setInterval(function () {
						count++;
						if (count > 60) clearInterval(timer);
						var obj = $("body>div>object[data*='irs01.net']");
						if (obj.length > 0) {
							clearInterval(timer);
							obj.parent("div").css({ right: "" });
						}
					}, 500);
				}
			})();
			$(function () {
				$("#backtop").attr("href", "javascript:;").bind("click", function () {
					$("html,body").animate({ scrollTop: 0 }, 300, function () {
						//modified by 2014.07.17 ie6 7 8 修改 滚动监听 回不到基本信息
						if (! -[1, ]) {
							$("#left-nav li:first").addClass("current").siblings().removeClass("current");
						}
					});
				});
			});
			CarCompareObj.Init("btn-compare-car button_gray","button_gray btn-compare-car add");
			WaitCompareObj.Init();
		</script>
 	</div>
	<!--#include file="~/html/footer2014.shtml"-->
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript" language="javascript">
		OldPVStatistic.ID1 = "<%=cbe.Serial.Id %>";
		OldPVStatistic.ID2 = "<%=carID.ToString() %>";
		OldPVStatistic.Type = 0;
		mainOldPVStatisticFunction();
	</script>
	<!-- baa 浏览过的车型-->
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
	<script type="text/javascript">
		try {
			Bitauto.UserCars.addViewedCars('<%=cbe.Serial.Id %>');
		}
		catch (err)
		{ }
	</script>
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfhdbfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=cbe.Serial.Id%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
		var zamplus_tag_params = {
			modelId:modelStr,
			carId:<%=carID%>
			};
	</script>
	<script type="text/javascript">
		var _zampq = [];
		_zampq.push(["_setAccount", "12"]);
		(function () {
			var zp = document.createElement("script"); zp.type = "text/javascript"; zp.async = true;
			zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
			var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(zp, s);
		})();
	</script>
</body>
</html>
