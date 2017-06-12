function showUCar(csId, cityId, csSpell, csShowName, callbackFunc, top,lp,hp,ref) {
    var topStr = typeof (top) == "undefined" ? "" : "&count=" + top;
    var lpStr = typeof (lp) == "undefined" ? "" : "&lp=" + lp;
    var hpStr = typeof (hp) == "undefined" ? "" : "&hp=" + hp;
    var refStr = typeof (ref) == "undefined" ? "" : "&ref=" + ref;
	$.ajax({
		url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/CarListForYiChe.ashx?sid=" + csId + "&ctid=" + cityId + topStr + lpStr + hpStr + refStr,
		cache: true,
		dataType: "jsonp",
		jsonpCallback: "callback",
		success: function (data) {
			//特别页面调用函数
			if (callbackFunc != undefined && callbackFunc instanceof Function) {
			    callbackFunc(data, csId, csSpell, csShowName);
				return;
			}
			data = data.CarListInfo;
			if (data.length <= 0) return;
			var str = "";
			//str += "<h3><a target=\"_blank\" href=\"" + data[0].MoreCarUrL + "\">相关二手车</a></h3>";
			str += "<h3><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/\">相关二手车</a></h3>";
			str += "    <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" id=\"ucar_serialcity\">";
			str += "        <tbody>";
			//			str += "            <tr>";
			//			str += "                <th width=\"46%\" style=\"text-align: left\">";
			//			str += "                    车源信息";
			//			str += "                </th>";
			//			str += "                <th width=\"25%\">";
			//			str += "                    地区";
			//			str += "                </th>";
			//			str += "                <th width=\"20%\">";
			//			str += "                    价格";
			//			str += "                </th>";
			//			str += "            </tr>";
			$.each(data, function (i, n) {
				str += "<tr>";
				str += "<td style=\"text-align:left\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "\" class=\"car_name w100\">" + n.BrandName + " </a></td>";
				str += "<td class=\"cgray\"><a target=\"_blank\" href=\"" + n.CityUrL + "\">" + n.cityName + "</a></td>";
				str += "<td class=\"ucar_price\">" + n.DisplayPrice + "</td>";
				str += "</tr>";
				if (i >= 8)
					return;
			});
			str += "                    </tbody>";
			str += "                </table>";
			$(".line_box.ucar_box").html(str);
		}
	});
}