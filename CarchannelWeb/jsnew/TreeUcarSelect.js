var loadJS = {
    lock: false, ranks: []
	            , callback: function(startTime, callback) {
	                //载入完成
	                this.lock = false;
	                callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
	                this.read(); //解锁，在次载入
	            }
	            , read: function() {
	                //读取
	                if (!this.lock && this.ranks.length) {
	                    var head = document.getElementsByTagName("head")[0];

	                    if (!head) {
	                        ranks.length = 0, ranks = null;
	                        throw new Error('HEAD不存在');
	                    }

	                    var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

	                    this.lock = true;

	                    script.onload = script.onreadystatechange = function() {
	                        if (script && script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') return;

	                        script.onload = script.onreadystatechange = script.onerror = null, script.src = ''
					            , script.parentNode.removeChild(script), script = null; //清理script标记

	                        wc.callback(startTime, ranks.callback), startTime = ranks = null;
	                    };

	                    script.charset = ranks.charset || 'gb2312';
	                    script.src = ranks.src;

	                    head.appendChild(script);
	                }
	            }
	            , push: function(src, charset, callback) {
	                //加入队列
	                this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
	                this.read();
	            }
}
var UcarSelect = {
    userYear: [{ name: "不限", min: "-1", max: "0" }
                    , { name: "一年以内", min: "0", max: "1" }
                    , { name: "1-2年", min: "1", max: "2" }
                    , { name: "2-3年", min: "2", max: "3" }
                    , { name: "3-4年", min: "3", max: "4" }
                    , { name: "4-5年", min: "4", max: "5" }
                    , { name: "5-6年", min: "5", max: "6" }
                    , { name: "6年以上", min: "6", max: "100"}],
    driveMilery: [{ name: "不限", min: "-1", max: "0" }
                             , { name: "1万公里以下", min: "0", max: "1" }
                             , { name: "1-3万公里", min: "1", max: "3" }
                             , { name: "3-5万公里", min: "3", max: "5" }
                             , { name: "5-10万公里", min: "5", max: "10"}],
    carSource: [{ name: "不限", id: "0" }, { name: "个人车源", id: "1" }, { name: "商家车源", id: "2" }, { name: "认证车源", id: "3"}],
    currentYear: { min: "-1", max: "0" },
    currentSource: 0,
    currentdriveMilery: { min: "-1", max: "0" },
    currentAddress: { Province: "0", city: "0" },
    cityUrl: 'http://cache.ucar.cn/ajax/cityjs.ashx?provinceid=@id@',
    controlUserYear: 'carUseYear',
    controlDriverMilery: 'carDriveMileage',
    controlCarSource: 'carSource',
    controlCarProvince: 'carProvince',
    controlCarCity: 'carCity',
    selectCondition: { "p": ["pMin", "pMax"], "l": ["level"], "d": ["dMin", "dMax"],
        "t": ["tValue"], "uy": ["yMin", "yMax"], "dm": ["dmMin", "dmMax"],
        "cs": ["carSource"], "ad": ["provinces", "citys"], "orc": ["orderCondition"],
        "or": ["order"]
    },
    ConditionSelect: { "pMin": "", "pMax": "", "level": "", "dMin": "", "dMax": "", "tValue": "", "yMin": "", "yMax": "",
        "dmMin": "", "dmMax": "", "carSource": "", "provinces": "", "citys": "", "orderCondition": "", "order": ""
    },
    controlOrder: { "CarPrice": "1", "publishTime": "2", "EffectiveYear": "3", "linkDriveMilery": "4" },
    styleOrder: { "orange_up": "1", "orange_down": "2", "gray_up": "3", "gray_down": "4" },
    //得到select选择的值
    getSelectValued: function(control) {
        if (control == null) return 0;
        return control.options[control.selectedIndex].value;
    },
    getSelectText: function(control) {
        if (control == null) return "";
        return control.options[control.selectedIndex].text;
    },
    //初始化使用年限
    initUserYear: function() {
        var controlObj = document.getElementById(this.controlUserYear);
        if (!controlObj || this.userYear == null || this.userYear.length < 1) return;
        controlObj.length = 0;
        var index = 0;
        var isSelect = false;
        for (var i = 0; i < this.userYear.length; i++) {
            var opItem = new Option(this.userYear[i]["name"], i);
            controlObj.options.add(opItem);
            if (!isSelect
                            && this.currentYear != null
                            && this.currentYear["min"] == this.userYear[i]["min"]
                            && this.currentYear["max"] == this.userYear[i]["max"]) {
                index = i;
                isSelect = true;
            }
        }
        controlObj.value = index;
        controlObj.onchange = this.changedUserYear;
    }, //当使用年限查询改变时
    changedUserYear: function() {
        var pro = UcarSelect;
        var paramObj = pro.userYear[pro.getSelectValued(this)];
        if (paramObj == null) return;
        var yMin = paramObj["min"];
        var yMax = paramObj["max"];
        pro.ConditionSelect["yMin"] = yMin;
        pro.ConditionSelect["yMax"] = yMax;
        pro.searchCar();
    }, //初始化行驶里程
    initDriverMilery: function() {
        var controlObj = document.getElementById(this.controlDriverMilery);
        if (!controlObj || this.carSource == null || this.carSource.length < 1) return;
        controlObj.length = 0;
        var index = 0;
        var isSelect = false;
        for (var i = 0; i < this.driveMilery.length; i++) {
            var opItem = new Option(this.driveMilery[i]["name"], i);
            controlObj.options.add(opItem);
            if (!isSelect
                            && this.currentdriveMilery != null
                            && this.currentdriveMilery["min"] == this.driveMilery[i]["min"]
                            && this.currentdriveMilery["max"] == this.driveMilery[i]["max"]) {
                index = i;
                isSelect = true;
            }
        }
        controlObj.value = index;
        controlObj.onchange = this.changedDriverMilery;
    }, //当行驶里程改变时
    changedDriverMilery: function() {
        var pro = UcarSelect;
        var paramObj = pro.driveMilery[pro.getSelectValued(this)];
        if (paramObj == null) return;
        var dmMin = paramObj["min"];
        var dmMax = paramObj["max"];
        pro.ConditionSelect["dmMin"] = dmMin;
        pro.ConditionSelect["dmMax"] = dmMax;
        pro.searchCar();
    }, //初始化车型来源
    initCarSource: function() {
        var controlObj = document.getElementById(this.controlCarSource);
        if (!controlObj || this.driveMilery == null || this.driveMilery.length < 1) return;
        controlObj.length = 0;
        var index = 0;
        var isSelect = false;
        for (var i = 0; i < this.carSource.length; i++) {
            var opItem = new Option(this.carSource[i]["name"], i);
            controlObj.options.add(opItem);
            if (!isSelect
                            && this.currentSource != null
                            && this.currentSource == this.carSource[i]["id"]) {
                index = i;
                isSelect = true;
            }
        }
        controlObj.value = index;
        controlObj.onchange = this.changedSource;
    }, //改变车型来源
    changedSource: function() {
        var pro = UcarSelect;
        var paramObj = pro.carSource[pro.getSelectValued(this)];
        if (paramObj == null) return;
        var cs = paramObj["id"];
        pro.ConditionSelect["carSource"] = cs;
        pro.searchCar();
    },
    //初始化省份
    initProvince: function() {
        var controlObj = document.getElementById(this.controlCarProvince);
        if (!controlObj || typeof ucar_provinces == 'undefined' || ucar_provinces == null) return;
        controlObj.length = 0;

        var index = 0;
        var isSelect = false;
        ucar_provinces = "不限,0," + ucar_provinces;
        var provinces = ucar_provinces.split(',');

        for (var i = 0; i < provinces.length; i++) {
            var opItem = new Option(provinces[i], provinces[i + 1]);
            controlObj.options.add(opItem);
            i += 1;
            if (!isSelect && this.currentAddress != null) {
                index = this.currentAddress["Province"];
                isSelect = true;
            }
        }
        var pro = this;
        controlObj.value = index;
        controlObj.onchange = this.changedProvince;
        var url = this.cityUrl.replace(/@id@/g, index);
        if (this.currentAddress["Province"] > 0) {
            loadJS.push(url, "utf-8", function() { pro.initCity(); });
        }
    },
    //初始化城市
    initCity: function() {
        var controlObj = document.getElementById(this.controlCarCity);
        if (!controlObj || typeof ucar_citys == 'undefined' || ucar_citys == null) return;
        controlObj.length = 0;

        var index = 0;
        var isSelect = false;
        ucar_citys = "不限,0," + ucar_citys;
        var citys = ucar_citys.split(',');

        for (var i = 0; i < citys.length; i++) {
            var opItem = new Option(citys[i], citys[i + 1]);
            controlObj.options.add(opItem);
            i += 1;
            if (!isSelect && this.currentAddress != null) {
                index = this.currentAddress["city"];
                isSelect = true;
            }
        }
        var pro = this;
        controlObj.value = index;
        controlObj.onchange = this.changedCity;
    },
    //改变省份
    changedProvince: function() {
        var pro = UcarSelect;
        var proId = pro.getSelectValued(this);
        pro.ConditionSelect["provinces"] = proId;
        pro.ConditionSelect["citys"] = 0;
        pro.searchCar();
    },
    //改变城市
    changedCity: function() {
        var pro = UcarSelect;
        var proId = pro.getSelectValued(this);
        pro.ConditionSelect["citys"] = proId;
        pro.searchCar();
    },
    //初始化链接
    initOrderLink: function() {
        if (this.controlOrder == null) return;
        for (var id in this.controlOrder) {
            var controlObj = document.getElementById(id);
            if (!controlObj) continue;
            controlObj.onclick = this.orderLink;
        }
    },
    //排序链接
    orderLink: function(e) {
        var pro = UcarSelect;
        var style = this.className;
        var id = this.id;
        var styleSplit = style.split('_');
        if (styleSplit[0] == "gray" && styleSplit[1] == "up")
            pro.ConditionSelect["order"] = 1;
        else if (styleSplit[0] == "orange" && styleSplit[1] == "down")
            pro.ConditionSelect["order"] = 1;
        else
            pro.ConditionSelect["order"] = 2;
        //初始化用户选择的顺序
        pro.ConditionSelect["orderCondition"] = pro.controlOrder[id];
        pro.searchCar();
    },
    searchCar: function() {
        var url = _SearchUrl;
        var parmsUrl = "";
        for (var params in this.selectCondition) {
            if ((params == "p" || params == "d" || params == "uy" || params == "dm") &&
                            this.ConditionSelect[this.selectCondition[params][1]] == 0) continue;

            if (this.selectCondition[params].length < 2) {
                parmsUrl += "&" + params + "=" + this.ConditionSelect[this.selectCondition[params][0]];
                continue;
            }
            var temp = "";
            for (var i = 0; i < this.selectCondition[params].length; i++) {
                temp += "-" + this.ConditionSelect[this.selectCondition[params][i]];
            }
            parmsUrl += "&" + params + "=" + temp.substring(1, temp.length);
        }
        url += "?" + parmsUrl;
        window.location = url;
    },
    //初始化控件
    init: function() {
        this.initUserYear();
        this.initDriverMilery();
        this.initCarSource();
        this.initProvince();
        this.initOrderLink();
    }
}

DomHelper.addEvent(window, "load", function() { UcarSelect.init(); }, false);