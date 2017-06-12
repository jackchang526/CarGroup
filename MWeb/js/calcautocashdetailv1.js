
function calcAutoCashAll() {
    if ($('#hidCarPrice').val() != "0") {
        shangPai = 500;
        $("#shangPai").html("500");
    } else {
        shangPai = 0;
        $("#shangPai").html("0");
    }
    //购置税
    calcAcquisitionTax();
    //车船使用税
    CalculateVehicleAndVesselTax();
    //交强险
    calcCompulsory();
    //第三责任险
    calcTPL();
    //车辆损失险
    calcCarDamage();
    //不计免赔特约险
    calcAbatement();
    //全车盗抢险
    calcCarTheft();
    //玻璃单独破碎险
    calcBreakageOfGlass();
    //自燃损失险
    calcSelfignite();
    //涉水险/发动机特别损失险
    calcCarEngineDamage();
    //车身划痕险
    calcCarDamageDW();
    //司机座位责任险
    calcLimitofDriver();
    //乘客座位责任险
    calcLimitofPassenger();
    //必要花费小计
    calcEssentialCost();
    //商业保险小计
    calcCommonTotal();
    //计算总价
    calcTotal();
}
$(function () {
    $("#chongZhi").on("click", function () {
        location.href = "/gouchejisuanqical/";
    });
});


//第三者责任险 车辆损失险 不计免赔
function JiBenX() {
    $('#chkDiSanZheX').attr("class", checkedClass);
    $('#chkCheSunShiX').attr("class", checkedClass);
    $('#chkBuJiX').attr("class", checkedClass);

    $('#chkQuanCheX').attr("class", uncheckedClass);
    $('#chkBoLiX').attr("class", uncheckedClass);
    $('#chkZiRanX').attr("class", uncheckedClass);
    $('#chkEngineX').attr("class", uncheckedClass);
    $('#chkChengKeX').attr("class", uncheckedClass);
    $('#chkSiJiX').attr("class", uncheckedClass);
    $('#chkCheShenX').attr("class", uncheckedClass);
    calcAutoCashAll();
}
//第三者责任险 车辆损失险 不计免赔 乘客坐位责任险 司机坐位责任险
function JingJiX() {
    $('#chkDiSanZheX').attr("class", checkedClass);
    $('#chkCheSunShiX').attr("class", checkedClass);
    $('#chkBuJiX').attr("class", checkedClass);
    $('#chkChengKeX').attr("class", checkedClass);
    $('#chkSiJiX').attr("class", checkedClass);

    $('#chkQuanCheX').attr("class", uncheckedClass);
    $('#chkBoLiX').attr("class", uncheckedClass);
    $('#chkZiRanX').attr("class", uncheckedClass);
    $('#chkCheShenX').attr("class", uncheckedClass);
    $('#chkEngineX').attr("class", uncheckedClass);
    calcAutoCashAll();
}

function QuanX() {
    $('#chkDiSanZheX').attr("class", checkedClass);
    $('#chkCheSunShiX').attr("class", checkedClass);
    $('#chkBuJiX').attr("class", checkedClass);
    $('#chkChengKeX').attr("class", checkedClass);
    $('#chkSiJiX').attr("class", checkedClass);
    $('#chkQuanCheX').attr("class", checkedClass);
    $('#chkBoLiX').attr("class", checkedClass);
    $('#chkZiRanX').attr("class", checkedClass);
    $('#chkCheShenX').attr("class", checkedClass);
    $('#chkEngineX').attr("class", checkedClass);
    calcAutoCashAll();
}

