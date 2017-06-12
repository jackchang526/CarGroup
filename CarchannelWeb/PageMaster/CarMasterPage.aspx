<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterPage.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.PageMaster.CarMasterPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=masterName%>】<%=masterName%>汽车报价_图片_<%=DateTime.Now.Year+masterName%>新款车型-易车网</title>
	<meta name="Keywords" content="<%=masterName%>,<%=masterName%>汽车,<%=masterName%>汽车报价,<%=masterName%>新款车型,易车网,car.bitauto.com" />
	<meta name="Description" content="<%=masterName%>:易车提供最新<%=masterName%>汽车报价,<%=masterName%>汽车图片,<%=masterName%>汽车新闻,视频,口碑,问答,二手车等,<%=masterName%>在线询价、低价买车尽在易车网。" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=masterUrlSpell %>/" />
	<%--<meta name="mobile-agent" content="format= xhtml; url=http://m.bitauto.com/g/carbrand.aspx?bigbrandid=<%=masterId %>" />--%>
	<!--#include file="~/ushtml/0000/yiche_2014_cube_chexingzhupinpaipinpai-771.shtml"-->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<div class="bt_ad">
		<ins id="div_372efa2f-ebc3-42b7-b13b-14bdc1a4f3a1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="<%=masterName %>" adplay_brandtype="<%=masterCountry %>" adplay_blockcode="372efa2f-ebc3-42b7-b13b-14bdc1a4f3a1"></ins>
	</div>
	<div class="bt_pageBox">

		<div class="header_style">
			<div class="bitauto_logo">
			</div>
			<div class="pp-logo-img">
				<a href="/<%=masterUrlSpell %>/">
					<img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=masterId %>_55.png"
                        alt="<%=masterName%>汽车"></a>
			</div>
			<div class="pp-logo-tit">
				<h1>
					<a href="/<%=masterUrlSpell %>/">
						<%=masterName%></a>
				</h1>
			</div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchV3.shtml"-->
			</div>
			<div class="bt_hot">
				热门搜索： <a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a>
				<a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/"
					target="_blank">降价</a>
			</div>
		</div>
		<div class="publicTabNew">
			<ul class="tab" id="ulForTempClickStat">
				<li id="treeNav_chexing" class=" current"><a target="_self" href="http://car.bitauto.com/<%=masterUrlSpell%>/">首页</a></li>
				<li id="treeNav_wenzhang"><a target="_self" href="http://car.bitauto.com/<%=masterUrlSpell%>/wenzhang/">文章</a></li>
				<li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/master/<%=masterId%>/">图片</a></li>
				<li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/master/<%=masterId%>_0_0.html">视频</a></li>
				<li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/mb<%=masterId%>/">报价</a></li>
				<li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/mb<%=masterId%>/">降价</a></li>
				<li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=masterUrlSpell%>/">经销商</a></li>
				<li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?master=<%=masterId%>/">置换</a></li>
				<li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=masterUrlSpell %>/">二手车</a></li>
				<li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/mb_<%=masterId%>/">口碑</a></li>
				<li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=masterId%>/solved/">问答</a></li>
			</ul>
		</div>
	</div>
	<div class="bt_page">
		<div class="col-all clearfix">
			<div class="col-con">
				<div class="pp-story-box">
					<!--品牌简介-->
					<%=masterIntroduce %>
				</div>
				<!--品牌最新资讯-->
				<div class="line-box">
					<%=masterTopNews%>
				</div>
				<!--主品牌下每个品牌中的子品牌列表-->
				<div class="line-box <%=longTitleCss %>">
					<%=serialListByBrand %>
					<%if (OnlyOneCbID != 0)
	   { %>
					<div class="more ad22017">
						<ins id="Ins1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
							adplay_brandid="<%= OnlyOneCbID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="e2e467ea-524d-4424-8368-03cf82f82a43"></ins>
					</div>
					<%} %>
				</div>
				<!--视频-->
				<%=videoHtml%>
				<!--经销商-->
				<div class="line-box">
					<%=brandListDealerHtml %>
				</div>
				<%--问答块--%>
				<!-- # include file="~/include/pd/2012/wenda/00001/201405_ask_common_gb2312_Manual.shtml"-->
				<%=askEntriesHtml %>
				<%--热门推荐块--%>
				<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
				<%--  <!--答疑-->
                <div class="line-box">
                    <%=askEntriesHtml %>
                </div>
                <!--图片-->
                <%=imageHtml%>
                <!--口碑-->
                <%=koubeiHtml %>
                <!--论坛-->
                <%=forumHtml %>
                <!-- SEO底部热门 -->
                <!--#include file="~/html/SeoBottomHotListShort.shtml"-->
                <!-- SEO底部热门 -->--%>
			</div>
			<div class="col-side">
				<!-- 热门品牌 -->
				<div class="line-box line-box_t0">
					<!--# include file="~/html/HotBrand.shtml"-->
					<!-- 热门品牌 -->
					<div class="side_title">
						<h4>
							<a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a>
						</h4>
					</div>
					<div class="carpic_list list_pic5555 ">
						<%=hotMasterBrandHtml %>
					</div>
				</div>
				<!--二手车-->
				<%--<%=usecarHtml %>--%>
				<div class="line-box ucar_box">
				</div>
				<!--百度推广-->
				<div class="line-box baidupush">
					<script type="text/javascript">
						/*bitauto 200*200，导入创建于2011-10-17*/
					    var cpro_id = 'u646188';
					</script>
					<script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
				</div>
				<!--百度推广-End -->
			</div>
		</div>
	</div>
	<script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
	<%--<script src="http://image.bitautoimg.com/carchannel/jsnew/taoche.js?v=20130802" type="text/javascript"></script>--%>
	<script src="http://image.bitautoimg.com/carchannel/jsnew/taoche.js?v=2015012614" type="text/javascript"></script>
	<%--<script src="../jsnew/taoche.js?v=20130802" type="text/javascript"></script>--%>
	<script>
	    TaoChe.showBrandUCar('<%=string.Join(",",brandIds.ToArray()) %>', bit_locationInfo.cityId, '<%=masterUrlSpell %>', '<%=masterName %>', 5);
	//var ucar_nav_link = $("#ucar_nav_link").attr("href");
		//$("#ucar_nav_link").attr("href", ucar_nav_link.replace("201", bit_locationInfo.cityId));
	</script>
	<!--本站统计代码-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript">
	    OldPVStatistic.ID1 = "<%=masterId.ToString() %>";
	    OldPVStatistic.ID2 = "0";
	    OldPVStatistic.Type = 2;
	    mainOldPVStatisticFunction();
	</script>
	<script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>
	<!-- 调用尾 -->
	<script type="text/javascript">
	    var CarCommonBSID = '<%= masterId.ToString() %>';
	</script>
	<!--#include file="~/html/footer2014.shtml"-->
	
	<script type="text/javascript">
	    function BrandZhanKai() {
	        $("#shortBrandStory").hide();
	        $("#detailBrandStory").show();
	    }
	    
	    function BrandShouQi() {
	        $("#shortBrandStory").show();
	        $("#detailBrandStory").hide();
	    }
	    
	    function CheBiaoZhanKai() {
	        $("#shortLogoStory").hide();
	        $("#detailLogoStory").show();
	    }

	    function CheBiaoShouQi() {
	        $("#shortLogoStory").show();
	        $("#detailLogoStory").hide();
	    }

	    // JavaScript Document
	    function addLoadEvent(func) {
	        var oldonload = window.onload;
	        if (typeof window.onload != 'function') {
	            window.onload = func;
	        } else {
	            window.onload = function () {
	                oldonload();
	                func();
	            }
	        }
	    }

	    function addClass(element, value) {
	        if (!element.className) {
	            element.className = value;
	        } else {
	            newClassName = element.className;
	            newClassName += " ";
	            newClassName += value;
	            element.className = newClassName;
	        }
	    }

	    function removeClass(element, value) {
	        var removedClass = element.className;
	        var pattern = new RegExp("(^| )" + value + "( |$)");
	        removedClass = removedClass.replace(pattern, "$1");
	        removedClass = removedClass.replace(/ $/, "");
	        element.className = removedClass;
	        return true;
	    }
	    function all_func() {
	        tabs("car_tab_MasterNews_ul", "data_table_MasterNews", "best_car", true);
	        tabs("car_MasterSerialList_ul", "data_table_MasterSerialList", "best_car", true);
	        tabs("car_MasterDealer_tab_ul", "data_table_MasterDealer", "best_car", true);
	    }
	    addLoadEvent(all_func)

	    /*=======================tab=============================*/
	    function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
	    function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

	    function tabsRemove(index, head, divs, div2s) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByTagName("li");
	            for (var i = 0; i < alis.length; i++) {
	                removeClass(alis[i], "current");
	                hide(divs + "_" + i);
	                if (div2s) { hide(div2s + "_" + i) };
	                if (i == index) {
	                    addClass(alis[i], "current");
	                }
	            }
	            show(divs + "_" + index);
	            if (div2s) { show(div2s + "_" + index) };
	        }
	    }
	    var thetabstime;
	    function tabs(head, divs, div2s, over) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByTagName("li");
	            for (var i = 0; i < alis.length; i++) {
	                alis[i].num = i;
	                if (over) {
	                    alis[i].onmouseover = function () {
	                        var thisobj = this;
	                        changetab(thisobj);
	                        //thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
	                    }
	                    alis[i].onmouseout = function () {
	                        clearTimeout(thetabstime);
	                    }
	                }
	                else {
	                    alis[i].onclick = function () {
	                        if (this.className == "current" || this.className == "last current") {
	                            changetab(this);
	                            return true;
	                        }
	                        else {
	                            changetab(this);
	                            return false;
	                        }
	                    }
	                }
	                function changetab(thebox) {
	                    tabsRemove(thebox.num, head, divs, div2s);
	                }
	            }
	        }
	    }
	</script>
	<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
<script type="text/javascript">
    var adplay_CityName = '';//城市
    var adplay_AreaName = '';//区域
    var adplay_BrandID = '<%=masterId%>';//品牌id 
    var adplay_BrandType = '<%=masterCountry%>';//品牌类型：（国产）或（进口）
    var adplay_BrandName = '<%=masterName%>';//品牌
    var adplay_BlockCode = '1e746e70-1cbb-48c6-81c9-0163808f7a70';//广告块编号
</script>
<script type="text/javascript" src="http://gimg.bitauto.com/js/sense.js"></script>
<!--百度热力图,与webtrents对比用，过几天可删（2011-11-29）-->
<script type="text/javascript">
    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
    document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
</script>
