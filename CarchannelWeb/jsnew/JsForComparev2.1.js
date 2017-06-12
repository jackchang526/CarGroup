// 车型频道对比

var ComparePageObject = {
	CarInfoPath: "http://car.bitauto.com/car/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path
	RootPath: "",    // root path
	ResourceDIR: "", // image dir
	PageDivContentID: "CarCompareContent",  // container id
	PageDivContentObj: null,   // container object
	IsIE: true, // client browser
	IsDelSame: false,  // is delete the same param
	IsDeployAll: false,
	IsOperateTheSame: false,
	XmlSrcLength: 0,
	XmlHttpForCompare: null,
	ArrCarInfo: new Array(),
	ArrPageContent: new Array(),
	ValidCount: 0,
	MaxTDLeft: 5,
	IsNeedSecTH: false,
	IsNeedSelect: true,
	NeedBlockTD: 0,
	MasterToCarDataPath: "http://car.bitauto.com/car/ajaxnew/GetMasterToCar.ashx",
	PageObjectForSelect: new Array(),
	XmlHttpForSelect: null,
	XmlObjectForSelect: null,
	ListForMaster: "",
	ListForSerial: "",
	ListForCar: "",
	SelectType: 1,
	SelectCSName: "",
	TableWidth: 1634,
	IsNeedDrag: true,
	IsUseURL: true,
	CarInfos: "",
	LoopTRColor: 0,
	CarIDsForURL: "",
	CarHotCompare: "http://car.bitauto.com/car/ajaxnew/GetCarHotCompareList.aspx",
	CarHotCompareDivContentID: "CarHotCompareList",  // container id
	CarHotCompareXML: null,
	ComputeSum: 0,
	ComputeCount: 0,
	ComputeAvg: 0,
	BSelect: null
}

// 车型对比信息
function CarCompareInfo(carid, carName, xmlHttpObject, isValid, carInfoXML) {
	this.CarID = carid;
	this.CarName = carName;
	this.XmlHttpObject = xmlHttpObject;
	this.IsValid = isValid;
	this.CarInfoXML = carInfoXML;
}

// 主品牌信息
function MasterBrandInfoSelectListForLevel(masterID, masterName, masterFirstSpell, arrSerialInfo) {
	this.MasterID = masterID;
	this.MasterName = masterName;
	this.MasterFirstSpell = masterFirstSpell;
	this.ArrSerialInfo = arrSerialInfo;
}

// 子品牌信息
function SerialInfoSelectListForLevel(csID, csName, csShowName, csAllSpell, arrCarInfo) {
	this.CsID = csID;
	this.CsName = csName;
	this.CsShowName = csShowName;
	this.CsAllSpell = csAllSpell;
	this.ArrCarInfo = arrCarInfo;
}

// 车型信息
function CarInfoSelectListForLevel(carID, carName, carYear) {
	this.CarID = carID;
	this.CarName = carName;
	this.CarYear = carYear;
}

// 下拉菜单----
function CompareBindSelect(url, slist, checkdata, addtionFunction, encoding) {
	BindSelect.call(this, url, slist, checkdata, {}, ParamsAbbr, ParamsGos, encoding, backgroup);
	this._AddtionFunction_ = addtionFunction;
}
CompareBindSelect.prototype = new BindSelect();
CompareBindSelect.prototype.AddOption = function(type, dataObj) {
	type = type.toLowerCase();
	var control = document.getElementById(this.SelectList[type]["selectid"]);
	if (!control || control.nodeName.toLowerCase() != "select") return;
	this.BindDefaultValue(type);

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

	//调用完，调用赋加方法
	if (typeof this._AddtionFunction_ == "function") {
		this._AddtionFunction_(type, this.SelectList[type]["selectid"]);
	}

	if (preObjType == null) return;
	var pro = this;
	control.onchange = function() {
		pro.DropDownChange(type, selectObj["value"]);
	}
	this.BindDefaultValue(preObjType);
}


function FixWidth(selectObj) {
	var newSelectObj = document.createElement("select");
	newSelectObj = selectObj.cloneNode(true);
	newSelectObj.setAttribute("ID", selectObj.id + "_Temp");
	newSelectObj.selectedIndex = selectObj.selectedIndex;
	newSelectObj.onmouseover = null;
	var e = selectObj;
	var absTop = e.offsetTop;
	var absLeft = e.offsetLeft;
	while (e = e.offsetParent) {
		absTop += e.offsetTop;
		absLeft += e.offsetLeft;
	}
	with (newSelectObj.style) {
		position = "absolute";
		top = absTop + "px";
		left = absLeft + "px";
		width = "";
	}
	var rollback = function() { RollbackWidth(selectObj, newSelectObj); };
	newSelectObj.onmouseout = function() {
		selectObj.selectedIndex = newSelectObj.selectedIndex;
		selectObj.style.visibility = "visible";
		document.body.removeChild(newSelectObj);
	}
	newSelectObj.focus();
	newSelectObj.onfocus = function() { newSelectObj.onmouseout = null; }; //新增加的事件，解决BUG
	newSelectObj.onblur = rollback;
	newSelectObj.onchange = rollback;
	selectObj.style.visibility = "hidden";
	document.body.appendChild(newSelectObj);

	if (newSelectObj) {
		if (newSelectObj.offsetWidth < 120)
		{ newSelectObj.style.width = 120 + 'px'; }
	}
}

function RollbackWidth(selectObj, newSelectObj) {
	if (!newSelectObj)
	{ return; }
	newSelectObj.onchange = null;
	newSelectObj.onblur = null;
	selectObj.selectedIndex = newSelectObj.selectedIndex;
	selectObj.style.visibility = "visible";
	var length = newSelectObj.options.length - 1;
	for (var i = length; i >= 0; i--) {
		if (newSelectObj[i].selected == true) {
			newSelectObj.options[i] = null;
		}
	}
	document.body.removeChild(newSelectObj);
	if (selectObj.id == "serial4") {
		// 子品牌选择
		var i = ComparePageObject.BSelect.GetValue("serial");
		ComparePageObject.BSelect.BindList("cartype", i);
	}
	if (selectObj.id == "cartype4") {
		// 车型
		onchangeCarForSelect();
	}
}

function showMessage(type, id) {
	if (type == "master") {
		// 主品牌绑定完
		var masterIndex = 0;
		if (CookieForSelectListTemp.getCookie("TempSelectMasterID")) {
			masterIndex = CookieForSelectListTemp.getCookie("TempSelectMasterID") * 1;
			if (masterIndex > 0) {
				window.setTimeout(function() {
					if (document.getElementById(id)) {
						document.getElementById(id).selectedIndex = masterIndex;
						var i = ComparePageObject.BSelect.GetValue("master");
						ComparePageObject.BSelect.BindList("serial", i);
					}
				}, 1);
			}
		}
		//		var i = ComparePageObject.BSelect.GetValue("master");
		//		if (i > 0) {
		//			ComparePageObject.BSelect.BindList("serial", i);
		//		}
	}
	else if (type == "serial") {
		// 主品牌被选择 子品牌绑定完
		// 改写主品牌cookie
		if (document.getElementById("master4")) {
			var masterSelectIndex = document.getElementById("master4").selectedIndex;
			if (masterSelectIndex > 0) {
				if (CookieForSelectListTemp.getCookie("TempSelectMasterID") && CookieForSelectListTemp.getCookie("TempSelectMasterID") != masterSelectIndex && CookieForSelectListTemp.getCookie("TempSelectSerialID"))
				{ CookieForSelectListTemp.clearCookie("TempSelectSerialID"); }
				CookieForSelectListTemp.setCookie("TempSelectMasterID", masterSelectIndex);
			}
			else
			{ /*alert("masterSelectIndex<=0");*/ }
		}

		var serialSelectIndex = 0;
		if (CookieForSelectListTemp.getCookie("TempSelectSerialID")) {
			serialSelectIndex = CookieForSelectListTemp.getCookie("TempSelectSerialID") * 1;
			if (serialSelectIndex > 0 && serialSelectIndex != document.getElementById(id).selectedIndex) {
				window.setTimeout(function() {
					if (document.getElementById(id)) {
						document.getElementById(id).selectedIndex = serialSelectIndex;
						var i = ComparePageObject.BSelect.GetValue("serial");
						ComparePageObject.BSelect.BindList("cartype", i);
					}
				}, 1);
			}
		}
		//		var i = bSelect.GetValue("serial");
		//		if (i > 0) {
		//			alert("i > 0 i="+i);
		//			bSelect.BindList("cartype", i);
		//		}
	}
	else if (type == "cartype") {
		// 子品牌被选择
		if (document.getElementById("serial4")) {
			var serialSelectIndex = document.getElementById("serial4").selectedIndex;
			if (serialSelectIndex > 0) {
				CookieForSelectListTemp.setCookie("TempSelectSerialID", serialSelectIndex);
			}
		}
	}
	else
	{ }
}

