// 车型及子品牌列表 添加对比用
var ListForCompare = {
	DataPath: "/car/ajaxnew/ListForComparev2.1.ashx?type=",     // 数据源位置
	PageLiObjArr: new Array(),
	PageXmlHttpRequest: null,                                                                                             // HttpRequest 对象
	PageXmlDocObject: null,                                                                                               // XML数据对象
	HotSerialCount: 42,
	SerialLevelListID: "SerialLevelListUL",
	SerialLevelListObj: null,
	SerialListForLevelID: "SerialListForLevelUL",
	SerialListForLevelObj: null,
	RootNodeName: "Root",                                                  // 数据根节点名
	IsIE: true,
	IsHidList: false,
	CarListDataPath: "/car/ajaxnew/SerialToCarForCompare.aspx",
	CarXmlHttpRequest: null,
	CarListDivObject: null,
	CarListDivXPosition: 0,
	CarListDivYPosition: 0,
	IsUseURL: true
};

// 区间对象
function PageLiObj(liIndex, liArrSerialObj, liSubRange) {
	this.Index = liIndex;
	this.ArrSerialObj = liArrSerialObj;
	this.Range = liSubRange;
};

// 区间子品牌对象
function ListSerialForLi(id, name, showName, level, bodyType, pv, multiPriceRange) {
	this.ID = id;
	this.Name = name;
	this.ShowName = showName;
	this.Level = level;
	this.BodyType = bodyType;
	this.Pv = pv;
	this.MultiPriceRange = multiPriceRange;
};

function LevelForRangeObj(hotSerial, has2, has3, hasSUV, hasMPV, hasPao, hasMian, hasQi) {
	this.HotSerial = hotSerial;
	this.Has2 = has2;
	this.Has3 = has3;
	this.HasSUV = hasSUV;
	this.HasMPV = hasMPV;
	this.HasPao = hasPao;
	this.HasMian = hasMian;
	this.HasQi = hasQi;
}

var LevelForRange = [
    { Name: "热门车型", HasNode: true, Type: 0 },
    { Name: "两厢轿车", HasNode: false, Type: 1 },
    { Name: "三厢轿车", HasNode: false, Type: 2 },
    { Name: "MPV多用途车", HasNode: false, Type: 4 },
    { Name: "SUV越野车", HasNode: false, Type: 3 },
    { Name: "跑车", HasNode: false, Type: 6 },
    { Name: "面包车", HasNode: false, Type: 5 },
    { Name: "其它", HasNode: false, Type: 7 }
];

var PriceForRange = [
    { Name: "热门车型", HasNode: true, Type: 0 },
    { Name: "5万以内", HasNode: false, Type: 1 },
    { Name: "5-8万", HasNode: false, Type: 2 },
    { Name: "8-12万", HasNode: false, Type: 3 },
    { Name: "12-18万", HasNode: false, Type: 4 },
    { Name: "18-25万", HasNode: false, Type: 5 },
    { Name: "25-40万", HasNode: false, Type: 6 },
    { Name: "40-80万", HasNode: false, Type: 7 },
    { Name: "80万以上", HasNode: false, Type: 8 }
];

function intiPageObjForCompareList() {
	ListForCompare.CarListDivObject = document.getElementById("pop_compare_forcarlist");
	if (document.getElementById(ListForCompare.SerialLevelListID))
	{ ListForCompare.SerialLevelListObj = document.getElementById(ListForCompare.SerialLevelListID); }
	if (document.getElementById(ListForCompare.SerialListForLevelID))
	{ ListForCompare.SerialListForLevelObj = document.getElementById(ListForCompare.SerialListForLevelID); }
}

