var page = 1, pageSize =10;
var SelectPhotoTool = {
    Parameters: new Array()
    , FuelTypeName: { 16: "纯电动", 128: "插电混动" }
    , BrandName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口" }
    , BodyFormName: { 1: "两厢", 2: "三厢", 64: "SUV" }
    , CSCount: 0
    , initPageCondition: function () {
        this.initFilterCondition();
        if (SelectPhotoTool.Parameters.toString().indexOf('page=') == -1) {
            SelectPhotoTool.Parameters.push("page=" + page);
        }
        var pageCount = Math.ceil(this.CSCount / pageSize);
        $("#curPageIndex").text(page);       
        this.bindPageEvent(pageCount);
        $(".m-pages-select").val(page);       
    }
    //初始化查询条件
    , initFilterCondition: function () {
        for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
            var idTemp = SelectPhotoTool.Parameters[i].split('=')[1];
            var typeTemp = SelectPhotoTool.Parameters[i].split('=')[0];
           
            if (typeTemp == "page") {
                page = idTemp;
            }
            var elementTemp = getElementById(typeTemp + '_' + idTemp);
            if (elementTemp) {
                setDisabled(elementTemp);                
            }
            if (idTemp > 0) {
                switch (typeTemp) {
                    case "f":
                        $("#btnFuelType").find("span").eq(0).html(this.FuelTypeName[idTemp]);
                        $("#btnFuelType").parent().addClass("current");
                        break;
                    case "g":
                        $("#btnBrandType").find("span").eq(0).html(this.BrandName[idTemp]);
                        $("#btnBrandType").parent().addClass("current");
                        break;
                    case "b":
                        $("#btnBodyform").find("span").eq(0).html(this.BodyFormName[idTemp]);
                        $("#btnBodyform").parent().addClass("current");
                        break;
                    default:
                        break;
                }
            }
        }
    }
    , initSingleFilter: function (type) {
        for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
            var idTemp = SelectPhotoTool.Parameters[i].split('=')[1];
            var typeTemp = SelectPhotoTool.Parameters[i].split('=')[0];

            if (typeTemp == type) {
                var elementTemp = getElementById(typeTemp + '_' + idTemp);
                if (elementTemp) {
                    this.setDisabled(elementTemp);
                }
            }
        }
    }
    //设置选中项不可用
    , setDisabled: function (elem) {
        if (!elem) return;
        $(elem).parent().addClass("current").siblings().removeClass("current");
    }
    //获取请求参数
    , GetSearchQueryString: function (hasPage) {
        var qsArray = new Array();
        for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
            var typeId = SelectPhotoTool.Parameters[i].split('=')[0];
            var id = SelectPhotoTool.Parameters[i].split('=')[1];
            if (hasPage) {
                if (typeId == "page") {
                    continue;
                }
            }
            qsArray.push(typeId + "=" + id);            
        }
        return qsArray.join('&');
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
            if (Number($("#curPageIndex").text()) < 2) { $prevBtn.addClass("m-pages-none"); }
            if (Number($("#curPageIndex").text()) == pageCount) { $nextBtn.addClass("m-pages-none"); }

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
        UpdatePage(rqStr);
    }
}
function getElementById(i) { return document.getElementById(i); }
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
function $$(id) { return document.getElementById(id); }
function GotoPage(conditionStr) {
    UpdatePara(conditionStr);
    UpdatePara("page_1");
    var queryString = SelectPhotoTool.GetSearchQueryString(0);
    UpdatePage(queryString);
}
function UpdatePara(conditionStr) {
    if (conditionStr.length >= 1) {
        var conType = conditionStr.split('_')[0];
        var conStr = conditionStr.split('_')[1];
        var strType = conType + "=";
        if (SelectPhotoTool.Parameters.toString().indexOf(strType) > -1) {
            for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
                if (SelectPhotoTool.Parameters[i].indexOf(strType) > -1) {
                    var delId = SelectPhotoTool.Parameters[i].split('=')[1]
                    SelectPhotoTool.Parameters.splice(i, 1);
                    break;
                }
            }
        }
        if (conStr != 0) {
            SelectPhotoTool.Parameters.push(conType + "=" + conStr);
        }
    }
}
function UpdatePage(queryString) {
    var toUrl = 'http://' + window.location.host + window.location.pathname;;
    if (queryString.length > 0)
        toUrl += "?" + queryString;
    window.location.href = toUrl;
}