// -----------

// 显示对比
function initPageForCompare(root) {
	ComparePageObject.ArrCarInfo.length = 0;
	ComparePageObject.ArrPageContent.length = 0;
	ComparePageObject.ValidCount = 0;

	var pageDiv = document.getElementById(ComparePageObject.PageDivContentID);
	if (pageDiv)
	{ ComparePageObject.PageDivContentObj = pageDiv; }
	else
	{ return; }
	ComparePageObject.IsIE = CheckBrowserForCompare();
	ComparePageObject.RootPath = root;

	var compare_msg; //"id9091,aaa|id6996,dfdf"; 
	// URL
	if (ComparePageObject.IsUseURL) {
		compare_msg = ComparePageObject.CarInfos;
	}
	else {
		// 检查是否主动对比
		if (checkCurrentActive()) {
			// compare_msg = CookieForCompare.getCookie("ActiveNewCompare");
		}
		else {
			compare_msg = CookieForCompare.getCookie("PassiveNewCompare");
			// 清除被动对比
			CookieForCompare.clearCookie("isPassiveCompare");
		}
	}
	// alert(compare_msg); 
	if (!compare_msg) {
		// 无车型 
	}
	else {
		var compare_msg_arr = compare_msg.split("|");
		ComparePageObject.XmlSrcLength = compare_msg_arr.length;
	}
	for (var i = 0; i < ComparePageObject.XmlSrcLength; i++) {
		var carCookie = compare_msg_arr[i].split(",");
		var carid = carCookie[0].substring(2, carCookie[0].length);
		var carName = carCookie[1];
		var carinfo = new CarCompareInfo();
		carinfo.CarID = carid;
		carinfo.CarName = carName;
		// ComparePageObject.ArrCarInfo.push(carinfo);
		startRequestForCompare(carinfo);
	}

	//    if(ComparePageObject.ValidCount>0)
	//    {
	// createPageForCompare(false);
	resetTableWidth();
	createPageForCompare(false);
	// 取车型热门对比
	if (ComparePageObject.XmlSrcLength > 0) {
		getCarHotCompareListByCarID(ComparePageObject.ArrCarInfo[0].CarID);
	}
	//    } 
}

// page method --------------------------

// 取需要对比车数据
function startRequestForCompare(carInfo) {
	carInfo.XmlHttpObject = createXMLHttpRequestForCompare();
	if (ComparePageObject.IsIE) {
		carInfo.XmlHttpObject.onreadystatechange = function() { handleStateChangeForGetCarCompareData(carInfo) };
	}
	carInfo.XmlHttpObject.open("GET", ComparePageObject.CarInfoPath + carInfo.CarID, false);
	carInfo.XmlHttpObject.send(null);
	if (!ComparePageObject.IsIE) {
		addCarXmlObjectToArray(carInfo, carInfo.XmlHttpObject.responseXML);
	}
}

function handleStateChangeForGetCarCompareData(carInfo) {
	if (carInfo.XmlHttpObject.readyState == 4 && carInfo.XmlHttpObject.status == 200) {
		addCarXmlObjectToArray(carInfo, carInfo.XmlHttpObject.responseXML);
	}
}

function addCarXmlObjectToArray(carInfo, xmlObj) {
	carInfo.CarInfoXML = xmlObj;
	carInfo.IsValid = true;
	ComparePageObject.ArrCarInfo.push(carInfo);
	ComparePageObject.ValidCount++;
}

