var page = 1, sort = 4, pageSize = 50;
var initFlag = true;
var anchorFlag = false;

//模拟text的change事件
(function ($) {
    $.fn.watch = function (callback) {
        return this.each(function () {
            //缓存以前的值  
            $.data(this, 'originVal', $(this).val());
            //event  
            $(this).on('keyup paste', function (event) {
                var originVal = $.data(this, 'originVal');
                var currentVal = $(this).val();

                if (originVal !== currentVal) {
                    $.data(this, 'originVal', $(this).val());
                    callback(this, event);
                }
            });
        });
    }
})(jQuery);

var SuperSelectCarTool = {
    Parameters: new Array()
    , apiUrl: "http://select.car.yiche.com/selectcartool/searchresult"
    , OneLeftScrollFlag: false //滚动菜单是否显示 ，用于 左侧滚动 > 居左距离
    , MenuOffsetTop: 304 //滚动菜单 相对车款头的高度偏移量

    , initPageCondition: function () {
        this.initFilterCondition();
        this.bindFilterClickEvent();

        //确定左侧菜单位置
        $("#left-nav").css({ left: $("#box").offset().left - 72 + "px", top: "438px" });

        if (SuperSelectCarTool.Parameters.toString().indexOf('page=') == -1) {
            SuperSelectCarTool.Parameters.push("page=" + page);
        }
        if (SuperSelectCarTool.Parameters.toString().indexOf("pagesize=") == -1) {
            SuperSelectCarTool.Parameters.push("pagesize=" + pageSize);
        }

        SuperSelectCarTool.UpdateCarResult();
    }
    //绑定条件点击事件
    , bindFilterClickEvent: function () {
        //绑定复选框，单选框的点击事件
        $("input[type='checkbox'],input[type='radio']").click(this.filterClickEvent);

        //绑定查询事件
        $(".confirmButton").click(function () {
            if (SuperSelectCarTool.Parameters.toString().indexOf('page=') > -1) {
                for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                    if (SuperSelectCarTool.Parameters[i].indexOf('page=') > -1) {
                        SuperSelectCarTool.Parameters.splice(i, 1);
                        break;
                    }
                }
            }
            SuperSelectCarTool.SearchCarResult();
            //statForTempString(1009, 'submit-确定', '', SuperSelectCarTool.SearchCarResult)

            //$("#left-nav ul li").removeClass("current");
        });
        //绑定自定义价格按钮事件
        $("a[id^='btnPriceSubmit']").click(function () {
            var minP = getElementById("p_min").value;
            var maxP = getElementById("p_max").value;
            getElementById("p_alert").innerHTML = "";
            if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0)))
                getElementById("p_alert").innerHTML = "价格不能为空。";
            else if (maxP != "" && parseInt(maxP) <= 0) { getElementById("p_alert").innerHTML = "请填写正确的价格区间。"; }
            else {
                if (SuperSelectCarTool.Parameters.toString().indexOf('p=') > -1) {
                    for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                        var key = "|" + SuperSelectCarTool.Parameters[i];
                        if (key.indexOf("|p=") > -1) {
                            var delId = SuperSelectCarTool.Parameters[i].split('=')[1];
                            SuperSelectCarTool.Parameters.splice(i, 1);
                            SuperSelectCarTool.UpdateParameters(false, 'p_' + delId, 'p');
                            break;
                        }
                    }
                }
                SuperSelectCarTool.Parameters.push("p=9");
                SuperSelectCarTool.UpdateParameters(true, 'p_9', 'p');
            }
            GetCarTotalityItem();
        });
        //绑定自定义排量按钮事件
        $("#d_min,#d_max").bind('keyup', function (event) {
            var v = $(this).val();
            if (!verifyDecRegex(/^\d{1,2}(\.\d{0,1})?$/gi, v))
                $(this).val("");
        }).blur(function () {
            var v = $(this).val();
            if (!verifyDecRegex(/^\d{1,2}(\.\d{1})?$/gi, v))
                $(this).val("");
        });
        $("#btnDisSubmit").click(function () {
            var minP = getElementById("d_min").value;
            var maxP = getElementById("d_max").value;
            getElementById("d_alert").innerHTML = "";
            if (((minP == "" || isNaN(minP) || parseFloat(minP) < 0) && (maxP == "" || isNaN(maxP) || parseFloat(maxP) < 0)))
                getElementById("d_alert").innerHTML = "排量不能为空。";
            else if (maxP != "" && parseFloat(maxP) <= 0) { getElementById("d_alert").innerHTML = "请填写正确的排量区间。"; }
            else {
                if (SuperSelectCarTool.Parameters.toString().indexOf('d=') > -1) {
                    for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                        var key = "|" + SuperSelectCarTool.Parameters[i];
                        if (key.indexOf("|d=") > -1) {
                            var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                            SuperSelectCarTool.Parameters.splice(i, 1);
                            SuperSelectCarTool.UpdateParameters(false, 'd_' + delId, 'd');
                            break;
                        }
                    }
                }
                SuperSelectCarTool.Parameters.push("d=7");
                SuperSelectCarTool.UpdateParameters(true, 'd_7', 'd');
            }
            GetCarTotalityItem();
        });
        //添加滚动监听事件
        $('[data-spy="scroll"]').each(function () {
            var $spy = $(this);
            $spy.scrollspy($spy.data(), function () { });
            $spy.scrollspy("refresh");
        });
        //默认排序
        $("#moRen").on("click", function () {
            var jiaGeClass = $(this).attr("class");
            if (jiaGeClass == "down-arrow" || jiaGeClass == "current down-arrow") {
                sort = 1;
            } else {
                sort = 4;
            }
            var obj = getSearchObject();
            obj["page"] = 1;
            obj["s"] = sort;
            location.href = 'http://' + window.location.host + window.location.pathname + "?" + getQueryString(obj);
        });
        //价格排序
        $("#jiaGe").on("click", function () {
            var jiaGeClass = $(this).attr("class");
            if (jiaGeClass == "up-arrow" || jiaGeClass == "current down-arrow") {
                sort = 2;
            } else {
                sort = 3;
            }
            var obj = getSearchObject();
            obj["page"] = 1;
            obj["s"] = sort;
            location.href = 'http://' + window.location.host + window.location.pathname + "?" + getQueryString(obj);
        });
        //页面文本搜索
        $("a[name='btnsearch']").on("click", function (event) {
            var txtObj = $(this).siblings(".input")[0],
                searchTxt = $.trim($(txtObj).val()).toLowerCase(),
                flag = false;            $(".tiaojianlistbox li.search-on,.tiaojianlistbox dt.search-on").each(function () {
                $(this).removeClass("search-on");
            });
            if (searchTxt.length == 0) {
                $(txtObj).addClass("error");
                $(txtObj).focus();
                return;
            }
            //统计
            if (typeof BglogPostLog != "undefined") { BglogPostLog("2.161.1760"); }
            $(".tiaojianlistbox label,.tiaojianlistbox dt").each(function () {
                var html;
                if ($(this).find(".popup-layout-1").length > 0) {
                    html = $.trim($(this).find(".popup-layout-1").prop('previousSibling').nodeValue).toLowerCase();
                }
                else {
                    html = $.trim($(this).text()).toLowerCase();
                }

                if (html.indexOf(searchTxt) > -1) {
                    if ($(this).is("dt")) {
                        $(this).addClass("search-on");
                    }
                    else {
                        $(this).parent().addClass("search-on");
                    }
                    flag = true;
                }
            });
            if ($(".search-on").size() > 0) {
                var _top = $(".search-on").eq(0).offset().top - $("#topfixed").height() - 18;
                $("html,body").animate({
                    "scrollTop": _top
                }, 500)
            } else {
                var alterMsg = searchTxt.indexOf("未找到") > -1 ? searchTxt : "未找到 " + searchTxt;
                $("input[name='txtsearch']").val(alterMsg).addClass("error");
                return false;
            }
        });

        $("input[name='txtsearch']").on("click", function () {
            if ($(this).attr("class").indexOf("error") > -1) {
                $("input[name='txtsearch']").removeClass("error").val("").attr("placeholder", "快速搜配置，如：自动泊车");
            }
        });
        $("input[name='txtsearch']").watch(function (obj, event) {
            event = event || window.event;
            var keyCode = event.keyCode ? event.keyCode : event.charCode;
            if (keyCode == 13) {
                return false;
            }
            if ($(obj).attr("class").indexOf("error") > -1) {
                $("input[name='txtsearch']").removeClass("error").val($.trim($(obj).val().replace("未找到", "")));
            }
            else {
                $("input[name='txtsearch']").val($(obj).val());
            }
        });

        document.onkeypress = function (event) {
            event = event || window.event;
            var keyCode = event.keyCode ? event.keyCode : event.charCode;
            if (keyCode == 13 && document.activeElement.name == "txtsearch") {
                $($("a[name='btnsearch']")[0]).trigger("click");
            }
        }
    }
    //点击事件处理
    , filterClickEvent: function () {
        if ($("input[type='checkbox']").is(":checked") || $("input[type='radio']").is(":checked")) {
            $("#nofilterContent").hide();
            $("#filterContent").show();
        }
        else {
            $("#nofilterContent").show();
            $("#filterContent").hide();
        }
        var id, self = $(this)[0];
        id = self.id;
        var element = getElementById(id);
        if (element) {
            //if (element.checked) {
            //    $("#" + id).closest("li").addClass("current");
            //}
            //else {
            //    $("#" + id).closest("li").removeClass("current");
            //}
            var conType = id.split('_')[0];
            var conStr = id.split('_')[1];
            switch (conType) {
                //价格
                case "p":
                    getElementById("p_alert").innerHTML = "";
                    if (element.checked) {
                        if (SuperSelectCarTool.Parameters.toString().indexOf('p=') > -1) {
                            for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                                var key = "|" + SuperSelectCarTool.Parameters[i];
                                if (key.indexOf("|p=") > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'p_' + delId, conType);
                                    break;
                                }
                            }
                        }
                        if (conStr == 9) {
                            $('#p_min').removeAttr("disabled");
                            $('#p_max').removeAttr("disabled");
                            $('#btnPriceSubmit').removeAttr("disabled");
                        }
                        else {
                            $('#p_min').attr("disabled", true);
                            $('#p_max').attr("disabled", true);
                            $('#btnPriceSubmit').attr("disabled", true);
                            if (conStr != 0) {
                                SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                                SuperSelectCarTool.UpdateParameters(true, id, conType);
                            }
                        }
                    }
                    break;
                //排量
                case "d":
                    getElementById("d_alert").innerHTML = "";
                    if (element.checked) {
                        if (SuperSelectCarTool.Parameters.toString().indexOf('d') > -1) {
                            for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                                var key = "|" + SuperSelectCarTool.Parameters[i];
                                if (key.indexOf("|d=") > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'd_' + delId, conType);
                                    break;
                                }
                            }
                        }
                        if (conStr == 7) {
                            $('#d_min').removeAttr("disabled");
                            $('#d_max').removeAttr("disabled");
                            $('#btnDisSubmit').removeAttr("disabled");
                        }
                        else {
                            $('#d_min').attr("disabled", true);
                            $('#d_max').attr("disabled", true);
                            $('#btnDisSubmit').attr("disabled", true);
                            if (conStr != 0) {
                                SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                                SuperSelectCarTool.UpdateParameters(true, id, conType);
                            }
                        }
                    }
                    break;
                //级别
                case "l":
                    if (conStr < 13) {
                        if (element.checked) {
                            SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                        }
                        else {
                            SuperSelectCarTool.Parameters.remove(conType + "=" + conStr);
                        }
                        SuperSelectCarTool.UpdateParameters(element.checked, id, conType);
                    }
                    else {
                        SuperSelectCarTool.UpdateAllSUVPara(element.checked, id);
                    }
                    if (conStr == 8) {
                        SuperSelectCarTool.UpdateSUVParaList(element.checked);
                    }
                    break;
                case "b":
                    if (element.checked) {
                        if (SuperSelectCarTool.Parameters.toString().indexOf('b=') > -1 || SuperSelectCarTool.Parameters.toString().indexOf('lv=') > -1) {
                            for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                                if (SuperSelectCarTool.Parameters[i].indexOf('b=') > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'b_' + delId, 'b');
                                    break;
                                }
                                if (SuperSelectCarTool.Parameters[i].indexOf('lv=') > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'lv_' + delId, "lv");
                                    break;
                                }
                            }
                        }
                        if (conStr != 0) {
                            SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                            SuperSelectCarTool.UpdateParameters(true, id, conType);
                        }
                    }
                    break;
                case "fc":
                    if (element.checked) {
                        if (SuperSelectCarTool.Parameters.toString().indexOf('fc=') > -1) {
                            for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                                if (SuperSelectCarTool.Parameters[i].indexOf('fc=') > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'fc_' + delId, 'fc');
                                    break;
                                }
                            }
                        }
                        if (conStr != 0) {
                            SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                            SuperSelectCarTool.UpdateParameters(true, id, conType);
                        }
                    }
                    break;
                case "lv":
                    if (element.checked) {
                        if (SuperSelectCarTool.Parameters.toString().indexOf('b=') > -1 || SuperSelectCarTool.Parameters.toString().indexOf('lv=') > -1) {
                            for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                                if (SuperSelectCarTool.Parameters[i].indexOf('b=') > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'b_' + delId, 'b');
                                    break;
                                }
                                if (SuperSelectCarTool.Parameters[i].indexOf('lv=') > -1) {
                                    var delId = SuperSelectCarTool.Parameters[i].split('=')[1]
                                    SuperSelectCarTool.Parameters.splice(i, 1);
                                    SuperSelectCarTool.UpdateParameters(false, 'lv_' + delId, "lv");
                                    break;
                                }
                            }
                        }
                        if (conStr != 0) {
                            SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                            SuperSelectCarTool.UpdateParameters(true, id, conType);
                        }
                    }
                    break;
                case "more":
                    //进气形式
                    if ((conStr > 99 && conStr < 104) || conStr == 0) {
                        if (element.checked) {
                            for (var i = 100; i < 104; i++) {
                                if (i != conStr) {
                                    SuperSelectCarTool.RemoveMoreCondition(conType, i);
                                    SuperSelectCarTool.UpdateParameters(false, 'more_' + i, conType);
                                }
                            }
                            //不限
                            if (conStr == 0) {
                                break;
                            }
                            SuperSelectCarTool.AddMoreCondition(conType, conStr);
                        }
                        else {
                            if (conStr == 0) {
                                break;
                            }
                            SuperSelectCarTool.RemoveMoreCondition(conType, conStr);
                        }
                    }
                    //智能钥匙
                    //else if (conStr == 225 || conStr == 226) {
                    //    if (conStr == 225) {
                    //        SuperSelectCarTool.RemoveMoreCondition(conType, 226);
                    //        SuperSelectCarTool.UpdateParameters(false, 'more_' + 226, conType);
                    //    }
                    //    else {
                    //        SuperSelectCarTool.RemoveMoreCondition(conType, 225);
                    //        SuperSelectCarTool.UpdateParameters(false, 'more_' + 225, conType);
                    //    }
                    //    SuperSelectCarTool.AddMoreCondition(conType, conStr);
                    //}
                    //车门
                    else if (conStr == 227 || conStr == 228) {
                        if (conStr == 227) {
                            SuperSelectCarTool.RemoveMoreCondition(conType, 228);
                            SuperSelectCarTool.UpdateParameters(false, 'more_' + 228, conType);
                        }
                        else {
                            SuperSelectCarTool.RemoveMoreCondition(conType, 227);
                            SuperSelectCarTool.UpdateParameters(false, 'more_' + 227, conType);
                        }
                        SuperSelectCarTool.AddMoreCondition(conType, conStr);
                    }
                    //行李箱
                    else if (conStr == 229 || conStr == 230) {
                        if (conStr == 229) {
                            SuperSelectCarTool.RemoveMoreCondition(conType, 230);
                            SuperSelectCarTool.UpdateParameters(false, 'more_' + 230, conType);
                        }
                        else {
                            SuperSelectCarTool.RemoveMoreCondition(conType, 229);
                            SuperSelectCarTool.UpdateParameters(false, 'more_' + 229, conType);
                        }
                        SuperSelectCarTool.AddMoreCondition(conType, conStr);
                    }
                    else {
                        if (element.checked) {
                            SuperSelectCarTool.AddMoreCondition(conType, conStr);
                        }
                        else {
                            SuperSelectCarTool.RemoveMoreCondition(conType, conStr);
                        }
                    }
                    SuperSelectCarTool.UpdateParameters(element.checked, id, conType);
                    break;
                default:
                    var add = true;
                    if (element.checked) {
                        SuperSelectCarTool.Parameters.push(conType + "=" + conStr);
                    }
                    else {
                        SuperSelectCarTool.Parameters.remove(conType + "=" + conStr);
                        add = false;
                    }
                    SuperSelectCarTool.UpdateParameters(add, id, conType);
                    break;
            }
            GetCarTotalityItem();
        }
    }
    //已选条件删除事件
    , deleteClickEvent: function (idEle) {
        var deleteId = idEle.id;
        var element = getElementById(deleteId);
        if (element && element.checked == true) {
            //$("#" + deleteId).closest("li").removeClass("current");      
            element.checked = false;
            var conType = deleteId.split('_')[0];
            var conStr = deleteId.split('_')[1];
            SuperSelectCarTool.Parameters.remove(conType + "=" + conStr);
            SuperSelectCarTool.UpdateParameters(false, deleteId, conType);
        }
        if (deleteId == "l_8") {
            SuperSelectCarTool.UpdateSUVParaList(false);
        }
        else if (deleteId == "p_9") {
            $('#p_min').attr("disabled", true);
            $('#p_max').attr("disabled", true);
            $('#btnPriceSubmit').attr("disabled", true);
        }
        else if (deleteId == "d_7") {
            $('#d_min').attr("disabled", true);
            $('#d_max').attr("disabled", true);
            $('#btnDisSubmit').attr("disabled", true);
        }
        //else if (deleteId == "more_101") {
        //    for (var i = 102; i < 106; i++) {
        //        $('#more_' + i).attr("checked", false);
        //        $('#more_' + i).attr("disabled", true);
        //        SuperSelectCarTool.UpdateParameters(false, 'more_' + i, conType);
        //        SuperSelectCarTool.RemoveMoreCondition(conType, i);
        //    }
        //}
        //else if (deleteId == "more_220") {
        //    for (var i = 221; i < 224; i++) {
        //        $('#more_' + i).attr("checked", false);
        //        $('#more_' + i).attr("disabled", true);

        //        SuperSelectCarTool.UpdateParameters(false, 'more_' + i, conType);
        //        SuperSelectCarTool.RemoveMoreCondition(conType, i);
        //    }
        //}
        GetCarTotalityItem();
        if ($("input[type='checkbox']").is(":checked") || $("input[type='radio']").is(":checked")) {
            $("#nofilterContent").hide();
            $("#filterContent").show();
        }
        else {
            $("#nofilterContent").show();
            $("#filterContent").hide();
        }
    }
    //初始化查询条件
    , initFilterCondition: function () {
        for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
            var idTemp = SuperSelectCarTool.Parameters[i].split('=')[1];
            var typeTemp = SuperSelectCarTool.Parameters[i].split('=')[0];
            var splitStr = ',';
            if (typeTemp == "more") {
                splitStr = '_'
            }
            if (typeTemp == "page") {
                page = idTemp;
            }
            if (typeTemp != "page" && typeTemp != "pagesize" && typeTemp != "s") {
                initFlag = false;
                anchorFlag = true;
            }
            if (typeTemp != "page" && idTemp > 1) {
                anchorFlag = true;
            }
            if (typeTemp != "s" && idTemp != '4') {
                anchorFlag = true;
            }

            switch (typeTemp) {
                case "l":
                    if (idTemp == 8) {
                        SuperSelectCarTool.UpdateSUVParaList(true);
                    }
                    break;
                case "p":
                    idTemp = SuperSelectCarTool.InitPrice(idTemp);
                    SuperSelectCarTool.Parameters[i] = typeTemp + "=" + idTemp
                    break;
                case "d":
                    idTemp = SuperSelectCarTool.InitDisplacement(idTemp);
                    SuperSelectCarTool.Parameters[i] = typeTemp + "=" + idTemp
                    break;
                case "fc":
                    idTemp = SuperSelectCarTool.InitFuelConsumption(idTemp);
                    SuperSelectCarTool.Parameters[i] = typeTemp + "=" + idTemp
                    break;
                default:
                    break;

            }
            if (idTemp.split(splitStr).length > 0) {
                var idListTemp = idTemp.split(splitStr);
                for (var j = 0, ls = idTemp.split(splitStr).length; j < ls; j++) {
                    SuperSelectCarTool.UpdateParameters(true, typeTemp + '_' + idListTemp[j], typeTemp);
                    var elementTemp = getElementById(typeTemp + '_' + idListTemp[j]);
                    if (elementTemp) {
                        elementTemp.checked = true;
                    }
                }
            }
            else {
                SuperSelectCarTool.UpdateParameters(true, typeTemp + '_' + idTemp, typeTemp);
                var elementTemp = getElementById(typeTemp + '_' + idTemp);
                if (elementTemp) {
                    elementTemp.checked = true;
                }
            }
        }
        //价格初始化
        var autoPrice = getElementById("p_9");
        if (autoPrice.checked == false) {
            $('#p_min').attr("disabled", true);
            $('#p_max').attr("disabled", true);
            $('#btnPriceSubmit').attr("disabled", true);
        }
        else {
            $('#p_min').attr("disabled", false);
            $('#p_max').attr("disabled", false);
            $('#btnPriceSubmit').attr("disabled", false);
        }
        //排量初始化
        var autoDis = getElementById("d_7");
        if (autoDis.checked == false) {
            $('#d_min').attr("disabled", true);
            $('#d_max').attr("disabled", true);
            $('#btnDisSubmit').attr("disabled", true);
        }
        else {
            $('#d_min').attr("disabled", false);
            $('#d_max').attr("disabled", false);
            $('#btnDisSubmit').attr("disabled", false);
        }
        //进气形式初始化
        //var jinqu = getElementById("more_101");
        //if (jinqu.checked == false) {
        //    for (var i = 102; i < 106; i++) {
        //        $('#more_' + i).attr("disabled", true);
        //    }
        //}
        //else {
        //    for (var i = 102; i < 106; i++) {
        //        $('#more_' + i).attr("disabled", false);
        //    }
        //}
        //车顶初始化
        //var roof = getElementById("more_220");
        //if (roof.checked == false) {
        //    for (var i = 221; i < 224; i++) {
        //        $('#more_' + i).attr("disabled", true);
        //    }
        //}
        //else {
        //    for (var i = 221; i < 224; i++) {
        //        $('#more_' + i).attr("disabled", false);
        //    }
        //}
    }
    //初始化价格
    , InitPrice: function (priceStr) {
        var priceId = "";
        switch (priceStr) {
            case "0-5":
                priceId = "1";
                break;
            case "5-8":
                priceId = "2";
                break;
            case "8-12":
                priceId = "3";
                break;
            case "12-18":
                priceId = "4";
                break;
            case "18-25":
                priceId = "5";
                break;
            case "25-40":
                priceId = "6";
                break;
            case "40-80":
                priceId = "7";
                break;
            case "80-9999":
                priceId = "8";
                break;
            default:
                if (priceStr != "") {
                    var arrayPrice = priceStr.split("-");
                    if (arrayPrice.length == 2) {
                        getElementById("p_min").value = arrayPrice[0];
                        getElementById("p_max").value = arrayPrice[1];
                    }
                    priceId = "9";
                }
                break;
        }
        return priceId
    }
    //初始化排量
    , InitDisplacement: function (disStr) {
        var disId = "";
        switch (disStr) {
            case "0-1.4":
                disId = "1";
                break;
            case "1.4-1.6":
                disId = "2";
                break;
            case "1.6-1.8":
                disId = "3";
                break;
            case "1.8-2.0":
                disId = "4";
                break;
            case "2.0-3.0":
                disId = "5";
                break;
            case "3.0-9999":
                disId = "6";
                break;
            default:
                if (disStr != "") {
                    var arrayDis = disStr.split("-");
                    if (arrayDis.length == 2) {
                        getElementById("d_min").value = arrayDis[0];
                        getElementById("d_max").value = arrayDis[1];
                    }
                    disId = "7";
                }
                break;
        }
        return disId;
    }
    //初始化油耗
    , InitFuelConsumption: function (disStr) {
        var disId = "";
        switch (disStr) {
            case "0-6":
                disId = "1";
                break;
            case "6-8":
                disId = "2";
                break;
            case "8-10":
                disId = "3";
                break;
            case "10-12":
                disId = "4";
                break;
            case "12-15":
                disId = "5";
                break;
            case "15-9999":
                disId = "6";
                break;
            default:
                break;
        }
        return disId;
    }
    //更新已选条件
    , UpdateParameters: function (operate, id, type) {
        var cur = getElementById(id);
        if (!cur) {
            return;
        }
        var typeId = id.split('_')[1];
        if (type == "s" || type == "page" || type == "pagesize") {
            return;
        }
        var parameterList = getElementById("parameters");
        if (parameterList) {
            var idTemp = "select_" + id;
        }
        if (operate) {
            var className;
            var value;
            //var value = $("#" + id).parent().text();
            if ($("#" + id).parent().find(".popup-layout-1").length > 0) {
                value = $("#" + id).parent().find(".popup-layout-1").prop('previousSibling').nodeValue;
            }
            else {
                value = $("#" + id).parent().text();
            }
            if (getElementById(idTemp)) {
                return;
            }
            className = $("#" + id).closest("dl").find("dt").html();
            if (type == 'p' && typeId == 9) {
                var minP = getElementById("p_min").value;
                var maxP = getElementById("p_max").value;
                if (minP != "" && parseInt(minP) > 0 && (maxP == "" || parseInt(maxP) <= 0)) {
                    value = minP + "-9999万";
                }
                else if (maxP != "" && parseInt(maxP) > 0 && (minP == "" || parseInt(minP) <= 0)) {
                    value = "0-" + maxP + "万";
                }
                else if (minP != "" && parseInt(minP) > 0 && maxP != "" && parseInt(minP) > 0) {
                    if (parseInt(maxP) > parseInt(minP)) {
                        value = minP + "-" + maxP + "万";
                    }
                    else {
                        getElementById("p_alert").innerHTML = "请填写正确的价格区间。";
                        return;
                    }
                }
            }
            else if (type == 'd' && typeId == 7) {
                var minP = getElementById("d_min").value;
                var maxP = getElementById("d_max").value;
                if (minP != "" && parseFloat(minP) > 0 && (maxP == "" || parseFloat(maxP) <= 0)) {
                    value = minP + "-9999L";
                }
                else if (maxP != "" && parseFloat(maxP) > 0 && (minP == "" || parseFloat(minP) <= 0)) {
                    value = "0-" + maxP + "L";
                }
                else if (minP != "" && parseFloat(minP) > 0 && maxP != "" && parseFloat(minP) > 0) {
                    if (parseFloat(maxP) > parseFloat(minP)) {
                        value = minP + "-" + maxP + "L";
                    }
                    else {
                        getElementById("d_alert").innerHTML = "请填写正确的排量区间。";
                        return;
                    }
                }
            }
            var parameterList = getElementById("parameters");
            if (parameterList) {
                var liEle = document.createElement("li");
                liEle.id = "select_" + id;
                liEle.innerHTML = "<a href=\"javascript:;\" onclick=\"SuperSelectCarTool.deleteClickEvent(" + id + ")\"><strong>" + className.toString() + "：</strong><span>" + value + "</span><i class=\"del\">删除</i></a>";
                parameterList.appendChild(liEle);
            }
        }
        else {
            if (getElementById(idTemp)) {
                parameterList.removeChild(getElementById(idTemp));
            }
        }
    }
    //更新SUV小类条件
    , UpdateSUVParaList: function (operate) {
        var ele = getElementById("suvList");
        if (ele) {
            var liList = ele.getElementsByTagName("li");
            for (var i = 0; i < liList.length; i++) {
                if (liList[i]) {
                    liList[i].firstChild.firstChild.checked = operate;
                }
                if (liList[i] && liList[i].firstChild.firstChild && operate) {
                    var suvSubId = liList[i] && liList[i].firstChild.firstChild.id;
                    SuperSelectCarTool.Parameters.remove("l=" + suvSubId.split('_')[1]);
                    SuperSelectCarTool.UpdateParameters(false, suvSubId, 'l');
                }
            }
        }
    }
    //更新全部SUV条件
    , UpdateAllSUVPara: function (operate, id) {
        var checked = true;
        var ele = getElementById("suvList");
        if (ele) {
            var liList = ele.getElementsByTagName("li");
            for (var i = 0; i < liList.length; i++) {
                if (liList[i] && liList[i].firstChild.firstChild && liList[i].firstChild.firstChild.checked == false) {
                    checked = false;
                }
            }
            var suvEle = getElementById("l_8")
            if (suvEle) {
                suvEle.checked = checked;
            }
            if (checked) {
                SuperSelectCarTool.Parameters.push("l=8");
                for (var i = 0; i < liList.length; i++) {
                    if (liList[i] && liList[i].firstChild.firstChild) {
                        var suvSubId = liList[i] && liList[i].firstChild.firstChild.id;
                        SuperSelectCarTool.Parameters.remove("l=" + suvSubId.split('_')[1]);
                        SuperSelectCarTool.UpdateParameters(false, suvSubId, 'l');
                    }
                }
            }
            else {
                if (SuperSelectCarTool.Parameters.indexOf("l=8") > -1) {
                    for (var i = 0; i < liList.length; i++) {
                        if (liList[i] && liList[i].firstChild.firstChild) {
                            if (liList[i].firstChild.firstChild.checked == true) {
                                var suvSubId = liList[i] && liList[i].firstChild.firstChild.id;
                                SuperSelectCarTool.Parameters.push("l=" + suvSubId.split('_')[1]);
                                SuperSelectCarTool.UpdateParameters(liList[i].firstChild.firstChild.checked, suvSubId, 'l');
                            }
                        }
                    }
                }
                else {
                    if (operate) {
                        SuperSelectCarTool.Parameters.push("l=" + id.split('_')[1]);
                    }
                    else {
                        SuperSelectCarTool.Parameters.remove("l=" + id.split('_')[1]);
                    }
                    SuperSelectCarTool.UpdateParameters(operate, id, 'l');
                }
                SuperSelectCarTool.Parameters.remove("l=8");
            }
            SuperSelectCarTool.UpdateParameters(checked, "l_8", 'l');
        }
    }
    //刷新页面获取查询结果
    , SearchCarResult: function () {
        var queryString = SuperSelectCarTool.GetSearchQueryString();
        var toUrl = 'http://' + window.location.host + window.location.pathname;
        if (queryString.length > 0) {
            toUrl += "?" + queryString;
        }

        window.location.href = toUrl;
    }
    //获取接口数据更新页面
    , UpdateCarResult: function () {
        var apiQueryString = SuperSelectCarTool.GetAPISearchQueryString();
        var url = SuperSelectCarTool.apiUrl;
        if (apiQueryString.length > 0) {
            url += "?" + apiQueryString + "&v=20171011";
        }
        $.ajax({
            url: url,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                DrawUlContent(json);
            }
        });
    }
    //获取API请求参数
    , GetAPISearchQueryString: function () {
        var qsArray = new Array();
        for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
            var typeId = SuperSelectCarTool.Parameters[i].split('=')[0];
            var id = SuperSelectCarTool.Parameters[i].split('=')[1];
            id = SuperSelectCarTool.GetQueryKeyValueById(typeId, id, true)
            var splitStr = ',';
            //更多条件
            if (typeId == "more") {
                splitStr = '_'
                //不限
                if (id < 100) {
                    continue;
                }
            }
            var existed = false;

            for (var j = 0, le = qsArray.length; j < le; j++) {
                var key = "|" + qsArray[j];
                if (key.indexOf("|" + typeId + '=') > -1) {
                    qsArray[j] = qsArray[j] + splitStr + id;
                    existed = true;
                }
            }

            if (existed == false) {
                qsArray.push(typeId + "=" + id);
            }
        }
        return qsArray.join('&');
    }
    //获取请求参数
    , GetSearchQueryString: function () {
        var qsArray = new Array();
        for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
            var typeId = SuperSelectCarTool.Parameters[i].split('=')[0];
            var id = SuperSelectCarTool.Parameters[i].split('=')[1];
            id = SuperSelectCarTool.GetQueryKeyValueById(typeId, id, false)
            var splitStr = ',';
            //更多条件
            if (typeId == "more") {
                splitStr = '_'
                //不限
                if (id < 100) {
                    continue;
                }
            }
            if (typeId == "pagesize") {
                continue;
            }
            if (typeId == "page") {
                continue;
            }
            var existed = false;
            for (var j = 0, le = qsArray.length; j < le; j++) {
                var key = "|" + qsArray[j];
                if (key.indexOf("|" + typeId + '=') > -1) {
                    qsArray[j] = qsArray[j] + splitStr + id;
                    existed = true;
                }
            }
            if (existed == false) {
                qsArray.push(typeId + "=" + id);
            }
        }
        return qsArray.join('&');
    }
    //获取选车条件的值
    , GetQueryKeyValueById: function (keyStr, idStr, isApi) {
        var valueStr = "";
        switch (keyStr) {
            case "p":
                if (idStr == '1')
                    valueStr = "0-5";
                else if (idStr == '2')
                    valueStr = "5-8";
                else if (idStr == '3')
                    valueStr = "8-12";
                else if (idStr == '4')
                    valueStr = "12-18";
                else if (idStr == '5')
                    valueStr = "18-25";
                else if (idStr == '6')
                    valueStr = "25-40";
                else if (idStr == '7')
                    valueStr = "40-80";
                else if (idStr == '8')
                    valueStr = "80-9999";
                else if (idStr == '9') {
                    var minP = getElementById("p_min").value;
                    var maxP = getElementById("p_max").value;
                    if (minP != "" && parseInt(minP) > 0 && (maxP == "" || parseInt(maxP) <= 0)) {
                        valueStr = minP + "-9999";
                    }
                    else if (maxP != "" && parseInt(maxP) > 0 && (minP == "" || parseInt(minP) <= 0)) {
                        valueStr = "0-" + maxP;
                    }
                    else if (minP != "" && parseInt(minP) > 0 && maxP != "" && parseInt(minP) > 0) {
                        if (parseInt(maxP) > parseInt(minP)) {
                            valueStr = minP + "-" + maxP;
                        }
                    }
                }
                break;
            case "d":
                if (idStr == '1')
                    valueStr = "0-1.4";
                else if (idStr == '2')
                    valueStr = "1.4-1.6";
                else if (idStr == '3')
                    valueStr = "1.6-1.8";
                else if (idStr == '4')
                    valueStr = "1.8-2.0";
                else if (idStr == '5')
                    valueStr = "2.0-3.0";
                else if (idStr == '6')
                    valueStr = "3.0-9999";
                else if (idStr == '7') {
                    var minP = getElementById("d_min").value;
                    var maxP = getElementById("d_max").value;
                    if (minP != "" && parseFloat(minP) > 0 && (maxP == "" || parseFloat(maxP) <= 0)) {
                        valueStr = minP + "-9999";
                    }
                    else if (maxP != "" && parseFloat(maxP) > 0 && (minP == "" || parseFloat(minP) <= 0)) {
                        valueStr = "0-" + maxP;
                    }
                    else if (minP != "" && parseFloat(minP) > 0 && maxP != "" && parseFloat(minP) > 0) {
                        if (parseFloat(maxP) > parseFloat(minP)) {
                            valueStr = minP + "-" + maxP;
                        }
                    }
                }
                break;

            //case "more":
            //	//电动车窗 车窗防夹 隔热玻璃 日间行车灯 特殊处理
            //	//if (idStr == '207' && isApi) {
            //	//    valueStr = "207_208";
            //	//}
            //	//else if (idStr == '209' && isApi) {
            //	//    valueStr = "209_257_258_259";
            //	//}
            //	//else if (idStr == '230' && isApi) {
            //	//    valueStr = "230_274";
            //	//}
            //	//else if (idStr == '260' && isApi) {
            //	//    valueStr = "260_261";
            //	//}

            //	//车门2-3,4-5
            //	if (idStr == '268' && isApi) {
            //		valueStr = "268_269";
            //	}
            //	else if (idStr == '270' && isApi) {
            //		valueStr = "270_271_272";
            //	}
            //	else {
            //		valueStr = idStr;
            //	}
            //	break;
            case "fc":
                if (idStr == 1) {
                    valueStr = "0-6";
                }
                else if (idStr == 2) {
                    valueStr = "6-8";
                }
                else if (idStr == 3) {
                    valueStr = "8-10";
                }
                else if (idStr == 4) {
                    valueStr = "10-12";
                }
                else if (idStr == 5) {
                    valueStr = "12-15";
                }
                else if (idStr == 6) {
                    valueStr = "15-9999";
                }
                break;
            default:
                valueStr = idStr;
                break;
        }
        return valueStr;
    }
    //添加更多条件参数
    , AddMoreCondition: function (type, id) {
        if (SuperSelectCarTool.Parameters.indexOf("more=") > -1) {
            if (SuperSelectCarTool.Parameters.indexOf("_" + id) > -1) {
                return;
            }
            SuperSelectCarTool.Parameters.push("_" + id);
        }
        else {
            if (SuperSelectCarTool.Parameters.indexOf("more=" + id) > -1) {
                return;
            }
            SuperSelectCarTool.Parameters.push(type + "=" + id);
        }
    }
    //删除更多条件参数
    , RemoveMoreCondition: function (type, id) {
        if (SuperSelectCarTool.Parameters.indexOf("more=") > -1) {
            SuperSelectCarTool.Parameters.remove("_" + id);
        }
        else {
            SuperSelectCarTool.Parameters.remove(type + "=" + id);
        }
    }
}
//更新符合查询条件的车型数
function GetCarTotalityItem() {
    var queryString = SuperSelectCarTool.GetAPISearchQueryString();
    var toUrl = SuperSelectCarTool.apiUrl;
    if (queryString.length > 0) {
        toUrl += "?" + queryString + "&v=20171011";
    }
    $.ajax({
        url: toUrl,
        cache: true,
        dataType: 'jsonp',
        jsonp: 'callback',
        jsonpCallback: "cartotalityitemCallback",
        success: function (data) {
            if (typeof data == "undefined") return;
            var styleCount = data.Count;
            var carCount = data.CarNumber;
            if (getElementById("styleTotal")) {
                if (styleCount > 0) {
                    $("#styleTotal").html("<p>为您找到<em>" + styleCount + "</em>个车型,<em>" + carCount + "</em>个车款</p><a href=\"/gaojixuanche/\" class=\"delall\">清空条件</a>");
                }
                else {
                    $("#styleTotal").html("<p><em>抱歉，未找到合适的车型</em></p><a href=\"/gaojixuanche/\" class=\"delall\">清空条件</a>");
                }
            }
        }
    });
}
//更新车型列表
function DrawUlContent(json) {
    $("#styleCount").text("共 " + json.Count + " 个车型," + json.CarNumber + " 个车款");
    if (initFlag) {
        $("#nofilterContent").show();
        $("#filterContent").hide();
    }
    else {
        $("#nofilterContent").hide();
        $("#filterContent").show();
    }

    if (json.Count > 0 && !initFlag) {
        $("#styleTotal").html("<p>为您找到<em>" + json.Count + "</em>个车型,<em>" + json.CarNumber + "</em>个车款</p><a href=\"/gaojixuanche/\" class=\"delall\">清空条件</a>");
    }
    else if (json.Count == 0 && !initFlag) {
        $("#styleTotal").html("<p><em>抱歉，未找到合适的车型</em></p><a href=\"/gaojixuanche/\" class=\"delall\">清空条件</a>");
    }
    if (json.ResList.length == "0") {
        $("#noResult").show();
        $("#params-styleList").hide();
    } else {
        $("#params-styleList").show();
        $("#noResult").hide();
        //初始化车款列表        
        var divContentArray = new Array();
        var currentLineCount = 0;
        var csIdsArr = [];
        $(json.ResList).each(function (index) {
            var x = index % 5 + 1;
            csIdsArr.push(json.ResList[index].SerialId);
            divContentArray.push("<div class=\"col-auto\" data-id=\"" + json.ResList[index].SerialId + "\"><div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
            divContentArray.push("<div class=\"img\"><a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\">");
            divContentArray.push("<img src=\"" + json.ResList[index].ImageUrl.replace("_1", "_3") + "\" alt=\"" + json.ResList[index].ShowName + "报价_价格\"/>");
            divContentArray.push("</a></div>");
            divContentArray.push("<ul class=\"p-list\">");
            divContentArray.push("<li class=\"name\"><a href=\"/" + json.ResList[index].AllSpell + "/\" target=\"_blank\">" + json.ResList[index].ShowName + "</a></li>");
            divContentArray.push("<li class=\"price\"><a href=\"/" + json.ResList[index].AllSpell + "/baojia/\" target=\"_blank\">" + json.ResList[index].PriceRange + "</a></li>");
            divContentArray.push("<li class=\"info layer-box x" + x + "\" bit-seachmore><a href=\"javascript:;\" bit-serial=\"" + json.ResList[index].SerialId + "\" bit-car=\"" + json.ResList[index].CarIdList + "\" bit-line=\"" + (currentLineCount - 1) + "\" bit-allspell=\"" + json.ResList[index].AllSpell + "\" class=\"sub-color\">" + json.ResList[index].CarNum + "个车款符合条件</a>");
            divContentArray.push("<i></i>");
            divContentArray.push("<div class=\"drop-layer\" style=\"display:none\"><span class=\"close\"></span>");
            //固定头start
            divContentArray.push("<div class=\"list-table\" style=\"position: absolute; left:0; top:0; z-index: 1;\">");
            divContentArray.push("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            divContentArray.push("<colgroup><col width=\"40%\"><col width=\"11%\"><col width=\"13%\"><col width=\"10%\"><col width=\"11%\"><col width=\"16%\"></colgroup>");
            divContentArray.push("<tbody>");
            divContentArray.push("<tr class=\"table-tit\">");
            divContentArray.push("<th class=\"first-item\"><strong>车款</strong></th>");
            divContentArray.push("<th>关注度</th>");
            divContentArray.push("<th>变速箱</th>");
            divContentArray.push("<th class=\"txt-right\">指导价</th>");
            divContentArray.push("<th class=\"txt-right\">参考最低价</th>");
            divContentArray.push("<th><div class=\"doubt\"  onmouseover=\"javascript:$(this).children('.prompt-layer').show();return false;\" onmouseout=\"javascript:$(this).children('.prompt-layer').hide();return false;\"><div class=\"prompt-layer\" style=\"display:none\">全国参考最低价</div></div></th>");
            divContentArray.push("</tr>");
            divContentArray.push("</tbody>");
            divContentArray.push("</table>");
            divContentArray.push("</div>");
            //固定头end
            divContentArray.push("<div class=\"list-table scroll-table\" table-serial=\"" + json.ResList[index].SerialId + "\"></div>");
            divContentArray.push("</div>");
            divContentArray.push("</li>");
            divContentArray.push("</ul>");
            divContentArray.push("</div></div>");
        });
        var divContent = divContentArray.join("");
        $("#divContent").html(divContent);
        typeof GetNewCarText == "function" && GetNewCarText(csIdsArr.join(","));
        InitPageControl(json.Count);
        InitCarItem();
    }
    //滚动到车型列表
    if (anchorFlag) {
        // $("html,body").animate({ scrollTop: $("#params-styleList").offset().top }, 300);
        window.location.hash = "anchorTarget";
    }
}
//车型列表分页
function InitPageControl(pageCount) {
    $("#divPage").pagination(pageCount, {
        items_per_page: 50,
        num_display_entries: 8,
        link_to: "javascript:;",
        current_page: (page - 1) <= 0 ? 0 : (page - 1),
        num_edge_entries: 1,
        callback: function (index) { PageClick(index + 1); return false; },
        prev_text: "&lt;",
        next_text: "&gt; ",
        next_class: "next-on",
        prev_class: "preview-on"
    });
}
function PageClick(num) {
    page = num;
    var obj = getSearchObject();
    obj["page"] = num;
    location.href = 'http://' + window.location.host + window.location.pathname + "?" + getQueryString(obj);
}

function getElementById(i) { return document.getElementById(i); }
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }

