$(function () {
    var $body = $('body');
    var data = {
    };
    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-price]',
        ds: data,
        templateid: '#gradeTemplate',
        fnEnd: function () { 
            bindSwiperStyle('p');
            $("#btnPriceSubmit").bind("click", function () {
                var p_min = $("#p_min").val();
                var p_max = $("#p_max").val();
                if (p_min != "" && !isNaN(p_min)) {
                    if (p_min != "" && !isNaN(p_min)) {
                        CarParam["p"] = "p_" + p_min + "-" + p_max;
                        Jump();
                    }
                }
            })
        }
    });

    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-dltype]',
        ds: data,
        templateid: '#gradeTemplate1',
        fnEnd: function () { 
            bindSwiperStyle('f');
        }
    });

    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-range]',
        ds: data,
        templateid: '#gradeTemplate2',
        fnEnd: function () { 
            bindSwiperStyle('bl');
        }
    });

    //   充电时间
    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-time]',
        ds: data,
        templateid: '#gradeTemplate3',
        fnEnd: function () { 
            bindSwiperStyle('ct');
        }
    });

    //   国别
    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-country]',
        ds: data,
        templateid: '#gradeTemplate4',
        fnEnd: function () { 
            bindSwiperStyle('g');
        }
    });

    //   车身类型
    $body.trigger('publicswipe1', {
        actionName: '[data-action=grade-cartype]',
        ds: data,
        templateid: '#gradeTemplate5',
        fnEnd: function () { 
            bindSwiperStyle('b');
        }
    });

});



var curUrlParam = "";
var baseUrl = "";
var CarParam = {
    p: "",  // 价格
    f: "",  // 动力类型
    bl: "",  // 续航里程
    ct: "",  // 充电时间
    g: "",  // 国别
    b: "",   // 车身形式
    s: 4,  //排序方式
    page: 1,  //当前第几页
    pagesize: 50 // 每页数量
}
var CarParamText = {
    "p_0-5": "5万以下",  // 价格
    "p_5-8": "5-8万",  // 价格
    "p_8-12": "8-12万",  // 价格
    "p_12-18": "12-18万",  // 价格
    "p_18-25": "18-25万",  // 价格
    "p_25-40": "25-40万",  // 价格
    "p_40-80": "40-80万",  // 价格
    "p_80-9999": "80万以上",  // 价格
    "f_16": "纯电",  // 动力类型
    "f_128": "插电混动",  // 动力类型
    "bl_0-100": "100公里以下",
    "bl_100-200": "100-200公里",
    "bl_200-300": "200-300公里",
    "bl_300-400": "300-400公里",
    "bl_400-500": "400-500公里",
    "bl_500": "500公里以上",
    "ct_0-6": "6小时以内",
    "ct_6-8": "6-8小时",
    "ct_8-12": "8-12小时",
    "ct_12": "12小时以上",
    "g_1": "国产",
    "g_2": "合资",
    "g_4": "进口",
    "b_1": "两厢",
    "b_2": "三厢",
    "b_64": "SUV"
}

var CarParamEnum = {
    p: "价格",  //
    f: "动力类型",  //
    bl: "续航里程",  //
    ct: "充电时间",  //
    g: "国别",  //
    b: "车身形式"
}

//var CarNumber = @ViewData["Count"].ToString();

// 分页功能
function bindPager() {
    var pages = (CarNumber % CarParam.pagesize) > 0 ? parseInt(CarNumber / CarParam.pagesize) + 1 : parseInt(CarNumber / CarParam.pagesize);
    if (pages <= 0) {
        $(".m-pages").hide();
        return;
    }
    var select = "";
    for (var i = 1; i <= pages; i++) {
        if (CarParam.page == i) {
            select += ("<option selected value='" + i + "'>" + i + "</option>");
        } else {
            select += ("<option value='" + i + "'>" + i + "</option>");
        }
    }
    //page_prev
    $("#page_list").unbind();
    $("#page_list").html(select);
    $("#page_list").bind("change", function () {
        CarParam.page = $(this).val();
        Jump();
    });

    $("#page_current").text(CarParam.page + "/" + pages);
    if (CarParam.page == 1) {
        $("#page_prev").addClass("m-pages-none");
        $("#page_prev").unbind();
        //$("#page_prev").attr("")
        //$("#page_list").addClass();
    } else {
        $("#page_prev").removeClass("m-pages-none");
        $("#page_prev").bind("click", function () {
            CarParam.page = CarParam.page - 1;
            Jump();
        })
    }
    if (CarParam.page == pages) {
        $("#page_next").addClass("m-pages-none");
        $("#page_next").unbind();
        //$("#page_prev").attr("")
        //$("#page_list").addClass();
    } else {
        $("#page_next").removeClass("m-pages-none");
        $("#page_next").bind("click", function () {
            CarParam.page = CarParam.page + 1;
            Jump();
        })
    }
    $(".m-pages").show();
}

// 按照指定条件排序
function setOrder(type) {
    if (type == "price") {
        if (CarParam.s == 2) {
            CarParam.s = 3;
        } else {
            CarParam.s = 2;
        }
    } else {
        CarParam.s = 4;
    }
    Jump();
}

function setPrice() {
    var p_min = $("#p_min").val();
    var p_max = $("#p_max").val();
    if (p_min == "" || isNaN(p_min)) {
        if (p_min == "" || isNaN(p_min)) {
            CarParam["p"] = "p_" + p_min + "-" + p_max;
            Jump();
        }
    }
}

