function getQueryStringForH5WT(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2];
    return null;
}

var WTmc_id = "";
var tempVarid = getQueryStringForH5WT("WT.mc_id");
if (tempVarid != null) {
    WTmc_id = tempVarid;
}
var WTmc_jz = "";
var tempVarjz = getQueryStringForH5WT("WT.mc_jz");
if (tempVarjz != null) {
    WTmc_jz = tempVarjz;
}

var CsSumamryCallBack = function() {
    return {
        HuiMaiChe: function() {
            var cityId = !!bit_locationInfo ? bit_locationInfo.cityId : 0;
            $.ajax({
                type: "get",
                url: "http://www.huimaiche.com/api/GetCarSerialPrice.ashx",
                cache: true,
                dataType: "jsonp",
                data: { cityId: cityId, csid: summary.serialId },
                jsonpCallback: "CsSumamryCallBack.callBackHuiMaiChe",
                success: function() {
                }
            });
        },
        callBackHuiMaiChe: function(data) {
            if (!!data) {
                var html = [];
                html.push("<div class=\"sale2\">");
                html.push("<h6>易车·惠买车：底价购车</h6>");
                html.push("<p class=\"sale_high\">" + data.buyer + "人正在购买，平均节省" + data.save + "万</p>");
                html.push("<p>买车送豪礼！服务更贴心！</p>");
                html.push("</div>");
                if (WTmc_id && WTmc_id != "") {
                    html.push("<button><a href=\"" + data.waplink + "&tracker_u=269_ycdsj_" + WTmc_id + "\">立即购买</a></button>");
                } else if (WTmc_jz && WTmc_jz != "") {
                    html.push("<button><a href=\"" + data.waplink + "&tracker_u=269_ycdsj_" + WTmc_jz + "\">立即购买</a></button>");
                } else {
                    html.push("<button><a href=\"" + data.waplink + "&tracker_u=269_ycdsj\">立即购买</a></button>");
                }
                // html.push('<button><a href="' + data.waplink + '&tracker_u=269_ycdsj">立即购买</a></button>');
                html.push("<div class=\"sale2-line\"></div>");
                $("#youhuidiv").prepend(html.join(""));
                // $("#priceinfowrapper").html(html.join(''));
            } else {
                // $("#priceinfowrapper").hide();
                var html = [];
                html.push("<ul class=\"noneinfo\">");
                html.push("<li><img src=\"http://img1.bitauto.com/uimg/4th/img/youhui1.png\"><p>省时省力</p></li>");
                html.push("<li><img src=\"http://img1.bitauto.com/uimg/4th/img/youhui2.png\"><p>品质保证</p></li>");
                html.push("<li class=\"nob\"><img src=\"http://img1.bitauto.com/uimg/4th/img/youhui3.png\"><p>省钱省心</p></li>");
                html.push("<li class=\"nob\"><img src=\"http://img1.bitauto.com/uimg/4th/img/youhui4.png\"><p>安全随时退</p></li>");
                html.push("</ul>");
                $("#youhuidiv").prepend(html.join(""));
            }
        }
    };
}();