//滚动监听
!function ($) {
    "use strict";
    function ScrollSpy(element, options) {
        var process = $.proxy(this.process, this)
            , $element = $(element).is('body') ? $(window) : $(element)
            , href;
        this.options = $.extend({}, $.fn.scrollspy.defaults, options);
        this.$scrollElement = $element.on('scroll.scroll-spy.data-api', process);
        this.selector = (this.options.target) + ' li > a';
        this.$body = $('body');
        this.refresh();
        this.process();
    }

    ScrollSpy.prototype = {
        constructor: ScrollSpy
        , refresh: function () {
            var self = this,
                $targets,
                scrollTop = self.$scrollElement.scrollTop();

            this.offsets = $([]);
            this.targets = $([]);

            $targets = this.$body.find(this.selector).map(function (i, n) {
                var $el = $(this),
                    targetName = $el.data('target'),
                    targetHeight = $el.height(),
                    $targetElement = $("#" + targetName);
                if ($targetElement
                    && ($targetElement.length > 0)) {
                    var ElementScrollTop = ($targetElement.offset().top - ($("#topfixed").offset().top));//50px margin-bottom值  2px bottom
                    //console.log($("#topfixed").height());
                    //console.log(targetName + $targetElement.offset().top);
                    //console.log(scrollTop + "," + ElementScrollTop);
                    if (scrollTop >= ElementScrollTop) {
                        self.activate(targetName)
                    }
                    $el.unbind("click");
                    $el.bind("click", function (e) {
                        e.preventDefault();
                        $("html,body").animate({ scrollTop: $targetElement.offset().top - $("#topfixed").height() - 2 }, 300, function () {
                            if (typeof self.options["callback"] != "undefined") { self.options["callback"](); }
                        });
                    });
                    return ([[ElementScrollTop, targetName]])
                } else
                    return null;
            })
                .sort(function (a, b) { return a[0] - b[0] })
                .each(function () {
                    self.offsets.push(this[0])
                    self.targets.push(this[1])
                });
        }
        , process: function () {
            var scrollTop = this.$scrollElement.scrollTop()
                , scrollHeight = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight
                , maxScroll = scrollHeight - this.$scrollElement.height()
                , offsets = this.offsets
                , targets = this.targets
                , activeTarget = this.activeTarget
                , i;
            //console.log(scrollTop);
            for (i = offsets.length; i--;) {
                activeTarget != targets[i];
                //console.log(offsets[i]);
                if (scrollTop >= offsets[i] && (!offsets[i + 1] || scrollTop <= offsets[i + 1])) {
                    this.activate(targets[i])
                }
            }
        }
        , activate: function (target) {
            this.activeTarget = target;
            $(this.selector)
                .parent('.current')
                .removeClass('current');
            var currSelector = this.selector + '[data-target="' + target + '"]';
            var active = $(currSelector)
                .parent('li')
                .addClass('current');
            active.trigger('activate');
        }
    }
    var old = $.fn.scrollspy
    $.fn.scrollspy = function (option, callbackFunc) {
        if (callbackFunc && callbackFunc instanceof Function) option["callback"] = callbackFunc;
        return this.each(function () {
            var $this = $(this)
                , data = $this.data('scrollspy')
                , options = typeof option == 'object' && option
            if (!data) $this.data('scrollspy', (data = new ScrollSpy(this, options)))
            if (typeof option == 'string') data[option]()
        })
    }
    $.fn.scrollspy.Constructor = ScrollSpy
    $.fn.scrollspy.defaults = {
        offset: 0,
        offsetList: 0
    }
    $.fn.scrollspy.noConflict = function () {
        $.fn.scrollspy = old
        return this
    }
}(jQuery);

