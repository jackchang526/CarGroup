﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialForBrandList.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageListV2.SerialForBrandList" %>

<% Response.ContentType = "text/html"; %>
<%if (_isNoHeader) { Response.Write(lrSerialForBrand.Text.Replace("href=\"/", "href=\"http://car.bitauto.com/")); }
    else
    {%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【汽车大全-汽车标志】-易车网(BitAuto.com)车型频道</title>
    <meta name="Keywords" content="汽车大全,汽车标志,车型大全,汽车标志列表" />
    <meta name="Description" content="汽车大全：易车网(BitAuto.com) 汽车车型为您提供各种汽车品牌型号信息。包括汽车报价、汽车标志、汽车图片、汽车经销商、汽车油耗、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />
    <script type="text/javascript">
        var tagIframe = null;
        var currentTagId = 2; 	//当前页的标签ID
    </script>
    <% 
        // 2345 合作 2017-03-23
        string wt = Request.QueryString["WT.mc_id"];
        if (wt == "2345nyif")
        { %>
    <!--#include file="/ushtml/0000/yiche_2014_cube_qichedaquan-1329.shtml"-->
    <%}
        else
        { %>
    <!--#include file="~/ushtml/0000/yiche_2016_cube_qichedaquan_style-1351.shtml"-->
    <%} %>
    <%--<link rel="stylesheet" type="text/css" href="http://image.bitautoimg.com/uimg/carazbanner/css/carazbanner.css" />--%>
    <script language="javascript" type="text/javascript" charset="gb2312" src="http://image.bitautoimg.com/uimg/carazbanner/js/carazbanner.js"></script>
    <script type="text/javascript" charset="gb2312" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->

    <!--头部开始-->
    <header class="header-main special-header2">
    <div class="container section-layout top" id="topBox">
        <div class="col-xs-4 left-box">
            <div class="section-left">
                <h1 class="logo"><a href="http://www.bitauto.com">易车yiche.com</a></h1>
                <h2 class="title">品牌大全</h2>
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
    <div id="navBox">
        <nav class="header-main-nav">
            <div class="container">
                <div class="col-auto left secondary">
                    <ul class="list">
                        <li id="accordMake" class="active"><a href="/brandlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按品牌</a></li>
                        <li id="accordStyle"><a href="/charlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按车型</a></li>
                        <li id="accordPrice"><a href="/price/<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按价格</a></li>
                        <li id="accordLevel"><a href="/levellist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按级别</a></li>
                        <li id="accordCountry"><a href="/countrylist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按国别</a></li>
                        <li id="accordUse"><a href="/functionlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按用途</a></li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
    <script type="text/javascript">
        document.getElementById("accordMake").className = "";
        document.getElementById("accordStyle").className = "";
        document.getElementById("accordPrice").className = "";
        document.getElementById("accordLevel").className = "";
        document.getElementById("accordCountry").className = "";
        document.getElementById("accordUse").className = "";
        switch (currentTagId) {
            case 1:
                document.getElementById("accordStyle").className = "active";
                break;
            case 2:
                document.getElementById("accordMake").className = "active";
                break;
            case 3:
                document.getElementById("accordPrice").className = "active";
                break;
            case 4:
                document.getElementById("accordLevel").className = "active";
                break;
            case 5:
                document.getElementById("accordCountry").className = "active";
                break;
            case 6:
                document.getElementById("accordUse").className = "active";
                break;
        }
    </script>
</header>
    <!--头部结束-->

    <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript">
        $("#topBox,.bit_top990").hide();
    </script>
    <%} %>
    <!--品牌大全-->
    <div class="container brandlist-section">
        <div id="theanchor">
        </div>
        <asp:Literal ID="lrSerialForBrand" EnableViewState="false" runat="server" />
    </div>
    <!--/品牌大全-->
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <!--提意见浮层-->
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript">
        $(".foot_box, .navtool-fixed-right").hide();
    </script>
    <iframe id="iframeC" name="iframeC" src="" width="0" height="0" style="display: none;"></iframe>

    <script type="text/javascript">
        function sethash() {
            hashH = document.documentElement.scrollHeight;
            urlC = "//www.2345.com/car/brand_iframe_guodu.htm";
            document.getElementById("iframeC").src = urlC + "#" + hashH;
        }
        window.onload = sethash;
    </script>
    <%} %>
</body>
</html>
<%} %>