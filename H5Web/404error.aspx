<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404error.aspx.cs" Inherits="H5Web._404error" %>


<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>404</title>
	<!--#include file="~/ushtml/0000/m_subpage-354.shtml"-->
</head>
<body>
	<div class="mbt-page">
	<!--#include file="~/include/pd/2014/wap/00001/201406_wap_erji_common_header_Manual.shtml"-->
     <div class="b-return">
	    <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
	    <span>无效页面</span>
     </div>
		<div class="m-result-page-box m-result-page-fail m-result-page-multi-line">
			很抱歉，您访问的页面不存在。</div>
		<a class="m-btn-more-news m-btn-gray" href="/">返回首页</a>
	</div>
    <script type="text/javascript">
        var url = "http://m.yiche.com/";
    </script>
	<!-- 公用底 -->
<!-- bottom search start -->
<div class="footer-box">
    <!--#include file="~/ushtml/block/so/myiche20140620/mSearchBoxBottom.shtml"-->
    <div class="clear"></div>
    <!-- bottom search end -->
    <!--#include file="~/include/pd/2014/wap/00001/201407_wap_common_footer_Manual.shtml"-->
</div>
<!-- body 结束前内容 所有页面通用 -->
<script type="text/javascript">
    var backurl = "http://m.yiche.com/";
    if (typeof (backurl) != "undefined") {
        Bitauto.GoBacker.setDefaultURL(backurl);
    }
</script>
</body>
</html>
