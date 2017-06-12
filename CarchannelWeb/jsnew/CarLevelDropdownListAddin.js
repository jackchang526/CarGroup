//----------------------------------------------------下拉列表绑定------------------------------------------------------------

// 下拉菜单----
function CompareBindSelect(url, slist, checkdata, addtionFunction, encoding)
{
	BindSelect.call(this, url, slist, checkdata, {}, ParamsAbbr, ParamsGos, encoding, backgroup);
	this._AddtionFunction_ = addtionFunction;
}
CompareBindSelect.prototype = new BindSelect();
CompareBindSelect.prototype.AddOption = function (type, dataObj) {
    type = type.toLowerCase();
    var control = document.getElementById(this.SelectList[type]["selectid"]);
    if (!control || control.nodeName.toLowerCase() != "select") return;
    this.BindDefaultValue(type);

    var selectObj = this.SelectList[type];
    var abb = this.Abbreviation[type]; //得到类型的前缀缩写
    if (dataObj == null) return;
    control.disabled = false;//by sk
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

    //调用完，调用赋加方法
    if (typeof this._AddtionFunction_ == "function") {
        this._AddtionFunction_(type, this.SelectList[type]["selectid"]);
    }

    if (preObjType == null) return;
    var pro = this;
    control.onchange = function () {
        pro.DropDownChange(type, selectObj["value"]);
    }
    this.BindDefaultValue(preObjType);
}


//默认的后绑定方法
function showMessage(type, id)
{
	if (type == "master" && document.getElementById("hidMasterId"))
	{
		var hidMasterId = document.getElementById("hidMasterId");
		if (hidMasterId)
		{
			var masterId = hidMasterId.value;
			if (parseInt(masterId) > 0)
			{
				document.getElementById(id).value = masterId;
				dropDownBindObj4.BindList("serial", masterId);
			}
		}
	}
	else if (type == "serial")
	{
		var ddlSerial = document.getElementById(id);
		ddlSerial.disabled = "";
		var hidSerialId = document.getElementById("hidSerialId");
		if (hidSerialId)
		{
			var serialId = hidSerialId.value;
			if (parseInt(serialId) > 0)
			{
				ddlSerial.value = serialId;
				dropDownBindObj4.BindList("cartype", serialId);
			}
		}
	}
	else if (type == "cartype")
	{
		var ddlCar = document.getElementById(id);
		ddlCar.disabled = "";
		var hidCarid = document.getElementById("hidCarId");
		if (hidCarid)
		{
			var carId = hidCarid.value;
			if (parseInt(carId) > 0)
				ddlCar.value = carId ;
		}
	}
}
