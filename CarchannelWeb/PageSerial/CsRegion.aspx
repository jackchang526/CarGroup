<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsRegion.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsRegion" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=cityName %><%=serialSEOName%>报价|<%=cityName %><%=serialSEOName%>4S店价格_优惠信息_<%=serialSEOName%>4S店报价】-易车网</title>
	<meta name="Keywords" content="<%=cityName %><%=serialSEOName%>4S报价, <%=cityName %><%=serialSEOName%>4S店, <%=cityName %><%=serialSEOName%>优惠信息,<%=serialSEOName%>4S店报价" />
	<meta name="Description" content="<%=cityName %><%=serialSEOName%>报价:易车网为您提供<%=cityName %><%=serialSEOName%>价格信息,包含<%=cityName %><%=serialSEOName%>4S店报价、<%=cityName %><%=serialSEOName%>4S店价格列表、<%=serialSEOName%>4S店促销信息、及<%=cityName %><%=serialSEOName%>4s店电话及地址,是您了解<%=cityName %><%=serialSEOName%>报价的首选网站!" />
	<!--#include file="~/ushtml/0000/car_localbuy_style_v2-265.shtml"-->
	<link href="http://beijing.bitauto.com/car/citylogo.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
</head>
<body>
	<!--#include file="~/html/header2012.shtml"-->
	<!--ad start-->
	<div class="bt_ad"><ins id="div_d8a72a70-98ff-4cec-a4f7-c2bb07d10829" type="ad_play" adplay_ip="" adplay_areaname="<%=cityId %>" adplay_cityname="" adplay_brandid="<%=serialId %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d8a72a70-98ff-4cec-a4f7-c2bb07d10829"></ins></div>
	<!--ad end-->
	<!--smenu start-->
	<div class="bt_page">
		<div class="bt_smenuNew">
			<div class="bt_navigatev1New">
				<div><span>您当前的位置：</span> <a href="http://<%=citySpell %>.bitauto.com/" target="_blank">
					<%=cityName %>易车网</a> &gt; <a href="http://car.bitauto.com/" target="_blank">车型</a> &gt; <a href="http://car.bitauto.com/<%=serialEntity.Brand.MasterBrand.AllSpell %>/" target="_blank">
						<%=serialEntity.Brand.MasterBrand.Name%></a>
					<% if (serialEntity.Brand.MasterBrand.Name != serialEntity.Brand.Name)
		{ %>
					&gt; <a href="http://car.bitauto.com/<%=serialEntity.Brand.AllSpell %>/" target="_blank">
						<%=serialEntity.Brand.Name%></a>
					<% } %>
					&gt; <a target="_self" href="http://car.bitauto.com/<%=serialSpell %>/">
						<%=serialShowName%></a> &gt; <strong>
							<%=cityName %>购车</strong></div>
			</div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchNew.shtml"-->
			</div>
			<%= SerialMianBaoAD %>
		</div>
	</div>
	<!--smenu end-->
	<%= CsHead %>
	<div class="bt_page">
		<div class="s_result_layer"><small>以下是<strong><%=cityName %></strong>购车信息，查看其他城市请</small>
			<div class="bt_change_btn"><span id="change_btn" class="change_btn_close">[<em>更换城市</em>]</span>
				<div style="display: none;" id="area_list" class="hotcity_list"><span id="area_list_close" class="close">关闭</span>
					<h4>
						热门城市</h4>
					<dl><dt>华北地区</dt> <dd><a href="http://beijing.bitauto.com/car/<%=serialSpell%>/">北京</a> <a href="http://tianjin.bitauto.com/car/<%=serialSpell%>/">天津</a> <a href="http://taiyuan.bitauto.com/car/<%=serialSpell%>/">太原</a> <a href="http://shijiazhuang.bitauto.com/car/<%=serialSpell%>/">石家庄</a> <a href="http://baoding.bitauto.com/car/<%=serialSpell%>/">保定</a> <a href="http://tangshan.bitauto.com/car/<%=serialSpell%>/">唐山</a> <a href="http://huhehaote.bitauto.com/car/<%=serialSpell%>/">呼和浩特</a> <a href="http://jinan.bitauto.com/car/<%=serialSpell%>/">济南</a> <a href="http://qingdao.bitauto.com/car/<%=serialSpell%>/">青岛</a> <a href="http://zibo.bitauto.com/car/<%=serialSpell%>/">淄博</a> <a href="http://yantai.bitauto.com/car/<%=serialSpell%>/">烟台</a> </dd></dl><dl><dt>东北地区</dt> <dd><a href="http://haerbin.bitauto.com/car/<%=serialSpell%>/">哈尔滨</a> <a href="http://daqing.bitauto.com/car/<%=serialSpell%>/">大庆</a> <a href="http://changchun.bitauto.com/car/<%=serialSpell%>/">长春</a> <a href="http://shenyang.bitauto.com/car/<%=serialSpell%>/">沈阳</a> <a href="http://dalian.bitauto.com/car/<%=serialSpell%>/">大连</a></dd> </dl><dl><dt>华东地区</dt> <dd><a href="http://shanghai.bitauto.com/car/<%=serialSpell%>/">上海</a> <a href="http://hangzhou.bitauto.com/car/<%=serialSpell%>/">杭州</a> <a href="http://wenzhou.bitauto.com/car/<%=serialSpell%>/">温州</a> <a href="http://nanjing.bitauto.com/car/<%=serialSpell%>/">南京</a> <a href="http://ningbo.bitauto.com/car/<%=serialSpell%>/">宁波</a> <a href="http://jinhua.bitauto.com/car/<%=serialSpell%>/">金华</a> <a href="http://suzhou.bitauto.com/car/<%=serialSpell%>/">苏州</a> <a href="http://xuzhou.bitauto.com/car/<%=serialSpell%>/">徐州</a> <a href="http://wuxi.bitauto.com/car/<%=serialSpell%>/">无锡</a> <a href="http://hefei.bitauto.com/car/<%=serialSpell%>/">合肥</a></dd> </dl><dl><dt>华南地区</dt> <dd><a href="http://guangzhou.bitauto.com/car/<%=serialSpell%>/">广州</a> <a href="http://shenzhen.bitauto.com/car/<%=serialSpell%>/">深圳</a> <a href="http://dongguan.bitauto.com/car/<%=serialSpell%>/">东莞</a> <a href="http://foshan.bitauto.com/car/<%=serialSpell%>/">佛山</a> <a href="http://nanning.bitauto.com/car/<%=serialSpell%>/">南宁</a> <a href="http://haikou.bitauto.com/car/<%=serialSpell%>/">海口</a> <a href="http://fuzhou.bitauto.com/car/<%=serialSpell%>/">福州</a> <a href="http://xiamen.bitauto.com/car/<%=serialSpell%>/">厦门</a> <a href="http://quanzhou.bitauto.com/car/<%=serialSpell%>/">泉州</a></dd> </dl><dl><dt>华中地区</dt> <dd><a href="http://zhengzhou.bitauto.com/car/<%=serialSpell%>/">郑州</a> <a href="http://luoyang.bitauto.com/car/<%=serialSpell%>/">洛阳</a> <a href="http://wuhan.bitauto.com/car/<%=serialSpell%>/">武汉</a> <a href="http://yichang.bitauto.com/car/<%=serialSpell%>/">宜昌</a> <a href="http://changsha.bitauto.com/car/<%=serialSpell%>/">长沙</a> <a href="http://nanchang.bitauto.com/car/<%=serialSpell%>/">南昌</a></dd> </dl><dl><dt>西北地区</dt> <dd><a href="http://xian.bitauto.com/car/<%=serialSpell%>/">西安</a> <a href="http://wulumuqi.bitauto.com/car/<%=serialSpell%>/">乌鲁木齐</a> <a href="http://lanzhou.bitauto.com/car/<%=serialSpell%>/">兰州</a> <a href="http://yinchuan.bitauto.com/car/<%=serialSpell%>/">银川</a></dd> </dl><dl><dt>西南地区</dt> <dd><a href="http://chongqing.bitauto.com/car/<%=serialSpell%>/">重庆</a> <a href="http://chengdu.bitauto.com/car/<%=serialSpell%>/">成都</a> <a href="http://kunming.bitauto.com/car/<%=serialSpell%>/">昆明</a> <a href="http://guiyang.bitauto.com/car/<%=serialSpell%>/">贵阳</a></dd> </dl></div>
			</div>
		</div>
		<!--行情新闻部分-->
		<div class="col-all">
			<div class="col-con">
				<div class="col-con_sub">
					<div class="line_box mainlist_box infor_five">
						<%=cityNewsHtml%>
					</div>
				</div>
				<div class="col-con_main">
					<div class="line_box mainlist_box infor_five"><h3><span><a href="http://<%=citySpell %>.bitauto.com/car/<%=serialSpell%>/cuxiao/" target="_blank">
						<%=cityName %><%=serialShowName %>促销·活动</a></span></h3><ins id="ep_union_1" partner="1" version="" isupdate="1" type="5" city_type="1" city_id="<%=cityId %>" city_name="0" car_type="2" brandid="0" serialid="<%=serialId %>" carid="0"></ins>
						<div class="more"><a href="http://<%=citySpell %>.bitauto.com/car/<%=serialSpell %>/cuxiao/" target="_blank">更多>></a></div>
					</div>
				</div>
			</div>
			<div class="col-side">
				<div class="line_box car0507_02">
					<%=compareHtml%>
				</div>
			</div>
			<div class="col-all">
				<div class="line_box mainlist_box">
					<%=priceHtml%>
				</div>
			</div>
			<div class="col-all">
				<!-- AD add May.30.2013 -->
				<ins id="div_aa9a54b3-b482-4115-809a-c5cd25bf4eb4" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID="<%=serialId %>"  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="aa9a54b3-b482-4115-809a-c5cd25bf4eb4"> </ins>
				<!--modified by sk 2013.04.10-->
				<ins id="div_0b4d5880-5022-490b-a413-d9ac3a539bd2" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID="<%=serialId %>"  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="0b4d5880-5022-490b-a413-d9ac3a539bd2"> </ins>
				<%if (hasSaleData)
	  { %>
				<%--<iframe width="960" height="295" frameborder="0" scrolling="no" src="http://car.bitauto.com/PageSerial/SellDataForSerialNoTop.aspx?csId=<%=serialId %>"></iframe>--%>
				<%} %>
				<div class="line_box car0427_01" id="vendorInfo"><h3><span><a href="http://dealer.bitauto.com/<%= serialSpell %>/" target="_blank">
					<%= serialSEOName%>-经销商推荐</a></span></h3>
					<div class="c"><ins id="ep_union_3" partner="1" version="" isupdate="1" type="1" city_type="1" city_id="<%=cityId %>" city_name="0" car_type="2" brandid="0" serialid="<%=serialId %>" carid="0"></ins></div>
					<div class="clear"></div>
					<div class="more"><a href="http://dealer.bitauto.com/<%= serialSpell %>/" target="_blank">更多>></a></div>
				</div>
				<%--<%=ucarHtml%>--%>
				<script src="http://image.bitautoimg.com/carchannel/jsnew/taoche.js?v=2015012614" type="text/javascript"></script>
				<div class="line_box" id="showbuyucar"></div>
				<script>
                    TaoChe.showBuyUCar(<%=serialId %>, bit_locationInfo.cityId, '<%= serialSpell %>', '<%=serialShowName %>');
				</script>
			</div>
			<!--SEO名-->
			<div class="col-all">
				<!--#include file="~/html/SeoHotSerialList.shtml"-->
				<div class="line_box link">
					<div class="h3_tab">
						<ul>
							<li class="current"><span>其它城市<%=serialSEOName%>报价</span></li>
						</ul>
					</div>
					<h3></h3>
					<div class="clear"></div>
					<div class="co">
						<div>
							<ul>
								<%=GetOtherCityPriceList() %>
							</ul>
						</div>
					</div>
					<div class="clear"></div>
				</div>
			</div>
		<script type="text/javascript" language="javascript">
			var CarCommonCSID = '<%= serialId.ToString() %>';
			var XCWebLogCollector = { area: '201' };
			if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
				XCWebLogCollector.area = bit_locationInfo.cityId;
			}
			if (typeof (CarCommonCSID) != "undefined") {
				XCWebLogCollector.serial = CarCommonCSID;
			}
		</script>
        <!-- 调用尾 -->
			<!-- 调用尾 -->
			<!--#include file="~/include/special/2010/00001/2012_lanmucommon_footer_Manual.shtml"-->
			<script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>
			<script src="http://www.bitauto.com/themes/2009/js/headcommon.js" type="text/javascript"></script>
			<script type="text/javascript" src="http://www.bitauto.com/themes/2009/js/search.js"></script>
		</div>
		<div style="display: none">
			<%= errorStr %></div>
		<!--WebTrends Analytics-->
		<!-- START OF SmartSource Data Collector TAG -->
		<script src="http://css.bitauto.com/bt/webtrends/dcs_tag_city13.js" type="text/javascript"></script>
		<!-- END OF SmartSource Data Collector TAG -->
		<!--news stat-->
		<!-- bitauto stat begin -->
		<script type="text/javascript" src="http://log.bitauto.com/newsstat/stat.js"></script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/csregion.js"></script>
		<script type="text/javascript" language="javascript">			//ShowVendorInfo(<%=serialId %>,<%=cityId %>,'<%=cityName %>'); </script>
		<!-- bitauto stat end -->
		<%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
	{ %>
		<!--百度热力图-->
		<script type="text/javascript">			var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://"); document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E")); </script>
		<%} %>
		<!--footer end-->
		<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