// 切换标签
function pageSelectTagForList(tagIndex) {
	showCarListDivAndResetPosition(false, 'pop_compare_forcarlist');
	intiPageObjForCompareList();
	ListForCompare.IsHidList = false;
	if (tagIndex >= 1 && tagIndex <= 12) {
		// 切换标签 
		for (var i = 1; i <= 12; i++) {
			var tags = document.getElementById("ListForCompare_li" + i);
			if (tags) {
				if (i == tagIndex)
				{ tags.className = "current"; }
				else
				{ tags.className = ""; }
			}
		}
		// 检查已取标签数据
		var getIndex = 0;
		var isGetBefore = false;
		if (ListForCompare.PageLiObjArr.length > 0) {
			for (var i = 0; i < ListForCompare.PageLiObjArr.length; i++) {
				if (ListForCompare.PageLiObjArr[i].Index == tagIndex) {
					isGetBefore = true;
					getIndex = i;
					break;
				}
			}
		}
		if (!isGetBefore) {
			// 取数据
			var pageLiObj = new PageLiObj;
			pageLiObj.Index = tagIndex;
			getDataByIndex(pageLiObj);
		}
		else {
			// 取过数据
			generateLevelRange(ListForCompare.PageLiObjArr[getIndex]);
			getSerialListBySelectLevel(ListForCompare.PageLiObjArr[getIndex].Index, 0);
		}
		showOrHideElementById('ListForCompare_divContent', true);
	}
}

function getDataByIndex(pageLiObj) {
	ListForCompare.IsIE = CheckBrowser();
	ListForCompare.PageXmlHttpRequest = createXMLHttpRequest();
	if (ListForCompare.IsIE) {
		ListForCompare.PageXmlHttpRequest.onreadystatechange = function () { handleStateChangeForListForCompare(pageLiObj) };
	}
	ListForCompare.PageXmlHttpRequest.open("GET", ListForCompare.DataPath + pageLiObj.Index, false);
	ListForCompare.PageXmlHttpRequest.send(null);
	if (!ListForCompare.IsIE) {
		parseXmlObjectForSerialArrList(pageLiObj, ListForCompare.PageXmlHttpRequest.responseXML);
	}
}

function handleStateChangeForListForCompare(pageLiObj) {
	if (ListForCompare.PageXmlHttpRequest.readyState == 4) {
		if (ListForCompare.PageXmlHttpRequest.status == 200) {
			parseXmlObjectForSerialArrList(pageLiObj, ListForCompare.PageXmlHttpRequest.responseXML);
		}
	}
}

function parseXmlObjectForSerialArrList(pageLiObj, xmlObj) {
	if (xmlObj) {
		var serialList = xmlObj.getElementsByTagName(ListForCompare.RootNodeName)[0].childNodes;
		pageLiObj.ArrSerialObj = new Array();
		pageLiObj.Range = ",0,";
		for (var i = 0; i < serialList.length; i++) {
			if (serialList[i].nodeType == 1) {
				var serialNode = new ListSerialForLi();
				serialNode.ID = serialList[i].getAttributeNode("ID").nodeValue;
				serialNode.Name = serialList[i].getAttributeNode("Name").nodeValue;
				serialNode.ShowName = serialList[i].getAttributeNode("ShowName").nodeValue;
				serialNode.Level = serialList[i].getAttributeNode("Level").nodeValue;
				serialNode.BodyType = serialList[i].getAttributeNode("BodyType").nodeValue;
				serialNode.Pv = serialList[i].getAttributeNode("Pv").nodeValue;
				serialNode.MultiPriceRange = serialList[i].getAttributeNode("MultiPriceRange").nodeValue;
				pageLiObj.ArrSerialObj.push(serialNode);
				checkLevelRange(pageLiObj, serialNode);
			}
		}
		ListForCompare.PageLiObjArr.push(pageLiObj);
		generateLevelRange(pageLiObj);
		getSerialListBySelectLevel(ListForCompare.PageLiObjArr[ListForCompare.PageLiObjArr.length - 1].Index, 0);
	}
}

