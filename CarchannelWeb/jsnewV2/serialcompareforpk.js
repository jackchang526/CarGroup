//$(function () {
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
Array.prototype.clone = function () { return this.slice(0); }
if (serialId1 > 0 && serialId2 > 0 && serialId1 != serialId2) {
    queryCsIdArr = [serialId1, serialId2];
    InitData(serialId1, serialId2);
} else {
    SetDropDownList("master1", "serial1", 0, 0);
    SetDropDownList("master2", "serial2", 0, 0);
}

//GetHotCompare();
SetSaleUvMonths();
BindPriceAndFuelClick();
//});

//初始化下拉框
function SetDropDownList(masterName, serialName, masterId, serialId) {
    BitA.DropDownListNew({
        container: { master: masterName, serial: serialName },
        include: { serial: "1" },
        dvalue: { master: masterId, serial: serialId },
        callback: { cartype: function (data) { } },
        datatype: 6,
        onchange: {
            serial: function (data) {
                var id1 = $("#serial1").find("span").attr("value");
                var id2 = $("#serial2").find("span").attr("value");
                if (id1 != id2) {
                    serialId1 = id1;
                    serialId2 = id2;
                    if (serialId1 != "" && serialId2 != "" && serialId1 > 0 && serialId2 > 0 && serialId1 != serialId2) {
                        location.href = "/duibi/" + serialId1 + "-" + serialId2 + "/";
                    }
                }
            }
        } 
    });
}

function InitData(serialId1, serialId2) {
    var carTypeL = "L"; // L表示左边的车
    GetCarBaseInfo(serialId1, carTypeL);
    //GetRatingItem(serialId1, carTypeL);
    GetImpress(serialId1, carTypeL);
    GetSameRelatedNews(serialId1, serialId2);
    //GetNews(serialId1, carTypeL);
    GetSeeToSeeUserCompare(serialId1, carTypeL);
    GetUv(serialId1, carTypeL, false);
    GetSale(serialId1, carTypeL, false);

    //---
    var carTypeR = "R"; // R表示左边的车
    GetCarBaseInfo(serialId2, carTypeR);
    GetImpress(serialId2, carTypeR);
    //GetRatingItem(serialId2, carTypeR);
    //GetNews(serialId2, carTypeR);
    GetSeeToSeeUserCompare(serialId2, carTypeR);
    GetUv(serialId2, carTypeR, false);
    GetSale(serialId2, carTypeR, false);
    //投票
    initVote();
}

//绑定油耗和价格点击事件，对比最高和对比最低
function BindPriceAndFuelClick() {
    var priceLi = $("#ul_price").children();
    for (var i = 0; i < priceLi.length; i++) {
        $(priceLi[i]).bind("click", function () {
            $("#ul_price").children().removeClass();
            $(this).addClass("win").siblings().addClass("lose");
            SetPrice();
        });
    }
    var fuelLi = $("#ul_fuel").children();
    for (var j = 0; j < fuelLi.length; j++) {
        $(fuelLi[j]).bind("click", function () {
            $("#ul_fuel").children().removeClass();
            $(this).addClass("win").siblings().addClass("lose");
            SetFuel();
        });
    }
}

//按月份查询销量和关注度
function SetSaleUvMonths() {
    bindClick("uv_monthlists");
    bindClick("sale_monthlists");
}

//月份绑定点击切换事件
function bindClick(ulId) {
    var carType = "uv";
    var type = ulId.substring(0, 2);
    if (type != carType) {
        carType = "sale";
    }
    var lis = $("#" + ulId).children();
    for (var i = 0; i < lis.length; i++) {
        $(lis[i]).bind("click", function () {
            $("#" + ulId).children().removeClass();
            $(this).addClass("current");
            //重新查询
            //var serialId1 = $("#serial1").val();
            //var serialId2 = $("#serial2").val();
            if (carType == "uv") {
                uvArr[0] = 0;
                uvArr[1] = 0;
                GetUv(serialId1, "L", true);
                GetUv(serialId2, "R", true);
            } else {
                saleArr[0] = 0;
                saleArr[1] = 0;
                GetSale(serialId1, "L", true);
                GetSale(serialId2, "R", true);
            }
        });
    }
}

//获取看了又看和相关对比
function GetSeeToSeeUserCompare(serialId, carType) {
    var urlStr = "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=carcomparelist&csid=" + serialId;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "seetoseeCallback" + carType,
        success: function (result) {
            if (result == null) {
                return;
            }
            if (carType == "L") {
                seeToseeArr[0] = result.seetosee;
                userCompareArr[0] = result.usercompare;
            } else {
                seeToseeArr[1] = result.seetosee;
                userCompareArr[1] = result.usercompare;
            }
            SetSeeToSee();
            //SetUserCompare();
        },
        error: function (error) {
            alert("获取数据失败" + error);
        }
    });
}

var seeToseeArr = new Array();
function SetSeeToSee() {
    if (seeToseeArr.length == 2) {
        var seetoseeL = seeToseeArr[0];
        var seetoseeR = seeToseeArr[1];
        if (seetoseeL == null || seetoseeR == null) {
            return;
        }
        var arr = new Array();
        var idLists = new Array();
        idLists.push(serialId1);
        idLists.push(serialId2);
        for (var k = 0; k < 6; k++) {
            if (seetoseeL.length > k) {
                var obj1 = seetoseeL[k];
                if (!idLists.contains(obj1.serialid)) {
                    idLists.push(obj1.serialid);
                    arr.push(obj1);
                }
            }
            if (seetoseeR.length > k) {
                var obj2 = seetoseeR[k];
                if (!idLists.contains(obj2.serialid)) {
                    idLists.push(obj2.serialid);
                    arr.push(obj2);
                }
            }
        }
        var length = arr.length > 5 ? 5 : arr.length;
        var resultStr = "";
        for (var i = 0; i < length; i++) {
            var serialName = arr[i].serialname;
            var price = arr[i].price;
            var imgurl = arr[i].imgurl;
            var url = arr[i].url;
            resultStr += "<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">";
            resultStr += "<div class=\"img\"><a target=\"_blank\" href=\"" + url + "\">";
            resultStr += "<img src=\"" + imgurl.replace("_1.","_3.") + "\"></a></div>";
            resultStr += "<ul class=\"p-list\">";
            resultStr += "<li class=\"name\"><a target=\"_blank\" href=\"" + url + "\">" + serialName + "</a></li>";
            if (price != null && price.length > 0) {
                resultStr += "<li class=\"price\"><a target=\"_blank\" href=\"" + url + "\">" + price + "</a></li>";
            } else {
                resultStr += "<li class=\"price\"><a target=\"_blank\" href=\"" + url + "\">暂无指导价</a></li>";
            }
            resultStr += "</div>";
        }
        if (resultStr.length > 0) {
            $("#seetosee").html(resultStr);
            $("#div_seetosee").show();
        }
    }
}

