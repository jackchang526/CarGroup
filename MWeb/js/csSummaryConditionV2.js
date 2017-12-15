/// <reference path="commonbase.js" />
var selSubStopYear, selSubLevel, selSubBodyForm, tuangouTag; //筛选项的值
var CsSummaryCondition = function () {
    var _serialId = GlobalSummaryConfig.SerialId;
    var _this = this;
    var _year = '';
    _this.Init = function (arr) {
        //绑定tab项事件
        _bindTabSwitchEvent();
        _bindLoadMoreEvent("&year=" + $("#yeartag li").first().attr("id"), $("#yearDiv0"));
        //绑定筛选项事件
        var $body = $('body');
        var data = {
        };
        $(arr).each(function (index, item) {  //监听3个筛选条件按钮事件
            $body.trigger('publicswipe1', {
                actionName: item,
                ds: data,
                templateid: '#commonTemplate',
                clickCallBack: function (clickEnd) {
                    _year = $("#yeartag li.current").attr("id");
                    if (typeof (_serialId) == "undefined") {
                        return;
                    }
                    var $curBtn = this;
                    var option = item.substr(13, item.length - 14);
                    $.ajax({
                        url: "/handlers/GetMoreCarList.ashx?action=getSubItem&id=" + _serialId + "&year=" + _year + "&option=" + option + "&serialAllSpell=" + serialAllSpell + "&v=20170526",
                        cache: true,
                        dataType: "json",
                        success: function (dataItem) {
                            data = dataItem;
                            clickEnd.call($curBtn);
                            if (option == "stopyear") {
                                selSubStopYear = $(item).find("span").text();
                            }
                        },
                        error: function () {
                            console.log("error");
                        }
                    });
                },                fliterData: function (ds, paras) {
                    return data;
                },                fnEnd: function () {
                    var $swipeLeft = this, $back = $('.' + $swipeLeft.parent().attr('data-back'));
                    $back.click(function () {
                        $back.trigger('close');
                    })

                    var option = item.substr(13, item.length - 14); //当前点击的option
                    if (option == "stopyear") {
                        $(".stopyear .ap .y2015 li").first().remove();
                    }

                    $swipeLeft.find('a').click(function (ev) {
                        ev.preventDefault();

                        var subOptionVal = $(this).data("value");
                        var rqString = "&serialAllSpell=" + serialAllSpell;
                        var curOptionText;
                        switch (option) {
                            case "stopyear": rqString += "&stopYearOption=" + subOptionVal;
                                curOptionText = subOptionVal + "款";
                                selSubStopYear = curOptionText;
                                break;
                            case "level": rqString += "&levelOption=" + subOptionVal;
                                if (subOptionVal == "不限") {
                                    curOptionText = "排量";
                                }
                                else {
                                    curOptionText = subOptionVal;
                                }
                                selSubLevel = curOptionText;
                                break;
                            case "bodyform": rqString += "&bodyFormOption=" + subOptionVal;
                                if (subOptionVal == "不限") {
                                    curOptionText = "变速箱";
                                }
                                else {
                                    curOptionText = subOptionVal;
                                }
                                selSubBodyForm = curOptionText;
                                break;
                            default: break;
                        }
                        $(item).find("span").text(curOptionText); //给弹出层的背后父级按钮加样式并赋值
                        if (subOptionVal == "不限") {
                            $(item).removeClass("current");
                        }
                        else {
                            $(item).addClass("current");
                        }
                        //当前选中状态的其它option
                        $(item).siblings().filter(".current").each(function (i, siblingOption) {
                            var opVal = $(siblingOption).find("span").text();
                            switch ($(siblingOption).attr("data-action")) {
                                case "stopyear":
                                    //如果是年份，要判断其显示状态
                                    if ($(siblingOption).css("display") != "none") {
                                        rqString += "&stopYearOption=" + opVal.replace("款", "");
                                    }
                                    break;
                                case "level": rqString += "&levelOption=" + opVal.replace("排量", "不限"); break;
                                case "bodyform": rqString += "&bodyFormOption=" + (opVal == "变速箱" ? "不限" : opVal); break;
                                default: break;
                            }
                        });

                        rqString = "&year=" + _year + rqString;
                        _newSendRequestAndRender(rqString, function (doc) {
                            var $targetDiv;
                            $("div[id^='yearDiv']").each(function () {
                                if ($(this).css("display") != "none") {
                                    $targetDiv = $(this);
                                }
                            });
                            if (doc.childNodes && doc.childNodes.length > 0) {
                                $targetDiv.html(doc);
                                _bindLoadMoreEvent(rqString, $targetDiv);
                            }
                            else {
                                $targetDiv.html('<div class="block-none-car"><div class="ico-no-car"></div><em>抱歉，没有符合条件的车款</em><a href="###" id="btnConditionClear" class="btn-center btn-one">清空条件</a></div>');
                                _bindConditionClearEvent($targetDiv);
                            }
                        })
                        $back.trigger('close');
                    })
                }
            });
        })
    }

    var _bindConditionClearEvent = function ($targetnode) {
        $("#btnConditionClear").on("click", function () {
            var rqStr = "&serialAllSpell=" + serialAllSpell + "&year=" + _year;
            resetOptions();
            if (_year == "nosalelist") {
                rqStr += "&stopYearOption=" + nearestYear;
                selSubStopYear = nearestYear + "款";
                $("[data-action=stopyear]").find("span").text(selSubStopYear);
            }
            var $curClearDiv = $(this).parent();
            //_sendRequestAndRender(rqStr,rqStr, $targetnode, $curClearDiv);
            _newSendRequestAndRender(rqStr, function (doc) {
                if ($curClearDiv) {
                    $curClearDiv.remove();
                }
                $targetnode.append(doc);
                _bindLoadMoreEvent(rqStr, $targetnode);
            })
        });
    }
    var _bindLoadMoreEvent = function (rqString, $targetDiv) {
        $("a[id^='btnLoadNext']").click(function () {
            var $curBtn = $(this);
            var curPageNum = $curBtn.attr("page");
            var totalPage = $curBtn.attr("totalpage");
            var requestParam = rqString + "&page=" + curPageNum;
            //_sendRequestAndRender(requestParam,rqString, $targetDiv, $curBtn);
            _newSendRequestAndRender(requestParam, function (doc) {
                if ($curBtn) {
                    $curBtn.remove();
                }
                $targetDiv.append(doc);
                _bindLoadMoreEvent(rqString, $targetDiv);
                GetCarAreaPriceRange();
            })
        });
    }
    var _bindTabSwitchEvent = function () {
        $("#yeartag li").each(function (k, item) {
            $(item).on("click", function () {
                $(item).addClass("current").siblings().removeClass("current");
                var curYear = $(this).attr("id");
                //补充“未上市”样式：去掉中间空隙
                //if (curYear == "unlisted")
                //	$(".tags-list").css("padding-bottom", "0px");
                //else
                //	$(".tags-list").css("padding-bottom", "10px");
                if (curYear == "nosalelist") {
                    $("[data-action=stopyear]").find("span").text(nearestYear + "款");
                }

                var reqStr = "&serialAllSpell=" + serialAllSpell + "&year=" + curYear;
                _newSendRequestAndRender(reqStr, function (doc) {
                    var $targetDiv = $("#yearDiv" + k);
                    $targetDiv.html(doc);
                    $targetDiv.show();
                    $targetDiv.siblings().hide();
                    //增加关注度start
                    if ($(item).attr("id") == "all" && tuanFlag)     //全部在售且有团购活动  显示3个“团购”标签
                    {
                        _this.getTuanGouTag();
                    }
                    //增加关注度end

                    if ($(item).attr("id") == "nosalelist")    //停售，默认显示最近年份
                    {
                        $("[data-action=stopyear]").show();
                    }
                    else { $("[data-action=stopyear]").hide(); }
                    if ($(item).attr("id") == "unlisted")    //未上市，不显示筛选项
                    {
                        $("[data-action=level]").hide();
                        $("[data-action=bodyform]").hide();
                    }
                    else {
                        $("[data-action=level]").show();
                        $("[data-action=bodyform]").show();
                    }
                    resetOptions();
                    //selSubStopYear = '', selSubLevel = '', selSubBodyForm = '';
                    GetCarAreaPriceRange();
                    _bindLoadMoreEvent(reqStr, $targetDiv);
                })
            })
        });
    }
    var _newSendRequestAndRender = function (requestParam, callback) {
        //console.log("加载更多：" + requestParam);
        $.ajax({
            type: "get",
            url: "/handlers/GetMoreCarList.ashx?action=getOptionDataList&id=" + _serialId + requestParam + "&v=20170526",
            cache: true,
            success: function (datalist) {
                var tempdiv = document.createElement("div");
                tempdiv.innerHTML = datalist;
                var childs = tempdiv.childNodes;
                var doc = document.createDocumentFragment();
                while (childs.length) {
                    doc.appendChild(childs[0]);
                }
                callback(doc);
                //Modify Date 2016-6-6 zf 新增“买买买”按钮事件  
                _bindBtnMaiEvent();  //该方法依赖于主页面对应方法名

                for (var k in baoxiaoOrImport) {
                    $("a[id^='car_filter_id_" + baoxiaoOrImport[k] + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + baoxiaoOrImport[k] + "&source=myc-zs-loan-01");
                    $("a[id^='car_filterzuidi_id_" + baoxiaoOrImport[k] + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + baoxiaoOrImport[k] + "&source=myc-zs-onsale-01").html("直销特卖");
                }
                //二手车点击统计
                //$(".btn-org a").click(function(e) {
                //    var ele = $(e.target);
                //    if (ele.text() == '买二手车') {
                //        var objid = 10002;
                //        var _sentImg = new Image(1, 1);
                //        _sentImg.src = "http://carstat.bitauto.com/weblogger/img/c.gif?logtype=temptypestring&objid=" + objid + "&" + Math.random();
                //    }
                //});
                //二手车统计结束
                var compareConfig = {
                    serialid: CarCommonCSID
                };                WaitCompare.initCompreData(compareConfig);
                //_bindJoinDuibiInit();
                Bglog_InitPostLog();
                mCsSummaryV2.loadSubsidy(_serialId, citycode);
            },
            error: function (a, b, c) {
                console.log("error2");
            }
        })
    }

    //var _bindJoinDuibiInit = function () {
    //    var cookieCar = CookieHelper.getCookie("m_comparecarlist"),
    //           arrCarId = [];
    //    if (cookieCar) {
    //        arrCarId = cookieCar.split('|');
    //    }
    //    if (arrCarId.length > 0) {
    //        $(arrCarId).each(function (index, item) {
    //            $("div[id^='yearDiv']").find("#car-compare-" + item).html("已加入").off("click").closest("li").addClass("btn-gray");
    //        });
    //    }

    //    //绑定所有车款加入对比事件
    //    $("div[id^='yearDiv']").find("a[id^='car-compare']").off("click.addCompare").on("click.addCompare", function () {
    //        var carId = $(this).data("id"), carName = $(this).data("name");
    //        WaitCompare.addCompareCar(carId, carName, $(this));
    //        $(this).html("已加入").off("click").closest("li").addClass("btn-gray");
    //    });
    //}

    _this.getTuanGouTag = function () {
        var jsonAttations; //carid:carpv集合
        var max3Car = [];
        var h = [];//组织json对象
        var allHotAttations = $("#yearDiv0 a[id^='carlist']").find('.gzd-box .gz-sty i');
        $(allHotAttations).each(function (index, item) {
            var eachCarId = $(item).parents("a[id^='carlist']").attr("id").substr(8);
            var eachPv = $(item).data("pv");
            var eachCarObj = "'" + eachCarId + "':" + eachPv;
            h.push(eachCarObj);
        });
        jsonAttations = eval('({' + h.join(',') + '})');
        for (var i = 0; i < 3; i++) {
            getMaxCarFromJson(jsonAttations, max3Car);
        }
        //console.log(max3Car);
        $(max3Car).each(function (index, carId) {
            mCsSummaryV2.SortEmTags(carId, typeof tuangouTag != "undefined" ? tuangouTag : "团购");
        })
    }
    var getMaxCarFromJson = function (jsonAttations, max3Car) {
        var max = -1;
        var maxCarId;
        $.each(jsonAttations, function (carid, carpv) {
            if (max < carpv) {
                max = carpv;
                maxCarId = carid;
            }
        });
        max3Car.push(maxCarId);
        delete jsonAttations[maxCarId];
    }

};
var csCondition = new CsSummaryCondition();
var compareConfig = {
    serialid: CarCommonCSID
};
$(function () {
    WaitCompare.initCompreData(compareConfig);
});

