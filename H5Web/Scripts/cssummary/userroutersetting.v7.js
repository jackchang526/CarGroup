var H5Router = Backbone.Router.extend({
    routes: {
        '': "main",
        'page1': "page1",
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
        'page14': "page14",
        'page1/:query': "other",
        'page3/:query': "other",
        'page4/:query': "other",
        'page5/:query': "other",
        'page6/:query': "other",
        'page7/:query': "other",
        'page8/:query': "other",
        'page9/:query': "other",
        'page10/:query': "other",
        'page11/:query': "other",
        'page12/:query': "other",
        'page13/:query': "other",
        'page14/:query': "other"
    }
});

var router = new H5Router();

var pageAction = {
    'page3': _.once(user.waiguansheji),
    'page4': _.once(user.pingcedaogou),
    'page5': _.once(user.liangdianpeizhi),
    'page6': _.once(user.remendianping),
    'page7': _.once(user.chekuanliebiao),
    'page8': _.once(user.youhuigouche),
    'page9': _.once(user.jingxiaoshang),
    'page10': _.once(user.baoxian),
    'page11': _.once(user.yanghu),
    'page12': _.once(user.kanlehaikan),
    'page14': _.once(user.ershouche)
};

var preLoad = function(name) {

    var pageCurrentIndex = Config.auchors.indexOf(name);
    var prePageIndex = pageCurrentIndex - 1;
    var nexPageIndex = pageCurrentIndex + 1;

    //上一页
    if (prePageIndex >= 0 && typeof Config.auchors[prePageIndex] != "undefined" && typeof pageAction[Config.auchors[prePageIndex]] != "undefined") {
        pageAction[Config.auchors[prePageIndex]](Config.auchors[prePageIndex]);
    }

    //当前页面
    if (pageCurrentIndex >= 0 && typeof Config.auchors[pageCurrentIndex] != "undefined" && typeof pageAction[Config.auchors[pageCurrentIndex]] != "undefined") {
        pageAction[Config.auchors[pageCurrentIndex]](Config.auchors[pageCurrentIndex]);
    }

    //下一页
    if (nexPageIndex >= 0 && typeof Config.auchors[nexPageIndex] != "undefined" && typeof pageAction[Config.auchors[nexPageIndex]] != "undefined") {
        pageAction[Config.auchors[nexPageIndex]](Config.auchors[nexPageIndex]);
    }
};

router.on("route:other", function(query) {
    var temphash = window.location.hash;
    if (temphash) {
        var hasharr = temphash.split("/");
        if (hasharr.length > 0) {
            var curPage = hasharr[0].replace("#", "");
            preLoad(curPage);
        }
    }
});

router.on("route:main", function() {
    if (Config.auchors.indexOf(window.location.hash) < 0) {
        window.location.hash = "#" + Config.auchors[0];
    }
});

router.on("route:page1", function() {
    preLoad("page1");
});

router.on("route:page3", function() {
    preLoad("page3");
});

router.on("route:page4", function() {
    preLoad("page4");
});

router.on("route:page5", function() {
    preLoad("page5");
});

router.on("route:page6", function() {
    preLoad("page6");
});

router.on("route:page7", function() {
    preLoad("page7");
});

router.on("route:page8", function() {
    preLoad("page8");
});

router.on("route:page9", function() {
    preLoad("page9");
});

router.on("route:page10", function() {
    preLoad("page10");
});

router.on("route:page11", function() {
    preLoad("page11");
});

router.on("route:page12", function() {
    preLoad("page12");
});

router.on("route:page13", function() {
    preLoad("page13");
});

router.on("route:page14", function() {
    preLoad("page14");
});

Backbone.history.start();