//相关对比
var userCompareArr = new Array();
function SetUserCompare() {
    if (userCompareArr.length == 2) {
        var userCompareL = userCompareArr[0];
        var userCompareR = userCompareArr[1];
        var carL = carBaseInfoArr[0];
        var carR = carBaseInfoArr[1];
        if (userCompareL == null || userCompareR == null || carL == null || carR == null) {
            return;
        }
        var arr = new Array();
        for (var i = 0; i < 3; i++) {
            if (carL != null && userCompareL.length > i) {
                var compareL = userCompareL[i];
                var obj = new Object();
                obj.carBaseSerialId = carL.serialId;
                obj.carBaseSerialname = carL.serialname;
                obj.carBasePricerange = carL.pricerange;
                obj.carBaseImageurl = carL.imageurl;
                obj.carBaseAllspell = carL.allspell;
                obj.compareSerialId = compareL.serialid;
                obj.compareSerialname = compareL.serialname;
                obj.comparePricerange = compareL.price;
                obj.compareImageurl = compareL.imgurl;
                obj.compareUrl = compareL.url;
                if (!((obj.carBaseSerialId == serialId1 && obj.compareSerialId == serialId2)
                    || (obj.carBaseSerialId == serialId2 && obj.compareSerialId == serialId1))) {
                    arr.push(obj); //排出与当前对比的重复
                }
            }
            if (carR != null && userCompareR.length > i) {
                var compareR = userCompareR[i];
                var objR = new Object();
                objR.carBaseSerialId = carR.serialId;
                objR.carBaseSerialname = carR.serialname;
                objR.carBasePricerange = carR.pricerange;
                objR.carBaseImageurl = carR.imageurl;
                objR.carBaseAllspell = carR.allspell;
                objR.compareSerialId = compareR.serialid;
                objR.compareSerialname = compareR.serialname;
                objR.comparePricerange = compareR.price;
                objR.compareImageurl = compareR.imgurl;
                objR.compareUrl = compareR.url;
                if (!((objR.carBaseSerialId == serialId1 && objR.compareSerialId == serialId2)
                    || (objR.carBaseSerialId == serialId2 && objR.compareSerialId == serialId1))) {
                    arr.push(objR); //排出与当前对比的重复
                }
            }
        }

        var length = arr.length > 3 ? 3 : arr.length;
        var resultStr = "";
        for (var j = 0; j < length; j++) {
            var carBaseSerialId = arr[j].carBaseSerialId;
            var carBaseSerialname = arr[j].carBaseSerialname;
            var carBasePricerange = arr[j].carBasePricerange;
            var carBaseImageurl = arr[j].carBaseImageurl;
            var carBaseAllspell = arr[j].carBaseAllspell;
            var compareSerialId = arr[j].compareSerialId;
            var compareSerialname = arr[j].compareSerialname;
            var comparePricerange = arr[j].comparePricerange;
            var compareImageurl = arr[j].compareImageurl;
            var compareUrl = arr[j].compareUrl;

            var vsHref = "<a target=\"_blank\" href=\"/duibi/" + carBaseSerialId + "-" + compareSerialId + "/\">";

            resultStr += "<div class=\"vs_photo\">" + vsHref + "<div class=\"car_box fl\">";
            resultStr += "<img src=\"" + carBaseImageurl + "\" />";
            resultStr += "<p class=\"title\">" + carBaseSerialname + "</p>";
            if (carBasePricerange != null && carBasePricerange.length > 0) {
                resultStr += "<p class=\"txt\">" + carBasePricerange + "</p>";
            } else {
                resultStr += "<p class=\"txt huizi\">暂无报价</p>";
            }
            resultStr += "</div><div class=\"car_box fr\">";
            resultStr += "<img src=\"" + compareImageurl + "\"/>";
            resultStr += "<p class=\"title\">" + compareSerialname + "</p>";
            if (comparePricerange != null && comparePricerange.length > 0) {
                resultStr += "<p class=\"txt\">" + comparePricerange + "</p>";
            } else {
                resultStr += "<p class=\"txt huizi\">暂无报价</p>";
            }
            resultStr += "</div><div class=\"vs_img\"><img src=\"http://img1.bitautoimg.com/uimg/index2014/images2/vs/vs_icon_small.png\"></div></a></div>";
        }
        if (resultStr.length > 0) {
            $("#userCompare").html(resultStr);
            BindVsimgHover();
            $("#div_usercompare").show();
        }
    }
}
//vs 图片变大效果
function BindVsimgHover() {
    $(".vs_photo").hover(
        function () {
            $(this).addClass("vs_hover");
        }, function () {
            $(this).removeClass("vs_hover");
        });
}

//获取热门对比
function GetHotCompare() {
    var urlStr = "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=hotcompare";
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "hotCompareCallback",
        success: function (result) {
            var resultStr = "";
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var serialid = result[i].serialid;
                    var serialname = result[i].serialname;
                    var price = result[i].price;
                    var imgurl = result[i].imgurl;
                    var allspell = result[i].allspell;
                    var toserialid = result[i].toserialid;
                    var toserialname = result[i].toserialname;
                    var toprice = result[i].toprice;
                    var toimgurl = result[i].toimgurl;
                    var toallspell = result[i].toallspell;

                    var vsHref = "<a target=\"_blank\" href=\"/duibi/" + serialid + "-" + toserialid + "/\">";
                    resultStr += "<div class=\"vs_photo\">" + vsHref + "<div class=\"car_box fl\">";
                    resultStr += "<img src=\"" + imgurl + "\" />";
                    resultStr += "<p class=\"title\">" + serialname + "</p>";
                    if (price != null && price.length > 0) {
                        resultStr += "<p class=\"txt\">" + price + "</p>";
                    } else {
                        resultStr += "<p class=\"txt huizi\">暂无报价</p>";
                    }
                    resultStr += "</div><div class=\"car_box fr\">";
                    resultStr += "<img src=\"" + toimgurl + "\"/>";
                    resultStr += "<p class=\"title\">" + toserialname + "</p>";
                    if (toprice != null && toprice.length > 0) {
                        resultStr += "<p class=\"txt\">" + toprice + "</p>";
                    } else {
                        resultStr += "<p class=\"txt huizi\">暂无报价</p>";
                    }
                    resultStr += "</div><div class=\"vs_img\"><img src=\"http://img1.bitautoimg.com/uimg/index2014/images2/vs/vs_icon_small.png\"></div></a></div>";
                }
                if (resultStr.length > 0) {
                    $("#hotCompare").html(resultStr);
                    BindVsimgHover();
                    $("#div_hotcompare").show();
                }
            }
        }
    });
}

