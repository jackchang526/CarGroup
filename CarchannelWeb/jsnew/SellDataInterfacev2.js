var scriptUrl = "http://image.bitautoimg.com/carchannel/jsnew/DataControlShow.js";
var loadJS = {
    lock: false, ranks: []
	, callback: function(startTime, callback) {
	    //载入完成
	    this.lock = false;
	    callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
	    this.read(); //解锁，在次载入
	}
	, read: function() {
	    //读取
	    if (!this.lock && this.ranks.length) {
	        var head = document.getElementsByTagName("head")[0];

	        if (!head) {
	            ranks.length = 0, ranks = null;
	            throw new Error('HEAD不存在');
	        }

	        var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

	        this.lock = true;

	        script.onload = script.onreadystatechange = function() {
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
	, push: function(src, charset, callback) {
	    //加入队列
	    this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
	    this.read();
	}
}

function BrandObj(id,name)
{
	this.Id = id;
	this.Name = name;
}

function SortBrand(b1,b2)
{
	if(b1.Name == b2.Name)
		return 0;
	else if(b1.Name > b2.Name)
		return 1;
	else
		return -1;
}

//Flash图表对象
function BAFlashChart()
{
    this.DataUrl = "http://api.car.bitauto.com/CarInfo/GetCarSellData.aspx?returnType=json"; 	//获取数据的地址
	this.BrandTree = null;		//厂商，品牌，子品牌级别树
	this.DdlProducer = null;	//厂商下拉列表
	this.DdlBrand = null;		//品牌下拉列表
	this.DdlSerial = null;		//子品牌下拉列表
	this.BtnQuery = null;		//查询按钮
	this.DefaultSerialId = 0;	//默认子品牌ID
	this.DefaultSerialName = ""; //默认子品牌名称
	this.Encoding = "";  //取数据编码
	this.Self = false;
	this.width = 0;
	this.height = 0;
	
	//输出框架代码
	this.GenterateSellQuery = function() {
	    loadJS.push("http://js.bitauto.com/dealer/Report/FlashChart/fusioncharts.js", "utf-8", null);
		document.write("<div class=\"col-all\">");
		document.write("<div class=\"line_box\">");
		if(this.Self)
			document.write("<h3><span>汽车销量查询</span></h3>");
		else
			document.write("<h3><span>汽车销量查询</span></h3>");
		document.write("<div class=\"col-con\">");
		document.write("<div class=\"search_sales\">");
		document.write("<span>选择企业：<select id=\"ddlProducer\" style=\"width:100px\"></select></span>");
		document.write("<span>选择品牌：<select id=\"ddlBrand\" style=\"width:100px\"></select></span>");
		document.write("<span>选择车型：<select id=\"ddlSerial\"style=\"width:100px\"></select></span>");
		document.write("<span><input id=\"btnQuery\" type=\"button\" value=\"查询\" class=\"btn\" /></span>");
		document.write("</div>");
		document.write("<h3 id=\"chartTitle\" class=\"ttl\">前六个月销量走势图</h3>");
		document.write("<div id=\"flashChart\" class=\"area_qxt\">");
		document.write("</div></div>");
		document.write("<div class=\"col-side\">");
		document.write("<div id=\"sellNews\" class=\"ranking_list line_box_alen\">");
		document.write("</div>");
		document.write("</div>");
		document.write("<div class=\"clear\"></div>");
		// ad May.31.2013
		document.write("<div class=\"more\"><ins id=\"div_542d453b-b855-4c98-ae2a-34384305dfa3\" type=\"ad_play\" adplay_IP=\"\" adplay_AreaName=\"\"  adplay_CityName=\"\"    adplay_BrandID=\"" + (typeof (serialId) != 'undefined' ? serialId : "") + "\"  adplay_BrandName=\"\"  adplay_BrandType=\"\"  adplay_BlockCode=\"542d453b-b855-4c98-ae2a-34384305dfa3\"> </ins></div>");
		document.write("</div>");
		document.write("</div>");
		document.write("");
		
		//初始化数据
		this.InitProducerBrandSerial();	
	}
	
	//初始化对象
	this.InitProducerBrandSerial = function () {
	    this.DdlProducer = document.getElementById("ddlProducer");
	    this.DdlBrand = document.getElementById("ddlBrand");
	    this.DdlSerial = document.getElementById("ddlSerial");
	    this.BtnQuery = document.getElementById("btnQuery");
	    this.SellNews = document.getElementById("sellNews");
	    this.ChartTitle = document.getElementById("chartTitle");
	    var xmlUrl = this.DataUrl + "&dataType=brandtree";
	    var thisObj = this;
	    loadJS.push(xmlUrl, "utf-8", function () {
	        thisObj.BrandTree = brandtreeJson;
	        thisObj.InitList(thisObj.DdlProducer, "producer", brandtreeJson);
	        thisObj.InitList(thisObj.DdlBrand, "brand", brandtreeJson[0].Brands);
	        thisObj.InitList(thisObj.DdlSerial, "", brandtreeJson[0].Brands[0].Serials);

	        //添加事件
	        thisObj.AddEvent();
	        thisObj.BtnQuery.click();
	    });
	}
	//装载选择列表
	this.InitList = function (ddlTag, brandLevel, nodes) {
	    ddlTag.options.length = 0;
	    if (brandLevel == "producer")
	        ddlTag.options.add(new Option("请选择厂商", "0"));
	    else if (brandLevel == "brand")
	        ddlTag.options.add(new Option("请选择品牌", "0"));
	    else
	        ddlTag.options.add(new Option("请选择车型", "0"));

	    var prdArray = new Array();
	    for (var i = 0; i < nodes.length; i++) {
	        prdArray.push(new BrandObj(nodes[i].id, nodes[i].name));
	    }
	    prdArray.sort(SortBrand);
	    for (var j = 0; j < prdArray.length; j++) {
	        ddlTag.options.add(new Option(prdArray[j].Name, prdArray[j].Id));
	    }
	}
	//挂接事件
	this.AddEvent = function()
	{
		this.DdlProducer.onchange = this.ProducerChanged;
		this.DdlProducer.tag = this;
		this.DdlBrand.onchange = this.BrandChanged;
		this.DdlBrand.tag = this;
		this.BtnQuery.onclick = this.GoQuery;
		this.BtnQuery.tag = this;
	}
	
	//厂商变更
	this.ProducerChanged = function () {
	    var pId = this.tag.DdlProducer.options[this.tag.DdlProducer.selectedIndex].value;
	    if (pId != "0") {
	        for (var i = 0; i < this.tag.BrandTree.length; i++) {
	            if (this.tag.BrandTree[i].id == pId) {
	                this.tag.InitList(this.tag.DdlBrand, "brand", this.tag.BrandTree[i].Brands);
	                break;
	            }
	        }
	    }
	    this.tag.DdlSerial.options.length = 1;
	}
	//品牌变更
	this.BrandChanged = function () {
	    var bId = this.tag.DdlBrand.options[this.tag.DdlBrand.selectedIndex].value;
	    if (bId != "0") {
	        for (var i = 0; i < this.tag.BrandTree.length; i++) {
	            for (var j = 0; j < this.tag.BrandTree[i].Brands.length; j++) {
	                if (this.tag.BrandTree[i].Brands[j].id == bId) {
	                    this.tag.InitList(this.tag.DdlSerial, "serial", this.tag.BrandTree[i].Brands[j].Serials);
	                    break;
	                }
	            }
	        }
	    }
	}
	//开始查询
	this.GoQuery = function () {
	    var flashObj = this.tag;
	    var producerId = !flashObj.DdlProducer ? 0 : flashObj.DdlProducer.options[flashObj.DdlProducer.selectedIndex].value;
	    var brandId = !flashObj.DdlBrand ? 0 : flashObj.DdlBrand.options[flashObj.DdlBrand.selectedIndex].value;
	    var serialId = !flashObj.DdlSerial ? 0 : flashObj.DdlSerial.options[flashObj.DdlSerial.selectedIndex].value;
        if(isNaN(producerId))
            producerId =  0;
        if(isNaN(brandId))
            brandId = 0;
        if(isNaN(serialId))
	            serialId = 0;
	    var params = "";
	    var name = "";
	    if (flashObj.DefaultSerialId != 0) {
	        params = "&sId=" + flashObj.DefaultSerialId;
	        name = flashObj.DefaultSerialName;
	    }
	    if (serialId != "0") {
	        params = "&sId=" + serialId;
	        name = flashObj.DdlSerial.options[flashObj.DdlSerial.selectedIndex].text;
	    }
	    else if (brandId != "0") {
	        params = "&bId=" + brandId;
	        name = flashObj.DdlBrand.options[flashObj.DdlBrand.selectedIndex].text;
	    }
	    else if (producerId != "0") {
	        params = "&pId=" + producerId;
	        name = flashObj.DdlProducer.options[flashObj.DdlProducer.selectedIndex].text;
	        name = name.substring(2, name.length);
	    }

	    //获取数据
	    loadJS.push(flashObj.DataUrl + "&datatype=query" + params, "utf-8", function () {
	        //queryJson 
	        var flashEle = document.getElementById("flashChart");
	        if (flashObj.width < 1) flashObj.width = 680;
	        if (flashObj.height < 1) flashObj.height = 160
	        flashObj.InsertMultiFlashChart(flashEle, queryJson.SellDatas, flashObj.width, flashObj.height, name);
	        if (name == "" && flashObj.ChartTitle)
	            flashObj.ChartTitle.innerHTML = "国产乘用车近六个月销量走势图";
	        else if (flashObj.ChartTitle)
	            flashObj.ChartTitle.innerHTML = name + "近六个月销量走势图";

	        //更新新闻
	        flashObj.UpdateNews(flashObj.SellNews, queryJson.listNews, name);
	    });
	}
	
	//插入Flash图表
	this.InsertFlashChart = function(element,chartData, width,height,pointName)
	{

		var flashStr= '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"'+
					'width="100%" height="'+height+'">'+
					'<param name="movie" value="http://car.bitauto.com/flash/SellDataChart.swf" />'+
					'<param name="quality" value="high" />'+
					'<param name="wmode" value="transparent">'+
					'<param name="FlashVars" value="datavalue='+chartData+'&counvalue='+pointName+'&FlashRight='+width+'&FlashLeft=15" />'+
					'<embed src="http://car.bitauto.com/flash/SellDataChart.swf" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer"'+
						'type="application/x-shockwave-flash" width="'+width+'" height="'+height+'" flashvars="datavalue='+chartData+'&counvalue='+pointName+':&FlashRight='+(width-10)+'&FlashLeft=15"></embed>'+
				'</object>';
		 element.innerHTML=flashStr;

    }

    this.InsertMultiFlashChart = function(element, chartDatas, width, height, pointName) {
        var pro = this;
        loadJS.push(scriptUrl, "utf-8", function() { pro.ShowMultiLine(element, chartDatas, width, height, pointName); });
    }
    //拼接多线的数据
    this.ShowMultiLine = function (element, chartDatas, width, height, pointName) {
        var lineShow = new LineShow();
        lineShow.width = width;
        lineShow.height = height;
        lineShow.pageElementId = element.id;
        if (chartDatas == null || chartDatas.length < 1) return;
        var xAxisValues = [];
        var data = [];
        //初始化X轴数据, 初始化线数据
        for (var i = 0; i < chartDatas.length; i++) {
            xAxisValues[i] = chartDatas[i].d;
            data[i] = chartDatas[i].s;
        }
        lineShow.xAxisValue = xAxisValues;
        var dataObj = new Object();
        dataObj["seriesName"] = pointName;
        dataObj["lineThickness"] = 3;
        dataObj["showValues"] = 1;
        dataObj["color"] = "4E82BF";
        dataObj["data"] = data;
        lineShow.dataValues.push(dataObj);
        //显示控件
        lineShow.showControl();
    }

    this.UpdateNews = function(ele, newsNodes, name) {
        if (!ele) return;
        var newsHtml = new Array();
        newsHtml.push("<h3>" + name + "资讯</h3>");
        newsHtml.push("<ul class=\"list\">");

        if (newsNodes != null && newsNodes != undefined) {
            for (var i = 0; i < newsNodes.length; i++) {
                newsHtml.push("<li><a target=\"_blank\" href=\"" + newsNodes[i].filepath + "\">" + newsNodes[i].facetitle + "</a></li>");
            }
        }
        newsHtml.push("</ul>");
        newsHtml.push("<div class=\"more\"></div>");
        ele.innerHTML = newsHtml.join("");
    }

    //===============销量地图=====================
    this.InitSellDataMapFrame = function () {
        var thisObj = this;
        document.write('<div class="col-all" id="car_DataMap"></div>');
        loadJS.push(this.DataUrl + "&dataType=datamap", "utf-8", function () {
            thisObj.GenerateSellDataMap("");
        });
    }
    this.GenerateSellDataMapOnClick = function (e) {
        this.tag.GenerateSellDataMap(this.getAttribute("month"));
    }
    this.GenerateSellDataMap = function (hisMonthStr) {
        if (!datamapJson || datamapJson.length <= 0)
            return;
        var monthNode = null;
        if (hisMonthStr.length == 0) {
            monthNode = datamapJson[0];
            hisMonthStr = monthNode.Value;
        }
        else {
            for (var i = 0; i < datamapJson.length;i++ ) {
                if (datamapJson[i].Value == hisMonthStr) {
                    monthNode = datamapJson[i];
                    break;
                }
            }
        }

        if (monthNode == null)
            return;

        var dateSegments = hisMonthStr.split('-');
        var tempDate = new Date(dateSegments[0], dateSegments[1] - 1, 0);

        var htmlBuilder = new Array();
        htmlBuilder.push("<div class=\"line_box\">");
        htmlBuilder.push("<h3><span>车型上牌量表现地图</span></h3>");
        htmlBuilder.push(this.GenerateDataMpaNavBar(hisMonthStr));
        htmlBuilder.push("<div class=\"rank_sales\">");
        htmlBuilder.push("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"ka_table1\">");
        var levelList = new Array("微型车", "小型车", "紧凑型", "中型车", "中大型", "豪华车", "跑车", "SUV", "MPV");
        for (i = 0; i < levelList.length; i++) {
            var levelName = levelList[i];
            if (levelName == "紧凑型" || levelName == "中大型")
                levelName += "车";
            var serialNodeList = null;
            for (var levelIndex = 0; levelIndex < monthNode.Levels.length; levelIndex++) {
                if (monthNode.Levels[levelIndex].level == levelList[i]) {
                    serialNodeList = monthNode.Levels[levelIndex].Serials;
                    break;
                }
            }
            if (!serialNodeList || serialNodeList.length <= 0)
                continue;
            htmlBuilder.push("<tr>");
            htmlBuilder.push("<th width=\"12%\">" + levelName + "</th>");
            htmlBuilder.push("<td width=\"88%\">");
            for (j = 0; j < serialNodeList.length; j++) {
                var serialNode = serialNodeList[j];
                var serialName = serialNode.SerialName;
                var serialSpell = serialNode.allSpell;
                htmlBuilder.push("<span><a href=\"http://car.bitauto.com/" + serialSpell + "/xiaoliangditu/" + hisMonthStr + "/\" target=\"_blank\">" + serialName + "</a></span>");
            }
            htmlBuilder.push("</td>");
            htmlBuilder.push("</tr>");
        }
        htmlBuilder.push("</table>");
        htmlBuilder.push("</div>");
        htmlBuilder.push("</div>");

        //InnerHtml
        var mapEle = document.getElementById("car_DataMap");
        if (mapEle) {
            mapEle.innerHTML = htmlBuilder.join("");

            var lis = mapEle.getElementsByTagName("li");
            if (lis) {
                for (var i = 0; i < lis.length; i++) {
                    if (lis[i].getAttribute("class") != "current") {
                        lis[i].onclick = this.GenerateSellDataMapOnClick;
                        lis[i].tag = this;
                    }
                }
            }
        }
    }

    this.GenerateDataMpaNavBar = function (hisMonthStr) {
        var htmlBuilder = new Array();
        htmlBuilder.push("<div class=\"h3_tab h3_tab_title tab_ka\">");
        htmlBuilder.push("<ul>");
        htmlBuilder.push("");
        var firstYear = "";
        var isChangeYear = false;
        for (i = 0; i < datamapJson.length; i++) {
            var monthStr = datamapJson[i].Value;
            var dateSegments = monthStr.split('-');
            var tempYear = dateSegments[0];
            var tempMonth = dateSegments[1];
            tempMonth = parseInt(tempMonth, 10);

            var monthText = tempMonth + "月";
            if (firstYear.length == 0)
                firstYear = tempYear;
            else if (firstYear != tempYear && !isChangeYear) {
                monthText = tempYear.substr(2) + "年" + monthText;
                isChangeYear = true;
            }


            if (hisMonthStr == monthStr)
                htmlBuilder.push("<li class=\"current\">" + monthText + "</li>");
            else
                htmlBuilder.push("<li month=\"" + monthStr + "\">" + monthText + "</li>");
        }
        htmlBuilder.push("</ul></div>");
        return htmlBuilder.join("");
    }

    //====================================

    //================================================
    //生成主要轿车，SUV,MPV的前十数据
    this.GenerateSellDataTable = function () {
        this.InitSellDataTableFrame();
        this.Genterate("car", "");
        this.Genterate("suv", "");
        this.Genterate("mpv", "");
        this.Genterate("weixingche", "");
        this.Genterate("xiaoxingche", "");
        this.Genterate("jincouxingche", "");
        this.Genterate("zhongxingche", "");
        this.Genterate("zhongdaxingche", "");
    }
    this.InitSellDataTableFrame = function () {
        document.write('<div class="col-all" id="car_SellData"></div>');
        document.write('<div class="col-all" id="suv_SellData"></div>');
        document.write('<div class="col-all" id="mpv_SellData"></div>');
        document.write('<div class="col-all" id="weixingche_SellData"></div>');
        document.write('<div class="col-all" id="xiaoxingche_SellData"></div>');
        document.write('<div class="col-all" id="jincouxingche_SellData"></div>');
        document.write('<div class="col-all" id="zhongxingche_SellData"></div>');
        document.write('<div class="col-all" id="zhongdaxingche_SellData"></div>');
    }
    this.GenerateOnClick = function () {
        this.tag.Genterate(this.getAttribute("dataType"), this.getAttribute("hisMonths"));
    }
    this.Genterate = function (dataType, dateStr) {
        dataType = dataType.toUpperCase();
        // 	if(dataType != "SUV" && dataType != "MPV")
        // 		dataType = "CAR";

        var selldataTable = document.getElementById(dataType.toLowerCase() + "_SellData");

        var htmlBuilder = new Array();

        var xmlUrl = this.DataUrl + "&dataType=" + dataType;

        if (dateStr != "")
            xmlUrl += "&date=" + dateStr;

        var thisObj = this;
        loadJS.push(xmlUrl, "utf-8", function () {
            if (!sellDataJson || !sellDataJson.curDate || !sellDataJson.historyData)
                return;
            //数据的时间
            var curDateStr = sellDataJson.curDate;
            var dateSegments = curDateStr.split('-');
            var curDate = new Date(dateSegments[0], dateSegments[1] - 1, dateSegments[2]);

            //历史数据
            var hisStr = sellDataJson.historyData;

            //所有数据
            var dataNodes = sellDataJson.SellDatas;

            htmlBuilder.push('<div class="line_box">');

            //写表头
            var tableTitle = thisObj.GetYearStr(dateSegments[0]) + "年" + (curDate.getMonth() + 1) + "月";
            if (dataType == "CAR")
                tableTitle += "轿车";
            else if (dataType == "WEIXINGCHE")
                tableTitle += "微型车";
            else if (dataType == "XIAOXINGCHE")
                tableTitle += "小型车";
            else if (dataType == "JINCOUXINGCHE")
                tableTitle += "紧凑型车";
            else if (dataType == "ZHONGXINGCHE")
                tableTitle += "中型车";
            else if (dataType == "ZHONGDAXINGCHE")
                tableTitle += "中大型车"
            else
                tableTitle += dataType;
            tableTitle += "销量排行";

            var curColName = thisObj.GetYearStr(curDate.getFullYear()) + "年" + (curDate.getMonth() + 1) + "月";

            var preMonthColName = "";
            var preMonth = curDate.getMonth() - 1;

            if (preMonth < 0) {
                preMonthColName = thisObj.GetYearStr(curDate.getFullYear() - 1) + "年12月";
            }
            else {
                preMonthColName = thisObj.GetYearStr(curDate.getFullYear()) + "年" + (preMonth + 1) + "月";
            }

            var preYearColName = thisObj.GetYearStr(curDate.getFullYear() - 1) + "年" + (curDate.getMonth() + 1) + "月";
            var curCountColName = thisObj.GetYearStr(curDate.getFullYear()) + "年1月-" + (curDate.getMonth() + 1) + "月";
            var preCountColName = thisObj.GetYearStr(curDate.getFullYear() - 1) + "年1月-" + (curDate.getMonth() + 1) + "月";

            htmlBuilder.push("<h3><span>" + tableTitle + "</span>");
            htmlBuilder.push("</h3>");
            var hisLink = thisObj.GenerateHistLink(dataType, hisStr, curDate);
            htmlBuilder.push(hisLink);
            htmlBuilder.push("<div class=\"rank_sales\">");
            htmlBuilder.push("<table width=\"100%\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\">");
            htmlBuilder.push("<tr><th width=\"7%\" rowspan=\"2\">月销量<br />排名</th>");
            htmlBuilder.push("<th width=\"14%\" rowspan=\"2\">品牌</th>");
            htmlBuilder.push("<th colspan=\"3\">环比</th>");
            htmlBuilder.push("<th colspan=\"2\">同比</th>");
            htmlBuilder.push("<th colspan=\"3\">累计</th>");
            htmlBuilder.push("</tr><tr>");
            htmlBuilder.push("<th>" + curColName + "</th>");
            htmlBuilder.push("<th>" + preMonthColName + "</th>");
            htmlBuilder.push("<th>环比增长</th>");
            htmlBuilder.push("<th>" + preYearColName + "</th>");
            htmlBuilder.push("<th>同比增长</th>");
            htmlBuilder.push("<th width=\"10%\">" + curCountColName + "</th>");
            htmlBuilder.push("<th width=\"10%\">" + preCountColName + "</th>");
            htmlBuilder.push("<th width=\"9%\">累计增长</th>");
            htmlBuilder.push("</tr>");
            //输出数据
            for (var rowIndex = 0; rowIndex < dataNodes.length; rowIndex++) {
                var dataNode = dataNodes[rowIndex];
                htmlBuilder.push("<tr><td>" + (rowIndex + 1) + "</td>");
                htmlBuilder.push("<td><a href=\"http://car.bitauto.com/" + dataNode.AllSpell.toLowerCase() + "/\" target=\"_blank\">" + dataNode.CsName + "</a></td>");
                htmlBuilder.push("<td>" + dataNode.CurrentSellData + "</td>");
                htmlBuilder.push("<td>" + dataNode.preMonthSellData + "</td>");
                htmlBuilder.push("<td>" + dataNode.preMonthIncrease + "</td>");
                var preYearSellNum = dataNode.preYearSellData;
                if (preYearSellNum == "0")
                    htmlBuilder.push("<td>--</td>");
                else
                    htmlBuilder.push("<td>" + preYearSellNum + "</td>");
                htmlBuilder.push("<td>" + dataNode.preYearIncrease + "</td>");
                htmlBuilder.push("<td>" + dataNode.currentCount + "</td>");
                var preYearNum = dataNode.preYearCount;
                if (preYearNum == "0")
                    htmlBuilder.push("<td>--</td>");
                else
                    htmlBuilder.push("<td>" + preYearNum + "</td>");
                htmlBuilder.push("<td>" + dataNode.countIncrease + "</td>");
                htmlBuilder.push("</tr>");
            }
            htmlBuilder.push("</table>");
            htmlBuilder.push("</div>");
            htmlBuilder.push("</div>");

            if (selldataTable) {
                selldataTable.innerHTML = htmlBuilder.join("");

                var lis = selldataTable.getElementsByTagName("li");
                if (lis) {
                    for (var i = 0; i < lis.length; i++) {
                        if (lis[i].getAttribute("class") != "current") {
                            lis[i].onclick = thisObj.GenerateOnClick;
                            lis[i].tag = thisObj;
                        }
                    }
                }
            }
        });
    }
    this.GetYearStr = function (year) {
        if (year == "2010" || year == 2010)
            return "2010";
        else
            return year.toString().substring(2, 4);
    }
    //生成历史的点击链接
    this.GenerateHistLink = function (dataType, hisStr, curDate) {
        var nowDate = new Date();
        var hisMonths = hisStr.split("|");
        var htmlBuilder = new Array();
        var isCrossYear = false;
        htmlBuilder.push("<div class=\"h3_tab h3_tab_title tab_ka\"><ul>");
        for (i = 0; i < hisMonths.length; i++) {
            var dateSegments = hisMonths[i].split('-');
            var tempDate = new Date(dateSegments[0], dateSegments[1] - 1, dateSegments[2]);
            htmlBuilder.push('<li');
            if (tempDate.toLocaleString() == curDate.toLocaleString())
                htmlBuilder.push(" class=\"current\"");
            else
                htmlBuilder.push(' dataType=\'' + dataType + '\' hisMonths=\'' + hisMonths[i] + '\');"');
            var dateStr = (tempDate.getMonth() + 1) + "月";
            if (nowDate.getFullYear() != tempDate.getFullYear() && !isCrossYear) {
                dateStr = dateSegments[0].substr(2, 2) + "年" + dateStr;
                isCrossYear = true;
            }
            htmlBuilder.push(">" + dateStr + "</li>");
        }
        htmlBuilder.push("</ul></div>");
        return htmlBuilder.join("");
    }
    //================================================
    //===============================================================
    //生成页面主要轿车，SUV，MPV的前十数据
    this.GenerateSellDataTableWithDataType = function (dataType) {
        if (dataType == null || dataType.length < 1) return;

        for (var i = 0; i < dataType.length; i++) {
            this.InitSellDataTableFrameWithDataType(dataType);
            this.Genterate(dataType[i], "");
        }
    }
    //生成容器块
    this.InitSellDataTableFrameWithDataType = function(dataType, className) {
        if (dataType == null || dataType.length < 1) return;

        for (var i = 0; i < dataType.length; i++) {
            document.write('<div class="' + className + '" id="' + dataType[i] + '_SellData"></div>');
        }
    }
    //================================================
}

/*
*tree销量首页多线显示脚本
*/
//显示线性图
function showLine() {
    var lineData = new BAFlashChart();
    lineData.width = 680;
    lineData.height = 240;
    lineData.DdlProducer = document.getElementById("ddlProducer");
    lineData.tag = lineData;
    initProduceList(lineData);
    lineData.GoQuery();
}
//初始化产商列表
function initProduceList(lineData) {
    loadJS.push(lineData.DataUrl + "&dataType=brandtree", "utf-8", function () {
        //所有厂商数据
        var pNodes = brandtreeJson;
        if (pNodes == null || pNodes.length < 1) return;
        var controlObj = lineData.DdlProducer;
        controlObj.options.length = 0;
        controlObj.options.add(new Option("全部厂商", "0"));
        var prdArray = new Array();
        for (var i = 0; i < pNodes.length; i++) {
            prdArray.push(new BrandObj(pNodes[i].id, pNodes[i].name));
        }
        prdArray.sort(SortBrand);
        for (var j = 0; j < prdArray.length; j++) {
            controlObj.options.add(new Option(prdArray[j].Name, prdArray[j].Id));
        }
        controlObj.onchange = function (obj) {
            return function () { showDataLine(obj); }
        } (lineData) 
    });
}
//显示数据
function showDataLine(lineData) {
    var tagName = "";
    if (parseInt(lineData.DdlProducer.options[lineData.DdlProducer.selectedIndex].value) == 0) {
        tagName = "国产乘用车销量指数";
    }
    else {
        tagName = lineData.DdlProducer.options[lineData.DdlProducer.selectedIndex].text + "销量指数";
    }
    document.getElementById("chartTitle").innerHTML = tagName;
    lineData.GoQuery();
}