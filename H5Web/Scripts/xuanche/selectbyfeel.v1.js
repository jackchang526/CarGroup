var wArr = [];
var moreArr = [];
var gArr = [];
var fArr = [];
var lArr = [];
var minprice = 0;
var maxprice = 9999;
var date = new Date();
var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
var SelectByFeel = {
    /// <summary>
    ///     根据URL参数页面初始化
    /// </summary>
    init: function() {
        var wStr = SelectByFeel.getQueryStringByName("w");
        if (typeof wStr != "undefined" && wStr !== "" && wStr != null) {
            var slist = wStr.split("_");
            if (slist.length > 0) {
                for (var w = 0; w < slist.length; w++) {
                    $("span[w=" + slist[w] + "]").parent().addClass("current");
                    SelectByFeel.AddEleToArr(wArr, slist[w]);
                }
            }
        }

        var mStr = SelectByFeel.getQueryStringByName("more");
        if (typeof mStr != "undefined" && mStr !== "" && mStr != null) {
            var mlist = mStr.split("_");
            if (mlist.length > 0) {
                for (var m = 0; m < mlist.length; m++) {
                    $("span[more=" + mlist[m] + "]").parent().addClass("current");
                    SelectByFeel.AddEleToArr(moreArr, mlist[m]);
                }
            }
        }

        var gStr = SelectByFeel.getQueryStringByName("g");
        if (typeof gStr != "undefined" && gStr !== "" && gStr != null) {
            var glist = gStr.split(",");
            if (glist.length > 0) {
                for (var g = 0; g < glist.length; g++) {
                    $("span[g=" + glist[g] + "]").parent().addClass("current");
                    SelectByFeel.AddEleToArr(gArr, glist[g]);
                }
            }
        }

        var fStr = SelectByFeel.getQueryStringByName("f");
        if (typeof fStr != "undefined" && fStr !== "" && fStr != null) {
            var flist = fStr.split(",");
            if (flist.length > 0) {
                $("span[f='" + fStr + "']").parent().addClass("current");
                for (var f = 0; f < flist.length; f++) {
                    SelectByFeel.AddEleToArr(fArr, flist[f]);
                }
            }
        }

        var lStr = SelectByFeel.getQueryStringByName("l");
        if (typeof lStr != "undefined" && lStr !== "" && lStr != null) {
            var llist = lStr.split(",");
            if (llist.length > 0) {
                for (var l = 0; l < llist.length; l++) {
                    SelectByFeel.AddEleToArr(lArr, llist[l]);
                }
                if (llist.indexOf("8") >= 0) {
                    $('span[l="7,8"]').parent().addClass("current");
                } 
                if (llist.indexOf("4") >= 0 && llist.indexOf("6") && llist.indexOf("15") && llist.indexOf("16") >= 0) {
                    $('span[l="4,6,7,15,16"]').parent().addClass("current");
                }
            }
        }

        var priceStr = SelectByFeel.getQueryStringByName("p");
        if (typeof priceStr != "undefined" && priceStr !== "" && priceStr != null) {
            var pricelist = priceStr.split("-"); 
            if (pricelist.length > 0) {
                var $ruler = $(".ruler"),
                    $sliderbar = $ruler.find(".sliderbar");

                var tempprice = 0;
                minprice = parseFloat(pricelist[0]);
                maxprice = parseFloat(pricelist[1]);
                if (minprice > maxprice) {
                    tempprice = minprice;
                    minprice = maxprice;
                    maxprice = tempprice;
                }

                $sliderbar.find(".min-dot span").parent().parent().attr("data-index", minprice);
                $sliderbar.find(".min-dot span").html(minprice);

                if (maxprice >= 100) {
                    $sliderbar.find(".max-dot span").parent().parent().attr("data-index", 100);
                    $sliderbar.find(".max-dot span").html("100+");
                } else {
                    $sliderbar.find(".max-dot span").parent().parent().attr("data-index", maxprice);
                    $sliderbar.find(".max-dot span").html(maxprice);
                }
            }
        }

        if (wArr.length > 0 || moreArr.length > 0 || gArr.length > 0 || fArr.length > 0 || lArr.length > 0 || minprice !== 0 || maxprice!==9999) {
            $(".button_del").attr("disabled", false);
            $(".button_del").removeClass("button_del_none");
        } else {
            $(".button_del").attr("disabled", true);
            $(".button_del").addClass("button_del_none");
        }

        SelectByFeel.getCarByFeel();
    },

    /// <summary>
    ///     调用接口获取数据
    /// </summary>
    getCarByFeel: function () {

        //特殊处理逻辑:夜店小王子和驴友同时选择时，不请求接口，结果设为0
        if (lArr.indexOf("8") >= 0 && lArr.indexOf("9") >= 0) {
            $(".button_del_other").html("有0款车型符合要求");
            $(".button_del_other").attr("disabled", true);
            $(".button_del_other").addClass("button_none");
            return;
        }

        var apiUrl = "http://select.car.yiche.com/selectcartoolv2/searchresult?external=Fourth&time=" + version +"&"+ SelectByFeel.getCondition(false);

        $.ajax({
            url: apiUrl,
            cache: true,
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "getCars",
            success: function(data) {
                if (typeof data == "undefined") return;
                if (data == null) return;

                var serialCount = data.Count;
                //var carCount = data.ResList.count;

                $(".button_del_other").html("有" + serialCount + "款车型符合要求");
                if (serialCount === 0) {
                    $(".button_del_other").attr("disabled", true);
                    $(".button_del_other").addClass("button_none");
                } else {
                    $(".button_del_other").attr("disabled", false);
                    $(".button_del_other").removeClass("button_none");
                }
            }
        });
    },

    /// <summary>
    ///     根据名称获取查询参数（可公用）
    /// </summary>
    getQueryStringByName: function(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2];
        return null;
    },

    /// <summary>
    ///     移除数组中指定元素（可公用）
    /// </summary>
    RemoveEleFromArr: function(arr, ele) {
        var index = arr.indexOf(ele);
        if (index > -1) {
            arr.splice(index, 1);
        }
    },

    /// <summary>
    ///     添加元素到数组中，存在于数组的元素不会重复添加（可公用）
    /// </summary>
    AddEleToArr: function(arr, ele) {
        if (typeof ele != "undefined" && ele !== "" && arr.indexOf(ele) < 0) {
            arr.push(ele);
        }
    },

    /// <summary>
    ///     获取当前页面的查询参数
    /// </summary>
    getCondition: function (isFrom) {
        var condition = "";
        if (isFrom) {
            condition += "h5from=feel";
        }
        if (wArr.length > 0) {
            if (condition !== "") {
                condition += "&";
            }
            condition += "w=" + wArr.join("_");
        }
        if (moreArr.length > 0) {
            if (condition !== "") {
                condition += "&";
            }
            condition += "more=" + moreArr.join("_");
        }
        if (gArr.length > 0) {
            if (condition !== "") {
                condition += "&";
            }
            condition += "g=" + gArr.join(",");
        }
        if (fArr.length > 0) {
            if (condition !== "") {
                condition += "&";
            }
            condition += "f=" + fArr.join(",");
        }
        if (lArr.length > 0) {
            if (condition !== "") {
                condition += "&";
            }
            condition += "l=" + lArr.join(",");
        }
        if (condition !== "") {
            condition += "&";
        }
        condition += "p=" + minprice + "-" + maxprice;
        
        return condition;
    }
};