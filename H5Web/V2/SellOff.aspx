<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellOff.aspx.cs" Inherits="H5Web.V2.SellOff" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>【<%= BaseSerialEntity.SeoName %>】优惠购车-易车</title>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/style.css"/>
</head>
<body>
<div class="context_scroll" data-name="selloff">
    <div class="context_scroll_box">
        <!--小头开始-->
        <!--小头结束-->
        <!--内容开始-->

        <!--卡片开始 惠买车-->
        <% if (!IsBaoXiao)
           { %>
            <script type="text/javascript">
                //惠买车接口：如果是包销车则不显示惠买车数据
                document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=huimaiche&method=youhuigouce&csid=<%= SerialId %>&cityId=<%= CityId %>' type='text/javascript'%3E%3C/script%3E"));
            </script>
        <% } %>
        <!--卡片结束-->

        <!--卡片开始 易车商城-->
        <script type="text/javascript">
            document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=yichemall&method=youhuigouce&csid=<%= SerialId %>&cityId=<%= CityId %>' type='text/javascript'%3E%3C/script%3E"));
        </script>
        <!--卡片结束-->

        <!--卡片开始 易车慧-->
        <script type="text/javascript">
            document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=market&method=youhuigouce&csid=<%= SerialId %>&cityId=<%= CityId %>' type='text/javascript'%3E%3C/script%3E"));
        </script>
        <!--卡片结束-->

        <!--卡片开始 贷款-->
        <% if (!IsBaoXiao)
           { %>
            <script type="text/javascript">
                //贷款接口：如果是包销车则不显示贷款数据
                document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=daikuan&method=youhuigouce&csid=<%= SerialId %>&cityId=<%= CityId %>' type='text/javascript'%3E%3C/script%3E"));
            </script>
        <% } %>
        <!--卡片结束-->

        <!--卡片开始 二手车-->
        <script type="text/javascript">
            //二手车接口：二手车优惠购车
            document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=ershouche&method=youhuigouce&csid=<%= SerialId %>&cityId=<%= CityId %>' type='text/javascript'%3E%3C/script%3E"));
        </script>
        <!--卡片结束-->

        <%--广告区域--%>

        <div class="box yishubox">
            <a href="http://gouche.m.yiche.com/home/YiShuBang/?csId=<%= SerialId %>">
                <img src="http://img1.bitautoimg.com/uimg/carservice/2015/web/images/gg-690x216.png"/>
            </a>
        </div>

        <!--内容结束-->
        <div class="h60"></div>
        <!--#include file="~/inc/footer_script_common.shtml"-->
    </div>
</div>
</body>
</html>