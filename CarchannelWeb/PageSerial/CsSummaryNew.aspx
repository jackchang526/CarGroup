<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummaryNew.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsSummaryNew" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>
		<%= "【"+serialSeoName+"】"+ masterBrandName+ serialName+"_"+ serialSeoName+"报价_"+ serialSeoName+"论坛_油耗-易车网" %>
	</title>
	<meta name="Keywords" content="<%= serialSeoName%>,<%= serialSeoName%>报价,<%= serialSeoName%>价格,<%= serialSeoName%>参数,<%= serialSeoName%>论坛,<%= serialSeoName%>图片,<%= serialSeoName%>油耗,<%= serialSeoName%>口碑,<%= masterBrandName%><%= serialSeoName%>" />
	<meta name="Description" content="<%= cse.Brand.Name%><%= serialName%>频道为您提供<%= serialSeoName%>最新报价, <%= serialSeoName%>参数配置, <%= serialSeoName%>图片, <%= serialSeoName%>油耗,二手<%= serialSeoName%>,<%= serialSeoName%>经销商信息,同时还提供<%= serialSeoName%>论坛和关于<%= serialSeoName%>怎么样等信息,查<%= serialSeoName%>最新报价,就上易车网" />
	<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
	<meta http-equiv="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=serialSpell %>/" />
	<meta http-equiv="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/w/carserial.aspx?serialid=<%=serialId %>" />
	<link rel="canonical" href="http://car.bitauto.com/<%=serialSpell %>/" />
	<!--#include file="~/ushtml/0000/car_summary_v2-252.shtml"-->
	<!-- baidu-tc/2.0 page { "title": "<%= "【"+serialSeoName+"】"+ masterBrandName+ serialName+"_"+ serialSeoName+"报价_"+ serialSeoName+"论坛_油耗-易车网" %>", "page_type": "CONTENT", "default_action": "SHOW" }-->
	<base target="_blank" />
