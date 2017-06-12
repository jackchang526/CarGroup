<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialForBrandList.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageList.SerialForBrandList" %>

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
    <!--
    var tagIframe = null;
    var currentTagId = 2; 	//当前页的标签ID
    -->
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
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingdaquan-770.shtml"-->
    <%} %>
    <%--<link rel="stylesheet" type="text/css" href="http://image.bitautoimg.com/uimg/carazbanner/css/carazbanner.css" />--%>
    <script language="javascript" type="text/javascript" charset="gb2312" src="http://image.bitautoimg.com/uimg/carazbanner/js/carazbanner.js"></script>
    <script type="text/javascript" charset="gb2312" src="http://www.bitauto.com/themes/09gq/09gq_adjs.js"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->

    <!--公共_LOGO热搜通栏-->
    <div class="header_style">
        <div class="bitauto_logo">
        </div>
        <!--页头导航_yiche_LOGO开始-->
        <div class="yiche_logo">
            <a href="http://www.bitauto.com">易车网</a>
        </div>
        <!--页头导航_yiche_LOGO结束-->
        <div class="yiche_lanmu">
            <em>|</em><span>品牌大全</span>
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
    <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript">
            $(".header_style,.nav_small,.bit_top990").hide();
    </script>
    <%} %>
    <!--主导航-->
    <div id="treeFiexdNav" class="publicTabNew">
        <ul class="tab" id="ulForTempClickStat">
            <li id="accordMake"><a href="/brandlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按品牌</a></li>
            <li id="accordStyle"><a href="/charlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按车型</a></li>
            <li id="accordPrice"><a href="/price/<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按价格</a></li>
            <li id="accordLevel"><a href="/levellist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按级别</a></li>
            <li id="accordCountry"><a href="/countrylist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按国别</a></li>
            <li id="accordUse"><a href="/functionlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按用途</a></li>
        </ul>
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
                document.getElementById("accordStyle").className = "current";
                break;
            case 2:
                document.getElementById("accordMake").className = "current";
                break;
            case 3:
                document.getElementById("accordPrice").className = "current";
                break;
            case 4:
                document.getElementById("accordLevel").className = "current";
                break;
            case 5:
                document.getElementById("accordCountry").className = "current";
                break;
            case 6:
                document.getElementById("accordUse").className = "current";
                break;
        }
    </script>
    <div class="bt_page">
        <div id="theanchor">
        </div>
        <asp:Literal ID="lrSerialForBrand" EnableViewState="false" runat="server" />
        <!-- 调用尾 -->
        <!--# include file="~/html/friendLink.shtml"-->
    </div>
    <!--#include file="~/html/footer2014.shtml"-->
    <!--提意见浮层-->
    <!--# include file="~/include/pd/2014/00001/201701_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript">
        $(".foot_box").hide();
    </script>
    <iframe id="iframeC" name="iframeC" src="" width="0" height="0" style="display:none;" ></iframe>

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