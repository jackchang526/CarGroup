<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterfaceDemo360.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.LeveldropdownlistData.InterfaceDemo360" %>
<%@ OutputCache Location="Downstream" Duration="36000" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>接口DEMO</title>
    <style type="text/css">
    .car-search { width:605px; font-size:12px; }
    .car-search form { float:left; }
    .car-search button { text-align:center; line-height:23px; height:28px; margin-right:3px; display:inline; font-size:14px; }
    .car-search select { width:120px; margin-right:3px; display:inline; padding:4px 3px; }


    body,h1,h2,h3,h4,p,dl,dt,dd,ul,li,form,th,td,table,label,article, aside, dialog, footer, header, section, footer, nav, figure,hgroup{margin:0;padding:0;border:0;outline:0;font-size:100%;vertical-align:baseline;background:transparent;}
    body,button,input,select,textarea,li,dt,dd,div,p,span{font:12px/1 Arial;}
    article,aside,dialog,footer,header,section,footer,nav,figure,hgroup{display:block;}
    ul{list-style:none;}
    img{border:none;}
    em,b{font-style:normal;}
    b{font-weight:normal;}
    a{cursor:pointer;}
    button,input,select,textarea{font-size:100%;outline:0;vertical-align:middle;margin:0;}
    button{cursor:pointer;}
    table{border-collapse:collapse;border-spacing:0;}
    .clearfix:after{content:"\0020";display:block;height:0;clear:both;visibility:hidden;}
    .clearfix{clear:both;zoom:1;}
    </style>
    <meta http-equiv="content-type" content="text/html; charset=gbk" />