//获取编辑怎么看文章
function GetSameRelatedNews(serialId1, serialId2) {
    var urlStr = "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=samerelatednews&csids=" + serialId1 + "," + serialId2;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "samerelatednewsCallback",
        success: function (result) {
            var resultStr = "";
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var title = result[i].title;
                    var firstimg = result[i].firstimg;
                    var link = result[i].link;
                    var source = result[i].source;
                    var publishtime = GetNewsPublishTime(result[i].publishtime);
                    var commentcount = result[i].commentcount;
                    var author = result[i].author;
                    var summary = result[i].summary;
                    var newsid = result[i].newsid;
                    if (summary.length > 150) {
                        summary = summary.substring(0, 150);
                    }
                    if (firstimg != "") {
                        resultStr += "<div class=\"article-card horizon\">";
                    }
                    else {
                        resultStr += "<div class=\"article-card horizon text\">";
                    }
                
                    resultStr += "<div class=\"inner-box\">";
                    if(firstimg!="")
                    {
                        resultStr += "<a href=\"" + link + "\" class=\"figure\" target=\"_blank\">";
                        resultStr += "<img src=\"" + firstimg + "\">";
                        resultStr += "</a>";
                    }
                    resultStr += "<div class=\"details\">";
                    resultStr += "<h2><a href=\"" + link + "\" target=\"_blank\">" + title + "</a></h2>";
                    resultStr += "<div class=\"description\"><p>" + summary + "";
                    if (summary != "") {
                        resultStr += "...";
                    }
                    resultStr += "<a href=\"" + link + "\" target=\"_blank\">查看更多&gt;&gt;</a></p></div>";
                    resultStr += "<div class=\"info\"><div class=\"first\"><span class=\"view\" data-vnewsid=\"" + newsid + "\"></span> <span class=\"comment\" data-cnewsid=\"" + newsid + "\"></span></div>";
                    //resultStr += "<div class=\"info\"><div class=\"first\"><span lass=\"view\"><em data-vnewsid=\"" + newsid + "\"></em></span> <span class=\"comment\"><em data-cnewsid=\"" + newsid + "\"></em></span></div>";
                    resultStr += "<div class=\"last\"><span class=\"origin\">来源：<em>" + source + "</em></span><span class=\"author\">作者：" +
                        "<em>" + author + "</em></span><span class=\"time\">" + publishtime + "</span> </div>";
                                          
                    resultStr += "</div></div></div></div>";
                    
                }
                if (resultStr.length > 0) {
                    $("#samerelatedNews").html(resultStr);
                    $("#div_tuwenlist").show();
                }
            }
        },
        error: function (error) {
            alert("获取数据失败" + error);
        }
    });
}
//获取特定的日期格式
function GetNewsPublishTime(dateTime) {
    var time = new Date(dateTime);
    var year = time.getFullYear();
    var month = time.getMonth() + 1;
    var day = time.getDate();
    var s = year + "-" + Appendzero(month) + "-" + Appendzero(day);
    return s;
}
function Appendzero(obj) {
    if (obj < 10) return "0" + obj; else return obj;
}

var newsArr = new Array();
//获取相关文章
function GetNews(serialId, carType) {
    var carTypeStr = "_l";
    if (carType == "R") {
        carTypeStr = "_r";
    }
    var urlStr = "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=carnews&csid=" + serialId;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "carnewsCallback" + carType,
        success: function (result) {
            if (result == null) {
                return;
            }
            if (result.relatednewslist != null) {
                var news = new Array();
                var lists = result.relatednewslist;
                for (var i = 0; i < lists.length; i++) {
                    var cmsnewsid = lists[i].cmsnewsid;
                    var publishtime = lists[i].publishtime;
                    var title = lists[i].title;
                    var link = lists[i].link;
                    var newItem = new Object();
                    newItem.cmsnewsid = cmsnewsid;
                    newItem.title = title;
                    newItem.link = link;
                    newItem.publishtime = publishtime;
                    news.push(newItem);
                }
                if (carType == "L") {
                    newsArr[0] = news;
                } else {
                    newsArr[1] = news;
                }
                SetNews();
            }
        },
        error: function (error) {
            alert("获取数据失败" + error);
        }
    });
}

//两个车系文章去重后，设置新闻
function SetNews() {
    if (newsArr.length == 2) {
        var newsArrL = newsArr[0];
        var newsArrR = newsArr[1];
        if (newsArrL == null || newsArrR == null) {
            return;
        }
        var lists = newsArrL;
        var newsIdLists = new Array();
        for (var k = 0; k < newsArrL.length; k++) {
            newsIdLists.push(newsArrL[k].cmsnewsid);
        }
        for (var j = 0; j < newsArrR.length; j++) {
            if (!newsIdLists.contains(newsArrR[j].cmsnewsid)) {
                newsIdLists.push(newsArrR[j].cmsnewsid);
                lists.push(newsArrR[j]);
            }
        }
        //按时间倒序排序
        lists.sort(function (a, b) {
            var time1 = new Date(a.publishtime);
            var time2 = new Date(b.publishtime);
            return time2 - time1;
        });

        var resultStr = "";
        var length = lists.length > 9 ? 9 : lists.length;
        for (var i = 0; i < length; i++) {
            var item = lists[i];
            var title = item.title;
            var link = item.link;
            resultStr += "<li><a target=\"_blank\" href=\"" + link + "\">" + title + "</a></li>";
        }
        if (resultStr.length > 0) {
            $("#carNews").html(resultStr);
            $("#div_carnews").show();
        }
    }
}

//扩展array的contains方法
Array.prototype.contains = function (item) {
    var flag = false;
    //return RegExp("\\b" + item + "\\b").test(this);
    var arr = $(this);
    for (var i = 0; i < arr.length; i++) {
        if (item == arr[i]) {
            flag = true;
            break;
        }
    }
    return flag;
};

//获取基本信息
var carBaseInfoArr = new Array();
function GetCarBaseInfo(serialId, carType) {
    var carTypeStr = "_l";
    if (carType == "R") {
        carTypeStr = "_r";
    }
    var urlStr = "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=carbaseinfo&csid=" + serialId;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "carbaseCallback" + carType,
        success: function (result) {
            if (result == null) {
                return;
            }
            var obj = new Object();
            obj.serialId = serialId;
            obj.carType = carType;
            obj.carTypeStr = carTypeStr;
            obj.masterid = result.masterid;
            obj.serialname = result.serialname;
            obj.pricerange = result.pricerange;
            obj.imageurl = result.imageurl;
            obj.allspell = result.allspell;
            obj.minreferprice = result.minreferprice;
            obj.maxreferprice = result.maxreferprice;
            obj.minfuel = result.minfuel;
            obj.maxfuel = result.maxfuel;
            obj.len = result.len;
            obj.width = result.width;
            obj.height = result.height;
            obj.wheel = result.wheel;
            obj.imagelist = result.imagelist;
            if (carType == "R") {
                carBaseInfoArr[1] = obj;
                SetDropDownList("master2", "serial2", obj.masterid, obj.serialId);
            } else {
                carBaseInfoArr[0] = obj;
                SetDropDownList("master1", "serial1", obj.masterid, obj.serialId);
            }
            SetCarBaseInfo();
            //SetUserCompare();
        },
        error: function (error) {
            alert("获取数据失败11" + error);
        }
    });
}

