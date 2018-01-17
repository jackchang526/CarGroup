var SelectPhotoTool = {
   	 BodyForm: 0				//0 不限
    , BodyFormName: { 1: "两厢", 2: "三厢", 10: "suv " }
    , FuelType: 0	//燃料类型
    , FuelTypeName: { 7: "汽油", 8: "柴油", 2: "油电混合", 16: "纯电动", 128: "插电混合", 256: "天然气" }
    , Country: 0
    , CountryName: { 2: "日系", 4: "德系", 8: "美系", 16: "韩系", 484: "欧系" }
    , Type: "car"
    , Domain: window.location.host
    , currentId: ""
    , pageSize: 20
    , apiUrl: "http://select.car.yiche.com/api/selectcartool/searchresult"
    //初始化页面显示
    , initPageCondition: function () {
        this.initShowDefault();
        this.initBodyForm();
        this.initBrandType();
        this.bindFilterClickEvent();
        this.initRightSwipe();
        //获取数据列表
        this.GetSearchData();
    }
   
    //默认展开条件
    , initShowDefault: function () {
        var hash = window.location.hash;
        switch (hash.toLowerCase()) {
            case "#sd":
                this.currentId = "btnPrice";
                $(".leftPopup").css("zIndex", 199);
                var button = SelectCarTool.returnPopupItemId(this.currentId);
                SelectCarTool.toggle($("#" + button));
                window.setTimeout(function () { $("#p_min").get(0).focus(); }, 500);
                break;
        }
    }

    //点击事件处理
    , filterClickEvent: function () {
        var id = this.id, prevClickElement;
        $(".leftPopup").css("zIndex", 199);

        var button = SelectCarTool.returnPopupItemId(id);
        SelectCarTool.toggle($("#" + button));
        SelectCarTool.currentId = id;
        window.scrollTo(0, 0);
    }

    //返回条件弹出层id
    , returnPopupItemId: function (cid) {
        var result = "";
        switch (cid) {
            case "btnPrice": result = "m-filter-price"; break;
            case "btnLevel": result = "m-filter-level"; break;
            case "btnLevelsuv": result = "m-filter-levelsuv"; break;
            case "btnBrandType": result = "m-filter-brandtype"; break;
            case "btnTransmisstionType": result = "m-filter-trans"; break;
            case "btnDisplacement": result = "m-filter-dis"; break;
            case "btnMore": result = "m-filter-more"; break;
            case "btnCondition": result = "m-filter-condition"; break;
            case "btnBodyform": result = "m-filter-body"; break;

            case "btnDrive": result = "m-filter-drive"; break;
            case "btnFuel": result = "m-filter-fuel"; break;
            case "btnDoor": result = "m-filter-door"; break;
            case "btnSeat": result = "m-filter-seat"; break;
        }
        return result;
    }
   
    , GotoPage: function () {
        var queryString = this.GetSearchQueryString(0);
        var toUrl = _SearchUrl;
        if (queryString.length > 0)
            toUrl += "?" + queryString;
        location.href = toUrl;
    }
    //设置选中项不可用
    , setDisabled: function (elem) {
        if (!elem) return;
        //var info = elem.firstChild.innerHTML;
        //elem.innerHTML = "<a>" + info + "</a>";
        //elem.className = "current";
        $(elem).addClass("current");
    }
    //初始化车身
    , initBodyForm: function () {
        var str = "bodyform", v = this.BodyForm;
        var bodyFormEle = document.getElementById(str + v);
        if (bodyFormEle) {
            this.setDisabled(bodyFormEle);
        }
        if (this.IsWagon > "0") {
            $$("btnBodyform").childNodes[1].childNodes[1].innerHTML = "旅行版";
            $$("btnBodyform").className = "m-btn current";
        }
        else if (this.BodyForm != "0") {
            $$("btnBodyform").childNodes[1].childNodes[1].innerHTML = this.BodyFormName[this.BodyForm];
            $$("btnBodyform").className = "m-btn current";
        }
    }
    //初始化国别
    , initBrandType: function () {
        this.setDisabled($$("brandType" + this.Brand.toString()));
        this.setDisabled($$("countryType" + this.Country.toString()));
        if (this.Brand != "") {
            $$("btnBrandType").childNodes[1].childNodes[1].innerHTML = this.BrandName[this.Brand];
            $$("btnBrandType").className = "m-btn current";
        }
        if (this.Country != "") {
            $$("btnBrandType").childNodes[1].childNodes[1].innerHTML = this.CountryName[this.Country];
            $$("btnBrandType").className = "m-btn current";
        }
    }
    //获取请求参数
    , GetSearchQueryString: function (mode) {  //mode=0  处理url显示，隐藏多选；mode=1处理api接口请求，带上多选参数
        var qsArray = new Array();
        if (this.Level != 0)
            qsArray.push("l=" + this.Level.toString());
        if (parseInt(this.BodyDoors) > 0 && this.BodyDoors.length > 0) {
            if (more.length > 0)
            { more += "_"; }
            if (this.BodyDoors.indexOf('2-3') > -1) {
                more += "268_";    //处理url
                if (mode == 1) {    //处理api接口参数
                    more += "269_";
                }
            }
            if (this.BodyDoors.indexOf('4-6') > -1) {
                more += "270_";
                if (mode == 1) {
                    more += "271_272_";
                }
            }
            more = more.substr(0, more.length - 1);
        }
        if (this.PerfSeatNum && this.PerfSeatNum.length > 0) {
            if (more.length > 0) {
                more += "_";
            }
            if (this.PerfSeatNum.indexOf('2') > -1) {
                more += "279_";
            }
            if (this.PerfSeatNum.indexOf('4') > -1) {
                more += "280_";
            }
            if (this.PerfSeatNum.indexOf('5') > -1) {
                more += "281_";
            }
            if (this.PerfSeatNum.indexOf('6') > -1) {
                more += "282_";
            }
            if (this.PerfSeatNum.indexOf('7') > -1) {
                more += "283_";
            }
            if (this.PerfSeatNum.indexOf('8-9999') > -1) {
                more += "284_";
            }
            more = more.substr(0, more.length - 1);
        }
        //添加环保标准
        if (this.Environment.length > 0 && this.Environment != "0") {
            if (more.length > 0) {
                more += "_";
            }
            more += this.Environment + "_";
            more = more.substr(0, more.length - 1);
        }
        if (more.length > 0) {
            qsArray.push("more=" + more);
        }

        if (this.View != 0)
            qsArray.push("v=" + this.View.toString());
        if (this.Sort != 0)
            qsArray.push("s=" + this.Sort.toString());
        if (this.Brand != 0)
            qsArray.push("g=" + this.Brand.toString());
        if (this.Country != 0)
            qsArray.push("c=" + this.Country.toString());
        if (this.BodyForm != 0)
            qsArray.push("b=" + this.BodyForm.toString());
        if (this.IsWagon && this.IsWagon == 1)
            qsArray.push("lv=" + this.IsWagon);
        if (this.DriveType && this.DriveType > 0)
            qsArray.push("dt=" + this.DriveType);
        if (this.FuelType && this.FuelType > 0)
            qsArray.push("f=" + this.FuelType);


        return qsArray.join('&');
    }
    , GetSearchData: function () {
        var that = this;
        var apiQueryString = this.GetSearchQueryString(1);
        var url = this.apiUrl;
        if (apiQueryString.length > 0) {
            url += "?external=Car.m&" + apiQueryString;
        }
        //获取当前排序方式：
        var order = 4;
        var urlParamsDict = that.parseUrlParameters(apiQueryString);
        order = (urlParamsDict['s'] == undefined) ? 4 : urlParamsDict['s'];

        $.ajax({
            url: url,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                var result = json;
                if (typeof listSerialAD != "undefined" && listSerialAD.length > 0) {
                    if (result.ResList.length > 0) {
                        result.ResList = GetAdPosition(json);
                    }
                }
                DrawUlContent(result, order);
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
                            //if (curPageNum == 1) {
                            //    addCarListManual(curPageNum);
                            //}
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
                        //if (curPageNum == 1) {
                        //    addCarListManual(curPageNum);
                        //}
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
                if (curPageNum == 1 && typeof listSerialAD != "undefined" && listSerialAD.length > 0) {
                    result.ResList = GetAdPosition(json);
                }
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