<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialCompareForPK.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.SerialCompareForPK" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <title>
        <%=pageTitle %></title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
    <link rel="canonical" href="http://car.bitauto.com/duibi/<%=canonical%>" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_duibigongju_style-1262.shtml"-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <script type="text/javascript">
        var serialId1 = <%=serialId1 %>, serialId2 = <%=serialId2 %>;
    </script>
    <!--smenu start-->
    <header class="header-main summary-box">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-6 left-box">
                <div class="crumbs h-line">
                    <div class="crumbs-txt">
                        <span>当前位置：</span><a href="http://www.bitauto.com/" target="_blank">易车</a> &gt; <a href="/" target="_blank">车型</a> <%=navigate %>
                    </div>
                </div>
            </div>
            <div class="col-xs-6 right-box">
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <!--smenu end-->
        <!--互联互通 start-->
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul class="list list-justified">
                            <li id="xinche_index" class="active"><a href="/duibi/">综合对比</a></li>
                            <li id="xinche_ssxc"><a href="/chexingduibi/<%= csIds==""?"":"?csids="+csIds %><%= carIds==""?"":(csIds==""?"?carIDs="+carIds:"&carIDs="+carIds) %>">参数对比</a></li>
                            <li id="Li1"><a href="/tupianduibi/<%= csIds==""?"":"?csids="+csIds %><%= carIds==""?"":(csIds==""?"?carIDs="+carIds:"&carIDs="+carIds) %>">图片对比</a></li>
                            <li id="Li2"><a href="/koubeiduibi/<%= carIds==""?(csIds==""?"":"?csids="+csIds):(carIds.Split(',').Length>=2?"?car_id_l="+ carIds.Split(',')[0] +"&car_id_r="+carIds.Split(',')[1]:"") %>">口碑对比</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
        <!--互联互通 end-->
    </header>
    <!--互联互通 end-->
    <div class="container">
        <div class="db-head">
            <div class="db-sele-box-l" id="top_input_box_l">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master1">
                </div>
                <!--品牌选择结束-->
                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial1">
                </div>
                <!--车型选择结束-->
            </div>
            <div class="db-sele-box-r" id="top_input_box_r">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master2">
                </div>
                <!--品牌选择结束-->
                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial2">
                </div>
                <!--车型选择结束-->
            </div>
            <!------------车型对比焦点开始-------------->
            <div class="vs_focus" id="div_focus">
                <a id="focus_left_btn" style="display: none" href="javascript:;" class="focus_left focus_left_span">上一张</a> <a id="focus_right_btn" style="display: none" href="javascript:;" class="focus_right">下一张</a>
                <div class="foucs_cont">
                    <div class="car_box fl">
                        <h5 id="carSerialName_l" style="display: none">
                            <a href="#" target="_blank" name="carSerialName_l"></a>
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
                        <h5 id="carSerialName_r" style="display: none">
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
        </div>
        <!------------谁更便宜开始-------------->
        <div id="div_price" style="display: none" class="db-price">
            <h3>谁更便宜</h3>
            <div class="db-line" id="ul_price">
                <div class="win" price="min">两车对比最低配</div>
                <div class="lose" price="max">两车对比最高配</div>
            </div>
            <div class="db-car-box">
                <h6 id="carSerialName_price_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                <div class="price-box" id="price_l"></div>
                <div class="icon-box" id="price_l_span"></div>
                <p class="more" id="carSerialName_price_more_l">
                    <a href="#" target="_blank" id="more_price_l">更多朗逸报价</a>
                </p>
            </div>
            <div class="db-car-box">
                <h6 id="carSerialName_price_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                <div class="price-box" id="price_r"></div>
                <div class="icon-box" id="price_r_span"></div>
                <p class="more" id="carSerialName_price_more_r">
                    <a href="#" target="_blank" id="more_price_r">更多克鲁兹报价</a>
                </p>
            </div>
        </div>
        <!------------谁更便宜结束-------------->
        <!------------谁的口碑更好开始-------------->
        <div id="div_koubei" style="display: none" class="kb-better">
            <h3>谁的口碑更好</h3>
            <div class="better-box fl">
                <h6 id="carSerialName_koubei_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                <div id="rating_l" class="price-box"></div>
                <div class="start2-box inline-b">
                    <div class="start"><em style="width: 68%;"></em></div>
                </div>
                <div class="icon-box" id="youdian_l">
                    <p>优点</p>
                </div>
                <div class="icon-box" id="quedian_l">
                    <p>缺点</p>
                </div>
                <p id="carSerialName_koubei_more_l" class="more"><a href="#" id="more_koubei_l"></a></p>
            </div>
            <div class="better-box fr">
                <h6 id="carSerialName_koubei_r">
                    <a href="#" name="carSerialName_r" target="_blank"></a>
                </h6>
                <div id="rating_r" class="price-box"></div>
                <div class="start2-box inline-b">
                    <div class="start"><em style="width: 68%;"></em></div>
                </div>
                <div class="icon-box" id="youdian_r">
                    <p>优点</p>
                </div>
                <div class="icon-box" id="quedian_r">
                    <p>缺点</p>
                </div>
                <p class="more" id="carSerialName_koubei_more_r">
                    <a href="#" target="_blank" id="more_koubei_r"></a>
                </p>
            </div>
        </div>
        <!------------谁的口碑更好结束-------------->
        <!------------谁卖的更好开始-------------->
        <div id="div_sale" style="display: none" class="sell-better">
            <h3>谁卖的更好</h3>
            <div class="db-tags">
                <ul id="sale_monthlists">
                    <%=MonthStr %>
                </ul>
            </div>
            <div class="com_vs">
                <div class="sell-box">
                    <h6 id="carSerialName_sale_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                    <div class="price-box" id="sale_count_l"></div>
                    <div id="sale_l_span" class="icon-box"></div>
                    <p class="more" id="carSerialName_sale_more_l">
                        <a href="#" target="_blank" id="more_sale_l"></a>
                    </p>
                </div>
                <div class="sell-box">
                    <h6 id="carSerialName_sale_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                    <div class="price-box" id="sale_count_r"></div>
                    <div id="sale_r_span" class="icon-box"></div>
                    <p class="more" id="carSerialName_sale_more_r">
                        <a href="#" target="_blank" id="more_sale_r"></a>
                    </p>
                </div>
            </div>
        </div>
        <!------------谁卖的更好结束-------------->
        <!------------谁更受观注开始-------------->
        <div id="div_uv" style="display: none" class="sell-better follow-better">
            <h3>谁更受关注</h3>
            <div class="db-tags">
                <ul id="uv_monthlists">
                    <%=MonthStr %>
                </ul>
            </div>
            <div class="com_vs">
                <div class="sell-box">
                    <h6 id="carSerialName_uv_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                    <div class="price-box" id="uv_count_l"></div>
                    <div id="uv_l_span" class="icon-box"></div>
                    <p class="more" id="carSerialName_uv_more_l">
                        <a href="#" target="_blank" id="more_uv_l"></a>
                    </p>
                </div>
                <div class="sell-box">
                    <h6 id="carSerialName_uv_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                    <div class="price-box" id="uv_count_r"></div>
                    <div id="uv_r_span" class="icon-box"></div>
                    <p class="more" id="carSerialName_uv_more_r">
                        <a href="#" target="_blank" id="more_uv_r"></a>
                    </p>
                </div>
            </div>
        </div>
        <!------------谁更受观注结束-------------->
        <!------------谁空间更大开始-------------->
        <div id="div_space" style="display: none" class="space-better">
            <h3>谁空间更大</h3>
            <div class="s-cont-box">
                <div class="space-box">
                    <h6 id="carSerialName_space_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                    <ul class="left">
                        <li>
                            <span id="length_l"></span>
                            <div class="space-line-box baifenbi"><em style="width: 30%;"></em></div>
                        </li>
                        <li>
                            <span id="width_l"></span>
                            <div class="space-line-box baifenbi"><em style="width: 60%;"></em></div>
                        </li>
                        <li>
                            <span id="height_l"></span>
                            <div class="space-line-box baifenbi"><em style="width: 65%;"></em></div>
                        </li>
                        <li>
                            <span id="wheel_l"></span>
                            <div class="space-line-box baifenbi"><em style="width: 90%;"></em></div>
                        </li>
                    </ul>
                    <p class="more" id="carSerialName_space_more_l">
                        <a href="#" target="_blank" id="more_peizhi_l"></a>
                    </p>
                </div>
                <div class="space-box">
                    <h6 id="carSerialName_space_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                    <ul class="right">
                        <li>
                            <span id="length_r"></span>
                            <div class="space-line-box baifenbi"><em style="width: 30%;"></em></div>
                        </li>
                        <li>
                            <span id="width_r"></span>
                            <div class="space-line-box baifenbi"><em style="width: 60%;"></em></div>
                        </li>
                        <li>
                            <span id="height_r"></span>
                            <div class="space-line-box baifenbi"><em style="width: 65%;"></em></div>
                        </li>
                        <li>
                            <span id="wheel_r"></span>
                            <div class="space-line-box baifenbi"><em style="width: 90%;"></em></div>
                        </li>
                    </ul>
                    <p class="more" id="carSerialName_space_more_r">
                        <a href="#" target="_blank" id="more_peizhi_r"></a>
                    </p>
                </div>
                <div class="space-center">
                    <ul>
                        <li>长</li>
                        <li>宽</li>
                        <li>高</li>
                        <li>轴距</li>
                    </ul>
                </div>
            </div>
        </div>
        <!------------谁空间更大结束-------------->
        <!------------谁更省油开始-------------->
        <div id="div_fuel" style="display: none" class="fuel-better">
            <h3>谁更省油</h3>
            <div class="db-line" id="ul_fuel">
                <div class="win" fuel="min">两车对比最低配</div>
                <div class="lose" fuel="max">两车对比最高配</div>
            </div>
            <div class="fuel-box">
                <h6 id="carSerialName_fuel_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                <div class="price-box" id="fuel_l"></div>
                <div class="icon-box" id="fuel_l_span"></div>
                <p class="more" id="carSerialName_fuel_more_l">
                    <a href="#" target="_blank" id="more_fuel_l"></a>
                </p>
            </div>
            <div class="fuel-box">
                <h6 id="carSerialName_fuel_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                <div class="price-box" id="fuel_r"></div>
                <div class="icon-box" id="fuel_r_span"></div>
                <p class="more" id="carSerialName_fuel_more_r">
                    <a href="#" target="_blank" id="more_fuel_r"></a>
                </p>
            </div>
        </div>
        <!------------谁更省油结束-------------->
        <!------------网友更喜欢谁开始-------------->
        <div id="div_vote" style="display: none" class="prefer-better">
            <h3>网友更喜欢谁</h3>
            <div class="prefer-box" id="con-sl">
                <h6 id="carSerialName_vote_l"><a href="#" target="_blank" name="carSerialName_l"></a></h6>
                <em class="jia1" style="display: none;">+1</em>
                <a href="javascript:;" class="zan" id="vote-vl">我支持Ta </a>
                <a class="zan-ed" id="vote-gray-vl" style="display: none;">我支持Ta</a>
            </div>
            <div class="prefer-box" id="con-sr">
                <h6 id="carSerialName_vote_r"><a href="#" target="_blank" name="carSerialName_r"></a></h6>
                <em class="jia1" style="display: none;">+1</em>
                <a href="javascript:;" class="zan" id="vote-vr">我支持Ta</a>
                <a class="zan-ed" id="vote-gray-vr" style="display: none;">我支持Ta</a>
            </div>
            <div class="pre-line-box txt-right" id="vote-width">
                <span class="red" id="bft_left"></span>
                <span class="gray txt-right" id="bft_right"></span>
            </div>
        </div>
        <!------------网友更喜欢谁结束-------------->
        <!------------看看编辑怎么说开始-------------->
        <div id="div_tuwenlist" style="display: none">
            <div class="section-header header2">
                <div class="box">
                    <h2>看看编辑怎么说</h2>
                </div>
            </div>
            <div class="tuwenlistbox" id="samerelatedNews">
            </div>
        </div>
        <!------------看看编辑怎么说结束-------------->
        <!------------看过这两辆车的还看开始-------------->
        <div id="div_seetosee" style="display: none">
            <div class="section-header header2">
                <div class="box">
                    <h2>看过这两辆车的还看</h2>
                </div>
            </div>
            <div class="see-box" id="seetosee">
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
        <div id="div_newcompare">
            <div class="section-header header2">
                <div class="box">
                    <h2>最新对比</h2>
                </div>
            </div>
            <div class="article">
                <div class="list-txt list-txt-s list-txt-default list-txt-style4">
                    <ul>
                        <%=serialNewCompareHtml %>
                    </ul>
                </div>
            </div>
        </div>
        <%} %>
        <!------------最新对比结束-------------->

    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/dropdownlistnew.min.js?v=2016113015"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/jquery.autoslider.min.js?v=2016113015"></script>
     <script src="/jsnewv2/serialcompareforpk.js?v=2016120115" type="text/javascript"></script>
<%--    <script src="http://image.bitautoimg.com/carchannel/jsnewv2/serialcompareforpk.min.js?v=2016120115" type="text/javascript"></script>--%>
    <script type="text/javascript">
        (function () {
            //if (window.navigator.userAgent.indexOf("Chrome") !== -1) {
            var count = 0;
            var timer = setInterval(function () {
                count++;
                if (count > 60) clearInterval(timer);
                var obj = $("#samerelatedNews");
                if (obj.html()!="") {
                    clearInterval(timer);
                    $.getScript("http://image.bitautoimg.com/carchannel/jsnewv2/newsViewCount.min.js?v=2016113015", function () 

                    {});
                }
            }, 500);
            //}
        })();

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
<%--    <script type="text/javascript">
        (function(global) {
            global.SidebarConfig = {
                activate: false
            };
        })(window);
    </script>--%>
    <!-- 调用尾 -->
    <!--#include file="~/htmlV2/footer2016.shtml"-->
    <!--提意见浮层-->
    <!--   #  include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
</body>
</html>
