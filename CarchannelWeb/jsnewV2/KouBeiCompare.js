var KouBeiCompare = {
    ScoreL: 0,
    ScoreR: 0,
    Init: function() {
        var self = this;
        self.CreateDiffHtml();
        self.SetRecommendation();
    },
    CreateDiffHtml: function() {
        var self = this;
        var loopCount = arrField.length;
        for (var i = 0; i < loopCount; i++) {
            var blockJson = arrField[i];
            switch (blockJson.title) {
            case "车身尺寸":
                self.CreateWaiGuanHtml(blockJson);
                break;
            case "性能动力":
                self.CreateDongLiHtml(blockJson);
                break;
            case "油耗":
                self.CreateYouHaoHtml(blockJson);
                break;
            case "安全配置":
                self.CreatePeiZhiHtml(blockJson);
                break;
            case "行车辅助":
                self.CreateCaoKongHtml(blockJson);
                break;
            case "内部配置":
                self.CreateNeiShiHtml(blockJson);
                break;
            }
        }
    },
    //不参与权重计算
    CreateYouHaoHtml: function(data) {
        if (typeof koubeiCompareJson != "undefined"&&koubeiCompareJson.length == 2 && koubeiCompareJson[0][0][0] == carIdL && koubeiCompareJson[1][0][0] == carIdR) {

            var fuelTypeL = koubeiCompareJson[0][3][19]; //油耗类型
            var fuelTypeR = koubeiCompareJson[1][3][19]; //油耗类型

            var displacementL = ""; //排量
            var transmissionL = ""; //变速箱
            var inhaleTypeL = ""; //进气形式
            var fuelL = ""; //油耗
            var displacementUnitL = "";
            var fuelUnitL = "";

            var displacementR = ""; //排量
            var transmissionR = ""; //变速箱
            var inhaleTypeR = ""; //进气形式
            var fuelR = ""; //油耗
            var displacementUnitR = "";
            var fuelUnitR = "";

            var items = data.items;
            for (var i = 0; i < items.length; i++) {
                var prefixArr = [];
                var fieldArr = [];
                var unitArr = [];
                //多参数的情况
                if (items[i].sType == "matilpara") {
                    prefixArr = items[i].sTrPrefix.split(",");
                    fieldArr = items[i].sFieldIndex.split(",");
                    unitArr = items[i].unit.split(",");
                } else {
                    prefixArr.push(items[i].sTrPrefix);
                    fieldArr.push(items[i].sFieldIndex);
                    unitArr.push(items[i].unit);
                }


                //sType sTitle sPid sTrPrefix sFieldIndex unit
                switch (items[i].sTitle) {
                case "综合工况油耗":
                    for (var j = 0; j < prefixArr.length; j++) {
                        if (fuelTypeL == "电力") {
                            fuelL = "暂无";
                        } else {
                            fuelL += koubeiCompareJson[0][prefixArr[j]][fieldArr[j]];
                            fuelUnitL = unitArr[j];
                        }
                        if (fuelTypeR == "电力") {
                            fuelR = "暂无";
                        } else {
                            fuelR += koubeiCompareJson[1][prefixArr[j]][fieldArr[j]];
                            fuelUnitR = unitArr[j];
                        }
                    }
                    break;
                case "排量":
                    for (var k = 0; k < prefixArr.length; k++) {
                        var tempdisplacementL = koubeiCompareJson[0][prefixArr[k]][fieldArr[k]];
                        var tempdisplacementR = koubeiCompareJson[1][prefixArr[k]][fieldArr[k]];
                        displacementL += (tempdisplacementL == "" || tempdisplacementL == "无" || tempdisplacementL == "待查") ? "" : tempdisplacementL + unitArr[k];
                        displacementR += (tempdisplacementR == "" || tempdisplacementR == "无" || tempdisplacementR == "待查") ? "" : tempdisplacementR + unitArr[k];
                    }
                    break;
                case "变速箱":
                    for (var l = 0; l < prefixArr.length; l++) {
                        var tempTranL = koubeiCompareJson[0][prefixArr[l]][fieldArr[l]];
                        var tempTranR = koubeiCompareJson[1][prefixArr[l]][fieldArr[l]];
                        transmissionL += (tempTranL == "" || tempTranL == "无" || tempTranL == "待查") ? "" : tempTranL + unitArr[l];
                        transmissionR += (tempTranR == "" || tempTranR == "无" || tempTranR == "待查") ? "" : tempTranR + unitArr[l];
                    }
                    break;
                case "进气形式":
                    for (var x = 0; x < prefixArr.length; x++) {
                        var tempinhaleTypeL = koubeiCompareJson[0][prefixArr[x]][fieldArr[x]];
                        var tempinhaleTypeR = koubeiCompareJson[1][prefixArr[x]][fieldArr[x]];
                        inhaleTypeL += (tempinhaleTypeL == "" || tempinhaleTypeL == "无" || tempinhaleTypeL == "待查" || tempinhaleTypeL == "增压") ? "" : tempinhaleTypeL + unitArr[x];
                        inhaleTypeR += (tempinhaleTypeR == "" || tempinhaleTypeR == "无" || tempinhaleTypeR == "待查" || tempinhaleTypeR == "增压") ? "" : tempinhaleTypeR + unitArr[x];
                    }
                    break;
                default:
                }
            }
            var htmlArr = [];
            htmlArr.push("<div class=\"yh-cont fl\">");
            if (!isNaN(fuelL) && parseFloat(fuelL) > 0) {
                htmlArr.push("	<div class=\"yh-pic\">");
                htmlArr.push("		<div class=\"line\" style=\"height: " + (90 * fuelL) / 35.6 + "px\"></div>");
            } else {
                htmlArr.push("	<div class=\"yh-pic yh-pic-gray\">");
                htmlArr.push("		<div class=\"line\"></div>");
            }
            htmlArr.push("	</div>");
            htmlArr.push("	<div class=\"yh-mid\">");
            htmlArr.push("		<div class=\"youhao\">");
            if (parseFloat(fuelL) > 0) {
                htmlArr.push(parseFloat(fuelL).toFixed(1) + "<span class=\"danwei\">" + fuelUnitL + "</span>");
            } else {
                htmlArr.push("<p class=\"yh-gray\">暂无数据</p>"); 
            }

            if (!isNaN(fuelL) && !isNaN(fuelR)) {
                if (parseFloat(fuelL) < parseFloat(fuelR)) {
                    htmlArr.push("<span class=\"sheng\">省</span>");
                }
            }
            htmlArr.push("		</div>");
            htmlArr.push("		<div class=\"pinfen\">");
            
            if (parseFloat(YouHaoL) > 0) {
                htmlArr.push("			<span class=\"tit\">网友评分</span>" + YouHaoL + "<em>分</em>");
            } else {
                htmlArr.push("<p class=\"yh-gray\">暂无评分</p>");
            }
            htmlArr.push("		</div>");
            htmlArr.push("	</div>");
            htmlArr.push("	<div class=\"yh-right\">");
            htmlArr.push("		<ul>");
            if (displacementL != "" && fuelTypeL != "电力") {
                htmlArr.push("			<li>排量：" + displacementL + displacementUnitL + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">排量：暂无</li>");
            }
            if (transmissionL != "") {
                htmlArr.push("			<li>变速箱：" + transmissionL + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">变速箱：暂无</li>");
            }
            if (inhaleTypeL != "" && fuelTypeL != "电力") {
                htmlArr.push("			<li>进气形式：" + inhaleTypeL + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">进气形式：暂无</li>");
            }
            htmlArr.push("		</ul>");
            htmlArr.push("	</div>");
            htmlArr.push("</div>");

            htmlArr.push("<div class=\"yh-cont fr\">");
            if (!isNaN(fuelR) && parseFloat(fuelR) > 0) {
                htmlArr.push("	<div class=\"yh-pic\">");
                htmlArr.push("		<div class=\"line\" style=\"height: " + (90 * fuelR) / 35.6 + "px\"></div>");
            } else {
                htmlArr.push("	<div class=\"yh-pic yh-pic-gray\">");
                htmlArr.push("		<div class=\"line\"></div>");
            }
           
            htmlArr.push("	</div>");
            htmlArr.push("	<div class=\"yh-mid\">");
            htmlArr.push("		<div class=\"youhao\">");
            if (parseFloat(fuelR) > 0) {
                htmlArr.push(parseFloat(fuelR).toFixed(1) + "<span class=\"danwei\">" + fuelUnitR + "</span>");
            } else {
                htmlArr.push("<p class=\"yh-gray\">暂无数据</p>");
            }
            if (!isNaN(fuelL) && !isNaN(fuelR)) {
                if (parseFloat(fuelL) > parseFloat(fuelR)) {
                    htmlArr.push("<span class=\"sheng\">省</span>");
                }
            }
            htmlArr.push("		</div>");
            htmlArr.push("		<div class=\"pinfen\">");

            if (parseFloat(YouHaoR) > 0) {
                htmlArr.push("			<span class=\"tit\">网友评分</span>" + YouHaoR + "<em>分</em>");
            } else {
                htmlArr.push("<p class=\"yh-gray\">暂无评分</p>");
            }


            htmlArr.push("		</div>");
            htmlArr.push("	</div>");
            htmlArr.push("	<div class=\"yh-right\">");
            htmlArr.push("		<ul>");
            if (displacementR != "" && fuelTypeR != "电力") {
                htmlArr.push("			<li>排量：" + displacementR + displacementUnitR + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">排量：暂无</li>");
            }
            if (transmissionR != "") {
                htmlArr.push("			<li>变速箱：" + transmissionR + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">变速箱：暂无</li>");
            }
            if (inhaleTypeR != "" && fuelTypeR != "电力") {
                htmlArr.push("			<li>进气形式：" + inhaleTypeR + "</li>");
            } else {
                htmlArr.push("			<li class=\"gray\">进气形式：暂无</li>");
            }
            htmlArr.push("		</ul>");
            htmlArr.push("	</div>");
            htmlArr.push("</div>");
            $(".youhao-box").html(htmlArr.join(""));
        }
    },

    CreateCaoKongHtml: function(data) {

        var self = this;
        var items = data.items;
        var htmlArr = [];
        var itemArr = [];
        var countL = 0, countR = 0;

        if (typeof koubeiCompareJson != "undefined") {
            for (var i = 0; i < items.length; i++) {
                var tempvalueL = "";
                var tempvalueR = "";
                try {
                    tempvalueL = koubeiCompareJson[0][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                    tempvalueR = koubeiCompareJson[1][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                } catch (e) {

                }

                if (typeof items[i] == "undefined") {
                    continue;
                }
                if (items[i].isShow) {
                    itemArr.push("        <tr>");
                    itemArr.push("        	<td class=\"align-l\">" + items[i].sTitle + "</td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueL) == tempvalueL ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueL) + "</span></td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueR) == tempvalueR ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueR) + "</span></td>");
                    itemArr.push("        </tr>");
                }

                if (items[i].compareType == "ishave") {
                    if (tempvalueL == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueL) > -1)) {
                        self.ScoreL += 1;
                        countL += 1;
                    }
                    if (tempvalueR == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueR) > -1)) {
                        self.ScoreR += 1;
                        countR += 1;
                    }
                } else if (items[i].compareType == "value") {
                    if (!isNaN(tempvalueL) && !isNaN(tempvalueR)) {
                        var valueL = parseFloat(tempvalueL);
                        var valueR = parseFloat(tempvalueR);
                        if (valueL > valueR) {
                            self.ScoreL += 1;
                            countL += 1;
                        } else if (valueL < valueR) {
                            self.ScoreR += 1;
                            countR += 1;
                        } else {
                            self.ScoreL += 1;
                            countL += 1;
                            self.ScoreR += 1;
                            countR += 1;
                        }
                    }
                }
            }
            htmlArr.push("<table cellpadding=\"0\" cellspacing=\"0\">");
            htmlArr.push("	<tbody>");
            htmlArr.push("        <tr>");
            htmlArr.push("            <th>&nbsp;</th>");
            htmlArr.push("            <th><p class=\"tit\">" + ShowNameL + "</p>");
            if (countL > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countL + "项</span>");
            }
            htmlArr.push("            </th>");


            htmlArr.push("            <th><p class=\"tit\">" + ShowNameR + "</p>");
            if (countR > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countR + "项</span>");
            }
            htmlArr.push("            </th>");
            
            htmlArr.push("        </tr>");

            htmlArr.push(itemArr.join(""));

            htmlArr.push("        <tr class=\"txt-blue\">");
            htmlArr.push("        	<td class=\"align-l\">口碑评分</td>");
            htmlArr.push("        	<td>" + (parseFloat(CaoKongL) > 0 ? CaoKongL : "暂无评") + "分</td>");
            htmlArr.push("        	<td>" + (parseFloat(CaoKongR) > 0 ? CaoKongR : "暂无评") + "分</td>");
            htmlArr.push("        </tr>");

            htmlArr.push("	</tbody>");
            htmlArr.push("</table>");
            $("#caokong").html(htmlArr.join(""));
        }
    },
    CreatePeiZhiHtml: function(data) {

        var self = this;
        var items = data.items;
        var htmlArr = [];
        var countL = 0, countR = 0;
        var itemArr = [];
        if (typeof koubeiCompareJson != "undefined") {
            for (var i = 0; i < items.length; i++) {
                var tempvalueL = "";
                var tempvalueR = "";
                try {
                    tempvalueL = koubeiCompareJson[0][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                    tempvalueR = koubeiCompareJson[1][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                } catch (e) {

                }
                if (typeof items[i] == "undefined") {
                    continue;
                }
                if (items[i].isShow) {
                    itemArr.push("        <tr>");
                    itemArr.push("        	<td class=\"align-l\">" + items[i].sTitle + "</td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueL) == tempvalueL ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueL) + "</span></td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueR) == tempvalueR ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueR) + "</span></td>");
                    itemArr.push("        </tr>");
                }

                if (items[i].compareType == "ishave") {
                    if (tempvalueL == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueL) > -1)) {
                        self.ScoreL += 1;
                        countL += 1;
                    }
                    if (tempvalueR == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueR) > -1)) {
                        self.ScoreR += 1;
                        countR += 1;
                    }
                } else if (items[i].compareType == "value") {
                    if (!isNaN(tempvalueL) && !isNaN(tempvalueR)) {
                        var valueL = parseFloat(tempvalueL);
                        var valueR = parseFloat(tempvalueR);
                        if (valueL > valueR) {
                            self.ScoreL += 1;
                            countL += 1;
                        } else if (valueL < valueR) {
                            self.ScoreR += 1;
                            countR += 1;
                        } else {
                            self.ScoreL += 1;
                            countL += 1;
                            self.ScoreR += 1;
                            countR += 1;
                        }
                    }
                }
            }

            htmlArr.push("<table cellpadding=\"0\" cellspacing=\"0\">");
            htmlArr.push("	<tbody>");
            htmlArr.push("        <tr>");
            htmlArr.push("            <th>&nbsp;</th>");


            htmlArr.push("            <th><p class=\"tit\">" + ShowNameL + "</p>");
            if (countL > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countL + "项</span>");
            }
            htmlArr.push("            </th>");
            htmlArr.push("            <th><p class=\"tit\">" + ShowNameR + "</p>");
            if (countR > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countR + "项</span>");
            }
            htmlArr.push("            </th>");
            
            htmlArr.push("        </tr>");

            htmlArr.push(itemArr.join(""));

            htmlArr.push("        <tr class=\"txt-blue\">");
            htmlArr.push("        	<td class=\"align-l\">口碑评分</td>");
            htmlArr.push("        	<td>" + (parseFloat(PeiZhiL) > 0 ? PeiZhiL : "暂无评") + "分</td>");
            htmlArr.push("        	<td>" + (parseFloat(PeiZhiR) > 0 ? PeiZhiR : "暂无评") + "分</td>");
            htmlArr.push("        </tr>");

            htmlArr.push("	</tbody>");
            htmlArr.push("</table>");
            $("#peizhi").html(htmlArr.join(""));
        }
    },
    CreateNeiShiHtml: function(data) {
        var self = this;
        var items = data.items;
        var htmlArr = [];
        var itemArr = [];
        var countL = 0, countR = 0;
        if (typeof koubeiCompareJson != "undefined") {
            for (var i = 0; i < items.length; i++) {
                var tempvalueL = "";
                var tempvalueR = "";
                try {
                    tempvalueL = koubeiCompareJson[0][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                    tempvalueR = koubeiCompareJson[1][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                } catch (e) {

                }
                if (typeof items[i] == "undefined") {
                    continue;
                }
                if (items[i].isShow) {
                    itemArr.push("        <tr>");
                    itemArr.push("        	<td class=\"align-l\">" + items[i].sTitle + "</td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueL) == tempvalueL ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueL) + "</span></td>");
                    itemArr.push("        	<td><span class=\"" + (self.GetValueHtml(tempvalueR) == tempvalueR ? "f14" : "spot") + "\">" + self.GetValueHtml(tempvalueR) + "</span></td>");
                    itemArr.push("        </tr>");
                }

                if (items[i].compareType == "ishave") {
                    if (tempvalueL == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueL) > -1)) {
                        self.ScoreL += 1;
                        countL += 1;
                    }
                    if (tempvalueR == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueR) > -1)) {
                        self.ScoreR += 1;
                        countR += 1;
                    }
                } else if (items[i].compareType == "value") {
                    if (!isNaN(tempvalueL) && !isNaN(tempvalueR)) {
                        var valueL = parseFloat(tempvalueL);
                        var valueR = parseFloat(tempvalueR);
                        if (valueL > valueR) {
                            self.ScoreL += 1;
                            countL += 1;
                        } else if (valueL < valueR) {
                            self.ScoreR += 1;
                            countR += 1;
                        } else {
                            self.ScoreL += 1;
                            countL += 1;
                            self.ScoreR += 1;
                            countR += 1;
                        }
                    }
                }
            }

            htmlArr.push("<table cellpadding=\"0\" cellspacing=\"0\">");
            htmlArr.push("	<tbody>");
            htmlArr.push("        <tr>");
            htmlArr.push("            <th>&nbsp;</th>");

            htmlArr.push("            <th><p class=\"tit\">" + ShowNameL + "</p>");
            if (countL > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countL + "项</span>");
            }
            htmlArr.push("            </th>");
            htmlArr.push("            <th><p class=\"tit\">" + ShowNameR + "</p>");
            if (countR > 0) {
                htmlArr.push("            <span class=\"gray\">共" + countR + "项</span>");
            }
            htmlArr.push("            </th>");
            
            htmlArr.push("        </tr>");

            htmlArr.push(itemArr.join(""));

            htmlArr.push("        <tr class=\"txt-blue\">");
            htmlArr.push("        	<td class=\"align-l\">口碑评分</td>");
            htmlArr.push("        	<td>" + (parseFloat(NeiShiL) > 0 ? NeiShiL : "暂无评") + "分</td>");
            htmlArr.push("        	<td>" + (parseFloat(NeiShiR) > 0 ? NeiShiR : "暂无评") + "分</td>");
            htmlArr.push("        </tr>");

            htmlArr.push("	</tbody>");
            htmlArr.push("</table>");
            $("#neishi").html(htmlArr.join(""));
        }
    },
    CreateDongLiHtml: function(data) {
        var items = data.items;
        var itemArr = [];
        var countL = 0, countR = 0;
        if (typeof koubeiCompareJson != "undefined") {
            for (var i = 0; i < items.length; i++) {
                var tempvalueL = "";
                var tempvalueR = "";
                try {
                    tempvalueL = koubeiCompareJson[0][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                    tempvalueR = koubeiCompareJson[1][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                } catch (e) {

                }
                if (typeof items[i] == "undefined") {
                    continue;
                }
                if (items[i].isShow) {
                    tempvalueL = (tempvalueL == "" || tempvalueL == "无" || tempvalueL == "待查") ? "无数据" : tempvalueL;
                    tempvalueR = (tempvalueR == "" || tempvalueR == "无" || tempvalueR == "待查") ? "无数据" : tempvalueR;

                    itemArr.push("		<p class=\"tit\">" + items[i].sTitle + "(" + items[i].unit + ")" + "</p>");
                    itemArr.push("		<div class=\"chart\">");
                    if (tempvalueL == "无数据" && tempvalueR == "无数据") {
                        itemArr.push("			<div class=\"none-bg\"><span></span></div>");
                        itemArr.push("			<div class=\"none-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"none-head right\">" + tempvalueR + "</div>");
                    } else if (tempvalueL != "无数据" && tempvalueR == "无数据") {
                        itemArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                        itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"none-head right\">" + tempvalueR + "</div>");
                    } else if (tempvalueL == "无数据" && tempvalueR != "无数据") {
                        itemArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                        itemArr.push("			<div class=\"none-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                    } else {
                        if (parseFloat(tempvalueL) > parseFloat(tempvalueR)) {
                            if (items[i].sTitle == "官方0-100公里加速时间") {
                                itemArr.push("			<div class=\"win-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                                itemArr.push("			<div class=\"gray-head left\">" + tempvalueL + "</div>");
                                itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                            } else {
                                itemArr.push("			<div class=\"fail-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                                itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                                itemArr.push("			<div class=\"gray-head right\">" + tempvalueR + "</div>");
                            }
                        } else if (parseFloat(tempvalueL) == parseFloat(tempvalueR)) {
                            itemArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                            itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                            itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                        } else {
                            if (items[i].sTitle == "官方0-100公里加速时间") {
                                itemArr.push("			<div class=\"fail-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                                itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                                itemArr.push("			<div class=\"gray-head right\">" + tempvalueR + "</div>");
                            } else {
                                itemArr.push("			<div class=\"win-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                                itemArr.push("			<div class=\"gray-head left\">" + tempvalueL + "</div>");
                                itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                            }
                        }
                    }
                    itemArr.push("		</div>");
                }

                if (items[i].compareType == "ishave") {
                    if (tempvalueL == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueL) > -1)) {
                        self.ScoreL += 1;
                        countL += 1;
                    }
                    if (tempvalueR == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueR) > -1)) {
                        self.ScoreR += 1;
                        countR += 1;
                    }
                } else if (items[i].compareType == "value") {
                    if (!isNaN(tempvalueL) && !isNaN(tempvalueR)) {
                        var valueL = parseFloat(tempvalueL);
                        var valueR = parseFloat(tempvalueR);
                        if (items[i].sTitle == "官方0-100公里加速时间") {
                            if (valueL > valueR) {
                                self.ScoreR += 1;
                                countR += 1;
                            } else if (valueL < valueR) {
                                self.ScoreL += 1;
                                countL += 1;
                            } else {
                                self.ScoreL += 1;
                                countL += 1;
                                self.ScoreR += 1;
                                countR += 1;
                            }
                        } else {
                            if (valueL > valueR) {
                                self.ScoreL += 1;
                                countL += 1;
                            } else if (valueL < valueR) {
                                self.ScoreR += 1;
                                countR += 1;
                            } else {
                                self.ScoreL += 1;
                                countL += 1;
                                self.ScoreR += 1;
                                countR += 1;
                            }
                        }
                    }
                }
            }

            var htmlArr = [];
            if (countL != countR) {
                htmlArr.push("<div class=\"txt-box " + (countL > countR ? "" : "txt-r") + "\">");
                htmlArr.push("	<span>强</span>");
                htmlArr.push("</div>");
            }
            htmlArr.push("<div class=\"ctn-box\">");
            htmlArr.push(itemArr.join(""));
            htmlArr.push("		<p class=\"tit\">口碑评分</p>");
            htmlArr.push("		<div class=\"chart\">");
            if (parseFloat(DongLiL) > 0 && parseFloat(DongLiR) > 0) {
                if (parseFloat(DongLiL) > parseFloat(DongLiR)) {
                    htmlArr.push("			<div class=\"fail-bg\"><span style=\"width:" + (parseInt(DongLiL) / (parseInt(DongLiL) + parseInt(DongLiR))) * 100 + "%\"></span></div>");
                    htmlArr.push("			<div class=\"blue-head left\">" + (parseFloat(DongLiL) > 0 ? DongLiL : 0) + "</div><div class=\"gray-head right\">" + (parseFloat(DongLiR) > 0 ? DongLiR : 0) + "</div>");
                } else if (parseFloat(DongLiL) == parseFloat(DongLiR)) {
                    htmlArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                    htmlArr.push("			<div class=\"blue-head left\">" + (parseFloat(DongLiL) > 0 ? DongLiL : 0) + "</div><div class=\"blue-head right\">" + (parseFloat(DongLiR) > 0 ? DongLiR : 0) + "</div>");
                } else {
                    htmlArr.push("			<div class=\"win-bg\"><span style=\"width:" + (parseInt(DongLiL) / (parseInt(DongLiL) + parseInt(DongLiR))) * 100 + "%\"></span></div>");
                    htmlArr.push("			<div class=\"gray-head left\">" + (parseFloat(DongLiL) > 0 ? DongLiL : 0) + "</div><div class=\"blue-head right\">" + (parseFloat(DongLiR) > 0 ? DongLiR : 0) + "</div>");
                }
            } else {
                htmlArr.push("			<div class=\"none-bg\"><span></span></div>");
                htmlArr.push("			<div class=\"none-head left\">无数据</div>");
                htmlArr.push("			<div class=\"none-head right\">无数据</div>");
            }
            htmlArr.push("		</div>");
            htmlArr.push("</div>");
            $("#dongli").html(htmlArr.join(""));
        }
    },
    CreateWaiGuanHtml: function(data) {
        var self = this;
        var items = data.items;
        var itemArr = [];
        var countL = 0, countR = 0;
        if (typeof koubeiCompareJson != "undefined") {
            for (var i = 0; i < items.length; i++) {
                //var tempvalueL = koubeiCompareJson[0][items[i].sTrPrefix][items[i].sFieldIndex];
                //var tempvalueR = koubeiCompareJson[1][items[i].sTrPrefix][items[i].sFieldIndex];
                var tempvalueL = "";
                var tempvalueR = "";
                try {
                    tempvalueL = koubeiCompareJson[0][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                    tempvalueR = koubeiCompareJson[1][parseInt(items[i].sTrPrefix)][parseInt(items[i].sFieldIndex)];
                } catch (e) {

                }
                if (typeof items[i] == "undefined") {
                    continue;
                }
                if (items[i].isShow) {
                    tempvalueL = (tempvalueL == "" || tempvalueL == "无" || tempvalueL == "待查") ? "无数据" : tempvalueL;
                    tempvalueR = (tempvalueR == "" || tempvalueR == "无" || tempvalueR == "待查") ? "无数据" : tempvalueR;

                    itemArr.push("		<p class=\"tit\">" + items[i].sTitle + "(" + items[i].unit + ")" + "</p>");
                    itemArr.push("		<div class=\"chart\">");
                    if (tempvalueL == "无数据" && tempvalueR == "无数据") {
                        itemArr.push("			<div class=\"none-bg\"><span></span></div>");
                        itemArr.push("			<div class=\"none-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"none-head right\">" + tempvalueR + "</div>");
                    } else if (tempvalueL != "无数据" && tempvalueR == "无数据") {
                        itemArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                        itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"none-head right\">" + tempvalueR + "</div>");
                    } else if (tempvalueL == "无数据" && tempvalueR != "无数据") {
                        itemArr.push("			<div class=\"win-bg\"><span></span></div>");
                        itemArr.push("			<div class=\"none-head left\">" + tempvalueL + "</div>");
                        itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                    } else {
                        if (parseInt(tempvalueL) > parseInt(tempvalueR)) {
                            itemArr.push("			<div class=\"fail-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                            itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                            itemArr.push("			<div class=\"gray-head right\">" + tempvalueR + "</div>");
                        }
                        else if (parseInt(tempvalueL) == parseInt(tempvalueR))
                        {
                            itemArr.push("			<div class=\"win-bg\"><span></span></div>");
                            itemArr.push("			<div class=\"blue-head left\">" + tempvalueL + "</div>");
                            itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                        }
                        else {
                            itemArr.push("			<div class=\"win-bg\"><span style=\"width:" + (parseInt(tempvalueL) / (parseInt(tempvalueL) + parseInt(tempvalueR))) * 100 + "%\"></span></div>");
                            itemArr.push("			<div class=\"gray-head left\">" + tempvalueL + "</div>");
                            itemArr.push("			<div class=\"blue-head right\">" + tempvalueR + "</div>");
                        }
                    }
                    itemArr.push("		</div>");
                }

                if (items[i].compareType == "ishave") {
                    if (tempvalueL == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueL) > -1)) {
                        self.ScoreL += 1;
                        countL += 1;
                    }
                    if (tempvalueR == "有" || (arrValue[items[i].sPid] != undefined && arrValue[items[i].sPid].indexOf(tempvalueR) > -1)) {
                        self.ScoreR += 1;
                        countR += 1;
                    }
                } else if (items[i].compareType == "value") {
                    if (!isNaN(tempvalueL) && !isNaN(tempvalueR)) {
                        var valueL = parseFloat(tempvalueL);
                        var valueR = parseFloat(tempvalueR);
                        if (valueL > valueR) {
                            self.ScoreL += 1;
                            countL += 1;
                        } else if (valueL < valueR) {
                            self.ScoreR += 1;
                            countR += 1;
                        } else {
                            self.ScoreL += 1;
                            countL += 1;
                            self.ScoreR += 1;
                            countR += 1;
                        }
                    }
                }
            }
            var htmlArr = [];
            if (countL != countR) {
                htmlArr.push("<div class=\"txt-box " + (countL > countR ? "" : "txt-r") + "\">");
                htmlArr.push("	<span>大</span>");
                htmlArr.push("</div>");
            }
            htmlArr.push("<div class=\"ctn-box\">");
            htmlArr.push(itemArr.join(""));
            htmlArr.push("		<p class=\"tit\">口碑评分</p>");
            htmlArr.push("		<div class=\"chart\">");

            if (parseFloat(WaiGuanL) > 0 && parseFloat(WaiGuanR) > 0) {
                if (parseFloat(WaiGuanL) > parseFloat(WaiGuanR)) {
                    htmlArr.push("			<div class=\"fail-bg\"><span style=\"width:" + (parseInt(WaiGuanL) / (parseInt(WaiGuanL) + parseInt(WaiGuanR))) * 100 + "%\"></span></div>");
                    htmlArr.push("			<div class=\"blue-head left\">" + (parseFloat(WaiGuanL) > 0 ? WaiGuanL : 0) + "</div><div class=\"gray-head right\">" + (parseFloat(WaiGuanR) > 0 ? WaiGuanR : 0) + "</div>");
                } else if (parseFloat(WaiGuanL) == parseFloat(WaiGuanR)) {
                    htmlArr.push("			<div class=\"fail-bg\"><span style=\"width:100%\"></span></div>");
                    htmlArr.push("			<div class=\"blue-head left\">" + (parseFloat(WaiGuanL) > 0 ? WaiGuanL : 0) + "</div><div class=\"blue-head right\">" + (parseFloat(WaiGuanR) > 0 ? WaiGuanR : 0) + "</div>");
                } else {
                    htmlArr.push("			<div class=\"win-bg\"><span style=\"width:" + (parseInt(WaiGuanL) / (parseInt(WaiGuanL) + parseInt(WaiGuanR))) * 100 + "%\"></span></div>");
                    htmlArr.push("			<div class=\"gray-head left\">" + (parseFloat(WaiGuanL) > 0 ? WaiGuanL : 0) + "</div><div class=\"blue-head right\">" + (parseFloat(WaiGuanR) > 0 ? WaiGuanR : 0) + "</div>");
                }
            } else {
                htmlArr.push("			<div class=\"none-bg\"><span></span></div>");
                htmlArr.push("			<div class=\"none-head left\">无数据</div>");
                htmlArr.push("			<div class=\"none-head right\">无数据</div>");
            }
            htmlArr.push("		</div>");
            htmlArr.push("</div>");
            $("#waiguan").html(htmlArr.join(""));
        }},
    GetValueHtml: function(value) {
        var valueHtml = value;
        if (value == "有") {
            valueHtml = "●";
        } else if (value == "选配") {
            valueHtml = "○";
        } else if (value == "无") {
            valueHtml = "-";
        } else if (value == "") {
            valueHtml = "-";
        }
        return valueHtml;
    },
    SetRecommendation: function() {
        var self = this;
        if (parseInt(carIdL) > 0 && parseInt(carIdR) > 0) {
            if (self.ScoreL >= self.ScoreR) {
                $("#recommendationL").show();
                $("#recommendation_flow_L").show();
            } else {
                $("#recommendationR").show();
                $("#recommendation_flow_R").show();
            }
        }
    }

};
var arrValue = {
    "685": ["EBD", "CBC", "EBV"],
    "684": ["EBA", "BAS", "BA", "EVA"],
    "698": ["ASR", "TCS", "TRC", "ATC"],
    "700": ["ESP", "DSC", "VSC", "ESC"],
    "552": ["手动", "电动"],
    "467": ["12V", "220V"]
};
var arrField = [
    {
        "title": "车身尺寸",
        "unit": "大",
        "moreTitle": "更多空间差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { "sType": "para", "sTitle": "长", "sPid": 588, "sTrPrefix": 2, "sFieldIndex": 0, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "宽", "sPid": 593, "sTrPrefix": 2, "sFieldIndex": 1, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "高", "sPid": 586, "sTrPrefix": 2, "sFieldIndex": 2, "unit": "mm", "isShow": true, "compareType": "value" },
            { "sType": "para", "sTitle": "轴距", "sPid": 592, "sTrPrefix": 2, "sFieldIndex": 3, "unit": "mm", "isShow": true, "compareType": "value" }
        ]
    },
    {
        "title": "性能动力",
        "unit": "强",
        "moreTitle": "更多动力差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { sType: "para", sTitle: "整备质量", sPid: "669", sTrPrefix: "2", sFieldIndex: "6", unit: "kg", isShow: true, compareType: "value" },
            { sType: "para", sTitle: "官方0-100公里加速时间", sPid: "650", sTrPrefix: "1", sFieldIndex: "13", unit: "s", isShow: true, compareType: "value" },
            { sType: "para", sTitle: "最大马力", sPid: "791", sTrPrefix: "3", sFieldIndex: "13", unit: "Ps", isShow: true, compareType: "value" },
            { sType: "para", sTitle: "最大扭矩", sPid: "429", sTrPrefix: "3", sFieldIndex: "16", unit: "Nm", isShow: true, compareType: "value" }
        ]
    },
    {
        "title": "油耗",
        "unit": "省",
        "moreTitle": "更多油耗差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": false,
        "items": [
            { sType: "para", sTitle: "综合工况油耗", sPid: "782", sTrPrefix: "1", sFieldIndex: "10", unit: "L/100km", isShow: true, "compareType": "value" },
            { sType: "para", sTitle: "排量", sPid: "785", sTrPrefix: "1", sFieldIndex: "5", unit: "L", isShow: true },
            { sType: "matilpara", sTitle: "变速箱", sPid: "724,712", sTrPrefix: "1,1", sFieldIndex: "6,7", unit: "挡 ,", isShow: true },
            { sType: "matilpara", sTitle: "进气形式", sPid: "425,408", sTrPrefix: "3,3", sFieldIndex: "4,5", unit: " ,", isShow: true }
        ]
    },
    {
        "title": "安全配置",
        "unit": "项",
        "moreTitle": "更多安全配置差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "儿童锁", sPid: "494", sTrPrefix: "6", sFieldIndex: "21", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "前排侧安全气囊", sPid: "691", sTrPrefix: "6", sFieldIndex: "2", unit: "", isShow: true },
            { sType: "para", sTitle: "前排头部气囊(气帘)", sPid: "690", sTrPrefix: "6", sFieldIndex: "3", unit: "", isShow: true },
            { sType: "para", sTitle: "驾驶位安全气囊", sPid: "682", sTrPrefix: "6", sFieldIndex: "0", unit: "", isShow: true },
            { sType: "para", sTitle: "胎压监测装置", sPid: "714", sTrPrefix: "6", sFieldIndex: "15", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "零压续行(零胎压继续行驶)", sPid: "715", sTrPrefix: "6", sFieldIndex: "16", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "中控门锁", sPid: "493", sTrPrefix: "6", sFieldIndex: "20", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "遥控钥匙", sPid: "538", sTrPrefix: "6", sFieldIndex: "22", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "无钥匙进入系统", sPid: "952", sTrPrefix: "6", sFieldIndex: "26", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "无钥匙启动系统", sPid: "469", sTrPrefix: "6", sFieldIndex: "23", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "发动机防盗系统", sPid: "683", sTrPrefix: "6", sFieldIndex: "25", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "电子防盗系统", sPid: "699", sTrPrefix: "6", sFieldIndex: "24", unit: "", isShow: false, "compareType": "ishave" },
        ]
    },
    {
        "title": "行车辅助",
        "unit": "项",
        "moreTitle": "更多行车辅助差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "自动驻车", sPid: "811", sTrPrefix: "8", sFieldIndex: "6", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "倒车雷达（车后）", sPid: "702", sTrPrefix: "8", sFieldIndex: "10", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "自动泊车入位", sPid: "816", sTrPrefix: "8", sFieldIndex: "17", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "定速巡航", sPid: "545", sTrPrefix: "8", sFieldIndex: "13", unit: "", isShow: true, "compareType": "ishave" },
            { sType: "para", sTitle: "刹车防抱死（ABS）", sPid: "673", sTrPrefix: "8", sFieldIndex: "0", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "电子制动力分配系统", sPid: "685", sTrPrefix: "8", sFieldIndex: "1", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "紧急制动辅助系统", sPid: "684", sTrPrefix: "8", sFieldIndex: "2", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "牵引力控制", sPid: "698", sTrPrefix: "8", sFieldIndex: "3", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "动态稳定控制系统", sPid: "700", sTrPrefix: "8", sFieldIndex: "4", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "随速助力转向调节(EPS)", sPid: "732", sTrPrefix: "8", sFieldIndex: "5", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "上坡辅助", sPid: "812", sTrPrefix: "8", sFieldIndex: "7", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "陡坡缓降", sPid: "813", sTrPrefix: "8", sFieldIndex: "8", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "泊车雷达（车前）", sPid: "800", sTrPrefix: "8", sFieldIndex: "9", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "倒车影像", sPid: "703", sTrPrefix: "8", sFieldIndex: "11", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "全景摄像头", sPid: "820", sTrPrefix: "8", sFieldIndex: "12", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "自适应巡航", sPid: "893", sTrPrefix: "8", sFieldIndex: "14", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "GPS导航系统", sPid: "516", sTrPrefix: "8", sFieldIndex: "15", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "车道偏离预警系统", sPid: "955", sTrPrefix: "8", sFieldIndex: "25", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "主动刹车/主动安全系统", sPid: "818", sTrPrefix: "8", sFieldIndex: "19", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "整体主动转向系统", sPid: "841", sTrPrefix: "8", sFieldIndex: "20", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "夜视系统", sPid: "819", sTrPrefix: "8", sFieldIndex: "21", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "盲点检测/并线辅助", sPid: "898", sTrPrefix: "8", sFieldIndex: "22", unit: "", isShow: false, "compareType": "ishave" }
        ]
    },
    {
        "title": "内部配置",
        "unit": "项",
        "moreTitle": "更多内部配置差异>>",
        "moreUrl": "http://car.m.yiche.com/chexingduibi/?carids=",
        "isShowDiff": true,
        "items": [
            { sType: "para", sTitle: "天窗型式", sPid: "567", sTrPrefix: "9", sFieldIndex: "5", unit: "", isShow: true },
            { sType: "para", sTitle: "前大灯类型", sPid: "614", sTrPrefix: "10", sFieldIndex: "0", unit: "", isShow: true },
            { sType: "para", sTitle: "中控台液晶屏", sPid: "488", sTrPrefix: "13", sFieldIndex: "12", unit: "", isShow: true },
            { sType: "para", sTitle: "座椅材料", sPid: "544", sTrPrefix: "12", sFieldIndex: "1", unit: "", isShow: true },
            { sType: "para", sTitle: "方向盘前后调节", sPid: "799", sTrPrefix: "11", sFieldIndex: "0", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘上下调节", sPid: "798", sTrPrefix: "11", sFieldIndex: "1", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘调节方式", sPid: "552", sTrPrefix: "11", sFieldIndex: "2", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘记忆设置", sPid: "549", sTrPrefix: "11", sFieldIndex: "3", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "多功能方向盘", sPid: "528", sTrPrefix: "11", sFieldIndex: "5", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "方向盘加热", sPid: "956", sTrPrefix: "11", sFieldIndex: "12", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "全液晶仪表盘", sPid: "988", sTrPrefix: "11", sFieldIndex: "13", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "行车电脑显示屏", sPid: "832", sTrPrefix: "11", sFieldIndex: "7", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "HUD抬头数字显示", sPid: "518", sTrPrefix: "11", sFieldIndex: "8", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "后排杯架", sPid: "474", sTrPrefix: "11", sFieldIndex: "10", unit: "", isShow: false, "compareType": "ishave" },
            { sType: "para", sTitle: "车内电源电压", sPid: "467", sTrPrefix: "11", sFieldIndex: "11", unit: "", isShow: false, "compareType": "ishave" }
        ]
    }
];