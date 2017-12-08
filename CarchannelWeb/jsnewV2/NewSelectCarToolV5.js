function getElementById(i) { return document.getElementById(i); }
var SelectCar = SelectCar || {};
SelectCar.Tools = {
    getQueryObject: function (queryString) {
        var result = {},
            re = /([^&=]+)=([^&]*)/g, m;
        while (m = re.exec(queryString)) {
            result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
        }
        return result;
    },
    getQueryString: function (data) {
        var tdata = '';
        for (var key in data) {
            tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
        }
        tdata = tdata.replace(/^&/g, "");
        return tdata
    },
    addEvent: function (elm, type, fn, useCapture) {
        if (elm.addEventListener) {
            elm.addEventListener(type, fn, useCapture);
            return true;
        } else if (elm.attachEvent) {
            var r = elm.attachEvent('on' + type, fn);
            return r;
        } else {
            elm['on' + type] = fn;
        }
    },
    getElementByClassName: function (tagName, className) {
        var classObj = document.getElementsByTagName(tagName);
        var len = classObj.length;
        for (var i = 0; i < len; i++) {
            if (classObj[i].className == className) {
                return classObj[i];
                break;
            }
        }
    },
    hasClass: function (element, className) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        return element.className.match(reg);
    },
    addClass: function (element, className) {
        if (!this.hasClass(element, className)) {
            element.className += " " + className;
        }
    },
    removeClass: function (element, className) {
        if (this.hasClass(element, className)) {
            var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
            element.className = element.className.replace(reg, ' ');
        }
    }
}
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
//var adObj = [{ Name: "Level", Pos: 3, SerialId: 3152, Url: "", Imageurl: "", Starttime: "2015-8-21", Endtime: "2015-8-31" }];

