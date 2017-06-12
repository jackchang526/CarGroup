/*
对级联下拉列表新版的升级工作，支持如下的功能：
1.公开onchange方法（方法执行的前后顺序）;
2.可以根据调用类不同，配置不同的事件跳转程序;
3.支持两种跳转方式：window.open和window.location可以包括window.replace
4.支持自定义初始描述;
5.支持初始化选中功能;
6.需要根据HTML控件类型，判断是应该加载“onclick”还是"onchange"
7.支持父级下拉菜单参数;
*/
/*
对象argsObj属性说明
"url": 数据请求的接口地址
"selectlist":要绑定的对象聚合
"encoding":创建脚本块的时候得编码方式
"checkdata":筛选要绑定数据的方式
"beforeChange":绑定数据前要执行的方法
"afterChange":绑定数据后要执行的方法
"paramsabbr":与绑定类型对应，提供绑定类型的数据对象前缀，与服务器端输出变量对应，建议不要修改
"paramsgos":optiongroup对象的样式定义，如果无特殊需要也建议不要修改
"separatedcolor":分隔背景色
*/
function Version2BindSelect(argsObj) {
    BindSelect.call(this, argsObj["url"], argsObj["selectlist"], argsObj["checkdata"], {}, argsObj["paramsabbr"], argsObj["paramsgos"], argsObj["encoding"], argsObj["separatedcolor"]);
    this._operationObj_ = argsObj;
}
Version2BindSelect.prototype = new BindSelect();
Version2BindSelect.prototype.AddOption = function (type, dataObj) {
    var params = this._operationObj_;
    type = type.toLowerCase();
    var control = document.getElementById(this.SelectList[type]["selectid"]);
    if (!control || control.nodeName.toLowerCase() != "select") return;

    //调用用户绑定的方法
    if (typeof params["beforeChange"] == 'function') {
        params["beforeChange"](type, this.SelectList[type]["selectid"], dataObj);
    }

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
    var pro = this;
    //给下一级绑定方法，绑定默认值
    if (preObjType != null) {
        control.onchange = function () {
            pro.DropDownChange(type, selectObj["value"]);
        }
        this.BindDefaultValue(preObjType);
    }
    window.setTimeout(function (t, p, para) {
        return function () {
            var isExitsInitValue = false;
            //如果绑定对象的初始值不为空
            if (typeof pro.SelectList[t]["initValue"] != 'undefined'
                    && pro.SelectList[t]["initValue"] != null) {
                control.value = pro.SelectList[t]["initValue"];
                pro.SelectList[t]["initValue"] = 0;
                isExitsInitValue = true;
            }
            //调用完，调用赋加方法
            if (typeof para["afterChange"] == "function") {
                para["afterChange"](t, pro.SelectList[t]["selectid"], dataObj);
            }

            //如果已经赋加了初始值
            if (isExitsInitValue && p != null) {
                var valueObject = pro.GetValueObject(t);
                if (valueObject == null) return;
                pro.BindList(p, valueObject["id"])
            }
        }
    } (type, preObjType, params), 1);
}
Version2BindSelect.prototype.getDefaultValue = function (type) {
    if (typeof this.SelectList[type]["dvalue"] != 'undefined'
            && this.SelectList[type]["dvalue"] != null) {
        return this.SelectList[type]["dvalue"];
    }
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
/*
Version2ButtonOnClick参数如下:
dropdownObj:下拉列表绑定对象
linkConfig:链接配置
"gotype":跳转方式1为window.open;2为window.location;3为window.location.replace
"event":绑定事件
"paramslist":跳转参数
*/
var Version2ButtonOnClick = function (ddObj, lConfig) {
    if (typeof ddObj == 'undefined'
            || ddObj == null
            || typeof lConfig == 'undefined'
            || lConfig == null
            || typeof lConfig["paramslist"] == 'undefined'
            || lConfig["paramslist"] == null) return;
    this._bindSelectObject_ = ddObj;
    this._LinkConfig_ = lConfig;
}
//初始化函数
Version2ButtonOnClick.prototype.Init = function () {
    this._bindSelectObject_.BindList();
    this.InitClick();
}
//初始化控件事件
Version2ButtonOnClick.prototype.InitClick = function () {
    if (typeof this._LinkConfig_ == 'undefined'
            || this._LinkConfig_ == null
            || typeof this._LinkConfig_["paramslist"] == 'undefined'
            || this._LinkConfig_["paramslist"] == null) return;
    //循环绑定事件
    var pro = this;
    for (var id in this._LinkConfig_["paramslist"]) {
        var control = document.getElementById(id);
        if (!control) continue;
        DomHelper.addEvent(control, this._LinkConfig_["event"], function (btnid) { return function () { pro.Click(btnid); } } (id), false);
    }
}
//得到要绑定的URL
Version2ButtonOnClick.prototype.BindUrl = function (urlObj, selectObj, type) {
    if (urlObj == null || selectObj == null || type == "") return "";
    var urltemp = urlObj["url"]; //得到链接模板
    var bindObject = selectObj.GetValueObject(type); //得到绑定值对应的对象
    if (bindObject == null) return "";
    var pattern;
    for (var id in urlObj) {
        if (id == "url") continue;
        if (id == "definedparam") {
            for (var param in urlObj[id]) {
                pattern = new RegExp("@" + param + "@", "gi");
                urltemp = urltemp.replace(pattern, urlObj[id][param]); //得到要绑定的url
            }
            continue;
        }
        if (id == "parent") {
            urltemp = this.UrlReplace(urltemp, urlObj[id], selectObj, type);
            continue;
        }
        //要替换的规则
        pattern = new RegExp("@" + id + "@", "gi");
        var value = bindObject[urlObj[id]];
        if (typeof value == 'undefined' || value == null) continue;
        var rg = new RegExp("\/", "gi");
        value = value.replace(rg, "");
        urltemp = urltemp.replace(pattern, value); //得到要绑定的url
    }
    return urltemp;

}
//替换URL
Version2ButtonOnClick.prototype.UrlReplace = function (url, paramObject, selectObj, type) {
    if (url == ""
        || typeof paramObject == 'undefined'
        || paramObject == null) return;

    for (var param in paramObject) {
        var bindObject = selectObj.GetValueObject(param);
        for (var valueparam in paramObject[param]) {
            //要替换的规则
            pattern = new RegExp("@" + valueparam + "@", "gi");
            var value = bindObject[paramObject[param][valueparam]];
            if (typeof value == 'undefined' || value == null) continue;
            if (valueparam == "name") value = value;
            url = url.replace(pattern, value);
        }
    }
    return url;
}
//点击事件
Version2ButtonOnClick.prototype.Click = function (objId) {
    if (objId == "") return;
    var opObj = this._LinkConfig_["paramslist"][objId]; //按钮操作对象
    if (opObj == null) return;
    /*
    循环看用户选择了那个品牌
    */
    var gourl = "";
    for (var type in opObj) {
        if (type == "default") {
            gourl = opObj[type]["url"];
            for (var param in opObj[type]["definedparam"]) {
                pattern = new RegExp("@" + param + "@", "gi");
                gourl = gourl.replace(pattern, opObj[type]["definedparam"][param]); //得到要绑定的url
            }
            break;
        }
        if (typeof this._bindSelectObject_ == 'undefined' || this._bindSelectObject_ == null) continue;
        var userSelectValue = this._bindSelectObject_.GetValue(type); 
        if (typeof userSelectValue == 'undefined' || parseInt(userSelectValue) == 0) continue;
        /*拼接用户需要达到的链接*/
        gourl = this.BindUrl(opObj[type], this._bindSelectObject_, type);
        break;
    }
    //如果地址不为空，跳转
    if (gourl == "") return;
    if (typeof this._LinkConfig_["gotype"] == 'undefined'
            || this._LinkConfig_["gotype"] == null
            || this._LinkConfig_["gotype"] == 1) {
        window.open(gourl, "", "", "");
    }
    else if (this._LinkConfig_["gotype"] == 2) {
        window.location = gourl;
    }
    else {
        window.location.replace(gourl);
    }
}