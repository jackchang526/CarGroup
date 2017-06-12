<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialForCityPriceRank.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.PageList.SerialForCityPriceRank" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>
		<%=seoFullName%>汽车排行榜】价格区间汽车报价_汽车价格关注排行榜-易车网</title>
	<meta name="Keywords" content="<%=seoFullName%>汽车排行榜,汽车报价,汽车价格,易车网" />
	<meta name="Description" content="<%=seoFullName%>汽车排行榜：易车网汽车价格关注排行榜为您提供全国<%=seoFullName%>车型的汽车关注排名，是您了解<%=seoFullName%>汽车报价的最佳汽车网站。" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_paihang-793.shtml"-->
	<script type="text/javascript">
    <!--
		var tagIframe = null;
		var currentTagId = 61; 	//当前页的标签ID
    -->
	</script>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
	<div class="bt_pageBox">
 		<!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
 		<div class="header_style">
			<div class="bitauto_logo">
			</div>
			<!--页头导航_yiche_LOGO开始-->
			<div class="yiche_logo">
				<a href="http://www.bitauto.com">易车网</a></div>
			<!--页头导航_yiche_LOGO结束-->
			<div class="yiche_lanmu">
				<em>|</em><a href="#">排行</a></div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchV3.shtml"-->
			</div>
			<div class="bt_hot">
				热门搜索：<a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a>
				<a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/"
					target="_blank">降价</a></div>
		</div>
		<div class="publicTabNew">
			<ul class="tab" id="">
				<li id=""><a href="http://news.bitauto.com/hot/">热点内容</a></li>
				<%--<li id=""><a href="#">综合油耗</a></li>
				<li id=""><a href="#">网友口碑</a></li>
				<li id=""><a href="#">养车费用</a></li>--%>
				<li id=""><a href="/weixingche/paihang/">级别关注</a></li>
				<li id="" class="current"><a href="#">价格关注</a></li>
			</ul>
		</div>
		<div class="pinpai_box">
			<div class="pinpai_menu">
				<ul class="list8">
					<%=priceHtml %>
				</ul>
			</div>
		</div>
	</div>
	<!--page start-->
	<div class="bt_page">
		<!--紧凑型车关注度排行-->
		<div class="line-box">
			<div class="title-con">
				<div class="title-box">
					<h3>
						<a href="/<%=price %>/paihang/">
							<%=fullName%>关注度排行</a></h3>
					<ul class="title-tab">
						<li class="bt-hover"><a id="currentCity" class="pop" href="javascript:;">
							<%=cityName %><strong></strong></a>
							<div id="popCity" class="title-popbox title-popbox-area" style="display: none;">
								<dl>
									<%=cityListHtml%>
								</dl>
							</div>
						</li>
					</ul>
				</div>
			</div>
			<!--内容 start-->
			<div class="rank-list-box-bg rank-list-box-bg990_4">
				<div class="rank-list-qian">
					<ol>
						<%=serialsHtml%>
					</ol>
				</div>
				<div class="clear">
				</div>
			</div>
			<!--内容 end-->
		</div>
		<!--紧凑型车关注度排行-->
	</div>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
	<script>
		(function () {
			var timer;
			$("#currentCity,#popCity").hover(function () { clearTimeout(timer); $("#popCity").show(); }, function () { timer = setTimeout(function () { $("#popCity").hide(); }, 500); });
		})();
	</script>
	<!-- 调用尾 -->
	<!--#include file="~/html/footer2014.shtml"-->
	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<!--#include file="~/include/pd/2014/00001/201701_cxzsy_ycfdc_Manual.shtml"-->
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
