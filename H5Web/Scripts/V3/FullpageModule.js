﻿define("fullpage", function(require) {

    var isMicroMessager = function() {
        var ua = window.navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    }();

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

    if (isMicroMessager) {
        $("#sharebutton").on("click", function(ev) {
            ev.preventDefault();
            dcsMultiTrack("DCS.dcsuri", "/h5car/onclick/sharebutton.onclick", "WT.ti", "车型综述页购车服务分享");
            $("#standard_wx_pop").addClass("standard_wx_pop_start").stop().fadeIn();
            $boxbg.stop().fadeIn();
        });
    }

    var $fullpage = $("#fullpage"), section = $fullpage.find(".section");
    var settimeForHide = 0;

    function initFullPage() {
        $fullpage.fullpage({
            anchors: Config.auchors,
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

            afterLoad: function (anchorLink, index) {

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
                if (settimeForHide == -1) {
                    return;
                };
                clearTimeout(settimeForHide);
                settimeForHide = setTimeout(function() {
                    $("#sharefloat").show(800);
                    setTimeout(function() { $("#sharefloat").hide(800); }, 4000);
                    settimeForHide = -1;
                }, 3000);
                /*end 分享*/

                var logowrapper = $(".fixed_box");
                var menuBtn = $("#menubutton");

                /*star 定制版处理逻辑*/
                switch (Config.CustomizationTypeEnum[Config.currentCustomizationType]) {
                case Config.CustomizationTypeEnum["CheZhuKa"]:

                    if (anchorLink == "page1" || anchorLink == "page2") {
                        logowrapper.fadeIn(); //淡出
                    } else {
                        logowrapper.fadeOut(); //淡入
                    }
                    
                    break;
                case Config.CustomizationTypeEnum["User"]:
                    //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
                    if ("page1" == anchorLink || "page2" == anchorLink) {
                        logowrapper.fadeIn(); //淡出
                        menuBtn.fadeOut();
                    } else {
                        logowrapper.fadeOut(); //淡入
                        menuBtn.fadeIn();
                    }
                    break;
                }


                switch (Config.CustomizationTypeEnum[Config.currentCustomizationType]) {
                case Config.CustomizationTypeEnum["User"]:
                    if (isMicroMessager) {
                        var shareBtn = $("#sharebutton");
                        if (anchorLink == "page8") {
                            shareBtn.show();
                        } else {
                            shareBtn.hide();
                            $("#standard_wx_pop").removeClass("standard_wx_pop_start").stop().fadeOut();
                            $boxbg.stop().fadeOut();
                            if ($menubox.attr("class").indexOf("menu_box_hover") > -1)
                                $menubox.removeClass("menu_box_hover").addClass("menu_box_down");
                        }
                    }
                    break;
                }
                /*end 定制版处理逻辑*/
                
            },

            onLeave: function(index, nextIndex, direction) {
                var logowrapper = $(".fixed_box");
                var menuBtn = $("#menubutton");
                /*star 定制版处理逻辑*/
                switch (Config.CustomizationTypeEnum[Config.currentCustomizationType]) {
                case Config.CustomizationTypeEnum["CheZhuKa"]:
                    if (Config.auchors.indexOf("page1") == (nextIndex - 1) || Config.auchors.indexOf("page2") == (nextIndex - 1)) {
                        logowrapper.fadeIn(); //淡出
                    } else {
                        logowrapper.fadeOut(); //淡入
                    }
                    break;
                case Config.CustomizationTypeEnum["User"]:
                    //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
                    if (Config.auchors.indexOf("page1") == (nextIndex - 1) || Config.auchors.indexOf("page2") == (nextIndex - 1)) {
                        logowrapper.fadeIn(); //淡出
                        menuBtn.fadeOut();
                    } else {
                        logowrapper.fadeOut(); //淡入
                        menuBtn.fadeIn();
                    }
                    break;
                }
                /*end 定制版处理逻辑*/
            }
            //
        });
    }

    initFullPage();
});