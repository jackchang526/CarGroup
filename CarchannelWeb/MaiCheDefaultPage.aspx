<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaiCheDefaultPage.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.MaiCheDefaultPage" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>【买车|买车指南-买车注意事项】-易车网BitAuto.com</title>
	<meta name="keywords" content="买车, 买车指南, 买车流程, 买车注意事项" />
	<meta name="description" content="易车网(BitAuto.com)看车频道为您提供最为全面的买车资讯，快捷及时的汽车谍照，新车进口车信息，汽车科技安全资讯，试车推荐信息，为您全面解读购车须知。是您购选爱车的第一网络媒体平台。" />
	<!--#include file="~/ushtml/car/bitauto_car_buycar.shtml"-->
	<script type="text/javascript" charset="gb2312" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
	<style type="text/css">
    /*reset*/
    body, div, dl, dt, dd, ul, ol, li, h1, h2, h3, h4, h5, h6, pre, form, fieldset, input, textarea, p, blockquote, th, td { margin:0; padding:0; }
    body { font-size:12px; font-family:\5b8b\4f53,Arial, Helvetica, sans-serif; background:#fff; line-height:150%; }
    table { border-collapse:collapse; border-spacing:0; }
    fieldset, img { border:0; }
    legend { display:none; }
    address, caption, cite, code, dfn, em, strong, th, var { font-style:normal; font-weight:normal; }
    ol, ul { list-style:none; }
    caption, th { text-align:left; }
    h1, h2, h3, h4, h5, h6 { font-size:100%; font-weight:normal; }
    q:before, q:after { content:''; }
    abbr, acronym { border:0; }
    a:link, a:visited { color:#164a84; text-decoration:none; }
    a:hover { color:#c00; text-decoration:underline; }
    a strong { color:#c00; }
    .bt_page { width:950px; margin:0 auto; text-align:left; background:#fff; }
    .clear { clear:both; height:0; overflow:hidden; }
    /*reset end
    ==============================================================================================================================*/
    .c0621_01{margin:8px 0 0 0}
    .c0621_01 dl{background:none; margin:0 0 0 20px; display:inline; float:left; padding:5px 0; width:702px;}
    .c0621_01 dt{width:58px; color:#656565; float:left; padding:3px 0 0;}
    .c0621_01 dd {float: left;padding: 2px 0 0;width: 620px;}
    .c0621_01 .current a{background:#FFFFFF; color:#000000; font-weight:bold; display:block; padding:0}
    .c0621_01 li{display: inline;float: left;height: 20px;margin-right: 23px;white-space: nowrap;margin-right:18px}
    .car_sub_select{width:628px; height:33px; clear:both; margin:0 0 0 70px; position:relative}
    .car_sub_select .ico_arrow{width:13px; height:8px; background:url(http://image.bitautoimg.com/uimg/car/images/treePicv1.png) no-repeat -947px -92px; position:absolute; z-index:2; top:0; left:58px}
    .car_sub_select ul{width:619px; height:22px; border:1px solid #E6E6E6; padding:2px 0 0 7px; position:absolute; bottom:0; left:0; z-index:1}
    .car_sub_select ul li.current a{color:#000000; font-weight:bold}
	.c0621_01 { height:200px }

	/*20111129*/
	.c0908_03 { height:138px }
	.c0908_03 h4 { padding:15px 0 5px }
	.cardaikuan{ border-top:1px solid #DEE3E7; text-align:center; display:inline; float:left; width:100%; padding:10px 0; height:50px; overflow:hidden;}
	.cardaikuan p{ font-weight:bold;}
	.fun{ padding:0 0 0 50%;}
	.fun a:link,  .fun a:visited {
		background: none repeat scroll 0 0 #164A84;
		border-color: #2567B0 #00264C #00264C #2567B0;
		border-style: solid;
		border-width: 1px;
		color: #fff;
		display: inline-block;
		float: left;
		height: 19px;
		margin: 10px 0 0 -35px;
		overflow: hidden;
		padding: 0 10px;
	}
	.fun a:hover{
			 color: #fff;
	}
    </style>
</head>
<body>
	<ins id="div_a8a7e088-9848-4170-8c07-745891dfab25" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="a8a7e088-9848-4170-8c07-745891dfab25"> </ins>
	<div class="bt_ad" style="margin:0 auto;padding:0">
	<ins id="div_d8424a88-6eed-488f-b213-8041939f2267" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="d8424a88-6eed-488f-b213-8041939f2267"></ins></div>
	<!--买车页头开始-->
	<!--#include file="~/include/special/2010/00001/2010_lanmuCommon_head_UTF8_Manual.shtml"-->
	<!--smenu start-->
	<!-- 车易购个人信息条 -->
	<div id="buycar_e"></div>
	<div class="bt_ad">
		<!--顶部通栏720_80开始-->
		<ins id="div_f579df15-c8c4-496b-98f0-9627b73578d2" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="f579df15-c8c4-496b-98f0-9627b73578d2"></ins>
		<!--顶部通栏720_80结束-->
	</div>
	<!--买车页面包屑开始-->
	<div class="bt_smenu"><div class="bt_navigate"><a href="http://www.bitauto.com/" target="_blank" class="bt_logo">易车网</a> <a href="http://mai.bitauto.com/" target="_blank" class="bt_carbuy"><h1>买车</h1></a></div>
		<div class="bt_search">
			<fieldset>
				<legend>车易搜</legend>
				<!--#  include file="~/ushtml/block/so/so_bar_new_maiche.shtml"-->
                <!--块内容如下-->                
                <style  type="text/css">
                /*search pop*/
                #sug { border:1px solid #817F82; position:absolute; z-index: 200; width: 350px; top: 21px; background:#fff; display:none; white-space:nowrap; text-align:left; }
                #sug ul { margin:0; padding:0; list-style:none; }
                #sug ul li { background:#fff; color:#000; line-height:10px; padding:5px; margin:0 0 1px 0; cursor:default; float:none; height:10px; overflow:hidden; border-right:none; font-size:14px; width:auto; }
                #sug ul li.mo { background:#014da2; color:#fff }
                #sug ul li.right { text-align:right; }
                #sug ul li.ml_l {background:#fff; color:#164A84}
                #sug a:link, #sug a:visited { color:#014da2; text-decoration:none; }
                #sug a:hover { color:#d01d19; text-decoration:none; }
                .sug_reset { background:#fff }
                .sug_onblur { background:#fff url(http://image.bitauto.com/bt/logo/so_search_bg.gif) no-repeat right center }
                </style>

                <div style="position: relative; float: left; text-align: left;">
                    <form id="sug_form" name="sug_form" target="_blank" method="get" action="http://www.cheyisou.com/post.aspx">
                        <input type="text" class="sug_onblur text" id="sug_txt_keyword" name="so_keywords" value="" />
                        <div onselectstart="return false" id="sug" style="top: 24px; float: left; overflow: hidden">
                        </div>
                        <input type="submit" value="搜索" class="bt_input" id="sug_submit"/>
                        <input id="sug_hidurl" value="" type="hidden" />
                        <input id="sug_datatype" name="sug_datatype" value="chexing" type="hidden" />
                        <input id="sug_encoding" name="sug_encoding" value="utf-8" type="hidden" />
                    </form>
                </div>

                <script type="text/javascript">
                    var sug_txt_css = "text";
                    var so = document.createElement('SCRIPT');
                    so.src = 'http://image.bitautoimg.com/bsearch/js_bar/sug_form_one.js?v12';
                    document.getElementsByTagName('HEAD')[0].appendChild(so);
                    var ti = setInterval(function() { if (!window.bitauto) { return } else { window.bitauto.init(); clearInterval(ti) } }, 200)
                    window.onunload = function() { }; 
                </script>  
			</fieldset>
		</div>
		<!--#   include file="~/ushtml/block/so/hot_word/new_hot_new_maiche.shtml"-->
        <!--块内容如下-->   
        <div class="bt_hot">
        <a href="http://www.cheyisou.com/wenzhang/%E4%B8%8A%E5%B8%82%E6%96%B0%E8%BD%A6/?para=en|utf8" target="_blank">上市新车</a>  
        <a href="http://www.cheyisou.com/wenzhang/%E8%B4%AD%E8%BD%A6%E4%BC%98%E6%83%A0/?para=en|utf8" target="_blank">购车优惠</a>  
        <a href="http://www.cheyisou.com/wenzhang/%E5%9B%A2%E8%B4%AD/?para=en|utf8" target="_blank">团购</a>
        </div>
	</div>
	<!--  -->
	<div class="bt_page"><div class="publicTab1">
		<ul class="tab">
			<li class="on"><a>买车首页</a></li>
			<li><a target="_blank" href="http://car.bitauto.com/">汽车大全</a></li>
			<li><a target="_self" href="http://mai.bitauto.com/xinche/">国产新车</a></li>
			<li><a target="_self" href="http://mai.bitauto.com/jinkou/">进口车</a></li>
			<li><a target="_self" href="http://mai.bitauto.com/gangkou/">港口商情</a></li>
			<li><a target="_blank" href="http://yiche.taoche.com/">二手车</a></li>
			<li><a target="_blank" href="http://car.bitauto.com/tree_hangqing/">行情</a></li>
			<li><a target="_blank" href="http://price.bitauto.com/">报价</a></li>
			<li><a target="_blank" href="http://dealer.bitauto.com/">经销商</a></li>
			<li><a target="_blank" href="http://news.bitauto.com/chanxiao/">销量</a></li>
			<li><a target="_blank" href="http://mai.bitauto.com/baoxian/">保险</a></li>
			<li class="last"><a target="_self" href="http://mai.bitauto.com/xindai/">车贷</a></li>
			<!--<li class="last"><a target="_blank" href="http://life.bitauto.com/huodong/">超级团购</a></li>-->
		</ul>
	</div>
	<div style="height: auto; margin: 0pt auto; width: 950px;" class="bt_ad">
	<ins id="div_468c9d17-915f-4f83-9e1a-4fc98323d295" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="468c9d17-915f-4f83-9e1a-4fc98323d295"> </ins>
	</div>
	<ins id="Ins1" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="468c9d17-915f-4f83-9e1a-4fc98323d295"> </ins>
		<!-- <a href="http://news.bitauto.com/others/20101230/0905271058.html" target="_blank"><strong>摇号</strong></a> |  -->
		<div class="buy_h3"><h3 class="look">看车选车</h3><span class="ad"><a href="http://news.bitauto.com/jshbz/20111018/1705489489.html" target="_blank">节能补贴新政49款车型符合标准</a></span></div>
		<div class="line_box c0908_09"><div id="carbuy_tab" class="carbuy_tab">
			<ul>
				<li tab='newcar' class="current"><a style="text-decoration: none">新车</a></li>
				<li tab='ucar'><a style="cursor: pointer; text-decoration: none">二手车</a></li>
			</ul>
		</div>
			<script language="javascript" type="text/javascript">
				function changeTag() {
					var liObj = this.parentNode;

					var cObj = document.getElementById('carbuy_tab');
					var liObjlist = cObj.getElementsByTagName("li");

					for (var i = 0; i < liObjlist.length; i++) {
						if (liObjlist[i].getAttribute("tab") == liObj.getAttribute("tab")) continue;
						liObjlist[i].className = '';
						document.getElementById(liObjlist[i].getAttribute("tab")).style.display = "none";
						liObjlist[i].childNodes[0].style.cursor = "pointer";
					}
					liObj.className = "current";
					document.getElementById(liObj.getAttribute("tab")).style.display = "block";
					this.style.cursor = "";
				}
				function initCarBuyTab() {
					var cObj = document.getElementById('carbuy_tab');
					if (!cObj) return;

					var linkObjlist = cObj.getElementsByTagName("a");
					if (linkObjlist == null || linkObjlist.length < 1) return;

					for (var i = 0; i < linkObjlist.length; i++) {
						linkObjlist[i].onclick = changeTag;
					}
				}
				initCarBuyTab();
			</script>
			<div class="l">
			<div id="newcar" class="c0621_01">
                <dl>
                    <dt>价　格：</dt>
                    <dd>
                        <ul>
                            <li class="current" id="price0"><a href="http://car.bitauto.com/xuanchegongju/" target="_blank">不限</a></li>
                            <li id="price1"><a href="http://car.bitauto.com/xuanchegongju/?p=0-5" stattype="price0-5"  target="_blank">5万以下</a></li>
                            <li id="price2"><a href="http://car.bitauto.com/xuanchegongju/?p=5-8" stattype="price5-8"  target="_blank">5万-8万</a></li>
                            <li id="price3"><a href="http://car.bitauto.com/xuanchegongju/?p=8-12" stattype="price8-12"  target="_blank">8万-12万</a></li>
                            <li id="price4"><a href="http://car.bitauto.com/xuanchegongju/?p=12-18" stattype="price12-18"  target="_blank">12万-18万</a></li>
                            <li id="price5"><a href="http://car.bitauto.com/xuanchegongju/?p=18-25" stattype="price18-25"  target="_blank">18万-25万</a></li>
                            <li id="price6"><a href="http://car.bitauto.com/xuanchegongju/?p=25-40" stattype="price25-40"  target="_blank">25万-40万</a></li>
                            <li id="price7"><a href="http://car.bitauto.com/xuanchegongju/?p=40-80" stattype="price40-80"  target="_blank">40万-80万</a></li>
                            <li id="price8"><a href="http://car.bitauto.com/xuanchegongju/?p=80-9999" stattype="price80-9999"  target="_blank">80万以上</a></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>级　别：</dt>
                    <dd>
                        <ul>
                            <li class="current" id="level0"><a href="http://car.bitauto.com/xuanchegongju/"  target="_blank">不限</a></li>
                            <li id="level63"><a href="http://car.bitauto.com/xuanchegongju/?l=63" stattype=""  target="_blank">轿车</a></li>
                            <li id="level7"><a href="http://car.bitauto.com/xuanchegongju/?l=7" stattype="" target="_blank">MPV</a></li>
                            <li id="level8"><a href="http://car.bitauto.com/xuanchegongju/?l=8" stattype="" target="_blank">SUV</a></li>
                            <li id="level9"><a href="http://car.bitauto.com/xuanchegongju/?l=9" stattype="" target="_blank">跑车</a></li>
                            <li id="level11"><a href="http://car.bitauto.com/xuanchegongju/?l=11" stattype="" target="_blank">面包车</a></li>
                        </ul>
                    </dd>
                </dl>
				<div id="jiaocheBox" class="car_sub_select">
		<div class="ico_arrow"></div>
		<ul>
		    <li id="Li1"><a href="http://car.bitauto.com/xuanchegongju/?l=63"stattype="jiaoche">全部</a></li>
		    <li id="level1"><a href="http://car.bitauto.com/xuanchegongju/?l=1" stattype="weixingche">微型车</a></li>
		    <li id="level2"><a href="http://car.bitauto.com/xuanchegongju/?l=2" stattype="xiaoxingche">小型车</a></li>
		    <li id="level3"><a href="http://car.bitauto.com/xuanchegongju/?l=3" stattype="jincouxingche">紧凑型车</a></li>
		    <li id="level5"><a href="http://car.bitauto.com/xuanchegongju/?l=5" stattype="zhongxingche">中型车</a></li>
		    <li id="level4"><a href="http://car.bitauto.com/xuanchegongju/?l=4" stattype="zhongdaxingche">中大型车</a></li>
		    <li id="level6"><a href="http://car.bitauto.com/xuanchegongju/?l=6" stattype="haohuache">豪华车</a></li>
		</ul>
	</div>
                <dl>
                    <dt>国　别：</dt>
                    <dd>
                        <ul>
                            <li class="current" id="brandType0"><a href="http://car.bitauto.com/xuanchegongju/" target="_blank">不限</a></li>
                            <li id="brandType1"><a href="http://car.bitauto.com/xuanchegongju/?g=1" stattype="" target="_blank">自主</a></li>
                            <li id="brandType2"><a href="http://car.bitauto.com/xuanchegongju/?g=2" stattype="" target="_blank">合资</a></li>
                            <li id="brandType4"><a href="http://car.bitauto.com/xuanchegongju/?g=4" stattype="" target="_blank">进口</a></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>变速箱：</dt>
                    <dd>
                        <ul>
                            <li class="current" id="trans0"><a href="http://car.bitauto.com/xuanchegongju/" target="_blank">不限</a></li>
                            <li id="trans1"><a href="http://car.bitauto.com/xuanchegongju/?t=1" stattype="trans_mt" target="_blank">手动</a></li>
                            <li id="trans2"><a href="http://car.bitauto.com/xuanchegongju/?t=62" stattype="trans_at" target="_blank">自动</a></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>排　量：</dt>
                    <dd>
                        <ul>
                            <li class="current" id="dis0"><a href="http://car.bitauto.com/xuanchegongju/" target="_blank">不限</a></li>
                            <li id="dis1"><a href="http://car.bitauto.com/xuanchegongju/?d=0-1.3" stattype="dis0-13" target="_blank">1.3以下</a></li>
                            <li id="dis2"><a href="http://car.bitauto.com/xuanchegongju/?d=1.3-1.6" stattype="dis13-16" target="_blank">1.3-1.6L</a></li>
                            <li id="dis3"><a href="http://car.bitauto.com/xuanchegongju/?d=1.7-2.0" stattype="dis17-20" target="_blank">1.7-2.0L</a></li>
                            <li id="dis4"><a href="http://car.bitauto.com/xuanchegongju/?d=2.1-3.0" stattype="dis21-30" target="_blank">2.1-3.0L</a></li>
                            <li id="dis5"><a href="http://car.bitauto.com/xuanchegongju/?d=3.1-5.0" stattype="dis31-50" target="_blank">3.1-5.0L</a></li>
                            <li id="dis6"><a href="http://car.bitauto.com/xuanchegongju/?d=5.0-9" stattype="dis50-999" target="_blank">5.0L以上</a></li>
                        </ul>
                    </dd>
                </dl>
                
                <div class="hideline"></div>
            </div>
				<div id="ucar" class="c0621_01" style="display: none"><dl><dt>价 格：</dt><dd><ul>
					<li class="current"><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/">不限</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=0-3">3万以下</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=3-5">3-5万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=5-8">5-8万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=8-12">8-12万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=12-18">12-18万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=18-25">18-25万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=25-40">25-40万</a></li><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?p=40-9999">40万以上</a></li></ul>
				</dd></dl><dl>
            <dt>级 别：</dt><dd><ul>
                <li class="current"><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/">不限</a></li>
                <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=63">轿车</a></li>
                <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=7">MPV</a></li>
                    <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=8">SUV</a></li>
                    <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=9">跑车</a></li>
                    <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=11">面包车</a></li>
                    </ul>
            </dd>
        </dl>
        <div id="Div1" class="car_sub_select"><div class="ico_arrow"></div>
        <ul><li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=63">全部</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=1">微型车</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=2">小型车</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=3">紧凑型车</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=5">中型车</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=4">中大型车</a></li>
        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?l=6">豪华车</a></li></ul></div>
        <dl>
            <dt>变速箱：</dt><dd><ul>
                <li class="current"><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/">不限</a></li><li>
                    <a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?t=1">手动</a></li>
                    <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?t=62">自动</a></li>
                    </ul>
            </dd>
        </dl>

        <dl>
            <dt>排 量：</dt><dd><ul>
                <li class="current"><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/">不限</a></li><li>
                    <a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?d=0-1.0">1.0以下</a></li>
                    <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?d=1.0-1.6">1.0-1.6L</a></li>
                        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?d=1.6-2.0">1.6-2.0L</a></li>
                        <li><a target="_blank" href="http://car.bitauto.com/tree_ucar/search/?d=2.0-3.0">2.0-3.0L</a></li>
                            <li><a target="_blank"  href="http://car.bitauto.com/tree_ucar/search/?d=3.0-9">3.0L以上</a></li></ul>
            </dd>
        </dl><div class="hideline"></div>
				</div>
				<div class="c0908_01"><div class="c">
					<p class="a">
						<select id="baaMaster">
							<option>请选择品牌</option>
						</select><select id="baaSerial"><option>请选择车型</option>
						</select><input id="goAsk" type="button" value="提问题" class="carbuy_btn2" /><input id="goBaa" type="button" value="去论坛" class="carbuy_btn2" /></p>
				</div>
				</div>
			</div>
			<div class="c0908_02">
			<div class="c0908_03">
				<h4 class="carbuyh4">
					按品牌找车</h4>
				<p>
					<select id="MasterSelectList">
					</select></p>
				<p>
					<select id="SerialSelectList">
						<option>请选择车型</option>
					</select></p>
				<div align="center">
					<input type="button" value="看车型" id="goCar" class="carbuy_btn2" /></div>
			</div>
			<div class="cardaikuan">
        	<p>在线汽车贷款，0利率，0月供！</p>
        	<div class="fun"><a href="http://market.bitauto.com/gmac2010/index.aspx" target="_blank">贷款买车</a></div>
        </div>
				<div class="c0908_04">
					<h4 class="carbuyh4">
						对比工具</h4>
					<p>
						你可以在此比较不同车型的差异</p>
					<ul>
						<li><a href="http://car.bitauto.com/chexingduibi/" target="_blank" title="车型对比">车型对比</a></li>
						<li><a href="http://car.bitauto.com/tupianduibi/" target="_blank" title="图片对比">图片对比</a></li>
						<li><a href="http://koubei.bitauto.com/duibi/" target="_blank" title="口碑对比">口碑对比</a></li>
						<li><a href="http://car.bitauto.com/pingceduibi/" target="_blank" title="评测对比">评测对比</a></li>
					</ul>
				</div>
			</div>
		</div>
		<!--  -->
		<div class="buy_h3"><h3 class="pre">买前准备</h3><span class="ad"></span></div>
		<!--  -->
		<div class="line_box c0908_05"><div class="c0908_06">
			<h4 class="carbuyh4">
				<span><a href="http://price.bitauto.com/" target="_blank">查报价</a></span>|<span><a target="_blank" href="http://car.bitauto.com/gouchejisuanqi/">购车费用预估</a></span></h4>
			<div class="c">
				<p>
					<select id="mbSelectList">
						<option>请选择品牌</option>
					</select></p>
				<p>
					<select id="sbSelectList">
						<option>请选择车型</option>
					</select></p>
				<p>
					<select id="carSelectList">
						<option>请选择车款</option>
					</select></p>
				<div>
					<input id="pBtn" type="button" value="查报价" class="carbuy_btn1" /><input id="allBtn" type="button" value="全款购车" class="carbuy_btn2" /><input type="button" id="dBtn" value="贷款购车" class="carbuy_btn2" /></div>
			</div>
		</div>
			<!--  -->
			<div class="c0908_07">
				<h4 class="carbuyh4">
					<a href="http://dealer.bitauto.com/" target="_blank">4S店经销商</a></h4>
				<div class="c"><div class="s">
					<!--#  include file="~/ushtml/block/so/so_bar_maiche_jingxiaoshang.shtml"-->
                    <!--块内容如下-->   
                    <style  type="text/css">
                    /*search pop*/
                    #sug_ext { border:1px solid #817F82; position:absolute; z-index: 200; width: 350px; top: 21px; background:#fff; display:none; white-space:nowrap; text-align:left; }
                    #sug_ext ul { margin:0; padding:0; list-style:none; }
                    #sug_ext ul li { background:#fff; color:#000; line-height:10px; padding:5px; margin:0 0 1px 0; cursor:default; float:none; height:10px; overflow:hidden; border-right:none; font-size:14px; width:auto; }
                    #sug_ext ul li.mo { background:#014da2; color:#fff }
                    #sug_ext ul li.right { text-align:right; }
                    #sug_ext ul li.ml_l {background:#fff; color:#164A84}
                    #sug_ext a:link, #sug a:visited { color:#014da2; text-decoration:none; }
                    #sug_ext a:hover { color:#d01d19; text-decoration:none; }
                    .sug_reset { background:#fff }
                    .sug_onblur { background:#fff url(http://image.bitauto.com/bt/logo/so_search_bg.gif) no-repeat right center }
                    </style>

                    <div style="position: relative; float: left; text-align: left;">
                        <form id="sug_form_ext" name="sug_form_ext" target="_blank" method="get" action="http://www.cheyisou.com/post_ext.aspx">
                            <input type="text" class="sug_onblur text" id="sug_txt_keyword_ext" name="so_keywords_ext" value="" /><input type="submit" value="搜索" class="bt_input" id="sug_submit_ext"/>
                            <div onselectstart="return false" id="sug_ext" style="top: 24px; float: left; overflow: hidden">
                            </div>        
                            <input id="Hidden1" value="" type="hidden" />
                            <input id="sug_datatype_ext" name="sug_datatype_ext" value="jingxiaoshang" type="hidden" />
                            <input id="sug_encoding_ext" name="sug_encoding_ext" value="utf-8" type="hidden" />
                        </form>
                    </div>

                    <script type="text/javascript">
                        var sug_txt_css_ext = "text";
                        var so_ext = document.createElement('SCRIPT');
                        so_ext.src = 'http://image.bitautoimg.com/bsearch/js_bar/sug_form_two.js?v2';
                        document.getElementsByTagName('HEAD')[0].appendChild(so_ext);
                        var ti_ext = setInterval(function() { if (!window.bitauto_ext) { return } else { window.bitauto_ext.init(); clearInterval(ti_ext) } }, 200)
                        window.onunload = function() { }; 
                    </script>
					<div class="clear"></div>
					<div class="c0908_08">
						<h5 class="carbuyh5">
							<span><a href="http://city.bitauto.com/" target="_blank">各地车市</a></span> <em>341个城市</em></h5>
						<ul>
							<li><a href="http://beijing.bitauto.com/cheshi/" target="_blank">北京</a></li>
							<li><a href="http://shanghai.bitauto.com/cheshi/" target="_blank">上海</a></li>
							<li><a href="http://guangzhou.bitauto.com/cheshi/" target="_blank">广州</a></li>
							<li><a href="http://shenzhen.bitauto.com/cheshi/" target="_blank">深圳</a></li>
							<li><a href="http://fuzhou.bitauto.com/cheshi/" target="_blank">福州</a></li>
							<li><a href="http://haikou.bitauto.com/cheshi/" target="_blank">海口</a></li>
							<li><a href="http://nanning.bitauto.com/cheshi/" target="_blank">南宁</a></li>
							<li><a href="http://nanjing.bitauto.com/cheshi/" target="_blank">南京</a></li>
							<li><a href="http://suzhou.bitauto.com/cheshi/" target="_blank">苏州</a></li>
							<li><a href="http://hangzhou.bitauto.com/cheshi/" target="_blank">杭州</a></li>
							<li><a href="http://ningbo.bitauto.com/cheshi/" target="_blank">宁波</a></li>
							<li><a href="http://hefei.bitauto.com/cheshi/" target="_blank">合肥</a></li>
							<li><a href="http://zhengzhou.bitauto.com/cheshi/" target="_blank">郑州</a></li>
							<li><a href="http://nanchang.bitauto.com/cheshi/" target="_blank">南昌</a></li>
							<li><a href="http://wuhan.bitauto.com/cheshi/" target="_blank">武汉</a></li>
							<li><a href="http://changsha.bitauto.com/cheshi/" target="_blank">长沙</a></li>
							<li><a href="http://chengdu.bitauto.com/cheshi/" target="_blank">成都</a></li>
							<li><a href="http://chongqing.bitauto.com/cheshi/" target="_blank">重庆</a></li>
							<li><a href="http://kunming.bitauto.com/cheshi/" target="_blank">昆明</a></li>
							<li><a href="http://guiyang.bitauto.com/cheshi/" target="_blank">贵阳</a></li>
							<li><a href="http://xian.bitauto.com/cheshi/" target="_blank">西安</a></li>
							<li><a href="http://lanzhou.bitauto.com/cheshi/" target="_blank">兰州</a></li>
							<li><a href="http://taiyuan.bitauto.com/cheshi/" target="_blank">太原</a></li>
							<li><a href="http://shijiazhuang.bitauto.com/cheshi/" target="_blank">石家庄</a></li>
							<li><a href="http://jinan.bitauto.com/cheshi/" target="_blank">济南</a></li>
							<li><a href="http://qingdao.bitauto.com/cheshi/" target="_blank">青岛</a></li>
							<li><a href="http://tianjin.bitauto.com/cheshi/" target="_blank">天津</a></li>
							<li><a href="http://changchun.bitauto.com/cheshi/" target="_blank">长春</a></li>
							<li><a href="http://shenyang.bitauto.com/cheshi/" target="_blank">沈阳</a></li>
							<li><a href="http://dalian.bitauto.com/cheshi/" target="_blank">大连</a></li>
							<li><a href="http://haerbin.bitauto.com/cheshi/" target="_blank">哈尔滨</a></li>
							<li><a href="http://huhehaote.bitauto.com/cheshi/" target="_blank">呼和浩特</a></li>
							<li><a href="http://city.bitauto.com/" target="_blank">更多>></a></li>
						</ul>
					</div>
				</div>
			</div>
		</div>
		</div> 
		<div id="daodiangouche" class="buy_h3"><h3 class="buy">到店购车</h3><span class="ad"><a href="http://www.bitauto.com/zhuanti/daogou/gsqgl/" target="_blank" class="firstA">提车上牌全攻略</a></span> </div>
		<div class="line_box c0908_10">
			<ul>
				<li class="step1"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-1.html" target="_blank"><span>办理条件与注意事项</span></a></li>
				<li class="step2"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-2.html" target="_blank" class="firstA">购买保险</a> <a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-3.html" target="_blank" class="secondA">缴纳车船税</a></li>
				<li class="step3"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-6.html" target="_blank" class="firstA">缴纳购置税</a> <a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-7.html" target="_blank" class="secondA">工商验证</a></li>
				<li class="step4"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-8.html" target="_blank"><span>登记注册</span></a></li>
				<li class="step5"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-9.html" target="_blank"><span>领取证件标志</span></a></li>
				<li class="step6"><a href="http://news.bitauto.com/gouchezhinan/20110922/1405470134-12.html" target="_blank"><span>使用车管家>></span></a></li>
			</ul>
		</div>
		<!--#include file="~/html/commonfooter.shtml"-->
	</div>
	<!--#include file="~/include/09gq/gg/maicheban/chexinglanmuye/00001/mclmyspts_Manual.shtml"-->
	<ins id="div_4a35df7d-63b4-4ad7-83d6-944d19d2794e" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="" adplay_brandtype="" adplay_blockcode="4a35df7d-63b4-4ad7-83d6-944d19d2794e"></ins>
	<script src="http://js.inc.baa.bitautotech.com/bitauto/bitauto.login.js" type="text/javascript"></script>
	<script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsCommon/jsGoCarAndPriceJsonNew.js"></script>
	<script type="text/javascript" language="javascript" charset="utf-8" src="http://car.bitauto.com/car/ajaxnew/GetCityList.ashx?type=p"></script>
	<script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsnew/GoCarTypeAndPrice.js"></script>
	<script language="javascript" type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jsnew/GoAllCalculateAndCredit.js?d=20120130"></script>
	<script language="javascript" type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jsnew/GoAskAndFriend.js"></script>
	<script type="text/javascript" language="javascript">
		var PageSelectObject;
		DomHelper.addEvent(window, "load", function () {
			if (typeof JSonData != 'undefined') {
				PageSelectObject = StartInit("select", "select", "MasterSelectList", "SerialSelectList", "goCar", "", "0");
				PageSelectObject.Init();

				PageSelectObject = new PriceSelectInitObj("mbSelectList", "sbSelectList", "carSelectList", "pBtn", "allBtn", "dBtn");
				PageSelectObject.Init();
				PageSelectObject = new Baa("baaMaster", "baaSerial", "baaProvince", "baaCity", "goAsk", "goBaa", "goFriend");
				PageSelectObject.Init();
			}
		}, false);
	</script>
	<!-- 车易购个人信息 -->
	<script type="text/javascript" charset="gb2312" src="http://go.bitauto.com/bitauto/usercar.ashx?fmax=40&code=gb2312"></script>
	<script src="http://www.bitauto.com/themes/2009/js/headcommon.js" type="text/javascript"></script>
	<script type="text/javascript" src="http://www.bitauto.com/themes/2009/js/search.js"></script>
	<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"></script>
	<!--WebTrends Analytics-->
	<!-- START OF SmartSource Data Collector TAG -->
	<script src="http://css.bitauto.com/bt/webtrends/dcs_tag_city13.js" type="text/javascript"></script>
	<!-- END OF SmartSource Data Collector TAG -->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