function createPageForCompare(isDelSame) {
	ComparePageObject.IsDelSame = isDelSame;
	//    try {
	//        if (ComparePageObject.ValidCount == 0) {
	//            pageSelectTagForList(1);
	//        }
	//    }
	//    catch (err)
	//    { }
	if (ComparePageObject.IsNeedSelect) {
		ComparePageObject.NeedBlockTD = ComparePageObject.ValidCount >= 5 ? 1 : 5 - ComparePageObject.ValidCount;
		if (ComparePageObject.ValidCount == 10)
		{ ComparePageObject.NeedBlockTD = 0; }
	}
	else
	{ ComparePageObject.NeedBlockTD = 0; }
	ComparePageObject.IsNeedSecTH = ComparePageObject.ValidCount >= 5 ? true : false;
	var loopCount = arrField.length;
	ComparePageObject.ArrPageContent.push("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" id=\"compareTable\" style=\"width: " + ComparePageObject.TableWidth + "px;\">");
	for (var i = 0; i < loopCount; i++) {
		switch (arrField[i].sType) {
			case "fieldPara":
				if (ComparePageObject.ValidCount > 0) createPara(arrField[i]);
				break;
			case "fieldMulti":
				if (ComparePageObject.ValidCount > 0) createMulti(arrField[i]);
				break;
			case "bar":
				if (ComparePageObject.ValidCount > 0) createBar(arrField[i]);
				break;
			case "fieldPrice":
				if (ComparePageObject.ValidCount > 0) createPrice(arrField[i]);
				break;
			case "fieldPic":
				createPic();
				break;
		}
	}

	ComparePageObject.ArrPageContent.push("</table>");
	// end
	//    ComparePageObject.ArrPageContent.push("<div class=\"car_compare_listbtn\">");
	//    ComparePageObject.ArrPageContent.push("<ul>");
	//    ComparePageObject.ArrPageContent.push("</ul><div class=\"clear\"></div></div>");

	if (ComparePageObject.PageDivContentObj) {
		ComparePageObject.PageDivContentObj.innerHTML = ComparePageObject.ArrPageContent.join("");
	}
	ComparePageObject.ArrPageContent.length = 0;
	if (ComparePageObject.IsNeedSelect) {
		if (ComparePageObject.XmlObjectForSelect) {
			// not first page load 
			// parseXMLForSelectControl(); 
		}
		else {
			if (ComparePageObject.ValidCount < 10) {
				//------------------------------------------
				var selectList = { "master": { "selectid": "master4", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=master&serias=m&key=master_0_4_m" }
        , "serial": { "selectid": "serial4", "value": "id", "text": "name", "serias": "m", "include": "1", "datatype": "4", "condition": "type=4&pid=@pid@&include=1&rt=serial&serias=m&key=serial_@pid@_4_m" }
        , "cartype": { "selectid": "cartype4", "value": "id", "text": "name", "datatype": "4", "serias": "m", "condition": "type=4&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_4_m" }
				};
				var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
				ComparePageObject.BSelect = new CompareBindSelect(url, selectList, null, showMessage, "utf-8");
				ComparePageObject.BSelect.BindList();
				// -------------------------------------------
				// intiPageSelectControl();
			}
		}
	}
}

// create pic for compare
function createPic() {
	ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" class=\"pic_tr\">");
	ComparePageObject.ArrPageContent.push("<th width=\"160\" class=\"newheadts\"><h2>添加方式一</h2>");
	if (ComparePageObject.ValidCount >= 2)
	{ ComparePageObject.ArrPageContent.push("<p>左右拖动图片可改变车型在列表中的位置</p>"); }
	ComparePageObject.ArrPageContent.push("</th>");

	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// insert Sec TH 
		if (i == ComparePageObject.MaxTDLeft) {
			ComparePageObject.ArrPageContent.push("<th width=\"160\" class=\"newheadts\"><h2>添加方式一</h2>");
			if (ComparePageObject.ValidCount >= 2)
			{ ComparePageObject.ArrPageContent.push("<p>左右拖动图片可改变车型在列表中的位置</p>"); }
			ComparePageObject.ArrPageContent.push("</th>");
		}
		if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
			if (i == 0)
			{ ComparePageObject.ArrPageContent.push("<td width=\"158\" id=\"td_" + i + "\" onMouseOver=\"checkIsChange(this);\" class=\"f\"><div class=\"sx\"></div>"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td width=\"158\" id=\"td_" + i + "\" onMouseOver=\"checkIsChange(this);\">"); }
			try {
				var pic = "";
				if (ComparePageObject.IsIE)
				{ pic = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/CarImg/@PValue").value; }
				else
				{ pic = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/CarImg/@PValue"); }
				if (pic.length < 1)
				{ ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><div class=\"close\" onclick=\"delCarToCompare('" + ComparePageObject.ArrCarInfo[i].CarID + "');\"></div><img id=\"img_" + i + "\" name=\"dragImg\" width=\"120\" height=\"80\" src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif\" alt=\"按住鼠标左键，可拖动到其他列\" /></div>"); }
				else
				{ ComparePageObject.ArrPageContent.push("<div id=\"divImg_" + i + "\" class=\"compare_pic\"><div class=\"close\" onclick=\"delCarToCompare('" + ComparePageObject.ArrCarInfo[i].CarID + "');\"></div><img id=\"img_" + i + "\" name=\"dragImg\" width=\"120\" height=\"80\" src=\"" + pic + "\" alt=\"按住鼠标左键，可拖动到其他列\" /></div>"); }
				// ComparePageObject.ArrPageContent.push("<div><a href=\"javascript:resetCompareCar('" + ComparePageObject.ArrCarInfo[i].CarID + "');\" class=\"g\"></a><a href=\"javascript:delCarToCompare('" + ComparePageObject.ArrCarInfo[i].CarID + "')\" class=\"del\"></a></div>");
				if (i == 0) {
					ComparePageObject.ArrPageContent.push("<div><a class=\"fst\" href=\"javascript:void(0);\">当前首选</a><a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"r\">右移</a></div>");
				}
				else if (i == ComparePageObject.ValidCount - 1) {
					ComparePageObject.ArrPageContent.push("<div><a class=\"fst\" href=\"javascript:resetCompareCar('" + ComparePageObject.ArrCarInfo[i].CarID + "');\">首选</a><a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"l\">左移</a></div>");
				}
				else {
					ComparePageObject.ArrPageContent.push("<div><a class=\"fst\" href=\"javascript:resetCompareCar('" + ComparePageObject.ArrCarInfo[i].CarID + "');\">首选</a><a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"l\">左移</a><a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"r\">右移</a></div>");
					// ComparePageObject.ArrPageContent.push("<div><a class=\"fst\" href=\"javascript:resetCompareCar('" + i + "');\">首选</a><a href=\"javascript:moveLeftForCarCompare('" + i + "');\" class=\"l\">左移</a><a href=\"javascript:moveRightForCarCompare('" + i + "');\" class=\"r\">右移</a></div>");
				}
				// ComparePageObject.ArrPageContent.push("<div><a href=\"javascript:resetCompareCar('" + ComparePageObject.ArrCarInfo[i].CarID + "');\" class=\"g\"></a><a href=\"javascript:delCarToCompare('" + ComparePageObject.ArrCarInfo[i].CarID + "')\" class=\"del\"></a></div>");
			}
			catch (err)
             { }
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}

	//when less 
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
				// insert Sec TH
				ComparePageObject.ArrPageContent.push("<th width=\"160\" class=\"newheadts\"><h2>添加方式二</h2>");
				if (ComparePageObject.ValidCount >= 2)
				{ ComparePageObject.ArrPageContent.push("<p>左右拖动图片可改变车型在列表中的位置</p>"); }
				ComparePageObject.ArrPageContent.push("</th>");
			}
			ComparePageObject.ArrPageContent.push("<td><div class=\"compare_blank\">添加车型</div>");
			if (i == 0) {
				ComparePageObject.ArrPageContent.push("<div id=\"selectForMaster\"><select id=\"master4\" ><option>选择品牌</option></select></div>");
				ComparePageObject.ArrPageContent.push("<div id=\"selectForSerial\"><select id=\"serial4\"  onmouseover=\"FixWidth(this)\"><option>选择车型</option></select></div>");
				ComparePageObject.ArrPageContent.push("<div id=\"selectForCar\"><select id=\"cartype4\" onmouseover=\"FixWidth(this)\" onchange=\"onchangeCarForSelect();\" ><option>选择车款</option></select></div>");
			}
			else {
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择品牌</option></select>");
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择车型</option></select>");
				ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择车款</option></select>");
			}
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}

	ComparePageObject.ArrPageContent.push("");
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create param for compare
function createPara(arrFieldRow) {
	// when Perf_ZongHeYouHao compute avg
	if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
		ComparePageObject.ComputeSum = 0;
		ComparePageObject.ComputeCount = 0;
		ComparePageObject.ComputeAvg = 0;
	}

	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// insert Sec TH
		if (i == ComparePageObject.MaxTDLeft) {
			if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
				arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "##Avg##</th>");
			}
			else {
				arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
			}
		}
		if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
			if (i == 0) {
				arrTemp.push("<td class=\"f\">");
			}
			else {
				arrTemp.push("<td>");
			}

			try {
				var field = "";
				if (ComparePageObject.IsIE) {
					field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue").value;
					if (field.length > 0) {
						if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
							computeWhenNeedAvg(field);
						}
						field += " " + arrFieldRow["unit"];
					}
					if (arrSame.length < 1)
					{ arrSame.push(field); }
					else {
						if (field == arrSame[0])
						{ firstSame = firstSame && true; }
						else
						{ firstSame = firstSame && false; }
					}
				}
				else {
					field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue");
					if (field.length > 0) {
						if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"])) {
							computeWhenNeedAvg(field);
						}
						field += " " + arrFieldRow["unit"];
					}
					if (arrSame.length < 1)
					{ arrSame.push(field); }
					else {
						if (field == arrSame[0])
						{ firstSame = firstSame && true; }
						else
						{ firstSame = firstSame && false; }
					}
				}
				if (field.indexOf("待查") >= 0 || field == "") {
					isAllunknown = true && isAllunknown;
				}
				else {
					isAllunknown = false;
				}
				if (field.indexOf("有") == 0)
				{ field = "●"; }
				if (field.indexOf("选配") == 0)
				{ field = "○"; }
				if (field.indexOf("无") == 0)
				{ field = "-"; }
				arrTemp.push(field);
				// modified by chengl Dec.28.2009 for calculator
				if (arrFieldRow["sFieldName"] == "CarReferPrice" && field != "无" && field != "待查") {
					arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid=" + ComparePageObject.ArrCarInfo[i].CarID + "\"  target=\"_blank\"></a>");
				}
			}
			catch (err) {
				arrTemp.push("-");
				firstSame = firstSame && false;
			}
			arrTemp.push("</td>");
			// ComparePageObject.ArrPageContent.push(field+"</td>"); 
		}
		else {
			arrTemp.push("<td width=\"158\">null");
			arrTemp.push("</td>");
		}
	}
	// alert(firstSame + " " + ComparePageObject.IsDelSame); 
	if (firstSame && ComparePageObject.IsDelSame) {
		return;
	}
	else {
		if (!isAllunknown) {
			// ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\"><th>");
			if (ComparePageObject.LoopTRColor % 2 == 1) {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			else {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"]) && ComparePageObject.ComputeSum > 0 && ComparePageObject.ComputeCount > 1) {
				// 显示平均值
				ComparePageObject.ComputeAvg = (ComparePageObject.ComputeSum / ComparePageObject.ComputeCount).toFixed(1);
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"] + "<br/>均值:" + ComparePageObject.ComputeAvg + " " + arrFieldRow["unit"]);
				ComparePageObject.ArrPageContent.push("</th>");
				ComparePageObject.ArrPageContent.push(arrTemp.join("").replace("##Avg##", "<br/>均值:" + ComparePageObject.ComputeAvg + " " + arrFieldRow["unit"]));
			}
			else {
				// 无有效平均值
				ComparePageObject.ArrPageContent.push("<th>");
				ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
				ComparePageObject.ArrPageContent.push("</th>");
				ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			}

			ComparePageObject.LoopTRColor++;
		}
		else {
			return;
		}
	}

	//when less 
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
				// insert Sec TH
				if (checkParamIsNeedComputeAvg(arrFieldRow["sFieldName"]) && ComparePageObject.ComputeAvg > 0 && ComparePageObject.ComputeCount > 1) {
					ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "<br/>均值:" + ComparePageObject.ComputeAvg + " " + arrFieldRow["unit"] + "</th>");
				}
				else {
					ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
				}
			}
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function computeWhenNeedAvg(pvalue) {
	if (pvalue != "" && !isNaN(pvalue)) {
		var v = 1 * pvalue;
		if (v > 0) {
			ComparePageObject.ComputeSum += v;
			ComparePageObject.ComputeCount++;
		}
	}
}

function checkParamIsNeedComputeAvg(fieldName) {
	var isNeed = false;
	if (fieldName == "Perf_ZongHeYouHao") {
		isNeed = true;
	}
	return isNeed;
}

