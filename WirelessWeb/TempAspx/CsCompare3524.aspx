﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompare3524.aspx.cs" Inherits="WirelessWeb.TempAspx.CsCompare3524" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0"/>
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
	<title>【<%=se.SeoName%>配置】<%=se.Brand.SeoName%><%=se.Name%>详细参数介绍-手机易车网</title>
	<meta name="keywords" content="<%=se.SeoName%>参数,<%=se.SeoName%>配置,<%=se.Brand.SeoName%><%=se.Name%>" />
	<meta name="description" content="<%=se.SeoName%>配置:<%=se.Brand.SeoName%><%=se.Name%>频道为您提供<%=se.SeoName%>综合配置信息,包括<%=se.SeoName%>安全装备,<%=se.SeoName%>操控配置,<%=se.SeoName%>内饰配置,<%=se.SeoName%>参数性能,<%=se.SeoName%>车型资料大全等,查<%=se.SeoName%>参数配置,就上易车网" />
	<!--#include file="~/ushtml/0000/myiche2016_cube_canshupeizhijianhua-1335.shtml"-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js"></script>
</head>
<body>
    <!-- header start -->
    <div class="op-nav">
        <a id="gobackElm" href="javascript:window.history.go(-1);" class="btn-return">返回</a>
        <div class="tt-name">
            <a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%= se.ShowName %></h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
    <div class="op-nav-mark" style="display: none;"></div>
    <!-- header end -->
    <!-- 互联互通 start -->
    <%=CsHeadHTML %>
    <!-- 互联互通 end -->

