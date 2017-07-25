<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaoZhiLvPaiHang.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageLevelV2.BaoZhiLvPaiHang" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>2017<%= LevelFullName %>保值率排行榜_保值率最高的<%= LevelFullName %>易车网</title>
     <meta name="Keywords" content="<%= LevelFullName %>保值率,<%= LevelFullName %>保值率排行榜,保值率最高的<%= LevelFullName %>" />
	<meta name="Description" content="什么<%= LevelFullName %>保值率最高？易车网为您整理2017保值率最高的<%= LevelFullName %>排行榜，看保值率最高<%= LevelFullName %>，尽在易车网保值率频道！" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <!--#include file="/ushtml/0000/yiche_2016_cube_qicheguanzhupaihang_style-1352.shtml"-->
</head>
<body>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--头部开始-->
    <header class="header-main special-header2">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-6 left-box"> 
                <div class="section-left">
                    <h1 class="logo"><a href="http://www.bitauto.com">易车yiche.com</a></h1>
                    <h2 class="title">汽车五年保值率排行榜</h2>
                </div>
            </div>
            <div class="col-xs-6 right-box">
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul class="list">
                            <%=LevelHtml %>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    </header>
    <!--头部结束-->

    <div class="container paihang-section">
        <div class="row head-box">
            <h3 class="title col-auto"><%= LevelFullName %>五年保值率排行榜</h3>
        </div>
        <div class="ol-list">
            <ol class="list"> 
                <%= SerialHtml %>
            </ol>
        </div>
    </div>
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <!-- 艾瑞 -->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
