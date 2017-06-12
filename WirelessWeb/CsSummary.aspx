<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsSummary.aspx.cs" Inherits="WirelessWeb.CsSummary" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="BitAuto.CarChannel.Model" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title><%= _title %></title>
	<meta name="Keywords" content="<%= _keyWords %>" />
	<meta name="Description" content="<%= _Description %>" />
	<link rel="canonical" href="http://car.m.yiche.com/<%=_serialAllSpell %>/" />
	<!--#include file="~/ushtml/0000/myiche2015_cube_summary-981.shtml"-->
	<script type="text/javascript" charset="utf-8" src="http://ip.yiche.com/iplocation/setcookie.ashx"> </script>
</head>
<body>
	<script type="text/javascript">
		var carlevel = '<%= _serialLevel %>';
		var uCarLowestPrice= '<%= uCarLowestPrice %>';  //二手车最低价
		var serialAllSpell = "<%=_serialAllSpell%>";
	</script>
	<div id="container">
	<!--头部APP推广 -->
        	<!-- ad start -->
	<!--#include file="~/include/pd/2014/wap/00001/201406_wap_top_banner_Manual.shtml"-->
	<!-- ad end -->
         <!--新加弹出层 start-->

    <div id="master_container"  style="z-index:888888;display:none" class="brandlayer mthead">
        	<!--#include file="~/html/compareCarTemplate.html"-->
    </div>
    <!--新加弹出层 end-->
        <div class="op-nav-out">
            <div class="op-nav-static " id="op-nav">
                <div class="op-nav"> 
                    <a href="javascript:void(0);" class="btn-return"  data-channelid="27.23.690" id="gobackElm">返回</a>
                    <div class="tt-name"><a href="http://m.yiche.com/" class="yiche-logo" data-channelid="27.23.1032">易车</a><h1><%= _serialShowName %></h1></div>

                    <div class="function">
                        <a href="http://car.m.yiche.com/brandlist.html" data-channelid="27.23.455">换车</a>
                    </div>
                    <a href="javascript:RedirectShowSearch();" class="btn-search" data-channelid="27.23.1034">搜索</a>
                    <a href="javascript:void(0);" class="btn-menu" id="h_nav" data-channelid="27.23.1033">
                        <span><em class="em-f"></em><em></em><em></em><i></i></span>
                    </a>
                    <div class="menu-pop" id="h_popNav" style="display: none;">
                        <div class="menu-crumbs">
                            <a href="http://m.yiche.com/" data-channelid="27.23.456">首页</a><i></i><a href="http://car.m.yiche.com/brandlist.html" data-channelid="27.23.457">选车</a><i></i><%= _serialShowName %>
                        </div>
                        <ul class="menu-nav">
                            <li><a href="http://m.yiche.com/" data-channelid="27.23.501">首页</a></li>
                            <li><a href="http://car.m.yiche.com/brandlist.html" data-channelid="27.23.502">选车</a></li>
                            <li><a href="http://price.m.yiche.com/" data-channelid="27.23.680">报价</a></li>
                            <li><a href="http://dealer.m.yiche.com/" data-channelid="27.23.681">经销商</a></li>
                            <li><a href="http://koubei.m.yiche.com/" data-channelid="27.23.682">口碑</a></li>
                            <li><a href="http://m.taoche.com/all/" data-channelid="27.23.683">二手车</a></li>
                            <li><a href="http://news.m.yiche.com/" data-channelid="27.23.684">文章</a></li>
                            <li><a href="http://v.m.yiche.com/" data-channelid="27.23.685">视频</a></li>
                            <li><a href="http://baa.m.yiche.com/" data-channelid="27.23.686">论坛</a></li>
                            <li><a href="http://ask.m.yiche.com/" data-channelid="27.23.687">问答</a></li>
                            <li><a href="http://yiqishuo.m.yiche.com/" data-channelid="27.23.688">易起说</a></li>
                            <li><a href="http://app.m.yiche.com/" data-channelid="27.23.689">APP</a></li>
                            <li><a></a></li>
                            <li><a></a></li>
                            <li><a></a></li>
                        </ul>
                        <div class="menu-login" data-channelid="26.4.4" id="LoginContainer">
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript" src="http://image.bitautoimg.com/bsearch/mobilesug201506/showsearchbox.js"></script>

		<%--<div class="b-return">
			<a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
			<span><%= _serialShowName %></span>
			<div class="opt" data-channelid="27.23.455">
				<a href="/brandlist.html">换车</a>
			</div>
		</div>--%>
		<%=CsHeadHTML %>
		<div class='sum-car-img'>
			<% if (focusImg.Count > 0)
	  { %>
			<%= focusImg[0] %>
			<div class='right-area'> 
				<% for (int count = 2; count <= focusImg.Count; count++)
	   { %>
				<%= focusImg[count - 1] %>
				<% } %>
			</div>
			<% } %>
		</div>
		<div class='clear'></div>

		<div class='sum-info'>

			<div class='car-main'>
				<div class='car-logo'>
					<a href="http://car.m.yiche.com/<%=_serialEntity.Brand.MasterBrand.AllSpell %>/"><img src='http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%=_serialEntity.Brand.MasterBrand.Id %>_100.png'></a>
				</div>
				<dl>
					<dt><h2><%= _serialSeoName %></h2></dt>
					<dd>
						<a href='http://price.m.yiche.com/nb<%=_serialId %>/' data-channelid="27.23.952">
							<strong><%= _serialPrice %></strong><i class='dujia' id="dujia" style="display: none">独家</i>
						</a>
					</dd>
				</dl>
				<a href='javascript:void(0);' id="favstar" data-channelid="27.23.726" class='ico-favorite'>
					<i></i>
					关注
				</a>
				<%--<a href='#' class='ico-favorite ico-favorite-sel'>
                        <i></i>
                        已关注
                    </a>--%>
			</div>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
			<script type="text/javascript">
				//登录 车型关注
				function initLoginFavCar(carLoginresult) {
					if (carLoginresult.isLogined) {
						try {
							var added = false;
							if (typeof carLoginresult != 'undefined' && typeof carLoginresult.plancar != 'undefined' && carLoginresult.plancar.length > 0) {
								for (var i = 0; i < carLoginresult.plancar.length; i++) {
									if (carLoginresult.plancar[i].CarSerialId == '<%= _serialId %>') {
			                    		added = true;
			                    		$("#favstar").html("<i></i>已关注").addClass("ico-favorite ico-favorite-sel");
			                    		break;
			                    	}
			                    }
							}
							if (!added) {
								var hash;
								hash = window.location.hash;
								if (hash && hash == "#add") {
									var obj = $("#favstar");
									FocusCar(obj);
									location.hash = "";
								}
							}
							$("#favstar").bind("click", function() {
								FocusCar($(this));
							});
						} catch(e) {}
					} else {
			        	//location.hash = "add";
						$("#favstar").attr("href", 'http://i.m.yiche.com/authenservice/login.aspx?returnUrl=' + encodeURIComponent(location.href)+'#add');
					}
				}
				//添加 取消关注车型
				function FocusCar(obj) {
					var id = <%= _serialId %>;
                	obj.attr('class') == "ico-favorite" ? Bitauto.UserCars.addConcernedCar(id, function() {
                		if (Bitauto.UserCars.concernedcar.message[0] == "已超过上限") {
                			alert("关注数量已达上限");
                		} 
                		else {
                			obj.addClass("ico-favorite ico-favorite-sel").html("<i></i>已关注");
                			Bitauto.UserCars.concernedcar.arrconcernedcar.unshift(id);
                            
                		}
                	}) : Bitauto.UserCars.delConcernedCar(id, function() {
                		obj.removeClass("ico-favorite-sel").html("<i></i>关注");
                	});
                }
                $(function() {
                	Bitauto.Login.onComplatedHandlers.add("memory once", initLoginFavCar);
                });
			</script>
			<div class='car-price'>
				<dl class='w'>
					<dt class='w3'>指导价：</dt>
					<dd><%= _serialRefPrice %></dd>
				</dl>
				<dl>
					<%--<dt class='w3'>关注度：</dt>--%>
					<dd><%= _serialTotalPV %></dd>

				</dl>
				<div class='clear'></div>

				<% if (isElectrombile)
	   { %>
				<dl class='w'>
					<dt class='w3'>续航：</dt>
					<dd>
						<% if (!string.IsNullOrEmpty(mileageRange))
		 { %>
						<%= mileageRange %>
						<% }
		 else
		 { %>
                                暂无
                            <% } %>
					</dd>
				</dl>
				<% }
	   else
	   { %>
				<dl class='w'>
					<dt class='w3'>油耗：</dt>
					<dd>
						<% if (!string.IsNullOrEmpty(_serialFuelCost))
		 { %>
						<a href='http://car.m.yiche.com/<%= _serialAllSpell %>/youhao/?ref=Hlht_yh' class='a-oil' data-channelid="27.23.953"><%= _serialFuelCost %></a>
						<% }
		 else
		 { %>
                                暂无
                            <% } %>
					</dd>
				</dl>
				<% } %>

				<dl>
					<%--<dt class='w2'>产地：</dt>--%>
					<dd><%= _chanDi %></dd>
				</dl>
				<div class='clear'></div>
				<% if (SerialColorList.Count > 0)
	   { %>
				<dl class='line'>
					<dt class='w3'>颜色：</dt>
					<dd>
						<% foreach (SerialColorEntity serialColorEntity in SerialColorList)
		 { %>
						<%= "<em style='background: " + serialColorEntity.ColorRGB + "'></em>" %>
						<% } %>
					</dd>
				</dl>
				<% } %>
			</div>
			<div class='clear'></div>


			<% if (_serialEntity.SaleState == "停销")
	  { %>
			<div class='sum-btn sum-btn-two' id="feibaoxiaoche" style="display: block;">
				<ul>
					<li><a data-channelid="27.23.118" href='http://m.taoche.com/pinggu/?ref=mchexizsgu'>二手车估价</a></li>
					<li class='btn-org'><a data-channelid="27.23.117" href='http://m.taoche.com/<%=_serialAllSpell %>/?ref=mchexizsmai&leads_source=m002014'>买二手车</a></li>
				</ul>
			</div>
			<% }
	  else if (_serialEntity.SaleState == "待销")
	  { }
	  else
	  { %>
			<div class='sum-btn' id="feibaoxiaoche" style="display: block;">
				<ul>
					<li><a data-channelid="27.23.114" href='http://chedai.m.yiche.com/<%= _serialAllSpell %>/chedai/?from=ycm1&leads_source=m002001'>贷款</a></li>
                    <li><a data-channelid="27.23.115" href='http://zhihuan.m.taoche.com/s<%= _serialId %>/?ref=mchexizshuan&leads_source=m002002'>置换</a></li>
					<li class='btn-org'><a data-channelid="27.23.116" href='http://price.m.yiche.com/zuidijia/nb<%= _serialId %>/?leads_source=m002003'>询底价</a></li>
				</ul>
			</div>
			<% } %>


			<div class='sum-btn sum-btn-two' id="baoxiaoche" style="display: none">
				<ul>
					<li><a data-channelid="27.23.114" href='http://m.yichemall.com/car/Detail/index?modelId=<%= _serialId %>&from=ycm1'>贷款</a></li>
					<%--<li id="mall-shijia" style="display: none;"><a href='#'>试驾</a></li>--%>
					<li class='btn-org'><a data-channelid="27.23.120" href='http://m.yichemall.com/car/Detail/index?modelId=<%= _serialId %>&source=100064'>直销特卖</a></li>
				</ul>
			</div>
		</div>
		<!-- 易车购车服务 start -->
		<div class="car-service" id="carservice" style="display: none">
			<div class="tt-first">
				<h3>易车购车服务</h3>
				<a href="http://11.m.yiche.com/" class="tt-first-double">11.11购车狂欢节</a>
				<div class="opt-more opt-more-gray"><a href="http://gouche.m.yiche.com/sb<%= _serialId %>/">更多</a></div>
			</div>
			<ul id="buycar-youhui" class="one-service service-new">
			</ul>
		</div>
		<!-- 易车购车服务 end -->
        <!-- 限时特惠 start -->
        <div class="prefer-tim">
           <%-- <span class="yh-tap"></span>
            <div class="cont-box">
                <h4>本地限时特惠</h4>
                <p>凯迪拉克团购召集令，<em>千元油卡</em>送送送！</p>
            </div>--%>
        </div>
        <!-- 限时特惠 end -->
		<%= _carList %>

		<!-- 推广 start -->
		<script type="text/javascript">
			function showNewsInsCode(dxc, xxc, mpv, suv) {
				var adBlockCode = xxc;
				if (carlevel == '中大型车' || carlevel == '中型车' || carlevel == '跑车' || carlevel == '豪华车') {
					adBlockCode = dxc;
				} else if (carlevel == '微型车' || carlevel == '小型车' || carlevel == '紧凑型车') {
					adBlockCode = xxc;
				} else if (carlevel == '概念车' || carlevel == 'MPV' || carlevel == '面包车' || carlevel == '皮卡' || carlevel == '其它') {
					adBlockCode = mpv;
				} else if (carlevel == 'SUV') {
					adBlockCode = suv;
				}
				document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
			}

			showNewsInsCode('6a64c6c9-9936-4e56-adf5-71c83f4e07f9', 'dc0429a5-7e32-4adf-b29c-0f0683e64745', '24f397d8-4dc5-4388-ade9-27ced12938f2', '51346d54-224f-4fe4-a166-e41f5c307e28');
		</script>
		<!-- 推广 end -->

		<%--<!-- 易车帮你找低价 start -->
		<span id="ad-img" data-channelid="27.23.729">
			<a href="http://gouche.m.yiche.com/home/YiShuBang/?csId=<%= _serialId %>" class="ad-img">
				<img src="http://image.bitautoimg.com/uimg/mbt2015/images/pic_zhaodijia.png">
			</a>
		</span>
		<!-- 易车帮你找低价 end -->--%>

		<!-- 口碑 start -->
		<div class="tt-first tt-first-no-bd" id="koubei" data-channelid="27.23.730">
			<h3>口碑</h3>
			<div class="opt-more opt-more-gray"><a href="http://car.m.yiche.com/<%= _serialAllSpell %>/koubei/">更多</a></div>
		</div>
		<%= koubeiImpressionHtml %>
		<!-- 口碑 end -->
        <!-- 编辑评车 start -->
        <div class="voice-swiper-outer" style="display:none">
            <div class="voice-swiper" id="voiceSwiper">
                <ul class="swiper-wrapper" id="audio_list">
                    
                </ul>
                <div class="pagination-voice"></div>            </div>
        </div>
        <!-- 编辑评车 start -->
		<!-- 文章 行情 start -->
		<a id="hash_hangqing"></a>
		<div class="tt-first" id="m-tabs-article">
			<ul class="tags-sub tags-sub-left tags-sub-left-tt">
				<% if (!string.IsNullOrEmpty(_serialNews))
	   { %>
				<li class="current"><a href="#">文章</a></li>
				<% } %>
				<li><a href="#" <%= string.IsNullOrEmpty(_serialNews) ? "class='current'" : "" %>>行情</a></li>
			</ul>
			<div id="changecity" class="opt-city" <%= string.IsNullOrEmpty(_serialNews) ? "" : "style='display: none'" %>><a href="javascript:;" id="m-hangqing-city" data-action="province">北京<i></i></a></div>
		</div>
		<div class="swiper-container-article">
			<div class="swiper-wrapper">
				<!-- 文章 start -->
				<% if (!string.IsNullOrEmpty(_serialNews))
	   { %>
				<div class="swiper-slide" id="m_article" data-channelid="27.23.732">
					<%= _serialNews %>
					<%--<a href="http://car.m.yiche.com/<%= _serialAllSpell %>/wenzhang/" class="btn-more"><i>查看更多文章</i></a>--%>
				</div>
				<% } %>
				<!-- 文章 end -->

				<!-- 行情 start -->
				<div class="swiper-slide">
					<div class="news-list" id="carnews1" data-channelid="27.23.733">
					</div>
				</div>
				<!-- 行情 end -->
			</div>
		</div>
		<!-- 文章 行情 end -->

		<script type="text/javascript">
			showNewsInsCode('eb8313b8-1538-46af-8881-5e244e319f29', '5a4a7476-5780-41ba-9d3d-76c45bc305ed', 'dd915105-d655-452f-85dd-9b4787edcf42', 'ae935b54-96a1-4e20-ae6c-c2e1f6605193');
		</script>

		<!-- 视频 start -->
		<% if (VideoList != null && VideoList.Count > 0)
	 { %>
		<div class="tt-first tt-first-no-bd" data-channelid="27.23.734">
			<h3>视频</h3>
			<div class="opt-more opt-more-gray"><a href="http://v.m.yiche.com/car/serial/<%= _serialId %>_0_0.html">更多</a></div>
		</div>
		<div class="pic-txt pic-txt-video" id="m_video" data-channelid="27.23.735">
			<ul>
				<%
		 string ids = "";
		 foreach (VideoEntity videoEntity in VideoList)
		 {
			 ids += videoEntity.VideoId + ",";
				%>
				<li>
					<a href="<%= videoEntity.ShowPlayUrl.Replace("v.bitauto.com","v.m.yiche.com") %>">
						<span>
							<img src="<%= videoEntity.ImageLink.Replace("Video", "newsimg_300x168/Video") %>">
							<i></i>
						</span>
						<p><%= videoEntity.ShortTitle %></p>
						<% var timeSpan = new TimeSpan(0, 0, videoEntity.Duration); %>
						<em><i id="viewcount_<%= videoEntity.VideoId %>">0</i><i class="time"><%= timeSpan.ToString(@"mm\:ss") %></i></em>
					</a>
				</li>
				<% } %>
			</ul>
			<input type="hidden" id="ids" value="<%= ids.Length > 0 ? ids.Substring(0, ids.Length - 1) : "" %>" />
		</div>
		<% } %>
		<script type="text/javascript">
			$(function() {
				var ids = $("#ids").val();
				if ($("#ids").length > 0 && ids.length > 0) {
					$.ajax({
						url: "http://v.bitauto.com/vbase/CacheManager/GetVideoTotalVisitCommentCountByIds?ids=" + ids,
						async: false,
						dataType: "jsonp",
						jsonpCallback: "successHandler",
						cache: true,
						success: function(data) {
							$(data).each(function(index, item) {
								$("#viewcount_" + item.VideoId).html(item.TotalVisit);
							});
						}
					});
				}
			})
		</script>
		<!-- 视频 end -->

		<!-- 论坛热帖 start -->
		<%= _forumNewsHtml %>
		<!-- 论坛热帖 end -->

		<!--"~/include/pd/2012/wap/00001/201317_wap_cxy_ada_Manual.shtml"-->
        <ins id="div_bec90be3-b648-4cd9-8c33-96c6be62451d" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID="<%= _serialId %>"  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="bec90be3-b648-4cd9-8c33-96c6be62451d"> </ins>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/common/cityjsonv2.js?v=2016012210"> </script>
		<a name="a-dealer"></a>
		<script type="text/javascript">
			showNewsInsCode('7246b970-9063-47c3-b140-c9104515047e', '03011683-4c06-42b0-8f97-17687d814a7b', 'c392b629-2113-4dc0-8a2f-4ee04d6a561d', '9b714349-7d34-4389-be52-67985391a3cd');
		   
			var citycode = 201,cityName='北京';
			if (typeof(bit_locationInfo) != "undefined" && typeof(bit_locationInfo.cityId) != "undefined") {
				citycode = bit_locationInfo.cityId;
				cityName=bit_locationInfo.cityName;
			}
			$("#m-hangqing-city").html(cityName + "<i></i>");
			var csId = <%= _serialId %>;
		    document.write('<scri' + 'pt src="http://m.h5.qiche4s.cn/priceapi/ajax/InitData.ashx?action=cardealerlist&brandid=<%= _serialId %>&citycode=' + citycode + '" type="text/javascript"></s' + 'cript>');
		</script>

		<!--贷款 start-->
		<div id="m-loan-title" class="tt-first" style="display: none;" data-channelid="27.23.739">
			<h3>贷款推荐</h3>
			<div class="opt-more opt-more-gray"><a href='http://chedai.m.yiche.com/<%= _serialAllSpell %>/chedai/?from=ycm37'>更多</a></div>
		</div>
		<div id="m-loan-con" class="m-loan" style="display: none;" data-channelid="27.23.740">
			<ul></ul>
		</div>
		<!--贷款 end-->

		<!--养护 start-->
		<div id="yanghu"></div>
		<!--养护 end-->

		<!--看了还看 start-->
		<%= _serialToSee %>
		<!--看了还看 end-->

		<!--最近看过 start-->
		<div class="browse-car" id="more1" data-channelid="27.23.744">
		</div>
		<!--最近看过 end-->

		<!-- black popup start -->
		<%--<div class="leftmask leftmask4" style="display: none;"></div>
		<div id="provinceList" class="leftPopup province" data-back="leftmask3" style="z-index: 40; display: none;">
		</div>
		<div id="cityList" class="leftPopup city" data-back="leftmask3" style="z-index: 50; display: none;">
			<div class="swipeLeft"></div>
		</div>--%>
        <!--省份层 start-->
    <div class="leftmask leftmask6" style="display: none;"></div>
    <div id="provinceList" class="leftPopup province year" data-back="leftmask6" style="display: none;">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--省份层 end-->

    <!--市层 start-->
    <div id="cityList" class="leftPopup month model2 city" data-back="leftmask6" style="display: none;">
        <div class="swipeLeft swipeLeft-sub">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--市层 end-->
		<!-- black popup end -->

		<!--#include file="~/include/pd/2014/wap/00001/201507_wap_index_ydtj_Manual.shtml" -->
		<script type="text/javascript">
			showNewsInsCode('893f9a91-0f1e-484c-8263-37d815e64d1b', '09dc099f-5b52-4a70-8edd-faa0d7ff4818', '469f45cc-1b72-461c-ba68-0b560c885455', 'cd11763a-ea3e-4315-b054-8baecd23cab8');
			showNewsInsCode('d919899b-178d-42c6-bfdf-13d8fe743450', 'e570bef3-1939-4012-9c2b-1de99256e34b', 'f034550f-7b8c-47f4-b6f3-63b7fb1026d3', '4a871eb9-835d-4207-a356-a035198b2035');
			var CarCommonCSID = '<%= _serialId.ToString(CultureInfo.InvariantCulture) %>';
		</script>

       <%--delete by zf 20160104
            <!--弹出层 start-->
        <div class="leftmask leftmaskduibibtn" style="display: none;"></div>
        <div class="leftPopup model" data-back="leftmaskduibibtn" data-zindex="777777" style="display: none;">
            <div class="swipeLeft">
                <div class="loading">
                    <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" />
                    <p>正在加载...</p>
                </div>
            </div>
        </div>
        <!--弹出层 end-->--%>
        <!--一级公共模板 start-->
        <script type="text/template" id="duibibtnTemplate">
            <div class="y2015-car-01">
                <div class="slider-box">
                    <ul class="first-list">
                        {for(var i =0;i< list.SelectedCars.length;i++){}
                       <li>
                           <div class="line-box"><a class='select' href="/m{= list.SelectedCars[i].CarId }/">{=list.SelectedCars[i].CarName }</a><a href="javascript:;" data-carid="{=list.SelectedCars[i].CarId }" class="btn-close"><i></i></a></div>
                       </li>
                        {}}
                    {for(var i =0;i< list.AddLables.Count;i++){}
                        <li class="add">
                            <div class="line-box"><a href="javascript:;">添加对比车款</a></div>
                        </li>
                        {}}
                    <li class="alert">最多对比4个车款</li>
                    </ul>
                </div>
            </div>
            <div class="swipeLeft-header">
                <a href="###" class="btn-clear">清除</a>
                <!--btn-clear disable-->
                <a href="###" class="btn-comparison">开始对比</a>
                <!--btn-comparison disable-->
                <div class="clear"></div>
            </div>
        </script>
        <!--一级公共模板 end-->

        <div class="leftmask duibi-leftmask leftmask2 " style="display: none;"></div>
        <div class="leftPopup duibicar duibi-leftPopup" data-zindex="777777" data-back="leftmask2" style="display: none;">
            <div class="swipeLeft">
                <div class="y2015-car-01">
                    <ul class="first-list">
                       
                    </ul>
                </div>
                <div class="swipeLeft-header">
                    <a href="###" class="btn-clear">清空</a>
                    <!--btn-clear disable-->
                    <a href="###" class="btn-comparison">开始对比</a>
                    <!--btn-comparison disable-->
                    <div class="clear"></div>
                </div>
            </div>
        </div>
        <div class="float-r-box">
			<!--文章调查问卷start-->
			<a target="_blank" href="http://survey01.sojump.com/jq/8854583.aspx" class="gg-float gg-float-show" id="ggFloat"><img src="http://image.bitautoimg.com/uimg/mbt2015/images/ico_float_wenjuan.png" /></a>
			<!--文章调查问卷end--> 
			<!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml "-->
			<a id="compare-pk" data-channelid="27.23.747"  href="#compare"  data-action="duibicar" class="float-r-ball float-pk">
				<span><p>对比</p></span>
				<i></i>
			</a>
		</div>

     <div class="footer15">
	    <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <div class="breadcrumb">
            <div class="breadcrumb-box">
		    <a href="http://m.yiche.com/">首页</a> &gt; <a href="http://car.m.yiche.com/brandlist/">选车</a> &gt; <a href="http://car.m.yiche.com/brandlist/<%= _serialEntity.Brand.MasterBrand.AllSpell %>/"><%= _serialEntity.Brand.MasterBrand.Name %></a> &gt; <span><%= _serialShowName %></span>
                </div>
	    </div>
	    <!--#include file="~/html/footerV3.shtml"-->
    </div>
	</div>
     <!--弹出层 start-->
    <div class="leftmask leftmaskcondition" style="display: none;"></div>
    <div class="leftPopup stopyear" data-back="leftmaskcondition" style="display: none;">
        <div class="swipeLeft">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
                <p>正在加载...</p>
            </div>
        </div>
    </div>
     <div class="leftPopup  level" data-back="leftmaskcondition" style="display: none;">
        <div class="swipeLeft">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
                <p>正在加载...</p>
            </div>
        </div>
    </div>
     <div class="leftPopup bodyform" data-back="leftmaskcondition" style="display: none;">
        <div class="swipeLeft">
            <div class="loading">
                <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
                <p>正在加载...</p>
            </div>
        </div>
    </div>
    <!--弹出层 end-->
    <!--loading模板 start -->
    <div class="template-loading" style="display: none;">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
    <!--loading模板 end -->
    <!--音频模板 start -->
    <script type="text/lazyT-template" id="template_audios">
                {#~ D:item:index #}
                <li class="swiper-slide">
                    <div class="audio-box">
                        <div id="audio_{#=item.AudioId#}" class="jp-audio  audio_{#=item.AudioId#}" role="application" aria-label="media player">
                            <div class="audio-list">
                                <!-- 播放器 start -->
                                <div class="play-box">
                                    <div class="jp-type-single">
                                        <div class="jp-controls">
                                            <img src="{#=item.AuthorImage#}" alt="" />
                                            <span></span>
                                            <div class="jp-zz"></div>
                                            <button class="jp-play" data-channelid="27.23.1028" style="cursor:pointer" role="button" data-id="audio_{#=item.AudioId#}" data-urls="{#=item.PlayLinks.join(',')#}" tabindex="0"></button>
                                        </div>
                                        <div class="jp-cont-warp">
                                            <div class="jp-progress">
                                                <div class="jp-seek-bar">
                                                    <div class="jp-play-bar"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="jp-time-holder">
                                            <div class="jp-current-time" role="timer" aria-label="time">{#=item.DurationString#}</div>
                                        </div>
                                    </div>
                                </div>
                                <!-- 播放器 end -->
                                <!-- 文字介绍 start -->
                                <div class="aud-txt-box">
                                    <div class="prog-txt">
                                        <i class="t"></i>
                                        <i class="b"></i>
                                        <p>{#=item.Content#}</p>
                                    </div>
                                    <div class="source-box">
                                        <span class="name-box">{#=item.AuthorName#} - {#=item.Introduce#}</span>
                                        <span class="time-box jp-current-time"></span>
                                    </div>
                                </div>
                                <!-- 文字介绍 end -->
                            </div>
                        </div>
                        <div class="aud-tip"></div>
                    </div>
                </li>
                {#~#}
            </script>
    <!--音频模板 end -->
    <!--公共模板 start-->
    <script type="text/template" id="commonTemplate">
        <div class="ap">
            <ul class="y2015">
            <li><a href="#" data-value="不限">不限</a></li>
            { for(var j = 0 ; j < SubItem.length ; j++){ }
            <li {=(SubItem[j].toString()==selSubStopYear||SubItem[j].toString()==selSubLevel||SubItem[j].toString()==selSubBodyForm)?"class='current'":""}><a href="#" data-value="{=SubItem[j].toString().replace('款','')}">{=SubItem[j]}</a></li>
            {}}
            </ul>
        </div>
    </script>
    <!--公共模板 end-->
    
        <!--省模板start-->
    <script type="text/template" id="provinceTemplate">
        <dl class="tt-list">
            { for(var n in provinces){}
            <dt><span>{=n}</span></dt>
                {for(var i=0;i<provinces[n].length;i++){}
                <dd>
                    <a href="#" {=provinces[n][i].children ? 'data-action="city"':''}  data-id="{=provinces[n][i].id}" class="{=provinces[n][i].children ? '':'nbg'} {= provinces[n][i].id.toString() == current_province.toString()  ? 'current':''}">
                        <p>{=provinces[n][i].name}</p>
                    </a>
                </dd>
               {}}
            {}}
        </dl>
    </script>
    <!--省模板end-->

    <!--市模板start-->
    <script type="text/template" id="cityTemplate">
        <div class="ap">
            <ul class="first-list rp">
                <li class="root"><a>安徽</a></li>
                {for(var i=0;i  <citys.length;i++){}
                    <li><a data-id="{=citys[i].id}" class="{= citys[i].id.toString() == current_city.toString()  ? 'current':''}">{=citys[i].name}</a></li>
                {}}
            </ul>
        </div>
    </script>
    <!--市模板end-->

    <!--loading模板 start -->
    <div class="template-loading" style="display: none;">
        <div class="loading">
            <img src="http://image.bitautoimg.com/uimg/mbt2015/images/loading.gif" alt="loading..."/>
            <p>正在加载...</p>
        </div>
    </div>
    <!--loading模板 end -->
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/model.js?v=2016011315"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/addcompare.js?v=201510271146"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompare.js?v=20151117"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/iscroll.js?v=20150828"></script>--%>
    <script type="text/javascript">
    	$("a.btn-menu span").on("click",function(event){
    		event.preventDefault();
    		var menupop = $(this).parent().siblings("div.menu-pop");
    		if($(menupop).css("display") == "none"){
    			$(menupop).css("display","block");
    			$(this).parent("a.btn-menu").addClass("btn-menu-show");
    		}
    		else{
    			$(menupop).css("display","none");
    			$(this).parent("a.btn-menu").removeClass("btn-menu-show");
    		}
    	});

    	$(document).on('touchstart', function (event) {
    		event.stopPropagation();
    		var clickEle = $(event.target).closest(".menu-pop").attr("id");
    		if (clickEle != 'h_popNav'&& $(event.target).closest(".btn-menu").attr("id") != "h_nav"){
    			$("#h_popNav").hide();
    			$("#h_nav").removeClass("btn-menu-show")
    		}
    		//if(clickEle != 'f_popNav' && $(event.target).closest(".btn-menu").attr("id") != "f_nav"){
    		//	$("#f_popNav").hide();
    		//	$("#f_nav").removeClass("btn-menu-show")
    		//}
    	});
    	$(document).ready(function () {
    		var documentHeight = $(document).height();
    		$('#pSearch').css('height', documentHeight);
    	});
    </script>
	<% if (_yearCount > 0)
	{ %>
	<script type="text/javascript">
		// 临时统计方法 
		function statForTempString(objid, str1, str2) {
			var _sentImg = new Image(1,
				1);
			_sentImg.src = "http://carstat.bitauto.com/weblogger/img/c.gif?logtype=temptypestring&objid="
				+ objid + "&str1=" + encodeURIComponent(str1) + "&str2=" + encodeURIComponent(str2)
				+ "&" + Math.random();
		}

		var year = "all";
		var divNum = 0;
		//(function() {
		//    var tabs = document.getElementById("yeartag").getElementsByTagName("li");
		//    for (var i = 0; i < tabs.length; i++) {
		//        tabs[i].onclick = function() {
		//            if (this.className == 'current') return false;
		//            var x = 0;
		//            for (var j = 0; j < tabs.length; j++) {
		//                if (tabs[j] == this) {
		//                    x = j;
		//                } else {
		//                    tabs[j].className = "";
		//                    document.getElementById(("yearDiv" + j)).style.display = "none";
		//                }
		//            }
		//            this.className = "current";
		//            document.getElementById(("yearDiv" + x)).style.display = "";
		//            divNum = x;
		//        };
		//    }
		//}
		//)();
		//还原筛选项的值 
		function resetOptions(){
			$("[data-action=level]").removeClass("current").find("span").text("排量");
			$("[data-action=bodyform]").removeClass("current").find("span").text("变速箱");
			selSubStopYear='', selSubLevel='', selSubBodyForm='';
		}
	</script>
	<% } %>

	<%--<!--导航开始-->
	<!--#include file="~/include/pd/2012/wap/00001/201503_wap_zsy_cd_js_Manual.shtml" -->--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js?v=20130606"> </script>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/ajax.js"> </script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/city348IDMapName.js?v=20140903"> </script>
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/csSummary.min.js?v=20160524"> </script>
    <%--<script type="text/javascript" src="/Js/csSummary.js?v=20160329"> </script>--%>
	<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"> </script>
	<!--# include file="~/include/pd/2012/wap/00001/201209_wap_JS_Manual.shtml" -->
	<script src="http://image.bitautoimg.com/uimg/wap/js/idangerous.swiper-2.0.min.js"></script>
	<script type="text/javascript">
		<%--// 默认元素ID "m-car-nav"
		CarNavForWireless.DivID = "m-car-nav";
		// 导航当前标签索引(0:综述,1:配置,2:图片,3:油耗,4:详解,5:口碑,6:视频,7:论坛)
		CarNavForWireless.CurrentTagIndex = 0;
		//add by sk 2016.03.29 temp
		try
		{ mainWirelessPVStatisticFunction(<%=_serialId%>, 0, this.CurrentTagIndex); }
		catch (err) { }--%>

		//行情
		//loadNewsHangqingByCity(<%= _serialId %>, '<%= _serialShowName %>', citycode, 4);
		mSerialSummary.loadNewsHangqingV2(<%= _serialId %>, '<%= _serialShowName %>', citycode, 4);
		//特卖
		getDemandAndJiangJia(<%= _serialId %>, '<%= _serialAllSpell %>', bit_locationInfo.cityId);
		// 浏览过的车型
		Bitauto.Login.onComplatedHandlers.push(function(loginResult) {
			Bitauto.UserCars.setUserLoginState({
				isLogin: loginResult.isLogined
			});
			//添加浏览过的车
			Bitauto.UserCars.addViewedCars(<%= _serialId %>);
	    });
		var viewedList = "";
		for (var i = 0; i < Bitauto.UserCars.viewedcar.arrviewedcar.length && i < 4; i++) {
			viewedList += Bitauto.UserCars.viewedcar.arrviewedcar[i];
			if (i != Bitauto.UserCars.viewedcar.arrviewedcar.length - 1) {
				viewedList += ",";
			}
		}
		loadViewedCars(viewedList);
		//加统计代码
		addTrackingCode(<%= _serialId %>);
	</script>
	<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"> </script>
	<%--<script type="text/javascript" src="http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csid=<%= _serialId %>&op=GetCsForWireless&callback=CarNavForWireless.GenerateNav"
		charset="utf-8"> </script>--%>
	<!-- footer begin -->
    <script type="text/javascript" src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js"></script>
	<script type="text/javascript" src="http://log.bitauto.com/easypassweblog/TelCallTracing/script/telcalltracing.js"> </script>
 	<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"> </script>
 	<script type="text/javascript">
 		var curSerialId=<%= _serialId %>;
 		var apiBrandId='<%=_serialEntity.BrandId%>';
 		var apiSerialId=curSerialId;
 		var apiCarId='';

 		var pcount = <%= pageCount %>;
 	    var nearestYear='<%=nearestYear %>';
 		var tabs = document.getElementById("yeartag").getElementsByTagName("li");
 		var btnMore = {};
	</script>
	<!--include file="~/html/uploadaladingapp.shtml"-->
	<!--#include file="~/include/pd/2014/wap/00001/201406_wap_top_banner_js_Manual.shtml"-->
	<script type="text/javascript">
		// 看了还看
		if ($("#m-tabs-kankan li").length >= 2) {	 
			var tabsSwiperKankan = new Swiper('.swiper-container-kankan', {
				calculateHeight: true,
				loop: true,
				onInit: function(swiper){
					$("#m-tabs-kankan li").eq( (swiper.activeIndex - 1) % (swiper.slides.length - 2) ).addClass('current');
				},
				onSlideChangeEnd: function(swiper){
					$("#m-tabs-kankan .current").removeClass('current');
					$("#m-tabs-kankan li").eq( (swiper.activeIndex - 1) % (swiper.slides.length - 2) ).addClass('current');
				}
			})

			$("#m-tabs-kankan li").on('touchstart mousedown', function (e) {
	        
				e.preventDefault()
				$("#m-tabs-kankan .current").removeClass('current')
				$(this).addClass('current')
				tabsSwiperKankan.swipeTo($(this).index())
			})
	      
	   
			$("#m-tabs-kankan li").click(function (e) {
				e.preventDefault()
			})		}
		// 文章切换
		var tabsSwiperArticle = new Swiper('.swiper-container-article', {
			speed: 500,
			onSlideChangeStart: function() {
				$("#m-tabs-article .current").removeClass('current');
				$("#m-tabs-article li").eq(tabsSwiperArticle.activeIndex).addClass('current');

				/* 2015-07-01 控制行情城市显示 start*/
				if (tabsSwiperArticle.activeIndex == 0) {
					$("#changecity").hide();
				} else {
					$("#changecity").show();
				}
				/*end*/
			}
		});
		$("#m-tabs-article li").on('touchstart mousedown', function(e) {
			e.preventDefault();
			$("#m-tabs-article .current").removeClass('current');
			$(this).addClass('current');
			tabsSwiperArticle.swipeTo($(this).index());
		});
		$("#m-tabs-article li").click(function(e) {
			e.preventDefault();
		});

		// 应用下载
		var mySwiperApp = new Swiper('#m-app-part-scroll', {
			pagination: '.pagination-app',
			loop: true,
			grabCursor: true,
			paginationClickable: true
		});

		$(document).ready(function() {
			if ($("#m-app-part-scroll").find("li").length < 4) {
				$(".pagination-app").hide();
			}
		});

	</script>
	<%--<script type="text/javascript" src="http://image.bitautoimg.com/stat/PageAreaStatistics.js"> </script>--%>
	<script type="text/javascript">
		var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr='<%=_serialId%>-'+(zamCityId.length>=4?zamCityId.substring(0,2):zamCityId.substring(0,1))+'-'+zamCityId+'';
	    var zamplus_tag_params = {
	    	modelId: modelStr,
	    	carId: 0
	    };
	</script>
	<script type="text/javascript">
		var _zampq = [];
		_zampq.push(["_setAccount", "12"]);
		(function() {
			var zp = document.createElement("script");
			zp.type = "text/javascript";
			zp.async = true;
			zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
			var s = document.getElementsByTagName("script")[0];
			s.parentNode.insertBefore(zp, s);
		})();
	</script>
	<%--<script type="text/javascript">
		if ($('#focusNews2608').length > 0 && $('#carList2608').length > 0) {
			PageAreaStatistics.init("36,37,38,39,40,41,42,43,44,128,152,182,183,184,185,186");
		} else {
			PageAreaStatistics.init("36,37,38,39,40,41,42,43,44,128,152,182,183,184,185,186,189,190,191,192,193,194,195,196,197,198,199,200,201,202,203,342,350,351");
		}
	</script>--%>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/iscroll.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/underscore.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/model.js?v=20160128"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/rightswipe.js?v=20160128"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/note.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/v2/brand.js?v=20160118"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/waitcompareV2.js?v=2016011818"></script>
    <script type="text/javascript" src="/js/rightswipe-cityV2.js?v=20160202"></script>
	<script type="text/javascript" src="/js/csSummaryCondition.js"></script>--%>
	<script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/wirelessjs/v2/iscroll.js,carchannel/wirelessjs/v2/underscore.js,carchannel/wirelessjs/v2/model.js,carchannel/wirelessjs/v2/rightswipe.js,carchannel/wirelessjs/v2/note.js,carchannel/wirelessjs/v2/brand.js,carchannel/wirelessjs/waitcompareV2.js,carchannel/wirelessjs/rightswipe-cityv2.js,carchannel/wirelessjs/cssummarycondition.js?v=2016040818"></script> 
      <script src="http://image.bitautoimg.com/uimg/wap/js/idangerous.swiper-2.0.min.js"></script><script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/jquery.jplayer.min.js?v=2016050614"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/audio/jplayer/audioJPlayer.js?v=2016050614"></script>

    <script type="text/javascript">
    	var compareConfig={    		serialid:CarCommonCSID    	};    	$(function(){    		WaitCompare.initCompreData(compareConfig);    		//RigthSwipeCity.initCity();    	}); 	</script>
    <script  type="text/javascript">
    	var tuanFlag=false;
    	$(function(){
    		//年款切换
    		var csCondition =new CsSummaryCondition();
    		csCondition.Init(["[data-action=stopyear]","[data-action=level]","[data-action=bodyform]"]);

    		var tuangouParam = 'mediaid=2&locationid=1&cmdId='+curSerialId+'&cityId='+citycode ;
    		//易团购
    		(function getYiTuanGou(params)
    		{
    			$.ajax({
    				url: "http://api.market.bitauto.com/MessageInterface/YiTuanGou/GetYiTuanGouUrl.ashx?" + params,//?mediaid=2&locationid=1&cmdId=3999&cityId=201
    				async: false,
    				dataType: "jsonp",
    				//jsonpCallback: "successHandler",
    				//cache: true,
    				success: function(data) {
    					var h=[];
    					if(data&&data.result=="yes"){
    						tuanFlag=true;  
    						var tuangouUrl=data.url;
    						var slogan=data.slogan;
    						h.push("<span class=\"yh-tap\"></span>");
    						h.push("<div class=\"cont-box\">");
    						h.push("<h4>本地限时特惠</h4>");
    						h.push("<p>"+slogan+"</p>");
    						h.push("</div>");
    						$(".prefer-tim").html(h.join(''));
    						$(".prefer-tim").on("click",function(){
    							window.location.href=tuangouUrl;
    						});
    						csCondition.getTuanGouTag();
    					}
    				}
    			});
    		})(tuangouParam)

    		//音频
    		var audioCreator;
    		$.getJSON("http://api.admin.bitauto.com/audiobase/audio/GetAudioAudioResource?IsCarModel=1&pagesize=5&serialId=" +CarCommonCSID , function (data) {  //CarCommonCSID  3701  3777
    			if(data.length>0)
    			{
    				$(".voice-swiper-outer").show();   
    				var audioTemplate = document.getElementById('template_audios').innerHTML;
    				audioCreator = lazyT.tmpl(audioTemplate);
    				var audioHtml = audioCreator(data);
    				$("#audio_list").html(audioHtml);
    				//控制音频简介字数限制
    				var audioLiContent=$("#audio_list").find("li");
    				$.each(audioLiContent,function(index,item){
    					var audioContent=$(this).find(".aud-txt-box p").text(); 
    					var audioTxtCnt=0;
    					if(audioContent.length>0){
    						for(var i=0;i<audioContent.length;i++)
    						{
    							var curStr=audioContent[i];
    							if(isChn(curStr))
    							{
    								audioTxtCnt+=2;
    							}
    							else
    							{
    								audioTxtCnt+=1;
    							}
    							if(audioTxtCnt>=90)
    							{
    								audioContent=audioContent.substr(0,i+1);
    								$(this).find(".aud-txt-box p").text(audioContent+"...");
    								break;
    							}
    						}
    					}
    				});
    				//音频部分的翻页切换
    				var audioCount=$("#audio_list").find("li").length;
    				if(audioCount>1)
    				{
    					var mySwiperVoice = new Swiper('#voiceSwiper', {
    						pagination: '.pagination-voice',
    						loop: true,
    						calculateHeight: true,
    						grabCursor: true,
    						paginationClickable: true,
    						onSlideChangeStart:function(){
    							jp.stop();
    						}
    					});
    				}
    				else
    				{
    					var mySwiperVoice = new Swiper('#voiceSwiper', {
    						pagination: '.pagination-voice',
    						loop: false,
    						calculateHeight: false,
    						grabCursor: false,
    						paginationClickable: false
    					});
    					$(".pagination-voice").hide();
    				}
    				jp.init({swfPath: "/js/audio"});    			}
    		});
    	});
    	// 检查是否为中文
    	function isChn(str){
    		var reg = /^[\u4E00-\u9FA5]+$/;
    		if(!reg.test(str)){
    			return false;
    		}
    		return true;
    	}
    </script>
    <script>
    	var mNewsTagBar = document.getElementById("op-nav");
    	var topHeight = 0;
    	var tempBar = mNewsTagBar;
    	var ua = navigator.userAgent;
    	if (/(ip(ad|hone|od))/i.test(ua)) {

    	} else {
    		while(tempBar){
    			topHeight = topHeight + tempBar.offsetTop;
    			tempBar = tempBar.offsetParent;
    		}
    		$(window).bind("scroll", function(){
    			var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
    			if(scrollHeight >= topHeight){
    				tempIsFloatTopNavigation = true;
    				mNewsTagBar.className = "op-nav-static op-nav-fixed";
    			} else {
    				mNewsTagBar.className = "op-nav-static";
    				tempIsFloatTopNavigation = false;
    			}
    		});
    	}
	</script>
	<!--评论数JS-->
	<!--#include file="/include/pd/2014/common/00001/201506_typls_js_Manual.shtml"-->
</body>
</html>
   