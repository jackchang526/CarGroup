<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCarCompare.aspx.cs" Inherits="H5Web.V6.AddCarCompare" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>对比选车</title>

    <!--#include file="~/ushtml/0000/4th_2015-2_chexinduibi_style-1120.shtml"-->
</head>
<body class="bg-gray">
    <div id="container">
        <header>
            <h2>车款对比</h2>
        </header>
        <div class="con_top_bg"></div>
        <div class="car-pk">
            <a href="javascript:;" data-action="changecar" data-index="1">添加车款</a>
            <i class="icon_pk"></i>
            <a href="javascript:;" data-action="changecar" data-index="2">添加车款</a>
        </div>
        <div class="wrap20 mt30 bt30">
            <a href="###" id="buttoncompare" class="button gray">两车对决</a>
        </div>
        <div id="master_container" style="z-index: 888888; display: none" class="brandlayer mthead">
            <!--#include file="~/inc/compareCarTemplate.html"-->
        </div>
    </div>
    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="/Scripts/carcompare/iscroll.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/underscore.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/model.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/note.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/rightswipe.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/brand.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/changecar.v1.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/carcompare/iscroll.v1.js,carchannel/h5/js/carcompare/underscore.v1.js,carchannel/h5/js/carcompare/model.v1.js,carchannel/h5/js/carcompare/note.v1.js,carchannel/h5/js/carcompare/rightswipe.v1.js,carchannel/h5/js/carcompare/brand.v1.js,carchannel/h5/js/carcompare/changecar.v1.js"></script>
    <script type="text/javascript">
        $(function(){
            changecar.init({serialId:<%= SerialId %>,carId:<%= CarId %>,callback: changecar.SelectCarCallback});
        });
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
    <script type="text/javascript">
        var summary = { serialId: 0,IsNeedShare:true  };
        var XCWebLogCollector = { area: '201' };
        if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
            XCWebLogCollector.area = bit_locationInfo.cityId;
        }
        if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
            XCWebLogCollector.serial = summary.serialId;
        }
    </script>
    <script type="text/javascript">
        var __zp_tag_params = {
            modelId: summary.serialId,
            carId: 0
        };
    </script>
    <script type="text/javascript">
        var forweixinObj = {
            debug: false,
            appId: 'wx0c56521d4263f190',
            jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
        }
        var pageShareContent = {
            title: '【汽车大全_热门车型大全】-易车网',
            keywords: '掌上车型手册，360°解读你所关注的车型。',
            desc: '掌上车型手册，360°解读你所关注的车型。',
            link: 'http://car.h5.yiche.com/?',
            imgUrl: 'http://image.bitautoimg.com/carchannel/pic/yiche100.jpg'
        }
    </script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/H5Stat.js?20150716"></script>
</body>
</html>