function SetCarBaseInfo() {
    if (carBaseInfoArr.length == 2) {
        var carBaseInfoL = carBaseInfoArr[0];
        var carBaseInfoR = carBaseInfoArr[1];
        if (carBaseInfoL != null && carBaseInfoR != null) {
            SetSerialName(carBaseInfoL);
            SetSerialName(carBaseInfoR);
            SetPriceFuelLoad();
            SetBodySize();
            fillImageBox(carBaseInfoArr);
            $("#focus_left_btn").show();
            $("#focus_right_btn").show();
            $("#focus_count_default").hide();
            $("#div_focus").removeClass("vs_focus_t40");
            $("#div_vote").show();
        }
    }
}

function SetSerialName(carInfo) {
    var nameIdStr = "carSerialName" + carInfo.carTypeStr;
    //$("#" + nameIdStr).html(carInfo.serialname);
    $("a[name=" + nameIdStr + "]").html(carInfo.serialname);
    $("a[name=" + nameIdStr + "]").attr("href", "/" + carInfo.allspell + "/");
    $("#" + nameIdStr).show();

    //设置更多 图片、口碑、油耗 等链接
    $("#more_image" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "图片");
    $("#more_image" + carInfo.carTypeStr).attr("href", "http://photo.bitauto.com/serial/" + carInfo.serialId + "/");
    $("#more_image" + carInfo.carTypeStr).parent().show();
    $("#more_price" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "报价");
    $("#more_price" + carInfo.carTypeStr).attr("href", "/" + carInfo.allspell + "/baojia/");
    $("#more_koubei" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "口碑");
    $("#more_koubei" + carInfo.carTypeStr).attr("href", "/" + carInfo.allspell + "/koubei/");
    $("#more_sale" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "销量");
    $("#more_sale" + carInfo.carTypeStr).attr("href", "http://index.bitauto.com/xiaoliang/s" + carInfo.serialId + "/");
    $("#more_uv" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "关注");
    $("#more_uv" + carInfo.carTypeStr).attr("href", "http://index.bitauto.com/guanzhu/s" + carInfo.serialId + "/");
    $("#more_peizhi" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "配置");
    $("#more_peizhi" + carInfo.carTypeStr).attr("href", "/" + carInfo.allspell + "/peizhi/");
    $("#more_fuel" + carInfo.carTypeStr).html("更多" + carInfo.serialname + "油耗");
    $("#more_fuel" + carInfo.carTypeStr).attr("href", "/" + carInfo.allspell + "/youhao/");

}

//页面加载时设置，当两个最高配价格(油耗)都不为空时，默认显示最高配
function SetPriceFuelLoad() {
    var carBaseInfoL = carBaseInfoArr[0];
    var carBaseInfoR = carBaseInfoArr[1];
    if (carBaseInfoL != null && carBaseInfoR != null) {
        var minPriceL = carBaseInfoL.minreferprice;
        var maxPriceL = carBaseInfoL.maxreferprice;
        var minPriceR = carBaseInfoR.minreferprice;
        var maxPriceR = carBaseInfoR.maxreferprice;
        if (minPriceL > 0 && minPriceR > 0) {
            SetPrice();
        } else {
            if (maxPriceL > 0 && maxPriceR > 0) {
                $("#ul_price").children().removeClass();
                $("#ul_price").find("div[price=max]").addClass("win").siblings().addClass("lose");
                SetPrice();
            }
        }
        var minFuelL = carBaseInfoL.minfuel;
        var minFuelR = carBaseInfoR.minfuel;
        var maxFuelL = carBaseInfoL.maxfuel;
        var maxFuelR = carBaseInfoR.maxfuel;
        if (minFuelL > 0 && minFuelR > 0) {
            SetFuel();
        } else {
            if (maxFuelL > 0 && maxFuelR > 0) {
                $("#ul_fuel").children().removeClass();
                $("#ul_fuel").find("div[fuel=max]").addClass("win").siblings().addClass("lose");
                SetFuel();
            }
        }
    }
}

function SetPrice() {
    var carBaseInfoL = carBaseInfoArr[0];
    var carBaseInfoR = carBaseInfoArr[1];
    if (carBaseInfoL != null && carBaseInfoR != null) {
        var priceSelected = $("#ul_price").find("div[class=win]").attr("price");
        var priceL = 0;
        var priceR = 0;
        var minPriceL = parseFloat(carBaseInfoL.minreferprice);
        var maxPriceL = parseFloat(carBaseInfoL.maxreferprice);
        var minPriceR = parseFloat(carBaseInfoR.minreferprice);
        var maxPriceR = parseFloat(carBaseInfoR.maxreferprice);
        if (minPriceL == 0 && maxPriceL == 0 && minPriceR == 0 && maxPriceR == 0) {
            return;
        }
        if ((minPriceL == 0 || minPriceR == 0) && (maxPriceL == 0 || maxPriceR == 0)) {
            return;
        }
        if (priceSelected == "min") {
            priceL = minPriceL;
            priceR = minPriceR;
        } else {
            priceL = maxPriceL;
            priceR = maxPriceR;
        }
        $("#price_l").removeClass("no_data");
        $("#price_r").removeClass("no_data");
        if (priceL > 0 && priceR > 0) {
            $("#price_l").html("<span>￥</span>" + priceL + "万");
            $("#price_r").html("<span>￥</span>" + priceR + "万");
            var lengthL = (priceL / (priceR + priceL) * maxNumber);
            var lengthR = (priceR / (priceR + priceL) * maxNumber);
            SetCompareImage("price_l_span", lengthL, priceL <= priceR);
            SetCompareImage("price_r_span", lengthR, priceR <= priceL);
        } else {
            if (priceL > 0) {
                $("#price_l").html("<span>￥</span>" + priceL + "万");
                $("#price_r").html("暂无报价");
                $("#price_r").addClass("no_data");
                SetCompareImage("price_l_span", maxNumber, true);
                SetCompareImage("price_r_span", 0, false);
            }
            if (priceR > 0) {
                $("#price_r").html("<span>￥</span>" + priceR + "万");
                $("#price_l").html("暂无报价");
                $("#price_l").addClass("no_data");
                SetCompareImage("price_l_span", 0, false);
                SetCompareImage("price_r_span", maxNumber, true);
            }
            if (priceL == 0 && priceR == 0) {
                $("#price_l").html("暂无报价");
                $("#price_l").addClass("no_data");
                $("#price_r").html("暂无报价");
                $("#price_r").addClass("no_data");
            }
        }
        $("#div_price").show();
    }
}

