var isMicroMessager = function () {
    var ua = window.navigator.userAgent.toLowerCase();
    if (ua.match(/MicroMessenger/i) == "micromessenger") {
        return true;
    } else {
        return false;
    }
}();

var $fullpage = $("#fullpage"), section = $fullpage.find(".section");
var settimeForHide = 0;

var user_ChangeCar = { "page1": "85.6.748", "page2": "85.6.749", "page3": "85.6.750", "page4": "85.6.751", "page5": "85.6.752", "page6": "85.6.753", "page7": "85.6.754", "page8": "85.6.755", "page9": "85.6.757", "page10": "85.6.758", "page11": "85.6.759", "page12": "85.6.760", "page13": "85.6.761", "page14": "85.6.756" };


var xunjia = { "page13": "85.6.902", "page12": "85.6.901", "page11": "85.6.900", "page10": "85.6.899", "page9": "85.6.898", "page14": "85.6.897", "page8": "85.6.896", "page7": "85.6.895", "page6": "85.6.894", "page5": "85.6.893", "page4": "85.6.892", "page3": "85.6.891" }

function initFullPage() {
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
        fixedElements: ".menu, .menu_box, .menu_box_bg ",
        slidesNavigation: true,
        slidesNavPosition: "bottom",
        afterLoad: function (anchorLink, index) {

            /*star 页面统计*/
            try {
                bglogexec(); //统计数据方法 2015-10-09
            } catch (e) {
                //console.log(e.message);
            }
            /*end 页面统计*/
            
            switch (Config.CustomizationTypeEnum[Config.currentCustomizationType]) {
                case Config.CustomizationTypeEnum["User"]:
                    /*star 用户版换车按钮点击统计特殊处理*/
                    $("#nav-change").attr("onclick", "BglogPostLog('" + user_ChangeCar[anchorLink] + "');");
                    /*end 用户版换车按钮点击统计*/
                    
                    //start 询价统计特殊处理
                    $("#nav-price").attr("onclick", "BglogPostLog('" + xunjia[anchorLink] + "');");
                    //end 询价统计
                    break;
                    
            }

            /*star 分享*/
            if (isMicroMessager) { //解决只在微信里弹出分享提示
                if (settimeForHide != -1) {
                    clearTimeout(settimeForHide);
                    settimeForHide = setTimeout(function () {
                        $("#sharefloat").show(800);
                        setTimeout(function () { $("#sharefloat").hide(800); }, 4000);
                        settimeForHide = -1;
                    }, 3000);
                };
            }
            /*end 分享*/

            var logowrapper = $(".fixed_box");
            var menuBtn = $("#menubutton");

            /*star 定制版处理逻辑*/
            switch (Config.CustomizationTypeEnum[Config.currentCustomizationType]) {
                case Config.CustomizationTypeEnum["User"]:
                    //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
                    if ("page1" == anchorLink ) {
                        logowrapper.fadeIn(); //淡出
                        menuBtn.fadeOut();
                    } else {
                        logowrapper.fadeOut(); //淡入
                        menuBtn.fadeIn();
                        $("#navigation").fadeIn();
                    }
                    break;
            }
            /*end 定制版处理逻辑*/

        },
        afterSlideLoad: function (anchorLink, index, slideIndex, direction) {
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
        onLeave: function (index, nextIndex, direction) {

            $("#navigation").fadeOut();

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


//用户板视频图片 强制设置宽高比例
$(document).ready(function () {
    $("#video-pic-wall img").each(function () {
        $(this).height($(this).width() / 1.7857142857);
    });
});

//右上整站导航js
$(document).ready(function () {
    var navbox = $("#navigation-box");
    var navbg = $("#navigation-bg");
    var navmain = $("#nav-main");
    var navclose = $("#nav-close");

    $(".navigation-box-button a").click(function() {
        navbox.css("top", -navbox.outerHeight());
        navbox.remove("navigation-box-show");
        navbg.hide();
        navmain.show();
    });

    navbox.css("top", -navbox.outerHeight());
    navmain.click(function () {
        navbox.css("top", navbox.outerHeight() - navbox.outerHeight());
        navbox.addClass("navigation-box-show");
        navbg.show();
        navmain.hide();
    });
    navclose.click(function () {
        navbox.css("top", -navbox.outerHeight());
        navbox.remove("navigation-box-show");
        navbg.hide();
        navmain.show();
    });
    navbg.click(function () {
        $(this).hide();
        navbox.css("top", -navbox.outerHeight());
        navbox.remove("navigation-box-show");
        navmain.show();
    });

});