// 检查2级列表
function checkLevelRange(pageLiObj, listSerialForLi) {
	try {
		if (pageLiObj.Index >= 1 && pageLiObj.Index <= 8) {
			// 价格区间
			if (listSerialForLi.BodyType == "两厢" && pageLiObj.Range.indexOf(",1,") < 0) {
				pageLiObj.Range += "1,";
			}
			if (listSerialForLi.BodyType == "三厢" && pageLiObj.Range.indexOf(",2,") < 0) {
				pageLiObj.Range += "2,";
			}
			if (listSerialForLi.Level == "SUV" && pageLiObj.Range.indexOf(",3,") < 0) {
				pageLiObj.Range += "3,";
			}
			if (listSerialForLi.Level == "MPV" && pageLiObj.Range.indexOf(",4,") < 0) {
				pageLiObj.Range += "4,";
			}
			if (listSerialForLi.Level == "面包车" && pageLiObj.Range.indexOf(",5,") < 0) {
				pageLiObj.Range += "5,";
			}
			if (listSerialForLi.Level == "跑车" && pageLiObj.Range.indexOf(",6,") < 0) {
				pageLiObj.Range += "6,";
			}
			if (listSerialForLi.Level == "其它" && pageLiObj.Range.indexOf(",7,") < 0) {
				pageLiObj.Range += "7,";
			}
		}
		if (pageLiObj.Index >= 9 && pageLiObj.Index <= 12) {
			for (var i = 1; i <= 8; i++) {
				if (listSerialForLi.MultiPriceRange.indexOf("," + i + ",") >= 0 && pageLiObj.Range.indexOf("," + i + ",") < 0) {
					pageLiObj.Range += i + ",";
				}
			}
		}
	}
	catch (err)
    { }
}

// 2级标签
function generateLevelRange(pageLiObj) {
	var listArr = new Array();
	if (pageLiObj.Index >= 1 && pageLiObj.Index <= 8) {
		if (pageLiObj.Range.length > 0) {
			for (var i = 0; i < LevelForRange.length; i++) {
				if (pageLiObj.Range.indexOf("," + LevelForRange[i].Type + ",") >= 0) {
					listArr.push("<li id=\"SelectLiForLevel_" + LevelForRange[i].Type + "\"><a href=\"javascript:getSerialListBySelectLevel(" + pageLiObj.Index + "," + LevelForRange[i].Type + ");\">" + LevelForRange[i].Name + "</a></li>");
				}
			}
		}
	}
	if (pageLiObj.Index >= 9 && pageLiObj.Index <= 12) {
		if (pageLiObj.Range.length > 0) {
			for (var i = 0; i < PriceForRange.length; i++) {
				if (pageLiObj.Range.indexOf("," + PriceForRange[i].Type + ",") >= 0) {
					listArr.push("<li id=\"SelectLiForLevel_" + PriceForRange[i].Type + "\"><a href=\"javascript:getSerialListBySelectLevel(" + pageLiObj.Index + "," + PriceForRange[i].Type + ");\">" + PriceForRange[i].Name + "</a></li>");
				}
			}
		}
	}
	if (ListForCompare.SerialLevelListObj)
	{ ListForCompare.SerialLevelListObj.innerHTML = listArr.join(""); }
}

