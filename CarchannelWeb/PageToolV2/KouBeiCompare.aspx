<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KouBeiCompare.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.KouBeiCompare" %>

<!DOCTYPE html>
<html>
<head>
    <title>口碑对比</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit"/>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
        <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
        <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->

    <!--#include file="~/ushtml/0000/yiche_2016_cube_koubeiduibi_style-1293.shtml"-->

    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnewV2/dropdownlistnew.min.js?v=20161228"></script>
    <script type="text/javascript">
        <%= AllCarJsArray %>;
        var carIdL = "<%= CarIdL %>";
        var carIdR = "<%= CarIdR %>";

        var CaoKongL = "<%= CaoKongL %>";
        var PeiZhiL = "<%= PeiZhiL %>";
        var NeiShiL = "<%= NeiShiL %>";
        var DongLiL = "<%= DongLiL %>";
        var WaiGuanL = "<%= WaiGuanL %>";
        var YouHaoL = "<%= YouHaoL %>";

        var CaoKongR = "<%= CaoKongR %>";
        var PeiZhiR = "<%= PeiZhiR %>";
        var NeiShiR = "<%= NeiShiR %>";
        var DongLiR = "<%= DongLiR %>";
        var WaiGuanR = "<%= WaiGuanR %>";
        var YouHaoR = "<%= YouHaoR %>";

        var ShowNameL = "<%= ShowNameL %>";
        var ShowNameR = "<%= ShowNameR %>";
    </script>
    <script src="http://image.bitautoimg.com/carchannel/jsnewv2/KouBeiCompare.min.js?v=201612301028"></script>
    <%--<script src="/jsnewV2/KouBeiCompare.js"></script>--%>
    <script type="text/javascript">
        function appendParaToCurrentUrl(name, value) {
            var currentUrl = window.location.href;
            var targetUrl = "";
            if (currentUrl.indexOf('?') > 0) {
                if (currentUrl.indexOf(name) > 0) {
                    //console.log("修改传入的name值");
                    var kvpair = currentUrl.split("?")[1].split("&");
                    for (var i = 0; i < kvpair.length; i++) {
                        if (kvpair[i].indexOf(name) >= 0) {
                            kvpair.splice(i, 1);
                            break;
                        }
                    }
                    kvpair.push(name + "=" + value);
                    targetUrl = currentUrl.split("?")[0] + "?" + kvpair.join("&");
                } else {
                    //console.log("追加name=value");
                    if (currentUrl.indexOf("&") == currentUrl.length - 1) {
                        targetUrl = window.location.href + name + "=" + value;
                    } else {
                        targetUrl = window.location.href + "&" + name + "=" + value;
                    }
                }
            } else {
                targetUrl = window.location.href + "?" + name + "=" + value;
            }
            window.location.href = targetUrl;
        }

        function setCarDisabled() {
            var idArr = [];
            idArr.push(carIdL);
            idArr.push(carIdR);
            for (var i = 0; i < idArr.length; i++) {
                var carId = idArr[i];
                if (carId > 0) {
                    //点击换辆车出现的车款下拉弹层
                    var aTagSmall = $("a[bita-value='" + carId + "']");
                    var ddTagSmall = $(aTagSmall).parent();
                    var showText = $(aTagSmall).html();
                    //console.log(showText);
                    //if (ddTagSmall.length > 0) {
                    //    $(ddTagSmall).html("<span>" + carInfoJson[i].CarName + "<strong>" + priceSmall + "</strong></span>");
                    //}
                    //页面刚加载时的车款下拉弹层
                    var aTagBig = $("a[bita-value='" + carId + "']");
                    var ddTagBig = $(aTagBig).parent();
                    var priceBig = $(aTagBig).attr("bita-price");
                    if (ddTagBig.length > 0) {
                        $(ddTagBig).html("<span>" + showText + "</span>");
                    }
                }
            }
        }

        $(function() {
            $(".zj-btn").bind("mouseover", function() {
                $(this).children(":first").show();
            });
            $(".zj-btn").bind("mouseout", function() {
                $(this).children(":first").hide();
            });
            if (parseInt('<%= SerialIdL %>') > 0 && parseInt('<%= SerialIdR %>') > 0) {
                KouBeiCompare.Init();
            }


            $(window).scroll(function() {
                if ($(window).scrollTop() >= 660) {
                    $(".fix-layer").show();
                } else {
                    $(".fix-layer").hide();
                }
            });
        });
    </script>