var swiperInfoCard = new Swiper('.swiper-info-card', {
    pagination: '.pagination-info-card',
    slidesPerView: 'auto',
    paginationClickable: true,
    spaceBetween: 15,
});
$("#kongjianTab li").on('touchstart mousedown', function (e) {
    e.preventDefault();
    $("#kongjianTab .current").removeClass('current');
    $(this).addClass('current');
    $('.swiper-img-box li').hide();
    $('.swiper-img-box li').eq($(this).index()).show();

    $('#kongjianList ul').hide();
    $('#kongjianList ul').eq($(this).index()).show();
});
$("#kongjianTab li").click(function (e) {
    e.preventDefault();
});

$(function () {
    //年款切换
    csCondition.Init(["[data-action=stopyear]", "[data-action=level]", "[data-action=bodyform]"]);
});

var tuangouParam = 'mediaid=2&locationid=1&cmdId=' + GlobalSummaryConfig.SerialId + '&cityId=' + GlobalSummaryConfig.CityId;
//易团购
(function getYiTuanGou(params) {
    $.ajax({
        url: "http://api.market.bitauto.com/MessageInterface/YiTuanGou/GetYiTuanGouUrl.ashx?" + params, //?mediaid=2&locationid=1&cmdId=3999&cityId=201
        async: false,
        dataType: "jsonp",
        success: function (data) {
            var h = [];
            if (data && data.result == "yes") {
                tuanFlag = true;
                var tuangouUrl = data.url;
                var slogan = data.slogan;
                var title = data.title;
                tuangouTag = data.tag;
                h.push("<span class=\"yh-tap-txt\"><b>" + tuangouTag+"</b></span>");
                h.push("<div class=\"cont-box\">");
                h.push("<h4>" + title + "</h4>");
                h.push("<p>" + slogan + "</p>");
                h.push("</div>");
                $(".prefer-tim").html(h.join(''));
                $(".prefer-tim").on("click", function () {
                    window.location.href = tuangouUrl;
                });
                csCondition.getTuanGouTag();
            }
        }
    });
})(tuangouParam);

