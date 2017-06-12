var SelectByParam = {
    ParameterWhiteList: ["w", "f", "p", "l", "g", "more", "b", "lv", "c", "t", "dt", "d", "fc", "s", "page", "pagesize"],//查询条件参数名单
    apiUrl: "",
    BindParamsClickEvent: function() {
        $(".fashao a").click(function() {
            var id = ($(this).parent().attr("id"));
            var paramName = id.split("_")[0];
            var paramValue = id.split("_")[1];
            var splitChar = ",";

            var tempArr = [];
            switch (paramName) {
                //单选
                case "b":
                case "d":
                case "fc":
                    if ($(this).parent().attr("class") === "current") {
                        $(this).parent().removeClass("current");
                        $(this).parent().siblings().removeClass("current");
                        delete SelectByParam.paramObj[paramName];
                    } else {
                        $(this).parent().siblings().removeClass("current");
                        $(this).parent().addClass("current");
                        //ID与Value有映射关系
                        if (paramName === "d") {
                            var tempVal = SelectByParam.GetDisplacementStr(paramValue);
                            SelectByParam.paramObj[paramName] = tempVal;
                        } else {
                            SelectByParam.paramObj[paramName] = paramValue;
                        }
                    }
                    break;
                default:
                    if (paramName === "more") {
                        switch (paramValue) {
                            case "268":
                                paramValue = paramValue + "_269"; //2-3门
                                break;
                            case "270":
                                paramValue = paramValue + "_271_272"; //4-6门
                                break;
                            case "263":
                                paramValue = paramValue + "_264"; //4-5座
                                break;
                            case "204":
                                paramValue = paramValue + "_205_206"; //天窗
                                break;
                            case "141":
                                paramValue = paramValue + "_143_144_145_146_148_149"; //四轮碟刹
                                break;
                        }
                    }
                    if (typeof SelectByParam.paramObj[paramName] === "undefined") {
                        $(this).parent().addClass("current");
                        SelectByParam.paramObj[paramName] = paramValue;
                    } else {
                        tempArr = SelectByParam.paramObj[paramName].split(splitChar);
                        var tempIndex = tempArr.indexOf(paramValue);
                        if ($(this).parent().attr("class") === "current") {
                            $(this).parent().removeClass("current");
                            if (tempIndex >= 0) {
                                tempArr.splice(tempIndex, 1);
                            }
                        } else {
                            $(this).parent().addClass("current");
                            if (tempIndex < 0) {
                                tempArr.push(paramValue);
                            }
                        }
                        if (tempArr.length === 0) {
                            delete SelectByParam.paramObj[paramName];
                        } else {
                            SelectByParam.paramObj[paramName] = tempArr.join(splitChar);
                        }
                    }
                    break;
            }

            ////测试
            //console.log(SelectByParam.paramObj);
            //$.each(SelectByParam.paramObj, function (key, value) {
            //    console.log(key + "----" + value);
            //});

            SelectByParam.UpdateCarResult();
        });
    },

    //更新符合条件的车款数量
    GetQueryString: function(isForInterface) {
        if (isForInterface) {
            delete SelectByParam.paramObj["h5from"];
        }

        var qsArray = [];
        $.each(SelectByParam.paramObj, function (key, value) {
            if (SelectByParam.ParameterWhiteList.indexOf(key.toLowerCase()) >= 0) {
                var joinChar = ",";
                switch (key) {
                    case "b": //车身单选
                        if (isForInterface && parseInt(value) === 3) {
                            qsArray.push("lv=1");
                        } else {
                            qsArray.push(key + "=" + value.split(",").join(joinChar));
                        }
                        break;
                    case "more":
                        joinChar = "_";
                        qsArray.push(key + "=" + value.split(",").join(joinChar));
                        break;
                    default:
                        qsArray.push(key + "=" + value.split(",").join(joinChar));
                        break;
                }
            }
        });
        return qsArray.join("&");
    },

    UpdateCarResult: function() {
        var date = new Date();
        var version = date.getFullYear().toString() + (date.getMonth() + 1).toString() + date.getDate().toString() + date.getHours().toString();

        var apiQueryString = SelectByParam.GetQueryString(true);
        var url = SelectByParam.apiUrl;
        if (apiQueryString.length > 0) {
            url += "?" + apiQueryString;
        }
        if (url !== "") {
            if (url.indexOf("?") > -1) {
                url += "&external=Fourth&time=" + version;
            } else {
                url += "?external=Fourth&time=" + version;
            }
        }
        $.ajax({
            url: url,
            dataType: "jsonp",
            jsonpCallback: "getcars",
            cache: true,
            success: function(data) {
                $("#searchResult").html("有" + data.Count + "款车型符合要求");
                if (data.Count === 0) {
                    $("#searchResult").addClass("button_none");
                    $("#searchResult").attr("disabled", true);
                } else {
                    $("#searchResult").removeClass("button_none");
                    $("#searchResult").attr("disabled", false);
                }
            }
        });
        var keyListInParamObj = [];
        $.each(SelectByParam.paramObj, function (key, value) {
            if (SelectByParam.ParameterWhiteList.indexOf(key.toLowerCase()) >= 0) {
                keyListInParamObj.push(key);
            }
            //if (key !== "" && key !== "time" && key !== "h5from") {
            //    keyListInParamObj.push(key);
            //}
        });
        if (keyListInParamObj.length > 0) {
            $("#btDelAll").removeClass("button_del_none");
            $("#btDelAll").attr("disabled", false);
        } else {
            $("#btDelAll").addClass("button_del_none");
            $("#btDelAll").attr("disabled", true);
        }
    },

    //全部清空条件
    ClearParams: function() {
        SelectByParam.paramObj = {};
        $(".fashao a").parent().removeClass("current");
        //价格回位
        var $ruler = $(".ruler");
        $ruler.trigger("setvalue", { min: 0, max: 100 });
        SelectByParam.UpdateCarResult();
    },

    //根据排量获取页面ID
    GetDisplacementId: function(disStr) {
        var disId = "";
        switch (disStr) {
        case "0-1.3":
            disId = "1";
            break;
        case "1.3-1.6":
            disId = "2";
            break;
        case "1.7-2.0":
            disId = "3";
            break;
        case "2.1-3.0":
            disId = "4";
            break;
        case "3.1-5.0":
            disId = "5";
            break;
        case "5.0-9999":
            disId = "6";
            break;
        }
        return disId;
    },

    //根据页面ID获取排量值
    GetDisplacementStr: function(disId) {
        var disStr = "";
        switch (disId) {
        case "1":
            disStr = "0-1.3";
            break;
        case "2":
            disStr = "1.3-1.6";
            break;
        case "3":
            disStr = "1.7-2.0";
            break;
        case "4":
            disStr = "2.1-3.0";
            break;
        case "5":
            disStr = "3.1-5.0";
            break;
        case "6":
            disStr = "5.0-9999";
            break;
        }
        return disStr; 
    },

    //页面初始化
    initFilterCondition: function() {
        $.each(SelectByParam.paramObj, function(key, value) {
            //console.log(key + "----" + value);

            switch (key) {
            case "d":
                var tempVal = SelectByParam.GetDisplacementId(value);
                $("#" + key + "_" + tempVal).addClass("current");
                break;
            case "b": //单选
                if (parseInt(value) === 3) {
                    $("#b_3").siblings().removeClass("current");
                    $("#b_3").addClass("current");
                } else {
                    $("#" + key + "_" + value).addClass("current");
                }
                break;
            case "p":
                var minPrice = value.split("-")[0] < 0 ? 0 : value.split("-")[0];
                var maxPrice = value.split("-")[1] > 100 ? 100 : value.split("-")[1];
                $("#max-dot span").html(maxPrice);
                $("#max-dot").parent().attr("data-index", maxPrice);
                $("#min-dot span").html(minPrice);
                $("#min-dot").parent().attr("data-index", minPrice);
                break;
            case "more":
                $.each(value.split("_"), function(index, item) {
                    $("#" + key + "_" + item).addClass("current");
                });
                break;;
            default:
                $.each(value.split(","), function(index, item) {
                    $("#" + key + "_" + item).addClass("current");
                });

                break;
            }
        });
    },

    init: function() {
        SelectByParam.initFilterCondition();
        SelectByParam.UpdateCarResult();
        SelectByParam.BindParamsClickEvent();
        $("#btDelAll").click(SelectByParam.ClearParams);
    }
};

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