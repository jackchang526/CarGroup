
var selectElecCar = {
    param: {
        p: "",  // 价格
        f: "",  // 动力类型
        bl: "",  // 续航里程
        ct: "",  // 充电时间
        g: "",  // 国别
        b: ""   // 车身形式
    },
    reqUrl: "http://select24.car.yiche.com/selectcartool/searchresult?page=1&pagesize=100&",
    bindEvent: function () {
        var that = this;
        $(".sort ul li a ").bind("click", function () {
            var oldclass = $(this).attr("class");
            var id = $(this).attr("t");
            $("a[t^='" + id.split("_")[0] + "_']").removeClass("current");
            if (oldclass == "current") {
                $(this).attr("class", "");
            } else {
                $(this).attr("class", "current");
            }

            //$("#selectResult").removeClass("car-current");
            $("#clearSelected").removeClass("ljt-current");
            that.selectCar(that);
        })
    },
    BindclearSelectedCar: function () {
        $("#clearSelected").bind("click", function () {
            $(".sort ul li a ").each(function () {
                $(this).attr("class", "");
            });

            $("#selectResult").text("共有0款车型符合条件");
            $("#selectResult").addClass("car-current");
            $("#selectResult").attr("href", "javascript:;");
            $("#clearSelected").addClass("ljt-current");

        })
    },
    selectCar: function (that) {
        for (var p in that.param) {
            that.param[p] = "";
        }

        $(".sort ul li a ").each(function () {
            if ($(this).attr("class") == "current") {
                //
                var t = $(this).attr("t");
                var ts = t.split("_");
                if (ts.length > 1) {
                    if (that.param[ts[0]].length == 0) {
                        that.param[ts[0]] = ts[1];
                    } else {
                        that.param[ts[0]] += ("," + ts[1]);
                    }
                }
            }
        });
        var pp = "";
        var idx = 0;
        selectElecCar.setPrice();

        for (var p in that.param) {
            if (that.param[p] == "") {
                continue;
            }
            if (idx == 0) {
                pp = "";
                pp = p + "=" + that.param[p];
            } else {
                pp += ("&" + p + "=" + that.param[p]);
            }
            idx += 1;
        }
        if (pp == "") {
            $("#selectResult").text("共有0款车型符合条件");
            $("#selectResult").addClass("car-current");
            $("#selectResult").attr("href", "javascript:;");
            $("#clearSelected").addClass("ljt-current");
            return;
        }
        if (that.param["f"] == "") {
            pp += "&f=16,128"
        }
        //var pricemin = $("#min-dot span").text();
        //var pricemax = $("#max-dot span").text();

        //if (pricemax == "90+" || pricemax == "不限") {
        //    pp += ("&p=" + pricemin + "-9999");
        //} else {
        //    pp += ("&p=" + pricemin + "-" + pricemax);
        //}

        that.requestData(pp)
        //console.log(pp);
    },
    requestData: function (reqparams) {
        $.ajax({
            url: this.reqUrl + reqparams,
            dataType: "jsonp",
            jsonpCallback: "selectElecCarJsonpCallback",
            cache: true,
            success: function (json) {
                //console.log(json);
                var result = json;

                if (result.Count > 0) { 
                    $("#selectResult").text("共有" + result.Count + "款车型符合条件");
                    $("#selectResult").removeClass("car-current"); 
                    $("#selectResult").attr("href", "../selectcarlist?" + selectElecCar.getJumpParam());
                } else {
                    $("#selectResult").text("共有0款车型符合条件");
                    $("#selectResult").addClass("car-current");
                    $("#selectResult").attr("href", "javascript:;");
                }
            },
            error: function (res) {
                $("#selectResult").text("共有0款车型符合条件");
                $("#selectResult").addClass("car-current");
                $("#selectResult").attr("href", "../selectcarlist");
            }
        });
    },
    getRequestParam: function () {
    },
    getJumpParam: function () {
        var pp = "";
        var idx = 0;
        for (var p in selectElecCar.param) {
            if (selectElecCar.param[p] == "") {
                continue;
            }
            if (idx == 0) {
                pp = "";
                pp = p + "=" + selectElecCar.param[p];
            } else {
                pp += ("&" + p + "=" + selectElecCar.param[p]);
            }
            idx += 1;
        }
        return pp;
    }, 
    setPrice: function () {
        var pricemin = $("#min-dot span").text();
        var pricemax = $("#max-dot span").text();
        if (pricemax == "90+" || pricemax == "不限") {
            selectElecCar.param["p"] = (pricemin + "-9999");
        } else {
            selectElecCar.param["p"] = (pricemin + "-" + pricemax);
        }
    }
} 