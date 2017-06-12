require.config({
    //baseUrl: "scripts",
    paths: {
        jquery: "jquery/jquery-1.8.2",
        //jquery: "//image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min",
        jqueryfullpage: "jquery/jquery.fullpage2.6.5",
        ////jqueryfullpage: "//image.bitautoimg.com/carchannel/h5/js/Common/jquery.fullpage2.6.5",
        jquerytmpl: "jquery/jquery.tmpl.min",
        ////jquerytmpl: "//image.bitautoimg.com/carchannel/h5/js/Common/jquery.tmpl.min",

        underscore: "underscore/underscore",
        backbone: "backbone/backbone",

        anchors: "cssummary/anchors",
        util: "cssummary/util",
        customizationpage: "cssummary/customizationpage",
        commondata: "cssummary/commondata",
        user: "cssummary/user",
        color: "cssummary/color",
        router: "cssummary/router"
    },

    //    // shim属性，专门用来配置不兼容的模块。具体来说，每个模块要定义
    //    //（1）exports值（输出的变量名），表明这个模块外部调用时的名称；
    //    //（2）deps数组，表明该模块的依赖性
    shim: {
        jquery: {
            exports: "$"
        },
        underscore: {
            exports: "_"
        },
        backbone: {
            deps: ["underscore", "jquery"],
            exports: "Backbone"
        },
        jquerytmpl: {
            deps: ["jquery"],
            exports: "jquerytmpl"
        },
        color: {
            deps: ["jquery"],
            exports: "color"
        }
    }


});

require(["jquery", "anchors", "commondata", "user", "util", "underscore", "backbone", "jqueryfullpage", "jquerytmpl", "customizationpage", "color"], function($, anchors, commondata, user, util, _, Backbone) {


    /*star 数据请求*/
    //commondata.waiguansheji("page3");
    var waiguansheji = _.once(commondata.waiguansheji);
    //commondata.pingcedaogou("page4");
    var pingcedaogou = _.once(commondata.pingcedaogou);
    //commondata.liangdianpeizhi("page5");
    var liangdianpeizhi = _.once(commondata.liangdianpeizhi);
    //user.remendianping("page6");
    var remendianping = _.once(user.remendianping);
    //user.chekuanliebiao("page7");
    var chekuanliebiao = _.once(user.chekuanliebiao);
    //user.youhuigouche("page8");
    var youhuigouche = _.once(user.youhuigouche);
    //user.jingxiaoshang("page9");
    var jingxiaoshang = _.once(user.jingxiaoshang);
    //user.baoxian("page10");
    var baoxian = _.once(user.baoxian);
    //user.yanghu("page11");
    var yanghu = _.once(user.yanghu);
    //user.kanlehaikan("page12");
    var kanlehaikan = _.once(user.kanlehaikan);
    //user.ershouche("page14");
    var ershouche = _.once(user.ershouche);
    /*end 数据请求*/

    var testrouter = Backbone.Router.extend({
        routes: {
            '': "main",
            'page2': "page2",
            'page3': "page3",
            'page4': "page4",
            'page5': "page5",
            'page6': "page6",
            'page7': "page7",
            'page8': "page8",
            'page9': "page9",
            'page10': "page10",
            'page11': "page11",
            'page12': "page12",
            'page13': "page13",
            'page14': "page14"
        }
    });
    var router = new testrouter();
    router.on("route:main", function() {
        alert("首页");
    });
    router.on("route:page2", function() {
        //alert("page2");
    });
    router.on("route:page3", function() {
        waiguansheji("page3");
    });
    router.on("route:page4", function() {
        //alert("page4");
        pingcedaogou("page4");
    });
    router.on("route:page5", function() {
        //alert("page5");
        liangdianpeizhi("page5");
    });
    router.on("route:page6", function() {
        //alert("page6");
        remendianping("page6");
    });
    router.on("route:page7", function() {
        //alert("page7");
        chekuanliebiao("page7");
    });
    router.on("route:page8", function() {
        //alert("page8");
        youhuigouche("page8");
    });
    router.on("route:page9", function() {
        //alert("page9");
        jingxiaoshang("page9");
    });
    router.on("route:page10", function() {
        //alert("page10");
        baoxian("page10");
    });
    router.on("route:page11", function() {
        //alert("page11");
        yanghu("page11");
    });
    router.on("route:page12", function() {
        //alert("page12");
        kanlehaikan("page12");
    });
    router.on("route:page14", function() {
        //alert("page14");
        ershouche("page14");
    });
    Backbone.history.start();

    /*star 滑动效果*/
    $("#fullpage").fullpage({
        anchors: anchors.Auchors,
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
        },

        afterLoad: function(anchorLink, index) {

            /*star page2颜色图设置src属性*/
            $("." + anchorLink).find("img[data-src]").each(function() {
                $(this).attr("src", $(this).data("src"));
                $(this).removeAttr("data-src");
            });
            /*end */

            var logowrapper = $(".fixed_box");
            //该回调函数的参数nextIndex是从1开始计算的，所以需要计算之后在使用
            if ("page1" === anchorLink || "page2" === anchorLink) {
                logowrapper.fadeIn(); //淡出
            } else {
                logowrapper.fadeOut(); //淡入
            }
        },

        onLeave: function(index, nextIndex, direction) {
            var logowrapper = $(".fixed_box");
            if (anchors.Auchors.indexOf("page1") === (nextIndex - 1) || anchors.Auchors.indexOf("page2") === (nextIndex - 1)) {
                logowrapper.fadeIn(); //淡出

            } else {
                logowrapper.fadeOut(); //淡入
            }
        }
        //
    });
    /*end 滑动效果*/

    /*star 换车*/
    var par = "";
    var wtMcId = util.GetQueryStringByName("WT.mc_id");

    if (wtMcId) {
        par += "&WT.mc_id=" + wtMcId;
    }

    var ad = util.GetQueryStringByName("ad");

    if (ad) {
        par += "&ad=" + ad;
    }

    var order = util.GetQueryStringByName("order");

    if (order) {
        par += "&order=" + order;
    }

    var returnUrl = "http://" + window.location.host;
    var h5From = util.GetQueryStringByName("h5from");

    switch (h5From) {
    case "fashao":
    case "feel":
        if (window.sessionStorage["listUrl"]) {
            returnUrl = window.sessionStorage["listUrl"];
        } else {
            returnUrl += "?" + par;
        }
        break;
    case "brand":
        returnUrl = "http://" + window.location.host + "/chebiaodang/?" + par;
        break;
    }

    $("#changecar_f").attr("href", returnUrl);
    /*end 换车*/


});