</head>
<body>
	<a name="pageTopForFeedback"></a>
	<script language="javascript">
		if (typeof (bitLoadScript) == "undefined")
			bitLoadScript = function (url, callback, charset) {
				var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
				if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
				else { s.onload = function () { if (callback) callback(); }; }
				s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
			};
	</script>
	<!-- baidu-tc begin {"action":"DELETE"} -->
	<!--书角广告代码-->
	<ins id="div_0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a" type="ad_play" adplay_ip="" adplay_areaname=""
		adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
		adplay_blockcode="0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a"></ins>
	<!-- 顶通新增收起广告(2596锐界) -->
	<ins id="div_2e7e5549-b9d7-4fb3-bec7-9f7b39efe715" type="ad_play" adplay_ip="" adplay_areaname=""
		adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
		adplay_blockcode="2e7e5549-b9d7-4fb3-bec7-9f7b39efe715"></ins>
	<!-- 顶通新增收起广告(2596锐界)-->
	<div class="bt_ad">
		<ins id="div_2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="2d0a1c4b-d4f6-42b4-92e3-3ab21fab8d02"></ins>
	</div>
	<!-- baidu-tc end -->
	<!-- baidu-tc begin {"action":"FOLD"} -->
	<!--#include file="~/html/header2012.shtml"-->
	<!-- baidu-tc end -->
	<!--smenu start-->
	<!--a_d start-->
	<!-- baidu-tc begin {"action":"DELETE"} -->
	<div class="bt_ad">
		<%=serialTopAdCode%>
	</div>
	<!-- baidu-tc end -->
	<!--a_d end-->
	<%= CsHead %>
	<!--page start-->
	<!-- baidu-tc begin {"action":"SHOW"} -->
	<div class="bt_page">
		<!-- 概览 -->
		<%=SerialInfoBarHtml%>
		<script type="text/javascript">
			function showCarList() { document.getElementById("this").style.display = "block"; document.getElementById("tit").style.color = "#000"; document.getElementById("tit").style.cursor = "default"; }
			function hideCarList() { document.getElementById("this").style.display = "none"; document.getElementById("tit").style.color = "#164A84"; document.getElementById("tit").style.cursor = "pointer"; }
		</script>
		<!-- Dec.6.2011 new AD -->
		<!-- baidu-tc begin {"action":"DELETE"} -->
		<ins id="div_d8728661-13a7-4a8c-8287-71f7e6d605c2" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="d8728661-13a7-4a8c-8287-71f7e6d605c2"></ins>
		<!-- baidu-tc end -->
		<!-- -->
		<!-- 两 栏之图片 -->
		<div class="col-con">
			<!-- 三栏之左栏 -->
			<div class="col-sub">
				<!-- baidu-tc begin {"action":"DELETE"} -->
				<div class="col-sub_ad">
					<ins id="div_1ae7f3d9-c409-4b06-b739-9d18aeed10db" type="ad_play" adplay_ip="" adplay_areaname=""
						adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="1ae7f3d9-c409-4b06-b739-9d18aeed10db"></ins>
				</div>
				<!-- baidu-tc end -->
				<!-- 焦点图 -->
				<%= CsPicJiaodian %>
				<!-- 概况 -->
				<div class="clear">
				</div>
				<%=CsDetailInfo%>
				<!--买车必看-->
				<%=CsMustSeeInfo %>
			</div>
			<!-- 三栏之中栏 -->
			<div class="col-main ofvis">
				<!-- 列表 导购推荐 论坛话题-->
				<div class="line_box h317" style="z-index: 2">
					<%=focusNewsHtml %>
				</div>
				<div class="line_box h154">
					<%=daogouHtml %>
				</div>
				<div class="line_box">
					<%=forumHtml %>
				</div>
				<div class="ad-41030">
					<ins id="div_ab5d1cc0-0115-4e6b-b8fb-e8f68ed85efd" type="ad_play" adplay_ip="" adplay_areaname=""
						adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
						adplay_blockcode="ab5d1cc0-0115-4e6b-b8fb-e8f68ed85efd"></ins>
				</div>
			</div>
			<div class="clear">
			</div>
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<div class="summaryMiddleAD">
				<ins id="middleADForCar" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="37940534-3acb-4358-8f99-ac9abc6624ca">
				</ins>
			</div>
			<!-- 单一车型页（城市）/中部通栏 -->
			<div class="summaryMiddleAD">
				<ins id="div_fefd085a-31d6-44f3-9ba0-e75930818d3f" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="fefd085a-31d6-44f3-9ba0-e75930818d3f"></ins>
			</div>
			<!-- baidu-tc end -->
			<!-- 核心看点 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<%if (!SpecialSerialIdArray.Contains(serialId)) { WriteHexinkandianHTML(); }%>
			<!-- baidu-tc end -->
			<!-- 在销车型 -->
			<div class="line_box" id="car_list" style="z-index: 999;">
				<%=carListTableHtml%>
			</div>
			<%--<!--分类位置图片-->
			<%=serialPositionImageHtml%>--%>
			<!--彩虹条-->
			<%=rainbowHtml%>
			<!--某车图释-->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<%=serialImageCarHtml%>
			<!-- baidu-tc end -->
			<!-- 某车图片 -->
			<%=serialImageHtml%>
			<!-- 视频 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<%=videosHtml%>
			<!-- baidu-tc end -->
			<!-- 口碑 -->
			<div class="line_box  choice">
				<%= dianpingHtml%>
			</div>
			<%if (SpecialSerialIdArray.Contains(serialId)) { WriteHexinkandianHTML(); }%>
			<!--试驾评测-->
			<%=editorCommentHtml%>
			<!-- 经销商 -->
			<div class="line_box dealer" id="vendorInfo">
				<h3>
					<span><a target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">
						<%=serialSeoName %>-经销商推荐</a></span></h3>
				<ins id="ep_union_4" partner="1" version="" isupdate="1" type="1" city_type="-1"
					city_id="0" city_name="0" car_type="2" brandid="0" serialid="<%= serialId %>"
					carid="0"></ins>
				<div class="more">
					<a rel="nofollow" target="_blank" href="http://dealer.bitauto.com/<%= serialSpell %>/">
						更多&gt;&gt;</a></div>
			</div>
			<!-- 答疑 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<%= serialAskHtml %>
			<!-- baidu-tc end -->
			<!--维修保养-->
			<!--%=maintainceHtml%-->
			<!-- 单一车型页（城市）/底部通栏 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<div class="summaryMiddleAD">
				<ins id="div_043ff6cd-b37a-4ca8-a25c-c81e8b05a94a" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="043ff6cd-b37a-4ca8-a25c-c81e8b05a94a"></ins>
				<!--modified by sk 2013.04.10-->
				<ins id="div_9710ab96-a655-4fcc-b2a3-5fb823705f6e" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="9710ab96-a655-4fcc-b2a3-5fb823705f6e"></ins>
			</div>
			<!-- baidu-tc end -->
			<!-- SEO导航 -->
			<div class="line_box cs0623_01">
				<dl>
					<dt>接下来要看：</dt>
					<dd>
						<ul>
							<li><a rel="nofollow" href="/<%= serialSpell %>/peizhi/" target="_self">
								<%= serialShowName%><span>参数配置</span></a></li>
							<li><a rel="nofollow" href="/<%= serialSpell %>/tupian/" target="_self">
								<%= serialShowName%><span>图片</span></a></li>
							<%=nextSeePingceHtml %>
							<li><a rel="nofollow" href="/<%= serialSpell %>/baojia/" target="_self">
								<%= serialShowName%><span>报价</span></a></li>
							<li><a rel="nofollow" href="http://www.taoche.com/buycar/serial/<%= serialSpell %>/"
								target="_blank">
								<%= serialShowName%><span>二手车</span></a></li>
							<li><a rel="nofollow" href="/<%= serialSpell %>/koubei/" target="_self">
								<%= serialShowName%><span>怎么样</span></a></li>
							<li><a rel="nofollow" href="/<%= serialSpell %>/youhao/" target="_self">
								<%= serialShowName%><span>油耗</span></a></li>
							<li><a rel="nofollow" href="<%= baaUrl %>">
								<%= serialShowName%><span>论坛</span></a></li>
							<%=nextSeeDaogouHtml %>
						</ul>
					</dd>
				</dl>
				<div class="clear">
				</div>
			</div>
			<!-- SEO导航 -->
			<!-- SEO底部热门 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<!--#include file="~/html/SeoBottomHotListShort.shtml"-->
			<!-- baidu-tc end -->
			<!-- SEO底部热门 -->
		</div>
		<div class="col-side">
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<!-- ad -->
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="9957c7cc-f9ae-431e-bfc6-270e006a285e">
				</ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="040b5cb9-85ab-485f-a32b-5bfad0b9d891">
				</ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="ADCSSummaryRight3" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
					adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="77558cb7-bd1f-4f3c-9c6b-bb0ac8ffdc26">
				</ins>
			</div>
			<!-- New AD Dec.20.2011 -->
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="div_4411299b-01d5-4ecc-be88-ee96caa343db" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="4411299b-01d5-4ecc-be88-ee96caa343db"></ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="div_2e763592-7039-452a-aa1c-a6db3a446853" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="2e763592-7039-452a-aa1c-a6db3a446853"></ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="div_ec334652-8e11-4911-9062-7bcada8435ea" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="ec334652-8e11-4911-9062-7bcada8435ea"></ins>
			</div>
			<!-- -->
			<!-- baidu-tc end -->
			<%=ImpressionHtml%>
			<!--特价车 开始-->
			<div class="tejiache_box2" style="display: none;" id="tejiache_box">
			</div>
			<!--特价车 结束-->
			<!-- 看过某车的还看过 -->
			<div class="line_box zs100412_1">
				<h3>
					<span>看<%=serialShowName.Replace("(进口)", "").Replace("（进口）", "")%>的还看</span></h3>
				<ul class="pic_list">
					<%=serialToSeeHtml%>
				</ul>
				<div class="hiedline">
				</div>
				<div class="more">
					<a href="#"></a>
				</div>
				<div class="clear">
				</div>
			</div>
			<!-- AD -->
			<!-- add by chengl Sep.13.2012 -->
			<ins id="div_19b0a5f4-6cc0-409f-9973-70c94bb72c9c" type="ad_play" adplay_ip="" adplay_areaname=""
				adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
				adplay_blockcode="19b0a5f4-6cc0-409f-9973-70c94bb72c9c"></ins>
			<!-- 网友都用某车和谁比 -->
			<%=hotSerialCompareHtml %>
			<!--此品牌下其别子品牌-->
			<%=GetBrandOtherSerial() %>
			<!--二手车-->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<%--<%=UCarHtml %>--%>
			<!--# include file="~/html/cheyimai_chexing.shtml"-->
			<div class="line_box ucar_box">
			</div>
			<!-- baidu-tc end -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<div class="col-side_ad" style="width: 220px; margin-bottom: 10px; overflow: hidden">
				<script type="text/javascript" id="zp_script_94" src="http://mcc.chinauma.net/static/scripts/p.js?id=94&w=220&h=220&sl=1&delay=5"
					zp_type="1"></script>
			</div>
			<!-- baidu-tc end -->
			<!-- 画中画广告1、2 -->
			<!-- baidu-tc begin {"action":"DELETE"} -->
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="div_149e57b4-e495-40e1-ae1c-30f4b80d955e" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="149e57b4-e495-40e1-ae1c-30f4b80d955e"></ins>
			</div>
			<div class="col-side_ad" style="width: 220px; overflow: hidden">
				<ins id="div_3a3bdb2a-ab81-415d-8d0a-a3395a4364d3" type="ad_play" adplay_ip="" adplay_areaname=""
					adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
					adplay_blockcode="3a3bdb2a-ab81-415d-8d0a-a3395a4364d3"></ins>
			</div>
			<!-- baidu-tc end -->
			<!--推广-->
			<!--百度推广-End -->
			<div class="clear">
			</div>
		</div>
	</div>
	<!-- baidu-tc end -->
	<!-- 导航脚本 -->
	<%= CsHeadJs %>
	<script type="text/javascript">
	    var serialId = <%=serialId %>;
	    var serialallSpell = '<%=serialSpell %>';
	</script>
	<!-- 对比浮动框 -->
	<!-- baidu-tc begin {"action":"DELETE"} -->
	<div id="divWaitCompareLayer" class="comparebar" style="display: none;">
	</div>
	<!-- baidu-tc end -->
	<!-- 尾 -->
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/jquery.lazyload.min.js"></script>
	<script type="text/javascript">
		$("#exampleCarPic img").eq(0).lazyload({
			placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
			threshold: 50
		});
		$("#carPicSize img").lazyload({
			placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
			threshold: 100,
			skip_invisible: false
		});
		$("#DivPicBlockForStat img,.car-pic img,.car-video20130802 img").lazyload({
			placeholder: "http://image.bitautoimg.com/uimg/index120401/images/picholder.gif",
			threshold: 50
		});
		$.ajax({ url: 'http://image.bitautoimg.com/carchannel/jsnew/CarChannelBaikeJson.js?v=20131008', cache: true, dataType: "script", success: function () {
			$("#car_core > li").each(function () {
				var item = $(this).find("span");
				var title = item.html().replace("：", "").replace(":", "");
				for (var i = 0; i < CarChannelBaikeJson.length; i++) {
					if (CarChannelBaikeJson[i][title] != undefined) {
						item.html("<a rel=\"nofollow\" href=\"" + CarChannelBaikeJson[i][title] + "\" target=\"_blank\">" + title + "：</a>");
						break;
					}
				}
			});
		}
		});
	</script>
	<script src="http://image.bitautoimg.com/carchannel/jsnew/ucarserialcity.js?v=20150115"
		type="text/javascript"></script>
	<!-- 本站代码提前 -->
	<script language="javascript" type="text/javascript" defer="defer">
		bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/JsForWaitComparevSummary.js,carchannel/jsnew/csSummaryv2.4.js?20131129", function () {
			try {
				// getSerialGoodsNew(<%=serialId %>,'<%=serialShowName %>','<%=serialWhiteImageUrl %>',bit_locationInfo.cityId);
				if (document.getElementById('carYearList_all'))
				{ document.getElementById('carYearList_all').className = 'current'; }
				// 子品牌信息 行情价 link
				if (document.getElementById('linkForHQPrice') && typeof (carChannelZoneSpellForHangqing) != 'undefined')
				{ document.getElementById("linkForHQPrice").href = document.getElementById("linkForHQPrice").href + carChannelZoneSpellForHangqing + "/"; }
				// 对比浮动框
				insertWaitCompareDiv();
				// 经销商
				// ShowVendorInfo("<%= serialId %>", bit_locationInfo.cityId);
				//切换城市文字变色
				addUnderline("province_list");
				// 图释切换
				exampleCarColor();
				carPicSize();
				scrollCarColor();
				// 收藏
				//				CsSummaryFavorites.CurrentCsID = "<%= serialId %>";
				//				CsSummaryFavorites.FavoritesTitle = "<%=serialSeoName %>-易车网";
				//				CsSummaryFavorites.CheckIsShowFavorites();
			}
			catch (err)
			{ }
		}, "utf-8");
	</script>
	<!--本站统计代码-->
	<script type="text/javascript" language="javascript">
		bitLoadScript("http://image.bitautoimg.com/mergejs,s=carchannel/jsStat/StatisticJsOldPV.js", function () {
			OldPVStatistic.ID1 = "<%=serialId.ToString() %>";
			OldPVStatistic.ID2 = "0";
			OldPVStatistic.Type = 0;
			mainOldPVStatisticFunction();
		});
	</script>
	<!--本站统计结束-->
	<!-- 本站代码提前 end -->
	<script type="text/javascript">
		var cityId = 201;
		if(typeof (bit_locationInfo) != 'undefined'){
			cityId = bit_locationInfo.cityId;
		}
        if (typeof (showUCar) != "undefined") {
            showUCar(<%=serialId %>, cityId,'<%=serialSpell %>','<%=serialShowName%>');
        }
		//二手车ip定向
		var locationUCar = document.getElementById("location_ucar");
		if (locationUCar) {
			locationUCar.href = locationUCar.href.replace("paesf0bxc", "paesf" + cityId + "bxc");
		}
	</script>
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsnew/ued_common.js?v=20120628"></script>
	<!-- 广告代码要求提前Jul.14.2011 -->
	<script type="text/javascript">
		var adplay_CityName = ''; //城市
		var adplay_AreaName = ''; //区域
		var adplay_BrandID = '<%= serialId %>'; //品牌id 
		var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
		var adplay_BrandName = ''; //品牌
		var adplay_BlockCode = '48de7532-95b6-4100-9662-593c4f8533a2'; //广告块编号
	</script>
	<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
	<script type="text/javascript">
		var adplay_CityName = ''; //城市
		var adplay_AreaName = ''; //区域
		var adplay_BrandID = '<%= serialId %>'; //品牌id 
		var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
		var adplay_BrandName = ''; //品牌
		var adplay_BlockCode = '0db5acb8-dd8f-4b8d-97cb-cc7d50a71b8a'; //广告块编号
	</script>
	<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
	<!-- AD -->
	<script type="text/javascript">
		var adplay_CityName = ''; //城市
		var adplay_AreaName = ''; //区域
		var adplay_BrandID = '<%= serialId %>'; //品牌id 
		var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
		var adplay_BrandName = ''; //品牌
		var adplay_BlockCode = '820925db-53c1-4bf8-89d2-198f4c599f4e'; //广告块编号
	</script>
	<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
	<script type="text/javascript" language="javascript">
		var CarCommonCSID = '<%= serialId.ToString() %>';
	</script>
	<!--#include file="~/html/footer2012.shtml"-->
	<%if (cse.Level != null && cse.Level.Name != "概念车")
   {%>
	<!-- baa 浏览过的车型-->
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
	<script type="text/javascript">
		try {
			Bitauto.UserCars.addViewedCars('<%=serialId.ToString() %>');
		}
		catch (err)
		 { }
	</script>
	<%} %>
	<!--footer end-->
	<%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388 || serialId == 2122 || serialId == 2196 || serialId == 1611 || serialId == 3152 || serialId == 2871 || serialId == 3382)
   { %>
	<!--百度热力图-->
	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<%} %>
	<!--提意见浮层-->
	<!--#include file="~/html/yiche_oldside.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type='text/javascript'>
		var _zpq = _zpq || [];
		_zpq.push(['_setPageID', '38']);
		_zpq.push(['_setPageType', 'productPage']);
		_zpq.push(['_setParams', '<%= serialId %>']);
	</script>
	<script type='text/javascript'>
		var _zpq = _zpq || [];
		_zpq.push(['_setAccount', '12']);
		(function () {
			var zp = document.createElement('script'); zp.type = 'text/javascript'; zp.async = true;
			zp.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'cdn.zampda.net/s.js';
			var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(zp, s);
		})();
	</script>
</body>
</html>
<!-- 经销商块改INS -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