$(function () {
    $("#shangyeDiv div[id^=chk]").on("click", function () {
        $(this).toggleClass("jsq-item-checked");
        calcAutoCashAll();
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-chechuan]').rightSwipe({
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#cheChuanDl a").on("click", function () {
        $("#cheChuanDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backCheChuan").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-ZuoWeiS]').rightSwipe({
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#zuoWeiSDl a").on("click", function () {
        $("#zuoWeiSDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backZuoWei").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-DiSanZheX]').rightSwipe({
            isclick: function () {
                if ($("#chkDiSanZheX").attr("class") == checkedClass) {
                    return true;
                } else {
                    return false;
                }
            },
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#diSanZheXDl a").on("click", function () {
        $("#diSanZheXDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backDiSanZhe").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-BoLiX]').rightSwipe({
            isclick: function () {
                if ($("#chkBoLiX").attr("class") == checkedClass) {
                    return true;
                } else {
                    return false;
                }
            },
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#boLiXDl a").on("click", function () {
        $("#boLiXDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backBoLi").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-CheShenX]').rightSwipe({
            isclick: function () {
                if ($("#chkCheShenX").attr("class") == checkedClass) {
                    return true;
                } else {
                    return false;
                }
            },
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#cheShenXDl a").on("click", function () {
        $("#cheShenXDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backCheShen").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-SiJiX]').rightSwipe({
            isclick: function () {
                if ($("#chkSiJiX").attr("class") == checkedClass) {
                    return true;
                } else {
                    return false;
                }
            },
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#siJiXDl a").on("click", function () {
        $("#siJiXDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backSiJi").trigger("close");
    });
});

$(function () {
    //右侧侧附加选择层
    (function () {
        var iscroll = false;
        $('[data-action=popup-ChengKeX]').rightSwipe({
            isclick: function () {
                if ($("#chkChengKeX").attr("class") == checkedClass) {
                    return true;
                } else {
                    return false;
                }
            },
            clickEnd: function (b) {
                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'));
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');

                    if (!iscroll) {
                        iscroll = true;
                        $leftPopup.myScroll = new IScroll($swipeLeft[0], {
                            probeType: 1,
                            snap: 'dd',
                            momentum: true,
                            click: true
                        });
                    }
                } else {
                    $leftPopup.myScroll.scrollTo(0, 0, 0, false);
                }
            }
        });
    })();

    $("#chengKeXDl a").on("click", function () {
        $("#chengKeXDl dd").attr("class", "");
        $(this).parent().attr("class", "current");
        calcAutoCashAll();
        $("#backChengKe").trigger("close");
    });
});

function selectCarId(carId) {
    location.href = "/gouchejisuanqi/?carid=" + carId;
}


var arrMarkSerial = [], saleYearCount = 0;  //非停销年款
$(function () {
    //api.brand.currentid = @ViewData["CarBrandId"]; //品牌
    //api.car.currentid = @ViewData["CarSerialId"];  //车系
    api.model.currentid = $("#hidCarID").val();//车款
    var $body = $('body');
    var $qkzj_top_layer = $('.qkzj_top_layer', $body);//“全款总价”层
    //初始化品牌
    $('[data-action=brandlayer]', $body).rightSwipeAnimation({
        fnEnd: function () {
            var curSerialId = $("[data-action=brandlayer]").data("value");
            var $model = this;
            $body.animate({ scrollTop: 0 }, 30);
            var tags = $('.tag', $body);
            $qkzj_top_layer.css('opacity', 0); ////屏蔽“全款总价”层
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
                                    });
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
                        };
                    }

                    $model.find('.btn-return').click(function (ev) {
                        ev.preventDefault();
                        $(this).parents('.brandlayer').prev()[0].style.cssText = '';
                        $model.trigger('closeWindow');
                        $qkzj_top_layer.css('opacity', 1); //恢复“全款总价”层
                    });
                }
            });

            $model.find('.btn-return').click(function () {
                $model.trigger('close');
            });
        }
    });
});

//自适应页脚
//层自适应
var _commonSlider = function ($model, $body) {
    if ($model.height() > $(document.body).height()) {
        $(document.body).height($model.height());
    } else if ($model.height() < $(document.body).height()) {
        $('.masterBox', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight - 40);
        $('.brandlist').height(document.documentElement.clientHeight - 40);
    }
};

//折叠下划线控制
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
        }, 300);
    });
}