function SetFuel() {
    var carBaseInfoL = carBaseInfoArr[0];
    var carBaseInfoR = carBaseInfoArr[1];
    if (carBaseInfoL != null && carBaseInfoR != null) {
        var fuelSelected = $("#ul_fuel").find("div[class=win]").attr("fuel");
        var fuelL = 0;
        var fuelR = 0;
        var minFuelL = parseFloat(carBaseInfoL.minfuel);
        var minFuelR = parseFloat(carBaseInfoR.minfuel);
        var maxFuelL = parseFloat(carBaseInfoL.maxfuel);
        var maxFuelR = parseFloat(carBaseInfoR.maxfuel);
        if (minFuelL == 0 && minFuelR == 0 && maxFuelL == 0 && maxFuelR == 0) {
            return;
        }
        if ((minFuelL == 0 || minFuelR == 0) && (maxFuelL == 0 || maxFuelR == 0)) {
            return;
        }
        if (fuelSelected == "min") {
            fuelL = minFuelL;
            fuelR = minFuelR;
        } else {
            fuelL = maxFuelL;
            fuelR = maxFuelR;
        }
        $("#fuel_l").removeClass("no_data");
        $("#fuel_r").removeClass("no_data");
        if (fuelL > 0 && fuelR > 0) {
            $("#fuel_l").html(fuelL + "L");
            $("#fuel_r").html(fuelR + "L");
            var lengthL = (fuelL / (fuelL + fuelR) * maxNumber);
            var lengthR = (fuelR / (fuelL + fuelR) * maxNumber);
            SetCompareImage("fuel_l_span", lengthL, fuelL <= fuelR);
            SetCompareImage("fuel_r_span", lengthR, fuelR <= fuelL);
        } else {
            if (fuelL > 0) {
                $("#fuel_l").html(fuelL + "L");
                $("#fuel_r").html("暂无数据");
                $("#fuel_r").addClass("no_data");
                SetCompareImage("fuel_l_span", maxNumber, true);
                SetCompareImage("fuel_r_span", 0, false);
            }
            if (fuelR > 0) {
                $("#fuel_l").html("暂无数据");
                $("#fuel_l").addClass("no_data");
                $("#fuel_r").html(fuelR + "L");
                SetCompareImage("fuel_l_span", 0, false);
                SetCompareImage("fuel_r_span", maxNumber, true);
            }
            if (fuelL == 0 && fuelR == 0) {
                $("#fuel_l").html("暂无数据");
                $("#fuel_l").addClass("no_data");
                $("#fuel_r").html("暂无数据");
                $("#fuel_r").addClass("no_data");
            }
        }
        $("#div_fuel").show();
    }
}

//设置车身尺寸
function SetBodySize() {
    var carBaseInfoL = carBaseInfoArr[0];
    var carBaseInfoR = carBaseInfoArr[1];
    if (carBaseInfoL != null && carBaseInfoR != null) {
        var lenL = carBaseInfoL.len;
        var lenR = carBaseInfoR.len;
        var maxSize = lenL > lenR ? lenL : lenR;
        if (maxSize > 0) {
            var widthL = carBaseInfoL.width;
            var heightL = carBaseInfoL.height;
            var wheelL = carBaseInfoL.wheel;
            var widthR = carBaseInfoR.width;
            var heightR = carBaseInfoR.height;
            var wheelR = carBaseInfoR.wheel;

            if ((lenL == 0 && widthL == 0 && heightL == 0 && wheelL == 0)
                || (lenR == 0 && widthR == 0 && heightR == 0 && wheelR == 0)) {
                return;
            }

            SetLengthPx("length_l", Math.floor(lenL / maxSize * maxLengthPx), lenL >= lenR);
            SetLengthPx("length_r", Math.floor(lenR / maxSize * maxLengthPx), lenL <= lenR);
            SetLengthPx("width_l", Math.floor(widthL / maxSize * maxLengthPx), widthL >= widthR);
            SetLengthPx("width_r", Math.floor(widthR / maxSize * maxLengthPx), widthL <= widthR);
            SetLengthPx("height_l", Math.floor(heightL / maxSize * maxLengthPx), heightL >= heightR);
            SetLengthPx("height_r", Math.floor(heightR / maxSize * maxLengthPx), heightL <= heightR);
            SetLengthPx("wheel_l", Math.floor(wheelL / maxSize * maxLengthPx), wheelL >= wheelR);
            SetLengthPx("wheel_r", Math.floor(wheelR / maxSize * maxLengthPx), wheelL <= wheelR);

            $("#length_l").html(lenL + "mm");
            $("#width_l").html(widthL + "mm");
            $("#height_l").html(heightL + "mm");
            $("#wheel_l").html(wheelL + "mm");
            $("#length_r").html(lenR + "mm");
            $("#width_r").html(widthR + "mm");
            $("#height_r").html(heightR + "mm");
            $("#wheel_r").html(wheelR + "mm");
            //如果获胜次数大于2，则显示为获胜
            var winCountL = 0;
            var winCountR = 0;
            if (lenL >= lenR) {
                winCountL++;
            } if (lenL <= lenR) {
                winCountR++;
            }
            if (widthL >= widthR) {
                winCountL++;
            } if (widthL <= widthR) {
                winCountR++;
            }
            if (heightL >= heightR) {
                winCountL++;
            } if (heightL <= heightR) {
                winCountR++;
            }
            if (wheelL >= wheelR) {
                winCountL++;
            } if (wheelL <= wheelR) {
                winCountR++;
            }
            if (winCountL > 2 && winCountL > winCountR) {
                $("#div_space").find("a[name=carSerialName_l]").parent().parent().addClass("win-box");
                $("#div_space").find("a[name=carSerialName_r]").parent().parent().removeClass("win-box");
            }
            if (winCountR > 2 && winCountR > winCountL) {
                $("#div_space").find("a[name=carSerialName_r]").parent().parent().addClass("win-box");
                $("#div_space").find("a[name=carSerialName_l]").parent().parent().removeClass("win-box");
            }
            $("#div_space").show();
        }
    }
}
var maxLengthPx = 456; //320 px，车身尺寸，长度条的最大长度
function SetLengthPx(idStr, length, isWin) {
    var parentDiv = $("#" + idStr).parent();
    if (isWin) {
        //$(parentDiv).addClass("win");
        $(parentDiv).find("em").css({ background: "#c00", width: length + "px" });
    } else {
        //$(parentDiv).removeClass("win");
        $(parentDiv).find("em").css({ background: "#333", width: length + "px" });
    }
}

