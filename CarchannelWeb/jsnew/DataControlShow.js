var columsChartTemplate =
{
    //dataset最大包含的属性名数据
    dataSectionInElement: ["seriesName", "lineThickness", "showValues", "color", "anchorBorderColor", "anchorBgColor"],
    columnsDataSectionInElement: ["caption", "xAxisName", "yAxisName", "showValues", "decimals", "formatNumberScale", "useRoundEdges", "bgColor", "showBorder", "baseFontSize"],
    //控件头
    controlHeader: "<chart caption='@caption@' subcaption='' showBorder='0' showLegend='0' lineThickness='1' showValues='1' formatNumberScale='0'"
                + "  divLineColor='326FB8' divLineAlpha='50' divLineIsDashed='1' showAlternateHGridColor='1' alternateHGridAlpha='5'"
                + " alternateHGridColor='326FB8' shadowAlpha='40' numvdivlines='@numvdivlines@' chartRightMargin='30' yAxisValuesPadding='5'"
                + " yAxisMaxValue='@maxValue@' canvasPadding='30' bgColor='FFFFFF' bgAngle='270' bgAlpha='10,10'>",
    //控件脚
    controlFooter: "<styles>"
		        + "<definition>"
                + "<style name='CaptionFont' type='font' size='12'/>"
                + "</definition>"
                + "<application>"
                + "<apply toObject='CAPTION' styles='CaptionFont' />"
                + "<apply toObject='SUBCAPTION' styles='CaptionFont' />"
                + "</application>"
                + "</styles>"
                + "</chart>",
    //柱状图数据标签
    columnsDataTagName: "chart",
    //柱状图标签内容
    columnsSetion: "<set label='@name@' value='@data@' link='n-http://car.bitauto.com/@allspell@/' />",
    //数据标签名
    dataTagName: "dataset",
    //数据项格式
    dataSection: "<set value='@data@' />",
    //分类标签名
    categoryTagName: "categories",
    //分类项格式
    categorySection: "<category label='@xAxisValue@' />"
}