</head>

<body>
<span id="yicheAnchor" name="yicheAnchor" style="display: block; font-size: 0; height: 0; line-height: 0; width: 0;"></span>
<!--#include file="~/htmlV2/header2016.shtml"-->
<header class="header-main summary-box">
    <div class="container section-layout top" id="topBox">
        <div class="col-xs-6 left-box">
            <div class="crumbs h-line">
                <div class="crumbs-txt">
                    <span>当前位置：</span><a href="http://www.bitauto.com/" target="_blank">易车</a> &gt; <a href="/" target="_blank">车型</a>&gt; <strong>口碑对比</strong>
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
                        <li id="xinche_index">
                            <a href="/duibi/<%= (SerialIdL > 0 && SerialIdR > 0 ? SerialIdL + "-" + SerialIdR + "/" : "") + (CarIdL > 0 && CarIdR > 0 ? "?carIDs=" + CarIdL + "," + CarIdR : "") %>">综合对比</a>
                        </li>
                        <li id="xinche_ssxc">
                            <a href="/chexingduibi/<%= CarIdL > 0 && CarIdR > 0 ? "?carIDs=" + CarIdL + "," + CarIdR : "" %>">参数对比</a>
                        </li>
                        <li id="Li1">
                            <a href="/tupianduibi/<%= CarIdL > 0 && CarIdR > 0 ? "?carids=" + CarIdL + "," + CarIdR : "" %>">图片对比</a>
                        </li>
                        <li id="koubeiduibi" class="active">
                            <a href="/koubeiduibi/<%= CarIdL > 0 && CarIdR > 0 ? "?car_id_l=" + CarIdL + "&car_id_r=" + CarIdR : "" %>">口碑对比</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
    <!--互联互通 end-->