var maxNumber = 14;
//设置 价格、油耗、销量、关注 的小图标
function SetCompareImage(idL, lengthL, isWin) {
    var htmlL = "";
    var length = Math.floor(lengthL);
    if (0 < lengthL && lengthL < 1) {
        length = 1;
    }
    for (var i = 0; i < length; i++) {
        htmlL += "<span></span>";
    }
    $("#" + idL).html(htmlL);
    var parent = $("#" + idL).parent();
    if (isWin) {
        $(parent).addClass("win-box");
    } else {
        $(parent).removeClass("win-box");
    }
}

//获取口碑评分
var ratingItemArr = new Array();
function GetRatingItem(serialId, carType) {
    var carTypeStr = "_l";
    if (carType == "R") {
        carTypeStr = "_r";
    }
    var urlStr = "http://api.baa.bitauto.com/koubei/car/ajax/x_getratingitem.ashx?csid=" + serialId;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "ratingitemCallback" + carType,
        success: function (result) {
            var csid = result.csId;
            if (csid == null) {
                return;
            }
            var obj = new Object();
            obj.carType = carType;
            obj.carTypeStr = carTypeStr;
            obj.csId = result.csId;
            obj.appearance = result.appearance;
            obj.comfort = result.comfort;
            obj.configuration = result.configuration;
            obj.operation = result.operation;
            obj.performtoprice = result.performtoprice;
            obj.power = result.power;
            obj.space = result.space;
            obj.upholstery = result.upholstery;
            if (carType == "L") {
                ratingItemArr[0] = obj;
            } else {
                ratingItemArr[1] = obj;
            }
            SetRatingItem();
        }
    });
}

//设置口碑数据
var maxLenth = maxLengthPx;
var maxAccount = 5;
function SetRatingItem() {
    if (ratingItemArr.length == 2) {
        var ratingItemL = ratingItemArr[0];
        var ratingItemR = ratingItemArr[1];
        if (ratingItemL != null && ratingItemR != null) {
            $("#appearance_account_l").html(ratingItemL.appearance + "分");
            SetLengthPx("appearance_account_l", ratingItemL.appearance / maxAccount * maxLengthPx, ratingItemL.appearance >= ratingItemR.appearance);
            $("#comfort_account_l").html(ratingItemL.comfort + "分");
            SetLengthPx("comfort_account_l", ratingItemL.comfort / maxAccount * maxLengthPx, ratingItemL.comfort >= ratingItemR.comfort);
            $("#operation_account_l").html(ratingItemL.operation + "分");
            SetLengthPx("operation_account_l", ratingItemL.operation / maxAccount * maxLengthPx, ratingItemL.operation >= ratingItemR.operation);
            $("#performtoprice_account_l").html(ratingItemL.performtoprice + "分");
            SetLengthPx("performtoprice_account_l", ratingItemL.performtoprice / maxAccount * maxLengthPx, ratingItemL.performtoprice >= ratingItemR.performtoprice);
            $("#power_account_l").html(ratingItemL.power + "分");
            SetLengthPx("power_account_l", ratingItemL.power / maxAccount * maxLengthPx, ratingItemL.power >= ratingItemR.power);
            $("#space_account_l").html(ratingItemL.space + "分");
            SetLengthPx("space_account_l", ratingItemL.space / maxAccount * maxLengthPx, ratingItemL.space >= ratingItemR.space);
            $("#upholstery_account_l").html(ratingItemL.upholstery + "分");
            SetLengthPx("upholstery_account_l", Math.floor(ratingItemL.upholstery / maxAccount * maxLengthPx), ratingItemL.upholstery >= ratingItemR.upholstery);

            $("#appearance_account_r").html(ratingItemR.appearance + "分");
            SetLengthPx("appearance_account_r", Math.floor(ratingItemR.appearance / maxAccount * maxLengthPx), ratingItemL.appearance <= ratingItemR.appearance);
            $("#comfort_account_r").html(ratingItemR.comfort + "分");
            SetLengthPx("comfort_account_r", Math.floor(ratingItemR.comfort / maxAccount * maxLengthPx), ratingItemL.comfort <= ratingItemR.comfort);
            $("#operation_account_r").html(ratingItemR.operation + "分");
            SetLengthPx("operation_account_r", Math.floor(ratingItemR.operation / maxAccount * maxLengthPx), ratingItemL.operation <= ratingItemR.operation);
            $("#performtoprice_account_r").html(ratingItemR.performtoprice + "分");
            SetLengthPx("performtoprice_account_r", Math.floor(ratingItemR.performtoprice / maxAccount * maxLengthPx), ratingItemL.performtoprice <= ratingItemR.performtoprice);
            $("#power_account_r").html(ratingItemR.power + "分");
            SetLengthPx("power_account_r", Math.floor(ratingItemR.power / maxAccount * maxLengthPx), ratingItemL.power <= ratingItemR.power);
            $("#space_account_r").html(ratingItemR.space + "分");
            SetLengthPx("space_account_r", Math.floor(ratingItemR.space / maxAccount * maxLengthPx), ratingItemL.space <= ratingItemR.space);
            $("#upholstery_account_r").html(ratingItemR.upholstery + "分");
            SetLengthPx("upholstery_account_r", Math.floor(ratingItemR.upholstery / maxAccount * maxLengthPx), ratingItemL.upholstery <= ratingItemR.upholstery);
        }
    }
}

//获取印象数据
var impressArr = new Array();
function GetImpress(serialId, carType) {
    var carTypeStr = "l";
    if (carType == "R") {
        carTypeStr = "r";
    }
    var urlStr = "http://api.baa.bitauto.com/koubei/car/ajax/x_getimpress.ashx?csid=" + serialId;
    $.ajax({
        url: urlStr,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "impressCallback" + carType,
        success: function (result) {
            var obj = new Object();
            obj.carType = carType;
            obj.carTypeStr = carTypeStr;
            obj.rating = result.Rating;
            obj.goodList = result.GoodList;
            obj.badList = result.BadList;
            if (carType == "L") {
                impressArr[0] = obj;
            } else {
                impressArr[1] = obj;
            }
            SetImpress();
        }
    });
}

function SetImpress() {
    if (impressArr.length == 2) {
        var impressL = impressArr[0];
        var impressR = impressArr[1];
        if (impressL != null && impressR != null) {
            var ratingL = impressL.rating;
            var ratingR = impressR.rating;
            if (ratingL == 0 || ratingR == 0) {
                return;
            }
            SetImpressData(impressL.carType);
            SetImpressData(impressR.carType);           
            if (ratingL == ratingR) {
                $("#rating_l").parent().addClass("win-box");
                $("#rating_r").parent().addClass("win-box");
            }
            if (ratingL > ratingR) {
                $("#rating_l").parent().addClass("win-box");
                $("#rating_r").parent().removeClass("win-box");
            }
            if (ratingL < ratingR) {
                $("#rating_r").parent().addClass("win-box");
                $("#rating_l").parent().removeClass("win-box");
            }
            $("#div_koubei").show();
        }
    }
}

