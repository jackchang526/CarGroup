/// <reference path="jquery-1.4.1.min.js" />
//"l", "d", "g", "c", "t", "dt", "f", "b", "lv", "more"
var SelectSceneCarTool = {
    Price: ""
	, Level: ""					//0不限，1-10对应各级别
	, Displacement: ""         //排量
    , Brand: ""                  //自主，合资
    , Country: ""                  //国别
	, TransmissionType: ""		//0不限，1手动，2自动
    , DriveType: ""	//驱动方式
     , Fuel: ""	//燃料
    , Body: ""    //车身
    ,Wagon:""     //旅行版
    , MoreConditionStr:""
	, Type: "car"
	, Domain: window.location.host
    , pageSize: 20
    , apiUrl: "http://select.car.yiche.com/api/selectcartool/searchresult"
    ,timer:0
    //初始化页面显示
	, initPageCondition: function () {
	    this.initPrice();
	    this.bindFilterClickEvent();
	    this.bindPriceBtnClickEvent();
	    //获取数据列表
	    this.GetSearchData();
	}
    //绑定条件点击事件
	, bindFilterClickEvent: function () {
	    var self = this;
	    var checkList = $(".qingj-filter .qingj-filter-list li");
	    //绑定复选框选择事件
	    $(checkList).each(function (index, item) {
	        $(item).on("click", function (e) {
	            e.preventDefault();
	            var $allCheckedList = $(".qingj-filter .qingj-filter-list li").find(".checked");//判断至少有一个选项选中

	            var checkBoxParam = $(this).data("param");    //more=204_205_206  dt=4,8,16   l=7  t=8
	            var $curCheckBoxCss = $(this).find(".radio-box label div");
	            if ($curCheckBoxCss.hasClass("checked")) {
	                if ($allCheckedList.length > 1) {
	                    $curCheckBoxCss.removeClass("checked");
	                }
	                else {
	                    $(".jump-pop").show();
	                    self.setTimer();
	                    return false;
	                }
	            }
	            else {
	                $(".jump-pop").hide();
	                $curCheckBoxCss.addClass("checked");
	            }
	            //处理各参数
	            var paramDict = checkBoxParam.split('=');
	            var paramName = paramDict[0];
	            var paramValue = paramDict[1];
	            switch (paramName)
	            {
	                case "l":
	                    if (self.Level.indexOf(paramValue) < 0) {
	                        if (self.Level == '') {self.Level = paramValue}
	                        else {self.Level += ',' + paramValue;}
	                    }
	                    else {
	                        self.Level = commatrim(self.Level.replace(paramValue, ''));
	                    }
	                    break;
	                case "d":
	                    if (self.Displacement.indexOf(paramValue) < 0) {
	                        if (self.Displacement == '') {
	                            self.Displacement = paramValue
	                        }
	                        else { self.Displacement += ',' + paramValue; }
	                    }
	                    else {
	                        self.Displacement = commatrim(self.Displacement.replace(paramValue, ''));
	                    }
	                    break;
	                case "g":
	                    if (self.Brand.indexOf(paramValue) < 0) {
	                        if (self.Brand == '') {
	                            self.Brand = paramValue
	                        }
	                        else { self.Brand += ',' + paramValue; }
	                    }
	                    else {
	                        self.Brand = commatrim(self.Brand.replace(paramValue, ''));
	                    }
	                    break;
	                case "c":
	                    if (self.Country.indexOf(paramValue) < 0) {
	                        if (self.Country == '') {
	                            self.Country = paramValue
	                        }
	                        else { self.Country += ',' + paramValue; }
	                    }
	                    else {
	                        self.Country = commatrim(self.Country.replace(paramValue, ''));
	                    }
	                    break;
	                case "t":
	                    if (self.TransmissionType.indexOf(paramValue) < 0) {
	                        if (self.TransmissionType == '') {
	                            self.TransmissionType = paramValue
	                        }
	                        else { self.TransmissionType += ',' + paramValue; }
	                    }
	                    else {
	                        self.TransmissionType = commatrim(self.TransmissionType.replace(paramValue, ''));
	                    }
	                    break;
	                case "dt":
	                    if (self.DriveType.indexOf(paramValue) < 0) {
	                        if (self.DriveType == '') {
	                            self.DriveType = paramValue
	                        }
	                        else { self.DriveType += ',' + paramValue; }
	                    }
	                    else {
	                        self.DriveType = commatrim(self.DriveType.replace(paramValue, ''));
	                    }
	                    break;
	                case "f":
	                    if (self.Fuel.indexOf(paramValue) < 0) {
	                        if (self.Fuel == '') {
	                            self.Fuel = paramValue
	                        }
	                        else { self.Fuel += ',' + paramValue; }
	                    }
	                    else {
	                        self.Fuel = commatrim(self.Fuel.replace(paramValue, ''));
	                    }
	                    break;
	                case "b":
	                    if (self.Body.indexOf(paramValue) < 0) {
	                        if (self.Body == '') {
	                            self.Body = paramValue
	                        }
	                        else { self.Body += ',' + paramValue; }
	                    }
	                    else {
	                        self.Body = commatrim(self.Body.replace(paramValue, ''));
	                    }
	                    break;
	                case "lv":
	                    if (self.Wagon.indexOf(paramValue) < 0) {
	                        if (self.Wagon == '') {
	                            self.Wagon = paramValue
	                        }
	                        else { self.Wagon += ',' + paramValue; }
	                    }
	                    else {
	                        self.Wagon = commatrim(self.Wagon.replace(paramValue, ''));
	                    }
	                    break;
	                case "more":
	                    if ((self.MoreConditionStr).indexOf(paramValue) < 0) {
	                        if (self.MoreConditionStr == '') {
	                            self.MoreConditionStr = paramValue;
	                        }
	                        else {
	                            self.MoreConditionStr += '_' + paramValue;
	                        }
	                    }
	                    else {
	                        self.MoreConditionStr =linetrim(self.MoreConditionStr.replace(paramValue, ''));
	                    }
	                    break;
	                default: break;
	            }
                //最后跳转
	            self.GotoPage();
	        });
	    })
	    $("body").on("click", function () {
	        $(".jump-pop").hide();
	    })
	}
    , setTimer: function () {
        clearTimeout(this.timer);
        this.timer = setTimeout(function () {
            $(".jump-pop").hide();
        }, 2000);
    }
    //价格点击事件
    , bindPriceBtnClickEvent: function () {
        var self = this;
        var priceBtnList = $(".pd15 .price-range li");
        $(priceBtnList).each(function (index, item) {
            $(item).on("click", function (e) {
                $(this).addClass("current").siblings().removeClass("current");
                var priceParam = $(this).data("param");
                if (priceParam.length < 1)
                { self.Price = '';}
                else
                {
                    var paramDict = priceParam.split('=');
                    var paramName = paramDict[0];
                    var paramValue = paramDict[1];
                    if (paramValue&&paramValue.length>0)
                    {
                        self.Price = paramValue;
                    }
                }
                self.GotoPage();
            });
        });
    }
    , GotoPage: function () {
        var queryString = this.GetSearchQueryString(0);
        var toUrl = _SearchUrl;
        if (queryString.length > 0)
            toUrl += "&" + queryString;
        location.href = toUrl;
    }
    //设置选中项不可用
	, setDisabled: function (elem) {
	    if (!elem) return;
	    $(elem).addClass("current").siblings().removeClass("current");
	}
    //初始化价格
	, initPrice: function () {
	    switch (this.Price) {
	        case "0-18":
	            this.setDisabled($$("price1"));
	            break;
	        case "18-25":
	            this.setDisabled($$("price2"));
	            break;
	        case "25-40":
	            this.setDisabled($$("price3"));
	            break;
	        case "40-9999":
	            this.setDisabled($$("price4"));
	            break;
	        default:
	                this.setDisabled($$("price0"));
	            break;
	    }
	}
	, SetMoreCondition: function (mcConditionStr) {
	    this.MoreConditionStr =linetrim(mcConditionStr);
	}
    //获取请求参数
	, GetSearchQueryString: function (mode) {  //mode=0  处理url显示，隐藏多选；mode=1处理api接口请求，带上多选参数
	    var qsArray = new Array();
	    if (this.Price && this.Price.length > 0)
	        qsArray.push("p=" + this.Price);

	    if (this.Level && this.Level.length > 0)
	        qsArray.push("l=" + this.Level);
	    if (this.Displacement && this.Displacement.length > 0)
	        qsArray.push("d=" + this.Displacement);
	    if (this.Brand && this.Brand.length > 0)
	        qsArray.push("g=" + this.Brand);
	    if (this.Country && this.Country.length > 0)
	        qsArray.push("c=" + this.Country);
	    if (this.TransmissionType && this.TransmissionType.length > 0)
	        qsArray.push("t=" + this.TransmissionType);
	    if (this.DriveType && this.DriveType.length > 0)
	        qsArray.push("dt=" + this.DriveType);
	    if (this.Fuel && this.Fuel.length > 0)
	        qsArray.push("f=" + this.Fuel);
	    if (this.Body && this.Body.length > 0)
	        qsArray.push("b=" + this.Body);
	    if (this.Wagon && this.Wagon.length > 0)
	        qsArray.push("lv=" + this.Wagon);

	    var mc = this.MoreConditionStr;
	    if (mc.length > 0) {
	        mc = mc.replace("__","_");
	        qsArray.push("more=" + mc);
	    }
	    return qsArray.join('&');
	}
    , GetSearchData: function () {
        var that = this;
        var apiQueryString = this.GetSearchQueryString(1);
        var url = this.apiUrl;
        if (apiQueryString.length > 0) {
            url += "?external=Car.m&" + apiQueryString;
        }

        $.ajax({
            url: url,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                var result = json;
                DrawUlContent(result);
                var pageCount = Math.ceil(json.Count / that.pageSize);
                that.bindPageEvent(pageCount);
            }
        });
    }
    , bindPageEvent: function (pageCount) {
        var that = this;
        //当存在车款时，才处理分页
        if (pageCount > 0) {
            $(".m-pages").css("display", "");
            $("#totalPage").html(pageCount);
            var $prevBtn = $(".m-pages-pre");
            var $nextBtn = $(".m-pages-next");
            var $selectOption = $(".m-pages-select");
            if (pageCount > 1) {
                //下拉页列表
                var option = '';
                for (var i = 0; i < pageCount; i++) {
                    var curOptNum = i + 1;
                    option += "<option>" + curOptNum + "</option>";
                }
                $(".m-pages-select").append(option);

                //Desc:处理“上一页”事件
                $prevBtn.on("click", function () {
                    var curPageNum = Number($("#curPageIndex").text());    //当前页数
                    if (curPageNum > 1) {
                        curPageNum -= 1;
                        var rqStr = that.GetSearchQueryString(1) + "&page=" + curPageNum;
                        that.getPageData(rqStr, curPageNum, function () {
                            //判断“上一页”可点击状态
                            if (curPageNum <= 1) {
                                $prevBtn.addClass("m-pages-none"); //设置不可点击
                            }
                            //判断“下一页”可点击状态
                            if ($nextBtn.hasClass("m-pages-none") && curPageNum < pageCount) {
                                $nextBtn.removeClass("m-pages-none");//设置可点击
                            }
                        })
                    }
                    else { $prevBtn.addClass("m-pages-none"); }
                });
                //处理“下一页”事件
                $nextBtn.on("click", function () {
                    var curPageNum = Number($("#curPageIndex").text());    //当前页数
                    if (curPageNum < pageCount) {
                        curPageNum += 1;
                        var rqStr = that.GetSearchQueryString(1) + "&page=" + curPageNum;
                        that.getPageData(rqStr, curPageNum, function () {
                            //判断“下一页”可点击状态
                            if (curPageNum >= pageCount) {
                                $nextBtn.addClass("m-pages-none");
                            }
                            //判断“上一页”可点击状态
                            if ($prevBtn.hasClass("m-pages-none") && curPageNum > 1) {
                                $prevBtn.removeClass("m-pages-none");
                            }
                        });
                    }
                    else { $nextBtn.addClass("m-pages-none"); }
                });

                //处理下拉列表页数点击事件
                $selectOption.change(function () {
                    var curOptionVal = $(this).val();
                    curPageNum = curOptionVal;

                    //判断“上一页”可点击状态
                    if (curPageNum < 2) { $prevBtn.addClass("m-pages-none"); }
                    else if ($prevBtn.hasClass("m-pages-none")) { $prevBtn.removeClass("m-pages-none"); }
                    //判断“下一页”可点击状态
                    if (curPageNum >= pageCount) { $nextBtn.addClass("m-pages-none"); }
                    else if ($nextBtn.hasClass("m-pages-none")) { $nextBtn.removeClass("m-pages-none"); }

                    var rqStr = that.GetSearchQueryString(1) + "&page=" + curPageNum;
                    that.getPageData(rqStr, curPageNum, function () {

                    });
                });
            }
            else {
                //如果结果列表页数只有一页，则“下一页”不可点
                $nextBtn.addClass("m-pages-none");
                //下拉框不可选
                $('.m-pages-select').prop("disabled", true);
            }
        }
    }
    , getPageData: function (rqStr, curPageNum, callBack) {
        //获取当前排序方式：
        var order = 4;
        var urlParamsDict = this.parseUrlParameters(rqStr);
        order = (urlParamsDict['s'] == undefined) ? 4 : urlParamsDict['s'];
        $.ajax({
            url: this.apiUrl + "?external=Car.m&" + rqStr,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                var result = json;
                $("#curPageIndex").text(curPageNum);
                DrawUlContent(result, order);
                callBack();
                //返回顶部
                //$(document).scrollTop($(".searchResult").offset().top);
                $(document).scrollTop(0);
            },
            error: function (xhr) {
            }
        });
    }
    , parseUrlParameters: function (rqStr) {
        var pattern = /(\w+)=(\w+)/ig;//定义正则表达式 
        var parames = {};//定义数组 
        rqStr.replace(pattern, function (a, b, c) { parames[b] = c; });
        return parames;//返回这个数组. 
    }
}

