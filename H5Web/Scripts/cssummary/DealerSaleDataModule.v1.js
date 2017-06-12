var requestCount = 0;
var timeout = Config.timeout;
var dealersale = {
    resetSlides: function(name) {
        //var index = Config.auchors.indexOf(name);
        //$("#fullpage").fullpage.resetSlides(index);
        if (Config.auchors[Config.auchors.length - 1] === name) {
            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
        }
    },
    removePage: function (name) {
        $("div[data-anchor='" + name + "']").remove();
        $("#menu_box a[href='#" + name + "']").parent().remove();
        var index = Config.auchors.indexOf(name);
        Config.auchors.splice(index, 1);
    },
    //图片和视频
    waiguansheji: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetImageAndVideoData.ashx?csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data !== "" && data != null) {
                    $("#picvideotmpl").tmpl({ "data": data }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                    $("#video-pic-wall img").each(function() {
                        $(this).height($(this).width() / 1.7857142857);
                    });
                    break;
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //亮点配置
    liangdianpeizhi: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data !== "" && data != null && data.length > 0) {
                    $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //车型报价
    chekuaibaojia: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalecarprice&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (data!==""&&checkData(data)) {
                    $("#dealersalecarprice").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //热门促销
    remencuxiao: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersalenews&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (checkData(data)) {
                    $("#dealersalenews").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //热销车型(看了还看)
    rexiaochexing: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleserial&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (checkData(data)) {
                    $("#dealersaleserial").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //商家店铺
    shangjiadianpu: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetDataAsynV3.ashx?service=dealersale&method=dealersaleshop&dealerpersonid=" + Config.dealerpersonid + "&csid=" + Config.serialId + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                if (checkData(data)) {
                    $("#dealersaleshop").tmpl({ html: data.trim() }).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    //评测新闻
    pingcexinwen: function(name) {
        $.ajax({
            type: "GET",
            url: "/handlers/GetNewsListFilter.ashx?top=10&csid=" + Config.serialId + "&dealerid=" + Config.dealerId + "&v=" + Config.version + "&",
            timeout: timeout, //超时时间设置，单位毫秒
            success: function(data) {
                var length = data.length;
                var obj = {};
                obj["isCustomization"] = false; //是否为定制版
                if (Config.currentCustomizationType !== "User" || Config.isAd === 0) {
                    obj["isCustomization"] = true;
                }
                if (length > 0) {
                    var listgroup = [];
                    listgroup.push(data.slice(0, 3));
                    if (length > 3) {
                        if (obj["isCustomization"] === true) {
                            listgroup.push(data.slice(3, 6));
                        } else {
                            listgroup.push(data.slice(3, 5));
                        }
                    }

                    if (length > 6) {
                        if (obj["isCustomization"] === true) {
                            listgroup.push(data.slice(6, 9));
                        } else {
                            listgroup.push(data.slice(6));
                        }
                    }

                    obj["listgroup"] = listgroup;
                    //$("#pingcenewstmp").tmpl(obj).appendTo("div[data-anchor='" + name + "']");
                    $("#pingcetmp20160413").tmpl(obj).appendTo("div[data-anchor='" + name + "']");
                    $("#menu_box a[href='#" + name + "']").parent().show();
                    dealersale.resetSlides(name);
                } else {
                    dealersale.removePage(name);
                }
            },
            complete: function(xhr, status) {
                requestCount++;
                switch (status) {
                case "success":
                case "timeout":
                case "error":
                    break;
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                dealersale.removePage(name);
            }
        });
    },

    initFullPage: function () {

        $("#menu_box a[href='#page1']").parent().show();

        var $fullpage = $("#fullpage");
        var settimeForHide = 0;
        $fullpage.fullpage({
            anchors: Config.auchors,
            menu: "#menu",
            css3: true,
            normalScrollElementTouchThreshold: 5,
            scrollingSpeed: 700,
            animateAnchor: false,
            controlArrows: false,
            verticalCentered: false,
            loopHorizontal: false,
            fixedElements: ".fixed_box, .menu, .menu_box, .menu_box_bg ",
            slidesNavigation: true,
            slidesNavPosition: "bottom",
            afterSlideLoad: function(anchorLink, index, slideIndex, direction) {
                /*star 侧滑统计(分页)*/
                if (slideIndex.length > 0 && slideIndex.indexOf("-") > 0) {
                    var slideindex = slideIndex.split("-")[1];
                    if (slideindex && slideindex > 1) {
                        try {
                            bglogexec(); //统计数据方法 2015-10-12
                        } catch (e) {
                        }
                    }
                }
                /*end 侧滑统计*/
            },
            afterLoad: function(anchorLink, index) {

                /*star 页面统计*/
                try {
                    bglogexec(); //统计数据方法 2015-10-09
                } catch (e) {
                    //console.log(e.message);
                }
                /*end 页面统计*/

                $("." + anchorLink).find("img[data-src]").each(function() {
                    $(this).attr("src", $(this).data("src"));
                    $(this).removeAttr("data-src");
                });

                /*star 分享*/
                //if (isMicroMessager) { //解决只在微信里弹出分享提示
                //    if (settimeForHide != -1) {
                //        clearTimeout(settimeForHide);
                //        settimeForHide = setTimeout(function() {
                //            $("#sharefloat").show(800);
                //            setTimeout(function() { $("#sharefloat").hide(800); }, 4000);
                //            settimeForHide = -1;
                //        }, 3000);
                //    };
                //}
                /*end 分享*/
            }
            //
        });
    }
};
//dealersale.initFullPage();

dealersale.waiguansheji("page3");
dealersale.pingcexinwen("page4");
dealersale.liangdianpeizhi("page5");
dealersale.chekuaibaojia("page6");
dealersale.remencuxiao("page7");
dealersale.rexiaochexing("page8");
dealersale.shangjiadianpu("page9");


var timeFun = setInterval(function () {
    if (requestCount === 7) {
        clearInterval(timeFun);
        $("#menubutton").show();

        dealersale.initFullPage();
    }
}, 5);

var $boxbg = $("#menu_box_bg"), $menubox = $("#menu_box");
$("#menubutton").click(function () {
    $("#menu_box").removeClass("menu_box_down").addClass("menu_box_hover");
    $("#menu_box_bg").show();
});

$boxbg.click(function () {
    if ($menubox.attr("class").indexOf("menu_box_hover") > -1)
        $menubox.removeClass("menu_box_hover").addClass("menu_box_down");
    $boxbg.hide();
    $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
});

$("#menu_box").bind("click", function (e) {
    var ele = e.target;
    if (ele.tagName == "A") {
        $boxbg.click();
    }
});