<script type="text/javascript" language="javascript">
	function openwindow(url, name, iWidth, iHeight) {
		var url; //转向网页的地址;
		var name; //网页名称，可为空;
		var iWidth; //弹出窗口的宽度;
		var iHeight; //弹出窗口的高度;
		var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
		var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
		window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=no,resizeable=no,location=no,status=no');
	}
	function AreasTrans(BtnChange, BtnClose, AreaList) {
		var div = document.getElementById(AreaList);
		var btn = document.getElementById(BtnChange);
		if ((div) && (btn)) {
			btn.onclick = function () {
				if (div.style.display == "none") {
					div.style.display = "block";
					btn.className = "change_btn_open";
				}
				else {
					div.style.display = "none";
					btn.className = "change_btn_close";
				}
			}
			var close_btn = document.getElementById(BtnClose);
			close_btn.onclick = function () {
				document.getElementById(AreaList).style.display = "none";
				btn.className = "change_btn_close";
			}
		}
	}

	function AreasTransfunc() {
		AreasTrans("change_btn", "area_list_close", "area_list");
		AreasTrans("change_btn_bj", "area_list_close_bj", "area_list_bj");
		AreasTrans("change_btn_heb", "area_list_close_heb", "area_list_heb");
	}
	try {
		addLoadEvent(AreasTransfunc);
	}
	catch (err)
{ }
</script>
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