</header>
<div class="container">
    <!--顶部图片对比块开始-->
    <div class="pic-db-box">
        <div class="vs-box">
            <div class="vs-opac"></div>
            <div class="txt">VS</div>
        </div>
        <!--左侧主体开始-->
        <div class="photo-box fl">
            <img src="<%= SerialPicUrlL %>"/>
            <div class="car-sele-box">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master4">
                </div>
                <!--品牌选择结束-->

                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial4">
                </div>
                <!--车型选择结束-->

                <!--车款选择开始-->
                <div class="brand-form no-second" id="cartype4">
                </div>
                <!--车款选择结束-->

                <input id="hid_masterid_l" type="hidden" value="<%= MasterIdL %>"/>
                <input id="hid_serialid_l" type="hidden" value="<%= SerialIdL %>"/>
                <input id="hid_carid_l" type="hidden" value="<%= CarIdL %>"/>

                <script type="text/javascript">
                    //绑定下拉选择框
                    BitA.DropDownListNew({
                        container: { master: "master4", serial: "serial4", cartype: "cartype4" },
                        include: { serial: "1", cartype: "1" },
                        dvalue: { master: '<%= MasterIdL %>', serial: '<%= SerialIdL %>', cartype: '<%= CarIdL %>' },
                        datatype: 5,
                        callback: {
                            cartype: function(data) {
                                setCarDisabled();
                            }
                        },
                        onchange: {
                            master: function(data) {
                            },
                            serial: function(data) {
                            },
                            cartype: function(data) {
                                appendParaToCurrentUrl("car_id_l", data.id);
                            }
                        }
                    });
                </script>
            </div>
            <span class="tuijian" id="recommendationL" style="display: none">推荐</span>
        </div>
        <!--左侧主体结束-->

        <!--右侧主体开始-->
        <div class="photo-box fr">
            <img src="<%= SerialPicUrlR %>"/>
            <div class="car-sele-box">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master5"></div>
                <!--品牌选择结束-->
                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial5"></div>
                <!--车型选择结束-->
                <!--车款选择开始-->
                <div class="brand-form no-second" id="cartype5"></div>
                <!--车款选择结束-->

                <input id="hid_masterid_r" runat="server" type="hidden" value="-1"/>
                <input id="hid_serialid_r" runat="server" type="hidden" value="-1"/>
                <input id="hid_carid_r" runat="server" type="hidden" value="-1"/>

                <script type="text/javascript">
                    //绑定下拉选择框
                    BitA.DropDownListNew({
                        container: { master: "master5", serial: "serial5", cartype: "cartype5" },
                        include: { serial: "1", cartype: "1" },
                        dvalue: { master: '<%= MasterIdR %>', serial: '<%= SerialIdR %>', cartype: '<%= CarIdR %>' },
                        datatype: 5,
                        callback: {
                            cartype: function(data) {
                                setCarDisabled();
                            }
                        },
                        onchange: {
                            master: function(data) {

                            },
                            serial: function(data) {
                            },
                            cartype: function(data) {
                                appendParaToCurrentUrl("car_id_r", data.id);
                            }
                        }
                    });
                </script>
            </div>
            <span class="tuijian" id="recommendationR" style="display: none">推荐</span>
        </div>
        <!--右侧主体结束-->
    </div>
    <!--顶部图片对比块结束-->
    <% if (CarIdL > 0 && CarIdR > 0 & SerialIdL > 0 & SerialIdR > 0)
       { %>
        <h3>谁更值</h3>
        <%= WhoIsValueable %>
        <h3>分项对比</h3>
        <h5><em></em>油耗</h5>
        <div class="youhao-box"></div>

        <h5><em></em>操控<span>●标配 ○选配  - 无</span></h5>
        <%--duibi-table-gray--%>
        <div id="caokong" class="duibi-table"></div>

        <h5><em></em>配置<span>●标配 ○选配  - 无</span></h5>
        <div id="peizhi" class="duibi-table"></div>

        <h5><em></em>内饰<span>●标配 ○选配  - 无</span></h5>
        <div id="neishi" class="duibi-table"></div>

        <h5><em></em>动力</h5>
        <div id="dongli" class="dlwg"></div>

        <h5><em></em>外观</h5>
        <div id="waiguan" class="dlwg wg-bot"></div>

        <a href="/chexingduibi/<%= CarIdL > 0 && CarIdR > 0 ? "?carIDs=" + CarIdL + "," + CarIdR : "" %>" target="_blank" class="db-more-btn">+ 更多分项对比</a>

        <% if (WangYouDianPingL != "" && WangYouDianPingR != "")
           { %>
            <h3 class="wydp">网友点评</h3>
            <div class="dianpin-box">
                <div class="dianpin-cont fl">
                    <%= WangYouDianPingL %>
                </div>
                <div class="dianpin-cont fr">
                    <%--<span class="gray">暂无点评</span>--%>
                    <%= WangYouDianPingR %>
                </div>
            </div>
        <% } %>

        <% if (KoubeiReportListL.Count > 0 || KoubeiReportListR.Count > 0)
           { %>
            <h3>口碑报告</h3>
            <div class="baogao-box">
                <div class="baogao-cont fl">
                    <div class="img-box">
                        <% if (KoubeiReportListL.Count > 0)
                           { %>
                            <a target="_blank" href="<%= KoubeiReportListL[0].FilePath %>">
                                <img src="<%= SerialPicWhiteDictionary[SerialIdL].Replace("_2.", "_6.") %>"/>
                                <h6><%= KoubeiReportListL[0].Title %></h6>
                            </a>
                        <% }
                           else
                           { %>
                            <img src="<%= SerialPicWhiteDictionary[SerialIdL].Replace("_2.", "_6.") %>"/>
                            <h6>暂无报告</h6>
                        <% } %>
                    </div>

                </div>
                <div class="baogao-cont fr">
                    <div class="img-box">
                        <% if (KoubeiReportListR.Count > 0)
                           { %>
                            <a target="_blank" href="<%= KoubeiReportListR[0].FilePath %>">
                                <img src="<%= SerialPicWhiteDictionary[SerialIdR].Replace("_2.", "_6.") %>"/>
                                <h6><%= KoubeiReportListR[0].Title %></h6>
                            </a>
                        <% }
                           else
                           { %>
                            <img src="<%= SerialPicWhiteDictionary[SerialIdR].Replace("_2.", "_6.") %>"/>
                            <h6>暂无报告</h6>
                        <% } %>

                    </div>
                </div>
            </div>
        <% } %>

    <% } %>

    <!--新增加部分开始-->
    <%= GetHotCar() %>
    <!--新增加部分结束-->

