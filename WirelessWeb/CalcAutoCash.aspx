﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalcAutoCash.aspx.cs" Inherits="WirelessWeb.CalcAutoCash" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>计算器_全款</title>
    <!--#include file="~/ushtml/0000/myiche2015_cube_jisuanqi_style-991.shtml"-->

     <style>
        #master_container {
            background-color: #fff;
        }
    </style>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/ajax.js?v=201209"></script>
 <%--   <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/carchange.js?v=2013052413"></script>--%>
</head>

<body>
    <div class="op-nav">
        <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
        <div class="tt-name"><a href="http://m.yiche.com/" class="yiche-logo">易车</a><h1>购车计算器</h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
     <div class="op-nav-mark" style="display: none;"></div>
    <input id="hidCarID" type="hidden" value="<%=carId %>"/>
     <input id="hidCarPrice" type="hidden" value="<%=carReferPrice %>" />
    <input id="startCalUrl" type="hidden" />
   <div class="masterBox">
        <!--页签导航开始-->
        <div class="first-tags">
            <ul>
                <li class="current"><a id="quankuanUrl" href="javascript:void(0)"><span>全款计算</span></a></li>
                <li><a id="daikuanUrl" href="/qichedaikuanjisuanqical/"><span>贷款计算</span></a></li>
                <li><a id="baoxianUrl" href="/qichebaoxianjisuancal/"><span>保险计算</span></a></li>
            </ul>
        </div>
        <!--页签导航结束-->
        <div class="m_height">
            <div id="topLayer" class="top_layer">请选择购买车款或输入裸车价格</div>
            <div class="xj-form">
                <a href="javascript:void(0);" data-action="brandlayer" data-value="<%=carSerialId %>" class="xj-change-city">
                    <span>选择车款</span>
                    <strong id="select_carname"><%=title %></strong>
                </a>
                <div class="xj-user-info-box">
                    <div class="xj-user-info">
                        <span>裸车价格</span>
                        <input id="luochePrice" type="number" class="current" />
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                $(function () {
                    $(function () {
                        $("#luochePrice").on("keyup input", function () {
                            var carPrice = $(this).val();
                            if (carPrice.length > 8) {
                                $(this).val(carPrice.substr(0, 8));
                            }
                        });

                        //$("#luochePrice").on("blur", function () {
                        //    $(this).attr("class", "");
                        //});

                        //$("#luochePrice").on("focus", function () {
                        //    $(this).attr("class", "current");
                        //});
                    });
                });
            </script>
            <div class="m-jsq-btn-box">
                <a id="startCal" href="javascript:void(0)" class="btn-one btn-orange">开始计算</a>
            </div>
        </div>
    <script type="text/javascript">
        var oldCarPrice = 0;

        $(function () {
            $("#chongZhi").on("click", function () {
                location.href = "/gouchejisuanqical/";
            });

            $("#startCal").on("click", function () {
                var luochePrice = GetIntValue($("#luochePrice").val());
                var condition1 = (isNaN(luochePrice) || !luochePrice);//非数字或者没值
                var carid = $("#hidCarID").val();

                if (!carid && condition1) {
                    $("#topLayer").show();
                    location.href = "javascript:void(0)";
                } else {
                    if (!carid) {
                        if (isNaN(luochePrice) || !luochePrice) {
                            $("#luochePrice").val("0");
                            $("#topLayer").show();
                        } else {
                            $("#topLayer").hide();
                            location.href = "/gouchejisuanqi/?carprice=" + luochePrice;
                        }
                    } else {
                        var newCarPrice = GetIntValue($("#luochePrice").val());
                        if (oldCarPrice != newCarPrice) {
                            location.href = "/gouchejisuanqi/?carid=" + carid + "&carprice=" + newCarPrice;
                        } else {
                            location.href = "/gouchejisuanqi/?carid=" + carid;
                        }
                    }
                }
            });

          
            var carId = <%=carId %>;
            if (carId) {
                var carName ='<%=title %>';
                if (carName.length > 20) {
                    $("#select_carname").attr("class", "act liahang");
                }
                $("#luochePrice").val($("#hidCarPrice").val());
                setCalcToolUrl("carid", carId);
            }
        });

        //设置连接地址
        function setCalcToolUrl(carIdOrPrice, paraValue) {
            if (carIdOrPrice == "carPrice") {
                $("#quankuanUrl").attr("href", "/gouchejisuanqical/?carprice=" + paraValue);
                $("#daikuanUrl").attr("href", "/qichedaikuanjisuanqical/?carprice=" + paraValue);
                $("#baoxianUrl").attr("href", "/qichebaoxianjisuancal/?carprice=" + paraValue);
            } else {
                $("#quankuanUrl").attr("href", "/gouchejisuanqical/?carid=" + paraValue);
                $("#daikuanUrl").attr("href", "/qichedaikuanjisuanqical/?carid=" + paraValue);
                $("#baoxianUrl").attr("href", "/qichebaoxianjisuancal/?carid=" + paraValue);
            }

        }

        //选择车款
        function selectCarId(carId) {
            location.href = "/gouchejisuanqical/?carid=" + carId;
        }

        //格式化千位符(6701->6,701)
        function formatCurrency(num) {
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num)) num = "0";
            var sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            num = Math.floor(num / 100).toString();
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
            return (((sign) ? '' : '-') + num);
        }

        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        //4.784->4784
        function GetIntValue(num) {
            num = num.toString().replace(/\,/g, '');
            return parseInt(num);
        }
    </script>

    <!--footer start-->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
	    <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
    <!--footer end-->