//查询条件对象
var conditionObj =
    {
        Price: ""
        , Level: 0					//0不限，1-10对应各级别
        , LevelName: new Array('', '微型车', '小型车', '紧凑型车', '中大型车', '中型车', '豪华车', 'MPV', 'SUV', '跑车', '其他', '面包车', '皮卡', '小型SUV', '紧凑型SUV', '中型SUV', '中大型SUV', '全尺寸SUV')		//级别名称
        , Displacement: ""
        , TransmissionType: 0		//0不限，1手动，2自动
        , TransmissionTypeName: { 1: "手动", 126: "自动", 32: "机械自动（AMT）", 2: "自动（AT）", 4: "手自一体", 8: "无极变速（CVT）", 16: "双离合（DSG）" }
        , DriveType: 0	//驱动方式
        , DriveTypeName: { 1: "前驱", 2: "后驱", 252: "四驱", 4: "全时四驱", 8: "分时四驱", 16: "适时四驱", 32: "智能四驱", 64: "四轮驱动", 128: "前置四驱" }
        , FuelType: 0	//燃料类型
        , FuelTypeName: { 7: "汽油", 8: "柴油", 2: "油电混合", 16: "纯电力", 128: "油气混合", 256: "天然气" }
        , BodyDoors: ""	//车门数
        , PerfSeatNum: ""	//座位数
        , IsWagon: 0	//是否旅行版
        , MoreCondition: []
        , MoreConditionName: []
        , View: 0					//0默认大图显示，1列表显示
        , Sort: 0					//0默认显示关注高-低，1关注低-高，2按价格排列低-高，3价格高-低
        , Brand: 0
        , Country: 0					//0不限，1自主，2合资，3进口
        , BrandName: { 7: "不限", 8: "德系", 9: "美系", 10: "日韩", 11: "欧系", 12: "日本", 16: "韩国" }//new Array('不限', '自主', '合资', '', '进口', "", "德系", "日韩", "美系", "欧系")
        , CountryName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口" }
        , BodyForm: 0				//0不限，1两厢及掀背，2三厢
        , BodyFormName: { 1: "两厢", 2: "三厢", 8: "旅行版" }
        , toolKey: true				//展开开关
        , showPeizhi: false          //是否显示配置
        , Type: "car"
        , Domain: window.location.host
        , PriceTimer: 0
        //, FuelConsumption: ""
        , apiUrl: "http://select24.car.yiche.com/selectcartool/searchresult"
        , Page: 1
        //初始化页面显示
        , InitPageCondition: function () {
            this.InitPrice();
            this.InitLevel();
            this.InitDisplacement();
            this.InitTransmisstionType();
            this.InitMoreCondition();
            this.InitBrandType();
            this.InitCountry();
            this.InitBodyForm();
            this.InitDriveType();
            this.InitFuelType();
            //this.InitFuelConsumption();
            this.InitEvent();
            $("li[id^=more_]").click(this.singleFilterClickEvent);
            $("input[type='checkbox']").click(this.filterClickEvent);
            SetClearCarConditionLink(_SearchUrl); //清空条件
            this.GetSearchResult();
        }
        , InitPageConditionV2: function () {
            this.InitEvent();
            $("input[type='checkbox']").click(this.filterClickEvent);
        }
        , InitEvent: function () {
            var self = this;
            var btnPriceSubmit = getElementById("btnPriceSubmit");
            if (btnPriceSubmit) {
                SelectCar.Tools.addEvent(btnPriceSubmit, "click", function () {
                    if (btnPriceSubmit.className == "btn-md") {
                        return;
                    }
                    var minP = getElementById("p_min").value;
                    var maxP = getElementById("p_max").value;
                    var palert = getElementById("p_alert");
                    if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0))) {
                        palert.innerHTML = "价格不能为空。";
                        palert.style.display = "";
                    }
                    else if (maxP != "" && parseInt(maxP) <= 0) {
                        palert.innerHTML = "请填写正确的价格区间。";
                        palert.style.display = "";
                    }
                    else {
                        if (minP != "" && parseInt(minP) > 0 && (maxP == "" || parseInt(maxP) <= 0)) {
                            GotoPage("p" + minP + "-9999"); return false;
                        }
                        else if (maxP != "" && parseInt(maxP) > 0 && (minP == "" || parseInt(minP) <= 0)) {
                            GotoPage("p0-" + maxP + ""); return false;
                        }
                        else if (minP != "" && parseInt(minP) > 0 && maxP != "" && parseInt(minP) > 0) {
                            if (parseInt(maxP) > parseInt(minP)) {
                                GotoPage("p" + minP + "-" + maxP + ""); return false;
                            }
                            else {
                                palert.innerHTML = "请填写正确的价格区间。";
                                palert.style.display = "";
                            }
                        }
                    }
                }, true);
            }
            var btnPriceCus = getElementById("btnPriceCus");
            if (btnPriceCus) {
                SelectCar.Tools.addEvent(btnPriceCus, "click", function () {
                    var customP = getElementById("p_custom");
                    customP.style.display = "none";
                    $("#p_custom_value").remove();
                    var customN = getElementById("p_custom_null");
                    customN.style.display = "block";
                })
            }
            var transEle = getElementById("trans126");
            if (transEle) {
                SelectCar.Tools.addEvent(transEle, "mouseover", function () {
                    var popup = getElementById("trans_popup");
                    popup.style.display = "block";
                    transEle.className = "last current";
                }, false);
                SelectCar.Tools.addEvent(transEle, "mouseout", function () {
                    var popup = getElementById("trans_popup");
                    popup.style.display = "none";
                    if (!self.TransmissionType || self.TransmissionType < 2)
                        transEle.className = "last";
                }, false);
            }
            var driveType = getElementById("drivetype252");
            if (driveType) {
                SelectCar.Tools.addEvent(driveType, "mouseover", function () {
                    var popup = getElementById("drivetype_popup");
                    popup.style.display = "block";
                    driveType.className = "last current";
                }, false);
                SelectCar.Tools.addEvent(driveType, "mouseout", function () {
                    var popup = getElementById("drivetype_popup");
                    popup.style.display = "none";
                    if (!self.DriveType || self.DriveType <= 2)
                        driveType.className = "last";
                }, false);
            }

            var level = getElementById("level8");
            if (level) {
                SelectCar.Tools.addEvent(level, "mouseover", function () {
                    var popup = getElementById("suv_popup");
                    popup.style.display = "block";
                    level.className = "last current";
                }, false);
                SelectCar.Tools.addEvent(level, "mouseout", function () {
                    var popup = getElementById("suv_popup");
                    popup.style.display = "none";
                    if (!self.Level || [8, 13, 14, 15, 16, 17].indexOf(self.Level) == -1)
                        level.className = "last";
                }, false);
            }

            var priceMin = getElementById("p_min");
            var priceMax = getElementById("p_max");
            var btnPriceSubmit = getElementById("btnPriceSubmit");
            if (priceMin && priceMax) {
                SelectCar.Tools.addEvent(priceMin, "keyup", function () {
                    var valueMin = priceMin.value.replace(/(\D|\d{5})/g, '');
                    var valueMax = priceMax.value;
                    priceMin.value = valueMin;
                    //priceMax.value = valueMax;
                    if (valueMin != "" || valueMax != "") {
                        btnPriceSubmit.className = "btn-md-point";
                    } else {
                        btnPriceSubmit.className = "btn-md";
                    }
                });
                SelectCar.Tools.addEvent(priceMax, "keyup", function () {
                    var valueMin = priceMin.value;
                    var valueMax = priceMax.value.replace(/(\D|\d{5})/g, '');
                    //priceMin.value = valueMin;
                    priceMax.value = valueMax;
                    if (valueMin != "" || valueMax != "") {
                        btnPriceSubmit.className = "btn-md-point";
                    } else {
                        btnPriceSubmit.className = "btn-md";
                    }
                });
            }
        }
        , filterClickEvent: function () {
            var id, self = $(this)[0];
            id = self.id;
            var element = getElementById(id);
            if (element) {
                var conType = id.split('_')[0];
                var conStr = id.split('_')[1];
                if (conType != "more") {
                    return;
                }
                if (element.checked) {
                    if (conditionObj.MoreCondition.indexOf(conStr) > -1) {
                        return;
                    }
                    conditionObj.MoreCondition.push(conStr);
                }
                else {
                    if (conditionObj.MoreCondition.indexOf(conStr) > -1) {
                        conditionObj.MoreCondition.remove(conStr);
                    }
                }
                GotoPage("more");
            }
        }
        , singleFilterClickEvent: function () {
            var id, self = $(this)[0];
            id = self.id;
            var element = getElementById(id);
            if (element) {
                var conType = id.split('_')[0];
                var conStr = id.split('_')[1];
                if (conType != "more") {
                    return;
                }
                if ((conStr >= 279 && conStr <= 284) || conStr == 1) {
                    //座位数
                    for (var i = 279; i <= 284; i++) {
                        if (conStr != i && conditionObj.MoreCondition.indexOf(i) > -1) {
                            conditionObj.MoreCondition.remove(i);
                        }
                        else if (conStr == i && conditionObj.MoreCondition.indexOf(i) == -1 && conStr != 1) {
                            conditionObj.MoreCondition.push(conStr);
                        }
                    }
                }
                //排放
                else if ((conStr >= 120 && conStr <= 122) || conStr == 2 || conStr.indexOf('.') > -1) {
                    for (var i = 120; i <= 122; i++) {
                        if (conStr != i && conditionObj.MoreCondition.indexOf(i) > -1) {
                            conditionObj.MoreCondition.remove(i);
                        }
                    }
                    if (conditionObj.MoreCondition.indexOf(conStr) == -1 && conStr != 2) {
                        conditionObj.MoreCondition.push(conStr);
                    }
                }
                GotoPage("more");
            }
        }
        //初始化价格
        , InitPrice: function () {
            switch (this.Price) {
                case "0-5":
                    getElementById("price1").className = "current";
                    break;
                case "5-8":
                    getElementById("price2").className = "current";
                    break;
                case "8-12":
                    getElementById("price3").className = "current";
                    break;
                case "12-18":
                    getElementById("price4").className = "current";
                    break;
                case "18-25":
                    getElementById("price5").className = "current";
                    break;
                case "25-40":
                    getElementById("price6").className = "current";
                    break;
                case "40-80":
                    getElementById("price7").className = "current";
                    break;
                case "80-9999":
                    getElementById("price8").className = "current";
                    break;
                default:
                    if (this.Price != "") {
                        var customN = getElementById("p_custom_null");
                        customN.style.display = "none";
                        var arrayPrice = this.Price.split("-");
                        if (arrayPrice.length == 2) {
                            getElementById("p_min").value = "";
                            getElementById("p_max").value = "";
                        }
                        var customP = getElementById("p_custom");
                        customP.style.display = "block";
                        $("<li class='current'id='p_custom_value'><a href='javascript:;'>" + this.Price + "万" + "</a></li>").insertBefore(customP);

                    }
                    else
                        getElementById("price0").className = "current";
                    break;
            }
        }
        //初始化级别
        , InitLevel: function () {
            if (this.Level == "")
                this.Level = "0";
            var levelEle = getElementById("level" + this.Level);
            if (levelEle)
                levelEle.className = "current";
            if ([13, 14, 15, 16, 17].indexOf(this.Level) != -1) {
                levelSuvEle = getElementById("level8");
                if (levelSuvEle) {
                    levelSuvEle.className = "last current";
                    levelSuvEle.firstChild.innerHTML = this.LevelName[parseInt(this.Level)];
                    if (levelEle && this.Level != 8) {
                        var firstChild = levelEle.firstElementChild | levelEle.firstChild;
                        firstChild.href = "javascript:;";
                        firstChild.className = "none";
                    }
                }
            }
            //		//轿车级别
            //		if (this.Level <= 6 && this.Level > 0 || this.Level == 63) {
            //			levelEle = getElementById("level63_1");
            //			if (levelEle)
            //				levelEle.className = "current";
            //		}
            //		this.OpenOrCloseJiaocheBox();
        }
        //初始化排量
        , InitDisplacement: function () {
            switch (this.Displacement) {
                case "0-1.3":
                    getElementById("dis1").className = "current";
                    break;
                case "1.3-1.6":
                    getElementById("dis2").className = "current";
                    break;
                case "1.7-2.0":
                    getElementById("dis3").className = "current";
                    break;
                case "2.1-3.0":
                    getElementById("dis4").className = "current";
                    break;
                case "3.1-5.0":
                    getElementById("dis5").className = "current";
                    break;
                case "5.0-9":
                    getElementById("dis6").className = "current";
                    break;
                default:
                    getElementById("dis0").className = "current";
                    break;
            }
        }
        ////初始化油耗
        //, InitFuelConsumption: function () {
        //	switch (this.FuelConsumption) {
        //		case "0-6":
        //			getElementById("fc1").className = "current";
        //			break;
        //		case "6-8":
        //			getElementById("fc2").className = "current";
        //			break;
        //		case "8-10":
        //			getElementById("fc3").className = "current";
        //			break;
        //		case "10-12":
        //			getElementById("fc4").className = "current";
        //			break;
        //		case "12-15":transEle.firstChild
        //			getElementById("fc5").className = "current";
        //			break;
        //		case "15-9999":
        //			getElementById("fc6").className = "current";
        //			break;
        //		default:
        //			getElementById("fc0").className = "current";
        //			break;
        //	}
        //}
        //初始化变速箱类型
        , InitTransmisstionType: function () {
            var transEle = getElementById("trans" + this.TransmissionType.toString());
            if (transEle)
                transEle.className = "current";
            if (this.TransmissionType >= 2) {
                transATEle = getElementById("trans126");
                if (transATEle) {
                    transATEle.className = "last current";
                    transATEle.firstChild.innerHTML = this.TransmissionTypeName[this.TransmissionType];
                    if (transEle && this.TransmissionType != 126) {
                        var firstChild = transEle.firstElementChild | transEle.firstChild;
                        firstChild.href = "javascript:;";
                        firstChild.className = "none";
                    }
                }
            }
        }
        , InitBodyForm: function () {
            var str = "bodyform", v = this.BodyForm;
            if (this.IsWagon > 0) {
                str = "wagon";
                v = this.IsWagon;
            }
            var bodyFormEle = getElementById(str + v);
            if (bodyFormEle)
                bodyFormEle.className = "current";
        }
        , InitDriveType: function () {
            var driveType = getElementById("drivetype" + this.DriveType);
            if (driveType)
                driveType.className = "current";
            if (this.DriveType > 2) {
                driveTypeEle = getElementById("drivetype252");
                if (driveTypeEle) {
                    driveTypeEle.className = "last current";
                    driveTypeEle.firstChild.innerHTML = this.DriveTypeName[this.DriveType];
                    if (driveType && this.DriveType != 252) {
                        var firstChild = driveType.firstElementChild | driveType.firstChild;
                        firstChild.href = "javascript:;";
                        firstChild.className = "none";
                    }
                }
            }
        }
        , InitFuelType: function () {
            var fuelTypeEle = getElementById("fueltype" + this.FuelType);
            if (fuelTypeEle)
                fuelTypeEle.className = "current";
        }
        //根据选中的规则打开或关闭自动的详细选项框
        , OpenOrCloseZidongBox: function () {
            var zidongBox = getElementById("zidongBox");
            if (!zidongBox)
                return;
            //打开详细选项
            if (this.TransmissionType >= 2)
                zidongBox.style.display = "block";
            else
                zidongBox.style.display = "none";
        }
        //打开或关闭轿车的层
        , OpenOrCloseJiaocheBox: function () {
            var jiaocheBox = getElementById("jiaocheBox");
            if (!jiaocheBox)
                return;
            if (this.Level > 6 && this.Level != 63)
                jiaocheBox.style.display = "none";
            else
                jiaocheBox.style.display = "block";
        }
        , InitBrandType: function () {
            var brandEle = getElementById("brandType" + this.Brand.toString());
            if (brandEle)
                brandEle.className = "current";
        }
        , InitCountry: function () {
            var brandEle = getElementById("country" + this.Country.toString());
            if (brandEle)
                brandEle.className = "current";
        }
        //初始化更多条件
        , InitMoreCondition: function () {
            for (i = 0; i < this.MoreCondition.length; i++) {
                var mcCheckEle = getElementById("more_" + this.MoreCondition[i].toString());
                if (mcCheckEle && mcCheckEle.type.toLowerCase() == "checkbox") {
                    mcCheckEle.checked = true;
                }
                else if (mcCheckEle) {
                    if (this.MoreCondition[i].indexOf(".") < 0) {
                        $("#more_" + this.MoreCondition[i].toString()).addClass("current").siblings().removeClass("current");
                    } else {
                        $("#more_" + this.MoreCondition[i].toString().replace(".", "\\.")).addClass("current").siblings().removeClass("current");
                    }
                }
            }
        }
        //设置更多条件
        , SetMoreCondition: function (mcConditionStr) {
            var moreCondition = mcConditionStr.split('_');
            if (moreCondition.indexOf(126) > -1 && moreCondition.indexOf(123) > -1) {
                moreCondition.remove(126);
                moreCondition.remove(123);
                moreCondition.push("126.123");
            }
            if (moreCondition.indexOf(125) > -1 && moreCondition.indexOf(122) > -1) {
                moreCondition.remove(125);
                moreCondition.remove(122);
                moreCondition.push("125.122");
            }
            for (i = 0; i < moreCondition.length; i++) {
                var mcChar = moreCondition[i];
                this.MoreCondition[i] = mcChar;
            }
        }
        , GetMoreconditionDescription: function (mcConditionStr) {
            var counter = 0;
            var maxNum = mcConditionStr.length;
            if (maxNum > 26)
                maxNum = 26;
            var nameArray = new Array();
            for (i = 0; i < maxNum; i++) {
                var mcChar = mcConditionStr.charAt(i);
                if (mcChar == '1')
                    nameArray.push(this.MoreConditionName[i]);
            }
            return nameArray.join(',');
        }
        //获取当前条件的查询字符串
        , GetSearchQueryString: function (isApi) {
            var qsArray = new Array();
            if (this.Price.length > 0)
                qsArray.push("p=" + this.Price);
            if (this.Level != 0)
                qsArray.push("l=" + this.Level.toString());
            if (this.Displacement.length > 0)
                qsArray.push("d=" + this.Displacement);
            if (this.TransmissionType != 0)
                qsArray.push("t=" + this.TransmissionType.toString());
            //var mc = this.MoreCondition.join('_');
            var tempMoreCondition = this.MoreCondition;
            for (var i = 0; i < tempMoreCondition.length; i++) {
                if (tempMoreCondition[i].indexOf('.') > -1) {
                    tempMoreCondition[i] = tempMoreCondition[i].replace('.', '_');
                }
            }
            var mc = tempMoreCondition.join('_');
            if (this.MoreCondition.length > 0) {
                //天窗形式-单天窗、双天窗、全景
                if (this.MoreCondition.indexOf("207") > -1 && isApi) {
                    mc = mc + "_208_209";
                }
                //自动空调-前排自动空调;双温区自动空调
                if (this.MoreCondition.indexOf("244") > -1 && isApi) {
                    mc = mc + "_245";
                }
                //自动空调-前排自动空调;双温区自动空调
                if (this.MoreCondition.indexOf("246") > -1 && isApi) {
                    mc = mc + "_247_248";
                }
                if (this.MoreCondition.indexOf("197") > -1 && isApi) {
                    mc = mc + "_198";
                }
                ////四轮碟刹
                //if (this.MoreCondition.indexOf("141") > -1 && isApi) {
                //    mc = mc + "_144_143_145_146_148_149_150";
                //}
                qsArray.push("more=" + mc);
            }
            if (this.View != 0)
                qsArray.push("v=" + this.View.toString());
            if (this.Sort != 0)
                qsArray.push("s=" + this.Sort.toString());
            if (this.Brand != 0)
                qsArray.push("g=" + this.Brand.toString());
            if (this.Country != 0)
                qsArray.push("c=" + this.Country.toString());
            if (this.BodyForm != 0)
                qsArray.push("b=" + this.BodyForm.toString());
            //if (this.IsWagon && this.IsWagon == 1)
            //    qsArray.push("lv=" + this.IsWagon);
            if (this.DriveType && this.DriveType > 0)
                qsArray.push("dt=" + this.DriveType);
            if (this.FuelType && this.FuelType > 0)
                qsArray.push("f=" + this.FuelType);
            if (this.BodyDoors && this.BodyDoors.length > 0)
                qsArray.push("bd=" + this.BodyDoors);
            if (this.PerfSeatNum && this.PerfSeatNum.length > 0)
                qsArray.push("sn=" + this.PerfSeatNum);
            if (this.Page && this.Page > 1)
                qsArray.push("page=" + this.Page);
            //if (this.FuelConsumption.length > 0)
            //	qsArray.push("fc=" + this.FuelConsumption);
            if (isApi)
                qsArray.push("external=Car");
            return qsArray.join('&');
        }
        //是否有查询条件
        , HasSelectCondition: function () {
            return (this.Price.length > 0 || this.Level != 0
                || this.Displacement.length > 0
                || this.TransmissionType != 0
                || this.MoreCondition.length > 0
                //|| this.FuelConsumption.length > 0
                || this.Brand != 0 || this.Country != 0 || this.BodyForm != 0 || this.DriveType != 0 || this.FuelType != 0 || this.BodyDoors.length > 0 || this.PerfSeatNum.length > 0 || this.IsWagon == 1)
        }
        , GetSearchResult: function () {
            var apiQueryString = this.GetSearchQueryString(true);
            var url = this.apiUrl;
            if (apiQueryString.length > 0) {
                url += "?" + apiQueryString + "&v=20171011";
            }
            $.ajax({
                url: url,
                dataType: "jsonp",
                jsonpCallback: "jsonpCallback",
                cache: true,
                success: function (json) {
                    var result = json;
                    if (typeof ad_carlistdata != "undefined" && ad_carlistdata.length > 0) {
                        result.ResList = GetAdPosition(json);
                    }
                    DrawUlContent(result);
                }
            });
        }
        //获取查询条件说明
        , GetConditionDescription: function (queryStr) {
            if (queryStr == null || typeof queryStr == 'undefined')
                return "";
            var desArray = new Array();
            var conArray = queryStr.split('&');
            for (i = 0; i < conArray.length; i++) {
                var queryKV = conArray[i].split('=');
                if (queryKV.length == 2) {
                    var desc = this.GetConditionDescriptionByKeyValue(queryKV[0], queryKV[1]);
                    desArray.push(desc);
                }
            }
            return desArray.join(',');
        }
        //获取选车条件的描述
        , GetConditionDescriptionByKeyValue: function (keyStr, valueStr) {
            var valueDes = "";
            switch (keyStr) {
                case "p":
                    if (valueStr == '0-5')
                        valueDes = "5万以下";
                    else if (valueStr == '80-9999')
                        valueDes = "80万以上";
                    else
                        valueDes = valueStr + "万";
                    break;
                case "d":
                    if (valueStr == '0-1.3')
                        valueDes = "1.3L以下";
                    else if (valueStr == '5.0-9')
                        valueDes = "5.0L以上";
                    else
                        valueDes = valueStr + "L";
                    break;
                case "l":
                    if (valueStr == "63")
                        valueDes = "轿车";
                    else
                        valueDes = this.LevelName[parseInt(valueStr)];
                    break;
                case "t":
                    valueDes = this.TransmissionTypeName[valueStr];
                    break;
                case "g":
                    valueDes = this.BrandName[parseInt(valueStr)];
                    break;
                case "c":
                    valueDes = this.CountryName[parseInt(valueStr)];
                    break;
                case "m":
                    valueDes = this.GetMoreconditionDescription(valueStr);
                    break;
                case "b":
                    valueDes = this.BodyFormName[valueStr];
                    break;
                case "dt":
                    valueDes = this.DriveTypeName[valueStr];
                    break;
                case "f":
                    valueDes = this.FuelTypeName[valueStr];
                    break;
                //case "lv":
                //    if (valueStr == "1")
                //        valueDes = "旅行版";
                //    break;
                case "bd":
                    if (valueStr != "")
                        valueDes = valueStr + "门";
                    break;
                case "sn":
                    if (valueStr != "" && valueStr != "0")
                        valueDes = valueStr + "座";
                    break;
            }

            return valueDes;
        }
    }
