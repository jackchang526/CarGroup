<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperSelectCarTool.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.SuperSelectCarTool" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<title>【高级选车工具_汽车高级搜索_按条件找车】车型大全-易车网</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="Keywords" content="选车,选车工具,车型筛选,车型查询,易车网" />
	<meta name="Description" content="易车网高级选车工具频道为您提供按照各种条件查询车型功能，包括按汽车价格、参数配置、汽车级别、国产进口、变速方式、汽车排量等，如何挑选一款符合您心意的好车，易车网高级选车工具帮您解决。" />    <!--#include file="~/ushtml/0000/car_gaojixuanche-984.shtml"-->
</head>

<body data-offset="-130" data-target=".left-nav" data-spy="scroll">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<div class="bt_pageBox">
		<!--#include file="~/include/special/2010/00001/2014_lanmuCommon_header_Manual.shtml"-->
		<div class="header_style style_bottomline">
			<div class="bitauto_logo"></div>
			<!--页头导航_yiche_LOGO开始-->
			<div class="yiche_logo"><a href="http://www.bitauto.com">易车</a></div>

			<!--页头导航_yiche_LOGO结束-->
			<div class="yiche_lanmu"><em>|</em><span>高级选车工具</span></div>

			<div class="bt_searchNew">
				<!--#include file="~/html/bt_searchV3.shtml"-->
			</div>
			<div class="bt_hot">热门搜索：<a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a> <a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></div>
		</div>
	</div>

	<div class="bt_page" id="box">
		 <!--搜索条件star-->
	    <div class="tjbox" id="topfixed" >
		    <div class="tiaojianbox" id="filterContent"  style="display:none" >
			    <ul class="list" id="parameters">
			    </ul>
			    <div class="sumbit">
				    <div class="button_orange button_88_34"><a class="confirmButton" href="javascript:void(0);">确定</a></div>
				    <div class="text_chexing" id="styleTotal"><p>&nbsp;</p></div>
			    </div>
		    </div>
             <div class="tiaojianbox" id="nofilterContent">
			<div class="tiaojian_text" >请选择查询条件！</div>
			<div class="sumbit">
				<div class="button_gray button_88_34"><a href="javascript:void(0);">确定</a></div>
			</div>
		</div>
	    </div>
		<!--条件star-->
		<div class="line-box tiaojian_allbox" id="main_box">
			<div class="title-con">
				<div class="title-box title-box2">
					<h4>基本信息</h4>
					<div class="more"><a href="http://car.bitauto.com/">切换到简版选车工具&gt;&gt;</a></div>
				</div>
			</div>
			<div class="tiaojianlistbox" id="params-carinfo">
				<dl>
			    <dt>热门品牌</dt>
				<dd>
				    <ul class="tj_list" id = "masterbrandList">
					    <li><label><input type="checkbox" id="mid_8"/>大众</label></li>
					    <li><label><input type="checkbox" id="mid_7"/>丰田</label></li>
					    <li><label><input type="checkbox" id="mid_26"/>本田</label></li>
					    <li><label><input type="checkbox" id="mid_30"/>日产</label></li>
					    <li><label><input type="checkbox" id="mid_3"/>宝马</label></li>
					    <li><label><input type="checkbox" id="mid_127"/>别克</label></li>
					    <li><label><input type="checkbox" id="mid_9"/>奥迪</label></li>
					    <li><label><input type="checkbox" id="mid_49"/>雪佛兰</label></li>
					    <li><label><input type="checkbox" id="mid_13"/>现代</label></li>
					    <li><label><input type="checkbox" id="mid_42"/>奇瑞</label></li>
                        <li><label><input type="checkbox" id="mid_15"/>比亚迪</label></li>
					    <li><label><input type="checkbox" id="mid_17"/>福特</label></li>
					    <li><label><input type="checkbox" id="mid_18"/>马自达</label></li>
					    <li><label><input type="checkbox" id="mid_34"/>吉利</label></li>
					    <li><label><input type="checkbox" id="mid_2"/>奔驰</label></li>
					    <li><label><input type="checkbox" id="mid_16"/>铃木</label></li>
                        <li><label><input type="checkbox" id="mid_196"/>哈弗</label></li>
                        <li><label><input type="checkbox" id="mid_28"/>起亚</label></li>
                        <li><label><input type="checkbox" id="mid_5"/>标致</label></li>
                        <li><label><input type="checkbox" id="mid_10"/>斯柯达</label></li>
                        <li><label><input type="checkbox" id="mid_6"/>雪铁龙</label></li>	
					</ul>
				</dd>
			   </dl>
			   <dl>
			    <dt>价格</dt>
				<dd>
				    <ul class="tj_list">
                        <li><label><input name="price" type="radio" id="p_0"/>不限</label></li>
					    <li><label><input name="price" type="radio" id="p_1"/>5万以下</label></li>
					    <li><label><input name="price" type="radio" id="p_2"/>5-8万</label></li>
					    <li><label><input name="price" type="radio" id="p_3"/>8-12万</label></li>
					    <li><label><input name="price" type="radio" id="p_4"/>12-18万</label></li>
					    <li><label><input name="price" type="radio" id="p_5"/>18-25万</label></li>
					    <li><label><input name="price" type="radio" id="p_6"/>25-40万</label></li>
					    <li><label><input name="price" type="radio" id="p_7"/>40-80万</label></li>
					    <li><label><input name="price" type="radio" id="p_8"/>80万以上</label></li>
					    <li><div class="popup-price"><div class="jiagefanwei" id = "autoPrice"><input name="price" type="radio" id="p_9"/><input id = "p_min" onkeyup="value=value.replace(/(\D|\d{5})/g,'')" maxlength="4" class="inputborder input36" />至 <input id = "p_max" onkeyup="value=value.replace(/(\D|\d{5})/g,'')" maxlength="4" class="inputborder input36" />万</div><div class="button_gray button_60_26"><a id = "btnPriceSubmit" href="javascript:;">确定</a></div></div> <span class="comment_red" id="p_alert"></span></li>
					</ul>
				</dd>
			</dl>
                        <dl>
			    <dt>级别</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="l_1"/>微型车</label></li>
					    <li><label><input type="checkbox" id="l_2"/>小型车</label></li>
					    <li><label><input type="checkbox" id="l_3"/>紧凑型车</label></li>
					    <li><label><input type="checkbox" id="l_5"/>中型车</label></li>
					    <li><label><input type="checkbox" id="l_4"/>中大型车</label></li>
					    <li><label><input type="checkbox" id="l_6"/>豪华车</label></li>
					    <li><label><input type="checkbox" id="l_7"/>MPV</label></li>
					    <li class="ico-arrow"><label><input type="checkbox" id="l_8"/>SUV</label><i class="sanjiao"></i></li>
					    <li><label><input type="checkbox" id="l_9"/>跑车</label></li>
					    <li><label><input type="checkbox" id="l_11"/>面包车</label></li>
                        <li><label><input type="checkbox" id="l_12"/>皮卡</label></li>
                        <li><label><input type="checkbox" id="l_18"/>客车</label></li>
					</ul>
					<div class="clear"></div>
					<div class="openlayer">
						<ul id = "suvList">
							<li><label><input type="checkbox" id="l_13"/>小型SUV</label></li>
							<li><label><input type="checkbox" id="l_14"/>紧凑型SUV</label></li>
							<li><label><input type="checkbox" id="l_15"/>中型SUV</label></li>
							<li><label><input type="checkbox" id="l_16"/>中大型SUV</label></li>
							<li><label><input type="checkbox" id="l_17"/>全尺寸SUV</label></li>
						</ul>	
					</div>                  
				</dd>
			</dl>
            <dl>
			    <dt>厂商</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="g_1"/>自主</label></li>
					    <li><label><input type="checkbox" id="g_2"/>合资</label></li>
					    <li><label><input type="checkbox" id="g_4"/>进口</label></li>
					  
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>国别</dt>
				<dd>
				    <ul class="tj_list">
                        <li><label><input type="checkbox" id="c_4"/>德系</label></li>
					    <li><label><input type="checkbox" id="c_2"/>日系</label></li>
					    <li><label><input type="checkbox" id="c_16"/>韩系</label></li>
					    <li><label><input type="checkbox" id="c_8"/>美系</label></li>
					    <li><label><input type="checkbox" id="c_484"/>欧系</label></li>
					    <li><label><input type="checkbox" id="c_509"/>非日系</label></li>
					</ul>
				</dd>
			</dl>
			<dl>
			    <dt>排量</dt>
				<dd>
				    <ul class="tj_list">
                        <li><label><input type="radio" name="dis" id="d_0"/>不限</label></li>
					    <li><label><input type="radio" name="dis" id="d_1"/>1.4L以下</label></li>
					    <li><label><input type="radio" name="dis" id="d_2"/>1.4-1.6L</label></li>
					    <li><label><input type="radio" name="dis" id="d_3"/>1.6-1.8L</label></li>
					    <li><label><input type="radio" name="dis" id="d_4"/>1.8-2.0L</label></li>
					    <li><label><input type="radio" name="dis" id="d_5"/>2.0-3.0L</label></li>
					    <li><label><input type="radio" name="dis" id="d_6"/>3.0L以上</label></li>
					    <li>
                        <div class="jiagefanwei"><input type="radio" name="dis" id="d_7"/><input id="d_min" maxlength="4" class="inputborder input36" />至 <input id="d_max" maxlength="4"  class="inputborder input36"/>L</div><div class="button_gray button_60_26"><a id="btnDisSubmit" href="javascript:;">确定</a></div><span class="comment_red" id="d_alert"></span></li>
					</ul>
				</dd>
			</dl>
			 <dl>
			    <dt>变速箱</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="t_1"/>手动</label></li>
					    <li><label><input type="checkbox" id="t_32"/>半自动</label></li>
					    <li><label><input type="checkbox" id="t_2"/>自动</label></li>
					    <li><label><input type="checkbox" id="t_4"/>手自一体</label></li>
					    <li><label><input type="checkbox" id="t_8"/>无极变速</label></li>
					    <li><label><input type="checkbox" id="t_16"/>双离合</label></li>
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>驱动</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="dt_1"/>前驱</label></li>
					    <li><label><input type="checkbox" id="dt_2"/>后驱</label></li>
					    <li><label><input type="checkbox" id="dt_4"/>全时四驱</label></li>
					    <li><label><input type="checkbox" id="dt_8"/>分时四驱</label></li>
					    <li><label><input type="checkbox" id="dt_16"/>适时四驱</label></li>
					</ul>
				</dd>
			</dl>
			<dl>
			    <dt>燃料</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="f_7"/>汽油</label></li>
					    <li><label><input type="checkbox" id="f_8"/>柴油</label></li>
					    <li><label><input type="checkbox" id="f_16"/>纯电动</label></li>
					    <li><label><input type="checkbox" id="f_2"/>油电混合</label></li>
					    <li><label><input type="checkbox" id="f_4"/>油气混合</label></li>
					    <li><label><input type="checkbox" id="f_32"/>LPG</label></li>
					    <li><label><input type="checkbox" id="f_64"/>CNG</label></li>
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>油耗</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="radio" name="fuelcon" id="fc_0"/>不限</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_1"/>6L以下</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_2"/>6-8L</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_3"/>8-10L</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_4"/>10-12L</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_5"/>12-15L</label></li>
					    <li><label><input type="radio" name="fuelcon" id="fc_6"/>15L以上</label></li>
					</ul>
				</dd>
			</dl>
			<dl>
				<dt>供油方式</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="more_275"/>单点电喷</label></li>
					    <li><label><input type="checkbox" id="more_276"/>多点电喷</label></li>
					    <li><label><input type="checkbox" id="more_277"/>混合喷射</label></li>
					    <li><label><input type="checkbox" id="more_278"/>直喷</label></li>
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>车身</dt>
				<dd>
				    <ul class="tj_list">
                        <li><label><input type="radio" name="body" id="b_0"/>不限</label></li>
					    <li><label><input type="radio" name="body" id="b_1"/>两厢</label></li>
					    <li><label><input type="radio" name="body" id="b_2"/>三厢</label></li>
					    <li><label><input type="radio" name="body" id="lv_1"/>旅行版</label></li>
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>车门数</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="more_268"/>2门</label></li>
						<li><label><input type="checkbox" id="more_269"/>3门</label></li>
					    <li><label><input type="checkbox" id="more_270"/>4门</label></li>
					    <li><label><input type="checkbox" id="more_271"/>5门</label></li>
						<li><label><input type="checkbox" id="more_272"/>6门</label></li>
					</ul>
				</dd>
			</dl>
            <dl>
			    <dt>座位数</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="checkbox" id="more_262"/>2座</label></li>
					    <li><label><input type="checkbox" id="more_263"/>4座</label></li>
					    <li><label><input type="checkbox" id="more_264"/>5座</label></li>
					    <li><label><input type="checkbox" id="more_265"/>6座</label></li>
					    <li><label><input type="checkbox" id="more_266"/>7座</label></li>
					    <li><label><input type="checkbox" id="more_267"/>7座以上</label></li>
					</ul>
				</dd>
			</dl>
			</div>

			<div class="title-con">
				<div class="title-box title-box2">
					<h4>更多条件</h4>
				</div>
			</div>
            <div class="tiaojianlistbox">
              <h6 id="params-carengine">发动机</h6>
                <dl><dt>进气形式</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="radio" name="airadmission" id="more_0"/>不限</label></li>
					    <li><label><input type="radio" name="airadmission" id="more_100"/>自然吸气</label></li>
					    <li class="ico-arrow"><label><input type="radio" name="airadmission" id="more_101"/>增压</label><i class="sanjiao"></i></li>
					</ul>
					<div class="clear"></div>
					<div class="openlayer">
						<ul>
							<li><label><input type="checkbox" id="more_102"/>涡轮增压</label></li>
							<li><label><input type="checkbox" id="more_103"/>双涡轮增压</label></li>
							<li><label><input type="checkbox" id="more_104"/>机械增压</label></li>
							<li><label><input type="checkbox" id="more_105"/>涡轮机械双增压</label></li>
						</ul>
					</div>
				</dd></dl>
                <dl><dt>发动机位置</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_106"/>前置</label></li>
                <li><label><input type="checkbox" id="more_107"/>中置</label></li>
                <li><label><input type="checkbox" id="more_108"/>后置</label></li>
                </ul></dd></dl>
                <dl><dt>气缸排列</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_109"/>L型</label></li>
                <li><label><input type="checkbox" id="more_110"/>V型</label></li>
                <li><label><input type="checkbox" id="more_111"/>B型</label></li>
                <li><label><input type="checkbox" id="more_112"/>W型</label></li>
                <li><label><input type="checkbox" id="more_113"/>H型</label></li>
                <li><label><input type="checkbox" id="more_114"/>R型</label></li>
                <li><label><input type="checkbox" id="more_115"/>转子</label></li>
                </ul></dd></dl>
                <dl><dt>气缸数</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_116"/>3缸以下</label></li>
                <li><label><input type="checkbox" id="more_117"/>3缸</label></li>
                <li><label><input type="checkbox" id="more_118"/>4缸</label></li>
                <li><label><input type="checkbox" id="more_119"/>5缸</label></li>
                <li><label><input type="checkbox" id="more_120"/>6缸</label></li>
                <li><label><input type="checkbox" id="more_121"/>8缸</label></li>
                </ul></dd></dl>
                <dl><dt>环保标准</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_122"/>欧5</label></li>
                <li><label><input type="checkbox" id="more_123"/>欧4</label></li>
                <li><label><input type="checkbox" id="more_124"/>欧3</label></li>
                <li><label><input type="checkbox" id="more_125"/>国5</label></li>
                <li><label><input type="checkbox" id="more_126"/>国4</label></li>
                <li><label><input type="checkbox" id="more_127"/>京5</label></li>
                </ul></dd></dl>
                <h6 id="params-bottomstop">底盘制动</h6>
                <dl><dt>底盘结构</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_128"/>承载式</label></li>
                <li><label><input type="checkbox" id="more_129"/>非承载式</label></li>
                <li><label><input type="checkbox" id="more_130"/>半承载式</label></li>
                </ul></dd></dl>
                <dl><dt>悬架</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_131"/>独立前悬架</label></li>
                <li><label><input type="checkbox" id="more_132"/>独立后悬架</label></li>
                <li><label><input type="checkbox" id="more_133"/>非独立前悬架</label></li>
                <li><label><input type="checkbox" id="more_134"/>非独立后悬架</label></li>
                <li><label><input type="checkbox" id="more_135"/>可调悬架</label></li>
                <li><label><input type="checkbox" id="more_136"/>空气悬架</label></li>
                <li><label><input type="checkbox" id="more_137"/>四轮独立悬架</label></li>
                </ul></dd></dl>
                <dl><dt>差速锁位置</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_138"/>中央差速器锁</label></li>
                <li><label><input type="checkbox" id="more_139"/>前轴差速器锁</label></li>
                <li><label><input type="checkbox" id="more_140"/>后轴差速器锁</label></li>
                </ul></dd></dl>
                <dl><dt>前轮制动</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_141"/>盘式</label></li>
                <li><label><input type="checkbox" id="more_142"/>鼓式</label></li>
                <li><label><input type="checkbox" id="more_143"/>通风盘</label></li>
                <li><label><input type="checkbox" id="more_144"/>实心盘</label></li>
                <li><label><input type="checkbox" id="more_145"/>盘鼓结合</label></li>
                </ul></dd></dl>
                <dl><dt>后轮制动</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_146"/>盘式</label></li>
                <li><label><input type="checkbox" id="more_147"/>鼓式</label></li>
                <li><label><input type="checkbox" id="more_148"/>通风盘</label></li>
                <li><label><input type="checkbox" id="more_149"/>实心盘</label></li>
                <li><label><input type="checkbox" id="more_150"/>盘鼓结合</label></li>
                </ul></dd></dl>
                <dl><dt>手刹类型</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_151"/>手刹</label></li>
                <li><label><input type="checkbox" id="more_152"/>脚刹</label></li>
                <li><label><input type="checkbox" id="more_153"/>中央拉索式</label></li>
                <li><label><input type="checkbox" id="more_154"/>电子驻车</label></li>
                <li><label><input type="checkbox" id="more_155"/>机械驻车</label></li>
                <li><label><input type="checkbox" id="more_156"/>强力弹簧驻车</label></li>
                </ul></dd></dl>
                <dl><dt>转向助力</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_157"/>电子</label></li>
                <li><label><input type="checkbox" id="more_158"/>液压</label></li>
                <li><label><input type="checkbox" id="more_159"/>电子液压</label></li>
                <li><label><input type="checkbox" id="more_160"/>电子电传</label></li>
                </ul></dd></dl>
                <dl><dt>变速杆</dt><dd><ul class="tj_list">
                <li><label><input type="radio" name="gearlever" id="more_1"/>不限</label></li>
                <li><label><input type="radio" name="gearlever" id="more_161"/>电子挡杆</label></li>
                <li><label><input type="radio" name="gearlever" id="more_162"/>机械挡杆</label></li>
                <li><label><input type="radio" name="gearlever" id="more_163"/>方向盘拨片</label></li>
                </ul></dd></dl>
                <dl><dt>挡位个数</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_164"/>3</label></li>
                <li><label><input type="checkbox" id="more_165"/>4</label></li>
                <li><label><input type="checkbox" id="more_166"/>5</label></li>
                <li><label><input type="checkbox" id="more_167"/>6</label></li>
                <li><label><input type="checkbox" id="more_168"/>7</label></li>
                <li><label><input type="checkbox" id="more_169"/>8</label></li>
                <li><label><input type="checkbox" id="more_170"/>9</label></li>
                </ul></dd></dl>
                <h6 id="params-safeconfig">安全配置</h6>
                <dl><dt>气囊位置</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_171"/>驾驶位</label></li>
                <li><label><input type="checkbox" id="more_172"/>副驾驶位</label></li>
                <li><label><input type="checkbox" id="more_173"/>前排头部</label></li>
                <li><label><input type="checkbox" id="more_174"/>前排侧</label></li>
                <li><label><input type="checkbox" id="more_175"/>后排头部</label></li>
                <li><label><input type="checkbox" id="more_176"/>后排侧</label></li>
                </ul></dd></dl>
                <dl><dt>安全系统</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_177"/>胎压监测</label></li>
                <li><label><input type="checkbox" id="more_178"/>零压续行</label></li>
                <li><label><input type="checkbox" id="more_179"/>无钥匙启动</label></li>
                <li><label><input type="checkbox" id="more_180"/>儿童锁</label></li>
                <li><label><input type="checkbox" id="more_181"/>儿童座椅固定</label></li>
                </ul></dd></dl>
                <h6 id="params-drivingassistance">行车辅助</h6>
                <dl><dt>操控辅助</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_182"/>刹车防抱死(ABS)</label></li>
                <li><label><input type="checkbox" id="more_183"/>刹车辅助(EBA/BAS/BA/EVA等)</label></li>
                <li><label><input type="checkbox" id="more_184"/>动态稳定控制系统(ESP/DSC/VSC/ESC)</label></li>
                <li><label><input type="checkbox" id="more_185"/>弯道制动控制系统(CBC)</label></li>
                <li><label><input type="checkbox" id="more_186"/>随速助力转向调节(EPS)</label></li>
                <li><label><input type="checkbox" id="more_187"/>电子制动力分配系统(EBD/CBC/EBV)</label></li>
                </ul></dd></dl>
                <dl><dt>电子辅助</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_188"/>上坡辅助</label></li>
                <li><label><input type="checkbox" id="more_189"/>自动泊车</label></li>
                <li><label><input type="checkbox" id="more_190"/>自动驻车</label></li>
                <li><label><input type="checkbox" id="more_191"/>主动安全</label></li>
                <li><label><input type="checkbox" id="more_192"/>并线辅助</label></li>
                <li><label><input type="checkbox" id="more_193"/>夜视系统</label></li>
                <li><label><input type="checkbox" id="more_194"/>定速巡航</label></li>
                <li><label><input type="checkbox" id="more_195"/>全景摄像头</label></li>
                <li><label><input type="checkbox" id="more_196"/>GPS导航</label></li>
                <li><label><input type="checkbox" id="more_197"/>自适应巡航</label></li>
                <li><label><input type="checkbox" id="more_198"/>启停系统</label></li>
                <li><label><input type="checkbox" id="more_199"/>倒车雷达</label></li>
                <li><label><input type="checkbox" id="more_200"/>倒车影像</label></li>
                <li><label><input type="checkbox" id="more_201"/>陡坡缓降</label></li>
                <li><label><input type="checkbox" id="more_202"/>车前泊车雷达</label></li>
                <li><label><input type="checkbox" id="more_203"/>HUD抬头数字</label></li>
                <li><label><input type="checkbox" id="more_279"/>电子限速</label></li>
                </ul></dd></dl>
                <h6 id ="params-bodyparts">车身</h6>
                <dl><dt>车窗</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_204"/>单天窗</label></li>
                <li><label><input type="checkbox" id="more_205"/>双天窗</label></li>
                <li><label><input type="checkbox" id="more_206"/>全景天窗</label></li>
                <li><label><input type="checkbox" id="more_207"/>电动车窗</label></li>
                <li><label><input type="checkbox" id="more_209"/>车窗防夹</label></li>
                <li><label><input type="checkbox" id="more_260"/>隔热玻璃</label></li>
                </ul></dd></dl>
                <dl><dt>后视镜</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_210"/>记忆功能</label></li>
                <li><label><input type="checkbox" id="more_211"/>加热功能</label></li>
                <li><label><input type="checkbox" id="more_212"/>电动折叠</label></li>
                <li><label><input type="checkbox" id="more_213"/>电动调节</label></li>
                <li><label><input type="checkbox" id="more_214"/>内后视镜防眩目</label></li>
                </ul></dd></dl>
                <dl><dt>感应雨刷</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_215"/>雨量传感</label></li>
                <li><label><input type="checkbox" id="more_216"/>车速传感</label></li>
                <li><label><input type="checkbox" id="more_217"/>雨量及车速传感</label></li>
                </ul></dd></dl>
   
			    <dl><dt>车顶</dt>
				<dd>
				    <ul class="tj_list">
					    <li><label><input type="radio" name="roof" id="more_2""/>不限</label></li>
					    <li><label><input type="radio" name="roof" id="more_219"/>硬顶</label></li>
					    <li class="ico-arrow"><label><input type="radio" name="roof" id="more_220"/>敞篷</label><i class="sanjiao"></i></li>
					</ul>
					<div class="clear"></div>
					<div class="openlayer">
						<ul>
							<li><label><input type="checkbox" id="more_221"/>软顶敞篷</label></li>
							<li><label><input type="checkbox" id="more_222"/>硬顶敞篷</label></li>
							<li><label><input type="checkbox" id="more_223"/>软硬双顶敞篷</label></li>
						</ul>
					</div>
				</dd>
			    </dl>
                <h6 id="params-lights">灯光</h6>
                <dl><dt>前照灯类型</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_224"/>氙气大灯</label></li>
                <li><label><input type="checkbox" id="more_225"/>卤素大灯</label></li>
                <li><label><input type="checkbox" id="more_226"/>LED大灯</label></li>
                </ul></dd></dl>
                <dl><dt>前大灯</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_227"/>自动开闭</label></li>
                <li><label><input type="checkbox" id="more_228"/>随动转向</label></li>
                </ul></dd></dl>
                <dl><dt>其他灯光</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_229"/>车内氛围灯</label></li>
                <li><label><input type="checkbox" id="more_230"/>日间行车灯</label></li>
                </ul></dd></dl>
                <h6 id ="params-innerconfig">内部配置</h6>
                <dl><dt>方向盘</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_231"/>多功能方向盘</label></li>
                <li><label><input type="checkbox" id="more_232"/>真皮方向盘</label></li>
                </ul></dd></dl>
                <dl><dt>座椅功能</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_233"/>座椅加热</label></li>
                <li><label><input type="checkbox" id="more_234"/>座椅按摩功能</label></li>
                <li><label><input type="checkbox" id="more_235"/>电动座椅记忆</label></li>
                <li><label><input type="checkbox" id="more_236"/>电动座椅调节</label></li>
                <li><label><input type="checkbox" id="more_237"/>驾驶座椅通风</label></li>
                <li><label><input type="checkbox" id="more_238"/>前排座椅通风</label></li>
                <li><label><input type="checkbox" id="more_239"/>后排座椅通风</label></li>
                </ul></dd></dl>
                <dl id="anchorTarget"><dt>娱乐通讯</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_240"/>蓝牙系统</label></li>
                <li><label><input type="checkbox" id="more_241"/>AUX音源接口</label></li>
                <li><label><input type="checkbox" id="more_242"/>USB音源接口</label></li>
                <li><label><input type="checkbox" id="more_243"/>中控液晶屏</label></li>
                <li><label><input type="checkbox" id="more_244"/>220V电压电源</label></li>
                </ul></dd></dl>
                <dl><dt>空气调节</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_245"/>空调</label></li>
                <li><label><input type="checkbox" id="more_246"/>温度分区</label></li>
                <li><label><input type="checkbox" id="more_247"/>后排独立空调</label></li>
                <li><label><input type="checkbox" id="more_248"/>后排出风口</label></li>
                <li><label><input type="checkbox" id="more_249"/>空气净化（PM2.5过滤）</label></li>
                </ul></dd></dl>
                <dl><dt>座椅材料</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_250"/>真皮</label></li>
                <li><label><input type="checkbox" id="more_251"/>皮革</label></li>
                <li><label><input type="checkbox" id="more_252"/>织物</label></li>
                <li><label><input type="checkbox" id="more_253"/>真皮+织物</label></li>
                <li><label><input type="checkbox" id="more_254"/>皮革+织物</label></li>
                </ul></dd></dl>
                <dl><dt>其他</dt><dd><ul class="tj_list">
                <li><label><input type="checkbox" id="more_255"/>运动座椅</label></li>
                <li><label><input type="checkbox" id="more_256"/>车顶行李箱架</label></li>
                </ul></dd></dl>
            </div>

			<div style="position: fixed; top: 304px; left: 0px;" id="left-nav" class="left-nav left-nav-duibi">
				<ul>
					<li class="current"><a href="javascript:;" data-target="params-carinfo">基本信息</a></li>
					<li class=""><a href="javascript:;" data-target="params-carengine">发动机</a></li>
					<li class=""><a href="javascript:;" data-target="params-bottomstop">底盘制动</a></li>
					<li class=""><a href="javascript:;" data-target="params-safeconfig">安全配置</a></li>
					<li class=""><a href="javascript:;" data-target="params-drivingassistance">行车辅助</a></li>
					<li class=""><a href="javascript:;" data-target="params-bodyparts">车身</a></li>
					<li class=""><a href="javascript:;" data-target="params-lights">灯光</a></li>
					<li class=""><a href="javascript:;" data-target="params-innerconfig">内部配置</a></li>
				</ul>
				<a style="display: none;" id="close-left-nav" class="close-left-nav" href="javascript:;">关闭浮层</a>
			</div>
			<div class="clear"></div>
		</div>
		<!--车型列表star-->
		<div class="line-box" id="params-styleList">
			<div class="title-con">
				<div class="title-box title-box2">
					<h4>车型列表</h4>
					<span id = "styleCount"></span>
					<div class="more"><%=sortModeHtml %></div>
				</div>
			</div>
			<div class="carpic_list  box_990_2" id= "divContent"></div>
            <div class="the_pages">
			<div id="divPage"> 
			</div>
		</div>
		</div>
       <!--无结果-->
		<div class="line-box" id="noResult" style ="display:none;">
			<div style="z-index:1" class="title-con"><div class="title-box title-box2"><h4>车型列表</h4></div></div><div class="no-txt-box"><p class="tit">抱歉，未找到合适的车型</p><p>请修改条件再次查询，或者去 <a target="_blank" href="http://www.taoche.com/all/">易车二手车</a> 看看</p></div>
			<div class="clear">
			</div>
		</div>
	</div>
    
    <!-- 对比浮动框 -->
	<div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none; margin-right: -360px;" data-page="compare" animateright="-533" animatebottom="229">
		<div class="tt" id="bar_minicompare" style="cursor: move;">
			<h6>车型对比</h6>
			<a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
		</div>
		<div class="content">
			<ul id="idListULForWaitCompare" class="fixed-list"></ul>
			<div class="fixed-box">
				<div class="fixed-input" id="CarSelectSimpleContainerParent">
					<input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly" />
					<%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
					<div class="right" userful="showcartypesim">
						<div class="star">
							<i class="star-i"></i>
						</div>
					</div>
					<div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
				</div>
				<div class="clear"></div>
				<div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
			</div>
			<div class="wamp">
				<em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
				<div class="clear"></div>
			</div>
			<div class="alert-center" id="AlertCenterDiv" style="display: none;">
				<p>最多对比10个车款</p>
			</div>
		</div>
	</div>
	<!--漂浮层模板start-->
	<div class="effect" style="display: none;">
		<div class="car-summary-btn-duibi button_gray"><a href="###" target="_self"><span>对比</span></a></div>
	</div>
	<!--漂浮层模板end-->
 	
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/gouche/pc/jquery.pagination.js?v=20150425"></script>
    
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV3.min.js?v=20160715"></script>
    <%--<script type="text/javascript" src="/jsnew/commons.js"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js"></script>--%>
   <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/superselectcartool.min.js?v=20160727"></script>
    <%--<script type="text/javascript" src="/jsnew/superselectcartool.js"></script>--%>
    <script type="text/javascript">
        $(function(){
            WaitCompareObj.Init();
        });
    </script>
    <script type="text/javascript">
    	SuperSelectCarTool.Parameters = <%=configParaHtml%>;
    	SuperSelectCarTool.initPageCondition();
</script>
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/footer2014.shtml"-->
</body>
</html>

