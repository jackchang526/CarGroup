<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZTDiscovery.aspx.cs" Inherits="H5Web.zhuanti.ZTDiscovery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>发现</title>
    <!-- #include file="/ushtml/0000/4th_2015-2_activity_style-1257.shtml" -->
</head>
<body class="activity">
    <div class="nav-header">
        <div class="nav-left">
            <a href="javascript:history.back();" class="lnk-go"></a>
            <i class="h-icon-<%=action %>"></i>
        </div>
        <div class="nav-right">
            <a class="lnk-duibi" href="/addchexingduibi/">车款对比</a>
        </div>
    </div>
    <div class="content">
        <% if (action == "news")
           { %>
        <!-- #include file="/include/pd/2016/h5/00001/201611_thefour_discovery_news_Manual.shtml" -->
        <% }
           else if (action == "video")
           { %>
        <!-- #include file="/include/pd/2016/h5/00001/201611_thefour_discovery_video_Manual.shtml" -->
        <% }
           else if (action == "activity")
           { %>
        <!-- #include file="/include/pd/2016/h5/00001/201611_thefour_discovery_activity_Manual.shtml" -->
        <% } %>
    </div>
</body>
</html>