// create multi param for compare
function createMulti(arrFieldRow) {
	var paramArray = arrFieldRow["sFieldName"].split(',');
	var unitArray = arrFieldRow["unit"].split(',');
	var joinCodeArray = arrFieldRow["joinCode"].split(',');

	var firstSame = true;
	var isAllunknown = true;
	var arrSame = new Array();
	var arrTemp = new Array();
	var isShowLoop = 0;

	// loop every car
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {

		// insert Sec TH
		if (i == ComparePageObject.MaxTDLeft) {
				arrTemp.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
		}
			if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
				if (i == 0) {
					arrTemp.push("<td class=\"f\">");
				}
				else {
					arrTemp.push("<td>");
				}
				try {
					var multiField = "";
					for (var pint = 0; pint < paramArray.length; pint++) {
						// loop every param
						var field = "";
						if (ComparePageObject.IsIE) {
							// IE
							if (ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + paramArray[pint] + "/@PValue")) {
								field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + paramArray[pint] + "/@PValue").value;
								if (field.length > 0) {
									field += unitArray[pint];
									multiField = multiField + joinCodeArray[pint] + field;
								}
							}
						}
						else {
							//FF
							field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + paramArray[pint] + "/@PValue");
							if (field.length > 0) {
								field += unitArray[pint];
								multiField = multiField + joinCodeArray[pint] + field;
							}
						}
					}
					if (arrSame.length < 1) {
						arrSame.push(multiField);
					}
					else {
						if (multiField == arrSame[0])
						{ firstSame = firstSame && true; }
						else
						{ firstSame = firstSame && false; }
					}
					if (multiField.indexOf("待查") >= 0 || multiField == "") {
						isAllunknown = true && isAllunknown;
					}
					else {
						isAllunknown = false;
					}
					if (multiField.indexOf("有") == 0)
					{ multiField = "●"; }
					if (multiField.indexOf("选配") == 0)
					{ multiField = "○"; }
					if (multiField.indexOf("无") == 0)
					{ multiField = "-"; }
					arrTemp.push(multiField);
				}
				catch (err) {
					arrTemp.push("-");
					firstSame = firstSame && false;
				}
				arrTemp.push("</td>");
			}
			else {
				arrTemp.push("<td width=\"158\">null");
				arrTemp.push("</td>");
			}
			isShowLoop++;
	}

	//-----------
	if (firstSame && ComparePageObject.IsDelSame) {
		return;
	}
	else {
		if (!isAllunknown) {
			if (ComparePageObject.LoopTRColor % 2 == 1) {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			else {
				ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\" >");
			}
			ComparePageObject.ArrPageContent.push("<th>");
			ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
			ComparePageObject.ArrPageContent.push("</th>");
			ComparePageObject.ArrPageContent.push(arrTemp.join(""));
			ComparePageObject.LoopTRColor++;
		}
		else {
			return;
		}
	}
	//when less
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
				// insert Sec TH
					ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
			}
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
	//-----------

}

// create bar for compare
function createBar(arrFieldRow) {
	// ComparePageObject.LoopTRColor = 0; // reset color for tr
	ComparePageObject.ArrPageContent.push("<tr class=\"car_compare_name\">");
	ComparePageObject.ArrPageContent.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_0\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// insert Sec TH 
		if (i == ComparePageObject.MaxTDLeft) {
			ComparePageObject.ArrPageContent.push("<th><h2><a id=\"TRForSelect_" + arrFieldRow["sTrPrefix"] + "_" + i + "\" href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
		}
		if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
			if (i == 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"f\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }
			var cs_Name = "";
			var car_year = "";
			var cs_allSpell = "";
			try {
				if (ComparePageObject.IsIE) {
					cs_Name = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_ShowName/@PValue").value;
					car_year = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Car_YearType/@PValue").value;
					cs_allSpell = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_AllSpell/@PValue").value.toLowerCase();
				}
				else {
					cs_Name = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_ShowName/@PValue");
					car_year = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Car_YearType/@PValue");
					cs_allSpell = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/Cs_AllSpell/@PValue").toLowerCase();
				}
				if (car_year.length >= 4) {
					// car_year = " " + car_year.substring(2, car_year.length) + "款";
					car_year = car_year + "款 ";
				}
				else {
					car_year = "";
				}
			}
			catch (err)
            { }
			ComparePageObject.ArrPageContent.push("<a href=\"/" + cs_allSpell + "/m" + ComparePageObject.ArrCarInfo[i].CarID + "/\" target=\"_blank\">" + car_year + cs_Name + " " + ComparePageObject.ArrCarInfo[i].CarName + "</a></td>");
		}
	}

	//when less 
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
				// insert Sec TH
				ComparePageObject.ArrPageContent.push("<th><h2><a href=\"javascript:hiddenTR('" + arrFieldRow["sTrPrefix"] + "');\">" + arrFieldRow["sFieldTitle"] + "</a></h2></th>");
			}
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

// create price for compare
function createPrice(arrFieldRow) {
	if (ComparePageObject.LoopTRColor % 2 == 1) {
		ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(247, 248, 248);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(247, 248, 248)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
	}
	else {
		ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" style=\"background-color: rgb(255, 255, 255);\" onmouseout=\"changeTRColorWhenOnMouse(this,'rgb(255, 255, 255)')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
	}
	ComparePageObject.LoopTRColor++;
	// ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr" + arrFieldRow["sTrPrefix"] + "_" + arrFieldRow["sFieldName"] + "\">");
	ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
	// alert(ComparePageObject.ArrPageContent.join(""));
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		// insert Sec TH 
		if (i == ComparePageObject.MaxTDLeft) {
			ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
		}
		if (ComparePageObject.ArrCarInfo[i].CarInfoXML) {
			if (i == 0)
			{ ComparePageObject.ArrPageContent.push("<td class=\"f\">"); }
			else
			{ ComparePageObject.ArrPageContent.push("<td>"); }

			try {
				var field = "";
				if (ComparePageObject.IsIE) {
					field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue").value;

				}
				else {
					field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML, "/CarParams/" + arrFieldRow["sFieldName"] + "/@PValue");
				}
			}
			catch (err)
            { ComparePageObject.ArrPageContent.push("-"); }
			if (field.length < 1 || field == "无") {
				ComparePageObject.ArrPageContent.push("无");
			}
			else {
				ComparePageObject.ArrPageContent.push("<a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid=" + ComparePageObject.ArrCarInfo[i].CarID + "&citycode=0\">" + field.replace('～', '-') + "</a>");
			}
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}

	//when less 
	if (ComparePageObject.NeedBlockTD > 0) {
		for (var i = 0; i < ComparePageObject.NeedBlockTD; i++) {
			if ((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft) {
				// insert Sec TH
				ComparePageObject.ArrPageContent.push("<th>" + arrFieldRow["sFieldTitle"] + "</th>");
			}
			ComparePageObject.ArrPageContent.push("<td>&nbsp;");
			ComparePageObject.ArrPageContent.push("</td>");
		}
	}
	ComparePageObject.ArrPageContent.push("</tr>");
}

function checkCurrentActive() {
	return true;
}

// 排除相同项
function delTheSameForCompare() {
	if (ComparePageObject.ValidCount > 1) {
		if (!ComparePageObject.IsOperateTheSame) {
			createPageForCompare(true);
			ComparePageObject.IsOperateTheSame = true;
			changeCheckBoxStateByName("checkboxForDelTheSame", true);
		}
		else {
			createPageForCompare(false);
			ComparePageObject.IsOperateTheSame = false;
			changeCheckBoxStateByName("checkboxForDelTheSame", false);
		}
	}
}

// change checkbox state for delete the same param
function changeCheckBoxStateByName(name, state) {
	var checkBoxs = document.getElementsByName(name);
	// alert(checkBoxs.length); 
	if (checkBoxs && checkBoxs.length > 0) {
		for (var i = 0; i < checkBoxs.length; i++) {
			checkBoxs[i].checked = state;
		}
	}
}

// hidd or show TR
function hiddenTR(prefixName) {
	var boolIsHid = true;
	var tableObject = document.getElementById('compareTable');
	for (var i = 0; i < tableObject.rows.length; i++) {
		if (tableObject.rows[i].id.indexOf("tr" + prefixName + "_") == 0) {
			if (tableObject.rows[i].style.display == "none") {
				tableObject.rows[i].style.display = "";
				boolIsHid = false;
			}
			else {
				tableObject.rows[i].style.display = "none";
				boolIsHid = true;
			}
		}
	}
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		var tableObject = document.getElementById('TRForSelect_' + prefixName + '_' + i);
		if (tableObject) {
			if (boolIsHid) {
				tableObject.className = "close";
			}
			else {
				tableObject.className = "";
			}
		}
	}
}

// show or hid compare list
function showHidAll() {
	var tableObject = document.getElementById('compareTable');
	for (var i = 0; i < tableObject.rows.length; i++) {
		if (tableObject.rows[i].id.indexOf("tr") == 0) {
			if (ComparePageObject.IsDeployAll) {
				tableObject.rows[i].style.display = "block";
			}
			else {
				tableObject.rows[i].style.display = "none";
			}
		}
	}
	ComparePageObject.IsDeployAll = !ComparePageObject.IsDeployAll;
	// FF 刷table
	if (!ComparePageObject.IsIE) {
		createPageForCompare(false);
	}
}

