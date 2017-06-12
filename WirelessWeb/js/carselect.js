//添加主品牌页面
function addCar() {
	var container = document.getElementById("container");
	var maseter_container = document.getElementById("master_container");
	BitAjax({
		url: "/handlers/GetCarDataJson.ashx?action=master",
		cache: true,
		success: function (data) {
			var datajson = eval("(" + data + ")");
			//if (datajson = "" ) return;
			if (container)
				container.style.display = "none";
			if (maseter_container)
				maseter_container.style.display = "block";
			maseter_container.innerHTML = getmasterListHtml(datajson);
			//隐藏目录
			var popMenu = document.getElementById("popMenu");
			if (popMenu)
				popMenu.style.display = "none";
		}
	});
}
function getmasterListHtml(datajson) {
	var charHtml = [];
	//返回菜单
	charHtml.push(" <div class=\"b-return\"><a class=\"btn-return\" href=\"javascript:goCarCompare();\">返回</a><span>选品牌</span></div>");
	//字母列表
	charHtml.push("<div class=\"letter-list\"><ul>");
	//热门品牌
	charHtml.push("	<li id=\"char_hot\"><a href=\"#char_nav_hot\">热</a></li>");
	for (var key in datajson.CharList) {
		if (datajson.CharList[key] > 0) {
			charHtml.push("	<li id=\"char_" + key + "\"><a href=\"#char_nav_" + key + "\">" + key + "</a></li>");
		}
	}
	charHtml.push("</ul></div>");
	//主品牌列表
	charHtml.push("<div class=\"brand-list bybrand_list\">");
	//热门品牌
	charHtml.push("<div id=\"char_nav_hot\" class=\"tt-small\"><span>热门品牌</span></div>");
	charHtml.push("<div class=\"wrap\"><ul>");
	charHtml.push("<li><a href=\"javascript:addSerial(8,'大众')\"> <span class=\"brand-logo m_8_b\"></span><span class=\"brand-name\">大众</span></a></li>");
	charHtml.push("<li><a href=\"javascript:addSerial(13,'现代')\"> <span class=\"brand-logo m_13_b\"></span><span class=\"brand-name\">现代</span></a></li>");
	charHtml.push("<li><a href=\"javascript:addSerial(17,'福特')\"> <span class=\"brand-logo m_17_b\"></span><span class=\"brand-name\">福特</span></a></li>");
	charHtml.push("<li><a href=\"javascript:addSerial(28,'起亚')\"> <span class=\"brand-logo m_28_b\"></span><span class=\"brand-name\">起亚</span></a></li>");
	charHtml.push("</ul></div>");

	for (var key in datajson.MsterList) {
		if (datajson.MsterList[key].length > 0) {
			charHtml.push("<div id=\"char_nav_" + key + "\" class=\"tt-small\"><span>" + key + "</span></div>");
			charHtml.push("<div class=\"wrap\"><ul>");
			for (var i = 0; i < datajson.MsterList[key].length; i++) {
				charHtml.push("<li id=\"char_nav_" + datajson.MsterList[key][i].AllSpell + "\"><a href=\"javascript:addSerial(" + datajson.MsterList[key][i].MasterId + ",'" + datajson.MsterList[key][i].MasterName + "')\"> <span class=\"brand-logo m_" + datajson.MsterList[key][i].MasterId + "_b\"></span><span class=\"brand-name\">" + datajson.MsterList[key][i].MasterName + "</span></a></li>");
			}
			charHtml.push("</ul></div>");
		}
	}
	charHtml.push("</div>");
	return charHtml.join('');
}
//添加子品牌页面
function addSerial(id, name) {
	var container = document.getElementById("master_container");
	var maseter_container = document.getElementById("serial_container");
	BitAjax({
		url: "/handlers/GetCarDataJson.ashx?action=serial&pid=" + id,
		cache: true,
		success: function (data) {
			var datajson = eval("(" + data + ")");
			//if (datajson = "" ) return;
			if (container)
				container.style.display = "none";
			if (maseter_container)
				maseter_container.style.display = "block";
			maseter_container.innerHTML = getserialListHtml(datajson, id, name);
			document.documentElement.scrollTop = 0;
			document.body.scrollTop = 0;
		}
	});

}