function getSerialListBySelectLevel(index, type) {
	showCarListDivAndResetPosition(false, 'pop_compare_forcarlist');
	if (ListForCompare.PageLiObjArr.length > 0) {// && index <= ListForCompare.PageLiObjArr.length) {
		for (var i = 0; i < ListForCompare.PageLiObjArr.length; i++) {
			if (ListForCompare.PageLiObjArr[i].Index == index) {
				var serialListArr = new Array();
				if (ListForCompare.PageLiObjArr[i].ArrSerialObj.length > 0) {
					if (index >= 1 && index <= 8) {
						// 热门新车
						if (type == 0) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (j >= ListForCompare.HotSerialCount)
								{ break; }
								serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
							}
						}
						// 两厢轿车
						if (type == 1) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].BodyType == "两厢") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// 三厢轿车
						if (type == 2) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].BodyType == "三厢") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// SUV
						if (type == 3) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].Level == "SUV") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// MPV
						if (type == 4) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].Level == "MPV") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// 面包车
						if (type == 5) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].Level == "面包车") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// 跑车
						if (type == 6) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].Level == "跑车") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
						// 其它
						if (type == 7) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].Level == "其它") {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
					}
					if (index >= 9 && index <= 12) {
						// 热门新车
						if (type == 0) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (j >= ListForCompare.HotSerialCount)
								{ break; }
								serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
							}
						}
						if (type >= 1 && type <= 8) {
							for (var j = 0; j < ListForCompare.PageLiObjArr[i].ArrSerialObj.length; j++) {
								if (ListForCompare.PageLiObjArr[i].ArrSerialObj[j].MultiPriceRange.indexOf("," + type + ",") >= 0) {
									serialListArr.push("<li id=\"serialListLi_" + j + "\"><a href=\"javascript:getCarDataBySerial(" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ID + ",'serialListLi_" + j + "');\">" + ListForCompare.PageLiObjArr[i].ArrSerialObj[j].ShowName + "</a></li>");
								}
							}
						}
					}
				}
				if (ListForCompare.SerialListForLevelObj)
				{ ListForCompare.SerialListForLevelObj.innerHTML = serialListArr.join(""); }
			}
		}
	}
	// 更新样式
	var arrTemp = new Array();
	if (index >= 1 && index <= 8)
	{ arrTemp = LevelForRange; }
	if (index >= 9 && index <= 12)
	{ arrTemp = PriceForRange; }
	for (var i = 0; i < arrTemp.length; i++) {
		if (!document.getElementById("SelectLiForLevel_" + arrTemp[i].Type))
		{ continue; }
		if (arrTemp[i].Type != type) {
			document.getElementById("SelectLiForLevel_" + arrTemp[i].Type).className = "";
		}
		else {
			document.getElementById("SelectLiForLevel_" + arrTemp[i].Type).className = "current";
		}
	}
}

// 请求对比子品牌列表
function requestListForCompare(isNeedCar) {
	if (isNeedCar)
	{ ListForCompare.IsNeedCarList = true; }
	else
	{ ListForCompare.IsNeedCarList = false; }
	// inti Page Object
	if (document.getElementById(ListForCompare.PageDivID))
	{ ListForCompare.PageDivObject = document.getElementById(ListForCompare.PageDivID); }

	ListForCompare.IsIE = CheckBrowser();
	ListForCompare.PageXmlHttpRequest = createXMLHttpRequest();
	if (ListForCompare.IsIE) {
		ListForCompare.PageXmlHttpRequest.onreadystatechange = handleStateChangeForListForCompare;
	}
	ListForCompare.PageXmlHttpRequest.open("GET", ListForCompare.DataPath, false);
	ListForCompare.PageXmlHttpRequest.send(null);
	if (!ListForCompare.IsIE) {
		ListForCompare.PageXmlDocObject = ListForCompare.PageXmlHttpRequest.responseXML;
		parsePageXmlObjectForCompareList();
	}
}

// 显示隐藏标记
function showOrHideElementById(id, isShow) {
	var showOrHideElementA = document.getElementById("showOrHideElementA");
	var pageObj = document.getElementById(id);
	if (pageObj) {
		if (isShow) {
			pageObj.style.display = "";
			if (showOrHideElementA) {
				showOrHideElementA.className = "close";
				if (ListForCompare.IsIE)
				{ showOrHideElementA.innerText = '收起'; }
				else
				{ showOrHideElementA.textContent = '收起'; }
			}
		}
		else {
			if (pageObj.style.display == "none") {
				pageObj.style.display = "";
				if (showOrHideElementA) {
					showOrHideElementA.className = "close";
					if (ListForCompare.IsIE)
					{ showOrHideElementA.innerText = '收起'; }
					else
					{ showOrHideElementA.textContent = '收起'; }
				}
				if (ListForCompare.PageLiObjArr.length == 0) {
					pageSelectTagForList(1); return;
				}
			}
			else {
				pageObj.style.display = "none";
				if (showOrHideElementA) {
					showOrHideElementA.className = "show";
					if (ListForCompare.IsIE)
					{ showOrHideElementA.innerText = '展开'; }
					else
					{ showOrHideElementA.textContent = '展开'; }
					showCarListDivAndResetPosition(false, "pop_compare_forcarlist");
				}
			}
		}
	}
}

