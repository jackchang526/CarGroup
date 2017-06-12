<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_masterbrand.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.CarTree.CX_masterbrand" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=_MasterBrandName %>汽车】<%=_MasterBrandName %>汽车报价_介绍_全部<%=_SerialCount %>款车型-易车网
	</title>
	<meta name="Keywords" content="<%=_MasterBrandName %>,<%=_MasterBrandName %>汽车<%=_ContainsBrandName %>,易车网" />
	<meta name="Description" content="<%=_MasterBrandName %>汽车:易车网车型频道为您提供<%=_MasterBrandName %>汽车介绍,<%=_MasterBrandName %>汽车全部<%=_SerialCount %>款车型最新汽车报价/价格,图片,参数配置,经销商,导购,评测,图解,行情,更多<%=_MasterBrandName %>汽车信息尽在易车网" />
	<!--#include file="~/ushtml/0000/yiche_2014_cube_car-685.shtml" -->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
	<!--顶通-->
	<!--#include file="~/html/tree_header2014.shtml"-->
	<%--<%=NavbarHtml%>--%>
	<!--#include file="~/include/pd/2014/common/00001/201402_shuxing_nav_chexing_Manual.shtml" -->
	<div class="tree_wrap_box">
		<!--左侧树形-->
		<div id="leftTreeBox" class="treeBoxv1">
		</div>
		<!--右侧内容-->
		<div class="treeMainv1">
			<%=NavPathHtml%>
			<!--主品牌介绍-->
			<%--<%=_MasterIntroduce%>--%>
			<!-- 子品牌及品牌列表 -->
			<%=_SerialListShow%>
			<script type="text/javascript">
			var params = {};
			params.tagtype = "chexing";
			params.pagetype = "masterbrand";
			params.objid = <%=_MasterBrandId %>;
			</script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/lefttreenew.js?v=2015121715"></script>
			<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/CommonFunction.js"></script>
			<!-- footer -->
			<script type="text/javascript">
				var CarCommonBSID = '<%= _MasterBrandId.ToString() %>';
				function all_func() {
					try {
						tabs("divCsLevelIndex", "divCsLevel", null, true);
					}
					catch (err) { }
				}
				addLoadEvent(all_func);
				// 如果没有选中的 选中全部
				var firstTab;
				var hasCurrentTab = false;
				var tab_headCheck = document.getElementById("divCsLevelIndex");
				if (tab_headCheck) {
					var alis = tab_headCheck.getElementsByTagName("li");
					for (var i = 0; i < alis.length; i++) {
						if (i == 0)
						{ firstTab = alis[0]; }
						if (alis[i].className == "current")
						{ hasCurrentTab = true; break; }
					}
					if (!hasCurrentTab) {
						tabsRemove(0, "divCsLevelIndex", "divCsLevel", null);
					}
				}
			</script>
 			<!--#include file="~/html/treefooter2014.shtml"-->
		</div>
	</div>
	<!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
