<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DaoGou.aspx.cs" Inherits="WirelessWeb.DaoGou" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <title>导购详情</title>

    <!--#include file="~/ushtml/0000/myiche2016_cube_daogouxiangqing-1187.shtml"-->

</head>
<body>
<!-- header start -->

<% switch (Topic)
   {
       case "1":
%>
    <div class="daogou-box">
        <img src="http://image.bitautoimg.com/uimg/mbt2016/images/youhaoche_top_img1.jpg"/>
        <a id="gobackElm" href="javascript:;" class="btn-return"></a>
        <div class="dg-sub-tt">有好车</div>
        <h1>10万元左右大空间SUV，总有一款适合你！</h1>
    </div>
    <%
           break;
       case "2":
    %>
    <div class="daogou-box">
        <img src="http://image.bitautoimg.com/uimg/mbt2016/images/youhaoche_top_img2.jpg"/>
        <a id="gobackElm" href="javascript:;" class="btn-return"></a>
        <div class="dg-sub-tt">有好车</div>
        <h1>
            低价也能风驰电掣！6款杀进7.1秒的5门小钢炮
        </h1>
    </div>
    <%
           break;
       case "3":
    %>
    <div class="daogou-box">
        <img src="http://image.bitautoimg.com/uimg/mbt2016/images/youhaoche_top_img3.jpg"/>
        <a id="gobackElm" href="javascript:;" class="btn-return"></a>
        <div class="dg-sub-tt">有好车</div>
        <h1>谁说小车不舒适？10款大空间的紧凑型轿车推荐</h1>
    </div>
    <%
           break;
       case "4":
    %>
    <div class="daogou-box">
        <img src="http://image.bitautoimg.com/uimg/mbt2016/images/youhaoche_top_img4.jpg"/>
        <a id="gobackElm" href="javascript:;" class="btn-return"></a>
        <div class="dg-sub-tt">有好车</div>
        <h1>
            外国车太贵？看看这5款高性价比合资SUV
        </h1>
    </div>
<%
           break;
   } %>
<!-- header end -->

<!-- card start -->
<% foreach (var entity in DaoGouEntities)
   { %>
    <div class="daogou-card b-shadow">
        <div class="car-box">
            <a href="/<%= entity.AllSpell %>/" target="_blank" data-channelid="<%=entity.ChannelId %>">
                <img src="<%= entity.ImageUrl %>"/>
            </a>
            <h2>
                <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%= entity.BrandId %>_55.png"/>
                <a href="/<%= entity.AllSpell %>/" target="_blank" data-channelid="<%=entity.ChannelId %>"><%= entity.BrandName %> <%= entity.ShowName %></a>
            </h2>
            <strong><%= entity.ReferPrice %></strong>
        </div>

        <div class="car-info">
            <div class="car-info-arrow-line">
                <div class="car-info-arrow"></div>
            </div>
            <ul <%= Topic == "1"||Topic == "3" ? "class='two-line'" : "" %>>
                <%
                    var count = 1;
                    foreach (var item in entity.DataDescription.Split(';'))
                    {
                        var kvStr = item.Split(':');
                %>
                    <li>
                        <span class="bg<%= count %>"><%= kvStr[0] %></span><%= kvStr[1].Trim() %>
                    </li>
                <%
                        count++;
                    } %>
            </ul>
            <h6>推荐理由</h6>
            <p><%= entity.Description %></p>
        </div>
    </div>
<% } %>
<!-- card end -->

<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/bsearch/mobilesug201506/showsearchbox.js"></script>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
<!-- footer start -->
<div class="footer15">
    <!--搜索框-->
    <!--#include file="~/html/footersearch.shtml"-->
    <!--#include file="~/html/footerV3.shtml"-->
</div>
<!-- footer end -->
</body>

</html>