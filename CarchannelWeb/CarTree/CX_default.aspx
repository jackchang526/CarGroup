<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_default.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTree.CX_default" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head id="Head1">
	<title>【汽车大全|汽车标志_汽车标志大全_热门车型大全】-易车网</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="Keywords" content="汽车大全,汽车标志,车型大全,汽车标志列表" />
	<meta name="Description" content="汽车大全:易车网车型大全频道为您提供各种汽车品牌型号信息,包括汽车报价,汽车标志,汽车图片,汽车经销商,汽车油耗,汽车资讯,汽车点评,汽车问答,汽车论坛等等……" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/" />
	<meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/car.aspx" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car-685.shtml" -->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
	<script type="text/javascript">
		var _SearchUrl = '<%=this._SearchUrl %>';
	</script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--顶通-->
	<!--#include file="~/html/tree_header2014.shtml"-->
	<div class="tree_wrap_box">
		<ins id="div_22bf5713-566c-42e7-bd96-37d91bc6e48a" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="22bf5713-566c-42e7-bd96-37d91bc6e48a"></ins>
	</div>
	<%--<%=NavbarHtml%>--%>
	<!--#include file="~/include/pd/2014/common/00001/201402_shuxing_nav_chexing_Manual.shtml" -->
	<div class="tree_wrap_box">
		<!--左侧树形-->
		<div id="leftTreeBox" class="treeBoxv1">
		</div>
		<!--右侧内容-->
		<div class="treeMainv1">
			<!--广告-->
			<ins style="display: none; margin: 0 0; float: left; width: 100%:" id="div_93824a04-0084-4532-bcd2-6ee1c21a21ba"
				type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid=""
				adplay_brandname="" adplay_brandtype="" adplay_blockcode="93824a04-0084-4532-bcd2-6ee1c21a21ba"></ins>
			<!--#include file="~/include/pd/2014/common/00001/201406_shuxing_common_dh_gb2312_Manual.shtml" -->
			<div class="line-box">
				<div class="title-con">
					<div class="title-box">
						<h3>按条件选车</h3>
						<span id="lastSelectCar"></span>
						<div class="more">
							<%--<a target="_blank" href="http://www.bitauto.com/feedback/FAQ.aspx?col=9&tab=5">温馨提示</a>
							| --%><a target="_blank" href="http://www.bitauto.com/zhuanti/daogou/gsqgl/ ">购车流程</a>
						</div>
					</div>
				</div>
				<!--#include file="~/html/selectCarTool.shtml"-->
			</div>
			<!--热门品牌-->
			<div class="line-box">
				<div class="title-con">
					<div class="title-box">
						<h3>
							<a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a></h3>
					</div>
				</div>
				<div class="carpic_list hot_cars">
					<%=hotMasterBrandHtml %>
					<!--# include file="~/html/HomepageHotBrand.shtml"-->
				</div>
			</div>
			<!-- AD -->
			<ins id="div_3b69e344-7183-4cd4-8fc8-c3887f967cf4" type="ad_play" adplay_ip="" adplay_areaname=""
				adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
				adplay_blockcode="3b69e344-7183-4cd4-8fc8-c3887f967cf4" style="margin: 0px 0px 10px; float: left; width: 100%;"></ins>
			<!-- AD -->
			<!-- 热门车型 -->
			<div class="line-box">
				<div class="title-con">
					<div class="title-box">
						<ul id="data_tab3" class="title-tab title-tab-h3">
							<li class="current"><a href="javascript:;">热门车型</a><em>|</em></li>
							<li><a href="javascript:;">新车</a><em>|</em></li>
							<li id="s-taoche-tab"><a href="javascript:;">二手车</a></li>
							<!--<li class="">易车惠</li>-->
						</ul>
					</div>
				</div>
				<div class="carpic_list">
					<%=GetHotCarTypeNew()%>
					<%=GetNewCarTypeNew()%>
					<ul id="data_box3_2" style="display: none;"></ul>
					<!-- include file="~/include/pd/2014/yichehui/00001/201402_Car_tjcx_ych_Manual.shtml"-->
				</div>
				<div class="clear">
				</div>
			</div>
			<script type="text/javascript">
				var params = {};
				params.tagtype = "chexing";
			</script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/lefttreenew.js?v=2015121715"></script>
			<!--广告-->
			<ins id="div_fc509494-b0cc-461b-b8ef-8ae018603eb7" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="fc509494-b0cc-461b-b8ef-8ae018603eb7"></ins>
			<!--车型关注排行榜  -->
			<div class="line-box">
				<div class="title-con">
					<div class="title-box">
						<h3>车型关注排行榜</h3>
						<ul id="data_tab1" class="title-tab">
							<li class="current"><a href="javascript:;">按价位</a><em>|</em></li>
							<li><a href="javascript:;">按级别</a></li>
						</ul>
					</div>
				</div>
				<%=GetCarAttentionList() %>
				<div class="clear">
				</div>
			</div>
			<!-- 答疑块 -->
			<div class="line-box">
				<!--# include file="~/include/pd/2012/wenda/00001/201405_ask_common_gb2312_Manual.shtml"-->
				<%=GetAskHtml()%>
			</div>
			<!-- add by chengl Jun.27.2012 -->
			<ins id="div_a5ace7c5-f758-45f9-917e-9aa6c4c43735"
				type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid=""
				adplay_brandname="" adplay_brandtype="" adplay_blockcode="a5ace7c5-f758-45f9-917e-9aa6c4c43735"></ins>
			<!-- add by sk 2013.02.28 -->
			<ins id="div_884b0e00-9cd4-486f-bb23-3557e1b2c079" type="ad_play" adplay_ip="" adplay_areaname=""
				adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
				adplay_blockcode="884b0e00-9cd4-486f-bb23-3557e1b2c079"></ins>
			<!-- add by chengl Jun.27.2014 -->
			<ins id="div_ba1e16ec-8931-4e55-b4e3-d75b465e0077" type="ad_play" adplay_ip="" adplay_areaname=""
				adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype=""
				adplay_blockcode="ba1e16ec-8931-4e55-b4e3-d75b465e0077"></ins>
			<!-- 友情链接 -->
			<!--#include file="~/html/htmlFriendLink.shtml"-->
            <!--#include file="~/html/treefooter2014.shtml"-->
                      </div>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
            <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/gouche/pc/jquery.pagination.js?v=20150425"></script>--%>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newselectcartoolv3.min.js?d=201604211649"></script>
			<script type="text/javascript">
				var global_hash = window.location.hash;
				if (global_hash == "#jrtt") {
					$("#div_22bf5713-566c-42e7-bd96-37d91bc6e48a,.bit_top990").remove();
					$("#leftTreeBox").css({ top: "0px" });
					$(".header_style").css({ "overflow": "hidden", "height": "0", "padding": "0" });
					$("ins").remove();
				}
				conditionObj.Type = "car";
				conditionObj.InitPageConditionV2();
				loadJS.push("http://yicheapi.taoche.cn/CarSourceInterface/ForJson/HotSerialsForBitAuto.ashx?callback=showTaoChe", 'utf-8', showTaoChe);
				function showTaoChe(data) {
 					if (!(data && data.HotSerialList && data.HotSerialList.length > 0)) return;
					var arrTaoHtml = [], dataList = data.HotSerialList;
					for (var i = 0; i < 8 && i < dataList.length; i++) {
					    arrTaoHtml.push("<li><a id=\"newCsID_2486\" href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\"><img src=\"" + dataList[i].CarPicUrl + "\" alt=\"" + dataList[i].CsName + "\"></a><div class=\"title\"><a href=\"" + dataList[i].CsUrl + "&ref=chexing&leads_source=p047001\" target=\"_blank\">" + dataList[i].CsName + "</a></div><div class=\"txt\">" + dataList[i].DisplayPrice + "</div></li>");
					}
					document.getElementById("data_box3_2").innerHTML = arrTaoHtml.join('');
				}
				(function(){
					var cookiesName="car_superlink",cObj=CookieHelper.readCookie(cookiesName),toolAlert=document.getElementById("tool-alert");
					 ((cObj!=null&&cObj=="1") ? toolAlert.style.display="none":toolAlert.style.display="block");
					SelectCar.Tools.addEvent(document.getElementById("tool-close"),"click",function(){
						CookieHelper.setCookie(cookiesName,"1",{"expires":3600,"path":"/","domain":"car.bitauto.com"});toolAlert.style.display="none";
					},false);
				})();
			</script>
			<script type="text/javascript" charset="gb2312" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
			<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"></script>
		</div>
 	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<script type="text/javascript">
	    //$("#feedbackDiv").before("<li class=\"w4 d11-backtop\"><a href=\"http://survey01.sojump.com/jq/7431792.aspx\"  title=\"\" target=\"_blank\">问卷调查</a></li>");
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://"); document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));

    </script>
	<%--<!-- Baidu Button BEGIN -->
	<script type="text/javascript" id="bdshare_js" data="type=slide&amp;img=3&amp;pos=left&amp;uid=653519"></script>
	<script type="text/javascript" id="bdshell_js"></script>
	<script type="text/javascript">		var bds_config = { "bdTop": 255 }; var bdscript = document.getElementById("bdshell_js"); var bdscriptloaded = 0; bdscript.onload = bdscript.onreadystatechange = function () { if (bdscriptloaded) { return } var a = bdscript.readyState; if ("undefined" == typeof a || a == "loaded" || a == "complete") { bdscriptloaded = 1; var inter = setInterval(function () { var share = document.getElementById("bdshare"); if (share && share.tagName.toUpperCase() == "DIV") { share.style.width = "24px"; clearInterval(inter); } }, 1000); bdscript.onload = bdscript.onreadystatechange = null; } }; bdscript.src = "http://bdimg.share.baidu.com/static/js/shell_v2.js?cdnversion=" + new Date().getHours(); </script>
	<!-- Baidu Button END -->--%>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<script type="text/javascript">		// 易车惠 2014.3.18 // $("#data_box3_0 li a").each(function () { // if (this.id && this.id.indexOf("hotCsID_") == 0 // && this.id.replace("hotCsID_", "").length == 4) { // csid = this.id.replace("hotCsID_", ""); // $.ajax({ url: 'http://api.car.bitauto.com/Mai/GetSerialGoodsNew.ashx?serialid=' + csid + '&cityid=' + bit_locationInfo.cityId, cache: true, dataType: "jsonp", jsonpCallback: "mai_callback", success: function (data) { // if (!data.CarList || data.CarList.length <= 0) return; // // alert(data.CarList[0].GoodsUrl); // $("#hotCsID_" + csid).before("<div class=\"zy_t\"><h5>惠</h5><div class=\"triangle-bottomleft\"></div><div class=\"triangle-down\"></div></div>"); // } // }); // } // }); </script>