//删除一个更多条件设置
function DelMoreCondition(mcIndex) {
    var mcLiEle = getElementById("mcLi" + mcIndex);
    //alert(mcLiEle);
    var mcCheckEle = getElementById("mcCheck" + mcIndex);
    if (mcLiEle)
        mcLiEle.parentNode.removeChild(mcLiEle);
    if (mcCheckEle)
        mcCheckEle.checked = false;
    mcIndex = parseInt(mcIndex);
    conditionObj.MoreCondition[mcIndex] = '0';
    GotoPage("m");
}
//删除一个车身选项
function delBodyForm(index) {
    var bodyform = getElementById("bodyform_" + index);
    if (bodyform) {
        bodyform.checked = false;
    }
    GotoPage("b");
}

//根据条件页面转向
function GotoPage(conditionStr, anchor) {
    if (conditionStr.length >= 1) {
        var conType = conditionStr.charAt(0);
        var conStr = conditionStr.substr(1);
        //		if (conType == 'p' || conType == 'l' || conType == 'd' || conType == 't' || conType == 'm') {
        //			conditionObj.BodyForm = 0;
        //		}
        switch (conType) {
            case 'p':
                conditionObj.Price = conStr;
                break;
            case 'l':
                conditionObj.Level = parseInt(conStr);
                break;
            case 'd':
                conditionObj.Displacement = conStr;
                break;
            case 't':
                conditionObj.TransmissionType = parseInt(conStr);
                break;
            case 'v':
                conditionObj.View = parseInt(conStr);
                break;
            case 's':
                conditionObj.Sort = parseInt(conStr);
                conditionObj.Page = 1;
                break;
            case 'g':
                conditionObj.Brand = parseInt(conStr);
                break;
            case 'c':
                conditionObj.Country = parseInt(conStr);
                break;
            case 'b':
                var tmp_bodyform = 0;
                for (var key in conditionObj.BodyFormName) {
                    var bodyform = getElementById("bodyform_" + key);
                    if (bodyform && bodyform.checked) {
                        tmp_bodyform = parseInt(tmp_bodyform) + parseInt(key);
                    }
                }
                conditionObj.BodyForm = tmp_bodyform;
                //conditionObj.BodyForm = parseInt(conStr);
                break;
            //case ''
            //case 'fc':
            //	conditionObj.FuelConsumption = conStr;
            //	break;
        }
    }
    var queryString = conditionObj.GetSearchQueryString(false);
    var toUrl = _SearchUrl;
    if (queryString.length > 0) {
        toUrl += "?" + queryString + (anchor == undefined ? "#anchorTitle" : ("#" + anchor));
    }
    else {
        toUrl += anchor == undefined ? "#anchorTitle" : ("#" + anchor);
    }
    //alert(toUrl);
    window.location.href = toUrl;
}
//设置清空
function SetClearCarConditionLink(searchUrl) {
    var lastSel = document.getElementById("selectcarmore");
    if (!lastSel)
        return;
    //设置清空是否显示
    var lastSCHtml = "";
    if (conditionObj.HasSelectCondition()) {
        lastSCHtml = '<a href="/xuanchegongju/">清空条件</a>';
    }

    if (lastSCHtml.length > 0)
        lastSel.innerHTML = lastSCHtml + lastSel.innerHTML;
}
//广告
function GetAdPosition(json) {
    for (var j = 0; j < ad_carlistdata.length; j++) {
        var flag = false;
        //投放位置
        for (var index = 0; index < json.ResList.length; index++) {
            if (json.ResList[index].SerialId == ad_carlistdata[j].SerialId) {
                if (ad_carlistdata[j].Pos > 0) {
                    json.ResList.remove(json.ResList[index]);
                    json.ResList.splice(ad_carlistdata[j].Pos - 1, 0, ad_carlistdata[j]);
                    flag = true;
                }
                break;
            }
        }
        if (!flag) {
            json.ResList.splice(json.ResList.length - 1, 1);
            json.ResList.splice(ad_carlistdata[j].Pos - 1, 0, ad_carlistdata[j]);
        }
    }
    return json.ResList;
}
//更新车型列表
function DrawUlContent(json) {
    if (json.Count > 0) {
        $("#styleTotal").html("共 " + json.Count + "个车型，" + json.CarNumber + "个车款");
    }

    if (json.ResList.length == "0") {
        $("#noResult").show();
        $("#divContent").hide();
        $("#divPageRow").hide();
        $("#carsort").hide();
    } else {
        $("#divContent").show();
        $("#divPageRow").show();
        $("#noResult").hide();
        //初始化车款列表        
        var divContentArray = new Array(),
            serialSpellArray = new Array();
        //divContentArray.push("<ul>");
        var currentLineCount = 0;
        var csIdsArr = [];
        $(json.ResList).each(function (index) {
            csIdsArr.push(json.ResList[index].SerialId);
            serialSpellArray[json.ResList[index].SerialId] = json.ResList[index].AllSpell;
            divContentArray.push("<div class=\"col-xs-3\" data-id=\"" + json.ResList[index].SerialId + "\">");
            divContentArray.push("    <div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
            divContentArray.push("        <div class=\"img\">");
            divContentArray.push("            <a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\"><img src=\"" + json.ResList[index].ImageUrl.replace("_1.", "_3.") + "\" alt=\"" + json.ResList[index].ShowName + "报价_价格\"/></a>");
            divContentArray.push("    </div>");
            divContentArray.push("    <ul class=\"p-list\">");
            divContentArray.push("        <li class=\"name\"><a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\">" + json.ResList[index].ShowName + "</a></li>");
            divContentArray.push("        <li class=\"price\"" + (conditionObj.Sort > 4 ? " style=\"display:none;\"" : "") + "><a href=\"/" + json.ResList[index].AllSpell + "/baojia/\"  target=\"_blank\">" + json.ResList[index].PriceRange + "</a></li>");
            divContentArray.push("        <li class=\"info layer-box x" + (index % 4 + 1) + "\" bit-seachmore><a href=\"javascript:;\" bit-serial=\"" + json.ResList[index].SerialId + "\" bit-car=\"" + json.ResList[index].CarIdList + "\" bit-line=\"" + (currentLineCount - 1) + "\" bit-allspell=\"" + json.ResList[index].AllSpell + "\">" + json.ResList[index].CarNum + "个车款符合条件</a><i></i></li>");
            divContentArray.push("    </ul>");
            divContentArray.push("</div>");
            divContentArray.push("</div>");
        });

        var divContent = divContentArray.join("");
        $("#divContent").html(divContent);
        typeof GetNewCarText == "function" && GetNewCarText(csIdsArr.join(","));
        //call koubei start
        if (conditionObj.Sort > 4) {
            getKouBeiItem(serialSpellArray);
        }
        //call koubei end
        InitPageControl(json.Count);
        callbackGetItem();
    }
}
//车型列表分页
function InitPageControl(pageCount) {
    $("#divPage").pagination(pageCount, {
        items_per_page: 20,
        num_display_entries: 8,
        link_to: "javascript:;",
        current_page: (conditionObj.Page - 1) <= 0 ? 0 : (conditionObj.Page - 1),
        num_edge_entries: 1,
        callback: function (index) { PageClick(index + 1); return false; },
        prev_text: "&lt;",
        next_text: "&gt; ",
        next_class: "next-on",
        prev_class: "preview-on"
    });
}
function PageClick(num) {
    conditionObj.Page = num;
    var obj = getSearchObject();
    obj["page"] = num;
    location.href = 'http://' + window.location.host + window.location.pathname + "?" + getQueryString(obj) + "#anchorcarlist";
}
function getQueryString(data) {
    var tdata = '';
    for (var key in data) {
        tdata += "&" + (key) + "=" + (data[key]);
    }
    tdata = tdata.replace(/^&/g, "");
    return tdata
}

function getSearchObject() {
    var results = {};
    var url = window.location.search.substr(1);
    if (url) {
        var srchArray = url.split("&");
        var tempArray = new Array();

        for (var i = 0; i < srchArray.length; i++) {
            tempArray = srchArray[i].split("=");
            results[tempArray[0]] = tempArray[1];
        }
    }
    return results;
}
function callbackGetItem() {
    var carSynData = {}, IE6 = ! -[1,] && !window.XMLHttpRequest, IE7 = navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1;
    $("li[bit-seachmore] a[bit-serial]").bind("click", function () {
        var self = $(this);
        var serialId = $(this).attr("bit-serial");
        var allSpell = $(this).attr("bit-allspell");
        var carIds = $(this).attr("bit-car");
        var line = parseInt($(this).attr("bit-line"));
        if ($(this).parent().hasClass("active")) {
            if (IE6 || IE7) {
                $(this).removeClass("active").children("div.drop-layer").hide();
            }
            else
                $(self).parent().children("div.drop-layer").slideUp(200, function () { $(self).parent().removeClass("active").children("div.drop-layer").hide() });
            return;
        }
        if ($(self).parent().children("div.drop-layer").length > 0) {
            if (IE6 || IE7)
                $(this).removeClass("active").children("div.drop-layer").hide();
            else {
                $("#divContent li.active").each(function () {
                    $(this).removeClass("active").children("div.drop-layer").hide();
                });
                $(self).parent().addClass("active").find("div.drop-layer").slideDown(200);
            }
        } else {
            $.ajax({
                url: "http://api.car.bitauto.com/CarInfo/GetCarListForSelectCar.ashx?serialId=" + serialId + "&carids=" + carIds, dataType: "jsonp", jsonpCallback: "SimpleSelectCarCallback", cache: true,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert("textStatus: " + textStatus);
                },
                
                success: function (data) {
                    if (data.CarList.length <= 0) { $("#carlist_loading").html("暂无车型数据"); return; }
                    var content = [], title = [];
                    title.push("<div class=\"list-table\" style=\"position: absolute; left: 0; top: 0; z-index: 1;\">");
                    title.push("    <table id=\"compare_wait\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                    title.push("        <colgroup>");
                    title.push("            <col width=\"39%\">");
                    title.push("            <col width=\"11%\">");
                    title.push("            <col width=\"11%\">");
                    title.push("            <col width=\"10%\">");
                    title.push("            <col width=\"11%\">");
                    title.push("            <col width=\"18%\">");
                    title.push("        </colgroup>");
                    title.push("        <tbody>");
                    title.push("            <tr class=\"table-tit\">");
                    title.push("                <th class=\"first-item\">车款</th>");
                    title.push("                <th>关注度</th>");
                    title.push("                <th>变速箱</th>");
                    title.push("                <th class=\"txt-right txt-right-padding\">指导价</th>");
                    title.push("                <th class=\"txt-right\">参考最低价</th>");
                    //title.push("                <th>");
                    //title.push("                    <div class=\"doubt\">");
                    //title.push("                        <div class=\"prompt-layer\" style=\"display:none;\">全国参考最低价</div>");
                    //title.push("                    </div>");
                    //title.push("                </th>");
                    title.push("                <th></th>");
                    title.push("            </tr>");
                    title.push("         </tbody>");
                    title.push("    </table>");
                    title.push("</div>");

                    content.push("<div class=\"drop-layer\">");
                    content.push(title.join(""));
                    content.push("<span class=\"close\"></span>");
                    content.push("<div class=\"list-table scroll-table\">");
                    content.push("    <table id=\"compare_sale\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\">");
                    content.push("        <colgroup>");
                    content.push("            <col width=\"39%\">");
                    content.push("            <col width=\"11%\">");
                    content.push("            <col width=\"11%\">");
                    content.push("            <col width=\"10%\">");
                    content.push("            <col width=\"11%\">");
                    content.push("            <col width=\"18%\">");
                    content.push("        </colgroup>");
                    content.push("        <tbody>");
                    content.push("            <tr class=\"table-tit\" style=\"visibility: hidden;\">");
                    content.push("                <th class=\"first-item\">车款</th>");
                    content.push("                <th>关注度</th>");
                    content.push("                <th>变速箱</th>");
                    content.push("                <th class=\"txt-right txt-right-padding\">指导价</th>");
                    content.push("                <th class=\"txt-right\">参考最低价</th>");
                    content.push("                <th></th>");
                    content.push("            </tr>");
                    $.each(data.CarList, function (i, n) {
                        var yearType = n.CarYear.length > 0 ? n.CarYear + "款" : "未知年款";
                        var strState = n.ProduceState == "停产" ? " <span class=\"color-block3\">停产</span>" : "";
                        var percent = data.MaxPv > 0 ? (n.CarPV / data.MaxPv * 100.0) : 0;
                        //var gearNum = (n.UnderPan_ForwardGearNum != "" && n.UnderPan_ForwardGearNum != "待查" && n.UnderPan_ForwardGearNum != "无级") ? n.UnderPan_ForwardGearNum + "挡" : "";
                        var transmissionType = "";
                        if (n.TransmissionType == "CVT无级变速" || n.TransmissionType == "E-CVT无级变速" || n.TransmissionType == "单速变速箱") {
                            transmissionType = n.TransmissionType;
                        }
                        else if (n.TransmissionType != "" && n.UnderPan_ForwardGearNum != "") {
                            transmissionType = n.UnderPan_ForwardGearNum + "挡 " + n.TransmissionType;
                        }
                        var referPrice = n.ReferPrice.length > 0 ? n.ReferPrice + "万" : "暂无";
                        content.push("<tr id=\"car_filter_id_" + n.CarID + "\">");
                        content.push("    <td class=\"txt-left\">");
                        content.push("        <a href=\"/" + allSpell + "/m" + n.CarID + "/\" target=\"_blank\">" + yearType + " " + n.CarName + " </a>" + strState + "");
                        content.push("    </td>");
                        content.push("    <td>");
                        content.push("    <div class=\"w\">");
                        content.push("        <div class=\"p\" style=\"width: " + percent + "%\"></div>");
                        content.push("    </div>");
                        content.push("    </td>");
                        content.push("    <td>" + transmissionType + "</td>");
                        content.push("    <td class=\"txt-right\"><span>" + referPrice + "</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + n.CarID + "\" target=\"_blank\"></a>");
                        content.push("    </td>");
                        //取最低报价
                        var minPrice = n.CarPriceRange;
                        if (minPrice.length <= 0)
                        { content.push("<td class=\"txt-right\"><span>暂无报价</span></td>"); }
                        else if (minPrice.indexOf("-") != -1) {
                            minPrice = minPrice.substring(0, minPrice.indexOf('-'));
                            content.push("<td class=\"txt-right\"><span><a href=\"/" + allSpell + "/m" + n.CarID + "/baojia/\" target=\"_blank\">" + minPrice + "</a></span></td>");
                        } else { content.push("<td style=\"txt-right\"><span>" + minPrice + "</span></td>"); }

                        content.push("    <td class=\"txt-right\"><a class=\"btn btn-primary btn-xs\" href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + n.CarID + "/\" target=\"_blank\">询底价</a> <a class=\"btn btn-secondary btn-xs\"  target=\"_self\" href=\"javascript:;\" data-use=\"compare\" data-id=\"" + n.CarID + "\">+对比</a></td>");
                        content.push("</tr>");
                    });
                    content.push("             </tbody>");
                    content.push("        </table>");
                    content.push("    </div>");
                    content.push("</div>");

                    if (IE6 || IE7) {
                        $(self).parent().addClass("active").append(content.join(''));
                    } else {
                        $("#divContent li.active").each(function () {
                            $(this).removeClass("active").children("div.drop-layer").hide();
                        });
                        $(self).parent().addClass("active").append(content.join('')).hide().slideDown(200);
                    }
                    initCarListEvent();                  
                    typeof InitCompareEvent == "function" && InitCompareEvent(); 
                    GetCarAreaPriceRangeForSelect(carIds);
                }
            });
        }
        return false;
    });
    //滑动效果
    function initCarListEvent() {
        $("#divContent span.close").click(function () {
            //$(this).parent().hide();
            $(this).parents("li").removeClass("active");
            $(this).parents("li").find("div.drop-layer").slideUp(200);
        });
        $("#divContent div.doubt").hover(
            function () {
                $(this).children(".prompt-layer").show();
            },
            function () {
                $(this).children(".prompt-layer").hide();
            }
        );

        //车型列表效果
        $('#divContent div.drop-layer tr:gt(0)').hover(
            function () {
                $(this).addClass('hover-bg-color');
            },
            function () {
                $(this).removeClass('hover-bg-color');
            }
        );
    }
}

function getKouBeiItem(serialSpellArray) {
    var csIds = [];
    $("li[bit-seachmore] a").each(function () {
        var curSerialId = $(this).attr("bit-serial");
        csIds.push(curSerialId);
    });
    var csParam = csIds.join(',');
    $.ajax({
        url: 'http://api.car.bitauto.com/carinfo/GetSerialInfo.ashx?dept=getcskoubeibaseinfo&csids=' + csParam,
        dataType: "jsonp",
        jsonpCallback: "koubeiCallBack",
        cache: true,
        success: function (json) {
            $(csIds).each(function (index) {
                var curSerialObj = json[csIds[index]];
                if (curSerialObj && curSerialObj != "undefined") {
                    var curSerialPoint = curSerialObj.Rating,
                        intSerialPoint = Math.floor(curSerialPoint);
                    var curPointHtml = [];
                    curPointHtml.push("<a href=\"/" + serialSpellArray[csIds[index]] + "/koubei/\" target=\"_blank\">");
                    curPointHtml.push("<div class=\"star-box1\">");
                    for (var i = 0; i < intSerialPoint; i++) {
                        curPointHtml.push("<i class=\"yes\"></i>");
                    }
                    curPointHtml.push("<i class=\"no\"><i class=\"yes\" style=\"width: " + (curSerialPoint - intSerialPoint) * 100 + "%;\"></i></i>");
                    for (var i = 0; i < 5 - intSerialPoint - 1; i++) {
                        curPointHtml.push("<i class=\"no\"></i>");
                    }
                    curPointHtml.push("<em class=\"data\">" + curSerialPoint + "分</em>");
                    curPointHtml.push("</div>");
                    curPointHtml.push("</a>");
                    $("[bit-serial=" + csIds[index] + "]").parent().siblings().filter(".price").html(curPointHtml.join('')).show();
                }
            })
        }
    });
}