function LineShow()
{
    //Y轴最大值
    this.yMaxValue = 0;
    //Y轴最小值:必须设置否则为零
    this.yMinValue = 0;
    //x轴值数组
    this.xAxisValue = [];
    //线的数据数组,元数据为{属性名:属性值,...,data:[值1,值2,]},其中属性名和(dataSectionInElement)对应，可没有(dataSectionInElement)中包含的值
    this.dataValues = [];
    //包含控件的div
    this.pageElementId = "";
    //控件宽
    this.width = 0;
    //控件高
    this.height = 0;
    //附加信息
    this.xAddtionalMess = [];
    //Flash标题
    this.Caption = "";
    
    this.getMaxValue = function(maxValue) {
        maxValue = parseInt((maxValue * 1.3));
        var xMaxStr = maxValue.toString();
        return (parseInt(xMaxStr.substring(0, 1)) + 1) * (Math.pow(10, xMaxStr.length - 1));
    };
    //替换数据
    this.replaceData = function(arr1, arr2, text) {
        this.cyc(arr1, function(obj, i) {
            text = text.replace(eval('/' + arr2[i] + '/g'), obj);
        })
        return text;
    };
    //初始化分类
    this.initCategory = function() {
        if (this.xAxisValue == null || this.xAxisValue.length < 1) return "";
        var categoriesContent = [];
        for (var i = 0; i < this.xAxisValue.length; i++) {
        	categoriesContent[i] = columsChartTemplate.categorySection.replace(/@xAxisValue@/g, this.xAxisValue[i]);
        }

        return "<" + columsChartTemplate.categoryTagName + ">" + categoriesContent.join("") + "</" + columsChartTemplate.categoryTagName + ">";
    };
    //初始化数据头
    this.initDataHeader = function(data, sectionElement, tagName) {
        if (sectionElement == null || sectionElement.length < 1) return "<" + tagName + ">";
        if (data == null) return "<" + tagName + ">";
        var dataHeaderContent = [];

        for (var i = 0; i < sectionElement.length; i++) {
            var stagName = sectionElement[i];
            if (data[stagName] == null) continue;
            dataHeaderContent.push(stagName + "='" + data[stagName] + "'");
        }

        return "<" + tagName + " " + dataHeaderContent.join(" ") + ">";
    };
    //初始化数据
    this.initData = function() {
        if (this.dataValues == null || this.dataValues.length < 1) return "";
        var dataContent = [];
        var maxValue = 0;

        for (var i = 0; i < this.dataValues.length; i++) {
            var data = this.dataValues[i];
            if (data["data"] == null || data["data"].length < 1) continue;
            var header = this.initDataHeader(data, columsChartTemplate.dataSectionInElement, columsChartTemplate.dataTagName);
            var dataList = [];
            for (var j = 0; j < data["data"].length; j++) {
                if (parseInt(data["data"][j]) > maxValue) maxValue = parseInt(data["data"][j]);
                dataList[j] = columsChartTemplate.dataSection.replace(/@data@/g, data["data"][j]);
            }
            dataContent.push(header + dataList.join("") + "</" + columsChartTemplate.dataTagName + ">");
        }
        //如果用户没有给最大值赋值，则
        if (this.yMaxValue <= 0) this.yMaxValue = this.getMaxValue(maxValue);
        return dataContent.join("");
    };
    this.initColumns3DData = function()
    {
    	if (this.dataValues == null || this.dataValues.length < 1) return "";
    	if (this.xAxisValue == null || this.xAxisValue.length < 1 || this.xAxisValue.length != this.dataValues[0]["data"].length) return "";
    	var dataContent = "";
    	var maxValue = 0;

    	var data = this.dataValues[0];
    	if (data["data"] == null || data["data"].length < 1) return "";
    	var header = this.initDataHeader(data, columsChartTemplate.columnsDataSectionInElement, columsChartTemplate.columnsDataTagName);
    	var dataList = [];
    	for (var j = 0; j < data["data"].length; j++)
    	{
    		if (!this.xAddtionalMess[j])
    			continue;
    		if (parseInt(data["data"][j]) > maxValue) maxValue = parseInt(data["data"][j]);
    		var datastr = columsChartTemplate.columnsSetion.replace(/@name@/g, this.xAxisValue[j]);
    		datastr = datastr.replace(/@data@/g, data["data"][j]);
    		datastr = datastr.replace(/@allspell@/g, this.xAddtionalMess[j]["allSpell"].toLowerCase()); //alert(datastr);
    		dataList[j] = datastr;
    	}
    	return header + dataList.join("") + "</" + columsChartTemplate.columnsDataTagName + ">";
    };
    this.CreateControl = function() {
        var chartObj = new FusionCharts("http://js.bitauto.com/dealer/Report/FlashChart/MSLine.swf", "IndexTendFlash", this.width + "px", this.height + "px", "0", "0", "", "noScale", "EN");
        chartObj.setTransparent(true);
        return chartObj;
    };
    this.Create3DColumnControl = function() {
        var chartObj = new FusionCharts("http://js.bitauto.com/dealer/Report/FlashChart/Column2D.swf", "CompareIndexFlash", this.width + "px", this.height + "px", "0", "0");
        chartObj.setTransparent(true);
        return chartObj;
    };
    //显示控件
    this.showControl = function() {
        var pro = this;
        if (pro.xAxisValue == null || pro.xAxisValue.length < 1) return;
        if (pro.dataValues == null || pro.dataValues.length < 1) return;
        if (!document.getElementById(pro.pageElementId)) return;
        var controlObj = pro.CreateControl();
        if (!controlObj) return;
        var categories = pro.initCategory();
        var dataset = pro.initData();
        var dataHeader = columsChartTemplate.controlHeader.replace(/@maxValue@/g, this.yMaxValue);
        dataHeader = dataHeader.replace(/@caption@/g, this.Caption);
        dataHeader = dataHeader.replace(/@numvdivlines@/g, pro.xAxisValue.length < 2 ? 2 : pro.xAxisValue.length - 2);

        var data = dataHeader + categories + dataset + columsChartTemplate.controlFooter;
        document.getElementById(pro.pageElementId).innerHTML = "";
        controlObj.setDataXML(data);
        window.setTimeout(function() { controlObj.render(pro.pageElementId); }, 0.2);
    };
    //显示柱状图控件
    this.showColumns = function() {
        var pro = this;
        if (pro.xAxisValue == null || pro.xAxisValue.length < 1) return;
        if (pro.dataValues == null || pro.dataValues.length < 1) return;
        if (!document.getElementById(pro.pageElementId)) return;
        var controlObj = pro.Create3DColumnControl();
        if (!controlObj) return;
        var dataContent = pro.initColumns3DData();
        document.getElementById(pro.pageElementId).innerHTML = "";
        controlObj.setDataXML(dataContent);
        window.setTimeout(function() { controlObj.render(pro.pageElementId); }, 0.2);
    }
}