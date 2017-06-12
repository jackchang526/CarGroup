var page = 1, sort = 0, pageSize = 40, from = 0;
var date = new Date();
var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();
var SearchCarList = {
    ParameterWhiteList: ["w", "f", "p", "l", "g", "more", "b", "lv", "c", "t", "dt", "d", "fc", "s", "page", "pagesize"],
    Params: {},
    apiUrl: "",
    Page: page,
    PageSize: pageSize,
    PageCount: 0,
    Sort: sort,
    init: function() {
        var locateUrl = SearchCarList.apiUrl + "?" + SearchCarList.GetAPIQueryString(true);
        if (locateUrl.indexOf("?") > -1) {
            locateUrl += "&external=Fourth&time=" + version;
        } else {
            locateUrl += "?external=Fourth&time=" + version;
        }
        $.ajax({
            type: "get",
            url: locateUrl,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "getcars",
            success: function(data) {
                $("#carListTmpl").tmpl(data).appendTo("#CarListWraper");
                SearchCarList.PageCount = parseInt((data.Count - 1) / SearchCarList.PageSize + 1);
                SearchCarList.BindPager();

                //投放广告
                //$("#CarListWraper").SetAd({currentpagenum: page}); 

                SearchCarList.BindSort();
                SearchCarList.BindGoBack();

                if (data.Count > pageSize) {
                    $(".m-pages").show();
                }
            }
        });
        sessionStorage["listUrl"] = window.location.href;
    },

    /// <summary>
    ///     接口参数初始化 
    /// </summary>
    GetAPIQueryString: function(isForInterface) {
        var qsArray = [];
        var moreParam = SearchCarList.Params["more"];
        if (typeof moreParam != "undefined") {
            switch (moreParam) {
            case "268":
                SearchCarList.Params["more"] = moreParam + "_269"; //2-3门
                break;
            case "270":
                SearchCarList.Params["more"] = moreParam + "_271_272"; //4-6门
                break;
            case "263":
                SearchCarList.Params["more"] = moreParam + "_264"; //4-5座
                break;
            case "204":
                SearchCarList.Params["more"] = moreParam + "_205_206"; //天窗
                break;
            case "141":
                SearchCarList.Params["more"] = moreParam + "_143_144_145_146_148_149"; //四轮碟刹
                break;
            }
        }

        if (typeof SearchCarList.Params["pageSize"] == "undefined") {
            SearchCarList.Params["pageSize"] = pageSize;
        }

        //向接口传递参数时b=3时需要改成lv=1
        if (isForInterface && typeof SearchCarList.Params["b"] !== "undefined" && parseInt(SearchCarList.Params["b"]) === 3) {
            delete SearchCarList.Params["b"];
            SearchCarList.Params["lv"] = 1;
        }

        $.each(SearchCarList.Params, function(key, value) {
            if (key === "page" && value !== "0") {
                page = value;
            }
            if (key === "s") {
                sort = value;
            }
            if (key === "h5from") {
                from = value;
            }
            //特殊处理h5from参数
            if (key === "h5from" && !isForInterface) {
                qsArray.push(key + "=" + value);
            }
            if (SearchCarList.ParameterWhiteList.indexOf(key.toLowerCase()) >= 0) {
                qsArray.push(key + "=" + value);
            }
        });
        return qsArray.join("&");
    },

    BindPager: function() {
        $("#currentPage").html(page + "/" + SearchCarList.PageCount);
        var optionArr = [];
        for (var i = 1; i <= SearchCarList.PageCount; i++) {
            if (i !== page)
                optionArr.push("<option value='" + i + "'>" + i + "</option>");
            else
                optionArr.push("<option selected='true' value='" + i + "'>" + i + "</option>");
        }
        $("#selectPage").html(optionArr.join(""));
        $("#selectPage").change(function() {
            PageClick($("#selectPage option:selected").val());
        });

        //start 列表页
        var par = "h5from=" + from + "&" + util.GetKeyValueString(["ad", "order", "lg", "ly","tele"]);
        $(".car_list a").each(function(index, item) {
            if (SearchCarList.Params["ly"] != null && SearchCarList.Params["ly"] == "xxj") {
                var tempcsid = $(item).parent().attr("id");
                $(item).attr("href", "http://dealer.h5.yiche.com/searchOrder/" + tempcsid + "/0/" + "?leads_source=H001005&" + SearchCarList.GetAPIQueryString(true) + "&" + par);
            } else {
                var href = $(item).attr("href");
                if (href.indexOf("?") >= 0) {
                    $(item).attr("href", href + "&" + par);
                } else {
                    $(item).attr("href", href + "?" + par);
                }
            }
        });
        //end 列表页

        if (page === 1) {
            $("#prevPage").attr("disabled", true);
            $("#prevPage").addClass("m-pages-none");
            $("#prevPage").removeAttr("href");
            if (SearchCarList.PageCount > 1) {
                $("#nextPage").removeClass("m-pages-none");
            } else {
                $("#nextPage").addClass("m-pages-none");
                $("#nextPage").attr("disabled", true);
                $("#nextPage").removeAttr("href");
            }
        } else if (page > 1 && page < SearchCarList.PageCount) {
            $("#prevPage").removeClass("m-pages-none");
            $("#nextPage").removeClass("m-pages-none");
        } else if (page >= SearchCarList.PageCount) {
            $("#nextPage").attr("disabled", true);
            $("#nextPage").removeAttr("href");
            $("#prevPage").removeClass("m-pages-none");
            $("#nextPage").addClass("m-pages-none");
        }

    },
    BindSort: function() {
        switch (parseInt(sort)) {
        case 0:
        case 1:
            if (!$("#followSort").parent().hasClass("current")) {
                $("#followSort").parent().addClass("current");
                //$("#wordSort").parent().removeClass("current");
                $("#priceSort").parent().removeClass("current");
            }
            break;
        case 2:
            if (!$("#priceSort").parent().hasClass("current")) {
                $("#priceSort").parent().addClass("current");
                //$("#wordSort").parent().removeClass("current");
                $("#followSort").parent().removeClass("current");
            }
            $("#priceSort").parent().removeClass("arrow-t-d");
            $("#priceSort").parent().addClass("arrow-d-t");
            break;
        case 3:
            if (!$("#priceSort").parent().hasClass("current")) {
                $("#priceSort").parent().addClass("current");
                //$("#wordSort").parent().removeClass("current");
                $("#followSort").parent().removeClass("current");
            }
            $("#priceSort").parent().removeClass("arrow-d-t");
            $("#priceSort").parent().addClass("arrow-t-d");
            break;
            //case 5:
            //case 6:
            //    if (!$("#wordSort").parent().hasClass("current")) {
            //        $("#wordSort").parent().addClass("current");
            //        $("#followSort").parent().removeClass("current");
            //        $("#priceSort").parent().removeClass("current");
            //    }
            //    break;

        }

        $("#prevPage").bind("click", function(e) {
            if (page > 1)
                PageClick(parseInt(page) - 1);
        });
        $("#nextPage").bind("click", function() {
            if (page < SearchCarList.PageCount)
                PageClick(parseInt(page) + 1);
        });
        $("#followSort").bind("click", function() {
            if (sort === "0")
                return;
            sort = 0;
            Sort(sort);
        });
        //$("#wordSort").bind("click", function () {          
        //    if (sort == 6)
        //        return;
        //    sort = 6;
        //    Sort(sort);
        //});
        $("#priceSort").bind("click", function() {
            if (sort === "2") {
                sort = 3;
            } else {
                sort = 2; //默认升序
            }
            Sort(sort);
        });
    },
    BindGoBack: function() {
        $("#goBack").bind("click", function() {
            //if (typeof window.sessionStorage != 'undefined') {
            //    window.location.href = sessionStorage["sourceUrl"];
            //} else {
            var obj = getSearchObject();
            switch (from) {
            case "fashao":
                window.location.href = "/fashaoyou/" + getReturnQueryString(obj);
                break;
            case "feel":
                window.location.href = "/ganjuekong/" + getReturnQueryString(obj);
                break;
            }
            //}
        });
    }
};

