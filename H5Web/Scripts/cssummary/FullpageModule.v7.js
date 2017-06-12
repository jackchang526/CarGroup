var $fullpage = $("#fullpage"), section = $fullpage.find(".section");

var settimeForHide = 0;

var user_ChangeCar_mtaqqcom = { "page1": "a", "page3": "g", "page4": "e", "page5": "f", "page6": "h", "page7": "b", "page8": "c", "page9": "i", "page10": "j", "page11": "k", "page12": "l", "page13": "m", "page14": "d" };
var user_XunJia_mtaqqcom = {"page3": "f", "page4": "d", "page5": "e", "page6": "g", "page7": "a", "page8": "b", "page9": "h", "page10": "i", "page11": "j", "page12": "k", "page13": "l", "page14": "c" };
var user_DuiBi_mtaqqcom = {"page3": "f", "page4": "d", "page5": "e", "page6": "g", "page7": "a", "page8": "b", "page9": "h", "page10": "i", "page11": "j", "page12": "k", "page13": "l", "page14": "c" };
var user_DaoHang_mtaqqcom = {"page3": "f", "page4": "d", "page5": "e", "page6": "g", "page7": "a", "page8": "b", "page9": "h", "page10": "i", "page11": "j", "page12": "k", "page13": "l", "page14": "c" };

var user_ChangeCar = { "page1": "85.6.748", "page2": "85.6.749", "page3": "85.6.750", "page4": "85.6.751", "page5": "85.6.752", "page6": "85.6.753", "page7": "85.6.754", "page8": "85.6.755", "page9": "85.6.757", "page10": "85.6.758", "page11": "85.6.759", "page12": "85.6.760", "page13": "85.6.761", "page14": "85.6.756" };
var xunjia = { "page13": "85.6.902", "page12": "85.6.901", "page11": "85.6.900", "page10": "85.6.899", "page9": "85.6.898", "page14": "85.6.897", "page8": "85.6.896", "page7": "85.6.895", "page6": "85.6.894", "page5": "85.6.893", "page4": "85.6.892", "page3": "85.6.891", "page1": "85.6.1282" };
var duibi = { "page13": "85.6.930", "page12": "85.6.929", "page11": "85.6.928", "page10": "85.6.927", "page9": "85.6.926", "page14": "85.6.925", "page8": "85.6.924", "page7": "85.6.923", "page6": "85.6.922", "page5": "85.6.921", "page4": "85.6.920", "page3": "85.6.919", "page1": "85.6.918" };

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

            /*star 用户版换车按钮点击统计特殊处理*/
            $("#nav-change").attr("onclick", "BglogPostLog('" + user_ChangeCar[anchorLink] + "');MtaH5.clickStat('b1',{'" + user_ChangeCar_mtaqqcom[anchorLink] + "':'true'})");
            /*end 用户版换车按钮点击统计*/

            //start 询价统计特殊处理
            if (typeof user_XunJia_mtaqqcom[anchorLink]!="undefined") {
                $("#nav-price").attr("onclick", "BglogPostLog('" + xunjia[anchorLink] + "');MtaH5.clickStat('b2',{'" + user_XunJia_mtaqqcom[anchorLink] + "':'true'})");
            } else {
                $("#nav-price").attr("onclick", "BglogPostLog('" +xunjia[anchorLink]+ "');");
            }
            //end 询价统计

            //start 对比
            if (typeof user_DuiBi_mtaqqcom[anchorLink] != "undefined") {
                $("#nav-compare").attr("onclick", "BglogPostLog('" +duibi[anchorLink]+ "');MtaH5.clickStat('b3',{'" +user_DuiBi_mtaqqcom[anchorLink] + "':'true'})");
            } else {
                $("#nav-compare").attr("onclick", "BglogPostLog('" + duibi[anchorLink]+ "');");
            }
            //end 对比

            //导航
            if (typeof user_DaoHang_mtaqqcom[anchorLink] != "undefined") {
                $("#nav-main").attr("onclick", "MtaH5.clickStat('b3',{'" + user_DaoHang_mtaqqcom[anchorLink]+ "':'true'})");
            } else {
                $("#nav-main").removeAttr("onclick");
            }
            //


            var logowrapper = $(".fixed_box");
            var menuBtn = $("#menubutton");

            /*star 定制版处理逻辑*/
            //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
            if ("page1" === anchorLink) {
                logowrapper.fadeIn(); //淡出
                menuBtn.fadeOut();
                //右侧浮层球
                $('.speak').hide();
            } else {
                logowrapper.fadeOut(); //淡入
                menuBtn.fadeIn();
                $("#navigation").fadeIn();
                $('.speak').show();
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
            //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
            if (Config.auchors.indexOf("page1") == (nextIndex - 1)) {
                logowrapper.fadeIn(); //淡出
                menuBtn.fadeOut();
            } else {
                logowrapper.fadeOut(); //淡入
                menuBtn.fadeIn();
            }
            /*end 定制版处理逻辑*/
        }
        //
    });
}

initFullPage();

//右上整站导航js
$(document).ready(function () {
    var navbox = $("#navigation-box");
    var navbg = $("#navigation-bg");
    var navmain = $("#nav-main");
    var navclose = $("#nav-close");

    $(".navigation-box-button a").click(function () {
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