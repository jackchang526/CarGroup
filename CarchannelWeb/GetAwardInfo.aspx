<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetAwardInfo.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.GetAwardInfo" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%="【"+ YearName + AwardName +"】"%>汽车奖项_评选-易车网</title>
    <meta name="Keywords" content="<%=YearName + AwardName%>,汽车大奖,奖项,评选" />
    <meta name="Description" content="<%=YearName + AwardName%>专题页汇集了<%=YearName + AwardName%>的获奖车型，并精准专业的介绍了<%=YearName + AwardName%>的简介、评选车型、优秀车型、历届获奖车型展示，是广大车友们了解最新获得<%=AwardName%>的车型展示首选网站！" />
    <!--#include file="~/ushtml/0000/2014_other_huojiangchexing150310-912.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
</head>
<body>
    <!--顶通-->
    <div class="bt_pageBox">
	<!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
    <div class="header_style style_bottomline">
			<div class="bitauto_logo">
			</div>
			<!--页头导航_yiche_LOGO开始-->
			<div class="yiche_logo">
				<a href="http://www.bitauto.com">易车网</a>
			</div>
			<!--页头导航_yiche_LOGO结束-->
			<div class="yiche_lanmu">
				<em>|</em><span>汽车奖项</span>
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
    </div>
    <%=AwardHtml %>
    <input id="hidYear" type="hidden" runat="server"/>
    <!-- 调用尾 -->
	<!--#include file="~/html/footer2014.shtml"-->
</body>
<script type="text/javascript">
    $(function () {
        var year = $("#hidYear").val();
        var yearTag = $(".title-tab").find("a");
        if (year) {
            $("#y_" + year + "").show();
            $.each(yearTag, function () {
                var yearContent = $(this).text();
                var yeatText = yearContent.substr(0, yearContent.length - 1);
                if (year == yeatText) {
                    $(this).parent().attr("class", "current");
                }
            });
        }
        else {
            yearTag.eq(0).parent().attr("class", "current");
            $("div[id^=y_]").eq(0).show();
        }



        $(".title-tab").find("a").on("click", function () {
            var yearContent = $(this).text();
            var intYear = yearContent.substr(0, yearContent.length - 1);
            $("#y_" + intYear + "").show();
            $("div[id^=y_]:not(#y_" + intYear + ")").hide();
            $(this).parent().attr("class", "current");
            $(this).parent().siblings().attr("class", "");
        });
    });
</script>
</html>