// reset compare list
function resetCompareCar(id) {
	if (ComparePageObject.ValidCount < 2) {
		// only 1 car 
		return;
	}
	var num = -1;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.ArrCarInfo[i].CarID == id) {
			num = i;
		}
	}
	// not first object
	if (num > 0) {
		swapArray(ComparePageObject.ArrCarInfo, 0, num);
		// update the same car 
		createPageForCompare(false);
	}
}

function moveLeftForCarCompare(index) {
	if (index > 0 && ComparePageObject.ValidCount > index && ComparePageObject.ArrCarInfo.length > index) {
		swapArray(ComparePageObject.ArrCarInfo, index - 1, index);
		createPageForCompare(ComparePageObject.IsOperateTheSame);
	}
}

function moveRightForCarCompare(index) {
	if (index >= 0 && (ComparePageObject.ValidCount - 1) > index && (ComparePageObject.ArrCarInfo.length - 1) > index) {
		swapArray(ComparePageObject.ArrCarInfo, index, index * 1 + 1);
		createPageForCompare(ComparePageObject.IsOperateTheSame);
	}
}

// swap Array object
function swapArray(obj, index1, index2) {
	var temp = obj[index1];
	obj[index1] = obj[index2];
	obj[index2] = temp;

	if (obj.length > 1) {
		if (index1 == 0 || index2 == 0) {
			// 更新热门对比车型
			getCarHotCompareListByCarID(ComparePageObject.ArrCarInfo[0].CarID);
		}
	}
}

function createXMLHttpRequestForCompare() {
	if (window.ActiveXObject) {
		var xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) {
		var xmlHttp = new XMLHttpRequest();
	}
	return xmlHttp;
}

function CheckBrowserForCompare() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

