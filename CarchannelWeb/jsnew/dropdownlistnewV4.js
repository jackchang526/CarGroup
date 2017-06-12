﻿var requestDatalist = {};
var browserName = navigator.userAgent.toLowerCase();
var BitA = BitA || {};
BitA.Tools = {
    format: function () {
        if (arguments.length == 0)
            return null;
        var str = arguments[0];
        var obj = arguments[1];
        for (var key in obj) {
            var re = new RegExp('\\{' + key + '\\}', 'gi');
            str = str.replace(re, obj[key]);
        }
        return str;
    },
    getStringLength: function (str) {
        var realLength = 0,
            len = str.length,
            charCode = -1;
        for (var i = 0; i < len; i++) {
            charCode = str.charCodeAt(i);
            if (charCode >= 0 && charCode <= 128) realLength += 1;
            else realLength += 2;
        }
        return realLength;
    }
};
//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}
var jq = jQuery;
(function ($) {
    function BindSelectNew(options) {
        this.defaults = {
            url: "http://api.car.bitauto.com/CarInfo/MasterBrandToSerialNew.aspx",
            encode: "utf-8",
            container: { producer: "bit_producer1", master: "bit_master1", brand: "bit_brand1", serial: "bit_serial1", cartype: "bit_cartype1" },
            callback: {},
            onchange: {},
            parent: {}, //未测
            serias: "m",
            include: {},
            dvalue: {},
            checkdata: null, //未测
            field: { deffield: { value: "id", text: "name" } },
            datatype: 5,
            condition: "type={type}&pid={pid}&rt={rt}&serias={serias}&key={rt}_{pid}_{type}_{serias}",
            background: "",
            groupoprtionstyle: { "style": "normal", "color": "#efefef", "align": "center" },
            abbreviation: { "producer": "p", "master": "m", "brand": "b", "serial": "s", "cartype": "t" },
            deftext: {
                producer: { "value": "0", "text": "请选择厂商" },
                master: { "value": "0", "text": "请选择品牌" },
                brand: { "value": "0", "text": "请选择品牌" },
                serial: { "value": "0", "text": "请选择系列" },
                cartype: { "value": "0", "text": "请选择车款" }
            },
            btn: {
                car: {
                    id: "bit_def_btnCar",
                    url: {
                        cartype: {},
                        serial: { "url": "{defurl}{spell}/", params: { "spell": "urlSpell" } },
                        master: { "url": "{defurl}{spell}/", params: { "spell": "urlSpell" } }
                    },
                    defurl: { "url": "http://car.bitauto.com/" }
                },
                price: {
                    id: "bit_def_btnPrice",
                    url: {
                        cartype: {},
                        serial: { "url": "http://car.bitauto.com/{param1}/baojia/", params: { "param1": "urlSpell" }, defparams: {} },
                        master: { "url": "http://price.bitauto.com/keyword.aspx?mb_id={param1}", params: { "param1": "id" } }
                    },
                    defurl: { "url": "http://price.bitauto.com/" }
                }
            },
            gotype: 1,
            bind: null
        };
        $.extend(true, this.defaults, options); //对默认配置深度复制
        //容器不需要深度复制
        if (options["container"]) {
            this.defaults["container"] = options["container"];
        } else {
            //查找默认id
            var defcontainer = {};
            for (var type in this.defaults.container) {
                var obj = document.getElementById(this.defaults.container[type]);
                if (obj)
                    defcontainer[type] = this.defaults.container[type];
            }
            this.defaults["container"] = defcontainer;
        }
        selectValue = {};
        this.Data = {};
        this.bindList();
    };
    //绑定
    BindSelectNew.prototype.bindList = function () {
        var loop = 0;
        //初始化选择框
        for (var type in this.defaults.container) {
            this.setDefaultValue(type);
        }
        for (var type in this.defaults.container) {
            loop++;
            if (loop == 1) this.GetDataList(type, 0);
        }
    }
    //设置初始默认值
    BindSelectNew.prototype.setDefaultValue = function (type) {
        if (type == "master") {
            var content = "<span class=\"default activ\">" + this.getDefaultValue(type).text + "</span><a href=\"javascript:;\" class=\"jt\"></a><div class=\"zcfcbox sele-2016\" style=\"position:absolute;display:none;\"><div class=\"brand_bg\" id=\"" + type + "div\"><div class=\"brand_letters\" id=\"master-index_letters\"></div><div class=\"brand_name_bg\" id=\"select-list-m\"><div class=\"brand_name\" id=\"master-index_list\"></div></div></div></div>";
            $("#" + this.defaults.container[type]).unbind("click").html(content);
        } else if (type == "serial") {
            var content = "<span class=\"default\">" + this.getDefaultValue(type).text + "</span><a href=\"javascript:;\" class=\"jt\"></a><div class=\"zcfcbox clearfix\" style=\"display:none;\" id=\"divCar_serial\"><div class=\"brand_bg models_bg\" id=\"" + type + "div\"><h6><a data-id=\"2\" href=\"javascript:;\" title=\"奔驰\">全部车型</a></h6><div class=\"models_detail_bg\" style=\"height:330px;\"><div class=\"models_detail\"></div></div></div></div>";
            $("#" + this.defaults.container[type]).unbind("click").html(content);
        } else {
            //var content = "<span class=\"sel-item sel-item-disabled\"><span>" + this.getDefaultValue(type).text + "</span><em></em></span><div class=\"popup-box h330 car\" id=\"" + type + "div\" style=\"display:none;\"></div>";
            var content = "<span class=\"default\">" + this.getDefaultValue(type).text + "</span><a href=\"javascript:;\" class=\"jt\"></a><div class=\"zcfcbox clearfix\" style=\"display:none;\"><div class=\"brand_bg models_bg style_bg\" id=\"" + type + "div\" style=\"display: block;\"><div class=\"models_detail_bg\" style=\"height:330px;\"><div class=\"models_detail\"></div></div></div></div>";
            $("#" + this.defaults.container[type]).unbind("click").html(content);
        }
    }
    //获取数据
    BindSelectNew.prototype.GetDataList = function (type, parentDataId) {
        //得到查询条件
        var include = "";
        var conditions = BitA.Tools.format(this.defaults.condition, { "pid": parentDataId, "type": "" + this.defaults.datatype + "", "rt": "" + type + "", "serias": "" + this.defaults.serias + "" });
        if (typeof this.defaults.include != 'undefined' && typeof this.defaults.include[type] != "undefined") {
            conditions += "&include=" + this.defaults.include[type];
        }
        var objName = type + "_" + parentDataId + "_" + this.defaults.datatype + "_" + this.defaults.serias;
        //如果对象包含向上指引
        if (typeof this.defaults.include[type] != 'undefined' && this.defaults.include[type] != null) {
            //objName = objName + "_" + this.defaults.include[type];
        }
        var url = this.defaults.url + "?" + conditions;
        var pro = this;
        $.ajax({ url: url, cache: true, dataType: "script", success: function () { pro.CallBack(type, parentDataId, objName); } })
    }
    //处理异步请求成功后，程序要进行的过程
    BindSelectNew.prototype.CallBack = function (type, parentDataId, objName) {
        //得到返回的数据对象
        var data = requestDatalist[objName];
        var thumType = this.defaults.abbreviation[type];
        if (typeof data == 'undefined' || data == null || $.isEmptyObject(data)) {
            this.setDefaultValue(type);
            return;
        }
        //定义结果数组
        var result = [];
        //当数据筛选的方法不为空时
        if (this.defaults.checkdata != null) result = this.defaults.checkdata(data, thumType);
        else { //如果筛选为空，则把数据变成数组
            var pattern = RegExp(thumType, "gi");
            for (var id in data) {
                result.push(id.replace(pattern, ""));
            }
        }
        //用于绑定的数据
        var bindData = {};
        //赋值用于绑定的数据,并将它添加到控件缓存数据中
        var ilength = result.length;
        for (var i = 0; i < ilength; i++) {
            var index = result[i];
            var entity = data[thumType + index];
            if (typeof entity == 'undefined' || entity == null) continue;
            bindData[thumType + index] = entity;
            if (typeof this.Data[type] == 'undefined' || this.Data[type] == null) this.Data[type] = {};
            this.Data[type][thumType + index] = entity;
        }

        //如果父id大于0,给上一级的数据链表赋值
        var preType = this.getRelatObjctType(type, -1);
        if (parentDataId > 0 && preType != null) {
            var thumType = this.defaults.abbreviation[preType];
            this.Data[preType][thumType + parentDataId]["nl"] = result;
        }
        //添加下拉列表元素
        this.addDataItem(type, bindData);
        //设置默认值
        this.setItemDefaultValue(type);
        //回调函数
        var callbackFunc = this.defaults.callback[type];
        if (typeof callbackFunc != 'undefined' && callbackFunc instanceof Function) {
            callbackFunc(bindData);
        }
    }
    //填充数据项
    BindSelectNew.prototype.addDataItem = function (type, dataObj) {
        var content = [];
        var tempHtml = new Array();
        var thorldList = {},
            objCount = 0,
            objStopCount = 0,
            objSaleCount = 0,
            maxLen = 0,
            groupObj = {},
            groupStopObj = {},
            tempText = "";
        var contentStop = [];
        var charList = [];
        var hasSale = false;
        for (var entity in dataObj) {
            var obj = dataObj[entity]; //得到要绑定的对象
            if (obj == null) continue;

            var ftype = this.defaults.field[type] ? type : "deffield";
            var value = obj[this.defaults.field[ftype]["value"]];
            var text = obj[this.defaults.field[ftype]["text"]];


            if (value == null || value == "" || text == null || text == "") continue;
            var preObj = obj["goid"]; //得到要绑定组
            //判断元素类型
            if ((type == "master" || type == "producer") && thorldList[obj["tSpell"]] == null) {
                thorldList[obj["tSpell"]] = "";
                //thorldValue++;

            } else if ((type == "serial") && groupObj[preObj] == null) {
                if (objCount > 1) {
                    content.push("</dl>");
                }
                groupObj[preObj] = "";
                content.push("<dl><dt><a href=\"javascript:;\">" + obj["goname"] + "</a></dt>");

                objCount++;
            } else if ((type == "cartype") && groupObj[preObj] == null && obj["salestate"] != "停销") {
                if (objSaleCount > 0) {
                    content.push("</dl>");
                }
                hasSale = true;
                groupObj[preObj] = "";
                content.push("<dl><dt>" + obj["goname"] + "</dt>");
                objSaleCount++;

            } else if ((type == "cartype") && groupStopObj[preObj] == null && obj["salestate"] == "停销") {
                if (objStopCount > 0) {
                    contentStop.push("</dl>");
                }
                groupStopObj[preObj] = "";
                contentStop.push("<dl><dt>" + obj["goname"] + "</dt>");
                objStopCount++;
            }
            //为计算字符串长度添加
            tempText = text;
            //判断当前元素是否，并给当前元素加背景色
            //var className = '';
            //if ((type == "master" || type == "producer")
            //         && thorldValue % 2 == 0) className = this.defaults.background;
            //车款价格
            if (type == "cartype") {
                var price = obj["referprice"];
                if (obj["salestate"] == "停销") {
                    contentStop.push("<dd><a href=\"javascript:;\" title=\"" + text + "\"  bita-value=\"" + value + "\" bita-text=\"" + text + "\">");
                    //if (price != undefined || price == "") {
                    //    price = (price == "" || price == 0) ? "暂无" : price + "万";
                    //    contentStop.push("<strong>" + price + "</strong>");
                    //    tempText = price + text;
                    //}
                    contentStop.push("" + text + "</a></dd>");
                } else {
                    content.push("<dd><a href=\"javascript:;\" title=\"" + text + "\"  bita-value=\"" + value + "\" bita-text=\"" + text + "\">");
                    //if (price != undefined || price == "") {
                    //    price = (price == "" || price == 0) ? "暂无" : price + "万";
                    //    content.push("<strong>" + price + "</strong>");
                    //    tempText = price + text;
                    //}
                    content.push("" + text + "</a></dd>");
                }
            } else {
                //如果该类型为第一级
                if (type == "master" || type == "producer") {
                    if (!charList.contains(obj["tSpell"])) {
                        if (objCount > 1) {
                            content.push("</dl>");
                        }
                        content.push("<dl id=\"master-indexletters_" + obj["tSpell"] + "\">");
                    }
                    content.push("<dd><a href=\"javascript:;\" bita-value=\"" + value + "\" bita-text=\"" + text + "\">" + obj["tSpell"] + " " + text + "</a></dd>");
                    if (!charList.contains(obj["tSpell"])) {
                        charList.push(obj["tSpell"]);
                    }
                } else if (type == "serial") {
                    if (obj["csSale"] == "2") {
                        content.push("<dd><a href=\"javascript:;\" bita-value=\"" + value + "\"bita-text=\"" + text + "\">" + text + "<span>停售</span></a></dd>");
                    } else {
                        content.push("<dd><a href=\"javascript:;\"  bita-value=\"" + value + "\"bita-text=\"" + text + "\">" + text + "</a></dd>");
                    }
                } else
                    content.push("<dd><a href=\"javascript:;\" bita-value=\"" + value + "\"bita-text=\"" + text + "\">" + text + "</a></dd>");
            }
            //取车型最大长度
            var currentLength = BitA.Tools.getStringLength(tempText);
            if (currentLength > maxLen) maxLen = currentLength;

            objCount++;
        }
        if (charList && charList.length > 0) {
            //tempHtml.push("<div class=\"pinpzm\" id=\"master-index_letters\">");
            for (var i = 0; i < charList.length; i++) {
                tempHtml.push("<span" + (i == 0 ? " class=\"current\"" : "") + "><a href=\"javascript:;\">" + charList[i] + "</a></span>");
            }
            //tempHtml.push("</div>");
        }
        //if (type == "master") {
        //    content.push("</div></div></div>");
        //}
        //else
        if (type == "cartype") {
            if (hasSale) {
                content.push("</dl>");
            }
            if (contentStop.length > 0) {
                content.push("<p class=\"ts-tit\">以下是停售车款</p>");
                content.push(contentStop.join(''));
            }
            content.push("</dl>");
        }
        //else {
        //    content.push("</div></div>");
        //}

        var typeElement = $("#" + this.defaults.container[type]);
        if (type == "master") {
            typeElement.find(".brand_letters").html(tempHtml.join(''));
            typeElement.find(".brand_name").append(content.join(''));
        } else {
            typeElement.find(".models_detail").append(content.join(''));
            typeElement.find("span.default").addClass("activ");
        }
        objCount++;

        //绑定事件
        this.bindEvent(type);
        //判断该下拉列表是否有下一级，如果有且有默认值 填充数据
        var preObjType = this.getRelatObjctType(type, 1);
        var pro = this;
        if (preObjType != null) {
            if (typeof this.defaults.dvalue[type] != "undefined" && this.defaults.dvalue[type] != null) {
                this.GetDataList(preObjType, this.defaults.dvalue[type]);
            }
        }
    }

    //设置选中值
    BindSelectNew.prototype.setItemDefaultValue = function (type) {
        if (this.defaults.dvalue != null && this.defaults.dvalue[type] != undefined) {
            if (this.defaults.dvalue[type] != "0") {
                var selected = $("#" + this.defaults.container[type]).find(".brand_bg a[bita-value='" + this.defaults.dvalue[type] + "']")
                if (selected.length) {
                    var defaultspan = $("#" + this.defaults.container[type]).find("span.default");
                    $(defaultspan).attr("value", this.defaults.dvalue[type]).html(selected.attr("bita-text"));
                    //置灰不可选择
                    if (type == "cartype") {
                        selected.addClass("none").unbind("click");
                        var text = $(defaultspan).text();
                        var len = BitA.Tools.getStringLength(text);
                        var w = $(defaultspan).width(); //$(this)[0].offsetWidth;
                        if ((len * 6) > w) {
                            $(defaultspan).attr("title", text);
                        }
                    }

                    this.defaults.dvalue[type] = null;
                }
            }
        }
    }
    //获取下拉列表默认值
    BindSelectNew.prototype.getDefaultValue = function (type) {
        switch (type) {
            case "producer":
                return this.defaults.deftext.producer;
            case "master":
                return this.defaults.deftext.master;
            case "brand":
                return this.defaults.deftext.brand;
            case "serial":
                return this.defaults.deftext.serial;
            case "cartype":
                return this.defaults.deftext.cartype;
            default:
                return null;
        }
    }
    //得到上级或者下级的对象
    BindSelectNew.prototype.getRelatObjctType = function (type, step) {
        var threshold = this.getIndexType(type);
        var list = [];
        for (var obj in this.defaults.container) {
            list.push(obj);
        }
        threshold += step;
        if (threshold < 0 || threshold > list.length) return null;
        return list[threshold];
    }
    //得到类型的索引
    BindSelectNew.prototype.getIndexType = function (type) {
        var index = 0;
        for (var obj in this.defaults.container) {
            if (obj == type.toLowerCase()) return index;
            index++;
        }
    }
    //获取所有容器
    BindSelectNew.prototype.getContainer = function () {
        var container = [];
        for (var obj in this.defaults.container) {
            container.push(obj)
        }
        return container;
    }
    //获取选中值
    BindSelectNew.prototype.getValue = function (type) {
        var typeElemeng = $("#" + this.defaults.container[type]).find(".sel-item span");
        return typeElemeng.attr("value") == undefined ? selectValue[type] : typeElemeng.attr("value");
    }
    //绑定事件
    BindSelectNew.prototype.bindEvent = function (type) {
        var self = this;
        //显示隐藏 数据集
        $("#" + this.defaults.container[type]).children(".activ,.jt").click(function () {
            //e.preventDefault();
            var popup = $(this).siblings(".zcfcbox");
            if (popup.css("display") == "none")
                popup.show();
            else
                popup.hide();
            var allContainer = self.getContainer();
            for (var i = 0; i < allContainer.length; i++) {
                if (type != allContainer[i]) {
                    $("#" + self.defaults.container[allContainer[i]]).children(".zcfcbox").hide();
                }
            }
        });

        //绑定下拉框事件
        $("#" + this.defaults.container[type]).find(".popup-box .pop-tt").click(function () { $(this).parent().hide(); })
        $("#" + this.defaults.container[type]).find(".brand_bg dd a").bind("click", function (e) {
            e.preventDefault();
            var value = $(this).attr("bita-value");
            var text = $(this).attr("bita-text");
            $(this).closest(".zcfcbox").prev().siblings("span.default").html(text).attr("value", value);
            $(this).closest(".zcfcbox").hide();
            //初始化子级数据
            var index = self.getIndexType(type);
            var containers = self.getContainer();
            for (var i = index + 1; i < containers.length; i++) {
                self.setDefaultValue(containers[i]);
            }
            //获取类型数据
            selectValue[type] = value;
            //切换事件
            var changeFunc = self.defaults.onchange[type];
            if (typeof changeFunc != 'undefined' && changeFunc instanceof Function) {
                changeFunc({ id: value, name: text });
            }
            //获取下级数据
            var nextType = self.getRelatObjctType(type, 1);
            if (nextType == null) return;
            self.GetDataList(nextType, value);
        });
        $("#" + this.defaults.container[type]).find(".brand_bg .brand_letters span a").bind("click", function (e) {
            e.preventDefault();
            var charSpell = $(this).text();
            var elem = $(this).closest(".brand_bg").find("#master-indexletters_" + charSpell);

            var topHeight = 0;
            var curNode = elem;
            while (curNode && curNode.attr("id") != "masterdiv") {
                topHeight += $(curNode).position().top;
                curNode = $(curNode).offsetParent();
            }
            var brand_name = $(this).closest(".brand_bg").find(".brand_name");
            var curHeight = $(brand_name).scrollTop();
            $(brand_name).scrollTop(Math.abs(topHeight + curHeight));
            $(this).parent().siblings().removeClass("current");
            $(this).parent().addClass("current");
        });
        //空白地点击 隐藏下拉框
        $(document).click(function (e) {
            //e.preventDefault();
            e = e || window.event;
            var target = e.srcElement || e.target;
            if ($(target).closest("span.default").length <= 0 && $(target).closest(".brand_bg").length <= 0 && $(target).closest("a.jt").length <= 0) {
                $(".brand-form .zcfcbox").hide();
            }
        });
    }
    //调用下拉列表对象
    window.BitA.DropDownListNew = function (options) {
        if (!options) options = {};
        return new BindSelectNew(options);
    }
})(jq)