$(function () {
    var theid = $("#topfixed");
    var thebox = $("#box");
    var floatkey;
    //////20110811修改隐藏显示浮动层
    var idmainoffsettop = $("#main_box").offset().top; //id的 offsettop
    var idmainoffsettop_top = idmainoffsettop  //上浮动层出现top定位
    var idleftwidth = '72'; //左侧浮动层的宽度

    ////////////////屏幕改变大小开始
    $(window).resize(function () {
        var boxoffset = thebox.offset();
        var boxoffsetLeft = boxoffset.left; //计算box的offleft值
        var scrollsLeft = $(window).scrollLeft(); //计算窗口左卷动值
        var scrolls = $(this).scrollTop();
        ////左侧菜单

        $("#left-nav").css({ left: boxoffsetLeft - idleftwidth + "px" });
        ////////////左上角结束
        if (floatkey) {//如果是在浮动状态
            //如果box的offsetleft =0 说明窗口小， 那么定位left=0 或者 负的leftscroll
            //如果box的offsetleft >0 说明窗口大，那么定位left=offsetleft 或者 leftscroll-offsetleft
            if (boxoffsetLeft == 0) {//窗口小
                if (scrollsLeft > 0) {
                    if (window.XMLHttpRequest) {
                        theid.css({
                            left: 0
                        });
                    }
                    else {//IE
                        theid.css({
                            left: 0
                        });
                    }
                } else {
                    theid.css({
                        left: 0
                    });
                }
            }
            if (boxoffsetLeft > 0) {//窗口大
                if (scrollsLeft < boxoffsetLeft) {
                    if (window.XMLHttpRequest) {
                        theid.css({
                            left: 0
                        });
                    } else {
                        theid.css({
                            left: 0
                        });
                    }
                } else {
                    if (window.XMLHttpRequest) {
                        theid.css({
                            left: 0
                        });
                    } else {
                        theid.css({
                            left: 0
                        });
                    }
                }
            }
        }
    });
    ////////////////屏幕改变大小结束
    ///////////////////屏幕卷动
    $(window).scroll(function () {
        var scrolls = $(this).scrollTop(); //窗口上卷动
        var scrollsLeft = $(this).scrollLeft(); //窗口左卷动值
        var boxoffset = thebox.offset();
        var boxoffsetLeft = boxoffset.left; //计算box的offleft值

        //左侧菜单操作
        //mathLeftMenu(scrollsLeft, boxoffsetLeft, scrolls, idmainoffsettop_top);
        ////////////left浮动模式结束///////////////////
        ////////////////控制上下卷动屏幕，出现浮动效果
        //console.log(scrolls);
        if (scrolls > idmainoffsettop_top) {//如果向上滚动大于id的top位置
            if (!floatkey) {
                theid.before("<div id=\"header-placeholder\" class=\"tjbox\"></div>");
                $("#header-placeholder").css({ height: (theid.height() - 10) + "px" });
            }
            floatkey = true; //开启浮动模式
            if (window.XMLHttpRequest) {//非IE6						 	
                theid.css({
                    position: "fixed",
                    top: "0",
                    left: 0,
                    display: "block"
                });
            } else {//IE6				 
                theid.css({
                    position: "absolute",
                    top: scrolls,
                    left: 0,
                    display: "block"
                });
            }
        }
        else if (scrolls <= idmainoffsettop_top) {//如果向上滚动小于id的top位置
            $("#header-placeholder").remove();
            floatkey = false; //关闭浮动模式。
            theid.css({
                position: "relative",
                left: "0",
                top: 0,
                display: ""
            });
        }
        /////////////////////控制左右卷动屏幕的效果
        if (floatkey) {//如果处在浮动状态
            if (scrollsLeft > 0 && boxoffsetLeft > 0) {//有左滚动，窗口大于页面宽度
                if (window.XMLHttpRequest) {	//非IE6	
                    theid.css({
                        left: 0
                    });
                } else {//IE6
                    theid.css({
                        left: 0
                    });
                }
            }
            if (scrollsLeft > 0 && boxoffsetLeft == 0) {//有左滚动，窗口小于页面宽度
                if (window.XMLHttpRequest) {	//非IE6					
                    theid.css({
                        left: 0
                    });
                } else {//IE6
                    theid.css({
                        left: 0
                    });
                }
            }
            if (scrollsLeft == 0) {//无左滚动，窗口小于或者大于页面宽度。或者拉到最左边。
                theid.css({
                    left: 0 //left数值等于id原有的offsetleft
                });
            }
        }
    });
    ///////////////////////屏幕卷动结束  
    //fix for firefox 浮动
    $(window).scrollTop($(window).scrollTop() - 1);
});