</head>
<body>
    <div class="car-search">
        <form>
            <script type="text/javascript" charset="utf-8" src="http://image.bitautoimg.com/carchannel/jsnew/leverdropdownlist.js"></script>
            <script language="javascript" charset="utf-8" type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newlevellistgocarandprice.js"></script>
            <script language="javascript" type="text/javascript" charset="utf-8">
            	/*
            	继承下拉列表选择框
            	*/
            	function MaiCheBindSelect(url, selectList, checkData, data, abbreviation, gos, encode, bgcolor, defaultDescribe) {
            		BindSelect.call(this, url, selectList, checkData, data, abbreviation, gos, encode, bgcolor);
            		this._describe_ = defaultDescribe;
            	}

            	MaiCheBindSelect.prototype = new BindSelect();
            	//得到默认值
            	MaiCheBindSelect.prototype.getDefaultValue = function (type) {
            		switch (type) {
            			case "producer":
            				return null;
            			case "master":
            				return this._describe_[type];
            			case "brand":
            				return null;
            			case "serial":
            				return this._describe_[type];
            			case "cartype":
            				return this._describe_[type];
            			default: return null;
            		}
            	}
            	/*
            	绑定数据项
            	*/
            	MaiCheBindSelect.prototype.AddOption = function (type, dataObj) {
            		type = type.toLowerCase();
            		var control = document.getElementById(this.SelectList[type]["selectid"]);
            		if (!control || control.nodeName.toLowerCase() != "select") return;
            		this.BindDefaultValue(type);
            		control.disabled = false;
            		if (control.className != "" && control.className.indexOf("select_current") < 0)
            			control.className += " select_current";
            		var selectObj = this.SelectList[type];
            		var abb = this.Abbreviation[type]; //得到类型的前缀缩写
            		if (dataObj == null) return;
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
            		control.onchange = function () {
            			pro.DropDownChange(type, selectObj["value"]);
            		}
            		this.BindDefaultValue(preObjType);
            	}
            	//绑定默认值
            	MaiCheBindSelect.prototype.BindDefaultValue = function (type) {
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
            		control.setAttribute("disabled", "true");
            		if (control.className != "" && control.className.indexOf("current") > -1)
            			control.className.replace(/current/g, "");
            		var nexttype = this.getRelatObjctType(type, 1);
            		DomHelper.clearDomObject(control);
            		control.appendChild(DomHelper.createOption(this.getDefaultValue(type)));
            		this.BindDefaultValue(nexttype);
            	}
            	/*
            	初始化下拉列表类
            	*/
            	function MaiCheInitDropDownList(selectList, dataUrl, encode, checkData, defaultDescribe) {
            		InitDropDownList.call(this, selectList, dataUrl, encode, checkData);
            		this._describe_ = defaultDescribe;
            	}
            	MaiCheInitDropDownList.prototype = new InitDropDownList();
            	//初始化类实现
            	MaiCheInitDropDownList.prototype.InitBindSelect = function () {
            		var selectObj = new MaiCheBindSelect(this.url, this.slist
	                                                , this.CheckData, {}, this.abbrlist
	                                                , this.gosobject, this.EnCode, backgroup, this._describe_);
            		this.BindSelect = selectObj;
            		this.BindSelect.BindList();
            	}
            	/*
            	继承按钮类
            	*/
            	function MaiCheButtonClick(sList, bObj, initUrl, encode, checkData, defaultDescribe) {
            		ButtonClick.call(this, sList, bObj, initUrl, encode, checkData);
            		this._describe_ = defaultDescribe;
            	}

            	MaiCheButtonClick.prototype = new ButtonClick();

            	MaiCheButtonClick.prototype.Init = function () {
            		//初始化下拉列表
            		this._selectObj_ = new MaiCheInitDropDownList(this._select_, this._url_, this._encode_, this._checkdata_, this._describe_);
            		this._selectObj_.InitBindSelect();
            		//初始化控件onclick绑定事件
            		this.InitButtonOnClickEvent();
            	}
            	MaiCheButtonClick.prototype.BindUrl = function (urlObj, selectObj, type) {
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
            			var rg = new RegExp("\/", "gi");
            			value = value.replace(rg, "");
            			urltemp = urltemp.replace(pattern, value); //得到要绑定的url
            		}
            		return urltemp;

            	}
            	MaiCheButtonClick.prototype.UrlReplace = function (url, paramObject, selectObj, type) {
            		if (url == ""
                || typeof paramObject == 'undefined'
                || paramObject == null) return;

            		for (var param in paramObject) {
            			var bindObject = selectObj.GetValueObject(param);
            			for (var valueparam in paramObject[param]) {
            				//要替换的规则
            				pattern = new RegExp("@" + valueparam + "@", "gi");
            				var value = bindObject[paramObject[param][valueparam]];
            				if (valueparam == "name") value = value;
            				url = url.replace(pattern, value);
            			}
            		}
            		return url;
            	}
            	MaiCheButtonClick.prototype.Click = function (id) {
            		if (id == "") return;
            		var opObj = this._btnobject_[id]; //按钮操作对象
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
            			var selectObject = this._selectObj_.BindSelect; //得到绑定对象的数据
            			if (typeof selectObject == 'undefined' || selectObject == null) continue;
            			var userSelectValue = selectObject.GetValue(type);
            			if (parseInt(userSelectValue) == 0) continue;
            			/*拼接用户需要达到的链接*/
            			gourl = this.BindUrl(opObj[type], selectObject, type);
            			break;
            		}
            		//如果地址不为空，跳转
            		if (gourl != "") window.open(gourl, "", "", "");
            	}
            </script>
            <!---Start:主品牌to子品牌-->
            <select id="master20" style="width:120px"></select><select id="serial20"  style="width:120px"></select>
            <button id="goCarTree" type="button">看车型</button>
            <script language="javascript" type="text/javascript">
            	//点击对象
            	var btnObj = { "goCarTree": { "serial": { "url": "http://car.bitauto.com/@param1@/?WT.mc_id=360ss", "param1": "urlSpell" }
                                        , "master": { "url": "http://car.bitauto.com/tree_chexing/mb_@param1@/?WT.mc_id=360ss", "param1": "id" }
                                        , "default": { "url": "http://car.bitauto.com/?WT.mc_id=360ss" }
            	}
            	}
            	//选择列表绑定对象
            	var selectList = { "master": { "selectid": "master20", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=master&serias=m&key=master_0_4_m" }
                    , "serial": { "selectid": "serial20", "value": "id", "text": "name", "serias": "m", "include": "1", "datatype": "4", "condition": "type=4&pid=@pid@&include=1&rt=serial&serias=m&key=serial_@pid@_4_m" }
            	};
            	//要请求的地址
            	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
            	var defaultSelectValueAndText = { "master": { "value": 0, "text": "请选择品牌" }, "serial": { "value": 0, "text": "请选择车型" }, "cartype": { "value": 0, "text": "请选择车款"} };
            	//初始化对象
            	var btnClickObject = new MaiCheButtonClick(selectList, btnObj, url, "utf-8", null, defaultSelectValueAndText);
            	btnClickObject.Init();
            </script>
        </form>
    </div>
</body>
</html>