function PageClick(num) {
    page = num;
    var obj = getSearchObject();
    obj["page"] = num;
    location.href = "http://" + window.location.host + window.location.pathname + "?" + getQueryString(obj);
}

function Sort(num) {
    sort = num;
    var obj = getSearchObject();
    obj["s"] = sort;
    obj["page"] = 1;
    location.href = "http://" + window.location.host + window.location.pathname + "?" + getQueryString(obj);
}

function getQueryString(data) {
    var tdata = "";
    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            tdata += "&" + (key) + "=" + (data[key]);
        }
    }
    tdata = tdata.replace(/^&/g, "");
    return tdata;
}

function getReturnQueryString(data) {
    var removeKeyArr = ["leads_source", "h5from", "page", "pagesize"];
    var tdata = "";
    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            if (removeKeyArr.indexOf(key) < 0 && data[key] != undefined)
                tdata += "&" + (key) + "=" + (data[key]);

            //if (key !== "leads_source" && key !== "h5from" && key !== "page" && key !== "pagesize" && data[key] != undefined)
            //    tdata += "&" + (key) + "=" + (data[key]);
        }
    }
    tdata = tdata.replace(/^&/g, "");
    return tdata !== "" ? "?" + tdata : tdata;
}

function getSearchObject() {
    var results = {};
    var url = window.location.search.substr(1);
    if (url) {
        var srchArray = url.split("&");
        var tempArray = [];
        for (var i = 0; i < srchArray.length; i++) {
            if (srchArray[i] !== "") {
                tempArray = srchArray[i].split("=");
                if (typeof tempArray[0] !== "undefined" && tempArray[0] !== "" && tempArray[0] !== null) {
                    if (typeof tempArray[1] !== "undefined" && tempArray[1] !== null) {
                        results[tempArray[0]] = tempArray[1];
                    }
                }
            }
        }
    }
    return results;
}

function getElementById(i) { return document.getElementById(i); }

Array.prototype.remove = function(b) {
    var a = this.indexOf(b);
    if (a >= 0) {
        this.splice(a, 1);
        return true;
    }
    return false;
};
Array.prototype.indexOf = function(value) {
    for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i;
    return -1;
};

function getQueryStringByName(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2];
    return null;
};