///////////////////////获取车型详细列表
function InitCarItem() {
    var carSynData = {}, IE6 = ! -[1,] && !window.XMLHttpRequest, IE7 = navigator.userAgent.toLowerCase().indexOf("msie 7.0") != -1;
    $("li[bit-seachmore] a[bit-serial]").bind("click", function () {
        var self = $(this);
        var serialId = $(this).attr("bit-serial");
        var allSpell = $(this).attr("bit-allspell");
        var carIds = $(this).attr("bit-car");
        var $container = $("div[table-serial='" + serialId + "']");//$(this).parent().find(".list-table");
        if ($(this).parent().hasClass("active") && carSynData[serialId]) {
            if (IE6 || IE7) {
                $container.parent().hide(); self.parent().removeClass("active");
            }
            else
                $container.parent().slideUp(200, function () { self.parent().removeClass("active"); });
            return;
        }
        var $curSerialLi = $(this).parent().addClass("active").closest("li");
        $curSerialLi.closest(".col-auto").siblings().find("ul li.active").removeClass("active").find(".drop-layer").hide();

        if (carSynData[serialId]) {
            if (IE6 || IE7) {
                $container.html(carSynData[serialId]);
                $container.parent().show();
            }
            else {
                $container.html(carSynData[serialId]).parent().slideDown(200);

            }
            $(".close").click(function () { $(this).parent().hide(); self.parent().removeClass("active"); });
            //initCarListHover();
            // 对比浮动框 初始
            initCompareButton();
        } else {
            $.ajax({
                url: "http://api.car.bitauto.com/CarInfo/GetCarListForSelectCar.ashx?serialId=" + serialId + "&carids=" + carIds, dataType: "jsonp", jsonpCallback: "GetCarListForSelectCarCallBack", cache: true,
                beforeSend: function (xhr) {
                    $("div[table-serial='" + serialId + "']").html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\"><tbody><tr><th width=\"32%\" class=\"pdL10\">车款</th><th width=\"8%\" class=\"pd-left-one\">关注度</th><th width=\"10%\" class=\"pd-left-one\">变速箱</th><th width=\"10%\" class=\"pd-left-two\">指导价</th><th width=\"10%\" class=\"pd-left-three\">参考最低价</th><th width=\"18%\"><div class=\"wenh\" onmouseover=\"javascript:$(this).children('.tc-wenh').show();return false;\" onmouseout=\"javascript:$(this).children('.tc-wenh').hide();return false;\"><div class=\"tc tc-wenh\" style=\"display:none;\"><div class=\"tc-box\"><i></i><p>全国参考最低价</p></div></div></div></th></th></tr><tr><td colspan=\"6\"><div id=\"carlist_loading\" class=\"pdL10\">正在加载...</div></td></tr></tbody></table>");
                    //$(".btn-close").click(function () { $(this).closest(".tool-filter-table").html(''); $(".tool-filter-car").removeClass("current"); });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert("textStatus: " + textStatus);
                },
                success: function (data) {
                    if (data.CarList.length <= 0) { $("#carlist_loading").html("暂无车型数据"); return; }
                    var content = [];

                    content.push("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                    content.push("<colgroup><col width=\"40%\"><col width=\"11%\"><col width=\"13%\"><col width=\"10%\"><col width=\"11%\"><col width=\"16%\"></colgroup>");
                    content.push("<tbody>");
                    content.push("<tr class=\"table-tit\" style=\"visibility: hidden;\">");
                    content.push("<th class=\"first-item\"><strong>车款</strong></th>");
                    content.push("<th>关注度</th>");
                    content.push("<th>变速箱</th>");
                    content.push("<th class=\"txt-right\">指导价</th>");
                    content.push("<th class=\"txt-right\">参考最低价</th>");
                    content.push("<th><div class=\"doubt\"  onmouseover=\"javascript:$(this).children('.prompt-layer').show();return false;\" onmouseout=\"javascript:$(this).children('.prompt-layer').hide();return false;\"><div class=\"prompt-layer\" style=\"display:none\">全国参考最低价</div></div></th>");
                    content.push("</tr>");

                    $.each(data.CarList, function (i, n) {
                        if (i % 2 == 0) {
                            content.push("<tr>");
                        }
                        else {
                            content.push("<tr class=\"hover-bg-color\">");
                        }
                        content.push("<td class=\"txt-left\">");
                        var yearType = n.CarYear.length > 0 ? n.CarYear + "款" : "未知年款";
                        var strState = n.ProduceState == "停产" ? " <span class=\"color-block3\">停产</span>" : (n.ProduceState == "团购" ? "<a href=\"javascript:void(0);\" target=\"_blank\" class=\"color-block\">团购</a>" : "");
                        content.push("<a href=\"/" + allSpell + "/m" + n.CarID + "/\" target=\"_blank\">" + yearType + " " + n.CarName + " </a>" + strState);
                        content.push("</td>");
                        content.push("<td>");
                        var percent = data.MaxPv > 0 ? (n.CarPV / data.MaxPv * 100.0) : 0;
                        content.push("<div class=\"w\"><div class=\"p\" style=\"width:" + percent + "%\"></div></div>");
                        content.push("</td>");
                        //var gearNum = (n.UnderPan_ForwardGearNum != "" && n.UnderPan_ForwardGearNum != "待查" && n.UnderPan_ForwardGearNum != "无级") ? n.UnderPan_ForwardGearNum + "挡" : ""
                        var transmissionType = "";
                        if (n.TransmissionType == "CVT无级变速" || n.TransmissionType == "E-CVT无级变速" || n.TransmissionType == "单速变速箱") {
                            transmissionType = n.TransmissionType;
                        }
                        else if (n.TransmissionType != "" && n.UnderPan_ForwardGearNum != "") {
                            transmissionType = n.UnderPan_ForwardGearNum + "挡 " + n.TransmissionType;
                        }
                        content.push("<td>" + transmissionType + "</td>");
                        content.push("<td class=\"txt-right\"><span>" + n.ReferPrice + "万</span><a title=\"购车费用计算\" class=\"car-comparetable-ico-cal\" rel=\"nofollow\" href=\"/gouchejisuanqi/?carid=" + n.CarID + "\" target=\"_blank\"></a></td>");
                        //取最低报价
                        var minPrice = n.CarPriceRange;
                        if (minPrice.length <= 0)
                        { content.push("<td class=\"txt-right\"><span>暂无报价</span></td>"); }
                        else if (minPrice.indexOf("-") != -1) {
                            minPrice = minPrice.substring(0, minPrice.indexOf('-'));
                            content.push("<td class=\"txt-right\"><span><a href=\"/" + allSpell + "/m" + n.CarID + "/baojia/\" target=\"_blank\">" + minPrice + "</a></span></td>");
                        } else { content.push("<td class=\"txt-right\"><span>" + minPrice + "</span></td>"); }
                        content.push("<td class=\"txt-right\">");
                        content.push("<a class=\"btn btn-primary btn-xs\" href=\"http://dealer.bitauto.com/zuidijia/nb" + serialId + "/nc" + n.CarID + "/\" target=\"_blank\">询底价</a>");
                        content.push(" <a class=\"btn btn-secondary btn-xs\" id=\"carcompare_btn_new_" + n.CarID + "\" target=\"_self\" href=\"javascript:;\" data-use=\"compare\" data-id=\"" + n.CarID + "\">+对比</a>");
                        content.push("</td>");
                        content.push("</tr>");
                    });
                    content.push("</tbody>");
                    content.push("</table>");
                    carSynData[serialId] = content.join('');
                    if (IE6 || IE7) {
                        $("div[table-serial='" + serialId + "']").html(content.join('')).parent().show();
                    } else {
                        $("div[table-serial='" + serialId + "']").html(content.join('')).parent().slideDown(200);
                    }
                    $(".close").click(function () { $(this).parent().hide(); self.parent().removeClass("active"); });
                    // 对比浮动框 初始
                    initCompareButton();
                    //initCarListHover();
                }
            });
        }
        return false;
    });
}
//滑动效果
//function initCarListHover() {
//	//车型列表效果
//    $('div.list-table tr').hover(
//			function () {
//				$(this).addClass('hover-bg-color');
//				$(this).find(".car-summary-btn-xunjia").removeClass('button_gray').addClass('button_orange');
//				if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
//					$(this).find(".car-summary-btn-duibi").removeClass('button_gray').addClass('button_orange');
//			},
//			function () {
//				$(this).removeClass('hover-bg-color');
//				$(this).find(".car-summary-btn-xunjia").removeClass('button_orange').addClass('button_gray');
//				//if (!$(this).find(".car-summary-btn-duibi").hasClass("button_none"))
//				$(this).find(".car-summary-btn-duibi").removeClass('button_orange').addClass('button_gray');
//			}
//		);
//}
function initCompareButton() {
    typeof InitCompareEvent == "function" && InitCompareEvent();
}
function getQueryString(data) {
    var tdata = '';
    for (var key in data) {
        tdata += "&" + (key) + "=" + (data[key]);
    }
    tdata = tdata.replace(/^&/g, "");
    return tdata
}


function getSearchObject() {
    var results = {};
    var url = window.location.search.substr(1);
    if (url) {
        var srchArray = url.split("&");
        var tempArray = new Array();

        for (var i = 0; i < srchArray.length; i++) {
            tempArray = srchArray[i].split("=");
            results[tempArray[0]] = tempArray[1];
        }
    }
    return results;
}

function verifyDecRegex(regex, str) {
    var result = true;
    if (!regex.test(str))
        result = false;
    return result;
}