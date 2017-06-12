var CarCompareObj = {
    ContentTableId: "datatable",
    CompareCarArr: new Array(),
    PageContentArr: new Array(),
    IsShowBar: { isShow: false, barHtml: "", placeholder: "" },
    IsCanChangeCar: false,//是否可以更换车款
    ChangeCarId: 0,
    Init: function () {
        var self = this;
        if (hotCompareCarIdArr) {
            self.GetCompareCar(hotCompareCarIdArr);
        }
        else {
            return;
        }
        self.CreatePageForCompare();
        self.InitOtherCar();
        self.InitCarLayer();
    },
    CreatePageForCompare: function () {
        var self = this;
        if (self.CompareCarArr.length == 0) {
            return;
        }
        var loopCount = arrField.length;
        for (var i = 0; i < loopCount; i++) {
            switch (arrField[i].sType) {
                case "title":
                    self.GetCarTitle(arrField[i]);
                    break;
                case "bar":
                    self.CreateBar(arrField[i], false);
                    break;
                case "noTitlePara":
                    self.CreateNoTitle(arrField[i]);
                    break;
                case "fieldPara":
                    self.CreatePara(arrField[i]);
                    break;
                case "fieldMulti":
                    self.CreateMulti(arrField[i]);
                    break;
            }
        }
        self.CreateBar(arrField[loopCount - 1], true);

        $("#" + self.ContentTableId).html(self.PageContentArr.join(""));
        self.PageContentArr.length = 0;
        self.IsCanChangeCar = true;
        self.InitCarLayer();
    },
    CreatePara: function (arrFieldRow) {
        var self = this;
        var value = self.CompareCarArr[0][arrFieldRow["sTrPrefix"]][arrFieldRow["sFieldIndex"]];
        value = value == "" ? "待查" : value;
        if (!isNaN(value)) {
            value = parseFloat(value);
        }
        var isShow = false;
        for (var i = 1; i < self.CompareCarArr.length; i++) {
            var tempValue = self.CompareCarArr[i][arrFieldRow["sTrPrefix"]][arrFieldRow["sFieldIndex"]];
            tempValue = tempValue == "" ? "待查" : tempValue;
            if (!isNaN(tempValue)) {
                tempValue = parseFloat(tempValue);
            }
            if (value != tempValue) {
                isShow = true;
                self.IsShowBar.isShow = true;
                break;
            }
        }
        if (isShow) {
            self.PageContentArr.push("<tr>");
            for (var i = 0; i < self.CompareCarArr.length; i++) {
                var value = self.CompareCarArr[i][arrFieldRow["sTrPrefix"]][arrFieldRow["sFieldIndex"]];
                //value = value == "" ? "待查" : value;
                if (value == "") {
                    value = "待查";
                }
                else if (value == "选配") {
                    value = "○" + value;
                }
                else if (value == "有") {
                    value = "●标配";
                }
                else if (value == "无") {
                    value = "-" + value;
                }
                else {
                    value = value + (arrFieldRow["unit"] != "" && value != "待查" ? arrFieldRow["unit"] : "")
                }

                if (arrFieldRow["sFieldTitle"] == "车身颜色") {
                    var colorArr = value.split('|');
                    value = "";
                    for (var j = 0; j < colorArr.length; j++) {
                        var rgb = colorArr[j].split(',')[1];
                        if (rgb != "") {
                            value += "<strong><em style=\"background: " + rgb + "\"></em></strong>";
                        }
                    }
                }
                if (arrFieldRow["sPid"] == 577) {
                    switch (value) {
                        case "90号": value = value + "(北京89号)"; break;
                        case "93号": value = value + "(北京92号)"; break;
                        case "97号": value = value + "(北京95号)"; break;
                        default: break;
                    }
                }
                self.PageContentArr.push("<td>");
                self.PageContentArr.push("<section>");
                self.PageContentArr.push("<h5>" + arrFieldRow["sFieldTitle"] + "</h5>");
                self.PageContentArr.push("<p" + (arrFieldRow["sFieldTitle"] == "车身颜色" ? " class=\"color\"" : "") + ">" + value + "</p>");
                self.PageContentArr.push("</section>");
                self.PageContentArr.push("</td>");
            }
            self.PageContentArr.push("</tr>");
        }
    },
    CreateMulti: function (arrFieldRow) {
        var self = this;
        var paramArray = arrFieldRow["sFieldIndex"].split(',');
        var unitArray = arrFieldRow["unit"].split(',');
        
        var pidArray = arrFieldRow["sPid"].split(',');
        var prefixArray = arrFieldRow["sTrPrefix"].split(',');
        var joinCode = arrFieldRow["joinCode"];

        var valueArr = new Array();//最终显示值
        var isShow = false;
        for (var i = 0; i < self.CompareCarArr.length; i++) {
            var valArr = new Array();//单个字段值
            for (var j = 0; j < paramArray.length; j++) {
                if(self.CompareCarArr[i][prefixArray[j]][paramArray[j]] != ""){
                    valArr.push(self.CompareCarArr[i][prefixArray[j]][paramArray[j]] + (unitArray.length > 1 ? unitArray[j] : ""));
                }
            }
            if (arrFieldRow["size"] && arrFieldRow["size"] == "1") {
                valueArr.push(valArr.join(joinCode));
            }
            else if (arrFieldRow["sPid"].indexOf("699") > -1) {
                if (valArr.contains("有")) {
                    valueArr.push("有");
                }
                else if (valArr.contains("无")) {
                    valueArr.push("无");
                }
                else {
                    valueArr.push("待查");
                }
            }
            else {
                for (var k = 0; k < valArr.length; k++) {
                    if (valArr[k] != "") {
                        valueArr.push(valArr[k]);
                        break;
                    }
                }
            }
        }
        if (valueArr.length > 0) {
            var value = valueArr[0];
            for (var i = 1; i < valueArr.length; i++) {
                if (value != valueArr[i]) {
                    isShow = true;
                    self.IsShowBar.isShow = true;
                }
            }
        }
        if (isShow) {
            self.PageContentArr.push("<tr>");
            for (var i = 0; i < valueArr.length; i++) {
                self.PageContentArr.push("<td>");
                self.PageContentArr.push("<section>");
                self.PageContentArr.push("<h5>" + arrFieldRow["sFieldTitle"] + "</h5>");
                self.PageContentArr.push("<p>" + (valueArr[i] != "" ? valueArr[i] : "") + "</p>");
                self.PageContentArr.push("</section>");
                self.PageContentArr.push("</td>");
            }
            
            self.PageContentArr.push("</tr>");
        }
    },
    CreateBar: function (arrFieldRow, isLast) {
        var self = this;
        if (self.IsShowBar.isShow) {
            for (var i = 0; i < self.PageContentArr.length; i++) {
                if (self.PageContentArr[i] == self.IsShowBar.placeholder) {
                    self.PageContentArr[i] = self.IsShowBar.barHtml;
                    break;
                }
            }
            self.IsShowBar.isShow = false;
        }
        else {
            if (self.IsShowBar.placeholder != "") {
                for (var i = 0; i < self.PageContentArr.length; i++) {
                    if (self.PageContentArr[i] == self.IsShowBar.placeholder) {
                        self.PageContentArr[i] = "";
                        break;
                    }
                }
            }
        }
        if (!isLast) {
            self.IsShowBar.placeholder = "[[" + arrFieldRow["sFieldTitle"] + "]]";
            self.IsShowBar.barHtml = "<tr><td colspan=\"2\"><h3>" + arrFieldRow["sFieldTitle"] + "</h3></td></tr>";
            self.PageContentArr.push(self.IsShowBar.placeholder);
        }
    },
    CreateNoTitle: function (arrFieldRow) {
        var self = this;
        var paramArray = arrFieldRow["sFieldIndex"].split(',');
        var unitArray = arrFieldRow["unit"].split(',');
        var pidArray = arrFieldRow["sPid"].split(',');
        var prefixArray = arrFieldRow["sTrPrefix"].split(',');
        var joinCode = arrFieldRow["joinCode"];

        var valueArr = new Array();
        var isHaveValue = false;
        if (arrFieldRow["sFieldTitle"] == "排量功率") {//排量功率
            for (var i = 0; i < self.CompareCarArr.length; i++) {
                var value = "";
                for (var j = 0; j < paramArray.length; j++) {
                    value += self.CompareCarArr[i][prefixArray[j]][paramArray[j]] == "" ? "" : (self.CompareCarArr[i][prefixArray[j]][paramArray[j]] + (pidArray[j] == "785" ? (self.CompareCarArr[i][3][4].indexOf("增压") > -1 ? "T" : "L") : unitArray[j])) + (i == self.CompareCarArr.length ? "" : joinCode);
                    if (value != "") {
                        isHaveValue = true;
                    }
                }
                valueArr.push(value);
            }
        }
        else if (arrFieldRow["sFieldTitle"] == "档位变速箱") {
            //档位变数箱
            var value = "";
            for (var i = 0; i < self.CompareCarArr.length; i++) {
                for (var j = 0; j < paramArray.length; j = j + 2) {
                    var val1 = self.CompareCarArr[i][prefixArray[j]][paramArray[j]];
                    var val2 = self.CompareCarArr[i][prefixArray[j + 1]][paramArray[j + 1]];
                    if (val1 != "" && val2 != "") {
                        value = val1 + "档&nbsp;" + val2;
                    }
                    else if (val1 != "" && val2 == "") {
                        value = self.CompareCarArr[i][prefixArray[j]][paramArray[j]] + "档变速箱";
                    }
                    else if (val1 == "" && val2 != "") {
                        value = val2 + "变速箱";
                    }

                    if (value != "") {
                        isHaveValue = true;
                    }
                }
                valueArr.push(value);
            }
        }

        if (isHaveValue) {
            self.IsShowBar.isShow = true;
            self.PageContentArr.push("<tr>");
            for (var i = 0; i < valueArr.length; i++) {
                self.PageContentArr.push("<td><span>" + (valueArr[i] == "" ? "待查" : valueArr[i]) + "</span></td>");
            }
            self.PageContentArr.push("</tr>");
        }
    },
    GetCarTitle: function (arrFieldRow) {//初始化车款标题
        var self = this;
        if (self.CompareCarArr.length == 0) {
            return;
        }
        self.IsCanChangeCar = false;
        var carIdArr = new Array();
        var paramArray = arrFieldRow["sFieldIndex"].split(',');
        var prefixArray = arrFieldRow["sTrPrefix"].split(',');

        //车款名称
        self.PageContentArr.push("<tr>");
        for (var i = 0; i < self.CompareCarArr.length; i++) {
            carIdArr.push(self.CompareCarArr[i][paramArray[3]][prefixArray[3]]);
            self.PageContentArr.push("<th class=\"" + (i == 0 ? "left" : "right") + "\" width=\"50%\"><h2>" + (self.CompareCarArr[i][paramArray[0]][prefixArray[0]] == "" ? "" : (self.CompareCarArr[i][paramArray[0]][prefixArray[0]] + "款&nbsp;")) + self.CompareCarArr[i][paramArray[2]][prefixArray[2]] + "</h2></th>");
        }
        self.PageContentArr.push("</tr>");

        //价格
        self.PageContentArr.push("<tr class=\"price\">");
        for (var i = 0; i < self.CompareCarArr.length; i++) {
            self.PageContentArr.push("<td class=\"" + (i == 0 ? "left" : "right") + "\">" + self.CompareCarArr[i][paramArray[1]][prefixArray[1]] + "</td>");
        }
        self.PageContentArr.push("</tr>");

        //更换车款
        self.PageContentArr.push("<tr>");
        self.PageContentArr.push("<td colspan=\"2\" class=\"link\">");
        for (var i = 0; i < self.CompareCarArr.length; i++) {
            self.PageContentArr.push("<div><a href=\"javascript:void(0);\" data-action=\"popup-car\" name=\"changecar\" carid=\"" + self.CompareCarArr[i][paramArray[3]][prefixArray[3]] + "\">更换车款</a></div>");
            if (i == 0) {
                self.PageContentArr.push("<div><a href=\"http://car.m.yiche.com/chexingduibi/?carIDs=" + carIdArr.join(",") + "\" target=\"_parent\">查看详细配置</a></div>");
            }
        }
        self.PageContentArr.push("</td>");
        self.PageContentArr.push("</tr>");
    },
    GetCompareCar: function (hotCompareCarIdArr) {//获取前2个要比较的车款
        var self = this;
        if (hotCompareCarIdArr) {
            for (var i = 0; i < hotCompareCarIdArr.length; i++) {
                self.CompareCarArr.push(self.GetCarInfoByCarId(hotCompareCarIdArr[i]));
            }
        }
    },
    GetCarInfoByCarId: function (carId) {//按车款id查找车款信息
        if (carCompareJson) {
            for (var i = 0; i < carCompareJson.length; i++) {
                if (carCompareJson[i][0][0] == carId) {
                    return carCompareJson[i];
                }
            }
        }
    },
    InitChangeCarEvent: function () {
        var self = this;
        $("#datatable").find("a[name='changecar']").each(function () {
            $(this).click(function () {
                if (!self.IsCanChangeCar) {
                    return;
                }
                self.ChangeCarId = $(this).attr("carid");
            });
        });
    },
    InitOtherCar: function () {
        var self = this;
        if (carCompareJson) {
            $(".swipeLeft").hide();
            $(".swipeLeft-loading").show();
            
            var otherCarHtml = new Array();
            var yearType = "";
            for (var i = 0; i < carCompareJson.length; i++) {
                var carId = carCompareJson[i][0][0];
                if (carCompareJson[i][0][7] != yearType) {
                    yearType = carCompareJson[i][0][7];
                    otherCarHtml.push("<dt><span>" + yearType + "款</span></dt>");
                }
                var isHaveCompare = false;
                for (var j = 0; j < self.CompareCarArr.length; j++) {
                    if (self.CompareCarArr[j][0][0] == carId) {
                        isHaveCompare = true;
                        break;
                    }
                }
                otherCarHtml.push(" <dd" + (isHaveCompare ? " class=\"current\"" : " carid=\"" + carCompareJson[i][0][0] + "\"") + ">");
                otherCarHtml.push("	<a href=\"javascript:void(0);\">");
                otherCarHtml.push("	<p>" + (isHaveCompare ? "[已添加]" : "") + carCompareJson[i][0][1] + "</p>");
                otherCarHtml.push("	<strong>" + carCompareJson[i][1][0] + "</strong>");
                otherCarHtml.push("	</a>");
                otherCarHtml.push("</dd>");
            }
            $(".tt-list").html(otherCarHtml.join(""));
            $(".swipeLeft").show();
            $(".swipeLeft-loading").hide();

            self.ChangeCar();
        }
    },
    ChangeCar: function () {
        var self = this;
        $("dl[class='tt-list'] dd[carid]").each(function () {
            $(this).click(function (e) {
                event.stopPropagation();
                var newcarid = $(this).attr("carid");
                for (var i = 0; i < self.CompareCarArr.length; i++) {
                    if (self.CompareCarArr[i][0][0] == self.ChangeCarId) {
                        self.CompareCarArr[i] = self.GetCarInfoByCarId(newcarid);
                        break;
                    }
                }
                //self.GetCarTitle();
                self.CreatePageForCompare();
                $('.leftmask1').trigger("close");
                self.InitOtherCar();
            });
        });
    },
    InitCarLayer: function () {
        var self = this;
        var $car = $('[data-action=popup-car]');
        $car.rightSwipe({
            isclick: function () {
                if (!self.IsCanChangeCar) {
                    return;
                }
                self.ChangeCarId = $(this).attr("carid");
            },
            clickEnd: function (b) {

                var $leftPopup = this;
                if (b) {
                    var $back = $('.' + $leftPopup.attr('data-back'))
                    $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
                    var $swipeLeft = $leftPopup.find('.swipeLeft');
                    $car.$y2015 = $swipeLeft.find('.y2015-car-02');
                    var $cbox = $car.$y2015.children(0);
                    $cbox.height(document.documentElement.clientHeight - 55);
                }
            }
        })
    }
}