<div class="peizhi-box">
	
	<div class="md-jibenxinxi" id="jibenxinxi"></div>

	<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz1.png" />
	<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz2.png" />

	<div class="pd20">
		<div class="small-tt">车身颜色</div>
		<ul class="color-list">
			<li style="background:#000000"></li>
			<li style="background:#625442"></li>
			<li style="background:#AE1D32"></li>
			<li style="background:#B58206"></li>
			<li style="background:#E0E0E2"></li>
			<li style="background:#FFFFFF"></li>
		</ul>
		
		<div class="small-tt">内饰颜色</div>
		<ul class="color-list">
			<li style="background:#000000"></li>
			<li style="background:#A68B69"></li>
		</ul>

	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="fadongji"></div>
			<div class="big-tt">
				1.5T涡轮增压发动机
			</div>
			<ul class="fadongji-list">
				<li>
					<dl>
						<dt><strong>112</strong>kw</dt>
						<dd>最大功率</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>152</strong>ps</dt>
						<dd>最大马力</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>205</strong>nm</dt>
						<dd>最大扭矩</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>9.65</strong>秒</dt>
						<dd>百公里加速</dd>
					</dl>
				</li>
			</ul>

			<div class="big-tt">
				1.6L自然吸气发动机
			</div>
			<ul class="fadongji-list">
				<li>
					<dl>
						<dt><strong>93</strong>kw</dt>
						<dd>最大功率</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>126</strong>ps</dt>
						<dd>最大马力</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>160</strong>nm</dt>
						<dd>最大扭矩</dd>
					</dl>
				</li>
				<li>
					<dl>
						<dt><strong>12.6</strong>秒</dt>
						<dd>百公里加速</dd>
					</dl>
				</li>
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>发动机启动技术</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="luntai"></div>
			<div class="big-tt">
				轮胎
			</div>
			<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz3.png" style="margin-top:10px;" />
			<div class="tt tt-blue">
				全系标配
			</div>
			<ul class="ul-list">
				<li>胎压监测装置</li>
				<li>非全尺寸备胎</li>
				<li>铝合金轮毂</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="xingchefuzhu"></div>
			<div class="big-tt">
				行车辅助
			</div>
			<div class="tt tt-blue">
				全系标配
			</div>
			
			<ul class="icon-list">
				<li>
					<span class="ico-abs"></span>
					<dl>
						<dt>刹车防抱死</dt>
						<dd>ABS</dd>
					</dl>
				</li>
				<li>
					<span class="ico-esp"></span>
					<dl>
						<dt>动态稳定控制</dt>
						<dd>ESP</dd>
					</dl>
				</li>
				<li>
					<span class="ico-dingsuxunhang"></span>
					<dl>
						<dt>定速巡航</dt>
						<dd></dd>
					</dl>
				</li>

				<li>
					<span class="ico-ebd"></span>
					<dl>
						<dt>电子制动分配</dt>
						<dd>EBD</dd>
					</dl>
				</li>
				<li>
					<span class="ico-qianyinlikongzhi"></span>
					<dl>
						<dt>牵引力控制</dt>
						<dd>ASR/TCS/TRC等</dd>
					</dl>
				</li>
				<li>
					<span class="ico-daocheleida"></span>
					<dl>
						<dt>倒车雷达</dt>
						<dd>车后</dd>
					</dl>
				</li>
				<li>
					<span class="ico-shachefuzhu"></span>
					<dl>
						<dt>刹车辅助</dt>
						<dd>EBA/BAS/BA/EVA</dd>
					</dl>
				</li>

				<li>
					<span class="ico-shangpofuzhu"></span>
					<dl>
						<dt>上坡辅助</dt>
						<dd></dd>
					</dl>
				</li>
				<li>
					<span class="ico-eps"></span>
					<dl>
						<dt>随速助力转向</dt>
						<dd>EPS</dd>
					</dl>
				</li>	
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>倒车影像</li>
				<li>GPS导航系统</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="yingyinxitong"></div>
			<div class="big-tt">
				影音系统
			</div>
			<div class="tt tt-blue">
				全系标配
			</div>
			<ul class="ul-list">
				<li>行车电脑显示屏</li>
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>中控台液晶屏</li>
				<li>蓝牙系统</li>
				<li>USB、iPod、SD卡外接口</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="anquanqinang"></div>
			<div class="big-tt">
				安全气囊
			</div>
			<div class="tt tt-blue">
				全系标配
			</div>
			<ul class="ul-list">
				<li>驾驶位安全气囊</li>
				<li>副驾驶位安全气囊</li>
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>前排侧安全气囊</li>
				<li>前排头部气囊（气帘）</li>
				<li>后排侧安全气囊</li>
				<li>后排头部气囊（气帘）</li>
			</ul>

			<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz5.png" style="margin-top:40px;" />
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="chuang"></div>
			<div class="big-tt">
				窗/后视镜
			</div>
			<div class="tt tt-blue">
				全系标配
			</div>
			<ul class="ul-list">
				<li>电动窗防夹功能</li>
				<li>后视镜带侧转向灯</li>
				<li>外后视镜加热功能</li>
				<li>外后视镜电动调节</li>
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>单天窗</li>
				<li>内后视镜防眩目功能</li>
			</ul>

			<div class="tt tt-yellow">
				全系无以下配置
			</div>
			<ul class="ul-list">
				<li>外后视镜电动折叠功能</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="deng"></div>
			<div class="big-tt">
				灯
			</div>
			<div class="tt tt-blue">
				全系标配
			</div>
			<ul class="ul-list">
				<li>前大灯延时关闭</li>
				<li>前大灯照射范围调整</li>
			</ul>

			<div class="tt tt-red">
				部分车款配置
			</div>
			<ul class="ul-list">
				<li>转向辅助灯</li>
			</ul>

			<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz6.png" style="margin-top:30px;" />
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="fangxiangpan"></div>
			<div class="big-tt">
				方向盘
			</div>
			<img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz7.png" style="margin-top:20px;" />
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="suo"></div>
			<div class="big-tt">
				锁
			</div>
			<ul class="ul-list">
				<li>中控门锁</li>
				<li>遥控钥匙</li>
				<li>儿童锁</li>
				<li>发动机电子防盗</li>
				<li>无钥匙进入系统</li>
				<li>无钥匙启动系统</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="zuoyi"></div>
			<div class="big-tt">
				座椅
			</div>
			<ul class="ul-list">
				<li>织物/真皮座椅</li>
				<li>儿童安全座椅固定装置</li>
				<li>座椅加热</li>
				<li>手动/电动调节座椅高低</li>
			</ul>
		</div>
	</div>

	<div class="pd20">
		<div class="peizhi-item">
			<div class="md-jibenxinxi" id="kongtiao"></div>
			<div class="big-tt">
				空调
			</div>
			<ul class="ul-list">
				<li>手动/自动空调</li>
			</ul>
		</div>
	</div>
</div>
<a href="/qiruim16/peizhi" class="return-peizhi" data-channelid="27.165.1757">查看车款配置</a>
   
    <script src="http://image.bitautoimg.com/uimg/mbt2015/js/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="//bglog.bitauto.com/getbglog.js?v=20170405"></script>
   <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <%-- 添加停留时长统计代码--%>
    <!--#include file="/include/pd/2016/wap/00001/201606_wap_daogou_tuijian_js_Manual.shtml"-->
</body>
</html>