function LoadUrlParam() {
    var pp = GetRequest();
    CarParam = $.extend(CarParam, pp);

    // todo: 绑定样式
    bindSelectedStyle();
    bindPager();
}

function bindSelectedStyle() {
    for (k in CarParam) {
        if (k == "idx") {
            continue;
        }
        if (k == "s") {
            if (CarParam[k] == 4) {
                $("#order_attend").addClass("current");
                $("#order_price").removeClass("jx-current");
                $("#order_price").removeClass("sx-current");
            } else if (CarParam[k] == 2) {
                $("#order_price").addClass("sx-current");
                $("#order_price").removeClass("jx-current");
                $("#order_attend").removeClass("current");

            } else if (CarParam[k] == 3) {
                $("#order_price").addClass("jx-current");
                $("#order_price").removeClass("sx-current");
                $("#order_attend").removeClass("current");
            }
        } else if (k == "p") {

            if (CarParam[k] == "") {
                var n = $("#t_" + k + " a span");//.text(CarParamEnum[k]).parent.removeClass("current");
                n.text(CarParamEnum[k]);
                $("#t_" + k).removeClass("current");
            } else {
                if (CarParamText[CarParam[k]] == undefined) {
                    var n = $("#t_" + k + " a span");//.text(CarParamEnum[k]).parent.removeClass("current");
                    n.text(CarParam[k].replace("p_", "") + "万");
                    $("#t_" + k).addClass("current");
                } else {
                    var n = $("#t_" + k + " a span");//.text(CarParamEnum[k]).parent.removeClass("current");
                    n.text(CarParamText[k + "_" + CarParam[k]]);
                    $("#t_" + k).addClass("current");
                }
            }

        } else if (CarParam[k] == "") {
            var n = $("#t_" + k + " a span");//.text(CarParamEnum[k]).parent.removeClass("current");
            n.text(CarParamEnum[k]);
            $("#t_" + k).removeClass("current");
        } else {
            var n = $("#t_" + k + " a span");//.text(CarParamEnum[k]).parent.removeClass("current");
            n.text(CarParamText[k + "_" + CarParam[k]]);
            $("#t_" + k).addClass("current");
        }
    }
}

function bindSwiperStyle(k) { 
    var curk = $("a[t^='" + k + "']");
    if (curk.length > 0) {
        for (var i = 0; i < curk.length; i++) {
            var t = $(curk[i]).attr("t");
            if (CarParam[k] != "" && t.indexOf(CarParam[k]) > 0) {
                $(curk[i].parentNode).addClass("current");
                //$("#t_" + k + " a span").text(CarParam[k]);
            } else {
                $(curk[i].parentNode).removeClass("current");
            }
        }
    }
}

function GetRequest() {
    //url例子：XXX.aspx?ID=" + ID + "&Name=" + Name；
    var url = location.search; //获取url中"?"符以及其后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1)//url中存在问号，也就说有参数。
    {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            if (strs[i].split("=")[0] != "") {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
    }
    //console.log(theRequest);
    return theRequest;
}

function GotoPage(param) {
    var params = param.split('_');
    if (param.indexOf('_') > 0) {
        //var v = CarParam[params[0]];
        CarParam[params[0]] = params[1];
    } else {
        //
        CarParam[params[0]] = "";
    }
    Jump();
}

function Jump() {
    var turnUrl = "";
    for (p in CarParam) {
        if (CarParam[p] == "") {
        } else {
            turnUrl += ("&" + p + "=" + CarParam[p]);
        }
    }
    var path = window.location.hostname + window.location.pathname + "?" + turnUrl;
    window.location = "?" + turnUrl;
}

// 绑定上市新车标签
function BindNewCarIntoMarketTime() {
    //var requrl = "http://api.car.bitauto.com/carinfo/GetCarIntoMarketText.ashx?callback=GetNewCarTextCallback&csids=1649,4675&isshowdate=0&type=serial"
    var ids = "";

    var lis = $(".buy-car ul li");
    if (lis.length <= 0)
    { return; }
    for (var idx = 0; idx < lis.length; idx++) {
        ids += $(lis[idx]).attr("id").replace("s_", "") + ",";
    }
    //console.log(ids);
    ids = ids.substr(0, ids.length - 1);
    //console.log(ids);
    var requrl = "http://api.car.bitauto.com/carinfo/GetCarIntoMarketText.ashx?csids=" + ids + "&isshowdate=0&type=serial";

    $.ajax({
        url: requrl,
        dataType: "jsonp",
        jsonpCallback: "NewCarIntoMarketTimeJsonpCallback",
        cache: true,
        success: function (json) {
            //console.log(json);
            var result = json;
            if (result != null && result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var tt = $("#s_" + result[i].csid + " .ico-shangshi");
                    tt.text(result[i].text);
                    if (result[i].text == "即将上市") {
                        tt.addClass("ico-shangshi-blue");
                    } else {
                        //var tt = $("s_" + result[i].csid + " .ico-shangshi");
                        //tt.text(result[i].text);
                        tt.removeClass("ico-shangshi-blue");
                        //tt.show();
                    }
                    tt.show();
                }
            }
        },
        error: function (res) {
            //
        }
    });


}