</div>

<!--顶部定位浮动层开始-->
<div class="fix-layer" style="display: none">
    <div class="container">
        <div class="cont-box fl">
            <div class="head-box">
                <span class="tag" id="recommendation_flow_L" style="display: none">推荐</span>
                <strong><%= SerialNameL %></strong>
                <span class="car"><%= ShowNameL %></span>
            </div>
            <!--车型选择开始-->
            <div class="car-sele-box">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master6">
                </div>
                <!--品牌选择结束-->

                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial6">
                </div>
                <!--车型选择结束-->

                <!--车款选择开始-->
                <div class="brand-form no-second" id="cartype6">
                </div>
                <!--车款选择结束-->
            </div>
            <!--车型选择结束-->

            <script type="text/javascript">
                //绑定下拉选择框
                BitA.DropDownListNew({
                    container: { master: "master6", serial: "serial6", cartype: "cartype6" },
                    include: { serial: "1", cartype: "1" },
                    dvalue: { master: '<%= MasterIdL %>', serial: '<%= SerialIdL %>', cartype: '<%= CarIdL %>' },
                    datatype: 5,
                    callback: {
                        cartype: function(data) {
                            setCarDisabled();
                        }
                    },
                    onchange: {
                        master: function(data) {
                        },
                        serial: function(data) {
                        },
                        cartype: function(data) {
                            appendParaToCurrentUrl("car_id_l", data.id);
                        }
                    }
                });
            </script>
        </div>

        <div class="cont-box fr">
            <div class="head-box">
                <span class="tag" id="recommendation_flow_R" style="display: none">推荐</span>
                <strong><%= SerialNameR %></strong>
                <span class="car"><%= ShowNameR %></span>
            </div>
            <!--车型选择开始-->
            <div class="car-sele-box">
                <!--品牌选择开始-->
                <div class="brand-form no-second" id="master7">
                </div>
                <!--品牌选择结束-->

                <!--车型选择开始-->
                <div class="brand-form no-second" id="serial7">
                </div>
                <!--车型选择结束-->

                <!--车款选择开始-->
                <div class="brand-form no-second" id="cartype7">
                </div>
                <!--车款选择结束-->
            </div>
            <!--车型选择结束-->

            <script type="text/javascript">
                //绑定下拉选择框
                BitA.DropDownListNew({
                    container: { master: "master7", serial: "serial7", cartype: "cartype7" },
                    include: { serial: "1", cartype: "1" },
                    dvalue: { master: '<%= MasterIdR %>', serial: '<%= SerialIdR %>', cartype: '<%= CarIdR %>' },
                    datatype: 5,
                    callback: {
                        cartype: function(data) {
                            setCarDisabled();
                        }
                    },
                    onchange: {
                        master: function(data) {
                        },
                        serial: function(data) {
                        },
                        cartype: function(data) {
                            appendParaToCurrentUrl("car_id_r", data.id);
                        }
                    }
                });
            </script>
        </div>

    </div>
</div>
<!--顶部定位浮动层结束-->

<!--[if IE]>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/excanvas.min.js?v=20161228"></script>
<![endif]-->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/jquery.knob.min.js?v=20161228"></script>
<script src="http://image.bitautoimg.com/carchannel/jsnewv2/knob.extend.min.js?v=20161228"></script>

<script type="text/javascript">
    $(function() {
        $(document).click(function(e) {
            //e.preventDefault();
            e = e || window.event;
            var target = e.srcElement || e.target;
            if ($(target).closest(".car-sele-box").length > 0) {
                $(".zcfcbox").not($(target).closest(".car-sele-box").find(".zcfcbox")).hide();
            }
        });
    });
    $(function() {
        $("#knob_l").extend_knob({
            max: 5,
            changeFn: function() {
            }
        });
        $("#knob_r").extend_knob({
            max: 5,
            changeFn: function() {
            }
        });
    });
</script>
<!--#include file="~/htmlV2/footer2016.shtml"-->
</body>
</html>