</body>
</html>
<!-- 右下广告 -->
<script type="text/javascript">
	var adplay_CityName = ''; //城市
	var adplay_AreaName = ''; //区域
	var adplay_BrandID = ''; //品牌id 
	var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
	var adplay_BrandName = ''; //品牌
	var adplay_BlockCode = 'fb4948ab-c2d4-493a-ac7a-d87c6591b605'; //广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<script type="text/javascript">
	var adplay_CityName = ''; //城市
	var adplay_AreaName = ''; //区域
	var adplay_BrandID = ''; //品牌id 
	var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
	var adplay_BrandName = ''; //品牌
	var adplay_BlockCode = '6774e482-4280-420a-81fd-cec7edda6637'; //广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<!-- add by chengl Jun.27.2012 -->
<script type="text/javascript">
	var adplay_CityName = ''; //城市
	var adplay_AreaName = ''; //区域
	var adplay_BrandID = ''; //品牌id 
	var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
	var adplay_BrandName = ''; //品牌
	if (global_hash != "#jrtt") {
		var adplay_BlockCode = '56dda5da-43a3-406d-9291-974566759ddd'; //广告块编号
	}
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<!-- add by chengl May.7.2013 -->
<script type="text/javascript">
	var adplay_CityName = ''; //城市
	var adplay_AreaName = ''; //区域
	var adplay_BrandID = ''; //品牌id 
	var adplay_BrandType = ''; //品牌类型：（国产）或（进口）
	var adplay_BrandName = ''; //品牌
	var adplay_BlockCode = '8eba16c6-95bf-477c-8967-a611040a5762'; //广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<!--车型栏目页/底部浮层-->
<script type="text/javascript">
	var adplay_CityName = '';//城市
	var adplay_AreaName = '';//区域
	var adplay_BrandID = '';//品牌id 
	var adplay_BrandType = '';//品牌类型：（国产）或（进口）
	var adplay_BrandName = '';//品牌
	var adplay_BlockCode = 'aa88205a-8a73-4bf9-8b9b-9232299bc59e';//广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>

<%--<!--add by hepw Jan.26.2016 for select car condition's click count-->
<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script> --%>