function getserialListHtml(datajson, id, name) {
	var charHtml = [];
	//返回菜单
	charHtml.push(" <div class=\"b-return\"><a class=\"btn-return\" href=\"javascript:goMasterBrand();\">返回</a><span>选车系</span></div>");

	charHtml.push("<div class=\"choose-car-name-close bybrand_list\">");
	charHtml.push("<div class=\"brand-logo-none-border m_" + id + "_b\"></div>");
	charHtml.push("<span class=\"brand-name\">" + name + "</span></div>");
	for (var i = 0; i < datajson.length; i++) {
		if (datajson[i].Child.length <= 0) continue;
		charHtml.push("<div class=\"tt-small\"><span>" + datajson[i].BrandName + "</span></div>");
		charHtml.push("<div class=\"pic-txt-h pic-txt-9060\"><ul>");
		for (var j = 0; j < datajson[i].Child.length; j++) {
			charHtml.push("<li><a href=\"javascript:addstyle(" + datajson[i].Child[j].SerialId + ",'" + datajson[i].Child[j].SerialName + "')\"> <img src=\"" + datajson[i].Child[j].ImageUrl + "\" /><h4>" + datajson[i].Child[j].SerialName + "</h4><p><strong>" + datajson[i].Child[j].Price + "</strong></p></a></li>");
		}
		charHtml.push("</ul></div>");
	}

	return charHtml.join('');
}

//添加车型页面
function addstyle(id, name) {
	var container = document.getElementById("serial_container");
	var maseter_container = document.getElementById("carinfo_container");
	BitAjax({
		url: "/handlers/GetCarDataJson.ashx?action=car&pid=" + id,
		cache: true,
		success: function (data) {
			var datajson = eval("(" + data + ")");
			//if (datajson = "" ) return;
			if (container)
				container.style.display = "none";
			if (maseter_container)
				maseter_container.style.display = "block";
			maseter_container.innerHTML = getcarListHtml(datajson, id, name);
			//已选择车型
			for (var i = 0; i < ComparePageObject.arrCarIds.length; i++) {
				var dlCar = $("#" + ComparePageObject.arrCarIds[i]);
				dlCar.attr("class", "none");
				var txtA = dlCar.children().html();
				dlCar.html(txtA);
			}
			document.documentElement.scrollTop = 0;
			document.body.scrollTop = 0;
		}
	});

}
function getcarListHtml(datajson, id, name) {
	var charHtml = [];
	//返回菜单
	charHtml.push(" <div class=\"b-return\"><a class=\"btn-return\" href=\"javascript:goSerial();\">返回</a><span>选车款</span></div>");

	charHtml.push("<div class=\"choose-car-style-close\">");
	charHtml.push("<div class=\"car-style-img\"><img src='" + datajson.ImageUrl + "' /></div>");
	charHtml.push("<span class=\"car-style-name\">" + name + "</span></div>");
	//for (var i = 0; i < datajson.CarList.length; i++) {
	for (var year in datajson.CarList) {
		charHtml.push("<div class=\"tt-small\"><span>" + year.replace('s', '') + "款</span></div>");
		charHtml.push("<div class=\"pic-txt-h pic-txt-9060\"><ul>");
		var list = datajson.CarList[year]
		for (var j = 0; j < list.length; j++) {
			charHtml.push("<li id = '" + list[j].CarId + "'><a href=\"javascript:selectCarId(" + list[j].CarId + ")\"> <h4>" + list[j].CarName + "</h4><p><strong>" + list[j].Price + "</strong></p></a></li>");
		}
		charHtml.push("</ul></div>");

		//        }
	}
	return charHtml.join('');
}

//返回到对比页面
function goCarCompare() {
	var container = document.getElementById("container");
	var maseter_container = document.getElementById("master_container");
	var popMenu = document.getElementById("popMenu");
	if (container)
		container.style.display = "block";
	if (maseter_container) {
		maseter_container.style.display = "none";
	}
	if (popMenu)
		popMenu.style.display = "block";
}
//返回到选择主品牌
function goMasterBrand() {
	var container = document.getElementById("container");
	var serial_container = document.getElementById("serial_container");
	var maseter_container = document.getElementById("master_container");
	if (serial_container)
		serial_container.style.display = "none";
	if (maseter_container) {
		maseter_container.style.display = "block";
	} else {
		if (container)
			container.style.display = "block";
	}
}
//返回到选择子品牌
function goSerial() {
	var container = document.getElementById("container");
	var carinfo_container = document.getElementById("carinfo_container");
	var maseter_container = document.getElementById("serial_container");
	if (carinfo_container)
		carinfo_container.style.display = "none";
	if (maseter_container) {
		maseter_container.style.display = "block";
	} else {
		if (container)
			container.style.display = "block";
	}
}