//行情
mCsSummaryV2.loadNewsHangqingV2(GlobalSummaryConfig.SerialId, "@(serialEntity.ShowName)", citycode, 4);
mCsSummaryV2.loadSubsidy(GlobalSummaryConfig.SerialId, citycode);
$(function () {
    // 浏览过的车型
    Bitauto && Bitauto.Login && Bitauto.Login.onComplatedHandlers && Bitauto.Login.onComplatedHandlers.push(function (loginResult) {
        Bitauto.UserCars.setUserLoginState({
            isLogin: loginResult.isLogined
        });
        //添加浏览过的车
        Bitauto.UserCars.addViewedCars(GlobalSummaryConfig.SerialId);
    });
});
var viewedList = "";
for (var i = 0; i < Bitauto.UserCars.viewedcar.arrviewedcar.length && i < 4; i++) {
    viewedList += Bitauto.UserCars.viewedcar.arrviewedcar[i];
    if (i != Bitauto.UserCars.viewedcar.arrviewedcar.length - 1) {
        viewedList += ",";
    }
}
loadViewedCars(viewedList);


// 看了还看
if ($("#m-tabs-kankan li").length >= 2) {
    var tabsSwiperKankan = new Swiper('.swiper-container-kankan', {
        calculateHeight: true,
        loop: true,
        onInit: function (swiper) {
            $("#m-tabs-kankan li").eq((swiper.activeIndex - 1) % (swiper.slides.length - 2)).addClass('current');
        },
        onSlideChangeEnd: function (swiper) {
            $("#m-tabs-kankan .current").removeClass('current');
            $("#m-tabs-kankan li").eq((swiper.activeIndex - 1) % (swiper.slides.length - 2)).addClass('current');
        }
    });
    $("#m-tabs-kankan li").on('touchstart mousedown', function (e) {
        e.preventDefault();
        $("#m-tabs-kankan .current").removeClass('current');
        $(this).addClass('current');
        tabsSwiperKankan.swipeTo($(this).index());
    });
    $("#m-tabs-kankan li").click(function (e) {
        e.preventDefault();
    });
}
// 应用下载
var mySwiperApp = new Swiper('#m-app-part-scroll', {
    pagination: '.pagination-app',
    loop: true,
    grabCursor: true,
    paginationClickable: true
});

$(document).ready(function () {
    if ($("#m-app-part-scroll").find("li").length < 4) {
        $(".pagination-app").hide();
    }
});
