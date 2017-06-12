<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LevelPaihang.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageLevel.LevelPaihang" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=levelFullName%>_排行榜】-易车网BitAuto.com</title>
	<meta name="Keywords" content="车型数据库, 汽车最新报价,车型导购,汽车评测,汽车新闻,汽车图片,汽车问答,汽车点评，汽车经销商，汽车论坛" />
	<meta name="Description" content="易车网(BitAuto.com) 汽车车型为您提供各种汽车车型所有信息。包括汽车报价、最新报价、汽车图片、汽车参数、汽车配置、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_paihang-793.shtml"-->
	<script type="text/javascript">
    <!--
	var tagIframe = null;
	var currentTagId = 61; 	//当前页的标签ID
	-->
	</script>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<div class="bt_pageBox">
		<!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
		<div class="header_style">
			<div class="bitauto_logo">
			</div>
			<!--页头导航_yiche_LOGO开始-->
			<div class="yiche_logo">
				<a href="http://www.bitauto.com">易车网</a>
			</div>
			<!--页头导航_yiche_LOGO结束-->
			<div class="yiche_lanmu">
				<em>|</em><span>网友关注度排行</span>
			</div>
			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchV3.shtml"-->
			</div>
			<div class="bt_hot">
				热门搜索：<a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a>
				<a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/"
					target="_blank">降价</a>
			</div>
		</div>
		<div class="publicTabNew">
			<ul class="tab" id="">
				<%=levelHtml %>
			</ul>
		</div>
	</div>
	<!--page start-->
	<div class="bt_page">
		<!--紧凑型车关注度排行-->
		<div class="line-box">
			<div class="title-con">
				<div class="title-box title-box2">
					<h4>  <%=levelFullName %>关注度排行 </h4>
					<span>7日排行</span>
					<div class="more_ul">
						<ul class="title-tab title-right">
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