//数组包含元素
Array.prototype.contains = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == item) {
            return true;
        }
    }
    return false;
}

var arrField = [
    { sType: "title", sFieldIndex: "0,1,0,0", sFieldTitle: "", sPid: "", sTrPrefix: "7,0,1,0", unit: "", joinCode: "", scrollId: "" },

   { sType: "bar", sFieldIndex: "", sFieldTitle: "动力", sPid: "", sTrPrefix: "1", unit: "", joinCode: "", scrollId: "" },
   { sType: "noTitlePara", sFieldIndex: "5,14", sFieldTitle: "排量功率", sPid: "785,430", sTrPrefix: "1,3", unit: "L,KW", joinCode: "&nbsp;", isVantage: "1", size: "1" },
   { sType: "noTitlePara", sFieldIndex: "1,0", sFieldTitle: "档位变速箱", sPid: "724,712", sTrPrefix: "4,4", unit: "档,变速箱", joinCode: "&nbsp;", isVantage: "1", size: "1" },

   { sType: "bar", sFieldIndex: "", sFieldTitle: "基本信息-差异", sPid: "", sTrPrefix: "1", unit: "", joinCode: "", scrollId: "params-carinfo" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "保修政策", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "车船税减免", sPid: "", sTrPrefix: "1", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "市区工况油耗", sPid: "783", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "市郊工况油耗", sPid: "784", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "综合工况油耗", sPid: "782", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "网友油耗", sPid: "658", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "易车实测油耗", sPid: "788", sTrPrefix: "1", unit: "L/100km", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "官方0-100公里加速时间", sPid: "650", sTrPrefix: "1", unit: "s", joinCode: "", isVantage: "1", size: "0" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "易车实测0-100公里加速时间", sPid: "786", sTrPrefix: "1", unit: "s", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "最高车速", sPid: "663", sTrPrefix: "1", unit: "km/h", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "乘员人数（含司机）", sPid: "665", sTrPrefix: "1", unit: "个", joinCode: "" },
 { sType: "bar", sFieldIndex: "", sFieldTitle: "车体-差异", sPid: "", sTrPrefix: "3", unit: "", joinCode: "", scrollId: "params-carbody" },
 { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "车身颜色", sPid: "598", sTrPrefix: "0", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车长", sPid: "588", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "车宽", sPid: "593", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "车高", sPid: "586", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "轴距", sPid: "592", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前轮距", sPid: "585", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后轮距", sPid: "582", sTrPrefix: "2", unit: "mm", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "整备质量", sPid: "669", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "满载质量", sPid: "668", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "最小离地间隙", sPid: "589", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "最大涉水深度", sPid: "662", sTrPrefix: "2", unit: "mm", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "接近角", sPid: "591", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "通过角", sPid: "890", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "离去角", sPid: "581", sTrPrefix: "2", unit: "°", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "行李厢容积", sPid: "465", sTrPrefix: "2", unit: "L", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "行李厢最大拓展容积", sPid: "466", sTrPrefix: "2", unit: "L", joinCode: "", isVantage: "1", size: "1" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "行李厢盖开合方式", sPid: "466", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李厢打开方式", sPid: "441", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "车门数", sPid: "563", sTrPrefix: "2", unit: "个", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "车顶型式", sPid: "573", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "车篷型式", sPid: "570", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "车顶行李厢架", sPid: "627", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "后导流尾翼", sPid: "616", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "运动包围", sPid: "597", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "风阻系数", sPid: "670", sTrPrefix: "2", unit: "", joinCode: "", isVantage: "1", size: "0" },
{ sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "货厢形式", sPid: "964", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "26,27", sFieldTitle: "货厢长度", sPid: "965,966", sTrPrefix: "2,2", unit: "mm,mm", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldMulti", sFieldIndex: "28,29", sFieldTitle: "货厢宽度", sPid: "967,970", sTrPrefix: "2,2", unit: "mm,mm", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldMulti", sFieldIndex: "30,31", sFieldTitle: "货厢高度", sPid: "968,969", sTrPrefix: "2,2", unit: "mm,mm", joinCode: ",-", isVantage: "1", size: "1" },
{ sType: "fieldPara", sFieldIndex: "32", sFieldTitle: "车厢形式", sPid: "971", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "33", sFieldTitle: "座位排列", sPid: "972", sTrPrefix: "2", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "34", sFieldTitle: "额定载重量", sPid: "973", sTrPrefix: "2", unit: "T", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "35", sFieldTitle: "最大总重量", sPid: "974", sTrPrefix: "2", unit: "T", joinCode: "" },
 { sType: "bar", sFieldIndex: "", sFieldTitle: "发动机-差异", sPid: "", sTrPrefix: "3", unit: "", joinCode: "", scrollId: "params-carengine" },
 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "发动机位置", sPid: "428", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "发动机型号", sPid: "436", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "排量", sPid: "785", sTrPrefix: "3", unit: "L", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "排量", sPid: "423", sTrPrefix: "3", unit: "mL", joinCode: "" },
 { sType: "fieldMulti", sFieldIndex: "4,5", sFieldTitle: "进气形式", sPid: "425,408", sTrPrefix: "3,3", unit: " ,", joinCode: ","},
 { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "气缸排列型式", sPid: "418", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "汽缸数", sPid: "417", sTrPrefix: "3", unit: "个", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "每缸气门数", sPid: "437", sTrPrefix: "3", unit: "个", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "气门结构", sPid: "410", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "压缩比", sPid: "414", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "缸径", sPid: "415", sTrPrefix: "3", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "行程", sPid: "434", sTrPrefix: "3", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "最大马力", sPid: "791", sTrPrefix: "3", unit: "Ps", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "最大功率", sPid: "430", sTrPrefix: "3", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "最大功率转速", sPid: "433", sTrPrefix: "3", unit: "rpm", joinCode: "", isVantage: "1", size: "1" },
    { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "最大扭矩", sPid: "429", sTrPrefix: "3", unit: "Nm", joinCode: "", isVantage: "1", size: "1" },
	{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "最大扭矩转速", sPid: "432", sTrPrefix: "3", unit: "rpm", joinCode: "", isVantage: "1", size: "1" },
 	{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "特有技术", sPid: "435", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "燃料类型", sPid: "578", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "燃油标号", sPid: "577", sTrPrefix: "3", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "供油方式", sPid: "580", sTrPrefix: "3", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "燃油箱容积", sPid: "576", sTrPrefix: "3", unit: "L", joinCode: "", isVantage: "1", size: "1" },
 { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "缸盖材料", sPid: "419", sTrPrefix: "3", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "缸体材料", sPid: "416", sTrPrefix: "3", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "25", sFieldTitle: "环保标准", sPid: "421", sTrPrefix: "3", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "26", sFieldTitle: "启停系统", sPid: "894", sTrPrefix: "3", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "29", sFieldTitle: "油箱材质", sPid: "978", sTrPrefix: "3", unit: "", joinCode: "" },
	{ sType: "bar", sFieldIndex: "", sFieldTitle: "电池/电动机-差异", sPid: "", sTrPrefix: "16", unit: "", joinCode: "", scrollId: "params-electric" },
	 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "电机最大功率", sPid: "870", sTrPrefix: "16", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "27", sFieldTitle: "电机最大功率-转速", sPid: "871", sTrPrefix: "3", unit: "kW/rpm", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电机最大扭矩", sPid: "872", sTrPrefix: "16", unit: "Nm", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "28", sFieldTitle: "电机最大扭矩-转速", sPid: "873", sTrPrefix: "3", unit: "Nm/rpm", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电机额定功率", sPid: "869", sTrPrefix: "16", unit: "kW", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "系统电压", sPid: "874", sTrPrefix: "16", unit: "V", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "电机类型", sPid: "866", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "普通充电时间", sPid: "879", sTrPrefix: "16", unit: "分钟", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "快速充电时间", sPid: "878", sTrPrefix: "16", unit: "分钟", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "电池电压", sPid: "877", sTrPrefix: "16", unit: "V", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "电池容量", sPid: "876", sTrPrefix: "16", unit: "kWh", joinCode: "", isVantage: "1", size: "1" },
	 { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "电池类型", sPid: "875", sTrPrefix: "16", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "百公里耗电量", sPid: "868", sTrPrefix: "16", unit: "kw/100km", joinCode: "", isVantage: "1", size: "0" },
	 { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "纯电最高续航里程", sPid: "883", sTrPrefix: "16", unit: "km", joinCode: "", isVantage: "1", size: "1" },

	 { sType: "bar", sFieldIndex: "", sFieldTitle: "变速箱-差异", sPid: "", sTrPrefix: "4", unit: "", joinCode: "", scrollId: "params-transmission" },
	    //{ sType: "fieldMulti", sFieldIndex: "1,0", sFieldTitle: "变速箱", sPid: "724,712", sTrPrefix: "4,4", unit: "挡,", joinCode: " " ,size : "1" },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "换挡拨片", sPid: "547", sTrPrefix: "4", unit: "", joinCode: "" },
 	 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电子档杆", sPid: "844", sTrPrefix: "4", unit: "", joinCode: "" },

	 { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "变速箱型号", sPid: "979", sTrPrefix: "4", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "前进挡数", sPid: "980", sTrPrefix: "4", unit: "个", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "倒挡数", sPid: "981", sTrPrefix: "4", unit: "个", joinCode: "" },
	 { sType: "bar", sFieldIndex: "", sFieldTitle: "底盘制动-差异", sPid: "", sTrPrefix: "5", unit: "", joinCode: "", scrollId: "params-bottomstop" },
	 { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车体结构", sPid: "844", sTrPrefix: "5", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "最小转弯半径", sPid: "590", sTrPrefix: "5", unit: "m", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "转向助力", sPid: "735", sTrPrefix: "5", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前制动类型", sPid: "726", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "后制动类型", sPid: "718", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "驻车制动类型", sPid: "716", sTrPrefix: "5", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "驱动方式", sPid: "655", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "空气悬挂", sPid: "814", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "可调悬挂", sPid: "708", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "前悬挂类型", sPid: "728", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后悬挂类型", sPid: "720", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "中央差速器锁", sPid: "733", sTrPrefix: "5", unit: "", joinCode: "" },

    { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "前桥（轴）描述", sPid: "975", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后桥描述", sPid: "976", sTrPrefix: "5", unit: "", joinCode: "" },
	{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "弹簧片数", sPid: "977", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldIndex: "", sFieldTitle: "安全配置-差异", sPid: "", sTrPrefix: "6", unit: "", joinCode: "", scrollId: "params-safeconfig" },
   { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "驾驶位安全气囊", sPid: "682", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "副驾驶位安全气囊", sPid: "697", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前排侧安全气囊", sPid: "691", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前排头部气囊(气帘)", sPid: "690", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "膝部气囊", sPid: "835", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "后排侧安全气囊", sPid: "680", sTrPrefix: "6", unit: "", joinCode: "" },
    { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "后排头部气囊(气帘)", sPid: "679", sTrPrefix: "6", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "安全带气囊", sPid: "845", sTrPrefix: "6", unit: "", joinCode: "" },
	 { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "安全带未系提示", sPid: "836", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "安全带限力功能", sPid: "701", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "安全带预收紧功能", sPid: "678", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "前安全带调节", sPid: "677", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后排安全带", sPid: "675", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后排中间三点式安全带", sPid: "676", sTrPrefix: "6", unit: "", joinCode: "" },
	  { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "电子限速", sPid: "656", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "胎压监测装置", sPid: "714", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "零压续行(零胎压继续行驶)", sPid: "715", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "可溃缩转向柱", sPid: "713", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "溃缩式制动踏板", sPid: "696", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "车内中控锁", sPid: "837", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "中控门锁", sPid: "493", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "儿童锁", sPid: "494", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "遥控钥匙", sPid: "538", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "无钥匙启动系统", sPid: "469", sTrPrefix: "6", unit: "", joinCode: "" },
	   { sType: "fieldMulti", sFieldIndex: "24,25", sFieldTitle: "发动机电子防盗", sPid: "699,683", sTrPrefix: "6,6", unit: "", joinCode: "" },
		{ sType: "bar", sFieldIndex: "", sFieldTitle: "车轮-差异", sPid: "", sTrPrefix: "7", unit: "", joinCode: "", scrollId: "params-wheel" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "前轮胎规格", sPid: "729", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "后轮胎规格", sPid: "721", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前轮毂规格", sPid: "727", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "后轮毂规格", sPid: "719", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "备胎类型", sPid: "707", sTrPrefix: "7", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "轮毂材料", sPid: "704", sTrPrefix: "7", unit: "", joinCode: "" },

{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "轮胎数量", sPid: "982", sTrPrefix: "7", unit: "个", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "行车辅助-差异", sPid: "", sTrPrefix: "8", unit: "", joinCode: "", scrollId: "params-drivingassistance" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "刹车防抱死(ABS)", sPid: "673", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电子制动力分配系统(EBD)", sPid: "685", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "刹车辅助(EBA/BAS/BA/EVA等)", sPid: "684", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "牵引力控制(ASR/TCS/TRC/ATC等)", sPid: "698", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "动态稳定控制系统（ESP）", sPid: "700", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "随速助力转向调节(EPS)", sPid: "732", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "自动驻车", sPid: "811", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "上坡辅助", sPid: "812", sTrPrefix: "8", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "陡坡缓降", sPid: "813", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "泊车雷达(车前)", sPid: "800", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "倒车雷达(车后)", sPid: "702", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "倒车影像", sPid: "703", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "全景摄像头", sPid: "820", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "定速巡航", sPid: "545", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "自适应巡航", sPid: "893", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "GPS导航系统", sPid: "516", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "人机交互系统", sPid: "806", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "自动泊车入位", sPid: "816", sTrPrefix: "8", unit: "", joinCode: "" },
 { sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "并线辅助", sPid: "817", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "19", sFieldTitle: "主动刹车/主动安全系统", sPid: "818", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "20", sFieldTitle: "整体主动转向系统", sPid: "841", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "21", sFieldTitle: "夜视系统", sPid: "819", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "22", sFieldTitle: "盲点检测", sPid: "898", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "23", sFieldTitle: "发动机阻力矩控制系统（EDC/MSR）", sPid: "897", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "24", sFieldTitle: "弯道制动控制系统（CBC）", sPid: "896", sTrPrefix: "8", unit: "", joinCode: "" },
  { sType: "bar", sFieldIndex: "", sFieldTitle: "门窗/后视镜-差异", sPid: "", sTrPrefix: "9", unit: "", joinCode: "", scrollId: "params-doorswindow" },
  { sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "开门方式", sPid: "891", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "电动车窗", sPid: "601", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "防紫外线/隔热玻璃", sPid: "796", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "电动窗防夹功能", sPid: "594", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "天窗开合方式", sPid: "568", sTrPrefix: "9", unit: "", joinCode: "" },
  { sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "天窗型式", sPid: "567", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "后窗遮阳帘", sPid: "595", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "后排侧遮阳帘", sPid: "797", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "后雨刷器", sPid: "596", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "感应雨刷", sPid: "606", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "电动吸合门", sPid: "821", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "行李厢电动吸合门", sPid: "822", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后视镜带侧转向灯", sPid: "830", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "外后视镜记忆功能	", sPid: "625", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "外后视镜加热功能", sPid: "624", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "外后视镜电动折叠功能", sPid: "623", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "外后视镜电动调节", sPid: "622", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "内后视镜防眩目功能", sPid: "621", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "遮阳板化妆镜", sPid: "512", sTrPrefix: "9", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "灯光-差异", sPid: "", sTrPrefix: "10", unit: "", joinCode: "", scrollId: "params-lights" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "前照灯类型", sPid: "614", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "前大灯自动开闭", sPid: " 609", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "前照灯自动清洗功能", sPid: "608", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "前大灯延时关闭", sPid: " 611", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "前大灯随动转向", sPid: " 613", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "前照灯照射范围调整", sPid: " 612", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "会车前灯防眩目功能", sPid: " 610", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "前雾灯", sPid: "607", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "车厢前阅读灯", sPid: "539", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "车厢后阅读灯", sPid: "480", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "车内氛围灯", sPid: "795", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "日间行车灯", sPid: "794", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "LED尾灯", sPid: " 846", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "高位（第三）制动灯", sPid: " 620", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "转向头灯（辅助灯）", sPid: " 828", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "侧转向灯", sPid: "626", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "行李厢灯", sPid: "618", sTrPrefix: "10", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "内部配置-差异", sPid: "", sTrPrefix: "11", unit: "", joinCode: "", scrollId: "params-innerconfig" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "方向盘前后调节", sPid: "799", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "方向盘上下调节", sPid: "798", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "方向盘调节方式", sPid: "552", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "方向盘记忆设置", sPid: "549", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "方向盘表面材料", sPid: "548", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "多功能方向盘", sPid: "528", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "多功能方向盘功能", sPid: "527", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "行车电脑显示屏", sPid: "832", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "HUD抬头数字显示", sPid: "518", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "内饰颜色", sPid: "801", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排杯架", sPid: "474", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "车内电源电压", sPid: "467", sTrPrefix: "11", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "座椅-差异", sPid: "", sTrPrefix: "12", unit: "", joinCode: "", scrollId: "params-chair" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "运动座椅", sPid: "546", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "座椅材料", sPid: "544", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "驾驶座座椅调节方式", sPid: "508", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "驾驶座座椅调节方向", sPid: "507", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "副驾驶座椅调节方式", sPid: "503", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "副驾驶座椅调节方向", sPid: "502", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "驾驶座腰部支撑调节", sPid: "506", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "驾驶座肩部支撑调节", sPid: "802", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "8", sFieldTitle: "前座椅头枕调节", sPid: "515", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "9", sFieldTitle: "后排座椅调节方式", sPid: "833", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "10", sFieldTitle: "后排座位放倒比例", sPid: "482", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "11", sFieldTitle: "前座中央扶手", sPid: "514", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "后座中央扶手", sPid: "475", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "座椅通风", sPid: "804", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "14", sFieldTitle: "座椅加热", sPid: "504", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "15", sFieldTitle: "座椅按摩功能", sPid: "543", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "16", sFieldTitle: "电动座椅记忆", sPid: "803", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "17", sFieldTitle: "儿童安全座椅固定装置", sPid: "495", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "18", sFieldTitle: "第三排座椅", sPid: "805", sTrPrefix: "12", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "娱乐通讯-差异", sPid: "", sTrPrefix: "13", unit: "", joinCode: "", scrollId: "params-pastime" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "车载电话", sPid: "554", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "1", sFieldTitle: "蓝牙系统", sPid: "479", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "2", sFieldTitle: "外接音源接口", sPid: "810", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "4,3", sFieldTitle: "内置硬盘", sPid: "807,808", sTrPrefix: "13,13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "车载电视", sPid: "559", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "扬声器数量", sPid: "523", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "音响品牌", sPid: "473", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "8,9", sFieldTitle: "DVD", sPid: "510,509", sTrPrefix: "13,13", unit: ",碟", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "10,11", sFieldTitle: "CD", sPid: "490,489", sTrPrefix: "13,13", unit: ",碟", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "12", sFieldTitle: "中控台液晶屏", sPid: "488", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "13", sFieldTitle: "后排液晶屏", sPid: "477", sTrPrefix: "13", unit: "", joinCode: "" },
{ sType: "bar", sFieldIndex: "", sFieldTitle: "空调/冰箱-差异", sPid: "", sTrPrefix: "15", unit: "", joinCode: "", scrollId: "params-air" },
{ sType: "fieldPara", sFieldIndex: "0", sFieldTitle: "空调控制方式", sPid: "471", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldMulti", sFieldIndex: "1,2", sFieldTitle: "温度分区控制", sPid: "839,555", sTrPrefix: "15,15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "3", sFieldTitle: "后排独立空调", sPid: "838", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "4", sFieldTitle: "后排出风口", sPid: "478", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "5", sFieldTitle: "空气调节/花粉过滤", sPid: "840", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "6", sFieldTitle: "车内空气净化装置", sPid: "905", sTrPrefix: "15", unit: "", joinCode: "" },
{ sType: "fieldPara", sFieldIndex: "7", sFieldTitle: "车载冰箱", sPid: "485", sTrPrefix: "15", unit: "", joinCode: "" }
];