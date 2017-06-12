﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agent.aspx.cs" Inherits="H5Web.V2.module.broker.Agent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>约我买车-易车</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/style.css"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/cheguwen.css"/>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript">
        var summary = { serialId: <%= SerialId %> };
    </script>
</head>
<body>
<div class="context_scroll" data-name="agent">
    <div class="context_scroll_box">
        <% if (Brokerid > 0)
           { %>
            <script type="text/javascript">
                //经纪人接口：获取经纪人信息
                document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=agent&method=brokerinfo&csid=<%= SerialId %>&type=0&brokerid=<%= Brokerid %>' type='text/javascript'%3E%3C/script%3E"));
            </script>
        <% } %>
        <%--</div>--%>
        <div class="h60"></div>
        <!--#include file="~/inc/footer_script_common.shtml"-->
    </div>
</div>
</body>
</html>