function SetImpressData(carType) {
    var obj = impressArr[0];
    if (carType == "R") {
        obj = impressArr[1];
    }
    var goodImpressStr = "";
    var badImpressStr = "";
    var youdianId = "youdian_" + obj.carTypeStr;
    var quedianId = "quedian_" + obj.carTypeStr;
    var ratingId = "rating_" + obj.carTypeStr;

    var goodList = obj.goodList;
    var badList = obj.badList;
    var rating = obj.rating;
    if (goodList != null) {
        for (var i = 0; i < goodList.length; i++) {
            if (i == 6) {
                break;
            }
            var goodStr = goodList[i].K;
            goodImpressStr += "<span class='blue'>" + goodStr + "</span>";
        }
    }
    if (badList != null) {
        for (var j = 0; j < badList.length; j++) {
            if (j == 6) {
                break;
            }
            var badStr = badList[j].K;
            badImpressStr += "<span class='gray'>" + badStr + "</span>";
        }
    }
    goodImpressStr = goodImpressStr == "" ? "<span class='blue'>暂无印象</span>" : goodImpressStr;
    badImpressStr = badImpressStr == "" ? "<span class='gray'>暂无印象</span>" : badImpressStr;
    $("#" + youdianId).html("<p>优点</p>"+ goodImpressStr);
    $("#" + quedianId).html("<p>缺点</p>" + badImpressStr);
    $("#" + ratingId).html(rating + "分");
    var lenth = rating / maxAccount * 100;
    $("#" + ratingId).parent().find("div[class=start]").find("em").css({ "width": lenth + "%" });
}

//获取关注指数
var uvArr = [0, 0];
function GetUv(serialId, carType, isClick) {
    var month = $("#uv_monthlists").find("li[class=current]").attr("value");
    var indextype = "uv";
    var carTypeStr = "l";
    if (carType == "R") {
        carTypeStr = "r";
    }
    GetUvOrSale(indextype, serialId, month, carTypeStr, isClick);
}

//获取销量指数
var saleArr = [0, 0];
function GetSale(serialId, carType, isClick) {
    var month = $("#sale_monthlists").find("li[class=current]").attr("value");
    var indexType = "sale";
    var carTypeStr = "l";
    if (carType == "R") {
        carTypeStr = "r";
    }
    GetUvOrSale(indexType, serialId, month, carTypeStr, isClick);
}

function GetUvOrSale(indexType, serialId, month, carType, isClick) {
    var idStr = indexType + "_count_" + carType; //uv_count_r
    $("#" + idStr).html("");
    var urlStr = "http://api.easypass.cn/indexDataProvider/GetIndexData.ashx?indextype=" + indexType + "&date=" + month + "&csid=" + serialId;
    var count = 0;
    $.ajax({
        url: urlStr,
        async: false,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "uvCallback" + indexType + carType + month,
        success: function (result) {
            if (result != null) {
                count = result.count;
                if (indexType == "sale") {
                    if (carType == "l") {
                        saleArr[0] = count;
                    } else {
                        saleArr[1] = count;
                    }
                    SetSale();
                } else {
                    if (carType == "l") {
                        uvArr[0] = count;
                    } else {
                        uvArr[1] = count;
                    }
                    SetUv();
                }
            } else { //向前推一个月，重新获取销量数据
                if (indexType == "sale") {
                    var lis = $("#sale_monthlists").find("li");
                    var selectedValue = $("#sale_monthlists").find("li[class=current]").attr("value");
                    var li2Value = $(lis[1]).attr("value");
                    if (selectedValue != li2Value && !isClick) {
                        $(lis).removeClass("current");
                        $(lis[1]).addClass("current");
                        GetSale(serialId1, "L", false);
                        GetSale(serialId2, "R", false);
                    } else {
                        SetSale();
                    }
                } else {//向前推一个月，重新获取关注数据
                    var uvlis = $("#uv_monthlists").find("li");
                    var uvselectedValue = $("#uv_monthlists").find("li[class=current]").attr("value");
                    var uvli2Value = $(uvlis[1]).attr("value");
                    if (uvselectedValue != uvli2Value && !isClick) {
                        $(uvlis).removeClass("current");
                        $(uvlis[1]).addClass("current");
                        GetUv(serialId1, "L", false);
                        GetUv(serialId2, "R", false);
                    } else {
                        SetUv();
                    }
                }
            }
        }
    });
}

function SetSale() {
    if (saleArr.length == 2) {
        var countL = saleArr[0];
        var countR = saleArr[1];
        if (countL != null && countR != null) {
            $("#sale_count_l").removeClass("no_data");
            $("#sale_count_r").removeClass("no_data");
            var lengthL = 0;
            var lengthR = 0;
            if (countL != 0) {
                lengthL = (countL / (countL + countR) * maxNumber);
            }
            if (countR != 0) {
                lengthR = (countR / (countL + countR) * maxNumber);
            }

            SetCompareImage("sale_l_span", lengthL, countL >= countR);
            SetCompareImage("sale_r_span", lengthR, countR >= countL);

            countL = addCommas(countL);
            countR = addCommas(countR);
            countL = countL == 0 ? "暂无数据" : countL;
            countR = countR == 0 ? "暂无数据" : countR;
            $("#sale_count_l").html(countL);
            $("#sale_count_r").html(countR);
            if (countL == "暂无数据") {
                $("#sale_count_l").addClass("no_data");
            }
            if (countR == "暂无数据") {
                $("#sale_count_r").addClass("no_data");
            }
            if (countL != "暂无数据" && countL.length > 0
                && countR != "暂无数据" && countR.length > 0) {
                $("#div_sale").show();
            }
        }
    }
}

function SetUv() {
    if (uvArr.length == 2) {
        var countL = uvArr[0];
        var countR = uvArr[1];
        if (countL != null && countR != null) {
            $("#uv_count_l").removeClass("no_data");
            $("#uv_count_r").removeClass("no_data");
            var lengthL = 0;
            var lengthR = 0;
            if (countL != 0) {
                lengthL = (countL / (countL + countR) * maxNumber);
            }
            if (countR != 0) {
                lengthR = (countR / (countL + countR) * maxNumber);
            }

            SetCompareImage("uv_l_span", lengthL, countL >= countR);
            SetCompareImage("uv_r_span", lengthR, countR >= countL);
            countL = addCommas(countL);
            countR = addCommas(countR);
            countL = countL == 0 ? "暂无数据" : countL;
            countR = countR == 0 ? "暂无数据" : countR;
            $("#uv_count_l").html(countL);
            $("#uv_count_r").html(countR);
            if (countL == "暂无数据") {
                $("#uv_count_l").addClass("no_data");
            }
            if (countR == "暂无数据") {
                $("#uv_count_r").addClass("no_data");
            }
            $("#div_uv").show();
        }
    }
}
//显示千分位
function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

