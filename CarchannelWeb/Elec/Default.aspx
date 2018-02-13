<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Elec.Default" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新能源</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta name="Keywords" content=" " />
    <meta name="Description" content="  " />
    <meta http-equiv="mobile-agent" content="format=html5; url=http://car.m.yiche.com/elec/" />
    <link rel="alternate" media="only screen and (max-width: 640px)" href=" http://car.m.yiche.com/elec/" />
    <link rel="canonical" href="http://car.bitauto.com/elec/" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <link rel="stylesheet" href="http://192.168.0.10:8888/a-ued产出物/01-网站产品/102-2016网站改版/css/common.css">

    <link rel="stylesheet" href="http://192.168.0.10:8888/a-ued产出物/01-网站产品/102-2016网站改版/css/_pic-list.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/a-ued产出物/01-网站产品/102-2016网站改版/css/_txt-list.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/a-ued产出物/01-网站产品/102-2016网站改版/css/_screen.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/a-ued产出物/01-网站产品/102-2016网站改版/新能源/style/style.css">
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--顶通-->
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <!--#   include file="~/include/pd/2016/common/00001/201609_SUV_head_Manual.shtml" -->
    <header class="header-main special-header2">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <div class="section-left">
                    <h1 class="logo"><a href="yiche.com">易车yiche.com</a></h1>
                    <h2 class="title">新能源</h2>
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
            <nav class="header-main-nav type-1"></nav>
        </div>
    </header>
    <div class="container section-gap">
        <ul class="list list-car-category">
            <li><span><a href="">热门</a></span></li>
            <li><span><a href="">SUV</a></span></li>
            <li class="active"><span><a href="">超长里程</a></span></li>
            <li><span><a href="">5万以下</a></span></li>
            <li><span><a href="">5-8万</a></span></li>
            <li><span><a href="">8-12万</a></span></li>
            <li><span><a href="">12-18万</a></span></li>
            <li><span><a href="">18-25万</a></span></li>
            <li><span><a href="">25-40万</a></span></li>
            <li><span><a href="">25-80万</a></span></li>
            <li><span><a href="">80万以上</a></span></li>
        </ul>
        <div class="row slider-frag">
            <div class="slide-box slide-1200" id="slideTest5">
                <div class="slide-box-bg">
                    <ul class="slide-box-big">
                        <!--#include file="~/include/pd/2016/car/00001/201801_pc_newenergy_banner_Manual.shtml" -->
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <div class="container main">
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="screen-frag section-gap">
                        <div class="section-header header2">
                            <div class="box">
                                <h2>挑选新能源车</h2>
                            </div>
                            <div class="more">
                                <a href="/gaojixuanche/">更多&gt;&gt;</a>
                            </div>
                        </div>
                        <div class="spl-layout1">
                            <div class="screen-group">
                                <dl class="list">
                                    <dt>能源：</dt>
                                    <dd>
                                        <label>
                                            <input type="radio" id="f_0">不限</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="f_16">纯电动</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="f_128">插电混动</label></dd>
                                </dl>
                            </div>
                            <div class="screen-group">
                                <dl class="list">
                                    <dt>续航：</dt>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="bl_1">0-150km</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="bl_2">150-200km</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="bl_3">200-300km</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="bl_5">300-400km</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="bl_6">400km以上</label></dd>
                                </dl>
                            </div>
                            <div class="screen-group">
                                <dl class="list">
                                    <dt>价格：</dt>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_0">不限</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_3">8-12万</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_4">12-18万</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_5">18-25万</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_6">25-40万</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_7">40-80万</label></dd>
                                    <dd>
                                        <label>
                                            <input type="radio" id="p_8">80万以上</label></dd>
                                </dl>
                            </div>
                            <div class="screen-group">
                                <dl class="list">
                                    <dt>级别：</dt>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_1">微型车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_2">小型车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_3">紧凑型车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_5">中型车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_4">中大型车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_6">豪华车</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_8">SUV</label></dd>
                                    <dd>
                                        <label>
                                            <input type="checkbox" id="l_9">跑车</label></dd>
                                </dl>
                            </div>
                        </div>
                        <div class="spl-layout2">
                            <a class="btn btn-primary2 btn-sm" href="#">确定</a>
                            <span>为您找到<em>18</em>个车型，<em>107</em>个车款</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--#include file="~/htmlV2/footer2016.shtml"-->
</body>
</html>
