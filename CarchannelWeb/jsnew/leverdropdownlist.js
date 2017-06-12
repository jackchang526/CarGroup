var ParamsAbbr = { "producer": "p", "master": "m", "brand": "b", "serial": "s", "cartype": "t" }; //与级联类中的类别对应，并提供类别中对象前缀
var ParamsGos = { "style": "normal", "color": "#efefef", "align": "center" }; //Option组的样式对象
var backgroup = "#efefef";
var requestDatalist = {};
/*
动态加载脚本块<script>类
*/
var loadJS = {
    lock: false, ranks: []
    , callback: function (startTime, callback) {
        //载入完成
        this.lock = false;
        callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
        this.read(); //解锁，在次载入
    }
    , read: function () {
        //读取
        if (!this.lock && this.ranks.length) {
            var head = document.getElementsByTagName("head")[0];

            if (!head) {
                ranks.length = 0, ranks = null;
                throw new Error('HEAD不存在');
            }

            var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

            this.lock = true;

            script.onload = script.onreadystatechange = function () {
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
    , push: function (src, charset, callback) {
        //加入队列
        this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
        this.read();
    }
}
/*
Document操作类
*/
var DomHelper = {
    cancelClick: function (e) {
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
    addEvent: function (elm, evType, fn, useCapture) {
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
    }
        , createOption: function (obj) {
            if (obj == null) return null;
            var optionItem = document.createElement("OPTION");
            optionItem.setAttribute("value", obj["value"]);
            optionItem.appendChild(document.createTextNode(obj["text"]));
            if (obj["bgcolor"] != null) optionItem.style.backgroundColor = obj["bgcolor"];
            return optionItem;
        }
        , createGroupOption: function (obj) {
            if (obj == null) return;
            var optionItem = document.createElement("OPTGROUP");
            optionItem.label = obj["text"];
            optionItem.style.fontStyle = obj["style"];
            optionItem.style.background = obj["color"];
            optionItem.style.textAlign = obj["align"];
            return optionItem;
        }
        , getSelectElementValue: function (obj) {
            if (obj == null || !obj) return;
            var value = obj.options[obj.selectedIndex].value;
            return value;
        }
        , clearDomObject: function (obj) {
            if (!obj) return;
            var eachGroup = obj.firstChild;
            while (eachGroup != null) {
                obj.removeChild(eachGroup);
                eachGroup = obj.firstChild;
            }
            obj.disabled = true; //初始重选，下拉列表不可用。by songkai
        }
}
/*
用于控件级联列表显示的类
*/
function BindSelect(url, selectList, checkData, data, abbreviation, gos, encode, bgcolor) {
    this.SelectList = selectList; //{“master”:{“selectid”:””,”value”:””,”text”:””},”brand”:{同上},”serial”:{同上},”cartype”:{同上}}
    this.CheckData = checkData;
    this.Abbreviation = abbreviation; //{ "master": "m", "brand": "b", "serial": "s", "cartype": "t" };
    this.groupOprtionStyle = gos; //{ "style": "", "color": "", "align": "" };
    this.Data = data;
    this.Url = url;
    this.EnCode = encode;
    this.optionBackColor = bgcolor;
}
/*
* 绑定下拉列表的统一方法，如果该类型的数据已经存在
，则直接调用AddOption方法，或者调用GetDataList得到数据
*/
BindSelect.prototype.BindList = function (type, parentDataId) {
    if (type == null || type == "") {
        for (index in this.SelectList) {
            type = index; break;
        }
        this.BindList(type, 0);
        return;
    }
    var typeObj = this.Data[type];
    //如果没有父ID并且此类型对象存在，则直接绑定对象属性
    if (parentDataId == 0
            && typeof typeObj != 'undefined'
            && typeObj != null) {
        this.AddOption(type, typeObj);
        return;
    }
    else if (parentDataId == 0)//如果父ID还是等于0
    {
        this.GetDataList(type, 0); //通地createscript方式得到数据
        return;
    }

    /*以下为parentDataId不为零的情况*/
    var isOptionObject = {}; //定义一个要绑定的对象
    var preType = this.getRelatObjctType(type, -1); //得到他上一级的类型
    var thumType = this.Abbreviation[preType];
    var currentthumType = this.Abbreviation[type];
    //如果数据对象不存在，或者上一级的对象不类型不存在，或者上一级的对象不存在
    if (this.Data == null
            || typeof this.Data[preType] == 'undefined'
            || this.Data[preType] == null) return;
    //得到父ID对照的对象
    var preObject = this.Data[preType][thumType + parentDataId];
    if (typeof preObject == 'undefined' || preObject == null) return;

    //如果父对象中不包含子对象数组
    if (preObject["nl"] == null || preObject["nl"].length < 1) {
        this.GetDataList(type, parentDataId);
        return;
    }
    //如果父对象中包含存在子对象数组
    if (typeof typeObj == 'undefined' || typeObj == null) return;
    //赋值要绑定的对象
    var ilength = preObject["nl"].length;
    for (var i = 0; i < ilength; i++) {
        var typeId = currentthumType + preObject["nl"][i];
        var nodeObject = typeObj[typeId];
        if (typeof nodeObject == 'undefined' || nodeObject == null) continue;
        isOptionObject[typeId] = nodeObject;
    }
    this.AddOption(type, isOptionObject);

}
//通过数据类型得到数据数组
BindSelect.prototype.getDataListByType = function (type, parentDataId) {
    if (this.Data == null || this.Data[type] == null) return;
    var list = [];
    for (index in this.Data[type]) {
        list.push(index);
    }
    return list;
}
/*
根据条件拼接链接，并得到用户要求的数据
*/
BindSelect.prototype.GetDataList = function (type, parentDataId) {
    //得到查询条件
    var conditions = this.SelectList[type]["condition"].replace(/@pid@/gi, parentDataId);
    var url = this.Url + "?" + conditions;
    var objName = type + "_" + parentDataId + "_" + this.SelectList[type]["datatype"] + "_" + this.SelectList[type]["serias"];
    //如果对象包含向上指引
    if (typeof this.SelectList["include"] != 'undefined'
         && this.SelectList["include"] != null) {
        objName = objName + "_" + this.SelectList["include"];
    }
    var pro = this;
    loadJS.push(url, this.EnCode, function () { pro.CallBackOpertion(type, parentDataId, objName); });
}
/*
处理异步请求成功后，程序要进行的过程
*/
BindSelect.prototype.CallBackOpertion = function (type, parentDataId, objName) {
    //得到返回的数据对象
    var data = requestDatalist[objName];
    var thumType = this.Abbreviation[type];
    if (typeof data == 'undefined' || data == null) {
        this.BindDefaultValue(type);
        return;
    }
    //定义结果数组
    var result = [];
    //当数据筛选的方法不为空时
    if (this.CheckData != null) result = this.CheckData(data);
    else {//如果筛选为空，则把数据变成数组
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
    if (parentDataId > 0) {
        var preType = this.getRelatObjctType(type, -1);
        var thumType = this.Abbreviation[preType];
        this.Data[preType][thumType + parentDataId]["nl"] = result;
    }
    //添加下拉列表元素
    this.AddOption(type, bindData);

}
//得到绑定值
BindSelect.prototype.GetValue = function (type) {
    if (this.SelectList == null || type == "") return;
    var obj = this.SelectList[type.toLowerCase()];
    if (obj == null || obj["selectid"] == "") return;
    var controlObj = document.getElementById(obj["selectid"]);
    if (!controlObj) return;
    return DomHelper.getSelectElementValue(controlObj);

}
//得到绑定值对象
BindSelect.prototype.GetValueObject = function (type) {
    var value = this.GetValue(type);
    var valuedesc = this.SelectList[type]["value"];
    var selectObj = this.Data[type];
    var preContent = this.Abbreviation[type.toLowerCase()];
    var pattern = new RegExp(preContent, "g");
    for (index in selectObj) {
        if (selectObj[index][valuedesc] == value) {
            return selectObj[index];
        }
    }
}
/*
绑定数据项
*/
BindSelect.prototype.AddOption = function (type, dataObj) {
    type = type.toLowerCase();
    var control = document.getElementById(this.SelectList[type]["selectid"]);
    if (!control || control.nodeName.toLowerCase() != "select") return;
    this.BindDefaultValue(type);

    var selectObj = this.SelectList[type];
    var abb = this.Abbreviation[type]; //得到类型的前缀缩写
    if (dataObj == null) return;
    control.disabled = false; //有数据，更改下拉列表可用。by songkai
    //要找到要绑定的组
    var tempParentList = {}; //定义一个组对象
    var thorld = 0; //定义组的阈值
    for (var entity in dataObj) {
        var obj = dataObj[entity];
        var groupObj = obj["goid"]; //得到要绑定的对象
        if (typeof groupObj == 'undefined' || groupObj == null)
        { continue; }
        else {
            var existObj = tempParentList[groupObj];
            if (typeof existObj != 'undefined') { continue; }
            else {
                tempParentList[groupObj] = 1
                thorld++;
            }
        }
    }
    tempParentList = {};
    var optionarea = document.createDocumentFragment(); //创建一个document片段
    var thorldValue = 0;
    var thorldList = {};
    //绑定对象
    for (var entity in dataObj) {
        var obj = dataObj[entity]; //得到要绑定的对象
        if (obj == null) continue;
        var value = obj[selectObj["value"]];
        var text = obj[selectObj["text"]];
        //如果该类型为第一级
        if (type == "master" || type == "producer") {
            text = obj["tSpell"] + " " + text;
        }
        if (value == null || value == "" || text == null || text == "") continue;
        var preObj = obj["goid"]; //得到要绑定组
        //判断元素类型
        if ((type == "master" || type == "producer") && thorldList[obj["tSpell"]] == null) {
            thorldList[obj["tSpell"]] = "";
            thorldValue++;
        }
        var optionObj = { "value": value, "text": text };
        //判断当前元素是否，并给当前元素加背景色
        if ((type == "master" || type == "producer")
                 && thorldValue % 2 == 0) optionObj["bgcolor"] = this.optionBackColor;
        //如果父类超过2个，并且没有创建组
        if (thorld >= 2 && tempParentList[preObj] == null) {
            tempParentList[preObj] = 1; //添加已经绑定的组对象
            var groupObject = this.groupOprtionStyle;
            groupObject["text"] = obj["goname"];
            optionarea.appendChild(DomHelper.createGroupOption(groupObject));
        }
        optionarea.appendChild(DomHelper.createOption(optionObj));
    }
    control.appendChild(optionarea);
    //判断该下拉列表是否有下一级，如果有则绑定onchange事件
    var preObjType = this.getRelatObjctType(type, 1);
    if (preObjType == null) return;
    var pro = this;
    control.disabled = false; //有下一级，更改下拉列表可用。 by songkai
    control.onchange = function () {
        pro.DropDownChange(type, selectObj["value"]);
    }
    this.BindDefaultValue(preObjType);
}
//得到上级或者下级的对象
BindSelect.prototype.getRelatObjctType = function (type, step) {
    var threshold = this.getIndexType(type);
    var list = [];
    for (obj in this.SelectList) {
        list.push(obj);
    }
    threshold += step;
    if (threshold < 0 || threshold > list.length) return null;
    return list[threshold];
}
//得到类型的索引
BindSelect.prototype.getIndexType = function (type) {
    var index = 0;
    for (obj in this.SelectList) {
        if (obj == type.toLowerCase()) return index;
        index++;
    }
}
//得到默认值
BindSelect.prototype.getDefaultValue = function (type) {
    switch (type) {
        case "producer":
            return { "value": "0", "text": "请选择厂商" };
        case "master":
            return { "value": "0", "text": "请选择品牌" };
        case "brand":
            return { "value": "0", "text": "请选择品牌" };
        case "serial":
            return { "value": "0", "text": "请选择系列" };
        case "cartype":
            return { "value": "0", "text": "请选择车款" };
        default: return null;
    }
}
//绑定列表改变
BindSelect.prototype.DropDownChange = function (type, valuedesc) {
    var value = this.GetValue(type);
    var nexttype = this.getRelatObjctType(type, 1);
    var selectObj = this.Data[type];
    var preContent = this.Abbreviation[type.toLowerCase()];
    var pattern = new RegExp(preContent, "g");
    for (index in selectObj) {
        if (selectObj[index][valuedesc] == value) {
            this.BindList(nexttype, index.replace(pattern, ""));
            return;
        }
    }
    this.BindList(type, 0);
}
//绑定默认值
BindSelect.prototype.BindDefaultValue = function (type) {
    var obj = this.SelectList[type];
    if (obj == null) { return; }
    else if (obj["selectid"] == "") {
        this.BindDefaultValue(this.getRelatObjctType(type, 1));
        return;
    }
    var control = document.getElementById(obj["selectid"]);
    if (!control) {
        this.BindDefaultValue(nexttype);
        return;
    }
    var nexttype = this.getRelatObjctType(type, 1);
    DomHelper.clearDomObject(control);
    control.appendChild(DomHelper.createOption(this.getDefaultValue(type)));
    this.BindDefaultValue(nexttype);
}

/*
初始化下拉列表类
*/
function InitDropDownList(selectList, dataUrl, encode, checkData) {
    this.slist = selectList;
    this.abbrlist = ParamsAbbr;
    this.gosobject = ParamsGos;
    this.url = dataUrl;
    this.EnCode = encode;
    this.CheckData = checkData;
    this.BindSelect = {};
}
//初始化类实现
InitDropDownList.prototype.InitBindSelect = function () {
    var selectObj = new BindSelect(this.url, this.slist, this.CheckData, {}, this.abbrlist, this.gosobject, this.EnCode, backgroup);
    this.BindSelect = selectObj;
    this.BindSelect.BindList();
}