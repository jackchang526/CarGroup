<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialCompareForPK.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageTool.SerialCompareForPK" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=pageTitle %></title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
    <link rel="canonical" href="http://car.bitauto.com/duibi/<%=canonical%>" />
    <!--#include file="~/ushtml/0000/2014_car_duibi-871.shtml"-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
        line-height: 0; font-size: 0"></span>
    <!--#include file="~/html/header2014.shtml"-->
    <script type="text/javascript">
        var serialId1 = <%=serialId1 %>, serialId2 = <%=serialId2 %>;
    </script>
    <!--smenu start-->
    <div class="header_style" id="box">
        <div class="car_navigate">
            <div>
                <span>您当前的位置：</span><a rel="nofollow" href="http://www.bitauto.com/" target="_blank">易车</a>
                &gt; <a rel="nofollow" href="/" target="_blank">车型</a> <%=navigate %>
            </div>
        </div>
        <div class="bt_searchNew">
            <!--#include file="~/html/bt_searchV3.shtml"-->
        </div>
    </div>
    <!--smenu end-->
    <div class="bt_page">
        <!--互联互通 start-->
        <div class="bt_pageBox">
            <div class="publicTabNew">
                <ul id="" class="tab">
                    <li id="xinche_index" class="current"><a href="/duibi/">综合对比</a></li>
                    <li id="xinche_ssxc"><a href="/chexingduibi/<%= csIds==""?"":"?csids="+csIds %><%= carIds==""?"":(csIds==""?"?carIDs="+carIds:"&carIDs="+carIds) %>">
                        参数对比</a></li>
                    <li id="xinche_ssxc"><a href="/tupianduibi/<%= csIds==""?"":"?csids="+csIds %><%= carIds==""?"":(csIds==""?"?carIDs="+carIds:"&carIDs="+carIds) %>">
                        图片对比</a></li>
                    <%--<li id="xinche_1822"><a href="http://koubei.bitauto.com/duibi/">口碑对比</a></li>
					<li id="xinche_1822"><a href="/pingceduibi/">评测对比</a></li>--%>
                </ul>
                <%--<div class="more_duibi">
					<a href="#">保险计算</a> | <a href="#">贷款购车</a> | <a href="#">全款购车</a>
				</div>--%>
            </div>
        </div>
        <!--互联互通 end-->
        <div class="line-box line-box-t20">
            <div class="top_input_box fl" id="top_input_box_l">
                <select id="master1" name="" class="f-w200 f-curr">
                </select>&nbsp;
                <select id="serial1" name="" class="f-w200 f-curr">
                </select>
            </div>
            <div class="top_input_box fr" id="top_input_box_r">
                <select id="master2" name="" class="f-w200 f-curr">
                </select>&nbsp;
                <select id="serial2" name="" class="f-w200 f-curr">
                </select>
            </div>
            <!------------车型对比焦点开始-------------->
            <div class="vs_focus vs_focus_t40" id="div_focus">
                <a id="focus_left_btn" style="display: none" href="javascript:;" class="focus_left focus_left_span">
                    上一张</a> <a id="focus_right_btn" style="display: none" href="javascript:;" class="focus_right">
                        下一张</a>
                <div class="foucs_cont">
                    <div class="car_box fl">
                        <h5 id="carSerialName_l" style="display: none">
                            <a href="#" target="_balck" name="carSerialName_l"></a>
                        </h5>
                        <div class="photo_box" id="photobox-sl">
                            <ul id="photo-sl">
                            </ul>
                        </div>
                        <p class="link_box" style="display: none" id="carSerialName_focus_more_l">
                            <a href="#" target="_blank" id="more_image_l"></a>
                        </p>
                    </div>
                    <div class="car_box fr">
                        <h5 id="carSerialName_r" style="display: none" >
                                <a href="#" target="_balck" name="carSerialName_r"></a>
                        </h5>
                        <div class="photo_box" id="photobox-sr">
                            <ul id="photo-sr">
                            </ul>
                        </div>
                        <p class="link_box" style="display: none" id="carSerialName_focus_more_r">
                            <a href="#" target="_blank" id="more_image_r"></a>
                        </p>
                    </div>
                </div>
                <div class="vs_moren_txt" id="focus_count_default">
                    想知道谁更好?选车来对比！
                </div>
            </div>
            <!------------车型对比焦点结束-------------->
            <!------------谁更便宜开始-------------->
            <div id="div_price" style="display: none">
                <h3>
                    谁更便宜</h3>
                <div class="com_tabs">
                    <ul id="ul_price">
                        <li class="current" price="min"><em></em><a href="javascript:">两车对比最低配</a></li>
                        <li price="max"><em></em><a href="javascript:">两车对比最高配</a></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_price_l">
                                <a href="#" target="_balck" class="fl" name="carSerialName_l" ></a>
                            </div>
                            <span class="jiage fr" id="price_l"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="com_icon_box dao" id="price_l_span">
                        </div>
                        <p class="link" id="carSerialName_price_more_l">
                            <a href="#" target="_blank" id="more_price_l">更多朗逸报价</a>
                        </p>
                    </div>
                    <div class="cont_box win fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_price_r">
                                <a href="#" class="fr" target="_balck" name="carSerialName_r" ></a>
                            </div>
                            <span class="jiage fl" id="price_r"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="com_icon_box dao" id="price_r_span">
                        </div>
                        <p class="link" id="carSerialName_price_more_r">
                            <a href="#" target="_blank" id="more_price_r">更多克鲁兹报价</a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁更便宜结束-------------->
            <!------------谁的口碑更好开始-------------->
            <div id="div_koubei" style="display: none">
                <h3>
                    谁的口碑更好</h3>
                <div class="com_line_box">
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_koubei_l">
                                <a href="#" target="_balck" class="fl" name="carSerialName_l" ></a>
                            </div>
                            <span class="jiage fr">
                                <div id="rating_l">
                                </div>
                                <div class="mid_start_box">
                                    <span class="mid_start"><em style="width: 74%"></em></span>
                                </div>
                            </span>
                        </div>
                        <div class="youque_box">
                            <dl>
                                <dt>优点</dt>
                                <dd id="youdian_l">
                                </dd>
                            </dl>
                            <dl class="gary">
                                <dt>缺点</dt>
                                <dd id="quedian_l">
                                </dd>
                            </dl>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <div class="cont_box win fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_koubei_r">
                                <a href="#" class="fr" target="_balck" name="carSerialName_r" ></a>
                            </div>
                            <span class="jiage fl">
                                <div id="rating_r">
                                </div>
                                <div class="mid_start_box">
                                    <span class="mid_start"><em style="width: 74%"></em></span>
                                </div>
                            </span>
                        </div>
                        <div class="youque_box">
                            <dl>
                                <dt>优点</dt>
                                <dd id="youdian_r">
                                </dd>
                            </dl>
                            <dl class="gary">
                                <dt>缺点</dt>
                                <dd id="quedian_r">
                                </dd>
                            </dl>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <%-- <ul class="bfb">
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="appearance_lenth_l" style="background: #c00; width: 10%">
                                    <em style="display: block; width: 74%"></em>
                                </div>
                                <p class="fen" id="appearance_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="appearance_lenth_r" style="width: 100px; background: #999">
                                    <em style="width: 74%; display: none"></em>
                                </div>
                                <p class="fen" id="appearance_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>外观</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box">
                                <div class="baifenbi" id="upholstery_lenth_l" style="background: #999; width: 170px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="upholstery_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box win">
                                <div class="baifenbi" id="upholstery_lenth_r" style="width: 300px; background: #c00">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="upholstery_account_r">
                                    <em style="width: 44%; display: none"></em>
                                </p>
                            </div>
                            <div class="leibie">
                                <span>内饰</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="space_lenth_l" style="background: #c00; width: 370px">
                                    <em style="display: block; width: 74%"></em>
                                </div>
                                <p class="fen" id="space_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="space_lenth_r" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="space_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>空间</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="operation_lenth_l" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="operation_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="operation_lenth_r" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="operation_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>操控</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="power_lenth_l" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="power_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="power_lenth_r" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="power_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>动力</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="comfort_lenth_l" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="comfort_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="comfort_lenth_r" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="comfort_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>舒适</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" id="performtoprice_lenth_l" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="performtoprice_account_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" id="performtoprice_lenth_r" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="performtoprice_account_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>性价比</span>
                            </div>
                        </li>
                    </ul>--%>
                    <div class="cont_box fl">
                        <p class="link padd_t0" id="carSerialName_koubei_more_l">
                            <a href="#" target="_blank" id="more_koubei_l"></a>
                        </p>
                    </div>
                    <div class="cont_box fr">
                        <p class="link padd_t0" id="carSerialName_koubei_more_r">
                            <a href="#" target="_blank" id="more_koubei_r"></a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁的口碑更好结束-------------->
            <!------------谁卖的更好开始-------------->
            <div id="div_sale" style="display: none">
                <h3>
                    谁卖的更好</h3>
                <div class="com_line_box">
                </div>
                <div class="com_tabs">
                    <ul class="tabs_six" id="sale_monthlists">
                        <%=MonthStr %>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box win fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_sale_l">
                                <a href="#" class="fl" target="_blank" name="carSerialName_l" ></a>
                            </div>
                            <span class="jiage fr" id="sale_count_l"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="sale_l_span" class="com_icon_box mai">
                        </div>
                        <p class="link" id="carSerialName_sale_more_l">
                            <a href="#" target="_blank" id="more_sale_l"></a>
                        </p>
                    </div>
                    <div class="cont_box fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_sale_r">
                                <a href="#" target="_blank" class="fr" name="carSerialName_r"  ></a>
                            </div>
                            <span class="jiage fl" id="sale_count_r"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="sale_r_span" class="com_icon_box mai">
                        </div>
                        <p class="link" id="carSerialName_sale_more_r">
                            <a href="#" target="_blank" id="more_sale_r"></a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁卖的更好结束-------------->
            <!------------谁更受观注开始-------------->
            <div id="div_uv" style="display: none">
                <h3>
                    谁更受关注</h3>
                <div class="com_line_box">
                </div>
                <div class="com_tabs">
                    <ul class="tabs_six" id="uv_monthlists">
                        <%=MonthStr %>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box win fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_uv_l">
                                <a href="#" target="_blank" class="fl" name="carSerialName_l" ></a>
                            </div>
                            <span class="jiage fr" id="uv_count_l"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="uv_l_span" class="com_icon_box xin">
                        </div>
                        <p class="link" id="carSerialName_uv_more_l">
                            <a href="#" target="_blank" id="more_uv_l"></a>
                        </p>
                    </div>
                    <div class="cont_box fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_uv_r">
                                <a href="#" target="_blank" class="fr" name="carSerialName_r" ></a>
                            </div>
                            <span class="jiage fl" id="uv_count_r"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="uv_r_span" class="com_icon_box xin">
                        </div>
                        <p class="link" id="carSerialName_uv_more_r">
                            <a href="#" target="_blank" id="more_uv_r"></a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁更受观注结束-------------->
            <!------------谁空间更大开始-------------->
            <div id="div_space" style="display: none">
                <h3>
                    谁空间更大</h3>
                <div class="com_line_box">
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_space_l">
                                <a href="#" target="_blank" class="fl" name="carSerialName_l" ></a>
                            </div>
                        </div>
                    </div>
                    <div class="cont_box fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_space_r">
                                <a href="#" target="_blank" class="fr" name="carSerialName_r" ></a>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <ul class="bfb">
                        <li>
                            <div class="fl_baif_box">
                                <div class="baifenbi" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="length_l">
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="length_r">
                                </p>
                            </div>
                            <div class="leibie">
                                <span>长</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box">
                                <div class="baifenbi" style="background: #999; width: 300px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="width_l">
                                    2900mm
                                </p>
                            </div>
                            <div class="fr_baif_box win">
                                <div class="baifenbi" style="width: 300px; background: #c00">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="width_r">
                                    3000mm
                                </p>
                            </div>
                            <div class="leibie">
                                <span>宽</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="height_l">
                                    2900mm
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="height_r">
                                    2900mm
                                </p>
                            </div>
                            <div class="leibie">
                                <span>高</span>
                            </div>
                        </li>
                        <li>
                            <div class="fl_baif_box win">
                                <div class="baifenbi" style="background: #c00; width: 370px">
                                    <em style="display: none; width: 74%"></em>
                                </div>
                                <p class="fen" id="wheel_l">
                                    2900mm
                                </p>
                            </div>
                            <div class="fr_baif_box">
                                <div class="baifenbi" style="width: 10px; background: #999">
                                    <em style="width: 44%; display: none"></em>
                                </div>
                                <p class="fen" id="wheel_r">
                                    2900mm
                                </p>
                            </div>
                            <div class="leibie">
                                <span>轴距</span>
                            </div>
                        </li>
                    </ul>
                    <div class="cont_box fl">
                        <p class="link padd_t0" id="carSerialName_space_more_l">
                            <a href="#" target="_blank" id="more_peizhi_l"></a>
                        </p>
                    </div>
                    <div class="cont_box fr">
                        <p class="link padd_t0" id="carSerialName_space_more_r">
                            <a href="#" target="_blank" id="more_peizhi_r"></a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁空间更大结束-------------->
            <!------------谁更省油开始-------------->
            <div id="div_fuel" style="display: none">
                <h3>
                    谁更省油</h3>
                <div class="com_tabs">
                    <ul id="ul_fuel">
                        <li class="current" fuel="min"><em></em><a href="javascript:">两车对比最低配</a></li>
                        <li fuel="max"><em></em><a href="javascript:">两车对比最高配</a></li>
                    </ul>
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box fl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_fuel_l">
                                <a href="#" target="_blank" class="fl" name="carSerialName_l" ></a>
                            </div>
                            <span class="jiage fr" id="fuel_l"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="com_icon_box you" id="fuel_l_span">
                        </div>
                        <p class="link" id="carSerialName_fuel_more_l">
                            <a href="#" target="_blank" id="more_fuel_l"></a>
                        </p>
                    </div>
                    <div class="cont_box win fr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_fuel_r">
                                <a href="#" target="_blank" class="fr" name="carSerialName_r" ></a>
                            </div>
                            <span class="jiage fl" id="fuel_r"></span>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="com_icon_box you" id="fuel_r_span">
                        </div>
                        <p class="link" id="carSerialName_fuel_more_r">
                            <a href="#" target="_blank" id="more_fuel_r"></a>
                        </p>
                    </div>
                </div>
            </div>
            <!------------谁更省油结束-------------->
            <!------------网友更喜欢谁开始-------------->
            <div id="div_vote" style="display: none">
                <h3>
                    网友更喜欢谁</h3>
                <div class="com_line_box">
                </div>
                <div class="clear">
                </div>
                <div class="com_vs">
                    <div class="cont_box fl" id="con-sl">
                        <div class="head">
                            <div class="fontwidth fl" id="carSerialName_vote_l">
                                <a href="#" target="_blank" class="fl" name="carSerialName_l" ></a>
                            </div>
                            <a href="javascript:;" class="btn fr" id="vote-vl">我支持它</a> <span class="gray fr"
                                id="vote-gray-vl" style="display: none;">我支持它</span> <strong class="jia1" style="display: none;">
                                    +1</strong>
                        </div>
                    </div>
                    <div class="cont_box fr" id="con-sr">
                        <div class="head">
                            <div class="fontwidth fr" id="carSerialName_vote_r">
                                <a href="#" target="_blank" class="fr" name="carSerialName_r" ></a>
                            </div>
                            <a href="javascript:;" class="btn fl" id="vote-vr">我支持它</a> <span class="gray fl"
                                id="vote-gray-vr" style="display: none;">我支持它</span> <strong class="jia1" style="display: none;">
                                    +1</strong>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="xihuan_bfb" id="vote-width">
                        <div class="bft_left border_r bft_win" style="width: 50%">
                            <span>50%</span>
                        </div>
                        <div class="bft_right">
                            <span>50%</span>
                        </div>
                    </div>
                </div>
            </div>
            <!------------网友更喜欢谁结束-------------->
            <!------------看看编辑怎么说开始-------------->
            <div id="div_tuwenlist" style="display: none">
                <h3>
                    看看编辑怎么说</h3>
                <div class="com_line_box">
                </div>
                <div class="clear">
                </div>
                <div class="tuwenlistbox">
                    <ul id="samerelatedNews">
                    </ul>
                </div>
            </div>
            <!------------看看编辑怎么说结束-------------->
            <!------------看过这两辆车的还看开始-------------->
            <div class="line-box" id="div_seetosee" style="display: none">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4>
                            看过这两辆车的还看</h4>
                    </div>
                </div>
                <div class="carpic_list box_990">
                    <ul id="seetosee">
                    </ul>
                </div>
            </div>
            <!------------看过这两辆车的还看结束-------------->
            <!------------相关文章开始-------------->
            
            <%=NewsHtml %>
            <%--<div class="line-box" id="div_carnews" style="display: none">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4>
                            相关文章</h4>
                    </div>
                </div>
                <div class="text-list-box-b text-list-box-b-990">
                    <div class="text-list-box">
                        <ul id="carNews" class="text-list text-list-float text-list-float3 text-list-w1010">
                        </ul>
                    </div>
                </div>
            </div>--%>
            <!------------相关文章结束-------------->
            <!------------相关对比开始-------------->
            <%=UserCompareHtml %>
            <%--<div class="line-box" id="div_usercompare" style="display: none">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4>
                            相关对比</h4>
                    </div>
                </div>
                <div id="userCompare" class="vs_photo_box">
                </div>
                <div class="clear">
                </div>
            </div>--%>
            <!------------相关对比结束-------------->
            <!------------热门对比开始-------------->
            <%=HotCompareHtml %>
            <%--<div class="line-box" id="div_hotcompare" style="display: none">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4>
                            热门对比</h4>
                    </div>
                </div>
                <div class="vs_photo_box" id="hotCompare">
                </div>
                <div class="clear">
                </div>
            </div>--%>
            <!------------热门对比结束-------------->
            <!------------最新对比开始-------------->
            <%if (!string.IsNullOrEmpty(serialNewCompareHtml))
              {%>
            <div class="line-box" id="div_newcompare">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4>
                            最新对比</h4>
                    </div>
                </div>
                <div class="text-list-box-b text-list-box-b-990">
                    <div class="text-list-box">
                        <ul class="text-list text-list-float text-list-float3 text-list-w1010">
                            <%=serialNewCompareHtml %>
                        </ul>
                    </div>
                </div>
            </div>
            <%} %>
            <!------------最新对比结束-------------->
        </div>
    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlist.js?v=20140807"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/jquery.autoslider.js?v=2014112710"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnew/serialcompareforpk.js?v=201501091353"type="text/javascript"></script>
    
    <script type="text/javascript">
        
        
        $.getScript("http://image.bitautoimg.com/stat/PageAreaStatistics.js", function() {
            var numbers = "124,125,126,127,64";
            for (var i = 65; i <= 103; i++) {
                numbers += ","+i;
            }
            PageAreaStatistics.init(numbers);
        });
    </script>
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/CarCompareStat.js?20150303"></script>
    <script type="text/javascript" language="javascript">
        CarCompareStatistic.CsIDs = "<%= csIds %>";
        // 临时统计
        var movetimes = 0;
        $(document).mousemove(function (even) {
            movetimes++;
            if (movetimes > 50) {
                $(document).unbind("mousemove");
                mainCsCompareStatisticFunction();
            }
        });
    </script>
    <!-- 调用尾 -->
    <!--#include file="~/html/footer2014.shtml"-->
    <!--提意见浮层-->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
