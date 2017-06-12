var loadJS = {
    lock: false, ranks: [],
    callback: function(startTime, callback) {
        //载入完成
        this.lock = false;
        callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
        this.read(); //解锁，在次载入
    },
    read: function() {
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
    },
    push: function(src, charset, callback) {
        //加入队列
        this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
        this.read();
    }
}
var DomHelper = {
    cancelClick: function(e) {
        if (window.event && window.event.cancelBubble
            && window.event.returnValue) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
            return;
        }
        if (e && e.stopPropagation && e.preventDefault) {
            e.stopPropagetion();
            e.preventDefault();
        }
    },
    addEvent: function(elm, evType, fn, useCapture) {
        if (elm.addEventListener) {
            elm.addEventListener(evType, fn, useCapture);
            return true;
        }
        else if (elm.attachEvent) {
            var r = elm.attachEvent('on' + evType, fn);
            return r;
        }
        else {
            elm['on' + evType] = fn;
        }
    },
    clearControlNode: function(obj) {
        var eachGroup = obj.firstChild;
        while (eachGroup != null) {
            obj.removeChild(eachGroup);
            eachGroup = obj.firstChild;
        }
    },
    createOption: function(value, name) {
        var optionItem = document.createElement("OPTION");
        optionItem.setAttribute("value", value);
        optionItem.appendChild(document.createTextNode(name));
        return optionItem;
    },
    createOptionGroup: function(value, name) {
        var optionItem = document.createElement("OPTGROUP");
        optionItem.label = name;
        optionItem.style.fontStyle = "normal";
        optionItem.style.background = "#CCCCCC";
        optionItem.style.textAlign = "center";
        return optionItem;
    }
}
//查看报价,全款购车,贷款购车
function PriceSelectInitObj(mbId, sbId, cId, pBtn, allBtn, dBtn) {
    SelectInitObject.call(this, mbId, sbId, null, pBtn);
    this.m_CarId = cId;
    this.m_AllBtn = allBtn;
    this.m_DBtn = dBtn;
    this.m_PriceCarUrl = "http://price.bitauto.com/car.aspx?newcarId=@ID@&citycode=0&bizmode=-1";
    this.m_CarPriceUrl = "http://price.bitauto.com/brand.aspx?";
    this.m_AllSerialUrl = "http://car.bitauto.com/gouchejisuanqi/?serialid=@ID@";
    this.m_AllCarUrl = "http://car.bitauto.com/gouchejisuanqi/?carid=@ID@";
    this.m_DSerialUrl = "http://car.bitauto.com/qichedaikuanjisuanqi/?serialid=@ID@";
    this.m_DCarUrl = "http://car.bitauto.com/qichedaikuanjisuanqi/?carid=@ID@";
    this.m_requestCarUrl = "http://car.bitauto.com/car/ajaxnew/GetCarByCsID.aspx?type=json&csid=@ID@&name=have&@rad@";
}
PriceSelectInitObj.prototype = new SelectInitObject();
//初始化对象
PriceSelectInitObj.prototype.Init = function() {
    var mcobj = document.getElementById(this.m_MainId);
    if (!mcobj || mcobj.nodeName.toLowerCase() != "select") { alert('请设定主品牌下拉框'); return; }
    var scobj = document.getElementById(this.m_SecondId);
    if (!scobj || scobj.nodeName.toLowerCase() != "select") { alert('请设定子品牌下拉框'); return; }
    var ccobj = document.getElementById(this.m_CarId);
    if (!ccobj || ccobj.nodeName.toLowerCase() != "select") { alert('请设定子品牌下拉框'); return; }
    if (typeof JSonData == "undefined" || JSonData.length < 1) return;
    this.BindMainObject(JSonData.masterBrand, mcobj);
    this.BindSecondObject(null, scobj);
    this.BindThildObject(null, ccobj);
    var pBtn = document.getElementById(this.m_CarPriceId);
    var aBtn = document.getElementById(this.m_AllBtn);
    var dBtn = document.getElementById(this.m_DBtn);
    var pro = this;
    if (pBtn) pBtn.onclick = function() { pro.BindPriceClickObject(); }
    if (aBtn) aBtn.onclick = function() { pro.BindAllClickObject(); }
    if (dBtn) dBtn.onclick = function() { pro.BindDClickObject(); }
}
//绑定主下拉列表
PriceSelectInitObj.prototype.BindMainObject = function(objectData, mbObject) {
    var optionItem;
    var iLength = 0;
    DomHelper.clearControlNode(mbObject);
    optionItem = new Option("请选择品牌", -1);
    mbObject.options.add(optionItem);
    if (objectData == null || objectData.length < 1) {
        return;
    }
    iLength = objectData.length;
    for (var i = 0; i < iLength; i++) {
        if (objectData[i].id != -1) {
            optionItem = new Option(objectData[i].name, objectData[i].id);
            mbObject.options.add(optionItem);
        }
    }
    setTimeout(function() { mbObject.value = initMainSelectValue; }, 1);
    var pro = this;
    mbObject.onchange = function() {
        pro.BindMainObjectChange(pro, objectData);
    }

}
//主列表绑定事件
PriceSelectInitObj.prototype.BindMainObjectChange = function(obj, data) {
    var operationObject = document.getElementById(obj.m_MainId);
    var sObj = document.getElementById(obj.m_SecondId);
    if (!sObj) return;
    var currentValue = operationObject.options[operationObject.selectedIndex].value;
    if (parseInt(currentValue) == -1) {
        obj.BindSecondObject(null, document.getElementById(obj.m_SecondId));
        obj.BindThildObject(null, document.getElementById(obj.m_CarId));
    }

    var iLength = 0;
    iLength = data.length;
    for (var i = 0; i < iLength; i++) {
        if (data[i].id == parseInt(currentValue)) {
            //alert(typeof BrandJsonData);
            if (typeof BrandJsonData != 'undefined') {
                //alert(2);
                obj.BindSecondObjectType(data[i], sObj, 1);
                break;
            }
            //alert(3);
            obj.BindSecondObject(data[i].carSerial, sObj);
            break;
        }
    }
    obj.BindThildObject(null, document.getElementById(obj.m_CarId));
}
//初始化子品牌下拉列表
PriceSelectInitObj.prototype.BindSecondObject = function(objectData, mbObject) {
    //alert(1);
    var optionItem;
    var iLength = 0;
    DomHelper.clearControlNode(mbObject);
    optionItem = new Option("请选择系列", -1);
    mbObject.options.add(optionItem);
    if (objectData == null || objectData.length < 1) {
        return;
    }
    iLength = objectData.length;
    for (var i = 0; i < iLength; i++) {
        if (objectData[i].id != -1) {
            optionItem = new Option(objectData[i].name, objectData[i].id);
            mbObject.options.add(optionItem);
        }
    }
    setTimeout(function() { mbObject.value = initMainSelectValue; }, 1);
    var pro = this;
    mbObject.onchange = function() {
        pro.BindMainObjectChange(pro);
    }
}
//初始化包含品牌的下拉列表
PriceSelectInitObj.prototype.BindSecondObjectType = function(objectData, mbObject, type) {
    var optionItem;
    var iLength = 0;

    DomHelper.clearControlNode(mbObject);

    optionItem = new Option("请选择系列", -1);
    mbObject.options.add(optionItem);
    if (objectData == null) {
        return;
    }

    var tempMasterBrand;
    //得到有排列顺序的主品牌对象
    for (var i = 0; i < BrandJsonData.masterBrand.length; i++) {
        if (BrandJsonData.masterBrand[i].id != objectData.id) {
            continue;
        }
        tempMasterBrand = BrandJsonData.masterBrand[i];
        break;
    }
    //绑定品牌
    for (var j = 0; j < tempMasterBrand.carBrand.length; j++) {
        if (tempMasterBrand.carBrand.length > 1) {
            mbObject.appendChild(DomHelper.createOptionGroup(0, tempMasterBrand.carBrand[j].name));
        }
        //绑定子品牌
        for (var z = 0; z < objectData.carSerial.length; z++) {
            if (objectData.carSerial[z].brandid != tempMasterBrand.carBrand[j].id) continue;
            mbObject.appendChild(DomHelper.createOption(objectData.carSerial[z].id, objectData.carSerial[z].name));
        }
    }
    setTimeout(function() { mbObject.value = initMainSelectValue; }, 1);
    var pro = this;
    mbObject.onchange = function() {
        pro.BindSecondObjectChange(pro);
    }
}
//下拉列表2改变
PriceSelectInitObj.prototype.BindSecondObjectChange = function(obj) {
    var currentObj = document.getElementById(obj.m_SecondId);
    var nodeObj = document.getElementById(obj.m_CarId);
    if (!currentObj || !nodeObj) return;
    var currenValue = parseInt(currentObj.options[currentObj.selectedIndex].value);
    if (currenValue < 1) { obj.BindThildObject(null, nodeObj); return; }

    if (typeof loadJS == 'undefined') return;
    var requestUrl = this.m_requestCarUrl.replace(/@ID@/g, currenValue);
    requestUrl = requestUrl.replace(/@rad@/g, Math.random());
    loadJS.push(requestUrl, "utf-8", function() { obj.operaRequest(nodeObj, obj); });
}
//处理请求
PriceSelectInitObj.prototype.operaRequest = function(nodeObj, obj) {
    if (typeof cartypeList == 'undefined' || cartypeList.length < 1) { obj.BindThildObject(null, nodeObj); return; }
    obj.BindThildObject(cartypeList, nodeObj);

}
//绑定第三个下拉列表
PriceSelectInitObj.prototype.BindThildObject = function(data, obj) {
    var optionItem;
    var iLength = 0;

    DomHelper.clearControlNode(obj);

    optionItem = new Option("请选择车型", -1);
    obj.options.add(optionItem);
    if (data == null) {
        return;
    }

    var yearList = [];
    var tmpYear = 0;
    for (var i = 0; i < data.length; i++) {
        if (tmpYear == data[i]["YearType"]) continue;
        yearList.push(data[i]["YearType"]);
        tmpYear = data[i]["YearType"];
    }

    for (var i = 0; i < yearList.length; i++) {
        if (yearList.length > 1) {
            obj.appendChild(DomHelper.createOptionGroup(0, yearList[i] + "款"));
        }
        for (var j = 0; j < data.length; j++) {
            if (data[j]["YearType"] != yearList[i]) continue;
            obj.appendChild(DomHelper.createOption(data[j]["ID"], data[j]["Name"]));
        }
    }
} //绑定查看报价代码
PriceSelectInitObj.prototype.BindPriceClickObject = function() {
    var sObj = document.getElementById(this.m_SecondId);
    var cObj = document.getElementById(this.m_CarId);
    var currentValue = sObj.options[sObj.selectedIndex].value;
    var cValue = cObj.options[cObj.selectedIndex].value;
    var url = this.m_CarPriceUrl
    if (currentValue < 1) return;
    if (cValue < 1) {
        url += "newbrandid=" + currentValue;
        window.open(url, "", "", "");
        return;
    }
    url = this.m_PriceCarUrl;
    url = url.replace(/@ID@/, cValue);
    window.open(url, "", "", "");
} //绑定全款购车代码
PriceSelectInitObj.prototype.BindAllClickObject = function() {
    var sObj = document.getElementById(this.m_SecondId);
    var cObj = document.getElementById(this.m_CarId);
    var currentValue = sObj.options[sObj.selectedIndex].value;
    var cValue = cObj.options[cObj.selectedIndex].value;
    var url = this.m_AllSerialUrl
    if (currentValue < 1) return;
    if (cValue < 1) {
        url = url.replace(/@ID@/, currentValue);
        window.open(url, "", "", "");
        return;
    }
    url = this.m_AllCarUrl;
    url = url.replace(/@ID@/, cValue);
    window.open(url, "", "", "");
    return;
}  //绑定贷款购车代码
PriceSelectInitObj.prototype.BindDClickObject = function() {
    var sObj = document.getElementById(this.m_SecondId);
    var cObj = document.getElementById(this.m_CarId);
    var currentValue = sObj.options[sObj.selectedIndex].value;
    var cValue = cObj.options[cObj.selectedIndex].value;
    var url = this.m_DSerialUrl
    if (currentValue < 1) return;
    if (cValue < 1) {
        url = url.replace(/@ID@/, currentValue);
        window.open(url, "", "", "");
        return;
    }
    url = this.m_DCarUrl;
    url = url.replace(/@ID@/, cValue);
    window.open(url, "", "", "");
}