function DrawUlContent(result) {
    var h = [];
    if (result.ResList.length > 0) {
        $(".car-list-tt span").html(result.Count);
        h.push("<ul>");
        $(result.ResList).each(function (index) {
            var serialUrl = "/" + result.ResList[index].AllSpell + "/";
            var shortName = result.ResList[index].ShowName.toString().replace("(进口)", "");
            var curSerialId = result.ResList[index].SerialId;
            var imageUrl = result.ResList[index].ImageUrl.toString().replace("_1.", "_6.");
            var priceRange = result.ResList[index].PriceRange;
            var isAdvertise = result.ResList[index].Pos == undefined ? false : true;
            if (curSerialId == 1568) {
                shortName = "索纳塔八";
            }
            h.push("<li>");
            h.push("<a href=\"" + serialUrl + "\" class=\"car\"><span><img src=\"" + imageUrl + "\" />");
            if (isAdvertise)   //设置“特价”标签
                h.push("<i class=\"recommend\"></i>");
            h.push("</span><p>" + shortName + "</p>");
            h.push("<b>" + priceRange + "</b>");
            h.push("</a></li>");

        });
        h.push("</ul>");
        $(".m-pages").show();
    }
    else {
        $(".car-list-tt span").html(0);
        h.push("<div class=\"yxc-prompt-box\">");
        h.push("<div class=\"yxc-prompt-none\"></div><p>对不起，该价位没有合适的车型！</p>");
        h.push("</div>");
    }
    $(".car-list2").html(h.join(''));

}

function $$(id) { return document.getElementById(id); }

/**
* 删除左右两端的-(横线)
*/
function linetrim(str) {
    return str.replace(/(^_*)|(_*$)/g, '');
}
function commatrim(str) {
    return str.replace(/(^,*)|(,*$)/g, '');
}