//初始化投票数据
function initVote() {
    var cookieName = "serialpkvote", key = queryCsIdArr.clone().sort().join("_"), cookieV = getCookie(cookieName);
    if (cookieV != null && (cookieArr = cookieV.split(','), cookieArr.indexOf(key) != -1)) {
        $("#vote-vl,#vote-vr").hide();
        $("#vote-gray-vl,#vote-gray-vr").show();
    }

    $("#vote-vl").click(function (e) {
        e.preventDefault();
        if (queryCsIdArr[0] < queryCsIdArr[1])
            voteSerial(1, true);
        else
            voteSerial(2, true);
    });
    $("#vote-vr").click(function (e) {
        e.preventDefault();
        if (queryCsIdArr[0] < queryCsIdArr[1])
            voteSerial(2, false);
        else
            voteSerial(1, false);
    });


    $.ajax({
        url: "http://api.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=getvote&csid=" + queryCsIdArr.join(',')
        , cache: false
        , dataType: "jsonp"
        , jsonpCallback: "getVoteCallback"
        , success: function (data) {
            showVote(data);
        }
    });
}

//投票 flag id小1大2 
function voteSerial(flag, isLeft) {
    var cookieName = "serialpkvote", key = queryCsIdArr.clone().sort().join("_"), cookieV = getCookie(cookieName), cookieArr = [];
    if (cookieV != null && (cookieArr = cookieV.split(','), cookieArr.indexOf(key) != -1)) {
        return;
    }
    $.ajax({
        url: "http://api.car.bitauto.com/carinfo/serialcomparevote.ashx?csids=" + queryCsIdArr.join(',') + "&flag=" + flag,
        cache: false,
        dataType: "jsonp",
        jsonpCallback: "voteCallback",
        success: function (data) {
            showVote(data);
            //按钮状态
            var voteId = isLeft ? "con-sl" : "con-sr";
            $("#" + voteId).find(".jia1").show().fadeOut(2000);
            $("#vote-vl,#vote-vr").hide();
            $("#vote-gray-vl,#vote-gray-vr").show();
            //
            cookieArr.push(key);
            if (cookieArr.length > 9) cookieArr.splice(0, 1);
            setCookie(cookieName, cookieArr.join(','), 1, '/', 'car.bitauto.com');
        }
    });
}
//展示投票条长度
function showVote(data) {
    var left = data[queryCsIdArr[0]], right = data[queryCsIdArr[1]],
        leftper = (left == 0 && right == 0) ? 50 : parseInt((left / (left + right)) * 100),
        rigthper = 100 - leftper;
    if (left >= right) {
        if (left == right) {
            $("#con-sl").removeClass("win-box");
            $("#con-sr").removeClass("win-box");
        } else {
            $("#con-sl").addClass("win-box");
            $("#con-sr").removeClass("win-box");
        }
        $("#bft_left").addClass("red").removeClass("gray").siblings().removeClass("red").addClass("gray");

        $("#bft_left").animate({ width: leftper + "%" }, 500, function () { }).html("" + leftper + "%" + "");
        $("#bft_right").css({ width: rigthper + "%" }).html("" + rigthper + "%" + "");
    } else {
        $("#con-sr").addClass("win-box");
        $("#con-sl").removeClass("win-box");
        $("#bft_right").addClass("red").removeClass("gray").siblings().removeClass("red").addClass("gray");

        $("#bft_left").css({ width: leftper + "%" }).html("" + leftper + "%" + "");
        $("#bft_right").animate({ width: rigthper + "%" }, 500, function () { }).html("" + rigthper + "%" + "");
    }
    //$("#bft_left").animate({ width: leftper + "%" }, 500, function () { }).html("" + leftper + "%" + "");   
}
//取Cookie
function getCookie(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) {
        return unescape(arr[2]);
    }
    return null;
}
//设置Cookies
function setCookie(name, value, expires, path, domain, secure) {
    var today = new Date();
    today.setTime(today.getTime());
    if (expires) {
        expires = expires * 1000 * 60 * 60 * 24;
    }
    var expires_date = new Date(today.getTime() + (expires));
    document.cookie = name + '=' + escape(value) +
            ((expires) ? ';expires=' + expires_date.toGMTString() : '') + //expires.toGMTString() 
            ((path) ? ';path=' + path : '') +
            ((domain) ? ';domain=' + domain : '') +
            ((secure) ? ';secure' : '');
}
//删除Cookies
function deleteCookie(name, path, domain) {
    if (getCookie(name)) document.cookie = name + '=' +
                ((path) ? ';path=' + path : '') +
                ((domain) ? ';domain=' + domain : '') +
                ';expires=Thu, 01-Jan-1970 00:00:01 GMT';
}


//处理焦点图片
function fillImageBox(imageArray) {
    var slHtml = [], srHtml = [];
    if ((imageArray[0].imagelist.length == imageArray[1].imagelist.length)
        && (imageArray[0].imagelist.length == 5)) {
        for (var i = 0; i < imageArray[0].imagelist.length; i++) { //去除都是空白的照片
            var imgL = imageArray[0].imagelist[i].imgurl;
            var imgR = imageArray[1].imagelist[i].imgurl;
            if (imgL != null && imgR != null && imgL == imgR) {
                imageArray[0].imagelist.splice(i, 1);
                imageArray[1].imagelist.splice(i, 1);
                i--;
            }
        }
    }
    $.each(imageArray[0].imagelist, function (i, n) {
        var link = n.imglink == "" ? "javascript:;" : n.imglink;
        var target = n.imglink == "" ? "" : "target=\"_blank\"";
        slHtml.push("<li><a href=\"" + link + "\" " + target + "><img src=\"" + n.imgurl.replace("_4.", "_3.") + "\" alt=\"\"></a></li>");
    });
    $("#photo-sl").html(slHtml.join(''));
    $.each(imageArray[1].imagelist, function (i, n) {
        var link = n.imglink == "" ? "javascript:;" : n.imglink;
        var target = n.imglink == "" ? "" : "target=\"_blank\"";
        srHtml.push("<li><a href=\"" + link + "\" " + target + "><img src=\"" + n.imgurl.replace("_4.", "_3.") + "\" alt=\"\"></a></li>");
    });
    $("#photo-sr").html(srHtml.join(''));

    $("#photobox-sl").autoslider();
    $("#photobox-sr").autoslider();

}