// 取子品牌的车型数据
function getCarDataBySerial(csID, objID) {
	if (objID != "") {
		$("a.current").each(function () {
			$(this).removeClass("current");
		});
		$("#" + objID).find("a").addClass("current");
		var obj = document.getElementById(objID);
		if (obj) {
			var x = obj.offsetLeft, y = obj.offsetTop;
			var xOffset = obj.offsetWidth;
			var yOffset = obj.offsetHeight;
			while (obj = obj.offsetParent) {
				x += obj.offsetLeft;
				y += obj.offsetTop;
			}
			ListForCompare.CarListDivXPosition = x + 21; //  + xOffset / 3;
			ListForCompare.CarListDivYPosition = y + yOffset - 5; // / 2;
		}
	}
	if (csID < 1)
	{ return; }
	ListForCompare.CarXmlHttpRequest = createXMLHttpRequest();
	if (ListForCompare.IsIE) {
		ListForCompare.CarXmlHttpRequest.onreadystatechange = handleStateChangeForGetCarListBySerial;
	}
	ListForCompare.CarXmlHttpRequest.open("GET", ListForCompare.CarListDataPath + "?csid=" + csID, false);
	ListForCompare.CarXmlHttpRequest.send(null);
	if (!ListForCompare.IsIE) {
		getAndShowCarListForSerial(ListForCompare.CarXmlHttpRequest.responseText);
	}
}

function handleStateChangeForGetCarListBySerial() {
	if (ListForCompare.CarXmlHttpRequest.readyState == 4) {
		if (ListForCompare.CarXmlHttpRequest.status == 200) {
			getAndShowCarListForSerial(ListForCompare.CarXmlHttpRequest.responseText);
		}
	}
}

// 车型数据
function getAndShowCarListForSerial(carList) {
	ListForCompare.CarListDivObject.innerHTML = carList;
	showCarListDivAndResetPosition(true, "pop_compare_forcarlist");
	if (ListForCompare.IsIE) {
		ListForCompare.CarListDivObject.style.top = ListForCompare.CarListDivYPosition;
		ListForCompare.CarListDivObject.style.left = ListForCompare.CarListDivXPosition;
	}
	else {
		ListForCompare.CarListDivObject.style.top = ListForCompare.CarListDivYPosition + "px";
		ListForCompare.CarListDivObject.style.left = ListForCompare.CarListDivXPosition + "px";
	}
}

// 显示或隐藏对象
function showCarListDivAndResetPosition(isShow, objID) {
	var obj = document.getElementById(objID);
	if (obj) {
		if (isShow) {
			obj.style.display = "";
		}
		else {
			obj.style.display = "none";
			$("a.current").each(function () {
				$(this).removeClass("current");
			});
		}
	}
}

// 添加车型到对比
function addCarToCompareFromlist(carID, carName, csName) {
	showCarListDivAndResetPosition(false, "pop_compare_forcarlist");
	if (carID > 0) {
		try {
			addCarToCompareForSelect(carID, carName, csName); 
		}
		catch (err)
		{ }
	}
}

function createXMLHttpRequest() {
	if (window.ActiveXObject) {
		return new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) {
		return new XMLHttpRequest();
	}
}

function CheckBrowser() {
	if (window.ActiveXObject) {
		return true;
	}
	else {
		return false;
	}
}

