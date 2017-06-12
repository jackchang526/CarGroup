//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}

//下拉框选择车款
var CarSelectSim = {
    Container: null,
    CompareLayer: null,
    CarDataUrl: "http://api.car.bitauto.com/CarInfo/masterbrandtoserialforsug.ashx?",
    CarDataParms: "type={type}&pid={pid}&rt={rt}",
    DataType: "master",
    Callback: null,
    MasterName: "",
    MasterId: "",
    SerialId: "",
    SerialName: "",
    IsContinue : true,
    Init: function (container, callback, compareLayer) {
        var self = this;
        if (!CarSelectSim.IsContinue) {
            //console.log("返回");
            return;
        }
        //CarSelectSim.IsContinue = false;
        self.Container = container;
        self.Callback = callback;
        self.CompareLayer = compareLayer;

        self.ShowOrHide();
        //$(function () {
        self.AddEvent();
        //});
        //CarSelectSim.IsContinue = true;
    },
    AddEvent: function () {
        var self = this;
        //点击页面其他地方，关闭下拉框
        //document.getElementById(self.CompareLayer).onclick = function (event) {
        //    event.stopPropagation();
        //};
        $("#" + self.CompareLayer).click(function (event) {
            event.stopPropagation();
        });
        $(document).click(function () {
            if (CarSelectSim.Container && $("#" + CarSelectSim.Container).css("display") == "block") {
                $("#" + CarSelectSim.Container).hide();
            }
        });
        //document.onclick = function (event) {
        //    if (CarSelectSim.Container && $("#" + CarSelectSim.Container).css("display") == "block") {
        //        $("#" + CarSelectSim.Container).hide();
        //    }
        //};
    },
    ShowOrHide: function () {
        var self = this;
        if ($("#" + self.Container).css("display") == "block") {
            $("#" + self.Container).hide();
            return;
        }
        if ($("#" + self.Container).find("div[group='cartype']").length > 0) {
            $("#" + self.Container).find("div[group='cartype']").show();
            $("#" + self.Container).show();
        }
        else if ($("#" + self.Container).find("div[group='serial']").length > 0) {
            $("#" + self.Container).find("div[group='serial']").show();
            $("#" + self.Container).show();
        }
        else if ($("#" + self.Container).find("div[group='master']").length > 0) {
            $("#" + self.Container).find("div[group='master']").show();
            $("#" + self.Container).show();
        }
        else {
            self.GetMaster();
        }
    },
    GetMaster: function () {//获取主品牌
        if (!CarSelectSim.IsContinue) {
            //console.log("返回");
            return;
        }
        CarSelectSim.IsContinue = false;
        var self = this;
        var url = self.CarDataUrl + self.format(self.CarDataParms, { "type": "2", "pid": "0", "rt": "master", "callback": "CarSelectSim.FillMaster" });
        self.getdata(url, "master");
    },
    FillMaster: function (data) {//填充品牌
        if (!data) return;
        var self = this;
        var charList = data["CharList"];
        var dataList = data["DataList"];
        var tempHtml = new Array();
        //tempHtml.push("<div class=\"zcfcbox h398 clearfix\" id=\"CarSelectSimMaster\">");
        tempHtml.push("<div class=\"fuctit\" group=\"master\"><ul class=\"ppname\"><li class=\"current\">品牌<i></i></li></ul></div>");
        tempHtml.push("<div class=\"popup-box doenbox clearfix\" id=\"matser-div\" style=\"position:absolute\" group=\"master\">");

        if (charList && charList.length > 0) {
            tempHtml.push("<div class=\"pinpzm\" id=\"master-index_letters\">");
            for (var i = 0; i < charList.length ; i++) {
                tempHtml.push("<div onclick=\"CarSelectSim.LetterClick(this, '" + charList[i] + "');\"><a href=\"javascript:void(0)\">" + charList[i] + "</a></div>");
            }
            tempHtml.push("</div>");
        }
        tempHtml.push("<div class=\"pinp_rit\" id=\"\"><div class=\"pinp_main pinptext\" id=\"master-index_list\">");

        if (dataList && dataList.length > 0) {
            var charSpell = dataList[0]["tSpell"];
            tempHtml.push("<div class=\"pinp_main_zm\" id=\"master-indexletters_" + charSpell + "\">");
            for (var i = 0; i < dataList.length; i++) {
                if (charSpell != dataList[i]["tSpell"]) {
                    charSpell = dataList[i]["tSpell"];
                    tempHtml.push("</div><div class=\"pinp_main_zm\" id=\"master-indexletters_" + charSpell + "\">");
                }
                //tempHtml.push("<p><a href=\"###\" onclick=\"CarSelectSim.GetSerial(" + dataList[i]["id"] + ",'" + dataList[i]["name"] + "',event)\">" + dataList[i]["tSpell"] + " " + dataList[i]["name"] + "</a></p>");
                tempHtml.push("<p><a href=\"###\" mid=\"" + dataList[i]["id"] + "\" mname=\"" + dataList[i]["name"] + "\" name=\"master\">" + dataList[i]["tSpell"] + " " + dataList[i]["name"] + "</a></p>");
            }
            tempHtml.push("</div>");
        }

        tempHtml.push("</div></div>");
        tempHtml.push("</div>");
        //tempHtml.push("</div>");
        //self.CloseCarSelectSimple();
        $("#" + self.Container).html(tempHtml.join("")).show();

        self.AddMasterEvent();
        //CarSelectSim.IsContinue = true;
    },
    AddMasterEvent: function () {
        var self = this;
        $("#" + self.Container).find("a[name='master']").each(function () {
            $(this).click(function (event) {
                self.GetSerial($(this).attr("mid"), $(this).attr("mname"), event);
            });
        });
    },
    GetSerial: function (pid, pname, e) {
        e.stopPropagation();
        if (!CarSelectSim.IsContinue) {
            //console.log("返回");
            return;
        }
        CarSelectSim.IsContinue = false;
        var self = this;
        self.MasterId = pid;
        self.MasterName = pname;
        var url = self.CarDataUrl + self.format(self.CarDataParms, { "type": "2", "pid": pid, "rt": "serial" });
        self.getdata(url, "serial");
    },
    FillSerial: function (data) {//填充子品牌
        if (!data) return;
        var self = this;
        var tempHtml = new Array();
        //tempHtml.push("<div class=\"zcfcbox h398 clearfix\" id=\"CarSelectSimSerial\">");
        tempHtml.push("<div class=\"fuctit\" style=\"position:\"absolute\"\"  group=\"serial\">");
        tempHtml.push("<ul class=\"ppname\">");
        tempHtml.push("<li><a href=\"###\" onclick=\"javascript:CarSelectSim.RerturnToMaster('serial');\">品牌</a><i></i></li>");
        tempHtml.push("<li class=\"jomt\">&gt;</li>");
        tempHtml.push("<li class=\"current\">" + self.MasterName + "<i></i></li>");
        tempHtml.push("</ul>");
        tempHtml.push("</div>");

        tempHtml.push("<div class=\"tc-popup-box h330 model remove-lt\" group=\"serial\">");
        tempHtml.push("<div class=\"cxmian\">");
        tempHtml.push("<div class=\"csqubu\">全部</div>");
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                tempHtml.push("<div class=\"pinp_main_zm\">");
                tempHtml.push("<p><i>" + data[i]["gname"] + "</i></p>");
                var child = data[i]["child"];
                if (child && child.length > 0) {
                    for (var j = 0; j < child.length; j++) {
                        //tempHtml.push("<p><a href=\"###\" id=\"" + child[j]["id"] + "\" onclick=\"CarSelectSim.GetCarType(" + child[j]["id"] + ",'" + child[j]["name"] + "')\">" + child[j]["name"] + "</a></p>");
                        tempHtml.push("<p><a href=\"###\" sid=\"" + child[j]["id"] + "\" sname=\"" + child[j]["name"] + "\" name=\"serial\">" + child[j]["name"] + "</a></p>");
                    }
                }
                tempHtml.push("</div>");
            }
        }
        tempHtml.push("</div></div>");
        //tempHtml.push("</div>");
        //self.CloseCarSelectSimple();
        $("#" + self.Container + " > div[group='master']").hide();
        $("#" + self.Container).append(tempHtml.join("")).show();

        self.AddSerialEvent();
        //CarSelectSim.IsContinue = true;
    },
    AddSerialEvent: function () {
        var self = this;
        $("#" + self.Container).find("a[name='serial']").each(function () {
            $(this).click(function (event) {
                self.GetCarType($(this).attr("sid"), $(this).attr("sname"), event);
            });
        });
    },
    GetCarType: function (pid, pname) {
        if (!CarSelectSim.IsContinue) {
            //console.log("返回");
            return;
        }
        CarSelectSim.IsContinue = false;
        var self = this;
        self.SerialId = pid;
        self.SerialName = pname;
        var url = self.CarDataUrl + self.format(self.CarDataParms, { "type": "4", "pid": pid, "rt": "cartype", "callback": "CarSelectSim.FillCarType" });
        self.getdata(url, "cartype");
    },
    FillCarType: function (data) {//填充车款
        if (!data) return;
        var self = this;
        var tempHtml = new Array();
        //tempHtml.push("<div class=\"zcfcbox h398 clearfix\" id=\"CarSelectSimCarType\">");
        tempHtml.push("<div class=\"fuctit\" style=\"position:\"absolute\"\" group=\"cartype\">");
        tempHtml.push("<ul class=\"ppname\">");
        tempHtml.push("<li><a href=\"###\" onclick=\"javascript:CarSelectSim.RerturnToMaster('cartype');\">品牌</a><i></i></li>");
        tempHtml.push("<li class=\"jomt\">&gt;</li>");
        tempHtml.push("<li><a href=\"###\" title=\"" + self.MasterName + "\" onclick=\"javascript:CarSelectSim.ReturnToSerial();\">" + (self.MasterName.length > 5 ? self.MasterName.substr(0, 5) + "..." : self.MasterName) + "</a> <i></i></li>");
        tempHtml.push("<li class=\"jomt\">&gt;</li>");
        tempHtml.push("<li class=\"current\" title=\"" + self.SerialName + "\">" + (self.SerialName.length > 4 ? self.SerialName.substr(0, 4) + "..." : self.SerialName) + "<i></i></li>");
        tempHtml.push("</ul>");
        tempHtml.push("</div>");
        tempHtml.push("<div class=\"tc-popup-box h330 car remove-lt\" group=\"cartype\"><div class=\"cxmian\">");
        var compareCar = CookieForCompare.GetCookie("ActiveNewCompare");
        var carArr = new Array();
        if (compareCar) {
            var carArrCookies = compareCar.split("|");
            for (var i = 0; i < carArrCookies.length; i++) {
                var id = carArrCookies[i].split(",")[0];
                carArr.push(id.substring(2, id.length));
            }
        }
        for (var i = 0; i < data.length; i++) {
            tempHtml.push("<div class=\"pinp_main_zm\">");
            tempHtml.push("<p><i>" + data[i]["yeartype"] + "</i></p>");
            var child = data[i]["child"];
            for (var j = 0; j < child.length; j++) {
                if (carArr.contains(child[j]["id"])) {
                    tempHtml.push("<a class=\"off\" title=\"" + child[j]["name"] + "(已添加)\" href=\"###\">" + child[j]["name"] + "(已添加)</a>");
                } else {
                    tempHtml.push("<p><a href=\"###\" cid=\"" + child[j]["id"] + "\" cname=\"" + child[j]["name"] + "\" name=\"cartype\" title=\"" + child[j]["name"] + "\">" + child[j]["name"] + "</a></p>");
                }
            }
            tempHtml.push("</div>");
        }
        tempHtml.push("</div></div>");
        $("#" + self.Container + " > div[group='serial']").hide();
        $("#" + self.Container).append(tempHtml.join("")).show();

        self.AddCartypeEvent();
        //CarSelectSim.IsContinue = true;
    },
    AddCartypeEvent: function () {
        var self = this;
        $("#" + self.Container).find("a[name='cartype']").each(function () {
            $(this).click(function (event) {
                self.AddCompare($(this).attr("cid"), $(this).attr("cname"));
            });
        });
    },
    getdata: function (rurl, type) {
        var self = this;
        $.ajax({
            url: rurl,
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "getCarType",
            success: function (data) {
                switch (type) {
                    case "master": self.FillMaster(data); break;
                    case "serial": self.FillSerial(data); break;
                    case "cartype": self.FillCarType(data); break;
                    default: break;
                }
                CarSelectSim.IsContinue = true;
            }
        });
    },
    AddCompare: function (carId, carName) {
        var self = this;
        if (typeof (self.Callback) === "function") {
            $("#" + self.Container).html("").hide();
            if (WaitCompareObj) {
                WaitCompareObj.IsCanAddCompare = true;
            }
            self.Callback(carId, carName);
        }
    },
    format: function () {
        if (arguments.length == 0)
            return null;
        var str = arguments[0], obj = arguments[1];
        for (var key in obj) {
            var re = new RegExp('\\{' + key + '\\}', 'gi');
            str = str.replace(re, obj[key]);
        }
        return str;
    },
    LetterClick: function (obj, charSpell) {
        var self = this;
        var elem = document.getElementById("master-indexletters_" + charSpell);
        var top = self.MathOffsetTop(elem, "matser-div");
        document.getElementById("master-index_list").scrollTop = top;
    },
    MasterClick: function () {
        var self = this;
        self.GetSerial(this.getAttribute("id"));
    },
    MathOffsetTop: function (curNode, id) {
        var topHeight = 0;
        if (!curNode)
            return topHeight;
        while (curNode && curNode.getAttribute("id") != id) {
            topHeight += curNode.offsetTop;
            curNode = curNode.offsetParent;
        }
        return topHeight;
    },
    GetCookie: function (name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    },
    RerturnToMaster: function (type) {
        var self = this;
        if (type == "serial") {
            $("#" + self.Container + " > div[group='serial']").remove();
        }
        else if (type == "cartype") {
            $("#" + self.Container + " > div[group='serial'],div[group='cartype']").remove();
        }
        $("#" + self.Container + " > div[group='master']").show();
    },
    ReturnToSerial: function () {
        var self = this;
        $("#" + self.Container + " > div[group='cartype']").remove();
        $("#" + self.Container + " > div[group='serial']").show();
    },
    Close: function () {
        var self = this;
        $("#" + self.Container).html("").hide();
    }
}