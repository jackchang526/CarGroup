<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="barcomapre.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Iframe.barcomapre" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>右侧边栏</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->

    <!--#include file="/ushtml/0000/yiche_2016_navtool_right-1192.shtml"-->
</head>
<body>
    <div class="navtool-divbar">
        <div class="sele-box clearfix">
            <div class="brand-form" id="rightbar-brand-form">
                <span class="default activ">请选择品牌</span>
                <a href="javascript:;" class="jt"></a>
            </div>
        </div>
        <ul class="pk" id="rightbar-comparecar"></ul>
        <div class="btn-pk"><a class="btn btn-primary" id="btn-pk" href="javascript:;">开始对比</a></div>
        <div class="trash"><a href="javascript:;" id="trashcompare"><i></i>清空车款</a></div>
        <div class="more-layer" id="alert-layer" style="display:none;"></div>
    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/carSelectSimple.min.js?v=20161215"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/cooikehelper.min.js?v=201612151617"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/rightbarcarcompare.min.js?v=20161215"></script>
    <script type="text/javascript">
        document.domain = 'bitauto.com';
        $(function () {
            top.window.Bitauto.iMediator.clear('comparecar');
            barCompare = GetBarCompare();
            top.window.Bitauto.iMediator.subscribe('comparecar', SubAddCar);
        });
    </script>
</body>
</html>