function selectSingleNodeForFirefox(xmldom, path) {
	var xpe = new XPathEvaluator();
	var nsResolver = xpe.createNSResolver(xmldom.ownerDocument == null ? xmldom.documentElement : xmldom.ownerDocument.documentElement);
	var results = xpe.evaluate(path, xmldom, nsResolver, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
	if (results.singleNodeValue)
	{ return results.singleNodeValue.value; }
	else
	{ return ""; }
}

/*

// 下拉列表
function intiPageSelectControl(type, id) {
if (!type)
{ ComparePageObject.SelectType = 1; }
else
{ ComparePageObject.SelectType = type; }
var url = ComparePageObject.MasterToCarDataPath + "?type=" + ComparePageObject.SelectType + "&id=";
if (!id)
{ url += "0"; }
else
{ url += id; }
ComparePageObject.XmlHttpForSelect = createXMLHttpRequest();
if (ComparePageObject.IsIE) {
ComparePageObject.XmlHttpForSelect.onreadystatechange = handleStateChangeForSelectControl;
}
ComparePageObject.XmlHttpForSelect.open("GET", url, false);
ComparePageObject.XmlHttpForSelect.send(null);
if (!ComparePageObject.IsIE) {
var temp = ComparePageObject.XmlHttpForSelect.responseText;
if (ComparePageObject.SelectType == 1)
{ ComparePageObject.ListForMaster = temp }
if (ComparePageObject.SelectType == 2)
{ ComparePageObject.ListForSerial = temp }
if (ComparePageObject.SelectType == 3)
{ ComparePageObject.ListForCar = temp }
parseXMLForSelectControl();
}
}

function handleStateChangeForSelectControl() {
if (ComparePageObject.XmlHttpForSelect.readyState == 4) {
if (ComparePageObject.XmlHttpForSelect.status == 200) {
var temp = ComparePageObject.XmlHttpForSelect.responseText;
if (ComparePageObject.SelectType == 1)
{ ComparePageObject.ListForMaster = temp }
if (ComparePageObject.SelectType == 2)
{ ComparePageObject.ListForSerial = temp }
if (ComparePageObject.SelectType == 3)
{ ComparePageObject.ListForCar = temp }
parseXMLForSelectControl();
}
}
}

function parseXMLForSelectControl() {
if (ComparePageObject.SelectType && ComparePageObject.SelectType == 1) {
// 主品牌
var selectMaster = document.getElementById("selectForMaster");
if (ComparePageObject.ListForMaster && selectMaster) {
selectMaster.innerHTML = "<select id=\"masterSelectControl\" onchange=\"onchangeMasterForSelect();\">" + ComparePageObject.ListForMaster + "</select>";
}
// 级联选择过
if (CookieForSelectListTemp.getCookie("TempSelectMasterID")) {
var masterSelectControl = document.getElementById("masterSelectControl");
masterSelectControl.value = CookieForSelectListTemp.getCookie("TempSelectMasterID");
onchangeMasterForSelect();
}
}
if (ComparePageObject.SelectType && ComparePageObject.SelectType == 2) {
// 子品牌
var selectSerial = document.getElementById("selectForSerial");
if (ComparePageObject.ListForSerial && selectSerial) {
selectSerial.innerHTML = "<select onmouseover=\"FixWidth(this)\" id=\"serialSelectControl\" onchange=\"onchangeSerialForSelect();\">" + ComparePageObject.ListForSerial + "</select>";
}
// 级联选择过
if (CookieForSelectListTemp.getCookie("TempSelectSerialID")) {
var serialSelectControl = document.getElementById("serialSelectControl");
serialSelectControl.value = CookieForSelectListTemp.getCookie("TempSelectSerialID");
onchangeSerialForSelect();
}
}
if (ComparePageObject.SelectType && ComparePageObject.SelectType == 3) {
// 车型
var selectCar = document.getElementById("selectForCar");
if (ComparePageObject.ListForCar && selectCar) {
selectCar.innerHTML = "<select onmouseover=\"FixWidth(this)\" id=\"carSelectControl\" onchange=\"onchangeCarForSelect();\">" + ComparePageObject.ListForCar + "</select>";
}
}
}


function onchangeMasterForSelect() {
// 清空车型list
var carSelect = document.getElementById('selectForCar');
if (carSelect) {
carSelect.innerHTML = "<select id=\"carSelectControl\" onchange=\"onchangeCarForSelect();\"><option>选择车款</option></select>";
}

var masterSelectValue = document.getElementById('masterSelectControl').options[document.getElementById('masterSelectControl').selectedIndex].value;
if (masterSelectValue && masterSelectValue > 0) {
if (CookieForSelectListTemp.getCookie("TempSelectMasterID") && CookieForSelectListTemp.getCookie("TempSelectMasterID") != masterSelectValue && CookieForSelectListTemp.getCookie("TempSelectSerialID"))
{ CookieForSelectListTemp.clearCookie("TempSelectSerialID"); }
CookieForSelectListTemp.setCookie("TempSelectMasterID", masterSelectValue);
intiPageSelectControl(2, masterSelectValue);
}
}

function onchangeSerialForSelect() {
var selectSelectValue = document.getElementById('serialSelectControl').options[document.getElementById('serialSelectControl').selectedIndex].value;
if (selectSelectValue && selectSelectValue > 0) {
CookieForSelectListTemp.setCookie("TempSelectSerialID", selectSelectValue);
intiPageSelectControl(3, selectSelectValue);
}
}

function onchangeCarForSelect() {
if (document.getElementById('carSelectControl').selectedIndex > 0) {
var carCar = document.getElementById('carSelectControl').options[document.getElementById('carSelectControl').selectedIndex].value;
var carName = document.getElementById('carSelectControl').options[document.getElementById('carSelectControl').selectedIndex].text
addCarToCompareForSelect(carCar, carName, "");
}
}

*/

function onchangeCarForSelect() {
	if (document.getElementById('cartype4').selectedIndex > 0) {
		var carCar = document.getElementById('cartype4').options[document.getElementById('cartype4').selectedIndex].value;
		var carName = document.getElementById('cartype4').options[document.getElementById('cartype4').selectedIndex].text
		addCarToCompareForSelect(carCar, carName, "");
	}
}

//-------------------- add car to compare 
// add compare to cookie
function addCarToCompareForSelect(id, name, csName) {
	var compare = ComparePageObject.CarInfos;
	// var compare = CookieForCompare.getCookie("ActiveNewCompare");
	var com_arr_carID = null;
	var com_arr = null;
	if (compare) {
		com_arr_carID = ComparePageObject.CarIDsForURL.split("|");
		com_arr = compare.split("|");
		if (com_arr.length >= 10) {
			alert("对比车型不能多于10个");
			return;
		}
		if (compare.indexOf("id" + id + ",") >= 0) {
			alert("您选择的车型,已经在对比列表中!");
			return;
		}
	}
	else {
		com_arr_carID = new Array();
		com_arr = new Array();
	}
	com_arr_carID.push(id);
	com_arr.push('id' + id + ',' + name);
	document.location.href = "/chexingduibi/?carIDs=" + com_arr_carID.join(",") + "#CarHotCompareList";
	//    CookieForCompare.clearCookie("ActiveNewCompare"); 
	//    CookieForCompare.setCookie("ActiveNewCompare", com_arr.join("|")); 
	//    initPageForCompare("");
}

function delCarToCompare(caiID) {
	var newCarIDArr = new Array();
	if (ComparePageObject.ValidCount < 1) {
		alert('没有可删的了');
		return;
	}
	var num = -1;
	for (var i = 0; i < ComparePageObject.ValidCount; i++) {
		if (ComparePageObject.ArrCarInfo[i].CarID == caiID) {
			num = i; //alert(' yes :' + id + ' ' + i); 
		}
		else {
			newCarIDArr.push(ComparePageObject.ArrCarInfo[i].CarID);
		}
	}
	if (num >= 0) {
		// changeComparedShow(Compare.carID[num],Compare.carName[num]);
		//        ComparePageObject.ArrCarInfo.splice(num,1);
		//        ComparePageObject.ValidCount--; 
		if (newCarIDArr.length > 0)
		{ document.location.href = "/chexingduibi/?carIDs=" + newCarIDArr.join(",") + "#CarHotCompareList"; }
		else
		{ document.location.href = "/chexingduibi/"; }
		// for reset compare cookie 
		//        resetCookieForCompareCar(caiID); 
		//        resetTableWidth();
		//        createPageForCompare(false); 
	}
}

function resetCookieForCompareCar(id) {
	var arrForNewCar = new Array();
	for (var i = 0; i < ComparePageObject.ArrCarInfo.length; i++) {
		arrForNewCar.push("id" + ComparePageObject.ArrCarInfo[i].CarID + "," + ComparePageObject.ArrCarInfo[i].CarName);
	}
	CookieForCompare.clearCookie("ActiveNewCompare");
	CookieForCompare.setCookie("ActiveNewCompare", arrForNewCar.join("|"));

	// reset csName cookie
	var arrForNewCs = new Array();
	var carSerial = CookieForCompare.getCookie("ActiveCarSerialCompare");
	var carSerial_arr = null;
	if (carSerial) {
		var idForCs;
		var nameForCs;
		carSerial_arr = carSerial.split("|");
		for (var i = 0; i < carSerial_arr.length; i++) {
			idForCs = carSerial_arr[i].split(",")[0];
			nameForCs = carSerial_arr[i].split(",")[1];
			if (idForCs != "id" + id)
			{ arrForNewCs.push("id" + idForCs + "," + nameForCs); }
		}
		CookieForCompare.clearCookie("ActiveCarSerialCompare");
		CookieForCompare.setCookie("ActiveCarSerialCompare", arrForNewCs.join("|"));
	}
}

function resetTableWidth() {
	if (ComparePageObject.ValidCount > 0) {
		if (ComparePageObject.ValidCount >= 5) {
			if (ComparePageObject.ValidCount >= 10)
			{ ComparePageObject.TableWidth = 1884; }
			else
			{ ComparePageObject.TableWidth = (160 * 2) + (158 * (ComparePageObject.ValidCount + 1)) + ((ComparePageObject.ValidCount + 1) * 2); }
		}
		else {
			ComparePageObject.TableWidth = 160 + (158 * 5) + 2;
		}
	}
	else {
		ComparePageObject.TableWidth = 146 + (134 * 6) + 2;
	}
}

function checkIsChange(tdObj) {
	targetNum = tdObj.id.replace('td_', '');
	if (targetNum == currentTD || targetNum == "" || currentTD == "") {
	}
	else {
		swapArray(ComparePageObject.ArrCarInfo, currentTD, targetNum);
		targetNum = "";
		currentTD = "";
		createPageForCompare(false);
	}
}

function changeTRColorWhenOnMouse(obj, color) {
	if (obj)
	{ obj.style.background = color; }
}


// 车型热门对比
function getCarHotCompareListByCarID(carid) {
	ComparePageObject.CarHotCompareXML = createXMLHttpRequestForCompare();
	if (ComparePageObject.IsIE) {
		ComparePageObject.CarHotCompareXML.onreadystatechange = function() { handleStateChangeForGetCarHotCompare() };
	}
	ComparePageObject.CarHotCompareXML.open("GET", ComparePageObject.CarHotCompare + "?carid=" + carid, false);
	ComparePageObject.CarHotCompareXML.send(null);
	if (!ComparePageObject.IsIE) {
		// alert(ComparePageObject.CarHotCompareXML.responseText);
		if (ComparePageObject.CarHotCompareXML.responseText && ComparePageObject.CarHotCompareXML.responseText != "") {
			if (document.getElementById(ComparePageObject.CarHotCompareDivContentID)) {
				document.getElementById(ComparePageObject.CarHotCompareDivContentID).innerHTML = ComparePageObject.CarHotCompareXML.responseText;
			}
		}
	}
}

function handleStateChangeForGetCarHotCompare() {
	if (ComparePageObject.CarHotCompareXML.readyState == 4 && ComparePageObject.CarHotCompareXML.status == 200) {
		// alert(ComparePageObject.CarHotCompareXML.responseText);
		if (ComparePageObject.CarHotCompareXML.responseText && ComparePageObject.CarHotCompareXML.responseText != "") {
			if (document.getElementById(ComparePageObject.CarHotCompareDivContentID)) {
				document.getElementById(ComparePageObject.CarHotCompareDivContentID).innerHTML = ComparePageObject.CarHotCompareXML.responseText;
			}
		}
	}
}

//--------------------  Drag  --------------------
var ie = document.all;
var nn6 = document.getElementById && !document.all;

var isdrag = false;
var x, y;
var dobj;
var currentTD = "";
var targetNum = "";

function movemouse(e) {
	if (isdrag) {
		dobj.style.left = nn6 ? tx + e.clientX - x + "px" : tx + event.clientX - x;
		dobj.style.top = nn6 ? ty + e.clientY - y + "px" : ty + event.clientY - y;
		return false;
	}
}

function selectmouse(e) {
	try {
		var fobj = nn6 ? e.target : event.srcElement;
		var topelement = nn6 ? "HTML" : "BODY";
		if (fobj.tagName);
		while (fobj.tagName != topelement && fobj.name != "dragImg") {
			if (fobj.tagName);
			fobj = nn6 ? fobj.parentNode : fobj.parentElement;
		}
		if (fobj.name == "dragImg") {
			currentTD = fobj.id.replace('img_', '');

			isdrag = true;
			fobj.style.position = 'relative';
			fobj.style.zIndex = '90';
			dobj = fobj;
			tx = parseInt(dobj.style.left + 0);
			ty = parseInt(dobj.style.top + 0);
			x = nn6 ? e.clientX : event.clientX;
			y = nn6 ? e.clientY : event.clientY;
			document.onmousemove = movemouse;
			return false;
		}
	}
	catch (err)
    { return false; }
}
function resetImg(e) {
	try {
		var fobj = nn6 ? e.target : event.srcElement;
		isdrag = false;
		if (fobj.name == "dragImg") {
			fobj.style.position = '';
			fobj.style.left = 0;
			fobj.style.top = 0;
			setTimeout("currentTD = '';", 200);
		}
		else {
			if (currentTD != '') {
				var fobj2 = document.getElementById("img_" + currentTD);
				fobj2.style.position = '';
				fobj2.style.left = 0;
				fobj2.style.top = 0;
				currentTD = '';
			}
		}
		return false;
	}
	catch (err) {
		return false;
	}
}
window.onload = function() {
	if (ComparePageObject.IsNeedDrag) {
		document.onmousedown = selectmouse;
		document.onmouseup = resetImg;
	}
}

//--------------------  Cookie  --------------------
var CookieForSelectListTemp = {
	setCookie: function(name, value) {
		document.cookie = name + "=" + escape(value) + "; domain=car.bitauto.com";
	},

	getCookie: function(name) {
		var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

		if (arr != null) {
			return unescape(arr[2]);
		}
		return null;
	},

	clearCookie: function(name) {
		if (CookieForCompare.getCookie(name)) {
			document.cookie = name + "=;domain=car.bitauto.com";
		}
	}
};

// page method --------------------------
var arrField = [
   { sType: "fieldPic", sFieldName: "", sFieldTitle: "图片", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "基本信息", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "CarReferPrice", sFieldTitle: "厂家指导价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPrice", sFieldName: "AveragePrice", sFieldTitle: "商家报价", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Car_RepairPolicy", sFieldTitle: "保修政策", sTrPrefix: "1", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ExhaustForFloat", sFieldTitle: "排量（升）", sTrPrefix: "1", unit: "L", joinCode: "" },
//{ sType: "fieldPara", sFieldName: "UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "1", unit: "", joinCode: "" },
   {sType: "fieldMulti", sFieldName: "UnderPan_ForwardGearNum,UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "1", unit: "档,", joinCode: ", " },
   { sType: "bar", sFieldName: "", sFieldTitle: "基本性能", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ZongHeYouHao", sFieldTitle: "综合工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiQuYouHao", sFieldTitle: "市区工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ShiJiaoYouHao", sFieldTitle: "市郊工况油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FuelCostPer100", sFieldTitle: "百公里等速油耗", sTrPrefix: "2", unit: "L", joinCode: "" },
//   { sType: "fieldPara", sFieldName: "Perf_FuelCostPerRate100", sFieldTitle: "百公里等速油耗速度", sTrPrefix: "2", unit: "km/h", joinCode: "" },
   {sType: "fieldPara", sFieldName: "Perf_MaxClimbDegree", sFieldTitle: "最大爬坡度", sTrPrefix: "2", unit: "%", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxClimbDegreeValue", sFieldTitle: "最大爬坡度(值)", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_MinWheelRadius", sFieldTitle: "最小转弯半径", sTrPrefix: "2", unit: "m", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_MaxPaddleDepth", sFieldTitle: "最大涉水深度", sTrPrefix: "2", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_DriveType", sFieldTitle: "驱动方式", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_SeatNum", sFieldTitle: "乘员人数（含司机）", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_WindBlock", sFieldTitle: "风阻系数", sTrPrefix: "2", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_SlackSpeedNoise", sFieldTitle: "车内怠速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_AveSpeedNoise", sFieldTitle: "车内等速噪音", sTrPrefix: "2", unit: "dB(A)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_FEaxleWeight", sFieldTitle: "前轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_BEaxleWeight", sFieldTitle: "后轴荷", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_Weight", sFieldTitle: "整备质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_TotalWeight", sFieldTitle: "允许总质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_Tonnage", sFieldTitle: "最大承载质量", sTrPrefix: "2", unit: "kg", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "车身结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Doors", sFieldTitle: "车门数", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Type", sFieldTitle: "车身型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Islong", sFieldTitle: "车身加长型", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_TipType", sFieldTitle: "车顶型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Louver", sFieldTitle: "天窗型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_LouverOCType", sFieldTitle: "天窗开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_sailType", sFieldTitle: "车篷型式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_CalashOCType", sFieldTitle: "车篷开合方式", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_CalashOCTime", sFieldTitle: "车篷开合时间", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Body_Struc", sFieldTitle: "车体结构", sTrPrefix: "3", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "外部尺寸", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Length", sFieldTitle: "长", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Width", sFieldTitle: "宽", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_Height", sFieldTitle: "高", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_WheelBase", sFieldTitle: "轴距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_FrontTread", sFieldTitle: "前轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_BackTread", sFieldTitle: "后轮距", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_FhangLength", sFieldTitle: "前悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_BhangLength", sFieldTitle: "后悬长度", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_NearCorner", sFieldTitle: "接近角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_AwayCorner", sFieldTitle: "离去角", sTrPrefix: "4", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutSet_MinGapFromEarth", sFieldTitle: "最小离地间隙", sTrPrefix: "4", unit: "mm", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "内部尺寸", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FheadSpace", sFieldTitle: "前排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BheadSpace", sFieldTitle: "后排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FjianSpace", sFieldTitle: "前排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BjianSpace", sFieldTitle: "后排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_FFootSpace", sFieldTitle: "前排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BFootSpace", sFieldTitle: "后排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdHeight", sFieldTitle: "第三排头部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdJSpace", sFieldTitle: "第三排肩部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_ThirdFootSpace", sFieldTitle: "第三排腿部空间", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpwidth", sFieldTitle: "行李箱最大开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageMWidth", sFieldTitle: "行李箱最小开口宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpheight", sFieldTitle: "行李箱开口离地高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageHeight", sFieldTitle: "行李箱内部高度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageDepth", sFieldTitle: "行李箱内部深度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BaggageWidth", sFieldTitle: "行李箱内部最大宽度", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_TrunkCapacity", sFieldTitle: "行李箱容积", sTrPrefix: "5", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_TrunkCapacityE", sFieldTitle: "行李箱最大拓展容积", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Inset_BackUpOpenType", sFieldTitle: "行李箱打开方式", sTrPrefix: "5", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "燃油&发动机", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelCapacity", sFieldTitle: "燃油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_SboxSpace", sFieldTitle: "副油箱容积", sTrPrefix: "6", unit: "L", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelType", sFieldTitle: "燃料类型", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_FuelTab", sFieldTitle: "燃油标号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Oil_SupplyType", sFieldTitle: "燃料供给型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Type", sFieldTitle: "型号", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Company", sFieldTitle: "生产厂家", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Exhaust", sFieldTitle: "排量", sTrPrefix: "6", unit: "mL", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxSpeed", sFieldTitle: "最高转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxPower", sFieldTitle: "最大功率-功率值", sTrPrefix: "6", unit: "kW", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_PowerSpeed", sFieldTitle: "最大功率-转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_MaxNJ", sFieldTitle: "最大扭矩-扭矩值", sTrPrefix: "6", unit: "Nm", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_NJSpeed", sFieldTitle: "最大扭矩—转速", sTrPrefix: "6", unit: "r/min(rpm)", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderRank", sFieldTitle: "气缸排列型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Location", sFieldTitle: "发动机位置", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_InhaleType", sFieldTitle: "进气型式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_AddPressType", sFieldTitle: "增压方式", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Camshaft", sFieldTitle: "凸轮轴", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_SpTech", sFieldTitle: "特有技术", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderNum", sFieldTitle: "汽缸数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValvePerCylinder", sFieldTitle: "每缸气门数", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderDM", sFieldTitle: "缸径", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Route", sFieldTitle: "行程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValveTM", sFieldTitle: "可变气门正时", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_ValveVTM", sFieldTitle: "可变气门升程", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CompressRat", sFieldTitle: "压缩比", sTrPrefix: "6", unit: ":1", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderMaterial", sFieldTitle: "缸体材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_CylinderTMaterial", sFieldTitle: "缸盖材料", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_EnvirStandard", sFieldTitle: "环保标准", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_EGR", sFieldTitle: "废气再循环（EGR）", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Catalyze", sFieldTitle: "三元催化器", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_AIFluid_VL", sFieldTitle: "防冻液容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Engine_Eoil_VL", sFieldTitle: "机油容积", sTrPrefix: "6", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "底盘操控", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RedirectorType", sFieldTitle: "转向机形式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_DriveAsistTurn", sFieldTitle: "转向助力", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_AsistTurnTune", sFieldTitle: "随速助力转向调节(EPS)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_circleNum", sFieldTitle: "方向盘回转总圈数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransmissionType", sFieldTitle: "变速器型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ForwardGearNum", sFieldTitle: "前进档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_GearChangePosition", sFieldTitle: "变速箱变速杆位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseGearNum", sFieldTitle: "分动箱档数", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseType", sFieldTitle: "分动箱型式", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TransferCaseControl", sFieldTitle: "分动箱操纵", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ReductionRatio", sFieldTitle: "主减速比", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_CentralDiffLock", sFieldTitle: "中央差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontAxleDiffLock", sFieldTitle: "前轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearAxleDiffLock", sFieldTitle: "后轴差速器锁", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontBrakeType", sFieldTitle: "前制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearBrakeType", sFieldTitle: "后制动类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ParkingBrake", sFieldTitle: "驻车制动器", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ShockAbsorberType", sFieldTitle: "减震器类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontSuspensionType", sFieldTitle: "前悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearSuspensionType", sFieldTitle: "后悬挂类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SuspensionHeightControl", sFieldTitle: "悬挂高度调节", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RimMaterial", sFieldTitle: "轮毂材料", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontRimStandard", sFieldTitle: "前轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearRimStandard", sFieldTitle: "后轮毂规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_FrontTyreStandard", sFieldTitle: "前轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RearTyreStandard", sFieldTitle: "后轮胎规格", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SpareWheelStandard", sFieldTitle: "备胎类型", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_SpareWheelPosition", sFieldTitle: "备胎位置", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_ZeroPressureDrive", sFieldTitle: "零压续行(零胎压继续行驶)", sTrPrefix: "7", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "外部配置", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BodyColor", sFieldTitle: "车身颜色", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_PaintType", sFieldTitle: "车身油漆", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_InductEmpennage", sFieldTitle: "后导流尾翼", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_TopSnelf", sFieldTitle: "车顶行李箱架", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Besiege", sFieldTitle: "运动包围", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Doorglasstype", sFieldTitle: "车门玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_CarWindow", sFieldTitle: "车窗", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Fwindglasstype", sFieldTitle: "前风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_LockCease", sFieldTitle: "电动窗锁止功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_Bwindglasstype", sFieldTitle: "后风窗玻璃类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_AvoidNipHead", sFieldTitle: "电动窗防夹功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BackCurtain", sFieldTitle: "后窗遮阳帘", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BWindHot", sFieldTitle: "后风窗加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_BBrushSensor", sFieldTitle: "后雨刷器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FBrushSensor", sFieldTitle: "雨刷传感器", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorDazzle", sFieldTitle: "内后视镜防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorElecTune", sFieldTitle: "外后视镜电动调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorFold", sFieldTitle: "外后视镜电动折叠功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrorHot", sFieldTitle: "外后视镜加热功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_ReMirrormemory", sFieldTitle: "外后视镜记忆功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FrontLightType", sFieldTitle: "前照灯类型", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightDazzle", sFieldTitle: "会车前灯防眩目功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightSteer", sFieldTitle: "前大灯随动转向", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightDelay", sFieldTitle: "前大灯延时关闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FfogLamp", sFieldTitle: "前雾灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightClose", sFieldTitle: "前大灯自动开闭", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightHeightTune", sFieldTitle: "前照灯照射高度调节", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_FLightAutoClean", sFieldTitle: "前照灯自动清洗功能", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_SideLight", sFieldTitle: "侧转向灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_luggageLight", sFieldTitle: "行李箱灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "OutStat_PerchStopLight", sFieldTitle: "高位(第三)制动灯", sTrPrefix: "8", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "内饰", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerMaterial", sFieldTitle: "方向盘表面材料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerTuneDirection", sFieldTitle: "方向盘调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerRange", sFieldTitle: "方向盘幅数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerMomery", sFieldTitle: "方向盘记忆设置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerTuneType", sFieldTitle: "方向盘调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SteerEtc", sFieldTitle: "方向盘换档", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelDisplay", sFieldTitle: "仪表板显示型式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelLightColor", sFieldTitle: "仪表板背光颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PanelLumTune", sFieldTitle: "仪表板亮度可调", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Hud", sFieldTitle: "HUD抬头数字显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Computer", sFieldTitle: "行车电脑", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Clock", sFieldTitle: "时钟", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Otemperature", sFieldTitle: "车外温度显示", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_OilWarn", sFieldTitle: "燃油不足警告方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Tach", sFieldTitle: "转速表", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Guide", sFieldTitle: "罗盘/指南针", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Altitude", sFieldTitle: "海拔仪", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatColor", sFieldTitle: "座椅颜色", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatMaterial", sFieldTitle: "座椅面料", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SportSeat", sFieldTitle: "运动座椅", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SeatKnead", sFieldTitle: "座椅按摩功能", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatHot", sFieldTitle: "驾驶座座椅加热", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatProp", sFieldTitle: "驾驶座腰部支撑调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatTuneType", sFieldTitle: "驾驶座座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatTuneDirection", sFieldTitle: "驾驶座座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DSeatMomery", sFieldTitle: "驾驶座椅调节记忆位置组数", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DASeatTuneType", sFieldTitle: "副驾驶座椅调节方式", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DASeatTuneDirection", sFieldTitle: "副驾驶座椅调节方向", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FCenterArmrest", sFieldTitle: "前座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BCenterArmrest", sFieldTitle: "后座中央扶手", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSafePillow", sFieldTitle: "主动式安全头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatPillow", sFieldTitle: "后座椅头枕", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FSeatPillowA", sFieldTitle: "前座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatPillowA", sFieldTitle: "后座椅头枕调节", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ChildSeatFix", sFieldTitle: "儿童安全座椅固定装置", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BSeatLieRate", sFieldTitle: "后排座位放倒比例", sTrPrefix: "9", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "影音空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Cassette", sFieldTitle: "卡带", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Radio", sFieldTitle: "收音机", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MP3Player", sFieldTitle: "MP3", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_VCDPlayer", sFieldTitle: "VCD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CDPlayer", sFieldTitle: "CD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CDNum", sFieldTitle: "CD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DVDPlayer", sFieldTitle: "DVD", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_DVDNum", sFieldTitle: "DVD碟数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Video", sFieldTitle: "车载电视", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CCEscreen", sFieldTitle: "中控台液晶屏", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BEscreen", sFieldTitle: "后排液晶显示器", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Audiobrand", sFieldTitle: "音响品牌", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_LoudHailer", sFieldTitle: "扬声器数量", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AirCSystem", sFieldTitle: "空调", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AirCType", sFieldTitle: "空调控制方式", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_HumidityTune", sFieldTitle: "湿度调节", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_TemperAreaCount", sFieldTitle: "温区个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BleakAirNum", sFieldTitle: "后排出风口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_leakAirNum", sFieldTitle: "出风口个数", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ExternalAudioInterface", sFieldTitle: "外接音源接口", sTrPrefix: "10", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "便利功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_PowerNum", sFieldTitle: "车内电源插口数量", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_12VPower", sFieldTitle: "车内电源电压", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ReadingLight", sFieldTitle: "车厢前阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BReadingLight", sFieldTitle: "车厢后阅读灯", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ILightCloseDelay", sFieldTitle: "车内灯光延时关闭", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_SpeedCruise", sFieldTitle: "定速巡航系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_GPS", sFieldTitle: "GPS电子导航", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Perf_ElecLimitSpeed", sFieldTitle: "电子限速", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RRadar", sFieldTitle: "倒车雷达", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_RImage", sFieldTitle: "倒车影像", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CenterControlLock", sFieldTitle: "中控门锁", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_TyrePressureWatcher", sFieldTitle: "胎压检测装置", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_POwerCleaner", sFieldTitle: "车门/行李箱电吸", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_WireLink", sFieldTitle: "无线上网功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Tel", sFieldTitle: "车载电话", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Bluetooth", sFieldTitle: "蓝牙系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_carFridge", sFieldTitle: "车载冰箱", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MultiFuncSteer", sFieldTitle: "多功能方向盘", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_MultiFuncsSteer", sFieldTitle: "多功能方向盘功能", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_AIgnitionSys", sFieldTitle: "无钥匙点火系统", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_Rckey", sFieldTitle: "遥控钥匙", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_RemoteTrunk", sFieldTitle: "遥控行李箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_COpenType", sFieldTitle: "行李箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_TrunkType", sFieldTitle: "行李箱盖开合方式", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_RemoteOilCover", sFieldTitle: "遥控油箱盖", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_OilLidOpen", sFieldTitle: "油箱盖车内开启", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_CenterBox", sFieldTitle: "中央置物盒", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FCarShelf", sFieldTitle: "前排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_BCarShelf", sFieldTitle: "后排杯架", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ClothesHook", sFieldTitle: "衣物挂钩", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_FaceMirror", sFieldTitle: "遮阳板化妆镜", sTrPrefix: "11", unit: "", joinCode: "" },
   { sType: "bar", sFieldName: "", sFieldTitle: "安全配置", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ABD", sFieldTitle: "ABD(自动制动差速器)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ABS", sFieldTitle: "ABS(刹车防抱死制动系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ASR", sFieldTitle: "ASR(驱动防滑装置)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_DSC", sFieldTitle: "DSC(动态稳定控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EBA", sFieldTitle: "EBA/EVA(紧急制动辅助系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EBD", sFieldTitle: "EBD/EBV(电子制动力分配)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EDS", sFieldTitle: "EDS(电子差速锁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ESP", sFieldTitle: "ESP(电子稳定程序)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_TCS", sFieldTitle: "TCS(牵引力控制系统)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_VATS", sFieldTitle: "电子防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_EATS", sFieldTitle: "发动机防盗系统", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_Other", sFieldTitle: "主动安全-其他", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_DriverGasBag", sFieldTitle: "驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_SubDriverGasBag", sFieldTitle: "副驾驶位安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_FheadGasbag", sFieldTitle: "前排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BheadGasbag", sFieldTitle: "后排头部气囊(气帘)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_FsadGasbag", sFieldTitle: "前排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BsadGasbag", sFieldTitle: "后排侧安全气囊", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_SGasbagShrink", sFieldTitle: "副气囊锁止功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_GasbagNum", sFieldTitle: "气囊气帘数量", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BeltPosTune", sFieldTitle: "前安全带调节", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BeltPreTighten", sFieldTitle: "安全带预收紧功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_lifeBeltlimit", sFieldTitle: "安全带限力功能", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BackBelt", sFieldTitle: "后排安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_BackthreespotBelt", sFieldTitle: "后排中间三点式安全带", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "InStat_ChildLock", sFieldTitle: "儿童锁", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_ShrinkFootBrake", sFieldTitle: "溃缩式制动踏板", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "Safe_DoorAvoidHamCL", sFieldTitle: "车门防撞杆(防撞侧梁)", sTrPrefix: "12", unit: "", joinCode: "" },
   { sType: "fieldPara", sFieldName: "UnderPan_turnShrink", sFieldTitle: "可溃缩转向柱", sTrPrefix: "12", unit: "", joinCode: "" }
];