</div>
        <div id="master_container" data-url="/html/selectCarTemplate.html?v=20151207" class="brandlayer mthead">
    </div>
     <%--   <script src="/js/v2/iscroll.js"></script>
    <script src="/js/v2/underscore.js"></script>
    <script src="/js/v2/model.js"></script>
    <script src="/js/v2/note.js"></script>
    <script src="/js/v2/rightswipe.js"></script>
    <script src="/js/v2/brand.js"></script>--%>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/iscroll.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/underscore.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/model.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/note.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/rightswipe.js?v=2015112415"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/brand.js?v=2015112415"></script>
    <script type="text/javascript">
        var arrMarkSerial = [], saleYearCount = 0;  //非停销年款
        $(function () {
            api.brand.currentid = '<%=carBrandId%>';   //品牌
            api.car.currentid = '<%=carSerialId %>';  //车系
            api.model.currentid = $("#hidCarID").val();//车款
            var $body = $('body');
            //初始化品牌
            $('[data-action=brandlayer]', $body).rightSwipeAnimation({
                fnEnd: function () {
                    var curSerialId = $("[data-action=brandlayer]").data("value");
                    var $model = this;     
                    $body.animate({scrollTop:0},30);
                    var tags = $('.tag', $body);

                    //切换标签
                    $('.brandlist').tag({
                        tagName: '.first-tags',
                        fnEnd: function (idx) {
                            tags.hide();
                            if ((api.model.currentid == 0 || api.car.currentid == 0 || api.brand.currentid == 0)) {
                                idx = 1; $(".brandlist .first-tags ul li").eq(idx).addClass("current").siblings().remove();
                            }
                            tags.eq(idx).show();

                            if (idx == 0) {     //当前品牌
                                $('.curSerialSub', $body).swipeApi({
                                    url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&datatype=1&pid=" + curSerialId,
                                    templateid: '#curSerialTemplate',
                                    flatFn: function (data) {
                                        for (var n in data.CarList) {
                                            var SaleStateCount = 0;
                                            for (var i = 0; i < data.CarList[n].length; i++) {
                                                if (data.CarList[n][i].SaleState != '停销') {
                                                    SaleStateCount++;
                                                }
                                            }
                                            if (SaleStateCount > 0) {
                                                arrMarkSerial[n.replace('s', '')] = true;
                                                saleYearCount++;
                                            }
                                        }
                                        return data;
                                    },
                                    callback: function (html) {
                                        this.html(html);
                                        var curSerialName = $("#curSerialName").val();
                                        $('.curSerialName').html(curSerialName);

                                        //获取第二层的高度值
                                        var heights = [];
                                        function toResize() {
                                            heights.length = 0;
                                            $('.tag .y2015 ul').each(function (index, curr) {
                                                heights[index] = $(curr).height();
                                            })
                                        }

                                        $(window).resize(toResize).trigger('resize');
                                        $('[data-slider=pic-txt-hh]').sliderBox({
                                            heightFn: function (index) { return heights[index]; },
                                            isCloseFn: function (idx, index) {
                                                var isopen = !this.hasClass('open');
                                                if (isopen) {
                                                    this.prev().find('i').removeClass('up');
                                                }
                                                else {
                                                    this.prev().find('i').addClass('up');
                                                }
                                                return isopen;
                                            },
                                            onlyOne: settings.sliderBox.onlyOne,
                                            clickEnd: function (paras) {
                                                if (settings.sliderBox.onlyOne) {
                                                    this.parent().find('.tt-small i').removeClass('up');
                                                }
                                                if (paras.k == 'up') {
                                                    this.find('i').removeClass('up');
                                                } else {
                                                    this.find('i').addClass('up');
                                                }
                                                removeborder.call($model);
                                            }

                                        });
                                        removeborder.call($model);
                                        $(this).find(".pic-txt-h li a").on("click", function () {
                                            var curCarId = $(this).data("id");
                                            selectCarId(curCarId);
                                        });
                                        _commonSlider($model, $body);
                                    }
                                });
                            }
                            else if (idx == 1) {    //按品牌查找 
                                //初始化品牌
                                $body.trigger('brandinit');
                                //车款点击回调事件
                                api.model.clickEnd = function (paras) {
                                    api.brand.currentid = paras.masterid;
                                    api.car.currentid = paras.carid;
                                    api.model.currentid = paras.modelid;
                                    var curCarId = $(this).data("id");
                                    selectCarId(curCarId);
                                }
                            }

                            $model.find('.btn-return').click(function (ev) {
                                ev.preventDefault();
                                $(this).parents('.brandlayer').prev()[0].style.cssText = '';
                                $model.trigger('closeWindow');
                            })
                        }
                    });

                    $model.find('.btn-return').click(function () {
                        $model.trigger('close');
                    })
                }
            });
        });        //自适应页脚
        //层自适应        var _commonSlider = function ($model, $body) {
            if ($model.height() > $(document.body).height()) {
                $(document.body).height($model.height())
            } else if ($model.height() < $(document.body).height()) {
                $('.masterBox', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight - 40);
                $('.brandlist').height(document.documentElement.clientHeight - 40);
            }
        }        //折叠下划线控制
        function removeborder() {
            var $smalls = this.find('.tt-small');
            $smalls.each(function (index, curr) {
                var $current = $(curr);
                setTimeout(function () {
                    if (index < $smalls.length - 1) {
                        if ($current.next().height() == 0) {
                            $current.css('border-bottom', '1px solid #E9E9E9');
                        } else {
                            $current.css('border-bottom', '0px solid #E9E9E9');
                        }
                    }
                }, 300)
            })
        }
    </script>
</body>
</html>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->

