var CarCompare = {
    KoubeiDiffUrl: "/handlers/GetKoubeiImpression.ashx?csids=",
    CarId1: 0,
    CarId2: 0,
    CsId1: 0,
    CsId2: 0,
    DiffDetailHtmlArray: new Array(),
    TitleHtmlArray: new Array(),
    SaleCountUrl: "http://api.easypass.cn/indexDataProvider/GetIndexData.ashx?indextype=sale&date={date}&csid={csid}",
    LocalKey: "CarH5CompareHistory",
    Init: function() {
        var self = this;
        if (!CarDetailJson || !carCompareJson) return;
        this.CarId1 = CarDetailJson[0]["CarId"];
        this.CarId2 = CarDetailJson[1]["CarId"];
        this.CsId1 = CarDetailJson[0]["CsId"];
        this.CsId2 = CarDetailJson[1]["CsId"];
        this.KoubeiDiffUrl = this.KoubeiDiffUrl + this.CsId1 + "," + this.CsId2;
        self.CreateDiffHtml();
        self.GetKoubeiDiff();
        self.GetSaleCount();
        self.SetCompareHistory();
    },

    GetKoubeiDiff: function() {
        var self = this;
        var url = this.KoubeiDiffUrl;
        $.getJSON(url, function(data) {
            if (!data || !data[self.CsId1] || !data[self.CsId2]) return;
            var koubeiHtml = new Array();
            var good1 = data[self.CsId1].good;
            var bad1 = data[self.CsId1].bad;
            var good2 = data[self.CsId2].good;
            var bad2 = data[self.CsId2].bad;
            while (good1.length > 5) {
                good1.pop();
            }
            while (bad1.length > 5) {
                bad1.pop();
            }
            while (good2.length > 5) {
                good2.pop();
            }
            while (bad2.length > 5) {
                bad2.pop();
            }
            var length = good1.length + bad1.length + good2.length + bad2.length;
            if (length == 0) return;
            koubeiHtml.push("<h3>口碑差异</h3>");
            koubeiHtml.push("<table class=\"label-list\">");
            for (var i = 0; i < length; i++) {
                koubeiHtml.push("<tr>");
                if (i > (good1.length + bad1.length) && i > (good2.length + bad2.length)) {
                    break;
                }
                if (i < good1.length) {
                    var wordArr = good1[i].split("|");
                    koubeiHtml.push("<td><span class=\"red\"><a href=\"http://car.m.yiche.com/" + CarDetailJson[0]["CsAllSpell"] + "/koubei/word/" + wordArr[0] + "/?tagname=" + (wordArr.length > 1 ? wordArr[1] : "") + "#acWord\">" + wordArr[0] + "</a></span></td>");
                } else if (i >= good1.length && i < (bad1.length + good1.length)) {
                    var wordArr = bad1[i - good1.length].split("|");
                    koubeiHtml.push("<td><span><a href=\"http://car.m.yiche.com/" + CarDetailJson[0]["CsAllSpell"] + "/koubei/word/" + wordArr[0] + "/?tagname=" + (wordArr.length > 1 ? wordArr[1] : "") + "#acWord\">" + wordArr[0] + "</a></span></td>");
                } else {
                    koubeiHtml.push("<td></td>");
                }
                koubeiHtml.push("<td class=\"center\"></td>");
                if (i < good2.length) {
                    var wordArr = good2[i].split("|");
                    koubeiHtml.push("<td><span class=\"red\"><a href=\"http://car.m.yiche.com/" + CarDetailJson[1]["CsAllSpell"] + "/koubei/word/" + wordArr[0] + "/?tagname=" + (wordArr.length > 1 ? wordArr[1] : "") + "#acWord\">" + wordArr[0] + "</a></span></td>");
                } else if (i >= good2.length && i < (bad2.length + good2.length)) {
                    var wordArr = bad2[i - good1.length].split("|");
                    koubeiHtml.push("<td><span><a href=\"http://car.m.yiche.com/" + CarDetailJson[1]["CsAllSpell"] + "/koubei/word/" + wordArr[0] + "/?tagname=" + (wordArr.length > 1 ? wordArr[1] : "") + "#acWord\">" + wordArr[0] + "</a></span></td>");
                } else {
                    koubeiHtml.push("<td></td>");
                }
                koubeiHtml.push("</tr>");
            }
            koubeiHtml.push("</table>");
            $(".block[data-type='koubei']").html(koubeiHtml.join("")).show();
        });
    },

    GetSaleCount: function() {
        var self = this;
        var now = new Date();
        var year = now.getFullYear();
        var lastMonth = now.getMonth();
        if (lastMonth == 0) {
            year = year - 1;
            lastMonth = 12;
        } else if (lastMonth < 10) {
            lastMonth = "0" + lastMonth;
        }
        var url1 = self.SaleCountUrl.replace("{date}", year + "" + lastMonth).replace("{csid}", self.CsId1);
        var url2 = self.SaleCountUrl.replace("{date}", year + "" + lastMonth).replace("{csid}", self.CsId2);
        $.when($.ajax({
            url: url1,
            cache: true,
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "GetSaleCount" + self.CarId1
        }), $.ajax({
            url: url2,
            cache: true,
            dataType: "jsonp",
            jsonp: "callback",
            jsonpCallback: "GetSaleCount" + self.CarId2
        })).done(function(data1, data2) {
            if (data1[0] != undefined && data2[0] != undefined) {
                var saleHtmlArr = new Array();
                var count1 = parseInt(data1[0].count);
                var count2 = parseInt(data2[0].count);
                if (count1 == 0 && count2 == 0) return;
                var score1 = count1 / (count1 + count2) * 100 * 0.5 + 25;
                var score2 = 100 - score1;
                saleHtmlArr.push("<h3>" + parseInt(lastMonth) + "月销量对比</h3>");
                saleHtmlArr.push("<div class=\"bar\">");
                saleHtmlArr.push("<div class=\"bl win\" style=\"width: " + score1 + "%;\">");
                saleHtmlArr.push("<span><em>" + count1 + "</em>辆</span>");
                saleHtmlArr.push("</div>");
                saleHtmlArr.push("<div class=\"br\" style=\"width: " + score2 + "%;\">");
                saleHtmlArr.push("<span><em>" + count2 + "</em>辆</span>");
                saleHtmlArr.push("</div>");
                saleHtmlArr.push("</div>");
                $(".block[data-type='saleCount']").html(saleHtmlArr.join("")).show();
            }
        });
    },

    CreateDiffHtml: function() {
        var self = this;
        var loopCount = arrField.length;
        for (var i = 0; i < loopCount; i++) {
            var blockJson = arrField[i];
            var itemInfo = self.CreateItemHtml(blockJson.items);
            //self.CreateTitleHtml(blockJson, itemInfo);
        }
        self.TitleHtmlArray.push(" <a href=\"http://car.m.yiche.com/chexingduibi/?carids=" + self.CarId1 + "," + self.CarId2 + "\" class=\"bar-more\">查看详细车型对比>></a>");

        $("div.block").first().append(self.TitleHtmlArray.join(""));
        $("div.block").first().after(self.DiffDetailHtmlArray.join(""));
        //动态加载横条
        $("[data-bar]").lazybar();
    },
    CreateTitleHtml: function(blockJson, itemInfo) {
        var self = this;
        var blockHtmlArray = new Array();
        var valuePercent1 = 50; // 
        var valuePercent2 = 50; //100 - valuePercent1;
        if (itemInfo[0] > 0 || itemInfo[1] > 0) {
            valuePercent1 = parseFloat(itemInfo[0]) / (parseFloat(itemInfo[0]) + parseFloat(itemInfo[1])) * 100 * 0.5 + 25; //乘以0.5，是保证2边最小留25%
            valuePercent2 = 100 - valuePercent1;
        }

        blockHtmlArray.push("<div class=\"block\">");
        blockHtmlArray.push("<h3>" + blockJson.title + "</h3>");
        if (blockJson.isShowDiff) {
            blockHtmlArray.push("<h5>" + blockJson.title + " 差" + Math.abs(parseInt(itemInfo[0]) - parseInt(itemInfo[1])) + "项</h5>");
        }
        blockHtmlArray.push("<div class=\"bar mt20\">");
        blockHtmlArray.push("<div class=\"bl win\" style=\"width: " + valuePercent1 + "%;\">");
        if (blockJson.isShowDiff) {
            blockHtmlArray.push("<span><em>" + itemInfo[0] + "</em>项</span>");
        } else if (valuePercent1 > valuePercent2 && blockJson.title != "油耗") {
            blockHtmlArray.push("<span>" + blockJson.unit + "</span>");
        } else if (valuePercent1 < valuePercent2 && blockJson.title == "油耗") {
            blockHtmlArray.push("<span>" + blockJson.unit + "</span>");
        }
        blockHtmlArray.push("</div>");
        blockHtmlArray.push("<div class=\"br\" style=\"width: " + valuePercent2 + "%;\">");
        if (blockJson.isShowDiff) {
            blockHtmlArray.push("<span><em>" + itemInfo[1] + "</em>项</span>");
        } else if (valuePercent1 < valuePercent2 && blockJson.title != "油耗") {
            blockHtmlArray.push("<span>" + blockJson.unit + "</span>");
        } else if (valuePercent1 > valuePercent2 && blockJson.title == "油耗") {
            blockHtmlArray.push("<span>" + blockJson.unit + "</span>");
        }
        blockHtmlArray.push("</div>");
        blockHtmlArray.push("</div>");
        blockHtmlArray.push(itemInfo[2]);
        //blockHtmlArray.push("<a href=\"" + (blockJson.moreUrl + self.CarId1 + "," + self.CarId2) + "\" class=\"bar-more\">" + blockJson.moreTitle + "</a>");
        blockHtmlArray.push("</div>");

        self.DiffDetailHtmlArray.push(blockHtmlArray.join(""));
        self.TitleHtmlArray.push("<div data-bar=\"lazy\" class=\"bar\">");
        self.TitleHtmlArray.push("<div class=\"bl win\" style=\"width: 50%;\" data-width=\"" + valuePercent1 + "%\">");
        self.TitleHtmlArray.push("<span>" + blockJson.title + "</span>");
        self.TitleHtmlArray.push("</div>");
        self.TitleHtmlArray.push("<div class=\"br\" style=\"width: 50%;\" data-width=\"" + valuePercent2 + "%\">");
        self.TitleHtmlArray.push("<span>" + blockJson.title + "</span>");
        self.TitleHtmlArray.push("</div>");
        self.TitleHtmlArray.push("</div>");
    },
    CreateItemHtml: function(itemJson) {
        var self = this;
        var itemHtmlArray = new Array();
        var score1 = 0;
        var score2 = 0;
        if (itemJson.length > 0) {
            itemHtmlArray.push("<table class=\"paras-list\"><tbody>");
            for (var i = 0; i < itemJson.length; i++) {
                var value1;
                var value2;
                var fuelType1 = carCompareJson[0][3][19];
                var fuelType2 = carCompareJson[1][3][19];
                var title = itemJson[i]["sTitle"];
                if (itemJson[i].isShow) {
                    if (itemJson[i].sType == "para") {
                        value1 = carCompareJson[0][itemJson[i]["sTrPrefix"]][itemJson[i]["sFieldIndex"]];
                        value2 = carCompareJson[1][itemJson[i]["sTrPrefix"]][itemJson[i]["sFieldIndex"]];
                        var unit = itemJson[i]["unit"];
                        unit = unit == "" ? "" : ("(" + itemJson[i]["unit"] + ")");
                        title = title + unit;
                    } else if (itemJson[i].sType == "matilpara") {
                        var sTrPrefixArr = itemJson[i]["sTrPrefix"].split(",");
                        var sFieldIndexArr = itemJson[i]["sFieldIndex"].split(",");
                        var unitArr = itemJson[i]["unit"].split(",");
                        if (fuelType1 == "电力") {
                            value1 = carCompareJson[0][sTrPrefixArr[1]][sFieldIndexArr[1]];
                        } else {
                            var tempVal1 = carCompareJson[0][sTrPrefixArr[0]][sFieldIndexArr[0]];
                            var tempVal2 = carCompareJson[0][sTrPrefixArr[1]][sFieldIndexArr[1]];
                            value1 = (tempVal1 == "" || tempVal1 == "无" || tempVal1 == "待查" || tempVal1.indexOf("增压") > -1 ? "" : (tempVal1 + unitArr[0]))
                                + (tempVal2 == "" || tempVal2 == "待查" || tempVal2 == "无" ? "" : (tempVal2 + unitArr[1]));
                        }
                        if (fuelType2 == "电力") {
                            value2 = carCompareJson[1][sTrPrefixArr[1]][sFieldIndexArr[1]];
                        } else {
                            var tempVal1 = carCompareJson[1][sTrPrefixArr[0]][sFieldIndexArr[0]];
                            var tempVal2 = carCompareJson[1][sTrPrefixArr[1]][sFieldIndexArr[1]];
                            value2 = (tempVal1 == "" || tempVal1 == "无" || tempVal1 == "待查" || tempVal1.indexOf("增压") > -1 ? "" : (tempVal1 + unitArr[0]))
                                + (tempVal2 == "" || tempVal2 == "待查" || tempVal2 == "无" ? "" : (tempVal2 + unitArr[1]));
                        }
                    }
                    itemHtmlArray.push("<tr><td><span>" + self.GetValueHtml(value1) + "</span></td><td>" + title + "</td><td><span>" + self.GetValueHtml(value2) + "</span></td></tr>");
                }

                if (itemJson[i].sTitle == "整备质量") { //性能动力：最大功率（430）/整备质量（669）；若是纯电动车则提取电机最大功率（870）
                    if (parseFloat(carCompareJson[0][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex]) > 0 && parseFloat(carCompareJson[1][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex]) > 0) {
                        value1 = fuelType1 == "电力" ? carCompareJson[0][16][0] : carCompareJson[0][3][14];
                        value2 = fuelType2 == "电力" ? carCompareJson[1][16][0] : carCompareJson[1][3][14];
                        score1 = isNaN(value1) ? 0 : (value1 / carCompareJson[0][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex]);
                        score2 = isNaN(value2) ? 0 : (value2 / carCompareJson[1][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex]);
                    }
                } else if (itemJson[i].sTitle == "综合工况油耗") { //油耗： 综合工况油耗（782）。值大的长
                    value1 = carCompareJson[0][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex];
                    value2 = carCompareJson[1][itemJson[i].sTrPrefix][itemJson[i].sFieldIndex];
                    score1 = isNaN(value1) ? 0 : value1;
                    score2 = isNaN(value2) ? 0 : value2;
                } else {
                    var compareType = itemJson[i].compareType;
                    if (compareType == "value") { //比大小
                        if (!isNaN(value1) && !isNaN(value2)) {
                            value1 = parseFloat(value1);
                            value2 = parseFloat(value2);
                            if (value1 > value2) {
                                score1 += 1;
                            } else if (value1 < value2) {
                                score2 += 1;
                            } else {
                                score1 += 1;
                                score2 += 1;
                            }
                        }
                    }
                    if (compareType == "ishave") {
                        value1 = carCompareJson[0][itemJson[i]["sTrPrefix"]][itemJson[i]["sFieldIndex"]];
                        value2 = carCompareJson[1][itemJson[i]["sTrPrefix"]][itemJson[i]["sFieldIndex"]];
                        if (value1 == "有" || (arrValue[itemJson[i].sPid] != undefined && arrValue[itemJson[i].sPid].indexOf(value1) > -1)) {
                            score1 += 1;
                        }
                        if (value2 == "有" || (arrValue[itemJson[i].sPid] != undefined && arrValue[itemJson[i].sPid].indexOf(value2) > -1)) {
                            score2 += 1;
                        }
                    }
                }
            }

            itemHtmlArray.push("</tbody></table>");
        }
        return [score1, score2, itemHtmlArray.join("")];
    },
    GetValueHtml: function(value) {
        var valueHtml = value;
        if (value == "有") {
            valueHtml = "<i class=\"icon-disc\"></i>";
        } else if (value == "选配") {
            valueHtml = "<i class=\"icon-circle\"></i>";
        } else if (value == "无") {
            valueHtml = "<i class=\"icon-line\"></i>";
        } else if (value == "") {
            valueHtml = "暂无";
        }
        return valueHtml;
    },
    SetCompareHistory: function() {
        var self = this;
        var historyCar = localStorage.getItem(self.LocalKey);
        var historyCarArr = new Array();
        if (historyCar != null) {
            historyCarArr = historyCar.split(",");
        }
        if (historyCarArr.indexOf(self.CarId2) < 0) {
            historyCarArr.unshift(self.CarId2);
        }
        if (historyCarArr.indexOf(self.CarId1) < 0) {
            historyCarArr.unshift(self.CarId1);
        }
        while (historyCarArr.length > 10) {
            historyCarArr.pop();
        }
        localStorage.setItem(self.LocalKey, historyCarArr.join(","));
    }
};
var arrValue = {
    "685": ["EBD", "CBC", "EBV"],
    "684": ["EBA", "BAS", "BA", "EVA"],
    "698": ["ASR", "TCS", "TRC", "ATC"],
    "700": ["ESP", "DSC", "VSC", "ESC"],
    "552": ["手动", "电动"],
    "467": ["12V", "220V"]
};
var arrField = [
    {
        "title": "车身尺寸",
        "unit": "大",
        "moreTitle": "更多空间差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { "sType": "para", "sTitle": "长", "sPid": 588, "sTrPrefix": 2, "sFieldIndex": 0, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "宽", "sPid": 593, "sTrPrefix": 2, "sFieldIndex": 1, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "高", "sPid": 586, "sTrPrefix": 2, "sFieldIndex": 2, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "轴距", "sPid": 592, "sTrPrefix": 2, "sFieldIndex": 3, "unit": "mm", "isShow": true, "compareType": "value" }
        ]
    },
    {
        "title": "性能动力",
        "unit": "强",
        "moreTitle": "更多动力差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { sType: "para", sTitle: "整备质量", sPid: "669", sTrPrefix: "2", sFieldIndex: "6", unit: "kg", isShow: true },
            { sType: "para", sTitle: "官方0-100公里加速时间", sPid: "650", sTrPrefix: "1", sFieldIndex: "13", unit: "s", isShow: true },
            { sType: "para", sTitle: "最大马力", sPid: "791", sTrPrefix: "3", sFieldIndex: "13", unit: "Ps", isShow: true },
            { sType: "para", sTitle: "最大扭矩", sPid: "429", sTrPrefix: "3", sFieldIndex: "16", unit: "Nm", isShow: true }
        ]
    },
    {
        "title": "油耗",
        "unit": "省",
        "moreTitle": "更多油耗差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { sType: "para", sTitle: "综合工况油耗", sPid: "782", sTrPrefix: "1", sFieldIndex: "10", unit: "L/100km", isShow: true, "compareType": "value" },
            { sType: "para", sTitle: "排量", sPid: "785", sTrPrefix: "1", sFieldIndex: "5", unit: "L", isShow: true },
            { sType: "matilpara", sTitle: "变速箱", sPid: "724,712", sTrPrefix: "1,1", sFieldIndex: "6,7", unit: "挡 ,", isShow: true },
            { sType: "matilpara", sTitle: "进气形式", sPid: "425,408", sTrPrefix: "3,3", sFieldIndex: "4,5", unit: " ,", isShow: true }
        ]
    },
    {
        "title": "安全配置",
        "unit": "项",
        "moreTitle": "更多安全配置差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "儿童锁", sPid: "494", sTrPrefix: "6", sFieldIndex: "21", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "前排侧安全气囊", sPid: "691", sTrPrefix: "6", sFieldIndex: "2", unit: "", isShow: true },
            { sType: "para", sTitle: "前排头部气囊(气帘)", sPid: "690", sTrPrefix: "6", sFieldIndex: "3", unit: "", isShow: true },
            { sType: "para", sTitle: "驾驶位安全气囊", sPid: "682", sTrPrefix: "6", sFieldIndex: "0", unit: "", isShow: true },
            { sType: "para", sTitle: "胎压监测装置", sPid: "714", sTrPrefix: "6", sFieldIndex: "15", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "零压续行(零胎压继续行驶)", sPid: "715", sTrPrefix: "6", sFieldIndex: "16", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "中控门锁", sPid: "493", sTrPrefix: "6", sFieldIndex: "20", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "遥控钥匙", sPid: "538", sTrPrefix: "6", sFieldIndex: "22", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "无钥匙进入系统", sPid: "952", sTrPrefix: "6", sFieldIndex: "26", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "无钥匙启动系统", sPid: "469", sTrPrefix: "6", sFieldIndex: "23", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "发动机防盗系统", sPid: "683", sTrPrefix: "6", sFieldIndex: "25", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "电子防盗系统", sPid: "699", sTrPrefix: "6", sFieldIndex: "24", unit: "", isShow: false, "compareType": "ishave" },
        ]
    },
    {
        "title": "行车辅助",
        "unit": "项",
        "moreTitle": "更多行车辅助差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "自动驻车", sPid: "811", sTrPrefix: "8", sFieldIndex: "6", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "倒车雷达（车后）", sPid: "702", sTrPrefix: "8", sFieldIndex: "10", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "自动泊车入位", sPid: "816", sTrPrefix: "8", sFieldIndex: "17", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "定速巡航", sPid: "545", sTrPrefix: "8", sFieldIndex: "13", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "刹车防抱死（ABS）", sPid: "673", sTrPrefix: "8", sFieldIndex: "0", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "电子制动力分配系统", sPid: "685", sTrPrefix: "8", sFieldIndex: "1", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "紧急制动辅助系统", sPid: "684", sTrPrefix: "8", sFieldIndex: "2", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "牵引力控制", sPid: "698", sTrPrefix: "8", sFieldIndex: "3", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "动态稳定控制系统", sPid: "700", sTrPrefix: "8", sFieldIndex: "4", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "随速助力转向调节(EPS)", sPid: "732", sTrPrefix: "8", sFieldIndex: "5", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "上坡辅助", sPid: "812", sTrPrefix: "8", sFieldIndex: "7", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "陡坡缓降", sPid: "813", sTrPrefix: "8", sFieldIndex: "8", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "泊车雷达（车前）", sPid: "800", sTrPrefix: "8", sFieldIndex: "9", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "倒车影像", sPid: "703", sTrPrefix: "8", sFieldIndex: "11", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "全景摄像头", sPid: "820", sTrPrefix: "8", sFieldIndex: "12", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "自适应巡航", sPid: "893", sTrPrefix: "8", sFieldIndex: "14", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "GPS导航系统", sPid: "516", sTrPrefix: "8", sFieldIndex: "15", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "车道偏离预警系统", sPid: "955", sTrPrefix: "8", sFieldIndex: "25", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "主动刹车/主动安全系统", sPid: "818", sTrPrefix: "8", sFieldIndex: "19", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "整体主动转向系统", sPid: "841", sTrPrefix: "8", sFieldIndex: "20", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "夜视系统", sPid: "819", sTrPrefix: "8", sFieldIndex: "21", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "盲点检测/并线辅助", sPid: "898", sTrPrefix: "8", sFieldIndex: "22", unit: "", isShow: false, "compareType": "ishave" }
        ]
    },
    {
        "title": "内部配置",
        "unit": "项",
        "moreTitle": "更多内部配置差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "天窗型式", sPid: "567", sTrPrefix: "9", sFieldIndex: "5", unit: "", isShow: true },
            { sType: "para", sTitle: "前大灯类型", sPid: "614", sTrPrefix: "10", sFieldIndex: "0", unit: "", isShow: true },
            { sType: "para", sTitle: "中控台液晶屏", sPid: "488", sTrPrefix: "13", sFieldIndex: "12", unit: "", isShow: true },
            { sType: "para", sTitle: "座椅材料", sPid: "544", sTrPrefix: "12", sFieldIndex: "1", unit: "", isShow: true },
            { sType: "para", sTitle: "方向盘前后调节", sPid: "799", sTrPrefix: "11", sFieldIndex: "0", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘上下调节", sPid: "798", sTrPrefix: "11", sFieldIndex: "1", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘调节方式", sPid: "552", sTrPrefix: "11", sFieldIndex: "2", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘记忆设置", sPid: "549", sTrPrefix: "11", sFieldIndex: "3", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "多功能方向盘", sPid: "528", sTrPrefix: "11", sFieldIndex: "5", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘加热", sPid: "956", sTrPrefix: "11", sFieldIndex: "12", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "全液晶仪表盘", sPid: "988", sTrPrefix: "11", sFieldIndex: "13", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "行车电脑显示屏", sPid: "832", sTrPrefix: "11", sFieldIndex: "7", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "HUD抬头数字显示", sPid: "518", sTrPrefix: "11", sFieldIndex: "8", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "后排杯架", sPid: "474", sTrPrefix: "11", sFieldIndex: "10", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "车内电源电压", sPid: "467", sTrPrefix: "11", sFieldIndex: "11", unit: "", isShow: false, "compareType": "ishave" }
        ]
    }
];