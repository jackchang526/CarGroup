<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompareRank.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerial.CsCompareRank" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=m_CSE.Cs_SeoName.Trim()%>排行榜-<%=m_CSE.Cs_SeoName.Trim()%>关注排行榜】_<%=m_CSE.MasterName.Trim()%><%=m_CSE.Cs_SeoName.Trim()%>-易车网
	</title>
	<meta name="keywords" content="<%=m_CSE.Cs_SeoName.Trim()%>排行榜,<%=m_CSE.Cs_SeoName.Trim()%>关注排行榜,<%=m_CSE.MasterName.Trim()%><%=m_CSE.Cs_SeoName.Trim()%>" />
	<meta name="description" content="<%=m_CSE.Cs_SeoName.Trim()%>排行榜:易车网车型频道为您提供最权威的<%=m_CSE.MasterName.Trim()%><%=m_CSE.Cs_SeoName.Trim()%>排行榜、<%=m_CSE.MasterName.Trim()%><%=m_CSE.Cs_SeoName.Trim()%>网民关注排行榜、<%=m_CSE.Cs_SeoName.Trim()%>最新行情资讯、<%=m_CSE.Cs_SeoName.Trim()%>优惠、<%=m_CSE.Cs_SeoName.Trim()%>经销商降价信息、网友点评讨论等。" />
 	<!--#include file="~/ushtml/0000/yiche_2014_cube_paihangbang-747.shtml"-->
</head>
<body>
	<!-- 头 -->
	<!--#include file="~/html/header2014.shtml"-->
	<!--smenu start-->
	<!--a_d start-->
	<div class="bt_ad">
		<!-- AD New Dec.31.2011 -->
		<ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
	</div>
	<!--a_d end-->
	<%= CsHeadHTML %>
	<div class="bt_page">
		<!-- 全国排名 -->
		<div class="line-box">
			<%=PvString %>
		</div>
		<!--全国对比列表  -->
		<div class="line-box">
		<%=CompareRank%>
		</div>
		<!-- AD -->
		<ins id="div_aa9a54b3-b482-4115-809a-c5cd25bf4eb4" type="ad_play" adplay_ip="" adplay_areaname=""
			adplay_cityname="" adplay_brandid="<%= serialId %>" adplay_brandname="" adplay_brandtype=""
			adplay_blockcode="aa9a54b3-b482-4115-809a-c5cd25bf4eb4"></ins>
	</div>
	<script type="text/javascript" language="javascript">
		var CarCommonCSID = '<%= serialId.ToString() %>';
	</script>
	<!-- 尾 -->
	<!--#include file="~/html/footer2014.shtml"-->
	<%if (serialId == 2370 || serialId == 2608 || serialId == 3398 || serialId == 3023 || serialId == 2388)
   { %>
	<!--百度热力图-->
	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<%} %>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
