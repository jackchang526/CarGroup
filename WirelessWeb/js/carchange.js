//添加主品牌页面
function addCar() {
	var container = document.getElementById("container");
	var maseter_container = document.getElementById("master_container");
	BitAjax({
		url: "/handlers/GetMasterHTML.ashx",
		cache: true,
		success: function (data) {
			if (data != "") {
				if (container)
					container.style.display = "none";
				if (maseter_container)
					maseter_container.style.display = "block";
				maseter_container.innerHTML = data;
				//				loadJS.push("http://image.bitautoimg.com/carchannel/WirelessJs/carmasterselect.js?v=201209", "utf-8", function () {
				//					CarMasterSelect.url = "http://api.car.bitauto.com/CarInfo/SerialListJson.aspx?bsid={bsid}&callback=OutCallback";
				//					CarMasterSelect.BindMasterClick();
				//				});
				$.getScript("http://image.bitautoimg.com/carchannel/WirelessJs/carmasterselect.js?v=201209", function () {
					//绑定主品牌事件
					CarMasterSelect.url = "http://api.car.bitauto.com/CarInfo/SerialListJson.aspx?bsid={bsid}&callback=OutCallback";
					CarMasterSelect.BindMasterClick();
				});
			}
		}
	});
}
//回调某一主品牌下子品牌html
function OutCallback(data) {
	var popupBox = "";
	popupBox += "<div class=\"m-popup m-cars\">";
	for (var i = 0; i < data.length; i++) {
		var brand = data[i];
		popupBox += "<dl>";
		var brandName = brand.Name.replace("进口", "");
		if (data.length == 1 && brandName == CarMasterSelect.masterName) { }
		else
			popupBox += "<dt><a href=\"javascript:;\">" + brand.Name + "</a></dt>";
		for (var j = 0; j < brand.CsList.length; j++) {
			var serial = brand.CsList[j];
			popupBox += "<dd><a href=\"javascript:selectCar(" + serial.ID + ");\">" + serial.Name + "</a></dd>";
		}
		popupBox += "</dl>";
	}
	popupBox += "<div class=\"clear\"></div>";
	popupBox += "</div>";
	CarMasterSelect.data[CarMasterSelect.masterId] = popupBox;
	CarMasterSelect.setCallBack(popupBox);
}
//选择某子品牌下车型
function selectCar(serialId) {
	var container = document.getElementById("container");
	var maseter_container = document.getElementById("master_container");
	var carinfo_container = document.getElementById("carinfo_container");
	BitAjax({
		url: "/handlers/GetCarBySerialIdHTML.ashx?id=" + serialId,
		cache: true,
		success: function (data) {
			if (data != "") {
				document.documentElement.scrollTop = 0;
				document.body.scrollTop = 0;
				if (container)
					container.style.display = "none";
				if (maseter_container)
					maseter_container.style.display = "none";
				if (carinfo_container)
					carinfo_container.style.display = "block";
				carinfo_container.innerHTML = data;
				yearTab();
				if (typeof ComparePageObject != "undefined" && typeof ComparePageObject.arrCarIds != "undefined") {
					//已选择车型
					for (var i = 0; i < ComparePageObject.arrCarIds.length; i++) {
						var dlCar = $("#dl" + ComparePageObject.arrCarIds[i]);
						dlCar.attr("class", "m-summary-price-none");
						var txtA = dlCar.parent().html();
						dlCar.parent().parent().html(txtA);
					}
				}
			}
		}
	});
}
//车款选择页面 年款切换标签
function yearTab() {
	var tabs = document.getElementById("yeartag").getElementsByTagName("li");
	for (var i = 0; i < tabs.length; i++) {
		tabs[i].onclick = function () {
			if (this.className == 'current') return false;
			var x = 0;
			for (var j = 0; j < tabs.length; j++) {
				if (tabs[j] == this) {
					x = j;
				}
				else {
					tabs[j].className = "";
					document.getElementById(("yearDiv" + j)).style.display = "none";
				}
			}
			this.className = "current";
			document.getElementById(("yearDiv" + x)).style.display = "";
		}
	}
}

//返回到对比页面
function goCarCompare() {
	var container = document.getElementById("container");
	var maseter_container = document.getElementById("master_container");
	if (container)
		container.style.display = "block";
	if (maseter_container) {
		maseter_container.style.display = "none";
	}
}
//返回到选择车型
function goMasterBrand() {
	var container = document.getElementById("container");
	var carinfo_container = document.getElementById("carinfo_container");
	var maseter_container = document.getElementById("master_container");
	if (carinfo_container)
		carinfo_container.style.display = "none";
	if (maseter_container) {
		maseter_container.style.display = "block";
	} else {
		if (container)
			container.style.display = "block";
	}
}

