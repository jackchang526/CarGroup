﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WirelessWeb._Default" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【汽车大全|汽车标志_汽车标志大全_热门车型大全】-手机易车网</title>
	<meta name="keywords" content="汽车大全,汽车标志,车型大全,汽车标志大全" />
	<meta name="description" content="汽车大全:手机易车网车型大全频道为您提供各种汽车品牌型号信息,包括汽车报价,汽车标志,汽车图片,汽车经销商,汽车油耗,汽车资讯,汽车点评,汽车问答,汽车论坛等等……" />
	<!--#include file="~/ushtml/0000/myiche2015_cube_xuanche_style-news-1004.shtml"-->
</head>
<body class="bg-fff">
	<!-- header start -->
	<div class="op-nav">
		<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

		<div class="tt-name"><a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>选车</h1>
		</div>
		<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
	</div>
	<div class="op-nav-mark" style="display: none;"></div>
	<!-- header end -->
	<!-- 标签切换 start -->
	<div class="first-tags">
		<ul>
			<li id="xc_brand"><a href="/brandlist.html" data-channelid="27.150.1436"><span>按品牌</span></a></li>
			<li class="current"><a href="#" data-channelid="27.150.1435"><span>按条件</span></a></li>
            <li id="xc_scene" class="tags-new"><a href="/scenelist.html" data-channelid="27.150.1416"><span>按情景<b><i>NEW<i></i></i></b></span></a></li>
		</ul>
	</div>
	<!-- 标签切换 end -->

	<!-- 选车工具 start -->

	<%--最近看过 Start--%>
	<div id="xc_recentSee">
	</div>
	<%--  最近看过 End--%>

	<!-- 按价格 start -->
	<div class="tt-small">
		<span>价格</span>
	</div>
	<div class="sort sort4" id="xc_price">
		<ul>
			<li><a href="/xuanchegongju/?p=0-5">5万以下</a></li>
			<li><a href="/xuanchegongju/?p=5-8">5-8万</a></li>
			<li><a href="/xuanchegongju/?p=8-12">8-12万</a></li>
			<li><a href="/xuanchegongju/?p=12-18">12-18万</a></li>
			<li><a href="/xuanchegongju/?p=18-25">18-25万</a></li>
			<li><a href="/xuanchegongju/?p=25-40">25-40万</a></li>
			<li><a href="/xuanchegongju/?p=40-80">40-80万</a></li>
			<li><a href="/xuanchegongju/?p=80-9999">80万以上</a></li>
		</ul>
	</div>
	<!-- 按价格 end -->
	<ins id="div_7729691d-e503-471d-8797-d0c5ea4978ab" type="ad_play" adplay_ip="" adplay_areaname=""
		adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
		adplay_blockcode="7729691d-e503-471d-8797-d0c5ea4978ab"></ins>
	<!-- 按级别 start -->
	<div class="tt-small mt10">
		<span>级别</span>
	</div>
	<div class="sort sort-car sort-imgs sort3" id="xc_level">
		<ul>
			<li>
				<a href="/xuanchegongju/?l=1" data-channelid="27.160.1650">
					<span class="car-weixing"></span>
					<em>微型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=2" data-channelid="27.160.1651">
					<span class="car-xiaoxing"></span>
					<em>小型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=3" data-channelid="27.160.1652">
					<span class="car-jincouxing"></span>
					<em>紧凑型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=5" data-channelid="27.160.1653">
					<span class="car-zhongxing"></span>
					<em>中型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=4" data-channelid="27.160.1654">
					<span class="car-zhongdaxing"></span>
					<em>中大型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=6" data-channelid="27.160.1655">
					<span class="car-haohuaxing"></span>
					<em>豪华型车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=7" data-channelid="27.160.1656">
					<span class="car-mpv"></span>
					<em>MPV</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=8" data-channelid="27.160.1657">
					<span class="car-suv"></span>
					<em>SUV</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=9" data-channelid="27.160.1658">
					<span class="car-paoche"></span>
					<em>跑车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=11" data-channelid="27.160.1660">
					<span class="car-mianbao"></span>
					<em>面包车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?f=16" data-channelid="27.160.1661">
					<span class="car-diandongche"></span>
					<em>电动车</em>
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?l=12" data-channelid="27.160.1662">
					<span class="car-pika"></span>
					<em>皮卡</em>
				</a>
			</li>
            <li>
				<a href="/xuanchegongju/?l=18" data-channelid="27.160.1659">
					<span class="car-keche"></span>
					<em>客车</em>
				</a>
			</li>
		</ul>
	</div>
	<!-- 按级别 end -->
	<!-- 按国别 start -->
	<div class="tt-small mt10">
		<span>国别</span>
	</div>
	<div class="sort sort4  remove-margin-top" id="xc_country">
		<ul>
			<li><a href="/xuanchegongju/?g=1">自主</a></li>
			<li><a href="/xuanchegongju/?g=2">合资</a></li>
			<li><a href="/xuanchegongju/?g=4">进口</a></li>
			<li><a href="/xuanchegongju/?c=4">德系</a></li>
			<li><a href="/xuanchegongju/?c=2">日系</a></li>
			<li><a href="/xuanchegongju/?c=16">韩系</a></li>
			<li><a href="/xuanchegongju/?c=8">美系</a></li>
			<li><a href="/xuanchegongju/?c=484">欧系</a></li>
		</ul>
		<div class="clear"></div>
	</div>
	<!-- 按国别 end -->
	<!-- 按车身 start -->
	<div class="tt-small mt10">
		<span>车身</span>
	</div>
	<div class="sort sort4 remove-margin-top" id="xc_carbody">
		<ul>
			<li>
				<a href="/xuanchegongju/?b=1">两厢
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?b=2">三厢
				</a>
			</li>
			<li>
				<a href="/xuanchegongju/?lv=1">旅行版
				</a>
			</li>
		</ul>
		<div class="clear"></div>
	</div>
	<!-- 按车身 end -->
	<!-- 按变速箱 start -->
	<div class="tt-small mt10">
		<span>变速箱</span>
	</div>
	<div class="sort sort4 remove-margin-top" id="xc_gearbox">
		<ul>
			<li><a href="/xuanchegongju/?t=1">手动档</a></li>
			<li><a href="/xuanchegongju/?t=2">自动档</a></li>
		</ul>
	</div>
	<!-- 按变速箱 end -->
	<!-- 更多选择配置 end -->
	<div class="all-block" style="display: none;">
		<!--排量 start-->
		<div class="tt-small mt10">
			<span>排量</span>
		</div>
		<div class="sort sort4 remove-margin-top" id="xc_displacement">
			<ul>
				<li><a href="/xuanchegongju/?d=0-1.3">1.3L以下</a></li>
				<li><a href="/xuanchegongju/?d=1.3-1.6">1.3-1.6L</a></li>
				<li><a href="/xuanchegongju/?d=1.7-2.0">1.7-2.0L</a></li>
				<li><a href="/xuanchegongju/?d=2.1-3.0">2.1-3.0L</a></li>
				<li><a href="/xuanchegongju/?d=3.1-5.0">3.1-5.0L</a></li>
				<li><a href="/xuanchegongju/?d=5.0-9">5.0L以上</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--排量 end-->

		<!--驱动 start-->
		<div class="tt-small mt10">
			<span>驱动</span>
		</div>
		<div class="sort sort4 remove-margin-top" id="xc_drive">
			<ul>
				<li><a href="/xuanchegongju/?dt=1">前驱</a></li>
				<li><a href="/xuanchegongju/?dt=2">后驱</a></li>
				<li><a href="/xuanchegongju/?dt=252">四驱</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--驱动 end-->

		<!--环保标准 start-->
		<div class="tt-small mt10">
			<span>排　放</span>
		</div>
		<div class="sort sort4 remove-margin-top" id="xc_environmentstandard">
			<ul>
				<li><a href="/xuanchegongju/?more=126">国4</a></li>
				<li><a href="/xuanchegongju/?more=125">国5</a></li>
				<li><a href="/xuanchegongju/?more=127">京5</a></li>
				<li><a href="/xuanchegongju/?more=123">欧4</a></li>
				<li><a href="/xuanchegongju/?more=122">欧5</a></li>
				<li><a href="/xuanchegongju/?more=126_123">国4/欧4</a></li>
				<li><a href="/xuanchegongju/?more=125_122">国5/欧5</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--环保标准 end-->
		<!--油耗 start-->
		<%-- <div class="tt-small mt10">
            <span>油耗</span>
        </div>
        <div class="sort sort4 remove-margin-top">
            <ul>
                <li><a href="###">6L以下</a></li>
                <li><a href="###">6-8L</a></li>
                <li><a href="###">8-10L</a></li>
                <li><a href="###">10-12L</a></li>
                <li><a href="###">12-15L</a></li>
                <li><a href="###">15L以上</a></li>
            </ul>
            <div class="clear"></div>
        </div>--%>
		<!--油耗 end-->
		<!--能源 start-->
		<div class="tt-small mt10">
			<span>能源</span>
		</div>

		<div class="sort sort5 remove-margin-top" id="xc_energy">
			<ul>
				<li><a href="/xuanchegongju/?f=7" data-channelid="27.160.1663">汽油</a></li>
				<li><a href="/xuanchegongju/?f=8" data-channelid="27.160.1664">柴油</a></li>
				<li><a href="/xuanchegongju/?f=16" data-channelid="27.160.1665">纯电动</a></li>
				<li><a href="/xuanchegongju/?f=2" data-channelid="27.160.1666">油电混合</a></li>
				<li><a href="/xuanchegongju/?f=4" data-channelid="27.160.1667">油气混合</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--能源 end-->
		<!--车门数 start-->
		<div class="tt-small mt10">
			<span>车门数</span>
		</div>
		<div class="sort sort4 remove-margin-top" id="xc_door">
			<ul>
				<li><a href="/xuanchegongju/?more=268">2-3门</a></li>
				<li><a href="/xuanchegongju/?more=270">4-6门</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--车门数 end-->
		<!--座位数 start-->
		<div class="tt-small mt10">
			<span>座位数</span>
		</div>

		<div class="sort sort5 remove-margin-top" id="xc_seats">
			<ul>
				<li>
					<a href="/xuanchegongju/?more=262">2座</a>
				</li>
				<li>
					<a href="/xuanchegongju/?more=263">4-5座</a>
				</li>
				<li>
					<a href="/xuanchegongju/?more=265">6座</a>
				</li>
				<li>
					<a href="/xuanchegongju/?more=266">7座</a>
				</li>
				<li>
					<a href="/xuanchegongju/?more=267">7座以上</a>
				</li>
			</ul>
			<div class="clear"></div>
		</div>
		<!--座位数 end-->

		<div class="tt-small mt10">
			<span>选择配置（可多选）</span>
			<a href="javascript:void(0);" class="clear" id="clearsetting">清空配置</a>
		</div>
		<div class="sort sort4 remove-margin-top" id="xc_settings">
			<ul>
				<li><a href="javascript:void(0)" id="mcCheck204">天窗</a></li>
				<li><a href="javascript:void(0)" id="mcCheck196">GPS导航</a></li>
				<li><a href="javascript:void(0)" id="mcCheck200">倒车影像</a></li>
				<li><a href="javascript:void(0)" id="mcCheck180">儿童锁</a></li>
				<li><a href="javascript:void(0)" id="mcCheck102">涡轮增压</a></li>
				<li><a href="javascript:void(0)" id="mcCheck179">无钥匙启动</a></li>
				<li><a href="javascript:void(0)" id="mcCheck141">四轮碟刹</a></li>
				<li><a href="javascript:void(0)" id="mcCheck250">真皮座椅</a></li>
				<li><a href="javascript:void(0)" id="mcCheck184">ESP</a></li>
				<li><a href="javascript:void(0)" id="mcCheck224">氙气大灯</a></li>
				<li><a href="javascript:void(0)" id="mcCheck194">定速巡航</a></li>
				<li><a href="javascript:void(0)" id="mcCheck274">自动空调</a></li>
				<li><a href="javascript:void(0)" id="mcCheck177">胎压监测</a></li>
				<li><a href="javascript:void(0)" id="mcCheck189">自动泊车</a></li>
				<li><a href="javascript:void(0)" id="mcCheck249">空气净化器</a></li>
				<%--  <li><a href="javascript:void(0)" id="mcCheck209">电动窗防夹</a></li>--%>
				<li><a href="javascript:void(0)" id="mcCheck163">换档拨片</a></li>
				<li><a href="javascript:void(0)" id="mcCheck236">电动座椅</a></li>
				<li><a href="javascript:void(0)" id="mcCheck181">儿童座椅接口</a></li>
			</ul>
			<div class="clear"></div>
		</div>
		<div class="confirm-box">
			<a href="javascript:void(0);" class="disable" id="savesetting">确定</a>
			<span>已经选择<em>0</em>项</span>
		</div>
		<div class="clear"></div>
	</div>
	<!-- 更多选择配置 end -->
	<a href="javascript:void(0)" class="more article-pages-txt-show">更多选择条件<i></i></a>
	<!-- 广告 start -->
	<ins id="div_90547610-1f85-4440-bf4c-e4d104c260dd" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="90547610-1f85-4440-bf4c-e4d104c260dd"></ins>
	<!-- 广告 end -->

	<!--# include file="~/include/pd/2014/wap/00001/201407_wap_yytj_Manual.shtml" -->
	<!--#include file="~/include/pd/2014/wap/00001/201507_wap_index_ydtj_Manual.shtml" -->
	<!-- wap端新增资源的广告 -->
	<ins id="div_2a05eecc-6416-4c0b-ab7e-018e54388d3a" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="2a05eecc-6416-4c0b-ab7e-018e54388d3a"></ins>
	<%--<ins adplay_blockcode="4518731c-308b-4e2c-ab40-fb07fba381e1" adplay_brandtype="" adplay_brandname="" adplay_brandid="" adplay_cityname="" adplay_areaname="" adplay_ip="" type="ad_play" id="div_4518731c-308b-4e2c-ab40-fb07fba381e1">
      
    </ins>--%>
	<div class="footer15">
		<!--搜索框-->
		<!--#include file="~/html/footersearch.shtml"-->
		<div class="breadcrumb">
			<div class="breadcrumb-box">
				<a href="http://m.yiche.com/">首页</a> &gt; <span>选车</span>
			</div>
		</div>
		<!--#include file="~/html/footerV3.shtml"-->
	</div>
	<div class="float-r-box">
		<!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
	</div>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js?v=20130606"> </script>
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"> </script>
	<script type="text/javascript">
		var url = "http://m.yiche.com/";
		var viewedList = "";
		for (var i = 0; i < Bitauto.UserCars.viewedcar.arrviewedcar.length && i < 4; i++) {
			viewedList += Bitauto.UserCars.viewedcar.arrviewedcar[i];
			if (i != Bitauto.UserCars.viewedcar.arrviewedcar.length - 1) {
				viewedList += ",";
			}
		}
		loadViewedCars(viewedList);
		//根据浏览过的车系Id集合获取浏览车
		function loadViewedCars(serialIdList) {
			if (typeof serialIdList == "undefined" || serialIdList <= 0)
				return;
			var url = "http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csIDList=" + serialIdList + "&op=getviewedcar";
			loadJS.push(url, 'utf-8', jsonpCallBackForViewed);
		}

		//将浏览过的车写入dom
		function jsonpCallBackForViewed(data) {
			var viewed = document.getElementById("xc_recentSee");
			if (viewed) {
				var tempHtml = typeof jjviewed == "undefined" ? "<div class=\"none-con-list\">无最近浏览</div>" : getViewedCarHtml(jjviewed);
				viewed.innerHTML = tempHtml;
			}
		}
		//最近看过
		function getViewedCarHtml(jjviewed) {
			var html = [];
			if (typeof jjviewed["nlist"] != 'undefined'
                && jjviewed["nlist"] != null
                && jjviewed["nlist"].length > 0) {
				html.push("<div class='tt-small'>");
				html.push("<span>最近看过：</span>");
				html.push("</div>");
				html.push("<div class='f-sort'>");
				html.push("<ul class='scroll'>");
				for (var i = 0; i < jjviewed["nlist"].length; i++) {
					var viewedValue = eval(jjviewed["nlist"][i]);
					if (viewedValue == null) continue;
					var spell = viewedValue.CsAllSpell;
					var title = viewedValue.CsName;
					var image = viewedValue.CsImage;
					var price = viewedValue.CsPrice == "" ? "暂无报价" : viewedValue.CsPrice;
					if (title.length >= 5)
					{ html.push("<li class='max5'><a href='" + spell + "'>" + title + "</a></li>"); }
					else
					{
						html.push("<li><a href='" + spell + "'>" + title + "</a></li>");
					}
				}
				html.push("</ul>");
				html.push("<div class='clear'></div>");
			} else {
				html.push("<div class='tt-small'><span>最近看过：</span></div><div class='f-sort'>暂无浏览记录</div>");
			}
			return html.join("");
		}


	</script>

	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/model.js?v=20150907"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/swiper.js?v=2015082816"> </script>

	<script type="text/javascript">
		//在“完成”事件中，需要配置一个全局变量供页面跳转
		var _SearchUrl = "/xuanchegongju/";
	</script>
	<script type="text/javascript">
		$(function () {
			// 应用下载  左右滑动效果
			var mySwiperApp = new Swiper('#m-app-part-scroll', {
				pagination: '.pagination-app',
				loop: true,
				grabCursor: true,
				paginationClickable: true
			});
			$(document).ready(function () {
				if ($("#m-app-part-scroll").find("li").length < 4) {
					$(".pagination-app").hide();
				};
			});

			$('.footer-box').css('padding', 0);

			//监听下拉收起事件
			var $allblock = $('.all-block');
			$('.more').on('click', function (ev) {
				ev.preventDefault();
				var $this = $(this);
				if ($allblock[0].style.display == 'none') {
					$allblock.slideDown(function () {
						$this.html('收起<i></i>');
						$this.addClass('article-pages-txt-hide').removeClass('article-pages-txt-show');
					});

				} else {
					$allblock.slideUp(function () {
						$this.html('更多选择条件<i></i>');
						$this.addClass('article-pages-txt-show').removeClass('article-pages-txt-hide');
					});
				}
			});


			var countSetting = 0;
			//监听"选择配置"各a标签点击事件：  第1次点击选中，第2次点击取消选中
			$("#xc_settings").on("click", "ul li a", function (e) {
				var eve = e || window.event;
				var $curSetting = $(eve.target);
				if ($curSetting.attr("class") && $curSetting.attr("class") == "currnet") {
					$curSetting.removeClass("currnet");
					if (countSetting > 0) { countSetting -= 1; }
				}
				else {
					$curSetting.addClass("currnet");
					//计数加1
					countSetting += 1;
				}
				if (countSetting > 0) {
					$("#savesetting").removeClass("disable");
				}
				else {
					$("#savesetting").addClass("disable");
				}
				$(".confirm-box span em").html(countSetting);
			});
			//监听清空配置事件
			$("#clearsetting").on("click", function () {
				$("#xc_settings ul li a").each(function () {
					var $curA = $(this);
					if ($curA.attr("class") && $curA.attr("class") == "currnet") {
						$curA.removeClass("currnet");
					}
				});
				countSetting = 0;
				$("#savesetting").addClass("disable");
				$(".confirm-box span em").html(countSetting);
			});

			//监听 选择配置-“完成” 事件 
			$("#savesetting").on("click", function () {
				var MoreCondition = [];
				var allCheckedLi = $("#xc_settings").find("[id^='mcCheck']");
				$(allCheckedLi).each(function (liIndex, liItem) {
					if ($(liItem).hasClass("currnet")) {
						var curCheckNo = $(liItem).attr("id").replace("mcCheck", "");
						MoreCondition.push(curCheckNo);
					}
				});
				var mc = MoreCondition.join('_');
				if (mc.length > 0) {
					window.location.href = '/xuanchegongju/?more=' + mc;
				}
			});

			//监听最下方搜索文本处清空文本事件
			$(".m-btn-close").on('click', function () {
				$("input[name='txtkeyword']").val('');
			});
		});


		if (typeof (bitLoadScript) == "undefined")
			bitLoadScript = function (url, callback, charset) {
				var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
				if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
				else { s.onload = function () { if (callback) callback(); }; }
				s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
			};

		bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
			PageAreaStatistics.init("280,281,282,283,284,285,286,287,288,289,290,291,292");
		}, 'utf-8');

	</script>
</body>
</html>
