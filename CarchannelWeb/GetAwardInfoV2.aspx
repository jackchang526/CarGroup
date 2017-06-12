<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetAwardInfoV2.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.GetAwardInfoV2" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%="【"+ YearName + AwardName +"】"%>汽车奖项_评选-易车网</title>
      <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta name="Keywords" content="<%=YearName + AwardName%>,汽车大奖,奖项,评选" />
    <meta name="Description" content="<%=YearName + AwardName%>专题页汇集了<%=YearName + AwardName%>的获奖车型，并精准专业的介绍了<%=YearName + AwardName%>的简介、评选车型、优秀车型、历届获奖车型展示，是广大车友们了解最新获得<%=AwardName%>的车型展示首选网站！" />
    <!--#include file="~/ushtml/0000/yiche_2016_jiangxiang_style-1255.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
</head>
<body>
    <!--#include file="~/htmlV2/header2016.shtml"--> 
    <header class="header-main special-header2">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <div class="section-left">
                    <h1 class="logo"><a href="http://www.yiche.com">汽车</a></h1>
                    <h2 class="title">汽车奖项</h2>
                </div>
            </div>
            <div class="col-xs-8 right-box">
                <ul class="list keyword-list">
                    <li class="dt">热门搜索：</li>
                    <li><a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a></li>
                    <li><a href="http://v.bitauto.com/ycjm.shtml" target="_blank">原创节目</a></li>
                    <li><a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></li>
                </ul>
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
    </header>
    <%=AwardHtml %>
    <input id="hidYear" type="hidden" runat="server"/>
    <!-- 调用尾 -->
    <script type="text/javascript">
        (function (global) {
            global.SidebarConfig = {
                activate: true
            };
        })(window);
    </script>
	<!--#include file="~/htmlV2/footer2016.shtml"-->
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
