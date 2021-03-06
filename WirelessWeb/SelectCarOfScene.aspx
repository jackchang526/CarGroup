﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCarOfScene.aspx.cs" Inherits="WirelessWeb.SelectCarOfScene" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【汽车标志_汽车品牌大全_<%=DateTime.Now.ToString("yyyy") %>年热门车型推荐】-手机易车网</title>
	<meta name="keywords" content="汽车大全,汽车标志,车型大全,<%=DateTime.Now.ToString("yyyy") %>年热门车型，易车网" />
	<meta name="description" content="手机易车网选车频道，为您提供各种汽车品牌型号及报价信息，主要涵盖：“汽车品牌、汽车标志、汽车报价、<%=DateTime.Now.ToString("yyyy") %>年热门车型推荐等,是您选车购车的第一网络平台." />
	<!--#include file="~/ushtml/0000/myiche2016_cube_qingjingxuanche-1184.shtml"-->
</head>
<body>
    <div class="mbt-page">
		<!-- header start -->
		<div class="op-nav">
			<a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

			<div class="tt-name">
				<a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1>选车</h1>
			</div>
			<!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
		</div>
		<!-- header end --> 
		<div class="brandfilter">
			<!-- 标签切换 start -->
			<div class="first-tags">
				<ul>
					<li id="xc_brand"><a href="/brandlist.html" data-channelid="27.150.1436"><span>按品牌</span></a></li>
					<li id="xc_condition"><a href="/" data-channelid="27.150.1435"><span>按条件</span></a></li>
                    <li class="current tags-new""><a href="/brandlist.html" data-channelid="27.150.1416"><span>按情景<b><i>NEW<i></i></i></b></span></a></li>
				</ul>
			</div>
			<!-- 标签切换 end -->
            <div class="qingj-tips">您有没有特殊的用车情景？可根据您的需求来选配置。</div>
            <div class="qingj-box-outer">
                <div class="qingj-box">
                    <div class="qingj-left qingj-left-1"><span>路况·天气</span></div>
                    <div class="qingj-right">
                        <ul>
                            <li><a href="/selectscenecar/?word=1&more=215_216_211_184_191_143" data-channelid="27.150.1418">夏天雨水多</a></li>
                            <li><a href="/selectscenecar/?word=2&dt=4,8,16&more=184_183_182_191_211" data-channelid="27.150.1419">冬天雪很多</a></li>
                            <li><a href="/selectscenecar/?word=3&more=245_233_177_211" data-channelid="27.150.1420">昼夜温差大</a></li>
                            <li><a href="/selectscenecar/?word=4&dt=4,8,16&more=201_188_183_190" data-channelid="27.150.1432">我家山路多</a></li>
                            <li><a href="/selectscenecar/?word=5&dt=4,8,16&l=8&more=131_132_184" data-channelid="27.150.1433">坑洼路况差</a></li>
                            <li><a href="/selectscenecar/?word=6&more=231_177_191_194_197_204" data-channelid="27.150.1434">总跑郊区</a></li>
                        </ul>
                    </div>
                </div>

                <div class="qingj-box">
                    <div class="qingj-left qingj-left-2"><span>性格·技术</span></div>
                    <div class="qingj-right">
                        <ul>
                            <li><a href="/selectscenecar/?word=7&more=197_191_183_184_228" data-channelid="27.150.1426">遇事儿就慌</a></li>
                            <li><a href="/selectscenecar/?word=8&more=102_187_148_143_177_184" data-channelid="27.150.1424">我有路怒症</a></li>
                            <li><a href="/selectscenecar/?word=9&more=200_191_197_227_190" data-channelid="27.150.1422">绝对新手</a></li>
                            <li><a href="/selectscenecar/?word=10&more=102_143_163_187_184" data-channelid="27.150.1423">开的就是猛</a></li>
                            <li><a href="/selectscenecar/?word=11&more=183_190_200_210_227" data-channelid="27.150.1421">女司机</a></li>
                        </ul>
                    </div>
                </div>

                <div class="qingj-box">
                    <div class="qingj-left qingj-left-3"><span>身材·成员</span></div>
                    <div class="qingj-right">
                        <ul>
                            <li><a href="/selectscenecar/?word=12&t=8&more=131_132_204_236" data-channelid="27.150.1427">家有大胖子</a></li>
                            <li><a href="/selectscenecar/?word=13&t=8&more=131_132_204_236" data-channelid="27.150.1428">身高185</a></li>
                            <li><a href="/selectscenecar/?word=14&more=210_236_235_249" data-channelid="27.150.1429">家里司机多</a></li>
                            <li><a href="/selectscenecar/?word=15&more=180_181_209_249" data-channelid="27.150.1431">家里有孩子</a></li>
                            <li><a href="/selectscenecar/?word=16&l=7,8&more=266_246_204_205_206" data-channelid="27.150.1430">家里人多</a></li>
                        </ul>
                    </div>
                </div>
            </div>

		<div class="footer15">
		<!--搜索框-->
		<!--#include file="~/html/footersearch.shtml"-->
		<div class="breadcrumb">
			<div class="breadcrumb-box">
				<a href="http://m.yiche.com/">首页</a> &gt;  <a href="/scenelist.html">情景选车</a>
			</div>
		</div>
		<!--#include file="~/html/footerV3.shtml"-->
	</div>
	</div>
    </div>
</body>
</html>