var csSummary = function(params) {
    var defaults = {
        auchors: ["page1", "page2", "page3", "page4", "page5", "page6", "page7", "page8", "page9", "page10", "page11", "page12"],
        isexistcolor: true,
        colors: [],
        serialId: 0,
        dealerId: 0,
        brokerId: 0
    };
    $.extend(defaults, params);
    var $boxbg = $("#menu_box_bg"), $menubox = $("#menu_box");
    $("#menubutton").click(function() {
        $("#menu_box").removeClass("menu_box_down").addClass("menu_box_hover");
        $("#menu_box_bg").show();
    });
    $boxbg.click(function() {
        if ($menubox.attr("class").indexOf("menu_box_hover") > -1)
            $menubox.removeClass("menu_box_hover").addClass("menu_box_down");
        $boxbg.hide();
        $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
    });

    $("#menu_box").bind("click", function(e) {
        var ele = e.target;
        if (ele.tagName == "A") {
            $boxbg.click();
        }
    });

    $("#sharebutton").on("click", function(ev) {
        ev.preventDefault();
        dcsMultiTrack("DCS.dcsuri", "/h5car/onclick/sharebutton.onclick", "WT.ti", "车型综述页购车服务分享");
        $("#standard_wx_pop").addClass("standard_wx_pop_start").stop().fadeIn();
        $boxbg.stop().fadeIn();
    });

    if (WTmc_id && WTmc_id != "") {
        $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + summary.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_id=" + WTmc_id);
    } else if (WTmc_jz && WTmc_jz != "") {
        $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + summary.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_jz=" + WTmc_jz);
    } else {
        $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + summary.serialId + "&cityId=" + bit_locationInfo.cityId);
    }

    // $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + summary.serialId + "&cityId=" + bit_locationInfo.cityId);
    //汽车优惠信息
    //(function() {
    //    var cityId = !!bit_locationInfo ? bit_locationInfo.cityId : 0;
    //    $.ajax({
    //        type: "get",
    //        url: "http://api.car.bitauto.com/Mai/GetSerialParallelAndSell.ashx",
    //        cache: true,
    //        dataType: "jsonp",
    //        data: { cityId: cityId, serialId: defaults.serialId },
    //        jsonpCallback: "CsSumamryCallBack.callBackMall",
    //        success: function() {
    //        }
    //    });
    //}());
    CsSumamryCallBack.HuiMaiChe();
    (function($) {
        //X向滚动
        $.fn.dragX = function(options) {
            var setting = {
                onstart: null,
                onmove: null,
                onend: null,
            };
            options = Object.extend(options, setting);
            var $this = this;
            $this.X = $this.disX = 0;
            $this.touches({
                touchstart: function(ev) {
                    ev.preventDefault();
                    $this.disX = ev.targetTouches[0].pageX - $this.X;
                    options.onstart && options.onstart.call($this, $this.disX, ev.targetTouches[0].pageX);
                },
                touchmove: function(ev) {
                    ev.preventDefault();
                    $this.X = ev.targetTouches[0].pageX - $this.disX;
                    options.onmove && options.onmove.call($this, $this.X);
                },
                touchend: function(ev) {
                    options.onend && options.onend.call($this, $this.X, ev.changedTouches[0].pageX);
                }
            });
        }; //触摸屏事件
        $.fn.touches = function(options) {
            var setting = {
                init: null, //初始化
                touchstart: null, //按下
                touchmove: null, //滑动
                touchend: null //抬起
            };
            options = Object.extend(options, setting);
            var $this = this, touchesDiv = $this[0];
            touchesDiv.addEventListener("touchstart", function(ev) {
                options.touchstart && options.touchstart.call($this, ev);

                function fnMove(ev) {

                    options.touchmove && options.touchmove.call($this, ev);
                }

                function fnEnd(ev) {
                    options.touchend && options.touchend.call($this, ev);
                    document.removeEventListener("touchmove", fnMove, false);
                    document.removeEventListener("touchend", fnEnd, false);
                }

                document.addEventListener("touchmove", fnMove, false);
                document.addEventListener("touchend", fnEnd, false);
                return false;
            }, false);
            options.init && options.init.call($this);
        }; //手势方向(X轴)
        $.fn.directionX = function(options) {
            var setting = {
                init: null,
                selectfn: null,
                max: 30
            };
            options = Object.extend(options, setting);
            var $this = this;
            if ($this.length == 0) return;
            $this.ox = 0;
            $this.dragX({
                onstart: function(x) {
                    $this.ox = 0;
                },
                onmove: function(x) {
                    if (x - $this.ox < -options.max) {
                        clearTimeout($this.timeout);
                        $this.timeout = setTimeout(function() { options.selectfn && options.selectfn.call($this, "left"); }, 300);

                    } else if (x - $this.ox > options.max) {
                        clearTimeout($this.timeout);
                        $this.timeout = setTimeout(function() { options.selectfn && options.selectfn.call($this, "right"); }, 300);
                    }
                    if ($this.ox == 0) {
                        $this.ox = x;
                    }
                },
                onend: function() {
                    $this.ox = 0;
                }
            });
            options.init && options.init.call($this);
        };
    })(jQuery);

    $(function() {
        $(".standard_car_pic_1").directionX({
            init: function() {
                var $this = this, imgs = $this.find("img"), $message = $this.next().children(0);;
                $this.index = 0;
                $this.on("setIndex", function(event, i) {
                    imgs.each(function(index, curr) {
                        var $current = $(curr);
                        if (index == i) {
                            $current.fadeIn();
                        } else {
                            $current.fadeOut();
                        }
                    });
                    $this.trigger("setMessage", i);
                    $this.index = i;
                });

                $this.on("prev", function(event) {
                    $this.index = $this.index - 1;
                    if ($this.index < 0) {
                        $this.index = imgs.length - 1;
                    }
                    $this.trigger("setIndex", $this.index);
                });

                $this.on("next", function(event) {
                    $this.index = $this.index + 1;
                    if ($this.index > imgs.length - 1) {
                        $this.index = 0;
                    }
                    $this.trigger("setIndex", $this.index);
                });


                var as = $this.next().next().children();
                $this.on("setMessage", function(event, i) {
                    var $a = as.eq(i);
                    as.removeClass("current");
                    $a.addClass("current");
                    $message.html($a.children(0).attr("data-value"));
                }).trigger("setMessage", $this.index);

                as.each(function(index, curr) {
                    var $current = $(curr);
                    (function($o, i) {
                        $o.on("click", function(ev) {
                            var $a = $(this);
                            ev.preventDefault();
                            $this.trigger("setIndex", i);

                        });
                    })($current, index);

                });

            },
            selectfn: function(v) {
                var $this = this;
                switch (v) {
                case "left":
                    $this.trigger("next");
                    break;
                case "right":
                    $this.trigger("prev");
                    break;
                }
            }
        });
    });

    $boxbg.touches({
        touchstart: function(ev) {
            ev.preventDefault();
            $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
            $boxbg.stop().fadeOut();

            if ($menubox.attr("class").indexOf("menu_box_hover") > -1) {
                $menubox.removeClass("menu_box_hover").addClass("menu_box_down");
            }
        },
        touchmove: function(ev) {
            ev.preventDefault();
        }
    });


    var settimeForHide = 0;
    var $fullpage = $("#fullpage"), section = $fullpage.find(".section");

    function initFullPage() {
        $fullpage.fullpage({
            sectionsColor: ["#fff", "#fff", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa", "#f8f8fa"],
            anchors: defaults.auchors,
            menu: "#menu",
            css3: true,
            normalScrollElementTouchThreshold: 5,
            scrollingSpeed: 700,
            controlArrows: false,
            verticalCentered: false,
            loopHorizontal: false,
            fixedElements: ".fixed_box, .menu, .menu_box, .menu_box_bg ",
            slidesNavigation: true,
            slidesNavPosition: "bottom",
            afterLoad: function(anchorLink, index) {
                var shareBtn = $("#sharebutton");
                if (anchorLink == "page9") {
                    shareBtn.show();
                } else {
                    shareBtn.hide();
                    $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
                    $boxbg.stop().fadeOut();
                }
                $("." + anchorLink).find("img[data-src]").each(function() {
                    $(this).attr("src", $(this).data("src"));
                    $(this).removeAttr("data-src");
                });
                if (settimeForHide == -1) {
                    return false;
                };
                clearTimeout(settimeForHide);
                settimeForHide = setTimeout(function() {
                    $("#sharefloat").show(800);
                    setTimeout(function() { $("#sharefloat").hide(800); }, 4000);
                    settimeForHide = -1;
                }, 3000);
            },
            onLeave: function(index, nextIndex, direction) {
                var logowrapper = $(".fixed_box");
                var menuBtn = $("#menubutton");

                if (nextIndex == 2) {
                    if (defaults.auchors.indexOf("page2") != -1) {
                        logowrapper.fadeIn();
                        menuBtn.fadeOut();
                    } else {
                        menuBtn.fadeIn();
                        logowrapper.fadeOut();
                    }
                }
                if (nextIndex == 1 && direction == "up") {
                    logowrapper.fadeIn();
                    menuBtn.fadeOut();
                }
                if (index == 2 && direction == "down") {
                    logowrapper.fadeOut();
                    menuBtn.fadeIn();
                }
                if (index == 1 && nextIndex !== 2 && direction == "down") {
                    logowrapper.fadeOut();
                    menuBtn.fadeIn();
                }
                var $section = section.eq(nextIndex - 1), $slidesNav = $section.find(".fp-slidesNav");
                if ($section.find(".slide").length > 1) {
                    $slidesNav.show();
                } else {
                    $slidesNav.hide();
                }
            }
        });
    }

    initFullPage();

    //获取车款列表
    function GetCarList() {
        $.get("/handlers/getcarlist.ashx?top=19&csid=" + defaults.serialId, function(data) {
            if (!data) {
                return;
            }
            var length = data.carlist.length;
            var listGroup = [];
            var list = data.carlist;
            for (var i = 0; i < Math.ceil(length / 4); i++) {

                listGroup.push({ "index": i, "carlist": list.slice(i * 4, (i + 1) * 4) });
            }
            var tmpl = $("#carlisttmp").tmpl({ "imagecount": data.count, "listgroup": listGroup }).appendTo("div[data-anchor='page7']");
            $("#fullpage").fullpage.resetSlides(6);
        });

    }

    GetCarList();

    //获取看了还看信息
    function GetSeeAgain() {
        $.get("/handlers/SeeAgain.ashx?csid=" + defaults.serialId, function(data) {
            if (!data) {
                return;
            }
            $("#seeagaintmp").tmpl({ "list": data }).appendTo("div[data-anchor='page12']");
        });
    }

    GetSeeAgain();

    //获取评测导购新闻
    function GetPingceNews() {
        $.get("/handlers/GetNewsList.ashx?top=10&csid=" + defaults.serialId, function(data) {
            var length = data.length;
            if (length == 0)
                return;
            var obj = {};
            if (length >= 3)
                obj["isSecondAd"] = true;
            var listgroup = [];
            listgroup.push(data.slice(0, 3));
            if (length > 3)
                listgroup.push(data.slice(3, 6));
            if (length > 6)
                listgroup.push(data.slice(6));
            obj["listgroup"] = listgroup;
            $("#pingcenewstmp").tmpl(obj).appendTo("div[data-anchor='page4']");
            $("#fullpage").fullpage.resetSlides(3);
        });
    }

    GetPingceNews();

    function GetPeizhi() {
        $.get("/handlers/GetSerialSparkle.ashx?top=11&csid=" + defaults.serialId, function(data) {
            $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='page5']");
        });
    }

    GetPeizhi();

    function GetYouHuiGouChe() {
        var cityId = !!bit_locationInfo ? bit_locationInfo.cityId : 0;
        var logo = "http://image.bitautoimg.com/carchannel/pic/cspic/" + defaults.serialId + ".jpg";
        var huimaiche = { title: "获4S店底价，在线订车", html: "" };
        var yichemall = { title: "买车无难事，网上直销", html: "" };
        var yichehui = { title: "品牌特卖-官方品牌旗舰店", html: "" };
        var yicheloan = { title: "贷款购车优惠方案", html: "" };
        var yicheershou = { title: "购买二手车，价低有保障", html: "" };
        var obj = {
            logo: logo,
            huimaiche: huimaiche,
            yichemall: yichemall,
            yichehui: yichehui,
            yicheloan: yicheloan,
            yicheershou: yicheershou
        };
        var num = 0;
        //惠买车
        $.get("/handlers/GetDataAsynV3.ashx?service=huimaiche&method=youhuigouce&csid=" + defaults.serialId + "&cityid=" + cityId, function(data) {
            num++;
            if (data.length == 0)
                obj.huimaiche = undefined;
            else
                obj.huimaiche.html = data;
        });

        //商城
        $.get("/handlers/GetDataAsynV3.ashx?service=yichemall&method=youhuigouce&csid=" + defaults.serialId + "&cityid=" + cityId, function(data) {
            num++;
            if (data.length == 0)
                obj.yichemall = undefined;
            else
                obj.yichemall.html = data;
        });
        //易车惠
        num++;
        //贷款
        $.get("/handlers/GetDataAsynV3.ashx?service=daikuan&method=youhuigouce&csid=" + defaults.serialId + "&cityid=" + cityId, function(data) {
            num++;
            if (data.length == 0)
                obj.yicheloan = undefined;
            else
                obj.yicheloan.html = data;
        });
        //二手车
        $.get("/handlers/GetDataAsynV3.ashx?service=ershouche&method=youhuigouce&csid=" + defaults.serialId + "&cityid=" + cityId, function(data) {
            num++;
            if (data.length == 0)
                obj.yicheershou = undefined;
            else
                obj.yicheershou.html = data;
        });
        var seconds = 0;
        var timer = setInterval(function() {
            seconds += 100;
            if (num >= 5 || seconds >= 5000) {
                $("#gouchetmpl").tmpl(obj).appendTo("div[data-anchor='page8']");
                $("#fullpage").fullpage.resetSlides(7);
                clearInterval(timer);
            }
        }, 100);
    }

    GetYouHuiGouChe();

    function GetKouBei() {
        $.get("/handlers/GetEditorComment.ashx?csid=" + defaults.serialId, function(data) {
            $("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='page6']");
            $("#fullpage").fullpage.resetSlides(5);
        });
    }

    GetKouBei();
};