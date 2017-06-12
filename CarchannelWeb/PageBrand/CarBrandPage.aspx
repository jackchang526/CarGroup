<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarBrandPage.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageBrand.CarBrandPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=brandName%>】<%=brandName%>汽车报价_图片_<%=DateTime.Now.Year+brandName%>新款车型-易车网</title>
	<meta name="Keywords" content="<%=brandName%>,<%=brandName%>汽车,<%=brandName%>汽车报价,<%=DateTime.Now.Year %><%=brandName %>新款车型,易车网,car.bitauto.com" />
	<meta name="Description" content="<%=brandName%>汽车:提供最新<%=brandName%>汽车报价,<%=brandName%>汽车图片,<%=brandName%>汽车新闻,视频,口碑,问答,二手车等。<%=brandName%>在线询价、低价买车尽在易车网。" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=brandSpell %>/" />
	<%--<meta name="mobile-agent" content="format= xhtml; url=http://m.bitauto.com/g/carbrand.aspx?brandid=<%=brandId %>" />--%>
	<!--#include file="~/ushtml/0000/yiche_2014_cube_chexingzhupinpaipinpai-771.shtml"-->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--#include file="~/html/header2014.shtml"-->
	<div class="header_style">
		<div class="bitauto_logo">
		</div>
		<div class="pp-logo-img">
			<a href="/<%=brandSpell %>/">
				<img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=masterId %>_55.png"
                    alt="<%=brandName%>汽车"></a>
		</div>
		<div class="pp-logo-tit">
			<h1>
				<a href="/<%=brandSpell %>/">
					<%=brandName%></a>
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
	<div class="bt_page">
		<div class="publicTabNew">
			<ul class="tab" id="ulForTempClickStat">
				<li id="treeNav_chexing" class=" current"><a target="_self" href="http://car.bitauto.com/<%=brandSpell%>/">首页</a></li>
				<li id="treeNav_wenzhang"><a target="_self" href="http://car.bitauto.com/<%=brandSpell%>/wenzhang/">文章</a></li>
				<li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/brand/<%=brandId%>/">图片</a></li>
				<li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/brand/<%=brandId%>_0_0.html">视频</a></li>
				<li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/b<%=brandId%>/">报价</a></li>
				<li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/b<%=brandId%>/">降价</a></li>
				<li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=brandSpell%>/">经销商</a></li>
				<li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?brand=<%=brandId%>/">置换</a></li>
				<li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=brandSpell %>/">二手车</a></li>
				<li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/b_<%=brandId%>/">口碑</a></li>
				<li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=masterId%>/">问答</a></li>
			</ul>
		</div>
		<div class="col-all clearfix">
			<div class="col-con">
				<div class="pp-story-box">
					<!--简介-->
					<%=brandIntroduce%>
				</div>
				<div class="line-box">
					<!--新闻资讯-->
					<%=brandTopNews%>
				</div>
				<div class="line-box">
					<!--品牌中的子品牌列表-->
					<%=serialListByBrand %>
				</div>
				<%--<!--图片-->
                <%=imageHtml%>
                <!--口碑-->
                <%=koubeiHtml %>
                <!--答疑-->
                <%=askEntriesHtml %>
                <!--论坛-->
                <%=forumHtml %>--%>
				<div class="line-box">
					<!--视频-->
					<%=videoHtml%>
				</div>
				<div class="line-box">
					<div class="title-con">
						<div class="title-box">
							<h3>
								<a href="http://dealer.bitauto.com/<%=brandSpell %>/" target="_blank">
									<%=brandName.Replace("·", "&bull;")%>-经销商推荐</a></h3>
							<div class="more">
								<a href="http://dealer.bitauto.com/<%=brandSpell %>/" target="_blank" rel="nofollow">更多&gt;&gt;</a>
							</div>
						</div>
					</div>
					<!--经销商-->
					<div id="venderInfo" class="line_box dealer">
						<%--<ins id="ep_union_107" partner="1" version="" isupdate="1" type="1" city_type="-1"
							city_id="0" city_name="0" car_type="1" brandid="<%=brandId %>" serialid="0" carid="0"></ins>--%>
						<%--<ins id="ep_union_83" partner="1" version="" isupdate="1" type="1" city_type="-1"
                            city_id="0" city_name="0" car_type="1" brandid="<%=brandId %>" serialid="0" carid="0">
                        </ins>--%>
						<script type="text/javascript">
						    document.writeln("<ins Id=\"ep_union_129\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"1\" city_type=\"1\" city_id=\"" + bit_locationInfo.cityId + "\" city_name=\"0\" car_type=\"1\" brandId=\"<%=brandId %>\" serialId=\"0\" carId=\"0\"></ins>");
						</script>
						<div class="clear">
						</div>
					</div>
				</div>
				<%--问答块--%>
				<!--#include file="~/include/pd/2012/wenda/00001/201405_ask_common_gb2312_Manual.shtml"-->
				<%--热门推荐块--%>
				<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_tuijian_Manual.shtml"-->
				<%-- <!-- SEO底部热门 -->
                <!--#include file="~/html/SeoBottomHotListShort.shtml"-->
                <!-- SEO底部热门 -->--%>
			</div>
			<div class="col-side">
				<!--# include file="~/html/HotBrand.shtml"-->
				<!-- 热门品牌 -->
				<div class="line-box line-box_t0">
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
				<%--<!-- 品牌下热门子品牌 -->
                <%=brandHotSerial%>
                <!--品牌的排行-->
                <%=_BrandRankingHTML%>
                <script language="javascript" type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/brandpvrankingspancode.js"></script>
                
                <!-- 热门品牌 -->
                <% if (IsHasFriendLink)
                   { %>
                <%= FriendLinkNew %>
                <% }
                   else
                   { %>
                <!--#include file="~/html/ForignLink.shtml"-->
                <% } %>--%>
				<!--百度推广-->
				<div class="line_box baidupush">
					<script type="text/javascript">
						/*bitauto 200*200，导入创建于2011-10-17*/
					    var cpro_id = 'u646188';
					</script>
					<script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
				</div>
				<!--百度推广-End -->
			</div>
		</div>
		<!--经销商代码-->
		<script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
		<script src="http://image.bitautoimg.com/carchannel/jsnew/taoche.js?v=2015012614" type="text/javascript"></script>
		<%-- <script src="../jsnew/taoche.js?v=20130802" type="text/javascript"></script>--%>
		<script>
		    TaoChe.showBrandUCar('<%=brandId %>', bit_locationInfo.cityId, '<%=brandSpell %>', '<%=brandName %>', 5);
		//var ucar_nav_link = $("#ucar_nav_link").attr("href");
			//$("#ucar_nav_link").attr("href", ucar_nav_link.replace("201", bit_locationInfo.cityId));
		</script>
		<!--本站统计代码-->
		<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
		<script type="text/javascript">
		    OldPVStatistic.ID1 = "<%=brandId %>";
		    OldPVStatistic.ID2 = "0";
		    OldPVStatistic.Type = 3;
		    mainOldPVStatisticFunction();
		</script>
		<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
		<script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>
		<!-- 调用尾 -->
		<script type="text/javascript">
		    var CarCommonCBID = '<%= brandId.ToString() %>';
		</script>
	</div>
	<!--#include file="~/html/footer2014.shtml"-->
	<!-- 弹出 -->
	<div class="tc-popup-box tc-popup-box-onlytxt" id="popup-box-info" style="display: none;">
		<div class="tt">
			<h6>
				<%=brandName%>
                品牌故事</h6>
			<a href="javascript:;" class="btn-close" id="btn-close-info" target="_self">关闭</a>
		</div>
		<div class="tc-popup-con p-set">
			<div class="tc-scroll">
				<p>
					<%=brandIntro%>
				</p>
			</div>
		</div>
	</div>
	<div class="tc-popup-box tc-popup-box-onlytxt" id="popup-box-story" style="display: none;">
		<div class="tt">
			<h6>
				<%=brandName%>
                车标故事</h6>
			<a href="javascript:;" class="btn-close" id="btn-close-story" target="_self">关闭</a>
		</div>
		<div class="tc-popup-con p-set">
			<div class="tc-scroll">
				<p>
					<%=logoStory%>
				</p>
			</div>
		</div>
	</div>
	<script>
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
	    !function ($) {
	        $.fn.showCenter = function () {
	            var top = (($(window).height() - this.height()) / 2) + 18;
	            var left = ($(window).width() - this.width()) / 2;
	            var scrollTop = $(document).scrollTop();
	            var scrollLeft = $(document).scrollLeft();

	            if (! -[1, ] && !window.XMLHttpRequest) {
	                return this.css({ position: 'absolute', 'z-index': 1100, 'top': top + scrollTop + 60, left: left + scrollLeft }).show();
	            }
	            else {
	                return this.css({ position: 'fixed', 'z-index': 1100, 'top': "218px", left: left }).show();
	            }
	        }
	    }(jQuery);

	    $("#show-box-info").click(function () { $("#popup-box-info").showCenter(); });
	    $("#show-box-story").click(function () { $("#popup-box-story").showCenter(); });
	    $("#btn-close-info").click(function () { $("#popup-box-info").hide(); })
	    $("#btn-close-story").click(function () { $("#popup-box-story").hide(); });
	</script>
	<%if (brandId == 20109)
   { %>
	<!--百度热力图-->
	<script type="text/javascript">
	    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
	    document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<%} %>

	<!--#include file="~/html/CommonBodyBottom.shtml"-->
	<!--提意见浮层-->
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
<script type="text/javascript">
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
        tabs("car_tab_BrandNews_ul", "data_table_BrandNews", "best_car", true);
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
                        thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
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
